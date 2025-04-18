namespace Projet_2025_1
{
    partial class SearchForm
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
            this.txtSearchTags = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.listBoxFilteredImages = new System.Windows.Forms.ListBox();
            this.pictureBoxPreviewSearch = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreviewSearch)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSearchTags
            // 
            this.txtSearchTags.Location = new System.Drawing.Point(100, 30);
            this.txtSearchTags.Name = "txtSearchTags";
            this.txtSearchTags.Size = new System.Drawing.Size(150, 25);
            this.txtSearchTags.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(280, 30);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 25);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            // 
            // listBoxFilteredImages
            // 
            this.listBoxFilteredImages.FormattingEnabled = true;
            this.listBoxFilteredImages.ItemHeight = 15;
            this.listBoxFilteredImages.Location = new System.Drawing.Point(100, 70);
            this.listBoxFilteredImages.Name = "listBoxFilteredImages";
            this.listBoxFilteredImages.Size = new System.Drawing.Size(200, 100);
            this.listBoxFilteredImages.TabIndex = 2;
            // 
            // pictureBoxPreviewSearch
            // 
            this.pictureBoxPreviewSearch.Location = new System.Drawing.Point(350, 70);
            this.pictureBoxPreviewSearch.Name = "pictureBoxPreviewSearch";
            this.pictureBoxPreviewSearch.Size = new System.Drawing.Size(150, 150);
            this.pictureBoxPreviewSearch.TabIndex = 3;
            this.pictureBoxPreviewSearch.TabStop = false;
            // 
            // SearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 250);
            this.Controls.Add(this.pictureBoxPreviewSearch);
            this.Controls.Add(this.listBoxFilteredImages);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.txtSearchTags);
            this.Name = "SearchForm";
            this.Text = "SearchForm";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreviewSearch)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox txtSearchTags;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListBox listBoxFilteredImages;
        private System.Windows.Forms.PictureBox pictureBoxPreviewSearch;
    }
}
