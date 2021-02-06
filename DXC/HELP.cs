/*
 * Создано в SharpDevelop.
 * Пользователь: user
 * Дата: 17.12.2020
 * Время: 11:42
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace DXC
{
	/// <summary>
	/// Description of HELP.
	/// </summary>
	public  class HELP
	{
		public ListBox LB1;
		public HELP()
		{
			
		}

	//public static void InvokeLog(string msg)
	 //   {
	
		//MainForm.InvokeLog("",msg);
	        
	   // }
	
	/// <summary>
	/// объединение списка аварий в один с учетом существующих и закрытых аварий
	/// </summary>
	/// <param name="list1"></param>
	/// <param name="list2"></param>
	/// <returns></returns>
	public List<Alarm> MergeAlarms(List<Alarm> list1, List<Alarm> list2)
	{
		try {
			for (int i = 0; i < list1.Count(); i++) {
        		var A=list1[i];
        		//только активные в list1
        	                   if(A.active && list2.Any(x=>x==A))//new alarms contains old Active alarm
        	                  	{ 
        	                  		var NewAlm=list2.First(x=>x==A); //авария в list2, которая есть и в list1
        	                  		if( !NewAlm.active) //Закрываем старую аварию, если новая закрыта(неактивна)
        	                  		{
        	                  			A.End=NewAlm.End;
        	                  			A.active=false; 
        	                  			A.status=NewAlm.status;
        	                  			list1[i]=A;
        	                  			
        	                  		}
        	                  	}
        	                   
        	}
        	//просто добавляем к первому списку отсутсвующие аварии
        	foreach (Alarm alarm in list2) {
						if (list1.All(x => x != alarm))
							list1.Add(alarm);
        	}
        	return list1;
		
				
		} catch (Exception ex) {
			MessageBox.Show(ex.Message,"HELP. MergeAlarms");
			return list1;
		}

	}
}
}
