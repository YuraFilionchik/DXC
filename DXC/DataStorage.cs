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
	    public string FileNameDxcInfo{
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
		public void SaveDXCinfo()
        {
		if(!String.IsNullOrEmpty(FileNameDxcInfo))	File.WriteAllText(FileNameDxcInfo, dxc.ToString());
		}
		public void SaveAllAlarms()
		 {
			if(dxc==null || dxc.Alarms==null)return;
		     //разделяет все аварии по количеству, например по 5000 и сохраняет в отдельные файлы
		     var lists=SplitAlarms(dxc.Alarms, CountPerFile);
		     foreach(var list in lists)
		     {
		        if(list.Count!=0) WriteAlarmsToFile(list,GenerateFileName(list));
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
		 		if(groups[c].Count >= count) 
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
			if (alarms == null | alarms.Count == 0) return "";
		 	if(!Directory.Exists(dxc.Info.SysName))Directory.CreateDirectory(dxc.Info.SysName);
			string flag = alarms.Count==CountPerFile ?"":"~";
			
		 	return dxc.Info.SysName+"\\"+flag+dxc.Info.SysName+" Аварии за период="+alarms.Min(x=>x.Start).ToString(DateFormat)+" - "+
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
			try
			{
				//read all records from file
				List<Alarm> oldAlarms = new List<Alarm>();
				List<string> result = new List<string>();
				if (File.Exists(file))
				{
					var lines = File.ReadAllLines(file);
					//read OldAlarms
					foreach (string line in lines)
					{
						Alarm a = new Alarm(false);
						a.ParseLine(line);
						oldAlarms.Add(a);
					}

					//поиск и закрытие отработанных аварий, которые в старых списках еще открыты
					var mergedAlarms = MergeAlarms(oldAlarms, alarms);
					
				if(file!=GenerateFileName(mergedAlarms))//после объединения имя должно поменяться
					{
						File.Delete(file);
						file = GenerateFileName(mergedAlarms);
					}else	
						File.WriteAllText(file, ""); //обнулили файл
					result = mergedAlarms.ConvertAll(x => x.ExportLineCsv());

				}//if file not exist
				else
				{
					result = alarms.ConvertAll(x => x.ExportLineCsv());
				}
								
				File.WriteAllLines(file, result.ToArray());
			}


			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Datastorage.Write listAlarms to file");
			}

		}
		
		public List<Alarm> ReadAlarmsFromFile(string file)
        {
			List<Alarm> results = new List<Alarm>();
			try
			{
				if (!File.Exists(file)) { MessageBox.Show("Файл " + file + " не найден"); return results; }

				var lines = File.ReadAllLines(file);
				foreach (string line in lines)
				{

					Alarm alm = new Alarm(false).ParseLine(line);

					results.Add(alm);
				}
				return results;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "DataStorage. Reading alarms from file :" + file);
				return results;
			}
		}

		/// <summary>
		/// объединяет аварии,включая проверку аварии на изменение статуса(активная или нет) исключая повторы 
		/// </summary>
		/// <param name="list1"></param>
		/// <param name="list2"></param>
		/// <returns>ОбЪединенный список аварий</returns>	   
		public List<Alarm> MergeAlarms(List<Alarm> list1, List<Alarm> list2)
        {
			try
			{
				for (int i = 0; i < list1.Count(); i++)
				{
					var a = list1[i];
					//только активные в list1
					if (a.Active && list2.Any(x => x == a))//new alarms contains old Active alarm
					{
						var newAlm = list2.First(x => x == a); //авария в list2, которая есть и в list1
						if (!newAlm.Active) //Закрываем старую аварию, если новая закрыта(неактивна)
						{
							a.End = newAlm.End;
							a.Active = false;
							a.Status = newAlm.Status;
							list1[i] = a;

						}
					}

				}
				//просто добавляем к первому списку отсутсвующие аварии
				foreach (Alarm alarm in list2)
				{
					if (list1.All(x => x != alarm))
						list1.Add(alarm);
				}
				return list1;


			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "HELP. MergeAlarms");
				return list1;
			}
		}
	}
	
	
}
