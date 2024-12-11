namespace Application.UseCases.Users.Commands.GenerateToken
{
    public class GenerateTokenRequest
    {
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}