/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 01.02.2021
 * Time: 14:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.IO;
using System.Text.RegularExpressions;

namespace DXC
{
	/// <summary>
	/// Description of MyDGV.
	/// </summary>
	public class DataStorage
	{
	    private ClassDxc dxc;
	    private int CountPerFile=2000;
	    private const string DateFormat="dd.MM.yyyy";
	    public string fileDxc{
	        get{
	            if(dxc!=null && dxc.Info!=null)
	        return dxc.Info.SysName+".txt";
	        else return "";
	    }
	        
	    }
	    
		public DataStorage(ClassDxc Dxc)
		{
		    dxc=Dxc;
			
		}
		
		public void SaveAllAlarms()
		 {
			if(dxc==null || dxc.Alarms==null)return;
		     //разделяет все аварии по количеству, например по 5000 и сохраняет в отдельные файлы
		     var lists=SplitAlarms(dxc.Alarms, CountPerFile);
		     foreach(var list in lists)
		     {
		         WriteAlarmsToFile(list,GenerateFileName(list));
		     }
		 }
		 //разбивает список на несколько подсписков по количеству 
		 private List<Alarm>[] SplitAlarms(List<Alarm> alarms, int count)
		 {
		 	try {
		 		if(alarms.Count<=count) return new List<Alarm>[] {alarms};
		 	int n=(int)Math.Ceiling((double)alarms.Count/count);//кол-во сегментов
		 	List<Alarm>[] groups= new List<Alarm>[n];
		 	groups[0]=new List<Alarm>();
		 	int c=0;
		 	foreach (Alarm alarm in alarms) {
		 		if(groups[c].Count > count) 
		 		{
		 			c++;
		 			groups[c]=new List<Alarm>();
		 		}
		 		groups[c].Add(alarm);
		 	}
		 	return groups;
		 	}catch(Exception ex)
		 	{
		 		Log.WriteLog("SplitAlarms",ex.Message);
		 		return new List<Alarm>[0];
		 	}
		 }
		 //генерация строки названия файла с диапазоном дат записанных аварий и названием dxc 
		 private string GenerateFileName(List<Alarm> alarms)
		 {
		 	if(!Directory.Exists(dxc.Info.SysName))Directory.CreateDirectory(dxc.Info.SysName);
		 	return dxc.Info.SysName+"\\"+dxc.Info.SysName+" Аварии за период="+alarms.Min(x=>x.Start).ToString(DateFormat)+" - "+
		 		alarms.Max(x=>x.Start).ToString(DateFormat)+".txt";
		     
		 }
		 private DateTime[] ParseFileName(string name)
		 {
		 	
		 	string formatRegEx=@"\d\d[.]\d\d[.]\d\d\d\d";
		 	var  matches=Regex.Matches(name, formatRegEx);
				 if( matches.Count==2) 
				 {
				 	DateTime t1,t2;
				 	if(DateTime.TryParseExact(matches[0].Value,DateFormat,null,System.Globalization.DateTimeStyles.None,out t1 )
				 	   && DateTime.TryParseExact(matches[1].Value,DateFormat,null,System.Globalization.DateTimeStyles.None,out t2 ))
				 	{
				 		
				 		if(t1>t2) {
				 		DateTime.TryParseExact(matches[0].Value,DateFormat,null,System.Globalization.DateTimeStyles.None,out t2 );
				 	    DateTime.TryParseExact(matches[1].Value,DateFormat,null,System.Globalization.DateTimeStyles.None,out t1 );	
				 		}
				 		
				 		return new DateTime[] {t1,t2};
				 	}
				 	
				 	return new DateTime[1];
				 } else return new DateTime[1];
		 	
		 }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		 public List<Alarm> GetAlarmsFromInterval(DateTime from, DateTime to)
		 {
			List<Alarm> listAlarms = new List<Alarm>();
			if(!Directory.Exists(dxc.Info.SysName)) return listAlarms;
			var fileNames=Directory.GetFiles(dxc.Info.SysName);
foreach (string name in fileNames) {
				var interval=ParseFileName(name.Split('\\').Last());
				if(interval.Length!=2) continue;
				List<Alarm> readedAlarms=new List<Alarm>();
				if((interval[0] <= from && interval[1] >= to) ||
				   (interval[0] <= from && interval[1] >= from )||
				   (interval[0] <= to && interval[1] >= to))
					 readedAlarms=ReadAlarmsFromFile(name);
					listAlarms=MergeAlarms(listAlarms,readedAlarms);
}
			return listAlarms;
		}

		/// <summary>
		/// Сохраняет указанные аварии в файл. Если файл существует, то уже записанные аварии объединяются.
		/// </summary>
		/// <param name="alarms">Аварии для сохранения</param>
		/// <param name="file"></param>
		private void WriteAlarmsToFile(List<Alarm> alarms, string file)
        {
			ClassDxc.WriteAlarmsToFile(file,alarms);
        	
        }
        private List<Alarm> ReadAlarmsFromFile(string file)
        {
        	List<Alarm> alarms=new List<Alarm>();
        	return alarms;
        }
       //объединяет аварии, исключая повторы
        private List<Alarm> MergeAlarms(List<Alarm> alarms1, List<Alarm> alarms2)
        {
            List<Alarm> alarms=new List<Alarm>();
            return alarms;
        }
	}
	
	
}
