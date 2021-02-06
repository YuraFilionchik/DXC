using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DXC
{
   public class IniFile
    {
        string Path; //Имя файла.

        [DllImport("kernel32", CharSet = CharSet.Unicode)] // Подключаем kernel32.dll и описываем его функцию WritePrivateProfilesString
        static extern long WritePrivateProfileString(string Section, string Key, string Value, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)] // Еще раз подключаем kernel32.dll, а теперь описываем функцию GetPrivateProfileString
        static extern int GetPrivateProfileString(string Section, string Key, string Default, StringBuilder RetVal, int Size, string FilePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode )] // Еще раз подключаем kernel32.dll, а теперь описываем функцию GetPrivateProfileString
        static extern int GetPrivateProfileString(string Section, string Key, string Default, IntPtr RetVal, int Size, string FilePath);

     
        public string[]  GetAllKeys(string Section)
       {
           IntPtr RetVal = Marshal.AllocHGlobal(4096 * sizeof(char));
          // GetPrivateProfileString(Section, null, "", RetVal, 255, Path);
            string t = "";
            List<string> result = new List<string>();
            int n = GetPrivateProfileString( Section, null, null, RetVal, 4096 * sizeof( char ), Path) - 1;
            if ( n > 0 )
                t = Marshal.PtrToStringUni( RetVal, n );
 
            Marshal.FreeHGlobal( RetVal );
 
            return t.Split('\0' );
           
       }

        // С помощью конструктора записываем путь до файла и его имя.
        public IniFile(string IniPath)
        {

            Path = new FileInfo(IniPath).FullName.ToString();
        }

        //Читаем ini-файл и возвращаем значение указного ключа из заданной секции.
        public string ReadINI(string Section, string Key)
        {
            var RetVal = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", RetVal, 255, Path);
            
                return RetVal.ToString();
        }
        //Записываем в ini-файл. Запись происходит в выбранную секцию в выбранный ключ.
        public void Write(string Section, string Key, string Value)
        {
           
            WritePrivateProfileString(Section, Key, Value, Path);
        }

        //Удаляем ключ из выбранной секции.
        public void DeleteKey(string Key, string Section = null)
        {
            Write(Section, Key, null);
        }
        //Удаляем выбранную секцию
        public void DeleteSection(string Section = null)
        {
            Write(Section, null, null);
        }
        //Проверяем, есть ли такой ключ, в этой секции
        public bool KeyExists(string Key, string Section = null)
        {
            return ReadINI(Section, Key).Length > 0;
        }
    }
}