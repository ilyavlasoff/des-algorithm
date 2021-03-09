using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace des_algorithm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            this.autoGenRadio.Checked = true;
            this.keyInputTextBox.Enabled = false;
        }

        private void autoGenRadio_CheckedChanged(object sender, EventArgs e)
        {
            this.keyInputTextBox.Enabled = !this.keyInputTextBox.Enabled;
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            DesAlgorithm da = new DesAlgorithm();
            da.Encrypt(this.sourceDataTextBox.Text);
        }
    }
}
