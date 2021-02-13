/*
 * Создано в SharpDevelop.
 * Пользователь: user
 * Дата: 11.12.2020
 * Время: 15:59
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
namespace DXC
{
	partial class EditDxc
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.ComboBox comboBox1;
		
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.btAddDXC = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.tbName = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.tbIP = new System.Windows.Forms.TextBox();
			this.tbSysName = new System.Windows.Forms.TextBox();
			this.tbSYNC = new System.Windows.Forms.TextBox();
			this.tbBackup = new System.Windows.Forms.TextBox();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.Slot = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Port = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Direction = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Monitored = new System.Windows.Forms.DataGridViewCheckBoxColumn();
			this.lbInfo = new System.Windows.Forms.Label();
			this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
			this.SuspendLayout();
			// 
			// comboBox1
			// 
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(12, 12);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(121, 21);
			this.comboBox1.TabIndex = 0;
			// 
			// btAddDXC
			// 
			this.btAddDXC.Location = new System.Drawing.Point(157, 9);
			this.btAddDXC.Name = "btAddDXC";
			this.btAddDXC.Size = new System.Drawing.Size(139, 23);
			this.btAddDXC.TabIndex = 1;
			this.btAddDXC.Text = "Добавить новое DXC";
			this.btAddDXC.UseVisualStyleBackColor = true;
			this.btAddDXC.Click += new System.EventHandler(this.BtAddDxcClick);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.button1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.button1.Location = new System.Drawing.Point(12, 474);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(124, 40);
			this.button1.TabIndex = 2;
			this.button1.Text = "Сохранить выбранное";
			this.button1.UseVisualStyleBackColor = false;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// button2
			// 
			this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.button2.Location = new System.Drawing.Point(412, 474);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(123, 40);
			this.button2.TabIndex = 3;
			this.button2.Text = "Отмена";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Click += new System.EventHandler(this.Button2Click);
			// 
			// tbName
			// 
			this.tbName.Location = new System.Drawing.Point(180, 44);
			this.tbName.Name = "tbName";
			this.tbName.Size = new System.Drawing.Size(133, 20);
			this.tbName.TabIndex = 4;
			this.tbName.TextChanged += new System.EventHandler(this.TbNameTextChanged);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label1.Location = new System.Drawing.Point(12, 44);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(151, 35);
			this.label1.TabIndex = 5;
			this.label1.Text = "Название:";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label2.Location = new System.Drawing.Point(12, 67);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(151, 35);
			this.label2.TabIndex = 5;
			this.label2.Text = "IP Address:";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label3.Location = new System.Drawing.Point(12, 90);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(151, 23);
			this.label3.TabIndex = 5;
			this.label3.Text = "Системное имя:";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label4.Location = new System.Drawing.Point(12, 113);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(151, 35);
			this.label4.TabIndex = 5;
			this.label4.Text = "Синхронизация:";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.label5.Location = new System.Drawing.Point(12, 136);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(151, 35);
			this.label5.TabIndex = 5;
			this.label5.Text = "Backup:";
			// 
			// tbIP
			// 
			this.tbIP.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tbIP.ForeColor = System.Drawing.Color.Black;
			this.tbIP.Location = new System.Drawing.Point(180, 67);
			this.tbIP.Name = "tbIP";
			this.tbIP.ReadOnly = true;
			this.tbIP.Size = new System.Drawing.Size(133, 20);
			this.tbIP.TabIndex = 4;
			// 
			// tbSysName
			// 
			this.tbSysName.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tbSysName.ForeColor = System.Drawing.Color.Black;
			this.tbSysName.Location = new System.Drawing.Point(180, 89);
			this.tbSysName.Name = "tbSysName";
			this.tbSysName.ReadOnly = true;
			this.tbSysName.Size = new System.Drawing.Size(133, 20);
			this.tbSysName.TabIndex = 4;
			// 
			// tbSYNC
			// 
			this.tbSYNC.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tbSYNC.ForeColor = System.Drawing.Color.Black;
			this.tbSYNC.Location = new System.Drawing.Point(180, 113);
			this.tbSYNC.Name = "tbSYNC";
			this.tbSYNC.ReadOnly = true;
			this.tbSYNC.Size = new System.Drawing.Size(133, 20);
			this.tbSYNC.TabIndex = 4;
			// 
			// tbBackup
			// 
			this.tbBackup.BackColor = System.Drawing.Color.WhiteSmoke;
			this.tbBackup.ForeColor = System.Drawing.Color.Black;
			this.tbBackup.Location = new System.Drawing.Point(180, 136);
			this.tbBackup.Name = "tbBackup";
			this.tbBackup.ReadOnly = true;
			this.tbBackup.Size = new System.Drawing.Size(355, 20);
			this.tbBackup.TabIndex = 4;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.AutoGenerateColumns = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.Slot,
									this.Port,
									this.Direction,
									this.Monitored});
			this.dataGridView1.DataSource = this.bindingSource1;
			this.dataGridView1.Location = new System.Drawing.Point(12, 162);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
			this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.dataGridView1.Size = new System.Drawing.Size(523, 294);
			this.dataGridView1.TabIndex = 6;
			// 
			// Slot
			// 
			this.Slot.DataPropertyName = "BordNumber";
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
			this.Slot.DefaultCellStyle = dataGridViewCellStyle1;
			this.Slot.HeaderText = "Карта";
			this.Slot.Name = "Slot";
			this.Slot.Width = 70;
			// 
			// Port
			// 
			this.Port.DataPropertyName = "PortNumber";
			this.Port.HeaderText = "Порт";
			this.Port.Name = "Port";
			this.Port.Width = 70;
			// 
			// Direction
			// 
			this.Direction.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Direction.DataPropertyName = "Name";
			this.Direction.HeaderText = "Направление";
			this.Direction.MinimumWidth = 200;
			this.Direction.Name = "Direction";
			this.Direction.Width = 200;
			// 
			// Monitored
			// 
			this.Monitored.DataPropertyName = "Monitored";
			this.Monitored.HeaderText = "Оповещение об аварии";
			this.Monitored.Name = "Monitored";
			this.Monitored.ToolTipText = "Звуковая сигнализация при появлении аварии";
			this.Monitored.Width = 80;
			// 
			// lbInfo
			// 
			this.lbInfo.ForeColor = System.Drawing.Color.Red;
			this.lbInfo.Location = new System.Drawing.Point(320, 42);
			this.lbInfo.Name = "lbInfo";
			this.lbInfo.Size = new System.Drawing.Size(187, 20);
			this.lbInfo.TabIndex = 7;
			this.lbInfo.Text = "..";
			this.lbInfo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
			// 
			// EditDXC
			// 
			this.AcceptButton = this.button1;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(550, 526);
			this.Controls.Add(this.lbInfo);
			this.Controls.Add(this.dataGridView1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.tbBackup);
			this.Controls.Add(this.tbSYNC);
			this.Controls.Add(this.tbSysName);
			this.Controls.Add(this.tbIP);
			this.Controls.Add(this.tbName);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.btAddDXC);
			this.Controls.Add(this.comboBox1);
			this.Name = "EditDxc";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "EditDXC";
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		public System.Windows.Forms.BindingSource bindingSource1;
		private System.Windows.Forms.Label lbInfo;
		private System.Windows.Forms.DataGridViewCheckBoxColumn Monitored;
		private System.Windows.Forms.DataGridViewTextBoxColumn Direction;
		private System.Windows.Forms.DataGridViewTextBoxColumn Port;
		private System.Windows.Forms.DataGridViewTextBoxColumn Slot;
		private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
		public System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.TextBox tbBackup;
		private System.Windows.Forms.TextBox tbSYNC;
		private System.Windows.Forms.TextBox tbSysName;
		private System.Windows.Forms.TextBox tbIP;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox tbName;
		private System.Windows.Forms.Button btAddDXC;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
	}
}
