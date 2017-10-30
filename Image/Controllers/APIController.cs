using System;
using System.Collections.Generic;
using System.Linq;
using Image.Models.DataBaseConnections;
using Image.Models.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Image.Controllers
{
    public class ApiController : Controller
    {
        private readonly ImageDataContext _databaseConnection;
        private readonly IHostingEnvironment _hostingEnv;

        public ApiController(IHostingEnvironment env, ImageDataContext databaseConnection)
        {
            _hostingEnv = env;
            _databaseConnection = databaseConnection;
        }
        /// <summary>
        /// The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImages()
        {
            return Json(_databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).ToList());
        }
        /// <summary>
        /// The method returns a single images via a json object
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public JsonResult GetSingleImage(long imageId)
        {
            var image = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).SingleOrDefault(n=>n.ImageId == imageId);
            return Json(image);
        }
        /// <summary>
        /// The method returns list of images based on a search criterion from the user
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public JsonResult SearchImages([FromBody] SearchCriteria criteria)
        {
            var images = _databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location)
                .Include(n => n.ImageCategory).Include(n => n.ImageSubCategory).Where(
                    n => n.LocationId == criteria.LocationId
                         && n.CameraId == criteria.CameraId && n.ImageCategoryId == criteria.ImageCategoryId &&
                         n.ImageSubCategoryId == criteria.ImageSubCategoryId).ToList();
           return Json(images);

        }
    }
}
