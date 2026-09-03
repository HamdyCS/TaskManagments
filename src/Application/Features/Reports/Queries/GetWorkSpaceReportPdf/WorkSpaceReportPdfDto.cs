using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Reports.Queries.GetWorkSpaceReportPdf
{
    public class WorkSpaceReportPdfDto
    {
        public byte[] PdfBytes { get; set; }

        public string FileName { get; set; }
    }
}
