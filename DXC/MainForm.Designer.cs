/*
 * Created by SharpDevelop.
 * User: Ситал
 * Date: 20.02.2017
 * Time: 15:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace DXC
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbInterval = new System.Windows.Forms.TextBox();
			this.button5 = new System.Windows.Forms.Button();
			this.lbAll = new System.Windows.Forms.ListBox();
			this.button6 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.lbProgress = new System.Windows.Forms.ToolStripStatusLabel();
			this.ProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.lbProgressAfter = new System.Windows.Forms.ToolStripStatusLabel();
			this.lbAlmCount = new System.Windows.Forms.ToolStripStatusLabel();
			this.dataGridView1 = new System.Windows.Forms.DataGridView();
			this.bord = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.port = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Direction = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.start = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.end = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.status = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.contextPorts = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.свойствоПортаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.файлToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.выбратьПапкуДляBackupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.списокDXCToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.выходToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
			this.timerProgress = new System.Windows.Forms.Timer(this.components);
			this.button7 = new System.Windows.Forms.Button();
			this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
			this.contextPorts.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
			this.SuspendLayout();
			// 
			// splitContainer1
			// 
			this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.splitContainer1.Location = new System.Drawing.Point(0, 27);
			this.splitContainer1.Margin = new System.Windows.Forms.Padding(1);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.checkBox1);
			this.splitContainer1.Panel1.Controls.Add(this.label3);
			this.splitContainer1.Panel1.Controls.Add(this.label2);
			this.splitContainer1.Panel1.Controls.Add(this.tbInterval);
			this.splitContainer1.Panel1.Controls.Add(this.button5);
			this.splitContainer1.Panel1.Controls.Add(this.lbAll);
			this.splitContainer1.Panel1.Controls.Add(this.button6);
			this.splitContainer1.Panel1.Controls.Add(this.button4);
			this.splitContainer1.Panel1.Controls.Add(this.button3);
			this.splitContainer1.Panel1.Controls.Add(this.button2);
			this.splitContainer1.Panel1.Controls.Add(this.button1);
			this.splitContainer1.Panel1.Controls.Add(this.label1);
			this.splitContainer1.Panel1MinSize = 120;
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.statusStrip1);
			this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
			this.splitContainer1.Panel2.Controls.Add(this.listBox1);
			this.splitContainer1.Panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.Panel2Paint);
			this.splitContainer1.Panel2MinSize = 500;
			this.splitContainer1.Size = new System.Drawing.Size(1284, 694);
			this.splitContainer1.SplitterDistance = 156;
			this.splitContainer1.TabIndex = 2;
			this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.SplitContainer1SplitterMoved);
			// 
			// checkBox1
			// 
			this.checkBox1.Checked = true;
			this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
			this.checkBox1.Location = new System.Drawing.Point(13, 360);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(119, 42);
			this.checkBox1.TabIndex = 15;
			this.checkBox1.Text = "Корректировать время аварий";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.CheckBox1CheckedChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(85, 245);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(58, 21);
			this.label3.TabIndex = 14;
			this.label3.Text = "секунд";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(13, 224);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(130, 21);
			this.label2.TabIndex = 14;
			this.label2.Text = "Интервал опроса:";
			// 
			// tbInterval
			// 
			this.tbInterval.Location = new System.Drawing.Point(13, 244);
			this.tbInterval.Name = "tbInterval";
			this.tbInterval.Size = new System.Drawing.Size(74, 20);
			this.tbInterval.TabIndex = 13;
			// 
			// button5
			// 
			this.button5.BackColor = System.Drawing.Color.LightBlue;
			this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.button5.Location = new System.Drawing.Point(13, 156);
			this.button5.Name = "button5";
			this.button5.Size = new System.Drawing.Size(138, 65);
			this.button5.TabIndex = 12;
			this.button5.Text = "Запуск мониторинга аварий";
			this.button5.UseVisualStyleBackColor = false;
			this.button5.Click += new System.EventHandler(this.Button5Click);
			// 
			// lbAll
			// 
			this.lbAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lbAll.FormattingEnabled = true;
			this.lbAll.Location = new System.Drawing.Point(13, 28);
			this.lbAll.Name = "lbAll";
			this.lbAll.Size = new System.Drawing.Size(138, 121);
			this.lbAll.TabIndex = 11;
			// 
			// button6
			// 
			this.button6.Location = new System.Drawing.Point(15, 409);
			this.button6.Name = "button6";
			this.button6.Size = new System.Drawing.Size(75, 23);
			this.button6.TabIndex = 10;
			this.button6.Text = "test2";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Visible = false;
			this.button6.Click += new System.EventHandler(this.Button6Click);
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(12, 438);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 23);
			this.button4.TabIndex = 10;
			this.button4.Text = "test1";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Visible = false;
			this.button4.Click += new System.EventHandler(this.Button4Click);
			// 
			// button3
			// 
			this.button3.BackColor = System.Drawing.Color.MistyRose;
			this.button3.Location = new System.Drawing.Point(8, 272);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(140, 38);
			this.button3.TabIndex = 6;
			this.button3.Text = "Обновить аварии DXC";
			this.button3.UseVisualStyleBackColor = false;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button2
			// 
			this.button2.BackColor = System.Drawing.Color.Beige;
			this.button2.Enabled = false;
			this.button2.Location = new System.Drawing.Point(11, 467);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(132, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "Обновить DXC info";
			this.button2.UseVisualStyleBackColor = false;
			this.button2.Visible = false;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.button1.Location = new System.Drawing.Point(12, 665);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(100, 26);
			this.button1.TabIndex = 4;
			this.button1.Text = "Backup";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(100, 16);
			this.label1.TabIndex = 2;
			this.label1.Text = "Список DXC";
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.lbProgress,
									this.ProgressBar1,
									this.lbProgressAfter,
									this.lbAlmCount});
			this.statusStrip1.Location = new System.Drawing.Point(0, 671);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.statusStrip1.Size = new System.Drawing.Size(1124, 23);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// lbProgress
			// 
			this.lbProgress.Name = "lbProgress";
			this.lbProgress.Size = new System.Drawing.Size(193, 18);
			this.lbProgress.Text = "Мониторинг аварий не активен";
			// 
			// ProgressBar1
			// 
			this.ProgressBar1.ForeColor = System.Drawing.Color.LimeGreen;
			this.ProgressBar1.Name = "ProgressBar1";
			this.ProgressBar1.Size = new System.Drawing.Size(200, 17);
			this.ProgressBar1.Step = 1;
			this.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			// 
			// lbProgressAfter
			// 
			this.lbProgressAfter.Name = "lbProgressAfter";
			this.lbProgressAfter.Size = new System.Drawing.Size(22, 18);
			this.lbProgressAfter.Text = "__";
			// 
			// lbAlmCount
			// 
			this.lbAlmCount.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.lbAlmCount.ForeColor = System.Drawing.Color.DarkRed;
			this.lbAlmCount.Name = "lbAlmCount";
			this.lbAlmCount.Size = new System.Drawing.Size(152, 18);
			this.lbAlmCount.Text = "Активных аварий: ";
			this.lbAlmCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// dataGridView1
			// 
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.dataGridView1.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
									this.bord,
									this.port,
									this.Direction,
									this.name,
									this.start,
									this.end,
									this.status});
			this.dataGridView1.Location = new System.Drawing.Point(354, 0);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.ReadOnly = true;
			this.dataGridView1.RowHeadersVisible = false;
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightGreen;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
			this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle1;
			this.dataGridView1.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.White;
			this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.dataGridView1.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
			this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Size = new System.Drawing.Size(767, 666);
			this.dataGridView1.TabIndex = 1;
			// 
			// bord
			// 
			this.bord.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.bord.HeaderText = "Карта";
			this.bord.Name = "bord";
			this.bord.ReadOnly = true;
			this.bord.Width = 62;
			// 
			// port
			// 
			this.port.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
			this.port.HeaderText = "Порт";
			this.port.Name = "port";
			this.port.ReadOnly = true;
			this.port.Width = 57;
			// 
			// Direction
			// 
			this.Direction.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Direction.HeaderText = "Направление";
			this.Direction.MinimumWidth = 200;
			this.Direction.Name = "Direction";
			this.Direction.ReadOnly = true;
			this.Direction.Width = 200;
			// 
			// name
			// 
			this.name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.name.HeaderText = "Название аварии";
			this.name.MinimumWidth = 150;
			this.name.Name = "name";
			this.name.ReadOnly = true;
			this.name.Width = 150;
			// 
			// start
			// 
			this.start.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.start.HeaderText = "Начало";
			this.start.MinimumWidth = 100;
			this.start.Name = "start";
			this.start.ReadOnly = true;
			// 
			// end
			// 
			this.end.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.end.HeaderText = "Конец";
			this.end.Name = "end";
			this.end.ReadOnly = true;
			this.end.Width = 63;
			// 
			// status
			// 
			this.status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.status.HeaderText = "Состояние";
			this.status.Name = "status";
			this.status.ReadOnly = true;
			this.status.Width = 86;
			// 
			// listBox1
			// 
			this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left)));
			this.listBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
			this.listBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listBox1.ContextMenuStrip = this.contextPorts;
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(0, 0);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(348, 665);
			this.listBox1.TabIndex = 0;
			// 
			// contextPorts
			// 
			this.contextPorts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.свойствоПортаToolStripMenuItem,
									this.toolStripSeparator1,
									this.toolStripMenuItem2,
									this.toolStripTextBox1});
			this.contextPorts.Name = "contextPorts";
			this.contextPorts.Size = new System.Drawing.Size(341, 100);
			// 
			// свойствоПортаToolStripMenuItem
			// 
			this.свойствоПортаToolStripMenuItem.Name = "свойствоПортаToolStripMenuItem";
			this.свойствоПортаToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
			this.свойствоПортаToolStripMenuItem.Text = "Свойства порта...";
			this.свойствоПортаToolStripMenuItem.Click += new System.EventHandler(this.свойствоПортаToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(187, 6);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(340, 22);
			this.toolStripMenuItem2.Text = "Переименовать порт";
			// 
			// toolStripTextBox1
			// 
			this.toolStripTextBox1.BackColor = System.Drawing.SystemColors.Info;
			this.toolStripTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.toolStripTextBox1.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.toolStripTextBox1.ForeColor = System.Drawing.Color.DarkBlue;
			this.toolStripTextBox1.Name = "toolStripTextBox1";
			this.toolStripTextBox1.Size = new System.Drawing.Size(280, 22);
			this.toolStripTextBox1.ToolTipText = "Введите новое название выбранного порта";
			this.toolStripTextBox1.Click += new System.EventHandler(this.toolStripTextBox1_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.файлToolStripMenuItem,
									this.toolStripMenuItem1});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.menuStrip1.Size = new System.Drawing.Size(1284, 26);
			this.menuStrip1.TabIndex = 6;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// файлToolStripMenuItem
			// 
			this.файлToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
									this.выбратьПапкуДляBackupToolStripMenuItem,
									this.списокDXCToolStripMenuItem,
									this.выходToolStripMenuItem});
			this.файлToolStripMenuItem.Name = "файлToolStripMenuItem";
			this.файлToolStripMenuItem.Size = new System.Drawing.Size(81, 22);
			this.файлToolStripMenuItem.Text = "Настройки";
			// 
			// выбратьПапкуДляBackupToolStripMenuItem
			// 
			this.выбратьПапкуДляBackupToolStripMenuItem.Name = "выбратьПапкуДляBackupToolStripMenuItem";
			this.выбратьПапкуДляBackupToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
			this.выбратьПапкуДляBackupToolStripMenuItem.Text = "Выбрать папку для Backup...";
			this.выбратьПапкуДляBackupToolStripMenuItem.Click += new System.EventHandler(this.ВыбратьПапкуДляBackupToolStripMenuItemClick);
			// 
			// списокDXCToolStripMenuItem
			// 
			this.списокDXCToolStripMenuItem.Name = "списокDXCToolStripMenuItem";
			this.списокDXCToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
			this.списокDXCToolStripMenuItem.Text = "Список DXC...";
			this.списокDXCToolStripMenuItem.Click += new System.EventHandler(this.СписокDxcToolStripMenuItemClick);
			// 
			// выходToolStripMenuItem
			// 
			this.выходToolStripMenuItem.Name = "выходToolStripMenuItem";
			this.выходToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
			this.выходToolStripMenuItem.Text = "Выход";
			this.выходToolStripMenuItem.Click += new System.EventHandler(this.выходToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
			this.toolStripMenuItem1.BackColor = System.Drawing.Color.DarkSalmon;
			this.toolStripMenuItem1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(242, 22);
			this.toolStripMenuItem1.Text = "Замечания и предложения...";
			this.toolStripMenuItem1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.toolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem1Click);
			// 
			// timerProgress
			// 
			this.timerProgress.Interval = 500;
			// 
			// button7
			// 
			this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.button7.BackColor = System.Drawing.SystemColors.Control;
			this.button7.BackgroundImage = global::DXC.Properties.Resources.umute;
			this.button7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.button7.Cursor = System.Windows.Forms.Cursors.Hand;
			this.button7.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
			this.button7.FlatAppearance.BorderSize = 0;
			this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.button7.Location = new System.Drawing.Point(996, 1);
			this.button7.Name = "button7";
			this.button7.Size = new System.Drawing.Size(32, 25);
			this.button7.TabIndex = 7;
			this.button7.UseVisualStyleBackColor = false;
			this.button7.Click += new System.EventHandler(this.button7_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1284, 721);
			this.Controls.Add(this.button7);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.splitContainer1);
			this.Name = "MainForm";
			this.Text = "DXC";
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel1.PerformLayout();
			this.splitContainer1.Panel2.ResumeLayout(false);
			this.splitContainer1.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
			this.contextPorts.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
		private System.Windows.Forms.ToolStripStatusLabel lbProgressAfter;
		private System.Windows.Forms.ToolStripStatusLabel lbProgress;
		private System.Windows.Forms.Timer timerProgress;
		private System.Windows.Forms.ToolStripProgressBar ProgressBar1;
		private System.Windows.Forms.DataGridViewTextBoxColumn Direction;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.ToolStripStatusLabel lbAlmCount;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.TextBox tbInterval;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.DataGridViewTextBoxColumn status;
		private System.Windows.Forms.DataGridViewTextBoxColumn end;
		private System.Windows.Forms.DataGridViewTextBoxColumn start;
		private System.Windows.Forms.DataGridViewTextBoxColumn name;
		private System.Windows.Forms.DataGridViewTextBoxColumn port;
		private System.Windows.Forms.DataGridViewTextBoxColumn bord;
		private System.Windows.Forms.BindingSource bindingSource1;
		private System.Windows.Forms.DataGridView dataGridView1;
		private System.Windows.Forms.ListBox lbAll;
		private System.Windows.Forms.ToolStripMenuItem выбратьПапкуДляBackupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem файлToolStripMenuItem;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button button1;
		public System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem выходToolStripMenuItem;
        public System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.ToolStripMenuItem списокDXCToolStripMenuItem;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.ContextMenuStrip contextPorts;
        private System.Windows.Forms.ToolStripMenuItem свойствоПортаToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
		private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
	}
}
