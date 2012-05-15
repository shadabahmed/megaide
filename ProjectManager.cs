using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using NETXP.Controls.Docking;
using QWEditor;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Diagnostics;
using System.Text;
using System.Xml;


namespace MegaIDE
{
	/// <summary>
	/// Implemens the project functionality and project options dialog.
	/// </summary>
	public sealed class ProjectManagerClass : System.Windows.Forms.Form
	{
		#region Singleton Implementation
		private static ProjectManagerClass projectManager;
		/// <summary>
		/// Singleton implementation variable
		/// </summary>
		public static ProjectManagerClass ProjectManager
		{
			get
			{		
				if(projectManager==null)
					projectManager=new ProjectManagerClass();
				return projectManager;
			}
		}
		#endregion

		#region Private Form Variables 
		private System.Windows.Forms.Label selectedLibrariesLabel;
		private System.Windows.Forms.Label availableLibrariesLabel;
		private System.Windows.Forms.TabPage libraryOptions;
		private System.Windows.Forms.ImageList treeImageList;
		private System.Windows.Forms.GroupBox groupBox;
		private NETXP.Controls.MultiPage settingsPages;
		private System.Windows.Forms.TreeView optionsTree;
		private System.Windows.Forms.Label page1TitleLabel;
		private System.Windows.Forms.ListBox selectedLibList;
		private System.Windows.Forms.ListBox availableLibList;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TabPage advancedOptions;
		private System.ComponentModel.IContainer components;
		private System.Windows.Forms.Button selectButton;
		private System.Windows.Forms.Button deselectButton;
		private System.Windows.Forms.Button selectAllButton;
		private System.Windows.Forms.Button deselectAllButton;
		private System.Windows.Forms.PropertyGrid advancedOptionsPropertyGrid;
		private System.IO.FileSystemWatcher fileSystemWatcher;
		private string libraryOptionsBoxHeading;
		private string libraryOptionsBoxText;
		#endregion

		#region Project Variables
		
		//Variables stored in project files
		private string projectName;
		private ProjectTypes projectType;
		private string projectVersion;
		private NETXP.Controls.TreeViewEx projectFilesTree;
		private ProjectOptions projectOptions;
		
		// Project variables not saved
		private string projectFolder;
		private string projectFileName;		
		private bool isProjectModified;
		private FileStream projectFileStream;
		private string megaLibVersion;
	
		// Paths
		private static string gccPath;

		// Project Libraries Variables
		private System.Collections.ArrayList libList;
		private string assemblyNamespace;
		private System.Reflection.Assembly libAssembly;
		
		// GCC Output Controls
		private RichTextBoxEx outputWindow;
		private ErrorListView errorList;
	
		private serialportcomm.RS232 serialPort;

		#endregion

		#region Project Tree Context Menu Variables
		private NETXP.Components.Extenders.MenuImageExtender contextMenuImageExtender;
		private System.Windows.Forms.ContextMenu projectContextMenu;
		private System.Windows.Forms.MenuItem menuOpenProject;
		private System.Windows.Forms.MenuItem menuSaveProject;
		private System.Windows.Forms.MenuItem menuAddExistingFile;
		private System.Windows.Forms.MenuItem menuAddNewFile;
		private System.Windows.Forms.MenuItem menuOpen;
		private System.Windows.Forms.MenuItem menuRename;
		private System.Windows.Forms.MenuItem menuBuildProject;
		private System.Windows.Forms.MenuItem menuProgramProject;
		private System.Windows.Forms.MenuItem menuSimulateProject;
		private System.Windows.Forms.MenuItem menuExcludeFromProject;
		private System.Windows.Forms.MenuItem menuDelete;
		private System.Windows.Forms.MenuItem menuProjectOptions;
		private System.Windows.Forms.MenuItem menuCloseProject;
		private System.Windows.Forms.MenuItem menuSaveFile;
		private System.Windows.Forms.MenuItem menuAddNewFolder;
		private System.Windows.Forms.MenuItem menuSaveFileAs;
		private System.Windows.Forms.MenuItem menuNewProject;
		#endregion

		#region Properties

		/// <summary>
		/// Gets or Sets if the project is modified.
		/// </summary>
		public bool IsProjectModified
		{
			get{return isProjectModified;}
			set{isProjectModified=value;}
		}

		/// <summary>
		/// Gets the currently opened project file name.
		/// </summary>
		public string ProjectFileName
		{
			get{return projectFileName;}
		}

		/// <summary>
		/// Gets the currently opened project folder path.
		/// </summary>
		public string ProjectFolder
		{
			get{return projectFolder;}
		}

		/// <summary>
		/// Returns an object from the libList.
		/// </summary>
		/// <param name="index">
		/// Index number.
		/// </param>
		/// <returns>
		/// Object with the selected index.
		/// </returns>
		public object GetSelectedObject(int index)
		{
			return libList[index];
		}
		
		/// <summary>
		/// Path for GCC compiler
		/// </summary>
		public static string GCCPath
		{
			get{return gccPath;}
			set{gccPath=value;}
		}

		#endregion 

		#region Initalization Code

		/// <summary>
		/// Contructor for this class
		/// </summary>
		private ProjectManagerClass()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
		
			
			// Project Options Dialog Box Initialzation
			TreeNode projectOptionsFolderNode=new System.Windows.Forms.TreeNode("Project Options", 1, 1);
			projectOptionsFolderNode.Tag=true;
			optionsTree.Nodes.Add(projectOptionsFolderNode);
			TreeNode libraryOptionsNode=new System.Windows.Forms.TreeNode("Library Options", 0, 2);
			libraryOptionsNode.Tag=false;
			TreeNode advancedOptionsNode=new System.Windows.Forms.TreeNode("Advanced Options", 0, 2);
			advancedOptionsNode.Tag=false;
			projectOptionsFolderNode.Nodes.AddRange(new TreeNode[]{libraryOptionsNode,advancedOptionsNode});
			
			// Project variables intialization
			isProjectModified=false;
			projectFilesTree=MainFormClass.MainForm.ProjectFilesTree;
			
			projectFilesTree.AfterLabelEdit+=new NodeLabelEditEventHandler(projectFilesTree_AfterLabelEdit);
			projectFilesTree.DoubleClick+=new EventHandler(projectFilesTree_DoubleClick);
			
			projectFilesTree.ContextMenu=projectContextMenu;
			
			// GCC Output controls initialization
			outputWindow=MainFormClass.MainForm.OutputWindow;
			errorList=MainFormClass.MainForm.ErrorList;

			// Setting GCC Path
			//gccPath=MainFormClass.MainForm.AppDirectory+"\\avr-gcc\\bin";
			
			// Loading the libraries Assembly
			//libAssembly=AppDomain.CurrentDomain.Load("MegaLib");
			libAssembly=Assembly.Load("MegaLib");
			megaLibVersion=libAssembly.FullName.Substring(libAssembly.FullName.IndexOf("Version=")+8,7);
			//MessageBox.Show(libAssembly.FullName+"| |"+megaLibVersion+"|");

			// Library Options Box Initializations
			libraryOptionsBoxHeading="Library Options";
			libraryOptionsBoxText="Select only the libraries you need. Selecting unnecessary libraries will increase code size and device programming time.";
	
			// Initializing Liblist
			libList=new ArrayList(32);
		
			//Serial Port
			serialPort=new serialportcomm.RS232();
			serialPort.Timeout=50;
		}

		#endregion

		#region Project Options Dialog Box Functions
		
		/// <summary>
		/// Shows "Project Options" dialog box.
		/// </summary>
		public void ShowOptionsDialog()
		{
			settingsPages.ShowTabs=false;
			settingsPages.Refresh();
			if(optionsTree.SelectedNode==null) 	
			{
				optionsTree.SelectedNode=optionsTree.Nodes[0].Nodes[0];
				settingsPages.SelectedIndex=0;
			}
			
			// Make a temporary projectOptions variable
			ProjectOptions tempSettings=new ProjectOptions();
			
			tempSettings.CompilerFlags=projectOptions.CompilerFlags;
			tempSettings.LinkerFlags=projectOptions.LinkerFlags;
			tempSettings.CrystalFrequency=projectOptions.CrystalFrequency;
			tempSettings.OptimizationFlags=projectOptions.OptimizationFlags;
			tempSettings.ProjectType=projectOptions.ProjectType;
			tempSettings.MCU=projectOptions.MCU;
			tempSettings.PrintfLibraryType=projectOptions.PrintfLibraryType;
			tempSettings.ScanfLibraryType=projectOptions.ScanfLibraryType;
			tempSettings.GenerateListFile=projectOptions.GenerateListFile;
			tempSettings.GenerateMapFile=projectOptions.GenerateMapFile;
			tempSettings.ProgrammingPort=projectOptions.ProgrammingPort;
			tempSettings.OutputFolder=projectOptions.OutputFolder;
			tempSettings.LinkMathLibrary=projectOptions.LinkMathLibrary;
			tempSettings.OutputType=projectOptions.OutputType;
			tempSettings.CharProperty=projectOptions.CharProperty;
			tempSettings.BitFieldsProperty=projectOptions.BitFieldsProperty;
			tempSettings.ObjCopyArguments=projectOptions.ObjCopyArguments;

			tempSettings.AllowLibTypeChange=projectOptions.AllowLibTypeChange;
			tempSettings.AllowMCUChange=projectOptions.AllowMCUChange;
			tempSettings.AllowCrysFreqChange=projectOptions.AllowCrysFreqChange;
			tempSettings.AllowMathLibChange=projectOptions.AllowMathLibChange;
			tempSettings.GCC_Version=projectOptions.GCC_Version;
			
			advancedOptionsPropertyGrid.SelectedObject=tempSettings;
			
			if(ShowDialog(MainFormClass.MainForm)==DialogResult.OK)
			{
				// Reload libraries
				projectOptions=tempSettings;
				
				MainFormClass.MainForm.ClearLibraries();

				// Create Instances of Libraries Newly Selected . Keeping those of selected previously.
				ArrayList tempLibList=new ArrayList(32);
				for(int index=0;index<selectedLibList.Items.Count;index++)
				{
					bool addModule=true;
					foreach(object libModule in libList)
					{
						if(libModule.GetType().Name.Equals(selectedLibList.Items[index]))
						{
							addModule=false;
							tempLibList.Add(libModule);
							break;
						}
					}
					if(addModule)
						index=tempLibList.Add(libAssembly.CreateInstance(assemblyNamespace+"."+selectedLibList.Items[index].ToString(),false));		
				}
				libList.Clear();
				libList=tempLibList;
				
				// Adding to libraries node in the tree.
				projectFilesTree.Nodes[0].Nodes[0].Nodes[0].Nodes.Clear();
				for(int index=0;index<libList.Count;index++)
				{
					TreeNode libNode=new TreeNode(libList[index].GetType().Name,7,7);
					libNode.Tag=ProjectTreeItemTypes.LibraryItem;
					projectFilesTree.Nodes[0].Nodes[0].Nodes[0].Nodes.Insert(index,libNode);
				}
				projectFilesTree.Nodes[0].Nodes[0].Nodes[0].Expand();
				MainFormClass.MainForm.PopulateLibraries(libList);
				
				//// Setting the library variables.
				libAssembly.GetType("MegaLib.BaseLib",true,false).GetProperty("CrystalFrequency").SetValue(null,projectOptions.CrystalFrequency,BindingFlags.SetProperty|BindingFlags.Static,null,null,null);
				libAssembly.GetType("MegaLib.BaseLib",true,false).GetProperty("Microcontroller").SetValue(null,projectOptions.MCU.ToString(),BindingFlags.SetProperty|BindingFlags.Static,null,null,null);		

				isProjectModified=true;
			}
			else
			{
				for(int index=0;index<selectedLibList.Items.Count;index++)
				{
					selectedLibList.SetSelected(index,true);
					foreach(object libModule in libList)
					{
						if(libModule.GetType().Name.Equals(selectedLibList.Items[index].ToString()))
						{
							selectedLibList.SetSelected(index,false);
							break;
						}
					}
				}
				RemoveListItems();		
			}
		}
		
		/// <summary>
		/// Move list items from available to selected.
		/// </summary>
		private void AddListItems()
		{
			foreach(object selectedItem in availableLibList.SelectedItems)
			{
				if((bool)libAssembly.GetType(assemblyNamespace+"."+selectedItem.ToString(),true,false).GetProperty("IsAvailable").GetValue(null,BindingFlags.GetProperty|BindingFlags.Static,null,null,null))		
				{
					libAssembly.GetType(assemblyNamespace+"."+selectedItem.ToString(),true,false).GetProperty("IsIncluded").SetValue(null,true,BindingFlags.SetProperty|BindingFlags.Static,null,null,null);
					selectedLibList.Items.Add(selectedItem);
				}
			}
			foreach(object selectedItem in selectedLibList.Items)
			{
				availableLibList.Items.Remove(selectedItem);
			}

		}

		/// <summary>
		/// Remove list items from selected to the available items list.
		/// </summary>
		private void RemoveListItems()
		{
			foreach(object selectedItem in selectedLibList.SelectedItems)
			{
				libAssembly.GetType(assemblyNamespace+"."+selectedItem.ToString(),true,false).GetProperty("IsIncluded").SetValue(null,false,BindingFlags.SetProperty|BindingFlags.Static,null,null,null);
				availableLibList.Items.Add(selectedItem);
			}
			foreach(object selectedItem in availableLibList.Items)
			{
				selectedLibList.Items.Remove(selectedItem);
			}		 
		}

		#endregion

		#region Project Options Dialog Box Event Handlers
		
