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
		public string currentIP;
		public string backupPath;
		private string PropertiesFile = "DXC.ini";
	    public IniFile Cfg;
	    public ClassDXC CurrentDXC;
	    public List<ClassDXC> dxc_list=new List<ClassDXC>();//список всех DXC
	    int IntervalRequests=30000; //Интервал опроса DXC при вкл. мониторинге
		int IntervalProgressTimer=500;	    //интервал обновления прогрессбара
	    int RemainMilisec;
	    Color MonitorButtonOffColor=Color.LightBlue;
	    Color MonitorButtonOnColor=Color.IndianRed;
        public MainForm()
		{
        	 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
InitializeComponent();
           Cfg = new IniFile(PropertiesFile);
           Program.Helper.LB1=listBox1;
            ReadSettings();
            ViewDXCNames(dxc_list);
            timer1.Interval=IntervalRequests;
            timerProgress.Interval=IntervalProgressTimer;
            tbInterval.Text=(IntervalRequests*0.001).ToString();
			dataGridView1.ScrollBars=ScrollBars.Both;
			RemainMilisec=IntervalRequests;
          

 #region events
 			listBox1.SelectedValueChanged+= new EventHandler(listBox1_SelectedValueChanged);
           this.Closing += MainForm_Closing;
           this.Shown+=	FormShown;
           this.lbAll.SelectedIndexChanged+= new EventHandler(lbAllSelectedIndexChanged);
           if(lbAll.Items.Count>0) lbAll.SelectedIndex=0;
            timerProgress.Tick+= new EventHandler(timerProgress_Tick);
            timer1.Tick+= new EventHandler(timer1_Tick);
#endregion
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
        			DisplayAlarmsDGV(CurrentDXC.alarms.Where(x=>x.portNumber==port.PortNumber && 
        			                                         x.bordNumber==port.BordNumber).ToList());
        		} else       		
        		DisplayAlarmsDGV(CurrentDXC.alarms);
        	}
        	else 
        	{
        		if(listBox1.SelectedItems.Count!=0 &&listBox1.SelectedItem.ToString().Contains("Port"))
        			//отображаем только выбранный порт
        		{
        			Port port=new Port(listBox1.SelectedItem.ToString().Split('=')[1]);
        			DisplayAlarmsDGV(CurrentDXC.GetCorrectedAlarms().Where(x=>x.portNumber==port.PortNumber && 
        			                                         x.bordNumber==port.BordNumber).ToList());
        		}else
        		DisplayAlarmsDGV(CurrentDXC.GetCorrectedAlarms());
        	}
        	} catch (Exception ex) {
        		MessageBox.Show(ex.Message);
        		 Log.WriteLog(methodName, ex.Message);
        	}

        }

        //обработчик событий DXC
        void CurrentDXC_DXCEvent(string DXC_Name, string msg)
        {
        	InvokeLog(DXC_Name,msg);
        }

        //обновление аварий
        void timer1_Tick(object sender, EventArgs e)
        {//обновление только текущего
        	string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
            timerProgress.Stop();
     		  var t0=DateTime.Now;
        
        	if(CurrentDXC==null) return;
        	DisableButtons();
        	CurrentDXC.ReadAlarms(2);
        	if(!checkBox1.Checked) DisplayAlarmsDGV(CurrentDXC.alarms);
        	else DisplayAlarmsDGV(CurrentDXC.GetCorrectedAlarms());
                #region read alarms other DXC
            foreach(var dxc in dxc_list)
                {
                    if (dxc == CurrentDXC) continue;
                }
                #endregion
                EnableButtons();
        	TimeSpan dt=DateTime.Now-t0;
        	RemainMilisec=timer1.Interval-(int)dt.TotalMilliseconds;
        	ProgressBar1.Value=0;
        	#region logging
        	//Log.WriteLog(methodName,"time0: "+t0.ToLongTimeString());
//        	Log.WriteLog(methodName,"dt: "+dt.TotalMilliseconds+"ms");
//        	Log.WriteLog(methodName,"timer1.interval = "+timer1.Interval.ToString());
        	#endregion
        	timerProgress.Start();
        	//TODO monitoring all dxc`s

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
            ProgressBar1.Value=(int)(100*(timer1.Interval-RemainMilisec)/timer1.Interval)+1;
        	lbProgressAfter.Text=((int)(RemainMilisec)/1000).ToString()+"c";
        	RemainMilisec-=IntervalProgressTimer;
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
        	
        	        	
        }
        
        //Select DXC in listbox
        void lbAllSelectedIndexChanged(object sender, EventArgs e)
        {
        	 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
			if(lbAll.SelectedItems.Count!=1)return;
        	if(CurrentDXC!=(null)) CurrentDXC.DXCEvent-= new DXCEventHandler(CurrentDXC_DXCEvent);//отписка от старого dxc
        	CurrentDXC=dxc_list.Find(x=>x.custom_Name==lbAll.SelectedItem.ToString());
        	ClearLog();
        	CurrentDXC.ReadInfoFromIP();
        	CurrentDXC.DXCEvent+= new DXCEventHandler(CurrentDXC_DXCEvent);
        	InvokeLog("",CurrentDXC.ToString());
        	#region test
        	//CurrentDXC.alarms=ReadAlarmsFromFile("test_ALARMS.txt");
        	#endregion
        	//lbAlmCount.Text="Актывных аварий: "+CurrentDXC.alarms.Count(x=>x.active);
        	dataGridView1.Rows.Clear();
        	if(!checkBox1.Checked)
        	{
        				
        		DisplayAlarmsDGV(CurrentDXC.alarms);
        	}
        	else 
        	{
        		
        		DisplayAlarmsDGV(CurrentDXC.GetCorrectedAlarms());
        	}
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
        	
        }
        
        public void DisplayAlarmsDGV(List<Alarm> alarms)
        {


 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
            	if(dataGridView1.Rows.Count!=0 && 
            	   dataGridView1.Rows.Count<alarms.Count)//есть новые аварии
            		System.Console.Beep(500,200);
        	dataGridView1.Rows.Clear();
            #region	Manual Add row
            foreach (Alarm alarm in alarms) {
        	
        			DataGridViewRow row=new DataGridViewRow();
        			row.CreateCells(dataGridView1);
        		row.Cells[0].Value=alarm.bordNumber;
        		row.Cells[1].Value=alarm.portNumber;
        		Port p=CurrentDXC.Ports.FirstOrDefault(c=>c.
        		                                                   BordNumber==alarm.bordNumber && 
        		                                                   c.PortNumber==alarm.portNumber);
        		if(p!=null)
        		row.Cells[2].Value=p.Name;
        		row.Cells[3].Value=alarm.name;
        		row.Cells[4].Value=alarm.Start;
        		row.Cells[5].Value=alarm.End;
        		row.Cells[6].Value=alarm.status;
			var style=row.DefaultCellStyle;
        		if(alarm.active) {
        			style.BackColor=Color.LightCoral;
        		}
        		if(!alarm.active) {
        			style.BackColor=Color.LightGreen;
        		}
        		if(alarm.status=="EVENT") {
        			style.BackColor=Color.LightGray;
        		}
        		row.DefaultCellStyle=style;
				dataGridView1.Rows.Add(row);
        	}
            #endregion
          
			
        	//if(alarms.Any(x=>x.active)) System.Console.Beep();
        	//Сортировка 
        	dataGridView1.Sort(dataGridView1.Columns[4],System.ComponentModel.ListSortDirection.Descending);
        	lbAlmCount.Text="Активных аварий: "+alarms.Count(x=>x.active);
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
			//CurrentDXC=new DXC(textBox1.Text);
           // CurrentDXC.LoadFromFile(Cfg);
			//InvokeLog("",CurrentDXC.ToString());
		}
       

        void MainForm_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
        	 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
			SetMonitoring(false);
        	SaveSettings();
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
        	
          
        }
       /// <summary>
       /// Чтение списка аварий из ранее сохраненного файла
       /// </summary>
       /// <param name="file"></param>
       /// <returns></returns>
        public List<Alarm> ReadAlarmsFromFile(string file)
        {
        	List<Alarm> results=new List<Alarm>();
        	 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
				if(!File.Exists(file)) {MessageBox.Show("Файл "+file+" не найден"); return results; }
        		var lines=File.ReadAllLines(file);
        		foreach (string line in lines) {
        			results.Add(new Alarm(false).ParseLine(line));
        		}
        		return results;
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
                MessageBox.Show(exception.Message,"Reading alarms from file :"+file);
        		return results;
            }
        		
        		
        	
        		
        	
        }
        
       /// <summary>
       /// Сохраняет список аварий в существующий файл с объединением или создает новый, если его не существуют
       /// </summary>
       /// <param name="file">полное имя файла</param>
       /// <param name="alarms">список аварий, которые нужно добавить в файл</param>
        public void WriteAlarmsToFile(string file,IEnumerable<Alarm> alarms)
        {
        	try {
        	//read all records from file
        	List<Alarm> oldAlarms=new List<Alarm>();
        	List<string> result=new List<string>();
        	if(File.Exists(file))
        	{
        	var lines=File.ReadAllLines(file);
        	
        	foreach (string line in lines) {
        		Alarm A=new Alarm(false);
        		A.ParseLine(line);
        		oldAlarms.Add(A);
        	}
        	
        	//поиск и закрытие отработанных аварий, которые в старых списках еще открыты
        	var MergedAlarms=Program.Helper.MergeAlarms(oldAlarms,alarms.ToList());
//        	for (int i = 0; i < oldAlarms.Count(); i++) {
//        		var A=oldAlarms[i];
//        		//только активные
//        	                   if(A.active && alarms.Any(x=>x==A))//new alarms contains old Active alarm
//        	                  	{ 
//        	                  		var NewAlm=alarms.First(x=>x==A);
//        	                  		if( !NewAlm.active) //Закрываем аварию
//        	                  		{
//        	                  			A.End=NewAlm.End;
//        	                  			A.active=false; 
//        	                  			oldAlarms[i]=A;
//        	                  		}
//        	                  	}
//        	                   
//        	}
        	
//        	foreach (Alarm alarm in alarms) {
//						if (oldAlarms.All(x => x != alarm))
//							oldAlarms.Add(alarm);
//        	}
        	File.WriteAllText(file,""); //обнулили файл
        	List<string> list=new List<string>();
        	result=MergedAlarms.ConvertAll(x=>x.ExportLineCSV());
        	

        	}//if file not exist
        	else {
        		result=(alarms as List<Alarm>).ConvertAll(x=>x.ExportLineCSV());
        	}
        	File.WriteAllLines(file, result.ToArray());
}
        
        		
        catch (Exception ex) {
        		MessageBox.Show(ex.Message,"Write Alarms to file");
        		Log.WriteLog("WriteAlmsToFile",ex.Message);
        }
        	
        }
        /// <summary>
        /// Генерирует имя файла для лога аварий выбранного DXC
        /// </summary>
        /// <param name="dxc"></param>
        /// <returns></returns>
        public static string GetAlarmsFilePath(ClassDXC dxc)
        {
        	if(dxc == null) return "";
        	if(!String.IsNullOrWhiteSpace(dxc.info.sys_name)) return
        		dxc.info.sys_name+"-"+dxc.ip+"-Alarms.txt";
        	else return "";
        }
        public void SaveSettings()
        {
        	try {
        		#region Сохранение списка IP-NAME
        		Cfg.DeleteSection("LIST_DXC");
        		foreach (ClassDXC dxc in dxc_list) {
        			Cfg.Write("LIST_DXC",dxc.ip,dxc.custom_Name);
        		}
        		#endregion
        		#region Сохранение информации о DXC
        		//каждый DXC в отдельной секции
        		foreach (ClassDXC dxc in dxc_list) {
         			dxc.SaveToFile(Cfg);
        		}
        		#endregion
        		Cfg.Write("Global","backupDir",backupPath);
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
			    	dxc_list.Clear();
	        var DXC_keys=Cfg.GetAllKeys("LIST_DXC");
	        if(DXC_keys.Count()>0) //read all DXC NAME--IP
	        {//KEY - IP
	        	//VALUE - NAME
	        	foreach (var dxc_key in DXC_keys) {
	        		if(String.IsNullOrWhiteSpace(dxc_key)) continue;
	        		ClassDXC newDXC=new ClassDXC(dxc_key);
	        		newDXC.custom_Name=Cfg.ReadINI("LIST_DXC",dxc_key);
					if (dxc_list.All(x => x.ip != dxc_key))
						dxc_list.Add(newDXC);
	        	}
	        }
#endregion
#region Чтение Информации о DXC
			if (dxc_list.Any()) {
				for (int i = 0; i < dxc_list.Count; i++) {
					ClassDXC D = dxc_list[i];
					D.LoadFromFile(Cfg);
					//D.backupPath = Cfg.ReadINI(D.ip, "BackupPath");
					//D.info.sys_name = Cfg.ReadINI(D.ip, "Sys_Name");
					string file = Cfg.ReadINI(D.ip, "Alarms_file");
					//if(Cfg.KeyExists("TimeCorrection",D.ip)) D.info.dt=new TimeSpan(long.Parse(Cfg.ReadINI(D.ip,"TimeCorrection")));
					if(File.Exists(file))D.ReadAlarmsFromFile(file);
					dxc_list[i] = D;
				}
			}

#endregion
	
	        if (Cfg.KeyExists("backupDir","Global")) backupPath = Cfg.ReadINI("Global", "backupDir");
	        else backupPath = "";

            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
			


	    }
	   static public bool IPformat(string ip)
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
			
		    if (Directory.Exists(backupPath)) folderBrowserDialog1.SelectedPath = backupPath;
		    else folderBrowserDialog1.SelectedPath = Directory.GetCurrentDirectory();
		  var dr=  folderBrowserDialog1.ShowDialog();
		    if (dr == DialogResult.OK)
		    {
		        backupPath = folderBrowserDialog1.SelectedPath;
		        Text=backupPath;
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
        		if(String.IsNullOrWhiteSpace(backupPath)) 
        		{ClearLog();
        			InvokeLog("backup", "Не выбрана папка для резервной копии");
        			return;
        		}
        		
        		string nDB="2";
        		string file="DB"+nDB+"CONF.CFG"; //local backup file
        		backupPath=backupPath.TrimEnd('\\')+"\\";
        		string dir=backupPath+CurrentDXC.info.sys_name+"\\"+DateTime.Now.ToShortDateString();
        		if(!Directory.Exists(dir))
          	Directory.CreateDirectory(dir);
           
            //ClearLog();
            //InvokeLog(DateTime.Now.ToShortTimeString()+"->backup","Сохранение базы на DXC");
           
           CurrentDXC.backupPath=dir+"\\"+file;
           // InvokeLog(DateTime.Now.ToShortTimeString()+"->backup","Копирование файла c DXC на ПК ");
           CurrentDXC.MakeBackUp(nDB);
          
                  CurrentDXC.SaveToFile(Cfg);
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
            	if(CurrentDXC.ReadInfoFromIP())
            {
            CurrentDXC.SaveToFile(Cfg);
            InvokeLog("DXC dsp st sys",CurrentDXC.ToString());
            }
            else 
            	if(CurrentDXC.LoadFromFile(Cfg))
            {
            	ClearLog();   
            	InvokeLog("DXC dsp st sys","Загружено c файла:");
            	InvokeLog("",CurrentDXC.ToString());
				
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
        		CurrentDXC.ReadAlarms(10);
        		if(!checkBox1.Checked) DisplayAlarmsDGV(CurrentDXC.alarms);
            else DisplayAlarmsDGV(CurrentDXC.GetCorrectedAlarms());
        }
        
        
		void Button4Click(object sender, EventArgs e)
		{
			CurrentDXC.UpdateDB("2");
		}
		
		//запуск окна редактирования DXC
		void СписокDXCToolStripMenuItemClick(object sender, EventArgs e)
		{
			EditDXC editForm=new EditDXC(dxc_list);
			DialogResult dr=editForm.ShowDialog();
			if(dr!=DialogResult.OK) return;
			dxc_list=editForm.listDXC;
			ViewDXCNames(dxc_list);
			
		}
		
		
		public void ViewDXCNames(List<ClassDXC> list)
		{
			lbAll.Items.Clear();
			foreach (ClassDXC dxc in list) {
				lbAll.Items.Add(dxc.custom_Name);				
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
			IntervalRequests=interv*1000;
			timer1.Interval=IntervalRequests;
			RemainMilisec=IntervalRequests;
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
		/// Запуск и остановка мониторинга, вкл выкл кнопок, цвет, таймеры
		/// </summary>
		/// <param name="start"></param>
		void SetMonitoring(bool start){
			if(start)//запуск
			{
				button5.BackColor=MonitorButtonOnColor;				
				button5.Text="Остановить мониторинг аварий";
				lbProgress.Text="Ожидание опроса DXC:";
				timer1.Start();
				timerProgress.Enabled=true;
			}
			else//остановить
			{
				timerProgress.Stop();
				ProgressBar1.Value=0;
				lbProgress.Text="Мониторинг аварий не активен";
				timer1.Stop();
				button5.Text="Запуск мониторинга аварий";
				button5.BackColor=MonitorButtonOffColor;
			}
		}
		void CheckBox1CheckedChanged(object sender, EventArgs e)
		{
			if(!checkBox1.Checked) DisplayAlarmsDGV(CurrentDXC.alarms);
        	else DisplayAlarmsDGV(CurrentDXC.GetCorrectedAlarms());
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
			Offers OF=new Offers();
			OF.ShowDialog();
		}
    }
}
