using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Projet_2025_1
{
    public partial class SearchForm : Form
    {
        private readonly TagRepository _tagRepository;

        public SearchForm()
        {
            InitializeComponent();

            // 设置数据库连接字符串
            string connectionString = "Server=127.0.0.1;Port=3306;Database=tags_images;Uid=root;";
            _tagRepository = new TagRepository(connectionString);

            // 按钮点击事件绑定
            btnSearch.Click += BtnSearch_Click;

            // 图片列表双击事件绑定
            listBoxFilteredImages.MouseDoubleClick += ListBoxFilteredImages_MouseDoubleClick;
        }

        /// <summary>
        /// 搜索按钮点击事件
        /// </summary>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string tagName = txtSearchTags.Text.Trim();

            if (string.IsNullOrEmpty(tagName))
            {
                MessageBox.Show("请输入要搜索的标签名称！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 获取与标签关联的图片列表
            var filteredImages = _tagRepository.GetImagesByTag(tagName);

            // 更新 ListBox
            listBoxFilteredImages.Items.Clear();
            if (filteredImages.Count == 0)
            {
                MessageBox.Show("未找到与该标签关联的图片！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            foreach (var image in filteredImages)
            {
                listBoxFilteredImages.Items.Add(new
                {
                    FileName = Path.GetFileName(image.FilePath),
                    FilePath = image.FilePath
                });
            }

            listBoxFilteredImages.DisplayMember = "FileName";
        }

        /// <summary>
        /// 双击图片列表中的图片以预览
        /// </summary>
        private void ListBoxFilteredImages_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBoxFilteredImages.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                dynamic selectedItem = listBoxFilteredImages.Items[index];
                string filePath = selectedItem.FilePath;

                // 显示图片预览
                DisplayImagePreview(filePath);
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
                    pictureBoxPreviewSearch.Image?.Dispose();
                    pictureBoxPreviewSearch.Image = Image.FromFile(filePath);
                    pictureBoxPreviewSearch.SizeMode = PictureBoxSizeMode.Zoom;
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
    }
}
