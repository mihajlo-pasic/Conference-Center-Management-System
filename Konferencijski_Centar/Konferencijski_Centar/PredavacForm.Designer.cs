namespace Konferencijski_Centar
{
    partial class PredavacForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.textBoxPrezimePredavaca = new System.Windows.Forms.TextBox();
            this.textBoxImePredavaca = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxTemaPredavanjaPredavaca = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(204, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 16);
            this.label2.TabIndex = 11;
            this.label2.Text = "Prezime:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(231, 93);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 16);
            this.label1.TabIndex = 10;
            this.label1.Text = "Ime:";
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(442, 280);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(131, 56);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(278, 280);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(131, 56);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // textBoxPrezimePredavaca
            // 
            this.textBoxPrezimePredavaca.Location = new System.Drawing.Point(278, 143);
            this.textBoxPrezimePredavaca.Name = "textBoxPrezimePredavaca";
            this.textBoxPrezimePredavaca.Size = new System.Drawing.Size(295, 22);
            this.textBoxPrezimePredavaca.TabIndex = 1;
            // 
            // textBoxImePredavaca
            // 
            this.textBoxImePredavaca.Location = new System.Drawing.Point(278, 90);
            this.textBoxImePredavaca.Name = "textBoxImePredavaca";
            this.textBoxImePredavaca.Size = new System.Drawing.Size(295, 22);
            this.textBoxImePredavaca.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(145, 198);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 16);
            this.label3.TabIndex = 13;
            this.label3.Text = "Tema predavanja:";
            // 
            // textBoxTemaPredavanjaPredavaca
            // 
            this.textBoxTemaPredavanjaPredavaca.Location = new System.Drawing.Point(278, 195);
            this.textBoxTemaPredavanjaPredavaca.Multiline = true;
            this.textBoxTemaPredavanjaPredavaca.Name = "textBoxTemaPredavanjaPredavaca";
            this.textBoxTemaPredavanjaPredavaca.Size = new System.Drawing.Size(295, 22);
            this.textBoxTemaPredavanjaPredavaca.TabIndex = 2;
            // 
            // PredavacForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxTemaPredavanjaPredavaca);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.textBoxPrezimePredavaca);
            this.Controls.Add(this.textBoxImePredavaca);
            this.Name = "PredavacForm";
            this.Text = "PredavacForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TextBox textBoxPrezimePredavaca;
        private System.Windows.Forms.TextBox textBoxImePredavaca;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxTemaPredavanjaPredavaca;
    }
}