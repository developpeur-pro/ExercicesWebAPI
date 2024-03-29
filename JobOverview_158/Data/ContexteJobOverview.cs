﻿using JobOverview_v158.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Version = JobOverview_v158.Entities.Version;

namespace JobOverview_v158.Data
{
	public class ContexteJobOverview : DbContext
	{
		public ContexteJobOverview(DbContextOptions options) : base(options)
		{
		}

		public virtual DbSet<Filiere> Filieres { get; set; }
		public virtual DbSet<Logiciel> Logiciels { get; set; }
		public virtual DbSet<Module> Modules { get; set; }
		public virtual DbSet<Version> Versions { get; set; }
		public virtual DbSet<Release> Releases { get; set; }

		public virtual DbSet<Service> Services { get; set; }
		public virtual DbSet<Equipe> Equipes { get; set; }
		public virtual DbSet<Personne> Personnes { get; set; }
		public virtual DbSet<Metier> Métiers { get; set; }

		public virtual DbSet<Activite> Activites { get; set; }
		public virtual DbSet<ActiviteMetier> ActivitesMetiers { get; set; }
		public virtual DbSet<Tache> Taches { get; set; }
		public virtual DbSet<Travail> Travaux { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			#region Logiciels
			modelBuilder.Entity<Filiere>(entity =>
			{
				entity.HasKey(e => e.Code);

				entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.Nom).HasMaxLength(60);
			});

			modelBuilder.Entity<Logiciel>(entity =>
			{
				entity.HasKey(e => e.Code);

				entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.CodeFiliere).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.Nom).HasMaxLength(60).IsUnicode(false);

				entity.HasOne<Filiere>().WithMany().HasForeignKey(d => d.CodeFiliere)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Module>(entity =>
			{
				entity.HasKey(e => new { e.Code, e.CodeLogiciel });

				entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.CodeLogiciel).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.CodeLogicielParent).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.CodeModuleParent).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.Nom).HasMaxLength(60).IsUnicode(false);

				entity.HasOne<Logiciel>().WithMany(l => l.Modules).HasForeignKey(d => d.CodeLogiciel)
							.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne<Module>().WithMany(m => m.SousModules)
						.HasForeignKey(d => new { d.CodeModuleParent, d.CodeLogicielParent })
						.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Version>(entity =>
			{
				entity.HasKey(e => new { e.Numero, e.CodeLogiciel });

				entity.Property(e => e.Numero);
				entity.Property(e => e.CodeLogiciel).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.DateOuverture);
				entity.Property(e => e.DateSortiePrevue);
				entity.Property(e => e.DateSortieReelle);

				entity.HasOne<Logiciel>().WithMany().HasForeignKey(d => d.CodeLogiciel)
						.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Release>(entity =>
			{
				entity.HasKey(e => new { e.Numero, e.NumeroVersion, e.CodeLogiciel });

				entity.Property(e => e.NumeroVersion);
				entity.Property(e => e.CodeLogiciel).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.DatePubli);

				entity.HasOne<Version>().WithMany(v => v.Releases)
						.HasForeignKey(d => new { d.NumeroVersion, d.CodeLogiciel });
			});

			#endregion

			#region Equipes
			modelBuilder.Entity<Service>(entity =>
			{
				entity.HasKey(e => e.Code);

				entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.Nom).HasMaxLength(60);
			});

			modelBuilder.Entity<Equipe>(entity =>
			{
				entity.HasKey(e => e.Code);

				entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.Nom).HasMaxLength(60).IsUnicode(false);
				entity.Property(e => e.CodeService).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.CodeFiliere).HasMaxLength(20).IsUnicode(false);

				entity.HasOne<Filiere>().WithMany(f => f.Equipes)
					.HasForeignKey(d => d.CodeFiliere)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(s => s.Service).WithMany()
					.HasForeignKey(d => d.CodeService)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Metier>(entity =>
			{
				entity.ToTable("Metiers");
				entity.HasKey(e => e.Code);

				entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.Titre).HasMaxLength(60).IsUnicode(false);
				entity.Property(e => e.CodeService).HasMaxLength(20).IsUnicode(false);

				entity.HasOne<Service>().WithMany()
					.HasForeignKey(d => d.CodeService)
					.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Personne>(entity =>
			{
				entity.HasKey(e => e.Pseudo);

				entity.Property(e => e.Pseudo).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.Nom).HasMaxLength(60);
				entity.Property(e => e.Prenom).HasMaxLength(60);
				entity.Property(e => e.TauxProductivite).HasColumnType("decimal(3,2)").HasDefaultValue(1m);
				entity.Property(e => e.CodeEquipe).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.CodeMetier).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.Manager).HasMaxLength(20).IsUnicode(false);

				entity.HasOne<Equipe>().WithMany(eq => eq.Personnes)
					.HasForeignKey(d => d.CodeEquipe)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne<Personne>().WithMany()
					.HasForeignKey(d => d.Manager)
					.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne(p => p.Métier).WithMany()
					.HasForeignKey(d => d.CodeMetier)
					.OnDelete(DeleteBehavior.NoAction);
			});

			#endregion

			#region Taches
			modelBuilder.Entity<Activite>(entity =>
			{
				entity.HasKey(e => e.Code);

				entity.Property(e => e.Code).HasMaxLength(20).IsUnicode(false);
				entity.Property(e => e.Titre).HasMaxLength(60);

				entity.HasMany<Metier>().WithMany(m => m.Activités).UsingEntity<ActiviteMetier>(
					l => l.HasOne<Metier>().WithMany().HasForeignKey(am => am.CodeMetier),
					r => r.HasOne<Activite>().WithMany().HasForeignKey(am => am.CodeActivite));
			});

			modelBuilder.Entity<Tache>(entity =>
			{
				entity.Property(e => e.Titre).HasMaxLength(60);
				entity.Property(e => e.DureePrevue).HasColumnType("decimal(3, 1)");
				entity.Property(e => e.DureeRestante).HasColumnType("decimal(3, 1)");
				entity.Property(e => e.Description).HasMaxLength(1000);
				entity.Property(e => e.Vers).IsConcurrencyToken();

				entity.HasOne<Activite>().WithMany().HasForeignKey(d => d.CodeActivite)
						.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne<Personne>().WithMany().HasForeignKey(d => d.Personne)
						.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne<Module>().WithMany().HasForeignKey(d => new { d.CodeModule, d.CodeLogiciel })
						.OnDelete(DeleteBehavior.NoAction);

				entity.HasOne<Version>().WithMany().HasForeignKey(d => new { d.NumVersion, d.CodeLogiciel })
						.OnDelete(DeleteBehavior.NoAction);
			});

			modelBuilder.Entity<Travail>(entity =>
			{
				entity.HasKey(e => new { e.DateTravail, e.IdTache });

				entity.Property(e => e.Heures).HasColumnType("decimal(3, 1)");
				entity.Property(e => e.TauxProductivite).HasColumnType("decimal(3, 2)").HasDefaultValue(1m);

				entity.HasOne<Tache>().WithMany(t => t.Travaux).HasForeignKey(d => d.IdTache);
			});
			#endregion

			if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
			{
				JeuDonnées.Créer(modelBuilder);
			}
		}
	}
}
