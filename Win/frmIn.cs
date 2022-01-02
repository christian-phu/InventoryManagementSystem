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
    public partial class frmIn : Form
    {
        public int maKho;
        public int loai;
        public string tuN;
        public string denN;
        WinQLVatTuEntities db = new WinQLVatTuEntities();
        public frmIn(int ma, int l, string tn, string dn)
        {
            InitializeComponent();
            maKho = ma;
            loai = l;
            tuN = tn;
            denN = dn;
        }

        private void frmIn_Load(object sender, EventArgs e)
        {
            if (loai == 1)
            {
                var data = db.VatTus.Where(x => x.SoLuong > 0).Select(x => new { x.TenVatTu, SoLuong = x.SoLuong.Value }).ToList();

                crTonKho cr = new crTonKho();
                cr.SetDataSource(data);
                crystalReportViewer.ReportSource = cr;
                crystalReportViewer.Refresh();
            }

            if (loai == 2)
            {
                var tuNgay = DateTime.Parse(tuN + " 00:00:00");
                var denNgay = DateTime.Parse(denN + " 23:59:59");

                var tu = tuNgay.ToShortDateString();
                var den = denNgay.ToShortDateString();
                var data = db.ChiTietNhapKhoes.Where(x => x.NhapKho.MaNhanVien == maKho && x.NhapKho.NgayThang >= tuNgay && x.NhapKho.NgayThang < denNgay).Select(x => new { MaNhanVien = x.NhapKho.MaNhanVien.Value, x.NhapKho.NhanVien.HoTen, x.VatTu.TenVatTu, SoLuong = x.SoLuong.Value, NgayThayDoi = x.NhapKho.NgayThang.Value, TuNgay = tu, DenNgay = den, x.NhapKho.GhiChu }).ToList();

                crNhapKho cr = new crNhapKho();
                cr.SetDataSource(data);
                crystalReportViewer.ReportSource = cr;
                crystalReportViewer.Refresh();
            }

            if (loai == 3)
            {
                var tuNgay = DateTime.Parse(tuN + " 00:00:00");
                var denNgay = DateTime.Parse(denN + " 23:59:59");

                var tu = tuNgay.ToShortDateString();
                var den = denNgay.ToShortDateString();

                var data = db.ChiTietXuatKhoes.Where(x => x.XuatKho.MaNhanVien == maKho && x.XuatKho.NgayThang >= tuNgay && x.XuatKho.NgayThang < denNgay && x.XuatKho.TrangThai == true).Select(x => new { MaNhanVien = x.XuatKho.MaNhanVien.Value, x.XuatKho.NhanVien.HoTen, x.VatTu.TenVatTu, SoLuong = x.SoLuong.Value, NgayThayDoi = x.XuatKho.NgayThang.Value, TuNgay = tu, DenNgay = den, x.XuatKho.GhiChu }).ToList();

                crXuatKho cr = new crXuatKho();
                cr.SetDataSource(data);
                crystalReportViewer.ReportSource = cr;
                crystalReportViewer.Refresh();
            }            

            if (loai == 5)
            {
                var tuNgay = DateTime.Parse(tuN + " 00:00:00");
                var denNgay = DateTime.Parse(denN + " 23:59:59");

                var tu = tuNgay.ToShortDateString();
                var den = denNgay.ToShortDateString();

                var list = db.ChiTietNhapKhoes.Where(x => x.NhapKho.MaNhanVien == maKho && x.NhapKho.NgayThang >= tuNgay && x.NhapKho.NgayThang < denNgay).ToList();
                var tong = list.Sum(x => x.DonGia.Value * x.SoLuong.Value);
                var data = list.Select(x => new { MaNhanVien = x.NhapKho.MaNhanVien.Value, x.NhapKho.NhanVien.HoTen, x.VatTu.TenVatTu, SoLuong = x.SoLuong.Value, NgayThayDoi = x.NhapKho.NgayThang.Value, TuNgay = tu, DenNgay = den, x.NhapKho.GhiChu, DonGia = x.DonGia.Value, ThanhTien = x.SoLuong.Value * x.DonGia.Value, TongTien = tong }).ToList();
                 
                crThanhToan cr = new crThanhToan();
                cr.SetDataSource(data);
                crystalReportViewer.ReportSource = cr;
                crystalReportViewer.Refresh();
            }
        }
    }
}
