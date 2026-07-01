using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    public class DatabaseOperationException(string message, Exception? innerException) : Exception(message, innerException);
}
