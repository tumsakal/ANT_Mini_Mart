using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace ANT_Mini_Mart.HumanResource
{
    public partial class FormStaff : Form
    {
        public FormStaff()
        {
            InitializeComponent();
        }
        void LoadData()
        {
            //string sql = @"SELECT S.ID, NID,
            //B.[Name] AS BranchName,
            //P.[Name] AS PositionName,
            //EngName, KhName, Gender, DOB, POB,
            //CurrentAddress, S.Phone, S.Email, Working
            //FROM Staff AS S INNER JOIN Branch AS B ON S.BranchID = B.ID
            //INNER JOIN Position AS P ON S.PositionID = P.ID";
            string sql = "SELECT * FROM v_staff_info";
            SqlCommand cmd = new SqlCommand(sql, Program.Connection);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                if(reader.HasRows)
                {
                    while (reader.Read() == true)
                    {
                        string id = reader["ID"].ToString();
                        string nid = reader["NID"].ToString();
                        string branch_name = reader["BranchName"].ToString();
                        string position_name = reader["PositionName"].ToString();
                        string eng_name = reader["EngName"].ToString();
                        string kh_name = reader["KhName"].ToString();
                        string gender = reader["Gender"].ToString();
                        DateTime dob = Convert.ToDateTime(reader["DOB"]);
                        string pob = reader["POB"].ToString();
                        string current_address = reader["CurrentAddress"].ToString();
                        string phone = reader["Phone"].ToString();
                        string email = reader["Email"].ToString();
                        bool working = Convert.ToBoolean(reader["Working"]);
                        //convert varbinary -> Image
                        //byte[] raw = (byte[])reader["Image"];
                        //System.IO.MemoryStream memory = new System.IO.MemoryStream(raw);
                        //Image img = Image.FromStream(memory);
                        //----------------Add to DataGridView---------------------------
                        dataGridView1.Rows.Add(id, nid, branch_name, position_name, 
                            eng_name, kh_name, gender ,dob, pob, current_address,
                            phone, email, working);
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FormStaff_Load(object sender, EventArgs e)
        {
            this.LoadData();
        }
    }
}
