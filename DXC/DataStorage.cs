﻿/*
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

namespace DXC
{
	/// <summary>
	/// Description of MyDGV.
	/// </summary>
	public class DataStorage
	{
	    private ClassDxc dxc;
	    public string fileDxc{
	        get{
	            if(dxc!=null && dxc.Info!=null)
	        return dxc.Info.SysName+".txt";
	        else return "";
	    }
	        
	    }
	    
		public DataStorage(ClassDxc dxc)
		{
		    dxc=dxc;
			
		}
		
		public void SaveAlarms()
		 {
		     
		     
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


			return listAlarms;
		}

		/// <summary>
		/// Сохраняет указанные аварии в файл. Если файл существует, то уже записанные аварии объединяются.
		/// </summary>
		/// <param name="alarms">Аварии для сохранения</param>
		/// <param name="file"></param>
		private void WriteAlarmsToFile(IEnumerable<Alarm> alarms, string file)
        {

        }
	}
	
	
}