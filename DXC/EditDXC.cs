/*
 * Создано в SharpDevelop.
 * Пользователь: user
 * Дата: 11.12.2020
 * Время: 15:59
 * 
 * Для изменения этого шаблона используйте меню "Инструменты | Параметры | Кодирование | Стандартные заголовки".
 */
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;

namespace DXC
{
	/// <summary>
	/// Description of EditDXC.
	/// </summary>
	public partial class EditDXC : Form
	{
		public List<ClassDXC> listDXC=new List<ClassDXC>();
		ClassDXC SelectedDXC;
		AddDXC AddForm=new AddDXC();
		public EditDXC()
		{
			InitializeComponent();		
			comboBox1.SelectedIndexChanged+= new EventHandler(comboBox1_SelectedIndexChanged);
			dataGridView1.CellEndEdit+= new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
			dataGridView1.DataSource=bindingSource1;
			#region datagridview
			
			#endregion
		}

		//End edit Cell datagridview1
		void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
//			try {
//			int slotN=int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
//			int portN=int.Parse(dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString());
//			switch (e.ColumnIndex) {
//				case 0: //Slot
//					SelectedDXC.Ports.FirstOrDefault(p=>p.BordNumber==slotN && p.PortNumber==portN)
//						.BordNumber=;
//				case 1: //port
//				case 2: //direction
//				case 3: //Alarmed
//			}
//			} catch (Exception ex) {
//				MessageBox.Show(ex.Message);
//			}
			
		}

		//Select DXC in ComboBox1
		void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			string selItem=comboBox1.SelectedItem.ToString();
			if(string.IsNullOrWhiteSpace(selItem)) return;
			SelectedDXC=listDXC.FirstOrDefault(x=>x.ip==selItem);
			bindingSource1.DataSource=SelectedDXC.Ports;
			dataGridView1.DataSource=bindingSource1;
//Порты
//dataGridView1.Rows.Clear();
//foreach (Port port in SelectedDXC.Ports) {
//	dataGridView1.Rows.Add(port.BordNumber,port.PortNumber,port.Name,port.Alarmed);
//	
//}

//Other textboxes
tbName.Text=SelectedDXC.custom_Name;
tbSysName.Text=SelectedDXC.info.sys_name;
tbIP.Text=SelectedDXC.ip;
tbSYNC.Text=SelectedDXC.info.nodalclock;
tbBackup.Text=SelectedDXC.backupPath;
			
		}
		
		public EditDXC(List<ClassDXC> list)
		{
			InitializeComponent();
			SelectedDXC=new ClassDXC("1.1.1.1");
			
			
			this.Load+= new EventHandler(EditDXC_Load);
			comboBox1.SelectedIndexChanged+= new EventHandler(comboBox1_SelectedIndexChanged);
			
			listDXC=list;
			if (!list.Any()) return;
			string[] ips=new string[list.Count];
			for (int i = 0; i < list.Count; i++) {
				ips[i]=listDXC[i].ip;
			}
			comboBox1.Items.AddRange(ips);
			if(ips.Count()>0) comboBox1.SelectedIndex=0;
			
		}

		void EditDXC_Load(object sender, EventArgs e)
		{
			
		
		}
		
		void BtAddDXCClick(object sender, EventArgs e)
		{
			
			DialogResult dr= AddForm.ShowDialog();
			ClassDXC newDxc=new ClassDXC(AddForm.IP);
			newDxc.custom_Name=AddForm.DXCName;
		if(listDXC.Any(x=>x.custom_Name==newDxc.custom_Name)) {
				MessageBox.Show(" Имя "+ newDxc.custom_Name+" уже существует у  IP: "+
				                listDXC.Find(x=>x.custom_Name==newDxc.custom_Name).ip);
				return;
			}
			//check Existing
			if (listDXC.All(x => x.ip != newDxc.ip))
				listDXC.Add(newDxc); else 
				{MessageBox.Show(" IP адресс "+newDxc.ip+" уже существует.");
					return;
				}
			ViewIPinCombo();
		}
		/// <summary>
		/// Отображение списка IP в Combobox1
		/// </summary>
		private void ViewIPinCombo()
		{
			string[] ips=new string[listDXC.Count];
			for (int i = 0; i < listDXC.Count; i++) {
				ips[i]=listDXC[i].ip;
			}
			comboBox1.Items.Clear();
			comboBox1.Items.AddRange(ips);
		}
		//Отмена
		void Button2Click(object sender, EventArgs e)
		{
			Close();
		}
		
		//Сохранить
		void Button1Click(object sender, EventArgs e)
		{
			foreach (ClassDXC dxc in listDXC) {
				dxc.SaveToFile(MainForm.Instance.Cfg);
			}
			DialogResult=DialogResult.OK;
			Close();
		}
		
		//changing DXC custom name
		void TbNameTextChanged(object sender, EventArgs e)
		{
			if(String.IsNullOrWhiteSpace(tbName.Text)) return;
			if(listDXC.All(x=>x.custom_Name!=tbName.Text))
			{
				SelectedDXC.custom_Name=tbName.Text;
				lbInfo.Text="";
			}else if(tbName.Text!=SelectedDXC.custom_Name)
				lbInfo.Text="Такое имя уже занято у "+listDXC.First(x=>x.custom_Name==tbName.Text).info.sys_name;
			
		}
	}
}
