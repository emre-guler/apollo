namespace Apollo.Models
{
    ///<summary>
    /// <para>Hata Kodları</para>
    /// <para>100 - Tüm alanlar doğru şekilde doldurulmalı.</para>
    /// <para>101 - Bu mail veya telefon ile hesap zaten var.</para> 
    /// <para>102 - Böyle bir kullanıcı yok veya şifre yanlış.</para>
    ///</summary>
    public class ErrorModel 
    {    
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public ErrorModel(string errorCode, string errorMessage)
        {
            this.ErrorCode = errorCode;
            this.ErrorMessage = errorMessage;
        }
    }
}