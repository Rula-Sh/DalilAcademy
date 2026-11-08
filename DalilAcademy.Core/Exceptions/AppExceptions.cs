namespace DalilAcademy.Core.Exceptions
{
    public class NotFoundException(string message) : Exception(message);
    public class ConflictException(string message) : Exception(message);
    public class BadRequestException(string message) : Exception(message);
    public class UnauthorizedException(string message) : Exception(message);
    public class InvalidFileException(string message) : Exception(message);
}
