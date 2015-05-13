using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProductConfigaration
{
    public partial class ProductInfoUI : Form
    {
        public ProductInfoUI()
        {
            InitializeComponent();
        }

        private string connectionString = @"Data Source=PC-301-13\SQLEXPRESS;Initial Catalog=ProductInfoMyBD;Integrated Security=True";

        List<Products> myProductses= new List<Products>(); 
        private void saveButtton_Click(object sender, EventArgs e)
        {
            Products myProducts= new Products();

            myProducts.productCode = productcodeTextCode.Text;
            myProducts.description = descriptionTextBox.Text;
            myProducts.quantity = double.Parse(quantityTextBox.Text);

            if (myProducts.productCode.Length < 3)
            {
                MessageBox.Show("Product code must be at least 3 character");
                return;
            }

            SqlConnection connection= new SqlConnection(connectionString);
            
            string query = "INSERT INTO Products VALUES('" + myProducts.productCode + "','" + myProducts.description +
                           "','" + myProducts.quantity + "')";

            SqlCommand command = new SqlCommand(query,connection);

            connection.Open();
            int rowEffect=command.ExecuteNonQuery();
            connection.Close();

            if (rowEffect > 0)
            {
                MessageBox.Show("Save Successful!");
            }
            else
            {
                MessageBox.Show("Save Failed!");
            }

            productcodeTextCode.Clear();
            descriptionTextBox.Clear();
            quantityTextBox.Clear();

        }

        public void ShowAllInfo()
        {
            myProductses.Clear();

            double totalQuantity=0;

            SqlConnection connection = new SqlConnection(connectionString);

            string query = "SELECT * FROM Products";

            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                Products products = new Products();

                products.productCode = reader["productCode"].ToString();
                products.description = reader["ProductDescription"].ToString();
                totalQuantity+=products.quantity = double.Parse(reader["quantity"].ToString());

                myProductses.Add(products);
            }
            reader.Close();
            connection.Close();

            productListView.Items.Clear();
            foreach (var show in myProductses)
            {
                ListViewItem items= new ListViewItem(show.productCode);

                items.SubItems.Add(show.description);
                items.SubItems.Add(show.quantity.ToString());

                productListView.Items.Add(items);
            }

            totalquantittTextBox.Text = totalQuantity + "";

        }

        private void showButton_Click(object sender, EventArgs e)
        {
            ShowAllInfo();
        }
        

    }
}
