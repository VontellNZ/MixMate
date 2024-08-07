namespace MixMate.DataAccess.Exceptions;

public class SongRepositoryException(string message, Exception innerException) : Exception(message, innerException){}
