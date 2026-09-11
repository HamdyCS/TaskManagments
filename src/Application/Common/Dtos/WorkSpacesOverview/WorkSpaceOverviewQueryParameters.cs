using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Dtos.WorkSpaceOverview
{
    public record WorkSpaceOverviewQueryParameters
    (
         DateTime? From,
         DateTime? To
    );
}
