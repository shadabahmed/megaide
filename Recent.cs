using System;

namespace MegaIDE
{
	/// <summary>
	/// Summary description for RecentFiles.
	/// </summary>
	public class Recent
	{
		public static System.Collections.ArrayList recentProjects;
		public static System.Collections.ArrayList recentFiles;
		public static System.Collections.ArrayList RecentProjects
		{
			get{return recentProjects;}
			//set{recentProjects=value;}
		}
		public static System.Collections.ArrayList RecentFiles
		{
			get{return recentFiles;}
			//set{recentFiles=value;}
		}
		public static void LoadFile()
		{
			try
			{
				recentProjects=new System.Collections.ArrayList(4);
				recentFiles=new System.Collections.ArrayList(4);
				System.IO.FileStream stream = new System.IO.FileStream("\\lastfiles.dat", System.IO.FileMode.Open);
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				recentProjects = (System.Collections.ArrayList)formatter.Deserialize( stream );
				recentFiles = (System.Collections.ArrayList)formatter.Deserialize( stream ); 
				stream.Close();
			}
			catch{}
		}
		public static void SaveFile()
		{
			try
			{
				System.IO.FileStream stream = new System.IO.FileStream("\\lastfiles.dat", System.IO.FileMode.Create);
				System.Runtime.Serialization.Formatters.Binary.BinaryFormatter  formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				formatter.Serialize( stream, recentProjects );
				formatter.Serialize( stream, recentFiles );
				stream.Flush();
				stream.Close();
			}
			catch{}
		}
		public static void AddRecentFile(string fileName)
		{
         if(recentFiles.Count==4)
			 recentFiles.RemoveAt(3);
	     recentFiles.Insert(0,fileName);
		}
		public static void AddRecentProject(string projectName)
		{
			if(recentProjects.Count==4)
				recentProjects.RemoveAt(3);
			recentProjects.Insert(0,projectName);
		}
		public Recent()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		/*static Recent()
		{
			LoadFile();
		}*/
	}
}
