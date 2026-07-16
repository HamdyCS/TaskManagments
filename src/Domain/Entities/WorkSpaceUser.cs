using Domain.Common.Enums;
using Domain.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class WorkSpaceUser : IBaseEntity
    {
        public long Id { get; set; }
        public long WorkSpaceId { get; set; }
        public string UserId { get; set; }
        public virtual WorkSpace WorkSpace { get; set; }
        public virtual User User { get; set; }
        public WorkSpaceRole WorkSpaceRole { get; set; }
    }
}
