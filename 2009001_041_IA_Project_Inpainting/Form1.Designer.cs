namespace _2009001_041_IA_Project_Inpainting
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.bnt_grayscale = new System.Windows.Forms.Button();
            this.btn_Colouredimages = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkRed;
            this.label1.Location = new System.Drawing.Point(285, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(214, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Image Inpainting";
            // 
            // bnt_grayscale
            // 
            this.bnt_grayscale.Location = new System.Drawing.Point(322, 125);
            this.bnt_grayscale.Name = "bnt_grayscale";
            this.bnt_grayscale.Size = new System.Drawing.Size(136, 23);
            this.bnt_grayscale.TabIndex = 1;
            this.bnt_grayscale.Text = "Gray Scale Images";
            this.bnt_grayscale.UseVisualStyleBackColor = true;
            this.bnt_grayscale.Click += new System.EventHandler(this.bnt_grayscale_Click);
            // 
            // btn_Colouredimages
            // 
            this.btn_Colouredimages.Location = new System.Drawing.Point(322, 220);
            this.btn_Colouredimages.Name = "btn_Colouredimages";
            this.btn_Colouredimages.Size = new System.Drawing.Size(136, 23);
            this.btn_Colouredimages.TabIndex = 2;
            this.btn_Colouredimages.Text = "Coloured Images";
            this.btn_Colouredimages.UseVisualStyleBackColor = true;
            this.btn_Colouredimages.Click += new System.EventHandler(this.btn_Colouredimages_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkBlue;
            this.label2.Location = new System.Drawing.Point(689, 371);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 38);
            this.label2.TabIndex = 3;
            this.label2.Text = "Aadish Gupta\r\nSanchit Garg\r\n";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(322, 308);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(136, 43);
            this.button1.TabIndex = 4;
            this.button1.Text = "Inpainting using Fast Marching method";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(845, 455);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_Colouredimages);
            this.Controls.Add(this.bnt_grayscale);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button bnt_grayscale;
        private System.Windows.Forms.Button btn_Colouredimages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}

