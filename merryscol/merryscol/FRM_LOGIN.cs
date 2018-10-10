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

namespace merryscol
{
    public partial class FRM_LOGIN : Form
    {
        SqlConnection con = new SqlConnection("Server=CYBER7;Database=merryscol;Integrated Security=true;");

        public FRM_LOGIN()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand com = new SqlCommand();
            com.Connection = con;
            com.CommandType = CommandType.Text;
            string strSQL = " select * from tbl_login where username ='" + txt_username.Text
                + "' and password='" + txt_password.Text + "'";

            com.CommandText = strSQL;
            SqlDataReader datareader = com.ExecuteReader();
            if (datareader.Read())
            {
                MessageBox.Show("Login berhasil", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                FRM_MENU_UTAMA menu = new FRM_MENU_UTAMA();
                menu.ShowDialog();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Username dan Password Salah", "Informasi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_username.Text = "";
                txt_password.Text = "";
                txt_username.Focus();
            }
            con.Close();
        }

        private void btn_batal_Click(object sender, EventArgs e)
        {
            txt_username.Text = "";
            txt_password.Text = "";
        }
    }
}
