namespace CamerackStudio.Models
{
    public class AppConfig
    {
        #region Cloudinary
        public string GeneralImage => "upload/q_auto:best/w_640,h_670/w_580,c_scale,x_0,y_0,a_0,l_watermark/";
        public string FreeImage => "upload/";
        public string TableImage => "upload/q_auto:best/w_60,h_60/w_80,c_scale,x_0,y_0,a_0,l_watermark/";
        public string A5Image => "upload/w_559,h_794/";
        public string A4Image => "upload/w_794,h_1123/";
        public string A3Image => "upload/w_1123,h_1587/";
        public string A2Image => "upload/w_1587,h_2245/";
        public string A1Image => "upload/w_2245,h_3571/";


        #endregion

        #region SSO
        //public string SSOBaseUrl => "http://localhost:53017/";
        public string SSOBaseUrl => "http://sso.camerack.com/";
        public string RegisterUsersUrl => SSOBaseUrl + "API/Register";
        public string LoginUrl => SSOBaseUrl + "API/Login";
        public string FetchUsersUrl => SSOBaseUrl + "API/AllUsers";
        public string ForgotPasswordLinkUrl => SSOBaseUrl + "API/ForgotPasswordResetLink";
        public string ResetPasswordUrl => SSOBaseUrl + "API/PasswordReset";
        public string ChangePasswordrl => SSOBaseUrl + "API/ChangePassword";
        public string EditProfileUrl => SSOBaseUrl + "API/UpdateProfile";
        public string ActivateAccountUrl => SSOBaseUrl + "API/ActivateAccount/";
        public string DeActivateAccount => SSOBaseUrl + "API/DeActivateAccount/";
        public string FetchUsersAccessKeys => SSOBaseUrl + "API/AllUsersAccessKeys";
        public string UpdatePasswordAccessKey => SSOBaseUrl + "API/UpdatePasswordAccessKey/";
        public string UpdateAccountActivationAccessKey => SSOBaseUrl + "API/UpdateAccountActivationAccessKey/";
        public string SavePushNotifications => SSOBaseUrl + "API/SavePushNotifications";
        public string UsersPushNotifications => SSOBaseUrl + "API/GetPushNotifications";
        public string UpdatePushNotifications => SSOBaseUrl + "API/UpdatePushNotifications";
        public string AllBanks => SSOBaseUrl + "API/GetAllBanks";
        public long ClientId => 3;

        #endregion

        #region Camerack API URL

        public string CamerackBaseUrl => "http://camerack.com/";
        //public string CamerackBaseUrl => "http://localhost:51851/";
        public string FetchOrdersUrl => CamerackBaseUrl + "API/GetAllOrders";
        public string FetchPaymentsUrl => CamerackBaseUrl + "API/GetAllPayments";
        public string FetchInvoiceUrl => CamerackBaseUrl + "API/GetAllInvoices";
        public string ApprovePaymentsUrl => CamerackBaseUrl + "API/ApprovePayment/";
        #endregion

        #region Mailer

        public string EmailServer => "smtp.gmail.com";
        public string Email => "support@camerack.com";
        public string Password => "Brigada95";
        public int Port => 465;
        public string NewUserHtml => "wwwroot/EmailTemplates/NewUser.html";
        public string ForgotPasswordHtml => "wwwroot/EmailTemplates/ForgotPassword.html";
        public string CompetitionHtml => "wwwroot/EmailTemplates/Competition.html";

        #endregion

        #region APP

        public string ProfilePicture => "wwwroot/UploadedImage/ProfilePicture/";

        public string ProfileBackgorundPicture => "wwwroot/UploadedImage/ProfileBackground/";
        public string ImageCategoryPicture => "wwwroot/UploadedImage/ImageCategory/";

        public string PhotoCategoryPicture => "wwwroot/UploadedImage/PhotoCategory/";
        public string AdvertPicture => "wwwroot/UploadedImage/Advert/";
        public string SliderPicture => "wwwroot/UploadedImage/Slider/";
        public string HeaderPicture => "wwwroot/UploadedImage/Header/";
        #endregion

        #region Cloudinary

        public string CloudinaryAccoutnName => "cloudmab";
        public string CloudinaryApiKey => "988581656515289";
        public string CloudinaryApiSecret => "Odh29Eet7Ajilw0O0kCflwtnj9E";

        #endregion
    }
}
