using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Exceptions
{
    public class CacheOperationException(Exception? innerException) :  Exception("An error occurred while performing a cache operation", innerException);
    
}
