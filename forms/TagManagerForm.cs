using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Projet_2025_1
{
    public partial class TagManagerForm : Form
    {
        /// <summary>
        /// 数据库访问仓储
        /// Référentiel d'accès à la base de données
        /// </summary>
        private TagRepository _tagRepository;

        /// <summary>
        /// 标志位，表示窗体是否加载完毕
        /// Drapeau indiquant si le formulaire a été chargé
        /// </summary>
        private bool _isFormLoaded = false;

        public TagManagerForm()
        {
            InitializeComponent();

            this.treeViewTags.AfterSelect -= treeViewTags_AfterSelect;

            this.Load += TagManagerForm_Load;

            this.treeViewTags.NodeMouseDoubleClick += treeViewTags_NodeMouseDoubleClick;

            string connectionString = "Server=127.0.0.1;Port=3306;Database=tags_images;Uid=root;Pwd=";
            _tagRepository = new TagRepository(connectionString);
        }

        private void TagManagerForm_Load(object sender, EventArgs e)
        {
            LoadTagsFromDatabase();
            _isFormLoaded = true;
            this.treeViewTags.AfterSelect += treeViewTags_AfterSelect;
        }

        private void LoadTagsFromDatabase()
        {
            treeViewTags.BeginUpdate();
            treeViewTags.Nodes.Clear();

            TreeNode virtualRootNode = new TreeNode("root")
            {
                Tag = null
            };
            treeViewTags.Nodes.Add(virtualRootNode);

            List<Tag> allTags = _tagRepository.GetAllTags();
            var rootTags = allTags.Where(t => t.ParentId == null).ToList();
            foreach (var rootTag in rootTags)
            {
                TreeNode rootNode = new TreeNode(rootTag.TagName)
                {
                    Tag = rootTag.Id
                };
                virtualRootNode.Nodes.Add(rootNode);
                AddChildNodes(rootNode, allTags);
            }

            treeViewTags.ExpandAll();
            treeViewTags.EndUpdate();
            treeViewTags.SelectedNode = null;
        }

        private void AddChildNodes(TreeNode parentNode, List<Tag> allTags)
        {
            int parentId = (int)parentNode.Tag;
            var childTags = allTags.Where(t => t.ParentId == parentId).ToList();
            foreach (var childTag in childTags)
            {
                TreeNode childNode = new TreeNode(childTag.TagName)
                {
                    Tag = childTag.Id
                };
                parentNode.Nodes.Add(childNode);
                AddChildNodes(childNode, allTags);
            }
        }

        private void treeViewTags_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!_isFormLoaded || treeViewTags.SelectedNode == null) return;
        }

        private void treeViewTags_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                string path = GetFullPath(e.Node);
                MessageBox.Show($"Le chemin de la balise actuellement sélectionnée est : {path}", "Informations sur les balises",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private string GetFullPath(TreeNode node)
        {
            if (node == null) return string.Empty;

            var sb = new StringBuilder();
            while (node != null)
            {
                sb.Insert(0, node.Text + "/");
                node = node.Parent;
            }
            return sb.ToString().TrimEnd('/');
        }

        private void btnAddTag_Click(object sender, EventArgs e)
        {
            string newTagName = Microsoft.VisualBasic.Interaction.InputBox(
                "Veuillez saisir un nouveau nom d'étiquette : ", "Ajouter des balises", "");

            if (!string.IsNullOrEmpty(newTagName))
            {
                TreeNode selectedNode = treeViewTags.SelectedNode;
                int? parentId = null;

                if (selectedNode != null && selectedNode.Text != "Créer une nouvelle balise de niveau supérieur")
                {
                    parentId = selectedNode.Tag == null ? (int?)null : (int)selectedNode.Tag;

                }

                if (_tagRepository.IsTagNameUnique(parentId, newTagName))
                {
                    int newTagId = _tagRepository.InsertTag(parentId, newTagName);
                    LoadTagsFromDatabase();
                }
                else
                {
                    MessageBox.Show("Le nom de la balise existe déjà (nom en double au même niveau), veuillez le saisir à nouveau !", "erreur",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Le nom de la balise ne peut pas être vide !", "erreur",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDeleteTag_Click(object sender, EventArgs e)
        {
            TreeNode node = treeViewTags.SelectedNode;
            if (node == null)
            {
                MessageBox.Show("Veuillez d'abord sélectionner une balise！",
                                "indice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (node.Tag == null)
            {
                MessageBox.Show("Le nœud racine virtuel ne peut pas être supprimé。",
                                "erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (treeViewTags.SelectedNode != null)
            {
                var confirm = MessageBox.Show("Êtes-vous sûr de vouloir supprimer la balise sélectionnée et toutes ses sous-balises ?",
                    "Confirmer la suppression", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    int tagId = (int)treeViewTags.SelectedNode.Tag;
                    _tagRepository.DeleteTag(tagId);
                    treeViewTags.Nodes.Remove(treeViewTags.SelectedNode);
                }
            }
            else
            {
                MessageBox.Show("Veuillez d'abord sélectionner une balise！", "indice",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        private void btnEditTag_Click(object sender, EventArgs e)
        {
            if (treeViewTags.SelectedNode != null)
            {
                string oldTagName = treeViewTags.SelectedNode.Text;
                string newTagName = Microsoft.VisualBasic.Interaction.InputBox(
                    "Veuillez d'abord sélectionner un tag ： ", "Modifier les balises", oldTagName);

                if (!string.IsNullOrEmpty(newTagName))
                {
                    int currentTagId = (int)treeViewTags.SelectedNode.Tag;
                    int? parentId = null;

                    if (treeViewTags.SelectedNode.Parent != null &&
                        treeViewTags.SelectedNode.Parent.Tag != null)
                    {
                        parentId = Convert.ToInt32(treeViewTags.SelectedNode.Parent.Tag);
                    }


                    if (_tagRepository.IsTagNameUnique(parentId, newTagName, currentTagId))
                    {
                        _tagRepository.UpdateTag(currentTagId, newTagName);
                        treeViewTags.SelectedNode.Text = newTagName;
                    }
                    else
                    {
                        MessageBox.Show("Le nom de la balise existe déjà (nom en double au même niveau), veuillez le saisir à nouveau ! ",
                                        "erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                }
                else
                {
                    MessageBox.Show("Le nom de la balise ne peut pas être vide！", "erreur",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Veuillez d'abord sélectionner une balise！", "indice",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        
        

    }
}
