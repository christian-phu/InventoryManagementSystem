using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win.Data;

namespace Win
{
    public partial class frmNhapKho : Form
    {
        WinQLVatTuEntities db = new WinQLVatTuEntities();
        public static List<ChiTietNhapKho> chiTiet = new List<ChiTietNhapKho>();

        frmDangNhap dn = new frmDangNhap();
        public int maChiTiet;
        public frmNhapKho()
        {
            InitializeComponent();
        }

        private void Them()
        {
            txtMaNhap.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;

            cbbKhoVatTu.Enabled = true;
        }

        private void Sua()
        {
            txtMaNhap.Enabled = false;
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
            btnXoa.Enabled = true;

            cbbKhoVatTu.Enabled = false;
        }

        private void ThemChiTiet()
        {
            btnThemChiTiet.Enabled = true;
            btnLuuChiTiet.Enabled = false;
            btnXoaChiTiet.Enabled = false;
            txtVatTu.Enabled = true;
        }

        private void SuaChiTiet()
        {
            btnThemChiTiet.Enabled = false;
            btnLuuChiTiet.Enabled = true;
            btnXoaChiTiet.Enabled = true;
            txtVatTu.Enabled = false;
        }

        private void LamMoi()
        {

            Them();
            LoadDuLieu();
            txtMaNhap.Text = "";
            txtGhiChu.Text = "";
            txtSoLuong.Text = "1";
            txtNhaCungCap.Text = "";
            txtNgayNhap.Text = DateTime.Now.ToShortDateString();
            lblTongTien.Text = "...";

            try
            {
                txtMaNhap.Text = (db.NhapKhoes.OrderByDescending(x => x.MaNhapKho).FirstOrDefault().MaNhapKho + 1).ToString();
            }
            catch
            {
                txtMaNhap.Text = "1";
            }

            try
            {
                cbbKhoVatTu.Text = dn.MaNhanVien() + "-" + dn.TaiKhoan();
            }
            catch
            {
                //
            }
            txtTiemKiem.Text = "Nhập từ khóa";
            chiTiet = new List<ChiTietNhapKho>();
            dgChiTiet.AutoGenerateColumns = false;
            dgChiTiet.DataSource = null;

            db = new WinQLVatTuEntities();
        }

        private void LoadDuLieu()
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = db.NhapKhoes.Select(x => new { x.MaNhapKho, TenKho = x.NhanVien.HoTen, x.NgayThang, NguoiGiao = x.NhaCungCap, x.GhiChu, TongTien = x.ChiTietNhapKhoes.Sum(y => y.SoLuong * y.DonGia) }).ToList();
            bdnDieuKhien.BindingSource = bs;
            dgvDuLieu.AutoGenerateColumns = false;
            dgvDuLieu.DataSource = bs;

            AutoCompleteStringCollection autoComplete = new AutoCompleteStringCollection();
            autoComplete.AddRange(db.VatTus.Select(x => x.TenVatTu + "~" + x.MaVatTu).ToArray());

            txtVatTu.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            txtVatTu.AutoCompleteSource = AutoCompleteSource.CustomSource;
            txtVatTu.AutoCompleteCustomSource = autoComplete;
        }

        private void LoadCombobox()
        {
            var manv = dn.MaNhanVien();
            cbbKhoVatTu.DataSource = db.NhanViens.Where(x => x.MaNhanVien == manv).Select(x => x.MaNhanVien + "-" + x.HoTen).ToList();
        }

        private void frmTaiKhoan_Load(object sender, EventArgs e)
        {
            LoadCombobox();
            LamMoi();
            LamMoiChiTiet();
        }

