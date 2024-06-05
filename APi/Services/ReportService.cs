using Tourplanner.Entities.Tours;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace Tourplanner.Services
{
    public interface IReportService
    {
        byte[] GenerateTourReport(Tour tourEntity);
        byte[] GenerateSummaryReport(IEnumerable<Tour> tours);
    }

    public class ReportService : IReportService
    {

        public ReportService() {QuestPDF.Settings.License = LicenseType.Community;}

        public byte[] GenerateTourReport(Tour tourEntity)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(2, Unit.Centimetre);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(13));


                    page.Header()
                        .Text("Tour: " + tourEntity.Name)
                        .FontSize(30).FontColor(Colors.Blue.Medium);



                    page.Content()
                        .Column(column =>
                        {
                            column.Spacing(10);

                            // Image
                            // if (!string.IsNullOrEmpty(tourEntity.ImagePath))
                            // {
                            //     column.Item().Image(tourEntity.ImagePath);
                            // }
                            // else
                            {
                                column.Item().Element(ContainerWithPadding).Image("Assets/img/map.png");
                            }


                            column.Item().Element(ContainerWithPadding)
                            .Column(detailsColumn =>
                            {
                                detailsColumn.Spacing(10);

                                detailsColumn.Item().Text(text =>
                                {
                                    text.Span(tourEntity.From);
                                    text.Span(" - ");
                                    text.Span(tourEntity.To);
                                });

                                detailsColumn.Item().Text(text =>
                                {
                                    text.Span(tourEntity.Description);
                                });

                                detailsColumn.Item().Text(text =>
                                {
                                    text.Span("Estimated Time: ").SemiBold();
                                    text.Span($"{tourEntity.EstimatedTime.ToString()} h");
                                });

                                detailsColumn.Item().Text(text =>
                                {
                                    text.Span("Transport Type: ").SemiBold();
                                    text.Span(tourEntity.TransportType.ToString());
                                });

                                detailsColumn.Item().Text(text =>
                                {
                                    text.Span("Distance: ").SemiBold();
                                    text.Span($"{tourEntity.Distance} km");
                                });

                                detailsColumn.Item().Text(text =>
                                {
                                    text.Span("Popularity: ").SemiBold();
                                    text.Span(Math.Ceiling(tourEntity.Popularity).ToString() + " / 10");
                                });
                                Console.WriteLine(tourEntity.Popularity);

                                detailsColumn.Item().Text(text =>
                                {
                                    text.Span("Child Friendliness: ").SemiBold();
                                    text.Span(Math.Ceiling(tourEntity.ChildFriendliness).ToString() + " / 10");
                                });
                            });

                            // Tour logs table
                            column.Item().Element(ContainerWithPadding)
                            .Text("Tour Logs:")
                            .SemiBold()
                            .FontSize(20)
                            .FontColor(Colors.Black);

                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.ConstantColumn(120);
                                    columns.ConstantColumn(150);
                                    columns.RelativeColumn(30);
                                    columns.RelativeColumn(30);
                                    columns.RelativeColumn(30);
                                });

                                // Table header
                                table.Header(header =>
                                {
                                    header.Cell().Element(CellStyle).Text("Date");
                                    header.Cell().Element(CellStyle).Text("Comment");
                                    header.Cell().Element(CellStyle).Text("Difficulty");
                                    header.Cell().Element(CellStyle).Text("Duration");
                                    header.Cell().Element(CellStyle).Text("Rating");

                                    IContainer CellStyle(IContainer container) =>
                                        container.DefaultTextStyle(x => x.SemiBold()).Padding(5).BorderColor(Colors.Transparent);
                                });

                                // Table content
                                foreach (var log in tourEntity.TourLogs)
                                {
                                    table.Cell().Element(CellStyle).Text(log.DateTime.ToString("dd.MM.yyyy hh:mm"));
                                    table.Cell().Element(CellStyle).Text(log.Comment);
                                    table.Cell().Element(CellStyle).Text(log.Difficulty.ToString());
                                    table.Cell().Element(CellStyle).Text(log.Duration.ToString() + " min");
                                    table.Cell().Element(CellStyle).Text(log.Rating.ToString() + " / 10");

                                    IContainer CellStyle(IContainer container) =>
                                        container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).Padding(5);
                                }
                            });
                        });

                    page.Footer()
                        .AlignRight()
                        .Text(x =>
                        {
                            x.Span("Seite ");
                            x.CurrentPageNumber();
                        });

                });
            });

            // document.ShowInPreviewer();

            byte[] pdfBytes;
            using (var memoryStream = new MemoryStream())
            {
                document.GeneratePdf(memoryStream);
                pdfBytes = memoryStream.ToArray();
            }

            return pdfBytes;
        }

        public byte[] GenerateSummaryReport(IEnumerable<Tour> tours)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(2, Unit.Centimetre);
                    page.Size(PageSizes.A4);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(13));


                    page.Content()
                        .Column(column =>
                        {

                            column.Item()
                                .Text("Summary Report")
                                .FontSize(30).FontColor(Colors.Blue.Medium);

                            column.Spacing(10);

                            foreach (var tour in tours)
                            {
                                column.Item().Element(ContainerWithPadding)
                                .Column(detailsColumn =>
                                {
                                    detailsColumn.Spacing(10);

                                    detailsColumn.Item().Text(text =>
                                    {
                                        text.Span(tour.Name);
                                    });

                                    detailsColumn.Item().Text(text =>
                                    {
                                        text.Span("Estimated Time: ").SemiBold();
                                        text.Span($"{tour.EstimatedTime.ToString()} h");
                                    });

                                    detailsColumn.Item().Text(text =>
                                    {
                                        text.Span("Transport Type: ").SemiBold();
                                        text.Span(tour.TransportType.ToString());
                                    });

                                    detailsColumn.Item().Text(text =>
                                    {
                                        text.Span("Distance: ").SemiBold();
                                        text.Span($"{tour.Distance} km");
                                    });

                                    detailsColumn.Item().Text(text =>
                                    {
                                        text.Span("Popularity: ").SemiBold();
                                        text.Span(Math.Ceiling(tour.Popularity).ToString() + " / 10");
                                    });

                                    detailsColumn.Item().Text(text =>
                                    {
                                        text.Span("Child Friendliness: ").SemiBold();
                                        text.Span(Math.Ceiling(tour.ChildFriendliness).ToString() + " / 10");
                                    });
                                });
                            }
                        });

                    page.Footer()
                        .AlignRight()
                        .Text(x =>
                        {
                            x.Span("Seite ");
                            x.CurrentPageNumber();
                        });

                });

            });

            // document.ShowInPreviewer();

            byte[] pdfBytes;
            using (var memoryStream = new MemoryStream())
            {
                document.GeneratePdf(memoryStream);
                pdfBytes = memoryStream.ToArray();
            }

            return pdfBytes;

        }

        private static IContainer ContainerWithPadding(IContainer container) => container.PaddingTop(20);
    }
}
