using Revenue_Recognition_System.Exceptions;

namespace Revenue_Recognition_System.Services;

public class NotAuthorizedException(string userLogin) : Unauthorized();