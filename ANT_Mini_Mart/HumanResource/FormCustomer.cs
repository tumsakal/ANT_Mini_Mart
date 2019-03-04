using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
    }
}
