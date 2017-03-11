namespace VisualHullReconstruction
{
    partial class VisualHullApp
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBarGenerate = new System.Windows.Forms.ProgressBar();
            this.listViewEdited = new System.Windows.Forms.ListView();
            this.progressBarLoad = new System.Windows.Forms.ProgressBar();
            this.listViewOrig = new System.Windows.Forms.ListView();
            this.buttonGenerate = new System.Windows.Forms.Button();
            this.buttonLoadImages = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.progressBarHull = new System.Windows.Forms.ProgressBar();
            this.buttonHull = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressBarGenerate);
            this.panel1.Controls.Add(this.listViewEdited);
            this.panel1.Controls.Add(this.progressBarLoad);
            this.panel1.Controls.Add(this.listViewOrig);
            this.panel1.Controls.Add(this.buttonGenerate);
            this.panel1.Controls.Add(this.buttonLoadImages);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(445, 521);
            this.panel1.TabIndex = 0;
            // 
            // progressBarGenerate
            // 
            this.progressBarGenerate.Location = new System.Drawing.Point(3, 495);
            this.progressBarGenerate.Name = "progressBarGenerate";
            this.progressBarGenerate.Size = new System.Drawing.Size(310, 23);
            this.progressBarGenerate.TabIndex = 5;
            // 
            // listViewEdited
            // 
            this.listViewEdited.Location = new System.Drawing.Point(3, 259);
            this.listViewEdited.Name = "listViewEdited";
            this.listViewEdited.Size = new System.Drawing.Size(439, 230);
            this.listViewEdited.TabIndex = 4;
            this.listViewEdited.UseCompatibleStateImageBehavior = false;
            this.listViewEdited.View = System.Windows.Forms.View.Details;
            // 
            // progressBarLoad
            // 
            this.progressBarLoad.Location = new System.Drawing.Point(3, 230);
            this.progressBarLoad.Name = "progressBarLoad";
            this.progressBarLoad.Size = new System.Drawing.Size(328, 23);
            this.progressBarLoad.TabIndex = 3;
            // 
            // listViewOrig
            // 
            this.listViewOrig.Location = new System.Drawing.Point(3, 3);
            this.listViewOrig.Name = "listViewOrig";
            this.listViewOrig.Size = new System.Drawing.Size(439, 221);
            this.listViewOrig.TabIndex = 2;
            this.listViewOrig.UseCompatibleStateImageBehavior = false;
            this.listViewOrig.View = System.Windows.Forms.View.Details;
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.Location = new System.Drawing.Point(319, 495);
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(123, 23);
            this.buttonGenerate.TabIndex = 1;
            this.buttonGenerate.Text = "Generate Silhouettes";
            this.buttonGenerate.UseVisualStyleBackColor = true;
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // buttonLoadImages
            // 
            this.buttonLoadImages.Location = new System.Drawing.Point(337, 230);
            this.buttonLoadImages.Name = "buttonLoadImages";
            this.buttonLoadImages.Size = new System.Drawing.Size(105, 23);
            this.buttonLoadImages.TabIndex = 0;
            this.buttonLoadImages.Text = "Load Images";
            this.buttonLoadImages.UseVisualStyleBackColor = true;
            this.buttonLoadImages.Click += new System.EventHandler(this.buttonLoadImages_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.progressBarHull);
            this.panel2.Controls.Add(this.buttonHull);
            this.panel2.Location = new System.Drawing.Point(12, 536);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(444, 33);
            this.panel2.TabIndex = 1;
            // 
            // progressBarHull
            // 
            this.progressBarHull.Location = new System.Drawing.Point(3, 7);
            this.progressBarHull.Name = "progressBarHull";
            this.progressBarHull.Size = new System.Drawing.Size(310, 23);
            this.progressBarHull.TabIndex = 6;
            // 
            // buttonHull
            // 
            this.buttonHull.Location = new System.Drawing.Point(318, 7);
            this.buttonHull.Name = "buttonHull";
            this.buttonHull.Size = new System.Drawing.Size(123, 23);
            this.buttonHull.TabIndex = 6;
            this.buttonHull.Text = "Generate Visual Hull";
            this.buttonHull.UseVisualStyleBackColor = true;
            this.buttonHull.Click += new System.EventHandler(this.buttonHull_Click);
            // 
            // VisualHullApp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 572);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "VisualHullApp";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar progressBarGenerate;
        private System.Windows.Forms.ListView listViewEdited;
        private System.Windows.Forms.ProgressBar progressBarLoad;
        private System.Windows.Forms.ListView listViewOrig;
        private System.Windows.Forms.Button buttonGenerate;
        private System.Windows.Forms.Button buttonLoadImages;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ProgressBar progressBarHull;
        private System.Windows.Forms.Button buttonHull;
    }
}

