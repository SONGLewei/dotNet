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

            string connectionString = "Server=127.0.0.1;Port=3306;Database=tags_images;Uid=root;Pwd=123456";
            _tagRepository = new TagRepository(connectionString);

            btnSearch.Click += BtnSearch_Click;

            listBoxFilteredImages.MouseDoubleClick += ListBoxFilteredImages_MouseDoubleClick;
        }

        /// <summary>
        /// 搜索按钮点击事件
        /// Événement de clic sur le bouton de recherche
        /// </summary>
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string tagName = txtSearchTags.Text.Trim();

            if (string.IsNullOrEmpty(tagName))
            {
                MessageBox.Show("Veuillez saisir le nom du tag pour rechercher !", "indice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var filteredImages = _tagRepository.GetImagesByTag(tagName);

            listBoxFilteredImages.Items.Clear();
            if (filteredImages.Count == 0)
            {
                MessageBox.Show("Aucune image n'a été trouvée associée à cette balise !", "indice", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        /// Double-cliquez sur une image dans la liste des images pour la prévisualiser
        /// </summary>
        private void ListBoxFilteredImages_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int index = listBoxFilteredImages.IndexFromPoint(e.Location);
            if (index != ListBox.NoMatches)
            {
                dynamic selectedItem = listBoxFilteredImages.Items[index];
                string filePath = selectedItem.FilePath;

                DisplayImagePreview(filePath);
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
                    pictureBoxPreviewSearch.Image?.Dispose();
                    pictureBoxPreviewSearch.Image = Image.FromFile(filePath);
                    pictureBoxPreviewSearch.SizeMode = PictureBoxSizeMode.Zoom;
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
    }
}
