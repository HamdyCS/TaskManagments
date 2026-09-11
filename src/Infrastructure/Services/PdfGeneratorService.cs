using Application.Common.Dtos;
using Application.Common.Dtos.WorkSpacesOverview;
using Application.Common.Interfaces.Services;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Services
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        public PdfGeneratorService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

        //workspace report generation
        public byte[] GenerateWorkSpaceReportPdf(WorkSpaceReportDto workSpaceReportDto)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    _AddWorkSpaceReportPageSettings(page);
                    _AddWorkSpaceReportHeader(page, workSpaceReportDto);
                    _AddWorkSpaceReportContent(page, workSpaceReportDto);
                    _AddWorkSpaceReportFooter(page, workSpaceReportDto);
                });
            }).GeneratePdf();
        }

        private void _AddWorkSpaceReportPageSettings(PageDescriptor page)
        {
            page.Size(PageSizes.A4);
            page.Margin(30);
            page.PageColor(Colors.White);
            page.DefaultTextStyle(x => x.FontSize(20));
        }

        private void _AddWorkSpaceReportHeader(PageDescriptor page, WorkSpaceReportDto workSpaceReportDto)
        {
            page.Header().Column(column =>
            {
                column.Item()
                    .Text($"WorkSpace Report: {workSpaceReportDto.WorkSpaceName}")
                    .SemiBold()
                    .FontSize(36)
                    .FontColor(Colors.Blue.Medium);

                column.Item()
                    .PaddingTop(10)
                    .Text($"Generated on: {DateTime.Now.ToString("yyyy-MMMM-dd HH:mm:ss")}")
                    .FontSize(20)
                    .FontColor(Colors.Grey.Darken1);
            });
        }

        private void _AddWorkSpaceReportContent(PageDescriptor page, WorkSpaceReportDto workSpaceReportDto)
        {
            page.Content().Column(column =>
            {
                var ownerNames = workSpaceReportDto.OwnerNames != null
                    ? string.Join(", ", workSpaceReportDto.OwnerNames)
                    : "N/A";

                column.Item()
                    .PaddingVertical(10)
                    .Text($"Owner Names: {ownerNames}")
                    .FontSize(18)
                    .Bold();

                column.Item().Text($"Total Projects: {workSpaceReportDto.TotalProjects}");
                column.Item().Text($"Total Members: {workSpaceReportDto.TotalMembers}");
                column.Item().Text($"Total Tasks: {workSpaceReportDto.TotalTasks}");
                column.Item().Text($"Total Backlog Tasks: {workSpaceReportDto.TotalBacklogTasks}");
                column.Item().Text($"Total Todo Tasks: {workSpaceReportDto.TotalTodoTasks}");
                column.Item().Text($"Total In Progress Tasks: {workSpaceReportDto.TotalInProgressTasks}");
                column.Item().Text($"Total Review Tasks: {workSpaceReportDto.TotalReviewTasks}");
                column.Item().Text($"Total Done Tasks: {workSpaceReportDto.TotalDoneTasks}");

                column.Item().PaddingTop(15).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(HeaderCellStyle).Text("Name").Bold();
                        header.Cell().Element(HeaderCellStyle).Text("Tasks").Bold();
                        header.Cell().Element(HeaderCellStyle).Text("InProgress").Bold();
                        header.Cell().Element(HeaderCellStyle).Text("Done").Bold();

                        static IContainer HeaderCellStyle(IContainer container) =>
                            container
                                .Border(1)
                                .BorderColor("#B0B0B0")
                                .Background("#D9D9D9")
                                .Padding(6);
                    });

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
                                .BorderColor("#D3D3D3")
                                .Padding(6);
                    }
                });
            });
        }

        private void _AddWorkSpaceReportFooter(PageDescriptor page, WorkSpaceReportDto workSpaceReportDto)
        {
            page.Footer().AlignCenter().Text(t =>
            {
                t.Span("page ");
                t.CurrentPageNumber();
                t.Span(" of ");
                t.TotalPages();
            });
        }

        //workspaces overview report generation
        public byte[] GenerateWorkSpacesOverviewReportPdf(WorkSpacesOverviewReportDto workSpacesOverviewDto)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    _AddWorkSpacesOverviewPageSettings(page);
                    _AddWorkSpacesOverviewHeader(page);
                    _AddWorkSpacesOverviewContent(page, workSpacesOverviewDto);
                    _AddWorkSpacesOverviewFooter(page);
                });
            }).GeneratePdf();
        }

        private void _AddWorkSpacesOverviewPageSettings(PageDescriptor page)
        {
            page.Size(PageSizes.A4);
            page.MarginHorizontal(35);
            page.MarginVertical(30);
            page.PageColor(Colors.Grey.Lighten5);

            page.DefaultTextStyle(text =>
                text.FontFamily("Arial")
                    .FontSize(10)
                    .FontColor(Colors.Grey.Darken3));
        }

        private void _AddWorkSpacesOverviewHeader(PageDescriptor page)
        {
            page.Header()
                .PaddingBottom(20)
                .Column(column =>
                {
                    column.Item()
                        .Row(row =>
                        {
                            row.RelativeItem()
                                .Column(header =>
                                {
                                    header.Item()
                                        .Text("WORKSPACES")
                                        .FontSize(11)
                                        .Bold()
                                        .FontColor(Colors.Blue.Medium);

                                    header.Item()
                                        .PaddingTop(3)
                                        .Text("Overview Report")
                                        .FontSize(26)
                                        .Bold()
                                        .FontColor(Colors.Grey.Darken4);
                                });

                            row.AutoItem()
                                .AlignRight()
                                .Column(date =>
                                {
                                    date.Item()
                                        .Text("GENERATED")
                                        .FontSize(8)
                                        .Bold()
                                        .FontColor(Colors.Grey.Medium);

                                    date.Item()
                                        .PaddingTop(3)
                                        .Text(DateTime.Now.ToString("MMM dd, yyyy"))
                                        .FontSize(10)
                                        .SemiBold()
                                        .FontColor(Colors.Grey.Darken2);

                                    date.Item()
                                        .Text(DateTime.Now.ToString("HH:mm:ss"))
                                        .FontSize(9)
                                        .FontColor(Colors.Grey.Medium);
                                });
                        });

                    column.Item()
                        .PaddingTop(15)
                        .LineHorizontal(1)
                        .LineColor(Colors.Grey.Lighten2);
                });
        }

        private void _AddWorkSpacesOverviewContent(PageDescriptor page, WorkSpacesOverviewReportDto dto)
        {
            page.Content()
                .Column(column =>
                {
                    _AddOverviewKpiCards(column, dto);

                    column.Item().PaddingTop(25);

                    _AddTaskStatusSection(column, dto);

                    column.Item().PaddingTop(25);

                    _AddTaskPrioritySection(column, dto);
                });
        }

        private void _AddOverviewKpiCards(ColumnDescriptor column, WorkSpacesOverviewReportDto dto)
        {
            column.Item()
                .Row(row =>
                {
                    _AddKpiCard(row.RelativeItem(), "Regular Users", dto.RegularUsersCount.ToString(), "Users");
                    row.ConstantItem(10);
                    _AddKpiCard(row.RelativeItem(), "Workspaces", dto.WorkspacesCount.ToString(), "Spaces");
                    row.ConstantItem(10);
                    _AddKpiCard(row.RelativeItem(), "Projects", dto.ProjectsCount.ToString(), "Projects");
                    row.ConstantItem(10);
                    _AddKpiCard(row.RelativeItem(), "Tasks", dto.TasksCount.ToString(), "Tasks");
                });
        }

        private void _AddKpiCard(IContainer container, string title, string value, string subtitle)
        {
            container
                .Background(Colors.White)
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .CornerRadius(8)
                .Padding(14)
                .Column(column =>
                {
                    column.Item()
                        .Text(title)
                        .FontSize(9)
                        .SemiBold()
                        .FontColor(Colors.Grey.Medium);

                    column.Item()
                        .PaddingTop(7)
                        .Text(value)
                        .FontSize(22)
                        .Bold()
                        .FontColor(Colors.Grey.Darken4);

                    column.Item()
                        .PaddingTop(2)
                        .Text(subtitle)
                        .FontSize(8)
                        .FontColor(Colors.Grey.Lighten1);
                });
        }

        private void _AddTaskStatusSection(ColumnDescriptor column, WorkSpacesOverviewReportDto dto)
        {
            column.Item()
                .Background(Colors.White)
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .CornerRadius(8)
                .Padding(18)
                .Column(section =>
                {
                    section.Item()
                        .Text("Task Status Distribution")
                        .FontSize(15)
                        .Bold()
                        .FontColor(Colors.Grey.Darken4);

                    section.Item()
                        .PaddingTop(4)
                        .Text("Current distribution of tasks across all workspaces")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Medium);

                    section.Item().PaddingTop(15);

                    _AddTaskStatusTable(section, dto.TasksByStatusReportDtos);
                });
        }

        private void _AddTaskStatusTable(ColumnDescriptor column, IEnumerable<TasksByStatusReportDto> reports)
        {
            var data = reports?.ToList() ?? [];

            column.Item()
                .Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        _AddOverviewTableHeaderCell(header, "Status");
                        _AddOverviewTableHeaderCell(header, "Tasks");
                        _AddOverviewTableHeaderCell(header, "Share");
                    });

                    var total = data.Sum(x => x.Count);

                    foreach (var item in data)
                    {
                        var percentage = total == 0 ? 0 : (double)item.Count / total * 100;

                        _AddOverviewTableCell(table, item.TaskStatus.ToString());
                        _AddOverviewTableCell(table, item.Count.ToString());
                        _AddOverviewTableCell(table, $"{percentage:0.0}%");
                    }
                });
        }

        private void _AddTaskPrioritySection(ColumnDescriptor column, WorkSpacesOverviewReportDto dto)
        {
            column.Item()
                .Background(Colors.White)
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .CornerRadius(8)
                .Padding(18)
                .Column(section =>
                {
                    section.Item()
                        .Text("Tasks by Priority")
                        .FontSize(15)
                        .Bold()
                        .FontColor(Colors.Grey.Darken4);

                    section.Item()
                        .PaddingTop(4)
                        .Text("Task distribution based on priority level")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Medium);

                    section.Item().PaddingTop(15);

                    _AddTaskPriorityTable(section, dto.TasksByPriorityReportDtos);
                });
        }

        private void _AddTaskPriorityTable(ColumnDescriptor column, IEnumerable<TasksByPriorityReportDto> reports)
        {
            var data = reports?.ToList() ?? [];

            column.Item()
                .Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(2);
                        columns.RelativeColumn(2);
                    });

                    table.Header(header =>
                    {
                        _AddOverviewTableHeaderCell(header, "Priority");
                        _AddOverviewTableHeaderCell(header, "Tasks");
                        _AddOverviewTableHeaderCell(header, "Share");
                    });

                    var total = data.Sum(x => x.Count);

                    foreach (var item in data)
                    {
                        var percentage = total == 0 ? 0 : (double)item.Count / total * 100;

                        _AddOverviewTableCell(table, item.TaskPriority.ToString());
                        _AddOverviewTableCell(table, item.Count.ToString());
                        _AddOverviewTableCell(table, $"{percentage:0.0}%");
                    }
                });
        }

        private static void _AddOverviewTableHeaderCell(TableCellDescriptor header, string text)
        {
            header.Cell()
                .Background(Colors.Grey.Darken3)
                .PaddingVertical(9)
                .PaddingHorizontal(10)
                .Text(text)
                .FontSize(9)
                .Bold()
                .FontColor(Colors.White);
        }

        private static void _AddOverviewTableCell(TableDescriptor table, string text)
        {
            table.Cell()
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(9)
                .PaddingHorizontal(10)
                .Text(text)
                .FontSize(9)
                .FontColor(Colors.Grey.Darken3);
        }

        private void _AddWorkSpacesOverviewFooter(PageDescriptor page)
        {
            page.Footer()
                .PaddingTop(15)
                .BorderTop(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Row(row =>
                {

                    row.RelativeItem().PaddingTop(15)
                        .Text("Workspaces Overview Report")
                        .FontSize(8)
                        .FontColor(Colors.Grey.Medium);

                    row.AutoItem().PaddingTop(15)
                        .Text(text =>
                        {
                            text.Span("Page ").FontSize(8).FontColor(Colors.Grey.Medium);
                            text.CurrentPageNumber().FontSize(8).FontColor(Colors.Grey.Medium);
                            text.Span(" of ").FontSize(8).FontColor(Colors.Grey.Medium);
                            text.TotalPages().FontSize(8).FontColor(Colors.Grey.Medium);
                        });
                    
                });
        }
    }
}
