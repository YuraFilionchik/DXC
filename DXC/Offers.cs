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
		public string Content;
		private const string File="offers.txt";
		public Offers()
		{
			
			InitializeComponent();
			
			if(!System.IO.File.Exists(File)) Content="";
			else Content=System.IO.File.ReadAllText(File);
			richTextBox1.Text=Content;
			this.Closing+= new System.ComponentModel.CancelEventHandler(Offers_Closing);
		}
		
		

		void Offers_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
		
			Content=richTextBox1.Text;
			System.IO.File.WriteAllText(File,richTextBox1.Text);
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
