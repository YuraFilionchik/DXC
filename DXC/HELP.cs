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
using System.Media;
using System.Threading;

namespace DXC
{
	/// <summary>
	/// Description of HELP.
	/// </summary>
	public  class Help
	{
		public ListBox Lb1;
        public static bool SoundsOn = true;
        private static string _alarmFileMajor = "sounds/notify.wav"; 
        private static string _alarmFileMinor = "sounds/chimes.wav";
        private static string _notifySoundFile = "sound/notify.wav";
        private static string _closeAppFile = "sounds/close.wav";
        private static string _openAppFile = "sounds/open.wav";
        private static string _backupOkFile = "sounds/Speech Sleep.wav";
        private static string _clickFile = "sounds/Click-80.wav";
        private static string _clickFile2 = "sounds/Click.mp3";
        private static string _deniedFile1 = "sounds/denied1.wav";
        private static string _deniedFile2 = "sounds/denied2.wav";
        private static string _errorFile1 = "sounds/error1.wav";
        private static string _runFile = "sounds/Open.wav";
        private static string _stopFile = "sounds/Stop.wav";



		public Help()
		{
			
		}

        //public static void InvokeLog(string msg)
        //   {

        //MainForm.InvokeLog("",msg);

        // }
//TODO Add selecting Sounds menu
        #region Sounds
        private static void PlaySound(string file)
        { if(!SoundsOn) return;
            try
            {//TODO use mediaplayer to change volume
                System.Media.SoundPlayer sp = new SoundPlayer(file);
                sp.Play();
            }
            catch (Exception e)
            {
                Log.WriteLog("PlaySound", e.Message);
            }
        }

        public static void BeepRun()
        { PlaySound(_runFile);}
        public static void BeepStop()
        { PlaySound(_stopFile); }
        public static void BeepError()
        {
            PlaySound(_errorFile1);
        }
        public static void BeepDenied()
        {
            if(DateTime.Now.Millisecond%2==0)PlaySound(_deniedFile1);
            else PlaySound(_deniedFile2);
        }
        public static void BeepClick()
        { 
        	PlaySound(_clickFile);}

        public static void BeepClose()
        {
            PlaySound(_closeAppFile);
        }
        public static void BeepOpen()
        {// PlaySound(OpenAppFile);
        }
        public static void BeepBackupOk()
        { PlaySound(_backupOkFile);}
        public static void BeepNotify()
        {
            new Thread(() =>
            {
                PlaySound(_notifySoundFile);
            }).Start();

        }
        public static void BeepAlarmMajor()
        {
        	System.Console.Beep();
//            new Thread(() =>
//            {
//                PlaySound(AlarmFileMajor);
//            }).Start();

        }
        public static void BeepAlarmLong(int repeats)
        {
            new Thread(() =>
            {
                for (int i = 0; i < repeats; i++)
                {
                    // System.Console.Beep(600,200);

                    //for (int j = 0; j < 6; j++)
                    //{
                    //    System.Console.Beep(1000-j*100,50);
                    //}
                }

            }).Start();

        }
        public static void BeepAlarmMinor()
        {
            new Thread(() =>
            {
                PlaySound(_alarmFileMinor);

            }).Start();

        }


        #endregion



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
        		var a=list1[i];
        		//только активные в list1
        	                   if(a.Active && list2.Any(x=>x==a))//new alarms contains old Active alarm
        	                  	{ 
        	                  		var newAlm=list2.First(x=>x==a); //авария в list2, которая есть и в list1
        	                  		if( !newAlm.Active) //Закрываем старую аварию, если новая закрыта(неактивна)
        	                  		{
        	                  			a.End=newAlm.End;
        	                  			a.Active=false; 
        	                  			a.Status=newAlm.Status;
        	                  			list1[i]=a;
        	                  			
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
