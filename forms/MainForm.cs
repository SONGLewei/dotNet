using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projet_2025_1
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnManageTags_Click(object sender, EventArgs e)
        {
            TagManagerForm tagManagerForm = new TagManagerForm();
            tagManagerForm.Show(); 
        }

        private void btnManageImages_Click(object sender, EventArgs e)
        {
            ImageManagerForm imageManagerForm = new ImageManagerForm();
            imageManagerForm.Show();
        }

        private void btnSearchImages_Click(object sender, EventArgs e)
        {
            SearchForm searchForm = new SearchForm();
            searchForm.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
