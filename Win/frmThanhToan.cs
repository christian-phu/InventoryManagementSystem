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
    public partial class frmThanhToan : Form
    {
        WinQLVatTuEntities db = new WinQLVatTuEntities();
        frmDangNhap dn = new frmDangNhap();
        public frmThanhToan()
        {
            InitializeComponent();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            var ma = dn.MaNhanVien();
            dgView.AutoGenerateColumns = false;

            var tuNgay = DateTime.Parse(dtpTuNgay.Text + " 00:00:00");
            var denNgay = DateTime.Parse(dtpDenNgay.Text + " 23:59:59");

            var tu = tuNgay.ToShortDateString();
            var den = denNgay.ToShortDateString();

            var list = db.ChiTietNhapKhoes.Where(x => x.NhapKho.MaNhanVien == ma && x.NhapKho.NgayThang >= tuNgay && x.NhapKho.NgayThang < denNgay).ToList();
            var tong = list.Sum(x => x.DonGia.Value * x.SoLuong.Value);
            var data = list.Select(x => new { MaNhanVien = x.NhapKho.MaNhanVien.Value, x.NhapKho.NhanVien.HoTen, x.VatTu.TenVatTu, SoLuong = x.SoLuong.Value, NgayThayDoi = x.NhapKho.NgayThang.Value, TuNgay = tu, DenNgay = den, x.NhapKho.GhiChu, DonGia = x.DonGia.Value, ThanhTien = x.SoLuong.Value * x.DonGia.Value, TongTien = tong }).ToList();
            lblTongTien.Text = tong.ToString();
            dgView.DataSource = data;
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            var ma = dn.MaNhanVien();
            frmIn frm = new frmIn(ma, 5, dtpTuNgay.Text, dtpDenNgay.Text);
            frm.ShowDialog();
        }

        private void frmTonKho_Load(object sender, EventArgs e)
        {

        }
    }
}
