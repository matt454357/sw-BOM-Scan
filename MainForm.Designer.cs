/*
 * Created by SharpDevelop.
 * User: mtaylor
 * Date: 1/6/2010
 * Time: 9:10 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace sw_BOM_Scan
{
	partial class MainForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.txtActiveDoc = new System.Windows.Forms.TextBox();
			this.lblActiveDoc = new System.Windows.Forms.Label();
			this.lblTargetFile = new System.Windows.Forms.Label();
			this.txtTargetFile = new System.Windows.Forms.TextBox();
			this.cmdTargetFile = new System.Windows.Forms.Button();
			this.cmdScan = new System.Windows.Forms.Button();
			this.lblStatus = new System.Windows.Forms.Label();
			this.txtStatus = new System.Windows.Forms.TextBox();
			this.cmdRefresh = new System.Windows.Forms.Button();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.cmdCancel = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// txtActiveDoc
			// 
			this.txtActiveDoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtActiveDoc.BackColor = System.Drawing.SystemColors.Control;
			this.txtActiveDoc.Location = new System.Drawing.Point(89, 9);
			this.txtActiveDoc.Name = "txtActiveDoc";
			this.txtActiveDoc.Size = new System.Drawing.Size(306, 20);
			this.txtActiveDoc.TabIndex = 0;
			// 
			// lblActiveDoc
			// 
			this.lblActiveDoc.Location = new System.Drawing.Point(12, 9);
			this.lblActiveDoc.Name = "lblActiveDoc";
			this.lblActiveDoc.Size = new System.Drawing.Size(71, 20);
			this.lblActiveDoc.TabIndex = 1;
			this.lblActiveDoc.Text = "Active Doc";
			this.lblActiveDoc.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTargetFile
			// 
			this.lblTargetFile.Location = new System.Drawing.Point(12, 35);
			this.lblTargetFile.Name = "lblTargetFile";
			this.lblTargetFile.Size = new System.Drawing.Size(71, 20);
			this.lblTargetFile.TabIndex = 3;
			this.lblTargetFile.Text = "Target File";
			this.lblTargetFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtTargetFile
			// 
			this.txtTargetFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtTargetFile.BackColor = System.Drawing.SystemColors.Control;
			this.txtTargetFile.Location = new System.Drawing.Point(89, 35);
			this.txtTargetFile.Name = "txtTargetFile";
			this.txtTargetFile.Size = new System.Drawing.Size(306, 20);
			this.txtTargetFile.TabIndex = 2;
			// 
			// cmdTargetFile
			// 
			this.cmdTargetFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdTargetFile.Location = new System.Drawing.Point(401, 34);
			this.cmdTargetFile.Margin = new System.Windows.Forms.Padding(0);
			this.cmdTargetFile.Name = "cmdTargetFile";
			this.cmdTargetFile.Size = new System.Drawing.Size(62, 22);
			this.cmdTargetFile.TabIndex = 4;
			this.cmdTargetFile.Text = "SetTarget";
			this.cmdTargetFile.UseVisualStyleBackColor = true;
			this.cmdTargetFile.Click += new System.EventHandler(this.CmdTargetFileClick);
			// 
			// cmdScan
			// 
			this.cmdScan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdScan.Location = new System.Drawing.Point(401, 73);
			this.cmdScan.Margin = new System.Windows.Forms.Padding(0);
			this.cmdScan.Name = "cmdScan";
			this.cmdScan.Size = new System.Drawing.Size(62, 22);
			this.cmdScan.TabIndex = 7;
			this.cmdScan.Text = "Scan";
			this.cmdScan.UseVisualStyleBackColor = true;
			this.cmdScan.Click += new System.EventHandler(this.CmdScanClick);
			// 
			// lblStatus
			// 
			this.lblStatus.Location = new System.Drawing.Point(12, 75);
			this.lblStatus.Name = "lblStatus";
			this.lblStatus.Size = new System.Drawing.Size(71, 20);
			this.lblStatus.TabIndex = 6;
			this.lblStatus.Text = "Scan Status";
			this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtStatus
			// 
			this.txtStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.txtStatus.BackColor = System.Drawing.SystemColors.Control;
			this.txtStatus.Location = new System.Drawing.Point(89, 75);
			this.txtStatus.Name = "txtStatus";
			this.txtStatus.Size = new System.Drawing.Size(306, 20);
			this.txtStatus.TabIndex = 5;
			// 
			// cmdRefresh
			// 
			this.cmdRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdRefresh.Location = new System.Drawing.Point(401, 7);
			this.cmdRefresh.Margin = new System.Windows.Forms.Padding(0);
			this.cmdRefresh.Name = "cmdRefresh";
			this.cmdRefresh.Size = new System.Drawing.Size(62, 22);
			this.cmdRefresh.TabIndex = 8;
			this.cmdRefresh.Text = "Refresh";
			this.cmdRefresh.UseVisualStyleBackColor = true;
			this.cmdRefresh.Click += new System.EventHandler(this.CmdRefreshClick);
			// 
			// saveFileDialog1
			// 
			this.saveFileDialog1.DefaultExt = "db3";
			this.saveFileDialog1.Filter = "SQLLite Database (*.db3)|*.db3|All Files (*.*)|*.*";
			this.saveFileDialog1.OverwritePrompt = false;
			this.saveFileDialog1.Title = "New Target Data File";
			// 
			// cmdCancel
			// 
			this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdCancel.Location = new System.Drawing.Point(401, 73);
			this.cmdCancel.Margin = new System.Windows.Forms.Padding(0);
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.Size = new System.Drawing.Size(62, 22);
			this.cmdCancel.TabIndex = 9;
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.UseVisualStyleBackColor = true;
			this.cmdCancel.Click += new System.EventHandler(this.CmdCancelClick);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(477, 106);
			this.Controls.Add(this.cmdRefresh);
			this.Controls.Add(this.lblStatus);
			this.Controls.Add(this.txtStatus);
			this.Controls.Add(this.cmdTargetFile);
			this.Controls.Add(this.lblTargetFile);
			this.Controls.Add(this.txtTargetFile);
			this.Controls.Add(this.lblActiveDoc);
			this.Controls.Add(this.txtActiveDoc);
			this.Controls.Add(this.cmdScan);
			this.Controls.Add(this.cmdCancel);
			this.Name = "MainForm";
			this.Text = "sw_BOM_Scan";
			this.Load += new System.EventHandler(this.MainFormLoad);
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button cmdCancel;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Button cmdRefresh;
		private System.Windows.Forms.Button cmdScan;
		private System.Windows.Forms.Label lblStatus;
		private System.Windows.Forms.TextBox txtStatus;
		private System.Windows.Forms.Button cmdTargetFile;
		private System.Windows.Forms.TextBox txtTargetFile;
		private System.Windows.Forms.Label lblTargetFile;
		private System.Windows.Forms.Label lblActiveDoc;
		private System.Windows.Forms.TextBox txtActiveDoc;
	}
}
