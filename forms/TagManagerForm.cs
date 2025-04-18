using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Projet_2025_1; // 引用新的文件
using MySql.Data.MySqlClient; // 如果需要 MySQL 连接

namespace Projet_2025_1
{
    public partial class TagManagerForm : Form
    {
        /// <summary>
        /// 数据库访问仓储
        /// </summary>
        private TagRepository _tagRepository;

        /// <summary>
        /// 标志位，表示窗体是否加载完毕
        /// </summary>
        private bool _isFormLoaded = false;

        public TagManagerForm()
        {
            InitializeComponent();

            // 移除设计器可能自动加的 AfterSelect 绑定
            this.treeViewTags.AfterSelect -= treeViewTags_AfterSelect;

            // 绑定窗体的 Load 事件
            this.Load += TagManagerForm_Load;

            // 绑定双击事件
            this.treeViewTags.NodeMouseDoubleClick += treeViewTags_NodeMouseDoubleClick;

            // 设置数据库连接字符串（示例，仅供参考）
            string connectionString = "Server=127.0.0.1;Port=3306;Database=tags_images;Uid=root;";
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

            // 添加虚拟根节点
            TreeNode virtualRootNode = new TreeNode("root")
            {
                Tag = null // 虚拟根节点的 Tag 为空
            };
            treeViewTags.Nodes.Add(virtualRootNode);

            // 加载真实的标签
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
            // 不再显示路径信息的弹窗
            if (!_isFormLoaded || treeViewTags.SelectedNode == null) return;

            // 可以根据需要在这里添加单击时的逻辑
        }

        private void treeViewTags_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node != null)
            {
                string path = GetFullPath(e.Node);
                MessageBox.Show($"当前选中标签的路径是：{path}", "标签信息",
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
            // 提示用户输入新标签名称
            string newTagName = Microsoft.VisualBasic.Interaction.InputBox(
                "请输入新标签名称：", "添加标签", "");

            if (!string.IsNullOrEmpty(newTagName))
            {
                TreeNode selectedNode = treeViewTags.SelectedNode; // 当前选中的节点
                int? parentId = null;

                // 如果选中了虚拟根节点，则将新标签设置为顶级标签
                if (selectedNode != null && selectedNode.Text != "新建顶级标签")
                {
                    parentId = (int)selectedNode.Tag;
                }

                // 检查标签名称是否唯一
                if (_tagRepository.IsTagNameUnique(parentId, newTagName))
                {
                    // 插入新标签
                    int newTagId = _tagRepository.InsertTag(parentId, newTagName);
                    TreeNode newNode = new TreeNode(newTagName)
                    {
                        Tag = newTagId
                    };

                    // 如果选中了虚拟根节点或未选中任何节点，则添加为顶级标签
                    if (parentId == null)
                    {
                        treeViewTags.Nodes[0].Nodes.Add(newNode); // 添加到虚拟根节点下
                    }
                    else
                    {
                        selectedNode.Nodes.Add(newNode);
                        selectedNode.Expand(); // 展开父节点以显示子节点
                    }
                }
                else
                {
                    MessageBox.Show("该标签名称已存在（同级重名），请重新输入！", "错误",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("标签名称不能为空！", "错误",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnDeleteTag_Click(object sender, EventArgs e)
        {
            if (treeViewTags.SelectedNode != null)
            {
                var confirm = MessageBox.Show("确认删除选中的标签及其所有子标签吗？",
                    "确认删除", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (confirm == DialogResult.Yes)
                {
                    int tagId = (int)treeViewTags.SelectedNode.Tag;
                    _tagRepository.DeleteTag(tagId);
                    treeViewTags.Nodes.Remove(treeViewTags.SelectedNode);
                }
            }
            else
            {
                MessageBox.Show("请先选择一个标签！", "提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnEditTag_Click(object sender, EventArgs e)
        {
            if (treeViewTags.SelectedNode != null)
            {
                string oldTagName = treeViewTags.SelectedNode.Text;
                string newTagName = Microsoft.VisualBasic.Interaction.InputBox(
                    "请输入新的标签名称：", "修改标签", oldTagName);

                if (!string.IsNullOrEmpty(newTagName))
                {
                    int currentTagId = (int)treeViewTags.SelectedNode.Tag;
                    int? parentId = null;
                    if (treeViewTags.SelectedNode.Parent != null)
                    {
                        parentId = (int)treeViewTags.SelectedNode.Parent.Tag;
                    }

                    if (_tagRepository.IsTagNameUnique(parentId, newTagName, currentTagId))
                    {
                        _tagRepository.UpdateTag(currentTagId, newTagName);
                        treeViewTags.SelectedNode.Text = newTagName;
                    }
                    else
                    {
                        MessageBox.Show("该标签名称已存在（同级重名），请重新输入！",
                                        "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("标签名称不能为空！", "错误",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("请先选择一个标签！", "提示",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
