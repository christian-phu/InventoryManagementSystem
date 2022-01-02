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
    public partial class frmXuatKho : Form
    {
        WinQLVatTuEntities db = new WinQLVatTuEntities();
        public static List<ChiTietXuatKho> chiTiet = new List<ChiTietXuatKho>();

        frmDangNhap dn = new frmDangNhap();
        public int maChiTiet;
        public frmXuatKho()
        {
            InitializeComponent();
        }

        private void Them()
        {
            txtMaXuat.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            btnDuyet.Enabled = false;

            if (dn.Quyen() == "Nhân viên")
            {
                btnThem.Enabled = false;
                btnLuu.Enabled = false;
                btnXoa.Enabled = false;
                btnDuyet.Enabled = false;
            }

            cbbKhoVatTu.Enabled = true;
        }

        private void Sua()
        {
            txtMaXuat.Enabled = false;
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
            btnXoa.Enabled = true;
            btnDuyet.Enabled = false;

            if (dn.Quyen() == "Nhân viên")
            {
                btnThem.Enabled = false;
                btnLuu.Enabled = false;
                btnXoa.Enabled = false;
                btnDuyet.Enabled = false;
            }

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
            txtMaXuat.Text = "";
            txtGhiChu.Text = "";
            txtSoLuong.Text = "1";
            txtNgayXuat.Text = DateTime.Now.ToShortDateString();
            lblTongTien.Text = "...";

            try
            {
                txtMaXuat.Text = (db.XuatKhoes.OrderByDescending(x => x.MaXuatKho).FirstOrDefault().MaXuatKho + 1).ToString();
            }
            catch
            {
                txtMaXuat.Text = "1";
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
            chiTiet = new List<ChiTietXuatKho>();
            dgChiTiet.AutoGenerateColumns = false;
            dgChiTiet.DataSource = null;

            db = new WinQLVatTuEntities();
        }

        private void LoadDuLieu()
        {
            BindingSource bs = new BindingSource();

            if (dn.Quyen() == "Nhân viên")
            {
                var ma = dn.MaNhanVien();
                bs.DataSource = db.XuatKhoes.Where(x => x.MaNhanVien == ma).Select(x => new { x.MaXuatKho, TenKho = x.NhanVien.HoTen, x.NgayThang, x.GhiChu, TongTien = x.ChiTietXuatKhoes.Sum(y => y.SoLuong * y.DonGia), x.TienDaThanhToan, TrangThai = x.TrangThai == true ? "Đã duyệt" : "Chưa duyệt" }).ToList();
            }
            else
            {
                bs.DataSource = db.XuatKhoes.Select(x => new { x.MaXuatKho, TenKho = x.NhanVien.HoTen, x.NgayThang, x.GhiChu, TongTien = x.ChiTietXuatKhoes.Sum(y => y.SoLuong * y.DonGia), x.TienDaThanhToan, TrangThai = x.TrangThai == true ? "Đã duyệt" : "Chưa duyệt" }).ToList();
            }

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
            if (dn.Quyen() == "Nhân viên")
            {
                var ma = dn.MaNhanVien();
                cbbKhoVatTu.DataSource = db.NhanViens.Where(x => x.Quyen == "Nhân viên" && x.MaNhanVien == ma).Select(x => x.MaNhanVien + "-" + x.HoTen).ToList();
            }
            else
            {
                cbbKhoVatTu.DataSource = db.NhanViens.Where(x => x.Quyen == "Nhân viên").Select(x => x.MaNhanVien + "-" + x.HoTen).ToList();
            }
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
                var obj = db.XuatKhoes.FirstOrDefault(x => x.MaXuatKho == ma);

                txtMaXuat.Text = obj.MaXuatKho.ToString();
                txtGhiChu.Text = obj.GhiChu;
                txtNgayXuat.Text = obj.NgayThang.Value.ToShortDateString();
                cbbKhoVatTu.Text = obj.MaNhanVien + "-" + obj.NhanVien.HoTen;
                lblTongTien.Text = obj.ChiTietXuatKhoes.Sum(x => x.DonGia * x.SoLuong).ToString();

                var list = db.ChiTietXuatKhoes.AsNoTracking().Where(x => x.MaXuatKho == ma).ToList();
                chiTiet = list.Select(x => new ChiTietXuatKho { MaChiTietXuatKho = x.MaChiTietXuatKho, MaXuatKho = x.MaXuatKho, MaVatTu = x.MaVatTu, SoLuong = x.SoLuong, TenVatTu = x.VatTu.TenVatTu, ThanhTien = x.DonGia * x.SoLuong, DonGia = x.DonGia }).ToList();

                dgChiTiet.AutoGenerateColumns = false;
                dgChiTiet.DataSource = chiTiet.Select(x => new { x.MaChiTietXuatKho, x.TenVatTu, x.SoLuong, x.DonGia, x.ThanhTien }).ToList();

                Sua();

                if (obj.TrangThai == false)
                {
                    if (dn.Quyen() == "Nhân viên")
                    {
                        btnDuyet.Enabled = false;
                    }
                    else
                    {
                        btnDuyet.Enabled = true;
                    }

                    btnLuu.Enabled = false;
                    btnXoa.Enabled = false;
                }
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
                var ma = int.Parse(txtMaXuat.Text);
                var obj = db.XuatKhoes.FirstOrDefault(x => x.MaXuatKho == ma);

                var chitiets = db.ChiTietXuatKhoes.Where(x => x.MaXuatKho == obj.MaXuatKho).ToList();
                chitiets.ForEach(item =>
                {
                    var vatTu = db.VatTus.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                    vatTu.SoLuong += item.SoLuong;

                    db.VatTus.Attach(vatTu);
                    db.Entry(vatTu).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    db.ChiTietXuatKhoes.Attach(item);
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    db.ChiTietXuatKhoes.Remove(item);
                    db.SaveChanges();
                });

                db.XuatKhoes.Attach(obj);
                db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                db.XuatKhoes.Remove(obj);
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
                var ma = int.Parse(txtMaXuat.Text);
                var obj = db.XuatKhoes.FirstOrDefault(x => x.MaXuatKho == ma);

                if (cbbKhoVatTu.Text == "" || chiTiet.Count == 0)
                {
                    MessageBox.Show("Nhập đầy đủ thông tin!");
                    return;
                }

                obj.GhiChu = txtGhiChu.Text;
                obj.MaNhanVien = int.Parse(cbbKhoVatTu.Text.Split('-')[0].ToString().Trim());
                obj.NgayThang = DateTime.Parse(txtNgayXuat.Text);
                obj.TienDaThanhToan = int.Parse(txtDaThanhToan.Text);

                //Xóa
                var chitiets = db.ChiTietXuatKhoes.Where(x => x.MaXuatKho == obj.MaXuatKho).ToList();
                chitiets.ForEach(item =>
                {
                    var vatTu = db.VatTus.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                    vatTu.SoLuong += item.SoLuong;

                    db.VatTus.Attach(vatTu);
                    db.Entry(vatTu).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    db.ChiTietXuatKhoes.Attach(item);
                    db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                    db.ChiTietXuatKhoes.Remove(item);
                    db.SaveChanges();
                });

                db.XuatKhoes.Attach(obj);
                db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //Thêm
                chiTiet.ForEach(item =>
                {
                    var chitetxuat = new ChiTietXuatKho
                    {
                        MaXuatKho = obj.MaXuatKho,
                        MaVatTu = item.MaVatTu,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia
                    };

                    db.ChiTietXuatKhoes.Add(chitetxuat);
                    db.SaveChanges();

                    var vatTu = db.VatTus.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                    vatTu.SoLuong -= item.SoLuong;

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
                if (txtMaXuat.Text == "" || cbbKhoVatTu.Text == "" || chiTiet.Count == 0)
                {
                    MessageBox.Show("Nhập đầy đủ thông tin!");
                    return;
                }

                var obj = new XuatKho
                {
                    MaNhanVien = int.Parse(cbbKhoVatTu.Text.Split('-')[0].ToString().Trim()),
                    GhiChu = txtGhiChu.Text,
                    NgayThang = DateTime.Parse(txtNgayXuat.Text),
                    TienDaThanhToan = int.Parse(txtDaThanhToan.Text),
                    TrangThai = true
                };

                db.XuatKhoes.Add(obj);
                db.SaveChanges();

                chiTiet.ForEach(item =>
                {
                    var chitetxuat = new ChiTietXuatKho
                    {
                        MaXuatKho = obj.MaXuatKho,
                        MaVatTu = item.MaVatTu,
                        SoLuong = item.SoLuong,
                        DonGia = item.DonGia
                    };

                    db.ChiTietXuatKhoes.Add(chitetxuat);
                    db.SaveChanges();

                    var vatTu = db.VatTus.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                    vatTu.SoLuong -= item.SoLuong;

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
            bs.DataSource = db.XuatKhoes.Where(x => x.NhanVien.HoTen.Contains(txtTiemKiem.Text)).Select(x => new { x.MaXuatKho, TenKho = x.NhanVien.HoTen, x.NgayThang, x.GhiChu, TongTien = x.ChiTietXuatKhoes.Sum(y => y.SoLuong * y.DonGia) }).ToList();
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
                    var chitietkho = db.VatTus.FirstOrDefault(x => x.MaVatTu == maSp);
                    if (chitietkho == null)
                    {
                        MessageBox.Show("Vật tư này chưa có trong kho!");
                    }
                    else
                    {
                        var sl = chitietkho.SoLuong;
                        if (sl <= 0)
                        {
                            MessageBox.Show("Vật tư trong kho đã hêt!");
                        }
                        else
                        {
                            if (int.Parse(txtSoLuong.Text) > sl)
                            {
                                MessageBox.Show("Số lượng vượt quá số lượng trong kho!");
                            }
                            else
                            {
                                var chitet = new ChiTietXuatKho
                                {
                                    MaChiTietXuatKho = int.Parse(DateTime.Now.ToString("HHmmssff")),
                                    MaVatTu = maSp,
                                    SoLuong = int.Parse(txtSoLuong.Text),
                                    TenVatTu = txtVatTu.Text.Split('~')[0].ToString(),
                                    DonGia = int.Parse(txtDonGia.Text),
                                    ThanhTien = int.Parse(txtSoLuong.Text) * int.Parse(txtDonGia.Text)
                                };

                                chiTiet.Add(chitet);
                                dgChiTiet.DataSource = null;
                                dgChiTiet.AutoGenerateColumns = false;
                                dgChiTiet.DataSource = chiTiet.Select(x => new { x.MaChiTietXuatKho, x.TenVatTu, x.SoLuong, x.DonGia, x.ThanhTien }).ToList();

                                LamMoiChiTiet();
                                TongTien();
                                MessageBox.Show("Thêm thành công!");
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Thêm không thành công!");
            }
        }

        private void btnLuuChiTiet_Click(object sender, EventArgs e)
        {
            try
            {
                var maSp = int.Parse(txtVatTu.Text.Split('~')[1].ToString().Trim());

                var chitietkho = db.VatTus.FirstOrDefault(x => x.MaVatTu == maSp);
                if (chitietkho == null)
                {
                    MessageBox.Show("Vật tư này chưa có trong kho!");
                }
                else
                {
                    var sl = chitietkho.SoLuong;
                    if (sl <= 0)
                    {
                        MessageBox.Show("Vật tư trong kho đã hêt!");
                    }
                    else
                    {
                        if (int.Parse(txtSoLuong.Text) > sl)
                        {
                            MessageBox.Show("Số lượng vượt quá số lượng trong kho!");
                        }
                        else
                        {
                            chiTiet.ForEach(item =>
                            {
                                if (item.MaChiTietXuatKho == maChiTiet)
                                {
                                    item.MaVatTu = int.Parse(txtVatTu.Text.Split('~')[1].ToString().Trim());
                                    item.SoLuong = int.Parse(txtSoLuong.Text);
                                    item.DonGia = int.Parse(txtDonGia.Text);
                                    item.ThanhTien = int.Parse(txtSoLuong.Text) * int.Parse(txtDonGia.Text);
                                }
                            });

                            dgChiTiet.DataSource = null;
                            dgChiTiet.AutoGenerateColumns = false;
                            dgChiTiet.DataSource = chiTiet.Select(x => new { x.MaChiTietXuatKho, x.TenVatTu, x.SoLuong, x.DonGia, x.ThanhTien }).ToList();

                            LamMoiChiTiet();
                            TongTien();

                            MessageBox.Show("Sửa thành công!");
                        }
                    }
                }
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
                var chitet = chiTiet.FirstOrDefault(x => x.MaChiTietXuatKho == maChiTiet);

                chiTiet.Remove(chitet);
                dgChiTiet.DataSource = null;
                dgChiTiet.AutoGenerateColumns = false;
                dgChiTiet.DataSource = chiTiet.Select(x => new { x.MaChiTietXuatKho, x.TenVatTu, x.SoLuong, x.DonGia, x.ThanhTien }).ToList();

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
                    var chitet = chiTiet.FirstOrDefault(x => x.MaChiTietXuatKho == maChiTiet);
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

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            TinhTien();
        }

        private void txtDonGia_TextChanged(object sender, EventArgs e)
        {
            TinhTien();
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

        private void txtDaThanhToan_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnDuyet_Click(object sender, EventArgs e)
        {
            try
            {
                var ma = int.Parse(txtMaXuat.Text);
                var obj = db.XuatKhoes.FirstOrDefault(x => x.MaXuatKho == ma);

                obj.TrangThai = true;

                var chitiets = db.ChiTietXuatKhoes.Where(x => x.MaXuatKho == obj.MaXuatKho).ToList();
                chitiets.ForEach(item =>
                {

                    var chitietkho = db.VatTus.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                    var sl = chitietkho.SoLuong;
                    if (sl <= 0)
                    {
                        db.ChiTietXuatKhoes.Attach(item);
                        db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                        db.ChiTietXuatKhoes.Remove(item);
                        db.SaveChanges();
                    }
                    else
                    {
                        if (int.Parse(txtSoLuong.Text) > sl)
                        {
                            db.ChiTietXuatKhoes.Attach(item);
                            db.Entry(item).State = System.Data.Entity.EntityState.Deleted;
                            db.ChiTietXuatKhoes.Remove(item);
                            db.SaveChanges();
                        }
                        else
                        {
                            var vatTu = db.VatTus.FirstOrDefault(x => x.MaVatTu == item.MaVatTu);
                            vatTu.SoLuong -= item.SoLuong;

                            db.VatTus.Attach(vatTu);
                            db.Entry(vatTu).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                });

                db.XuatKhoes.Attach(obj);
                db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                LamMoi();
                MessageBox.Show("Duyệt thành công!");

            }
            catch
            {
                MessageBox.Show("Duyệt không thành công!");
            }
        }
    }
}
