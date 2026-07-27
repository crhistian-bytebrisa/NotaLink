namespace NotaLink.Application.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string JWT { get; set; }
        public DateTime ExpireToken { get; set; }
    }
}
