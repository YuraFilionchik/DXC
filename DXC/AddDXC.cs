/*
 * Created by SharpDevelop.
 * User: Administrator
 * Date: 16.12.2020
 * Time: 15:01
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DXC
{
	/// <summary>
	/// Description of AddDXC.
	/// </summary>
	public partial class AddDXC : Form
	{
		public string IP="";
		public string DXCName="";
		public AddDXC()
		{

			InitializeComponent();
			this.Shown+= delegate { textBox1.Text=IP; textBox2.Text=DXCName; };
			
		}
		
		void BtOKClick(object sender, EventArgs e)
		{
			if(!MainForm.IPformat(textBox1.Text)) {
				MessageBox.Show("Неверный формат IP адреса");
				return;
			}
			if(String.IsNullOrWhiteSpace(textBox2.Text)) {
				MessageBox.Show("Введите какое-нибудь имя для DXC");
				return;
			}
			IP=textBox1.Text.Trim();
			DXCName=textBox2.Text.Trim();
		this.DialogResult= DialogResult.OK;
		Close();
		}
		
		void BtCancelClick(object sender, EventArgs e)
		{
			Close();
		}
	}
}
