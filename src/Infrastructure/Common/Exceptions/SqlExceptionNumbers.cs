using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.common.Exceptions
{
    public static class SqlExceptionNumbers
    {
        public const int UniqueIndex = 2601;
        public const int UniqueConstraint = 2627;
        public const int ForeignKeyConstraint = 547;
        public const int Deadlock = 1205;
        public const int Timeout = -2;
    }
}
