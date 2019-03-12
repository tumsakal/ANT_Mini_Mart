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
        SqlConnection conn = new SqlConnection("Server=.;Database=Mini_Mart;Integrated Security=true;");
        public FormCustomer()
        {
            InitializeComponent();
        }
        int GetAutoNumber(string table, string column)
        {
            string sql = $"SELECT MAX({column}) FROM {table}";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                object max = cmd.ExecuteScalar();
                if(max != DBNull.Value)
                    return Convert.ToInt32(max) + 1;
                else
                    return 1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
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
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"INSERT INTO Customer(Name, Gender, Phone, Email, Address, RegisterDate, IsActive) VALUES(@name, N'{cboGender.Text}', '{txtPhone.Text.Trim()}', '{txtEmail.Text.Trim()}', N'{txtAddress.Text.Trim()}', @dob, { Convert.ToInt32( chkActive.Checked) })";

            try
            {
                /*
                SqlParameter sp_name = new SqlParameter();
                sp_name.ParameterName = "@name";
                sp_name.SqlDbType = SqlDbType.NVarChar;
                sp_name.Size = 255;
                sp_name.Value = txtName.Text.Trim();
                cmd.Parameters.Add(sp_name);
                */
                cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                cmd.Parameters.AddWithValue("@dob", dtpRegisterDate.Value);
                int affected_row = cmd.ExecuteNonQuery();
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
            txtCustomerID.Text = this.GetAutoNumber("Customer", "ID").ToString();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT * FROM Customer;";
            try
            {
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
                }
                customer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormCustomer_Load(object sender, EventArgs e)
        {
            conn.Open();
            txtCustomerID.Text = this.GetAutoNumber("Customer", "ID").ToString();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right && e.RowIndex >= 0)
            {
                int customer_id =Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells[0].Value);
                contextMenuStrip1.Tag = customer_id;
                contextMenuStrip1.Show(Cursor.Position);
            }
        }
        private void detetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32( contextMenuStrip1.Tag);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "DELETE FROM Customer WHERE ID = " + id;
            try
            {
                cmd.ExecuteNonQuery();
                btnLoad.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(contextMenuStrip1.Tag);
            //get row
            DataGridViewRow row = null;
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if(Convert.ToInt32( dataGridView1.Rows[i].Cells[0].Value) == id)
                {
                    row = dataGridView1.Rows[i];
                    break;
                }
            }
            //
            txtCustomerID.Text = row.Cells[0].Value.ToString();
            txtName.Text = row.Cells[1].Value.ToString();
            cboGender.SelectedItem = row.Cells[2].Value.ToString();
            txtPhone.Text = row.Cells[3].Value.ToString();
            txtEmail.Text = row.Cells[4].Value.ToString();
            txtAddress.Text = row.Cells[5].Value.ToString();
            dtpRegisterDate.Value = Convert.ToDateTime(row.Cells[6].Value);
            chkActive.Checked = Convert.ToBoolean(row.Cells[7].Value);

            txtCustomerID.ReadOnly = true;
            txtName.Focus();
            btnSave.Click -= btnSave_Click;//dettach
            btnSave.Click += BtnSaveChange_Click;
        }

        private void BtnSaveChange_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txtCustomerID.Text.Trim());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "UPDATE Customer SET " +
                              $"Name = N'{txtName.Text.Trim()}'," +
                              $"Gender = N'{cboGender.Text}'," +
                              $"Phone ='{txtPhone.Text.Trim()}'," +
                              $"Email = '{ txtEmail.Text.Trim()}'," +
                              $"Address = N'{txtAddress.Text.Trim()}'," +
                              $"RegisterDate = '{dtpRegisterDate.Value.ToShortDateString()}'," +
                              $"IsActive = {Convert.ToInt32(chkActive.Checked)}" +
                              $"WHERE ID = {id}";
            try
            {
                cmd.ExecuteNonQuery();
                btnLoad.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //
            btnSave.Click -= BtnSaveChange_Click;//dettach update
            btnSave.Click += btnSave_Click;//attach insert
        }

        private void FormCustomer_FormClosed(object sender, FormClosedEventArgs e)
        {
            conn.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = $"SELECT * FROM Customer WHERE ID = {txtSearch.Text.Trim()} OR Name LIKE '%{txtSearch.Text.Trim()}%';";
            try
            {
                SqlDataReader customer = cmd.ExecuteReader();
                if (customer.HasRows == true)
                {
                    while (customer.Read() == true)
                    {
                        //int id = customer[0];
                        int id = Convert.ToInt32(customer["ID"]);
                        string name = customer["Name"].ToString();
                        string gender = customer["Gender"].ToString();
                        string phone = customer["Phone"].ToString();
                        string email = customer["Email"].ToString();
                        string address = customer["Address"].ToString();
                        DateTime register_date = Convert.ToDateTime(customer["RegisterDate"]);
                        bool isActive = Convert.ToBoolean(customer["IsActive"]);
                        dataGridView1.Rows.Add(id, name, gender, phone, email, address, register_date, isActive);
                    }
                    
                }
                customer.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}