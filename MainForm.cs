using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using NETXP.Controls.Docking;
using NETXP.Controls.TaskPane;
using NETXP.Controls.Bars;
using QWEditor;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Diagnostics;
using System.Threading;


namespace MegaIDE
{
	/// <summary>
	/// MainForm Class . Contains the IDE code and the file functions(e.g. Open,Close,Save etc) . Links with the ProjectManager Class for complete implementation.
	/// </summary>
	public sealed class MainFormClass : System.Windows.Forms.Form
	{

		#region Singleton Implementation

		private static MainFormClass mainForm;

		/// <summary>
		/// Gets the singleton form variable.
		/// </summary>
		public static MainFormClass MainForm
		{
			get
			{
				return mainForm;
			}
		}
		
		/// <summary>
		/// Sets the singleton form variable.
		/// </summary>
		public static MainFormClass MainFormInitializer
		{
			set
			{
				mainForm = value;
			}
		}
		#endregion
		
		#region Form Variables

		private NETXP.Controls.Bars.CommandBarManager commandBarManager;
		private NETXP.Controls.Bars.CommandBarDock commandBarDockLeft;
		private NETXP.Controls.Bars.CommandBarDock commandBarDockRight;
		private NETXP.Controls.Bars.CommandBarDock commandBarDockTop;
		private NETXP.Controls.Bars.CommandBarDock commandBarDockBottom;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem1;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem2;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem3;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem4;
		private NETXP.Controls.Bars.CommandBarButtonItem fileMenu;
		private NETXP.Controls.Bars.CommandBarButtonItem editMenu;
		private NETXP.Controls.Bars.CommandBarButtonItem viewMenu;
		private NETXP.Controls.Bars.CommandBarButtonItem optionsMenu;
		private NETXP.Controls.Bars.CommandBarButtonItem aboutMenu;
		private NETXP.Controls.Bars.StatusBar statusBar;
		private NETXP.Controls.Docking.DockingManagerExtender dockingManagerExtender;
		private NETXP.Controls.Bars.CommandBarButtonItem viewToolBox;
		private System.ComponentModel.IContainer components;
		private NETXP.Controls.Bars.CommandBarButtonItem viewResetLayout;
		private	RichTextBoxEx outputWindow;
		private NETXP.Controls.Bars.ListBar toolBox;
		private NETXP.Controls.Bars.CommandBarButtonItem viewOutputWindow;
		private NETXP.Controls.Bars.CommandBarButtonItem viewPropertiesWindow;
		private System.Windows.Forms.Panel dockingPanel;
		private NETXP.Controls.Bars.CommandBarButtonItem viewProjectFiles;
		private System.Windows.Forms.ImageList treeImageList;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem5;
		private System.Windows.Forms.PropertyGrid propertyGrid;
		private System.Windows.Forms.Panel propertiesWindow;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarBuild;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarProgram;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarSimulate;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarOpen;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarSave;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarSaveAll;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarCut;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarPaste;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarCopy;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarRedo;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarUndo;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarSearch;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarReplace;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarQuickSearch;
		private NETXP.Controls.Bars.CommandBar buildToolbar;
		private NETXP.Controls.Bars.CommandBarDock toolbarHolder;
		private NETXP.Controls.Bars.CommandBar menuBar;
		private NETXP.Controls.Bars.CommandBarButtonItem viewToolbars;
		private NETXP.Controls.Bars.CommandBarButtonItem viewFileToolbar;
		private NETXP.Controls.Bars.CommandBarButtonItem viewBuildToolbar;
		private NETXP.Controls.Bars.CommandBar fileToolbar;
		private NETXP.Controls.Docking.TabbedGroups tabbedGroups;
		private QWEditor.Parser parser;
		private System.Windows.Forms.ContextMenu tabContextMenu;
		private NETXP.Components.Extenders.MenuImageExtender contextMenuImageExtender;
		private NETXP.Controls.Bars.CommandBarButtonItem editRedo;
		private NETXP.Controls.Bars.CommandBarButtonItem editUndo;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem6;
		private NETXP.Controls.Bars.CommandBarButtonItem editCut;
		private NETXP.Controls.Bars.CommandBarButtonItem editCopy;
		private NETXP.Controls.Bars.CommandBarButtonItem editPaste;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem7;
		private NETXP.Controls.Bars.CommandBarButtonItem editFind;
		private NETXP.Controls.Bars.CommandBarButtonItem editReplace;
		private NETXP.Controls.Bars.CommandBarButtonItem editGotoLine;
		private NETXP.Controls.Bars.CommandBarButtonItem editSelectAll;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem9;
		private NETXP.Controls.Docking.TabGroupNode tabNode;
		private NETXP.Controls.Bars.CommandBarButtonItem filePageSetup;
		private NETXP.Controls.Bars.CommandBarButtonItem filePrint;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem8;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem10;
		private NETXP.Controls.Bars.CommandBarButtonItem fileExit;
		private NETXP.Controls.Bars.CommandBarButtonItem fileNew;
		private NETXP.Controls.Bars.CommandBarButtonItem fileOpen;
		private NETXP.Controls.Bars.CommandBarButtonItem fileClose;
		private NETXP.Controls.Bars.CommandBarButtonItem fileNewProject;
		private NETXP.Controls.Bars.CommandBarButtonItem fileNewFile;
		private NETXP.Controls.Bars.CommandBarButtonItem fileOpenProject;
		private NETXP.Controls.Bars.CommandBarButtonItem fileOpenFile;
		private System.Windows.Forms.OpenFileDialog openDialog;
		private System.Windows.Forms.SaveFileDialog saveDialog;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarOpenFile;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarOpenProject;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarNew;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarNewProject;
		private NETXP.Controls.Bars.CommandBarButtonItem toolBarNewFile;
		private NETXP.Controls.Bars.CommandBarButtonItem fileSave;
		private NETXP.Controls.Bars.CommandBarButtonItem fileSaveAll;
		private NETXP.Controls.Bars.CommandBarButtonItem fileSaveAs;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem11;
		private NETXP.Controls.Bars.CommandBarButtonItem fileRecentFiles;
		private NETXP.Controls.Bars.CommandBarButtonItem fileRecentProjects;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem12;
		private System.Windows.Forms.ComboBox propertiesComboBox;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarSaveFile;
		private NETXP.Controls.Bars.CommandBarButtonItem toolBarSaveProject;
		private NETXP.Controls.Bars.CommandBarButtonItem projectMenu;
		private NETXP.Controls.Bars.CommandBarButtonItem projectAddNewFile;
		private NETXP.Controls.Bars.CommandBarButtonItem projectAddExistingFile;
		private NETXP.Controls.Bars.CommandBarButtonItem projectAddNewFolder;
		private NETXP.Controls.Bars.CommandBarButtonItem projectCloseProject;
		private NETXP.Controls.Bars.CommandBarButtonItem projectOptions;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem13;
		private NETXP.Controls.Bars.CommandBarButtonItem toolsEditorOptions;
		private NETXP.Controls.Bars.CommandBarButtonItem projectBuild;
		private NETXP.Controls.Bars.CommandBarButtonItem projectProgram;
		private NETXP.Controls.Bars.CommandBarButtonItem projectSimulate;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem14;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem15;
		private System.Windows.Forms.MenuItem tabContextMenuUndo;
		private System.Windows.Forms.MenuItem tabContextMenuRedo;
		private System.Windows.Forms.MenuItem tabContextMenuCut;
		private System.Windows.Forms.MenuItem tabContextMenuCopy;
		private System.Windows.Forms.MenuItem tabContextMenuPaste;
		private System.Windows.Forms.MenuItem tabContextMenuGoto;
		private System.Windows.Forms.MenuItem tabContextMenuSetup;
		private System.Windows.Forms.MenuItem tabContextMenuPrint;
		private System.Windows.Forms.MenuItem menuSeperator1;
		private System.Windows.Forms.MenuItem menuSeperator2;
		private System.Windows.Forms.MenuItem menuSeperator3;
		private System.Windows.Forms.ToolTip toolBoxToolTip;
		private NETXP.Controls.XPButton errorStatusBarPanel;
		private NETXP.Controls.XPButton warningStatusBarPanel;
		private ErrorListView errorList;
		private bool showErrors;
		private bool showWarnings;
		private ArrayList tempErrorList;
		#endregion

		#region Project and File Variables
	
	    private QWEditor.SyntaxSettings globalSyntaxSettings;
		private string appDirectory;
		private NETXP.Controls.TreeViewEx projectFilesTree;
		private NETXP.Controls.Bars.CommandBarButtonItem projectSaveProject;
		private NETXP.Controls.Bars.CommandBarButtonItem viewErrorList;
		private System.Windows.Forms.ComboBox toolbarQuickCombo;
		private System.Windows.Forms.StatusBarPanel lncolStatusBarPanel;
		private System.Windows.Forms.StatusBarPanel statusBarButtonsHolder;
		private System.Windows.Forms.ContextMenu outputWindowContextMenu;
		private System.Windows.Forms.MenuItem outputWindowClearMenuItem;
		private System.Windows.Forms.MenuItem outputWindowCopyMenuItem;
		private System.Windows.Forms.MenuItem menuItem1;
		private NETXP.Controls.Bars.CommandBarLabelItem quickSearchComboHolder;
		private NETXP.Controls.Bars.ProgressPanel statusBarProgressPanel;
		private NETXP.Controls.Bars.CommandBarButtonItem aboutMegaIDE;
		private NETXP.Controls.Bars.CommandBarSeparatorItem commandBarSeparatorItem16;
		private NETXP.Controls.Bars.CommandBarButtonItem toolsResetEditorSettings;
		private NETXP.Controls.Bars.CommandBarButtonItem toolsAutoComplete;
		private NETXP.Controls.Bars.CommandBarButtonItem toolbarReset;
		private bool isProjectOpen;

		#endregion 
		
		#region Properties

		/// <summary>
		/// Sets the errors count in the status bar. Reviewed . To be changed after next version.
		/// </summary>
		public int Errors
		{
			set
			{
				if(value == 1)
				{
					errorStatusBarPanel.Text = "1 Error";
					errorStatusBarPanel.Size = new Size(70,21);
					warningStatusBarPanel.Location = new Point(76,2);
					statusBarButtonsHolder.Width = 168;
				}
				else
				{
					string errorsNum = value.ToString();
					errorStatusBarPanel.Size = new Size(75+(errorsNum.Length-1)*5,21);
					warningStatusBarPanel.Location = new Point(errorStatusBarPanel.Size.Width+6,2);
					statusBarButtonsHolder.Width = errorStatusBarPanel.Size.Width+warningStatusBarPanel.Width+6;
					errorStatusBarPanel.Text = value.ToString()+" Errors";
				}
			}
			get{return int.Parse(errorStatusBarPanel.Text.Split(' ')[0]);} 
		}

		/// <summary>
		/// Sets the warnings count in the status bar. Reviewed . To be changed after next version.
		/// </summary>
		public int Warnings
		{
			set
			{
				if(value == 1)
				{
					warningStatusBarPanel.Text = "1 Warning";
					warningStatusBarPanel.Size = new Size(87,21);
					statusBarButtonsHolder.Width = errorStatusBarPanel.Size.Width+93;
				}
				else
				{	
					string warNum = value.ToString();
					warningStatusBarPanel.Size = new Size(92+(warNum.Length-1)*5,21);	
					warningStatusBarPanel.Text = value.ToString()+" Warnings";
					statusBarButtonsHolder.Width = errorStatusBarPanel.Size.Width+warningStatusBarPanel.Width+6;
				}
			}
			get{return int.Parse(warningStatusBarPanel.Text.Split(' ')[0]);} 
		}
		
		/// <summary>
		/// Gets or Sets the 
		/// </summary>
		public int PercentDone
		{
			get{return statusBarProgressPanel.Value;}
			set{statusBarProgressPanel.Value = value;}
		}

		/// <summary>
		/// Gets the application directory path.
		/// </summary>
		public string AppDirectory
		{
			get{return appDirectory;}
		}	

		/// <summary>
		/// Gets the treeview control associated with project tree.
		/// </summary>
		public NETXP.Controls.TreeViewEx ProjectFilesTree
		{
			get { return projectFilesTree;}
		}
		
		/// <summary>
		/// Gets the error list listview control.
		/// </summary>
		public ErrorListView ErrorList
		{
			get{return errorList;}
		}
		
		/// <summary>
		/// Gets the output window RichTextBox.
		/// </summary>
		public RichTextBoxEx OutputWindow
		{
			get{return outputWindow;}
		}

		/// <summary>
		/// Gets or Sets the isProject open bool variable.
		/// </summary>
		public bool IsProjectOpen
		{
			get{return isProjectOpen;}
			set
			{
				if(value == false)
				{
					foreach(CommandBarItem toolbarItem in buildToolbar.Items)
						toolbarItem.Enabled=false;				
					projectMenu.Visible = false;
				}
				else
				{
					projectMenu.Visible = true;
					foreach(CommandBarItem toolbarItem in buildToolbar.Items)
						toolbarItem.Enabled=true;	
				}
				isProjectOpen = value;
			}
		}
		
		/// <summary>
		/// Gets or Sets the property to display errors in Error List.
		/// </summary>
		public bool ShowErrors
		{
			 get{return showErrors;}
		}
		
		/// <summary>
		/// Gets the property to display warnings in Error List.
		/// </summary>
		public bool ShowWarnings
		{
			get{return showWarnings;}	
		}
		
		/// <summary>
		/// Stores the temporary objects of the errorList.
		/// </summary>
		public ArrayList TempErrorList
		{
			get{return tempErrorList;}
		}

		#endregion

		#region File Operations
		
		/// <summary>
		/// Applies the syntax settings to all the open pages.
		/// </summary>
		private void ApplySyntaxSettings()
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{
				foreach(NETXP.Controls.Docking.TabPage page in tabNode.TabPages)
				{
					globalSyntaxSettings.ApplyToEdit(page.Control as SyntaxEdit);
				  
				}
			}
		}

