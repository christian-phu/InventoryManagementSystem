using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win.Data;

namespace Win
{
    public partial class frmTaiKhoan : Form
    {
        WinQLSanPhamEntities db = new WinQLSanPhamEntities();
        frmDangNhap dn = new frmDangNhap();
        public static int maNhanVien = 0;

        public frmTaiKhoan()
        {
            InitializeComponent();
        }

        private void Them()
        {
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
        }

        private void Sua()
        {
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void LamMoi()
        {
            Them();
            LoadDuLieu();
            txtTenDangNhap.Text = "";
            txtMatKhau.Text = "";
            txtHoTen.Text = "";
            cbbQuyen.SelectedIndex = 0;
        }

        private void LoadDuLieu()
        {
            try
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = db.NhanViens.ToList();
                bdnDieuKhien.BindingSource = bs;
                dgvDuLieu.AutoGenerateColumns = false;
                dgvDuLieu.DataSource = bs;
            }
            catch
            {
                MessageBox.Show("Dữ liệu đầu vào không đúng!");
            }
        }

        private void frmTaiKhoan_Load(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void dgvDuLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTenDangNhap.Text = "";
            txtMatKhau.Text = "";
            txtHoTen.Text = "";
            cbbQuyen.SelectedIndex = 0;
            try
            {
                int HangChon = e.RowIndex;
                if (HangChon != -1)
                {
                    var ma = int.Parse(dgvDuLieu[0, HangChon].Value.ToString());
                    var obj = db.NhanViens.FirstOrDefault(x => x.MaNhanVien == ma);
                    maNhanVien = ma;

                    txtTenDangNhap.Text = obj.TaiKhoan;
                    txtMatKhau.Text = obj.MatKhau;
                    cbbQuyen.Text = obj.Quyen;
                    txtHoTen.Text = obj.HoTen;
                    Sua();
                }
            }
            catch
            {
                MessageBox.Show("Dữ liệu đầu vào không đúng!");
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var taiKhoan = db.NhanViens.FirstOrDefault(x => x.TaiKhoan == txtTenDangNhap.Text);
                db.NhanViens.Attach(taiKhoan);
                db.Entry(taiKhoan).State = System.Data.Entity.EntityState.Deleted;
                db.NhanViens.Remove(taiKhoan);
                db.SaveChanges();

                LamMoi();
                MessageBox.Show("Xóa thành công!");
            }
            catch
            {
                MessageBox.Show("Xóa không thành công!");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                var taiKhoan = db.NhanViens.Where(x => x.TaiKhoan == txtTenDangNhap.Text).FirstOrDefault();

                if (txtTenDangNhap.Text != taiKhoan.TaiKhoan)
                {
                    var kiemTraTaiKhoan = db.NhanViens.Where(x => x.TaiKhoan == txtTenDangNhap.Text).FirstOrDefault();
                    if (kiemTraTaiKhoan != null)
                    {
                        MessageBox.Show("Tên đăng nhập đã tồn tại!");
                        return;
                    }
                }

                if (txtMatKhau.Text == "" || txtTenDangNhap.Text == "" || txtHoTen.Text == "")
                {
                    MessageBox.Show("Nhập đầy đủ thông tin!");
                    return;
                }

                taiKhoan.TaiKhoan = txtTenDangNhap.Text;
                taiKhoan.MatKhau = txtMatKhau.Text;
                taiKhoan.HoTen = txtHoTen.Text;
                taiKhoan.Quyen = cbbQuyen.Text;

                db.NhanViens.Attach(taiKhoan);
                db.Entry(taiKhoan).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                LamMoi();
                MessageBox.Show("Sửa thành công!");
            }
            catch
            {
                MessageBox.Show("Sửa không thành công!");
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                var taiKhoan = new NhanVien();

                var kiemTraTaiKhoan = db.NhanViens.Where(x => x.TaiKhoan == txtTenDangNhap.Text).FirstOrDefault();
                if (kiemTraTaiKhoan != null)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!");
                    return;
                }

                if (txtMatKhau.Text == "" || txtTenDangNhap.Text == "")
                {
                    MessageBox.Show("Nhập đầy đủ thông tin!");
                    return;
                }

                taiKhoan.TaiKhoan = txtTenDangNhap.Text;
                taiKhoan.MatKhau = txtMatKhau.Text;
                taiKhoan.HoTen = txtHoTen.Text;
                taiKhoan.Quyen = cbbQuyen.Text;

                db.NhanViens.Add(taiKhoan);
                db.SaveChanges();

                LamMoi();
                MessageBox.Show("Thêm thành công!");
            }
            catch
            {
                MessageBox.Show("Thêm không thành công!");
            }
        }
    }
}
