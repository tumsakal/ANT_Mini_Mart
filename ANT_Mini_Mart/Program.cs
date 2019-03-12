using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace ANT_Mini_Mart
{
    static class Program
    {
        public static SqlConnection Connection = new SqlConnection("Server=.;Database=Mini_Mart;Integrated Security=true;");
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Connection.Open();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new HumanResource.FormStaff());
            try
            {
                Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
