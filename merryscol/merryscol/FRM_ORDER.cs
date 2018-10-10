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
    public partial class FRM_ORDER : Form
    {
        SqlConnection con = new SqlConnection("Server=CYBER7;Database=merryscol;Integrated Security=true;");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        SqlDataReader rd;

        private void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from tbl_order", con);
            adapt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void DisplayDataSupplier()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from tbl_supplier", con);
            adapt.Fill(dt);
            dg_supplier.DataSource = dt;
            con.Close();
        }

        private void DisplayDataBarang()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from tbl_barang", con);
            adapt.Fill(dt);
            dg_barang.DataSource = dt;
            con.Close();
        }

        private void otomatis()
        {
            long hitung;
            string urut;

            con.Open();
            cmd = new SqlCommand("select no_order from tbl_order_detail where no_order in(select max(no_order) from tbl_order_detail) order by no_order desc", con);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                hitung = Convert.ToInt64(rd[0].ToString().Substring(rd["no_order"].ToString().Length - 4, 4)) + 1;

                string joinstr = "0000" + hitung;



                urut = "ORD" + joinstr.Substring(joinstr.Length - 4, 4);

            }
            else
            {
                urut = "ORD0001";
            }
            rd.Close();
            txt_no_order.Text = urut;
            con.Close();
        }

        private void cleartext()
        {
            txt_kode_supplier.Text = "";
            txt_nama_supplier.Text = "";
            txt_no_telp.Text = "";
            txt_alamat.Text = "";
            txt_kode_barang.Text = "";
            txt_nama_barang.Text = "";
            txt_style.Text = "";
            txt_warna.Text = "";
            txt_satuan.Text = "";
            txt_harga.Text = "0";
            txt_stock.Text = "0";
            txt_jumlah_order.Text = "";
            txt_keterangan.Text = "";
            txt_no_order.Enabled = false;
            txt_kode_supplier.Enabled = false;
            txt_kode_barang.Enabled = false;
            txt_stock.Enabled = false;
        }

        public FRM_ORDER()
        {
            InitializeComponent();
            DisplayData();
            DisplayDataSupplier();
            DisplayDataBarang();
            cleartext();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker1.Enabled = false;
            txt_no_order.Enabled = false;
            cleartext();
            otomatis();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand("insert into tbl_order_detail select * from tbl_order where no_order=@no_order", con);
            con.Open();
            cmd.Parameters.AddWithValue("@no_order", txt_no_order.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Berhasil simpan");
            cmd = new SqlCommand("delete tbl_order", con);
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
            DisplayData();
            cleartext();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (txt_no_order.Text != "" && txt_kode_barang.Text != "" && txt_kode_supplier.Text != "" && txt_jumlah_order.Text != "" && txt_keterangan.Text != "")
            {
                cmd = new SqlCommand("insert into tbl_order(no_order,tgl_order,kode_supplier,kode_barang,jumlah_order,keterangan) values(@no_order,@tgl_order,@kode_supplier,@kode_barang,@jumlah_order,@keterangan)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@no_order", txt_no_order.Text);
                cmd.Parameters.Add("@tgl_order", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                cmd.Parameters.AddWithValue("@kode_supplier", txt_kode_supplier.Text);
                cmd.Parameters.AddWithValue("@kode_barang", txt_kode_barang.Text);
                cmd.Parameters.AddWithValue("@jumlah_order", txt_jumlah_order.Text);
                cmd.Parameters.AddWithValue("@keterangan", txt_keterangan.Text);
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

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (txt_nama_barang.Text != "")
            {
                cmd = new SqlCommand("delete tbl_order where no_order=@no_order and kode_supplier=@kode_supplier and kode_barang=@kode_barang", con);
                con.Open();
                cmd.Parameters.AddWithValue("@no_order", txt_no_order.Text);
                cmd.Parameters.AddWithValue("@kode_supplier", txt_kode_supplier.Text);
                cmd.Parameters.AddWithValue("@kode_barang", txt_kode_barang.Text);
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

        private void pilih_supplier_Click(object sender, EventArgs e)
        {
            group_cari_supplier.Visible = true;
        }

        private void pilih_barang_Click(object sender, EventArgs e)
        {
            group_cari_barang.Visible = true;
        }

        private void dg_supplier_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_kode_supplier.Text = dg_supplier.Rows[e.RowIndex].Cells[0].Value.ToString();
            txt_nama_supplier.Text = dg_supplier.Rows[e.RowIndex].Cells[1].Value.ToString();
            txt_no_telp.Text = dg_supplier.Rows[e.RowIndex].Cells[2].Value.ToString();
            txt_alamat.Text = dg_supplier.Rows[e.RowIndex].Cells[3].Value.ToString();
            group_cari_supplier.Visible = false;
        }

        private void dg_barang_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_kode_barang.Text = dg_barang.Rows[e.RowIndex].Cells[0].Value.ToString();
            txt_nama_barang.Text = dg_barang.Rows[e.RowIndex].Cells[1].Value.ToString();
            txt_style.Text = dg_barang.Rows[e.RowIndex].Cells[2].Value.ToString();
            txt_warna.Text = dg_barang.Rows[e.RowIndex].Cells[3].Value.ToString();
            txt_satuan.Text = dg_barang.Rows[e.RowIndex].Cells[4].Value.ToString();
            txt_harga.Text = dg_barang.Rows[e.RowIndex].Cells[5].Value.ToString();
            txt_stock.Text = dg_barang.Rows[e.RowIndex].Cells[6].Value.ToString();
            group_cari_barang.Visible = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_no_order.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            dg_supplier_CellContentClick(sender, e);
            dg_barang_CellContentClick(sender, e);
            txt_kode_supplier.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txt_kode_barang.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txt_jumlah_order.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txt_keterangan.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            DisplayData();
        }
    }
}
