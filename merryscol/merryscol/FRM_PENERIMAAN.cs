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
    public partial class FRM_PENERIMAAN : Form
    {
        SqlConnection con = new SqlConnection("Server=CYBER7;Database=merryscol;Integrated Security=true;");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        SqlDataReader rd;

        private void DisplayData()
        {
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select * from tbl_penerimaan", con);
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
            cmd = new SqlCommand("select no_penerimaan from tbl_penerimaan_detail where no_penerimaan in(select max(no_penerimaan) from tbl_penerimaan_detail) order by no_penerimaan desc", con);
            rd = cmd.ExecuteReader();
            rd.Read();
            if (rd.HasRows)
            {
                hitung = Convert.ToInt64(rd[0].ToString().Substring(rd["no_penerimaan"].ToString().Length - 4, 4)) + 1;

                string joinstr = "0000" + hitung;



                urut = "PNM" + joinstr.Substring(joinstr.Length - 4, 4);

            }
            else
            {
                urut = "PNM0001";
            }
            rd.Close();
            txt_no_penerimaan.Text = urut;
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
            txt_jumlah_terima.Text = "";
            txt_keterangan.Text = "";
            txt_no_penerimaan.Enabled = false;
            txt_kode_supplier.Enabled = false;
            txt_kode_barang.Enabled = false;
            txt_stock.Enabled = false;
        }

        public FRM_PENERIMAAN()
        {
            InitializeComponent();
            DisplayData();
            DisplayDataSupplier();
            DisplayDataBarang();
            cleartext();
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker1.Enabled = false;
            txt_no_penerimaan.Enabled = false;
            cleartext();
            otomatis();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            cmd = new SqlCommand("insert into tbl_penerimaan_detail select * from tbl_penerimaan where no_penerimaan=@no_penerimaan", con);
            con.Open();
            cmd.Parameters.AddWithValue("@no_penerimaan", txt_no_penerimaan.Text);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Berhasil simpan");
            cmd = new SqlCommand("delete tbl_penerimaan", con);
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
            int stock, jumlah_terima, stock_akhir;
            stock = int.Parse(txt_stock.Text);
            jumlah_terima = int.Parse(txt_jumlah_terima.Text);
            stock_akhir = stock + jumlah_terima;

            if (txt_no_penerimaan.Text != "" && txt_kode_barang.Text != "" && txt_kode_supplier.Text != "" && txt_jumlah_terima.Text != "" && txt_keterangan.Text != "")
            {
                cmd = new SqlCommand("update tbl_barang set stock=@stock_akhir where kode_barang=@kode_barang", con);
                con.Open();
                cmd.Parameters.AddWithValue("@stock_akhir", stock_akhir);
                cmd.Parameters.AddWithValue("@kode_barang", txt_kode_barang.Text);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            else
            {
                MessageBox.Show("gagal simpan");
            }
            if (txt_no_penerimaan.Text != "" && txt_kode_barang.Text != "" && txt_kode_supplier.Text != "" && txt_jumlah_terima.Text != "" && txt_keterangan.Text != "")
            {
                cmd = new SqlCommand("insert into tbl_penerimaan(no_penerimaan,tgl_penerimaan,kode_supplier,kode_barang,jumlah_terima,keterangan) values(@no_penerimaan,@tgl_penerimaan,@kode_supplier,@kode_barang,@jumlah_terima,@keterangan)", con);
                con.Open();
                cmd.Parameters.AddWithValue("@no_penerimaan", txt_no_penerimaan.Text);
                cmd.Parameters.Add("@tgl_penerimaan", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                cmd.Parameters.AddWithValue("@kode_supplier", txt_kode_supplier.Text);
                cmd.Parameters.AddWithValue("@kode_barang", txt_kode_barang.Text);
                cmd.Parameters.AddWithValue("@jumlah_terima", txt_jumlah_terima.Text);
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
                cmd = new SqlCommand("delete tbl_penerimaan where no_penerimaan=@no_penerimaan and kode_supplier=@kode_supplier and kode_barang=@kode_barang", con);
                con.Open();
                cmd.Parameters.AddWithValue("@no_penerimaan", txt_no_penerimaan.Text);
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
            txt_no_penerimaan.Text = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            dg_supplier_CellContentClick(sender, e);
            dg_barang_CellContentClick(sender, e);
            txt_kode_supplier.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txt_kode_barang.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txt_jumlah_terima.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txt_keterangan.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            DisplayData();
        }
    }
}
