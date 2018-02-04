﻿// <auto-generated />
using CamerackStudio.Models.DataBaseConnections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace CamerackStudio.Migrations
{
    [DbContext(typeof(CamerackStudioDataContext))]
    partial class CamerackStudioDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CamerackStudio.Models.Entities.AboutUs", b =>
                {
                    b.Property<long>("AboutUsId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("File");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Text");

                    b.HasKey("AboutUsId");

                    b.ToTable("AboutUs");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.Advertisement", b =>
                {
                    b.Property<long>("AdvertisementId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AdClick");

                    b.Property<string>("Client")
                        .IsRequired();

                    b.Property<string>("ClientEmail")
                        .IsRequired();

                    b.Property<string>("ClientPhoneNumber")
                        .IsRequired();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("File");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PageCategory")
                        .IsRequired();

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("Website");

                    b.HasKey("AdvertisementId");

                    b.ToTable("Advertisements");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.Camera", b =>
                {
                    b.Property<long>("CameraId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("CameraId");

                    b.ToTable("Cameras");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.Faq", b =>
                {
                    b.Property<long>("FaqId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Text");

                    b.HasKey("FaqId");

                    b.ToTable("Faqs");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.HeaderImage", b =>
                {
                    b.Property<long>("HeaderImageId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("File");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("PageCategory")
                        .IsRequired();

                    b.HasKey("HeaderImageId");

                    b.ToTable("HeaderImages");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.Image", b =>
                {
                    b.Property<long>("ImageId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("A1");

                    b.Property<bool>("A2");

                    b.Property<bool>("A3");

                    b.Property<bool>("A4");

                    b.Property<bool>("A5");

                    b.Property<bool>("A6");

                    b.Property<long?>("AppUserId");

                    b.Property<long?>("CameraId");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<long?>("Discount");

                    b.Property<bool?>("Featured");

                    b.Property<string>("FileName");

                    b.Property<string>("FilePath");

                    b.Property<long?>("Height");

                    b.Property<long?>("ImageCategoryId")
                        .IsRequired();

                    b.Property<long?>("ImageSubCategoryId");

                    b.Property<string>("Inspiration");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<long?>("LocationId");

                    b.Property<long?>("SellingPrice")
                        .IsRequired();

                    b.Property<string>("Status");

                    b.Property<string>("Tags");

                    b.Property<string>("Theme")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<string>("UploadCategory");

                    b.Property<long?>("Width");

                    b.HasKey("ImageId");

                    b.HasIndex("CameraId");

                    b.HasIndex("ImageCategoryId");

                    b.HasIndex("ImageSubCategoryId");

                    b.HasIndex("LocationId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.ImageAction", b =>
                {
                    b.Property<long>("ImageActionId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action");

                    b.Property<DateTime>("ActionDate");

                    b.Property<long?>("AppUserId");

                    b.Property<long>("ClientId");

                    b.Property<long>("ImageId");

                    b.Property<long?>("OwnerId");

                    b.Property<long?>("Rating");

                    b.HasKey("ImageActionId");

                    b.HasIndex("ImageId");

                    b.ToTable("ImageActions");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.ImageCategory", b =>
                {
                    b.Property<long>("ImageCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("FileName");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ImageCategoryId");

                    b.ToTable("ImageCategories");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.ImageComment", b =>
                {
                    b.Property<long>("ImageCommentId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AppUserId");

                    b.Property<string>("Comment");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("ImageId");

                    b.Property<long?>("LastModifiedBy");

                    b.HasKey("ImageCommentId");

                    b.HasIndex("ImageId");

                    b.ToTable("ImageComments");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.ImageReport", b =>
                {
                    b.Property<long>("ImageReportId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("ImageId");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Reason");

                    b.HasKey("ImageReportId");

                    b.HasIndex("ImageId");

                    b.ToTable("ImageReports");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.ImageSubCategory", b =>
                {
                    b.Property<long>("ImageSubCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("ImageCategoryId");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ImageSubCategoryId");

                    b.HasIndex("ImageCategoryId");

                    b.ToTable("ImageSubCategories");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.ImageTag", b =>
                {
                    b.Property<long>("ImageTagId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long>("ImageId");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ImageTagId");

                    b.HasIndex("ImageId");

                    b.ToTable("ImageTags");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.Location", b =>
                {
                    b.Property<long>("LocationId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("LocationId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.PhotographerCategory", b =>
                {
                    b.Property<long>("PhotographerCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Description");

                    b.Property<string>("FileName");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("PhotographerCategoryId");

                    b.ToTable("PhotographerCategories");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.PhotographerCategoryMapping", b =>
                {
                    b.Property<long>("PhotographerCategoryMappingId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("AppUserId");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<long>("PhotographerCategoryId");

                    b.HasKey("PhotographerCategoryMappingId");

                    b.HasIndex("PhotographerCategoryId");

                    b.ToTable("PhotographerCategoryMappings");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.PrivacyPolicy", b =>
                {
                    b.Property<long>("PrivacyPolicyId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Text");

                    b.HasKey("PrivacyPolicyId");

                    b.ToTable("PrivacyPolicies");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.SliderImage", b =>
                {
                    b.Property<long>("SliderImageId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("File");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("MainHeaderText")
                        .IsRequired();

                    b.Property<string>("SliderName")
                        .IsRequired();

                    b.Property<string>("Style");

                    b.Property<string>("SubHeaderText")
                        .IsRequired();

                    b.HasKey("SliderImageId");

                    b.ToTable("SliderImages");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.TermAndCondition", b =>
                {
                    b.Property<long>("TermAndConditionId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Text")
                        .IsRequired();

                    b.HasKey("TermAndConditionId");

                    b.ToTable("TermsAndConditions");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.UserBank", b =>
                {
                    b.Property<long>("UserBankId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountName");

                    b.Property<long?>("AccountNumber");

                    b.Property<string>("AccountType");

                    b.Property<long?>("BankId");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Tin");

                    b.HasKey("UserBankId");

                    b.ToTable("UserBanks");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.Image", b =>
                {
                    b.HasOne("CamerackStudio.Models.Entities.Camera", "Camera")
                        .WithMany("Images")
                        .HasForeignKey("CameraId");

                    b.HasOne("CamerackStudio.Models.Entities.ImageCategory", "ImageCategory")
                        .WithMany("Images")
                        .HasForeignKey("ImageCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CamerackStudio.Models.Entities.ImageSubCategory", "ImageSubCategory")
                        .WithMany()
                        .HasForeignKey("ImageSubCategoryId");

                    b.HasOne("CamerackStudio.Models.Entities.Location", "Location")
                        .WithMany("Images")
                        .HasForeignKey("LocationId");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.ImageAction", b =>
                {
                    b.HasOne("CamerackStudio.Models.Entities.Image", "Image")
                        .WithMany("ImageActions")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.ImageComment", b =>
                {
                    b.HasOne("CamerackStudio.Models.Entities.Image", "Image")
                        .WithMany("ImageComments")
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.ImageReport", b =>
                {
                    b.HasOne("CamerackStudio.Models.Entities.Image", "Image")
                        .WithMany("ImageReports")
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.ImageSubCategory", b =>
                {
                    b.HasOne("CamerackStudio.Models.Entities.ImageCategory", "ImageCategory")
                        .WithMany("ImageSubCategories")
                        .HasForeignKey("ImageCategoryId");
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.ImageTag", b =>
                {
                    b.HasOne("CamerackStudio.Models.Entities.Image", "Image")
                        .WithMany("ImageTags")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CamerackStudio.Models.Entities.PhotographerCategoryMapping", b =>
                {
                    b.HasOne("CamerackStudio.Models.Entities.PhotographerCategory", "PhotographerCategory")
                        .WithMany("PhotographerCategoryMappings")
                        .HasForeignKey("PhotographerCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
