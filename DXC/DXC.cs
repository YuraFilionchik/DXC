/*
 * Создано в SharpDevelop.
 * Пользователь: user
 * Дата: 10.12.2020
 * Время: 14:34
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using TFTPClient;


namespace DXC
{ public delegate void DxcEventHandler(string dxcName, string msg);
	/// <summary>
	/// Description of DXC.
	/// </summary>
	 public class ClassDxc
    {
	 	public static string Ttt;
        public string Ip;
        public string CustomName;
        public string BackupPath;
        public List<Alarm> Alarms;
        public Dxcinfo Info;
        public List<Port> Ports;
        public event DxcEventHandler DxcEvent;
		public TFTPSession Session; 
        public override string ToString()
        {
        	string portsString="";
        	foreach (var port in Ports) {
        		portsString+=port.ToString()+"\n";
        	}
            return String.Format("ip={0}\nname={1}\nclock={2}\nDBnumber={3}\n" +
                                 "Installed modules:\n{4}\nПорты:\n{5}\ndate={6}\ntime={7}\nРазбежка времени: {8}дн\n {9}ч {10}мин {11}с", 
                                 Ip, Info.SysName, Info.Nodalclock, Info.ActiveDBnuber,
                                 Info.ToString(),portsString, Info.Date.ToShortDateString(), 
                                 Info.Time.ToLongTimeString(),(int)Info.Dt.TotalDays,
                                Info.Dt.Hours,Info.Dt.Minutes, Info.Dt.Seconds);
        }
        //private IniFile Cfg;
        public string Buffer;

        public ClassDxc(string ip)
        {
            string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
             Session = new TFTPSession();
            try
            {
            	if(!MainForm.Pformat(ip))
            	{
            		MessageBox.Show("Неверный формат IP");
            		return;
            	}
                Buffer = "";
                //Cfg=cfg;
                this.Ip = ip;
                this.CustomName="";
                this.BackupPath = "";
                this.Alarms = new List<Alarm>();
                this.Info=new Dxcinfo(false);
                this.Ports=new List<Port>();
                 
                 Session.Connected += new TFTPSession.ConnectedHandler(_session_Connected);
            Session.Transferring += new TFTPSession.TransferringHandler(_session_Transferring);
            Session.TransferFailed += new TFTPSession.TransferFailedHandler(_session_TransferFailed);
            Session.TransferFinished += new TFTPSession.TransferFinishedHandler(_session_TransferFinished);
            Session.Disconnected += new TFTPSession.DisconnectedHandler(_session_Disconnected);
                
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                Log.WriteLog(methodName, exception.Message);
                
            }

        }

#region	TFTP events
		private void _session_Connected()
        {
			MainForm.Instance.DisableButtons();
			DxcEvent(this.Info.SysName,"TFTP "+Ip+" connected");
        }

        private void _session_Transferring(long bytesTransferred, long bytesTotal)
        {
            //if (BytesTotal != 0)
            {
            	//if(((int)(100*BytesTransferred)/BytesTotal)%10==0)
            	//DXCEvent(this.info.sys_name,String.Format("{0}/{1}bytes",BytesTransferred,BytesTotal));
            }
            //else
            	//DXCEvent(this.info.sys_name,".");
        }

        private void _session_TransferFailed(short errorCode, string errorMessage)
        {
            //Console.WriteLine("Error {0}: {1}", ErrorCode, ErrorMessage);
            DxcEvent(this.Info.SysName,"Error: "+errorMessage);
        }

        private void _session_TransferFinished()
        {
        	DxcEvent(this.Info.SysName,"Файл загружен "+this.BackupPath);
        }

        private void _session_Disconnected()
        {
            MainForm.Instance.EnableButtons();
			DxcEvent(this.Info.SysName,"TFTP "+Ip+" Disconnected");
        } 
#endregion

        /// <summary>
        /// Load info from ini file
        /// </summary>
        /// <param name="cfg"></param>
        public bool LoadFromFile(IniFile cfg)
        {
            string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
                if (cfg.KeyExists("backup", Ip)) this.BackupPath = cfg.ReadIni(Ip, "backup");
                if (cfg.KeyExists("name", Ip)) this.Info.SysName = cfg.ReadIni(Ip, "name");
                if(cfg.KeyExists("TimeCorrection",Ip)) this.Info.Dt=new TimeSpan(long.Parse(cfg.ReadIni(Ip,"TimeCorrection")));
                if (string.IsNullOrWhiteSpace(this.Info.SysName)) return false;
                //load from file
                if (!File.Exists(this.Info.SysName + ".txt")) return false;
                var lines = File.ReadAllLines(this.Info.SysName + ".txt");
                this.Info.Modules=new Dictionary<string, string>();
                foreach (string line in lines)
                {
                    var values = line.Split('=');
                    if (values.Length < 2) continue;
                    if (values[0] == "ip" && values[1] != this.Ip)
                    {
                        MessageBox.Show("Несоответствие ip в файле " + values[1] + ". Файл " + this.Info.SysName + ".txt");
                        return false;
                    }
                    if (values[0] == "name" && values[1] != this.Info.SysName)
                    {
                        MessageBox.Show("Несоответствие имени в файле: " + values[1] + ". Файл " + this.Info.SysName + ".txt");
                        return false;
                    }
                    if (values[0] == "clock") {this.Info.Nodalclock = values[1]; continue;}
                    if (values[0] == "DBnumber") {this.Info.ActiveDBnuber = values[1]; continue;}
                    if (values[0].Contains("slot"))
                    {
                        this.Info.Modules.Add(values[0].Split('.')[1], values[1]);
                        continue;
                    }
                    
                     if (values[0].Contains("Port"))
                     {
                     	this.Ports.Add(new Port(values[1]));
                     	continue;
                     }
                     
               
                    
                     if (values[0] == "date") {this.Info.Date = DateTime.Parse(values[1]); continue;}
                     if (values[0] == "time") {this.Info.Time = DateTime.Parse(values[1]); continue;}
					

                }
                if(this.Ports.Count==0) //try import from modules
                     {
                     	foreach (var modul in Info.Modules) {
                     		int b=int.Parse(modul.Key);
                     		if(!modul.Value.Contains("D8E1")) continue;
                     		for (int i = 1; i < 9; i++) {
                     		Ports.Add(new Port{BordNumber=b,PortNumber=i});	
                     		}
                     		
                     	}
                     	
                     }
                return true;
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
                return false;
            }
            

        }
        
        
        /// <summary>
        /// Save all DXC info to file
        /// </summary>
        /// <param name="cfg"></param>
        public void SaveToFile(IniFile cfg)
        {
        	 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
            if(string.IsNullOrWhiteSpace(this.Info.SysName)) return;
            cfg.Write(Ip,"backup",this.BackupPath);
            cfg.Write(Ip,"name",this.Info.SysName);
            cfg.Write(Ip,"Alarms_file",MainForm.GetAlarmsFilePath(this)); //имя файла с историей аварий DXC
             cfg.Write(Ip,"TimeCorrection",this.Info.Dt.Ticks.ToString());
             WriteAlarmsToFile(MainForm.GetAlarmsFilePath(this));
             string file = this.Info.SysName + ".txt";
            File.WriteAllText(file, this.ToString());

            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
        	
            
        }
        #region Equals 
        // The code in this region is useful if you want to use this structure in collections.
        // If you don't need it, you can just remove the region and the ": IEquatable<Struct1>" declaration.

        public override bool Equals(object obj)
        {
            if (obj is ClassDxc)
                return Equals((ClassDxc)obj); // use Equals method below
            else
                return false;
        }

        public bool Equals(ClassDxc other)
        {
        	//if(other==null) return false;
            // add comparisions for all members here
            return (this.Ip == other.Ip &&
                this.Info == other.Info );
        }


//        public static bool operator ==(DXC left, DXC right)
//        {
//            return left.Equals(right);
//        }
//
//        public static bool operator !=(DXC left, DXC right)
//        {
//        	
//            return !left.Equals(right);
//        }
        #endregion

        public void UpdateDb(string number)
        {string methodName= new StackTrace(false).GetFrame(0).GetMethod().Name;
            if(!IpPingOk(Ip))
            {
                DxcEvent(this.CustomName, this.Ip + " адрес не доступен. Не удалось сохранить Backup!");
                return;
            }
        	TelnetConnection tc =new TelnetConnection(Ip,23);
            try
            {
                //TelnetClient tc=new TelnetClient();
                //                tc.Connect(IPAddress.Parse(IP),23);
                //                tc.DataReceived += Tc_DataReceived;
                //                while (tc.IsConnected())
                //                {
                //                    var  buffer = tc.InputBuffer;
                //                    if(buffer.BaseStream.CanRead)
                //                     Program.MF.InvokeLog(methodName, tc.InputBuffer.ReadString());
                //                    tc.Disconnect();
                //                }
                
                string command="upd db "+number;
                
                DxcEvent(this.Info.SysName, "Открытие telnet "+Ip);
                string ans = tc.Read();
             // DXCEvent(this.info.sys_name, "Успешно");
                Thread.Sleep(200);
                DxcEvent(this.Info.SysName, "Отправка команды "+command);
               tc.WriteLine(command);
               Thread.Sleep(2000);
               DxcEvent(this.Info.SysName, "Успешно.");
                tc.Close();
                DxcEvent(this.Info.SysName, "Соединение закрыто.");

            }
            catch (Exception exception)
            {
            	if(tc.IsConnected){
            		tc.Close(); 
                DxcEvent(this.Info.SysName, "Произошла ошибка. Соединение закрыто");
            	}
                Log.WriteLog(methodName, exception.Message);
            }
        }

        private void Tc_DataReceived(object sender, DataReceivedEventArgs e)
        {
            string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {

            var tc = (TelnetClient) sender;
            string answer = TelnetClient.ParseData(e.Data);
           // Program.MF.InvokeLog(methodName, answer);

            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
            
        }

        public void Template()
        {
            string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {

            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }

        }

        private Dxcinfo ParseDxcInfo(string text)
        {
        	 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
        	 if (string.IsNullOrWhiteSpace(text)) return new Dxcinfo();
            Dxcinfo info = new Dxcinfo();
            try
            {
            
            info.Modules = new Dictionary<string, string>();
            text = text.Replace('\r', ' ');
            var lines = text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                var wordsAll = line.Split(' ');
                var words = wordsAll.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

                if (words.Length >= 6 && words[3].Contains("NAME"))
                {
                    info.SysName = words[5]; //name
                    continue;
                }
                if (words.Length >= 4 && words[1].Contains("CLOCK"))
                {
                    info.Nodalclock = words[3]; //nodal clock
                    continue;
                }
                if (words.Length >= 9 && words[5].Contains("DATABASE") && words[6].Contains("NUMBER")) //DB number
                {
                    info.ActiveDBnuber = words[8];
                    continue;
                }
                if (words.Length >= 7 && words[0].Contains("I/O"))
                {
                    var wordsNext = lines[i + 2].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray(); ;
                    info.Modules.Add(words[2], wordsNext[2]);
                    info.Modules.Add(words[3], wordsNext[3]);
                    info.Modules.Add(words[4], wordsNext[4]);
                    info.Modules.Add(words[5], wordsNext[5]);
                    info.Modules.Add(words[6], wordsNext[6]);
                    continue;
                }
                if (words.Length >= 4 && words[0].Contains("TIME"))
                {
                    info.Time = DateTime.ParseExact(words[1], "HH:mm:ss", null);
                    info.Date = DateTime.Parse(words[3]);
                    DateTime fullDate=new DateTime(info.Date.Year,info.Date.Month,info.Date.Day,
                                                   info.Time.Hour,info.Time.Minute, info.Time.Second);
                    info.Dt=fullDate-DateTime.Now; //установка разницы во времени оборудования DXC и данного ПК
                }

            }

            return info;
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
                
                return info;
            }

        }
        public void MakeBackUp(string n)
        {
        	//UpdateDB(N); //save conf to Db2 file on DXC
        	CopyFileFromServer("DB"+n+"CONF.CFG",this.Ip, this.BackupPath);
        	//Thread.Sleep(1000);
        }
        

        
        /// <summary>
         /// Запись аварий в файл
         /// </summary>
         /// <param name="file"></param>
         /// <param name="alarms"></param>
        public void WriteAlarmsToFile(string file)
        {
        	try {
        	//read all records from file
        	List<Alarm> oldAlarms=new List<Alarm>();
        	List<string> result=new List<string>();
        	if(File.Exists(file))
        	{
        	var lines=File.ReadAllLines(file);
        	//read OldAlarms
        	foreach (string line in lines) {
        		Alarm a=new Alarm(false);
        		a.ParseLine(line);
        		oldAlarms.Add(a);
        	}
        	
        	//поиск и закрытие отработанных аварий, которые в старых списках еще открыты
        	var mergedAlarms=Program.Helper.MergeAlarms(oldAlarms,Alarms.ToList());

        	File.WriteAllText(file,""); //обнулили файл
        	List<string> list=new List<string>();
        	result=mergedAlarms.ConvertAll(x=>x.ExportLineCsv());
        	

        	}//if file not exist
        	else {
        		result=(Alarms as List<Alarm>).ConvertAll(x=>x.ExportLineCsv());
        	}
        	File.WriteAllLines(file, result.ToArray());
}
        
        		
        catch (Exception ex) {
        		MessageBox.Show(ex.Message,"DXC.Write Alarms to file");
        }
        	
        }
 
         /// <summary>
       /// Чтение списка аварий из ранее сохраненного файла, заменяя уже существующий набор alarms
       /// </summary>
       /// <param name="file"></param>
       /// <returns></returns>
        public List<Alarm> ReadAlarmsFromFile(string file)
        {List<Alarm> results=new List<Alarm>();
        	try {
        	//	DXCEvent(this.info.sys_name, "Чтение аварий из файла");
        		if(!File.Exists(file)) {MessageBox.Show("Файл "+file+" не найден"); return results; }
        		
        		var lines=File.ReadAllLines(file);
        		foreach (string line in lines) {
      			
        			Alarm alm=new Alarm(false).ParseLine(line);
        	  
        			results.Add(alm);
        		}
        		//MessageBox.Show("dubles:"+w.ToString());
        		//DXCEvent(this.info.sys_name, "Загружено "+results.Count+" аварий из "+file);
        		Alarms=results;
        		return results;
        	} catch (Exception ex) {
        		MessageBox.Show(ex.Message,"Reading alarms from file :"+file);
        		return results;
        	}
        }
        public List<Alarm> GetCorrectedAlarms()
        {
        	return FixAlarmTime(Alarms);
        }
        /// <summary>
        /// Производит поправку времени аварий к времени на данном ПК (устраняет разброс времени на DXC and PC)
        /// </summary>
        /// <param name="alarms"></param>
        /// <returns></returns>
        private List<Alarm> FixAlarmTime(List<Alarm> alarms)
        { 
        	List<Alarm> correctedAlarms=new List<Alarm>();
        	foreach (Alarm alarm in alarms) {
        		var alm=alarm.GetCopyAlarm();
        		alm.Start-=Info.Dt;
        		alm.End-=Info.Dt;
        		
        		correctedAlarms.Add(alm);
        	}
        	return correctedAlarms;
        }
        
        /// <summary>
        /// Connect via tftp to DXC and Copy conf file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ip"></param>
        /// <param name="destFile"></param>
        private void CopyFileFromServer(string file, string ip, string destFile)
       {
        	if(!IpPingOk(ip)) {
        		MessageBox.Show(ip+" адрес не доступен");
                DxcEvent(this.CustomName, this.Ip + " адрес не доступен");
                return;
        	}
        	try {
   
      		TransferOptions tOptions = new TransferOptions();            
            tOptions.LocalFilename = destFile;
            tOptions.RemoteFilename = file;
            tOptions.Host = ip;
            tOptions.Action = TransferType.Get;
            Session.Get(tOptions);
   
        	} catch (Exception ex) {
        		MessageBox.Show(ex.Message,"copy from server");
        	}
        
         

//        	TftpClient client=new TftpClient(ip,69);
//        	var transfer = client.Download(file);
//        	transfer.OnError+= new Tftp.Net.TftpErrorHandler(transfer_OnError);
//        	transfer.OnProgress+= new Tftp.Net.TftpProgressHandler(transfer_OnProgress);
//        	transfer.OnFinished+= new Tftp.Net.TftpEventHandler(transfer_OnFinished);
//        	            //Start the transfer and write the data that we're downloading into a memory stream
//            Stream stream = new MemoryStream();
//            
//            transfer.TransferMode=TftpTransferMode.mail;
//            transfer.Start(stream);
//            //Wait for the transfer to finish
//            TransferFinishedEvent.WaitOne();
//       
//            SaveStreamToFile(stream, DestFile);
//        
       
        }
        private void SaveStreamToFile(Stream stream, string destFile)
        {
        	try {
        		FileStream fs=new FileStream(destFile,FileMode.Create);
            stream.CopyTo(fs);
            StreamReader sr=new StreamReader(stream);
            MessageBox.Show(sr.ReadToEnd());
            fs.Close();	
            //MessageBox.Show(sr.ReadToEnd());
        	} catch (Exception ex) {
        		
        		MessageBox.Show(ex.Message,"Save stream to file");
        	}
        }
//        void transfer_OnFinished(Tftp.Net.ITftpTransfer transfer)
//        {
//        	//Log.WriteLog("","Файл скопирован");
//        	 TransferFinishedEvent.Set();
//        	// SaveStreamToFile(, DestFile);
//        	 
//        }

//        void transfer_OnProgress(Tftp.Net.ITftpTransfer transfer, Tftp.Net.TftpTransferProgress progress)
//        {
//        	//Log.WriteLog("","Копируется "+progress);
//        }
//
//        void transfer_OnError(Tftp.Net.ITftpTransfer transfer, Tftp.Net.TftpTransferError error)
//        {
//        	Log.WriteLog("",error.ToString());
//        	 TransferFinishedEvent.Set();
//        }
        public bool ReadInfoFromIp()
        {
        string methodName= new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {DxcEvent("system", "tcp-opened");
                if (!IpPingOk(Ip))
                {
                   // MessageBox.Show("Адрес "+ip+" не доступен.");
                    DxcEvent(this.CustomName, this.Ip + " адрес не доступен");
                    DxcEvent("system", "tcp-closed");
                    return false;
                }
                TelnetConnection tc =new TelnetConnection(Ip,23);
                string ans = tc.Read();
                Buffer=ans;
               // Program.MF.InvokeLog(methodName, ans);
                Thread.Sleep(100);
               tc.WriteLine("dsp st sys");
               bool readed=false;
               while (!readed) {//ответ. первый блок аварий
               	Thread.Sleep(100);//пауза
               	var b=tc.Read();
               	Buffer+=b; //накапливаем ответ команды
               	if(String.IsNullOrEmpty(b)) readed=true; //ждём когда закончится чтение
               }
               
                Info=ParseDxcInfo(Buffer);
                tc.Close();
                DxcEvent("system", "tcp-closed");
                return true;
            }
            catch (Exception exception)
            {
                //Program.MF.InvokeLog(methodName, exception.Message);
                Log.WriteLog(methodName, exception.Message);
                DxcEvent("system", "tcp-closed");
                return false;
            }
        }
       /// <summary>
       /// Чтение аварий через telnet и заполнение списка аварий 
       /// </summary>
       /// <param name="repeats">Количество повторов dsp alm</param>
        public void ReadAlarms(int repeats)
        {
        	 string methodName= new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
            	 DxcEvent("system", "tcp-opened");
if(!IpPingOk(Ip)) {
        		//MessageBox.Show(ip+" адрес не доступен");
                DxcEvent(this.CustomName, this.Ip + " адрес не доступен");
                 DxcEvent("system", "tcp-closed");
        		return;
        	}
        	 List<Alarm> buffAlarms=new List<Alarm>();
        	 TelnetConnection tc =new TelnetConnection(Ip,23);
                string ans = tc.Read();
                Buffer="";
              Thread.Sleep(100);
               tc.WriteLine("dsp alm"); //запрос
               Thread.Sleep(200);//пауза
               bool readed=false;
               while (!readed) {
               	var b0=tc.Read();
               	Thread.Sleep(200);//пауза
               	Buffer+=b0; //ответ. первый блок аварий
               	if(String.IsNullOrEmpty(b0)) readed=true;
               }
                             
               buffAlarms=ParseAlarms(Buffer); //аварии первого запроса
               
               if(Alarms.Any(x=>buffAlarms.Any(b=>b==x && b.Active==x.Active)))
               {//в буфере уже есть хотябы одна авария из ранее считанных
               	Alarms=Program.Helper.MergeAlarms(Alarms,buffAlarms);
               	 tc.Close(); 
               	 DxcEvent("system", "tcp-closed");
               	return;
               }//повторяем пока не дойдем до существующей аварии или не достигнем счетчика
               while (repeats>1 && !Alarms.Any(x=>buffAlarms.Any(b=>b==x && b.Active==x.Active))) 
               {
               	tc.Write(" ");				
				Thread.Sleep(100);
               	bool endread=false;
				Buffer="";
               while (!endread) 
               {
               	var b0=tc.Read();
               	Thread.Sleep(100);//пауза
               	Buffer+=b0; //ответ. первый блок аварий
               	if(String.IsNullOrEmpty(b0)) endread=true;
               }			
				buffAlarms=Program.Helper.MergeAlarms(buffAlarms,ParseAlarms(Buffer));//объединяем считанные аварии с прошлыми
               repeats--;
               }
                //вышли из цикла 

                #region analyze new alarms and generate event for Beep

                var diffAlarms = buffAlarms.Where(x => !Alarms.Any(c => c == x));//only new alarms
                if (diffAlarms.Any(x => this.Ports.Any(p => p.Monitored
                                                            && p.BordNumber == x.BordNumber
                                                            && p.PortNumber == x.PortNumber)))
                {//среди новых аварий есть аварии, принадлежащие порту с включенным мониторингом
                    DxcEvent(this.CustomName, "new alarms: "+diffAlarms.Count());
                }

                    #endregion
                Alarms=Program.Helper.MergeAlarms(Alarms,buffAlarms);
               tc.Close(); 
               DxcEvent("system", "tcp-closed");
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
                DxcEvent("system", "tcp-closed");
            }
        	
        }

/// <summary>
/// Парсинг аварий из выдачи команды dsp alm 
/// </summary>
/// <param name="buffer"></param>
/// <returns></returns>
	public static List<Alarm> ParseAlarms(string buffer)
		{
		string dateTimeLine="";
		string Line="";//for debugging
			List<Alarm> list=new List<Alarm>();
			if(String.IsNullOrWhiteSpace(buffer)) return list;
			try {
					string[] lines=buffer.Split('\n');
			foreach(string line in lines)
			{//по факту перебор строк идет с конца хронологии событий,
				//т.е. сначала конец события, а потом только начало в следующих строках
				if(String.IsNullOrWhiteSpace(line)) continue;
                Line = line;
				var words=Split(line,"  ");//2 spaces
				if(!words[0].Contains("IO-")) continue;
                    Alarm alm = new Alarm(false)
                    {
                        #region initialize
                        Start = new DateTime(1000, 1, 1),
                        End = new DateTime(1000, 1, 1),
                        PortNumber = 0,
                        BordNumber = 0,
                        #endregion
                        Io = words[0]
                    };
                    var subs=alm.Io.Split('-');
				var b=subs[1].Split(':')[0].Trim();
				var p=subs[1].Split(':')[1].Trim();
int nb=0;
int np=0;
				int.TryParse(b, out nb);
				int.TryParse(p, out np);
				alm.BordNumber=nb;
				alm.PortNumber=np;
				alm.Name=words[1];
				alm.Status=words[2].Trim();
				dateTimeLine=words[3];//for debugging
				var dateTime=DateTime.Parse(dateTimeLine.Trim('\a'));//.Split(' ')[0]);
				#region Start End DateTime
				if(alm.Status=="EVENT")
					alm.Start=alm.End=dateTime;
				else if(alm.Status=="OFF") 
				{   alm.End=dateTime;
					alm.Active=false;
				}
				else { //Начало события. Ищем, была ли эта авария ранее в списке с OFF)
					int ind=list.FindIndex(x=>x.BordNumber==alm.BordNumber &&
					           x.PortNumber==alm.PortNumber &&
					           x.Name==alm.Name &&
					           x.Start==new DateTime(1000,1,1));
					if(ind>=0)//есть авария с таким именем, портом и без даты начала
					{Alarm a=list[ind]; //End уже должно быть, авария закрыта
					a.Start=dateTime;
					list[ind]=a;
					continue;
					}else {
						alm.Start=dateTime;
						alm.Active=true;
					}
				}
				#endregion
				list.Add(alm);
			}
			
			return list;	
			} catch (Exception ex) {
				
				//MessageBox.Show(ex.Message, "ParseAlarms");
				Log.WriteLog("parseAlarms",ex.Message+"\r\ndateLine: "
				             +dateTimeLine+"\r\ndateLineTrim: "
				             +dateTimeLine.Trim('\a')
				            +"\r\nLine: "+ Line);
				return list;
			}
		}
 public static bool IpPingOk(string ip)
    {
    	Ping p = new Ping();
				if (p.Send(ip).Status == IPStatus.Success)
				{
					return true;
				}
				return false;

    }
 public static List<string> Split(string input, string sep)
 {
 	List<string> result=new List<string>();
 	int prev=0;
 	for (int i=0;i<input.Length;i++)
 	{bool flag=false;
 		if(i<prev) continue;
 		if(i+sep.Length<=input.Length)
 		for(int k=0;k<sep.Length;k++)
 		{
 			if(input[i+k]!=sep[k]) break;
 			//if all OK
					flag |= k == sep.Length - 1;
 		}
 		if(flag) 
 		{
 			if(!string.IsNullOrWhiteSpace(input.Substring(prev,i-prev)))
 				result.Add(input.Substring(prev,i-prev));
 			prev=i+sep.Length;
 		}
 		//last
 		if(i==input.Length-1) result.Add(input.Substring(prev,i-prev+1));
 	}
 	return result;
 }

 public Port DSP_CON(int bord,int port)
 {
     string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
     var PORT = this.Ports.FirstOrDefault(x => x.BordNumber == bord && x.PortNumber == port);
            try
     {
         if (!IpPingOk(Ip))
         {
             // MessageBox.Show("Адрес "+ip+" не доступен.");
             DxcEvent(this.CustomName, this.Ip + " адрес не доступен");
             return PORT;
         }

        
         TelnetConnection tc = new TelnetConnection(Ip, 23);
         string ans = tc.Read();
         Buffer = ans;
         Thread.Sleep(100);
         tc.WriteLine(String.Format("dsp con {0} {1}",bord,port));

         Thread.Sleep(1500);
         Buffer = tc.Read();
         //Thread.Sleep(500);
         if(!String.IsNullOrWhiteSpace(Buffer))
         PORT.Connections.ParseTextDSP_CON(Buffer);
         tc.Close();
         return PORT;
     }
     catch (Exception exception)
     {
         //Program.MF.InvokeLog(methodName, exception.Message);
         Log.WriteLog(methodName, exception.Message+"buffer: "+Buffer);
         return PORT;
     }
 }
    }
}
