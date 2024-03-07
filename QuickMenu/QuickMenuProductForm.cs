using System;
using System.Linq;
using System.Windows.Forms;

namespace QuickMenu
{
    public partial class QuickMenuProductForm : Form
    {
        private readonly FileOperations<string> fileOperations = new FileOperations<string>("QuickMenu.txt");

        private int _index;

        public QuickMenuProductForm(int index)
        {
            InitializeComponent();
            _index = index;
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string text = "#" + _index + "/" + listBoxProducts.Text;

            string file = fileOperations.ReadFile();
            string[] menus = file.Split('#');
            foreach (var menu in menus)
            {
                if (!string.IsNullOrEmpty(menu))
                {
                    string[] properties = menu.Split('/');

                    if (listBoxProducts.Text.Contains(properties[1]))
                    {
                        fileOperations.FindAndRemoveLine("#" + menu);
                    }
                }
            }

            fileOperations.CreateFile();
            fileOperations.WriteToFile(text, true);

            this.Close();
        }
    }
}
