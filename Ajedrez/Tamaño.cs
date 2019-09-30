using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ajedrez
{
    public partial class Tamaño : Form
    {
        frmCaballos ch;
        public Tamaño(frmCaballos c)
        {
            ch = c;
            InitializeComponent();
        }
        private void numericUpDown1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                ch.n = (int)numericUpDown1.Value;
                this.Close();
            }
            if (e.KeyChar == 27)
                this.Close();
        }
    }
}
