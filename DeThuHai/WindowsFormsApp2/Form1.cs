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
using cExcel = Microsoft.Office.Interop.Excel;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        List<Hang> hangsGlobal  = new List<Hang>();
        public SqlConnection ketnoi()
        {
            SqlConnection cnn;
            string str = @"Data Source=(local);Initial Catalog=DuLieu;Integrated Security=True";
            cnn = new SqlConnection(str);
            return cnn;
        }
        void LoadData()
        {
            List<Hang> hangs = new List<Hang>();
            SqlConnection cnn = ketnoi();
            cnn.Open();
            string sql = "select Mahang, Tenhang, Tenchatlieu,Dongianhap, Dongiaban, Soluong from tblHang h, tblChatlieu c where h.Machatlieu = c.MaChatlieu";
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                Hang hang = new Hang();
                hang.Mahang = Convert.ToInt32(dataReader["Mahang"]);
                hang.Tenhang = Convert.ToString(dataReader["Tenhang"]);
                hang.Tenchatlieu = Convert.ToString(dataReader["Tenchatlieu"]);
                hang.Soluong = Convert.ToInt32(dataReader["Soluong"]);
                hang.Dongianhap = Convert.ToDecimal(dataReader["Dongianhap"]);
                hang.Dongiaban = Convert.ToDecimal(dataReader["Dongiaban"]);
          
                hangs.Add(hang);
            }
            dataGridView1.DataSource = hangs;
            hangsGlobal = hangs;
            cnn.Close();
            
        }
        void LoadChatLieu()
        {
            List<ChatLieu> chatlieus = new List<ChatLieu>();

            SqlConnection cnn = ketnoi();
            cnn.Open();
            string sql = "select * from tblChatlieu";
            SqlCommand cmd = new SqlCommand(sql, cnn);
            SqlDataReader dataReader = cmd.ExecuteReader();
            while (dataReader.Read())
            {
                ChatLieu chatlieu = new ChatLieu();
                chatlieu.MaChatlieu = Convert.ToInt32(dataReader["MaChatlieu"]);
                chatlieu.Tenchatlieu = Convert.ToString(dataReader["TenChatlieu"]);
                chatlieus.Add(chatlieu);

            }
            comboBox1.DataSource = chatlieus;
            comboBox1.ValueMember = "MaChatlieu";
            comboBox1.DisplayMember = "Tenchatlieu";
            cnn.Close();
        }
        public bool KiemTraThongTin()
        {
            if(textMahang.Text == "")
            {
                MessageBox.Show("Mã hàng không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if(textTenHang.Text == "")
            {
                MessageBox.Show("Tên hàng không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (textGiaTu.Text == "" || textGiaDen.Text == "")
            {
                MessageBox.Show("Khoảng giá không được để trống !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            if (Convert.ToDecimal(textGiaTu.Text)  <0 || Convert.ToDecimal(textGiaDen.Text) <0|| Convert.ToDecimal(textGiaTu.Text)> Convert.ToDecimal(textGiaDen.Text))
            {
                MessageBox.Show("Nhập khoảng giá hợp lệ !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadChatLieu();
            LoadData();

        }

        private void buttonTim_Click(object sender, EventArgs e)
        {


            if (KiemTraThongTin())
            {
                List<Hang> hangs = new List<Hang>();
                int maMh = Convert.ToInt32(textMahang.Text);
                string tenHang = textTenHang.Text;
                decimal giaTu = Convert.ToDecimal(textGiaTu.Text);
                decimal giaDen = Convert.ToDecimal(textGiaDen.Text);
                string chatLieu = comboBox1.GetItemText(comboBox1.SelectedItem);



                hangs = (List<Hang>)hangsGlobal
                                    .Where(x =>
                                    x.Mahang == maMh && 
                                    x.Tenhang.ToLower().Contains(tenHang.ToLower()) &&
                                    x.Dongiaban >= giaTu && x.Dongiaban <= giaDen &&
                                    x.Tenchatlieu == chatLieu).ToList();
                dataGridView1.DataSource = hangs;

            }
            
        }

        private void buttonInExcel_Click(object sender, EventArgs e)
        {
            cExcel.Application excel = new cExcel.Application();
            cExcel.Workbook wb = excel.Workbooks.Add(cExcel.XlSheetType.xlWorksheet);
            cExcel.Worksheet ws = (cExcel.Worksheet)excel.ActiveSheet;
            excel.Visible = true;
            ws.Cells[1, 1] = "Cửa Hàng Bán Đồ Lưu Niệm Bình An";
            ws.Cells[2, 1] = "Địa Chỉ: 37B - TT Đông Anh - Hà Nội";
            ws.Cells[3, 1] = "Điện Thoại: 0976967619";
            ws.Cells[5, 2] = "Danh Sách Các Mặt Hàng";
            ws.Cells[7, 1] = "STT";
            ws.Cells[7, 2] = "Mã Hàng";
            ws.Cells[7, 3] = "Tên Hàng";
            ws.Cells[7, 4] = "Chất Liệu";
            ws.Cells[7, 7] = "Số Lượng";
            ws.Cells[7, 5] = "Giá Nhập";
            ws.Cells[7, 6] = "Giá Bán";
            for (int i = 8; i <=  dataGridView1.Rows.Count + 7; i++)
            {
                for (int j = 1; j <= 7; j++)
                {
                    if (j == 1)
                    {
                        ws.Cells[i, 1] = i - 7;
                    }
                    else
                    {
                        ws.Cells[i, j] = dataGridView1.Rows[i - 8].Cells[j - 2].Value;
                    }
                }
            }
        }

        private void buttonThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát không ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
