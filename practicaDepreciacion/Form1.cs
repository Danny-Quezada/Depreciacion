using AppCore.IServices;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace practicaDepreciacion
{
    public partial class Form1 : Form
    {
        IActivoServices activoServices;
        private int Seleccionado = -1;
        public Form1(IActivoServices ActivoServices)
        {
            this.activoServices = ActivoServices;
            InitializeComponent();
        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void dgvActivos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                Seleccionado = int.Parse(dgvActivos.Rows[e.RowIndex].Cells[0].Value.ToString());
                MessageBox.Show(Seleccionado.ToString());
            }
        }

        private void actualizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Seleccionado != -1)
            {
                
            }
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            dgvActivos.Rows.Add(txtNombre.Text, txtValor.Text);
            dgvActivos.Update();
     
            
        }
    }
}
