﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using feedBackMvc.Models;

#nullable disable

namespace feedBackMvc.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20240831131317_OUT_NhomCauHoiKhoaSatModel")]
    partial class OUT_NhomCauHoiKhoaSatModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("feedBackMvc.Models.Admins", b =>
                {
                    b.Property<int>("idAdmin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("idAdmin"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MatKhau")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Ten")
                        .HasColumnType("text");

                    b.HasKey("idAdmin");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_CauHoiKhaoSat", b =>
                {
                    b.Property<int>("IdIN_CauHoiKhaoSat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdIN_CauHoiKhaoSat"));

                    b.Property<string>("CauHoi")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("IdIN_NhomCauHoiKhaoSat")
                        .HasColumnType("integer");

                    b.Property<string>("TieuDeCauHoi")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdIN_CauHoiKhaoSat");

                    b.HasIndex("IdIN_NhomCauHoiKhaoSat");

                    b.HasIndex("TieuDeCauHoi")
                        .IsUnique();

                    b.ToTable("IN_CauHoiKhaoSat");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_DanhGia", b =>
                {
                    b.Property<int>("IdIN_DanhGia")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdIN_DanhGia"));

                    b.Property<int[]>("DanhGia")
                        .IsRequired()
                        .HasColumnType("int[]");

                    b.Property<int>("IdIN_MauKhaoSat")
                        .HasColumnType("integer");

                    b.Property<int>("IdIN_ThongTinNguoiBenh")
                        .HasColumnType("integer");

                    b.Property<DateTime>("NgayDanhGia")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("IdIN_DanhGia");

                    b.HasIndex("IdIN_MauKhaoSat");

                    b.HasIndex("IdIN_ThongTinNguoiBenh");

                    b.ToTable("IN_DanhGia");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_MauKhaoSat", b =>
                {
                    b.Property<int>("IdIN_MauKhaoSat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdIN_MauKhaoSat"));

                    b.Property<string[]>("CauHoiKhaoSat")
                        .HasColumnType("text[]");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string[]>("NhomCauHoiKhaoSat")
                        .HasColumnType("text[]");

                    b.Property<string>("TenMauKhaoSat")
                        .HasColumnType("text");

                    b.Property<int>("idAdmin")
                        .HasColumnType("integer");

                    b.HasKey("IdIN_MauKhaoSat");

                    b.HasIndex("idAdmin");

                    b.ToTable("IN_MauKhaoSat");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_NhomCauHoiKhaoSat", b =>
                {
                    b.Property<int>("IdIN_NhomCauHoiKhaoSat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdIN_NhomCauHoiKhaoSat"));

                    b.Property<string>("NoiDung")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TieuDe")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<int>("idAdmin")
                        .HasColumnType("integer");

                    b.HasKey("IdIN_NhomCauHoiKhaoSat");

                    b.HasIndex("TieuDe")
                        .IsUnique();

                    b.HasIndex("idAdmin");

                    b.ToTable("IN_NhomCauHoiKhaoSat");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_ThongTinChung", b =>
                {
                    b.Property<int>("IdIN_ThongTinChung")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdIN_ThongTinChung"));

                    b.Property<int>("IdIN_ThongTinNguoiBenh")
                        .HasColumnType("integer");

                    b.Property<string>("MaKhoa")
                        .HasColumnType("text");

                    b.Property<DateTime>("NgayDienPhieu")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NguoiTraLoi")
                        .HasColumnType("text");

                    b.Property<string>("TenBenhVien")
                        .HasColumnType("text");

                    b.Property<string>("TenKhoa")
                        .HasColumnType("text");

                    b.HasKey("IdIN_ThongTinChung");

                    b.HasIndex("IdIN_ThongTinNguoiBenh");

                    b.ToTable("IN_ThongTinChung");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_ThongTinNguoiBenh", b =>
                {
                    b.Property<int>("IdIN_ThongTinNguoiBenh")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdIN_ThongTinNguoiBenh"));

                    b.Property<bool?>("CoSuDungBHYT")
                        .HasColumnType("boolean");

                    b.Property<string>("GioiTinh")
                        .HasColumnType("text");

                    b.Property<string>("SoDienThoai")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("SoNgayNamVien")
                        .HasColumnType("integer");

                    b.Property<int?>("Tuoi")
                        .HasColumnType("integer");

                    b.HasKey("IdIN_ThongTinNguoiBenh");

                    b.ToTable("IN_ThongTinNguoiBenh");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_ThongTinYKienKhac", b =>
                {
                    b.Property<int>("IdIN_ThongTinYKienKhac")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdIN_ThongTinYKienKhac"));

                    b.Property<int>("IdIN_ThongTinNguoiBenh")
                        .HasColumnType("integer");

                    b.Property<DateTime>("NgayTao")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("PhanTramMongDoi")
                        .HasColumnType("integer");

                    b.Property<string>("QuayLaiVaGioiThieu")
                        .HasColumnType("text");

                    b.Property<string>("YKienKhac")
                        .HasColumnType("text");

                    b.HasKey("IdIN_ThongTinYKienKhac");

                    b.HasIndex("IdIN_ThongTinNguoiBenh");

                    b.ToTable("IN_ThongTinYKienKhac");
                });

            modelBuilder.Entity("feedBackMvc.Models.OUT_CauHoiKhaoSat", b =>
                {
                    b.Property<int>("IdOUT_CauHoiKhaoSat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdOUT_CauHoiKhaoSat"));

                    b.Property<string>("CauHoi")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("IdOUT_NhomCauHoiKhaoSat")
                        .HasColumnType("integer");

                    b.Property<string>("TieuDeCauHoi")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("IdOUT_CauHoiKhaoSat");

                    b.HasIndex("IdOUT_NhomCauHoiKhaoSat");

                    b.HasIndex("TieuDeCauHoi")
                        .IsUnique();

                    b.ToTable("OUT_CauHoiKhaoSat");
                });

            modelBuilder.Entity("feedBackMvc.Models.OUT_NhomCauHoiKhaoSat", b =>
                {
                    b.Property<int>("IdOUT_NhomCauHoiKhaoSat")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("IdOUT_NhomCauHoiKhaoSat"));

                    b.Property<string>("NoiDung")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TieuDe")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)");

                    b.Property<int>("idAdmin")
                        .HasColumnType("integer");

                    b.HasKey("IdOUT_NhomCauHoiKhaoSat");

                    b.HasIndex("TieuDe")
                        .IsUnique();

                    b.HasIndex("idAdmin");

                    b.ToTable("OUT_NhomCauHoiKhaoSat");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_CauHoiKhaoSat", b =>
                {
                    b.HasOne("feedBackMvc.Models.IN_NhomCauHoiKhaoSat", "NhomCauHoiKhaoSat")
                        .WithMany("CauHoiKhaoSats")
                        .HasForeignKey("IdIN_NhomCauHoiKhaoSat")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NhomCauHoiKhaoSat");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_DanhGia", b =>
                {
                    b.HasOne("feedBackMvc.Models.IN_MauKhaoSat", "MauKhaoSat")
                        .WithMany()
                        .HasForeignKey("IdIN_MauKhaoSat")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("feedBackMvc.Models.IN_ThongTinNguoiBenh", "ThongTinNguoiBenh")
                        .WithMany()
                        .HasForeignKey("IdIN_ThongTinNguoiBenh")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MauKhaoSat");

                    b.Navigation("ThongTinNguoiBenh");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_MauKhaoSat", b =>
                {
                    b.HasOne("feedBackMvc.Models.Admins", "admins")
                        .WithMany()
                        .HasForeignKey("idAdmin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("admins");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_NhomCauHoiKhaoSat", b =>
                {
                    b.HasOne("feedBackMvc.Models.Admins", "Admins")
                        .WithMany()
                        .HasForeignKey("idAdmin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admins");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_ThongTinChung", b =>
                {
                    b.HasOne("feedBackMvc.Models.IN_ThongTinNguoiBenh", "ThongTinNguoiBenh")
                        .WithMany()
                        .HasForeignKey("IdIN_ThongTinNguoiBenh")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ThongTinNguoiBenh");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_ThongTinYKienKhac", b =>
                {
                    b.HasOne("feedBackMvc.Models.IN_ThongTinNguoiBenh", "ThongTinNguoiBenh")
                        .WithMany()
                        .HasForeignKey("IdIN_ThongTinNguoiBenh")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ThongTinNguoiBenh");
                });

            modelBuilder.Entity("feedBackMvc.Models.OUT_CauHoiKhaoSat", b =>
                {
                    b.HasOne("feedBackMvc.Models.OUT_NhomCauHoiKhaoSat", "NhomCauHoiKhaoSat")
                        .WithMany("CauHoiKhaoSats")
                        .HasForeignKey("IdOUT_NhomCauHoiKhaoSat")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("NhomCauHoiKhaoSat");
                });

            modelBuilder.Entity("feedBackMvc.Models.OUT_NhomCauHoiKhaoSat", b =>
                {
                    b.HasOne("feedBackMvc.Models.Admins", "Admins")
                        .WithMany()
                        .HasForeignKey("idAdmin")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Admins");
                });

            modelBuilder.Entity("feedBackMvc.Models.IN_NhomCauHoiKhaoSat", b =>
                {
                    b.Navigation("CauHoiKhaoSats");
                });

            modelBuilder.Entity("feedBackMvc.Models.OUT_NhomCauHoiKhaoSat", b =>
                {
                    b.Navigation("CauHoiKhaoSats");
                });
#pragma warning restore 612, 618
        }
    }
}
