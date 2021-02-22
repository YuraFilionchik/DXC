/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 01.02.2021
 * Time: 14:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
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
	}
	
	
}
