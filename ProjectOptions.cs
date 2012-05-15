using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MegaIDE
{
	#region Enumerations
	/// <summary>
	/// Enumeration for Project Types.
	/// </summary>
	public enum ProjectTypes
	{
		/// <summary>
		/// Project type for the Megaboard v1.0
		/// </summary>
		MegaBoard_v1,
		/// <summary>
		/// Project type for generic Atmega128 projects
		/// </summary>
		ATMega128_Project
	};
   /// <summary>
   /// Enumeration for GCC Versions
   /// </summary>
	public enum GCCVersions {
		/// <summary>
		/// 22nd Jan 2007 Release
		/// </summary>
		Jan_22_2007=20070122,
		/// <summary>
		/// 4th April 2005 Release
		/// </summary>
		April_04_2005=20050404
	}; 
	/// <summary>
	/// Enumeration for Project Tree item types.
	/// </summary>
	public enum ProjectTreeItemTypes{
		/// <summary>
		/// Node repesents project type.
		/// </summary>
		ProjectType,
		
		/// <summary>
		/// Node represents project name.
		/// </summary>
		ProjectName,
		
		/// <summary>
		/// Node represents the libraries list.
		/// </summary>
		Library,
		
		/// <summary>
		/// Node represents a library item.
		/// </summary>
		LibraryItem,
		
		/// <summary>
		/// Node represents a directory
		/// </summary>
		Directory,
		
		/// <summary>
		/// Node represents a file
		/// </summary>
		File
	};
	
	/// <summary>
	/// Enumeration for microcontroller types
	/// </summary>
	public enum MCUTypes{
		/// <summary>
		/// Atmega128 microcontroller.
		/// </summary>
		ATMEGA128,
		
		/// <summary>
		/// Atmega103 microcontroller.
		/// </summary>
		ATMEGA103,
		
		/// <summary>
		/// Atmega64 microcontroller.
		/// </summary>
		ATMEGA64,
		
		/// <summary>
		/// Atmega32 microcontroller.
		/// </summary>
		ATMEGA32,
		
		/// <summary>
		/// Atmega16 microcontroller.
		/// </summary>
		ATMEGA16,
		
		/// <summary>
		/// Atmega161 microcontroller.
		/// </summary>
		ATMEGA161,
		
		/// <summary>
		/// Atmega162 microcontroller.
		/// </summary>
		ATMEGA162,
		
		/// <summary>
		/// Atmega169 microcontroller.
		/// </summary>
		ATMEGA169,
		
		/// <summary>
		/// Atmega8 microcontroller.
		/// </summary>
		ATMEGA8,
		
		/// <summary>
		/// ATMEGA8515 microcontroller.
		/// </summary>
		ATMEGA8515,
		
		/// <summary>
		/// ATMEGA8535 microcontroller.
		/// </summary>
		ATMEGA8535,
		
		/// <summary>
		/// ATtiny15 microcontroller.
		/// </summary>
		ATtiny15,
		
		/// <summary>
		/// ATtiny12 microcontroller.
		/// </summary>
		ATtiny12,
		
		/// <summary>
		/// ATTINY26 microcontroller.
		/// </summary>
		ATTINY26,
		
		/// <summary>
		/// AT90S8535 microcontroller.
		/// </summary>
		AT90S8535,
		
		/// <summary>
		/// AT90S8515 microcontroller.
		/// </summary>
		AT90S8515,
		
		/// <summary>
		/// AT90S4434 microcontroller.
		/// </summary>
		AT90S4434,
		/// <summary>
		/// AT90S4433 microcontroller.
		/// </summary>
		AT90S4433,
		
		/// <summary>
		/// AT90S2343 microcontroller.
		/// </summary>
		AT90S2343,
		
		/// <summary>
		/// AT90S2333 microcontroller.
		/// </summary>
		AT90S2333,
		
		/// <summary>
		/// AT90S2313 microcontroller.
		/// </summary>
		AT90S2313,
		
		/// <summary>
		/// AT90S4414 microcontroller.
		/// </summary>
		AT90S4414,
		
		/// <summary>
		/// AT90S1200 microcontroller.
		/// </summary>
		AT90S1200,
		/// <summary>
		/// AT90CAN32 Microcontroller 
		/// </summary>
		AT90CAN32,
		/// <summary>
		/// AT90CAN64 Microcontroller 
		/// </summary>
		AT90CAN64,
		/// <summary>
		/// AT90CAN128 Microcontroller 
		/// </summary>
		AT90CAN128,
		/// <summary>
		/// AT90PWM2 Microcontroller 
		/// </summary>
		AT90PWM2,
		/// <summary>
		/// AT90PWM3 Microcontroller 
		/// </summary>
		AT90PWM3,
		/// <summary>
		/// AT90USB646 Microcontroller 
		/// </summary>
		AT90USB646,
		/// <summary>
		/// AT90USB647 Microcontroller 
		/// </summary>
		AT90USB647,
		/// <summary>
		/// AT90USB1286 Microcontroller 
		/// </summary>
		AT90USB1286,
		/// <summary>
		/// AT90USB1287 Microcontroller 
		/// </summary>
		AT90USB1287,
	};
	
	/// <summary>
	/// Enumeration for compiler optimization types.
	/// </summary>
	public enum CompilerOptimizationFlags{
		/// <summary>
		/// No Optimization.
		/// </summary>
		None=0,
		
		/// <summary>
		/// Level 1 Optimization.
		/// </summary>
		Level1=1,
		
		/// <summary>
		/// Level 2 Optimization.
		/// </summary>
		Level2=2,
		
		/// <summary>
		/// Level 3 Optimization.
		/// </summary>
		Level3=3,

		/// <summary>
		/// Optimize for size.
		/// </summary>
		Size
	};
	
	/// <summary>
	/// Enumeration for math library types.
	/// </summary>
	public enum LibTypes{
		/// <summary>
		/// No floating point and reduced functionality. Takes least code size.
		/// </summary>
		Minimal,

		/// <summary>
		/// No floating point. Take more code size than minimal.
		/// </summary>
		Standard,

		/// <summary>
		/// Full floating point scanf and printf library.
		/// </summary>
		FloatingPoint
	};

	/// <summary>
	/// Enumeration for math library types.
	/// </summary>
	public enum OutputTypes
	{
		/// <summary>
		/// Intel hex file format.
		/// </summary>
		IntelHex,

		/// <summary>
		/// SREC Format.
		/// </summary>
		SREC,

		/// <summary>
		/// Binary Format.
		/// </summary>
		Binary
	};
	
	/// <summary>
	/// Enumerations for signed type for bitfields and char.
	/// </summary>
	public enum SignedTypes
	{
		/// <summary>
		/// Treated as unsigned.
		/// </summary>
		Unsigned,

		/// <summary>
		/// Treated as signed.
		/// </summary>
		Signed
	}
	#endregion

	/// <summary>
	/// Class to store all the compiler options for the projects.
	/// </summary>
	[Serializable()]
	public sealed class ProjectOptions
	{

		#region Variables
		
		#region Allow variables

		/// <summary>
		/// Allows or Disallows the modification of crystal frequency.
		/// </summary>
		private bool allowCrysFreqChange=true;

		/// <summary>
		/// Allows or Disallows the modification of MCU type.
		/// </summary>
		private bool allowMCUChange=true;

		/// <summary>
		/// Allows or Disallows the modification of math library type.
		/// </summary>
		private bool allowLibTypeChange=true;

		/// <summary>
		/// To let output property be changed.
		/// </summary>
		private bool allowOutputTypeChange=true;
		
		/// <summary>
		/// To let -lm option be included.
		/// </summary>
		private bool allowLinkMathLibChange=true;

		#endregion
		
		#region Compiler Variables

		/// <summary>
		/// Compiler flags as additional command line arguemts.
		/// </summary>
		private  string compilerFlags="";

		/// <summary>
		/// GCC Version Type
		/// </summary>
		private GCCVersions gccVersion=GCCVersions.Jan_22_2007;

		/// <summary>
		/// Compiler Optimization Flags.
		/// </summary>
		private  CompilerOptimizationFlags optimizationFlags=CompilerOptimizationFlags.None;
		
		/// <summary>
		/// To set whether char signed or unsigned.
		/// </summary>
		private SignedTypes charProperty=SignedTypes.Signed;
 
		/// <summary>
		/// To set whether bitfields signed or unsigned.
		/// </summary>
		private SignedTypes bitFieldsProperty=SignedTypes.Signed;
		
		#endregion
					
		#region Microcontroller & Misc. Variables
		
		/// <summary>
		/// MCU type.
		/// </summary>
		private MCUTypes mcu;
		
		/// <summary>
		/// Crystal Frequency in Hz.
		/// </summary>
		private ulong crystalFrequency;

		/// <summary>
		/// Type of the project.
		/// </summary>
		private ProjectTypes projectType;

		#endregion
	
		#region Linker Variables

		/// <summary>
		/// Linker flags.
		/// </summary>
		private string linkerFlags="";

		/// <summary>
		/// Whether to link math library or not.
		/// </summary>
		private bool linkMathLibrary=true;

		/// <summary>
		/// Scanf library type.
		/// </summary>
		private  LibTypes scanfLibraryType=LibTypes.FloatingPoint;

		/// <summary>
		/// Printf library type.
		/// </summary>
		private  LibTypes printfLibraryType=LibTypes.FloatingPoint;

		#endregion
		
		#region Output Variables
		
		/// <summary>
		/// Additional Obj Copy Arguments
		/// </summary>
		private string objCopyArguments="";

		/// <summary>
		/// Output folder.
		/// </summary>
		private string outputFolder="Output";

		/// <summary>
		/// Comm port.
		/// </summary>
		private string programmingPort="COM1";

		
		/// <summary>
		/// Sets the output type.
		/// </summary>
		private OutputTypes outputType=OutputTypes.IntelHex;

		/// <summary>
		/// Whether to generate list file or not.
		/// </summary>
		private  bool generateListFile=true;
		
		/// <summary>
		/// Whether to generate map file or not.
		/// </summary>
		private bool generateMapFile=true;

		#endregion

		#endregion
			
		#region Properties
		
		#region Allow Properties

		/// <summary>
		/// Gets or Sets the allowLinkMathLibChange.
		/// </summary>
		[Browsable(false)]
		public bool AllowMathLibChange
		{
			get{return allowLinkMathLibChange;}
			set{allowLinkMathLibChange=value;}
		}

		/// <summary>
		/// Gets or Sets the allowCrysFreqChange.
		/// </summary>
		[Browsable(false)]
		public bool AllowCrysFreqChange
		{
			get{return allowCrysFreqChange;}
			set{allowCrysFreqChange=value;}
		}
		
		/// <summary>
		/// Gets or Sets the allowLibTypeChange.
		/// </summary>
		[Browsable(false)]
		public bool AllowLibTypeChange
		{
			get{return allowLibTypeChange;}
			set{allowLibTypeChange=value;}
		}
		
		/// <summary>
		/// Gets or Sets the allowMCUChange.
		/// </summary>
		[Browsable(false)]
		public bool AllowMCUChange
		{
			get{return allowMCUChange;}
			set{allowMCUChange=value;}
		}

		/// <summary>
		/// Gets or Sets the allowOutputChange.
		/// </summary>
		[Browsable(false)]
		public bool AllowOutputTypeChange
		{
			get{return allowOutputTypeChange;}
			set{allowOutputTypeChange=value;}
		}
		
		#endregion

		#region Compiler Properties
		
		/// <summary>
		/// Gets or Sets the compiler flags.
		/// </summary>
		[Description("GCC Compiler Version"),Category("Compiler Options")]
		public GCCVersions GCC_Version
		{
			get{return gccVersion;}
			set
			{
				gccVersion=value;
				ProjectManagerClass.GCCPath = MainFormClass.MainForm.AppDirectory+"\\avr-gcc "+((int)gccVersion).ToString()+"\\bin";
			}
		}
	
		/// <summary>
		/// Gets or Sets the compiler flags.
		/// </summary>
		[Description("Additional flags to be included in compiler arguments."),Category("Compiler Options")]
		public string CompilerFlags
		{
			get{return compilerFlags;}
			set{compilerFlags=value;}
		}
			
		/// <summary>
		/// Gets or Sets the compiler optimization flags.
		/// </summary>
		[Description("Optimization flags for the compiler. There are three levels of optimization for performance and one for optimization for size. If you get logical errors in your program set this to none."),Category("Compiler Options")]
		public CompilerOptimizationFlags OptimizationFlags
		{
			get{return optimizationFlags;}
			set{optimizationFlags=value;}
		}

		/// <summary>
		/// Gets or Sets the compiler optimization flags.
		/// </summary>
		[Description("Signed property for the \"char\" type. Default is signed. Change it to unsigned only if you specifically want that option."),Category("Compiler Options")]
		public SignedTypes CharProperty
		{
			get{return bitFieldsProperty;}
			set{bitFieldsProperty=value;}
		}
		
		/// <summary>
		/// Gets or Sets the compiler optimization flags.
		/// </summary>
		[Description("Signed property for Bitfields. Default is signed. Change it to unsigned only if you specifically want the option."),Category("Compiler Options")]
		public SignedTypes BitFieldsProperty
		{
			get{return charProperty;}
			set{charProperty=value;}
		}

		#endregion

		#region Linker Properties
		
		/// <summary>
		/// Gets or Sets the linker flags.
		/// </summary>
		[Description("Additional linker arguments. The math library arguments are already included if it is selected."),Category("Linker Options")]
		public string LinkerFlags
		{
			get{return linkerFlags;}
			set{linkerFlags=value;}
		}

		/// <summary>
		/// Gets or Sets the scanf library type.
		/// </summary>
		[Description("Scanf library to be linked with the project. Floating Point scanf library takes maximum space"),Category("Linker Options")]
		public LibTypes ScanfLibraryType
		{
			get{return scanfLibraryType;}
			set
			{
				if(!allowLibTypeChange)
				{
					MessageBox.Show("Cannot change this property for the current project type.","Error!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				if(value==LibTypes.FloatingPoint)
				{
					linkMathLibrary=true;
				}
				scanfLibraryType=value;
			}
		}
		
		/// <summary>
		/// Gets or Sets the printf library type.
		/// </summary>
		[Description("Scanf library to be linked with the project. Floating Point scanf library takes maximum space"),Category("Linker Options")]
		public LibTypes PrintfLibraryType
		{
			get{return printfLibraryType;}
			set
			{
				if(!allowLibTypeChange)
				{
					MessageBox.Show("Cannot change this property for the current project type.","Error!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				if(value==LibTypes.FloatingPoint)
				{
					linkMathLibrary=true;
				}
				printfLibraryType=value;
			}
		}
		
		/// <summary>
		/// Gets or Sets the Link Library type.
		/// </summary>
		/// 
		[Description("Link math library. Must be set true when including math.h or using scanf and printf with floating point."),Category("Linker Options")]
		public bool LinkMathLibrary
		{
			get{return linkMathLibrary;}
			set
			{
				if(value==false && (printfLibraryType==LibTypes.FloatingPoint || scanfLibraryType==LibTypes.FloatingPoint))
				{
					MessageBox.Show("Cannot change this property while either of scanf or printf is set to floating point.","Error!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}

				if(!allowLinkMathLibChange)
				{
					MessageBox.Show("Cannot change this property for the current project type.","Error!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				linkMathLibrary=value;
			}
		}


		#endregion

		#region Output Properties
		
		/// <summary>
		/// Gets or Sets the additonal arguments for OBJCOPY.
		/// </summary>
		[Description("Additional arguments for obj-copy process. Do not modify this unless you are sure about it."),Category("Output and Programming Options")]	
		public string ObjCopyArguments
		{
			get{return objCopyArguments;}
			set{objCopyArguments=value;}
		}
		/// <summary>
		/// Gets or Sets the generate list file option.
		/// </summary>
		[Description("Path for the output files. Path must be relative to the project directory."),Category("Output and Programming Options")]	
		public string OutputFolder
		{
			set{outputFolder=value;}
			get{return outputFolder;}
		}

		/// <summary>
		/// Gets or Sets the generate list file option.
		/// </summary>
		[Description("Set whether to generate list file or not."),Category("Output and Programming Options")]	
		public bool GenerateListFile
		{
			set{generateListFile=value;}
			get{return generateListFile;}
		}

		/// <summary>
		/// Gets or Sets the generate map
		///  file option.
		/// </summary>
		[Description("Set whether to generate map file or not."),Category("Output and Programming Options")]	
		public bool GenerateMapFile
		{
			set{generateMapFile=value;}
			get{return generateMapFile;}
		}
		
		/// <summary>
		/// Gets or Sets the generate list file option.
		/// </summary>
		[Description("Select the port you connected your MegaBoard. Megaboard port number can be found in Device Manager of your system."),Category("Output and Programming Options")]	
		public string ProgrammingPort
		{
			set{programmingPort=value;}
			get{return programmingPort;}
		}
		
		/// <summary>
		/// Gets or Sets the outputTypel
		/// </summary>
		[Description("Select the format of the output file. Default is intel hex. "),Category("Output and Programming Options")]	
		public OutputTypes OutputType
		{
			get{return outputType;}
			set
			{
				if(!allowOutputTypeChange)
				{
					MessageBox.Show("Cannot change this property for the current project type.","Error!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				outputType=value;
			}
		}
		#endregion
		
		#region Microcontroller & Misc. Options

		/// <summary>
		/// Gets or Sets the project type.
		/// </summary>
		[Description("Current project type ."),ReadOnly(true),Category("Project Type")]
		public ProjectTypes ProjectType
		{
			get{return projectType;}
			set{projectType=value;}
		}

		/// <summary>
		/// Gets or Sets the MCU type.
		/// </summary>
		[Description("Shows the target microcontroller for the current project."),Category("Microcontroller Options")]
		public MCUTypes MCU
		{
			get{return mcu;}
			set
			{
				if(!allowMCUChange)
				{
					MessageBox.Show("Cannot change this property for the current project type.","Error!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				mcu=value;
			}
		}
		
		/// <summary>
		/// Gets or Sets the crystal frequency in Hz.
		/// </summary>
		[Description("Operating frequency in Hz of the target microcontroller."),Category("Microcontroller Options")]
		public ulong CrystalFrequency
		{
			get{return crystalFrequency;}
			set
			{
				if(!allowCrysFreqChange)
				{
					MessageBox.Show("Cannot change this property for the current project type.","Error!",MessageBoxButtons.OK,MessageBoxIcon.Warning);
					return;
				}
				crystalFrequency=value;
			}
		}
		
		#endregion

		#endregion

	}
}
