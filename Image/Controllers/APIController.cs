using System;
using System.Collections.Generic;
using System.Linq;
using Image.Models.DataBaseConnections;
using Image.Models.Entities;
using Image.Models.Enum;
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
            return Json(_databaseConnection.Images.Include(n => n.Camera).Include(n => n.Location).Include(n => n.ImageCategory)
                .Include(n=>n.ImageActions).Where(n=>n.Status == ImageStatus.Accepted.ToString()).ToList());
        }
        /// <summary>
        /// The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImageCategories()
        {
            return Json(_databaseConnection.ImageCategories.ToList());
        }
        /// <summary>
        /// The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImageComments()
        {
            return Json(_databaseConnection.ImageComments.ToList());
        }
        /// <summary>
        /// The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllPhotographyCategories()
        {
            return Json(_databaseConnection.PhotographerCategories.ToList());
        }
        /// <summary>
        /// The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllPhotographyCategoryMapping()
        {
            return Json(_databaseConnection.PhotographerCategoryMappings.ToList());
        }
        /// <summary>
        /// The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImageActions()
        {
            return Json(_databaseConnection.ImageActions.ToList());
        }
        /// <summary>
        /// The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImageLocations()
        {
            return Json(_databaseConnection.Locations.ToList());
        }
        /// <summary>
        /// The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllCompetitions()
        {
            return Json(_databaseConnection.Competition.ToList());
        }
        /// <summary>
        /// The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllCpmpetitionUploads()
        {
            return Json(_databaseConnection.CompetitionUploads.ToList());
        }
        /// <summary>
        /// The method returns all images from photo studio via json object
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAllImageCameras()
        {
            return Json(_databaseConnection.Cameras.ToList());
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
        [HttpPost]
        public JsonResult SaveImageAction([FromBody] ImageAction action)
        {
            _databaseConnection.Add(action);
            _databaseConnection.SaveChanges();
            return Json(action);

        }
        [HttpPost]
        public JsonResult SaveImageReport([FromBody] ImageReport report)
        {
            _databaseConnection.Add(report);
            _databaseConnection.SaveChanges();
            return Json(report);

        }
        [HttpPost]
        public JsonResult SaveComment([FromBody] ImageComment comment)
        {
            _databaseConnection.Add(comment);
            _databaseConnection.SaveChanges();
            return Json(comment);

        }
    }
}
