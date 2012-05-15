using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Text;

namespace MegaIDE
{
	/// <summary>
	/// This form shows the new document dialog.
	/// </summary>
	public sealed class NewDocumentDialogClass: System.Windows.Forms.Form
	{
		/// <summary>
		/// Dialog Modes Enumeration .
		/// </summary>
		public enum DialogModes:byte {
			/// <summary>
			/// Mode for new file dialog .
			/// </summary>
			NewFileMode,
			/// <summary>
			/// Mode for add file to project .
			/// </summary>
			AddFileMode,
			/// <summary>
			/// Mode for new project .
			/// </summary>
			NewProjectMode,
			/// <summary>
			/// Null for initializer .
			/// </summary>
			None
		};

		#region Singleton Implementation

		private static NewDocumentDialogClass newDocumentDialog;
		/// <summary>
		/// Singleton implementation static variable for New Document Dialog.
		/// </summary>
		public static NewDocumentDialogClass NewDocumentDialog
		{
			get
			{
				if(newDocumentDialog==null)
					newDocumentDialog=new NewDocumentDialogClass();
				return newDocumentDialog;
			}
		}
		
		#endregion

		#region Private Variables
		private System.Windows.Forms.Button browseButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox fileNameTextBox;
		private System.Windows.Forms.Label nameLabel;
		private System.Windows.Forms.Label locationLabel;
		private System.Windows.Forms.GroupBox buttonsGroup;
		private System.Windows.Forms.Button smallIconButton;
		private System.Windows.Forms.Button largeIconButton;
		private System.Windows.Forms.ListView typeList;
		private System.Windows.Forms.TreeView categoryTree;
		private System.Windows.Forms.ImageList smallIconsImageList;
		private System.Windows.Forms.ImageList largeIconsImageList;
		private System.Windows.Forms.Label categoryLabel;
		private System.Windows.Forms.Label typeLabel;
		private System.Windows.Forms.ImageList treeImageList;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
		private System.ComponentModel.IContainer components;
        private string fileExtension;
		private System.Windows.Forms.CheckBox isSeperateDirectory;
		private string fullFileName;
		private string fileName;
		private string folderName;
		private NETXP.Controls.ComboBoxEx fileLocationCombo;
		private DialogModes dialogMode;
        private MegaIDE.ProjectTypes projectType;
		#endregion
		
		#region Properties

		/// <summary>
		/// Gets the project type for the new project selected.
		/// </summary>
		public ProjectTypes ProjectType
		{
			get{return projectType;}
		}
		
		/// <summary>
		/// Gets or Sets the dialog mode.
		/// </summary>
		public DialogModes DialogMode
		{
			get{return dialogMode;}
			set{dialogMode=value;}
		}

		/// <summary>
		/// Gets the fully qualified filename of the new document file.
		/// </summary>
		public string FullFileName
		{
			get{return fullFileName;}
		}

		/// <summary>
		/// Gets the filename of the new document created.
		/// </summary>
		public string FileName
		{
			get{return fileName;}
		}
		
		#endregion

		#region Form Functions

		/// <summary>
		/// New Document Dialog Class Contructor.
		/// </summary>
		private NewDocumentDialogClass()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			largeIconButton_Click(null,null);
			dialogMode=DialogModes.None;
		}

		/// <summary>
		/// Shows "Add files to a project" dialog box. 
		/// </summary>
		/// <param name="parent">
		/// Parent form.
		/// </param>
		/// <param name="projectFolder">
		/// Path of the project folder.
		/// </param>
		/// <returns>
		/// Dialog result as DialogResult.OK or DialogResult.Cancel . 
		/// </returns>
		public System.Windows.Forms.DialogResult ShowAddFileProjectDialog(System.Windows.Forms.IWin32Window parent,string projectFolder)
		{	
			if(dialogMode!=DialogModes.AddFileMode)
			{	
				browseButton.Enabled=false;     
				fileLocationCombo.Enabled=false;
				dialogMode=DialogModes.AddFileMode;
				isSeperateDirectory.Checked=false;
				isSeperateDirectory.Enabled=false;
				isSeperateDirectory.Visible=false;
				categoryTree.Nodes.Clear();
				categoryTree.Refresh();
				categoryTree.Nodes.Add(new System.Windows.Forms.TreeNode("Source Files",0,1));
				categoryTree.Nodes.Add(new System.Windows.Forms.TreeNode("Text Files",0,1));
				categoryTree.SelectedNode=categoryTree.Nodes[0];
				categoryTree.Refresh();		
				folderBrowserDialog.Description="Folder for new file";
				fileLocationCombo.Text=projectFolder;
				Text="New File";
			}
			return ShowDialog();
		}

