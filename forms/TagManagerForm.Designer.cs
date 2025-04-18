namespace Projet_2025_1
{
    partial class TagManagerForm
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
            this.treeViewTags = new System.Windows.Forms.TreeView();
            this.btnAddTag = new System.Windows.Forms.Button();
            this.btnDeleteTag = new System.Windows.Forms.Button();
            this.btnEditTag = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // treeViewTags
            // 
            this.treeViewTags.Location = new System.Drawing.Point(54, 50);
            this.treeViewTags.Name = "treeViewTags";
            this.treeViewTags.Size = new System.Drawing.Size(200, 300);
            this.treeViewTags.TabIndex = 0;
            this.treeViewTags.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeViewTags_AfterSelect);
            // 
            // btnAddTag
            // 
            this.btnAddTag.Location = new System.Drawing.Point(300, 50);
            this.btnAddTag.Name = "btnAddTag";
            this.btnAddTag.Size = new System.Drawing.Size(100, 30);
            this.btnAddTag.TabIndex = 1;
            this.btnAddTag.Text = "Add Tag";
            this.btnAddTag.UseVisualStyleBackColor = true;
            this.btnAddTag.Click += new System.EventHandler(this.btnAddTag_Click);
            // 
            // btnDeleteTag
            // 
            this.btnDeleteTag.Location = new System.Drawing.Point(300, 100);
            this.btnDeleteTag.Name = "btnDeleteTag";
            this.btnDeleteTag.Size = new System.Drawing.Size(100, 30);
            this.btnDeleteTag.TabIndex = 2;
            this.btnDeleteTag.Text = "Delete Tag";
            this.btnDeleteTag.UseVisualStyleBackColor = true;
            this.btnDeleteTag.Click += new System.EventHandler(this.btnDeleteTag_Click);
            // 
            // btnEditTag
            // 
            this.btnEditTag.Location = new System.Drawing.Point(300, 150);
            this.btnEditTag.Name = "btnEditTag";
            this.btnEditTag.Size = new System.Drawing.Size(100, 30);
            this.btnEditTag.TabIndex = 3;
            this.btnEditTag.Text = "Edit Tag";
            this.btnEditTag.UseVisualStyleBackColor = true;
            this.btnEditTag.Click += new System.EventHandler(this.btnEditTag_Click);
            // 
            // TagManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 400);
            this.Controls.Add(this.treeViewTags);
            this.Controls.Add(this.btnAddTag);
            this.Controls.Add(this.btnDeleteTag);
            this.Controls.Add(this.btnEditTag);
            this.Name = "TagManagerForm";
            this.Text = "Tag Manager";
            this.Load += new System.EventHandler(this.TagManagerForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewTags;
        private System.Windows.Forms.Button btnAddTag;
        private System.Windows.Forms.Button btnDeleteTag;
        private System.Windows.Forms.Button btnEditTag;
    }
}
