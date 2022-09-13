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
		public void ConnectionSettings(bool Status)
		{
			DBLocation.Enabled = Status;
			DBName.Enabled = Status;
			UserName.Enabled = Status;
			password.Enabled = Status;
		}
		public void FolderPathCheckedListSettings(bool Status)
        {
			FolderPath.Enabled = Status;
			SelectFolder.Enabled = Status;
			FileCheckedList.Enabled = Status;
		}
		public void ScriptBtnSettings(bool Status)
		{
			OutputScripts.Enabled = Status;
			RunScript.Enabled = Status;
		}
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
					filename = "";
					//filename = filename + fname + "\r\n";
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
						filename = "";
						//filename = filename + fname + "\r\n";
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
				// 判斷檔案是否存在
				string FileName = saveFileDialog.FileName;
				if (!File.Exists(FileName))
				{
					using (StreamWriter writetext = new StreamWriter(FileName, true, Encoding.Default))
					{
						writetext.WriteLine("");
					}
				}
				//清空文件
				FileStream fs;
				fs = new FileStream(saveFileDialog.FileName, FileMode.Truncate, FileAccess.Write);//Truncate模式打開文件可以清空。
				fs.Close();
				//寫入script
				using (StreamWriter writetext = new StreamWriter(saveFileDialog.FileName,true,Encoding.Default))
                {
					writetext.WriteLine(sqlText);
                }
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
			string readtext = "";
			string FilePath = $@"{Directory.GetCurrentDirectory()}{"\\"}";
			DialogResult result = new DialogResult();

			//判斷資料夾路徑是否存在
			if(Directory.Exists(FolderPath.Text))
			{
				foreach (var str in FileCheckedList.CheckedItems)
				{
					sqlText += $@":r {FolderPath.Text}{"\\"}{str}{"\n"}";
				}
				//儲存清單.sql
				using (StreamWriter writetext = new StreamWriter($@"{FilePath}{"list.sql"}",true,Encoding.Default))
				{
					writetext.WriteLine(sqlText);
					sqlText = "";
				}
				//讀取 一次執行資料夾內所有的Script 文字
				using (StreamReader reader = new StreamReader($@"{FilePath}{"sample.bat"}", Encoding.Default))
				{
					readtext = reader.ReadToEnd();
                }

				//設定bat檔內的資練庫連線資訊
                readtext = readtext.Replace("dbIp=*", $@"dbIp={DBLocation.Text}");
				readtext = readtext.Replace("dbName=*", $@"dbName={DBName.Text}");
				readtext = readtext.Replace("dbUsrAcc=*", $@"dbUsrAcc={UserName.Text}");
				readtext = readtext.Replace("dbUsrPwd=*", $@"dbUsrPwd={password.Text}");
				readtext = readtext.Replace("batchFilePath=\"\"", $@"batchFilePath=""{FilePath}""");
				readtext = readtext.Replace("dbSqlFilePath=\"\"", $@"dbSqlFilePath=""{FilePath}{"list.sql"}""");

				// 判斷檔案是否存在
				string FileName = $@"{FilePath}{"RunScripts.bat"}";
				if (!File.Exists(FileName))
				{
					using (StreamWriter writetext = new StreamWriter(FileName,true,Encoding.Default))
					{
						writetext.WriteLine("");
					}
				}
				//清空文件
				FileStream fs;
				fs = new FileStream($@"{FilePath}{"RunScripts.bat"}", FileMode.Truncate, FileAccess.Write);//Truncate模式打開文件可以清空。
				fs.Close();
				//寫入欲執行bat
				using (StreamWriter writer = new StreamWriter($@"{FilePath}{"RunScripts.bat"}",true, Encoding.Default))
				{
					writer.WriteLine(readtext);
				}
				result = MessageBox.Show("是否執行勾選的script?", "提示", MessageBoxButtons.YesNo);
				if (result == DialogResult.Yes)
				{
					//如果log資料夾不存在就自動生成
					if (!Directory.Exists(($@"{FilePath}{"\\"}{"log"}")))
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
				MessageBox.Show("資料夾 路徑錯誤","提示");
            }
		}

        private void FileCheckedList_SelectedIndexChanged(object sender, EventArgs e)
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
