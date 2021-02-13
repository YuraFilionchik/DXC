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
{ public delegate void DXCEventHandler(string DXC_Name, string msg);
	/// <summary>
	/// Description of DXC.
	/// </summary>
	 public class ClassDXC
    {
	 	public static string ttt;
        public string ip;
        public string custom_Name;
        public string backupPath;
        public List<Alarm> alarms;
        public dxcinfo info;
        public List<Port> Ports;
        public event DXCEventHandler DXCEvent;
		public TFTPSession _session; 
        public override string ToString()
        {
        	string portsString="";
        	foreach (var port in Ports) {
        		portsString+=port.ToString()+"\n";
        	}
            return String.Format("ip={0}\nname={1}\nclock={2}\nDBnumber={3}\n" +
                                 "Installed modules:\n{4}\nПорты:\n{5}\ndate={6}\ntime={7}\nРазбежка времени: {8}дн\n {9}ч {10}мин {11}с", 
                                 ip, info.sys_name, info.nodalclock, info.activeDBnuber,
                                 info.ToString(),portsString, info.date.ToShortDateString(), 
                                 info.time.ToLongTimeString(),(int)info.dt.TotalDays,
                                info.dt.Hours,info.dt.Minutes, info.dt.Seconds);
        }
        //private IniFile Cfg;
        public string buffer;

        public ClassDXC(string ip)
        {
            string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
             _session = new TFTPSession();
            try
            {
            	if(!MainForm.IPformat(ip))
            	{
            		MessageBox.Show("Неверный формат IP");
            		return;
            	}
                buffer = "";
                //Cfg=cfg;
                this.ip = ip;
                this.custom_Name="";
                this.backupPath = "";
                this.alarms = new List<Alarm>();
                this.info=new dxcinfo(false);
                this.Ports=new List<Port>();
                 
                 _session.Connected += new TFTPSession.ConnectedHandler(_session_Connected);
            _session.Transferring += new TFTPSession.TransferringHandler(_session_Transferring);
            _session.TransferFailed += new TFTPSession.TransferFailedHandler(_session_TransferFailed);
            _session.TransferFinished += new TFTPSession.TransferFinishedHandler(_session_TransferFinished);
            _session.Disconnected += new TFTPSession.DisconnectedHandler(_session_Disconnected);
                
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
			DXCEvent(this.info.sys_name,"TFTP "+ip+" connected");
        }

        private void _session_Transferring(long BytesTransferred, long BytesTotal)
        {
            //if (BytesTotal != 0)
            {
            	//if(((int)(100*BytesTransferred)/BytesTotal)%10==0)
            	//DXCEvent(this.info.sys_name,String.Format("{0}/{1}bytes",BytesTransferred,BytesTotal));
            }
            //else
            	//DXCEvent(this.info.sys_name,".");
        }

        private void _session_TransferFailed(short ErrorCode, string ErrorMessage)
        {
            //Console.WriteLine("Error {0}: {1}", ErrorCode, ErrorMessage);
            DXCEvent(this.info.sys_name,"Error: "+ErrorMessage);
        }

        private void _session_TransferFinished()
        {
        	DXCEvent(this.info.sys_name,"Файл загружен "+this.backupPath);
        }

        private void _session_Disconnected()
        {
            MainForm.Instance.EnableButtons();
			DXCEvent(this.info.sys_name,"TFTP "+ip+" Disconnected");
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
                if (cfg.KeyExists("backup", ip)) this.backupPath = cfg.ReadINI(ip, "backup");
                if (cfg.KeyExists("name", ip)) this.info.sys_name = cfg.ReadINI(ip, "name");
                if(cfg.KeyExists("TimeCorrection",ip)) this.info.dt=new TimeSpan(long.Parse(cfg.ReadINI(ip,"TimeCorrection")));
                if (string.IsNullOrWhiteSpace(this.info.sys_name)) return false;
                //load from file
                if (!File.Exists(this.info.sys_name + ".txt")) return false;
                var lines = File.ReadAllLines(this.info.sys_name + ".txt");
                this.info.modules=new Dictionary<string, string>();
                foreach (string line in lines)
                {
                    var values = line.Split('=');
                    if (values.Length < 2) continue;
                    if (values[0] == "ip" && values[1] != this.ip)
                    {
                        MessageBox.Show("Несоответствие ip в файле " + values[1] + ". Файл " + this.info.sys_name + ".txt");
                        return false;
                    }
                    if (values[0] == "name" && values[1] != this.info.sys_name)
                    {
                        MessageBox.Show("Несоответствие имени в файле: " + values[1] + ". Файл " + this.info.sys_name + ".txt");
                        return false;
                    }
                    if (values[0] == "clock") {this.info.nodalclock = values[1]; continue;}
                    if (values[0] == "DBnumber") {this.info.activeDBnuber = values[1]; continue;}
                    if (values[0].Contains("slot"))
                    {
                        this.info.modules.Add(values[0].Split('.')[1], values[1]);
                        continue;
                    }
                    
                     if (values[0].Contains("Port"))
                     {
                     	this.Ports.Add(new Port(values[1]));
                     	continue;
                     }
                     
               
                    
                     if (values[0] == "date") {this.info.date = DateTime.Parse(values[1]); continue;}
                     if (values[0] == "time") {this.info.time = DateTime.Parse(values[1]); continue;}
					

                }
                if(this.Ports.Count==0) //try import from modules
                     {
                     	foreach (var modul in info.modules) {
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
            if(string.IsNullOrWhiteSpace(this.info.sys_name)) return;
            cfg.Write(ip,"backup",this.backupPath);
            cfg.Write(ip,"name",this.info.sys_name);
            cfg.Write(ip,"Alarms_file",MainForm.GetAlarmsFilePath(this)); //имя файла с историей аварий DXC
             cfg.Write(ip,"TimeCorrection",this.info.dt.Ticks.ToString());
             WriteAlarmsToFile(MainForm.GetAlarmsFilePath(this));
             string file = this.info.sys_name + ".txt";
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
            if (obj is ClassDXC)
                return Equals((ClassDXC)obj); // use Equals method below
            else
                return false;
        }

        public bool Equals(ClassDXC other)
        {
        	//if(other==null) return false;
            // add comparisions for all members here
            return (this.ip == other.ip &&
                this.info == other.info );
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

        public void UpdateDB(string Number)
        {string methodName= new StackTrace(false).GetFrame(0).GetMethod().Name;
            if(!IpPingOK(ip))
            {
                DXCEvent(this.custom_Name, this.ip + " адрес не доступен. Не удалось сохранить Backup!");
                return;
            }
        	TelnetConnection tc =new TelnetConnection(ip,23);
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
                
                string command="upd db "+Number;
                
                DXCEvent(this.info.sys_name, "Открытие telnet "+ip);
                string ans = tc.Read();
             // DXCEvent(this.info.sys_name, "Успешно");
                Thread.Sleep(200);
                DXCEvent(this.info.sys_name, "Отправка команды "+command);
               tc.WriteLine(command);
               Thread.Sleep(2000);
               DXCEvent(this.info.sys_name, "Успешно.");
                tc.Close();
                DXCEvent(this.info.sys_name, "Соединение закрыто.");

            }
            catch (Exception exception)
            {
            	if(tc.IsConnected){
            		tc.Close(); 
                DXCEvent(this.info.sys_name, "Произошла ошибка. Соединение закрыто");
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

        public void template()
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

        private dxcinfo parseDXCInfo(string text)
        {
        	 string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
        	 if (string.IsNullOrWhiteSpace(text)) return new dxcinfo();
            dxcinfo info = new dxcinfo();
            try
            {
            
            info.modules = new Dictionary<string, string>();
            text = text.Replace('\r', ' ');
            var lines = text.Split('\n');

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                var wordsAll = line.Split(' ');
                var words = wordsAll.Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

                if (words.Length >= 6 && words[3].Contains("NAME"))
                {
                    info.sys_name = words[5]; //name
                    continue;
                }
                if (words.Length >= 4 && words[1].Contains("CLOCK"))
                {
                    info.nodalclock = words[3]; //nodal clock
                    continue;
                }
                if (words.Length >= 9 && words[5].Contains("DATABASE") && words[6].Contains("NUMBER")) //DB number
                {
                    info.activeDBnuber = words[8];
                    continue;
                }
                if (words.Length >= 7 && words[0].Contains("I/O"))
                {
                    var wordsNext = lines[i + 2].Split(' ').Where(s => !string.IsNullOrWhiteSpace(s)).ToArray(); ;
                    info.modules.Add(words[2], wordsNext[2]);
                    info.modules.Add(words[3], wordsNext[3]);
                    info.modules.Add(words[4], wordsNext[4]);
                    info.modules.Add(words[5], wordsNext[5]);
                    info.modules.Add(words[6], wordsNext[6]);
                    continue;
                }
                if (words.Length >= 4 && words[0].Contains("TIME"))
                {
                    info.time = DateTime.ParseExact(words[1], "HH:mm:ss", null);
                    info.date = DateTime.Parse(words[3]);
                    DateTime fullDate=new DateTime(info.date.Year,info.date.Month,info.date.Day,
                                                   info.time.Hour,info.time.Minute, info.time.Second);
                    info.dt=fullDate-DateTime.Now; //установка разницы во времени оборудования DXC и данного ПК
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
        public void MakeBackUp(string N)
        {
        	//UpdateDB(N); //save conf to Db2 file on DXC
        	CopyFileFromServer("DB"+N+"CONF.CFG",this.ip, this.backupPath);
        	//Thread.Sleep(1000);
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
if(!IpPingOK(ip)) {
        		//MessageBox.Show(ip+" адрес не доступен");
                DXCEvent(this.custom_Name, this.ip + " адрес не доступен");
        		return;
        	}
        	 List<Alarm> buffAlarms=new List<Alarm>();
        	 TelnetConnection tc =new TelnetConnection(ip,23);
                string ans = tc.Read();
                buffer=ans;
              Thread.Sleep(100);
               tc.WriteLine("dsp alm"); //запрос
               Thread.Sleep(200);//пауза
               buffer=tc.Read(); //ответ. первый блок аварий
               buffAlarms=parseAlarms(buffer); //аварии первого запроса
               
               if(alarms.Any(x=>buffAlarms.Any(b=>b==x && b.active==x.active)))
               {//в буфере уже есть хотябы одна авария из ранее считанных
               	alarms=Program.Helper.MergeAlarms(alarms,buffAlarms);
               	 tc.Close(); 
               	return;
               }//повторяем пока не дойдем до существующей аварии или не достигнем счетчика
               while (repeats>1 && !alarms.Any(x=>buffAlarms.Any(b=>b==x && b.active==x.active))) {
               				
				tc.Write(" ");				
				Thread.Sleep(100);
				buffer+=tc.Read(); //второй блок аварий
				buffAlarms=Program.Helper.MergeAlarms(buffAlarms,parseAlarms(buffer));//объединяем считанные аварии с прошлыми
               repeats--;
               }
                //вышли из цикла 

                #region analyze new alarms and generate event for Beep

                var diffAlarms = buffAlarms.Where(x => !alarms.Any(c => c == x));//only new alarms
                if (diffAlarms.Any(x => this.Ports.Any(p => p.Monitored
                                                            && p.BordNumber == x.bordNumber
                                                            && p.PortNumber == x.portNumber)))
                {//среди новых аварий есть аварии, принадлежащие порту с включенным мониторингом
                    DXCEvent(this.custom_Name, "new alarms: "+diffAlarms.Count());
                }

                    #endregion
                alarms=Program.Helper.MergeAlarms(alarms,buffAlarms);
               tc.Close(); 
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
        	
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
        		Alarm A=new Alarm(false);
        		A.ParseLine(line);
        		oldAlarms.Add(A);
        	}
        	
        	//поиск и закрытие отработанных аварий, которые в старых списках еще открыты
        	var MergedAlarms=Program.Helper.MergeAlarms(oldAlarms,alarms.ToList());

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
        		alarms=results;
        		return results;
        	} catch (Exception ex) {
        		MessageBox.Show(ex.Message,"Reading alarms from file :"+file);
        		return results;
        	}
        }
        public List<Alarm> GetCorrectedAlarms()
        {
        	return FixAlarmTime(alarms);
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
        		var Alm=alarm.GetCopyAlarm();
        		Alm.Start-=info.dt;
        		Alm.End-=info.dt;
        		
        		correctedAlarms.Add(Alm);
        	}
        	return correctedAlarms;
        }
        
        /// <summary>
        /// Connect via tftp to DXC and Copy conf file
        /// </summary>
        /// <param name="file"></param>
        /// <param name="ip"></param>
        /// <param name="DestFile"></param>
        private void CopyFileFromServer(string file, string ip, string DestFile)
       {
        	if(!IpPingOK(ip)) {
        		MessageBox.Show(ip+" адрес не доступен");
                DXCEvent(this.custom_Name, this.ip + " адрес не доступен");
                return;
        	}
        	try {
   
      		TransferOptions tOptions = new TransferOptions();            
            tOptions.LocalFilename = DestFile;
            tOptions.RemoteFilename = file;
            tOptions.Host = ip;
            tOptions.Action = TransferType.Get;
            _session.Get(tOptions);
   
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
        private void SaveStreamToFile(Stream stream, string DestFile)
        {
        	try {
        		FileStream fs=new FileStream(DestFile,FileMode.Create);
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
        public bool ReadInfoFromIP()
        {
        string methodName= new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
                if (!IpPingOK(ip))
                {
                   // MessageBox.Show("Адрес "+ip+" не доступен.");
                    DXCEvent(this.custom_Name, this.ip + " адрес не доступен");
                    return false;
                }
                TelnetConnection tc =new TelnetConnection(ip,23);
                string ans = tc.Read();
                buffer=ans;
               // Program.MF.InvokeLog(methodName, ans);
                Thread.Sleep(100);
               tc.WriteLine("dsp st sys");
               
               Thread.Sleep(1000);
               buffer=tc.Read();
               //   Thread.Sleep(3000);
               // Program.MF.InvokeLog(methodName, buffer);
                info=parseDXCInfo(buffer);
                tc.Close();
                return true;
            }
            catch (Exception exception)
            {
                //Program.MF.InvokeLog(methodName, exception.Message);
                Log.WriteLog(methodName, exception.Message);
                return false;
            }
        }
/// <summary>
/// Парсинг аварий из выдачи команды dsp alm 
/// </summary>
/// <param name="buffer"></param>
/// <returns></returns>
	public static List<Alarm> parseAlarms(string buffer)
		{
		string dateTimeLine="";
		string Line="";
			List<Alarm> list=new List<Alarm>();
			if(String.IsNullOrWhiteSpace(buffer)) return list;
			try {
					string[] lines=buffer.Split('\n');
			foreach(string line in lines)
			{//по факту перебор строк идет с конца хронологии событий,
				//т.е. сначала конец события, а потом только начало в следующих строках
				if(String.IsNullOrWhiteSpace(line)) continue;
				var words=Split(line,"  ");//2 spaces
				if(!words[0].Contains("IO-")) continue;
				Alarm alm=new Alarm(false);
				#region initialize
				alm.Start=new DateTime(1000,1,1);
				alm.End=new DateTime(1000,1,1);
				alm.portNumber=0;
				alm.bordNumber=0;
				#endregion
				alm.io=words[0]; 
				var subs=alm.io.Split('-');
				var b=subs[1].Split(':')[0].Trim();
				var p=subs[1].Split(':')[1].Trim();
int nb=0;
int np=0;
				int.TryParse(b, out nb);
				int.TryParse(p, out np);
				alm.bordNumber=nb;
				alm.portNumber=np;
				alm.name=words[1];//TODO check Parse
				alm.status=words[2].Trim();
				dateTimeLine=words[3];//for debugging
				var dateTime=DateTime.Parse(dateTimeLine.Trim('\a'));//.Split(' ')[0]);
				#region Start End DateTime
				if(alm.status=="EVENT")
					alm.Start=alm.End=dateTime;
				else if(alm.status=="OFF") 
				{   alm.End=dateTime;
					alm.active=false;
				}
				else { //Начало события. Ищем, была ли эта авария ранее в списке с OFF)
					int ind=list.FindIndex(x=>x.bordNumber==alm.bordNumber &&
					           x.portNumber==alm.portNumber &&
					           x.name==alm.name &&
					           x.Start==new DateTime(1000,1,1));
					if(ind>=0)//есть авария с таким именем, портом и без даты начала
					{Alarm A=list[ind]; //End уже должно быть, авария закрыта
					A.Start=dateTime;
					list[ind]=A;
					continue;
					}else {
						alm.Start=dateTime;
						alm.active=true;
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
 public static bool IpPingOK(string ip)
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
 
    }
}
