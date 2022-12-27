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

namespace DeThuNhat
{
    public partial class Form1 : Form
    {
        SqlConnection connection;
        SqlCommand command;
        string str = @"Data Source=(local);Initial Catalog=DuLieu;Integrated Security=True";
        SqlDataAdapter adapter = new SqlDataAdapter();
        DataTable table = new DataTable();

        void loatData()
        {
            command = connection.CreateCommand();
            command.CommandText = "select * from CongViec";
            adapter.SelectCommand = command;
            table.Clear();
            adapter.Fill(table);
            dataGridView1.DataSource = table;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(str);
            connection.Open();
            loatData();
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow rowS = dataGridView1.Rows[e.RowIndex];
                textBox3.Text = rowS.Cells[0].Value.ToString();
                textBox1.Text = rowS.Cells[1].Value.ToString();
                textBox2.Text = rowS.Cells[2].Value.ToString();
            }
        }
        private int kiemTraThongTin()
        {
            if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0)
                return 0;
            return 1;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (kiemTraThongTin() == 1)
            {
                command = connection.CreateCommand();
                command.CommandText = "insert into CongViec values('" + textBox1.Text + "','" + textBox2.Text + "')";
                command.ExecuteNonQuery();
                loatData();
                MessageBox.Show("Thêm thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (kiemTraThongTin() == 1)
            {
                command = connection.CreateCommand();
                command.CommandText = "update CongViec set TenCongViec='" + textBox1.Text + "',LuongThang=" + textBox2.Text + " where MaCongViec='" + textBox3.Text + "'";
                command.ExecuteNonQuery();
                loatData();
                MessageBox.Show("Sửa thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (kiemTraThongTin() == 1)
            {
                command = connection.CreateCommand();
                command.CommandText = "delete from CongViec where MaCongViec=" + textBox3.Text;
                command.ExecuteNonQuery();
                loatData(); ;
                MessageBox.Show("Xóa thành công !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult f = MessageBox.Show("Bạn có muốn thoát chương trình không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (f == DialogResult.Yes)
            {
                this.Close();
                Application.Exit();
            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((!Char.IsDigit(e.KeyChar)) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
