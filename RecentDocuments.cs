using System;

namespace MegaIDE
{
	/// <summary>
	/// Class containing static methods to save recently opened documents list either in a file or registry .
	/// </summary>
	/// 
	public sealed class RecentDocuments
	{

		#region Private variables
		private static System.Collections.ArrayList recentProjectsList;
		private static System.Collections.ArrayList recentFilesList;
		private static bool useRegistry;
		#endregion 
		
		#region Properties
		/// <summary>
		/// Gets or Sets whether to use registry for saving files list.
		/// </summary>
		public static bool UseRegistry
		{
			get{return useRegistry;}
			set{useRegistry=value;}
		}
		
		/// <summary>
		/// Gets the recently opened projects list.
		/// </summary>
		public static System.Collections.ArrayList RecentProjectsList
		{
			get{return recentProjectsList;}
		}

		/// <summary>
		/// Gets the recently opened files list. 
		/// </summary>
		public static System.Collections.ArrayList RecentFilesList
		{
			get{return recentFilesList;}
		}
		
		#endregion

		#region Methods

		/// <summary>
		/// Load the recently opened documents list from the file specified.
		/// </summary>
		/// <param name="fileName">
		/// Path of the file to load lists from.
		/// </param>
		public static void LoadListFromFile(string fileName)
		{
			try
			{
				recentProjectsList=new System.Collections.ArrayList(4);
				recentFilesList=new System.Collections.ArrayList(4);
				System.IO.Stream stream = new System.IO.FileStream(fileName,System.IO.FileMode.Open);
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				recentProjectsList = (System.Collections.ArrayList)formatter.Deserialize( stream );
				recentFilesList = (System.Collections.ArrayList)formatter.Deserialize( stream ); 
				stream.Close();
			}
			catch{}
		}
		
		/// <summary>
		/// Saves recently opened documents list to the file specified.
		/// </summary>
		/// <param name="fileName">
		/// Path of the file to save lists to.
		/// </param>
		public static void SaveListToFile(string fileName)
		{
		    try
			 { 			
				System.IO.Stream stream = new System.IO.FileStream(fileName,System.IO.FileMode.Create);
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter  formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				formatter.Serialize( stream, recentProjectsList );
				formatter.Serialize( stream, recentFilesList );
				stream.Flush();
				stream.Close();
			}
			catch{}
		}
		
		/// <summary>
		/// Load the recently opened files and projects list from the registry (from "HKEY_LOCAL_MACHINE\SOFTWARE\RoboAntz Labs\MegaIDE\Recent Documents").
		/// </summary>
		public static void LoadListFromRegistry()
		{
			try
			{
				recentProjectsList=new System.Collections.ArrayList(4);
				recentFilesList=new System.Collections.ArrayList(4);
				Microsoft.Win32.RegistryKey regRecentFiles=Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\RoboAntz Labs\MegaIDE\Recent Documents\Recent Files");
				Microsoft.Win32.RegistryKey regRecentProjects=Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\RoboAntz Labs\MegaIDE\Recent Documents\Recent Projects");
				foreach(string recentFile in regRecentFiles.GetValueNames())
					recentFilesList.Add(regRecentFiles.GetValue(recentFile,""));
				foreach(string recentProject in regRecentProjects.GetValueNames())
					recentProjectsList.Add(regRecentProjects.GetValue(recentProject,""));
				regRecentFiles.Close();
				regRecentProjects.Close();
			}
			catch{}
		}

		/// <summary>
		/// Saves the recently opened documents list in the registry (at "HKEY_LOCAL_MACHINE\SOFTWARE\RoboAntz Labs\MegaIDE\Recent Documents").
		/// </summary>
		public static void SaveListToRegistry()
		{	
			int index=0;
			Microsoft.Win32.RegistryKey regRecentDocs=Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\RoboAntz Labs\MegaIDE\Recent Documents");
			regRecentDocs.DeleteSubKey("Recent Files",false);
            regRecentDocs.DeleteSubKey("Recent Projects",false);
			Microsoft.Win32.RegistryKey regRecentFiles=regRecentDocs.CreateSubKey("Recent Files");
			Microsoft.Win32.RegistryKey regRecentProjects=regRecentDocs.CreateSubKey("Recent Projects");
			if(recentFilesList.Count!=0)
				for(index=0;index<recentFilesList.Count;index++)
					regRecentFiles.SetValue(index.ToString(),recentFilesList[index]);
			if(recentProjectsList.Count!=0)
				for(index=0;index<recentProjectsList.Count;index++)
					regRecentProjects.SetValue(index.ToString(),recentProjectsList[index]);               
			regRecentFiles.Close();
			regRecentProjects.Close();
		}

		/// <summary>
		/// Adds a file path to the recent files list.
		/// </summary> 
		/// <param name="fileName">
		/// Path of the file.
		/// </param>
		public static void AddToRecentFiles(string fileName)
		{
			for(int index=0;index<recentFilesList.Count;index++)
			{
				if(fileName.ToLower()==((string)recentFilesList[index]).ToLower())
				{
					recentFilesList.RemoveAt(index);
					recentFilesList.Insert(0,fileName);
					return;
				}
			}
			if(recentFilesList.Count>=4)
				recentFilesList.RemoveAt(3);
			recentFilesList.Insert(0,fileName);
		}

		/// <summary>
		/// Removes a file path from the recent files list.
		/// </summary>
		/// <param name="fileName">
		/// Path of the file.
		/// </param>
		public static void RemoveFromRecentFiles(string fileName)
		{
			if(recentFilesList.Contains(fileName))
			 recentFilesList.Remove(fileName);
		}

		/// <summary>
		/// Adds a project file path to the recent projects list.
		/// </summary>
		/// <param name="projectName">
		/// Path of the project file.
		/// </param>
		public static void AddToRecentProjects(string projectName)
		{
			for(int index=0;index<recentProjectsList.Count;index++)
			{
				if(projectName.ToLower()==((string)recentProjectsList[index]).ToLower())
				{
					recentProjectsList.RemoveAt(index);
					recentProjectsList.Insert(0,projectName);
					return;
				}
			}
			if(recentProjectsList.Count>=4)
				recentProjectsList.RemoveAt(3);
			recentProjectsList.Insert(0,projectName);
		}

		/// <summary>
		/// Removes a project file path from the recent projects list.
		/// </summary>
		/// <param name="projectName">
		/// Path of the project file.
		/// </param>
		public static void RemoveFromRecentProjects(string projectName)
		{
			if(RecentProjectsList.Contains(projectName))
				RecentProjectsList.Remove(projectName);
		}

		#endregion

	}
}
