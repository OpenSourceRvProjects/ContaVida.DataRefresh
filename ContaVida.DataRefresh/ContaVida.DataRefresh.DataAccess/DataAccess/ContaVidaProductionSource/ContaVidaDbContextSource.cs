using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ContaVida.DataRefresh.DataAccess.DataAccess.ContaVidaProductionSource;

public partial class ContaVidaDbContextSource : DbContext
{
    public ContaVidaDbContextSource()
    {
    }

    public ContaVidaDbContextSource(DbContextOptions<ContaVidaDbContextSource> options)
        : base(options)
    {
    }

    public virtual DbSet<CorrectLogin> CorrectLogins { get; set; }

    public virtual DbSet<EventCounter> EventCounters { get; set; }

    public virtual DbSet<PersonalProfile> PersonalProfiles { get; set; }

    public virtual DbSet<Relapse> Relapses { get; set; }

    public virtual DbSet<ResetLoginPassword> ResetLoginPasswords { get; set; }

    public virtual DbSet<SignUpRequest> SignUpRequests { get; set; }

    public virtual DbSet<SystemMaintenance> SystemMaintenances { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CorrectLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CorrectL__3214EC0749FE9937");

            entity.ToTable("CorrectLogin");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IpAddress).HasColumnName("IP_Address");
            entity.Property(e => e.LoginDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.CorrectLogins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CorrectLo__UserI__48CFD27E");
        });

        modelBuilder.Entity<EventCounter>(entity =>
        {
            entity.ToTable("EventCounter");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.RefreshMinutesTime).HasDefaultValueSql("(NULL)");
            entity.Property(e => e.Status)
                .HasDefaultValue(true)
                .HasColumnName("status");

            entity.HasOne(d => d.PersonalProfile).WithMany(p => p.EventCounters)
                .HasForeignKey(d => d.PersonalProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventCoun__Perso__49C3F6B7");

            entity.HasOne(d => d.User).WithMany(p => p.EventCounters)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EventCoun__UserI__4AB81AF0");
        });

        modelBuilder.Entity<PersonalProfile>(entity =>
        {
            entity.ToTable("PersonalProfile");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CounterLimit).HasDefaultValue(100);
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.RelapseLimit).HasDefaultValue(150);

            entity.HasOne(d => d.User).WithMany(p => p.PersonalProfiles)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PersonalP__UserI__4BAC3F29");
        });

        modelBuilder.Entity<Relapse>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.EventCounterId).HasColumnName("EventCounterID");
            entity.Property(e => e.PersonalProfileId).HasColumnName("PersonalProfileID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.EventCounter).WithMany(p => p.Relapses)
                .HasForeignKey(d => d.EventCounterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EventCounterRelapses");

            entity.HasOne(d => d.PersonalProfile).WithMany(p => p.Relapses)
                .HasForeignKey(d => d.PersonalProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PersonalProfileRelapses");

            entity.HasOne(d => d.User).WithMany(p => p.Relapses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserRelapses");
        });

        modelBuilder.Entity<ResetLoginPassword>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ResetLog__3214EC07B4F356D7");

            entity.ToTable("ResetLoginPassword");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.ExpirationDate).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.ResetLoginPasswords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ResetLogi__UserI__4F7CD00D");
        });

        modelBuilder.Entity<SignUpRequest>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.Ip).HasColumnName("IP");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.SignUpRequests)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__SignUpReq__UserI__5070F446");
        });

        modelBuilder.Entity<SystemMaintenance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SystemMa__3214EC07F0CADF95");

            entity.ToTable("SystemMaintenance");

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
