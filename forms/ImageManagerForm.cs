using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Linq;

namespace Projet_2025_1
{
    public partial class ImageManagerForm : Form
    {
        private readonly ImageRepository _imageRepository;
        private readonly TagRepository _tagRepository;

        public ImageManagerForm()
        {
            InitializeComponent();

            // 设置数据库连接字符串
            string connectionString = "Server=127.0.0.1;Port=3306;Database=tags_images;Uid=root;";
            _imageRepository = new ImageRepository(connectionString);
            _tagRepository = new TagRepository(connectionString);

            // 绑定窗体事件
            this.Load += ImageManagerForm_Load;

            this.btnUpload.Click -= btnUpload_Click;
            this.btnAssignTag.Click -= btnAssignTag_Click;
            this.btnRemoveTag.Click -= btnRemoveTag_Click;

            this.btnUpload.Click += btnUpload_Click;
            this.btnAssignTag.Click += btnAssignTag_Click;
            this.btnRemoveTag.Click += btnRemoveTag_Click;

            // 绑定图片列表选择事件
            this.listBoxImages.SelectedIndexChanged += listBoxImages_SelectedIndexChanged;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void ImageManagerForm_Load(object sender, EventArgs e)
        {
            LoadImages();
        }

        /// <summary>
        /// 加载图片列表
        /// </summary>
        private void LoadImages()
        {
            // 清空现有项
            listBoxImages.Items.Clear();

            // 从数据库获取图片列表
            var images = _imageRepository.GetAllImages();

            foreach (var image in images)
            {
                // 显示文件名而不是完整路径
                var listItem = new
                {
                    FileName = Path.GetFileName(image.FilePath), // 文件名
                    FilePath = image.FilePath,                  // 完整路径
                    Id = image.Id
                };

                // 检查是否已存在相同文件名
                if (!listBoxImages.Items.Cast<dynamic>().Any(item => item.FileName == listItem.FileName))
                {
                    listBoxImages.Items.Add(listItem);
                }
            }

            // 指定 ListBox 显示的内容为文件名
            listBoxImages.DisplayMember = "FileName";
        }

        /// <summary>
        /// 上传图片按钮事件
        /// </summary>
        private void btnUpload_Click(object sender, EventArgs e)
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "JPEG Images|*.jpg;*.jpeg";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    string fileName = Path.GetFileName(filePath);

                    // 检查是否已存在相同文件名
                    if (listBoxImages.Items.Cast<dynamic>().Any(item => item.FileName == fileName))
                    {
                        MessageBox.Show("文件名已存在，请选择不同的文件或重命名后再上传！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // 插入图片记录到数据库
                    int imageId = _imageRepository.InsertImage(filePath);

                    var listItem = new
                    {
                        FileName = fileName, // 文件名
                        FilePath = filePath, // 完整路径
                        Id = imageId
                    };

                    listBoxImages.Items.Add(listItem);
                }
            }
        }

        /// <summary>
        /// 图片列表选择事件
        /// </summary>
        private void listBoxImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxImages.SelectedItem != null)
            {
                dynamic selectedItem = listBoxImages.SelectedItem;
                int imageId = selectedItem.Id;
                string filePath = selectedItem.FilePath;

                // 显示图片预览
                DisplayImagePreview(filePath);

                // 加载图片的标签
                LoadImageTags(imageId);
            }
        }

        /// <summary>
        /// 显示图片预览
        /// </summary>
        private void DisplayImagePreview(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    pictureBoxPreview.Image?.Dispose(); // 避免文件锁定问题
                    pictureBoxPreview.Image = System.Drawing.Image.FromFile(filePath);
                    pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"无法加载图片：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("文件不存在，请检查路径！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载图片关联的标签
        /// </summary>
        private void LoadImageTags(int imageId)
        {
            // 清空现有项
            listBoxImageTags.Items.Clear();

            // 获取图片关联的标签
            var tags = _tagRepository.GetTagsForImage(imageId);
            foreach (var tag in tags)
            {
                listBoxImageTags.Items.Add(tag.TagName);
            }
        }

        /// <summary>
        /// 分配标签按钮事件
        /// </summary>
        private void btnAssignTag_Click(object sender, EventArgs e)
        {
            if (listBoxImages.SelectedItem != null)
            {
                dynamic selectedImage = listBoxImages.SelectedItem;
                int imageId = selectedImage.Id;

                string newTag = Microsoft.VisualBasic.Interaction.InputBox(
                    "请输入要分配的标签：", "分配标签", "");
                if (!string.IsNullOrEmpty(newTag))
                {
                    int tagId = _tagRepository.GetOrCreateTag(newTag);
                    _tagRepository.LinkImageToTag(imageId, tagId);
                    LoadImageTags(imageId);
                }
            }
            else
            {
                MessageBox.Show("请先选择一张图片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 移除标签按钮事件
        /// </summary>
        private void btnRemoveTag_Click(object sender, EventArgs e)
        {
            if (listBoxImages.SelectedItem != null && listBoxImageTags.SelectedItem != null)
            {
                dynamic selectedImage = listBoxImages.SelectedItem;
                int imageId = selectedImage.Id;

                string selectedTag = listBoxImageTags.SelectedItem.ToString();
                var allTags = _tagRepository.GetAllTags();
                var tagToRemove = allTags.Find(tag => tag.TagName == selectedTag);

                if (tagToRemove != null)
                {
                    _tagRepository.UnlinkImageFromTag(imageId, tagToRemove.Id);
                    LoadImageTags(imageId);
                }
            }
            else
            {
                MessageBox.Show("请先选择一张图片和一个标签！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (listBoxImages.SelectedItem != null)
            {
                dynamic selectedImage = listBoxImages.SelectedItem;
                int imageId = selectedImage.Id;

                _tagRepository.UnlinkAllTagsFromImage(imageId);
                listBoxImages.Items.Remove(selectedImage);
                _imageRepository.DeleteImage(imageId);

                MessageBox.Show("图片及其关联标签已成功删除！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);

                pictureBoxPreview.Image = null;
                listBoxImageTags.Items.Clear();
            }
            else
            {
                MessageBox.Show("请先选择一张图片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
