namespace Image.Models
{
    public class AppConfig
    {
        #region Cloudinary
        public string GeneralImage => "upload/w_640,h_670/w_640,l_watermark/";
        public string TableImage => "upload/w_60,h_80/w_50,l_watermark/";
        public string A5Image => "upload/w_559,h_794/";
        public string A4Image => "upload/w_794,h_1123/";
        public string A3Image => "upload/w_1123,h_1587/";
        public string A2Image => "upload/w_1587,h_2245/";
        public string A1Image => "upload/w_2245,h_3571/";


        #endregion

        #region SSO
        public string SSOBaseUrl => "http://localhost:53017/";
        //public string SSOBaseUrl => "http://sso.camerack.com/";
        public string RegisterUsersUrl => SSOBaseUrl + "Account/Register";
        public string LoginUrl => SSOBaseUrl + "Account/Login";
        public string FetchUsersUrl => SSOBaseUrl + "AppUser/Index";
        public string ForgotPasswordLinkUrl => SSOBaseUrl + "Account/ForgotPasswordResetLink";
        public string ResetPasswordUrl => SSOBaseUrl + "Account/PasswordReset";
        public string ChangePasswordrl => SSOBaseUrl + "Account/ChangePassword";
        public string EditProfileUrl => SSOBaseUrl + "Account/UpdateProfile";
        public string ActivateAccountUrl => SSOBaseUrl + "Account/ActivateAccount";
        public string DeActivateAccount => SSOBaseUrl + "Account/DeActivateAccount";
        public long TenancyId => 1;

        #endregion

        #region Camerack API URL

        //public string CamerackBaseUrl => "http://camerack.com/";
        public string CamerackBaseUrl => "http://localhost:51851/";
        public string FetchOrdersUrl => CamerackBaseUrl + "API/GetAllOrders";
        public string FetchPaymentsUrl => CamerackBaseUrl + "API/GetAllPayments";
        public string FetchInvoiceUrl => CamerackBaseUrl + "API/GetAllInvoices";
        #endregion
    }
}
