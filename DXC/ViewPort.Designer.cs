namespace DXC
{
    partial class ViewPort
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
        	this.listBox1 = new System.Windows.Forms.ListBox();
        	this.dataGridView1 = new System.Windows.Forms.DataGridView();
        	this.ts = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	this.arrow = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	this.bord = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	this.port = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	this.tts = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
        	((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
        	this.SuspendLayout();
        	// 
        	// listBox1
        	// 
        	this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.listBox1.FormattingEnabled = true;
        	this.listBox1.Location = new System.Drawing.Point(2, 2);
        	this.listBox1.Name = "listBox1";
        	this.listBox1.Size = new System.Drawing.Size(312, 56);
        	this.listBox1.TabIndex = 0;
        	this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
        	// 
        	// dataGridView1
        	// 
        	this.dataGridView1.AllowUserToAddRows = false;
        	this.dataGridView1.AllowUserToDeleteRows = false;
        	this.dataGridView1.AllowUserToResizeColumns = false;
        	this.dataGridView1.AllowUserToResizeRows = false;
        	this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
        	        	        	| System.Windows.Forms.AnchorStyles.Left) 
        	        	        	| System.Windows.Forms.AnchorStyles.Right)));
        	this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
        	this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        	this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        	this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
        	        	        	this.ts,
        	        	        	this.arrow,
        	        	        	this.bord,
        	        	        	this.port,
        	        	        	this.tts,
        	        	        	this.status});
        	this.dataGridView1.Location = new System.Drawing.Point(2, 64);
        	this.dataGridView1.Name = "dataGridView1";
        	this.dataGridView1.ReadOnly = true;
        	this.dataGridView1.RowHeadersVisible = false;
        	this.dataGridView1.Size = new System.Drawing.Size(312, 648);
        	this.dataGridView1.TabIndex = 1;
        	// 
        	// ts
        	// 
        	this.ts.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
        	this.ts.HeaderText = "ts";
        	this.ts.MinimumWidth = 10;
        	this.ts.Name = "ts";
        	this.ts.ReadOnly = true;
        	this.ts.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        	this.ts.ToolTipText = "ts";
        	this.ts.Width = 40;
        	// 
        	// arrow
        	// 
        	this.arrow.HeaderText = "<-->";
        	this.arrow.MinimumWidth = 20;
        	this.arrow.Name = "arrow";
        	this.arrow.ReadOnly = true;
        	this.arrow.Resizable = System.Windows.Forms.DataGridViewTriState.False;
        	this.arrow.Width = 50;
        	// 
        	// bord
        	// 
        	this.bord.HeaderText = "Карта";
        	this.bord.Name = "bord";
        	this.bord.ReadOnly = true;
        	this.bord.Width = 40;
        	// 
        	// port
        	// 
        	this.port.HeaderText = "Порт";
        	this.port.Name = "port";
        	this.port.ReadOnly = true;
        	this.port.Width = 35;
        	// 
        	// tts
        	// 
        	this.tts.HeaderText = "ts";
        	this.tts.Name = "tts";
        	this.tts.ReadOnly = true;
        	this.tts.Width = 30;
        	// 
        	// status
        	// 
        	this.status.HeaderText = "Состояние";
        	this.status.Name = "status";
        	this.status.ReadOnly = true;
        	// 
        	// ViewPort
        	// 
        	this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        	this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        	this.ClientSize = new System.Drawing.Size(317, 716);
        	this.Controls.Add(this.dataGridView1);
        	this.Controls.Add(this.listBox1);
        	this.Name = "ViewPort";
        	this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        	this.Text = "ViewPort";
        	((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
        	this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ts;
        private System.Windows.Forms.DataGridViewTextBoxColumn arrow;
        private System.Windows.Forms.DataGridViewTextBoxColumn bord;
        private System.Windows.Forms.DataGridViewTextBoxColumn port;
        private System.Windows.Forms.DataGridViewTextBoxColumn tts;
        private System.Windows.Forms.DataGridViewTextBoxColumn status;
    }
}