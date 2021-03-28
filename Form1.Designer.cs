namespace des_algorithm
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.encryptButton = new System.Windows.Forms.Button();
            this.autoGenRadio = new System.Windows.Forms.RadioButton();
            this.manualKeyGenerationRadio = new System.Windows.Forms.RadioButton();
            this.keyInputTextBox = new System.Windows.Forms.TextBox();
            this.sourceDataTextBox = new System.Windows.Forms.RichTextBox();
            this.encryptedTextBox = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.decryptedTextBox = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // encryptButton
            // 
            this.encryptButton.Location = new System.Drawing.Point(383, 158);
            this.encryptButton.Name = "encryptButton";
            this.encryptButton.Size = new System.Drawing.Size(97, 23);
            this.encryptButton.TabIndex = 0;
            this.encryptButton.Text = "Зашифровать";
            this.encryptButton.UseVisualStyleBackColor = true;
            this.encryptButton.Click += new System.EventHandler(this.encryptButton_Click);
            // 
            // autoGenRadio
            // 
            this.autoGenRadio.AutoSize = true;
            this.autoGenRadio.Checked = true;
            this.autoGenRadio.Location = new System.Drawing.Point(14, 13);
            this.autoGenRadio.Name = "autoGenRadio";
            this.autoGenRadio.Size = new System.Drawing.Size(102, 17);
            this.autoGenRadio.TabIndex = 1;
            this.autoGenRadio.TabStop = true;
            this.autoGenRadio.Text = "Автогенерация";
            this.autoGenRadio.UseVisualStyleBackColor = true;
            this.autoGenRadio.CheckedChanged += new System.EventHandler(this.autoGenRadio_CheckedChanged);
            // 
            // manualKeyGenerationRadio
            // 
            this.manualKeyGenerationRadio.AutoSize = true;
            this.manualKeyGenerationRadio.Location = new System.Drawing.Point(14, 37);
            this.manualKeyGenerationRadio.Name = "manualKeyGenerationRadio";
            this.manualKeyGenerationRadio.Size = new System.Drawing.Size(67, 17);
            this.manualKeyGenerationRadio.TabIndex = 2;
            this.manualKeyGenerationRadio.Text = "Вручную";
            this.manualKeyGenerationRadio.UseVisualStyleBackColor = true;
            // 
            // keyInputTextBox
            // 
            this.keyInputTextBox.Location = new System.Drawing.Point(124, 37);
            this.keyInputTextBox.Name = "keyInputTextBox";
            this.keyInputTextBox.Size = new System.Drawing.Size(356, 20);
            this.keyInputTextBox.TabIndex = 3;
            // 
            // sourceDataTextBox
            // 
            this.sourceDataTextBox.Location = new System.Drawing.Point(12, 107);
            this.sourceDataTextBox.Name = "sourceDataTextBox";
            this.sourceDataTextBox.Size = new System.Drawing.Size(468, 45);
            this.sourceDataTextBox.TabIndex = 4;
            this.sourceDataTextBox.Text = "";
            // 
            // encryptedTextBox
            // 
            this.encryptedTextBox.Location = new System.Drawing.Point(12, 200);
            this.encryptedTextBox.Name = "encryptedTextBox";
            this.encryptedTextBox.Size = new System.Drawing.Size(468, 48);
            this.encryptedTextBox.TabIndex = 5;
            this.encryptedTextBox.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(99, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Исходные данные";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 183);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Зашифрованное значение";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(367, 63);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(113, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Генерация ключей";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 254);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Расшифрованное значение";
            // 
            // decryptedTextBox
            // 
            this.decryptedTextBox.Location = new System.Drawing.Point(12, 271);
            this.decryptedTextBox.Name = "decryptedTextBox";
            this.decryptedTextBox.Size = new System.Drawing.Size(468, 48);
            this.decryptedTextBox.TabIndex = 10;
            this.decryptedTextBox.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 325);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.decryptedTextBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.encryptedTextBox);
            this.Controls.Add(this.sourceDataTextBox);
            this.Controls.Add(this.keyInputTextBox);
            this.Controls.Add(this.manualKeyGenerationRadio);
            this.Controls.Add(this.autoGenRadio);
            this.Controls.Add(this.encryptButton);
            this.Name = "Form1";
            this.Text = "RSA";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button encryptButton;
        private System.Windows.Forms.RadioButton autoGenRadio;
        private System.Windows.Forms.RadioButton manualKeyGenerationRadio;
        private System.Windows.Forms.TextBox keyInputTextBox;
        private System.Windows.Forms.RichTextBox sourceDataTextBox;
        private System.Windows.Forms.RichTextBox encryptedTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RichTextBox decryptedTextBox;
    }
}

