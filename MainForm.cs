/*
 * Created by SharpDevelop.
 * User: mtaylor
 * Date: 1/6/2010
 * Time: 9:10 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Threading;

using System.IO;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

using SolidWorks.Interop.sldworks;
using SolidWorks.Interop.swconst;


namespace sw_BOM_Scan
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
		
		private String statLabel;
		private Thread thdScan;
		
		private DbConnection cnn;
		private DbCommand cmdCheckFiles;
		private DbCommand cmdInsertConfigs;
		private DbCommand cmdCheckConfigs;
		private DbCommand cmdShowChildren;
		private DbCommand cmdInsertAdjacency;
		private DbCommand cmdUpdateAdjacency;
		private DbCommand cmdGetAdjCount;
		private DbCommand cmdCheckAdjacency;
		
		private SldWorks swApp;
		private ModelDoc2 swMainModel;
		private AssemblyDoc swMainAssy;
		private Configuration swMainConfig;
		
		private Hashtable htPropField = new Hashtable();
		private Hashtable htFieldProp = new Hashtable();
		private string[] aFieldSequence;
		
		
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
			
		}
		
		
		
		void MainFormLoad(object sender, EventArgs e)
		{
			
	        // ******************************************************************************
	        // ***                                                                        ***
	        // ***        The custom property for 'Units of Measure' was originally       ***
	        // ***        named 'Alternate Quantity'.  When we added a property for       ***
	        // ***        tracking alternate quantity values (double precision            ***
	        // ***        numbers), we didn't want to go back and change the custom       ***
	        // ***        properties in every part file.                                  ***
	        // ***        As a work around:                                               ***
	        // ***            -we labeled the 'Alternate Quantity' text field 'UOM'       ***
	        // ***            -we are writing the units of measure to the                 ***
	        // ***             'Alternate Quantity' custom property                       ***
	        // ***            -we labeled the 'UOM' text field 'Alternate Quantity'       ***
	        // ***            -we are writing the alternate quantity value to the         ***
	        // ***             'UOM' custom property                                      ***
	        // ***                                                                        ***
	        // ******************************************************************************
			
			// Map SolidWorks custom properties to DB field names, in appropriate sequence
			htPropField.Add("AltQty", "Uom");
			htPropField.Add("PartNum", "PartNum");
			htPropField.Add("Description", "Description");
			htPropField.Add("Designed By", "DesignedBy");
			htPropField.Add("Date1", "DrawDate");
			htPropField.Add("Type", "Type");
			htPropField.Add("UOM", "AltQty");
			htPropField.Add("Eng Approval", "EngApproval");
			htPropField.Add("Eng Appr Date", "EngApprDate");
			htPropField.Add("Mfg Approval", "MfgApproval");
			htPropField.Add("Mfg Appr Date", "MfgApprDate");
			htPropField.Add("QA Approval", "QaApproval");
			htPropField.Add("QA Appr Date", "QaApprDate");
			htPropField.Add("Purch Approval", "PurchApproval");
			htPropField.Add("Purch Appr Date", "PurchApprDate");
			htPropField.Add("Material", "Material");
			htPropField.Add("Finish", "Finish");
			htPropField.Add("Coating", "Coating");
			htPropField.Add("Notes", "Notes");
			htPropField.Add("Revision", "Revision");
			htPropField.Add("ECO", "Ecos");
			htPropField.Add("EcoRevs", "EcoRevs");
			htPropField.Add("Zone", "Zone");
			htPropField.Add("EcoDescription", "EcoDescriptions");
			htPropField.Add("Date2", "EcoDates");
			htPropField.Add("EcoChk", "EcoChks");
			htPropField.Add("P_M", "Catalog");
		
			htFieldProp.Add("Uom", "AltQty");
			htFieldProp.Add("PartNum", "PartNum");
			htFieldProp.Add("Description", "Description");
			htFieldProp.Add("DesignedBy", "Designed By");
			htFieldProp.Add("DrawDate", "Date1");
			htFieldProp.Add("Type", "Type");
			htFieldProp.Add("AltQty", "UOM");
			htFieldProp.Add("EngApproval", "Eng Approval");
			htFieldProp.Add("EngApprDate", "Eng Appr Date");
			htFieldProp.Add("MfgApproval", "Mfg Approval");
			htFieldProp.Add("MfgApprDate", "Mfg Appr Date");
			htFieldProp.Add("QaApproval", "QA Approval");
			htFieldProp.Add("QaApprDate", "QA Appr Date");
			htFieldProp.Add("PurchApproval", "Purch Approval");
			htFieldProp.Add("PurchApprDate", "Purch Appr Date");
			htFieldProp.Add("Material", "Material");
			htFieldProp.Add("Finish", "Finish");
			htFieldProp.Add("Coating", "Coating");
			htFieldProp.Add("Notes", "Notes");
			htFieldProp.Add("Revision", "Revision");
			htFieldProp.Add("Ecos", "ECO");
			htFieldProp.Add("EcoRevs", "EcoRevs");
			htFieldProp.Add("Zone", "Zone");
			htFieldProp.Add("EcoDescriptions", "EcoDescription");
			htFieldProp.Add("EcoDates", "Date2");
			htFieldProp.Add("EcoChks", "EcoChk");
			htFieldProp.Add("Catalog", "P_M");
			
			aFieldSequence = new String[27];
			aFieldSequence[0] = "Uom";
			aFieldSequence[1] = "PartNum";
			aFieldSequence[2] = "Description";
			aFieldSequence[3] = "DesignedBy";
			aFieldSequence[4] = "DrawDate";
			aFieldSequence[5] = "Type";
			aFieldSequence[6] = "AltQty";
			aFieldSequence[7] = "EngApproval";
			aFieldSequence[8] = "EngApprDate";
			aFieldSequence[9] = "MfgApproval";
			aFieldSequence[10] = "MfgApprDate";
			aFieldSequence[11] = "QaApproval";
			aFieldSequence[12] = "QaApprDate";
			aFieldSequence[13] = "PurchApproval";
			aFieldSequence[14] = "PurchApprDate";
			aFieldSequence[15] = "Material";
			aFieldSequence[16] = "Finish";
			aFieldSequence[17] = "Coating";
			aFieldSequence[18] = "Notes";
			aFieldSequence[19] = "Revision";
			aFieldSequence[20] = "Ecos";
			aFieldSequence[21] = "EcoRevs";
			aFieldSequence[22] = "Zone";
			aFieldSequence[23] = "EcoDescriptions";
			aFieldSequence[24] = "EcoDates";
			aFieldSequence[25] = "EcoChks";
			aFieldSequence[26] = "Catalog";
			
		}
		
		
		
		void CmdRefreshClick(object sender, EventArgs e)
		{
			
			// Get solidworks process
			string strAttach = swAttach();
			if (strAttach!=null) {
				DialogResult dr = MessageBox.Show(strAttach,
					"Loading SW",
					MessageBoxButtons.OK,
					MessageBoxIcon.Exclamation,
					MessageBoxDefaultButton.Button1);
				
				return;
			}
			
			// Get active model
			string[] strResult = LoadActiveModel();
			txtActiveDoc.Text = strResult[2];
			
//	        ' clear previous scan data
//	        SetStatusLabel("Clearing old scan data")
//	        htFullBom = Nothing
//	        analysisOK = False
//	
//	        ' reset form objects
//	        cmdScan.Visible = True
//	        cmdScan.Enabled = True
//	        cmdAnalyze.Enabled = False
//	        cmdRepair.Enabled = False
//	        cmdWrite.Enabled = False
//	        Output_Option_0.Enabled = True
//	
//	        ' Get active model
//	        SetStatusLabel("Reloading active model")
//	        strModelInfo = LSW1.LoadActiveModel
//	        If strModelInfo(0) <> Nothing Then
//	            MsgBox(strModelInfo(0), MsgBoxStyle.OKOnly)
//	            Me.Close()
//	            Exit Sub
//	        End If
//	
//	        ' Set form title
//	        Me.Text = "BOM Export: " & strModelInfo(2)
//	        SetStatusLabel("")
			
		}
		
		
		
		void CmdTargetFileClick(object sender, EventArgs e) {
			
			String fileName;
			
			saveFileDialog1.FileName = null;		// Clear old file names
			saveFileDialog1.ShowDialog();			// Display the dialog
			fileName = saveFileDialog1.FileName;	// Get filename
			
			if ( File.Exists(fileName) ) {
				
				// Ask whether to append or overwrite
				DialogResult dr = MessageBox.Show("YES: Append data to " + fileName + ".\nNO: Overwrite this file",
					"Output Option",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Exclamation,
					MessageBoxDefaultButton.Button1);
				
				if (dr == DialogResult.Yes) {
					
					// Append data to specified file
					InitDB(fileName);
					
				} else if (dr == DialogResult.No) {
					
					// try to delete the file
					try {
						File.Delete(fileName);
					} catch (Exception ex) {
						MessageBox.Show("Could not overwrite " + fileName + ".\n" + ex.ToString(),
							"Output File Error",
							MessageBoxButtons.OK);
						return;
					}
					
					// Open a new file
					InitDB(fileName);
					
				} else {
					
					// Dialog was apparently closed
					return;
					
				}
				
			} else {
				
				// File does not exist, so make a new one
				InitDB(fileName);
				
			}
			
			
			txtTargetFile.Text = fileName;
			this.txtStatus.Text = "Output file ready for scan";
			
		}
		
		
		
		void CmdScanClick(object sender, EventArgs e)
		{
			
			// check for lightweight components
			if (swMainAssy.GetLightWeightComponentCount() > 0) {
				DialogResult dr = MessageBox.Show("All lightweight components will be fully resolved.  Continue?",
				                                  "Lightweight Components",
				                                  MessageBoxButtons.YesNo);
				if (dr == DialogResult.Yes) {
					this.txtStatus.Text = "Resolving lightweight components...";
					swMainAssy.ResolveAllLightWeightComponents(false);
					this.txtStatus.Text = "Proceding with scan";
				} else {
					this.txtStatus.Text = "Some components still lightweight";
					return;
				}
			}
			
			// scan once
			//DoScan();
			
			// create and start the scanning thread
			this.thdScan = new Thread(new ThreadStart(ScanControl));
			thdScan.Start();
			
			// change button config
			this.cmdScan.Visible = false;
			//this.cmdCancel.Enabled = true;
			
		}
		
		
		
		void CmdCancelClick(object sender, EventArgs e)
		{
			
			// stop scanning thread
			thdScan.Interrupt();
			
			// change button config
			this.cmdScan.Visible = true;
			//this.cmdCancel.Enabled = false;
			
		}
		
		
		
		void ScanControl() {
			
			MethodInvoker WriteLabelDelegate = new MethodInvoker(WriteLabel);
			
			try{
				DoScan();
			} catch(ThreadInterruptedException e) {
				this.statLabel = "Interupted \n "+e.ToString();
				Invoke(WriteLabelDelegate);
				return;
			}
			this.statLabel = "Scanning Completed Successfully";
			Invoke(WriteLabelDelegate);
			
		}
		
		
		
		ThreadInterruptedException DoScan() {
			
			try{
				Thread.Sleep(0);
			} catch(ThreadInterruptedException e) {
				return(e);
			}
			
			
			// Set the initial BOM level
			int intLevel = 0;
			
			// Find the root node.  This is only done ONCE for the entire assembly structure
			Component2 swRootComp = (Component2)swMainConfig.GetRootComponent();
			
			// Get name of configuration containing properties
			string strMainConfig = swMainConfig.Name;
			string strMainPath = swMainModel.GetPathName();
		
			// write custom properties for main model
			//DbWriteConfigs(swMainModel, strMainPath, true);
			DbWriteConfig(swMainModel, strMainPath, strMainConfig);
			
			// Recursively traverse the assembly
			if ( swRootComp != null ) {
				TraverseAssy(swRootComp, strMainPath, strMainConfig, intLevel);
			}
			
			return(null);
			
		}
		
		
			
#region " VB GetBOM "

//		public void GetBOM(ModelDoc2 ParentModel) {
//			
//			int intOutputOption;
//			string Title;
//			string Style;
//			string Msg;
//		
//			ModelDoc2 swModel;
//			AssemblyDoc swAssy;
//			string strTopName;
//			int swDocType;
//			component swRootComp;
//			configuration swConfig;
//			string strCurrConfig;
//			swProps sctProps;
//			string strID;
//		
//			int intSpaces;
//			int intLevel;
//		
//			//Dim blnOK As Boolean
//		
//		
//			htBomProps = new Hashtable();
//			swModel = ParentModel;
//			intLevel = 0;
//			// Set the initial BOM level
//		
//			frmSender.SetStatusLabel("Getting assembly object");
//			swAssy = swModel;
//		
//			// check for lightweight components
//			frmSender.SetStatusLabel("Checking for lightweight comps");
//			intLightWeight = swAssy.GetLightWeightComponentCount;
//			if (intLightWeight > 0) {
//				int intReturn;
//				intReturn = MsgBox("All lightweight components will be fully resolved.  Continue?", MsgBoxStyle.YesNo);
//				if (intReturn == MsgBoxResult.Yes) {
//					swAssy.ResolveAllLightWeightComponents(false);
//				}
//				else {
//					return;
//				}
//			}
//		
//			strTopName = swModel.GetPathName;
//			// Get the name of the highest level document
//			swDocType = GetTypeFromString(strTopName);
//			// Determine this document type
//		
//			// if the document is not an assembly then send
//			// an error message to the user.
//			if ((swDocType != swconst.swDocumentTypes_e.swDocASSEMBLY)) {
//				Msg = "This program only works with assemblies.";
//				Style = MsgBoxStyle.Exclamation;
//				// Error style dialog
//				Title = "BOM";
//				// Define title
//				MsgBox(Msg, Style, Title);
//				// Display error message
//				return;
//			}
//			
//			// Find the root node.  This is only done ONCE for the entire assembly structure
//			swConfig = swModel.GetActiveConfiguration();
//			swRootComp = swConfig.GetRootComponent();
//		
//			// Get name of configuration containing properties
//			strCurrConfig = swConfig.Name;
//		
//			// Get custom properties for the root node
//			sctProps = GetCustomStruct(ParentModel, strCurrConfig);
//			sctProps.FileName = strTopName;
//			sctProps.AsmQty = 1;
//		
//			// Create unique id for this parent,part combination
//			if (sctProps.ID == "") {
//				strID = sctProps.FileName;
//			}
//			else {
//				strID = sctProps.ID;
//			}
//		
//			// add to hash
//			htBomProps.Add(strID, sctProps);
//		
//			// Recursively traverse the assembly
//			if (!swRootComp == null) {
//				TraverseAssy(swRootComp, sctProps, intLevel);
//			}
//		
//			// set status label
//			frmSender.SetStatusLabel("Scan complete");
//		
//		}

#endregion
		
		
		
		void WriteLabel() {
			
			this.txtStatus.Text = this.statLabel.ToString();
			
		}
		
		
		
		string swAttach()
		{
			
			string strMessage;
			//bool blnStatus = true;
			
			//Check for the status of existing solidworks apps
			if (System.Diagnostics.Process.GetProcessesByName("sldworks").Length < 1) {
				strMessage = "Solidworks instance not detected.";
				//blnStatus = false;
			}
			else if (System.Diagnostics.Process.GetProcessesByName("sldworks").Length > 1) {
				strMessage = "Multiple instances of Solidworks detected.";
				//blnStatus = false;
			}
			else {
				strMessage = null;
				//swApp = System.Diagnostics.Process.GetProcessesByName("SldWorks.Application");
				swApp = (SldWorks)System.Runtime.InteropServices.Marshal.GetActiveObject("SldWorks.Application");
				//swApp = (SolidWorks.Interop.sldworks.Application)
			}
			
			return (strMessage);
			
		}
		
		
		
		string[] LoadActiveModel() {
			
			// returns string array
			//   element 0 = error message
			//   element 1 = model name with path
			//   element 2 = model name
			//   element 3 = referenced configuration name
			
			
			ModelDoc2 swDoc;
			DrawingDoc swDrawDoc;
			SolidWorks.Interop.sldworks.View swDrawView;
			swDocumentTypes_e swDocType;
			
			string strModelFile;
			string strModelName;
			//string strFileExt;
			string strConfigName = null;
			
			string[] strReturn = new string[4];
			
			int intErrors = 0;
			int intWarnings = 0;
			
			// Get the active document
			swDoc = (ModelDoc2)swApp.ActiveDoc;
			if (swDoc == null) {
				strReturn[0] = "Could not acquire an active document";
				return(strReturn);
			}
			
			//Check for the correct doc type
			strModelFile = swDoc.GetPathName();
			strModelName = strModelFile.Substring(strModelFile.LastIndexOf("\\") + 1, strModelFile.Length - strModelFile.LastIndexOf("\\") - 1);
			swDocType = (swDocumentTypes_e)swDoc.GetType();
			
			// get model document from drawing document
			if (swDocType == swDocumentTypes_e.swDocDRAWING) {
				
				swDrawDoc = (DrawingDoc)swDoc;
				swDrawView = (SolidWorks.Interop.sldworks.View)swDrawDoc.GetFirstView();
				swDrawView = (SolidWorks.Interop.sldworks.View)swDrawView.GetNextView();
				
				strModelFile = swDrawView.GetReferencedModelName();
				strConfigName = swDrawView.ReferencedConfiguration;
				strModelName = strModelFile.Substring(strModelFile.LastIndexOf("\\") + 1, strModelFile.Length - strModelFile.LastIndexOf("\\") - 1);
				swDocType = (swDocumentTypes_e)GetTypeFromString(strModelFile);
				
				if (swDocType != swDocumentTypes_e.swDocASSEMBLY & swDocType != swDocumentTypes_e.swDocPART) {
					strReturn[0] = "error getting file type from drawing view's referenced model";
					return(strReturn);
				}
				
			}
			
			// if the document is not an assembly then send
			// an error message to the user.
			if ( swDocType != swDocumentTypes_e.swDocASSEMBLY ) {
				strReturn[0] = "This program only works with assemblies";
				return(strReturn);
			}
			
			// Try to load the model file
			try {
				swMainModel = swApp.OpenDoc6(strModelFile, (int)swDocType, (int)swOpenDocOptions_e.swOpenDocOptions_Silent, strConfigName, ref intErrors, ref intWarnings);
			}
			catch (Exception e) {
				strReturn[0] = e.Message;
				return(strReturn);
			}
			
			// Write model info to shared variables
			if (strConfigName != null) {
				swMainConfig = (Configuration)swMainModel.GetConfigurationByName(strConfigName);
			} else {
				swMainConfig = (Configuration)swMainModel.GetActiveConfiguration();
			}
			swMainAssy = (AssemblyDoc)swMainModel;
			
			// check for lightweight components
			if (swMainAssy.GetLightWeightComponentCount() > 0) {
				DialogResult dr = MessageBox.Show("All lightweight components will be fully resolved.  Continue?",
				                                  "Lightweight Components",
				                                  MessageBoxButtons.YesNo);
				if (dr == DialogResult.Yes) {
					this.txtStatus.Text = "Resolving lightweight components...";
					swMainAssy.ResolveAllLightWeightComponents(false);
					this.txtStatus.Text = "Proceding with scan";
				} else {
					this.txtStatus.Text = "Some components still lightweight";
					return(strReturn);
				}
			}
			
			// Write model info to return array
			strReturn[1] = strModelFile;
			strReturn[2] = strModelName;
			strReturn[3] = strConfigName;
			return(strReturn);
			
		}
		
		
		
		ThreadInterruptedException TraverseAssy(Component2 swParentComp, string strParentPath, string strParentConfig, int intStartLevel) {
			
			// Check for cancel button
			try{
				Thread.Sleep(0);
			} catch(ThreadInterruptedException e) {
				return(e);
			}
			
			// If no component, then exit
			if (swParentComp == null) {
				return(null);
			}
			
			// Prepare to write status label
			MethodInvoker WriteLabelDelegate = new MethodInvoker(WriteLabel);
			
			// increment BOM level
			int intNextLevel = intStartLevel + 1;
			
			// Get the list of children (if any)
			object oChildren = swParentComp.GetChildren();
			System.Array aChildren = (Array)oChildren;
			//Component2[] swChildren = (Component2[])aChildren;
			//Component2[] swChildren = (Component2[])swParentComp.GetChildren();
			
			// die if array contains no children
			if (aChildren == null) {
				return(null);
			}
			
			// get children for each Child in this subassembly
			foreach ( Component2 swChildComp in aChildren ) {
				
				// Skip suppressed/excluded parts
				if ( (swComponentSuppressionState_e)swChildComp.GetSuppression() != swComponentSuppressionState_e.swComponentSuppressed && !swChildComp.ExcludeFromBOM ) {
					
					// Get the model doc and info of the component
					ModelDoc2 swChildDoc = (ModelDoc2)swChildComp.GetModelDoc();
					string strChildPath = swChildComp.GetPathName();
					string strChildConfig = swChildComp.ReferencedConfiguration;
					
					// write status to form label
					string strModelName = strChildPath.Substring(strChildPath.LastIndexOf("\\") + 1,
					                                             strChildPath.Length - strChildPath.LastIndexOf("\\") - 1);
					this.statLabel = strModelName;
					Invoke(WriteLabelDelegate);
					
					// Write custom properties for this config of this child 
					//DbWriteConfig( swChildDoc, strChildPath, strChildConfig );
					DbWriteConfig( swChildComp, strChildPath, strChildConfig );
					
					// Write BOM adjacency
					DbWriteAdjacency(strParentPath, strParentConfig, strChildPath, strChildConfig);
					
					// If this component not already traversed
					cmdCheckAdjacency.Parameters[0].Value = strChildPath;
					cmdCheckAdjacency.Parameters[1].Value = strChildConfig;
					long intExists = Convert.ToInt64(cmdCheckAdjacency.ExecuteScalar());
					if ( intExists==0 ) {
						
						// If components not hidden from BOM
						cmdShowChildren.Parameters[0].Value = strChildPath;
						cmdShowChildren.Parameters[1].Value = strChildConfig;
						long intShow = Convert.ToInt64(cmdShowChildren.ExecuteScalar());
						if ( intShow!=0 ) {
							
							// Recurse into this child
							TraverseAssy(swChildComp, strChildPath, strChildConfig, intNextLevel);
							
						}
					}
					
				}
				
			}
			
			return(null);
			
		}
		
		
		
		swDocumentTypes_e GetTypeFromString(string strModelPathName) {
			
			//************************************************************
			//**     strModelPathName = fully qualified name of file
			//************************************************************
			string strModelName;
			string strFileExt;
			swDocumentTypes_e swDocType;
			
			strModelName = strModelPathName.Substring(strModelPathName.LastIndexOf("\\") + 1, strModelPathName.Length - strModelPathName.LastIndexOf("\\") - 1);
			strFileExt = strModelPathName.Substring(strModelPathName.LastIndexOf(".") + 1, strModelPathName.Length - strModelPathName.LastIndexOf(".") - 1);
			
			switch (strFileExt) {
				case "SLDASM":
					swDocType = swDocumentTypes_e.swDocASSEMBLY;
					break;
				case "SLDPRT":
					swDocType = swDocumentTypes_e.swDocPART;
					break;
				case "SLDDRW":
					swDocType = swDocumentTypes_e.swDocDRAWING;
					break;
				default:
					swDocType = swDocumentTypes_e.swDocNONE;
					break;
			}
			
			return(swDocType);
			
		}			
		
		
		
		void DbWriteConfigs(Component2 swComp, string strPathName, bool blnRoot) {
			
			// Check to see if this files properties have already been written
			cmdCheckFiles.Parameters[0].Value = strPathName;
			int intExists = Convert.ToInt32(cmdCheckFiles.ExecuteScalar().ToString());
			if ( intExists!=0 ) {
				return;
			}
			
			// Prepare to write status label
			MethodInvoker WriteLabelDelegate = new MethodInvoker(WriteLabel);
			string strModelName = this.statLabel;
			
			// Get list of configs
			ModelDoc2 swModelDoc = (ModelDoc2)swComp.GetModelDoc2();
			string[] strConfgNames = (string[])swModelDoc.GetConfigurationNames();
			
			foreach (string strConfigName in strConfgNames) {
				
				// write status to form label
				this.statLabel = strModelName + "(" + strConfigName + ")";
				Invoke(WriteLabelDelegate);
				
				Configuration swConfig = (Configuration)swModelDoc.GetConfigurationByName(strConfigName);
				CustomPropertyManager swCustPropMgr = (CustomPropertyManager)swConfig.CustomPropertyManager;
				
				//swCustPropMgr.GetAll(ref object PropNames, ref object PropTypes, ref object PropValues);
				
				foreach (string strFieldName in aFieldSequence) {
					
					string strPropName = (string)htFieldProp[strFieldName];
					string strVal = null;
					string strResolved = null;
					
					swCustPropMgr.Get4(strPropName, true, out strVal, out strResolved);
					
				}
				
			}
			
		}
		
		
		
		void DbWriteConfig(Component2 swComp, string strPathName, string strConfigName) {
			
			//
			// The first argument is the only difference between this overload method, and
			// the original method.  The only difference in the actual code between the two
			// is the requirement for the following:
			//     ModelDoc2 swModelDoc = (ModelDoc2)swComp.GetModelDoc2();
			// Everything else should be copied exactly.
			//
			
			// Return if the config properties have already been written
			cmdCheckConfigs.Parameters[0].Value = strPathName;
			cmdCheckConfigs.Parameters[1].Value = strConfigName;
			int intExists = Convert.ToInt32(cmdCheckConfigs.ExecuteScalar().ToString());
			if ( intExists!=0 ) {
				return;
			}
			
			// Write status to form label
			MethodInvoker WriteLabelDelegate = new MethodInvoker(WriteLabel);
			string strModelName = this.statLabel;
			this.statLabel = strModelName + "(" + strConfigName + ")" + " ... getting custom properties";
			Invoke(WriteLabelDelegate);
			
			// Get model doc extension
			ModelDoc2 swModelDoc = (ModelDoc2)swComp.GetModelDoc2();
			ModelDocExtension swModelDocExt = (ModelDocExtension)swModelDoc.Extension;
			
			// Get property manager for this config
			CustomPropertyManager swCustPropMgr = (CustomPropertyManager)swModelDocExt.get_CustomPropertyManager(strConfigName);

            // Get property manager for file-system-level (name of standard config = "")
			CustomPropertyManager swAltCustPropMgr = (CustomPropertyManager)swModelDocExt.get_CustomPropertyManager("");

            // Get ShowChildComponensInBOM value
			Configuration swConfig = (Configuration)swModelDoc.GetConfigurationByName(strConfigName);
			int intShowChild;
			if (swConfig.ShowChildComponentsInBOM) {
				intShowChild = 1;
			} else {
				intShowChild = 0;
			}
			
			// Set query parameters
			cmdInsertConfigs.Parameters[0].Value = strPathName;
			cmdInsertConfigs.Parameters[1].Value = strConfigName;
			cmdInsertConfigs.Parameters[2].Value = 0;
			cmdInsertConfigs.Parameters[3].Value = intShowChild;
			for(int i=0; i < aFieldSequence.Length; i++) {
				
				string strPropName = (string)htFieldProp[aFieldSequence[i]];
				string strVal = null;
				string strResolved = null;
				
				swCustPropMgr.Get4(strPropName, false, out strVal, out strResolved);
                strResolved = strResolved.Trim();
				if (strResolved == null || strResolved == "") {
					swAltCustPropMgr.Get4(strPropName, false, out strVal, out strResolved);
                    strResolved = strResolved.Trim();
                }
				cmdInsertConfigs.Parameters[i+4].Value = strResolved;
				
			}
			
			// Execute query
			cmdInsertConfigs.ExecuteNonQuery();
			
		}
		
		
		
		void DbWriteConfig(ModelDoc2 swModelDoc, string strPathName, string strConfigName) {
			
			//
			// The first argument is the only difference between this overload method, and
			// the original method.  The only difference in the actual code between the two
			// is the requirement for the following:
			//     ModelDoc2 swModelDoc = (ModelDoc2)swComp.GetModelDoc2();
			// Everything else should be copied exactly.
			//


            // Return if the config properties have already been written
            cmdCheckConfigs.Parameters[0].Value = strPathName;
            cmdCheckConfigs.Parameters[1].Value = strConfigName;
            int intExists = Convert.ToInt32(cmdCheckConfigs.ExecuteScalar().ToString());
            if (intExists != 0)
            {
                return;
            }

            // Write status to form label
            MethodInvoker WriteLabelDelegate = new MethodInvoker(WriteLabel);
            string strModelName = this.statLabel;
            this.statLabel = strModelName + "(" + strConfigName + ")" + " ... getting custom properties";
            Invoke(WriteLabelDelegate);

            // Get model doc extension
            ModelDocExtension swModelDocExt = (ModelDocExtension)swModelDoc.Extension;

            // Get property manager for this config
            CustomPropertyManager swCustPropMgr = (CustomPropertyManager)swModelDocExt.get_CustomPropertyManager(strConfigName);

            // Get property manager for file-system-level (name of standard config = "")
            CustomPropertyManager swAltCustPropMgr = (CustomPropertyManager)swModelDocExt.get_CustomPropertyManager("");

            // Get ShowChildComponensInBOM value
            Configuration swConfig = (Configuration)swModelDoc.GetConfigurationByName(strConfigName);
            int intShowChild;
            if (swConfig.ShowChildComponentsInBOM)
            {
                intShowChild = 1;
            }
            else
            {
                intShowChild = 0;
            }

            // Set query parameters
            cmdInsertConfigs.Parameters[0].Value = strPathName;
            cmdInsertConfigs.Parameters[1].Value = strConfigName;
            cmdInsertConfigs.Parameters[2].Value = 0;
            cmdInsertConfigs.Parameters[3].Value = intShowChild;
            for (int i = 0; i < aFieldSequence.Length; i++)
            {

                string strPropName = (string)htFieldProp[aFieldSequence[i]];
                string strVal = null;
                string strResolved = null;

                swCustPropMgr.Get4(strPropName, false, out strVal, out strResolved);
                strResolved = strResolved.Trim();
                if (strResolved == null || strResolved == "")
                {
                    swAltCustPropMgr.Get4(strPropName, false, out strVal, out strResolved);
                    strResolved = strResolved.Trim();
                }
                cmdInsertConfigs.Parameters[i + 4].Value = strResolved;

            }

            // Execute query
            cmdInsertConfigs.ExecuteNonQuery();

        }
		
		
		
		void DbWriteAdjacency (string strParentPath, string strParentConfig, string strChildPath, string strChildConfig) {
			
			// Check for existing quantity
			long intCount = 1;
			cmdGetAdjCount.Parameters[0].Value = strParentPath;
			cmdGetAdjCount.Parameters[1].Value = strParentConfig;
			cmdGetAdjCount.Parameters[2].Value = strChildPath;
			cmdGetAdjCount.Parameters[3].Value = strChildConfig;
			object oTest = cmdGetAdjCount.ExecuteScalar();
			
			if (oTest==null) {
				
				cmdInsertAdjacency.Parameters[0].Value = strParentPath;
				cmdInsertAdjacency.Parameters[1].Value = strParentConfig;
				cmdInsertAdjacency.Parameters[2].Value = strChildPath;
				cmdInsertAdjacency.Parameters[3].Value = strChildConfig;
				cmdInsertAdjacency.Parameters[4].Value = intCount;
				cmdInsertAdjacency.ExecuteNonQuery();
				
			} else {
				
				intCount = Convert.ToInt64(oTest);
				intCount++;
				
				cmdUpdateAdjacency.Parameters[0].Value = intCount;
				cmdUpdateAdjacency.Parameters[1].Value = (string)strParentPath;
				cmdUpdateAdjacency.Parameters[2].Value = (string)strParentConfig;
				cmdUpdateAdjacency.Parameters[3].Value = (string)strChildPath;
				cmdUpdateAdjacency.Parameters[4].Value = (string)strChildConfig;
				
				cmdUpdateAdjacency.ExecuteNonQuery();
				
			}
			
			// Write status to form label
			MethodInvoker WriteLabelDelegate = new MethodInvoker(WriteLabel);
			string strModelName = this.statLabel;
			this.statLabel = strModelName + " ... writing adjacency (" + intCount.ToString() + ")";
			Invoke(WriteLabelDelegate);
			
		}
		
		
		
		void InitDB(string fileName) {
			
			cnn = new SQLiteConnection("Data Source=" + fileName);
			cnn.Open();
			
			DbCommand cmd = cnn.CreateCommand();
			
			cmd.CommandText = @"
					CREATE TABLE IF NOT EXISTS configs (
						filename TEXT,
 						configname TEXT,
						PlaceHoldFlag INTEGER,
						ShowChildren INTEGER,
 						
						Uom TEXT,
						PartNum TEXT,
						Description TEXT,
						DesignedBy TEXT,
						DrawDate TEXT,
						Type TEXT,
						AltQty REAL,
						EngApproval TEXT,
						EngApprDate TEXT,
						MfgApproval TEXT,
						MfgApprDate TEXT,
						QaApproval TEXT,
						QaApprDate TEXT,
						PurchApproval TEXT,
						PurchApprDate TEXT,
						Material TEXT,
						Finish TEXT,
						Coating TEXT,
						Notes TEXT,
						Revision TEXT,
						Ecos TEXT,
						EcoRevs TEXT,
						Zone TEXT,
						EcoDescriptions TEXT,
						EcoDates TEXT,
						EcoChks TEXT,
						Catalog TEXT,
 						
						PRIMARY KEY (filename,configname)
						
					)
				";
			cmd.ExecuteNonQuery();
			
			cmd.CommandText = @"
					CREATE TABLE IF NOT EXISTS adjacency (
						pname TEXT,
						pconfig TEXT,
						cname TEXT,
						cconfig TEXT,
						adjcount INTEGER,
						
						PRIMARY KEY (pname,pconfig,cname,cconfig),
						
						FOREIGN KEY (pname) REFERENCES configs(filename),
						FOREIGN KEY (pconfig) REFERENCES configs(configname),
						FOREIGN KEY (cname) REFERENCES configs(filename),
						FOREIGN KEY (cconfig) REFERENCES configs(configname)
					)
				";
			cmd.ExecuteNonQuery();
			
			
			// Common????
				SQLiteParameter sqpFileName = new SQLiteParameter();
				SQLiteParameter sqpFileType = new SQLiteParameter();
				SQLiteParameter sqpConfigName = new SQLiteParameter();
				
				SQLiteParameter sqpPlaceHoldFlag = new SQLiteParameter();
				SQLiteParameter sqpShowChildren = new SQLiteParameter();
				SQLiteParameter sqpUom = new SQLiteParameter();
				SQLiteParameter sqpPartNum = new SQLiteParameter();
				SQLiteParameter sqpDescription = new SQLiteParameter();
				SQLiteParameter sqpDesignedBy = new SQLiteParameter();
				SQLiteParameter sqpDrawDate = new SQLiteParameter();
				SQLiteParameter sqpType = new SQLiteParameter();
				SQLiteParameter sqpAltQty = new SQLiteParameter();
				SQLiteParameter sqpEngApproval = new SQLiteParameter();
				SQLiteParameter sqpEngApprDate = new SQLiteParameter();
				SQLiteParameter sqpMfgApproval = new SQLiteParameter();
				SQLiteParameter sqpMfgApprDate = new SQLiteParameter();
				SQLiteParameter sqpQaApproval = new SQLiteParameter();
				SQLiteParameter sqpQaApprDate = new SQLiteParameter();
				SQLiteParameter sqpPurchApproval = new SQLiteParameter();
				SQLiteParameter sqpPurchApprDate = new SQLiteParameter();
				SQLiteParameter sqpMaterial = new SQLiteParameter();
				SQLiteParameter sqpFinish = new SQLiteParameter();
				SQLiteParameter sqpCoating = new SQLiteParameter();
				SQLiteParameter sqpNotes = new SQLiteParameter();
				SQLiteParameter sqpRevision = new SQLiteParameter();
				SQLiteParameter sqpEcos = new SQLiteParameter();
				SQLiteParameter sqpEcoRevs = new SQLiteParameter();
				SQLiteParameter sqpZone = new SQLiteParameter();
				SQLiteParameter sqpEcoDescriptions = new SQLiteParameter();
				SQLiteParameter sqpEcoDates = new SQLiteParameter();
				SQLiteParameter sqpEcoChks = new SQLiteParameter();
				SQLiteParameter sqpCatalog = new SQLiteParameter();
				
				SQLiteParameter sqpPName = new SQLiteParameter();
				SQLiteParameter sqpPConfig = new SQLiteParameter();
				SQLiteParameter sqpCName = new SQLiteParameter();
				SQLiteParameter sqpCConfig = new SQLiteParameter();
				SQLiteParameter sqpCount = new SQLiteParameter();
				
			
			cmdCheckFiles = cnn.CreateCommand();
			cmdCheckFiles.CommandText = "SELECT COUNT(*) FROM Configs where filename like ?";
				cmdCheckFiles.Parameters.Add(sqpFileName);
			
			cmdCheckConfigs = cnn.CreateCommand();
			cmdCheckConfigs.CommandText = "SELECT COUNT(*) FROM Configs where filename like ? and configname like ?";
				cmdCheckConfigs.Parameters.Add(sqpFileName);
				cmdCheckConfigs.Parameters.Add(sqpConfigName);
			
			cmdShowChildren = cnn.CreateCommand();
			cmdShowChildren.CommandText = "SELECT ShowChildren FROM Configs where filename like ? and configname like ?";
				cmdShowChildren.Parameters.Add(sqpFileName);
				cmdShowChildren.Parameters.Add(sqpConfigName);
			
			cmdInsertConfigs = cnn.CreateCommand();
			cmdInsertConfigs.CommandText = "INSERT INTO configs VALUES(?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
				cmdInsertConfigs.Parameters.Add(sqpFileName);
				cmdInsertConfigs.Parameters.Add(sqpConfigName);
				cmdInsertConfigs.Parameters.Add(sqpPlaceHoldFlag);
				cmdInsertConfigs.Parameters.Add(sqpShowChildren);
				cmdInsertConfigs.Parameters.Add(sqpUom);
				cmdInsertConfigs.Parameters.Add(sqpPartNum);
				cmdInsertConfigs.Parameters.Add(sqpDescription);
				cmdInsertConfigs.Parameters.Add(sqpDesignedBy);
				cmdInsertConfigs.Parameters.Add(sqpDrawDate);
				cmdInsertConfigs.Parameters.Add(sqpType);
				cmdInsertConfigs.Parameters.Add(sqpAltQty);
				cmdInsertConfigs.Parameters.Add(sqpEngApproval);
				cmdInsertConfigs.Parameters.Add(sqpEngApprDate);
				cmdInsertConfigs.Parameters.Add(sqpMfgApproval);
				cmdInsertConfigs.Parameters.Add(sqpMfgApprDate);
				cmdInsertConfigs.Parameters.Add(sqpQaApproval);
				cmdInsertConfigs.Parameters.Add(sqpQaApprDate);
				cmdInsertConfigs.Parameters.Add(sqpPurchApproval);
				cmdInsertConfigs.Parameters.Add(sqpPurchApprDate);
				cmdInsertConfigs.Parameters.Add(sqpMaterial);
				cmdInsertConfigs.Parameters.Add(sqpFinish);
				cmdInsertConfigs.Parameters.Add(sqpCoating);
				cmdInsertConfigs.Parameters.Add(sqpNotes);
				cmdInsertConfigs.Parameters.Add(sqpRevision);
				cmdInsertConfigs.Parameters.Add(sqpEcos);
				cmdInsertConfigs.Parameters.Add(sqpEcoRevs);
				cmdInsertConfigs.Parameters.Add(sqpZone);
				cmdInsertConfigs.Parameters.Add(sqpEcoDescriptions);
				cmdInsertConfigs.Parameters.Add(sqpEcoDates);
				cmdInsertConfigs.Parameters.Add(sqpEcoChks);
				cmdInsertConfigs.Parameters.Add(sqpCatalog);
			
			cmdInsertAdjacency = cnn.CreateCommand();
			cmdInsertAdjacency.CommandText = "INSERT INTO adjacency VALUES(?,?,?,?,?)";
				cmdInsertAdjacency.Parameters.Add(sqpPName);
				cmdInsertAdjacency.Parameters.Add(sqpPConfig);
				cmdInsertAdjacency.Parameters.Add(sqpCName);
				cmdInsertAdjacency.Parameters.Add(sqpCConfig);
				cmdInsertAdjacency.Parameters.Add(sqpCount);
			
			cmdUpdateAdjacency = cnn.CreateCommand();
			cmdUpdateAdjacency.CommandText = @"
					UPDATE adjacency SET adjcount=?
					WHERE pname like ?
					AND pconfig like ?
					AND cname like ?
					AND cconfig like ?
				";
				cmdUpdateAdjacency.Parameters.Add(sqpCount);
				cmdUpdateAdjacency.Parameters.Add(sqpPName);
				cmdUpdateAdjacency.Parameters.Add(sqpPConfig);
				cmdUpdateAdjacency.Parameters.Add(sqpCName);
				cmdUpdateAdjacency.Parameters.Add(sqpCConfig);
			
			cmdGetAdjCount = cnn.CreateCommand();
			cmdGetAdjCount.CommandText = @"
					SELECT adjcount FROM adjacency
					WHERE pname like ?
					AND pconfig like ?
					AND cname like ?
					AND cconfig like ?
				";
				cmdGetAdjCount.Parameters.Add(sqpPName);
				cmdGetAdjCount.Parameters.Add(sqpPConfig);
				cmdGetAdjCount.Parameters.Add(sqpCName);
				cmdGetAdjCount.Parameters.Add(sqpCConfig);
			
			cmdCheckAdjacency = cnn.CreateCommand();
			cmdCheckAdjacency.CommandText = "SELECT count(*) FROM adjacency WHERE pname like ? AND pconfig like ?";
				cmdCheckAdjacency.Parameters.Add(sqpPName);
				cmdCheckAdjacency.Parameters.Add(sqpPConfig);
			
		}
		
		
		
		
		
	}
}
