namespace Shinsekai_API.Authentication
{
    public class AuthParams : IAuth
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}