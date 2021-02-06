/*
 * Created by SharpDevelop.
 * User: Ситал
 * Date: 20.02.2017
 * Time: 16:18
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
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
    

    public class Alarm : IEquatable<Alarm>
	{
    	public string name{get;set;}
		public DateTime Start{get;set;}
		public DateTime End{get;set;}
		public bool active{get;set;}
		public string io{get;set;}
		public string status{get;set;}
		public int bordNumber{get;set;}
		public int portNumber{get;set;}
		private char sep;
       
public Alarm()
{
			name = "";
			Start = new DateTime(1000,1,1);
			End =new DateTime(1000,1,1);
			active = true;
			io = "";
			status = "";
			bordNumber = 0;
			portNumber = 0;
			sep = ';';
}
		public Alarm(bool Active)
		{
			name = "";
			Start = new DateTime(1000,1,1);
			End =new DateTime(1000,1,1);
			active = Active;
			io = "";
			status = "";
			bordNumber = 0;
			portNumber = 0;
			sep = ';';
			
		}
		public Alarm GetCopyAlarm()
		{
			Alarm A=new Alarm();
			
			A.name = name; 
			A.Start = Start;
			A.End =End;
			A.active = active;
			A.io = io;
			A.status = status;
			A.bordNumber = bordNumber;
			A.portNumber = portNumber;
			A.sep = sep;
			
			return A;
			
		}
	    public override string ToString()
	    {
	    	if(End==new DateTime(1000,1,1)) return bordNumber+":"+portNumber+" == "+name+"\t\t\t"+status+
	        	" \t\t"+Start.ToShortDateString()+" "+Start.ToLongTimeString()+" --> Активная";
	    	else  return bordNumber+":"+portNumber+" == "+name+"\t\t\t"+status+
	        	" \t\t"+Start.ToShortDateString()+" "+Start.ToLongTimeString()+" --> "+End.ToShortDateString()+" "+End.ToLongTimeString();
	    }
	    public string ExportLineCSV()
	    {
	    	return Start.ToShortDateString() +" "+ Start.ToLongTimeString()+sep+
	    		End.ToShortDateString()+" "+End.ToLongTimeString()+sep+
	    		bordNumber+sep+
	    		portNumber+sep+
	    		name+sep+
	    		status+sep+
	    		active.ToString();
	    }

	    #region Equals and GetHashCode implementation
		// The code in this region is useful if you want to use this structure in collections.
		// If you don't need it, you can just remove the region and the ": IEquatable<Struct1>" declaration.
		public Alarm ParseLine(string line)
		{
			var words=line.Split(sep);
			if(words.Count()!=7) return this;
			Start=DateTime.Parse(words[0]);
			End=DateTime.Parse  (words[1]);
			bordNumber=int.Parse(words[2]);
			portNumber=int.Parse(words[3]);
			name=				 words[4];
			status=				 words[5];
			active=	  bool.Parse(words[6]);
			return this;
			
		}
		public override bool Equals(object obj)
		{
			if (obj is Alarm)
				return Equals((Alarm)obj); // use Equals method below
			else
				return false;
		}
		
		public bool Equals(Alarm other)
		{
			// add comparisions for all members here
			return (this.Start.Date == other.Start.Date &&
			        this.Start.ToLongTimeString()==other.Start.ToLongTimeString() &&
			        this.name==other.name &&
			        this.bordNumber==other.bordNumber &&
			       this.portNumber==other.portNumber);
		}

		
		public static bool operator ==(Alarm left, Alarm right)
		{
			return left.Equals(right);
		}
		
		public static bool operator !=(Alarm left, Alarm right)
		{
			return !left.Equals(right);
		}
		#endregion
	}
    public struct dxcinfo
    {
        public string sys_name;
        public string nodalclock;
        public string activeDBnuber;
        public Dictionary<string, string> modules;
        public DateTime time;
        public DateTime date;
        public TimeSpan dt; //корректировка времени
        
        public override string ToString()
        {
            string res = "";
            if(modules!=null && modules.Count!=0)
            foreach (KeyValuePair<string, string> module in modules)
            {
                res += "slot." + module.Key + "=" + module.Value + "\n";
            }
            return res;
        }
        public dxcinfo(bool a)
        {
			this.sys_name = "";
			nodalclock="";
			activeDBnuber="";
			modules=new Dictionary<string, string>();
			time=new DateTime(0);
			date=new DateTime(0);
			dt=new TimeSpan(0);
        }
        #region Equals

        /// <summary>
        /// Показывает, равен ли этот экземпляр заданному объекту.
        /// </summary>
        /// <returns>
        /// Значение true, если <paramref name="obj"/> и данный экземпляр относятся к одному типу и представляют одинаковые значения; в противном случае — значение false.
        /// </returns>
        /// <param name="obj">Другой объект, подлежащий сравнению. </param><filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (obj is dxcinfo)
                return Equals((dxcinfo)obj); // use Equals method below
            else
                return false;
        }

        private bool CompairModules(Dictionary<string, string> left, Dictionary<string, string> right)
        {
            if (left.Count != right.Count) return true;
            return left.All(pair => right[pair.Key] == pair.Value);
        }
        public bool Equals(dxcinfo other)
        {
            // add comparisions for all members here
            return (this.sys_name == other.sys_name &&
                this.nodalclock == other.nodalclock &&
                this.activeDBnuber == other.activeDBnuber &&
                CompairModules(this.modules,other.modules)
                );
        }


        public static bool operator ==(dxcinfo left, dxcinfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(dxcinfo left, dxcinfo right)
        {
            return !left.Equals(right);
        }
        #endregion
    }
    

   
    public static class Log
    {
        public static void WriteLog(string from, string msg)
        {
            logging.Writelog.WriteLog(from+"> "+msg);
            
        }


    }

    
}
