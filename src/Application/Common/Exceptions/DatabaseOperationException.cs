using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    public class DatabaseOperationException(Exception? innerException) : Exception(
        "An error occurred while performing a database operation", innerException);
}
