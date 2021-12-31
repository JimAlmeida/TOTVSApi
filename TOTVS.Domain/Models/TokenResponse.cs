namespace TOTVS.Domain.Models
{
    public class TokenResponse : BaseResponse
    {
        public object AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
    }
}