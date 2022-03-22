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
    public partial class FrmUpdate : Form
    {
        public IActivoServices services { get; set; }
        private int id;
        private Activo Activo;
        public FrmUpdate(int Id)
        {
            InitializeComponent();
            this.id = Id;
        }

        private void FrmUpdate_Load(object sender, EventArgs e)
        {
            
            Activo = services.GetById(id);
            txtNombre.Text = Activo.Nombre;
            txtValor.Text = Activo.Valor.ToString();
            txtValorR.Text = Activo.ValorResidual.ToString();
            txtAU.Text = Activo.VidaUtil.ToString();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtValor.Text) || string.IsNullOrEmpty(txtValorR.Text) || string.IsNullOrEmpty(txtAU.Text))
            {
                MessageBox.Show("Rellene todo el formulario, por favor.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            else
            {
                 
                    Activo activo = new Activo
                    {
                        Nombre = txtNombre.Text,
                        Valor = double.Parse(txtValor.Text),
                        ValorResidual = double.Parse(txtValorR.Text),
                        VidaUtil = int.Parse(txtAU.Text),
                        Id=Activo.Id
                    };
                services.Update(activo);
                this.Dispose();
                }
            
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar))
            {
                e.Handled = true;

                MessageBox.Show("NO SE PUEDEN NUMEROS");

            }
        }

        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;

                MessageBox.Show("NO SE PUEDEN LETRAS");

            }
        }

        private void txtValorR_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;

                MessageBox.Show("NO SE PUEDEN LETRAS");

            }
        }

        private void txtAU_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar))
            {
                e.Handled = true;

                MessageBox.Show("NO SE PUEDEN LETRAS");

            }
        }
    }
}
