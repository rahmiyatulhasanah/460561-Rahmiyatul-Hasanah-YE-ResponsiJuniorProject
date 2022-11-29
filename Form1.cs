using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Npgsql;


namespace _460561_Responsi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Menghubungkan C# dengan PostgreSQL
        public NpgsqlConnection conn;
        string connstring = "Host = localhost; port=2022; Username = postgres; Password = informatika; Database=responsi_rahmi";
        public DataTable dt;
        public static NpgsqlCommand cmd;
        private string sql = null;
        private DataGridViewRow r;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
        }

        private void btn_insert_Click(object sender, EventArgs e)
        {
            try
            {
                // Membuka koneksi dengan PostgreSQL
                conn.Open();

                // Command SQL yang akan dijalankan oleh PostgreSQL
                sql = @"SELECT * FROM kr_insert(:_nama_karyawan, :_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                //cmd.Parameters.AddWithValue("_id_karyawan", tb_idk.Text);
                cmd.Parameters.AddWithValue("_nama_karyawan", tb_namak.Text);
                cmd.Parameters.AddWithValue("_id_dep", tb_id_dep.Text);

                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Karyawan berhasil diinputkan", "Well Done!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btn_load.PerformClick();
                    tb_idk.Text = tb_namak.Text = tb_id_dep.Text = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Insert GAGAL!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {

            if (r == null)
            {
                MessageBox.Show("Mohon pilih baris data yang akan diupdate", "Good!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try
            {
                // Membuka koneksi dengan PostgreSQL
                conn.Open();

                // Command SQL yang akan dijalankan oleh PostgreSQL
                sql = @"SELECT * FROM kr_update(:_id_karyawan,:_nama_karyawan,:_id_dep)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                cmd.Parameters.AddWithValue("_nama_karyawan", tb_namak.Text);
                cmd.Parameters.AddWithValue("_id_dep", tb_id_dep.Text);

                if ((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Data Users berhasil diperbarui", "Well Done!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    conn.Close();
                    btn_load.PerformClick();
                    tb_namak.Text = tb_id_dep.Text = null;
                    r = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "Update GAGAL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (r == null)
            {
                MessageBox.Show("Mohon pilih baris data yang akan dihapus", "Warning!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Apakah benar anda ingin menghapus data " + r.Cells["_name"].Value.ToString() + " ?",
               "Hapus data terkonfirmasi", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
               MessageBoxDefaultButton.Button1) == DialogResult.Yes)

                try
                {
                    conn.Open();

                    sql = @"SELECT * FROM kr_delete(:_id_karyawan)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id_karyawan", r.Cells["_id_karyawan"].Value.ToString());
                    if ((int)cmd.ExecuteScalar() == 1)
                    {
                        MessageBox.Show("Data Karyawan berhasil dihapus", "Well Done!",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        conn.Close();
                        btn_load.PerformClick();
                        tb_namak.Text = tb_id_dep.Text = null;
                        r = null;
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Update FAIL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
        }

        private void btn_load_Click(object sender, EventArgs e)
        {
            try
            {
                conn.Open();
                dgvData.DataSource = null;
                sql = @"SELECT * FROM kr_select()";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                NpgsqlDataReader rd = cmd.ExecuteReader();
                dt.Load(rd);
                dgvData.DataSource = dt;

                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error:" + ex.Message, "GAGAL!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                r = dgvData.Rows[e.RowIndex];
                tb_namak.Text = r.Cells["_name_karyawan"].Value.ToString();
                tb_id_dep.Text = r.Cells["_id_dep"].Value.ToString();
            }
        }
    }
}
