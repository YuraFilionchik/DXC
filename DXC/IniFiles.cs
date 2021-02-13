using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace DXC
{
   public class IniFile
    {
        string _path; //Имя файла.

        [DllImport("kernel32", CharSet = CharSet.Unicode)] // Подключаем kernel32.dll и описываем его функцию WritePrivateProfilesString
        static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode)] // Еще раз подключаем kernel32.dll, а теперь описываем функцию GetPrivateProfileString
        static extern int GetPrivateProfileString(string section, string key, string @default, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode )] // Еще раз подключаем kernel32.dll, а теперь описываем функцию GetPrivateProfileString
        static extern int GetPrivateProfileString(string section, string key, string @default, IntPtr retVal, int size, string filePath);

     
        public string[]  GetAllKeys(string section)
       {
           IntPtr retVal = Marshal.AllocHGlobal(4096 * sizeof(char));
          // GetPrivateProfileString(Section, null, "", RetVal, 255, Path);
            string t = "";
            List<string> result = new List<string>();
            int n = GetPrivateProfileString( section, null, null, retVal, 4096 * sizeof( char ), _path) - 1;
            if ( n > 0 )
                t = Marshal.PtrToStringUni( retVal, n );
 
            Marshal.FreeHGlobal( retVal );
 
            return t.Split('\0' );
           
       }

        // С помощью конструктора записываем путь до файла и его имя.
        public IniFile(string iniPath)
        {

            _path = new FileInfo(iniPath).FullName.ToString();
        }

        //Читаем ini-файл и возвращаем значение указного ключа из заданной секции.
        public string ReadIni(string section, string key)
        {
            var retVal = new StringBuilder(255);
            GetPrivateProfileString(section, key, "", retVal, 255, _path);
            
                return retVal.ToString();
        }
        //Записываем в ini-файл. Запись происходит в выбранную секцию в выбранный ключ.
        public void Write(string section, string key, string value)
        {
           
            WritePrivateProfileString(section, key, value, _path);
        }

        //Удаляем ключ из выбранной секции.
        public void DeleteKey(string key, string section = null)
        {
            Write(section, key, null);
        }
        //Удаляем выбранную секцию
        public void DeleteSection(string section = null)
        {
            Write(section, null, null);
        }
        //Проверяем, есть ли такой ключ, в этой секции
        public bool KeyExists(string key, string section = null)
        {
            return ReadIni(section, key).Length > 0;
        }
    }
}