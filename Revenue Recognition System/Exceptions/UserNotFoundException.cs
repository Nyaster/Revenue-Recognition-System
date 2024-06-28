using Revenue_Recognition_System.Exceptions;

namespace Revenue_Recognition_System.Services;

public class UserNotFoundException(string userLogin) : NotFoundException;