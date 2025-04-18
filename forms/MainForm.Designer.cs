namespace Projet_2025_1
{
    partial class MainForm
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
            this.btnManageTags = new System.Windows.Forms.Button();
            this.btnManageImages = new System.Windows.Forms.Button();
            this.btnSearchImages = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnManageTags
            // 
            this.btnManageTags.Location = new System.Drawing.Point(68, 52);
            this.btnManageTags.Name = "btnManageTags";
            this.btnManageTags.Size = new System.Drawing.Size(193, 87);
            this.btnManageTags.TabIndex = 0;
            this.btnManageTags.Text = "ManageTags";
            this.btnManageTags.UseVisualStyleBackColor = true;
            this.btnManageTags.Click += new System.EventHandler(this.btnManageTags_Click);
            // 
            // btnManageImages
            // 
            this.btnManageImages.Location = new System.Drawing.Point(68, 162);
            this.btnManageImages.Name = "btnManageImages";
            this.btnManageImages.Size = new System.Drawing.Size(193, 106);
            this.btnManageImages.TabIndex = 1;
            this.btnManageImages.Text = "ManageImages";
            this.btnManageImages.UseVisualStyleBackColor = true;
            this.btnManageImages.Click += new System.EventHandler(this.btnManageImages_Click);
            // 
            // btnSearchImages
            // 
            this.btnSearchImages.Location = new System.Drawing.Point(68, 302);
            this.btnSearchImages.Name = "btnSearchImages";
            this.btnSearchImages.Size = new System.Drawing.Size(180, 109);
            this.btnSearchImages.TabIndex = 2;
            this.btnSearchImages.Text = "SearchImages";
            this.btnSearchImages.UseVisualStyleBackColor = true;
            this.btnSearchImages.Click += new System.EventHandler(this.btnSearchImages_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnSearchImages);
            this.Controls.Add(this.btnManageImages);
            this.Controls.Add(this.btnManageTags);
            this.Name = "MainForm";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnManageTags;
        private System.Windows.Forms.Button btnManageImages;
        private System.Windows.Forms.Button btnSearchImages;
    }
}