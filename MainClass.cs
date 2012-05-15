// Programmer : Shadab Ahmed Ansari
// Version : "1.0"
// Company : RoboAntzLabs
// Website : www.roboantzlabs.com
// email id : shadab@roboantzlabs.com
// Last Modified : 10/7/05 2:50 AM 

using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace MegaIDE
{
	/// <summary>
	/// MainClass containing Main method.
	/// </summary>
	public sealed class MainClass
	{

		#region Main Function
		/// <summary>
		/// The main entry point for the application. Starts MegaIDE with or without command line arguments.
		/// </summary>
		[STAThread]
		public static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.DoEvents();
			Application.ThreadException+=new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
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
	}
}