        private void dgvDuLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int HangChon = e.RowIndex;
            if (HangChon != -1)
            {
                var ma = int.Parse(dgvDuLieu[0, HangChon].Value.ToString());
                var obj = db.NhapKhoes.FirstOrDefault(x => x.MaNhapKho == ma);

                txtMaNhap.Text = obj.MaNhapKho.ToString();
                txtGhiChu.Text = obj.GhiChu;
                txtNgayNhap.Text = obj.NgayThang.Value.ToShortDateString();
                cbbKhoVatTu.Text = obj.MaNhanVien + "-" + obj.NhanVien.HoTen;
                txtNhaCungCap.Text = obj.NhaCungCap;
                lblTongTien.Text = obj.ChiTietNhapKhoes.Sum(x => x.DonGia * x.SoLuong).ToString();

                var list = db.ChiTietNhapKhoes.AsNoTracking().Where(x => x.MaNhapKho == ma).ToList();
                chiTiet = list.Select(x => new ChiTietNhapKho { MaChiTietNhapKho = x.MaChiTietNhapKho, MaNhapKho = x.MaNhapKho, MaVatTu = x.MaVatTu, SoLuong = x.SoLuong, TenVatTu = x.VatTu.TenVatTu, ThanhTien = x.DonGia * x.SoLuong, DonGia = x.DonGia }).ToList();
                dgChiTiet.AutoGenerateColumns = false;
                dgChiTiet.DataSource = chiTiet.Select(x => new { x.MaChiTietNhapKho, x.TenVatTu, x.SoLuong, x.DonGia, x.ThanhTien }).ToList();

                Sua();
                TongTien();
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            LamMoi();
            LamMoiChiTiet();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var ma = int.Parse(txtMaNhap.Text);
                var obj = db.NhapKhoes.FirstOrDefault(x => x.MaNhapKho == ma);

                var chitiets = db.ChiTietNhapKhoes.Where(x => x.MaNhapKho == obj.MaNhapKho).ToList();
                chitiets.ForEach(item =>
                {
                    var vatTu = db.VatTus.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                    vatTu.SoLuong += item.SoLuong;

                    db.VatTus.Attach(vatTu);
                    db.Entry(vatTu).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    db.ChiTietNhapKhoes.Attach(item);
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    db.ChiTietNhapKhoes.Remove(item);
                    db.SaveChanges();
                });

                db.NhapKhoes.Attach(obj);
                db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                db.NhapKhoes.Remove(obj);
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
                var ma = int.Parse(txtMaNhap.Text);
                var obj = db.NhapKhoes.FirstOrDefault(x => x.MaNhapKho == ma);

                if (cbbKhoVatTu.Text == "" || chiTiet.Count == 0)
                {
                    MessageBox.Show("Nhập đầy đủ thông tin!");
                    return;
                }

                obj.GhiChu = txtGhiChu.Text;
                obj.MaNhanVien = int.Parse(cbbKhoVatTu.Text.Split('-')[0].ToString().Trim());
                obj.NhaCungCap = txtNhaCungCap.Text;
                obj.NgayThang = DateTime.Parse(txtNgayNhap.Text);

                //Xóa
                var chitiets = db.ChiTietNhapKhoes.Where(x => x.MaNhapKho == obj.MaNhapKho).ToList();
                chitiets.ForEach(item =>
                {
                    var vatTu = db.VatTus.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                    vatTu.SoLuong -= item.SoLuong;

                    db.VatTus.Attach(vatTu);
                    db.Entry(vatTu).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    db.ChiTietNhapKhoes.Attach(item);
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    db.ChiTietNhapKhoes.Remove(item);
                    db.SaveChanges();
                });

                db.NhapKhoes.Attach(obj);
                db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //Thêm
                chiTiet.ForEach(item =>
                {
                    var chitetnhap = new ChiTietNhapKho
                    {
                        MaNhapKho = obj.MaNhapKho,
                        MaVatTu = item.MaVatTu,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia
                    };

                    db.ChiTietNhapKhoes.Add(chitetnhap);
                    db.SaveChanges();

                    var vatTu = db.VatTus.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                    vatTu.SoLuong += item.SoLuong;

                    db.VatTus.Attach(vatTu);
                    db.Entry(vatTu).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                });

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
                var loai = DateTime.Now.ToString("HHmmssff");
                if (txtMaNhap.Text == "" || cbbKhoVatTu.Text == "" || chiTiet.Count == 0)
                {
                    MessageBox.Show("Nhập đầy đủ thông tin!");
                    return;
                }

                var obj = new NhapKho
                {
                    MaNhanVien = int.Parse(cbbKhoVatTu.Text.Split('-')[0].ToString().Trim()),
                    NhaCungCap = txtNhaCungCap.Text,
                    GhiChu = txtGhiChu.Text,
                    NgayThang = DateTime.Parse(txtNgayNhap.Text)
                };

                db.NhapKhoes.Add(obj);
                db.SaveChanges();

                chiTiet.ForEach(item =>
                {
                    var chitetnhap = new ChiTietNhapKho
                    {
                        MaNhapKho = obj.MaNhapKho,
                        MaVatTu = item.MaVatTu,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia
                    };

                    db.ChiTietNhapKhoes.Add(chitetnhap);
                    db.SaveChanges();

                    var vatTu = db.VatTus.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                    vatTu.SoLuong += item.SoLuong;

                    db.VatTus.Attach(vatTu);
                    db.Entry(vatTu).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                });

                LamMoi();
                MessageBox.Show("Thêm thành công!");
            }
            catch
            {
                MessageBox.Show("Thêm không thành công!");
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = db.NhapKhoes.Where(x => x.NhanVien.HoTen.Contains(txtTiemKiem.Text)).Select(x => new { x.MaNhapKho, TenKho = x.NhanVien.HoTen, x.NgayThang, NguoiGiao = x.NhaCungCap, x.GhiChu, TongTien = x.ChiTietNhapKhoes.Sum(y => y.SoLuong * y.DonGia) }).ToList();
            bdnDieuKhien.BindingSource = bs;
            dgvDuLieu.AutoGenerateColumns = false;
            dgvDuLieu.DataSource = bs;
        }
        private void txtTiemKiem_Enter(object sender, EventArgs e)
        {
            if (txtTiemKiem.Text == "Nhập từ khóa")
            {
                txtTiemKiem.Text = "";
            }
        }

