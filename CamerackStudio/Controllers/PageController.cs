using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CamerackStudio.Models;
using CamerackStudio.Models.DataBaseConnections;
using CamerackStudio.Models.Encryption;
using CamerackStudio.Models.Entities;
using CamerackStudio.Models.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CamerackStudio.Controllers
{
    public class PageController : Controller
    {
        private readonly CamerackStudioDataContext _databaseConnection;

        public PageController(CamerackStudioDataContext databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }
        public IActionResult Faq()
        {
            return View(_databaseConnection.Faqs.FirstOrDefault());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Faq(Faq faq)
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            faq.DateLastModified = DateTime.Now;
            faq.LastModifiedBy = signedInUserId;

            _databaseConnection.Entry(faq).State = EntityState.Modified;
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully modified the FAQ!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Dashboard", "Home");
        }
        public IActionResult PrivacyPolicy()
        {
            return View(_databaseConnection.PrivacyPolicies.FirstOrDefault());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PrivacyPolicy(PrivacyPolicy policy)
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            policy.DateLastModified = DateTime.Now;
            policy.LastModifiedBy = signedInUserId;

            _databaseConnection.Entry(policy).State = EntityState.Modified;
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully modified the Privacy Policy!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Dashboard", "Home");
        }
        public IActionResult AboutUs()
        {
            return View(_databaseConnection.AboutUs.SingleOrDefault());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AboutUs(AboutUs aboutUs, IList<IFormFile> image)
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            aboutUs.DateLastModified = DateTime.Now;
            aboutUs.LastModifiedBy = signedInUserId;
            if (image.Count > 0)
                foreach (var file in image)
                {
                    var fileInfo = new FileInfo(file.FileName);
                    var ext = fileInfo.Extension.ToLower();
                    var name = DateTime.Now.ToFileTime().ToString();
                    var fileName = name + ext;
                    var uploadedImage = new AppConfig().AboutPicture + fileName;
                    using (var fs = System.IO.File.Create(uploadedImage))
                    {
                        if (fs != null)
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                            aboutUs.File = fileName;

                        }
                    }
                }
            _databaseConnection.Entry(aboutUs).State = EntityState.Modified;
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully modified the About Us!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Dashboard","Home");
        }
        public IActionResult Terms()
        {
            return View(_databaseConnection.TermsAndConditions.ToList());
        }
        public IActionResult CreateTerms()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTerms(TermAndCondition terms)
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            terms.DateLastModified = DateTime.Now;
            terms.LastModifiedBy = signedInUserId;

            _databaseConnection.TermsAndConditions.Add(terms);
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully added a new version of the T&C!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Terms");
        }
        public IActionResult EditTerms()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTerms(TermAndCondition terms)
        {
            var signedInUserId = Convert.ToInt64(HttpContext.Session.GetString("StudioLoggedInUserId"));
            terms.DateLastModified = DateTime.Now;
            terms.LastModifiedBy = signedInUserId;

            _databaseConnection.Entry(terms).State = EntityState.Modified;
            _databaseConnection.SaveChanges();
            TempData["display"] = "You have successfully modified the version of the T&C!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Terms");
        }
        // GET: ImageCategory/Delete/5
        [SessionExpireFilter]
        public ActionResult Delete(IFormCollection collection)
        {
            var id = Convert.ToInt64(collection["TermsId"]);
            var term = _databaseConnection.TermsAndConditions.Find(id);

            _databaseConnection.TermsAndConditions.Remove(term);
            _databaseConnection.SaveChanges();

            //display notification
            TempData["display"] = "You have successfully deleted the T&C!";
            TempData["notificationtype"] = NotificationType.Success.ToString();
            return RedirectToAction("Terms");
        }
    }
}