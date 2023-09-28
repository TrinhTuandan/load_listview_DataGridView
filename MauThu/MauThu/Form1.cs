using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MauThu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Loading dữ liệu từ sql vào ListView 
        SqlConnection cnn = null;
        string strcnn = "Server = DESKTOP-9K1PP5I; Database=QuanLySinhVien; User id = sa; pwd = 123 ";  
        private void btnLoadDuLieu_Click(object sender, EventArgs e)
        {
            if (cnn == null)       
                cnn = new SqlConnection(strcnn);          
            if (cnn.State == ConnectionState.Closed)        
                cnn.Open();
          
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select * From Student";
            cmd.Connection = cnn;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                string ma = reader.GetString(0);
                string ten = reader.GetString(1);
                double diem = reader.GetDouble(2);
                ListViewItem lvl = new ListViewItem(ma);
                lvl.SubItems.Add(ten);
                lvl.SubItems.Add(diem.ToString());
                listView1.Items.Add(lvl);
            }
            reader.Close();
            cnn.Close();
        }
        // Loading dữ liệu từ sql vào Datagridview
        DataSet ds = null; //Biến Toàn Cục Dataset của ds
        SqlDataAdapter adt =null;//Biến Toàn Cục SqlDataAdapter của adt
        private void btnDataGirtView_Click(object sender, EventArgs e)
        {
            if (cnn == null)
                cnn = new SqlConnection(strcnn);
            adt = new SqlDataAdapter("Select * From Student", cnn);
            SqlCommandBuilder buider = new SqlCommandBuilder(adt);// sử dụng thêm đối tượng SqlCommandBuilder để thực hiện lệnh Update
            ds = new DataSet();
            adt.Fill(ds,"Student");
            dataGridView1.DataSource = ds.Tables["Student"];

        }
        // Thêm Vào Datagridview
        private void btnThem_Click(object sender, EventArgs e)
        {
            DataRow row = ds.Tables["Student"].NewRow();
            row["StudentID"] = txtMa.Text;
            row["FullName"] = txtTen.Text;
            row["AverageScore"] = txtDiem.Text;
            row["FacultyID"] = txtKhoa.Text;
            ds.Tables["Student"].Rows.Add(row);
            int kq = adt.Update(ds.Tables["Student"]);
            if (kq > 0)
            {
                btnDataGirtView.PerformClick();
            }
            else
                MessageBox.Show("Thêm Thất Bại");
        }
    }
}