		/// <summary>
		/// Shows "New Project" dialog box.
		/// </summary>
		/// <param name="parent">
		/// Parent form.
		/// </param>
		/// <returns>
		/// Dialog result as DialogResult.OK or DialogResult.Cancel .
		/// </returns>
		public System.Windows.Forms.DialogResult ShowNewProjectDialog(System.Windows.Forms.IWin32Window parent)
		{
			if(dialogMode!=DialogModes.NewProjectMode)
			{	
				browseButton.Enabled=true;
				dialogMode=DialogModes.NewProjectMode;
				isSeperateDirectory.Checked=true;
				isSeperateDirectory.Enabled=true;
				isSeperateDirectory.Visible=true;
				categoryTree.Nodes.Clear();
				categoryTree.Refresh();
				categoryTree.Nodes.Add(new System.Windows.Forms.TreeNode("Megaboard Projects",0,1));
				categoryTree.Nodes.Add(new System.Windows.Forms.TreeNode("AVR Projects",0,1));
				categoryTree.SelectedNode=categoryTree.Nodes[0];
				categoryTree.Refresh();
				folderBrowserDialog.Description="Folder for new project";
				fileLocationCombo.Text=folderBrowserDialog.SelectedPath=System.IO.Directory.GetCurrentDirectory();
				Text="New Project";
			}
            return ShowDialog(parent); 
		}
		
