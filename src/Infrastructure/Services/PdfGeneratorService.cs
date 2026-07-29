using Application.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Application.Common.Interfaces.Services;
namespace Infrastructure.Services
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        public PdfGeneratorService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }
        public Byte[] GenerateWorkSpaceReportPdf(WorkSpaceReportDto workSpaceReportDto)
        {
            // Implementation for generating PDF report
            return Document.Create(container =>
             {
                 container.Page(page =>
                 {
                     _AddPageSettings(page);
                     _AddHeader(page, workSpaceReportDto);
                     _AddContent(page, workSpaceReportDto);
                     _AddFooter(page, workSpaceReportDto);

                 });
             }).GeneratePdf();
        }



        private void _AddPageSettings(PageDescriptor page)
        {
            page.Size(PageSizes.A4);
            page.Margin(30);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(20));
        }

        private void _AddHeader(PageDescriptor page, WorkSpaceReportDto workSpaceReportDto)
        {
            page.Header().Column(column =>
            {
                column.Item().Text($"WorkSpace Report: {workSpaceReportDto.WorkSpaceName}").SemiBold().FontSize(36)
                .FontColor(Colors.Blue.Medium);

                column.Item().PaddingTop(10).Text($"Generated on: {DateTime.Now.ToString("yyyy-MMMM-dd HH:mm:ss")}").FontSize(20).FontColor(Colors.Grey.Darken1);
            });

        }

        private void _AddContent(PageDescriptor page, WorkSpaceReportDto workSpaceReportDto)
        {
            page.Content().Column(column =>
            {
                // 1. قسم البيانات والإحصائيات
                var ownerNames = workSpaceReportDto.OwnerNames != null
                    ? string.Join(", ", workSpaceReportDto.OwnerNames)
                    : "N/A";

                column.Item().PaddingVertical(10).Text($"Owner Names: {ownerNames}").FontSize(18).Bold();

                column.Item().Text($"Total Projects: {workSpaceReportDto.TotalProjects}");
                column.Item().Text($"Total Members: {workSpaceReportDto.TotalMembers}");
                column.Item().Text($"Total Tasks: {workSpaceReportDto.TotalTasks}");
                column.Item().Text($"Total Backlog Tasks: {workSpaceReportDto.TotalBacklogTasks}");
                column.Item().Text($"Total Todo Tasks: {workSpaceReportDto.TotalTodoTasks}");
                column.Item().Text($"Total In Progress Tasks: {workSpaceReportDto.TotalInProgressTasks}");
                column.Item().Text($"Total Review Tasks: {workSpaceReportDto.TotalReviewTasks}");
                column.Item().Text($"Total Done Tasks: {workSpaceReportDto.TotalDoneTasks}");

                // 2. قسم جدول أداء الأعضاء
                column.Item().PaddingTop(15).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    // رؤوس الجدول (Header) - لاحظ استخدام .Element(...) لتطبيق التنسيق
                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderCellStyle).Text("Name").Bold();
                        header.Cell().Element(HeaderCellStyle).Text("Tasks").Bold();
                        header.Cell().Element(HeaderCellStyle).Text("InProgress").Bold();
                        header.Cell().Element(HeaderCellStyle).Text("Done").Bold();

                        static IContainer HeaderCellStyle(IContainer container) =>
                            container
                                .Border(1)
                                .BorderColor("#B0B0B0")      // لون الحدود الداكن
                                .Background("#D9D9D9")       // خلفية رمادية للرأس
                                .Padding(6);                  // المسافة الداخلية
                    });

                    // بيانات الجدول (Rows) - لاحظ استخدام .Element(...) للبيانات أيضاً
                    if (workSpaceReportDto.MemberPerformances != null)
                    {
                        foreach (var memberPerformance in workSpaceReportDto.MemberPerformances)
                        {
                            table.Cell().Element(DataCellStyle).Text(memberPerformance.Name ?? "");
                            table.Cell().Element(DataCellStyle).Text(memberPerformance.AssignedCount.ToString());
                            table.Cell().Element(DataCellStyle).Text(memberPerformance.InProgressCount.ToString());
                            table.Cell().Element(DataCellStyle).Text(memberPerformance.DoneCount.ToString());
                        }

                        static IContainer DataCellStyle(IContainer container) =>
                            container
                                .Border(1)
                                .BorderColor("#D3D3D3")      // لون شبكة Excel الفاتح
                                .Padding(6);                  // المسافة الداخلية
                    }
                });
            });
        }

        private void _AddFooter(PageDescriptor page, WorkSpaceReportDto workSpaceReportDto)
        {
            page.Footer().AlignCenter().Text(t =>
            {
                t.Span("page ");
                t.CurrentPageNumber();
                t.Span(" of ");
                t.TotalPages();
            });
        }
    }
}
