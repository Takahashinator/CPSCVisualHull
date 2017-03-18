namespace VisualHullReconstruction
{
    partial class TestForm
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
            this.buttonRunTests = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonRunTests
            // 
            this.buttonRunTests.Location = new System.Drawing.Point(358, 12);
            this.buttonRunTests.Name = "buttonRunTests";
            this.buttonRunTests.Size = new System.Drawing.Size(75, 23);
            this.buttonRunTests.TabIndex = 0;
            this.buttonRunTests.Text = "Run Tests";
            this.buttonRunTests.UseVisualStyleBackColor = true;
            this.buttonRunTests.Click += new System.EventHandler(this.buttonRunTests_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 12);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox1.Size = new System.Drawing.Size(339, 383);
            this.textBox1.TabIndex = 1;
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 407);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonRunTests);
            this.Name = "TestForm";
            this.Text = "TestForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonRunTests;
        private System.Windows.Forms.TextBox textBox1;
    }
}