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
		private char sep=';';
		public Port()
		{
			Name="";
			BordNumber=0;
			PortNumber=0;
			Alarmed=false;
			Monitored=true;
			Alarms=new List<Alarm>();
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
			
		}
	}
}
