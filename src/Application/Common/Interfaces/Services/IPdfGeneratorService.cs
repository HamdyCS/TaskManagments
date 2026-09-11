using Application.Common.Dtos;
using Application.Common.Dtos.WorkSpacesOverview;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Services
{
    public interface IPdfGeneratorService
    {
        public Byte[] GenerateWorkSpaceReportPdf(WorkSpaceReportDto workSpaceReportDto);
        byte[] GenerateWorkSpacesOverviewReportPdf(WorkSpacesOverviewReportDto workSpacesOverviewDto);
    }
}
