using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ANT_Mini_Mart.HumanResource
{
    public partial class FormNew : Form
    {
        public FormNew()
        {
            InitializeComponent();
        }

        private void btnsave_Click(object sender, EventArgs e)
        {
            string sql = string.Empty;
            if(this.Tag.ToString() == "INSERT")
            {
                sql = $@"INSERT INTO Staff VALUES(
                         '{txtstaffID.Text}', '{txtNID.Text.Trim()}',
                         '{txtBranchID.Text.Trim()}', {txtPositionID.Text.Trim()},
                         '{txtEngName.Text.Trim()}', N'{txtKhName.Text.Trim ()}',
                         N'{cboGender.Text.Trim()}', @dob,
                         N'{txtPOB.Text.Trim()}', N'{txtCurrentAddress.Text.Trim()}',
                         '{txtPhone.Text.Trim()}', '{txtEmail.Text.Trim()}',
                          @img, {Convert.ToInt32(chkWorking.Checked)}
                        )";
            }
            else if(this.Tag.ToString() == "UPDATE")
            {
                sql = $@"UPDATE Staff SET 
                            NID = '{txtNID.Text.Trim()}',
                            BrandchID = '{txtBranchID.Text.Trim()}',//varchar
                            PositionID =  {txtPositionID.Text.Trim()},//int
                            EngName = '{txtEngName.Text.Trim()}',
                            KhName = '{txtKhName.Text.Trim()}',
                            Gender = N'{cboGender.Text}',
                            DOB = @dob,
                            POB = N'{txtPOB.Text.Trim()}',
                            CurrentAddress = N'{txtCurrentAddress.Text.Trim()}',
                            Phone = '{txtPhone.Text.Trim()}',
                            Email = '{txtEmail.Text.Trim()}',
                            Image = @img,
                            Working = {Convert.ToInt32(chkWorking.Checked)}
                            WHERE ID = '{txtstaffID.Text.Trim()}'
                        ";
            }
            //----------------------------------------------------------
            SqlCommand cmd = new SqlCommand(sql, Program.Connection);
            cmd.Parameters.AddWithValue("@dob", dtpDOB.Value);
            //Convert Image -> byte[]
            if (pictureBox1.Image != null)
            {
                MemoryStream memory = new MemoryStream();
                pictureBox1.Image.Save(memory, pictureBox1.Image.RawFormat);
                cmd.Parameters.AddWithValue("@img", memory.ToArray());
            }
            else
            {
                cmd.Parameters.AddWithValue("@img", new byte[0]);//Column 'Image' allow null
            }
            try
            {
                cmd.ExecuteNonQuery();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
