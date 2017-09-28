namespace HW13_Alexander_Lao
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radiusUpDown = new System.Windows.Forms.NumericUpDown();
            this.gravityRadioButton = new System.Windows.Forms.RadioButton();
            this.planetRadioButton = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.radiusUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Simulation Parameters";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(208, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Center of gravity radius (pixels):";
            // 
            // radiusUpDown
            // 
            this.radiusUpDown.Location = new System.Drawing.Point(243, 46);
            this.radiusUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.radiusUpDown.Name = "radiusUpDown";
            this.radiusUpDown.Size = new System.Drawing.Size(416, 22);
            this.radiusUpDown.TabIndex = 2;
            this.radiusUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // gravityRadioButton
            // 
            this.gravityRadioButton.AutoSize = true;
            this.gravityRadioButton.Location = new System.Drawing.Point(32, 74);
            this.gravityRadioButton.Name = "gravityRadioButton";
            this.gravityRadioButton.Size = new System.Drawing.Size(224, 21);
            this.gravityRadioButton.TabIndex = 3;
            this.gravityRadioButton.TabStop = true;
            this.gravityRadioButton.Text = "Click to create center of gravity";
            this.gravityRadioButton.UseVisualStyleBackColor = true;
            // 
            // planetRadioButton
            // 
            this.planetRadioButton.AutoSize = true;
            this.planetRadioButton.Location = new System.Drawing.Point(32, 101);
            this.planetRadioButton.Name = "planetRadioButton";
            this.planetRadioButton.Size = new System.Drawing.Size(161, 21);
            this.planetRadioButton.TabIndex = 4;
            this.planetRadioButton.TabStop = true;
            this.planetRadioButton.Text = "Click to create planet";
            this.planetRadioButton.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(12, 128);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(647, 518);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 20;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 658);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.planetRadioButton);
            this.Controls.Add(this.gravityRadioButton);
            this.Controls.Add(this.radiusUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "HW13 - Alexander Lao - 11481444";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radiusUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown radiusUpDown;
        private System.Windows.Forms.RadioButton gravityRadioButton;
        private System.Windows.Forms.RadioButton planetRadioButton;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
    }
}

