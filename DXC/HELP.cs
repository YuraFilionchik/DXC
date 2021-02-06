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
	public  class HELP
	{
		public ListBox LB1;
        public static bool SoundsOn = true;
        private static string AlarmFileMajor = "sounds/chord.wav"; 
        private static string AlarmFileMinor = "sounds/chimes.wav";
        private static string NotifySoundFile = "sound/notify.wav";
        private static string CloseAppFile = "sounds/close.wav";
        private static string OpenAppFile = "sounds/open.wav";
        private static string BackupOKFile = "sounds/Speech Sleep.wav";
        private static string ClickFile = "sounds/Click.wav";
        private static string DeniedFile1 = "sounds/denied1.wav";
        private static string DeniedFile2 = "sounds/denied2.wav";
        private static string ErrorFile1 = "sounds/error1.wav";
        private static string RunFile = "sounds/Run.wav";
        private static string StopFile = "sounds/Stop.wav";



		public HELP()
		{
			
		}

        //public static void InvokeLog(string msg)
        //   {

        //MainForm.InvokeLog("",msg);

        // }

        #region Sounds
        private static void PlaySound(string file)
        { if(!SoundsOn) return;
            try
            {
                System.Media.SoundPlayer sp = new SoundPlayer(file);
                sp.Play();
            }
            catch (Exception e)
            {
                Log.WriteLog("PlaySound", e.Message);
            }
        }

        public static void BeepRun()
        { PlaySound(RunFile);}
        public static void BeepStop()
        { PlaySound(StopFile); }
        public static void BeepError()
        {
            PlaySound(ErrorFile1);
        }
        public static void BeepDenied()
        {
            if(DateTime.Now.Millisecond%2==0)PlaySound(DeniedFile1);
            else PlaySound(DeniedFile2);
        }
        public static void BeepClick()
        { PlaySound(ClickFile);}

        public static void BeepClose()
        {
            PlaySound(CloseAppFile);
        }
        public static void BeepOpen()
        { PlaySound(OpenAppFile);}
        public static void BeepBackupOK()
        { PlaySound(BackupOKFile);}
        public static void BeepNotify()
        {
            new Thread(() =>
            {
                PlaySound(NotifySoundFile);
            }).Start();

        }
        public static void BeepAlarmMajor()
        {
            new Thread(() =>
            {
                PlaySound(AlarmFileMajor);
            }).Start();

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
                PlaySound(AlarmFileMinor);

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
