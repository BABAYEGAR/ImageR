﻿// <auto-generated />
using Image.Models.DataBaseConnections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Image.Migrations
{
    [DbContext(typeof(ImageDataContext))]
    [Migration("20171024003819_Migrate15")]
    partial class Migrate15
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Image.Models.Entities.AppUser", b =>
                {
                    b.Property<long>("AppUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address");

                    b.Property<string>("BackgroundPicture");

                    b.Property<string>("Biography");

                    b.Property<string>("ConfirmPassword")
                        .IsRequired();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("ProfilePicture");

                    b.Property<string>("Rank");

                    b.Property<long?>("RoleId")
                        .IsRequired();

                    b.Property<string>("Status");

                    b.Property<string>("Username")
                        .IsRequired();

                    b.Property<string>("Website");

                    b.HasKey("AppUserId");

                    b.HasIndex("RoleId");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("Image.Models.Entities.AppUserAccessKey", b =>
                {
                    b.Property<long>("AppUserAccessKeyId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AccountActivationAccessCode");

                    b.Property<long>("AppUserId");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<DateTime?>("ExpiryDate");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("PasswordAccessCode");

                    b.HasKey("AppUserAccessKeyId");

                    b.HasIndex("AppUserId");

                    b.ToTable("AccessKeys");
                });

            modelBuilder.Entity("Image.Models.Entities.BillingAddress", b =>
                {
                    b.Property<long>("BillingAddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AppUserId");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("LandMark");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("State");

                    b.Property<string>("Street");

                    b.HasKey("BillingAddressId");

                    b.HasIndex("AppUserId");

                    b.ToTable("BillingAddress");
                });

            modelBuilder.Entity("Image.Models.Entities.Camera", b =>
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

            modelBuilder.Entity("Image.Models.Entities.Cart", b =>
                {
                    b.Property<long>("CartId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AppUserId");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("ImageId");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<long?>("Quantity");

                    b.HasKey("CartId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("ImageId");

                    b.ToTable("Carts");
                });

            modelBuilder.Entity("Image.Models.Entities.Competition", b =>
                {
                    b.Property<long>("CompetitionId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AppUserId");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<DateTime?>("EndDate")
                        .IsRequired();

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<DateTime?>("StartDate")
                        .IsRequired();

                    b.Property<string>("Status");

                    b.Property<string>("Theme")
                        .IsRequired();

                    b.HasKey("CompetitionId");

                    b.HasIndex("AppUserId");

                    b.ToTable("Competition");
                });

            modelBuilder.Entity("Image.Models.Entities.CompetitionCategory", b =>
                {
                    b.Property<long>("CompetitionCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("CompetitionId");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<long>("PhotographerCategoryId");

                    b.HasKey("CompetitionCategoryId");

                    b.HasIndex("CompetitionId");

                    b.HasIndex("PhotographerCategoryId");

                    b.ToTable("CompetitionCategories");
                });

            modelBuilder.Entity("Image.Models.Entities.CompetitionUpload", b =>
                {
                    b.Property<long>("CompetitionUploadId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AppUserId");

                    b.Property<long?>("CameraId");

                    b.Property<long>("CompetitionId");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Description");

                    b.Property<string>("FileName");

                    b.Property<string>("FilePath");

                    b.Property<string>("Inspiration");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<long?>("LocationId");

                    b.Property<string>("Title");

                    b.HasKey("CompetitionUploadId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("CameraId");

                    b.HasIndex("CompetitionId");

                    b.HasIndex("LocationId");

                    b.ToTable("CompetitionUploads");
                });

            modelBuilder.Entity("Image.Models.Entities.CompetitionVote", b =>
                {
                    b.Property<long>("CompetitionVoteId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AppUserId");

                    b.Property<long?>("CompetitionId");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<long>("Votes");

                    b.HasKey("CompetitionVoteId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("CompetitionId");

                    b.ToTable("CompetitionVote");
                });

            modelBuilder.Entity("Image.Models.Entities.Image", b =>
                {
                    b.Property<long>("ImageId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AppUserId");

                    b.Property<long?>("CameraId");

                    b.Property<string>("Caption");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("FileName");

                    b.Property<string>("FilePath");

                    b.Property<long?>("ImageCategoryId")
                        .IsRequired();

                    b.Property<long?>("ImageSubCategoryId");

                    b.Property<string>("Inspiration");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<long?>("LocationId");

                    b.Property<string>("SellingPrice")
                        .IsRequired();

                    b.Property<string>("Theme")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("ImageId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("CameraId");

                    b.HasIndex("ImageCategoryId");

                    b.HasIndex("ImageSubCategoryId");

                    b.HasIndex("LocationId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Image.Models.Entities.ImageCategory", b =>
                {
                    b.Property<long>("ImageCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ImageCategoryId");

                    b.ToTable("ImageCategories");
                });

            modelBuilder.Entity("Image.Models.Entities.ImageClick", b =>
                {
                    b.Property<long>("ImageClickId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long>("DisLike");

                    b.Property<long?>("ImageId");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<long>("Like");

                    b.HasKey("ImageClickId");

                    b.HasIndex("ImageId");

                    b.ToTable("ImageClicks");
                });

            modelBuilder.Entity("Image.Models.Entities.ImageComment", b =>
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

                    b.HasIndex("AppUserId");

                    b.HasIndex("ImageId");

                    b.ToTable("ImageComments");
                });

            modelBuilder.Entity("Image.Models.Entities.ImageRating", b =>
                {
                    b.Property<long>("ImageRatingId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("ImageId");

                    b.Property<long?>("Rating");

                    b.HasKey("ImageRatingId");

                    b.HasIndex("ImageId");

                    b.ToTable("ImageRatings");
                });

            modelBuilder.Entity("Image.Models.Entities.ImageSubCategory", b =>
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

            modelBuilder.Entity("Image.Models.Entities.ImageTag", b =>
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

            modelBuilder.Entity("Image.Models.Entities.Location", b =>
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

            modelBuilder.Entity("Image.Models.Entities.Order", b =>
                {
                    b.Property<long>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AppUserId");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("ImageId");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("OrderNumber");

                    b.HasKey("OrderId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("ImageId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Image.Models.Entities.Package", b =>
                {
                    b.Property<long>("PackageId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("Amount")
                        .IsRequired();

                    b.Property<bool>("Competition");

                    b.Property<bool>("Contracts");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<bool>("ImagePriority");

                    b.Property<long?>("ImageUploadNumber");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("PackageId");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("Image.Models.Entities.PackageItem", b =>
                {
                    b.Property<long>("PackageItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<long?>("PackageId");

                    b.HasKey("PackageItemId");

                    b.HasIndex("PackageId");

                    b.ToTable("PackageItem");
                });

            modelBuilder.Entity("Image.Models.Entities.Payment", b =>
                {
                    b.Property<long>("PaymentId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("Amount");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("Description");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("PaymentMethod");

                    b.HasKey("PaymentId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Image.Models.Entities.PhotographerCategory", b =>
                {
                    b.Property<long>("PhotographerCategoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("PhotographerCategoryId");

                    b.ToTable("PhotographerCategories");
                });

            modelBuilder.Entity("Image.Models.Entities.PhotographerCategoryMapping", b =>
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

                    b.HasIndex("AppUserId");

                    b.HasIndex("PhotographerCategoryId");

                    b.ToTable("PhotographerCategoryMappings");
                });

            modelBuilder.Entity("Image.Models.Entities.Role", b =>
                {
                    b.Property<long>("RoleId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<bool>("ManageApplicationUser");

                    b.Property<bool>("ManageCameras");

                    b.Property<bool>("ManageCompetition");

                    b.Property<bool>("ManageImageCategory");

                    b.Property<bool>("ManageImages");

                    b.Property<bool>("ManageLocations");

                    b.Property<bool>("ManageOrders");

                    b.Property<bool>("ManagePackages");

                    b.Property<bool>("ManagePayments");

                    b.Property<bool>("ManagePhotographerCategory");

                    b.Property<bool>("ManageRoles");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<bool>("ParticipateCompetition");

                    b.Property<bool>("PurchaseImage");

                    b.Property<bool>("UploadImage");

                    b.HasKey("RoleId");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("Image.Models.Entities.ShippingAddress", b =>
                {
                    b.Property<long>("ShippingAddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AppUserId");

                    b.Property<string>("City");

                    b.Property<string>("Country");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<string>("LandMark");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("State");

                    b.Property<string>("Street");

                    b.HasKey("ShippingAddressId");

                    b.HasIndex("AppUserId");

                    b.ToTable("ShippingAddress");
                });

            modelBuilder.Entity("Image.Models.Entities.SystemNotification", b =>
                {
                    b.Property<long>("SystemNotificationId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AppUserId");

                    b.Property<long?>("ControllerId");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<string>("Message");

                    b.Property<bool?>("Read");

                    b.HasKey("SystemNotificationId");

                    b.HasIndex("AppUserId");

                    b.ToTable("SystemNotifications");
                });

            modelBuilder.Entity("Image.Models.Entities.UserSubscription", b =>
                {
                    b.Property<long>("UserSubscriptionId")
                        .ValueGeneratedOnAdd();

                    b.Property<long?>("AppUserId");

                    b.Property<long?>("CreatedBy");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateLastModified");

                    b.Property<long?>("LastModifiedBy");

                    b.Property<long?>("PackageId");

                    b.Property<string>("Status");

                    b.HasKey("UserSubscriptionId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("PackageId");

                    b.ToTable("UserSubscriptions");
                });

            modelBuilder.Entity("Image.Models.Entities.AppUser", b =>
                {
                    b.HasOne("Image.Models.Entities.Role", "Role")
                        .WithMany("AppUsers")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Image.Models.Entities.AppUserAccessKey", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Image.Models.Entities.BillingAddress", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany("BillingAddresses")
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("Image.Models.Entities.Cart", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany("Carts")
                        .HasForeignKey("AppUserId");

                    b.HasOne("Image.Models.Entities.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("Image.Models.Entities.Competition", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany("Competitions")
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("Image.Models.Entities.CompetitionCategory", b =>
                {
                    b.HasOne("Image.Models.Entities.Competition", "Competition")
                        .WithMany("CompetitionCategories")
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Image.Models.Entities.PhotographerCategory", "PhotographerCategory")
                        .WithMany()
                        .HasForeignKey("PhotographerCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Image.Models.Entities.CompetitionUpload", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany("CompetitionUploads")
                        .HasForeignKey("AppUserId");

                    b.HasOne("Image.Models.Entities.Camera", "Camera")
                        .WithMany("CompetitionUploads")
                        .HasForeignKey("CameraId");

                    b.HasOne("Image.Models.Entities.Competition", "Competition")
                        .WithMany("CompetitionUploads")
                        .HasForeignKey("CompetitionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Image.Models.Entities.Location", "Location")
                        .WithMany("CompetitionUploads")
                        .HasForeignKey("LocationId");
                });

            modelBuilder.Entity("Image.Models.Entities.CompetitionVote", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany("CompetitionVotes")
                        .HasForeignKey("AppUserId");

                    b.HasOne("Image.Models.Entities.Competition")
                        .WithMany("CompetitionVotes")
                        .HasForeignKey("CompetitionId");
                });

            modelBuilder.Entity("Image.Models.Entities.Image", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany("Images")
                        .HasForeignKey("AppUserId");

                    b.HasOne("Image.Models.Entities.Camera", "Camera")
                        .WithMany("Images")
                        .HasForeignKey("CameraId");

                    b.HasOne("Image.Models.Entities.ImageCategory", "ImageCategory")
                        .WithMany("Images")
                        .HasForeignKey("ImageCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Image.Models.Entities.ImageSubCategory", "ImageSubCategory")
                        .WithMany()
                        .HasForeignKey("ImageSubCategoryId");

                    b.HasOne("Image.Models.Entities.Location", "Location")
                        .WithMany("Images")
                        .HasForeignKey("LocationId");
                });

            modelBuilder.Entity("Image.Models.Entities.ImageClick", b =>
                {
                    b.HasOne("Image.Models.Entities.Image", "Image")
                        .WithMany("ImageClicks")
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("Image.Models.Entities.ImageComment", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany("ImageComments")
                        .HasForeignKey("AppUserId");

                    b.HasOne("Image.Models.Entities.Image", "Image")
                        .WithMany("ImageComments")
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("Image.Models.Entities.ImageRating", b =>
                {
                    b.HasOne("Image.Models.Entities.Image", "Image")
                        .WithMany("ImageRatings")
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("Image.Models.Entities.ImageSubCategory", b =>
                {
                    b.HasOne("Image.Models.Entities.ImageCategory", "ImageCategory")
                        .WithMany("ImageSubCategories")
                        .HasForeignKey("ImageCategoryId");
                });

            modelBuilder.Entity("Image.Models.Entities.ImageTag", b =>
                {
                    b.HasOne("Image.Models.Entities.Image", "Image")
                        .WithMany("ImageTags")
                        .HasForeignKey("ImageId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Image.Models.Entities.Order", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany("Orders")
                        .HasForeignKey("AppUserId");

                    b.HasOne("Image.Models.Entities.Image", "Image")
                        .WithMany()
                        .HasForeignKey("ImageId");
                });

            modelBuilder.Entity("Image.Models.Entities.PackageItem", b =>
                {
                    b.HasOne("Image.Models.Entities.Package", "Package")
                        .WithMany("PackageItems")
                        .HasForeignKey("PackageId");
                });

            modelBuilder.Entity("Image.Models.Entities.PhotographerCategoryMapping", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany("PhotographerCategoryMappings")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Image.Models.Entities.PhotographerCategory", "PhotographerCategory")
                        .WithMany("PhotographerCategoryMappings")
                        .HasForeignKey("PhotographerCategoryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Image.Models.Entities.ShippingAddress", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany("ShippingAddresses")
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("Image.Models.Entities.SystemNotification", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany("SystemNotifications")
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("Image.Models.Entities.UserSubscription", b =>
                {
                    b.HasOne("Image.Models.Entities.AppUser", "AppUser")
                        .WithMany("UserSubscriptions")
                        .HasForeignKey("AppUserId");

                    b.HasOne("Image.Models.Entities.Package", "Package")
                        .WithMany("UserSubscriptions")
                        .HasForeignKey("PackageId");
                });
#pragma warning restore 612, 618
        }
    }
}
