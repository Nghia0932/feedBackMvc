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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình ràng buộc UNIQUE cho cột TieuDe trong bảng IN_NhomCauHoiKhaoSat
            modelBuilder.Entity<IN_NhomCauHoiKhaoSat>()
                .HasIndex(t => t.TieuDe)
                .IsUnique();

            // Giới hạn độ dài của cột TieuDe
            modelBuilder.Entity<IN_NhomCauHoiKhaoSat>()
                .Property(n => n.TieuDe)
                .HasMaxLength(5);

            // Cấu hình ràng buộc UNIQUE cho cột TieuDeCauHoi trong bảng IN_CauHoiKhaoSat
            modelBuilder.Entity<IN_CauHoiKhaoSat>()
                .HasIndex(t => t.TieuDeCauHoi)
                .IsUnique();

            // Cấu hình quan hệ giữa IN_NhomCauHoiKhaoSat và IN_CauHoiKhaoSat
            modelBuilder.Entity<IN_NhomCauHoiKhaoSat>()
                .HasMany(n => n.CauHoiKhaoSats)
                .WithOne(c => c.NhomCauHoiKhaoSat)
                .HasForeignKey(c => c.IdIN_NhomCauHoiKhaoSat);

            // Cấu hình các cột kiểu mảng cho PostgreSQL
            modelBuilder.Entity<IN_DanhGia>()
                .Property(f => f.DanhGia)
                .HasColumnType("int[]");

            // Additional configurations if needed
        }    
    }
}
