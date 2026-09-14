using DocumentFormat.OpenXml.Spreadsheet;
using MUTZI.GUIA.APPLICATION.DTOs.GuiaRemitente;
using MUTZI.GUIA.DOMAIN.Entities;
using MUTZI.GUIA.DOMAIN.Interfaces.Document;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MUTZI.GUIA.INFRASTRUCTURE.Document
{
    public class GuiaRemisionDocument : IGuiaRemisionDocument
    {

        private readonly GuiaRemision _dto;
        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public GuiaRemisionDocument(GuiaRemision dto)
        {
            _dto = dto;
        }

        public void Compose(IDocumentContainer container)
        {
            var textXS = 7.5f;
            var textMD = 8f;
            var textLG = 9f;
            var textColor = "#60615b";
            var borderColor = "#bababa";
            var borderSize = 1f;

            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4.Landscape());
                page.DefaultTextStyle(x => x
                    .FontColor(textColor)
                    .FontFamily("Arial")
                    .FontSize(textXS)
                );

                page.Header()
                    .Column(headerCol =>
                    {
                        headerCol.Spacing(10);

                        headerCol.Item()
                            .Row(row =>
                            {
                                row.ConstantItem(100)
                                    .Column(col =>
                                    {
                                        col.Item().Text("Aquí va el log de la empresa");
                                    });

                                row.RelativeItem()
                                    .Column(col =>
                                    {
                                        col.Spacing(5);
                                        col.Item().Text($"{_dto.Entity?.Name ?? "ERROR"}")
                                            .Bold().FontSize(textXS);
                                        col.Item().Text(_dto.Entity?.Address ?? "ERROR")
                                            .FontSize(textXS);
                                        col.Item().Text("");
                                        col.Item().Text($"{_dto.Entity?.Ubigeo.Distrito} - {_dto.Entity?.Ubigeo.Provincia} - {_dto.Entity?.Ubigeo.Departamento}")
                                            .FontSize(textXS);
                                    });

                                row.ConstantItem(200)
                                    .Border(borderSize)
                                    .BorderColor(borderColor)
                                    .CornerRadius(10)
                                    .Padding(10)
                                    .PaddingLeft(20)
                                    .PaddingRight(20)
                                    .Height(50)
                                    .Column(col =>
                                    {
                                        col.Spacing(5);
                                        col.Item().Text($"RUC: {_dto.Ruc}").AlignCenter().FontSize(textXS);
                                        col.Item().Text($"GUÍA DE REMISIÓN {_dto.TipoGuia} ELECTRÓNICA").Bold().FontSize(textLG).AlignCenter();
                                        col.Item().Text($"Nro. {_dto.NumeroGuia}").AlignCenter().FontSize(textXS);
                                    });
                            });

                        headerCol.Item()
                            .Border(borderSize)
                            .BorderColor(borderColor)
                            .CornerRadius(8)
                            .Padding(10)
                            .Column(col =>
                            {
                                col.Spacing(5);

                                col.Item()
                                .Row(row =>
                                {
                                    row.ConstantItem(80)
                                        .Column(leftCol =>
                                        {
                                            leftCol.Item().Text("Cliente:").FontSize(textXS).SemiBold();
                                        });
                                    row.RelativeItem()
                                        .Column(rightCol =>
                                        {
                                            rightCol.Spacing(5);
                                            rightCol.Item().Text(_dto.Destinatario.Entity?.Name != null ? _dto.Destinatario.Entity.Name : $"{_dto.Destinatario.Entity.FirstName} {_dto.Destinatario.Entity.LastName}").FontSize(textXS);
                                        });
                                });

                                col.Item()
                                .Row(row =>
                                {
                                    row.ConstantItem(80)
                                        .Column(leftCol =>
                                        {
                                            leftCol.Spacing(5);
                                            leftCol.Item().Text("RUC:").FontSize(textXS).SemiBold();
                                            leftCol.Item().Text("Fecha de emisión:").FontSize(textXS).SemiBold();
                                        });

                                    row.ConstantItem(140)
                                        .Column(rightCol =>
                                        {
                                            rightCol.Spacing(5);
                                            rightCol.Item().Text(_dto.Destinatario.Entity?.DocumentNumber).FontSize(textXS);
                                            rightCol.Item().Text(_dto.Fecha.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToUpper()).FontSize(textXS);
                                        });

                                    row.ConstantItem(50)
                                        .Column(leftCol =>
                                        {
                                            leftCol.Spacing(5);
                                            leftCol.Item().Text("Dirección:").FontSize(textXS).SemiBold();
                                            leftCol.Item().Text("Ciudad:").FontSize(textXS).SemiBold();
                                        });

                                    row.RelativeItem()
                                        .Column(rightCol =>
                                        {
                                            rightCol.Spacing(5);
                                            rightCol.Item().Text(_dto.Destinatario.Entity?.Address).FontSize(textXS);
                                            rightCol.Item().Text($"{_dto.Destinatario.Entity?.Ubigeo.Distrito} - {_dto.Destinatario.Entity?.Ubigeo.Provincia} - {_dto.Destinatario.Entity?.Ubigeo.Departamento}").FontSize(textXS);
                                        });
                                });

                            });

                    });


                page.Content()
                .PaddingTop(10)
                .Column(col =>
                {
                    col.Item().Row(row1 =>
                    {
                        row1.Spacing(10);
                        row1.RelativeItem()
                            .Column(leftCol =>
                            {
                                leftCol.Item()
                                    .PaddingLeft(10)
                                    .PaddingRight(10)
                                    .PaddingBottom(10)
                                    .Column(col =>
                                    {
                                        col.Item().Text("DETALLE DE LA GUÍA:").FontSize(textMD).SemiBold();
                                    });

                                leftCol.Item()
                                    .Border(borderSize)
                                    .BorderColor(borderColor)
                                    .CornerRadius(8)
                                    .Padding(10)
                                    .Row(row =>
                                    {
                                        row.Spacing(5);
                                        row.RelativeItem()
                                            .Column(col =>
                                            {
                                                col.Spacing(5);
                                                col.Item().Text(text =>
                                                 {
                                                     text.Span("Tipo de Transporte: ").FontSize(textXS).SemiBold();
                                                     text.Span(_dto.TipoTransporte.ToUpper()).FontSize(textXS);
                                                 });
                                                col.Item().Text(text =>
                                                {
                                                    text.Span("Und. de Medida: ").FontSize(textXS).SemiBold();
                                                    text.Span(_dto.DatosEnvios.First()?.UnidadMedida?.DescripcionCorta).FontSize(textXS);
                                                });
                                                col.Item().Text(text =>
                                                {
                                                    text.Span("Motivo: ").FontSize(textXS).SemiBold();
                                                    text.Span(_dto.MotivoTraslado?.Nombre.ToUpper()).FontSize(textXS);
                                                });
                                            });

                                        row.RelativeItem()
                                            .Column(col =>
                                            {
                                                col.Spacing(5);
                                                col.Item().Text(text =>
                                                {
                                                    text.Span("MTC: ").FontSize(textXS).SemiBold();
                                                    text.Span(_dto.GuiaRemisionEntityCarriers.First()?.Entity.MtcCode).FontSize(textXS);
                                                });
                                                col.Item().Text(text =>
                                                {
                                                    text.Span("Peso Bruto: ").FontSize(textXS).SemiBold();
                                                    text.Span((_dto.DatosEnvios.First()?.PesoBruto ?? 0m).ToString("F2")).FontSize(textXS);
                                                });
                                                col.Item().Text(text =>
                                                {
                                                    text.Span("Descripción: ").FontSize(textXS).SemiBold();
                                                    text.Span("--").FontSize(textXS);
                                                });
                                            });

                                        row.RelativeItem()
                                            .Column(col =>
                                            {
                                                col.Spacing(5);
                                                col.Item().Text(text =>
                                                {
                                                    text.Span("Fecha entrega de bienes al transportista: ").FontSize(textXS).SemiBold();
                                                    text.Span(_dto.DatosEnvios.First()?.FechaEntregaTransportista?.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToUpper()).FontSize(textXS);
                                                });
                                            });
                                    });
                            });

                        row1.RelativeItem()
                            .Column(rightCol =>
                            {
                                rightCol.Item()
                                    .Row(row =>
                                    {
                                        row.Spacing(10);
                                        row.RelativeItem()
                                            .Column(col =>
                                            {
                                                col.Item()
                                                    .PaddingLeft(10)
                                                    .PaddingRight(10)
                                                    .PaddingBottom(10)
                                                    .Column(col =>
                                                    {
                                                        col.Item().Text("PUNTO DE PARTIDA:").FontSize(textMD).SemiBold();
                                                    });

                                                col.Item()
                                                    .Border(borderSize)
                                                    .BorderColor(borderColor)
                                                    .CornerRadius(8)
                                                    .Padding(10)
                                                    .Column(col1 =>
                                                    {
                                                        col1.Item().Text($"{_dto.Remitente?.Address}, {_dto.Remitente?.Ubigeo?.Departamento} - {_dto.Remitente?.Ubigeo?.Provincia} - {_dto.Remitente?.Ubigeo?.Distrito}")
                                                            .AlignCenter();
                                                        col1.Item().Text("");
                                                        col1.Item().Text(text =>
                                                        {
                                                            text.Span("RUC y Cod. de establecimiento: ").FontSize(textXS).SemiBold();
                                                            text.Span($"{_dto.Remitente?.Entity?.DocumentNumber} - {_dto.Remitente?.CodeSunat}").FontSize(textXS);
                                                        });
                                                    });
                                            });

                                        row.RelativeItem()
                                            .Column(col =>
                                            {
                                                col.Item()
                                                    .PaddingLeft(10)
                                                    .PaddingRight(10)
                                                    .PaddingBottom(10)
                                                    .Column(col =>
                                                    {
                                                        col.Item().Text("PUNTO DE LLEGADA:").FontSize(textMD).SemiBold();
                                                    });

                                                col.Item()
                                                    .Border(borderSize)
                                                    .BorderColor(borderColor)
                                                    .CornerRadius(8)
                                                    .Padding(10)
                                                    .Column(col1 =>
                                                    {
                                                        col1.Item().Text($"{_dto.Destinatario?.Address}, {_dto.Destinatario?.Ubigeo?.Departamento} - {_dto.Destinatario?.Ubigeo?.Provincia} - {_dto.Destinatario?.Ubigeo?.Distrito}")
                                                            .AlignCenter();
                                                        col1.Item().Text("");
                                                        col1.Item().Text(text =>
                                                        {
                                                            text.Span("RUC y Cod. de establecimiento: ").FontSize(textXS).SemiBold();
                                                            text.Span($"{_dto.Destinatario?.Entity?.DocumentNumber} - {_dto.Destinatario?.CodeSunat}").FontSize(textXS);
                                                        });
                                                    });
                                            });

                                    });
                            });
                    });



                    col.Item().Text("Cliente: CAROLINA SAC");
                    col.Item().Text("RUC: 20341191476");
                    col.Item().Text("Fecha de emisión: 22-SEP-2025");
                    col.Item().Text("Dirección: Av. Barrenechea 566");
                    col.Item().Text("Ciudad: SAN ISIDRO - LIMA - LIMA");

                    col.Item().Text("DOCUMENTOS RELACIONADOS:").Bold();
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("TIPO DE DOCUMENTO").Bold();
                            header.Cell().Text("NÚMERO DE DOCUMENTO").Bold();
                        });

                        table.Cell().Text("FACTURA");
                        table.Cell().Text("F001-12345");
                    });

                    col.Item().Text("DETALLE DE LA GUÍA:").Bold();
                    col.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(30);
                            columns.ConstantColumn(50);
                            columns.ConstantColumn(60);
                            columns.ConstantColumn(60);
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Text("ITEM").Bold();
                            header.Cell().Text("CANTIDAD").Bold();
                            header.Cell().Text("UNIDAD").Bold();
                            header.Cell().Text("CÓDIGO").Bold();
                            header.Cell().Text("DESCRIPCIÓN").Bold();
                        });

                        table.Cell().Text("1");
                        table.Cell().Text("4");
                        table.Cell().Text("Unidad");
                        table.Cell().Text("P001");
                        table.Cell().Text("TABLET 2");
                    });

                    col.Item().Text("Observaciones: Entrega de productos electrónicos");

                    col.Item().Text("DATOS DEL TRANSPORTISTA").Bold();
                    col.Item().Text("RUC: 20600824628");
                    col.Item().Text("Razón social: PETIT PÉ S.A.C.");

                    col.Item().Text("DATOS DEL PROVEEDOR").Bold();
                    col.Item().Text("Operador de Servicios Electrónicos según Resolución Nº 034-005-0008776");

                    col.Item().Text("Peso Bruto: 120.00 KGM");
                    col.Item().Text("Motivo: COMPRA");
                    col.Item().Text("Descripción: Venta de mercadería");

                    col.Item().Text("PUNTO DE PARTIDA: Cal. Augusto Angulo Nro. 130, LIMA - MIRAFLORES");
                    col.Item().Text("PUNTO DE LLEGADA: Av. Barrenechea 566, LIMA - SAN ISIDRO");

                    col.Item().Text("Nº de documento: 20109072177");
                    col.Item().Text("Nombre o Razón: CENCOSUD RETAIL PERU S.A.");
                    col.Item().Text("Dirección: Cal. Augusto Angulo Nro. 130");
                });

                page.Footer()
                    .AlignCenter()
                    .Text("Representación impresa de la guía de remisión remitente, consulte en www.efact.pe\nAutorizado mediante la Resolución de intendencia N° 0340050004177/SUNAT")
                    .FontSize(9).Italic();
            });
        }
    }
}
