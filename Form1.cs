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
        private DesAlgorithm da;
        public Form1()
        {
            InitializeComponent();
            this.autoGenRadio.Checked = true;
            this.keyInputTextBox.Enabled = false;
            this.encryptButton.Enabled = false;
        }

        private void autoGenRadio_CheckedChanged(object sender, EventArgs e)
        {
            this.keyInputTextBox.Enabled = !this.keyInputTextBox.Enabled;
            this.encryptButton.Enabled = false;
        }

        private void encryptButton_Click(object sender, EventArgs e)
        {
            List<byte> sourceData = ConvertStringToByteList(this.sourceDataTextBox.Text);
            List<byte> encrypted = da.Des(sourceData, DesAlgorithm.Operation.encrypt);
            this.encryptedTextBox.Text = ConvertBytesToString(encrypted);
            List<byte> decrypted = da.Des(encrypted, DesAlgorithm.Operation.decrypt);
            this.decryptedTextBox.Text = ConvertBytesToString(decrypted);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(autoGenRadio.Checked)
            {
                this.da = new DesAlgorithm(false);
            }
            else if(manualKeyGenerationRadio.Checked)
            {
                ulong key = 0;
                if (!ulong.TryParse(keyInputTextBox.Text, out key))
                {
                    MessageBox.Show("Hекорректное значение ключа", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                this.da = new DesAlgorithm(true, key);
            }
            MessageBox.Show("Ключи сгенерированы", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.encryptButton.Enabled = true;
        }

        private static List<Byte> ConvertStringToByteList(String sourceString)
        {
            return Encoding.Unicode.GetBytes(sourceString).ToList<Byte>();
        }

        private static String ConvertBytesToString(List<Byte> byteSource)
        {
            return Encoding.Unicode.GetString(byteSource.ToArray());
        }
    }
}