		private void optionsTree_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			TreeNode clickedNode=optionsTree.GetNodeAt(e.X,e.Y);
			if(clickedNode != null)
			{
				optionsTree.SelectedNode=clickedNode;
				if(((bool)clickedNode.Tag)==true)
				{
					if(clickedNode.IsExpanded)
					{
						clickedNode.Collapse();
					}
					else
					{
						optionsTree.CollapseAll();
						clickedNode.Expand();
						if(clickedNode.Text=="Project Options")
						{
							clickedNode.Nodes[0].ImageIndex=2;
							settingsPages.SelectedIndex=0;
						}
					}
				}
				else
				{
					clickedNode.Parent.Nodes[0].ImageIndex=0;
					if(clickedNode.Text=="Library Options")
						settingsPages.SelectedIndex=0;
					else if(clickedNode.Text=="Advanced Options")
						settingsPages.SelectedIndex=1;
				}
			}
		}
		
		private void cancelButton_Click(object sender, System.EventArgs e)
		{
			DialogResult=DialogResult.Cancel;
		}

		private void okButton_Click(object sender, System.EventArgs e)
		{
			if(File.Exists(projectFolder+"\\"+((ProjectOptions)advancedOptionsPropertyGrid.SelectedObject).OutputFolder))
			{
				MessageBox.Show("A file already exists with the same path as the output folder. Please type in a different name.","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				return;
			}
			try
			{
				if(!Directory.Exists(projectFolder+"\\"+((ProjectOptions)advancedOptionsPropertyGrid.SelectedObject).OutputFolder))
					Directory.CreateDirectory(projectFolder+"\\"+((ProjectOptions)advancedOptionsPropertyGrid.SelectedObject).OutputFolder);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error creating output folder! "+ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			DialogResult=DialogResult.OK;
		}
		
		private void selectButton_Click(object sender, System.EventArgs e)
		{
			AddListItems();
		}

		private void selectAllButton_Click(object sender, System.EventArgs e)
		{
			for(int index=0;index<availableLibList.Items.Count;index++)
				availableLibList.SetSelected(index,true);
			AddListItems();
		}

		private void deselectButton_Click(object sender, System.EventArgs e)
		{
			RemoveListItems();
		}

		private void deselectAllButton_Click(object sender, System.EventArgs e)
		{
			for(int index=0;index<selectedLibList.Items.Count;index++)
				selectedLibList.SetSelected(index,true);
			RemoveListItems();
		}
		
		/// <summary>
		/// Paints the message box below the library lists.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void libraryOptions_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			Graphics g=e.Graphics;
			string remainingText=libraryOptionsBoxText;
			Pen graphicsPen=new Pen(System.Drawing.Color.FromArgb(172,168,153),1);
			g.SmoothingMode=System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
			g.DrawRectangle(graphicsPen,0,libraryOptions.Height-70,libraryOptions.Width-1,69);	
			g.DrawString(libraryOptionsBoxHeading,new Font("Microsoft Sans Serif",8.25f,FontStyle.Bold),Brushes.Black,3,libraryOptions.Height-67);
			if((remainingText.Length*5+3)>libraryOptions.Width)
			{
				
				int offset=0;
				for(int index=0;(index+offset)<remainingText.Length;index++)
				{
					if((index*5+3)>libraryOptions.Width)
					{
						if(index>15)
						{
							if(remainingText.LastIndexOf(' ',offset+index,15)!=-1)
								index=remainingText.LastIndexOf(' ',offset+index,15)-offset;
						}
						remainingText=remainingText.Insert(index+offset,"\n");
						offset=offset+index+1;
						index=0;
					}
				}
			}
			g.DrawString(remainingText,new Font("Microsoft Sans Serif",8.25f),Brushes.Black,3,libraryOptions.Height-53);				
		}	
		
		//TODO: Review
		private void libraryOptions_Resize(object sender, System.EventArgs e)
		{
			availableLibList.Width=selectButton.Location.X-28;
			availableLibList.Refresh();
			selectedLibList.Height=availableLibList.Height=libraryOptions.Height - 44 - 70;
			selectedLibrariesLabel.Location= new Point(selectButton.Location.X+60,24);
			selectedLibList.Location = new Point(selectButton.Location.X+60,44);
			selectedLibList.Width=libraryOptions.Width-8-selectedLibList.Location.X;
			selectedLibList.Refresh();
		}
		
		//TODO: Review
		private void availableLibList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(availableLibList.SelectedItem!=null)
			{
				string lastSelectedLib=(string)availableLibList.SelectedItem;
				libraryOptionsBoxHeading=(string)libAssembly.GetType(assemblyNamespace+"."+lastSelectedLib,true,false).GetProperty("LibraryName").GetValue(null,BindingFlags.GetProperty|BindingFlags.Static,null,null,null);
				libraryOptionsBoxText=(string)libAssembly.GetType(assemblyNamespace+"."+lastSelectedLib,true,false).GetProperty("LibraryDetails").GetValue(null,BindingFlags.GetProperty|BindingFlags.Static,null,null,null);
			}
			else if(selectedLibList.SelectedItem!=null)
			{
				string lastSelectedLib=(string)selectedLibList.SelectedItem;
				libraryOptionsBoxHeading=(string)libAssembly.GetType(assemblyNamespace+"."+lastSelectedLib,true,false).GetProperty("LibraryName").GetValue(null,BindingFlags.GetProperty|BindingFlags.Static,null,null,null);;
				libraryOptionsBoxText=(string)libAssembly.GetType(assemblyNamespace+"."+lastSelectedLib,true,false).GetProperty("LibraryDetails").GetValue(null,BindingFlags.GetProperty|BindingFlags.Static,null,null,null);
			}
			else
			{
				libraryOptionsBoxHeading="Library Options";
				libraryOptionsBoxText="Select only the libraries you need. Selecting unnecessary libraries will increase code size and device programming time.";
			}
			libraryOptions.Refresh();
		}
		
		//TODO: Review
		private void selectedLibList_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if(selectedLibList.SelectedItem!=null)
			{
				string lastSelectedLib=(string)selectedLibList.SelectedItem;
				libraryOptionsBoxHeading=(string)libAssembly.GetType(assemblyNamespace+"."+lastSelectedLib,true,false).GetProperty("LibraryName").GetValue(null,BindingFlags.GetProperty|BindingFlags.Static,null,null,null);;
				libraryOptionsBoxText=(string)libAssembly.GetType(assemblyNamespace+"."+lastSelectedLib,true,false).GetProperty("LibraryDetails").GetValue(null,BindingFlags.GetProperty|BindingFlags.Static,null,null,null);
			}
			else if(availableLibList.SelectedItem!=null)
			{
				string lastSelectedLib=(string)availableLibList.SelectedItem;
				libraryOptionsBoxHeading=(string)libAssembly.GetType(assemblyNamespace+"."+lastSelectedLib,true,false).GetProperty("LibraryName").GetValue(null,BindingFlags.GetProperty|BindingFlags.Static,null,null,null);;
				libraryOptionsBoxText=(string)libAssembly.GetType(assemblyNamespace+"."+lastSelectedLib,true,false).GetProperty("LibraryDetails").GetValue(null,BindingFlags.GetProperty|BindingFlags.Static,null,null,null);
			}
			else
			{
				libraryOptionsBoxHeading="Library Options";
				libraryOptionsBoxText="Select only the libraries you need. Selecting unnecessary libraries will increase code size and device programming time.";
			}
			libraryOptions.Refresh();
		}

		#endregion

		#region Project Methods
		
		private void GetProjectSourceFiles(TreeNode startingNode,ArrayList sourceFilesList)
		{
			foreach(TreeNode fileNode in startingNode.Nodes)
			{
				if(((ProjectTreeItemTypes)fileNode.Tag)==ProjectTreeItemTypes.File)
				{
					string filePath=fileNode.FullPath.Replace(projectFilesTree.Nodes[0].Nodes[0].FullPath+"\\","");
					if(Path.GetExtension(filePath).ToLower()==".c")
					{
						sourceFilesList.Add(filePath);						
					}
				}
				else if(((ProjectTreeItemTypes)fileNode.Tag)==ProjectTreeItemTypes.Directory)
				{
					if(fileNode.Nodes.Count != 0)
						GetProjectSourceFiles(fileNode,sourceFilesList);
				}
			}
		}

		/// <summary>
		/// Adds a file to the current project.
		/// </summary>
		/// <param name="fullFileName">
		/// Full filename of the selected file.
		/// </param>
		/// <param name="selectedNode"></param>
		public void AddFileToProject(string fullFileName,TreeNode selectedNode)
		{
			string folderPath=GetAbsolutePath(selectedNode);
			string destFileName=Path.GetFileName(fullFileName);
			string destFilePath=folderPath+"\\"+destFileName;
			try
			{
				// Checks if the file exists in the target folder or the file is selected from the target folder
				if(File.Exists(destFilePath) && fullFileName.ToLower() != destFilePath.ToLower())					
				{
					if(MessageBox.Show(destFileName+" already exists in folder "+folderPath+" .\nDo you want to replace it ?","MegaIDE",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.No)
						return;
				}

				// Copies the file if the target folder is different from the file folder
				if(fullFileName.ToLower() != (destFilePath).ToLower()) 
				{
					MainFormClass.MainForm.CloseOpenTab(destFilePath);
					File.Copy(fullFileName,destFilePath,true);
				}

				// If the file doesn't exist already in the current node
				if(GetFileNode(destFilePath)==null)
				{
					TreeNode fileNode=new TreeNode(destFileName,2,2);
					fileNode.Tag=MegaIDE.ProjectTreeItemTypes.File;
					selectedNode.Nodes.Add(fileNode);
					MainFormClass.MainForm.OpenFile(destFilePath);
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
		}

		/// <summary>
		/// Adds a new file to the current project.
		/// </summary>
		/// <param name="fullFileName">
		/// Full filename of the selected file.
		/// </param>
		/// <param name="selectedNode"></param>
		public void AddNewFileToProject(string fullFileName,TreeNode selectedNode)
		{
			string folderPath=GetAbsolutePath(selectedNode);
			string destFileName=Path.GetFileName(fullFileName);
			string destFilePath=folderPath+"\\"+destFileName;
			try
			{
				// Checks if the file exists in the target folder or the file is selected from the target folder
				if(File.Exists(destFilePath) && fullFileName.ToLower() != destFilePath.ToLower())					
				{
					if(MessageBox.Show(destFileName+" already exists in folder "+folderPath+" .\nDo you want to replace it ?","MegaIDE",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.No)
						return;
				}
				
				// Copies the file if the target folder is different from the file folder
				if(fullFileName.ToLower() != (destFilePath).ToLower()) 
				{
					File.Copy(fullFileName,destFilePath,true);
				}
				else
					MainFormClass.MainForm.CloseOpenTab(destFilePath);
		
			
				// If the file doesn't exist already in the current node
				if(GetFileNode(destFilePath)==null)
				{
					TreeNode fileNode=new TreeNode(destFileName,2,2);
					fileNode.Tag=MegaIDE.ProjectTreeItemTypes.File;
					selectedNode.Nodes.Add(fileNode);
					MainFormClass.MainForm.OpenFile(destFilePath);
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
		}

		/// <summary>
		/// Returns the full path of a node in the project folder.
		/// </summary>
		/// <param name="selectedNode">
		/// Selected node.
		/// </param>
		/// <returns>
		/// String containing the fullpath.
		/// </returns>
		private string GetAbsolutePath(TreeNode selectedNode)
		{
			return projectFolder+selectedNode.FullPath.Replace(projectFilesTree.Nodes[0].Nodes[0].FullPath,"");
		}
		
		/// <summary>
		/// Returns a treenode from a file path.
		/// </summary>
		/// <param name="fileName">
		/// Fill path of the filename
		/// </param>
		/// <returns>
		/// Treenode from the project tree(null is file is not in the project).
		/// </returns>
		public TreeNode GetFileNode(string fileName)
		{
			fileName=fileName.ToLower();
			string tempFolder=projectFolder.ToLower()+"\\";
			if(fileName.StartsWith(tempFolder))
			{
				fileName=fileName.Replace(tempFolder,"");
				TreeNode selectedNode=projectFilesTree.Nodes[0].Nodes[0];
				
				//Split the filename in node names sorted in heirarchy.
				string [] nodePaths=(fileName).Split('\\');
				
				//Iteratively search for the node.
				for(int index=0;index<nodePaths.Length;index++)
				{
					string nodePath=nodePaths[index];
					for(int nodeIndex=0;nodeIndex<selectedNode.Nodes.Count;nodeIndex++)
					{
						TreeNode node=selectedNode.Nodes[nodeIndex];
						if(Path.GetFileName(node.Text.ToLower())==Path.GetFileName(nodePath))
						{
							selectedNode=node;
							if(index==(nodePaths.Length-1))
								return selectedNode;
							break;
						}
						if(nodeIndex==(selectedNode.Nodes.Count-1))
							return null;
					} 
				}
			}
			return null; 
		}
		
		/// <summary>
		/// Creates a new project.
		/// </summary>
		public void NewProject()
		{
			// Checks for already opened project.
			if(MainFormClass.MainForm.IsProjectOpen)
			{
				if(!CloseProject())
					return;
			}
			try
			{
				// Gets project file name and initializes project variables.
				if(NewDocumentDialogClass.NewDocumentDialog.ShowNewProjectDialog(MainFormClass.MainForm)==DialogResult.OK)
				{
					projectFileName=NewDocumentDialogClass.NewDocumentDialog.FullFileName;
					projectFileStream=new FileStream(projectFileName,FileMode.Create);
					projectFileStream.Lock(0,projectFileStream.Length);
					
					projectName=Path.GetFileNameWithoutExtension(projectFileName);
					projectType=NewDocumentDialogClass.NewDocumentDialog.ProjectType;
					projectVersion=megaLibVersion;
					projectFolder=Path.GetDirectoryName(projectFileName);

					fileSystemWatcher.Path=projectFolder;
					
					projectOptions=new ProjectOptions();

					#region Project Type Specific Initialization
					
					if(projectType==ProjectTypes.MegaBoard_v1)
					{	
						// SettingNameSpace
						assemblyNamespace="MegaLib.MegaBoardLib";
                        projectOptions.GCC_Version=GCCVersions.Jan_22_2007;
						projectOptions.PrintfLibraryType=LibTypes.FloatingPoint;
						projectOptions.ScanfLibraryType=LibTypes.FloatingPoint;
						projectOptions.MCU=MCUTypes.ATMEGA128;
						projectOptions.CrystalFrequency=16000000;
						projectOptions.OutputFolder="Output";
						projectOptions.ProgrammingPort="COM1";
						projectOptions.LinkerFlags="";
						projectOptions.CompilerFlags="";
						projectOptions.GenerateListFile=true;
						projectOptions.GenerateMapFile=true;
						projectOptions.OptimizationFlags= CompilerOptimizationFlags.None;
						projectOptions.LinkMathLibrary=true;
					
						projectOptions.AllowMCUChange=false;
						projectOptions.AllowLibTypeChange=false;
						projectOptions.AllowCrysFreqChange=false;
						projectOptions.AllowMathLibChange=false;
						projectOptions.AllowOutputTypeChange=true;

						TreeNode rootNode=new TreeNode("Megaboard Project",0,0);
						rootNode.Tag=ProjectTreeItemTypes.ProjectType;
						projectFilesTree.Nodes.Add(rootNode);
						TreeNode projectNode=new TreeNode(projectName,1,1);
						projectNode.Tag=ProjectTreeItemTypes.ProjectName;
						rootNode.Nodes.Add(projectNode);
						rootNode.Expand();
						TreeNode libNode=new TreeNode("Libraries",5,5);
						libNode.Tag=ProjectTreeItemTypes.Library;
						projectNode.Nodes.Add(libNode);
						projectNode.Expand();

						// Creates a new file main.c .If file already exists then chooses a suitable name. 
						string fileName=projectFolder+@"\Main";
						int indexNumber=0;
						if(File.Exists(fileName+".c"))
							while(File.Exists(fileName+(++indexNumber).ToString()+".c"));
						if(indexNumber>0)
							fileName+=indexNumber.ToString();
						fileName+=".c";
						System.IO.StreamWriter fileWriter=new StreamWriter(fileName,false,System.Text.Encoding.ASCII);
						fileWriter.Write("#include\"MegaLib.h\"\nint main()\n{\n MegaLibInit();//Initialization Function\n //Write here\n while(1); //do not remove this line\n}\n");
						fileWriter.Flush();
						fileWriter.Close();	

						TreeNode mainFileNode=new TreeNode(Path.GetFileName(fileName),2,2);
						mainFileNode.Tag=ProjectTreeItemTypes.File;
						projectNode.Nodes.Add(mainFileNode);
						MainFormClass.MainForm.OpenFile(fileName);
						projectFilesTree.SelectedNode=mainFileNode;

					}
					else if(projectType==ProjectTypes.ATMega128_Project)
					{	
						// SettingNameSpace
						assemblyNamespace="MegaLib.ATMega128Lib";
						projectOptions.GCC_Version=GCCVersions.Jan_22_2007;
						projectOptions.PrintfLibraryType=LibTypes.FloatingPoint;
						projectOptions.ScanfLibraryType=LibTypes.FloatingPoint;
						projectOptions.MCU=MCUTypes.ATMEGA128;
						projectOptions.CrystalFrequency=16000000;
						projectOptions.OutputFolder="Output";
						projectOptions.ProgrammingPort="COM1";
						projectOptions.LinkerFlags="";
						projectOptions.CompilerFlags="";
						projectOptions.GenerateListFile=true;
						projectOptions.GenerateMapFile=true;
						projectOptions.OptimizationFlags= CompilerOptimizationFlags.None;
						projectOptions.LinkMathLibrary=true;

						projectOptions.AllowMCUChange=true;
						projectOptions.AllowLibTypeChange=true;
						projectOptions.AllowCrysFreqChange=true;
						projectOptions.AllowMathLibChange=true;
						projectOptions.AllowOutputTypeChange=true;

						TreeNode rootNode=new TreeNode("AVR Project",0,0);
						rootNode.Tag=ProjectTreeItemTypes.ProjectType;
						projectFilesTree.Nodes.Add(rootNode);
						TreeNode projectNode=new TreeNode(projectName,1,1);
						projectNode.Tag=ProjectTreeItemTypes.ProjectName;
						rootNode.Nodes.Add(projectNode);
						rootNode.Expand();
						TreeNode libNode=new TreeNode("Libraries",5,5);
						libNode.Tag=ProjectTreeItemTypes.Library;
						projectNode.Nodes.Add(libNode);
						projectNode.Expand();

						// Creates a new file main.c .If file already exists then chooses a suitable name. 
						string fileName=projectFolder+@"\Main";
						int indexNumber=0;
						if(File.Exists(fileName+".c"))
							while(File.Exists(fileName+(++indexNumber).ToString()+".c"));
						if(indexNumber>0)
							fileName+=indexNumber.ToString();
						fileName+=".c";
						System.IO.StreamWriter fileWriter=new StreamWriter(fileName,false,System.Text.Encoding.ASCII);
						fileWriter.Write("#include\"MegaLib.h\"\nint main()\n{\n MegaLibInit();//Initialization Function\n //Write here\n while(1); //do not remove this line\n}\n");
						fileWriter.Flush();
						fileWriter.Close();	

						TreeNode mainFileNode=new TreeNode(Path.GetFileName(fileName),2,2);
						mainFileNode.Tag=ProjectTreeItemTypes.File;
						projectNode.Nodes.Add(mainFileNode);
						MainFormClass.MainForm.OpenFile(fileName);
						projectFilesTree.SelectedNode=mainFileNode;

					}

					#endregion
				
					// Enabling file watcher.
					fileSystemWatcher.EnableRaisingEvents=true;
					
					// Init Lists
					availableLibList.Items.Clear();
					selectedLibList.Items.Clear();
					foreach(Type selectedType in libAssembly.GetExportedTypes())
					{
						if( selectedType.Namespace==assemblyNamespace && !selectedType.IsEnum)
						{	
							selectedType.GetProperty("IsIncluded").SetValue(null,false,BindingFlags.SetProperty,null,null,null);
							availableLibList.Items.Add(selectedType.Name);
						}
					}
					
					MainFormClass.MainForm.IsProjectOpen=true;
					MainFormClass.MainForm.Text="MegaIDE - "+projectName+".mbp";
						 
					RecentDocuments.AddToRecentProjects(projectFileName);
					MainFormClass.MainForm.UpdateRecentDocumentsList();
						
					// Sets the tab page to first page in project options dialog.
					optionsTree.SelectedNode=optionsTree.Nodes[0].Nodes[0];
					settingsPages.SelectedIndex=0;
						
					// Setting Base static properties.
					libAssembly.GetType("MegaLib.BaseLib",true,false).GetProperty("CrystalFrequency").SetValue(null,projectOptions.CrystalFrequency,BindingFlags.SetProperty|BindingFlags.Static,null,null,null);
					libAssembly.GetType("MegaLib.BaseLib",true,false).GetProperty("Microcontroller").SetValue(null,projectOptions.MCU.ToString(),BindingFlags.SetProperty|BindingFlags.Static,null,null,null);		

					ShowOptionsDialog();
						
					SaveProject();
					
					MainFormClass.MainForm.BringProjectTreeToFront();	
				}
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error creating new project! "+ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}				
		}
		
		/// <summary>
		/// Opens a project from a project file.
		/// </summary>
		/// <param name="fileName">
		/// Path of the project file.
		/// </param>
		public void OpenProject(string fileName)
		{
			//Checking for already opened project.
			if(MainFormClass.MainForm.IsProjectOpen)
			{
				if(fileName.ToLower()==projectFileName.ToLower())
				{	
					MessageBox.Show("The project is already open!","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Information);
					return;
				}

				if(!CloseProject())
					return;
			}

			
			ArrayList openFilesList=new ArrayList(5);
			isProjectModified=false;
			try
			{
				projectFileName=fileName;
				projectFileStream=new FileStream(projectFileName,FileMode.Open);
				projectFileStream.Lock(0,projectFileStream.Length);
				projectFolder=Path.GetDirectoryName(fileName);

				//Deserializing saved variables.
				BinaryFormatter deserializer=new BinaryFormatter();
				deserializer.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
				deserializer.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;	

				projectName=(string)deserializer.Deserialize(projectFileStream);
				string projectTypeString=(string)deserializer.Deserialize(projectFileStream);
				projectType=(ProjectTypes)Enum.Parse(typeof(ProjectTypes),projectTypeString);
				projectVersion=(string)deserializer.Deserialize(projectFileStream);
				
				// Loading Project Tree 
				ArrayList treeNodesList = (ArrayList)deserializer.Deserialize(projectFileStream);
				foreach (TreeNode treeNode in treeNodesList)
				{
					projectFilesTree.Nodes.Add(treeNode);
				}

				openFilesList=(ArrayList)deserializer.Deserialize(projectFileStream);
				projectOptions=(ProjectOptions)deserializer.Deserialize(projectFileStream);
				projectOptions.GCC_Version = projectOptions.GCC_Version;			
				if (projectVersion!=megaLibVersion) 
				{
					if(MessageBox.Show("The project has been created using MegaLib version "+projectVersion.Substring(0,3)+" . It will be changed to the current version ( "+megaLibVersion.Substring(0,3)+" ) .Would you like to continue ?","MegaIDE",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.No)
					{
						MainFormClass.MainForm.ClearLibraries();
						if(projectFilesTree.Nodes.Count != 0)
						{
							MainFormClass.MainForm.CloseProjectFiles();
							projectFilesTree.Nodes.Clear();
						}	
						try
						{
							projectFileStream.Close();
						}
						catch{}
						MainFormClass.MainForm.IsProjectOpen=false;
						return;
					}
					else
					{
						projectVersion=megaLibVersion;
						isProjectModified=true;
					}
				}
				libList=(ArrayList)deserializer.Deserialize(projectFileStream);
				
				// SetAssemblyNameSpace
				if(projectType==ProjectTypes.MegaBoard_v1)
					assemblyNamespace="MegaLib.MegaBoardLib";
				else if(projectType==ProjectTypes.ATMega128_Project)
					assemblyNamespace="MegaLib.ATMega128Lib";
			
				// Setting Base static properties.
				libAssembly.GetType("MegaLib.BaseLib",true,false).GetProperty("CrystalFrequency").SetValue(null,projectOptions.CrystalFrequency,BindingFlags.SetProperty|BindingFlags.Static,null,null,null);
				libAssembly.GetType("MegaLib.BaseLib",true,false).GetProperty("Microcontroller").SetValue(null,projectOptions.MCU.ToString(),BindingFlags.SetProperty|BindingFlags.Static,null,null,null);		
			
				// Loading available libraries
				availableLibList.Items.Clear();
				selectedLibList.Items.Clear();
				foreach(object selectedObject in libList)
				{
					selectedLibList.Items.Add(selectedObject.GetType().Name);
				}
				foreach(Type selectedType in libAssembly.GetExportedTypes())
				{
					if(selectedType.Namespace==assemblyNamespace && !selectedType.IsEnum)
					{
						if(selectedLibList.Items.Contains(selectedType.Name))
						{
							selectedType.GetProperty("IsIncluded").SetValue(null,true,BindingFlags.SetProperty|BindingFlags.Static,null,null,null);
						}
						else
						{
							availableLibList.Items.Add(selectedType.Name);
							selectedType.GetProperty("IsIncluded").SetValue(null,false,BindingFlags.SetProperty|BindingFlags.Static,null,null,null);
						}
					}
				}	

				MainFormClass.MainForm.PopulateLibraries(libList);	
				fileSystemWatcher.Path=projectFolder;
				fileSystemWatcher.EnableRaisingEvents=true;
			}
			catch(SerializationException)
			{
				// Code to run in case of invalid file
				MainFormClass.MainForm.ClearLibraries();
				if(projectFilesTree.Nodes.Count != 0)
				{
					MainFormClass.MainForm.CloseProjectFiles();
					projectFilesTree.Nodes.Clear();
					libList.Clear();
				}
				MessageBox.Show("Error opening project! The path may not be correct or the file could be corrupt.","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);	
				try
				{
					projectFileStream.Close();
				}
				catch{}
				MainFormClass.MainForm.IsProjectOpen=false;
				return;
			}
			catch(Exception ex)
			{
				// Code to run in case of invalid file
				MainFormClass.MainForm.ClearLibraries();
				if(projectFilesTree.Nodes.Count != 0)
				{
					MainFormClass.MainForm.CloseProjectFiles();
					projectFilesTree.Nodes.Clear();
				}
				MessageBox.Show("Error opening project! "+ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);	
				try
				{
					projectFileStream.Close();
				}
				catch{}
				MainFormClass.MainForm.IsProjectOpen=false;
				return;
			}

			//Checking for missing files
			ArrayList missingFilesList=new ArrayList(20);
			ArrayList missingNodesList=new ArrayList(20);
			CheckMissingFiles(projectFilesTree.Nodes[0].Nodes[0],missingFilesList,missingNodesList);
			
			if(missingFilesList.Count != 0)
			{
				if(ExitDialogClass.ExitDialog.ShowMissingFilesDialog(MainFormClass.MainForm,missingFilesList)==DialogResult.Cancel)
				{
					projectFilesTree.Nodes.Clear();
					MainFormClass.MainForm.ClearLibraries();
					ExitDialogClass.ExitDialog.RestoreSettings();
					return;
				}
				ExitDialogClass.ExitDialog.RestoreSettings();
				
				//Remove the missing files from the opened files list and tree.
				ArrayList fileNamesToRemove=new ArrayList(3);
				foreach(TreeNode fileNode in missingNodesList)
				{
					if(openFilesList.Count != 0)
					{
						foreach(string openFileName in openFilesList)
						{
							if((projectFolder+"\\"+openFileName).ToLower().StartsWith(GetAbsolutePath(fileNode).ToLower()))
								fileNamesToRemove.Add(openFileName);
						}
					}
					fileNode.Remove();
				}
				foreach(string fileNameToRemove in fileNamesToRemove)
				{
					openFilesList.Remove(fileNameToRemove);
				}
			    isProjectModified=true;
			}
			if(isProjectModified)
			{
				SaveProject();
			}
			MainFormClass.MainForm.IsProjectOpen=true;
			isProjectModified=false;
			if(openFilesList.Count != 0)
				MainFormClass.MainForm.OpenProjectFiles(projectFolder,openFilesList);
			
			NewDocumentDialogClass.NewDocumentDialog.DialogMode=NewDocumentDialogClass.DialogModes.None;
			MainFormClass.MainForm.Text="MegaIDE - "+projectName+".mbp";
			RecentDocuments.AddToRecentProjects(projectFileName);
			MainFormClass.MainForm.UpdateRecentDocumentsList();
			projectFilesTree.ExpandAll();
		}

		/// <summary>
		/// Saves the currently opened project
		/// </summary>
		public void SaveProject()
		{
			try
			{
				projectFileStream.Seek(0,System.IO.SeekOrigin.Begin);
				BinaryFormatter serializer=new BinaryFormatter();
				serializer.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
				serializer.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesWhenNeeded;

				serializer.Serialize(projectFileStream,projectName);
				serializer.Serialize(projectFileStream,projectType.ToString());
				serializer.Serialize(projectFileStream,projectVersion);
				// Saving Project Tree
				ArrayList treeNodesList = new ArrayList();
				foreach (TreeNode treeNode in projectFilesTree.Nodes)
					treeNodesList.Add(treeNode);
				serializer.Serialize(projectFileStream,treeNodesList);
				
				serializer.Serialize(projectFileStream,MainFormClass.MainForm.GetOpenProjectFilesList());
				serializer.Serialize(projectFileStream,projectOptions);
				serializer.Serialize(projectFileStream,libList);
				projectFileStream.Flush();
				isProjectModified=false;
			}
			catch(Exception ex)
			{
			 MessageBox.Show("Error saving project! "+ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);	
			}
		}

		/// <summary>
		/// Closes the currently opened project.
		/// </summary>
		/// <returns>
		/// Returns false if the closing is cancelled else returns true.
		/// </returns>
		public bool CloseProject()
		{
			DialogResult result=ExitDialogClass.ExitDialog.ShowSaveDialog(MainFormClass.MainForm,MainFormClass.MainForm.GetUnsavedProjectFiles());
			if(result==DialogResult.Cancel)
			{
				return false;
			}
			else if(result==DialogResult.Yes)
			{
				foreach(ExitDialogClass.ListItem selectedItem in ExitDialogClass.ExitDialog.SelectedItems)
				{
					if(selectedItem.SaveFileType==ExitDialogClass.SaveFileTypes.File)
						MainFormClass.MainForm.SaveFile(selectedItem.FileName);
					else if(selectedItem.SaveFileType==ExitDialogClass.SaveFileTypes.ProjectFile)
						SaveProject();
				}
			}
			projectFileStream.Flush();
			projectFileStream.Close();
			fileSystemWatcher.EnableRaisingEvents=false;
			MainFormClass.MainForm.CloseProjectFiles();
			projectFilesTree.Nodes.Clear();
			MainFormClass.MainForm.ClearLibraries();
			MainFormClass.MainForm.IsProjectOpen=false;
			isProjectModified=false;
			libList.Clear();

			// Resetting controls in the IDE.
			outputWindow.Clear();
			errorList.Items.Clear();
			MainFormClass.MainForm.Errors=0;
			MainFormClass.MainForm.Warnings=0;
			MainFormClass.MainForm.Text="MegaIDE";
			return true;
		}
		
		/// <summary>
		/// Checks missing files in a project.
		/// </summary>
		/// <param name="startingNode">
		/// Starting node to search in.
		///  </param>
		/// <param name="missingFilesList">
		/// Files list containing references to be removed.
		/// </param>
		/// <param name="missingNodesList">
		/// Nodes list containing nodes to be removed.
		/// </param>
		private void CheckMissingFiles(TreeNode startingNode,ArrayList missingFilesList,ArrayList missingNodesList)
		{
			foreach(TreeNode fileNode in startingNode.Nodes)
			{
				if(((ProjectTreeItemTypes)fileNode.Tag)==ProjectTreeItemTypes.File)
				{
					string filePath=GetAbsolutePath(fileNode);
					if(!File.Exists(filePath))
					{
						missingFilesList.Add("File : "+filePath.Replace(projectFolder+"\\",""));
						missingNodesList.Add(fileNode);
					}
				}
				else if(((ProjectTreeItemTypes)fileNode.Tag)==ProjectTreeItemTypes.Directory)
				{
					string filePath=GetAbsolutePath(fileNode);
					if(!Directory.Exists(GetAbsolutePath(fileNode)))
					{
						missingFilesList.Add("Directory : "+filePath.Replace(projectFolder+"\\",""));
						missingNodesList.Add(fileNode);
					}
					else
					{
						if(fileNode.Nodes.Count != 0)
							CheckMissingFiles(fileNode,missingFilesList,missingNodesList);
					}
				}
			}
		}

		#endregion

		#region Compilation and Programming Methods
		/// <summary>
		/// Builds the currently open project.
		/// </summary>
		public void BuildProject()
		{	
			// Disabling buttons and menus.
			MainFormClass.MainForm.BeginBuilding();
			menuBuildProject.Enabled=false;
			
			outputWindow.AppendText("---------------------- Build Started for Project : "+projectName+" ----------------------\n");
			
			#region Definitions file generation
			// Generation of definitions file.
			outputWindow.AppendText("\nCreating definitions file...\n");
			System.IO.StreamWriter streamWriter=new System.IO.StreamWriter(projectFolder+"\\"+"MegaLib.h",false,System.Text.Encoding.ASCII);
			try
			{
				streamWriter.Write("#ifndef _MEGABOARD_H_\n#define _MEGABOARD_H_\n/*Check for the target MCU and Definition Files*/\n#include<avr/io.h>\n\n#include<stdio.h>\n#include<avr/interrupt.h>\n#include<avr/interrupt.h>\n#include<string.h>\n#include<stdlib.h>\n#include<math.h>\n/* MegaBoard Specific Settings */\n#define cbi(sfr, bit) (_SFR_BYTE(sfr) &= ~_BV(bit))\n#define sbi(sfr, bit) (_SFR_BYTE(sfr) |= _BV(bit))\n");
				foreach(object selectedLib in libList)
				{
					streamWriter.Write((string)selectedLib.GetType().GetMethod("GenerateCode").Invoke(selectedLib,new object[]{}));
				}
				//streamWriter.Write("\nvoid MegaLibInit(void)\n{\n DDRA=0;\n DDRB=0;\n DDRC=0;\n DDRD=0;\n DDRE=0;\n DDRF=0;\n MCUCR=_BV(IVCE);\n MCUCR=00;\n SFIOR=00;\n");
				streamWriter.Write("\nvoid MegaLibInit(void)\n{\n DDRA=0;\n DDRB=0;\n DDRC=0;\n DDRD=0;\n MCUCR=_BV(IVCE);\n MCUCR=00;\n SFIOR=00;\n");
				
				foreach(object selectedLib in libList)
				{
					streamWriter.Write((string)selectedLib.GetType().GetMethod("GenerateInitCode").Invoke(selectedLib,new object[]{}));
				}	
				streamWriter.Write("\n}\n\n#endif /* _MEGABOARD_H_ */\n");	
				streamWriter.Flush();
				streamWriter.Close();
			}
			catch(Exception ex)
			{
				outputWindow.AppendText("\nError generating definitions file. \n\n"+ex.Message+"\n");
				outputWindow.AppendText(String.Format("\n---------------------- Build Process Stopped for Project : "+projectName+" ----------------------"));
				MainFormClass.MainForm.EndBuilding(2);
				menuBuildProject.Enabled=true;
				streamWriter.Close();
				return;
			}
			outputWindow.AppendText("\nDefinitions file generated !\n");
			outputWindow.Focus();
			#endregion

			MainFormClass.MainForm.PercentDone=33;

			//Getting C files in the project.
			ArrayList sourceFilesList=new ArrayList(10);
			GetProjectSourceFiles(projectFilesTree.Nodes[0].Nodes[0],sourceFilesList);
			
			#region Compilation and Linking
			// Compiling and Lining
			outputWindow.AppendText("\nCompiling and Linking...\n");
			
			if(Directory.Exists(projectFolder+"\\"+projectOptions.OutputFolder))
			{
				try
				{
					Directory.CreateDirectory(projectFolder+"\\"+projectOptions.OutputFolder);
				}
				catch(Exception ex)
				{
					outputWindow.AppendText("\nError creating output folder : "+projectOptions.OutputFolder+"\n"+ex.Message);
					outputWindow.AppendText(String.Format("\n---------------------- Build Process Stopped for Project : "+projectName+" ----------------------"));
					MainFormClass.MainForm.EndBuilding(2);
					menuBuildProject.Enabled=true;
					return;
				}
			}

			string elfFilePath="\""+projectOptions.OutputFolder+"\\"+projectName+".elf\"";	
			
			Process gccProcess=new Process();
			gccProcess.StartInfo.FileName="\""+gccPath+"\\avr-gcc.exe\"";
			
			// Arguments MCU
			StringBuilder gccArguments=new StringBuilder("-mmcu="+projectOptions.MCU.ToString().ToLower()+" -I. -Wall -Wbad-function-cast -Wredundant-decls -gdwarf-2",10000);
			
			// Printf library
			if(projectOptions.PrintfLibraryType==LibTypes.FloatingPoint)
				gccArguments.Append(" -Wl,-u,vfprintf -lprintf_flt");
			else if(projectOptions.PrintfLibraryType==LibTypes.Minimal)
				gccArguments.Append(" -Wl,-u,vfprintf -lprintf_min");
			
			// Scanf library
			if(projectOptions.ScanfLibraryType==LibTypes.FloatingPoint)
				gccArguments.Append(" -Wl,-u,vfscanf -lscanf_flt");
			else if(projectOptions.ScanfLibraryType==LibTypes.Minimal)
				gccArguments.Append(" -Wl,-u,vfscanf -lscanf_min");
			
			//Optimization
			if(projectOptions.OptimizationFlags==CompilerOptimizationFlags.Size)
				gccArguments.Append(" -Os");	
			else
				gccArguments.Append(" -O"+((int)projectOptions.OptimizationFlags).ToString());
			
			// Generate List File
			if(projectOptions.GenerateListFile)
				gccArguments.Append(" -Wa,-ahlmns=\""+projectOptions.OutputFolder+"\\"+projectName+".lst\",-gstabs");
			
			// Generate Map File
			if(projectOptions.GenerateMapFile)
				gccArguments.Append(" -Wl,-Map=\""+projectOptions.OutputFolder+"\\"+projectName+".map\",--cref");
			
			// Char Signed Type
			if(projectOptions.CharProperty==SignedTypes.Unsigned)
				gccArguments.Append(" -funsigned-char");

			// Bitfields Signed Type
			if(projectOptions.BitFieldsProperty==SignedTypes.Unsigned)
				gccArguments.Append(" -funsigned-bitfields");

			// Processor Frequency
			gccArguments.Append(" -DF_CPU="+projectOptions.CrystalFrequency+"UL");
			
			// Compiler flags
			gccArguments.Append(" "+projectOptions.CompilerFlags+" ");

			//Appending source files
			foreach(string sourceFileName in sourceFilesList)
			{
				gccArguments.Append(" \""+sourceFileName+"\"");
			}
			
			//Output file
			gccArguments.Append(" -o "+elfFilePath);
			
			// Linker flags
			gccArguments.Append(" "+projectOptions.LinkerFlags+" ");
			
			//Math Library
			if(projectOptions.LinkMathLibrary)
				gccArguments.Append(" -lm");

			outputWindow.AppendText("\navr-gcc "+gccArguments.ToString()+"\n"); 
			gccProcess.StartInfo.Arguments=gccArguments.ToString();
			gccProcess.StartInfo.WorkingDirectory=projectFolder;
			gccProcess.StartInfo.RedirectStandardError=true;
			gccProcess.StartInfo.RedirectStandardOutput=true;
			gccProcess.StartInfo.CreateNoWindow=true;
			gccProcess.StartInfo.UseShellExecute=false;
			gccProcess.StartInfo.ErrorDialog=true;
			
			errorList.Items.Clear();
			
			try
			{
				gccProcess.Start();
			}
			catch
			{
				gccProcess.Dispose();
				MessageBox.Show("Error starting the compiler. Make sure the installation is correct. If the problem persists  reinstall MegaIDE.","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
				outputWindow.AppendText("\nError starting compiler !\n");
				outputWindow.AppendText(String.Format("\n---------------------- Build Process Stopped for Project : "+projectName+" ----------------------\n"));
				MainFormClass.MainForm.EndBuilding(2);
				menuBuildProject.Enabled=true;
				return;
			}
			
			#region Error Parsing
			// Error parsing
			string gccOutputLine;
			int errorCount = 0; 
			int warningCount = 0;
			
			while((gccOutputLine=gccProcess.StandardError.ReadLine()) != null)
			{
				bool addOutputLineAsLink=false;
				string parseString=gccOutputLine;
				string lineNumber,errorFilePath, message,drive="";
				int errorIndex;
				
				if("warning:"==parseString.Substring(0,9).Trim().ToLower())
				{
					warningCount++;
				}

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
							if(lineNumber.Length != 0)
							{	
								try
								{
									index=int.Parse(lineNumber);
								}
								catch
								{
									index=-1;
								}
							}
							else
								index=-1;
							if(index>0)
							{
								index = parseString.IndexOf("rror")+1;
								if(index>0)
								{
									index=index+4;
									errorIndex=9;
									message=parseString.Substring(index).Trim();
								}
								else if((index=parseString.IndexOf("arning")+1)>0)
								{
									index = index + 6; 
									errorIndex = 10; 
									message = parseString.Substring(index).Trim();
								}
								else
								{
									errorIndex=9;
									message=parseString.Trim();
								}
								if (message.Length != 0) 
								{
									if(errorIndex==9)
									{
										errorCount++;
										if(MainFormClass.MainForm.ShowErrors)
											errorList.Items.Add(new ListViewItem(new string[] {"",message,lineNumber,drive+errorFilePath},9));
										else
											MainFormClass.MainForm.TempErrorList.Add(new ListViewItem(new string[] {"",message,lineNumber,drive+errorFilePath},9));
									}
									else
									{
										warningCount++;
										if(MainFormClass.MainForm.ShowWarnings)
											errorList.Items.Add(new ListViewItem(new string[] {"",message,lineNumber,drive+errorFilePath},10));
										else
											MainFormClass.MainForm.TempErrorList.Add(new ListViewItem(new string[] {"",message,lineNumber,drive+errorFilePath},10));
									}
									addOutputLineAsLink=true;
								}
								drive="";
							}		
						}
						else
						{
							// undefined reference without line number error
							message=parseString=parseString.Substring(index).Trim();
							if (message.Length != 0) 
							{
								errorCount++;
								if(MainFormClass.MainForm.ShowErrors)
									errorList.Items.Add(new ListViewItem(new string[] {"",message,"",drive+errorFilePath},9));
								else
									MainFormClass.MainForm.TempErrorList.Add(new ListViewItem(new string[] {"",message,"",drive+errorFilePath},9));
							
								addOutputLineAsLink=true;
							}
							drive="";	
						}
					}
				}
				if(addOutputLineAsLink)
				{
					outputWindow.InsertLink(gccOutputLine+"\n");
				}
				else
					outputWindow.AppendText(gccOutputLine+"\n");
			}
			
			if(errorList.Items.Count != 0)
			{
				errorList.Columns[1].Width=-2;
				errorList.Columns[2].Width=70;
				errorList.Columns[3].Width=-2;
			}
  
			// Changing errors and warning count.
			MainFormClass.MainForm.Errors=errorCount;
			MainFormClass.MainForm.Warnings=warningCount;

			#endregion
			
			// Monitoring exit code.
			
			if(gccProcess.ExitCode==0)
				outputWindow.AppendText("\nCompilation and Linking Successful !\n");
			else
			{
				gccProcess.Dispose();
				outputWindow.AppendText("\nCompilation and Linking Failed !\n");
				outputWindow.AppendText(String.Format("\n---------------------- Build Process Stopped for Project : "+projectName+" ----------------------"));
				if(errorCount != 0)
					MainFormClass.MainForm.EndBuilding(1);
				else
					MainFormClass.MainForm.EndBuilding(2);
				menuBuildProject.Enabled=true;
				return;
			}
			
			gccProcess.Dispose();
			outputWindow.Focus();

			#endregion

			MainFormClass.MainForm.PercentDone=80;
			
			#region Building Output File
			// Building Hex File
			
			outputWindow.AppendText("\nGenerating Output File...\n");
			
			Process objcopyProcess=new Process();
			objcopyProcess.StartInfo.FileName=gccProcess.StartInfo.FileName="\""+gccPath+"\\avr-objcopy.exe\"";
			
			objcopyProcess.StartInfo.Arguments="";
		
			if(projectOptions.OutputType==OutputTypes.IntelHex)
				objcopyProcess.StartInfo.Arguments+="-j .text -j .data -O ihex "+elfFilePath+" \""+projectOptions.OutputFolder+"\\"+projectName+".hex\"";
			else if(projectOptions.OutputType==OutputTypes.SREC)
				objcopyProcess.StartInfo.Arguments+="-j .text -j .data -O srec "+elfFilePath+" \""+projectOptions.OutputFolder+"\\"+projectName+".hex\"";
			else
				objcopyProcess.StartInfo.Arguments+="-j .text -j .data -O binary "+elfFilePath+" \""+projectOptions.OutputFolder+"\\"+projectName+".hex\"";
			
			objcopyProcess.StartInfo.Arguments+=projectOptions.ObjCopyArguments;


			objcopyProcess.StartInfo.WorkingDirectory=projectFolder;
			objcopyProcess.StartInfo.RedirectStandardError=true;
			objcopyProcess.StartInfo.RedirectStandardOutput=true;
			objcopyProcess.StartInfo.CreateNoWindow=true;
			objcopyProcess.StartInfo.UseShellExecute=false;
			objcopyProcess.StartInfo.ErrorDialog=true;

			outputWindow.AppendText("\navr-objcopy "+objcopyProcess.StartInfo.Arguments+"\n");
			try
			{	
				objcopyProcess.Start();
			}
			catch
			{
				objcopyProcess.Dispose();
				MessageBox.Show("Error starting \"avr-objcopy.exe\". Make sure the installation is correct. If the problem persists  reinstall MegaIDE.","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
				outputWindow.AppendText("\nError generating the Output File !\n");
				outputWindow.AppendText(String.Format("\n---------------------- Build Process Stopped for Project : "+projectName+" ----------------------"));
				MainFormClass.MainForm.EndBuilding(2);
				menuBuildProject.Enabled=true;
				return;
			}
			
			string objcopyOutputLine;
			while((objcopyOutputLine=objcopyProcess.StandardError.ReadLine()) != null)
			{
				outputWindow.AppendText("\n"+objcopyOutputLine+"\n");
			}

			if(objcopyProcess.ExitCode==0)
				outputWindow.AppendText("\nOutput File Generated !\n");
			else
			{
				objcopyProcess.Dispose();
				outputWindow.AppendText("\nError generating the Output File !\n");
				outputWindow.AppendText(String.Format("\n---------------------- Build Process Stopped for Project : "+projectName+" ----------------------"));
				MainFormClass.MainForm.EndBuilding(2);
				menuBuildProject.Enabled=true;
				return;
			}
				
			
			objcopyProcess.Dispose();
			
			#endregion

			MainFormClass.MainForm.PercentDone=100;
			
			#region Memory Consumption
			// Calculating the Memory Consumptions
			Process avrsizeProcess=new Process();
			avrsizeProcess.StartInfo.FileName="\""+gccPath+"\\avr-size.exe\"";

			avrsizeProcess.StartInfo.Arguments="-A "+elfFilePath;
			avrsizeProcess.StartInfo.WorkingDirectory=projectFolder;
			avrsizeProcess.StartInfo.RedirectStandardError=true;
			avrsizeProcess.StartInfo.RedirectStandardOutput=true;
			avrsizeProcess.StartInfo.CreateNoWindow=true;
			avrsizeProcess.StartInfo.UseShellExecute=false;
			avrsizeProcess.StartInfo.ErrorDialog=true;
			
			try
			{	
				avrsizeProcess.Start();
			}
			catch
			{
				avrsizeProcess.Dispose();
				outputWindow.AppendText("\nError reading the memory consumption from the output file ! The output file could still be usable. Make sure the installation is correct.\n");
				outputWindow.AppendText(String.Format("\n---------------------- Build Process Successful for Project : "+projectName+" ----------------------"));
				MainFormClass.MainForm.EndBuilding(2);
				menuBuildProject.Enabled=true;
				return;
			}

			string avrsizeOutputLine;
			uint textSize=0,bssSize=0,eepromSize=0,dataSize=0,noinitSize=0;

			while((avrsizeOutputLine=avrsizeProcess.StandardOutput.ReadLine()) != null)
			{
				if(avrsizeOutputLine.StartsWith(".data"))
				{
					avrsizeOutputLine=avrsizeOutputLine.Substring(5).Trim();
					avrsizeOutputLine=avrsizeOutputLine.Substring(0,avrsizeOutputLine.IndexOf(' ')).Trim();
					dataSize=uint.Parse(avrsizeOutputLine);
				}
				else if(avrsizeOutputLine.StartsWith(".text"))
				{
					avrsizeOutputLine=avrsizeOutputLine.Substring(5).Trim();
					avrsizeOutputLine=avrsizeOutputLine.Substring(0,avrsizeOutputLine.IndexOf(' ')).Trim();
					textSize=uint.Parse(avrsizeOutputLine);	
				}
				else if(avrsizeOutputLine.StartsWith(".bss"))
				{
					avrsizeOutputLine=avrsizeOutputLine.Substring(4).Trim();
					avrsizeOutputLine=avrsizeOutputLine.Substring(0,avrsizeOutputLine.IndexOf(' ')).Trim();
					bssSize=uint.Parse(avrsizeOutputLine);
				}
				else if(avrsizeOutputLine.StartsWith(".eeprom"))
				{
					avrsizeOutputLine=avrsizeOutputLine.Substring(7).Trim();
					avrsizeOutputLine=avrsizeOutputLine.Substring(0,avrsizeOutputLine.IndexOf(' ')).Trim();
					eepromSize=uint.Parse(avrsizeOutputLine);
				}
				else if(avrsizeOutputLine.StartsWith(".noinit"))
				{
					avrsizeOutputLine=avrsizeOutputLine.Substring(7).Trim();
					avrsizeOutputLine=avrsizeOutputLine.Substring(0,avrsizeOutputLine.IndexOf(' ')).Trim();
					noinitSize=uint.Parse(avrsizeOutputLine);
				}
			}

			#endregion

			if(avrsizeProcess.ExitCode==0)
			{
				outputWindow.AppendText(String.Format("\n---------------------- Build Successful for Project : "+projectName+" ----------------------\n\nROM Used : {0} bytes \nRAM Used(Approximate) : {1} bytes",dataSize+textSize,dataSize+bssSize));		
			}
			else
			{
				outputWindow.AppendText("\nError reading the memory consumption from the output file ! The output file could still be usable. Make sure the installation is correct.\n");
				outputWindow.AppendText(String.Format("\n---------------------- Build Process Successful for Project : "+projectName+" ----------------------"));
			}

			// Enabling buttons again.
			MainFormClass.MainForm.EndBuilding(2);
			menuBuildProject.Enabled=true;
		}

		
		/// <summary>
		/// Programs the device.
		/// </summary>
		public void ProgramDevice()
		{
			MainFormClass.MainForm.BeginProgramming();
			menuProgramProject.Enabled=false;
			
			outputWindow.AppendText("---------------------- Programming Started for Project : "+projectName+" ----------------------\n");

			//Checking for hex file.
			string hexFile=projectFolder+"\\"+projectOptions.OutputFolder+"\\"+projectName+".hex";
			if(!File.Exists(hexFile))
			{
				outputWindow.AppendText("\nError! Output file does not exist. Make sure the project has a successful build.\n");
				outputWindow.AppendText("\n---------------------- Programming Stopped for Project : "+projectName+" ----------------------");
				MainFormClass.MainForm.EndProgramming();
				menuProgramProject.Enabled=true;
				return;
			}
			
			outputWindow.AppendText("\nFound a succesful build dated : "+File.GetLastWriteTime(hexFile)+"\n");

			//Checking for board and making it jump to bootloader.
			#region Checking for board and making it jump to bootloader
			try
			{
				if(!projectOptions.ProgrammingPort.ToLower().StartsWith("com"))
					throw new FormatException("");
				int portNumber=int.Parse(projectOptions.ProgrammingPort.Substring(3));
				serialPort.Open(portNumber,115200,8,serialportcomm.RS232.DataParity.Parity_None,serialportcomm.RS232.DataStopBit.StopBit_1,512);
				serialPort.Write("$");
				try
				{
					serialPort.Read(1);
				}
				catch(serialportcomm.IOTimeoutException)
				{
					serialPort.Dtr=false;
					serialPort.Rts=true;
					serialPort.Rts=false;
					serialPort.Dtr=true;
					System.Threading.Thread.Sleep(100);
				}
				serialPort.Write("$");
				serialPort.Read(1);
				serialPort.Close();
			}
			catch(System.FormatException )
			{
				outputWindow.AppendText("\nError! Invalid Port Name.\n");
				outputWindow.AppendText("\n---------------------- Programming Stopped for Project : "+projectName+" ----------------------");
				MessageBox.Show("Error! Invalid Port Name. Please make sure the port name in the project options is valid.","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				MainFormClass.MainForm.EndProgramming();
				menuProgramProject.Enabled=true;
				if(serialPort.IsOpen)
					serialPort.Close();
				return;
			}
			catch(serialportcomm.IOTimeoutException)
			{
				outputWindow.AppendText("\nError! Device not found on "+projectOptions.ProgrammingPort+" .\n");
				outputWindow.AppendText("\n---------------------- Programming Stopped for Project : "+projectName+" ----------------------");
				MessageBox.Show("Error! Device not found on "+projectOptions.ProgrammingPort+" . Make sure that the device is connected to the right port and powered up.","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				MainFormClass.MainForm.EndProgramming();
				menuProgramProject.Enabled=true;
				if(serialPort.IsOpen)
					serialPort.Close();
				return;
			}
			catch(Exception ex)
			{
				outputWindow.AppendText("\nError! "+ex.Message+"\n");
				outputWindow.AppendText("\n---------------------- Programming Stopped for Project : "+projectName+" ----------------------");
				MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				MainFormClass.MainForm.EndProgramming();
				menuProgramProject.Enabled=true;
				if(serialPort.IsOpen)
					serialPort.Close();
				return;
			}
			#endregion

			//Starting avrdude programmer

			#region Programming

			Process avrdudeProcess=new Process();

			if(projectOptions.OutputType==OutputTypes.IntelHex)
				avrdudeProcess.StartInfo.Arguments=String.Format(" -p {0} -c stk500 -u -U flash:w:\"{1}\":i -P {2}",projectOptions.MCU.ToString(),hexFile,projectOptions.ProgrammingPort); 
			else if(projectOptions.OutputType==OutputTypes.SREC)
				avrdudeProcess.StartInfo.Arguments=String.Format(" -p {0} -c stk500 -u -U flash:w:\"{1}\":s -P {2}",projectOptions.MCU.ToString(),hexFile,projectOptions.ProgrammingPort); 
			else
				avrdudeProcess.StartInfo.Arguments=String.Format(" -p {0} -c stk500 -u -U flash:w:\"{1}\":r -P {2}",projectOptions.MCU.ToString(),hexFile,projectOptions.ProgrammingPort); 

			avrdudeProcess.StartInfo.FileName=gccPath+"\\avrdude.exe";
			avrdudeProcess.StartInfo.RedirectStandardError=true;
			avrdudeProcess.StartInfo.RedirectStandardOutput=true;
			avrdudeProcess.StartInfo.CreateNoWindow=true;
			avrdudeProcess.StartInfo.WorkingDirectory=projectFolder;
			avrdudeProcess.StartInfo.UseShellExecute=false;		
			
			outputWindow.Refresh();

			try
			{
				avrdudeProcess.Start();
			}
			catch
			{
				avrdudeProcess.Dispose();
				MessageBox.Show("Error starting the programmer. Make sure the installation is correct. If the problem persists  reinstall MegaIDE.","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
				outputWindow.AppendText("\nError starting programmer !\n");
				outputWindow.AppendText(String.Format("\n---------------------- Programming Stopped for Project : "+projectName+" ----------------------"));
				MainFormClass.MainForm.EndProgramming();
				menuProgramProject.Enabled=true;
				return;
			}
			
			outputWindow.AppendText("\nChecking Device... ");
			outputWindow.Refresh();
			outputWindow.Focus();

			string avrdudeOutputLine;
			
			int avrdudeProgrammingStage=0;
			bool showFailed=true;
			while((avrdudeOutputLine=avrdudeProcess.StandardError.ReadLine()) != null)
			{
				if(avrdudeOutputLine.StartsWith("%"))
				{
					int percentDone=int.Parse(avrdudeOutputLine.Substring(2));
					if(percentDone==100)
					{		
						if(avrdudeProgrammingStage==0)
							outputWindow.AppendText("Done \n\nProgramming Device... ");			
						else if(avrdudeProgrammingStage==1)
							outputWindow.AppendText("Done \n\nVerifying... ");
						else 
						{
							outputWindow.AppendText("Done \n\n");
							showFailed=false;
						}
						avrdudeProgrammingStage++;
						new MethodInvoker(outputWindow.Refresh).BeginInvoke(null,null);
					}
					MainFormClass.MainForm.PercentDone=percentDone;
				}
			}
			
			if(avrdudeProcess.ExitCode==0)
				outputWindow.AppendText("---------------------- Programming Successful for Project : "+projectName+" ----------------------");
			else
			{
				avrdudeProcess.Dispose();
				if(showFailed)
					outputWindow.AppendText("Failed\n");
				outputWindow.AppendText("\n---------------------- Programming Failed for Project : "+projectName+" ----------------------");			
			}
			#endregion
			
			// Re-enabling menus.
			MainFormClass.MainForm.EndProgramming();
			menuProgramProject.Enabled=true;
		}

		/// <summary>
		/// Starts the AVR Studio with the current project's elf file in simulation mode.
		/// </summary>
		public void SimulateProgram()
		{
			MainFormClass.MainForm.BeginSimulation();
			outputWindow.AppendText("---------------------- Starting Simulator for Project : "+projectName+" ----------------------\n");
			//Checking for elf file.
			string elfFile=projectFolder+"\\"+projectOptions.OutputFolder+"\\"+projectName+".elf";
			if(!File.Exists(elfFile))
			{
				outputWindow.AppendText("\nError! Output file does not exist. Make sure the project has a successful build.\n");
				MessageBox.Show("Error ! Could not the output file. Make sure the project has a successful build.","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				outputWindow.AppendText("\n---------------------- Simulation Aborted for Project : "+projectName+" ----------------------");
				MainFormClass.MainForm.EndSimulation(true);
				return;
			}
			outputWindow.AppendText("\nFound a succesful build dated : "+File.GetLastWriteTime(elfFile)+"\n");
			MainFormClass.MainForm.PercentDone=20;

			// Checking for AVR STUDIO Version and Path
			outputWindow.AppendText("\nChecking for AVR Studio installation and version...\n");
			string avrStudioPath;
			float avrStudioVersion;
			try
			{
				avrStudioPath=Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Atmel\AVRTools").GetValue("StudioPath").ToString();
				avrStudioVersion=float.Parse(((Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Atmel\AVRStudio4").GetSubKeyNames())[0]).Substring(0,4));
			}
			catch
			{
				outputWindow.AppendText("\nError ! Could not find any installed version of AVR Studio 4.10 or better .\n");
				MessageBox.Show("Error ! Could not find any installed version of AVR Studio 4.10 or better.\n","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				outputWindow.AppendText("\n---------------------- Simulation Aborted for Project : "+projectName+" ----------------------");
				MainFormClass.MainForm.EndSimulation(true);
				return;
			}
			if(avrStudioPath.Length==0)
			{
				outputWindow.AppendText("Error ! Could not find any installed version of AVR Studio 4.10 or better .\n");
				MessageBox.Show("Error ! Could not find any installed version of AVR Studio 4.10 or better","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				outputWindow.AppendText("\n---------------------- Simulation Aborted for Project : "+projectName+" ----------------------");
				MainFormClass.MainForm.EndSimulation(true);
				return;
			}
			outputWindow.AppendText("AVR Studio Version "+avrStudioVersion.ToString("0.00")+" found!");
			if(!(avrStudioVersion>4.09f))
			{
				outputWindow.AppendText(" Error ! This feature requires AVR Studio Version 4.10 or higher. Please upgrade your version.\n");
				MessageBox.Show("This feature requires AVR Studio Version 4.10 or higher.Your version is "+avrStudioVersion.ToString("0.00")+" . Please upgrade your version.","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Information);
				outputWindow.AppendText("\n---------------------- Simulation Aborted for Project : "+projectName+" ----------------------");
				MainFormClass.MainForm.EndSimulation(true);
				return;
			}
			
			outputWindow.AppendText(" OK\n");
				
			MainFormClass.MainForm.PercentDone=50;

			#region Creating APS file
			outputWindow.AppendText("\nGenerating simulation file... ");

			string apsFile=projectFolder+"\\"+projectOptions.OutputFolder+"\\"+projectName+".aps";
			
			try
			{
				XmlTextWriter apsFileXmlWriter=new XmlTextWriter(apsFile,null); 

				apsFileXmlWriter.Formatting=Formatting.Indented;  //for xml tags to be indented//
				apsFileXmlWriter.WriteStartDocument();   //Indicates the starting of document
			
				apsFileXmlWriter.WriteStartElement("AVRStudio");			
				
				apsFileXmlWriter.WriteStartElement("MANAGEMENT");
					apsFileXmlWriter.WriteElementString("ProjectName",projectName+"_elf");
					apsFileXmlWriter.WriteElementString("ICON","Object.bmp");
					apsFileXmlWriter.WriteElementString("ProjectType","1");
					apsFileXmlWriter.WriteElementString("Created","");//DateTime.Now.ToString("f"));
					apsFileXmlWriter.WriteElementString("Version","4");
					apsFileXmlWriter.WriteElementString("Build","4,11,0,401");
					apsFileXmlWriter.WriteElementString("ProjectTypeName","");
				apsFileXmlWriter.WriteEndElement();
			
				apsFileXmlWriter.WriteStartElement("CODE_CREATION");
					apsFileXmlWriter.WriteElementString("ObjectFile",elfFile);
					apsFileXmlWriter.WriteElementString("EntryFile","");
					apsFileXmlWriter.WriteElementString("SaveFolder","");
				apsFileXmlWriter.WriteEndElement();
			
				apsFileXmlWriter.WriteStartElement("DEBUG_TARGET");
					apsFileXmlWriter.WriteElementString("CURRENT_TARGET","AVR Simulator");
					apsFileXmlWriter.WriteElementString("CURRENT_PART",projectOptions.MCU.ToString());
					apsFileXmlWriter.WriteElementString("BREAKPOINTS","");
					
					apsFileXmlWriter.WriteStartElement("IO_EXPAND");
						apsFileXmlWriter.WriteElementString("HIDE","false");
					apsFileXmlWriter.WriteEndElement();
			
					apsFileXmlWriter.WriteStartElement("REGISTERNAMES");
					apsFileXmlWriter.WriteEndElement();
			
					apsFileXmlWriter.WriteElementString("COM","Auto");
					apsFileXmlWriter.WriteElementString("COMType","0");
					apsFileXmlWriter.WriteElementString("WATCHNUM","0");
					
					apsFileXmlWriter.WriteStartElement("WATCHNAMES");
					apsFileXmlWriter.WriteEndElement();
			
					apsFileXmlWriter.WriteElementString("BreakOnTraceFull","0");
				apsFileXmlWriter.WriteEndElement();
			
				//review
				/*apsFileXmlWriter.WriteStartElement("Files");
				apsFileXmlWriter.WriteEndElement();

				apsFileXmlWriter.WriteStartElement("Workspace");
				apsFileXmlWriter.WriteEndElement();
			
				apsFileXmlWriter.WriteStartElement("Events");
					apsFileXmlWriter.WriteStartElement("Breakpoints");
					apsFileXmlWriter.WriteEndElement();
					
					apsFileXmlWriter.WriteStartElement("Tracepoints");
					apsFileXmlWriter.WriteEndElement();
					
					apsFileXmlWriter.WriteStartElement("Bookmarks");
					apsFileXmlWriter.WriteEndElement();		
				apsFileXmlWriter.WriteEndElement();

				apsFileXmlWriter.WriteStartElement("Trace");
					apsFileXmlWriter.WriteStartElement("Filters");
					apsFileXmlWriter.WriteEndElement();
				apsFileXmlWriter.WriteEndElement();*/

				apsFileXmlWriter.WriteEndElement();
				apsFileXmlWriter.WriteEndDocument();			
				apsFileXmlWriter.Flush();
				apsFileXmlWriter.Close();

			}
			catch(Exception ex)
			{
				MessageBox.Show("Error! "+ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
				outputWindow.AppendText("Failed\n");
				outputWindow.AppendText("\n---------------------- Simulation Aborted for Project : "+projectName+" ----------------------");
				MainFormClass.MainForm.EndSimulation(true);
				return;
			}
			
			outputWindow.AppendText("Done\n");
			MainFormClass.MainForm.PercentDone=70;

			#endregion
			outputWindow.AppendText("\nStarting AVR Studio... ");

			Process avrStudioProcess=new Process();

			avrStudioProcess.StartInfo.FileName=avrStudioPath+"\\AvrStudio.exe";
			avrStudioProcess.StartInfo.Arguments="\""+apsFile+"\"";
			avrStudioProcess.StartInfo.WorkingDirectory=avrStudioPath;
			avrStudioProcess.StartInfo.RedirectStandardError=false;
			avrStudioProcess.StartInfo.RedirectStandardOutput=false;
			avrStudioProcess.StartInfo.CreateNoWindow=false;
			avrStudioProcess.StartInfo.UseShellExecute=false;		
			
			try
			{
				avrStudioProcess.Start();
			}
			catch
			{
				outputWindow.AppendText("Failed\n\nMake sure that the AVR Studio is installed correctly. If the problem persists reinstall AVR Studio.\n");
				outputWindow.AppendText("\n---------------------- Simulation Aborted for Project : "+projectName+" ----------------------");
				MainFormClass.MainForm.EndSimulation(true);
				return;
			}
			MainFormClass.MainForm.PercentDone=100;
			outputWindow.AppendText("Started\n");
			outputWindow.AppendText("\n---------------------- Simulator Started for Project : "+projectName+" ----------------------\n");
			MainFormClass.MainForm.EndSimulation(false);		
		}
		
		/// <summary>
		/// Resets the target Board
		/// </summary>
		public void ResetDevice()
		{
			try
			{
				int portNumber=int.Parse(projectOptions.ProgrammingPort.Substring(3));
				serialPort.Open(portNumber,115200,8,serialportcomm.RS232.DataParity.Parity_None,serialportcomm.RS232.DataStopBit.StopBit_1,512);
				serialPort.Dtr=false;
				serialPort.Rts=true;
				serialPort.Rts=false;
				serialPort.Dtr=true;
				serialPort.Close();	
			}
			catch (System.Exception ex)
			{
				MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Warning);
				if(serialPort.IsOpen)
					serialPort.Close();
			}
		}
			
		#endregion

		#region Project Tree Event Handlers
		
		/// <summary>
		/// Called after a project tree's node is renamed.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void projectFilesTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
		{
			projectFilesTree.LabelEdit=false;
			if(((ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.ProjectName)
			{
				if(e.Label==null)
				{
					e.CancelEdit=true;
					return;
				}
				if(projectType==ProjectTypes.MegaBoard_v1) 
				{ 
					
					if(projectType==ProjectTypes.MegaBoard_v1)
					{
						string newLabel=e.Label.Trim();	
						fileSystemWatcher.EnableRaisingEvents=false;
						if(System.IO.File.Exists(projectFolder+"\\"+newLabel+".mbp"))
						{
							if(MessageBox.Show(projectFolder+"\\"+newLabel+".mbp"+" already exists.\nDo you want to replace it ?","MegaIDE",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.No)
								return;
							try
							{
								File.Delete(projectFolder+"\\"+newLabel+".mbp");
							}
							catch(Exception ex)
							{
								MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
								e.CancelEdit=true;
								fileSystemWatcher.EnableRaisingEvents=true;
								return;
							}
						}
						try
						{
							projectFileStream.Close();
							File.Move(projectFileName,Path.GetFullPath(projectFolder+"\\"+newLabel+".mbp"));
							RecentDocuments.RemoveFromRecentProjects(projectFileName);
							fileSystemWatcher.EnableRaisingEvents=true;
						}
						catch(Exception ex)
						{
							MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
							e.CancelEdit=true;
							fileSystemWatcher.EnableRaisingEvents=true;
							return;
						}
						projectFileName=Path.GetFullPath(projectFolder+"\\"+newLabel+".mbp");
						projectName=newLabel;
						projectFileStream=new FileStream(projectFileName,FileMode.Open);
						projectFileStream.Lock(0,projectFileStream.Length);
						projectFilesTree.SelectedNode.Text=newLabel;
						RecentDocuments.AddToRecentProjects(projectFileName);
						MainFormClass.MainForm.UpdateRecentDocumentsList();
						MainFormClass.MainForm.Text="MegaIDE - "+projectName+".mbp";
						SaveProject();
						e.CancelEdit=true;
						return;
					}
				}
			}
			else if(((ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.File)
			{
				if(e.Label==null)
				{
					e.CancelEdit=true;
					return;
				}
				string newLabel=e.Label.Trim();
				string sourceFileName=Path.GetDirectoryName(GetAbsolutePath(projectFilesTree.SelectedNode))+"\\"+projectFilesTree.SelectedNode.Text;
				string destFileName=Path.GetDirectoryName(GetAbsolutePath(projectFilesTree.SelectedNode))+"\\"+newLabel;
				
				if(Path.GetExtension(projectFilesTree.SelectedNode.Text).ToLower() != Path.GetExtension(newLabel).ToLower())
					if(MessageBox.Show("Changing the extension of a file may cause errors !\nWould you like to continue ?","MegaIDE",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.No)
					{
						e.CancelEdit=true;
						return; 
					}
				fileSystemWatcher.EnableRaisingEvents=false;
				if(System.IO.File.Exists(destFileName) && destFileName.ToLower() != sourceFileName.ToLower())
				{
					if(MessageBox.Show(destFileName+" already exists.\nDo you want to replace it ?","MegaIDE",MessageBoxButtons.YesNo,MessageBoxIcon.Warning)==DialogResult.No)
						return;
					try
					{
						File.Delete(destFileName);
					}
					catch(Exception ex)
					{
						MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
						e.CancelEdit=true;
						fileSystemWatcher.EnableRaisingEvents=true;
						return;
					}
				}
				try
				{
					File.Move(sourceFileName,destFileName);
					fileSystemWatcher.EnableRaisingEvents=true;
				}
				catch(Exception ex)
				{
					MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
					e.CancelEdit=true;
					fileSystemWatcher.EnableRaisingEvents=true;
					return;
				}
				MainFormClass.MainForm.RenameTab(sourceFileName,destFileName);
				projectFilesTree.SelectedNode.Text=newLabel;
				RecentDocuments.RemoveFromRecentFiles(sourceFileName);
				RecentDocuments.AddToRecentFiles(destFileName);
				MainFormClass.MainForm.UpdateRecentDocumentsList();
				SaveProject();
				e.CancelEdit=true;
				return;
			}
			else if(((ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.Directory)
			{
				if(e.Label==null)
				{
					e.CancelEdit=true;
					return;
				}
				string newLabel=e.Label.Trim();
				string sourceDirName=Path.GetDirectoryName(GetAbsolutePath(projectFilesTree.SelectedNode))+"\\"+projectFilesTree.SelectedNode.Text;
				string destDirName=Path.GetDirectoryName(GetAbsolutePath(projectFilesTree.SelectedNode))+"\\"+newLabel;
				
				if(Directory.Exists(destDirName))
				{
					MessageBox.Show("Destination folder already exists. Connot rename the folder.","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
					e.CancelEdit=true;
					fileSystemWatcher.EnableRaisingEvents=true;
					return;
				}

				if(sourceDirName.ToLower() != destDirName.ToLower())
				{
					try
					{
						fileSystemWatcher.EnableRaisingEvents=false;
						Directory.Move(sourceDirName,destDirName);
						fileSystemWatcher.EnableRaisingEvents=true;
					}
					catch(Exception ex)
					{
						MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
						e.CancelEdit=true;
						fileSystemWatcher.EnableRaisingEvents=true;
						return;
					}
					MainFormClass.MainForm.CloseOpenTabs(sourceDirName);
					projectFilesTree.SelectedNode.Text=newLabel;
				}
				SaveProject();
				e.CancelEdit=true;
			}
		}
			
		private void projectFilesTree_DoubleClick(object sender, EventArgs e)
		{
			if(((MegaIDE.ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.File)
			{
				MainFormClass.MainForm.OpenFile(GetAbsolutePath(projectFilesTree.SelectedNode));
			}
			else if(((MegaIDE.ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.Directory)
			{
				System.Diagnostics.Process.Start("explorer",GetAbsolutePath(projectFilesTree.SelectedNode));
			}
			else if(((MegaIDE.ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.ProjectName)
			{
				System.Diagnostics.Process.Start("explorer",projectFolder);
			}
			projectFilesTree.SelectedNode.Expand();
			isProjectModified=true;
		}
		
		#endregion

		#region Filesystem Watcher Event Handlers
		/// <summary>
		/// Called when a file is deleted from the project folder.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fileSystemWatcher_Deleted(object sender, System.IO.FileSystemEventArgs e)
		{
			if(GetFileNode(e.FullPath) != null)
			{
				projectFilesTree.SelectedNode=GetFileNode(e.FullPath);
				projectFilesTree.SelectedNode.Nodes.Clear();
				projectFilesTree.SelectedNode.Remove();
				projectFilesTree.Update();
				isProjectModified=true;
				MessageBox.Show(MainFormClass.MainForm,e.FullPath+" has been deleted !","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Warning);
		
				// Called this way bcoz the watcher runs on a seperate thread.
				MainFormClass.MainForm.Invoke(new StringDelegate(MainFormClass.MainForm.CloseOpenTabs),new object[]{e.FullPath});	
			}	
		}
		
		/// <summary>
		/// Called when a file is renamed in the project folder.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void fileSystemWatcher_Renamed(object sender, System.IO.RenamedEventArgs e)
		{
			if(GetFileNode(e.FullPath) != null)
			{
				projectFilesTree.SelectedNode=GetFileNode(e.OldFullPath);
				projectFilesTree.SelectedNode.Text=e.Name;
				projectFilesTree.Update();
				isProjectModified=true;

				MessageBox.Show(MainFormClass.MainForm,e.OldFullPath+" has been renamed from outside !","MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Warning);
		
				// Called this way bcoz the watcher runs on a seperate thread.
				MainFormClass.MainForm.Invoke(new StringDelegate(MainFormClass.MainForm.CloseOpenTabs),new object[]{e.OldFullPath});		
			}		
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
				if(components  !=  null)
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(ProjectManagerClass));
			this.settingsPages = new NETXP.Controls.MultiPage();
			this.libraryOptions = new System.Windows.Forms.TabPage();
			this.selectButton = new System.Windows.Forms.Button();
			this.deselectButton = new System.Windows.Forms.Button();
			this.selectAllButton = new System.Windows.Forms.Button();
			this.deselectAllButton = new System.Windows.Forms.Button();
			this.selectedLibrariesLabel = new System.Windows.Forms.Label();
			this.availableLibrariesLabel = new System.Windows.Forms.Label();
			this.page1TitleLabel = new System.Windows.Forms.Label();
			this.selectedLibList = new System.Windows.Forms.ListBox();
			this.availableLibList = new System.Windows.Forms.ListBox();
			this.advancedOptions = new System.Windows.Forms.TabPage();
			this.advancedOptionsPropertyGrid = new System.Windows.Forms.PropertyGrid();
			this.optionsTree = new System.Windows.Forms.TreeView();
			this.treeImageList = new System.Windows.Forms.ImageList(this.components);
			this.groupBox = new System.Windows.Forms.GroupBox();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.fileSystemWatcher = new System.IO.FileSystemWatcher();
			this.contextMenuImageExtender = new NETXP.Components.Extenders.MenuImageExtender(this.components);
			this.menuOpenProject = new System.Windows.Forms.MenuItem();
			this.menuSaveProject = new System.Windows.Forms.MenuItem();
			this.menuAddExistingFile = new System.Windows.Forms.MenuItem();
			this.menuAddNewFile = new System.Windows.Forms.MenuItem();
			this.menuOpen = new System.Windows.Forms.MenuItem();
			this.menuRename = new System.Windows.Forms.MenuItem();
			this.menuBuildProject = new System.Windows.Forms.MenuItem();
			this.menuProgramProject = new System.Windows.Forms.MenuItem();
			this.menuSimulateProject = new System.Windows.Forms.MenuItem();
			this.menuExcludeFromProject = new System.Windows.Forms.MenuItem();
			this.menuDelete = new System.Windows.Forms.MenuItem();
			this.menuProjectOptions = new System.Windows.Forms.MenuItem();
			this.menuCloseProject = new System.Windows.Forms.MenuItem();
			this.menuSaveFile = new System.Windows.Forms.MenuItem();
			this.menuAddNewFolder = new System.Windows.Forms.MenuItem();
			this.menuNewProject = new System.Windows.Forms.MenuItem();
			this.menuSaveFileAs = new System.Windows.Forms.MenuItem();
			this.projectContextMenu = new System.Windows.Forms.ContextMenu();
			this.settingsPages.SuspendLayout();
			this.libraryOptions.SuspendLayout();
			this.advancedOptions.SuspendLayout();
			this.groupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).BeginInit();
			this.SuspendLayout();
			// 
			// settingsPages
			// 
			this.settingsPages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.settingsPages.Controls.Add(this.libraryOptions);
			this.settingsPages.Controls.Add(this.advancedOptions);
			this.settingsPages.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.settingsPages.Location = new System.Drawing.Point(160, 12);
			this.settingsPages.Name = "settingsPages";
			this.settingsPages.SelectedIndex = 0;
			this.settingsPages.Size = new System.Drawing.Size(408, 304);
			this.settingsPages.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
			this.settingsPages.TabIndex = 10;
			this.settingsPages.TabStop = false;
			// 
			// libraryOptions
			// 
			this.libraryOptions.Controls.Add(this.selectButton);
			this.libraryOptions.Controls.Add(this.deselectButton);
			this.libraryOptions.Controls.Add(this.selectAllButton);
			this.libraryOptions.Controls.Add(this.deselectAllButton);
			this.libraryOptions.Controls.Add(this.selectedLibrariesLabel);
			this.libraryOptions.Controls.Add(this.availableLibrariesLabel);
			this.libraryOptions.Controls.Add(this.page1TitleLabel);
			this.libraryOptions.Controls.Add(this.selectedLibList);
			this.libraryOptions.Controls.Add(this.availableLibList);
			this.libraryOptions.Location = new System.Drawing.Point(4, 22);
			this.libraryOptions.Name = "libraryOptions";
			this.libraryOptions.Size = new System.Drawing.Size(400, 278);
			this.libraryOptions.TabIndex = 0;
			this.libraryOptions.Resize += new System.EventHandler(this.libraryOptions_Resize);
			this.libraryOptions.Paint += new System.Windows.Forms.PaintEventHandler(this.libraryOptions_Paint);
			// 
			// selectButton
			// 
			this.selectButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.selectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.selectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.selectButton.Location = new System.Drawing.Point(180, 60);
			this.selectButton.Name = "selectButton";
			this.selectButton.Size = new System.Drawing.Size(40, 23);
			this.selectButton.TabIndex = 3;
			this.selectButton.Text = ">";
			this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
			// 
			// deselectButton
			// 
			this.deselectButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.deselectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.deselectButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.deselectButton.Location = new System.Drawing.Point(180, 100);
			this.deselectButton.Name = "deselectButton";
			this.deselectButton.Size = new System.Drawing.Size(40, 23);
			this.deselectButton.TabIndex = 4;
			this.deselectButton.Text = "<";
			this.deselectButton.Click += new System.EventHandler(this.deselectButton_Click);
			// 
			// selectAllButton
			// 
			this.selectAllButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.selectAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.selectAllButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.selectAllButton.Location = new System.Drawing.Point(180, 140);
			this.selectAllButton.Name = "selectAllButton";
			this.selectAllButton.Size = new System.Drawing.Size(40, 23);
			this.selectAllButton.TabIndex = 5;
			this.selectAllButton.Text = ">>";
			this.selectAllButton.Click += new System.EventHandler(this.selectAllButton_Click);
			// 
			// deselectAllButton
			// 
			this.deselectAllButton.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.deselectAllButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.deselectAllButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.deselectAllButton.Location = new System.Drawing.Point(180, 184);
			this.deselectAllButton.Name = "deselectAllButton";
			this.deselectAllButton.Size = new System.Drawing.Size(40, 23);
			this.deselectAllButton.TabIndex = 6;
			this.deselectAllButton.Text = "<<";
			this.deselectAllButton.Click += new System.EventHandler(this.deselectAllButton_Click);
			// 
			// selectedLibrariesLabel
			// 
			this.selectedLibrariesLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.selectedLibrariesLabel.Location = new System.Drawing.Point(240, 24);
			this.selectedLibrariesLabel.Name = "selectedLibrariesLabel";
			this.selectedLibrariesLabel.Size = new System.Drawing.Size(112, 16);
			this.selectedLibrariesLabel.TabIndex = 71;
			this.selectedLibrariesLabel.Text = "Selected Libraries :";
			// 
			// availableLibrariesLabel
			// 
			this.availableLibrariesLabel.Location = new System.Drawing.Point(8, 24);
			this.availableLibrariesLabel.Name = "availableLibrariesLabel";
			this.availableLibrariesLabel.Size = new System.Drawing.Size(112, 16);
			this.availableLibrariesLabel.TabIndex = 70;
			this.availableLibrariesLabel.Text = "Available Libraries :";
			// 
			// page1TitleLabel
			// 
			this.page1TitleLabel.Location = new System.Drawing.Point(8, 0);
			this.page1TitleLabel.Name = "page1TitleLabel";
			this.page1TitleLabel.Size = new System.Drawing.Size(304, 16);
			this.page1TitleLabel.TabIndex = 69;
			this.page1TitleLabel.Text = "Please select the libraries you want to use in the project .";
			// 
			// selectedLibList
			// 
			this.selectedLibList.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.selectedLibList.Location = new System.Drawing.Point(240, 44);
			this.selectedLibList.Name = "selectedLibList";
			this.selectedLibList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.selectedLibList.Size = new System.Drawing.Size(154, 173);
			this.selectedLibList.TabIndex = 7;
			this.selectedLibList.SelectedIndexChanged += new System.EventHandler(this.selectedLibList_SelectedIndexChanged);
			// 
			// availableLibList
			// 
			this.availableLibList.Location = new System.Drawing.Point(8, 44);
			this.availableLibList.Name = "availableLibList";
			this.availableLibList.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
			this.availableLibList.Size = new System.Drawing.Size(154, 173);
			this.availableLibList.TabIndex = 2;
			this.availableLibList.SelectedIndexChanged += new System.EventHandler(this.availableLibList_SelectedIndexChanged);
			// 
			// advancedOptions
			// 
			this.advancedOptions.Controls.Add(this.advancedOptionsPropertyGrid);
			this.advancedOptions.Location = new System.Drawing.Point(4, 22);
			this.advancedOptions.Name = "advancedOptions";
			this.advancedOptions.Size = new System.Drawing.Size(400, 278);
			this.advancedOptions.TabIndex = 1;
			// 
			// advancedOptionsPropertyGrid
			// 
			this.advancedOptionsPropertyGrid.CommandsVisibleIfAvailable = true;
			this.advancedOptionsPropertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.advancedOptionsPropertyGrid.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.advancedOptionsPropertyGrid.LargeButtons = false;
			this.advancedOptionsPropertyGrid.LineColor = System.Drawing.SystemColors.ScrollBar;
			this.advancedOptionsPropertyGrid.Location = new System.Drawing.Point(0, 0);
			this.advancedOptionsPropertyGrid.Name = "advancedOptionsPropertyGrid";
			this.advancedOptionsPropertyGrid.Size = new System.Drawing.Size(400, 278);
			this.advancedOptionsPropertyGrid.TabIndex = 0;
			this.advancedOptionsPropertyGrid.Text = "PropertyGrid";
			this.advancedOptionsPropertyGrid.ToolbarVisible = false;
			this.advancedOptionsPropertyGrid.ViewBackColor = System.Drawing.SystemColors.Window;
			this.advancedOptionsPropertyGrid.ViewForeColor = System.Drawing.SystemColors.WindowText;
			// 
			// optionsTree
			// 
			this.optionsTree.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Left)));
			this.optionsTree.HideSelection = false;
			this.optionsTree.ImageList = this.treeImageList;
			this.optionsTree.Location = new System.Drawing.Point(8, 16);
			this.optionsTree.Name = "optionsTree";
			this.optionsTree.Scrollable = false;
			this.optionsTree.ShowLines = false;
			this.optionsTree.ShowPlusMinus = false;
			this.optionsTree.ShowRootLines = false;
			this.optionsTree.Size = new System.Drawing.Size(144, 296);
			this.optionsTree.TabIndex = 1;
			this.optionsTree.MouseDown += new System.Windows.Forms.MouseEventHandler(this.optionsTree_MouseDown);
			// 
			// treeImageList
			// 
			this.treeImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.treeImageList.ImageSize = new System.Drawing.Size(16, 16);
			this.treeImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("treeImageList.ImageStream")));
			this.treeImageList.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// groupBox
			// 
			this.groupBox.Controls.Add(this.cancelButton);
			this.groupBox.Controls.Add(this.okButton);
			this.groupBox.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.groupBox.Location = new System.Drawing.Point(0, 320);
			this.groupBox.Name = "groupBox";
			this.groupBox.Size = new System.Drawing.Size(574, 48);
			this.groupBox.TabIndex = 2;
			this.groupBox.TabStop = false;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.cancelButton.Location = new System.Drawing.Point(488, 16);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.TabIndex = 9;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
				| System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.okButton.Location = new System.Drawing.Point(400, 16);
			this.okButton.Name = "okButton";
			this.okButton.TabIndex = 8;
			this.okButton.Text = "OK";
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// fileSystemWatcher
			// 
			this.fileSystemWatcher.EnableRaisingEvents = true;
			this.fileSystemWatcher.IncludeSubdirectories = true;
			this.fileSystemWatcher.SynchronizingObject = this;
			this.fileSystemWatcher.Deleted += new System.IO.FileSystemEventHandler(this.fileSystemWatcher_Deleted);
			this.fileSystemWatcher.Renamed += new System.IO.RenamedEventHandler(this.fileSystemWatcher_Renamed);
			// 
			// menuOpenProject
			// 
			this.contextMenuImageExtender.SetImage(this.menuOpenProject, ((System.Drawing.Image)(resources.GetObject("menuOpenProject.Image"))));
			this.menuOpenProject.Index = 1;
			this.menuOpenProject.OwnerDraw = true;
			this.menuOpenProject.Text = "Open Project...";
			this.menuOpenProject.Click += new System.EventHandler(this.menuOpenProject_Click);
			// 
			// menuSaveProject
			// 
			this.contextMenuImageExtender.SetImage(this.menuSaveProject, ((System.Drawing.Image)(resources.GetObject("menuSaveProject.Image"))));
			this.menuSaveProject.Index = 2;
			this.menuSaveProject.OwnerDraw = true;
			this.menuSaveProject.Text = "Save Project";
			this.menuSaveProject.Click += new System.EventHandler(this.menuSaveProject_Click);
			// 
			// menuAddExistingFile
			// 
			this.contextMenuImageExtender.SetImage(this.menuAddExistingFile, ((System.Drawing.Image)(resources.GetObject("menuAddExistingFile.Image"))));
			this.menuAddExistingFile.Index = 4;
			this.menuAddExistingFile.OwnerDraw = true;
			this.menuAddExistingFile.Text = "Add Existing File(s)...";
			this.menuAddExistingFile.Click += new System.EventHandler(this.menuAddExistingFile_Click);
			// 
			// menuAddNewFile
			// 
			this.contextMenuImageExtender.SetImage(this.menuAddNewFile, ((System.Drawing.Image)(resources.GetObject("menuAddNewFile.Image"))));
			this.menuAddNewFile.Index = 5;
			this.menuAddNewFile.OwnerDraw = true;
			this.menuAddNewFile.Text = "Add New File...";
			this.menuAddNewFile.Click += new System.EventHandler(this.menuAddNewFile_Click);
			// 
			// menuOpen
			// 
			this.contextMenuImageExtender.SetImage(this.menuOpen, ((System.Drawing.Image)(resources.GetObject("menuOpen.Image"))));
			this.menuOpen.Index = 7;
			this.menuOpen.OwnerDraw = true;
			this.menuOpen.Text = "Open";
			this.menuOpen.Click += new System.EventHandler(this.projectFilesTree_DoubleClick);
			// 
			// menuRename
			// 
			this.contextMenuImageExtender.SetImage(this.menuRename, ((System.Drawing.Image)(resources.GetObject("menuRename.Image"))));
			this.menuRename.Index = 10;
			this.menuRename.OwnerDraw = true;
			this.menuRename.Text = "Rename";
			this.menuRename.Click += new System.EventHandler(this.menuRename_Click);
			// 
			// menuBuildProject
			// 
			this.contextMenuImageExtender.SetImage(this.menuBuildProject, ((System.Drawing.Image)(resources.GetObject("menuBuildProject.Image"))));
			this.menuBuildProject.Index = 13;
			this.menuBuildProject.OwnerDraw = true;
			this.menuBuildProject.Text = "Build";
			this.menuBuildProject.Click += new System.EventHandler(this.menuBuildProject_Click);
			// 
			// menuProgramProject
			// 
			this.contextMenuImageExtender.SetImage(this.menuProgramProject, ((System.Drawing.Image)(resources.GetObject("menuProgramProject.Image"))));
			this.menuProgramProject.Index = 14;
			this.menuProgramProject.OwnerDraw = true;
			this.menuProgramProject.Text = "Program";
			this.menuProgramProject.Click += new System.EventHandler(this.menuProgramProject_Click);
			// 
			// menuSimulateProject
			// 
			this.contextMenuImageExtender.SetImage(this.menuSimulateProject, ((System.Drawing.Image)(resources.GetObject("menuSimulateProject.Image"))));
			this.menuSimulateProject.Index = 15;
			this.menuSimulateProject.OwnerDraw = true;
			this.menuSimulateProject.Text = "Simulate";
			this.menuSimulateProject.Click += new System.EventHandler(this.menuSimulateProject_Click);
			// 
			// menuExcludeFromProject
			// 
			this.contextMenuImageExtender.SetImage(this.menuExcludeFromProject, null);
			this.menuExcludeFromProject.Index = 12;
			this.menuExcludeFromProject.OwnerDraw = true;
			this.menuExcludeFromProject.Text = "Exclude From Project";
			this.menuExcludeFromProject.Click += new System.EventHandler(this.menuExcludeFromProject_Click);
			// 
			// menuDelete
			// 
			this.contextMenuImageExtender.SetImage(this.menuDelete, ((System.Drawing.Image)(resources.GetObject("menuDelete.Image"))));
			this.menuDelete.Index = 11;
			this.menuDelete.OwnerDraw = true;
			this.menuDelete.Text = "Delete";
			this.menuDelete.Click += new System.EventHandler(this.menuDelete_Click);
			// 
			// menuProjectOptions
			// 
			this.contextMenuImageExtender.SetImage(this.menuProjectOptions, ((System.Drawing.Image)(resources.GetObject("menuProjectOptions.Image"))));
			this.menuProjectOptions.Index = 16;
			this.menuProjectOptions.OwnerDraw = true;
			this.menuProjectOptions.Text = "Project Options...";
			this.menuProjectOptions.Click += new System.EventHandler(this.menuProjectOptions_Click);
			// 
			// menuCloseProject
			// 
			this.contextMenuImageExtender.SetImage(this.menuCloseProject, ((System.Drawing.Image)(resources.GetObject("menuCloseProject.Image"))));
			this.menuCloseProject.Index = 3;
			this.menuCloseProject.OwnerDraw = true;
			this.menuCloseProject.Text = "Close Project";
			this.menuCloseProject.Click += new System.EventHandler(this.menuCloseProject_Click);
			// 
			// menuSaveFile
			// 
			this.contextMenuImageExtender.SetImage(this.menuSaveFile, ((System.Drawing.Image)(resources.GetObject("menuSaveFile.Image"))));
			this.menuSaveFile.Index = 8;
			this.menuSaveFile.OwnerDraw = true;
			this.menuSaveFile.Text = "Save File";
			this.menuSaveFile.Click += new System.EventHandler(this.menuSaveFile_Click);
			// 
			// menuAddNewFolder
			// 
			this.contextMenuImageExtender.SetImage(this.menuAddNewFolder, ((System.Drawing.Image)(resources.GetObject("menuAddNewFolder.Image"))));
			this.menuAddNewFolder.Index = 6;
			this.menuAddNewFolder.OwnerDraw = true;
			this.menuAddNewFolder.Text = "Add New Folder";
			this.menuAddNewFolder.Click += new System.EventHandler(this.menuAddNewFolder_Click);
			// 
			// menuNewProject
			// 
			this.contextMenuImageExtender.SetImage(this.menuNewProject, ((System.Drawing.Image)(resources.GetObject("menuNewProject.Image"))));
			this.menuNewProject.Index = 0;
			this.menuNewProject.OwnerDraw = true;
			this.menuNewProject.Text = "New Project...";
			this.menuNewProject.Click += new System.EventHandler(this.menuNewProject_Click);
			// 
			// menuSaveFileAs
			// 
			this.contextMenuImageExtender.SetImage(this.menuSaveFileAs, null);
			this.menuSaveFileAs.Index = 9;
			this.menuSaveFileAs.OwnerDraw = true;
			this.menuSaveFileAs.Text = "Save File As...";
			this.menuSaveFileAs.Click += new System.EventHandler(this.menuSaveFileAs_Click);
			// 
			// projectContextMenu
			// 
			this.projectContextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																							   this.menuNewProject,
																							   this.menuOpenProject,
																							   this.menuSaveProject,
																							   this.menuCloseProject,
																							   this.menuAddExistingFile,
																							   this.menuAddNewFile,
																							   this.menuAddNewFolder,
																							   this.menuOpen,
																							   this.menuSaveFile,
																							   this.menuSaveFileAs,
																							   this.menuRename,
																							   this.menuDelete,
																							   this.menuExcludeFromProject,
																							   this.menuBuildProject,
																							   this.menuProgramProject,
																							   this.menuSimulateProject,
																							   this.menuProjectOptions});
			this.projectContextMenu.Popup += new System.EventHandler(this.projectContextMenu_Popup);
			// 
			// ProjectManagerClass
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(574, 368);
			this.Controls.Add(this.groupBox);
			this.Controls.Add(this.optionsTree);
			this.Controls.Add(this.settingsPages);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ProjectManagerClass";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "ProjectOptions";
			this.settingsPages.ResumeLayout(false);
			this.libraryOptions.ResumeLayout(false);
			this.advancedOptions.ResumeLayout(false);
			this.groupBox.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		#region Project Context Menu Event Handlers
		
		private void projectContextMenu_Popup(object sender, EventArgs e)
		{
			projectContextMenu.MenuItems.Clear();
			if(projectFilesTree.Nodes.Count != 0)
			{
				if(((ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.ProjectType)
				{
					projectContextMenu.MenuItems.Add(menuNewProject);
					projectContextMenu.MenuItems.Add(menuOpenProject);
					projectContextMenu.MenuItems.Add(menuSaveProject);
					projectContextMenu.MenuItems.Add(menuCloseProject);
					projectContextMenu.MenuItems.Add(new MenuItem("-"));
					projectContextMenu.MenuItems.Add(menuBuildProject);
					projectContextMenu.MenuItems.Add(menuProgramProject);
					projectContextMenu.MenuItems.Add(menuSimulateProject);
					projectContextMenu.MenuItems.Add(new MenuItem("-"));
					projectContextMenu.MenuItems.Add(menuProjectOptions);
				}
				else if(((ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.ProjectName)
				{
					projectContextMenu.MenuItems.Add(menuNewProject);
					projectContextMenu.MenuItems.Add(menuOpenProject);
					projectContextMenu.MenuItems.Add(menuSaveProject);
					projectContextMenu.MenuItems.Add(menuCloseProject);
					projectContextMenu.MenuItems.Add(new MenuItem("-"));
					projectContextMenu.MenuItems.Add(menuRename);
					projectContextMenu.MenuItems.Add(new MenuItem("-"));
					projectContextMenu.MenuItems.Add(menuOpen);
					projectContextMenu.MenuItems.Add(menuAddExistingFile);
					projectContextMenu.MenuItems.Add(menuAddNewFile);
					projectContextMenu.MenuItems.Add(menuAddNewFolder);


					projectContextMenu.MenuItems.Add(new MenuItem("-"));
					projectContextMenu.MenuItems.Add(menuBuildProject);
					projectContextMenu.MenuItems.Add(menuProgramProject);
					projectContextMenu.MenuItems.Add(menuSimulateProject);
					projectContextMenu.MenuItems.Add(new MenuItem("-"));
					projectContextMenu.MenuItems.Add(menuProjectOptions);
				} 
				else if(((ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.Library)
				{
					projectContextMenu.MenuItems.Add(menuProjectOptions);
				}
				else if(((ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.LibraryItem)
				{
					projectContextMenu.MenuItems.Add(menuProjectOptions);
				} 
				else if(((ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.File)
				{
					projectContextMenu.MenuItems.Add(menuOpen);
					projectContextMenu.MenuItems.Add(new MenuItem("-"));
					projectContextMenu.MenuItems.Add(menuRename);	
					projectContextMenu.MenuItems.Add(menuSaveFile);
					projectContextMenu.MenuItems.Add(menuSaveFileAs);
					projectContextMenu.MenuItems.Add(new MenuItem("-"));
					projectContextMenu.MenuItems.Add(menuDelete);
					projectContextMenu.MenuItems.Add(menuExcludeFromProject);

				} 
				else if(((ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.Directory)
				{
			
					projectContextMenu.MenuItems.Add(menuOpen);
					projectContextMenu.MenuItems.Add(menuAddExistingFile);
					projectContextMenu.MenuItems.Add(menuAddNewFile);
					projectContextMenu.MenuItems.Add(menuAddNewFolder);


					projectContextMenu.MenuItems.Add(new MenuItem("-"));
					projectContextMenu.MenuItems.Add(menuRename);	
					projectContextMenu.MenuItems.Add(menuDelete);
					projectContextMenu.MenuItems.Add(menuExcludeFromProject);
				} 
			}
		}
		
		private void menuCloseProject_Click(object sender, System.EventArgs e)
		{
			CloseProject();
		}

		private void menuOpenProject_Click(object sender, System.EventArgs e)
		{
			MainFormClass.MainForm.fileOpenProject_Click(null,null);	
		}

		private void menuNewProject_Click(object sender, System.EventArgs e)
		{
			NewProject();	
		}

		private void menuSaveProject_Click(object sender, System.EventArgs e)
		{
			SaveProject();
		}

		private void menuAddExistingFile_Click(object sender, System.EventArgs e)
		{
			MainFormClass.MainForm.projectAddExistingFile_Click(null,null);
		}

		/// <summary>
		/// Adds a new file to the project.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void menuAddNewFile_Click(object sender, System.EventArgs e)
		{
			if(NewDocumentDialogClass.NewDocumentDialog.ShowAddFileProjectDialog(this,GetAbsolutePath(projectFilesTree.SelectedNode))==DialogResult.OK)
			{
				AddNewFileToProject(NewDocumentDialogClass.NewDocumentDialog.FullFileName,projectFilesTree.SelectedNode);
				projectFilesTree.SelectedNode.Expand();
				isProjectModified=true;
			}	
		}
		
		/// <summary>
		/// Adds a new folder to the project.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void menuAddNewFolder_Click(object sender, System.EventArgs e)
		{
			string folderName="New Folder";
			string path=GetAbsolutePath(projectFilesTree.SelectedNode)+"\\";
			if(Directory.Exists(path+folderName) || File.Exists(path+folderName))
			{
				int i=0;
				while(Directory.Exists(path+folderName+(++i).ToString())||File.Exists(path+folderName+(i).ToString()));
				folderName=folderName+i.ToString();
			}
			try
			{
				Directory.CreateDirectory(path+folderName);
			}
			catch(Exception ex)
			{
				MessageBox.Show("Error creating directory. "+ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			TreeNode folderNode=new TreeNode(folderName,3,3);
			folderNode.Tag=MegaIDE.ProjectTreeItemTypes.Directory;
			projectFilesTree.SelectedNode.Nodes.Add(folderNode);
			projectFilesTree.SelectedNode.Expand();
			isProjectModified=true;
		}

		private void menuSaveFile_Click(object sender, System.EventArgs e)
		{
			MainFormClass.MainForm.SaveFile(ProjectManager.GetAbsolutePath(projectFilesTree.SelectedNode));
		}
		
		private void menuSaveFileAs_Click(object sender, System.EventArgs e)
		{
			MainFormClass.MainForm.SaveFileAs(GetAbsolutePath(projectFilesTree.SelectedNode));
		}

		private void menuRename_Click(object sender, System.EventArgs e)
		{
			projectFilesTree.LabelEdit=true;
			projectFilesTree.SelectedNode.BeginEdit();
		}

		private void menuDelete_Click(object sender, System.EventArgs e)
		{
			if(((ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.Directory)
			{
				string directoryPath=GetAbsolutePath(projectFilesTree.SelectedNode);
				if(MessageBox.Show("\""+projectFilesTree.SelectedNode.Text+"\" and all its contents will be deleted permanently !","MegaIDE",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning)==DialogResult.OK)
				{
					if(!Directory.Exists(directoryPath))
					{
						MessageBox.Show(GetAbsolutePath(projectFilesTree.SelectedNode)+" does not exist !" ,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
						projectFilesTree.SelectedNode.Remove();
						return;
					}
					try
					{
						fileSystemWatcher.EnableRaisingEvents=false;
						Directory.Delete(directoryPath,true);
						fileSystemWatcher.EnableRaisingEvents=true;
					}
					catch(Exception ex)
					{
						MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
						fileSystemWatcher.EnableRaisingEvents=true;
						return;
					}
					projectFilesTree.SelectedNode.Parent.Nodes.Remove(projectFilesTree.SelectedNode);
					SaveProject();
					MainFormClass.MainForm.CloseOpenTabs(directoryPath);
				}

			}
			else if(((ProjectTreeItemTypes)projectFilesTree.SelectedNode.Tag)==ProjectTreeItemTypes.File)
			{
				if(MessageBox.Show("\""+projectFilesTree.SelectedNode.Text+"\" will be deleted permanently !","MegaIDE",MessageBoxButtons.OKCancel,MessageBoxIcon.Warning)==DialogResult.OK)
				{
					if(!File.Exists(GetAbsolutePath(projectFilesTree.SelectedNode)))
					{
						
						MessageBox.Show(GetAbsolutePath(projectFilesTree.SelectedNode)+" does not exist !" ,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
						projectFilesTree.SelectedNode.Remove();
						return;
					}
					try
					{
						fileSystemWatcher.EnableRaisingEvents=false;
						File.Delete(GetAbsolutePath(projectFilesTree.SelectedNode));
						fileSystemWatcher.EnableRaisingEvents=true;
					}
					catch(Exception ex)
					{
						MessageBox.Show(ex.Message,"MegaIDE",MessageBoxButtons.OK,MessageBoxIcon.Error);
						fileSystemWatcher.EnableRaisingEvents=true;
						return;
					}
					MainFormClass.MainForm.CloseOpenTab(GetAbsolutePath(projectFilesTree.SelectedNode));
					projectFilesTree.SelectedNode.Remove();  
					SaveProject();
				}
			}
		}

		private void menuExcludeFromProject_Click(object sender, System.EventArgs e)
		{
			MainFormClass.MainForm.CloseOpenTabs(GetAbsolutePath(projectFilesTree.SelectedNode));
			projectFilesTree.SelectedNode.Nodes.Clear();
			projectFilesTree.SelectedNode.Remove();	
			isProjectModified=true;
		}

		private void menuProjectOptions_Click(object sender, System.EventArgs e)
		{
			ShowOptionsDialog();
		}

		private void menuBuildProject_Click(object sender, System.EventArgs e)
		{
			BuildProject();
		}
		
		private void menuProgramProject_Click(object sender, System.EventArgs e)
		{
			ProgramDevice();
		}
		
		private void menuSimulateProject_Click(object sender, System.EventArgs e)
		{
			SimulateProgram();
		}

		#endregion	
		
	}
	/// <summary>
	/// Delegate to invoke tab closer from file system watcher event.
	/// </summary>
	public delegate void StringDelegate(String fileName);
}
