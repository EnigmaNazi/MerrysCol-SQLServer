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
    public partial class FRM_SUPPLIER : Form
    {
        SqlConnection con = new SqlConnection("Server=CYBER7;Database=merryscol;Integrated Security=true;");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        SqlDataReader rd;

        private void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from tbl_supplier", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void otomatis()
        {
            long hitung;
            string urut;

            con.Open();
            cmd = new SqlCommand("select kode_supplier from tbl_supplier where kode_supplier in(select max(kode_supplier) from tbl_supplier) order by kode_supplier desc", con);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                hitung = Convert.ToInt64(rd[0].ToString().Substring(rd["kode_supplier"].ToString().Length - 4, 4)) + 1;

                string joinstr = "0000" + hitung;



                urut = "SPL" + joinstr.Substring(joinstr.Length - 4, 4);

            }
            else
            {
                urut = "SPL0001";
            }
            rd.Close();
            txt_kode_supplier.Text = urut;
            con.Close();

        }

        private void cleartext()
        {
            txt_kode_supplier.Text = "";
            txt_nama_supplier.Text = "";
            txt_telp_supplier.Text = "";
            txt_alamat_supplier.Text = "";
            txt_kode_supplier.Enabled = false;
        }

        public FRM_SUPPLIER()
        {
            InitializeComponent();
            cleartext();
            DisplayData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            otomatis();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txt_kode_supplier.Text != "" && txt_nama_supplier.Text != "" && txt_telp_supplier.Text != "" && txt_alamat_supplier.Text != "")
            {
                cmd = new SqlCommand("insert into tbl_supplier(kode_supplier,nama_supplier,telp_supplier,alamat_supplier) values(@kode_supplier,@nama_supplier,@telp_supplier,@alamat_supplier)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@kode_supplier", txt_kode_supplier.Text);
                cmd.Parameters.AddWithValue("@nama_supplier", txt_nama_supplier.Text);
                cmd.Parameters.AddWithValue("@telp_supplier", txt_telp_supplier.Text);
                cmd.Parameters.AddWithValue("@alamat_supplier", txt_alamat_supplier.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Data berhasil disimpan");
                DisplayData();
                cleartext();
            }
            else
            {
                MessageBox.Show("gagal simpan");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txt_kode_supplier.Text != "" && txt_nama_supplier.Text != "" && txt_telp_supplier.Text != "" && txt_alamat_supplier.Text != "")
            {
                cmd = new SqlCommand("update tbl_pelanggan set nama_pelanggan=@nama_pelanggan,@telp_pelanggan,@alamat_pelanggan where kode_pelanggan=@kode_pelanggan)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@kode_supplier", txt_kode_supplier.Text);
                cmd.Parameters.AddWithValue("@nama_supplier", txt_nama_supplier.Text);
                cmd.Parameters.AddWithValue("@telp_supplier", txt_telp_supplier.Text);
                cmd.Parameters.AddWithValue("@alamat_supplier", txt_alamat_supplier.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Data berhasil disimpan");
                DisplayData();
                cleartext();
            }
            else
            {
                MessageBox.Show("gagal simpan");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (txt_kode_supplier.Text != "")
            {
                cmd = new SqlCommand("delete tbl_supplier where kode_supplier=@kode_supplier", con);
                con.Open();
                cmd.Parameters.AddWithValue("@kode_supplier", txt_kode_supplier.Text);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("berhasil hapus");
                DisplayData();
                cleartext();
            }
            else
            {
                MessageBox.Show("gagal hapus");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_kode_supplier.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txt_nama_supplier.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txt_telp_supplier.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txt_alamat_supplier.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txt_kode_supplier.Enabled = false;
        }
    }
}
