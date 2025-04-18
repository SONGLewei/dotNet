namespace Projet_2025_1
{
    partial class ImageManagerForm
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
            this.btnUpload = new System.Windows.Forms.Button();
            this.btnAssignTag = new System.Windows.Forms.Button();
            this.btnRemoveTag = new System.Windows.Forms.Button();
            this.listBoxImages = new System.Windows.Forms.ListBox();
            this.listBoxImageTags = new System.Windows.Forms.ListBox();
            this.pictureBoxPreview = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).BeginInit();
            this.SuspendLayout();
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(351, 272);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(108, 23);
            this.btnUpload.TabIndex = 1;
            this.btnUpload.Text = "Upload";
            this.btnUpload.UseVisualStyleBackColor = true;
            this.btnUpload.Click += new System.EventHandler(this.btnUpload_Click);
            // 
            // btnAssignTag
            // 
            this.btnAssignTag.Location = new System.Drawing.Point(156, 370);
            this.btnAssignTag.Name = "btnAssignTag";
            this.btnAssignTag.Size = new System.Drawing.Size(114, 23);
            this.btnAssignTag.TabIndex = 7;
            this.btnAssignTag.Text = "AssignTag";
            this.btnAssignTag.UseVisualStyleBackColor = true;
            this.btnAssignTag.Click += new System.EventHandler(this.btnAssignTag_Click);
            // 
            // btnRemoveTag
            // 
            this.btnRemoveTag.Location = new System.Drawing.Point(516, 370);
            this.btnRemoveTag.Name = "btnRemoveTag";
            this.btnRemoveTag.Size = new System.Drawing.Size(105, 23);
            this.btnRemoveTag.TabIndex = 8;
            this.btnRemoveTag.Text = "RemoveTag";
            this.btnRemoveTag.UseVisualStyleBackColor = true;
            this.btnRemoveTag.Click += new System.EventHandler(this.btnRemoveTag_Click);
            // 
            // listBoxImages
            // 
            this.listBoxImages.FormattingEnabled = true;
            this.listBoxImages.ItemHeight = 15;
            this.listBoxImages.Location = new System.Drawing.Point(317, 123);
            this.listBoxImages.Name = "listBoxImages";
            this.listBoxImages.Size = new System.Drawing.Size(120, 94);
            this.listBoxImages.TabIndex = 9;
            this.listBoxImages.SelectedIndexChanged += new System.EventHandler(this.listBoxImages_SelectedIndexChanged);
            // 
            // listBoxImageTags
            // 
            this.listBoxImageTags.FormattingEnabled = true;
            this.listBoxImageTags.ItemHeight = 15;
            this.listBoxImageTags.Location = new System.Drawing.Point(497, 123);
            this.listBoxImageTags.Name = "listBoxImageTags";
            this.listBoxImageTags.Size = new System.Drawing.Size(164, 94);
            this.listBoxImageTags.TabIndex = 10;
            // 
            // pictureBoxPreview
            // 
            this.pictureBoxPreview.Location = new System.Drawing.Point(52, 23);
            this.pictureBoxPreview.Name = "pictureBoxPreview";
            this.pictureBoxPreview.Size = new System.Drawing.Size(218, 272);
            this.pictureBoxPreview.TabIndex = 11;
            this.pictureBoxPreview.TabStop = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(551, 272);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(145, 23);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "Deletelist_item";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // ImageManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.pictureBoxPreview);
            this.Controls.Add(this.listBoxImageTags);
            this.Controls.Add(this.listBoxImages);
            this.Controls.Add(this.btnRemoveTag);
            this.Controls.Add(this.btnAssignTag);
            this.Controls.Add(this.btnUpload);
            this.Name = "ImageManagerForm";
            this.Text = "ImageManagerForm";
            this.Load += new System.EventHandler(this.ImageManagerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxPreview)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnUpload;
        private System.Windows.Forms.Button btnAssignTag;
        private System.Windows.Forms.Button btnRemoveTag;
        private System.Windows.Forms.ListBox listBoxImages;
        private System.Windows.Forms.ListBox listBoxImageTags;
        private System.Windows.Forms.PictureBox pictureBoxPreview;
        private System.Windows.Forms.Button btnDelete;
    }
}