using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Reports.Queries.GetWorkSpacesOverviewReportPdf
{
    public class WorkSpacesOverviewReportPdfDto
    {
        public byte[] PdfBytes { get; set; }

        public string FileName { get; set; }
    }
}
