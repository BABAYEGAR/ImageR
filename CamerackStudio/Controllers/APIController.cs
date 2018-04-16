using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CamerackStudio.Models;
using CamerackStudio.Models.APIFactory;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CamerackStudio.Controllers
{
    public class ApiController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;

        public ApiController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImages()
        {
            var images = _databaseConnection.Images.Where(n => n.Status == ImageStatus.Accepted.ToString()).ToList();
            return Json(images);
        }
        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllSliderImages()
        {
            return Json(_databaseConnection.SliderImages.ToList());
        }
        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllAdvertisments()
        {
            return Json(_databaseConnection.Advertisements.ToList());
        }
        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllHeaderImages()
        {
            return Json(_databaseConnection.HeaderImages.ToList());
        }
        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImageCategories()
        {
            return Json(_databaseConnection.ImageCategories.ToList());
        }
        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImageSubCategories()
        {
            return Json(_databaseConnection.ImageSubCategories.ToList());
        }
        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImageComments()
        {
            return Json(_databaseConnection.ImageComments.ToList());
        }

        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImageTags()
        {
            return Json(_databaseConnection.ImageTags.ToList());
        }

        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllPhotographyCategories()
        {
            return Json(_databaseConnection.PhotographerCategories.ToList());
        }

        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllPhotographyCategoryMapping()
        {
            return Json(_databaseConnection.PhotographerCategoryMappings.ToList());
        }

        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImageActions()
        {
            return Json(_databaseConnection.ImageActions.ToList());
        }
        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllTermsAndConditions()
        {
            return Json(_databaseConnection.TermsAndConditions.ToList());
        }
        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetPrivacyPolicy()
        {
            return Json(_databaseConnection.PrivacyPolicies.FirstOrDefault());
        }
        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetFaq()
        {
            return Json(_databaseConnection.Faqs.FirstOrDefault());
        }
        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAboutUs()
        {
            return Json(_databaseConnection.AboutUs.FirstOrDefault());
        }
        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImageLocations()
        {
            return Json(_databaseConnection.Locations.ToList());
        }


        /// <summary>
        ///     The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImageCameras()
        {
            return Json(_databaseConnection.Cameras.ToList());
        }
        /// <summary>
        ///     The method returns a single images via a json object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult UpdateAdCount(long id)
        {
            var ad = _databaseConnection.Advertisements.Find(id);
            ad.AdClick = ad.AdClick + 1;
            _databaseConnection.Entry(ad).State = EntityState.Modified;
            _databaseConnection.SaveChanges();
            return Json(ad);
        }
        /// <summary>
        ///     The method returns a single images via a json object
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetSingleImage(long id)
        {
            var image = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).SingleOrDefault(n => n.ImageId == id);
            return Json(image);
        }

        [HttpPost]
        public async Task<JsonResult> SaveImageAction([FromBody] ImageAction action)
        {
            var actionExist =
                _databaseConnection.ImageActions.Where(
                    n => n.AppUserId == action.AppUserId && n.ImageId == action.ImageId).ToList();
            if (actionExist.Count <= 0)
            {
                _databaseConnection.Add(action);
                _databaseConnection.SaveChanges();
                var image = _databaseConnection.Images.SingleOrDefault(n => n.ImageId == action.ImageId);
                if (image != null)
                {
                    var notification = new PushNotification
                    {
                        AppUserId = action.OwnerId,
                        CreatedBy = action.AppUserId,
                        LastModifiedBy = action.AppUserId,
                        DateLastModified = DateTime.Now,
                        DateCreated = DateTime.Now,
                        Category = SystemNotificationCategory.Comment.ToString(),
                        Read = false,
                        ControllerId = image.ImageId,
                        ClientId = 4
                    };

                    var singleOrDefault = new AppUserFactory()
                        .GetAllUsers(new AppConfig().FetchUsersUrl).Result
                        .SingleOrDefault(n => n.AppUserId == image.AppUserId);
                    if (singleOrDefault != null)
                        notification.Message = singleOrDefault.Name +
                                               " Rated your Image";
                    await new AppUserFactory().SavePushNotification(new AppConfig().SavePushNotifications, notification);
                }
            }
            return Json(action);
        }

        [HttpPost]
        public JsonResult SaveReport([FromBody] ImageReport report)
        {
            _databaseConnection.Add(report);
            _databaseConnection.SaveChanges();
            return Json(report);
        }

        [HttpPost]
        public async Task<JsonResult> SaveComment([FromBody] ImageComment comment)
        {
            _databaseConnection.Add(comment);
            _databaseConnection.SaveChanges();

            var image = _databaseConnection.Images.SingleOrDefault(n => n.ImageId == comment.ImageId);
            if (comment.ImageCommentId > 0)
            {
                if (image != null)
                {
                    var notification = new PushNotification
                    {
                        AppUserId = image.AppUserId,
                        CreatedBy = comment.CreatedBy,
                        LastModifiedBy = comment.CreatedBy,
                        DateLastModified = DateTime.Now,
                        DateCreated = DateTime.Now,
                        Category = SystemNotificationCategory.Comment.ToString(),
                        Read = false,
                        ControllerId = image.ImageId,
                        ClientId = 4
                    };

                    var singleOrDefault = new AppUserFactory()
                        .GetAllUsers(new AppConfig().FetchUsersUrl).Result
                        .SingleOrDefault(n => n.AppUserId == image.AppUserId);
                    if (singleOrDefault != null)
                        notification.Message = singleOrDefault.Name +
                                               " Commented on your Image";
                    await new AppUserFactory().SavePushNotification(new AppConfig().SavePushNotifications, notification);
                }
            }
            return Json(comment);
        }

        [HttpPost]
        public JsonResult SaveImageData([FromBody] Image image)
        {
            var tags = new List<string>();
            var savedImageTags = new List<ImageTag>();
            try
            {
                _databaseConnection.Add(image);
                _databaseConnection.SaveChanges();

                //iterate tags
                if (!string.IsNullOrEmpty(image.Tags) && image.ImageId > 0)
                {
                    var values = image.Tags.Split(',');
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = values[i].Trim();
                        tags.Add(values[i]);
                    }
                    //save tags
                    foreach (var item in tags)
                    {
                        var tag = new ImageTag
                        {
                            Name = item,
                            ImageId = image.ImageId,
                            DateCreated = DateTime.Now,
                            DateLastModified = DateTime.Now,
                            CreatedBy = image.CreatedBy,
                            LastModifiedBy = image.CreatedBy
                        };
                        savedImageTags.Add(tag);
                    }
                    //save transaction
                    _databaseConnection.AddRange(savedImageTags);
                    _databaseConnection.SaveChanges();
                }
                return Json(image);
            }
            catch (Exception)
            {
                return Json(image);
            }
        }

        [HttpPost]
        public JsonResult SaveImageTags([FromBody] List<ImageTag> tags)
        {
            try
            {
                _databaseConnection.AddRange(tags);
                _databaseConnection.SaveChanges();
                return Json(tags);
            }
            catch (Exception)
            {
                return Json(tags);
            }
        }

        [HttpPost]
        public JsonResult SaveUserNotification([FromBody] PushNotification notification)
        {
            try
            {
                _databaseConnection.Add(notification);
                _databaseConnection.SaveChanges();
                return Json(notification);
            }
            catch (Exception)
            {
                return Json(notification);
            }
        }

        public RedirectResult LogOut()
        {
            HttpContext.Session.Remove("StudioLoggedInUser");
            HttpContext.Session.Remove("StudioLoggedInUserId");
            HttpContext.Session.Clear();
            _databaseConnection.Dispose();
            HttpContext.SignOutAsync();
            return Redirect("https://camerack.com/Account/Login?returnUrl=logOut");
        }

    }
}