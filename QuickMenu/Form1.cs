using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickMenu
{
    public partial class Form1 : Form
    {
        private readonly FileOperations<string> fileOperations = new FileOperations<string>("QuickMenu.txt");

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                handleParam.ExStyle |= 0x02000000; //WS_EX_COMPOSITED
                return handleParam;
            }
        }

        public Form1()
        {
            InitializeComponent();

            this.ResizeBegin += (s, e) =>
            {
                this.Opacity = 0.50;
                this.SuspendLayout();
            };

            this.ResizeEnd += (s, e) =>
            {
                this.Opacity = 1;
                this.ResumeLayout(true);
            };
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CreateQuickMenu(GetQuickMenu());
        }

        public List<Product> GetQuickMenu()
        {
            List<Product> products = new List<Product>();

            string file = fileOperations.ReadFile();
            string[] menus = file.Split('#');
            foreach (var menu in menus)
            {
                if (!string.IsNullOrEmpty(menu))
                {
                    string[] properties = menu.Split('/');

                    Product product = new Product
                    {
                        Index = Convert.ToInt32(properties[0]),
                        Name = properties[1],
                        Price = Convert.ToDouble(properties[2]),
                        BackColor = "15,15,15",
                        ForeColor = "15,15,15",
                        FontSize = 15
                    };

                    products.Add(product);
                }
            }

            return products;
        }

        public void CreateQuickMenu(List<Product> products)
        {
            tableLayoutPanelProducts.SuspendLayout();

            int columnCount = 0, rowCount = 0, productIndex = 0, productCount = 20;

            int c = 1;
            for (int i = 1; i <= productCount; i++)
            {
                for (int j = productCount; j >= i; j--)
                {
                    c = i * j;
                    if (c == productCount)
                    {
                        rowCount = i;
                        columnCount = j;
                        break;
                    }
                    break;
                }
                break;
            }

            tableLayoutPanelProducts.Controls.Clear();
            tableLayoutPanelProducts.ColumnStyles.Clear();
            tableLayoutPanelProducts.RowStyles.Clear();
            tableLayoutPanelProducts.RowCount = 0;
            tableLayoutPanelProducts.ColumnCount = 0;
            for (int column = 1; column <= 5; column++)
            {
                tableLayoutPanelProducts.ColumnCount = column;
                tableLayoutPanelProducts.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

                for (int row = 1; row <= rowCount; row++)
                {
                    tableLayoutPanelProducts.RowCount = row;
                    tableLayoutPanelProducts.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                }
            }
            tableLayoutPanelProducts.RowCount++;
            tableLayoutPanelProducts.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            tableLayoutPanelProducts.RowCount++;
            tableLayoutPanelProducts.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            tableLayoutPanelProducts.RowCount++;
            tableLayoutPanelProducts.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            for (int i = 0; i < columnCount * rowCount; i++)
            {
                if (productIndex < productCount)
                {
                    var product = products.Where(x => x.Index == productIndex).FirstOrDefault();

                    ProductUserControl productUserControl = new ProductUserControl(product == null ? null : product, productIndex, true)
                    {
                        Dock = DockStyle.Fill
                    };

                    productUserControl.ProductClick += ProductUserControl_Click;
                    productUserControl.ProductRightClick += ProductUserControl_RightClick;
                    productUserControl.SelectProductClick += ProductUserControl_SelectProductClick;

                    tableLayoutPanelProducts.Controls.Add(productUserControl);

                    productIndex++;
                }
            }

            tableLayoutPanelProducts.ResumeLayout();
        }

        private void ProductUserControl_RightClick(object sender, EventArgs e)
        {
            ProductUserControl productUserControl = (ProductUserControl)sender;

            string file = fileOperations.ReadFile();
            string[] menus = file.Split('#');
            foreach (var menu in menus)
            {
                if (!string.IsNullOrEmpty(menu))
                {
                    string[] properties = menu.Split('/');

                    if (properties[1] == productUserControl._product.Name)
                    {
                        fileOperations.FindAndRemoveLine("#" + menu);
                    }
                }
            }

            CreateQuickMenu(GetQuickMenu());
        }

        private void ProductUserControl_SelectProductClick(object sender, EventArgs e)
        {
            ProductUserControl productUserControl = (ProductUserControl)sender;

            QuickMenuProductForm quickMenuProductForm = new QuickMenuProductForm(productUserControl._index);
            quickMenuProductForm.ShowDialog();

            CreateQuickMenu(GetQuickMenu());
        }

        private void ProductUserControl_Click(object sender, EventArgs e)
        {

        }
    }
}
