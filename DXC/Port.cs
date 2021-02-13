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
		//TODO add fields data, CrossCon-s
        public Connections Connections;
		private char sep=';';
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
			return String.Format("Port={1}{0}{2}{0}{3}{0}{4}",sep,BordNumber,PortNumber,Name,Monitored);
		}
		
		/// <summary>
		/// Parse line from file
		/// </summary>
		/// <param name="line">BordNumber,PortNumber,Name,Monitored</param>
		public Port(string line)
		{
			if(String.IsNullOrWhiteSpace(line)) return;
			var words=line.Split(sep);
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
        public int L_ts=-1;
        public int L_bord=-1;
        public int L_port=-1;
        public int R_ts=-1;
        public int R_bord=-1;
        public int R_port=-1;
        public string status="NC";

        public CrossCon()
        {
			
        }

        public CrossCon(int Lb,int Lp,int Lts,int Rb,int Rp,int Rts,string stat)
        {
            L_bord = Lb;
            L_port = Lp;
            L_ts = Lts;
            R_bord = Rb;
            R_port = Rp;
            R_ts = Rts;
            status = stat;
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
                Cons[i] = new CrossCon {L_bord = p.BordNumber, L_port = p.PortNumber, L_ts = i+1};
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
		var TS_string=lines[i].Split(':')[1].Trim();//right side after :
		var TYPE_string=lines[i+1].Split(':')[1].Trim();
		var DEST_string=lines[i+2].Substring(7).Trim();
		var bloksTS=Split10chars(TS_string);
		var bloksTYPE=Split10chars(TYPE_string);
		var bloksDEST=Split10chars(DEST_string);
		if(bloksTS.Length==0 || bloksDEST.Length==0 || bloksTYPE.Length==0) continue;
		for(int t=0; t<bloksTS.Length; t++) //перебор всех блоков в строке
		{
			int TS=-1; //number of TS
			string N=bloksTS[t].Split('O')[1].Trim(); //'NO 22     '
			if(!int.TryParse(N, out TS)) continue;
			string TYPE=bloksTYPE[t].Trim();
			var DEST=bloksDEST[t].Trim().Split(':');
			if(DEST.Length!=3) continue;
			Cons[TS-1].status=TYPE;
			Cons[TS-1].R_bord=int.Parse(DEST[0].Trim());
			Cons[TS-1].R_port=int.Parse(DEST[1].Trim());
			Cons[TS-1].R_ts=int.Parse(DEST[2].Trim().TrimEnd('F').TrimEnd('/'));
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
        private string[] Split10chars(string input)
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
        				else bloks[i]=input.Substring(i*10);//last blok
        		}
        		return bloks;
        	} catch (Exception ex) {
        		Log.WriteLog("split10chars",ex.Message+"\r\ninput="+input);
        		return new string[0];
        	}
        }

    }
}