		/// <summary>
		/// Shows "New File" dialog box.
		/// </summary>
		/// <param name="parent">
		/// Parent form.
		/// </param>
		/// <returns>
		/// Dialog result as DialogResult.OK or DialogResult.Cancel .
		/// </returns>
		public System.Windows.Forms.DialogResult ShowNewFileDialog(System.Windows.Forms.IWin32Window parent)
		{
			if(dialogMode!=DialogModes.NewFileMode)
			{	
				browseButton.Enabled=true;
				fileLocationCombo.Enabled=true;
				dialogMode=DialogModes.NewFileMode;
				isSeperateDirectory.Checked=false;
				isSeperateDirectory.Enabled=false;
				isSeperateDirectory.Visible=false;
				categoryTree.Nodes.Clear();
				categoryTree.Refresh();
				categoryTree.Nodes.Add(new System.Windows.Forms.TreeNode("Source Files",0,1));
				categoryTree.Nodes.Add(new System.Windows.Forms.TreeNode("Text Files",0,1));
				categoryTree.SelectedNode=categoryTree.Nodes[0];
				categoryTree.Refresh();
				folderBrowserDialog.Description="Folder for new file";
				fileLocationCombo.Text=folderBrowserDialog.SelectedPath=System.IO.Directory.GetCurrentDirectory();
				Text="New File";
			}
			return ShowDialog(parent);
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

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(NewDocumentDialogClass));
			this.fileNameTextBox = new System.Windows.Forms.TextBox();
			this.typeList = new System.Windows.Forms.ListView();
			this.largeIconsImageList = new System.Windows.Forms.ImageList(this.components);
			this.smallIconsImageList = new System.Windows.Forms.ImageList(this.components);
			this.categoryTree = new System.Windows.Forms.TreeView();
			this.treeImageList = new System.Windows.Forms.ImageList(this.components);
			this.nameLabel = new System.Windows.Forms.Label();
			this.locationLabel = new System.Windows.Forms.Label();
			this.browseButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.buttonsGroup = new System.Windows.Forms.GroupBox();
			this.isSeperateDirectory = new System.Windows.Forms.CheckBox();
			this.smallIconButton = new System.Windows.Forms.Button();
			this.largeIconButton = new System.Windows.Forms.Button();
			this.categoryLabel = new System.Windows.Forms.Label();
			this.typeLabel = new System.Windows.Forms.Label();
			this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
			this.fileLocationCombo = new NETXP.Controls.ComboBoxEx();
			this.buttonsGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// fileNameTextBox
			// 
			this.fileNameTextBox.Location = new System.Drawing.Point(64, 184);
			this.fileNameTextBox.Name = "fileNameTextBox";
			this.fileNameTextBox.Size = new System.Drawing.Size(392, 20);
			this.fileNameTextBox.TabIndex = 3;
			this.fileNameTextBox.Text = "";
			// 
			// typeList
			// 
			this.typeList.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
			this.typeList.AllowColumnReorder = true;
			this.typeList.AutoArrange = false;
			this.typeList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.typeList.GridLines = true;
			this.typeList.HideSelection = false;
			this.typeList.LargeImageList = this.largeIconsImageList;
			this.typeList.Location = new System.Drawing.Point(232, 32);
			this.typeList.MultiSelect = false;
			this.typeList.Name = "typeList";
			this.typeList.Size = new System.Drawing.Size(224, 144);
			this.typeList.SmallImageList = this.smallIconsImageList;
			this.typeList.TabIndex = 2;
			this.typeList.View = System.Windows.Forms.View.SmallIcon;
			this.typeList.SelectedIndexChanged += new System.EventHandler(this.typeList_SelectedIndexChanged);
			// 
			// largeIconsImageList
			// 
			this.largeIconsImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.largeIconsImageList.ImageSize = new System.Drawing.Size(32, 32);
			this.largeIconsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("largeIconsImageList.ImageStream")));
			this.largeIconsImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// smallIconsImageList
			// 
			this.smallIconsImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.smallIconsImageList.ImageSize = new System.Drawing.Size(18, 18);
			this.smallIconsImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("smallIconsImageList.ImageStream")));
			this.smallIconsImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// categoryTree
			// 
			this.categoryTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.categoryTree.ImageList = this.treeImageList;
			this.categoryTree.Location = new System.Drawing.Point(8, 32);
			this.categoryTree.Name = "categoryTree";
			this.categoryTree.SelectedImageIndex = 1;
			this.categoryTree.ShowRootLines = false;
			this.categoryTree.Size = new System.Drawing.Size(216, 144);
			this.categoryTree.TabIndex = 1;
			this.categoryTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.categoryTree_AfterSelect);
			// 
			// treeImageList
			// 
			this.treeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.treeImageList.ImageSize = new System.Drawing.Size(20, 20);
			this.treeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImageList.ImageStream")));
			this.treeImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// nameLabel
			// 
			this.nameLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.nameLabel.Location = new System.Drawing.Point(8, 184);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(56, 23);
			this.nameLabel.TabIndex = 4;
			this.nameLabel.Text = "Name :";
			// 
			// locationLabel
			// 
			this.locationLabel.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.locationLabel.Location = new System.Drawing.Point(8, 224);
			this.locationLabel.Name = "locationLabel";
			this.locationLabel.Size = new System.Drawing.Size(56, 23);
			this.locationLabel.TabIndex = 5;
			this.locationLabel.Text = "Location :";
			// 
			// browseButton
			// 
			this.browseButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.browseButton.Location = new System.Drawing.Point(384, 224);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(72, 23);
			this.browseButton.TabIndex = 5;
			this.browseButton.Text = "Browse...";
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// okButton
			// 
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.okButton.Location = new System.Drawing.Point(288, 16);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(80, 23);
			this.okButton.TabIndex = 7;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(376, 16);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(80, 23);
			this.cancelButton.TabIndex = 8;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// buttonsGroup
			// 
			this.buttonsGroup.Controls.Add(this.isSeperateDirectory);
			this.buttonsGroup.Controls.Add(this.okButton);
			this.buttonsGroup.Controls.Add(this.cancelButton);
			this.buttonsGroup.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonsGroup.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.buttonsGroup.Location = new System.Drawing.Point(0, 254);
			this.buttonsGroup.Name = "buttonsGroup";
			this.buttonsGroup.Size = new System.Drawing.Size(466, 48);
			this.buttonsGroup.TabIndex = 6;
			this.buttonsGroup.TabStop = false;
			// 
			// isSeperateDirectory
			// 
			this.isSeperateDirectory.Enabled = false;
			this.isSeperateDirectory.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.isSeperateDirectory.Location = new System.Drawing.Point(24, 16);
			this.isSeperateDirectory.Name = "isSeperateDirectory";
			this.isSeperateDirectory.Size = new System.Drawing.Size(168, 24);
			this.isSeperateDirectory.TabIndex = 8;
			this.isSeperateDirectory.Text = "Create directory for Project";
			this.isSeperateDirectory.Visible = false;
			// 
			// smallIconButton
			// 
			this.smallIconButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.smallIconButton.Image = ((System.Drawing.Image)(resources.GetObject("smallIconButton.Image")));
			this.smallIconButton.Location = new System.Drawing.Point(408, 8);
			this.smallIconButton.Name = "smallIconButton";
			this.smallIconButton.Size = new System.Drawing.Size(24, 23);
			this.smallIconButton.TabIndex = 9;
			this.smallIconButton.Click += new System.EventHandler(this.smallIconButton_Click);
			// 
			// largeIconButton
			// 
			this.largeIconButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.largeIconButton.Image = ((System.Drawing.Image)(resources.GetObject("largeIconButton.Image")));
			this.largeIconButton.Location = new System.Drawing.Point(432, 8);
			this.largeIconButton.Name = "largeIconButton";
			this.largeIconButton.Size = new System.Drawing.Size(24, 23);
			this.largeIconButton.TabIndex = 10;
			this.largeIconButton.Click += new System.EventHandler(this.largeIconButton_Click);
			// 
			// categoryLabel
			// 
			this.categoryLabel.Location = new System.Drawing.Point(8, 16);
			this.categoryLabel.Name = "categoryLabel";
			this.categoryLabel.Size = new System.Drawing.Size(100, 16);
			this.categoryLabel.TabIndex = 13;
			this.categoryLabel.Text = "Category :";
			// 
			// typeLabel
			// 
			this.typeLabel.Location = new System.Drawing.Point(232, 16);
			this.typeLabel.Name = "typeLabel";
			this.typeLabel.Size = new System.Drawing.Size(100, 16);
			this.typeLabel.TabIndex = 14;
			this.typeLabel.Text = "Type :";
			// 
			// fileLocationCombo
			// 
			this.fileLocationCombo.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.fileLocationCombo.Flags = ((NETXP.Controls.AutoCompleteFlags)((NETXP.Controls.AutoCompleteFlags.FileSystemDirs | NETXP.Controls.AutoCompleteFlags.AutoSuggestForceOn)));
			this.fileLocationCombo.Location = new System.Drawing.Point(64, 224);
			this.fileLocationCombo.MRUHive = NETXP.Controls.MRUKeyHive.LocalMachine;
			this.fileLocationCombo.MRUKey = "Software\\RoboAntz Labs\\MegaIDE\\Recent Documents\\RecentRecentlyTyped";
			this.fileLocationCombo.Name = "fileLocationCombo";
			this.fileLocationCombo.Size = new System.Drawing.Size(304, 21);
			this.fileLocationCombo.TabIndex = 15;
			// 
			// NewDocumentDialogClass
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(466, 302);
			this.Controls.Add(this.fileLocationCombo);
			this.Controls.Add(this.typeLabel);
			this.Controls.Add(this.categoryLabel);
			this.Controls.Add(this.largeIconButton);
			this.Controls.Add(this.smallIconButton);
			this.Controls.Add(this.buttonsGroup);
			this.Controls.Add(this.browseButton);
			this.Controls.Add(this.locationLabel);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.categoryTree);
			this.Controls.Add(this.typeList);
			this.Controls.Add(this.fileNameTextBox);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "NewDocumentDialogClass";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.buttonsGroup.ResumeLayout(false);
			this.ResumeLayout(false);

		}
		#endregion

		#region Event Handlers

		private void smallIconButton_Click(object sender, System.EventArgs e)
		{
			smallIconButton.Enabled=false;
			largeIconButton.Enabled=true;
			typeList.View=System.Windows.Forms.View.List;
		}

		private void largeIconButton_Click(object sender, System.EventArgs e)
		{
			largeIconButton.Enabled=false;
			smallIconButton.Enabled=true;			
			typeList.View=System.Windows.Forms.View.LargeIcon;
		    typeList.Refresh();
		}

		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			DialogResult=DialogResult.Cancel;
		}


		/// <summary>
		/// To be reviewed later
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void categoryTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
		{
			if(dialogMode==DialogModes.NewFileMode || dialogMode==DialogModes.AddFileMode)
			{
				if(categoryTree.SelectedNode.Index==0)
				{
					typeList.Items.Clear();
					System.Windows.Forms.ListViewItem newHFile = new System.Windows.Forms.ListViewItem("Header File", 0);
					System.Windows.Forms.ListViewItem newCFile = new System.Windows.Forms.ListViewItem("C Source File", 1);
					newHFile.Tag=".h";
					newCFile.Tag=".c";
					newHFile.StateImageIndex = 0;
					newCFile.StateImageIndex = 0;
					typeList.Items.AddRange(new System.Windows.Forms.ListViewItem[]{newHFile,newCFile});

				}
				else if(categoryTree.SelectedNode.Index==1)
				{
					typeList.Items.Clear();
					System.Windows.Forms.ListViewItem newTextFile = new System.Windows.Forms.ListViewItem("Text File", 2);
					newTextFile.StateImageIndex = 0;
					newTextFile.Tag=".txt";
					typeList.Items.Add(newTextFile);

				}
			}
			else
			{
				if(categoryTree.SelectedNode.Index==0)
				{
					typeList.Items.Clear();
					System.Windows.Forms.ListViewItem newProjectFile = new System.Windows.Forms.ListViewItem("MegaBoard Project", 3);
					newProjectFile.StateImageIndex = 0;
					newProjectFile.Tag=".mbp";
					typeList.Items.Add(newProjectFile);
					projectType=ProjectTypes.MegaBoard_v1; 
				}
				else if(categoryTree.SelectedNode.Index==1)
				{
					typeList.Items.Clear();
					System.Windows.Forms.ListViewItem newProjectFile = new System.Windows.Forms.ListViewItem("ATMega128 Project", 4);
					newProjectFile.StateImageIndex = 0;
					newProjectFile.Tag=".mbp";
					typeList.Items.Add(newProjectFile);
					projectType=ProjectTypes.ATMega128_Project;
				}
			}
			typeList.Items[0].Selected=true;
		}
		
		private void typeList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(typeList.SelectedItems.Count!=0)
			{
				okButton.Enabled=true;
				fileNameTextBox.Text=typeList.SelectedItems[0].Text;
		  	    fileExtension=typeList.SelectedItems[0].Tag as string;
			}
		}

		private void browseButton_Click(object sender, System.EventArgs e)
		{
			folderBrowserDialog.SelectedPath=fileLocationCombo.Text;
			if(folderBrowserDialog.ShowDialog()==DialogResult.OK)
				fileLocationCombo.Text=folderBrowserDialog.SelectedPath;
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			fileName=fileNameTextBox.Text.Trim();
			folderName=fileLocationCombo.Text.Trim();
			if(folderName.EndsWith("\\"))
				folderName=folderName.TrimEnd(new char[]{'\\'});
			fullFileName=fileLocationCombo.Text.Trim();
			if(!System.IO.Directory.Exists(folderName))
			{
				MessageBox.Show("Folder path "+folderName+" not valid !","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return;
			}
			if(isSeperateDirectory.Checked)
			{	
				try
				{
						System.IO.Directory.CreateDirectory(folderName+"\\"+fileName);
							folderName=folderName+"\\"+fileName;	
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
					return;
				}
			}
			fullFileName=folderName+"\\"+fileName+fileExtension;
			if(System.IO.File.Exists(fullFileName))
               if(MessageBox.Show(fullFileName+" already exists.\nDo you want to replace it ?","MegaIDE",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.No)
				   return;
			try
			{
				System.IO.FileStream fileStream=new System.IO.FileStream(fullFileName,System.IO.FileMode.Create);
				if(fileExtension==".h")
				{	
					System.IO.StreamWriter streamWriter=new System.IO.StreamWriter(fileStream);
					streamWriter.Write(String.Format("#ifndef _{0}_H_\n#define _{0}_H_ 1\n\n//Add code here\n\n#endif /* _{0}_H_ */",fileName.ToUpper()));
					streamWriter.Close();
				}
				fileStream.Close();
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
			    return;
			}
            DialogResult=DialogResult.OK;
		}
		
		#endregion

	}
}
