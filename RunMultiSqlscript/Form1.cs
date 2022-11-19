using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Data.SqlTypes;
using System.Xml.Linq;

namespace RunMultiSqlscript
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

        #region 屬性
        //路徑設定
		public static string FilePath = $@"{Directory.GetCurrentDirectory()}{"\\"}";//當前工作路徑
		public static string LogPath = $@"{FilePath}{"\\"}{"log"}";//Log檔放置路徑

        //設定Log檔名
        public string FileLog = "view_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "_" + String.Format("{0:HHmmss}", DateTime.Now) + ".log";


        //資料庫連線區啟用設定
        public void ConnectionSettings(bool Status)
		{
			DBLocation.Enabled = Status;
			DBName.Enabled = Status;
			UserName.Enabled = Status;
			password.Enabled = Status;
		}
		//資料夾路徑&顯示項目區域啟用設定
		public void FolderPathCheckedListSettings(bool Status)
        {
			FolderPath.Enabled = Status;
			SelectFolder.Enabled = Status;
			FileCheckedList.Enabled = Status;
            AllChecked.Enabled = Status;
            AllCheckedClear.Enabled = Status;
        }
		//匯出list.sql&執行scripts區域啟用設定
		public void ScriptBtnSettings(bool Status)
		{
			OutputScripts.Enabled = Status;
			RunScript.Enabled = Status;
		}
        #endregion

        #region 讀取\寫入
        /// <summary>
        /// 清空檔案內容
        /// </summary>
        /// <param name="path"></param>
        public void ContentClear(string path)
        {
			FileStream fs;
			fs = new FileStream(path, FileMode.Truncate, FileAccess.Write);//Truncate模式打開文件可以清空。
			fs.Close();
		}
		/// <summary>
		/// 寫入檔案內容(ANSI編碼)
		/// </summary>
		/// <param name="path"></param>
		/// <param name="content"></param>
		public void ContentWrite(string path,string content)
        {
			using (StreamWriter writetext = new StreamWriter(path, true, Encoding.Default))
			{
				writetext.WriteLine(content);
			}
		}
		/// <summary>
		/// 讀取檔案內容
		/// </summary>
		/// <param name="path"></param>
		/// <returns readtext></returns>
		public string ContentRead(string path)
        {
			string readtext = "";
			using (StreamReader reader = new StreamReader(path, Encoding.Default))
			{
				readtext = reader.ReadToEnd();				
			}
			return readtext;
		}
        #endregion

        #region 功能
        /// <summary>
        /// 載入主畫面
        /// </summary>
        private void Form1_Load(object sender, EventArgs e)
        {
            //預設記住連線IP、DB名稱、使用者名稱
            DBLocation.Focus();
            DBLocation.Text = ConfigurationManager.AppSettings["connectionString"];
            DBName.Text = ConfigurationManager.AppSettings["databaseName"];
            UserName.Text = ConfigurationManager.AppSettings["userName"];

            //如果記住密碼被勾選，把密碼賦予文字框
            if (ConfigurationManager.AppSettings["rememberMe"].Equals("true"))
            {
                password.Text = ConfigurationManager.AppSettings["passWord"];
                RememberMe_CheckBox.Checked = true;
            }
        }
        /// <summary>
        /// 選擇資料夾
        /// </summary>
        private void SelectFolder_Click(object sender, EventArgs e)
		{
			FolderPath.Clear();
			FileCheckedList.Items.Clear();
			OutputScripts.Enabled = false;
			RunScript.Enabled = false;
			NotifyMsg.Visible = true;
			string filename;
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.Description = "請選擇資料夾";

			if (dialog.ShowDialog() == DialogResult.OK)
			{
				string filepath = dialog.SelectedPath;

				FolderPath.Text = filepath;

				//屏蔽尚未選擇資料夾字樣
				NotifyMsg.Visible = false;

				// 讀取資料夾中檔案名稱
				foreach (string fname in System.IO.Directory.GetFiles(filepath))
				{
					filename = fname;
					if (Path.GetFileName(filename).ToLower().Contains(".sql"))
					{
						FileCheckedList.Items.Add(Path.GetFileName(filename));
					}
				}
			}
		}

        /// <summary>
        /// 項目清單
        /// </summary>
        private void FileCheckedList_Click(object sender, EventArgs e)
		{
            try
            {
                int index = 0;//選取項目索引

                //檢查項目有無索引
                if (FileCheckedList.SelectedIndex != -1)
                {
                    index = FileCheckedList.SelectedIndex;
                    object Item = FileCheckedList.SelectedItem;

                    //檢查項目有無勾選
                    if (FileCheckedList.GetItemChecked(index))
                    {
                        FileCheckedList.Items.Add(Item, false);//取消勾選項目
                    }
                    else
                    {
                        //勾選項目並將索引與選取項目一同排序
                        FileCheckedList.Items.Insert(FileCheckedList.CheckedItems.Count, Item);
                        FileCheckedList.SetItemChecked(FileCheckedList.CheckedItems.Count, true);
                        index++;//索引增加1
                    }
                    FileCheckedList.Items.RemoveAt(index);//移除選取項目
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// 資料夾路徑區對應鍵盤按鍵
        /// </summary>
        private void FolderPath_KeyDown(object sender, KeyEventArgs e)
		{
			try
			{
				//鎖住Script相關按鈕
				ScriptBtnSettings(false);

				if (e.KeyCode == Keys.Enter)
				{
					FileCheckedList.Items.Clear();
					NotifyMsg.Visible = true;
					string filename;
					FolderBrowserDialog dialog = new FolderBrowserDialog();
					dialog.Description = "請選擇資料夾";
					string filepath = FolderPath.Text;

					//屏蔽尚未選擇資料夾字樣
					NotifyMsg.Visible = false;

					// 讀取資料夾中檔案名稱
					foreach (string fname in Directory.GetFiles(filepath))
					{
						filename = fname;
						if (Path.GetFileName(filename).ToLower().Contains(".sql"))
						{
							FileCheckedList.Items.Add(Path.GetFileName(filename));
						}
					}

				}
			}
			catch(Exception ex)
            {
				MessageBox.Show(ex.Message,"路徑位置錯誤");
            }
		}

        /// <summary>
        /// 輸出SQL清單
        /// </summary>
        private void OutputScripts_Click(object sender, EventArgs e)
        {
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.Filter = ".sql|";
			saveFileDialog.ShowDialog();
			string sqlText = "";

            if (!string.IsNullOrWhiteSpace(saveFileDialog.FileName))
			{
				foreach(var str in FileCheckedList.CheckedItems)
                {
					sqlText += $@":r {FolderPath.Text}{"\\"}{str}{"\n"}";//寫入sql字串資料
				}
				string FileName = saveFileDialog.FileName + saveFileDialog.Filter;//檔名+副檔名
				
				// 判斷檔案是否存在
				if (File.Exists(FileName))
				{
					//清空文件
					ContentClear(saveFileDialog.FileName);
				}
				//寫入script
				ContentWrite(saveFileDialog.FileName, sqlText);
            }
        }

        /// <summary>
        /// 資料庫連線
        /// </summary>
        private void DBConnect_Click(object sender, EventArgs e)
        {
			string ConnectionString = $@"Data Source={DBLocation.Text};Initial Catalog={DBName.Text};Persist Security Info=True;User Id={UserName.Text};Password={password.Text}";
			bool IsConnected = false;

			//記住連線資訊、帳號
            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			cfa.AppSettings.Settings["connectionString"].Value = DBLocation.Text;
			cfa.AppSettings.Settings["databaseName"].Value = DBName.Text;
            cfa.AppSettings.Settings["userName"].Value = UserName.Text;

 			if (DBConnect.Text == "中斷連線")
			{
				DBConnect.Text = "連線";
				ConnectionResult.Text = "未連線";
				ConnectionResult.ForeColor = Color.Black;

				//資料庫連線設定輸入開啟
				ConnectionSettings(true);
				//鎖住右側script選擇區域
				FolderPathCheckedListSettings(false);
				//鎖住右側script生成&執行區域
				ScriptBtnSettings(false);
			}
			else
			{
				ConnectionResult.Text = "連線中...";
				ConnectionResult.ForeColor = Color.Black;

				//測試資料庫連線
				SqlConnection sqlConnection = new SqlConnection(ConnectionString);
				try
				{
					sqlConnection.Open();
					IsConnected = true;
				}
				catch
				{
					IsConnected = false;
				}
				finally
				{
					sqlConnection.Close();
				}

				if (sqlConnection.State == ConnectionState.Broken || sqlConnection.State == ConnectionState.Closed)
				{
					if (IsConnected)
					{
						ConnectionResult.Text = "已連線";
						ConnectionResult.ForeColor = Color.Green;

						//資料庫連線設定輸入鎖住
						ConnectionSettings(false);
						//開啟右側script選擇區域
						FolderPathCheckedListSettings(true);

						if (FileCheckedList.CheckedItems.Count > 0)
						{
							//開啟右側script生成&執行區域
							ScriptBtnSettings(true);
						}
						DBConnect.Text = "中斷連線";
					}
					else
					{
						ConnectionResult.Text = "連線失敗";
						ConnectionResult.ForeColor = Color.Red;

						//鎖住右側script選擇生成&執行區域
						FolderPathCheckedListSettings(false);
						//鎖住右側script生成&執行區域
						ScriptBtnSettings(false);
					}
				}

                //確認是否記住密碼
                if (RememberMe_CheckBox.Checked)
                {
                    //記住密碼，保存密碼資訊
                    cfa.AppSettings.Settings["rememberMe"].Value = "true";
                    cfa.AppSettings.Settings["passWord"].Value = password.Text;
                }
                else
                {
                    //不記住密碼，清除密碼記憶資訊
                    cfa.AppSettings.Settings["rememberMe"].Value = "false";
                    cfa.AppSettings.Settings["passWord"].Value = "";
                }
                //保存連線資訊
                cfa.Save();
			}
        }

        /// <summary>
        /// 執行資料庫SQL更新
        /// </summary>
        private void RunScript_Click(object sender, EventArgs e)
        {
			string sqlText = "";
			FileLog = "view_" + String.Format("{0:yyyyMMdd}", DateTime.Now) + "_" + String.Format("{0:HHmmss}", DateTime.Now) + ".log";

			//判斷資料夾路徑是否存在
			if (Directory.Exists(FolderPath.Text))
			{
				foreach (var str in FileCheckedList.CheckedItems)
				{
					sqlText += $@":r {FolderPath.Text}{"\\"}{str}{"\n"}";
				}

				if (File.Exists($@"{FilePath}{"list.sql"}"))
				{
					//清空文件
					ContentClear($@"{FilePath}{"list.sql"}");
				}

				//儲存清單.sql
				ContentWrite($@"{FilePath}{"list.sql"}", sqlText);

                DialogResult result = MessageBox.Show("是否執行勾選的script?", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
				{
					//如果log資料夾不存在就自動生成
					if (!Directory.Exists(LogPath))
					{ 
						Directory.CreateDirectory("log");
                    }

					//設定sqlcmd參數
                    string argument = $@" -S {DBLocation.Text} -d {DBName.Text} -U {UserName.Text} -P {password.Text} -i ""{FilePath}{"list.sql"}"" -o ""{FilePath}{"log"}{"\\"}{FileLog}""";
					//開始執行sqlcmd
					ProcessStartInfo process = new ProcessStartInfo("sqlcmd", argument);
					process.UseShellExecute = false;
					process.CreateNoWindow = true;
                    process.WindowStyle = ProcessWindowStyle.Hidden;
                    process.RedirectStandardOutput = true;
                    Process proc = new Process();
					proc.StartInfo = process;
                    proc.Start();
					MessageBox.Show($@"已放置執行紀錄於.\log");
				}
            }
            else
            {
				MessageBox.Show("資料夾路徑錯誤","提示");
            }
		}

        /// <summary>
        /// 項目更動
        /// </summary>
        private void FileCheckedList_SelectedIndexChanged(object sender, EventArgs e)
        {
			ScriptBtnLock();//右側script生成&執行區域啟用/禁用
		}

        /// <summary>
        /// 全部選取
        /// </summary>
        private void AllChecked_Click(object sender, EventArgs e)
        {
			//選取全部列表項目
			for (int i = 0; i < FileCheckedList.Items.Count; i++)
			{
				FileCheckedList.SetItemChecked(i, true);
			}
			ScriptBtnLock();//右側script生成&執行區域啟用/禁用
		}

        /// <summary>
        /// 取消全選
        /// </summary>
        private void AllCheckedClear_Click(object sender, EventArgs e)
		{
			//取消選取全部列表項目
			for (int i = 0; i < FileCheckedList.Items.Count; i++)
			{
				FileCheckedList.SetItemChecked(i, false);
			}
			ScriptBtnLock();//右側script生成&執行區域啟用/禁用
		}

        /// <summary>
        /// 右側script生成&執行區域啟用/禁用
        /// </summary>
        public void ScriptBtnLock()
        {
			if (FileCheckedList.CheckedItems.Count > 0)
			{
				//開啟右側script生成&執行區域
				ScriptBtnSettings(true);
			}
			else
			{
				//鎖住右側script生成&執行區域
				ScriptBtnSettings(false);
			}
		}
        #endregion
    }
}
