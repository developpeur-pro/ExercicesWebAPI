﻿// <auto-generated />
using System;
using JobOverview_v158.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JobOverview_v158.Data.Migrations
{
    [DbContext(typeof(ContexteJobOverview))]
    [Migration("20230928070424_CreationBase")]
    partial class CreationBase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("JobOverview.Entities.Filiere", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.HasKey("Code");

                    b.ToTable("Filieres");
                });

            modelBuilder.Entity("JobOverview.Entities.Logiciel", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeFiliere")
                        .IsRequired()
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)");

                    b.HasKey("Code");

                    b.HasIndex("CodeFiliere");

                    b.ToTable("Logiciels");
                });

            modelBuilder.Entity("JobOverview.Entities.Module", b =>
                {
                    b.Property<string>("Code")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeLogiciel")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeLogicielParent")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("CodeModuleParent")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasMaxLength(60)
                        .IsUnicode(false)
                        .HasColumnType("varchar(60)");

                    b.HasKey("Code", "CodeLogiciel");

                    b.HasIndex("CodeLogiciel");

                    b.HasIndex("CodeModuleParent", "CodeLogicielParent");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("JobOverview.Entities.Release", b =>
                {
                    b.Property<short>("Numero")
                        .HasColumnType("smallint");

                    b.Property<float>("NumeroVersion")
                        .HasColumnType("real");

                    b.Property<string>("CodeLogiciel")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("DatePubli")
                        .HasColumnType("datetime2");

                    b.HasKey("Numero", "NumeroVersion", "CodeLogiciel");

                    b.HasIndex("NumeroVersion", "CodeLogiciel");

                    b.ToTable("Releases");
                });

            modelBuilder.Entity("JobOverview.Entities.Version", b =>
                {
                    b.Property<float>("Numero")
                        .HasColumnType("real");

                    b.Property<string>("CodeLogiciel")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<DateTime>("DateOuverture")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateSortiePrevue")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DateSortieReelle")
                        .HasColumnType("datetime2");

                    b.Property<short>("Millesime")
                        .HasColumnType("smallint");

                    b.HasKey("Numero", "CodeLogiciel");

                    b.HasIndex("CodeLogiciel");

                    b.ToTable("Versions");
                });

            modelBuilder.Entity("JobOverview.Entities.Logiciel", b =>
                {
                    b.HasOne("JobOverview.Entities.Filiere", null)
                        .WithMany()
                        .HasForeignKey("CodeFiliere")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });

            modelBuilder.Entity("JobOverview.Entities.Module", b =>
                {
                    b.HasOne("JobOverview.Entities.Logiciel", null)
                        .WithMany()
                        .HasForeignKey("CodeLogiciel")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("JobOverview.Entities.Module", null)
                        .WithMany()
                        .HasForeignKey("CodeModuleParent", "CodeLogicielParent")
                        .OnDelete(DeleteBehavior.NoAction);
                });

            modelBuilder.Entity("JobOverview.Entities.Release", b =>
                {
                    b.HasOne("JobOverview.Entities.Version", null)
                        .WithMany()
                        .HasForeignKey("NumeroVersion", "CodeLogiciel")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("JobOverview.Entities.Version", b =>
                {
                    b.HasOne("JobOverview.Entities.Logiciel", null)
                        .WithMany()
                        .HasForeignKey("CodeLogiciel")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
