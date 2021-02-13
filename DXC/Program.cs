/*
 * Created by SharpDevelop.
 * User: Ситал
 * Date: 20.02.2017
 * Time: 15:56
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Windows.Forms;
[assembly: System.CLSCompliant(true)]
namespace DXC
{

    /// <summary>
    /// Class with program entry point.
    /// </summary>
    internal sealed class Program
    {
       // public static MainForm MF;
       public static Help   Helper =new Help();
        
        /// <summary>
        /// Program entry point.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
               //MainForm MF = new MainForm();
               MainForm.Instance.ShowDialog();
                //Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Main");
                Log.WriteLog("MAIN",ex.Message);
            }

        }

    }

}
