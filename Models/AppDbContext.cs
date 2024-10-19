using Microsoft.EntityFrameworkCore;

namespace feedBackMvc.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Admins> Admins { get; set; }
        public DbSet<IN_MauKhaoSat> IN_MauKhaoSat { get; set; }
        public DbSet<IN_NhomCauHoiKhaoSat> IN_NhomCauHoiKhaoSat { get; set; }
        public DbSet<IN_CauHoiKhaoSat> IN_CauHoiKhaoSat { get; set; }
        public DbSet<IN_ThongTinNguoiBenh> IN_ThongTinNguoiBenh { get; set; }
        public DbSet<IN_ThongTinChung> IN_ThongTinChung { get; set; }
        public DbSet<IN_ThongTinYKienKhac> IN_ThongTinYKienKhac { get; set; }
        public DbSet<IN_DanhGia> IN_DanhGia { get; set; }
        public DbSet<OUT_NhomCauHoiKhaoSat> OUT_NhomCauHoiKhaoSat { get; set; }
        public DbSet<OUT_CauHoiKhaoSat> OUT_CauHoiKhaoSat { get; set; }
        public DbSet<OUT_MauKhaoSat> OUT_MauKhaoSat { get; set; }
        public DbSet<OUT_ThongTinNguoiBenh> OUT_ThongTinNguoiBenh { get; set; }
        public DbSet<OUT_ThongTinChung> OUT_ThongTinChung { get; set; }
        public DbSet<OUT_ThongTinYKienKhac> OUT_ThongTinYKienKhac { get; set; }
        public DbSet<OUT_DanhGia> OUT_DanhGia { get; set; }
        public DbSet<ORTHER_MauKhaoSat> ORTHER_MauKhaoSat { get; set; }
        public DbSet<ORTHER_NhomCauHoiKhaoSat> ORTHER_NhomCauHoiKhaoSat { get; set; }
        public DbSet<ORTHER_CauHoiKhaoSat> ORTHER_CauHoiKhaoSat { get; set; }
        public DbSet<ORTHER_DanhGia> ORTHER_DanhGia { get; set; }
        public DbSet<ORTHER_ThongTinNguoiDanhGia> ORTHER_ThongTinNguoiDanhGia { get; set; }
        public DbSet<ORTHER_ThongTinYKienKhac> ORTHER_ThongTinYKienKhac { get; set; }
        public DbSet<FallbackChatbot> FallbackChatbot { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //// Cấu hình ràng buộc UNIQUE cho cột TieuDe trong bảng IN_NhomCauHoiKhaoSat
            modelBuilder.Entity<IN_NhomCauHoiKhaoSat>()
                .HasIndex(t => t.TieuDe)
                .IsUnique();

            // Giới hạn độ dài của cột TieuDe
            modelBuilder.Entity<IN_NhomCauHoiKhaoSat>().Property(n => n.TieuDe).HasMaxLength(5);

            //// Cấu hình ràng buộc UNIQUE cho cột TieuDeCauHoi trong bảng IN_CauHoiKhaoSat
            modelBuilder.Entity<IN_CauHoiKhaoSat>()
                .HasIndex(t => t.TieuDeCauHoi)
                .IsUnique();

            // Cấu hình quan hệ giữa IN_NhomCauHoiKhaoSat và IN_CauHoiKhaoSat với ON DELETE CASCADE
            modelBuilder.Entity<IN_NhomCauHoiKhaoSat>()
                .HasMany(n => n.CauHoiKhaoSats)
                .WithOne(c => c.NhomCauHoiKhaoSat)
                .HasForeignKey(c => c.IdIN_NhomCauHoiKhaoSat)
                .OnDelete(DeleteBehavior.Cascade); // Thêm dòng này để cấu hình xóa cascade
            modelBuilder.Entity<IN_MauKhaoSat>()
                .HasOne(m => m.admins)
                .WithMany(a => a.MauKhaoSats) // Define the collection property in Admins class
                .HasForeignKey(m => m.idAdmin)
                .OnDelete(DeleteBehavior.Cascade); // You can choose Cascade or Restrict
            modelBuilder.Entity<IN_MauKhaoSat>()
                .Property(dg => dg.NgayTao)
                .HasColumnType("DATE");
            modelBuilder.Entity<IN_MauKhaoSat>()
                .Property(dg => dg.NgayBatDau)
                .HasColumnType("DATE");
            modelBuilder.Entity<IN_MauKhaoSat>()
                .Property(dg => dg.NgayKetThuc)
                .HasColumnType("DATE");

            // Cấu hình các cột kiểu mảng cho PostgreSQL
            modelBuilder.Entity<IN_DanhGia>()
                .Property(f => f.DanhGia)
                .HasColumnType("int[]");
            modelBuilder.Entity<IN_DanhGia>()
                .Property(dg => dg.NgayDanhGia)
                .HasColumnType("DATE");

            // Cấu hình quan hệ giữa IN_DanhGia và IN_MauKhaoSat
            modelBuilder.Entity<IN_DanhGia>()
                .HasOne(dg => dg.MauKhaoSat)
                .WithMany(mks => mks.DanhGia) // Assuming you have a collection property in IN_MauKhaoSat
                .HasForeignKey(dg => dg.IdIN_MauKhaoSat)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình quan hệ giữa IN_DanhGia và IN_ThongTinNguoiBenh
            modelBuilder.Entity<IN_DanhGia>()
                .HasOne(dg => dg.ThongTinNguoiBenh)
                .WithMany(ttnb => ttnb.DanhGia);
            modelBuilder.Entity<IN_DanhGia>()
                .Ignore(dg => dg.ThongTinNguoiBenh);
            modelBuilder.Entity<IN_DanhGia>()
                .HasIndex(dg => new { dg.IdIN_MauKhaoSat, dg.IdIN_ThongTinNguoiBenh, dg.NgayDanhGia })
                .IsUnique();
            modelBuilder.Entity<IN_ThongTinChung>()
                .Property(dg => dg.NgayDienPhieu)
                .HasColumnType("DATE");
            modelBuilder.Entity<IN_ThongTinYKienKhac>()
                .Property(dg => dg.NgayTao)
                .HasColumnType("DATE");


            // Additional configurations if needed
            //// Cấu hình ràng buộc UNIQUE cho cột TieuDe trong bảng OUT_NhomCauHoiKhaoSat
            modelBuilder.Entity<OUT_NhomCauHoiKhaoSat>()
                .HasIndex(t => t.TieuDe)
                .IsUnique();
            // Giới hạn độ dài của cột TieuDe
            modelBuilder.Entity<OUT_NhomCauHoiKhaoSat>().Property(n => n.TieuDe).HasMaxLength(5);
            // Cấu hình quan hệ giữa OUT_NhomCauHoiKhaoSat và OUT_CauHoiKhaoSat với ON DELETE CASCADE
            modelBuilder.Entity<OUT_NhomCauHoiKhaoSat>()
                .HasMany(n => n.CauHoiKhaoSats)
                .WithOne(c => c.NhomCauHoiKhaoSat)
                .HasForeignKey(c => c.IdOUT_NhomCauHoiKhaoSat)
                .OnDelete(DeleteBehavior.Cascade); // Thêm dòng này để cấu hình xóa cascade

            //// Cấu hình ràng buộc UNIQUE cho cột TieuDeCauHoi trong bảng IN_CauHoiKhaoSat
            modelBuilder.Entity<OUT_CauHoiKhaoSat>()
                .HasIndex(t => t.TieuDeCauHoi)
                .IsUnique();

            modelBuilder.Entity<OUT_MauKhaoSat>()
                .HasOne(m => m.admins)
                .WithMany(a => a.OUT_MauKhaoSats) // Define the collection property in Admins class
                .HasForeignKey(m => m.idAdmin)
                .OnDelete(DeleteBehavior.Cascade); // You can choose Cascade or Restrict
            modelBuilder.Entity<OUT_MauKhaoSat>()
                .Property(dg => dg.NgayTao)
                .HasColumnType("DATE");
            modelBuilder.Entity<OUT_MauKhaoSat>()
                .Property(dg => dg.NgayBatDau)
                .HasColumnType("DATE");
            modelBuilder.Entity<OUT_MauKhaoSat>()
                .Property(dg => dg.NgayKetThuc)
                .HasColumnType("DATE");
            modelBuilder.Entity<OUT_ThongTinChung>()
                .Property(dg => dg.NgayDienPhieu)
                .HasColumnType("DATE");
            modelBuilder.Entity<OUT_ThongTinYKienKhac>()
                .Property(dg => dg.NgayTao)
                .HasColumnType("DATE");
            // Cấu hình quan hệ giữa OUT_DanhGia và OUT_MauKhaoSat
            modelBuilder.Entity<OUT_DanhGia>()
                .HasOne(dg => dg.MauKhaoSat)
                .WithMany(mks => mks.DanhGia) // Assuming you have a collection property in IN_MauKhaoSat
                .HasForeignKey(dg => dg.IdOUT_MauKhaoSat)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình quan hệ giữa OUT_DanhGia và OUT_ThongTinNguoiBenh
            modelBuilder.Entity<OUT_DanhGia>()
                .HasOne(dg => dg.ThongTinNguoiBenh)
                .WithMany(ttnb => ttnb.OUT_DanhGia); // Assuming IN_ThongTinNguoiBenh has a collection of DanhGia
            modelBuilder.Entity<OUT_DanhGia>()
                .HasIndex(dg => new { dg.IdOUT_MauKhaoSat, dg.IdOUT_ThongTinNguoiBenh, dg.NgayDanhGia })
                .IsUnique();
            modelBuilder.Entity<OUT_DanhGia>()
                .Property(dg => dg.NgayDanhGia)
                .HasColumnType("DATE");
            //// Cấu hình ràng buộc UNIQUE cho cột TieuDe trong bảng IN_NhomCauHoiKhaoSat
            ///

            modelBuilder.Entity<ORTHER_NhomCauHoiKhaoSat>()
                .HasIndex(t => t.TieuDe)
                .IsUnique();

            // Giới hạn độ dài của cột TieuDe
            modelBuilder.Entity<ORTHER_NhomCauHoiKhaoSat>().Property(n => n.TieuDe).HasMaxLength(5);

            //// Cấu hình ràng buộc UNIQUE cho cột TieuDeCauHoi trong bảng IN_CauHoiKhaoSat
            modelBuilder.Entity<ORTHER_CauHoiKhaoSat>()
                .HasIndex(t => t.TieuDeCauHoi)
                .IsUnique();

            // Cấu hình quan hệ giữa IN_NhomCauHoiKhaoSat và IN_CauHoiKhaoSat với ON DELETE CASCADE
            modelBuilder.Entity<ORTHER_NhomCauHoiKhaoSat>()
                .HasMany(n => n.CauHoiKhaoSats)
                .WithOne(c => c.NhomCauHoiKhaoSat)
                .HasForeignKey(c => c.IdORTHER_NhomCauHoiKhaoSat)
                .OnDelete(DeleteBehavior.Cascade); // Thêm dòng này để cấu hình xóa cascade
            modelBuilder.Entity<ORTHER_MauKhaoSat>()
                .HasOne(m => m.admins)
                .WithMany(a => a.ORTHER_MauKhaoSats) // Define the collection property in Admins class
                .HasForeignKey(m => m.idAdmin)
                .OnDelete(DeleteBehavior.Cascade); // You can choose Cascade or Restrict
            modelBuilder.Entity<ORTHER_MauKhaoSat>()
                .Property(dg => dg.NgayTao)
                .HasColumnType("DATE");
            modelBuilder.Entity<ORTHER_MauKhaoSat>()
                .Property(dg => dg.NgayBatDau)
                .HasColumnType("DATE");
            modelBuilder.Entity<ORTHER_MauKhaoSat>()
                .Property(dg => dg.NgayKetThuc)
                .HasColumnType("DATE");

            // Cấu hình các cột kiểu mảng cho PostgreSQL
            modelBuilder.Entity<ORTHER_DanhGia>()
                .Property(f => f.DanhGia)
                .HasColumnType("int[]");
            modelBuilder.Entity<ORTHER_DanhGia>()
                .Property(dg => dg.NgayDanhGia)
                .HasColumnType("DATE");

            // Cấu hình quan hệ giữa IN_DanhGia và IN_MauKhaoSat
            modelBuilder.Entity<ORTHER_DanhGia>()
                .HasOne(dg => dg.MauKhaoSat)
                .WithMany(mks => mks.DanhGia) // Assuming you have a collection property in IN_MauKhaoSat
                .HasForeignKey(dg => dg.IdORTHER_MauKhaoSat)
                .OnDelete(DeleteBehavior.Cascade);

            // Cấu hình quan hệ giữa IN_DanhGia và IN_ThongTinNguoiBenh
            modelBuilder.Entity<ORTHER_DanhGia>()
                .HasOne(dg => dg.ThongTinNguoiDanhGia)
                .WithMany(ttnb => ttnb.DanhGia);
            modelBuilder.Entity<ORTHER_DanhGia>()
                .Ignore(dg => dg.ThongTinNguoiDanhGia);
            modelBuilder.Entity<ORTHER_DanhGia>()
                .HasIndex(dg => new { dg.IdORTHER_MauKhaoSat, dg.IdORTHER_ThongTinNguoiDanhGia, dg.NgayDanhGia })
                .IsUnique();
            modelBuilder.Entity<ORTHER_ThongTinYKienKhac>()
                .Property(dg => dg.NgayTao)
                .HasColumnType("DATE");

            modelBuilder.Entity<FallbackChatbot>()
                .Property(f => f.CreatedDate)
                .HasDefaultValueSql("CURRENT_DATE");
        }
    }
}