        private void txtTiemKiem_Leave(object sender, EventArgs e)
        {
            if (txtTiemKiem.Text == "")
            {
                txtTiemKiem.Text = "Nhập từ khóa";
            }
        }

        private void txtSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnThemChiTiet_Click(object sender, EventArgs e)
        {
            try
            {
                var maSp = int.Parse(txtVatTu.Text.Split('~')[1].ToString().Trim());
                if (chiTiet.Any(x => x.MaVatTu == maSp))
                {
                    MessageBox.Show("Sản phẩm này đã có trong danh sách!");
                }
                else
                {
                    var chitet = new ChiTietNhapKho
                    {
                        MaChiTietNhapKho = int.Parse(DateTime.Now.ToString("HHmmssff")),
                        MaVatTu = maSp,
                        SoLuong = int.Parse(txtSoLuong.Text),
                        TenVatTu = txtVatTu.Text.Split('~')[0].ToString(),
                        DonGia = int.Parse(txtDonGia.Text),
                        ThanhTien = int.Parse(txtSoLuong.Text) * int.Parse(txtDonGia.Text)
                    };

                    chiTiet.Add(chitet);
                    dgChiTiet.DataSource = null;
                    dgChiTiet.AutoGenerateColumns = false;
                    dgChiTiet.DataSource = chiTiet.Select(x => new { x.MaChiTietNhapKho, x.TenVatTu, x.SoLuong, x.DonGia, x.ThanhTien }).ToList();

                    LamMoiChiTiet();
                    TongTien();
                    MessageBox.Show("Thêm thành công!");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Thêm không thành công!");
            }
        }

        private void btnLuuChiTiet_Click(object sender, EventArgs e)
        {
            try
            {
                var maSp = int.Parse(txtVatTu.Text.Split('~')[1].ToString().Trim());
                chiTiet.ForEach(item =>
                    {
                        if (item.MaChiTietNhapKho == maChiTiet)
                        {
                            item.MaVatTu = int.Parse(txtVatTu.Text.Split('~')[1].ToString().Trim());
                            item.SoLuong = int.Parse(txtSoLuong.Text);
                            item.DonGia = int.Parse(txtDonGia.Text);
                            item.ThanhTien = int.Parse(txtSoLuong.Text) * int.Parse(txtDonGia.Text);
                        }
                    });

                dgChiTiet.DataSource = null;
                dgChiTiet.AutoGenerateColumns = false;
                dgChiTiet.DataSource = chiTiet.Select(x => new { x.MaChiTietNhapKho, x.TenVatTu, x.SoLuong, x.DonGia, x.ThanhTien }).ToList();

                LamMoiChiTiet();
                TongTien();

                MessageBox.Show("Sửa thành công!");
            }
            catch
            {
                MessageBox.Show("Sửa không thành công!");
            }
        }

        private void btnXoaChiTiet_Click(object sender, EventArgs e)
        {
            try
            {
                var chitet = chiTiet.FirstOrDefault(x => x.MaChiTietNhapKho == maChiTiet);

                chiTiet.Remove(chitet);
                dgChiTiet.DataSource = null;
                dgChiTiet.AutoGenerateColumns = false;
                dgChiTiet.DataSource = chiTiet.Select(x => new { x.MaChiTietNhapKho, x.TenVatTu, x.SoLuong, x.DonGia, x.ThanhTien }).ToList();

                LamMoiChiTiet();
                TongTien();

                MessageBox.Show("Xóa thành công!");
            }
            catch
            {
                MessageBox.Show("Xóa không thành công!");
            }
        }

        private void dgChiTiet_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int HangChon = e.RowIndex;
                if (HangChon != -1)
                {
                    maChiTiet = int.Parse(dgChiTiet[0, HangChon].Value.ToString());
                    var chitet = chiTiet.FirstOrDefault(x => x.MaChiTietNhapKho == maChiTiet);
                    txtVatTu.Text = chitet.TenVatTu + "~" + chitet.MaVatTu;
                    txtSoLuong.Text = chitet.SoLuong.ToString();
                    txtDonGia.Text = chitet.DonGia.ToString();
                    SuaChiTiet();
                    TinhTien();
                }
            }
            catch
            {
                MessageBox.Show("Dữ liệu đầu vào không đúng!");
            }
        }

        private void btnMoiChiTiet_Click(object sender, EventArgs e)
        {
            LamMoiChiTiet();
            TongTien();
        }

        public void LamMoiChiTiet()
        {
            ThemChiTiet();
            txtSoLuong.Text = "1";
            txtVatTu.Text = "";
            txtDonGia.Text = "";
            TinhTien();
        }

        private void txtDonGia_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtVatTu_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var maSp = int.Parse(txtVatTu.Text.Split('~')[1].ToString().Trim());
                var obj = db.VatTus.FirstOrDefault(x => x.MaVatTu == maSp);
                txtDonGia.Text = obj.DonGia.ToString();
                TinhTien();
            }
            catch { }
        }

        public void TinhTien()
        {
            try
            {
                lblThanhTien.Text = (int.Parse(txtSoLuong.Text) * int.Parse(txtDonGia.Text)).ToString();
            }
            catch
            {
                lblThanhTien.Text = "...";
            }
        }

        public void TongTien()
        {
            try
            {
                lblTongTien.Text = chiTiet.Sum(x => x.DonGia * x.SoLuong).ToString();
            }
            catch
            {
                lblTongTien.Text = "...";
            }
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            TinhTien();
        }

        private void txtDonGia_TextChanged(object sender, EventArgs e)
        {
            TinhTien();
        }
    }
}
