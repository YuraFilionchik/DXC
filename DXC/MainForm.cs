/*
 * Created by SharpDevelop.
 * User: Ситал
 * Date: 20.02.2017
 * Time: 15:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Text;
using System.Drawing;
using System.Threading;
using ThreadState = System.Threading.ThreadState;

namespace DXC
{


	public  partial class MainForm : Form
	{
		private static MainForm _instance;
		public static MainForm Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new MainForm();
                lock (_instance)
                    return MainForm._instance;
            }
            set
            {
                if (_instance == null)
                    _instance = new MainForm();
                lock (_instance)
                MainForm._instance = value;
            }
        }
		public string CurrentIp;
		public string BackupPath;
        public Thread UpdateAlarmsThread;
		private string _propertiesFile = "DXC.ini";
	    public IniFile Cfg;
	    public ClassDxc CurrentDxc;
	    public List<ClassDxc> DxcList=new List<ClassDxc>();//список всех DXC
	    int _intervalRequests=30000; //Интервал опроса DXC при вкл. мониторинге
		int _intervalProgressTimer=500;	    //интервал обновления прогрессбара
	    int _remainMilisec;
	    Color _monitorButtonOffColor=Color.LightBlue;
	    Color _monitorButtonOnColor=Color.IndianRed;
        DateTime FromFilter
        { get 
            { return new DateTime(dtFrom.Value.Year,dtFrom.Value.Month,dtFrom.Value.Day,0,0,0); } 
          set { dtFrom.Value = value; } 
        }
        DateTime ToFilter { get { return new DateTime(dtTo.Value.Year, dtTo.Value.Month, dtTo.Value.Day,23, 59, 59); } set { dtTo.Value = value; } }
        public MainForm()
		{
        	 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
InitializeComponent();
           Cfg = new IniFile(_propertiesFile);
           Program.Helper.Lb1=listBox1;
           FromFilter = DateTime.Now.AddDays(-7);
           ToFilter = DateTime.Now;
            ReadSettings();
            ViewDxcNames(DxcList);
            timer1.Interval=_intervalRequests;
            timerProgress.Interval=_intervalProgressTimer;
            tbInterval.Text=(_intervalRequests*0.001).ToString();
			dataGridView1.ScrollBars=ScrollBars.Both;
			_remainMilisec=_intervalRequests;

 #region events
 			listBox1.SelectedValueChanged+= new EventHandler(listBox1_SelectedValueChanged);
           this.Closing += MainForm_Closing;
           this.Shown+=	FormShown;
           this.lbAll.SelectedIndexChanged+= new EventHandler(LbAllSelectedIndexChanged);
            timerProgress.Tick+= new EventHandler(timerProgress_Tick);
            timer1.Tick+= new EventHandler(timer1_Tick);
                toolStripTextBox1.KeyPress += ToolStripTextBox1_KeyPress;
                contextPorts.Opening += ContextPorts_Opened;
            foreach (var dxc in DxcList)
            {
                dxc.DxcEvent+= new DxcEventHandler(CurrentDXC_DXCEvent);
            }
#endregion
           if(lbAll.Items.Count>0) lbAll.SelectedIndex=0;
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
			
        }

       






        //Selected port
        void listBox1_SelectedValueChanged(object sender, EventArgs e)
        {
        	string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
        	try {
    if(!checkBox1.Checked)
        	{
        		if(listBox1.SelectedItems.Count!=0 && listBox1.SelectedItem.ToString().Contains("Port"))
        			//отображаем только выбранный порт
        		{
        			Port port=new Port(listBox1.SelectedItem.ToString().Split('=')[1]);
        			DisplayAlarmsDgv(CurrentDxc.Alarms.Where(x=>x.PortNumber==port.PortNumber && 
        			                                         x.BordNumber==port.BordNumber).ToList());
        		} else       		
        		DisplayAlarmsDgv(CurrentDxc.Alarms);
        	}
        	else 
        	{
        		if(listBox1.SelectedItems.Count!=0 &&listBox1.SelectedItem.ToString().Contains("Port"))
        			//отображаем только выбранный порт
        		{
        			Port port=new Port(listBox1.SelectedItem.ToString().Split('=')[1]);
        			DisplayAlarmsDgv(CurrentDxc.GetCorrectedAlarms().Where(x=>x.PortNumber==port.PortNumber && 
        			                                         x.BordNumber==port.BordNumber).ToList());
        		}else
        		DisplayAlarmsDgv(CurrentDxc.GetCorrectedAlarms());
        	}
        	} catch (Exception ex) {
        		MessageBox.Show(ex.Message);
        		 Log.WriteLog(methodName, ex.Message);
        	}

        }

        //переименование порта
        private void ToolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (e.KeyChar == (char) 13) //Enter
                {
                    string name = toolStripTextBox1.TextBox.Text;
                    Port p = GetPortFromSelectedItem();
                    if(p.BordNumber==0) return;
                    CurrentDxc.Ports.Find(x => x.BordNumber == p.BordNumber
                                               && x.PortNumber == p.PortNumber).Name = name;
                    contextPorts.Close();
                    ClearLog();
                    InvokeLog("", CurrentDxc.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Переименование порта");
                Log.WriteLog("ToolStripRenamePort",ex.Message);
            }
        }
        //Появление меню выбранного порта
        private void ContextPorts_Opened(object sender, EventArgs e)
        {
            Port p = GetPortFromSelectedItem();
            if(p.BordNumber==0)
            {
                toolStripTextBox1.TextBox.Text="";
                return;

            }
            string name = CurrentDxc.Ports.Find(x => x.BordNumber == p.BordNumber
                                                     && x.PortNumber == p.PortNumber).Name;
            toolStripTextBox1.TextBox.Text = name;
        }


        //обработчик событий DXC
        void CurrentDXC_DXCEvent(string dxcName, string msg)
        {
        	if(msg.Contains("Файл загружен")) Help.BeepBackupOk();
            else if (msg.Contains("не доступен")) Help.BeepDenied();
            else if (msg.Contains("new alarms:"))
            {
                if(dxcName == CurrentDxc.CustomName) Help.BeepAlarmMajor();
                else Help.BeepAlarmMinor();
                //return;
            }
            if(dxcName=="system")
            {	if(msg=="tcp-closed")
            		EnableButtons();
            	if(msg=="tcp-opened")
            		DisableButtons();
            	return;
            }
            InvokeLog(dxcName,msg);
        }

        //обновление аварий
        void timer1_Tick(object sender, EventArgs e)
        {//обновление только текущего
        	string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
                timerProgress.Stop();
                var t0=DateTime.Now;
        
                if(CurrentDxc==null) return;
                DisableButtons();
                CurrentDxc.ReadAlarms(2);
                if(!checkBox1.Checked) DisplayAlarmsDgv(CurrentDxc.Alarms);
                else DisplayAlarmsDgv(CurrentDxc.GetCorrectedAlarms());
                #region read alarms other DXC
                UpdateAlarmsThread = new Thread(() => {
                    foreach (var dxc in DxcList)
                    {
                        if (dxc == CurrentDxc) continue;
                        int oldCount= dxc.Alarms.Count;
                        dxc.ReadAlarms(2);
                        if (dxc.Alarms.Count > oldCount) //new alarms exist
                        {
                            InvokeLog("", dxc.CustomName + " = новая авария");
                            Help.BeepAlarmMinor();
                        }
                    }
                });
                    
                UpdateAlarmsThread.Start();
            
                #endregion
                EnableButtons();
        	TimeSpan dt=DateTime.Now-t0;
        	_remainMilisec=timer1.Interval-(int)dt.TotalMilliseconds;
        	ProgressBar1.Value=0;
        	#region logging
        	//Log.WriteLog(methodName,"time0: "+t0.ToLongTimeString());
//        	Log.WriteLog(methodName,"dt: "+dt.TotalMilliseconds+"ms");
//        	Log.WriteLog(methodName,"timer1.interval = "+timer1.Interval.ToString());
        	#endregion
        	timerProgress.Start();
        	

            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
        	
        }
//tick to ProgressBar1
        void timerProgress_Tick(object sender, EventArgs e)
        {
        	 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
            ProgressBar1.Value=(int)(100*(timer1.Interval-_remainMilisec)/timer1.Interval)+1;
        	lbProgressAfter.Text=((int)(_remainMilisec)/1000).ToString()+"c";
        	_remainMilisec-=_intervalProgressTimer;
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
        	
        	        	
        }
        
        //Select DXC in listbox
        void LbAllSelectedIndexChanged(object sender, EventArgs e)
        {
        	 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
			if(lbAll.SelectedItems.Count!=1)return;
        	CurrentDxc=DxcList.Find(x=>x.CustomName==lbAll.SelectedItem.ToString());
        	ClearLog();
            CurrentDxc.ReadInfoFromIp();
            Help.BeepClick();
        	InvokeLog("",CurrentDxc.ToString());
        	#region test
        	//CurrentDXC.alarms=ReadAlarmsFromFile("test_ALARMS.txt");
        	#endregion
        	dataGridView1.Rows.Clear();
            ViewDXCAlarmsDGV(CurrentDxc,FromFilter,ToFilter);		
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
        	
        }
        public void ViewDXCAlarmsDGV(ClassDxc dxc,DateTime from,DateTime to)
        {
            try
            {
                if (dxc == null || from == null || to == null) return;
                var FROM = dxc.CorrectTimeToDXC(from);//приводим к локальному времени на dxc
                var TO = dxc.CorrectTimeToDXC(to);
                    IEnumerable<Alarm> Filteredalarms;
                if (!dxc.Alarms.Any(x => x.Start <= FROM))
                 //try read from arhiv
                {
                    dxc.ReadAlarmsFromInterval(FROM,TO);
                }
                if (!checkBox1.Checked)
                    Filteredalarms = dxc.Alarms.Where(a=>a.Start>=FROM && a.Start<=TO);
               else Filteredalarms = dxc.GetCorrectedAlarms().Where(a => a.Start >= from && a.Start <= to);
                DisplayAlarmsDgv(Filteredalarms.ToList());
            }
            catch (Exception ex)
            {
                Log.WriteLog("ViewAlarms.main.",ex.Message);
            }
        }
        public void DisplayAlarmsDgv(List<Alarm> alarms)
        {
 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
            	
        	dataGridView1.Rows.Clear();
                if (alarms.Count == 0) return;
            #region	Manual Add row
            foreach (Alarm alarm in alarms) {
        	
        			DataGridViewRow row=new DataGridViewRow();
        			row.CreateCells(dataGridView1);
        		row.Cells[0].Value=alarm.BordNumber;
        		row.Cells[1].Value=alarm.PortNumber;
        		Port p=CurrentDxc.Ports.FirstOrDefault(c=>c.
        		                                                   BordNumber==alarm.BordNumber && 
        		                                                   c.PortNumber==alarm.PortNumber);
        		if(p!=null)
        		row.Cells[2].Value=p.Name;
        		row.Cells[3].Value=alarm.Name;
        		row.Cells[4].Value=alarm.Start;
        		row.Cells[5].Value=alarm.End;
        		row.Cells[6].Value=alarm.Status;
			var style=row.DefaultCellStyle;
        		if(alarm.Active) {
        			style.BackColor=Color.LightCoral;
        		}
        		if(!alarm.Active) {
        			style.BackColor=Color.LightGreen;
        		}
        		if(alarm.Status=="EVENT") {
        			style.BackColor=Color.LightGray;
        		}
        		row.DefaultCellStyle=style;
				dataGridView1.Rows.Add(row);
        	}
                #endregion

                FromFilter = alarms.Min(x => x.Start);
                ToFilter = alarms.Max(x=>x.Start);
        	//if(alarms.Any(x=>x.active)) System.Console.Beep();
        	//Сортировка 
        	dataGridView1.Sort(dataGridView1.Columns[4],System.ComponentModel.ListSortDirection.Descending);
        	lbAlmCount.Text="Активных аварий: "+alarms.Count(x=>x.Active);
        	lbAlmCount.Text+="   Всего в списке: "+alarms.Count();
        	dataGridView1.AllowUserToResizeColumns=true;
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
                MessageBox.Show(exception.Message);
            }
        	
        }
		void FormShown(object sender, EventArgs e)
		{
			Help.BeepOpen();
		}
       

        void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        	 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
                Help.BeepClose();
                StopUpdateAlarmsThread();
			SetMonitoring(false);
        	SaveSettings();
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
        	
          
        }
      
        
       
        
        public void SaveSettings()
        {
        	try {
        		#region Сохранение списка IP-NAME
        		Cfg.DeleteSection("LIST_DXC");
        		foreach (ClassDxc dxc in DxcList) {
        			Cfg.Write("LIST_DXC",dxc.Ip,dxc.CustomName);
        		}
        		#endregion
        		#region Сохранение информации о DXC
        		//каждый DXC в отдельной секции
        		foreach (ClassDxc dxc in DxcList) {
         			dxc.SaveToFile(Cfg);
        		}
        		#endregion
        		Cfg.Write("Global","backupDir",BackupPath);
        	} catch (Exception ex) {
        		
        		MessageBox.Show(ex.Message, "saving settings");
        		Log.WriteLog("SaveSettings",ex.Message);
        	}
        }
	    public void ReadSettings()
	    {
	    	 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
            	#region Чтение имен DXC
			    	DxcList.Clear();
	        var dxcKeys=Cfg.GetAllKeys("LIST_DXC");
	        if(dxcKeys.Count()>0) //read all DXC NAME--IP
	        {//KEY - IP
	        	//VALUE - NAME
	        	foreach (var dxcKey in dxcKeys) {
	        		if(String.IsNullOrWhiteSpace(dxcKey)) continue;
	        		ClassDxc newDxc=new ClassDxc(dxcKey);
	        		newDxc.CustomName=Cfg.ReadIni("LIST_DXC",dxcKey);
					if (DxcList.All(x => x.Ip != dxcKey))
						DxcList.Add(newDxc);
	        	}
	        }
#endregion
#region Чтение Информации о DXC
			if (DxcList.Any()) {
				for (int i = 0; i < DxcList.Count; i++) {
					ClassDxc d = DxcList[i];
					d.LoadFromFile(Cfg);
					//D.backupPath = Cfg.ReadINI(D.ip, "BackupPath");
					//D.info.sys_name = Cfg.ReadINI(D.ip, "Sys_Name");
					//string file = Cfg.ReadIni(d.Ip, "Alarms_file");
					//if(Cfg.KeyExists("TimeCorrection",D.ip)) D.info.dt=new TimeSpan(long.Parse(Cfg.ReadINI(D.ip,"TimeCorrection")));
					d.ReadArhivAlarmsForDays(10);
					DxcList[i] = d;
				}
			}

#endregion
	
	        if (Cfg.KeyExists("backupDir","Global")) BackupPath = Cfg.ReadIni("Global", "backupDir");
	        else BackupPath = "";

            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
			


	    }
	   static public bool IsIPFormat(string ip)
	    {
	    	var segments=ip.Split('.');
	    	if(segments.Length!=4) return false;
	    	foreach(string seg in segments)
	    	{int n=300;
	    		if(!int.TryParse(seg,out n)) return false;
	    		if(n>255 || n<0) return false;
	    	}
	    	return true;
	    }
       
		void ВыбратьПапкуДляBackupToolStripMenuItemClick(object sender, EventArgs e)
		{
			
		    if (Directory.Exists(BackupPath)) folderBrowserDialog1.SelectedPath = BackupPath;
		    else folderBrowserDialog1.SelectedPath = Directory.GetCurrentDirectory();
		  var dr=  folderBrowserDialog1.ShowDialog();
		    if (dr == DialogResult.OK)
		    {
		        BackupPath = folderBrowserDialog1.SelectedPath;
		        Text=BackupPath;
		    }
		}

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

	    public void InvokeControlText(Control ctr, string text)
	    {
	        if (ctr.InvokeRequired) ctr.Invoke(new Action<string>(s => ctr.Text = s),text);
	        else ctr.Text = text;
	    }

	    public void ClearLog()
	    {
	    	if (listBox1.InvokeRequired) listBox1.Invoke(new Action<string>(s=>listBox1.Items.Clear()),"");
	        else listBox1.Items.Clear();
	    }
	    public  void InvokeLog(string from, string msg)
	    {
            msg=msg.Replace('\r', ' ');
            msg=msg.Replace('\u0001',' ');
            msg = msg.Replace('\u001f', ' ');
           // msg = msg.Replace('\u0001', ' ');
            msg = msg.Replace('\u0003', ' ');
            //\u0001\u001f\u0001\u0003
            string[] strokes;
            msg = from + ": " + msg;
            if (msg.Contains("\n"))
	        {
	            //разбитие на несколько строк
	             strokes = msg.Split('\n');
	            foreach (string stroke in strokes)
	            {
                    if (listBox1.InvokeRequired) listBox1.Invoke(new Action<string>(s => listBox1.Items.Add(s)), stroke);
                    else listBox1.Items.Add(stroke);
                }
	        }
            else
            {
                if (listBox1.InvokeRequired) listBox1.Invoke(new Action<string>(s => listBox1.Items.Add(s)), msg);
	        else listBox1.Items.Add(msg);
            }
           
	        
	    }
	    
	    //backup
        private void button1_Click(object sender, EventArgs e)
        {
        	try
        	{
        		if(String.IsNullOrWhiteSpace(BackupPath)) 
        		{ClearLog();
        			InvokeLog("backup", "Не выбрана папка для резервной копии");
                    Help.BeepNotify();
        			return;
        		}
        		
        		string nDb="2";
        		string file="DB"+nDb+"CONF.CFG"; //local backup file
        		BackupPath=BackupPath.TrimEnd('\\')+"\\";
        		string dir=BackupPath+CurrentDxc.Info.SysName+"\\"+DateTime.Now.ToShortDateString();
        		if(!Directory.Exists(dir))
          	Directory.CreateDirectory(dir);
           
            //ClearLog();
            //InvokeLog(DateTime.Now.ToShortTimeString()+"->backup","Сохранение базы на DXC");
           
           CurrentDxc.BackupPath=dir+"\\"+file;
           // InvokeLog(DateTime.Now.ToShortTimeString()+"->backup","Копирование файла c DXC на ПК ");
           CurrentDxc.MakeBackUp(nDb);
          
                  CurrentDxc.SaveToFile(Cfg);
        	}
        	
        	
        	catch(Exception ex)
        	{
        		MessageBox.Show(ex.Message);
        		Log.WriteLog("backup",ex.Message);
        	}
        }

        //Get sys info online
        private void button2_Click(object sender, EventArgs e)
        {
        	
            string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            ClearLog();
   try {
            	if(CurrentDxc.ReadInfoFromIp())
            {
            CurrentDxc.SaveToFile(Cfg);
            InvokeLog("DXC dsp st sys",CurrentDxc.ToString());
            }
            else 
            	if(CurrentDxc.LoadFromFile(Cfg))
            {
            	ClearLog();   
            	InvokeLog("DXC dsp st sys","Загружено c файла:");
            	InvokeLog("",CurrentDxc.ToString());
				
            } else 
            {
            	ClearLog();
            	InvokeLog("DXC info","Информации не найдено");
            	
            }
 } catch (Exception ex) {
                        	
                        	Log.WriteLog(methodName,ex.Message);
                        }
            
        }

        //read alarms
        private void button3_Click(object sender, EventArgs e)
        {
        	//if(CurrentDXC || String.IsNullOrWhiteSpace(CurrentDXC.ip)) return;
        		CurrentDxc.ReadAlarms(10);
        		if(!checkBox1.Checked) DisplayAlarmsDgv(CurrentDxc.Alarms);
            else DisplayAlarmsDgv(CurrentDxc.GetCorrectedAlarms());
        }
        
        //test1
		void Button4Click(object sender, EventArgs e)
		{
			DataStorage ds=new DataStorage(CurrentDxc);
			ds.SaveAllAlarms();
		}
        void Button6Click(object sender, EventArgs e)
        {
            Help.BeepAlarmMajor();
        }
        //запуск окна редактирования DXC
        void СписокDxcToolStripMenuItemClick(object sender, EventArgs e)
		{
			EditDxc editForm=new EditDxc(DxcList);
			DialogResult dr=editForm.ShowDialog();
			if(dr!=DialogResult.OK) return;
			DxcList=editForm.ListDxc;
			ViewDxcNames(DxcList);
			
		}
		
		
		public void ViewDxcNames(List<ClassDxc> list)
		{
			lbAll.Items.Clear();
			foreach (ClassDxc dxc in list) {
				lbAll.Items.Add(dxc.CustomName);				
			}
		}
		
 
		//run monitoring
		void Button5Click(object sender, EventArgs e)
		{
			 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
			int interv=1;
			if(!int.TryParse(tbInterval.Text,out interv) && interv<5)
			{
				MessageBox.Show("Неверное значение интервала");
				return;
			}
			_intervalRequests=interv*1000;
			timer1.Interval=_intervalRequests;
			_remainMilisec=_intervalRequests;
			if(timer1.Enabled){ //уже запущен
				SetMonitoring(false);//Останавливаем
			}else{//пока остановлен
				SetMonitoring(true);//Запускаем
			}
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
			
		}
		/// <summary>
		/// Запуск и остановка мониторинга, вкл выкл кнопок, цвет, таймеры, UpdateAlarmsThread
		/// </summary>
		/// <param name="start"></param>
		void SetMonitoring(bool start){
			if(start)//запуск
			{
              Help.BeepRun();
				button5.BackColor=_monitorButtonOnColor;				
				button5.Text="Остановить мониторинг аварий";
				lbProgress.Text="Ожидание опроса DXC:";
				timer1.Start();
				timerProgress.Enabled=true;

              
            }
			else//остановить
			{
                Help.BeepStop();
				timerProgress.Stop();
				ProgressBar1.Value=0;
				lbProgress.Text="Мониторинг аварий не активен";
				timer1.Stop();
				button5.Text="Запуск мониторинга аварий";
				button5.BackColor=_monitorButtonOffColor; 
                StopUpdateAlarmsThread();
			}
		}

        private void StopUpdateAlarmsThread()
        {
            try
            {
                if (UpdateAlarmsThread != null && UpdateAlarmsThread.ThreadState == ThreadState.Running)
                    UpdateAlarmsThread.Abort();
            }
            catch (Exception)
            {
                // ignored
            }
        }

        void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
            Help.BeepClick();
			if(!checkBox1.Checked) DisplayAlarmsDgv(CurrentDxc.Alarms);
        	else DisplayAlarmsDgv(CurrentDxc.GetCorrectedAlarms());
		}
		public void DisableButtons()
		{
			button1.Enabled=false;
			button2.Enabled=false;
			button3.Enabled=false;
			button5.Enabled=false;
			lbAll.Enabled=false;
		}
			public void EnableButtons()
		{
			button1.Enabled=true;
			button2.Enabled=true;
			button3.Enabled=true;
			button5.Enabled=true;
			lbAll.Enabled=true;
		}
		
		void Panel2Paint(object sender, PaintEventArgs e)
		{
			
		}
		
		void SplitContainer1SplitterMoved(object sender, SplitterEventArgs e)
		{
			
		}
		
		void ToolStripMenuItem1Click(object sender, EventArgs e)
		{
			Offers of=new Offers();
			of.ShowDialog();
		}

        //mute / unmute
        private void button7_Click(object sender, EventArgs e)
        {
            if (Help.SoundsOn) //Sounds ON
                //Выключаем
            {
                button7.FlatAppearance.BorderSize = 3;
                button7.BackgroundImage = DXC.Properties.Resources.mute;
                Help.SoundsOn = false;
            }
            else //Включаем  звуки
            {
                button7.FlatAppearance.BorderSize = 0;
                button7.BackgroundImage = DXC.Properties.Resources.umute;
                Help.SoundsOn = true;
            }
        }
        /// <summary>
        /// Get port from selected item in listbox1 (selected port)
        /// </summary>
        /// <returns></returns>
        private  Port GetPortFromSelectedItem()
        {
            try
            {
                if (listBox1.SelectedItems.Count == 0 || !listBox1.SelectedItem.ToString().Contains("Port")) return new Port();

                Port portFromSelectedItem = new Port(listBox1.SelectedItem.ToString().Split('=')[1]);
                return portFromSelectedItem;
            }
            catch(Exception)
            {
                return new Port();
            }
        }
        //просмотр свойств порта
        private void свойствоПортаToolStripMenuItem_Click(object sender, EventArgs e)
        {
                var portFromSelectedItem = GetPortFromSelectedItem();
                portFromSelectedItem = CurrentDxc.DSP_CON(portFromSelectedItem.BordNumber, portFromSelectedItem.PortNumber);
                ViewPort vp=new ViewPort(portFromSelectedItem);
                vp.ShowDialog();
                
        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void dtFrom_ValueChanged(object sender, EventArgs e)
        {
            ViewDXCAlarmsDGV(CurrentDxc, FromFilter, ToFilter);
        }

        private void dtTo_ValueChanged(object sender, EventArgs e)
        {
            ViewDXCAlarmsDGV(CurrentDxc, FromFilter, ToFilter);
        }
    }
}
