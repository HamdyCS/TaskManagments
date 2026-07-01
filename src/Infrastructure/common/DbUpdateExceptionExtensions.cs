using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.common
{
    public static class DbUpdateExceptionExtensions
    {
        public static bool IsUniqueConstraintViolation(this DbUpdateException ex)
        {
            if(ex.InnerException is SqlException sqlException)
            {
                //return if it is a unique index or unique constraint
                return sqlException.Number == SqlExceptionNumbers.UniqueIndex 
                    || sqlException.Number == SqlExceptionNumbers.UniqueConstraint;
            }

            return false;
        }

        public static bool IsForeignKeyConstraintViolation( this DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlException)
            {
                //return if it is a unique index or unique constraint
                return sqlException.Number == SqlExceptionNumbers.ForeignKeyConstraint;
            }

            return false;
        }
    }
}
