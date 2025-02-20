namespace Konferencijski_Centar
{
    partial class Prikaz
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
            this.dataGridViewPrikaz = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPrikaz)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewPrikaz
            // 
            this.dataGridViewPrikaz.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPrikaz.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPrikaz.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewPrikaz.Name = "dataGridViewPrikaz";
            this.dataGridViewPrikaz.RowHeadersWidth = 51;
            this.dataGridViewPrikaz.RowTemplate.Height = 24;
            this.dataGridViewPrikaz.Size = new System.Drawing.Size(741, 411);
            this.dataGridViewPrikaz.TabIndex = 1;
            // 
            // Prikaz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(765, 435);
            this.Controls.Add(this.dataGridViewPrikaz);
            this.Name = "Prikaz";
            this.Text = "Prikaz";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPrikaz)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewPrikaz;
    }
}