/*
 * Создано в SharpDevelop.
 * Пользователь: user
 * Дата: 11.12.2020
 * Время: 11:04
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

namespace DXC
{
	/// <summary>
	/// Description of Port.
	/// </summary>
	public class Port
	{
		public string Name{get;set;}
		public int BordNumber{get;set;}
		public int PortNumber{get;set;}
		public bool Alarmed{get;set;}
		public bool Monitored{get;set;}
		public List<Alarm> Alarms;
		//TODO add fields data
        public Connections Connections;
		private readonly char _sep=';';
		public Port()
		{
			Name="";
			BordNumber=0;
			PortNumber=0;
			Alarmed=false;
			Monitored=true;
			Alarms=new List<Alarm>();
			Connections=new Connections(this);
		}
		public override string ToString()
		{
			return String.Format("Port={1}{0}{2}{0}{3}{0}{4}",_sep,BordNumber,PortNumber,Name,Monitored);
		}
		
		/// <summary>
		/// Parse line from file
		/// </summary>
		/// <param name="line">BordNumber,PortNumber,Name,Monitored</param>
		public Port(string line)
		{
			if(String.IsNullOrWhiteSpace(line)) return;
			var words=line.Split(_sep);
			if(words.Length!=4) return;
			Name=words[2];
			int b=0;
			int p=0;
			int.TryParse(words[0],out b);
			int.TryParse(words[1],out p); 
			BordNumber=b;
			PortNumber=p;
			Alarmed=false;
			bool mon=true;
			bool.TryParse(words[3],out mon);
			Monitored=mon; 
			Alarms=new List<Alarm>();
			Connections=new Connections(this);			
		}
	}

	public class CrossCon
    {
        public int LTs=-1;
        public int LBord=-1;
        public int LPort=-1;
        public int RTs=-1;
        public int RBord=-1;
        public int RPort=-1;
        public string Status="NC";

        public CrossCon()
        {
			
        }

        public CrossCon(int lb,int lp,int lts,int rb,int rp,int rts,string stat)
        {
            LBord = lb;
            LPort = lp;
            LTs = lts;
            RBord = rb;
            RPort = rp;
            RTs = rts;
            Status = stat;
        }
    }

	/// <summary>
	/// Contains 31 cross-connections
	/// </summary>
    public class Connections
    {
        public CrossCon[] Cons;

        public Connections(Port p)
        {
            Cons = new CrossCon[31];
            for (var i = 0; i < Cons.Length; i++)
            {
                Cons[i] = new CrossCon {LBord = p.BordNumber, LPort = p.PortNumber, LTs = i+1};
            }
        }
        
        /// <summary>
        /// Чтение кросс-коннекций из ответа dxc на команду dsp con
        /// </summary>
        /// <param name="test">текст ответа на команду</param>
        public void ParseTextDSP_CON(string text)
        {
        	string methodName = new StackTrace(false).GetFrame(0).GetMethod().Name;
            try
            {
if (string.IsNullOrWhiteSpace(text)) return;
var lines=text.Split('\n');
for (int i = 0; i < lines.Length; i++) {
	if(String.IsNullOrWhiteSpace(lines[i])) continue;
	
	if(lines[i].Contains("TS  :"))
	{
		var tsString=lines[i].Split(':')[1].Trim();//right side after :
		var typeString=lines[i+1].Split(':')[1].Trim();
		var destString=lines[i+2].Substring(7).Trim();
		var bloksTs=Split10Chars(tsString);
		var bloksType=Split10Chars(typeString);
		var bloksDest=Split10Chars(destString);
		if(bloksTs.Length==0 || bloksDest.Length==0 || bloksType.Length==0) continue;
		for(int t=0; t<bloksTs.Length; t++) //перебор всех блоков в строке
		{
			int ts=-1; //number of TS
			string n=bloksTs[t].Split('O')[1].Trim(); //'NO 22     '
			if(!int.TryParse(n, out ts)) continue;
			string type=bloksType[t].Trim();
			var dest=bloksDest[t].Trim().Split(':');
			if(dest.Length!=3) continue;
			Cons[ts-1].Status=type;
			Cons[ts-1].RBord=int.Parse(dest[0].Trim());
			Cons[ts-1].RPort=int.Parse(dest[1].Trim());
			Cons[ts-1].RTs=int.Parse(dest[2].Trim().TrimEnd('F').TrimEnd('/'));
		}
	} 
}
            }
            catch (Exception exception)
            {
                Log.WriteLog(methodName, exception.Message);
            }
        }

        /// <summary>
        /// Разбивает строку на части по 10 символов
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string[] Split10Chars(string input)
        {	
        	try {
        		string[] bloks;
        		input=input.Trim();
        		if(String.IsNullOrWhiteSpace(input)) return new string[0];
        		if(input.Length>30) bloks=new string[7];
        		else bloks=new string[3]; //short string (last)
        		for (int i = 0; i <bloks.Length; i++) 
        		{
        				if(i!=bloks.Length-1)bloks[i]=input.Substring(i*10,10);
        				else
                            bloks[i]=input.Substring(i*10);//last blok
        		}
        		return bloks;
        	} catch (Exception ex) {
        		Log.WriteLog("split10chars",ex.Message+"\r\ninput="+input);
        		return new string[0];
        	}
        }

    }
}
