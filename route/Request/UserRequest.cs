
namespace Request;

public record UserCreateRequest(string email, string password, string username, int preference);
public record UserLoginRequest (string email, string password);
public record UserDeleteRequest(string password);
public record UserUpdateRequest(int preference);

