
namespace Request {

        public record UserCreateRequest( string email, string password, string username, int preference);
        public record UserLoginRequest(string username, string password);
}
