﻿using Domain.Entities;
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
    public partial class FrmEmpleado : Form
    {
        public FrmEmpleado()
        {
            InitializeComponent();
        }

        private void Guna2ImageButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Guna2ImageButton4_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void FrmEmpleado_Load(object sender, EventArgs e)
        {
            this.cmbEstado.Items.AddRange(Enum.GetValues(typeof(Activo)).Cast<object>().ToArray());
        }

        private void BtnEnviar_Click(object sender, EventArgs e)
        {

        }

        private void Guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Guna2TextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = true;

                MessageBox.Show("NO SE PUEDEN NUMEROS");

            }
        }

        private void Guna2TextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = true;

                MessageBox.Show("NO SE PUEDEN NUMEROS");

            }
        }
    }
}
