using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DXC
{
    public partial class ViewPort : Form
    {
        private Port _port;
        public ViewPort(Port p)
        {
            InitializeComponent();
            this._port = p;
            dataGridView1.Rows.Clear();
            foreach (CrossCon con in _port.Connections.Cons)
            {
                DataGridViewRow row=new DataGridViewRow();
                row.CreateCells(dataGridView1);
                row.Cells[0].Value=con.LTs;
                row.Cells[1].Value="<-->";
                row.Cells[2].Value=con.RBord;
                row.Cells[3].Value=con.RPort;
                row.Cells[4].Value=con.RTs;
                row.Cells[5].Value=con.Status;
                var style=row.DefaultCellStyle;
                if(con.Status.Contains("DATA")) style.BackColor=Color.LightGreen;
                if(con.Status.Contains("NC")) style.BackColor=Color.LightGray;
                #region style
                row.DefaultCellStyle=style;
                row.Height=14;
               // var cellStyle=row.Cells[0].Style;
               // var font=cellStyle.Font;
                //font.=true;
               // cellStyle.Font=font;
                //row.Cells[0].Style=cellStyle;
               // row.Cells[4].Style=cellStyle;
                #endregion
                dataGridView1.Rows.Add(row);
            }
            listBox1.Items.Add("Карта:\t\t"+p.BordNumber);
            listBox1.Items.Add("Порт\t\t"+p.PortNumber);
            listBox1.Items.Add("Направление: "+p.Name);
            this.Text="КАРТА "+p.BordNumber+"  ПОРТ "+p.PortNumber;
        }


        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
