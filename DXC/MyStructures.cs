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
    	public string Name{get;set;}
		public DateTime Start{get;set;}
		public DateTime End{get;set;}
		public bool Active{get;set;}
		public string Io{get;set;}
		public string Status{get;set;}
		public int BordNumber{get;set;}
		public int PortNumber{get;set;}
		private char _sep;
       
public Alarm()
{
			Name = "";
			Start = new DateTime(1000,1,1);
			End =new DateTime(1000,1,1);
			Active = true;
			Io = "";
			Status = "";
			BordNumber = 0;
			PortNumber = 0;
			_sep = ';';
}
		public Alarm(bool active)
		{
			Name = "";
			Start = new DateTime(1000,1,1);
			End =new DateTime(1000,1,1);
			this.Active = active;
			Io = "";
			Status = "";
			BordNumber = 0;
			PortNumber = 0;
			_sep = ';';
			
		}
		public Alarm GetCopyAlarm()
		{
			Alarm a=new Alarm();
			
			a.Name = Name; 
			a.Start = Start;
			a.End =End;
			a.Active = Active;
			a.Io = Io;
			a.Status = Status;
			a.BordNumber = BordNumber;
			a.PortNumber = PortNumber;
			a._sep = _sep;
			
			return a;
			
		}
	    public override string ToString()
	    {
	    	if(End==new DateTime(1000,1,1)) return BordNumber+":"+PortNumber+" == "+Name+"\t\t\t"+Status+
	        	" \t\t"+Start.ToShortDateString()+" "+Start.ToLongTimeString()+" --> Активная";
	    	else  return BordNumber+":"+PortNumber+" == "+Name+"\t\t\t"+Status+
	        	" \t\t"+Start.ToShortDateString()+" "+Start.ToLongTimeString()+" --> "+End.ToShortDateString()+" "+End.ToLongTimeString();
	    }
	    public string ExportLineCsv()
	    {
	    	return Start.ToShortDateString() +" "+ Start.ToLongTimeString()+_sep+
	    		End.ToShortDateString()+" "+End.ToLongTimeString()+_sep+
	    		BordNumber+_sep+
	    		PortNumber+_sep+
	    		Name+_sep+
	    		Status+_sep+
	    		Active.ToString();
	    }

	    #region Equals and GetHashCode implementation
		// The code in this region is useful if you want to use this structure in collections.
		// If you don't need it, you can just remove the region and the ": IEquatable<Struct1>" declaration.
		public Alarm ParseLine(string line)
		{
			var words=line.Split(_sep);
			if(words.Count()!=7) return this;
			Start=DateTime.Parse(words[0]);
			End=DateTime.Parse  (words[1]);
			BordNumber=int.Parse(words[2]);
			PortNumber=int.Parse(words[3]);
			Name=				 words[4];
			Status=				 words[5];
			Active=	  bool.Parse(words[6]);
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
			        this.Name==other.Name &&
			        this.BordNumber==other.BordNumber &&
			       this.PortNumber==other.PortNumber);
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
    public struct Dxcinfo
    {
        public string SysName;
        public string Nodalclock;
        public string ActiveDBnuber;
        public Dictionary<string, string> Modules;
        public DateTime Time;
        public DateTime Date;
        public TimeSpan Dt; //корректировка времени
        
        public override string ToString()
        {
            string res = "";
            if(Modules!=null && Modules.Count!=0)
            foreach (KeyValuePair<string, string> module in Modules)
            {
                res += "slot." + module.Key + "=" + module.Value + "\n";
            }
            return res;
        }
        public Dxcinfo(bool a)
        {
			this.SysName = "";
			Nodalclock="";
			ActiveDBnuber="";
			Modules=new Dictionary<string, string>();
			Time=new DateTime(0);
			Date=new DateTime(0);
			Dt=new TimeSpan(0);
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
            if (obj is Dxcinfo)
                return Equals((Dxcinfo)obj); // use Equals method below
            else
                return false;
        }

        private bool CompairModules(Dictionary<string, string> left, Dictionary<string, string> right)
        {
            if (left.Count != right.Count) return true;
            return left.All(pair => right[pair.Key] == pair.Value);
        }
        public bool Equals(Dxcinfo other)
        {
            // add comparisions for all members here
            return (this.SysName == other.SysName &&
                this.Nodalclock == other.Nodalclock &&
                this.ActiveDBnuber == other.ActiveDBnuber &&
                CompairModules(this.Modules,other.Modules)
                );
        }


        public static bool operator ==(Dxcinfo left, Dxcinfo right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Dxcinfo left, Dxcinfo right)
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
