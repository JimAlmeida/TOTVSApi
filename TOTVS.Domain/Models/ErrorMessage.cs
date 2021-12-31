namespace TOTVS.Domain.Models
{
    public class ErrorMessage
    {
        public ErrorMessage(string message)
        {
            Mensagem = message;
        }
        public string Mensagem { get; set; }
    }
}
