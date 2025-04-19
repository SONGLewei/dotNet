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

            string connectionString = "Server=127.0.0.1;Port=3306;Database=tags_images;Uid=root;Pwd=";
            _imageRepository = new ImageRepository(connectionString);
            _tagRepository = new TagRepository(connectionString);

            this.Load += ImageManagerForm_Load;

            this.btnUpload.Click -= btnUpload_Click;
            this.btnAssignTag.Click -= btnAssignTag_Click;
            this.btnRemoveTag.Click -= btnRemoveTag_Click;

            this.btnUpload.Click += btnUpload_Click;
            this.btnAssignTag.Click += btnAssignTag_Click;
            this.btnRemoveTag.Click += btnRemoveTag_Click;

            this.listBoxImages.SelectedIndexChanged += listBoxImages_SelectedIndexChanged;
        }

        /// <summary>
        /// 窗体加载事件
        /// Événement de chargement de formulaire
        /// </summary>
        private void ImageManagerForm_Load(object sender, EventArgs e)
        {
            LoadImages();
        }

        /// <summary>
        /// 加载图片列表
        /// Chargement de la liste d'images
        /// </summary>
        private void LoadImages()
        {
            listBoxImages.Items.Clear();

            var images = _imageRepository.GetAllImages();

            foreach (var image in images)
            {
                var listItem = new
                {
                    FileName = Path.GetFileName(image.FilePath),
                    FilePath = image.FilePath,
                    Id = image.Id
                };

                if (!listBoxImages.Items.Cast<dynamic>().Any(item => item.FileName == listItem.FileName))
                {
                    listBoxImages.Items.Add(listItem);
                }
            }

            listBoxImages.DisplayMember = "FileName";
        }

        /// <summary>
        /// 上传图片按钮事件
        /// Événement du bouton de téléchargement d'image
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

                    if (listBoxImages.Items.Cast<dynamic>().Any(item => item.FileName == fileName))
                    {
                        MessageBox.Show("Le nom du fichier existe déjà, veuillez sélectionner un fichier différent ou le renommer avant de le télécharger !", "indice", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    int imageId = _imageRepository.InsertImage(filePath);

                    var listItem = new
                    {
                        FileName = fileName,
                        FilePath = filePath,
                        Id = imageId
                    };

                    listBoxImages.Items.Add(listItem);
                }
            }
        }

        /// <summary>
        /// 图片列表选择事件
        /// Événement de sélection de liste d'images
        /// </summary>
        private void listBoxImages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxImages.SelectedItem != null)
            {
                dynamic selectedItem = listBoxImages.SelectedItem;
                int imageId = selectedItem.Id;
                string filePath = selectedItem.FilePath;

                DisplayImagePreview(filePath);

                LoadImageTags(imageId);
            }
        }

        /// <summary>
        /// 显示图片预览
        /// Afficher l'aperçu de l'image
        /// </summary>
        private void DisplayImagePreview(string filePath)
        {
            if (File.Exists(filePath))
            {
                try
                {
                    pictureBoxPreview.Image?.Dispose();
                    pictureBoxPreview.Image = System.Drawing.Image.FromFile(filePath);
                    pictureBoxPreview.SizeMode = PictureBoxSizeMode.Zoom;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Impossible de charger l'image :{ex.Message}", "erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Le fichier n'existe pas, veuillez vérifier le chemin !", "erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 加载图片关联的标签
        /// Charger les balises associées à l'image
        /// </summary>
        private void LoadImageTags(int imageId)
        {
            listBoxImageTags.Items.Clear();

            var tags = _tagRepository.GetTagsForImage(imageId);
            foreach (var tag in tags)
            {
                listBoxImageTags.Items.Add(tag.TagName);
            }
        }

        /// <summary>
        /// 分配标签按钮事件
        /// Attribution d'événements de bouton d'étiquette
        /// </summary>
        private void btnAssignTag_Click(object sender, EventArgs e)
        {
            if (listBoxImages.SelectedItem != null)
            {
                dynamic selectedImage = listBoxImages.SelectedItem;
                int imageId = selectedImage.Id;

                string newTag = Microsoft.VisualBasic.Interaction.InputBox(
                    "Veuillez saisir l'étiquette que vous souhaitez attribuer : ", "Attribution de balises", "");
                if (!string.IsNullOrEmpty(newTag))
                {
                    int tagId = _tagRepository.GetOrCreateTag(newTag);
                    _tagRepository.LinkImageToTag(imageId, tagId);
                    LoadImageTags(imageId);
                }
            }
            else
            {
                MessageBox.Show("Veuillez d'abord sélectionner une image !", "indice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 移除标签按钮事件
        /// Supprimer l'événement du bouton d'étiquette
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
                MessageBox.Show("Veuillez d'abord sélectionner une image et un tag !", "indice", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                MessageBox.Show("L'image et ses balises associées ont été supprimées avec succès !", "indice", MessageBoxButtons.OK, MessageBoxIcon.Information);

                pictureBoxPreview.Image = null;
                listBoxImageTags.Items.Clear();
            }
            else
            {
                MessageBox.Show("Veuillez d'abord sélectionner une image !", "indice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
