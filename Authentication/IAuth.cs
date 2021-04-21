namespace Shinsekai_API.Authentication
{
    public interface IAuth
    {
        public string Email { get; set; } 
        public string Password { get; set; }
    }
}