		/// <summary>
		/// Moves the cursor to particular line in specified file. Reviewed . To be changed after next version.
		/// </summary>
		/// <param name="fileName">
		/// Path of the file.
		/// </param>
		/// <param name="lineNumber">
		/// Line Number to go to.
		/// </param>
		public void GotoFileLine(string fileName,int lineNumber)
		{		
			if(!System.IO.File.Exists(fileName))
			{
				MessageBox.Show(fileName+" does not exist!","MegaIDE",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
				RecentDocuments.RemoveFromRecentFiles(fileName);
				UpdateRecentDocumentsList();
				return;
			}

			SyntaxEdit syntaxEdit;

			// Search in already opened files.
			if(tabbedGroups.RootSequence.Count != 0)
			{  
				foreach(NETXP.Controls.Docking.TabPage selectedPage in tabNode.TabPages)
				{
					if(selectedPage.ToolTipText.Equals(fileName))
					{
						tabNode.TabControl.SelectedTab=selectedPage;
						syntaxEdit=(selectedPage.Control as SyntaxEdit);
						syntaxEdit.Source.MoveToLine(lineNumber-1);
						syntaxEdit.LineSeparator.Options=QWEditor.SeparatorOptions.HighlightCurrentLine; 
						syntaxEdit.MouseDown+=new MouseEventHandler(SourceTabPage_MouseDown);	
						syntaxEdit.Refresh();
						return;
					}
				}
			}
			
			// Create a new tab if file does not exist.
			syntaxEdit=new SyntaxEdit();

			// Loading syntax highlighter is the file is a source file.
			if(Path.GetExtension(fileName).ToLower()==".c" || Path.GetExtension(fileName).ToLower()==".h")
				syntaxEdit.Lexer=parser;

			syntaxEdit.SourceStateChanged+=new NotifyEvent(syntaxEdit_SourceStateChanged);
			
			// Code completion event handler declaration.
			if(ProjectManagerClass.ProjectManager.GetFileNode(fileName) != null)
				syntaxEdit.NeedCodeCompletion+=new CodeCompletionEvent(syntaxEdit_NeedCodeCompletion);
        
			syntaxEdit.CodeCompletionHint.Images=treeImageList;
			syntaxEdit.CodeCompletionBox.Images=treeImageList;
			syntaxEdit.Selection.Options=QWEditor.SelectionOptions.UseColors;
			syntaxEdit.Gutter.Options=QWEditor.GutterOptions.PaintLinesOnGutter;
			syntaxEdit.Selection.InActiveBackColor=System.Drawing.Color.Yellow;
			syntaxEdit.Selection.InActiveForeColor=System.Drawing.Color.Black;
			syntaxEdit.Anchor=((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)| System.Windows.Forms.AnchorStyles.Left)| System.Windows.Forms.AnchorStyles.Right)));
			globalSyntaxSettings.ApplyToEdit(syntaxEdit);
			syntaxEdit.Scrolling.UpdateScroll();
			syntaxEdit.ShowScrollHint(10);
			syntaxEdit.ContextMenu=tabContextMenu;	
			syntaxEdit.Source.FileName=fileName;
			syntaxEdit.Source.LoadFile(fileName);

			// Creating new tab page
			NETXP.Controls.Docking.TabPage newTabPage=new NETXP.Controls.Docking.TabPage(Path.GetFileName(fileName));
			newTabPage.ToolTipText=fileName;
			newTabPage.Control=syntaxEdit;
			newTabPage.Tag=false; // false means file not modified
			
			// Check for readonly file. To be reviewed and removed with next QWEditor version.
			if((((byte)System.IO.File.GetAttributes(fileName))&7)>0)
			{
				syntaxEdit.Source.ReadOnly=true;
				newTabPage.Title+="(Read Only)";
			}

			if(tabbedGroups.RootSequence.Count==0)
			{ 
				tabNode=tabbedGroups.RootSequence.AddNewNode(); 
				tabNode.TabControl.SelectionChanged+=new EventHandler(TabControl_SelectionChanged);	
			}
   
			tabNode.TabControl.SelectedIndex=tabNode.TabPages.Add(newTabPage);		   			
			
			// Update recent files list.
			RecentDocuments.AddToRecentFiles(fileName);
			UpdateRecentDocumentsList();
			
			// Move to specified line.
			syntaxEdit.Source.MoveToLine(lineNumber-1);
			syntaxEdit.LineSeparator.Options=QWEditor.SeparatorOptions.HighlightCurrentLine; 
			syntaxEdit.MouseDown+=new MouseEventHandler(SourceTabPage_MouseDown);
			syntaxEdit.Refresh();	
		}
		
		/// <summary>
		/// Creates a new tab displaying the text content of the specified file.
		/// </summary>
		/// <param name="fileName">
		/// Path of the file.
		/// </param>
		public void OpenFile(string fileName)
		{
			//Check for file existence
			if(!System.IO.File.Exists(fileName))
			{
				MessageBox.Show(fileName+" does not exist!","MegaIDE",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
				RecentDocuments.RemoveFromRecentFiles(fileName);
				UpdateRecentDocumentsList();
				return;
			}

			SyntaxEdit syntaxEdit;

			//Check if already open
			if(tabbedGroups.RootSequence.Count != 0)
			{ 
				foreach(NETXP.Controls.Docking.TabPage selectedPage in tabNode.TabPages)
				{
					if(selectedPage.ToolTipText.ToLower().Equals(fileName.ToLower()))
					{
						tabNode.TabControl.SelectedTab=selectedPage;
						syntaxEdit=(selectedPage.Control as SyntaxEdit);
						syntaxEdit.Refresh();
						/*if(!(bool)selectedPage.Tag)
						{
							syntaxEdit=(selectedPage.Control as SyntaxEdit);
							syntaxEdit.Source.LoadFile(fileName);
							syntaxEdit.Refresh();	
						}*/
						return;
					}
				}
			}
			
			//Create a new tab
			syntaxEdit=new SyntaxEdit();

			// Loading syntax highlighter is the file is a source file.
			if(Path.GetExtension(fileName).ToLower()==".c" || Path.GetExtension(fileName).ToLower()==".h")
				syntaxEdit.Lexer=parser;

			syntaxEdit.SourceStateChanged+=new NotifyEvent(syntaxEdit_SourceStateChanged);
			
			// Code completion event handler declaration.
			if(isProjectOpen && ProjectManagerClass.ProjectManager.GetFileNode(fileName) != null)
				syntaxEdit.NeedCodeCompletion+=new CodeCompletionEvent(syntaxEdit_NeedCodeCompletion);
		
			// Initialization of syntaxEdit
			syntaxEdit.CodeCompletionHint.Images=treeImageList;
			syntaxEdit.CodeCompletionBox.Images=treeImageList;
			syntaxEdit.Selection.Options=QWEditor.SelectionOptions.UseColors;
			syntaxEdit.Gutter.Options=QWEditor.GutterOptions.PaintLinesOnGutter;
			syntaxEdit.Selection.InActiveBackColor=System.Drawing.Color.Yellow;
			syntaxEdit.Selection.InActiveForeColor=System.Drawing.Color.Black;
			syntaxEdit.Anchor=((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)| System.Windows.Forms.AnchorStyles.Left)| System.Windows.Forms.AnchorStyles.Right)));
			globalSyntaxSettings.ApplyToEdit(syntaxEdit);
			syntaxEdit.Scrolling.UpdateScroll();
			syntaxEdit.ShowScrollHint(10);
			syntaxEdit.ContextMenu=tabContextMenu;	
			syntaxEdit.Source.FileName=fileName;
			syntaxEdit.Source.LoadFile(fileName);
			
			// Creating new tab page
			NETXP.Controls.Docking.TabPage newTabPage=new NETXP.Controls.Docking.TabPage(Path.GetFileName(fileName));
			newTabPage.ToolTipText=fileName;
			newTabPage.Control=syntaxEdit;
			newTabPage.Tag=false; // false means file not modified

			// Check for readonly file. To be reviewed and removed with next QWEditor version.
			if((((byte)System.IO.File.GetAttributes(fileName))&7)>0)
			{
				syntaxEdit.Source.ReadOnly=true;
				newTabPage.Title+="(Read Only)";
			}
			
			if(tabbedGroups.RootSequence.Count==0)
			{ 
				tabNode=tabbedGroups.RootSequence.AddNewNode(); 
				tabNode.TabControl.SelectionChanged+=new EventHandler(TabControl_SelectionChanged);	
			}
   
			tabNode.TabControl.SelectedIndex=tabNode.TabPages.Add(newTabPage);		   			
			
			// Update recent files list.
			RecentDocuments.AddToRecentFiles(fileName);
			UpdateRecentDocumentsList();
		}
		
		/// <summary>
		/// Closes an open tab for a particular file.
		/// </summary>
		/// <param name="fileName">
		/// Full path of the file.
		/// </param>
		public void CloseOpenTab(string fileName)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{
				NETXP.Controls.Docking.TabPageCollection tabPageCollection=new TabPageCollection();
				foreach(NETXP.Controls.Docking.TabPage selectedTabPage in tabNode.TabPages)
					if(selectedTabPage.ToolTipText.ToLower().Equals(fileName.ToLower()))
					{
						tabNode.TabPages.Remove(selectedTabPage);
						break;
					}
			}
		}
		
		/// <summary>
		/// Closes open tabs with files starting with a particular path(e.g folder).
		/// </summary>
		/// <param name="path">
		/// Path of the folder.
		/// </param>
		public void CloseOpenTabs(string path)
		{ 
			if(tabbedGroups.RootSequence.Count != 0)
			{
				NETXP.Controls.Docking.TabPageCollection tabPageCollection=new TabPageCollection();
				foreach(NETXP.Controls.Docking.TabPage selectedTabPage in tabNode.TabPages)
				if(selectedTabPage.ToolTipText.ToLower().StartsWith(path.ToLower()))
				{
					tabPageCollection.Add(selectedTabPage);
				}
				foreach(NETXP.Controls.Docking.TabPage tabPage in tabPageCollection)
				{
					tabNode.TabPages.Remove(tabPage);
				}
			}
		}

		/// <summary>
		/// Saves all the project files if the project is open.
		/// </summary>
		public void SaveProjectFiles()
		{
			if(tabbedGroups.RootSequence.Count != 0 && isProjectOpen)
			{
				foreach(NETXP.Controls.Docking.TabPage selectedTabPage in tabNode.TabPages)
				{	
					if(ProjectManagerClass.ProjectManager.GetFileNode(selectedTabPage.ToolTipText) != null)
					{				
						SaveFile(selectedTabPage);
					}
				}
			}
		}

		/// <summary>
		/// Closes all the tabs which have project files opened.
		/// </summary>
		public void CloseProjectFiles()
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{
				NETXP.Controls.Docking.TabPageCollection tabPageCollection=new TabPageCollection();
				foreach(NETXP.Controls.Docking.TabPage selectedTabPage in tabNode.TabPages)
				{
					if(ProjectManagerClass.ProjectManager.GetFileNode(selectedTabPage.ToolTipText) != null)
					{
						tabPageCollection.Add(selectedTabPage);
					}
				}
				foreach(NETXP.Controls.Docking.TabPage selectedTabPage in tabPageCollection)
				{
					tabNode.TabPages.Remove(selectedTabPage);
				}
			}
		}

		/// <summary>
		/// Gets for the tabs a list of unsaved project files.
		/// </summary>
		/// <returns>
		/// Arraylist containing the path of the unsaved files.
		/// </returns>
		public ArrayList GetUnsavedProjectFiles()
		{
			ArrayList filesList=new ArrayList(4);
			if(tabbedGroups.RootSequence.Count != 0 && isProjectOpen)
			{
				foreach(NETXP.Controls.Docking.TabPage selectedTabPage in tabNode.TabPages)
				{
					if(ProjectManagerClass.ProjectManager.GetFileNode(selectedTabPage.ToolTipText) != null)
						if((bool)selectedTabPage.Tag)
							filesList.Add(selectedTabPage.ToolTipText);
				}	
			}
			filesList.TrimToSize();
			return filesList;
		}

		/// <summary>
		/// Returns a list of all unsaved files currently opened.
		/// </summary>
		/// <returns>
		/// Arraylist containing the paths of the unsaved files.
		/// </returns>
		public ArrayList GetUnsavedFiles()
		{
			ArrayList filesList=new ArrayList(4);
			if(tabbedGroups.RootSequence.Count != 0)
			{
				foreach(NETXP.Controls.Docking.TabPage selectedTabPage in tabNode.TabPages)
				{
					if((bool)selectedTabPage.Tag)
						filesList.Add(selectedTabPage.ToolTipText);
				}	
			}
			filesList.TrimToSize();
			return filesList;
		}

		/// <summary>
		/// Returns all the currently opened project files.
		/// </summary>
		/// <returns>
		/// Arraylist containing the relative paths of the opened project files.
		/// </returns>
		public ArrayList GetOpenProjectFilesList()
		{
			ArrayList filesList=new ArrayList(4);
			string openedFile="";
			if(tabbedGroups.RootSequence.Count != 0)
			{
				foreach(NETXP.Controls.Docking.TabPage selectedTabPage in tabNode.TabPages)
				{
					TreeNode fileNode=ProjectManagerClass.ProjectManager.GetFileNode(selectedTabPage.ToolTipText);
					if(fileNode != null)
					{
						string relativePath=fileNode.FullPath.Replace(projectFilesTree.Nodes[0].Nodes[0].FullPath+"\\","");
						filesList.Add(relativePath);
						if(tabNode.TabControl.SelectedTab==selectedTabPage)
							openedFile=relativePath;
					}
				}	
			}
			if(filesList.Count != 0 && openedFile.Length != 0)
				filesList.Add(openedFile);
			filesList.TrimToSize();
			return filesList;
		}

		/// <summary>
		/// Opens multiple files as specified.
		/// </summary>
		/// <param name="filesList">
		/// List of file paths to be opened.
		/// </param>
		public void OpenFiles(ArrayList filesList)
		{
			foreach(string  fileName in filesList)
				OpenFile(fileName);	
		}
		
		/// <summary>
		/// Opens multiple project files as specified.
		/// </summary>
		/// <param name="projectFolder">
		/// Project folder path.
		/// </param>
		/// <param name="filesList">
		/// Relative paths of files in the project.
		/// </param>
		public void OpenProjectFiles(string projectFolder,ArrayList filesList)
		{
			foreach(string  fileName in filesList)
				OpenFile(projectFolder+"\\"+fileName);	
		}
		
		/// <summary>
		/// Renames an open tab .
		/// </summary>
		/// <param name="oldFileName">
		/// Path containing old file name.
		/// </param>
		/// <param name="newFileName">
		/// Path containing the new file name.
		/// </param>
		public void RenameTab(string oldFileName,string newFileName)
		{
			try
				{
					foreach(NETXP.Controls.Docking.TabPage selectedTabPage in tabNode.TabPages)
						if(selectedTabPage.ToolTipText.Equals(oldFileName))
						{
							SyntaxEdit syntaxEdit=selectedTabPage.Control as SyntaxEdit;
							selectedTabPage.Title=Path.GetFileName(newFileName);
							syntaxEdit.Source.FileName=newFileName;
							selectedTabPage.ToolTipText=newFileName;
							if(((bool)selectedTabPage.Tag)==true)
								selectedTabPage.Title+="*";
							tabNode.TabControl.SelectedTab=selectedTabPage;
						    return;
						}
				}
				catch{}	
		}
		
		/// <summary>
		/// Saves an opened file.
		/// </summary>
		/// <param name="fileName">
		/// Path of the file.
		/// </param>
		public void SaveFile(string fileName)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{
				foreach(NETXP.Controls.Docking.TabPage selectedTabPage in tabNode.TabPages)
				{
					if(selectedTabPage.ToolTipText.ToLower()==fileName.ToLower()) 
					{
						SaveFile(selectedTabPage);
						return;
					}
				}
			}
		}
		
		/// <summary>
		/// Shows the Save File As dialog and saves the file.
		/// </summary>
		/// <param name="fileName">
		/// Path of the file.
		///	</param>
		public void SaveFileAs(string fileName)
		{
			string saveFilter="";
			string fileExtension=Path.GetExtension(fileName);
			if(fileExtension.Equals(".c"))
				saveFilter="C Files (*.c)|*.c|";
			else if(fileExtension.Equals(".h"))
				saveFilter="Header Files (*.h)|*.h|";
			else if(fileExtension.Equals(".txt"))
				saveFilter="Text Files (*.txt)|*.txt|";	
			saveDialog.Filter=saveFilter+"All Files (*.*)|*.*";
			saveDialog.Title="Save "+Path.GetFileName(fileName)+" As";
			if(saveDialog.ShowDialog()==DialogResult.OK)
			{
				if(System.IO.File.Exists(saveDialog.FileName))
					if((((byte)System.IO.File.GetAttributes(saveDialog.FileName))&7)>0)
					{
						MessageBox.Show("Cannot overwrite a system , hidden or read-only file !","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Hand); 
						return;
					}
				try
				{
					File.Copy(fileName,saveDialog.FileName);
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
				}
			}
		}
		
		/// <summary>
		/// Saves the file opened by a specified tab page.
		/// </summary>
		/// <param name="selectedTabPage">
		/// Tabpage selected.
		/// </param>
		private void SaveFile(NETXP.Controls.Docking.TabPage selectedTabPage)
		{
			try
			{
				if((bool)selectedTabPage.Tag)
				{
					SyntaxEdit syntaxEdit=selectedTabPage.Control as SyntaxEdit;
					if(syntaxEdit.Source.ReadOnly)
					{
						MessageBox.Show("Cannot overwrite a system , hidden or read-only file !","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Hand); 
						return;
					}
					
					syntaxEdit.Source.SaveFile(syntaxEdit.Source.FileName);  
					selectedTabPage.Title=Path.GetFileName(syntaxEdit.Source.FileName);
					selectedTabPage.Tag=false;
				}
			}
			catch{}
		}
		
		/// <summary>
		/// Shows the Save file As dialog before saving the file opened by the specified tab page.
		/// </summary>
		/// <param name="selectedTabPage">
		/// Tabpage selected.
		/// </param>
		private void SaveFileAs(NETXP.Controls.Docking.TabPage selectedTabPage)
		{
			if(tabbedGroups.RootSequence.Count != 0)
				{
				SyntaxEdit syntaxEdit=selectedTabPage.Control as SyntaxEdit;
				string currentFileName=syntaxEdit.Source.FileName;
				string saveFilter="";
				string fileExtension=Path.GetExtension(syntaxEdit.Source.FileName);
				if(fileExtension.Equals(".c"))
					saveFilter="C Files (*.c)|*.c|";
				else if(fileExtension.Equals(".h"))
					saveFilter="Header Files (*.h)|*.h|";
				else if(fileExtension.Equals(".txt"))
					saveFilter="Text Files (*.txt)|*.txt|";	
				saveDialog.Filter=saveFilter+"All Files (*.*)|*.*";
				saveDialog.Title="Save "+Path.GetFileName(syntaxEdit.Source.FileName)+" As";
				if(saveDialog.ShowDialog()==DialogResult.OK)
				{
					if(System.IO.File.Exists(saveDialog.FileName))
						if((((byte)System.IO.File.GetAttributes(saveDialog.FileName))&7)>0)
						{
							MessageBox.Show("Cannot overwrite a system , hidden or read-only file !","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Hand); 
							return;
						}
					if(syntaxEdit.Source.FileName != saveDialog.FileName)
					{
						RecentDocuments.AddToRecentFiles(saveDialog.FileName);
						UpdateRecentDocumentsList();
					}
					syntaxEdit.Source.SaveFile(saveDialog.FileName);
					selectedTabPage.Title=Path.GetFileName(saveDialog.FileName);
					syntaxEdit.Source.FileName=saveDialog.FileName;
					selectedTabPage.ToolTipText=saveDialog.FileName;
					selectedTabPage.Tag=false;
				}
			}		
		}	
			
		#endregion
		
		#region Form Functions
		
		/// <summary>
		/// Constructor without parameters.
		/// </summary>
		private MainFormClass()
		{
			//
			// Required for Windows Form Designer support
			//
			WaitCallback callbackFunc = new WaitCallback(LoadSettings);
			InitializeComponent();

			FormLoad();
			
			//Loader thread
			ThreadPool.QueueUserWorkItem(callbackFunc);
			//new MethodInvoker(LoadSettings).BeginInvoke(null,null);
			
	
			Height=System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
			Width=System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;

			// Initialization of the singleton variable
			MainFormClass.MainFormInitializer=this;
		}

		/// <summary>
		/// Constructor with parameters
		/// </summary>
		/// <param name="fileName">
		/// File to open
		/// </param>
		private MainFormClass(string fileName)
		{
			//
			// Required for Windows Form Designer support
			//
			WaitCallback callbackFunc = new WaitCallback(LoadSettings);
			InitializeComponent();
			
			FormLoad();
			//LoadSettings(new object());
			
			Height=System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
			Width=System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
			MainFormClass.MainFormInitializer=this;
		 
			ThreadPool.QueueUserWorkItem(callbackFunc,fileName);
			//new MethodInvoker(LoadSettings).BeginInvoke(null,null);
			
			//Opening the command line filepath.
			/*if(Path.GetExtension(fileName).ToLower()==".mbp")
			{	
				ProjectManagerClass.ProjectManager.Refresh();
				ProjectManagerClass.ProjectManager.OpenProject(fileName);
			}
			else
			{
				OpenFile(fileName);
			}*/

		}
		//TODO:Review
		/// <summary>
		/// Load the settings from the saved files.
		/// </summary>
		private void LoadSettings(object pathString)
		{
			RecentDocuments.LoadListFromRegistry();
			UpdateRecentDocumentsList();
		
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(GetType());
			
			// parser
			// 
			this.parser.Strings = null;
			this.parser.ReadSchemeFromResource(typeof(MainFormClass), "parser.XmlScheme");
			
				
			// Syntax Settings
			globalSyntaxSettings=new QWEditor.SyntaxSettings();
			if(System.IO.File.Exists(appDirectory+@"\Syntax.xml"))
			{
				globalSyntaxSettings.LoadFile(appDirectory+@"\Syntax.xml");
			}
			else
			{
				globalSyntaxSettings.AllowOutlining=false;
				globalSyntaxSettings.ShowMargin=false;
				globalSyntaxSettings.ScrollBars=RichTextBoxScrollBars.Both;
				globalSyntaxSettings.WordWrap=false;
				globalSyntaxSettings.GutterWidth=20;
			}

			if(pathString!=null)
			{
				string fileName = pathString as string;
				//Opening the command line filepath.
				if(Path.GetExtension(fileName).ToLower()==".mbp")
				{	
					Invoke(new MethodInvoker(ProjectManagerClass.ProjectManager.Refresh));
					Invoke(new StringDelegate(ProjectManagerClass.ProjectManager.OpenProject),new object[]{fileName});
					Invoke(new MethodInvoker(BringProjectTreeToFront));
				}
				else
				{
					Invoke(new StringDelegate(OpenFile),new object[]{fileName});
				}
			}
			System.Threading.Thread.Sleep(200);
			Invoke(new MethodInvoker(MaximizeForm));
		}
		
		private void MaximizeForm()
		{
			WindowState=FormWindowState.Maximized;
		}
		

		/// <summary>
		/// Loads dock objects, initializes the project tree and loads some other variables.
		/// </summary>
		private void FormLoad()
		{	
			//Current Directory 
			appDirectory=Application.StartupPath;
            
			IsProjectOpen=false;
			showErrors=true;
			showWarnings=true;
			
			//

			// Tree
			projectFilesTree=new NETXP.Controls.TreeViewEx();
			projectFilesTree.ImageIndex = -1;
			projectFilesTree.Location = new System.Drawing.Point(0,0);
			projectFilesTree.Size=new Size(175,320);
			projectFilesTree.Dock=System.Windows.Forms.DockStyle.Left;
			projectFilesTree.Name = "projectFilesTree";
			projectFilesTree.SelectedImageIndex = -1;
			projectFilesTree.TabIndex = 4;
			projectFilesTree.ImageList=treeImageList;
			projectFilesTree.HideSelection=false;
			projectFilesTree.MouseDown+=new MouseEventHandler(projectFilesTree_MouseDown);
			projectFilesTree.AfterExpand+=new TreeViewEventHandler(projectFilesTree_AfterExpand);
			projectFilesTree.AfterCollapse+=new TreeViewEventHandler(projectFilesTree_AfterCollapse);
			
			// TempErrorList
			tempErrorList=new ArrayList(20);

			// ErrorList
			errorList = new ErrorListView();
			errorList.Dock = System.Windows.Forms.DockStyle.Bottom;
			errorList.FullRowSelect = true;
			errorList.GridLines = true;
			errorList.Location = new System.Drawing.Point(0,320);
			errorList.Name = "errorList";
			errorList.Size = new System.Drawing.Size(792, 170);
			errorList.TabIndex = 8;
			errorList.View = System.Windows.Forms.View.Details;
			errorList.SmallImageList = treeImageList;
			errorList.ItemActivate+=new EventHandler(errorList_ItemActivate);
					
			ColumnHeader errorSymbolColumn = new System.Windows.Forms.ColumnHeader();
			ColumnHeader messageColumn  = new System.Windows.Forms.ColumnHeader();
			ColumnHeader lineNumberColumn  = new System.Windows.Forms.ColumnHeader();
			ColumnHeader fileNameColumn  = new System.Windows.Forms.ColumnHeader();
			
			errorSymbolColumn.Text=" ! ";
			errorSymbolColumn.Width=25;

			messageColumn.Text="Message";
			messageColumn.Width=200;
			messageColumn.TextAlign=HorizontalAlignment.Left;

			lineNumberColumn.Text="Line";
			lineNumberColumn.Width=70;
			lineNumberColumn.TextAlign=HorizontalAlignment.Center;

			fileNameColumn.Text="File";
			fileNameColumn.Width=400;
			fileNameColumn.TextAlign=HorizontalAlignment.Left;


			errorList.Columns.AddRange(new ColumnHeader[] {errorSymbolColumn,messageColumn,lineNumberColumn,fileNameColumn});
			

			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(GetType());
			
			
			// OutputWindow
			dockingManagerExtender.SetAllowDocking(outputWindow, true);
			dockingManagerExtender.SetFullTitle(outputWindow, "Output");
			dockingManagerExtender.SetIcon(outputWindow, ((System.Drawing.Image)(resources.GetObject("viewOutputWindow.Image"))));
			dockingManagerExtender.SetTitle(outputWindow, "Output");
			dockingManagerExtender.SetCloseOnHide(outputWindow,false);             
			dockingManagerExtender.SetTabbedMode(outputWindow,true);

			// ErrorList
			dockingManagerExtender.SetAllowDocking(errorList, true);
			dockingManagerExtender.SetFullTitle(errorList, "Error List");
			dockingManagerExtender.SetIcon(errorList, ((System.Drawing.Image)(resources.GetObject("viewErrorList.Image"))));
			dockingManagerExtender.SetTitle(errorList, "Error List");
			dockingManagerExtender.SetCloseOnHide(errorList,false);             
			dockingManagerExtender.SetTabbedMode(errorList,true);

			

			// ToolBox
			dockingManagerExtender.SetAllowDocking(toolBox, true);
			dockingManagerExtender.SetFullTitle(toolBox, "Toolbox");
			dockingManagerExtender.SetIcon(toolBox, ((System.Drawing.Image)(resources.GetObject("viewToolBox.Image"))));
			dockingManagerExtender.SetTitle(toolBox, "Toolbox");
			dockingManagerExtender.SetCloseOnHide(toolBox,false);
			dockingManagerExtender.SetTabbedMode(toolBox,true);

			//Tree
			dockingManagerExtender.SetAllowDocking(projectFilesTree, true);
			dockingManagerExtender.SetFullTitle(projectFilesTree, "Project Files");
			dockingManagerExtender.SetIcon(projectFilesTree, ((System.Drawing.Image)(resources.GetObject("viewProjectFiles.Image"))));
			dockingManagerExtender.SetTitle(projectFilesTree, "Project Files");
			dockingManagerExtender.SetCloseOnHide(projectFilesTree,false);
			dockingManagerExtender.SetTabbedMode(projectFilesTree,true);

			// Property Window
			dockingManagerExtender.SetAllowDocking(propertiesWindow, true);
			dockingManagerExtender.SetFullTitle(propertiesWindow, "Properties");
			dockingManagerExtender.SetIcon(propertiesWindow, ((System.Drawing.Image)(resources.GetObject("viewPropertiesWindow.Image"))));
			dockingManagerExtender.SetTitle(propertiesWindow, "Properties");
			dockingManagerExtender.SetCloseOnHide(propertiesWindow,false);
			dockingManagerExtender.SetTabbedMode(propertiesWindow,true);		
		}
			
		/// <summary>
		/// Resets the dock layout,the toolbar and global syntax settings.
		/// </summary>	
		private void ResetLayout()
		{  
			dockingManagerExtender.DockingManager.DockObjects["Output"].AutoHidden=false;
			dockingManagerExtender.DockingManager.DockObjects["Output"].DisplaySize=new System.Drawing.Size(792,170);
		    NETXP.Controls.Docking.WindowObjects outputWindowObject=dockingManagerExtender.DockingManager.AddDockObjectWithState(dockingManagerExtender.DockingManager.DockObjects["Output"],NETXP.Controls.Docking.State.DockBottom);	
			
			dockingManagerExtender.DockingManager.DockObjects["Error List"].AutoHidden=false;
			dockingManagerExtender.DockingManager.DockObjects["Error List"].DisplaySize=new System.Drawing.Size(792,170);
			dockingManagerExtender.DockingManager.AddDockObjectToWindowObjects(dockingManagerExtender.DockingManager.DockObjects["Error List"],outputWindowObject);
	
		    dockingManagerExtender.DockingManager.DockObjects["Toolbox"].AutoHidden=false;
			dockingManagerExtender.DockingManager.DockObjects["Toolbox"].DisplaySize=new System.Drawing.Size(175, 320);
			NETXP.Controls.Docking.WindowObjects toolboxWindowObject=dockingManagerExtender.DockingManager.AddDockObjectWithState(dockingManagerExtender.DockingManager.DockObjects["Toolbox"],NETXP.Controls.Docking.State.DockLeft);	

			dockingManagerExtender.DockingManager.DockObjects["Project Files"].HideButton=true;
			dockingManagerExtender.DockingManager.DockObjects["Project Files"].AutoHidden=false;
			dockingManagerExtender.DockingManager.DockObjects["Project Files"].DisplaySize=new System.Drawing.Size(175, 320);
			dockingManagerExtender.DockingManager.AddDockObjectToWindowObjects(dockingManagerExtender.DockingManager.DockObjects["Project Files"],toolboxWindowObject);

			dockingManagerExtender.DockingManager.DockObjects["Properties"].AutoHidden=false;
			dockingManagerExtender.DockingManager.DockObjects["Properties"].DisplaySize=new System.Drawing.Size(150, 320);
			dockingManagerExtender.DockingManager.AddDockObjectWithState(dockingManagerExtender.DockingManager.DockObjects["Properties"],NETXP.Controls.Docking.State.DockRight);	

            menuBar.RowNumber=0;
			menuBar.Position=0;

			fileToolbar.ShowBar();
			fileToolbar.RowNumber=1;
			fileToolbar.Position=0;

			buildToolbar.ShowBar();
			buildToolbar.RowNumber=1;
			fileToolbar.Position=1;

			toolbarQuickCombo.Items.Clear();
			toolbarQuickCombo.Text="";   
		}
		
		/// <summary>
		/// Resets the editor settings.
		/// </summary>
		private void ResetEditorSettings()
		{
			globalSyntaxSettings=new QWEditor.SyntaxSettings();
			globalSyntaxSettings.AllowOutlining=false;
			globalSyntaxSettings.ShowMargin=false;
			globalSyntaxSettings.ScrollBars=RichTextBoxScrollBars.Both;
			globalSyntaxSettings.WordWrap=false;
			globalSyntaxSettings.GutterWidth=20;
			ApplySyntaxSettings();
		}
		/// <summary>
		/// Updates Recent Documents List
		/// </summary>
		public void UpdateRecentDocumentsList()
		{
			fileRecentProjects.Items.Clear();
			fileRecentFiles.Items.Clear();
			for(int i=0;i<RecentDocuments.RecentFilesList.Count;i++)
			{
				string fileName=((string)RecentDocuments.RecentFilesList[i]);
				NETXP.Controls.Bars.CommandBarButtonItem newItem= new NETXP.Controls.Bars.CommandBarButtonItem();
				System.Text.StringBuilder rebuiltString=new System.Text.StringBuilder("&"+(i+1).ToString()+" ");
				if(fileName.Length<50)
					rebuiltString.Append(fileName);
				else
				{
					int startIndex=fileName.IndexOf('\\',0);
					rebuiltString.Append(fileName.Substring(0,startIndex+1));
					int lastIndex=startIndex;
					for(;lastIndex<fileName.LastIndexOf('\\');lastIndex++)
					{
                      if(fileName[lastIndex]=='\\')
						  if((fileName.Length-(lastIndex-startIndex+4))<50)
								  break;
					}
					if(lastIndex != startIndex)  
						rebuiltString.Append("...\\");
					rebuiltString.Append(fileName.Substring(lastIndex+1));
				}
				newItem.Text=rebuiltString.ToString();
				newItem.Tag=fileName;
				newItem.Click+=new EventHandler(fileRecentFiles_Click);
				fileRecentFiles.Items.Add(newItem);
			}
			for(int i=0;i<RecentDocuments.RecentProjectsList.Count;i++)
			{
				string fileName=((string)RecentDocuments.RecentProjectsList[i]);
				NETXP.Controls.Bars.CommandBarButtonItem newItem= new NETXP.Controls.Bars.CommandBarButtonItem();
				System.Text.StringBuilder rebuiltString=new System.Text.StringBuilder("&"+(i+1).ToString()+" ");
				if(fileName.Length<50)
					rebuiltString.Append(fileName);
				else
				{
					int startIndex=fileName.IndexOf('\\',0);
					rebuiltString.Append(fileName.Substring(0,startIndex+1));
					int lastIndex=startIndex;
					for(;lastIndex<fileName.LastIndexOf('\\');lastIndex++)
					{
						if(fileName[lastIndex]=='\\')
							if((fileName.Length-(lastIndex-startIndex+4))<50)
								break;
					}
					if(lastIndex != startIndex)  
						rebuiltString.Append("...\\");
					rebuiltString.Append(fileName.Substring(lastIndex+1));
				}
				newItem.Text=rebuiltString.ToString();
				newItem.Tag=fileName;
				newItem.Click+=new EventHandler(fileRecentProjects_Click);
				fileRecentProjects.Items.Add(newItem);
			}
		}

		/// <summary>
		/// Clears the libraries in toolbox, property grid and propertiescombobox.
		/// </summary>
		public void ClearLibraries()
		{
			toolBox.Groups.Clear();
			propertiesComboBox.Items.Clear();
			propertyGrid.SelectedObject=null;
		}

		/// <summary>
		/// Populates the toolbox, property grid and propertiescombobox. Reviewed . To be changed after next version. To include C source code parser.
		/// </summary>
		/// <param name="libList">
		/// 
		/// </param>
		public void PopulateLibraries(ArrayList libList)
		{
			toolBox.Groups.Clear();
			propertiesComboBox.Items.Clear();
			for(int index=0;index<libList.Count;index++)
			{
				NETXP.Controls.Bars.ListBarGroup module=new NETXP.Controls.Bars.ListBarGroup(libList[index].GetType().GetProperty("LibraryName").GetValue(libList[index],null).ToString());
				module.View=NETXP.Controls.Bars.ListBarGroupView.TextRight; 
				propertiesComboBox.Items.Insert(index,libList[index].GetType().Name);
				toolBox.Groups.Insert(index,module);
				string [,] functions=(string[,])libList[index].GetType().GetMethod("GetFunctionNames").Invoke(libList[index],new object[]{});
				for(int functionIndex=0;functionIndex<functions.GetLength(0);functionIndex++)
				{
					NETXP.Controls.Bars.ListBarItem function=new NETXP.Controls.Bars.ListBarItem();
					module.Items.Add(function);
					function.Image=treeImageList.Images[8];
					function.Caption=functions[functionIndex,0];
					function.Tag=functions[functionIndex,1];
					function.ToolTipText=functions[functionIndex,2];
				}				
			}
			if(propertiesComboBox.Items.Count != 0)
				propertiesComboBox.SelectedIndex=0;
			
			// Adding code completion event handlers to already open tabs.
			if(tabbedGroups.RootSequence.Count != 0)
			{
				foreach(NETXP.Controls.Docking.TabPage selectedTabPage in tabNode.TabPages)
				{
					if(ProjectManagerClass.ProjectManager.GetFileNode(selectedTabPage.ToolTipText) != null)
					{
						SyntaxEdit syntaxEdit=((SyntaxEdit)selectedTabPage.Control);
                        syntaxEdit.NeedCodeCompletion+=new CodeCompletionEvent(syntaxEdit_NeedCodeCompletion);
					}
				}	
			}
 
		}
		
		/// <summary>
		/// Brings project tree dock to front. Causing exceptions right now bcoz the dock loader
		/// on a different thread.
		/// </summary>
		public void BringProjectTreeToFront()
		{
			if(isProjectOpen)
			{
				dockingManagerExtender.DockingManager.DockObjects["Project Files"].BringToFront();
			}
		}

		#endregion

		#region File Menu Event Handlers
		
		private void fileNewFile_Click(object sender, System.EventArgs e)
		{
			if(NewDocumentDialogClass.NewDocumentDialog.ShowNewFileDialog(this)==DialogResult.OK)
				OpenFile(NewDocumentDialogClass.NewDocumentDialog.FullFileName); 
		}

		private void fileNewProject_Click(object sender, System.EventArgs e)
		{
			ProjectManagerClass.ProjectManager.NewProject();
		}

		private void fileClose_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{
				if((bool)tabNode.TabControl.SelectedTab.Tag)
				{
					DialogResult response=MessageBox.Show("Save Changes to "+Path.GetFileName((tabNode.TabControl.SelectedTab.Control as SyntaxEdit).Source.FileName)+" ?","MegaIDE",System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question);
					if(response==DialogResult.Cancel)
						return;
					else if(response==DialogResult.Yes)
						SaveFile(tabNode.TabControl.SelectedTab);
				}
				tabNode.TabControl.SelectedTab.Dispose();
				tabNode.TabPages.Remove(tabNode.TabControl.SelectedTab);
			}	
		}
		
		private void fileOpenFile_Click(object sender, System.EventArgs e)
		{
			openDialog.Title="Open File";
			openDialog.Multiselect=false;
			openDialog.Filter="Source Files (*.c;*.h)|*.c;*.h|Text Files (*.txt)|*.txt";
			if(openDialog.ShowDialog()==System.Windows.Forms.DialogResult.OK)
				OpenFile(openDialog.FileName);
		}
		
		/// <summary>
		/// Event handler for open project from file menu. Public bcoz also accessed from ProjectManager Class.
		/// </summary>
		/// <param name="sender">
		/// Sender object.
		/// </param>
		/// <param name="e">
		/// Event arguments.
		/// </param>
		public void fileOpenProject_Click(object sender, System.EventArgs e)
		{
			openDialog.Title="Open Project";
			openDialog.Multiselect=false;
			openDialog.Filter="Project Files (*.mbp)|*.mbp";
			if(openDialog.ShowDialog()==System.Windows.Forms.DialogResult.OK)
				ProjectManagerClass.ProjectManager.OpenProject(openDialog.FileName);
		}	

		private void fileSave_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
				SaveFile(tabNode.TabControl.SelectedTab);
		}

		private void fileSaveAs_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
				SaveFileAs(tabNode.TabControl.SelectedTab);
		}

		private void fileSaveAll_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{
				foreach(NETXP.Controls.Docking.TabPage tabPage in tabNode.TabPages)
				{
					SaveFile(tabPage);
				}
			}
			if(isProjectOpen)
				ProjectManagerClass.ProjectManager.SaveProject();
		}
		
		private void fileRecentFiles_Click(object sender, System.EventArgs e)
		{
			NETXP.Controls.Bars.CommandBarButtonItem item=(NETXP.Controls.Bars.CommandBarButtonItem)sender;
			OpenFile((string)item.Tag);
		}
		
		private void fileRecentProjects_Click(object sender, System.EventArgs e)
		{
			NETXP.Controls.Bars.CommandBarButtonItem item=(NETXP.Controls.Bars.CommandBarButtonItem)sender;
			string fileName=(string)item.Tag;
			if(!File.Exists(fileName))
			{
				MessageBox.Show(fileName+" does not exist!","MegaIDE",System.Windows.Forms.MessageBoxButtons.OK,System.Windows.Forms.MessageBoxIcon.Warning);
				RecentDocuments.RemoveFromRecentProjects(fileName);
				UpdateRecentDocumentsList();
				return;
			}
			else
				ProjectManagerClass.ProjectManager.OpenProject(fileName);
		}

		private void fileExit_Click(object sender, System.EventArgs e)
		{
			Close();
			Application.Exit();
		}

		#endregion

		#region View Menu and Toolbars Hide Event Handlers

		private void viewToolBox_Click(object sender, System.EventArgs e)
		{
			dockingManagerExtender.DockingManager.ShowDockObject(dockingManagerExtender.DockingManager.DockObjects["Toolbox"]);
			dockingManagerExtender.DockingManager.DockObjects["Toolbox"].BringToFront(); 
		}

		private void viewOutputWindow_Click(object sender, System.EventArgs e)
		{
			dockingManagerExtender.DockingManager.ShowDockObject(dockingManagerExtender.DockingManager.DockObjects["Output"]);
			dockingManagerExtender.DockingManager.DockObjects["Output"].BringToFront();
		}

		private void viewErrorList_Click(object sender, System.EventArgs e)
		{
			dockingManagerExtender.DockingManager.ShowDockObject(dockingManagerExtender.DockingManager.DockObjects["Error List"]);
			dockingManagerExtender.DockingManager.DockObjects["Error List"].BringToFront();
		}

		private void viewPropertiesWindow_Click(object sender, System.EventArgs e)
		{
			dockingManagerExtender.DockingManager.ShowDockObject(dockingManagerExtender.DockingManager.DockObjects["Properties"]);
			dockingManagerExtender.DockingManager.DockObjects["Properties"].BringToFront();
		}

		private void viewProjectFiles_Click(object sender, System.EventArgs e)
		{
			dockingManagerExtender.DockingManager.ShowDockObject(dockingManagerExtender.DockingManager.DockObjects["Project Files"]);
			dockingManagerExtender.DockingManager.DockObjects["Project Files"].BringToFront();
			projectFilesTree.Refresh();
		}
		private void viewResetLayout_Click(object sender, System.EventArgs e)
		{
			if(MessageBox.Show("Are you sure you want to reset the layout ?","MegaIDE",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
				ResetLayout();
		}

		private void viewFileToolbar_Click(object sender, System.EventArgs e)
		{
			fileToolbar.ShowBar();
		}

		private void viewBuildToolbar_Click(object sender, System.EventArgs e)
		{
			buildToolbar.ShowBar();
		}

		private void fileToolbar_Customize(object sender, System.EventArgs e)
		{
			fileToolbar.HideBar();
		}

		private void buildToolbar_Customize(object sender, System.EventArgs e)
		{
			buildToolbar.HideBar();
		}

		// TODO: Review
		private void aboutMegaIDE_Click(object sender, System.EventArgs e)
		{
			AboutDialogClass.AboutDialog.ShowDialog(this);		
			System.GC.Collect();
		}

		#endregion

		#region Tab Context Menu Event Handlers (Edit Menu, Print event and Search button handlers)
		
		private void TabCut_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count!=0)
			{ 
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				syntaxEdit.Selection.Cut();
				syntaxEdit.Focus();
			}
		}

		private void TabCopy_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{ 
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				syntaxEdit.Selection.Copy();
				syntaxEdit.Focus();
			}
		}

		private void TabPaste_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{ 
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				syntaxEdit.Selection.Paste();
				syntaxEdit.Focus();
			}
		}

		private void TabRedo_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{ 
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				syntaxEdit.Source.Redo();
				syntaxEdit.Focus();
			}
		}

		private void TabUndo_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{ 
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				syntaxEdit.Source.Undo();	
				syntaxEdit.Focus();
			}
		}

		private void TabFind_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{ 
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				syntaxEdit.SearchDialog.Execute(syntaxEdit,true,false);
			}
		}

		private void TabReplace_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{ 
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				syntaxEdit.SearchDialog.Execute(syntaxEdit,true,true);
			}
		}

		private void TabSelectAll_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{ 
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				syntaxEdit.Selection.SelectAll();
				syntaxEdit.Focus();
			}
		}
		
		private void toolbarQuickCombo_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
				if(e.KeyCode==Keys.Enter)
					TabQuickSearch_Click(null,null);
		}

		private void TabQuickSearch_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{ 
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				syntaxEdit.Find(toolbarQuickCombo.Text,QWEditor.SearchOptions.CaseSensitive);
				toolbarQuickCombo.Focus(); 	
				if(!toolbarQuickCombo.Items.Contains(toolbarQuickCombo.Text))
				{
					if(toolbarQuickCombo.Items.Count==10)
						toolbarQuickCombo.Items.RemoveAt(0);
					toolbarQuickCombo.Items.Add(toolbarQuickCombo.Text);
				}
			}
		}

		private void TabPageSetup_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				try
				{
					syntaxEdit.Printing.PageSetupDialog.ShowDialog();
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				}
			}
		}

		private void TabPrint_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				if(syntaxEdit.Printing.PrintDialog.ShowDialog()==DialogResult.OK)
					syntaxEdit.Printing.Print();
			}
		}

		private void TabGoto_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{ 
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				int line = syntaxEdit.Position.Y;
				if (syntaxEdit.GotoLineDialog.Execute(syntaxEdit, syntaxEdit.Lines.Count, ref line) == DialogResult.OK)
					syntaxEdit.MoveToLine(line);
			}			
		}
		#endregion

		#region Project Menu Event Handlers

		private void projectAddNewFile_Click(object sender, System.EventArgs e)
		{
			if(isProjectOpen)
			{
				if(projectFilesTree.SelectedNode==null)
				{
					projectFilesTree.SelectedNode=projectFilesTree.Nodes[0].Nodes[0];
				}
				ProjectManagerClass.ProjectManager.menuAddNewFile_Click(null,null);
			}
		}
		
		/// <summary>
		/// Event handler for add existing file. Shows dialog box and adds files.
		/// </summary>
		/// <param name="sender">
		/// Sender object.
		/// </param>
		/// <param name="e">
		/// Event argument.
		/// </param>
		public void projectAddExistingFile_Click(object sender, System.EventArgs e)
		{
			if(isProjectOpen)
			{
				openDialog.Title="Add File";
				openDialog.Multiselect=true;
				openDialog.Filter="Source Files (*.c;*.h)|*.c;*.h";   
				if(openDialog.ShowDialog()==DialogResult.OK)
				{
					foreach(string fullFileName in openDialog.FileNames)
					{
						ProjectManagerClass.ProjectManager.AddFileToProject(fullFileName,projectFilesTree.SelectedNode);		
					}
					projectFilesTree.SelectedNode.Expand();
					ProjectManagerClass.ProjectManager.IsProjectModified=true;
				}
			}
		}
		
		private void projectAddNewFolder_Click(object sender, System.EventArgs e)
		{
			if(isProjectOpen)
			{
				if(projectFilesTree.SelectedNode==null || (((MegaIDE.ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag) != MegaIDE.ProjectTreeItemTypes.Directory && ((MegaIDE.ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag) != MegaIDE.ProjectTreeItemTypes.ProjectName))
				{
					projectFilesTree.SelectedNode=projectFilesTree.Nodes[0].Nodes[0];
				}
				ProjectManagerClass.ProjectManager.menuAddNewFolder_Click(null,null);
			}			
		}
		
		private void projectSaveProject_Click(object sender, System.EventArgs e)
		{
			if(isProjectOpen)
				ProjectManagerClass.ProjectManager.SaveProject();
		}

		private void projectCloseProject_Click(object sender, System.EventArgs e)
		{
			if(isProjectOpen)
				ProjectManagerClass.ProjectManager.CloseProject();
		}

		private void projectBuild_Click(object sender, System.EventArgs e)
		{
			if(isProjectOpen)
				ProjectManagerClass.ProjectManager.BuildProject();
		}

		//to be implemented
		private void projectProgram_Click(object sender, System.EventArgs e)
		{
			if(isProjectOpen)
				ProjectManagerClass.ProjectManager.ProgramDevice();
		}

		//to be implemented
		private void projectSimulate_Click(object sender, System.EventArgs e)
		{
			if(isProjectOpen)
				ProjectManagerClass.ProjectManager.SimulateProgram();
		}
		private void projectOptions_Click(object sender, System.EventArgs e)
		{
			if(isProjectOpen)
				ProjectManagerClass.ProjectManager.ShowOptionsDialog();
		}

		#endregion

		#region Syntax Edit and Tab Event Handlers
		
		// Does not work. Reviewed . To be changed after next version.
		private void syntaxEdit_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MessageBox.Show("Help Requested");
		}
		
		/// <summary>
		/// Shows syntax options dialog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolsEditorOptions_Click(object sender, System.EventArgs e)
		{ 
			try
			{ 
				QWEditor.Dialogs.DlgSyntaxSettings Options = new QWEditor.Dialogs.DlgSyntaxSettings();
				Options.SyntaxSettings.Assign(globalSyntaxSettings); 
				Options.Text="Editor Options";
				Options.ShowInTaskbar=false;
				if (Options.ShowDialog() == DialogResult.OK)  
				{ 
					globalSyntaxSettings.Assign(Options.SyntaxSettings);
					ApplySyntaxSettings();		  
				}   
			}
			catch{}
		}
		
		/// <summary>
		/// Resets the editor settings.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void toolsResetEditorSettings_Click(object sender, System.EventArgs e)
		{
			if(MessageBox.Show("Are you sure you want to reset the editor settings ?","MegaIDE",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.Yes)
				ResetEditorSettings();
		}
		
		/// <summary>
		/// Called when text in the currently opened page changes.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void syntaxEdit_SourceStateChanged(object sender, NotifyEventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{ 
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				lncolStatusBarPanel.Text=String.Format("Ln  {0,-8}    Col  {1,-8}",syntaxEdit.Position.Y+1,syntaxEdit.Position.X+1);		
				if(syntaxEdit.Source.Modified && !((bool)tabNode.TabControl.SelectedTab.Tag))
				{
					tabNode.TabControl.SelectedTab.Tag=true;
					tabNode.TabControl.SelectedTab.Title+="*";
				}			
			}	
		}

		private void TabControl_SelectionChanged(object sender, EventArgs e)
		{
			try
			{
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				this.lncolStatusBarPanel.Text=String.Format("Ln  {0,-8}    Col  {1,-8}",syntaxEdit.Position.Y+1,syntaxEdit.Position.X+1);
			}
			catch
			{
				this.lncolStatusBarPanel.Text="";
			}
		}

		private void tabbedGroups_PageCloseRequest(NETXP.Controls.Docking.TabbedGroups tg, NETXP.Controls.Docking.CloseRequestEventArgs e)
		{
			if(this.tabbedGroups.RootSequence.Count != 0)
			{
				if((bool)tabNode.TabControl.SelectedTab.Tag)
				{
					DialogResult response=MessageBox.Show("Save Changes to "+Path.GetFileName(((SyntaxEdit)(tabNode.TabControl.SelectedTab.Control)).Source.FileName)+" ?","MegaIDE",System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question);
					if(response==DialogResult.Yes)
						this.SaveFile(tabNode.TabControl.SelectedTab);
					else if(response==DialogResult.Cancel)
						e.Cancel=true;
				}
			}		
		}		
			
		#endregion

		#region Project Tree and Libraries related Event handlers

		private void projectFilesTree_AfterExpand(object sender, TreeViewEventArgs e)
		{
			if((MegaIDE.ProjectTreeItemTypes)e.Node.Tag==MegaIDE.ProjectTreeItemTypes.Library)
			{
				e.Node.ImageIndex=e.Node.SelectedImageIndex=6;
			}
			else if((MegaIDE.ProjectTreeItemTypes)e.Node.Tag==MegaIDE.ProjectTreeItemTypes.Directory)
			{
				e.Node.ImageIndex=e.Node.SelectedImageIndex=4;
			}
		}
	
		private void projectFilesTree_AfterCollapse(object sender, TreeViewEventArgs e)
		{
			
			if((MegaIDE.ProjectTreeItemTypes)e.Node.Tag==MegaIDE.ProjectTreeItemTypes.Library)
			{
				e.Node.ImageIndex=e.Node.SelectedImageIndex=5;
			}
			else if((MegaIDE.ProjectTreeItemTypes)e.Node.Tag==MegaIDE.ProjectTreeItemTypes.Directory)
			{
				e.Node.ImageIndex=e.Node.SelectedImageIndex=3;
			}
		}

		private void projectFilesTree_MouseDown(object sender, MouseEventArgs ev) 
		{
			if(ev.Button==MouseButtons.Right) 
			{
				TreeNode node = projectFilesTree.GetNodeAt(ev.X, ev.Y);
				if(node != null) 
				{
					projectFilesTree.SelectedNode = node;
					if(((MegaIDE.ProjectTreeItemTypes)node.Tag)==ProjectTreeItemTypes.LibraryItem)
					{
						this.propertiesComboBox.SelectedIndex=node.Index;
					}
				}
			}
			else if(ev.Button==MouseButtons.Left)
			{
				TreeNode node = projectFilesTree.GetNodeAt(ev.X, ev.Y);
				if(node != null) 
				{
					if(((MegaIDE.ProjectTreeItemTypes)node.Tag)==ProjectTreeItemTypes.LibraryItem)
					{
						this.propertiesComboBox.SelectedIndex=node.Index;
					}
				}

			}
		}

		// Reviewed.
		private void propertyGrid_PropertyValueChanged(object s, System.Windows.Forms.PropertyValueChangedEventArgs e)
		{
			propertyGrid.Refresh();
			if(isProjectOpen)
				ProjectManagerClass.ProjectManager.IsProjectModified=true;
		}

		private void propertiesComboBox_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			this.propertyGrid.SelectedObject=ProjectManagerClass.ProjectManager.GetSelectedObject(propertiesComboBox.SelectedIndex);
		}
		
		//  Reviewed . To be changed after next version.
		private void toolBox_GroupClicked(object sender, NETXP.Controls.Bars.GroupClickedEventArgs e)
		{
			this.propertyGrid.SelectedObject=ProjectManagerClass.ProjectManager.GetSelectedObject(toolBox.Groups.IndexOf(e.Group));
			this.propertiesComboBox.SelectedIndex=toolBox.Groups.IndexOf(e.Group);
		}
		
		//  Reviewed . To be changed after next version.
		private void toolBox_ItemClicked(object sender, NETXP.Controls.Bars.ItemClickedEventArgs e)
		{
			try
			{
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				syntaxEdit.Source.NewLine();
				syntaxEdit.Source.Insert(" "+e.Item.Caption+"(");
				syntaxEdit.Focus();
				syntaxEdit.ParameterInfo();
			}
			catch{}
		}
		
		#endregion

		#region Misc and Incomplete Event Handlers

		/// <summary>
		/// Checks for unsaved files. Shows the exit dialog.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			try
			{
				DialogResult result=ExitDialogClass.ExitDialog.ShowSaveDialog(this,this.GetUnsavedFiles());
				if(result==DialogResult.Cancel)
				{
					e.Cancel=true;
					return;
				}
				else if(result==DialogResult.Yes)
				{
					foreach(ExitDialogClass.ListItem selectedItem in ExitDialogClass.ExitDialog.SelectedItems)
					{
						if(selectedItem.SaveFileType==ExitDialogClass.SaveFileTypes.File)
							this.SaveFile(selectedItem.FileName);
						else if(selectedItem.SaveFileType==ExitDialogClass.SaveFileTypes.ProjectFile)
							ProjectManagerClass.ProjectManager.SaveProject();
					}
				}
				RecentDocuments.SaveListToRegistry();
				globalSyntaxSettings.SaveFile(appDirectory+@"\Syntax.xml");
			}
			catch{}	
		}
		
		/// <summary>
		/// Memory conservation trick.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_Load(object sender, System.EventArgs e)
		{
			try
			{
				NETXP.Win32.API.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle,-1,-1);
			}
			catch{}
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(MainFormClass));
			this.commandBarManager = new NETXP.Controls.Bars.CommandBarManager();
			this.commandBarDockBottom = new NETXP.Controls.Bars.CommandBarDock();
			this.commandBarDockLeft = new NETXP.Controls.Bars.CommandBarDock();
			this.commandBarDockRight = new NETXP.Controls.Bars.CommandBarDock();
			this.commandBarDockTop = new NETXP.Controls.Bars.CommandBarDock();
			this.toolbarHolder = new NETXP.Controls.Bars.CommandBarDock();
			this.fileToolbar = new NETXP.Controls.Bars.CommandBar();
			this.toolbarQuickCombo = new System.Windows.Forms.ComboBox();
			this.toolbarNew = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarNewProject = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolBarNewFile = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarOpen = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarOpenProject = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarOpenFile = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarSave = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarSaveFile = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolBarSaveProject = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarSaveAll = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem1 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.toolbarCut = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarCopy = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarPaste = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem2 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.toolbarRedo = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarUndo = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem3 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.toolbarSearch = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarReplace = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem4 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.quickSearchComboHolder = new NETXP.Controls.Bars.CommandBarLabelItem();
			this.toolbarQuickSearch = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.menuBar = new NETXP.Controls.Bars.CommandBar();
			this.fileMenu = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.fileNew = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.fileNewProject = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.fileNewFile = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.fileOpen = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.fileOpenProject = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.fileOpenFile = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.fileClose = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem8 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.fileSave = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.fileSaveAs = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.fileSaveAll = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem11 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.filePageSetup = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.filePrint = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem10 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.fileRecentFiles = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.fileRecentProjects = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem12 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.fileExit = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.editMenu = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.editRedo = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.editUndo = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem6 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.editCut = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.editCopy = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.editPaste = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem9 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.editSelectAll = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.editGotoLine = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem7 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.editFind = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.editReplace = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.viewMenu = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.viewToolBox = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.viewOutputWindow = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.viewErrorList = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.viewPropertiesWindow = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.viewProjectFiles = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.viewToolbars = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.viewFileToolbar = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.viewBuildToolbar = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem5 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.viewResetLayout = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.projectMenu = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.projectAddNewFile = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.projectAddExistingFile = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.projectAddNewFolder = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem13 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.projectSaveProject = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.projectCloseProject = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem14 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.projectBuild = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.projectProgram = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.projectSimulate = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem15 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.projectOptions = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.optionsMenu = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolsEditorOptions = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolsAutoComplete = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.commandBarSeparatorItem16 = new NETXP.Controls.Bars.CommandBarSeparatorItem();
			this.toolsResetEditorSettings = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.aboutMenu = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.aboutMegaIDE = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.buildToolbar = new NETXP.Controls.Bars.CommandBar();
			this.toolbarBuild = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarProgram = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarSimulate = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.toolbarReset = new NETXP.Controls.Bars.CommandBarButtonItem();
			this.statusBar = new NETXP.Controls.Bars.StatusBar();
			this.errorStatusBarPanel = new NETXP.Controls.XPButton();
			this.warningStatusBarPanel = new NETXP.Controls.XPButton();
			this.statusBarButtonsHolder = new System.Windows.Forms.StatusBarPanel();
			this.lncolStatusBarPanel = new System.Windows.Forms.StatusBarPanel();
			this.statusBarProgressPanel = new NETXP.Controls.Bars.ProgressPanel();
			this.dockingPanel = new System.Windows.Forms.Panel();
			this.tabbedGroups = new NETXP.Controls.Docking.TabbedGroups();
			this.propertiesWindow = new System.Windows.Forms.Panel();
			this.propertiesComboBox = new System.Windows.Forms.ComboBox();
			this.propertyGrid = new System.Windows.Forms.PropertyGrid();
			this.toolBox = new NETXP.Controls.Bars.ListBar();
			this.toolBoxToolTip = new System.Windows.Forms.ToolTip(this.components);
			this.outputWindow = new MegaIDE.RichTextBoxEx();
			this.outputWindowContextMenu = new System.Windows.Forms.ContextMenu();
			this.outputWindowClearMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.outputWindowCopyMenuItem = new System.Windows.Forms.MenuItem();
			this.dockingManagerExtender = new NETXP.Controls.Docking.DockingManagerExtender(this.components);
			this.treeImageList = new System.Windows.Forms.ImageList(this.components);
			this.parser = new QWEditor.Parser();
			this.tabContextMenu = new System.Windows.Forms.ContextMenu();
			this.tabContextMenuUndo = new System.Windows.Forms.MenuItem();
			this.tabContextMenuRedo = new System.Windows.Forms.MenuItem();
			this.menuSeperator1 = new System.Windows.Forms.MenuItem();
			this.tabContextMenuCut = new System.Windows.Forms.MenuItem();
			this.tabContextMenuCopy = new System.Windows.Forms.MenuItem();
			this.tabContextMenuPaste = new System.Windows.Forms.MenuItem();
			this.menuSeperator2 = new System.Windows.Forms.MenuItem();
			this.tabContextMenuGoto = new System.Windows.Forms.MenuItem();
			this.menuSeperator3 = new System.Windows.Forms.MenuItem();
			this.tabContextMenuSetup = new System.Windows.Forms.MenuItem();
			this.tabContextMenuPrint = new System.Windows.Forms.MenuItem();
			this.contextMenuImageExtender = new NETXP.Components.Extenders.MenuImageExtender(this.components);
			this.openDialog = new System.Windows.Forms.OpenFileDialog();
			this.saveDialog = new System.Windows.Forms.SaveFileDialog();
			this.toolbarHolder.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fileToolbar)).BeginInit();
			this.fileToolbar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.menuBar)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.buildToolbar)).BeginInit();
			this.statusBar.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.statusBarButtonsHolder)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.lncolStatusBarPanel)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarProgressPanel)).BeginInit();
			this.dockingPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.tabbedGroups)).BeginInit();
			this.propertiesWindow.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.toolBox)).BeginInit();
			this.SuspendLayout();
			// 
			// commandBarManager
			// 
			this.commandBarManager.BottomDock = this.commandBarDockBottom;
			this.commandBarManager.Form = this;
			this.commandBarManager.LeftDock = this.commandBarDockLeft;
			this.commandBarManager.RightDock = this.commandBarDockRight;
			this.commandBarManager.TopDock = this.commandBarDockTop;
			// 
			// commandBarDockBottom
			// 
			this.commandBarDockBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.dockingManagerExtender.SetFullTitle(this.commandBarDockBottom, "commandBarDockBottom");
			this.commandBarDockBottom.Location = new System.Drawing.Point(0, 566);
			this.commandBarDockBottom.Manager = this.commandBarManager;
			this.commandBarDockBottom.Name = "commandBarDockBottom";
			this.commandBarDockBottom.Size = new System.Drawing.Size(792, 0);
			this.commandBarDockBottom.TabIndex = 3;
			this.commandBarDockBottom.Text = "CommandBarDock";
			this.dockingManagerExtender.SetTitle(this.commandBarDockBottom, "commandBarDockBottom");
			// 
			// commandBarDockLeft
			// 
			this.commandBarDockLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.dockingManagerExtender.SetFullTitle(this.commandBarDockLeft, "commandBarDockLeft");
			this.commandBarDockLeft.Location = new System.Drawing.Point(0, 0);
			this.commandBarDockLeft.Manager = this.commandBarManager;
			this.commandBarDockLeft.Name = "commandBarDockLeft";
			this.commandBarDockLeft.Size = new System.Drawing.Size(0, 566);
			this.commandBarDockLeft.TabIndex = 0;
			this.commandBarDockLeft.Text = "CommandBarDock";
			this.dockingManagerExtender.SetTitle(this.commandBarDockLeft, "commandBarDockLeft");
			// 
			// commandBarDockRight
			// 
			this.commandBarDockRight.Dock = System.Windows.Forms.DockStyle.Right;
			this.dockingManagerExtender.SetFullTitle(this.commandBarDockRight, "commandBarDockRight");
			this.commandBarDockRight.Location = new System.Drawing.Point(792, 0);
			this.commandBarDockRight.Manager = this.commandBarManager;
			this.commandBarDockRight.Name = "commandBarDockRight";
			this.commandBarDockRight.Size = new System.Drawing.Size(0, 566);
			this.commandBarDockRight.TabIndex = 1;
			this.commandBarDockRight.Text = "CommandBarDock";
			this.dockingManagerExtender.SetTitle(this.commandBarDockRight, "commandBarDockRight");
			// 
			// commandBarDockTop
			// 
			this.dockingManagerExtender.SetFullTitle(this.commandBarDockTop, "commandBarDockTop");
			this.commandBarDockTop.Location = new System.Drawing.Point(0, 0);
			this.commandBarDockTop.Manager = this.commandBarManager;
			this.commandBarDockTop.Name = "commandBarDockTop";
			this.commandBarDockTop.Size = new System.Drawing.Size(792, 0);
			this.commandBarDockTop.TabIndex = 2;
			this.commandBarDockTop.Text = "CommandBarDock";
			this.dockingManagerExtender.SetTitle(this.commandBarDockTop, "commandBarDockTop");
			// 
			// toolbarHolder
			// 
			this.toolbarHolder.Controls.Add(this.fileToolbar);
			this.toolbarHolder.Controls.Add(this.menuBar);
			this.toolbarHolder.Controls.Add(this.buildToolbar);
			this.dockingManagerExtender.SetFullTitle(this.toolbarHolder, "toolbarHolder");
			this.toolbarHolder.Location = new System.Drawing.Point(0, 0);
			this.toolbarHolder.Manager = this.commandBarManager;
			this.toolbarHolder.Name = "toolbarHolder";
			this.toolbarHolder.Size = new System.Drawing.Size(792, 50);
			this.toolbarHolder.TabIndex = 3;
			this.toolbarHolder.Text = "CommandBarDock";
			this.dockingManagerExtender.SetTitle(this.toolbarHolder, "toolbarHolder");
			// 
			// fileToolbar
			// 
			this.fileToolbar.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.fileToolbar.BackColor = System.Drawing.Color.Transparent;
			this.fileToolbar.Controls.Add(this.toolbarQuickCombo);
			this.fileToolbar.CustomizeText = "&Hide";
			this.fileToolbar.ID = 1522;
			this.fileToolbar.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																						 this.toolbarNew,
																						 this.toolbarOpen,
																						 this.toolbarSave,
																						 this.toolbarSaveAll,
																						 this.commandBarSeparatorItem1,
																						 this.toolbarCut,
																						 this.toolbarCopy,
																						 this.toolbarPaste,
																						 this.commandBarSeparatorItem2,
																						 this.toolbarRedo,
																						 this.toolbarUndo,
																						 this.commandBarSeparatorItem3,
																						 this.toolbarSearch,
																						 this.toolbarReplace,
																						 this.commandBarSeparatorItem4,
																						 this.quickSearchComboHolder,
																						 this.toolbarQuickSearch});
			this.fileToolbar.Location = new System.Drawing.Point(0, 24);
			this.fileToolbar.Margins.Bottom = 1;
			this.fileToolbar.Margins.Left = 1;
			this.fileToolbar.Margins.Right = 1;
			this.fileToolbar.Margins.Top = 1;
			this.fileToolbar.Name = "fileToolbar";
			this.fileToolbar.RowNumber = 1;
			this.fileToolbar.Size = new System.Drawing.Size(528, 26);
			this.fileToolbar.TabIndex = 1;
			this.fileToolbar.TabStop = false;
			this.fileToolbar.Text = "Command Bar";
			this.fileToolbar.Customize += new System.EventHandler(this.fileToolbar_Customize);
			// 
			// toolbarQuickCombo
			// 
			this.toolbarQuickCombo.Location = new System.Drawing.Point(332, 2);
			this.toolbarQuickCombo.MaxDropDownItems = 10;
			this.toolbarQuickCombo.Name = "toolbarQuickCombo";
			this.toolbarQuickCombo.Size = new System.Drawing.Size(85, 21);
			this.toolbarQuickCombo.TabIndex = 0;
			this.toolbarQuickCombo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.toolbarQuickCombo_KeyDown);
			// 
			// toolbarNew
			// 
			this.toolbarNew.Image = ((System.Drawing.Image)(resources.GetObject("toolbarNew.Image")));
			this.toolbarNew.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																						this.toolbarNewProject,
																						this.toolBarNewFile});
			this.toolbarNew.Text = "&New";
			this.toolbarNew.Click += new System.EventHandler(this.fileNewProject_Click);
			// 
			// toolbarNewProject
			// 
			this.toolbarNewProject.Image = ((System.Drawing.Image)(resources.GetObject("toolbarNewProject.Image")));
			this.toolbarNewProject.Text = "&New Project...";
			this.toolbarNewProject.Click += new System.EventHandler(this.fileNewProject_Click);
			// 
			// toolBarNewFile
			// 
			this.toolBarNewFile.Image = ((System.Drawing.Image)(resources.GetObject("toolBarNewFile.Image")));
			this.toolBarNewFile.Text = "&New File...";
			this.toolBarNewFile.Click += new System.EventHandler(this.fileNewFile_Click);
			// 
			// toolbarOpen
			// 
			this.toolbarOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolbarOpen.Image")));
			this.toolbarOpen.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																						 this.toolbarOpenProject,
																						 this.toolbarOpenFile});
			this.toolbarOpen.Text = "&Open";
			// 
			// toolbarOpenProject
			// 
			this.toolbarOpenProject.Image = ((System.Drawing.Image)(resources.GetObject("toolbarOpenProject.Image")));
			this.toolbarOpenProject.Text = "&Open Project...";
			this.toolbarOpenProject.Click += new System.EventHandler(this.fileOpenProject_Click);
			// 
			// toolbarOpenFile
			// 
			this.toolbarOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("toolbarOpenFile.Image")));
			this.toolbarOpenFile.Text = "&Open File...";
			this.toolbarOpenFile.Click += new System.EventHandler(this.fileOpenFile_Click);
			// 
			// toolbarSave
			// 
			this.toolbarSave.Image = ((System.Drawing.Image)(resources.GetObject("toolbarSave.Image")));
			this.toolbarSave.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																						 this.toolbarSaveFile,
																						 this.toolBarSaveProject});
			this.toolbarSave.Text = "&Save";
			this.toolbarSave.Click += new System.EventHandler(this.fileSave_Click);
			// 
			// toolbarSaveFile
			// 
			this.toolbarSaveFile.Image = ((System.Drawing.Image)(resources.GetObject("toolbarSaveFile.Image")));
			this.toolbarSaveFile.Text = "Save File";
			this.toolbarSaveFile.Click += new System.EventHandler(this.fileSave_Click);
			// 
			// toolBarSaveProject
			// 
			this.toolBarSaveProject.Image = ((System.Drawing.Image)(resources.GetObject("toolBarSaveProject.Image")));
			this.toolBarSaveProject.Text = "Save Project";
			this.toolBarSaveProject.Click += new System.EventHandler(this.projectSaveProject_Click);
			// 
			// toolbarSaveAll
			// 
			this.toolbarSaveAll.Image = ((System.Drawing.Image)(resources.GetObject("toolbarSaveAll.Image")));
			this.toolbarSaveAll.Text = "Save A&ll";
			this.toolbarSaveAll.Click += new System.EventHandler(this.fileSaveAll_Click);
			// 
			// toolbarCut
			// 
			this.toolbarCut.Image = ((System.Drawing.Image)(resources.GetObject("toolbarCut.Image")));
			this.toolbarCut.Text = "&Cut";
			this.toolbarCut.Click += new System.EventHandler(this.TabCut_Click);
			// 
			// toolbarCopy
			// 
			this.toolbarCopy.Image = ((System.Drawing.Image)(resources.GetObject("toolbarCopy.Image")));
			this.toolbarCopy.Text = "&Copy";
			this.toolbarCopy.Click += new System.EventHandler(this.TabCopy_Click);
			// 
			// toolbarPaste
			// 
			this.toolbarPaste.Image = ((System.Drawing.Image)(resources.GetObject("toolbarPaste.Image")));
			this.toolbarPaste.Text = "&Paste";
			this.toolbarPaste.Click += new System.EventHandler(this.TabPaste_Click);
			// 
			// toolbarRedo
			// 
			this.toolbarRedo.Image = ((System.Drawing.Image)(resources.GetObject("toolbarRedo.Image")));
			this.toolbarRedo.Text = "&Redo";
			this.toolbarRedo.Click += new System.EventHandler(this.TabRedo_Click);
			// 
			// toolbarUndo
			// 
			this.toolbarUndo.Image = ((System.Drawing.Image)(resources.GetObject("toolbarUndo.Image")));
			this.toolbarUndo.Text = "&Undo";
			this.toolbarUndo.Click += new System.EventHandler(this.TabUndo_Click);
			// 
			// toolbarSearch
			// 
			this.toolbarSearch.Image = ((System.Drawing.Image)(resources.GetObject("toolbarSearch.Image")));
			this.toolbarSearch.Text = "&Find";
			this.toolbarSearch.Click += new System.EventHandler(this.TabFind_Click);
			// 
			// toolbarReplace
			// 
			this.toolbarReplace.Image = ((System.Drawing.Image)(resources.GetObject("toolbarReplace.Image")));
			this.toolbarReplace.Text = "&Replace";
			this.toolbarReplace.Click += new System.EventHandler(this.TabReplace_Click);
			// 
			// quickSearchComboHolder
			// 
			this.quickSearchComboHolder.Text = "       Search Item        ";
			// 
			// toolbarQuickSearch
			// 
			this.toolbarQuickSearch.Image = ((System.Drawing.Image)(resources.GetObject("toolbarQuickSearch.Image")));
			this.toolbarQuickSearch.ShowText = true;
			this.toolbarQuickSearch.Text = "&Quick Search";
			this.toolbarQuickSearch.Click += new System.EventHandler(this.TabQuickSearch_Click);
			// 
			// menuBar
			// 
			this.menuBar.AllowClose = false;
			this.menuBar.AllowTearOff = false;
			this.menuBar.BackColor = System.Drawing.Color.Transparent;
			this.menuBar.CustomizeText = "&Customize Toolbar...";
			this.menuBar.FullRow = true;
			this.menuBar.ID = 1882;
			this.menuBar.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																					 this.fileMenu,
																					 this.editMenu,
																					 this.viewMenu,
																					 this.projectMenu,
																					 this.optionsMenu,
																					 this.aboutMenu});
			this.menuBar.Location = new System.Drawing.Point(0, 0);
			this.menuBar.Margins.Bottom = 1;
			this.menuBar.Margins.Left = 1;
			this.menuBar.Margins.Right = 1;
			this.menuBar.Margins.Top = 1;
			this.menuBar.Name = "menuBar";
			this.menuBar.Size = new System.Drawing.Size(792, 24);
			this.menuBar.Style = NETXP.Controls.Bars.CommandBarStyle.Menu;
			this.menuBar.TabIndex = 0;
			this.menuBar.TabStop = false;
			this.menuBar.Text = "Command Bar";
			// 
			// fileMenu
			// 
			this.fileMenu.DefaultItem = true;
			this.fileMenu.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																					  this.fileNew,
																					  this.fileOpen,
																					  this.fileClose,
																					  this.commandBarSeparatorItem8,
																					  this.fileSave,
																					  this.fileSaveAs,
																					  this.fileSaveAll,
																					  this.commandBarSeparatorItem11,
																					  this.filePageSetup,
																					  this.filePrint,
																					  this.commandBarSeparatorItem10,
																					  this.fileRecentFiles,
																					  this.fileRecentProjects,
																					  this.commandBarSeparatorItem12,
																					  this.fileExit});
			this.fileMenu.Text = "&File";
			// 
			// fileNew
			// 
			this.fileNew.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																					 this.fileNewProject,
																					 this.fileNewFile});
			this.fileNew.Text = "&New";
			// 
			// fileNewProject
			// 
			this.fileNewProject.Image = ((System.Drawing.Image)(resources.GetObject("fileNewProject.Image")));
			this.fileNewProject.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftN;
			this.fileNewProject.Text = "&Project...";
			this.fileNewProject.Click += new System.EventHandler(this.fileNewProject_Click);
			// 
			// fileNewFile
			// 
			this.fileNewFile.Image = ((System.Drawing.Image)(resources.GetObject("fileNewFile.Image")));
			this.fileNewFile.Shortcut = System.Windows.Forms.Shortcut.CtrlN;
			this.fileNewFile.Text = "&File...";
			this.fileNewFile.Click += new System.EventHandler(this.fileNewFile_Click);
			// 
			// fileOpen
			// 
			this.fileOpen.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																					  this.fileOpenProject,
																					  this.fileOpenFile});
			this.fileOpen.Text = "&Open";
			// 
			// fileOpenProject
			// 
			this.fileOpenProject.Image = ((System.Drawing.Image)(resources.GetObject("fileOpenProject.Image")));
			this.fileOpenProject.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftO;
			this.fileOpenProject.Text = "&Project...";
			this.fileOpenProject.Click += new System.EventHandler(this.fileOpenProject_Click);
			// 
			// fileOpenFile
			// 
			this.fileOpenFile.Image = ((System.Drawing.Image)(resources.GetObject("fileOpenFile.Image")));
			this.fileOpenFile.Shortcut = System.Windows.Forms.Shortcut.CtrlO;
			this.fileOpenFile.Text = "&File...";
			this.fileOpenFile.Click += new System.EventHandler(this.fileOpenFile_Click);
			// 
			// fileClose
			// 
			this.fileClose.Text = "&Close File";
			this.fileClose.Click += new System.EventHandler(this.fileClose_Click);
			// 
			// fileSave
			// 
			this.fileSave.Image = ((System.Drawing.Image)(resources.GetObject("fileSave.Image")));
			this.fileSave.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.fileSave.Text = "&Save File";
			this.fileSave.Click += new System.EventHandler(this.fileSave_Click);
			// 
			// fileSaveAs
			// 
			this.fileSaveAs.Text = "Save File &As...";
			this.fileSaveAs.Click += new System.EventHandler(this.fileSaveAs_Click);
			// 
			// fileSaveAll
			// 
			this.fileSaveAll.Image = ((System.Drawing.Image)(resources.GetObject("fileSaveAll.Image")));
			this.fileSaveAll.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftS;
			this.fileSaveAll.Text = "Save A&ll";
			this.fileSaveAll.Click += new System.EventHandler(this.fileSaveAll_Click);
			// 
			// filePageSetup
			// 
			this.filePageSetup.Image = ((System.Drawing.Image)(resources.GetObject("filePageSetup.Image")));
			this.filePageSetup.Text = "Page Set&up";
			this.filePageSetup.Click += new System.EventHandler(this.TabPageSetup_Click);
			// 
			// filePrint
			// 
			this.filePrint.Image = ((System.Drawing.Image)(resources.GetObject("filePrint.Image")));
			this.filePrint.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
			this.filePrint.Text = "&Print";
			this.filePrint.Click += new System.EventHandler(this.TabPrint_Click);
			// 
			// fileRecentFiles
			// 
			this.fileRecentFiles.Text = "Recent &Files";
			// 
			// fileRecentProjects
			// 
			this.fileRecentProjects.Text = "Recent Pro&jects";
			// 
			// fileExit
			// 
			this.fileExit.Text = "E&xit";
			this.fileExit.Click += new System.EventHandler(this.fileExit_Click);
			// 
			// editMenu
			// 
			this.editMenu.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																					  this.editRedo,
																					  this.editUndo,
																					  this.commandBarSeparatorItem6,
																					  this.editCut,
																					  this.editCopy,
																					  this.editPaste,
																					  this.commandBarSeparatorItem9,
																					  this.editSelectAll,
																					  this.editGotoLine,
																					  this.commandBarSeparatorItem7,
																					  this.editFind,
																					  this.editReplace});
			this.editMenu.Text = "&Edit";
			// 
			// editRedo
			// 
			this.editRedo.Image = ((System.Drawing.Image)(resources.GetObject("editRedo.Image")));
			this.editRedo.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
			this.editRedo.Text = "&Redo";
			this.editRedo.Click += new System.EventHandler(this.TabRedo_Click);
			// 
			// editUndo
			// 
			this.editUndo.Image = ((System.Drawing.Image)(resources.GetObject("editUndo.Image")));
			this.editUndo.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.editUndo.Text = "&Undo";
			this.editUndo.Click += new System.EventHandler(this.TabUndo_Click);
			// 
			// editCut
			// 
			this.editCut.Image = ((System.Drawing.Image)(resources.GetObject("editCut.Image")));
			this.editCut.Text = "&Cut";
			this.editCut.Click += new System.EventHandler(this.TabCut_Click);
			// 
			// editCopy
			// 
			this.editCopy.Image = ((System.Drawing.Image)(resources.GetObject("editCopy.Image")));
			this.editCopy.Text = "&Copy";
			this.editCopy.Click += new System.EventHandler(this.TabCopy_Click);
			// 
			// editPaste
			// 
			this.editPaste.Image = ((System.Drawing.Image)(resources.GetObject("editPaste.Image")));
			this.editPaste.Text = "&Paste";
			this.editPaste.Click += new System.EventHandler(this.TabPaste_Click);
			// 
			// editSelectAll
			// 
			this.editSelectAll.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.editSelectAll.Text = "&Select All";
			this.editSelectAll.Click += new System.EventHandler(this.TabSelectAll_Click);
			// 
			// editGotoLine
			// 
			this.editGotoLine.Image = ((System.Drawing.Image)(resources.GetObject("editGotoLine.Image")));
			this.editGotoLine.Text = "&Goto Line";
			this.editGotoLine.Click += new System.EventHandler(this.TabGoto_Click);
			// 
			// editFind
			// 
			this.editFind.Image = ((System.Drawing.Image)(resources.GetObject("editFind.Image")));
			this.editFind.Shortcut = System.Windows.Forms.Shortcut.CtrlF;
			this.editFind.Text = "&Find";
			this.editFind.Click += new System.EventHandler(this.TabFind_Click);
			// 
			// editReplace
			// 
			this.editReplace.Image = ((System.Drawing.Image)(resources.GetObject("editReplace.Image")));
			this.editReplace.Shortcut = System.Windows.Forms.Shortcut.CtrlH;
			this.editReplace.Text = "&Replace";
			this.editReplace.Click += new System.EventHandler(this.TabReplace_Click);
			// 
			// viewMenu
			// 
			this.viewMenu.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																					  this.viewToolBox,
																					  this.viewOutputWindow,
																					  this.viewErrorList,
																					  this.viewPropertiesWindow,
																					  this.viewProjectFiles,
																					  this.viewToolbars,
																					  this.commandBarSeparatorItem5,
																					  this.viewResetLayout});
			this.viewMenu.ShowText = true;
			this.viewMenu.Style = NETXP.Controls.Bars.ButtonItemStyle.DropDown;
			this.viewMenu.Text = "&View";
			// 
			// viewToolBox
			// 
			this.viewToolBox.Image = ((System.Drawing.Image)(resources.GetObject("viewToolBox.Image")));
			this.viewToolBox.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftT;
			this.viewToolBox.Text = "&Toolbox";
			this.viewToolBox.Click += new System.EventHandler(this.viewToolBox_Click);
			// 
			// viewOutputWindow
			// 
			this.viewOutputWindow.Image = ((System.Drawing.Image)(resources.GetObject("viewOutputWindow.Image")));
			this.viewOutputWindow.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftO;
			this.viewOutputWindow.Text = "&Output Window";
			this.viewOutputWindow.Click += new System.EventHandler(this.viewOutputWindow_Click);
			// 
			// viewErrorList
			// 
			this.viewErrorList.Image = ((System.Drawing.Image)(resources.GetObject("viewErrorList.Image")));
			this.viewErrorList.Text = "Error List";
			this.viewErrorList.Click += new System.EventHandler(this.viewErrorList_Click);
			// 
			// viewPropertiesWindow
			// 
			this.viewPropertiesWindow.Image = ((System.Drawing.Image)(resources.GetObject("viewPropertiesWindow.Image")));
			this.viewPropertiesWindow.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftP;
			this.viewPropertiesWindow.Text = "&Properties Window";
			this.viewPropertiesWindow.Click += new System.EventHandler(this.viewPropertiesWindow_Click);
			// 
			// viewProjectFiles
			// 
			this.viewProjectFiles.Image = ((System.Drawing.Image)(resources.GetObject("viewProjectFiles.Image")));
			this.viewProjectFiles.PadVertical = 0;
			this.viewProjectFiles.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftF;
			this.viewProjectFiles.Text = "Project Files";
			this.viewProjectFiles.Click += new System.EventHandler(this.viewProjectFiles_Click);
			// 
			// viewToolbars
			// 
			this.viewToolbars.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																						  this.viewFileToolbar,
																						  this.viewBuildToolbar});
			this.viewToolbars.Text = "Tool&bars";
			// 
			// viewFileToolbar
			// 
			this.viewFileToolbar.Text = "&File and Edit Toolbar";
			this.viewFileToolbar.Click += new System.EventHandler(this.viewFileToolbar_Click);
			// 
			// viewBuildToolbar
			// 
			this.viewBuildToolbar.Text = "&Build Toolbar";
			this.viewBuildToolbar.Click += new System.EventHandler(this.viewBuildToolbar_Click);
			// 
			// viewResetLayout
			// 
			this.viewResetLayout.Image = ((System.Drawing.Image)(resources.GetObject("viewResetLayout.Image")));
			this.viewResetLayout.Shortcut = System.Windows.Forms.Shortcut.CtrlShiftR;
			this.viewResetLayout.Text = "&Reset Layout";
			this.viewResetLayout.Click += new System.EventHandler(this.viewResetLayout_Click);
			// 
			// projectMenu
			// 
			this.projectMenu.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																						 this.projectAddNewFile,
																						 this.projectAddExistingFile,
																						 this.projectAddNewFolder,
																						 this.commandBarSeparatorItem13,
																						 this.projectSaveProject,
																						 this.projectCloseProject,
																						 this.commandBarSeparatorItem14,
																						 this.projectBuild,
																						 this.projectProgram,
																						 this.projectSimulate,
																						 this.commandBarSeparatorItem15,
																						 this.projectOptions});
			this.projectMenu.Text = "&Project";
			// 
			// projectAddNewFile
			// 
			this.projectAddNewFile.Image = ((System.Drawing.Image)(resources.GetObject("projectAddNewFile.Image")));
			this.projectAddNewFile.Text = "Add &New File...";
			this.projectAddNewFile.Click += new System.EventHandler(this.projectAddNewFile_Click);
			// 
			// projectAddExistingFile
			// 
			this.projectAddExistingFile.Image = ((System.Drawing.Image)(resources.GetObject("projectAddExistingFile.Image")));
			this.projectAddExistingFile.Text = "Add &Existing File(s)...";
			this.projectAddExistingFile.Click += new System.EventHandler(this.projectAddExistingFile_Click);
			// 
			// projectAddNewFolder
			// 
			this.projectAddNewFolder.Image = ((System.Drawing.Image)(resources.GetObject("projectAddNewFolder.Image")));
			this.projectAddNewFolder.Text = "Add New &Folder";
			this.projectAddNewFolder.Click += new System.EventHandler(this.projectAddNewFolder_Click);
			// 
			// projectSaveProject
			// 
			this.projectSaveProject.Image = ((System.Drawing.Image)(resources.GetObject("projectSaveProject.Image")));
			this.projectSaveProject.Text = "&Save Project";
			this.projectSaveProject.Click += new System.EventHandler(this.projectSaveProject_Click);
			// 
			// projectCloseProject
			// 
			this.projectCloseProject.Image = ((System.Drawing.Image)(resources.GetObject("projectCloseProject.Image")));
			this.projectCloseProject.Text = "Close Project";
			this.projectCloseProject.Click += new System.EventHandler(this.projectCloseProject_Click);
			// 
			// projectBuild
			// 
			this.projectBuild.Image = ((System.Drawing.Image)(resources.GetObject("projectBuild.Image")));
			this.projectBuild.Text = "&Build Project";
			this.projectBuild.Click += new System.EventHandler(this.projectBuild_Click);
			// 
			// projectProgram
			// 
			this.projectProgram.Image = ((System.Drawing.Image)(resources.GetObject("projectProgram.Image")));
			this.projectProgram.Text = "&Program Device";
			this.projectProgram.Click += new System.EventHandler(this.projectProgram_Click);
			// 
			// projectSimulate
			// 
			this.projectSimulate.Image = ((System.Drawing.Image)(resources.GetObject("projectSimulate.Image")));
			this.projectSimulate.Text = "&Simulate Code";
			this.projectSimulate.Click += new System.EventHandler(this.projectSimulate_Click);
			// 
			// projectOptions
			// 
			this.projectOptions.Image = ((System.Drawing.Image)(resources.GetObject("projectOptions.Image")));
			this.projectOptions.Text = "Project &Options";
			this.projectOptions.Click += new System.EventHandler(this.projectOptions_Click);
			// 
			// optionsMenu
			// 
			this.optionsMenu.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																						 this.toolsEditorOptions,
																						 this.toolsAutoComplete,
																						 this.commandBarSeparatorItem16,
																						 this.toolsResetEditorSettings});
			this.optionsMenu.Text = "&Tools";
			// 
			// toolsEditorOptions
			// 
			this.toolsEditorOptions.Image = ((System.Drawing.Image)(resources.GetObject("toolsEditorOptions.Image")));
			this.toolsEditorOptions.Text = "&Editor Options";
			this.toolsEditorOptions.Click += new System.EventHandler(this.toolsEditorOptions_Click);
			// 
			// toolsAutoComplete
			// 
			this.toolsAutoComplete.Image = ((System.Drawing.Image)(resources.GetObject("toolsAutoComplete.Image")));
			this.toolsAutoComplete.Text = "&Auto Complete";
			this.toolsAutoComplete.Click += new System.EventHandler(this.toolsAutoComplete_Click);
			// 
			// toolsResetEditorSettings
			// 
			this.toolsResetEditorSettings.Image = ((System.Drawing.Image)(resources.GetObject("toolsResetEditorSettings.Image")));
			this.toolsResetEditorSettings.Text = "&Reset Editor Settings";
			this.toolsResetEditorSettings.Click += new System.EventHandler(this.toolsResetEditorSettings_Click);
			// 
			// aboutMenu
			// 
			this.aboutMenu.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																					   this.aboutMegaIDE});
			this.aboutMenu.Text = "&About";
			// 
			// aboutMegaIDE
			// 
			this.aboutMegaIDE.Text = "&About MegaIDE...";
			this.aboutMegaIDE.Click += new System.EventHandler(this.aboutMegaIDE_Click);
			// 
			// buildToolbar
			// 
			this.buildToolbar.BackColor = System.Drawing.Color.Transparent;
			this.buildToolbar.CustomizeText = "&Hide";
			this.buildToolbar.ID = 2925;
			this.buildToolbar.Items.AddRange(new NETXP.Controls.Bars.CommandBarItem[] {
																						  this.toolbarBuild,
																						  this.toolbarSimulate,
																						  this.toolbarProgram,
																						  this.toolbarReset});
			this.buildToolbar.Location = new System.Drawing.Point(528, 24);
			this.buildToolbar.Margins.Bottom = 1;
			this.buildToolbar.Margins.Left = 1;
			this.buildToolbar.Margins.Right = 1;
			this.buildToolbar.Margins.Top = 1;
			this.buildToolbar.Name = "buildToolbar";
			this.buildToolbar.Position = 1;
			this.buildToolbar.RowNumber = 1;
			this.buildToolbar.Size = new System.Drawing.Size(264, 26);
			this.buildToolbar.TabIndex = 2;
			this.buildToolbar.TabStop = false;
			this.buildToolbar.Text = "Command Bar";
			this.buildToolbar.Customize += new System.EventHandler(this.buildToolbar_Customize);
			// 
			// toolbarBuild
			// 
			this.toolbarBuild.Image = ((System.Drawing.Image)(resources.GetObject("toolbarBuild.Image")));
			this.toolbarBuild.ShowText = true;
			this.toolbarBuild.Text = "&Build";
			this.toolbarBuild.Click += new System.EventHandler(this.projectBuild_Click);
			// 
			// toolbarProgram
			// 
			this.toolbarProgram.Image = ((System.Drawing.Image)(resources.GetObject("toolbarProgram.Image")));
			this.toolbarProgram.ShowText = true;
			this.toolbarProgram.Text = "&Program";
			this.toolbarProgram.Click += new System.EventHandler(this.projectProgram_Click);
			// 
			// toolbarSimulate
			// 
			this.toolbarSimulate.Image = ((System.Drawing.Image)(resources.GetObject("toolbarSimulate.Image")));
			this.toolbarSimulate.ShowText = true;
			this.toolbarSimulate.Text = "&Simulate";
			this.toolbarSimulate.Click += new System.EventHandler(this.projectSimulate_Click);
			// 
			// toolbarReset
			// 
			this.toolbarReset.Image = ((System.Drawing.Image)(resources.GetObject("toolbarReset.Image")));
			this.toolbarReset.ShowText = true;
			this.toolbarReset.Text = "&Reset";
			this.toolbarReset.Click += new System.EventHandler(this.projectReset_Click);
			// 
			// statusBar
			// 
			this.statusBar.Controls.Add(this.errorStatusBarPanel);
			this.statusBar.Controls.Add(this.warningStatusBarPanel);
			this.statusBar.Location = new System.Drawing.Point(0, 542);
			this.statusBar.Name = "statusBar";
			this.statusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																						 this.statusBarButtonsHolder,
																						 this.lncolStatusBarPanel,
																						 this.statusBarProgressPanel});
			this.statusBar.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.statusBar.ShowPanels = true;
			this.statusBar.Size = new System.Drawing.Size(792, 24);
			this.statusBar.TabIndex = 6;
			this.statusBar.Text = "statusBar";
			// 
			// errorStatusBarPanel
			// 
			this.errorStatusBarPanel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.errorStatusBarPanel.Image = ((System.Drawing.Image)(resources.GetObject("errorStatusBarPanel.Image")));
			this.errorStatusBarPanel.Location = new System.Drawing.Point(3, 2);
			this.errorStatusBarPanel.Name = "errorStatusBarPanel";
			this.errorStatusBarPanel.Size = new System.Drawing.Size(75, 21);
			this.errorStatusBarPanel.TabIndex = 10;
			this.errorStatusBarPanel.TabStop = false;
			this.errorStatusBarPanel.Text = "0 Errors";
			this.errorStatusBarPanel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolBoxToolTip.SetToolTip(this.errorStatusBarPanel, "Click to show or hide errors in Error List.");
			this.errorStatusBarPanel.UseXPThemes = false;
			this.errorStatusBarPanel.Click += new System.EventHandler(this.errorStatusBarPanel_Click);
			// 
			// warningStatusBarPanel
			// 
			this.warningStatusBarPanel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.warningStatusBarPanel.Image = ((System.Drawing.Image)(resources.GetObject("warningStatusBarPanel.Image")));
			this.warningStatusBarPanel.Location = new System.Drawing.Point(81, 2);
			this.warningStatusBarPanel.Name = "warningStatusBarPanel";
			this.warningStatusBarPanel.Size = new System.Drawing.Size(92, 21);
			this.warningStatusBarPanel.TabIndex = 12;
			this.warningStatusBarPanel.TabStop = false;
			this.warningStatusBarPanel.Text = "0 Warnings";
			this.warningStatusBarPanel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolBoxToolTip.SetToolTip(this.warningStatusBarPanel, "Click to show or hide warnings in Error List.");
			this.warningStatusBarPanel.UseXPThemes = false;
			this.warningStatusBarPanel.Click += new System.EventHandler(this.warningStatusBarPanel_Click);
			// 
			// statusBarButtonsHolder
			// 
			this.statusBarButtonsHolder.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarButtonsHolder.Width = 173;
			// 
			// lncolStatusBarPanel
			// 
			this.lncolStatusBarPanel.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.Raised;
			this.lncolStatusBarPanel.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.lncolStatusBarPanel.Width = 150;
			// 
			// statusBarProgressPanel
			// 
			this.statusBarProgressPanel.BorderStyle = System.Windows.Forms.StatusBarPanelBorderStyle.Raised;
			this.statusBarProgressPanel.Maximum = 100;
			this.statusBarProgressPanel.Minimum = 0;
			this.statusBarProgressPanel.Style = System.Windows.Forms.StatusBarPanelStyle.OwnerDraw;
			this.statusBarProgressPanel.Value = 0;
			// 
			// dockingPanel
			// 
			this.dockingPanel.BackColor = System.Drawing.SystemColors.ControlDark;
			this.dockingPanel.Controls.Add(this.tabbedGroups);
			this.dockingPanel.Controls.Add(this.propertiesWindow);
			this.dockingPanel.Controls.Add(this.toolBox);
			this.dockingPanel.Controls.Add(this.outputWindow);
			this.dockingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dockingManagerExtender.SetFullTitle(this.dockingPanel, "dockingPanel");
			this.dockingPanel.Location = new System.Drawing.Point(0, 50);
			this.dockingPanel.Name = "dockingPanel";
			this.dockingPanel.Size = new System.Drawing.Size(792, 492);
			this.dockingPanel.TabIndex = 7;
			this.dockingManagerExtender.SetTitle(this.dockingPanel, "dockingPanel");
			// 
			// tabbedGroups
			// 
			this.tabbedGroups.AllowDrop = true;
			this.tabbedGroups.AtLeastOneNode = false;
			this.tabbedGroups.BackColor = System.Drawing.SystemColors.ControlDark;
			this.tabbedGroups.DefaultGroupMinimumHeight = 1;
			this.tabbedGroups.DefaultGroupMinimumWidth = 1;
			this.tabbedGroups.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabbedGroups.LayoutLock = true;
			this.tabbedGroups.Location = new System.Drawing.Point(175, 0);
			this.tabbedGroups.MoveNextShortcut = System.Windows.Forms.Shortcut.F2;
			this.tabbedGroups.MovePreviousShortcut = System.Windows.Forms.Shortcut.F3;
			this.tabbedGroups.Name = "tabbedGroups";
			this.tabbedGroups.ProportionalResizeShortcut = System.Windows.Forms.Shortcut.F5;
			this.tabbedGroups.SaveControls = false;
			this.tabbedGroups.SelectedNode = null;
			this.tabbedGroups.SelectedShortcut = System.Windows.Forms.Shortcut.F4;
			this.tabbedGroups.Size = new System.Drawing.Size(467, 342);
			this.tabbedGroups.SplitHorizontalShortcut = System.Windows.Forms.Shortcut.None;
			this.tabbedGroups.SplitVerticalShortcut = System.Windows.Forms.Shortcut.None;
			this.tabbedGroups.TabIndex = 5;
			this.tabbedGroups.PageCloseRequest += new NETXP.Controls.Docking.TabbedGroups.PageCloseRequestHandler(this.tabbedGroups_PageCloseRequest);
			// 
			// propertiesWindow
			// 
			this.propertiesWindow.BackColor = System.Drawing.SystemColors.Control;
			this.propertiesWindow.Controls.Add(this.propertiesComboBox);
			this.propertiesWindow.Controls.Add(this.propertyGrid);
			this.propertiesWindow.Dock = System.Windows.Forms.DockStyle.Right;
			this.propertiesWindow.Location = new System.Drawing.Point(642, 0);
			this.propertiesWindow.Name = "propertiesWindow";
			this.propertiesWindow.Size = new System.Drawing.Size(150, 342);
			this.propertiesWindow.TabIndex = 4;
			// 
			// propertiesComboBox
			// 
			this.propertiesComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.propertiesComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.propertiesComboBox.Location = new System.Drawing.Point(0, 0);
			this.propertiesComboBox.Name = "propertiesComboBox";
			this.propertiesComboBox.Size = new System.Drawing.Size(150, 21);
			this.propertiesComboBox.TabIndex = 4;
			this.propertiesComboBox.SelectedIndexChanged += new System.EventHandler(this.propertiesComboBox_SelectedIndexChanged);
			// 
			// propertyGrid
			// 
			this.propertyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.propertyGrid.CommandsVisibleIfAvailable = true;
			this.propertyGrid.ImeMode = System.Windows.Forms.ImeMode.On;
			this.propertyGrid.LargeButtons = false;
			this.propertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.propertyGrid.Location = new System.Drawing.Point(0, 20);
			this.propertyGrid.Name = "propertyGrid";
			this.propertyGrid.Size = new System.Drawing.Size(150, 322);
			this.propertyGrid.TabIndex = 6;
			this.propertyGrid.Text = "PropertyGrid";
			this.propertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.propertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			this.propertyGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGrid_PropertyValueChanged);
			// 
			// toolBox
			// 
			this.toolBox.AllowDragGroups = false;
			this.toolBox.AllowDrop = true;
			this.toolBox.Cursor = System.Windows.Forms.Cursors.Default;
			this.toolBox.Dock = System.Windows.Forms.DockStyle.Left;
			this.toolBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.toolBox.Location = new System.Drawing.Point(0, 0);
			this.toolBox.Name = "toolBox";
			this.toolBox.Size = new System.Drawing.Size(175, 342);
			this.toolBox.TabIndex = 3;
			this.toolBox.ToolTip = this.toolBoxToolTip;
			this.toolBox.GroupClicked += new NETXP.Controls.Bars.GroupClickedEventHandler(this.toolBox_GroupClicked);
			this.toolBox.ItemDoubleClicked += new NETXP.Controls.Bars.ItemClickedEventHandler(this.toolBox_ItemClicked);
			// 
			// outputWindow
			// 
			this.outputWindow.ContextMenu = this.outputWindowContextMenu;
			this.outputWindow.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.outputWindow.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.outputWindow.Location = new System.Drawing.Point(0, 342);
			this.outputWindow.Name = "outputWindow";
			this.outputWindow.ReadOnly = true;
			this.outputWindow.Size = new System.Drawing.Size(792, 150);
			this.outputWindow.TabIndex = 7;
			this.outputWindow.Text = "";
			this.outputWindow.WordWrap = false;
			this.outputWindow.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.outputWindow_LinkClicked);
			// 
			// outputWindowContextMenu
			// 
			this.outputWindowContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																									this.outputWindowClearMenuItem,
																									this.menuItem1,
																									this.outputWindowCopyMenuItem});
			// 
			// outputWindowClearMenuItem
			// 
			this.contextMenuImageExtender.SetImage(this.outputWindowClearMenuItem, ((System.Drawing.Image)(resources.GetObject("outputWindowClearMenuItem.Image"))));
			this.outputWindowClearMenuItem.Index = 0;
			this.outputWindowClearMenuItem.OwnerDraw = true;
			this.outputWindowClearMenuItem.Text = "Clear All";
			this.outputWindowClearMenuItem.Click += new System.EventHandler(this.outputWindowClearMenuItem_Click);
			// 
			// menuItem1
			// 
			this.contextMenuImageExtender.SetImage(this.menuItem1, null);
			this.menuItem1.Index = 1;
			this.menuItem1.OwnerDraw = true;
			this.menuItem1.Text = "-";
			// 
			// outputWindowCopyMenuItem
			// 
			this.contextMenuImageExtender.SetImage(this.outputWindowCopyMenuItem, ((System.Drawing.Image)(resources.GetObject("outputWindowCopyMenuItem.Image"))));
			this.outputWindowCopyMenuItem.Index = 2;
			this.outputWindowCopyMenuItem.OwnerDraw = true;
			this.outputWindowCopyMenuItem.Text = "Copy";
			this.outputWindowCopyMenuItem.Click += new System.EventHandler(this.outputWindowCopyMenuItem_Click);
			// 
			// dockingManagerExtender
			// 
			this.dockingManagerExtender.AutomaticStatePersistence = true;
			this.dockingManagerExtender.ContainerControl = this;
			// 
			// dockingManagerExtender.DockingManager
			// 
			this.dockingManagerExtender.DockingManager.AutoResize = false;
			this.dockingManagerExtender.DockingManager.DockRegionMinMax = false;
			this.dockingManagerExtender.InnerControl = this.dockingPanel;
			this.dockingManagerExtender.OuterControl = this.dockingPanel;
			// 
			// treeImageList
			// 
			this.treeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.treeImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.treeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImageList.ImageStream")));
			this.treeImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// parser
			// 
			this.parser.Strings = null;
			this.parser.ReadSchemeFromResource(typeof(MainFormClass), "parser.XmlScheme");
			// 
			// tabContextMenu
			// 
			this.tabContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																						   this.tabContextMenuUndo,
																						   this.tabContextMenuRedo,
																						   this.menuSeperator1,
																						   this.tabContextMenuCut,
																						   this.tabContextMenuCopy,
																						   this.tabContextMenuPaste,
																						   this.menuSeperator2,
																						   this.tabContextMenuGoto,
																						   this.menuSeperator3,
																						   this.tabContextMenuSetup,
																						   this.tabContextMenuPrint});
			// 
			// tabContextMenuUndo
			// 
			this.contextMenuImageExtender.SetImage(this.tabContextMenuUndo, ((System.Drawing.Image)(resources.GetObject("tabContextMenuUndo.Image"))));
			this.tabContextMenuUndo.Index = 0;
			this.tabContextMenuUndo.OwnerDraw = true;
			this.tabContextMenuUndo.Text = "Undo";
			this.tabContextMenuUndo.Click += new System.EventHandler(this.TabUndo_Click);
			// 
			// tabContextMenuRedo
			// 
			this.contextMenuImageExtender.SetImage(this.tabContextMenuRedo, ((System.Drawing.Image)(resources.GetObject("tabContextMenuRedo.Image"))));
			this.tabContextMenuRedo.Index = 1;
			this.tabContextMenuRedo.OwnerDraw = true;
			this.tabContextMenuRedo.Text = "Redo";
			this.tabContextMenuRedo.Click += new System.EventHandler(this.TabRedo_Click);
			// 
			// menuSeperator1
			// 
			this.contextMenuImageExtender.SetImage(this.menuSeperator1, null);
			this.menuSeperator1.Index = 2;
			this.menuSeperator1.OwnerDraw = true;
			this.menuSeperator1.Text = "-";
			// 
			// tabContextMenuCut
			// 
			this.contextMenuImageExtender.SetImage(this.tabContextMenuCut, ((System.Drawing.Image)(resources.GetObject("tabContextMenuCut.Image"))));
			this.tabContextMenuCut.Index = 3;
			this.tabContextMenuCut.OwnerDraw = true;
			this.tabContextMenuCut.Text = "Cut";
			this.tabContextMenuCut.Click += new System.EventHandler(this.TabCut_Click);
			// 
			// tabContextMenuCopy
			// 
			this.contextMenuImageExtender.SetImage(this.tabContextMenuCopy, ((System.Drawing.Image)(resources.GetObject("tabContextMenuCopy.Image"))));
			this.tabContextMenuCopy.Index = 4;
			this.tabContextMenuCopy.OwnerDraw = true;
			this.tabContextMenuCopy.Text = "Copy";
			this.tabContextMenuCopy.Click += new System.EventHandler(this.TabCopy_Click);
			// 
			// tabContextMenuPaste
			// 
			this.contextMenuImageExtender.SetImage(this.tabContextMenuPaste, ((System.Drawing.Image)(resources.GetObject("tabContextMenuPaste.Image"))));
			this.tabContextMenuPaste.Index = 5;
			this.tabContextMenuPaste.OwnerDraw = true;
			this.tabContextMenuPaste.Text = "Paste";
			this.tabContextMenuPaste.Click += new System.EventHandler(this.TabPaste_Click);
			// 
			// menuSeperator2
			// 
			this.contextMenuImageExtender.SetImage(this.menuSeperator2, null);
			this.menuSeperator2.Index = 6;
			this.menuSeperator2.OwnerDraw = true;
			this.menuSeperator2.Text = "-";
			// 
			// tabContextMenuGoto
			// 
			this.contextMenuImageExtender.SetImage(this.tabContextMenuGoto, ((System.Drawing.Image)(resources.GetObject("tabContextMenuGoto.Image"))));
			this.tabContextMenuGoto.Index = 7;
			this.tabContextMenuGoto.OwnerDraw = true;
			this.tabContextMenuGoto.Text = "Goto Line";
			this.tabContextMenuGoto.Click += new System.EventHandler(this.TabGoto_Click);
			// 
			// menuSeperator3
			// 
			this.contextMenuImageExtender.SetImage(this.menuSeperator3, null);
			this.menuSeperator3.Index = 8;
			this.menuSeperator3.OwnerDraw = true;
			this.menuSeperator3.Text = "-";
			// 
			// tabContextMenuSetup
			// 
			this.contextMenuImageExtender.SetImage(this.tabContextMenuSetup, null);
			this.tabContextMenuSetup.Index = 9;
			this.tabContextMenuSetup.OwnerDraw = true;
			this.tabContextMenuSetup.Text = "Page Setup";
			this.tabContextMenuSetup.Click += new System.EventHandler(this.TabPageSetup_Click);
			// 
			// tabContextMenuPrint
			// 
			this.contextMenuImageExtender.SetImage(this.tabContextMenuPrint, null);
			this.tabContextMenuPrint.Index = 10;
			this.tabContextMenuPrint.OwnerDraw = true;
			this.tabContextMenuPrint.Text = "Print";
			this.tabContextMenuPrint.Click += new System.EventHandler(this.TabPrint_Click);
			// 
			// MainFormClass
			// 
			this.AutoScaleMode = AutoScaleMode.None;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(792, 566);
			this.Controls.Add(this.dockingPanel);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.toolbarHolder);
			this.Controls.Add(this.commandBarDockLeft);
			this.Controls.Add(this.commandBarDockRight);
			this.Controls.Add(this.commandBarDockTop);
			this.Controls.Add(this.commandBarDockBottom);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainFormClass";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "MegaIDE";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.MainForm_Closing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.toolbarHolder.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fileToolbar)).EndInit();
			this.fileToolbar.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.menuBar)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.buildToolbar)).EndInit();
			this.statusBar.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.statusBarButtonsHolder)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.lncolStatusBarPanel)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.statusBarProgressPanel)).EndInit();
			this.dockingPanel.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.tabbedGroups)).EndInit();
			this.propertiesWindow.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.toolBox)).EndInit();
			this.ResumeLayout(false);

		}

		

		

		

		

		/*[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles(); 
			Application.DoEvents();	
			Application.Run(MainFormClass.MainForm);
		}*/

		#endregion
		
		#region ErrorList and OutputWindow event handlers and methods
		
		/// <summary>
		/// Called just before the simulation process starts.
		/// </summary>
		public void BeginSimulation()
		{
			outputWindow.Clear();
			statusBarProgressPanel.Value=0;
			Cursor=Cursors.WaitCursor;		
		}
		
		/// <summary>
		/// Called after the simulation process has been started or terminated.
		/// </summary>
		public void EndSimulation(bool showError)
		{
			if(showError)
			{
				dockingManagerExtender.DockingManager.ShowDockObject(dockingManagerExtender.DockingManager.DockObjects["Output"]);
				dockingManagerExtender.DockingManager.DockObjects["Output"].BringToFront();
			}
			statusBarProgressPanel.Value=0;
			Cursor=Cursors.Default;
			outputWindow.Focus();
		}

		/// <summary>
		/// Method to be called just before programming.
		/// </summary>
		public void BeginProgramming()
		{
			toolbarProgram.Enabled=false;
			projectProgram.Enabled=false;
			outputWindow.Clear();
			statusBarProgressPanel.Value=0;
			dockingManagerExtender.DockingManager.ShowDockObject(dockingManagerExtender.DockingManager.DockObjects["Output"]);
			dockingManagerExtender.DockingManager.DockObjects["Output"].BringToFront();
			Cursor=Cursors.WaitCursor;		
		}
		
		/// <summary>
		/// Method to be called just after programming.
		/// </summary>
		public void EndProgramming()
		{
			statusBarProgressPanel.Value=0;
			toolbarProgram.Enabled=true;
			projectProgram.Enabled=true;
			Cursor=Cursors.Default;
			outputWindow.Focus();
		}

		/// <summary>
		/// Disables the build command buttons.
		/// </summary>
		public void BeginBuilding()
		{
			toolbarBuild.Enabled=false;
			projectBuild.Enabled=false;
			statusBarProgressPanel.Value=0;
			outputWindow.Clear();
			tempErrorList.Clear();
			SaveProjectFiles();
			Cursor=Cursors.WaitCursor;
		}
		
		/// <summary>
		/// Called after building the project to re enable buttons and menus.
		/// </summary>
		public void EndBuilding(int showWindow)
		{
			toolbarBuild.Enabled=true;
			projectBuild.Enabled=true;
			statusBarProgressPanel.Value=0;
			Cursor=Cursors.Default;
			if(showWindow==1)
			{
				dockingManagerExtender.DockingManager.ShowDockObject(dockingManagerExtender.DockingManager.DockObjects["Error List"]);
				dockingManagerExtender.DockingManager.DockObjects["Error List"].BringToFront();
				if(!showErrors)
					errorStatusBarPanel_Click(null,null);
			}
			else if(showWindow==2)
			{
				dockingManagerExtender.DockingManager.ShowDockObject(dockingManagerExtender.DockingManager.DockObjects["Output"]);
				dockingManagerExtender.DockingManager.DockObjects["Output"].BringToFront();
			}
			outputWindow.Focus();
		}

		/// <summary>
		/// Parses the listitem clicked in error list and opens file and goes to specified line.
		/// </summary>
		/// <param name="sender">
		/// Sender object.
		/// </param>
		/// <param name="e">
		/// Event Arguments
		/// </param>
		private void errorList_ItemActivate(object sender, EventArgs e)
		{
			if(errorList.SelectedItems[0].SubItems[2].Text.Length==0)
				return;
			if(errorList.SelectedItems[0].SubItems[3].Text[1]==':')
				GotoFileLine(errorList.SelectedItems[0].SubItems[3].Text,int.Parse(errorList.SelectedItems[0].SubItems[2].Text));
			else
				GotoFileLine(ProjectManagerClass.ProjectManager.ProjectFolder+"\\"+errorList.SelectedItems[0].SubItems[3].Text,int.Parse(errorList.SelectedItems[0].SubItems[2].Text));
		}
		
		/// <summary>
		/// Parses the link clicked in output window and opens file and goes to specified line.
		/// </summary>
		/// <param name="sender">
		/// Sender object.
		/// </param>
		/// <param name="e">
		/// Event Arguments
		/// </param>
		private void outputWindow_LinkClicked(object sender, System.Windows.Forms.LinkClickedEventArgs e)
		{
			string parseString=e.LinkText;
			string lineNumber="",errorFilePath="",drive="";

			int index=parseString.IndexOf('/');
			if(index==2)
			{
				drive=parseString.Substring(0,index);
				parseString=parseString.Substring(index).Trim();
			}

			index=parseString.IndexOf('\\');
			if(index==2)
			{
				drive=parseString.Substring(0,index);
				parseString=parseString.Substring(index).Trim();
			}
			
			index = parseString.IndexOf(':')+1;
			if(index>0)
			{
				if(!(parseString.Trim().StartsWith("In file included")))
				{
					errorFilePath=parseString.Substring(0,index-1).Trim();
					parseString=parseString.Substring(index).Trim();
						
							
					index = parseString.IndexOf(':')+1;
					if(index>0)
					{
						lineNumber=parseString.Substring(0,index-1).Trim();
						parseString=parseString.Substring(index).Trim();
						try
						{
							index=int.Parse(lineNumber);
						}
						catch
						{
							index=-1;
						}						
					}
				}
			}
			if(lineNumber.Length==0)
				return;
			if(drive.Length==0)
				GotoFileLine(ProjectManagerClass.ProjectManager.ProjectFolder+"\\"+errorFilePath,index);
			else
				GotoFileLine(drive+errorFilePath,index);
		}

		private void SourceTabPage_MouseDown(object sender, MouseEventArgs e)
		{
			(sender as SyntaxEdit).LineSeparator.Options=QWEditor.SeparatorOptions.None; 
			(sender as SyntaxEdit).MouseDown-=new MouseEventHandler(SourceTabPage_MouseDown);
		}

		private void errorStatusBarPanel_Click(object sender, System.EventArgs e)
		{
			showErrors=!showErrors;
			if(!showErrors)
			{
				errorStatusBarPanel.BackColor=System.Drawing.SystemColors.ControlDark;
				errorStatusBarPanel.ForeColor=System.Drawing.SystemColors.GrayText;
				
				foreach(ListViewItem listItem in errorList.Items)
				{
					if(listItem.ImageIndex==9)
					{
						tempErrorList.Add(listItem);				
					}
				}
				foreach(ListViewItem listItem in tempErrorList)
				{
					errorList.Items.Remove(listItem);
				}
			}
			else
			{
				errorStatusBarPanel.BackColor=System.Drawing.SystemColors.Control;
				errorStatusBarPanel.ForeColor=System.Drawing.SystemColors.ControlText;
				foreach(ListViewItem listItem in tempErrorList)
				{
					if(listItem.ImageIndex==9)
					{
						errorList.Items.Add(listItem);				
					}
				}
				foreach(ListViewItem listItem in errorList.Items)
				{
					tempErrorList.Remove(listItem);
				}
				if(errorList.Items.Count != 0)
				{
					errorList.Columns[1].Width=-2;
					errorList.Columns[2].Width=70;
					errorList.Columns[3].Width=-2;
				}
			}
			tabbedGroups.Focus();
		}

		private void warningStatusBarPanel_Click(object sender, System.EventArgs e)
		{
			showWarnings=!showWarnings;
			if(!showWarnings)
			{
				warningStatusBarPanel.BackColor=System.Drawing.SystemColors.ControlDark;	
				warningStatusBarPanel.ForeColor=System.Drawing.SystemColors.GrayText;
				foreach(ListViewItem listItem in errorList.Items)
				{
					if(listItem.ImageIndex==10)
					{
						tempErrorList.Add(listItem);				
					}
				}
				foreach(ListViewItem listItem in tempErrorList)
				{
					errorList.Items.Remove(listItem);
				}
			}
			else
			{
				warningStatusBarPanel.BackColor=System.Drawing.SystemColors.Control;
				warningStatusBarPanel.ForeColor=System.Drawing.SystemColors.ControlText;
				foreach(ListViewItem listItem in tempErrorList)
				{
					if(listItem.ImageIndex==10)
					{
						errorList.Items.Add(listItem);				
					}
				}
				foreach(ListViewItem listItem in errorList.Items)
				{
					tempErrorList.Remove(listItem);
				}
				if(errorList.Items.Count != 0)
				{
					errorList.Columns[1].Width=-2;
					errorList.Columns[2].Width=70;
					errorList.Columns[3].Width=-2;
				}
			}
			tabbedGroups.Focus();
		}

		private void outputWindowClearMenuItem_Click(object sender, System.EventArgs e)
		{
			outputWindow.Clear();
		}

		private void outputWindowCopyMenuItem_Click(object sender, System.EventArgs e)
		{
			outputWindow.Copy();
		}

		#endregion

		#region Main Function
		/// <summary>
		/// The main entry point for the application. Starts MegaIDE with or without command line arguments.
		/// </summary>
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.DoEvents();
			//Application.ThreadException+=new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
			if(args.Length==0)
				Application.Run(new MainFormClass());
			else
			{
				Application.Run(new MainFormClass(args[0]));
			}
		}
		
		private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
		{
			MessageBox.Show("Error! "+e.Exception.Message+"\nMake sure the installation is correct. If the problem persists re-install MegaIDE.","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Stop);
			
			Application.Exit();
		}

		#endregion

		#region Code Completion event handlers

		/// <summary>
		/// Autocompletion code. Will need changes with newer version of syntax edit.  Reviewed . To be changed after next version.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void syntaxEdit_NeedCodeCompletion(object sender, QWEditor.CodeCompletionArgs e)
		{
			try
			{
				if(e.CompletionType == CodeCompletionType.ParameterInfo ||(e. CompletionType == CodeCompletionType.None && e.KeyChar =='('))
				{
					SyntaxEdit edit = sender as SyntaxEdit;
					Point Pt = edit.Position;	
					int Left=0, Right=0;
					if(Pt.X>0)
						edit.Lines.GetWord(Pt.Y, Pt.X-2, out Left, out Right);
					if(Left != Right)
					{
						string word = edit.Lines[Pt.Y].Substring(Left, Right-Left + 2);
						foreach(NETXP.Controls.Bars.ListBarGroup selGroup in this.toolBox.Groups)
						{	
							if(selGroup.Items[0].Caption.StartsWith(word.Split('_')[0]))
							{
								IParameterInfo p = new QWEditor.ParameterInfo();
								foreach(NETXP.Controls.Bars.ListBarItem selItem in selGroup.Items)
								{
									if(((string)selItem.Tag).IndexOf(word) != -1)
									{
										QWEditor.IListMember m = p.AddMember();
										int index=((string)selItem.Tag).IndexOf('(');
										m.Qualifier=((string)selItem.Tag).Substring(0,((string)selItem.Tag).LastIndexOf(' ',index,index));
										m.ParamStr=((string)selItem.Tag).Substring(index,((string)selItem.Tag).IndexOf(')')-index+1);
										m.Description=selItem.ToolTipText;
										m.Name = selItem.Caption;
									}
								}
								p.ShowHints=true;
								p.ShowParams = true;
								p.ShowQualifiers=true;
								p.ShowResults=true;
								e.NeedShow = true;
								e.Provider = p;
								e.ToolTip = true;
							}
						}
					}
				}
				if(e.CompletionType == CodeCompletionType.ListMembers || (e. CompletionType == CodeCompletionType.None && e.KeyChar =='_'))
				{
					SyntaxEdit edit = sender as SyntaxEdit;
					Point Pt = edit.Position;	
					int Left=0, Right=0;
					if(Pt.X != 0)
						edit.Lines.GetWord(Pt.Y, Pt.X-1, out Left, out Right);
					if(Left != Right)
					{
						string word = edit.Lines[Pt.Y].Substring(Left, Right-Left + 1);
						foreach(NETXP.Controls.Bars.ListBarGroup selGroup in this.toolBox.Groups)
						{
							if(selGroup.Items[0].Caption.StartsWith(word))
							{
								IParameterInfo p = new QWEditor.ParameterInfo();
								foreach(NETXP.Controls.Bars.ListBarItem selItem in selGroup.Items)
								{
									IListMember m = p.AddMember();
									m.ImageIndex=8;
									m.Name = selItem.Caption.Replace(word,"");
									m.Result="";
									m.Description=selItem.ToolTipText;
								}
								p.ShowParams = true;
								p.ShowHints=true;
								e.NeedShow = true;
								e.Provider = p;
								e.ToolTip = false;
							}
						}
					}
				}        
			}
			catch{}
		}

		private void toolsAutoComplete_Click(object sender, System.EventArgs e)
		{
			if(tabbedGroups.RootSequence.Count != 0)
			{ 
				SyntaxEdit syntaxEdit=tabNode.TabControl.SelectedTab.Control as SyntaxEdit;
				syntaxEdit.Focus();
				syntaxEdit.ListMembers();
			}
		}
		
		#endregion

		#region Incomplete Code

		private void projectReset_Click(object sender, System.EventArgs e)
		{
			if(IsProjectOpen)
				ProjectManagerClass.ProjectManager.ResetDevice();
		}

		#endregion

	}
}
