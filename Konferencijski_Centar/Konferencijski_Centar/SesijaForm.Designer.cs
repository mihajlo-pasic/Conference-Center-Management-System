namespace Konferencijski_Centar
{
    partial class SesijaForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxOpis = new System.Windows.Forms.TextBox();
            this.textBoxNaziv = new System.Windows.Forms.TextBox();
            this.comboBoxDogadjaji = new System.Windows.Forms.ComboBox();
            this.comboBoxProstorije = new System.Windows.Forms.ComboBox();
            this.comboBoxVremePocetka = new System.Windows.Forms.ComboBox();
            this.comboBoxVremeZavrsetka = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxDatum = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(211, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 16);
            this.label3.TabIndex = 42;
            this.label3.Text = "Datum:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(222, 148);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 16);
            this.label2.TabIndex = 41;
            this.label2.Text = "Opis:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(177, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 16);
            this.label1.TabIndex = 40;
            this.label1.Text = "Naziv sesije:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(443, 382);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(131, 56);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(279, 382);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(131, 56);
            this.buttonOK.TabIndex = 7;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxOpis
            // 
            this.textBoxOpis.Location = new System.Drawing.Point(279, 147);
            this.textBoxOpis.Name = "textBoxOpis";
            this.textBoxOpis.Size = new System.Drawing.Size(295, 22);
            this.textBoxOpis.TabIndex = 2;
            // 
            // textBoxNaziv
            // 
            this.textBoxNaziv.Location = new System.Drawing.Point(279, 91);
            this.textBoxNaziv.Name = "textBoxNaziv";
            this.textBoxNaziv.Size = new System.Drawing.Size(295, 22);
            this.textBoxNaziv.TabIndex = 1;
            // 
            // comboBoxDogadjaji
            // 
            this.comboBoxDogadjaji.FormattingEnabled = true;
            this.comboBoxDogadjaji.Location = new System.Drawing.Point(279, 33);
            this.comboBoxDogadjaji.Name = "comboBoxDogadjaji";
            this.comboBoxDogadjaji.Size = new System.Drawing.Size(295, 24);
            this.comboBoxDogadjaji.TabIndex = 0;
            this.comboBoxDogadjaji.SelectedIndexChanged += new System.EventHandler(this.comboBoxDogadjaji_SelectedIndexChanged);
            // 
            // comboBoxProstorije
            // 
            this.comboBoxProstorije.FormattingEnabled = true;
            this.comboBoxProstorije.Location = new System.Drawing.Point(279, 243);
            this.comboBoxProstorije.Name = "comboBoxProstorije";
            this.comboBoxProstorije.Size = new System.Drawing.Size(295, 24);
            this.comboBoxProstorije.TabIndex = 4;
            this.comboBoxProstorije.SelectedIndexChanged += new System.EventHandler(this.comboBoxProstorije_SelectedIndexChanged);
            // 
            // comboBoxVremePocetka
            // 
            this.comboBoxVremePocetka.FormattingEnabled = true;
            this.comboBoxVremePocetka.Location = new System.Drawing.Point(279, 293);
            this.comboBoxVremePocetka.Name = "comboBoxVremePocetka";
            this.comboBoxVremePocetka.Size = new System.Drawing.Size(295, 24);
            this.comboBoxVremePocetka.TabIndex = 5;
            this.comboBoxVremePocetka.SelectedIndexChanged += new System.EventHandler(this.comboBoxVremePocetka_SelectedIndexChanged);
            // 
            // comboBoxVremeZavrsetka
            // 
            this.comboBoxVremeZavrsetka.FormattingEnabled = true;
            this.comboBoxVremeZavrsetka.Location = new System.Drawing.Point(279, 342);
            this.comboBoxVremeZavrsetka.Name = "comboBoxVremeZavrsetka";
            this.comboBoxVremeZavrsetka.Size = new System.Drawing.Size(295, 24);
            this.comboBoxVremeZavrsetka.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(180, 37);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(80, 16);
            this.label4.TabIndex = 49;
            this.label4.Text = "Za događaj:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(152, 296);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(108, 16);
            this.label5.TabIndex = 50;
            this.label5.Text = "Vrijeme početka:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(143, 346);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 16);
            this.label6.TabIndex = 51;
            this.label6.Text = "Vrijeme završetka:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(193, 247);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(67, 16);
            this.label7.TabIndex = 52;
            this.label7.Text = "Prostorija:";
            // 
            // comboBoxDatum
            // 
            this.comboBoxDatum.FormattingEnabled = true;
            this.comboBoxDatum.Location = new System.Drawing.Point(279, 194);
            this.comboBoxDatum.Name = "comboBoxDatum";
            this.comboBoxDatum.Size = new System.Drawing.Size(295, 24);
            this.comboBoxDatum.TabIndex = 3;
            // 
            // SesijaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 484);
            this.Controls.Add(this.comboBoxDatum);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBoxVremeZavrsetka);
            this.Controls.Add(this.comboBoxVremePocetka);
            this.Controls.Add(this.comboBoxProstorije);
            this.Controls.Add(this.comboBoxDogadjaji);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxOpis);
            this.Controls.Add(this.textBoxNaziv);
            this.Name = "SesijaForm";
            this.Text = "SesijaForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox textBoxOpis;
        private System.Windows.Forms.TextBox textBoxNaziv;
        private System.Windows.Forms.ComboBox comboBoxDogadjaji;
        private System.Windows.Forms.ComboBox comboBoxProstorije;
        private System.Windows.Forms.ComboBox comboBoxVremePocetka;
        private System.Windows.Forms.ComboBox comboBoxVremeZavrsetka;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxDatum;
    }
}