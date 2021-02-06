using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXC
{
public   static class Const
{
    public static string DisplaySystem = "dsp st sys";
        public static string UpdateDB(int N)
    {
        return "upd db " + N.ToString();
    }

    
}


    enum Verbs
    {
        WILL = 251,
        WONT = 252,
        DO = 253,
        DONT = 254,
        IAC = 255
    }
}
