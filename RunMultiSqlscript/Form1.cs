using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

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
				string FileName = saveFileDialog.FileName;
				
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

        private void DBConnect_Click(object sender, EventArgs e)
        {
			string ConnectionString = $@"Data Source={DBLocation.Text};Initial Catalog={DBName.Text};Persist Security Info=True;User Id={UserName.Text};Password={password.Text}";
			bool IsConnected = false;
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
			}
        }

        private void RunScript_Click(object sender, EventArgs e)
        {
			string sqlText = "";

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
                //讀取 一次執行資料夾內所有的Script 文字
                string readtext = ContentRead($@"{FilePath}{"sample.bat"}");

                //設定bat檔內的資練庫連線資訊
                readtext = readtext.Replace("dbIp=*", $@"dbIp={DBLocation.Text}");
				readtext = readtext.Replace("dbName=*", $@"dbName={DBName.Text}");
				readtext = readtext.Replace("dbUsrAcc=*", $@"dbUsrAcc={UserName.Text}");
				readtext = readtext.Replace("dbUsrPwd=*", $@"dbUsrPwd={password.Text}");
				readtext = readtext.Replace("batchFilePath=\"\"", $@"batchFilePath=""{FilePath}""");
				readtext = readtext.Replace("dbSqlFilePath=\"\"", $@"dbSqlFilePath=""{FilePath}{"list.sql"}""");

				string FileName = $@"{FilePath}{"RunScripts.bat"}";

				// 判斷檔案是否存在
				if(File.Exists(FileName))
				{
					//清空文件
					ContentClear(FileName);
				}

				//寫入欲執行bat
				ContentWrite(FileName, readtext);
                DialogResult result = MessageBox.Show("是否執行勾選的script?", "提示", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
				{
					//如果log資料夾不存在就自動生成
					if (!Directory.Exists(LogPath))
					{ 
						Directory.CreateDirectory("log");
                    }
					//開始執行批次檔
					ProcessStartInfo process = new ProcessStartInfo();
					process.FileName = "RunScripts.bat";
					process.WorkingDirectory = FilePath;
					Process.Start(process);
					MessageBox.Show($@"已放置執行紀錄於.\log");
				}
            }
            else
            {
				MessageBox.Show("資料夾路徑錯誤","提示");
            }
		}

        private void FileCheckedList_SelectedIndexChanged(object sender, EventArgs e)
        {
			ScriptBtnLock();//右側script生成&執行區域啟用/禁用
		}

        private void AllChecked_Click(object sender, EventArgs e)
        {
			//選取全部列表項目
			for (int i = 0; i < FileCheckedList.Items.Count; i++)
			{
				FileCheckedList.SetItemChecked(i, true);
			}
			ScriptBtnLock();//右側script生成&執行區域啟用/禁用
		}

		private void AllCheckedClear_Click(object sender, EventArgs e)
		{
			//取消選取全部列表項目
			for (int i = 0; i < FileCheckedList.Items.Count; i++)
			{
				FileCheckedList.SetItemChecked(i, false);
			}
			ScriptBtnLock();//右側script生成&執行區域啟用/禁用
		}
		//右側script生成&執行區域啟用/禁用
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
	}
}
