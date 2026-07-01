using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    //when a unique constraint is violated (for example, when trying to create a project with the same name as another project)
    public class UniqueConstraintViolationException(Exception? innerException) : Exception(
        "A unique constraint was violated", innerException);
   
}
