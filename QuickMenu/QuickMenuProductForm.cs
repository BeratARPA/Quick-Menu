using System;
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
            fileOperations.CreateFile();
            fileOperations.WriteToFile("#" + _index + "/" + listBoxProducts.Text, true);

            this.Close();
        }
    }
}
