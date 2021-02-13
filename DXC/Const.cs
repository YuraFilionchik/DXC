using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DXC
{
public   static class Const
{
    public static string DisplaySystem = "dsp st sys";
        public static string UpdateDb(int n)
    {
        return "upd db " + n.ToString();
    }

    
}


    enum Verbs
    {
        Will = 251,
        Wont = 252,
        Do = 253,
        Dont = 254,
        Iac = 255
    }
}
