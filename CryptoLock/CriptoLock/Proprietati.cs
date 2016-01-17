using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CriptoLock
{
    public partial class Proprietati : Form
    {
        public Proprietati()
        {
            InitializeComponent();
            textBox1.Text = Properties.Settings.Default.username;
        }

        private void Proprietati_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.username = textBox1.Text;
            textBox1.Text = Properties.Settings.Default.username;
        }
    }
}
