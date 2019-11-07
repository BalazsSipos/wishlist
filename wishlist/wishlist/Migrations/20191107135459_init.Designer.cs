﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using wishlist;

namespace wishlist.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191107135459_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("wishlist.Models.Event", b =>
                {
                    b.Property<long>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("AppUserId");

                    b.Property<DateTime>("EventDate");

                    b.Property<long?>("EventTypeId");

                    b.Property<string>("Message");

                    b.Property<string>("Name");

                    b.HasKey("EventId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("EventTypeId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("wishlist.Models.EventType", b =>
                {
                    b.Property<long>("EventTypeId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("EventTypeId");

                    b.ToTable("EventTypes");

                    b.HasData(
                        new
                        {
                            EventTypeId = 1L,
                            Name = "Christmas"
                        },
                        new
                        {
                            EventTypeId = 2L,
                            Name = "Wedding"
                        },
                        new
                        {
                            EventTypeId = 3L,
                            Name = "Birthday"
                        });
                });

            modelBuilder.Entity("wishlist.Models.Gift", b =>
                {
                    b.Property<long>("GiftId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("EventId");

                    b.Property<string>("GiftUrl");

                    b.Property<string>("Name");

                    b.Property<string>("PhotoUrl");

                    b.Property<long>("Price");

                    b.Property<int>("Quantity");

                    b.HasKey("GiftId");

                    b.HasIndex("EventId");

                    b.ToTable("Gifts");
                });

            modelBuilder.Entity("wishlist.Models.Identity.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("wishlist.Models.Invitation", b =>
                {
                    b.Property<long>("InvitationId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("EventId");

                    b.Property<string>("InvitedEmail");

                    b.Property<bool>("IsEmailSent");

                    b.HasKey("InvitationId");

                    b.HasIndex("EventId");

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("wishlist.Models.UserGift", b =>
                {
                    b.Property<long>("UserGiftId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BuyerUserId");

                    b.Property<long?>("GiftId");

                    b.HasKey("UserGiftId");

                    b.HasIndex("BuyerUserId");

                    b.HasIndex("GiftId");

                    b.ToTable("UserGifts");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("wishlist.Models.Identity.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("wishlist.Models.Identity.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("wishlist.Models.Identity.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("wishlist.Models.Identity.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("wishlist.Models.Event", b =>
                {
                    b.HasOne("wishlist.Models.Identity.AppUser", "AppUser")
                        .WithMany("Events")
                        .HasForeignKey("AppUserId");

                    b.HasOne("wishlist.Models.EventType", "EventType")
                        .WithMany()
                        .HasForeignKey("EventTypeId");
                });

            modelBuilder.Entity("wishlist.Models.Gift", b =>
                {
                    b.HasOne("wishlist.Models.Event", "Event")
                        .WithMany("Gifts")
                        .HasForeignKey("EventId");
                });

            modelBuilder.Entity("wishlist.Models.Invitation", b =>
                {
                    b.HasOne("wishlist.Models.Event", "Event")
                        .WithMany("Invitations")
                        .HasForeignKey("EventId");
                });

            modelBuilder.Entity("wishlist.Models.UserGift", b =>
                {
                    b.HasOne("wishlist.Models.Identity.AppUser", "BuyerUser")
                        .WithMany("ReservedGifts")
                        .HasForeignKey("BuyerUserId");

                    b.HasOne("wishlist.Models.Gift", "Gift")
                        .WithMany("BuyerUsers")
                        .HasForeignKey("GiftId");
                });
#pragma warning restore 612, 618
        }
    }
}
