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
	public partial class EditDxc : Form
	{
		public List<ClassDxc> ListDxc=new List<ClassDxc>();
		ClassDxc _selectedDxc;
		AddDxc _addForm=new AddDxc();
		public EditDxc()
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
			_selectedDxc=ListDxc.FirstOrDefault(x=>x.Ip==selItem);
			bindingSource1.DataSource=_selectedDxc.Ports;
			dataGridView1.DataSource=bindingSource1;
//Порты
//dataGridView1.Rows.Clear();
//foreach (Port port in SelectedDXC.Ports) {
//	dataGridView1.Rows.Add(port.BordNumber,port.PortNumber,port.Name,port.Alarmed);
//	
//}

//Other textboxes
tbName.Text=_selectedDxc.CustomName;
tbSysName.Text=_selectedDxc.Info.SysName;
tbIP.Text=_selectedDxc.Ip;
tbSYNC.Text=_selectedDxc.Info.Nodalclock;
tbBackup.Text=_selectedDxc.BackupPath;
			
		}
		
		public EditDxc(List<ClassDxc> list)
		{
			InitializeComponent();
			_selectedDxc=new ClassDxc("1.1.1.1");
			
			
			this.Load+= new EventHandler(EditDXC_Load);
			comboBox1.SelectedIndexChanged+= new EventHandler(comboBox1_SelectedIndexChanged);
			
			ListDxc=list;
			if (!list.Any()) return;
			string[] ips=new string[list.Count];
			for (int i = 0; i < list.Count; i++) {
				ips[i]=ListDxc[i].Ip;
			}
			comboBox1.Items.AddRange(ips);
			if(ips.Count()>0) comboBox1.SelectedIndex=0;
			
		}

		void EditDXC_Load(object sender, EventArgs e)
		{
			
		
		}
		
		void BtAddDxcClick(object sender, EventArgs e)
		{
			
			DialogResult dr= _addForm.ShowDialog();
			if(dr!=DialogResult.OK) return;
			ClassDxc newDxc=new ClassDxc(_addForm.Ip);
			newDxc.CustomName=_addForm.DxcName;
		if(ListDxc.Any(x=>x.CustomName==newDxc.CustomName)) {
				MessageBox.Show(" Имя "+ newDxc.CustomName+" уже существует у  IP: "+
				                ListDxc.Find(x=>x.CustomName==newDxc.CustomName).Ip);
				return;
			}
			//check Existing
			if (ListDxc.All(x => x.Ip != newDxc.Ip))
				ListDxc.Add(newDxc); else 
				{MessageBox.Show(" IP адресс "+newDxc.Ip+" уже существует.");
					return;
				}
			ViewIPinCombo();
		}
		/// <summary>
		/// Отображение списка IP в Combobox1
		/// </summary>
		private void ViewIPinCombo()
		{
			string[] ips=new string[ListDxc.Count];
			for (int i = 0; i < ListDxc.Count; i++) {
				ips[i]=ListDxc[i].Ip;
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
			foreach (ClassDxc dxc in ListDxc) {
				dxc.SaveToFile(MainForm.Instance.Cfg);
			}
			DialogResult=DialogResult.OK;
			Close();
		}
		
		//changing DXC custom name
		void TbNameTextChanged(object sender, EventArgs e)
		{
			if(String.IsNullOrWhiteSpace(tbName.Text)) return;
			if(ListDxc.All(x=>x.CustomName!=tbName.Text))
			{
				_selectedDxc.CustomName=tbName.Text;
				lbInfo.Text="";
			}else if(tbName.Text!=_selectedDxc.CustomName)
				lbInfo.Text="Такое имя уже занято у "+ListDxc.First(x=>x.CustomName==tbName.Text).Info.SysName;
			
		}
	}
}
