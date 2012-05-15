using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MegaIDE
{
	/// <summary>
	/// Summary description for AboutBox.
	/// </summary>
	public class AboutDialogClass : System.Windows.Forms.Form
	{

		#region Singleton Implementation
		private static AboutDialogClass aboutDialog;
		/// <summary>
		/// Singleton implementation property.
		/// </summary>
		public static AboutDialogClass AboutDialog
		{
			get
			{
				if(aboutDialog==null)
					aboutDialog=new AboutDialogClass();
				return aboutDialog;
			}
		}
		#endregion

		#region Dialog Gui Objects
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.ColumnHeader nameHeader;
		private System.Windows.Forms.ColumnHeader versionHeader;
		private System.Windows.Forms.ListView assemblyList;
		private System.Windows.Forms.ColumnHeader publicKeyHeader;
		private System.Windows.Forms.Label copyrightLabel;
		private System.Windows.Forms.Label assemblyLabel;
		private System.Windows.Forms.Label versionLabel;
		private System.Windows.Forms.Label megaideLabel;
		private System.Windows.Forms.Label computerNameLabel;
		private System.Windows.Forms.Label userNameLabel;
		private System.Windows.Forms.Label osLabel;
		private System.Windows.Forms.Label memoryLabel;
		private System.Windows.Forms.GroupBox okButtonGroupBox;
		private System.Windows.Forms.LinkLabel websiteLink;
		private System.Windows.Forms.PictureBox topLogoPictureBox;
		private System.Windows.Forms.GroupBox licensedToGroupBox;
		#endregion

		#region Constructor And Other Methods
		/// <summary>
		/// Required designer variable.
		/// </summary>
	
		private AboutDialogClass()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			userNameLabel.Text=System.Environment.UserName;
			computerNameLabel.Text=System.Environment.MachineName;
			
			OperatingSystem osInfo = Environment.OSVersion;
			string osName = "UNKNOWN";
			switch(osInfo.Platform)
			{
				case PlatformID.Win32Windows:
				{
					switch(osInfo.Version.Minor)
					{
						case 0:
						{
							osName = "Windows 95";
							break;
						}

						case 10:
						{
							if(osInfo.Version.Revision.ToString() == "2222A")
							{
								osName = "Windows 98 SE";
							}
							else
							{
								osName = "Windows 98";
							}
							break;
						}

						case 90:
						{
							osName = "Windows Me";
							break;
						}
					}
					break;
				}

				case PlatformID.Win32NT:
				{
					switch(osInfo.Version.Major)
					{
						case 3:
						{
							osName = "Windows NT 3.51";
							break;
						}

						case 4:
						{
							osName = "Windows NT 4.0";
							break;
						}

						case 5:
						{
							if(osInfo.Version.Minor == 0)
							{
								osName = "Windows 2000";
							}
							else if(osInfo.Version.Minor == 1)
							{
								osName = "Windows XP";
							}
							else if(osInfo.Version.Minor == 2)
							{
								osName = "Windows Server 2003";
							}
							break;
						}

						case 6:
						{
							osName = "Windows Vista";
							break;
						}
					}
					break;
				}
			}
			osLabel.Text="Operating System : "+osName;
			NETXP.Win32.API.MEMORYSTATUS memoryStatus=new NETXP.Win32.API.MEMORYSTATUS();
			NETXP.Win32.API.GlobalMemoryStatus(memoryStatus);
			memoryLabel.Text="Physical Memory : "+(memoryStatus.dwTotalPhys/(1023*1023)).ToString("0 MB"); 
			assemblyLabel.Text="Total "+AppDomain.CurrentDomain.GetAssemblies().Length+" assemblies loaded !";	
			foreach(System.Reflection.Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				System.Reflection.AssemblyName assemblyName=assembly.GetName();
				ListViewItem assemblyItem;
				if(assemblyName.Name=="MegaIDE")
				{
					versionLabel.Text="Version : "+assemblyName.Version.Major+"."+assemblyName.Version.Minor+"    Build : "+assemblyName.Version.Build.ToString()+"    Revision : "+assemblyName.Version.Revision;
				}
				if(assemblyName.GetPublicKeyToken()!=null)
					assemblyItem=new ListViewItem(new String[]{assemblyName.Name,assemblyName.Version.ToString(), ToHexString(assemblyName.GetPublicKeyToken())},0);
				else
					assemblyItem=new ListViewItem(new String[]{assemblyName.Name,assemblyName.Version.ToString(),"null"},0);
				assemblyList.Items.Add(assemblyItem);
			}
			assemblyList.Columns[0].Width=-1;
			assemblyList.Columns[1].Width=-1;
			assemblyList.Columns[2].Width=-2;
		}
		
		private static string ToHexString(byte[] bytes) 
		{
			char[] hexDigits = {
								   '0', '1', '2', '3', '4', '5', '6', '7',
								   '8', '9', 'a', 'b', 'c', 'd', 'e', 'f'
							   };

			char[] chars = new char[bytes.Length * 2];

			for (int i = 0; i < bytes.Length; i++) 
			{
				int b = bytes[i];
				chars[i * 2] = hexDigits[b >> 4];
				chars[i * 2 + 1] = hexDigits[b & 0xF];
			}
			return new string(chars);
		} 

		#endregion
		
		#region Windows Form Designer generated code
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		private System.ComponentModel.Container components = null;
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialogClass));
            this.okButtonGroupBox = new System.Windows.Forms.GroupBox();
            this.okButton = new System.Windows.Forms.Button();
            this.websiteLink = new System.Windows.Forms.LinkLabel();
            this.copyrightLabel = new System.Windows.Forms.Label();
            this.megaideLabel = new System.Windows.Forms.Label();
            this.osLabel = new System.Windows.Forms.Label();
            this.memoryLabel = new System.Windows.Forms.Label();
            this.assemblyList = new System.Windows.Forms.ListView();
            this.nameHeader = new System.Windows.Forms.ColumnHeader();
            this.versionHeader = new System.Windows.Forms.ColumnHeader();
            this.publicKeyHeader = new System.Windows.Forms.ColumnHeader();
            this.assemblyLabel = new System.Windows.Forms.Label();
            this.topLogoPictureBox = new System.Windows.Forms.PictureBox();
            this.versionLabel = new System.Windows.Forms.Label();
            this.licensedToGroupBox = new System.Windows.Forms.GroupBox();
            this.userNameLabel = new System.Windows.Forms.Label();
            this.computerNameLabel = new System.Windows.Forms.Label();
            this.okButtonGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topLogoPictureBox)).BeginInit();
            this.licensedToGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // okButtonGroupBox
            // 
            this.okButtonGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.okButtonGroupBox.Controls.Add(this.okButton);
            this.okButtonGroupBox.Controls.Add(this.websiteLink);
            this.okButtonGroupBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.okButtonGroupBox.Location = new System.Drawing.Point(0, 408);
            this.okButtonGroupBox.Name = "okButtonGroupBox";
            this.okButtonGroupBox.Size = new System.Drawing.Size(474, 48);
            this.okButtonGroupBox.TabIndex = 1;
            this.okButtonGroupBox.TabStop = false;
            // 
            // okButton
            // 
            this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.okButton.Location = new System.Drawing.Point(368, 16);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 2;
            this.okButton.Text = "OK";
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // websiteLink
            // 
            this.websiteLink.AutoSize = true;
            this.websiteLink.BackColor = System.Drawing.Color.Transparent;
            this.websiteLink.Font = new System.Drawing.Font("Trebuchet MS", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.websiteLink.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.websiteLink.LinkColor = System.Drawing.Color.Black;
            this.websiteLink.Location = new System.Drawing.Point(24, 16);
            this.websiteLink.Name = "websiteLink";
            this.websiteLink.Size = new System.Drawing.Size(115, 18);
            this.websiteLink.TabIndex = 3;
            this.websiteLink.TabStop = true;
            this.websiteLink.Text = "www.bluchip.co.in";
            this.websiteLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.websiteLink_LinkClicked);
            // 
            // copyrightLabel
            // 
            this.copyrightLabel.AutoSize = true;
            this.copyrightLabel.BackColor = System.Drawing.Color.Transparent;
            this.copyrightLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.copyrightLabel.ForeColor = System.Drawing.Color.White;
            this.copyrightLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.copyrightLabel.Location = new System.Drawing.Point(24, 390);
            this.copyrightLabel.Name = "copyrightLabel";
            this.copyrightLabel.Size = new System.Drawing.Size(387, 15);
            this.copyrightLabel.TabIndex = 7;
            this.copyrightLabel.Text = "Copyright © 2007-2008 BluChip Embedded Systems and Automation .";
            this.copyrightLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // megaideLabel
            // 
            this.megaideLabel.BackColor = System.Drawing.Color.Transparent;
            this.megaideLabel.Font = new System.Drawing.Font("Trebuchet MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.megaideLabel.ForeColor = System.Drawing.Color.White;
            this.megaideLabel.Image = ((System.Drawing.Image)(resources.GetObject("megaideLabel.Image")));
            this.megaideLabel.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.megaideLabel.Location = new System.Drawing.Point(16, 82);
            this.megaideLabel.Name = "megaideLabel";
            this.megaideLabel.Size = new System.Drawing.Size(232, 32);
            this.megaideLabel.TabIndex = 0;
            this.megaideLabel.Text = "MegaIDE v1.0 Beta";
            this.megaideLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // osLabel
            // 
            this.osLabel.BackColor = System.Drawing.Color.Transparent;
            this.osLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.osLabel.ForeColor = System.Drawing.Color.White;
            this.osLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.osLabel.Location = new System.Drawing.Point(24, 196);
            this.osLabel.Name = "osLabel";
            this.osLabel.Size = new System.Drawing.Size(276, 20);
            this.osLabel.TabIndex = 0;
            this.osLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // memoryLabel
            // 
            this.memoryLabel.BackColor = System.Drawing.Color.Transparent;
            this.memoryLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoryLabel.ForeColor = System.Drawing.Color.White;
            this.memoryLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.memoryLabel.Location = new System.Drawing.Point(308, 196);
            this.memoryLabel.Name = "memoryLabel";
            this.memoryLabel.Size = new System.Drawing.Size(160, 20);
            this.memoryLabel.TabIndex = 0;
            this.memoryLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // assemblyList
            // 
            this.assemblyList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameHeader,
            this.versionHeader,
            this.publicKeyHeader});
            this.assemblyList.Location = new System.Drawing.Point(24, 246);
            this.assemblyList.Name = "assemblyList";
            this.assemblyList.Size = new System.Drawing.Size(440, 136);
            this.assemblyList.TabIndex = 1;
            this.assemblyList.UseCompatibleStateImageBehavior = false;
            this.assemblyList.View = System.Windows.Forms.View.Details;
            // 
            // nameHeader
            // 
            this.nameHeader.Text = "Name";
            // 
            // versionHeader
            // 
            this.versionHeader.Text = "Version";
            // 
            // publicKeyHeader
            // 
            this.publicKeyHeader.Text = "Public Key";
            // 
            // assemblyLabel
            // 
            this.assemblyLabel.BackColor = System.Drawing.Color.Transparent;
            this.assemblyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assemblyLabel.ForeColor = System.Drawing.Color.White;
            this.assemblyLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.assemblyLabel.Location = new System.Drawing.Point(24, 222);
            this.assemblyLabel.Name = "assemblyLabel";
            this.assemblyLabel.Size = new System.Drawing.Size(440, 20);
            this.assemblyLabel.TabIndex = 0;
            this.assemblyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // topLogoPictureBox
            // 
            this.topLogoPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.topLogoPictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.topLogoPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("topLogoPictureBox.Image")));
            this.topLogoPictureBox.Location = new System.Drawing.Point(0, 0);
            this.topLogoPictureBox.Name = "topLogoPictureBox";
            this.topLogoPictureBox.Size = new System.Drawing.Size(474, 70);
            this.topLogoPictureBox.TabIndex = 18;
            this.topLogoPictureBox.TabStop = false;
            // 
            // versionLabel
            // 
            this.versionLabel.BackColor = System.Drawing.Color.Transparent;
            this.versionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.versionLabel.ForeColor = System.Drawing.Color.White;
            this.versionLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.versionLabel.Location = new System.Drawing.Point(64, 102);
            this.versionLabel.Name = "versionLabel";
            this.versionLabel.Size = new System.Drawing.Size(296, 23);
            this.versionLabel.TabIndex = 0;
            this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // licensedToGroupBox
            // 
            this.licensedToGroupBox.BackColor = System.Drawing.Color.Transparent;
            this.licensedToGroupBox.Controls.Add(this.userNameLabel);
            this.licensedToGroupBox.Controls.Add(this.computerNameLabel);
            this.licensedToGroupBox.ForeColor = System.Drawing.Color.White;
            this.licensedToGroupBox.Location = new System.Drawing.Point(24, 130);
            this.licensedToGroupBox.Name = "licensedToGroupBox";
            this.licensedToGroupBox.Size = new System.Drawing.Size(440, 60);
            this.licensedToGroupBox.TabIndex = 0;
            this.licensedToGroupBox.TabStop = false;
            this.licensedToGroupBox.Text = "Licensed To :";
            // 
            // userNameLabel
            // 
            this.userNameLabel.AutoSize = true;
            this.userNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.userNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.userNameLabel.ForeColor = System.Drawing.Color.White;
            this.userNameLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.userNameLabel.Location = new System.Drawing.Point(12, 16);
            this.userNameLabel.Name = "userNameLabel";
            this.userNameLabel.Size = new System.Drawing.Size(0, 15);
            this.userNameLabel.TabIndex = 0;
            this.userNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // computerNameLabel
            // 
            this.computerNameLabel.AutoSize = true;
            this.computerNameLabel.BackColor = System.Drawing.Color.Transparent;
            this.computerNameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.computerNameLabel.ForeColor = System.Drawing.Color.White;
            this.computerNameLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.computerNameLabel.Location = new System.Drawing.Point(12, 36);
            this.computerNameLabel.Name = "computerNameLabel";
            this.computerNameLabel.Size = new System.Drawing.Size(0, 15);
            this.computerNameLabel.TabIndex = 0;
            this.computerNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // AboutDialogClass
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(474, 456);
            this.Controls.Add(this.licensedToGroupBox);
            this.Controls.Add(this.versionLabel);
            this.Controls.Add(this.topLogoPictureBox);
            this.Controls.Add(this.assemblyLabel);
            this.Controls.Add(this.assemblyList);
            this.Controls.Add(this.memoryLabel);
            this.Controls.Add(this.osLabel);
            this.Controls.Add(this.megaideLabel);
            this.Controls.Add(this.okButtonGroupBox);
            this.Controls.Add(this.copyrightLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialogClass";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "About MegaIDE";
            this.okButtonGroupBox.ResumeLayout(false);
            this.okButtonGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topLogoPictureBox)).EndInit();
            this.licensedToGroupBox.ResumeLayout(false);
            this.licensedToGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		#region Events
		private void okButton_Click(object sender, System.EventArgs e)
		{
			DialogResult=DialogResult.OK;
		}

		private void websiteLink_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				System.Diagnostics.Process start=new System.Diagnostics.Process();
				start.StartInfo.FileName="explorer.exe";
				start.StartInfo.Arguments="http://www.bluchip.co.in";
				start.Start();
				start.Dispose();
			}
			catch{}
		}
		#endregion

	}
}
