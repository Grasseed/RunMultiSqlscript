﻿namespace RunMultiSqlscript
{
	partial class Form1
	{
		/// <summary>
		/// 設計工具所需的變數。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清除任何使用中的資源。
		/// </summary>
		/// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form 設計工具產生的程式碼

		/// <summary>
		/// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
		/// 這個方法的內容。
		/// </summary>
		private void InitializeComponent()
		{
            this.FileCheckedList = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DBLocation = new System.Windows.Forms.TextBox();
            this.DBName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.UserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.DBConnect = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.ConnectionResult = new System.Windows.Forms.Label();
            this.FolderPath = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SelectFolder = new System.Windows.Forms.Button();
            this.OutputScripts = new System.Windows.Forms.Button();
            this.RunScript = new System.Windows.Forms.Button();
            this.NotifyMsg = new System.Windows.Forms.Label();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // FileCheckedList
            // 
            this.FileCheckedList.CheckOnClick = true;
            this.FileCheckedList.Enabled = false;
            this.FileCheckedList.FormattingEnabled = true;
            this.FileCheckedList.Location = new System.Drawing.Point(347, 143);
            this.FileCheckedList.Name = "FileCheckedList";
            this.FileCheckedList.Size = new System.Drawing.Size(382, 154);
            this.FileCheckedList.TabIndex = 0;
            this.FileCheckedList.Click += new System.EventHandler(this.FileCheckedList_Click);
            this.FileCheckedList.SelectedIndexChanged += new System.EventHandler(this.FileCheckedList_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(345, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "須執行的SQL Script(*.sql):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(207, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "資料庫IP\\資料庫執行個體名稱(DB IP):";
            // 
            // DBLocation
            // 
            this.DBLocation.Location = new System.Drawing.Point(32, 54);
            this.DBLocation.Name = "DBLocation";
            this.DBLocation.Size = new System.Drawing.Size(257, 20);
            this.DBLocation.TabIndex = 3;
            this.DBLocation.Text = ".";
            // 
            // DBName
            // 
            this.DBName.Location = new System.Drawing.Point(32, 114);
            this.DBName.Name = "DBName";
            this.DBName.Size = new System.Drawing.Size(257, 20);
            this.DBName.TabIndex = 5;
            this.DBName.Text = "GoShop";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 98);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(132, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "資料庫名稱(DB Name):";
            // 
            // UserName
            // 
            this.UserName.Location = new System.Drawing.Point(32, 179);
            this.UserName.Name = "UserName";
            this.UserName.Size = new System.Drawing.Size(257, 20);
            this.UserName.TabIndex = 7;
            this.UserName.Text = "sa";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "使用者名稱(User Name):";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(32, 234);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(257, 20);
            this.password.TabIndex = 9;
            this.password.Text = "1qaz@WSX";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 218);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 15);
            this.label5.TabIndex = 8;
            this.label5.Text = "密碼(Password):";
            // 
            // DBConnect
            // 
            this.DBConnect.Location = new System.Drawing.Point(32, 280);
            this.DBConnect.Name = "DBConnect";
            this.DBConnect.Size = new System.Drawing.Size(75, 25);
            this.DBConnect.TabIndex = 10;
            this.DBConnect.Text = "連線";
            this.DBConnect.UseVisualStyleBackColor = true;
            this.DBConnect.Click += new System.EventHandler(this.DBConnect_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(130, 285);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(94, 15);
            this.label6.TabIndex = 11;
            this.label6.Text = "資料庫連線結果:";
            // 
            // ConnectionResult
            // 
            this.ConnectionResult.AutoSize = true;
            this.ConnectionResult.Location = new System.Drawing.Point(225, 285);
            this.ConnectionResult.Name = "ConnectionResult";
            this.ConnectionResult.Size = new System.Drawing.Size(43, 15);
            this.ConnectionResult.TabIndex = 12;
            this.ConnectionResult.Text = "未連線";
            // 
            // FolderPath
            // 
            this.FolderPath.Enabled = false;
            this.FolderPath.Location = new System.Drawing.Point(347, 54);
            this.FolderPath.Name = "FolderPath";
            this.FolderPath.Size = new System.Drawing.Size(257, 20);
            this.FolderPath.TabIndex = 14;
            this.FolderPath.Text = "D:\\Downloads\\High_Perfomance_Sqlscript\\script";
            this.FolderPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FolderPath_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(345, 38);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 15);
            this.label8.TabIndex = 13;
            this.label8.Text = "SQL Scrpit資料夾位置:";
            // 
            // SelectFolder
            // 
            this.SelectFolder.Enabled = false;
            this.SelectFolder.Location = new System.Drawing.Point(625, 54);
            this.SelectFolder.Name = "SelectFolder";
            this.SelectFolder.Size = new System.Drawing.Size(75, 25);
            this.SelectFolder.TabIndex = 15;
            this.SelectFolder.Text = "選擇資料夾";
            this.SelectFolder.UseVisualStyleBackColor = true;
            this.SelectFolder.Click += new System.EventHandler(this.SelectFolder_Click);
            // 
            // OutputScripts
            // 
            this.OutputScripts.Enabled = false;
            this.OutputScripts.Location = new System.Drawing.Point(348, 318);
            this.OutputScripts.Name = "OutputScripts";
            this.OutputScripts.Size = new System.Drawing.Size(102, 25);
            this.OutputScripts.TabIndex = 16;
            this.OutputScripts.Text = "輸出Script清單";
            this.OutputScripts.UseVisualStyleBackColor = true;
            this.OutputScripts.Click += new System.EventHandler(this.OutputScripts_Click);
            // 
            // RunScript
            // 
            this.RunScript.Enabled = false;
            this.RunScript.Location = new System.Drawing.Point(456, 318);
            this.RunScript.Name = "RunScript";
            this.RunScript.Size = new System.Drawing.Size(75, 25);
            this.RunScript.TabIndex = 17;
            this.RunScript.Text = "執行Script";
            this.RunScript.UseVisualStyleBackColor = true;
            this.RunScript.Click += new System.EventHandler(this.RunScript_Click);
            // 
            // NotifyMsg
            // 
            this.NotifyMsg.AutoSize = true;
            this.NotifyMsg.Location = new System.Drawing.Point(350, 147);
            this.NotifyMsg.Name = "NotifyMsg";
            this.NotifyMsg.Size = new System.Drawing.Size(91, 15);
            this.NotifyMsg.TabIndex = 18;
            this.NotifyMsg.Text = "尚未選擇資料夾";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.InfoText;
            this.label7.Location = new System.Drawing.Point(350, 300);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 15);
            this.label7.TabIndex = 19;
            this.label7.Text = "(按選擇順序執行)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 422);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.NotifyMsg);
            this.Controls.Add(this.RunScript);
            this.Controls.Add(this.OutputScripts);
            this.Controls.Add(this.SelectFolder);
            this.Controls.Add(this.FolderPath);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.ConnectionResult);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.DBConnect);
            this.Controls.Add(this.password);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.UserName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.DBName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DBLocation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FileCheckedList);
            this.Name = "Form1";
            this.Text = "RunMultiSqlscript";
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.CheckedListBox FileCheckedList;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox DBLocation;
		private System.Windows.Forms.TextBox DBName;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox UserName;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox password;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button DBConnect;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label ConnectionResult;
		private System.Windows.Forms.TextBox FolderPath;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button SelectFolder;
		private System.Windows.Forms.Button OutputScripts;
		private System.Windows.Forms.Button RunScript;
		private System.Windows.Forms.Label NotifyMsg;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label7;
    }
}

