using AppCore.IServices;
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
        public FrmUpdate(int Id)
        {
            InitializeComponent();
            this.id = Id;
        }

        private void FrmUpdate_Load(object sender, EventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

  
    }
}
