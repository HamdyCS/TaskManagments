namespace Application.Common.Exceptions
{
    //when the foreign key constraint is violated (for example, when trying to delete a project that has tasks associated with it)
    public class ForeignKeyConstraintViolationException(Exception? innerException) : Exception(
        "A foreign key constraint was violated", innerException);
   
}
