using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common.Interfaces
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }
    }
}
