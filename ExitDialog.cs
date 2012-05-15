using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MegaIDE
{
	/// <summary>
	/// ExitSaveDialog Class : Form which shows list of all unsaved files when closing the MegaIDE or a project.
	/// </summary>
	public sealed class ExitDialogClass : System.Windows.Forms.Form
	{

		#region Singleton Implementation

		private static ExitDialogClass exitDialog;
		/// <summary>
		/// Singleton implementation property.
		/// </summary>
		public static ExitDialogClass ExitDialog
		{
			get
			{
				if(exitDialog==null)
					exitDialog=new ExitDialogClass();
				return exitDialog;
			}
		}
		
		#endregion

		#region Listitem enumeration and class

		/// <summary>
		/// Enumeration for the item type in the unsaved files list.
		/// </summary>
		public enum SaveFileTypes
		{
			/// <summary>
			/// Marks the list entry for a code file.
			/// </summary>
			File,
			/// <summary>
			/// Marks the list entry for a project file.
			/// </summary>
			ProjectFile
		};
		/// <summary>
		/// Class type for items in the unsaved files list containing file path and item type. 
		/// </summary>
		public class ListItem
		{
			string fileName;
			SaveFileTypes saveFileType;
			/// <summary>
			/// Gets the file type entry for the list item.
			/// </summary>
			public SaveFileTypes SaveFileType
			{
				get{return saveFileType;}
				//set{saveFileType=value;}
			}
			/// <summary>
			/// Gets the file path of the list item.
			/// </summary>
			public string FileName
			{
				get{return fileName;}
			}
			
			/// <summary>
			/// Constructor
			/// </summary>
			/// <param name="fileName">Full File Name</param>
			/// <param name="saveFileType">Item Type</param>
			public ListItem(string fileName,SaveFileTypes saveFileType)
			{
				this.saveFileType=saveFileType;
				this.fileName=fileName;
			}
			/// <summary>
			/// ToString method.
			/// </summary>
			/// <returns>
			/// File name which the current list item represents.
			/// </returns>
			public override string ToString()
			{
				return System.IO.Path.GetFileName(fileName);
			}
		}
		
		#endregion

		#region Private Form Variables
		private System.Windows.Forms.Button yesButton;
		private System.Windows.Forms.Button noButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ListBox filesList;
		private System.Windows.Forms.Label infoLabel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		#endregion

		#region Form Functions
		/// <summary>
		/// Returns items selected for saving.
		/// </summary>
		public ListBox.SelectedObjectCollection SelectedItems
		{
			get { return filesList.SelectedItems;}
        }		
		/// <summary>
		/// ExitDialogClass contructor.
		/// </summary>
					 
		private ExitDialogClass()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		}

		/// <summary>
		/// Shows save dialog showing unsaved items when MegaIDE is closed.
		/// </summary>
		/// <param name="parent">
		/// Parent form.
		/// </param>
		/// <param name="unsavedFilesList">
		/// Arraylist containing the paths of the unsaved files.
		/// </param>
		/// <returns></returns>
		public DialogResult ShowSaveDialog(System.Windows.Forms.IWin32Window parent,ArrayList unsavedFilesList)
		{ 
			filesList.Items.Clear();
			if(MainFormClass.MainForm.IsProjectOpen)
			{	
				if(ProjectManagerClass.ProjectManager.IsProjectModified)
				{
					ListItem newItem=new ListItem(ProjectManagerClass.ProjectManager.ProjectFileName,SaveFileTypes.ProjectFile);
					filesList.SelectedIndex=filesList.Items.Add(newItem);
				}
			}
			foreach(string fileName in unsavedFilesList)
			{
				ListItem newItem=new ListItem(fileName,SaveFileTypes.File);
				if(ProjectManagerClass.ProjectManager.IsProjectModified && ProjectManagerClass.ProjectManager.GetFileNode(newItem.FileName)!=null)	
				{
					filesList.Items.Insert(1,"   "+newItem);
					filesList.SelectedIndex=1;
				}
				else
					filesList.SelectedIndex=filesList.Items.Add(newItem);
			}
			if(filesList.Items.Count==0)
				return DialogResult.No;
			return ShowDialog(parent);
		}

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
		/// Shows missing files when a project is opened.
		/// </summary>
		/// <param name="parent">
		/// Parent form.
		/// </param>
		/// <param name="fileListArray">
		/// Arraylist containing the paths of the missing files.
		/// </param>
		/// <returns></returns>
		public DialogResult ShowMissingFilesDialog(System.Windows.Forms.IWin32Window parent,ArrayList fileListArray)
		{
			infoLabel.Text="Following items do not exist! Click OK to remove them from the project.";
			yesButton.Visible=false;
			noButton.Text="OK";
			filesList.Items.Clear();
			foreach(string fileName in fileListArray)
				filesList.Items.Add(fileName);
			return ShowDialog(parent);
		}

		/// <summary>
		/// Restores the default foem look.
		/// </summary>
		public void RestoreSettings()
		{
			infoLabel.Text="Save changes to following items?";
			noButton.Text="No";
			yesButton.Visible=true;
		}

		#endregion

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.infoLabel = new System.Windows.Forms.Label();
			this.yesButton = new System.Windows.Forms.Button();
			this.noButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.filesList = new System.Windows.Forms.ListBox();
			this.SuspendLayout();
			// 
			// infoLabel
			// 
			this.infoLabel.Location = new System.Drawing.Point(8, 8);
			this.infoLabel.Name = "infoLabel";
			this.infoLabel.Size = new System.Drawing.Size(360, 23);
			this.infoLabel.TabIndex = 0;
			this.infoLabel.Text = "Save changes to following items?";
			// 
			// yesButton
			// 
			this.yesButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.yesButton.Location = new System.Drawing.Point(120, 208);
			this.yesButton.Name = "yesButton";
			this.yesButton.TabIndex = 1;
			this.yesButton.Text = "Yes";
			this.yesButton.Click += new System.EventHandler(this.yesButton_Click);
			// 
			// noButton
			// 
			this.noButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.noButton.Location = new System.Drawing.Point(208, 208);
			this.noButton.Name = "noButton";
			this.noButton.TabIndex = 2;
			this.noButton.Text = "No";
			this.noButton.Click += new System.EventHandler(this.noButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(296, 208);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// filesList
			// 
			this.filesList.HorizontalScrollbar = true;
			this.filesList.Location = new System.Drawing.Point(8, 32);
			this.filesList.Name = "filesList";
			this.filesList.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
			this.filesList.Size = new System.Drawing.Size(360, 160);
			this.filesList.TabIndex = 4;
			// 
			// ExitDialogClass
			// 
			this.AcceptButton = this.yesButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(376, 248);
			this.Controls.Add(this.filesList);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.noButton);
			this.Controls.Add(this.yesButton);
			this.Controls.Add(this.infoLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ExitDialogClass";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "MegaIDE";
			this.ResumeLayout(false);

		}
		#endregion

		#region Event Handlers for the form
		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			DialogResult=DialogResult.Cancel;
		}

		private void noButton_Click(object sender, System.EventArgs e)
		{
			DialogResult=DialogResult.No;
		}

		private void yesButton_Click(object sender, System.EventArgs e)
		{
			DialogResult=DialogResult.Yes;
		}
		#endregion	

	}
}
