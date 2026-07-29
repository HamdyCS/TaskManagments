using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Services
{
    public interface IPdfGeneratorService
    {
        public Byte[] GenerateWorkSpaceReportPdf(WorkSpaceReportDto workSpaceReportDto);
    }
}
