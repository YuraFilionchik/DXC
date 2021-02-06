/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 05.02.2021
 * Time: 11:26
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace DXC
{
	/// <summary>
	/// Description of Offers.
	/// </summary>
	public partial class Offers : Form
	{
		public string text;
		private const string file="offers.txt";
		public Offers()
		{
			
			InitializeComponent();
			
			if(!File.Exists(file)) text="";
			else text=File.ReadAllText(file);
			richTextBox1.Text=text;
			this.Closing+= new System.ComponentModel.CancelEventHandler(Offers_Closing);
		}
		
		

		void Offers_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		
			text=richTextBox1.Text;
			File.WriteAllText(file,richTextBox1.Text);
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
