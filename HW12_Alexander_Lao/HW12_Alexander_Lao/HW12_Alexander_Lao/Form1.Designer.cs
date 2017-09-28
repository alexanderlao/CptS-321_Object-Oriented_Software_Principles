namespace HW12_Alexander_Lao
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
            this.urlLabel = new System.Windows.Forms.Label();
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.downloadButton = new System.Windows.Forms.Button();
            this.downloadResultLabel = new System.Windows.Forms.Label();
            this.resultTextBox = new System.Windows.Forms.TextBox();
            this.sortingButton = new System.Windows.Forms.Button();
            this.singleTimeLabel = new System.Windows.Forms.Label();
            this.multiTimeLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // urlLabel
            // 
            this.urlLabel.AutoSize = true;
            this.urlLabel.Location = new System.Drawing.Point(13, 13);
            this.urlLabel.Name = "urlLabel";
            this.urlLabel.Size = new System.Drawing.Size(36, 17);
            this.urlLabel.TabIndex = 0;
            this.urlLabel.Text = "URL";
            // 
            // urlTextBox
            // 
            this.urlTextBox.Location = new System.Drawing.Point(13, 34);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(297, 22);
            this.urlTextBox.TabIndex = 1;
            // 
            // downloadButton
            // 
            this.downloadButton.Location = new System.Drawing.Point(13, 63);
            this.downloadButton.Name = "downloadButton";
            this.downloadButton.Size = new System.Drawing.Size(297, 26);
            this.downloadButton.TabIndex = 2;
            this.downloadButton.Text = "Go (download string from URL)";
            this.downloadButton.UseVisualStyleBackColor = true;
            this.downloadButton.Click += new System.EventHandler(this.downloadButton_Click);
            // 
            // downloadResultLabel
            // 
            this.downloadResultLabel.AutoSize = true;
            this.downloadResultLabel.Location = new System.Drawing.Point(13, 96);
            this.downloadResultLabel.Name = "downloadResultLabel";
            this.downloadResultLabel.Size = new System.Drawing.Size(182, 17);
            this.downloadResultLabel.TabIndex = 3;
            this.downloadResultLabel.Text = "Download Result (as string)";
            // 
            // resultTextBox
            // 
            this.resultTextBox.Location = new System.Drawing.Point(13, 117);
            this.resultTextBox.Multiline = true;
            this.resultTextBox.Name = "resultTextBox";
            this.resultTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.resultTextBox.Size = new System.Drawing.Size(297, 232);
            this.resultTextBox.TabIndex = 4;
            // 
            // sortingButton
            // 
            this.sortingButton.Location = new System.Drawing.Point(382, 13);
            this.sortingButton.Name = "sortingButton";
            this.sortingButton.Size = new System.Drawing.Size(266, 26);
            this.sortingButton.TabIndex = 5;
            this.sortingButton.Text = "Go (sorting)";
            this.sortingButton.UseVisualStyleBackColor = true;
            this.sortingButton.Click += new System.EventHandler(this.sortingButton_Click);
            // 
            // singleTimeLabel
            // 
            this.singleTimeLabel.AutoSize = true;
            this.singleTimeLabel.Location = new System.Drawing.Point(379, 42);
            this.singleTimeLabel.Name = "singleTimeLabel";
            this.singleTimeLabel.Size = new System.Drawing.Size(143, 17);
            this.singleTimeLabel.TabIndex = 6;
            this.singleTimeLabel.Text = "Single-threaded time:";
            // 
            // multiTimeLabel
            // 
            this.multiTimeLabel.AutoSize = true;
            this.multiTimeLabel.Location = new System.Drawing.Point(379, 63);
            this.multiTimeLabel.Name = "multiTimeLabel";
            this.multiTimeLabel.Size = new System.Drawing.Size(133, 17);
            this.multiTimeLabel.TabIndex = 7;
            this.multiTimeLabel.Text = "Multi-threaded time:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 361);
            this.Controls.Add(this.multiTimeLabel);
            this.Controls.Add(this.singleTimeLabel);
            this.Controls.Add(this.sortingButton);
            this.Controls.Add(this.resultTextBox);
            this.Controls.Add(this.downloadResultLabel);
            this.Controls.Add(this.downloadButton);
            this.Controls.Add(this.urlTextBox);
            this.Controls.Add(this.urlLabel);
            this.Name = "Form1";
            this.Text = "HW12 - Alexander Lao - 11481444";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label urlLabel;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Button downloadButton;
        private System.Windows.Forms.Label downloadResultLabel;
        private System.Windows.Forms.TextBox resultTextBox;
        private System.Windows.Forms.Button sortingButton;
        private System.Windows.Forms.Label singleTimeLabel;
        private System.Windows.Forms.Label multiTimeLabel;
    }
}

