using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;//sql data provider
namespace ANT_Mini_Mart.HumanResource
{
    public partial class FormCustomer : Form
    {
        public FormCustomer()
        {
            InitializeComponent();
        }
        private void ClearForm()
        {
            foreach (Control item in this.Controls)
            {
                switch (item)
                {
                    case TextBoxBase txt: txt.Clear(); break;
                }
            }
            cboGender.SelectedIndex = -1;
            dtpRegisterDate.Value = DateTime.Now;
            chkActive.Checked = false;
            txtCustomerID.Focus();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = ".";//server location
            builder.InitialCatalog = "Mini_Mart";//database name
            builder.IntegratedSecurity = true;// window authentication

            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = builder.ToString();//"Server=.;Database=Mini_Mart;Integrated Security=True;";

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"INSERT INTO Customer(Name, Gender, Phone, Email, Address, RegisterDate, IsActive) VALUES(N'{txtName.Text.Trim()}', N'{cboGender.Text}', '{txtPhone.Text.Trim()}', '{txtEmail.Text.Trim()}', N'{txtAddress.Text.Trim()}', '{dtpRegisterDate.Value.ToShortDateString()}', { Convert.ToInt32( chkActive.Checked) })";

            try
            {
                conn.Open();
                int affected_row = cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show($"{affected_row} row is inserted.");
                
                //reaload/refresh data
                //btnLoad_Click(null, null);
                //btnLoad.PerformClick();
                dataGridView1.Rows.Add(Convert.ToInt32( txtCustomerID.Text), txtName.Text, cboGender.Text, txtPhone.Text,
                                        txtEmail.Text, txtAddress.Text, dtpRegisterDate.Value,
                                        chkActive.Checked);
                this.ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Server=.;Database=Mini_Mart;Integrated Security=True;";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Customer;";
            try
            {
                conn.Open();
                SqlDataReader customer = cmd.ExecuteReader();
                if (customer.HasRows == true)
                {
                    while (customer.Read() == true)
                    {
                        //int id = customer[0];
                        int id = Convert.ToInt32( customer["ID"]);
                        string name = customer["Name"].ToString();
                        string gender = customer["Gender"].ToString();
                        string phone = customer["Phone"].ToString();
                        string email = customer["Email"].ToString();
                        string address = customer["Address"].ToString();
                        DateTime register_date = Convert.ToDateTime(customer["RegisterDate"]);
                        bool isActive = Convert.ToBoolean(customer["IsActive"]);
                        dataGridView1.Rows.Add(id, name, gender, phone, email, address, register_date, isActive);
                    }
                    customer.Close();
                }
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormCustomer_Load(object sender, EventArgs e)
        {

        }
    }
}
