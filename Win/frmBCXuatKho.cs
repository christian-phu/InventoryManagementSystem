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
    public partial class frmBCXuatKho : Form
    {
        WinQLSanPhamEntities db = new WinQLSanPhamEntities();
        frmDangNhap dn = new frmDangNhap();
        public frmBCXuatKho()
        {
            InitializeComponent();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            var maKho = int.Parse(cbbKho.Text.Split('-')[0].ToString().Trim());

            dgView.AutoGenerateColumns = false;

            var tuNgay = DateTime.Parse(dtpTuNgay.Text + " 00:00:00");
            var denNgay = DateTime.Parse(dtpDenNgay.Text + " 23:59:59");

            var tu = tuNgay.ToShortDateString();
            var den = denNgay.ToShortDateString();

            dgView.DataSource = db.ChiTietXuatKhoes.Where(x => x.XuatKho.MaNhanVien == maKho && x.XuatKho.NgayThang >= tuNgay && x.XuatKho.NgayThang < denNgay && x.XuatKho.TrangThai == true).Select(x => new { x.XuatKho.MaNhanVien, x.XuatKho.NhanVien.HoTen, x.SanPham.TenSanPham, SoLuong = x.SoLuong.Value, NgayThayDoi = x.XuatKho.NgayThang.Value, TuNgay = tu, DenNgay = den, x.XuatKho.GhiChu }).ToList();
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            var maKho = int.Parse(cbbKho.Text.Split('-')[0].ToString().Trim());

            frmIn frm = new frmIn(maKho, 3, dtpTuNgay.Text, dtpDenNgay.Text);
            frm.ShowDialog();
        }

        private void frmTonKho_Load(object sender, EventArgs e)
        {
            var quyen = dn.Quyen();
            var ma = dn.MaNhanVien();
            if (quyen == "Nhân viên")
            {
                cbbKho.DataSource = db.NhanViens.Where(x => x.MaNhanVien == ma).Select(x => x.MaNhanVien + "-" + x.HoTen).ToList();
            }
            else
            {
                cbbKho.DataSource = db.NhanViens.Select(x => x.MaNhanVien + "-" + x.HoTen).ToList();
            }
        }
    }
}
