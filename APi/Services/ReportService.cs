using Tourplanner.Entities.Tours;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Tourplanner.Services.FormattingUtils;
using Tourplanner.Entities;
using QuestPDF.Previewer;

namespace Tourplanner.Services
{
    public interface IReportService
    {
        byte[] GenerateTourReport(Tour tourEntity);
        byte[] GenerateSummaryReport(IEnumerable<Tour> tours);
    }

    public class ReportService : IReportService
    {
        public ReportService()
        {
            QuestPDF.Settings.License = LicenseType.Community;
        }

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


                    page.Content()
                        .Column(column =>
                        {
                            column.Item().Element(ContainerWithPadding)
                                .Text(tourEntity.Name)
                                .SemiBold()
                                .FontSize(30)
                                .FontColor(Colors.Blue.Medium);

                            if (tourEntity.Map != null && tourEntity.Map.Tiles.Any())
                            {
                                AddMapTiles(column, tourEntity.Map);
                            }
                            AddTourDetails(column, tourEntity);

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

            return GeneratePdf(document);
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
                            column.Item().Element(ContainerWithPadding)
                                .Text("Summary Report")
                                .SemiBold()
                                .FontSize(30)
                                .FontColor(Colors.Blue.Medium);


                            foreach (var tour in tours)
                            {

                                column.Item().Element(ContainerWithPadding)
                                    .Column(detailsColumn =>
                                    {


                                        detailsColumn.Item().Element(ContainerWithPadding)
                                      .Text(tour.Name)
                                      .SemiBold()
                                      .FontSize(30)
                                      .FontColor(Colors.Black);
                                        if (tour.Map != null && tour.Map.Tiles.Any())
                                        {
                                            AddMapTiles(detailsColumn, tour.Map);
                                        }
                                        AddTourDetails(detailsColumn, tour);
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

            return GeneratePdf(document);
        }

        private static void AddTourDetails(ColumnDescriptor container, Tour tourEntity)
        {
            container.Item().Element(ContainerWithPadding)
                .Column(detailsColumn =>
                {
                    detailsColumn.Spacing(10);

                    detailsColumn.Item().Text(text =>
                    {
                        text.Span(tourEntity.From);
                        text.Span(" - ");
                        text.Span(tourEntity.To);
                    });

                    detailsColumn.Item().Text(tourEntity.Description);

                    detailsColumn.Item().Text(text =>
                    {
                        text.Span("Estimated Time: ").SemiBold();
                        text.Span($"{Formatting.SecondsToDaysMinutesHours(tourEntity.EstimatedTime)} h");
                    });

                    detailsColumn.Item().Text(text =>
                    {
                        text.Span("Transport Type: ").SemiBold();
                        text.Span(tourEntity.TransportType.ToString());
                    });

                    detailsColumn.Item().Text(text =>
                    {
                        text.Span("Distance: ").SemiBold();
                        text.Span($"{Formatting.MetersToKmAndMeters(tourEntity.Distance)} km");
                    });

                    detailsColumn.Item().Text(text =>
                    {
                        text.Span("Popularity: ").SemiBold();
                        text.Span(Math.Ceiling(tourEntity.Popularity).ToString() + " / 10");
                    });

                    detailsColumn.Item().Text(text =>
                    {
                        text.Span("Child Friendliness: ").SemiBold();
                        text.Span(Math.Ceiling(tourEntity.ChildFriendliness).ToString() + " / 10");
                    });

                    if (tourEntity.TourLogs.Any())
                    {
                        detailsColumn.Item().Element(ContainerWithPadding)
                            .Text("Tour Logs")
                            .SemiBold()
                            .FontSize(20)
                            .FontColor(Colors.Black);

                        detailsColumn.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(120);
                                columns.ConstantColumn(150);
                                columns.RelativeColumn(30);
                                columns.RelativeColumn(30);
                                columns.RelativeColumn(30);
                            });

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
                    }
                });
        }

        private static void AddMapTiles(ColumnDescriptor container, Map mapEntity)
        {
            int minX = mapEntity.Tiles.Min(tile => tile.X);
            int minY = mapEntity.Tiles.Min(tile => tile.Y);

            int maxRows = mapEntity.Tiles.Max(tile => tile.Y - minY) + 1;
            int maxCols = mapEntity.Tiles.Max(tile => tile.X - minX) + 1;
            var tileSize = 125;

            container.Item().Element(ContainerWithPadding)
                .Column(col =>
                {
                    for (int row = 0; row < maxRows; row++)
                    {
                        col.Item().Row(rowContent =>
                        {
                            rowContent.Spacing(0);
                            for (int colIndex = 0; colIndex < maxCols; colIndex++)
                            {
                                var tile = mapEntity.Tiles.FirstOrDefault(t => t.X - minX == colIndex && t.Y - minY == row);
                                if (tile != null && !string.IsNullOrEmpty(tile.Base64Encoded))
                                {
                                    var imageData = Convert.FromBase64String(tile.Base64Encoded);
                                    rowContent.ConstantItem(tileSize)
                                        .Image(imageData);
                                }
                                else
                                {
                                    rowContent.ConstantItem(tileSize)
                                        .Placeholder();
                                }
                            }
                        });
                    }
                });
        }

        private static byte[] GeneratePdf(Document document)
        {
            using var memoryStream = new MemoryStream();
            document.GeneratePdf(memoryStream);
            return memoryStream.ToArray();
        }

        private static IContainer ContainerWithPadding(IContainer container) => container.PaddingTop(20);
    }
}
