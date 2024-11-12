using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.IO.Image;
using System.IO;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Kernel.Geom;
using gestionactivos.Models;
using gestionactivos.Error;
using NuGet.Configuration;
namespace gestionactivos.pdf
{
    public class Template
    {
        public string GeneratePdf(string outputPath, List<GenerarpdfModel> asignaciones)
        {
            var errorLogger = new ErrorLogger();
            try
            {
                var writer = new PdfWriter(outputPath);
                var pdf = new PdfDocument(writer);

                var document = new Document(pdf, PageSize.LEGAL);

                // Definir los márgenes si es necesario
                document.SetMargins(20, 20, 20, 20);

                // Crear tabla con proporciones de columna 20%, 60%, 20%
                Table table = new Table(UnitValue.CreatePercentArray(new float[] { 20, 60, 20 }))
                    .SetWidth(UnitValue.CreatePercentValue(100));

                // Agregar celdas a la tabla principal

                ImageData imageData = ImageDataFactory.Create("wwwroot/Img/Plantilla/logoSumimas.png");
                Image image = new Image(imageData).ScaleToFit(100, 50) // Escala la imagen
                    .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                table.AddCell(new Cell().Add(image)
                    .SetHeight(50)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPadding(5)
                    .SetMargin(0)
                    .SetBorder(new SolidBorder(1))
                );

                // Crear tabla anidada
                Table nested = new Table(UnitValue.CreatePercentArray(1))
                    .SetWidth(UnitValue.CreatePercentValue(100)); // Asegúrate de que la tabla anidada ocupe todo el ancho de la celda



                nested.AddCell(new Cell().Add(new Paragraph("SERVICIOS Y GARANTIAS"))
                    .SetHeight(30)
                    .SetFontSize(10)
                    .SetBorderBottom(new SolidBorder(1))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPadding(0) // Elimina padding para evitar espacio adicional
                    .SetMargin(0)); // Elimina margin para evitar espacio adicional


                var EventoMovimiento = asignaciones[0].EventoMovimiento;

                if (EventoMovimiento == "Asignación - Historica" || EventoMovimiento == "Devolucion - Historica")
                {
                    EventoMovimiento = "Devolución";
                }

                nested.AddCell(new Cell().Add(new Paragraph("REPORTE DE SERVICIO - " + EventoMovimiento))
                    .SetHeight(30)
                    .SetFontSize(10)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPadding(0) // Elimina padding para evitar espacio adicional
                    .SetMargin(0)); // Elimina margin para evitar espacio adicional

                // Celda que ocupa una columna y contiene la tabla anidada
                Cell nesthousing = new Cell(1, 1)
                    .Add(nested)
                    .SetPadding(0)
                    .SetMargin(0); // Elimina padding y margin para ajustar el contenido correctamente

                // Reemplazar la celda número 2 con la celda que contiene la tabla anidada
                table.AddCell(nesthousing);

                // Crear la segunda tabla anidada para la columna 3
                Table nest = new Table(UnitValue.CreatePercentArray(1))
                    .SetWidth(UnitValue.CreatePercentValue(100)); // Asegúrate de que la tabla anidada ocupe todo el ancho de la celda

                nest.AddCell(new Cell().Add(new Paragraph("CÓDIGO: SG-FR34"))
                    .SetHeight(30)
                    .SetFontSize(7)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPadding(0) // Elimina padding para evitar espacio adicional
                    .SetMargin(0)); // Elimina margin para evitar espacio adicional

                nest.AddCell(new Cell().Add(new Paragraph("FECHA: 16/05/2023"))
                    .SetHeight(15)
                    .SetFontSize(7)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPadding(0) // Elimina padding para evitar espacio adicional
                    .SetMargin(0)); // Elimina margin para evitar espacio adicional

                nest.AddCell(new Cell().Add(new Paragraph("VERSIÓN: 3"))
                    .SetHeight(15)
                    .SetFontSize(7)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetPadding(0) // Elimina padding para evitar espacio adicional
                    .SetMargin(0)); // Elimina margin para evitar espacio adicional

                // Celda que ocupa una columna y contiene la tabla anidada
                Cell nesthousi = new Cell(1, 1)
                    .Add(nest)
                    .SetPadding(0)
                    .SetMargin(0); // Elimina padding y margin para ajustar el contenido correctamente

                // Reemplazar la celda número 3 con la celda que contiene la tabla anidada
                table.AddCell(nesthousi);



                document.Add(table);





                Table table2 = new Table(1);
                table2.SetWidth(UnitValue.CreatePercentValue(100));

                table2.AddCell(new Cell().Add(new Paragraph(""))
                    .SetHeight(10)
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE));

                document.Add(table2);



                List bulletList = new List().SetListSymbol("");
                bulletList.Add(new ListItem("DE:"));
                bulletList.Add(new ListItem("DIRECCIÓN"));
                bulletList.Add(new ListItem("NIT"));
                bulletList.Add(new ListItem("TEL:/ FAX:"));
                bulletList.Add(new ListItem("PROYECTO / CLIENTE:"));

                List bulletList2 = new List().SetListSymbol("");


                var RazonSocialEmpresa = asignaciones[0].RazonSocialEmpresa;
                var DireccionEmpresa = asignaciones[0].DireccionEmpresa;
                var NitEmpresa = asignaciones[0].NitEmpresa;
                var TelefonoEmpresa = asignaciones[0].TelefonoEmpresa;
                var NombreClienteFuncionario = asignaciones[0].NombreClienteFuncionario;


                bulletList2.Add(new ListItem(RazonSocialEmpresa));
                bulletList2.Add(new ListItem(DireccionEmpresa));
                bulletList2.Add(new ListItem(NitEmpresa));
                bulletList2.Add(new ListItem(TelefonoEmpresa));
                bulletList2.Add(new ListItem(NombreClienteFuncionario));


                Table table3 = new Table(2)
                    .SetWidth(UnitValue.CreatePercentValue(100))
                    .SetFontSize(8);

                Cell cell = new Cell().Add(bulletList);
                table3.AddCell(cell);

                Cell cell2 = new Cell().Add(bulletList2);
                table3.AddCell(cell2);

                document.Add(table3);

                var FechaMovimiento = asignaciones[0].FechaMovimiento;
                string fechaMovimientoStr = FechaMovimiento.ToString("dd/MM/yyyy");
                var LugarEntrega = $"{asignaciones[0].NombreSedeFuncionario}, {asignaciones[0].DireccionSedeFuncionario}";
                var NombreEncargado = $"{asignaciones[0].NombreFuncionario}  {asignaciones[0].ApellidoFuncionario}";
                var AreaFuncionario = asignaciones[0].AreaFuncionario;


                List bulletList3 = new List().SetListSymbol("");
                bulletList3.Add(new ListItem("FECHA:"));
                bulletList3.Add(new ListItem("LUGAR DE ENTREGA:"));
                bulletList3.Add(new ListItem("ENTREGADO A:"));
                bulletList3.Add(new ListItem("AREA: "));


                List bulletList4 = new List().SetListSymbol("");
                bulletList4.Add(new ListItem(fechaMovimientoStr));
                bulletList4.Add(new ListItem(LugarEntrega));
                bulletList4.Add(new ListItem(NombreEncargado));
                bulletList4.Add(new ListItem(AreaFuncionario));


                var CuidadSede = asignaciones[0].CuidadSede;
                var TelefonoEncargado = asignaciones[0].TelefonoFuncionario;
                var CargoEncargado = asignaciones[0].CargoFuncionario;
                var PisoEncargado = asignaciones[0].PisoFuncionario;


                List bulletList5 = new List().SetListSymbol("");
                bulletList5.Add(new ListItem("CIUDAD:"));
                bulletList5.Add(new ListItem("TELÉFONO:"));
                bulletList5.Add(new ListItem("CARGO:"));
                bulletList5.Add(new ListItem("PISO:"));


                List bulletList6 = new List().SetListSymbol("");
                bulletList6.Add(new ListItem(CuidadSede));
                bulletList6.Add(new ListItem(TelefonoEncargado));
                bulletList6.Add(new ListItem(CargoEncargado));
                bulletList6.Add(new ListItem(PisoEncargado));
                Cell cellWithBorder = new Cell().Add(bulletList6);




                Table table4 = new Table(4)
                    .SetFontSize(8);

                Cell celltable1 = new Cell().Add(bulletList3);
                table4.AddCell(celltable1);
                celltable1.SetWidth(122);


                Cell celltable2 = new Cell().Add(bulletList4);
                table4.AddCell(celltable2);
                celltable2.SetWidth(180);

                Cell celltable3 = new Cell().Add(bulletList5);
                table4.AddCell(celltable3);
                celltable3.SetWidth(50);

                Cell celltable4 = new Cell().Add(bulletList6);
                table4.AddCell(celltable4);
                celltable4.SetWidth(210);


                document.Add(table4);


                Table tablePrincipal = new Table(5)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetWidth(UnitValue.CreatePercentValue(100))
                    .SetFontSize(8);


                // Añadir encabezados
                tablePrincipal.AddCell(new Cell().Add(new Paragraph("ITEM").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tablePrincipal.AddCell(new Cell().Add(new Paragraph("SERIAL").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tablePrincipal.AddCell(new Cell().Add(new Paragraph("PLACA SUMIMAS").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tablePrincipal.AddCell(new Cell().Add(new Paragraph("PLACA MINTIC").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tablePrincipal.AddCell(new Cell().Add(new Paragraph("MODELO / DESCRIPCIÓN").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                //Filas
                var SerialArticulo = asignaciones[0].SerialArticulo;
                var PlacaArticulo = asignaciones[0].PlacaArticulo;
                var CategoriaArticulo = asignaciones[0].CategoriaArticulo;
                var ModeloArticulo = asignaciones[0].ModeloArticulo;

                tablePrincipal.AddCell(new Cell().Add(new Paragraph("1")));
                tablePrincipal.AddCell(new Cell().Add(new Paragraph(SerialArticulo)));
                tablePrincipal.AddCell(new Cell().Add(new Paragraph(PlacaArticulo)));
                tablePrincipal.AddCell(new Cell().Add(new Paragraph("")));
                tablePrincipal.AddCell(new Cell().Add(new Paragraph($"{ModeloArticulo} ")));

                document.Add(tablePrincipal);






                float[] mainTableWidthsValidacion = { 30, 10, 10, 50 };
                Table tableValidacion = new Table(UnitValue.CreatePercentArray(mainTableWidthsValidacion))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetWidth(UnitValue.CreatePercentValue(100))
                    .SetFontSize(8);

                // Añadir el encabezado 
                Cell headerValidacion = new Cell(1, 4)
                    .Add(new Paragraph("VALIDACIÓN DE ENTREGA").SetBold())
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER);
                tableValidacion.AddCell(headerValidacion);

                // Añadir los subtítulos
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SOFTWARE O COMPLEMENTO").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("CUMPLE").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO CUMPLE").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("OBSERVACIÓN").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY));

                //Filas 
                tableValidacion.AddCell(new Cell().Add(new Paragraph("7 ZIP").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));

                tableValidacion.AddCell(new Cell().Add(new Paragraph("GOOGLE CHROME").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));
                //Filas 
                tableValidacion.AddCell(new Cell().Add(new Paragraph("FIREFOX").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));

                tableValidacion.AddCell(new Cell().Add(new Paragraph("FORTICLIENT").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));
                //Filas 
                tableValidacion.AddCell(new Cell().Add(new Paragraph("TEAMS").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));

                tableValidacion.AddCell(new Cell().Add(new Paragraph("OFFICE 365").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));

                //Filas 
                tableValidacion.AddCell(new Cell().Add(new Paragraph("MICROCLAUDIA").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));

                tableValidacion.AddCell(new Cell().Add(new Paragraph("PDF EXCHANGE").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));
                //Filas 
                tableValidacion.AddCell(new Cell().Add(new Paragraph("BITLOCKER").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));

                tableValidacion.AddCell(new Cell().Add(new Paragraph("SERVICE MANAGER").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));
                //Filas 
                tableValidacion.AddCell(new Cell().Add(new Paragraph("USUARIO ADMIN").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));

                tableValidacion.AddCell(new Cell().Add(new Paragraph("DOMINIO").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));
                //Filas 
                tableValidacion.AddCell(new Cell().Add(new Paragraph("IVANTI").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));

                tableValidacion.AddCell(new Cell().Add(new Paragraph("HORUS").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("")).SetPadding(0));

                tableValidacion.AddCell(new Cell().Add(new Paragraph("BACKUP").SetMarginLeft(2)).SetPadding(0).SetTextAlignment(TextAlignment.LEFT));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SI")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("NO")).SetPadding(0));
                tableValidacion.AddCell(new Cell().Add(new Paragraph("SE APRUEBA POR PARTE DEL USUARIO LA COPIA DE LA INFORMACION, DONDE REALIZA LAVALIDACION DE TODOS SUS DATOS Y DA VISTO BUENO A LA TRANSFERENCIA DE SU INFORMACION").SetFontSize(6).SetTextAlignment(TextAlignment.JUSTIFIED)));


                // Puedes continuar agregando más filas como las anteriores

                // Añadir tablas al documento


                document.Add(tableValidacion);  // Añadir tabla de validación de entrega



                // Crear la tabla principal con 6 columnas
                float[] mainTableWidthsElementos = { 15, 20, 15, 20, 15, 20 };
                Table tableElementos = new Table(UnitValue.CreatePercentArray(mainTableWidthsElementos))
                    .SetWidth(UnitValue.CreatePercentValue(100))
                    .SetFontSize(8).SetPadding(0).SetMargin(0);

                // Añadir el encabezado de "ELEMENTOS ENTREGADOS" que cubre 6 columnas
                Cell headerElementos = new Cell(1, 6)
                    .Add(new Paragraph("ELEMENTOS ENTREGADOS").SetBold())
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER);
                tableElementos.AddCell(headerElementos);

                // Añadir los subtítulos de las columnas
                for (int i = 0; i < 3; i++)
                {
                    tableElementos.AddCell(new Cell().Add(new Paragraph("ITEM").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(0).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                    tableElementos.AddCell(new Cell().Add(new Paragraph("SERIAL").SetBold()).SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetPadding(0).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE));
                }

                // Selección de adicionales válidos
                var adicionales = asignaciones.SelectMany(a => a.Adicionales).ToList();
                var validAdicionales = adicionales.Where(a => !string.IsNullOrEmpty(a.CategoriaAdicional) ||
                                                              !string.IsNullOrEmpty(a.ModeloAdicional) ||
                                                              !string.IsNullOrEmpty(a.SerialAdicional) ||
                                                              !string.IsNullOrEmpty(a.PlacaAdicional)).ToList();

                int rowCount = 0; // Contador de filas

                if (validAdicionales.Count == 0)
                {
                    // Si no hay adicionales válidos, sólo se añaden los extras
                    List<Tuple<string, bool>> extras = new List<Tuple<string, bool>> {
        new Tuple<string, bool>("Maleta", asignaciones[0].ExtraMaleta),
        new Tuple<string, bool>("Guaya", asignaciones[0].ExtraGuaya),
        new Tuple<string, bool>("Base", asignaciones[0].ExtraBase),
        new Tuple<string, bool>("Cargador", asignaciones[0].ExtraCargador),
        new Tuple<string, bool>("PadMouse", asignaciones[0].ExtraPadMouse),
        new Tuple<string, bool>("Diadema", asignaciones[0].ExtraDiadema)
    };

                    foreach (var extra in extras)
                    {
                        if (extra.Item2 && rowCount < 5)
                        {
                            tableElementos.AddCell(new Cell().Add(new Paragraph(extra.Item1)));
                            tableElementos.AddCell(new Cell().Add(new Paragraph("SI")));
                            rowCount++;
                        }
                    }
                }
                else
                {
                    // Si hay adicionales válidos, agregar y contar las filas
                    foreach (var adicional in validAdicionales)
                    {
                        if (rowCount >= 5) break;

                        string categoriaModelo = $"{adicional.CategoriaAdicional}, {adicional.ModeloAdicional}";
                        string serialPlaca = $"S/N {adicional.SerialAdicional}, Placa: {adicional.PlacaAdicional}";

                        tableElementos.AddCell(new Cell().Add(new Paragraph(categoriaModelo)));
                        tableElementos.AddCell(new Cell().Add(new Paragraph(serialPlaca)));
                        rowCount++;
                    }

                    // Agregar extras después de los adicionales si hay espacio
                    List<Tuple<string, bool>> extras = new List<Tuple<string, bool>> {
        new Tuple<string, bool>("Maleta", asignaciones[0].ExtraMaleta),
        new Tuple<string, bool>("Guaya", asignaciones[0].ExtraGuaya),
        new Tuple<string, bool>("Base", asignaciones[0].ExtraBase),
        new Tuple<string, bool>("Cargador", asignaciones[0].ExtraCargador),
        new Tuple<string, bool>("PadMouse", asignaciones[0].ExtraPadMouse),
        new Tuple<string, bool>("Diadema", asignaciones[0].ExtraDiadema)
    };

                    foreach (var extra in extras)
                    {
                        if (extra.Item2 && rowCount < 5)
                        {
                            tableElementos.AddCell(new Cell().Add(new Paragraph(extra.Item1)));
                            tableElementos.AddCell(new Cell().Add(new Paragraph("SI")));
                            rowCount++;
                        }
                    }
                }

                // Rellenar filas vacías hasta alcanzar las 5 filas
                for (int i = rowCount; i < 5; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        tableElementos.AddCell(new Cell().Add(new Paragraph("").SetMarginLeft(2).SetPadding(0).SetHeight(10)));
                        tableElementos.AddCell(new Cell().Add(new Paragraph("").SetMarginLeft(2).SetPadding(0).SetHeight(10)));
                    }
                }

                // Añadir la tabla al documento
                document.Add(tableElementos);






                // Definir anchos específicos para la tabla principal
                float[] mainTableWidths = { 20, 80 }; // Anchos de columnas: 20% para la columna "MORRAL" y 80% para la tabla anidada

                // Crear la tabla principal con 2 columnas y anchos específicos
                Table tableObservacion = new Table(UnitValue.CreatePercentArray(mainTableWidths))
                    .SetWidth(UnitValue.CreatePercentValue(100)) // Ancho total de la tabla
                    .SetFontSize(8);

                // Agregar celda con el texto "MORRAL" en la primera columna
                tableObservacion.AddCell(new Cell().Add(new Paragraph("OBSERVACIONES"))
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());

                // Crear la tabla anidada (segunda tabla) con 4 columnas y anchos específicos
                float[] columnWidths = { 15, 30, 20, 35 }; // Proporción del ancho de las columnas
                Table tableObservacion2 = new Table(UnitValue.CreatePercentArray(columnWidths))
                    .SetWidth(UnitValue.CreatePercentValue(100)) // Ancho total de la tabla
                    .SetFontSize(8);

                Cell headerObservacion = new Cell(1, 4) // Encabezado ocupando las 4 columnas
                    .Add(new Paragraph("ELEMENTOS ENTREGADOS").SetBold())
                    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                    .SetTextAlignment(TextAlignment.CENTER);
                tableObservacion2.AddCell(headerObservacion);



                var CedulaResponsable = asignaciones[0].CedulaContratista;

                if (CedulaResponsable != null)
                {
                    var NombreResponsable = $"{asignaciones[0].NombreContratista}  {asignaciones[0].ApellidoContratista}";
                    var AreaResponsable = asignaciones[0].AreaContratista;
                    var PisoContratista = asignaciones[0].PisoContratista;

                    var FirmaResponsable = asignaciones[0].FirmaContratista;
                    byte[] imageBytes = null;

                    if (FirmaResponsable != null) // Verifica si la firma no es nula
                    {
                        imageBytes = Convert.FromBase64String(FirmaResponsable);
                        ImageData imageData3 = ImageDataFactory.Create(imageBytes);
                        Image firmaImage3 = new Image(imageData3)
                            .ScaleToFit(100, 50) // Escala la imagen
                            .SetHorizontalAlignment(HorizontalAlignment.CENTER);

                        tableObservacion2.AddCell(new Cell().Add(new Paragraph("CEDULA:")));
                        tableObservacion2.AddCell(new Cell().Add(new Paragraph(CedulaResponsable)));
                        tableObservacion2.AddCell(new Cell(4, 1).Add(new Paragraph("FIMA CONTRATISTA")) // Columna vacía, ocupa 4 filas
                            .SetTextAlignment(TextAlignment.CENTER));
                        tableObservacion2.AddCell(new Cell(4, 1).Add(firmaImage3) // Otra columna vacía, ocupa 4 filas
                            .SetTextAlignment(TextAlignment.CENTER));
                    }
                    else
                    {
                        tableObservacion2.AddCell(new Cell().Add(new Paragraph("Pendiente por firmar"))
                            .SetTextAlignment(TextAlignment.CENTER)); // Mensaje de pendiente
                    }





                    tableObservacion2.AddCell(new Cell().Add(new Paragraph("ENTREGADO A:")));
                    tableObservacion2.AddCell(new Cell().Add(new Paragraph(NombreResponsable)));

                    tableObservacion2.AddCell(new Cell().Add(new Paragraph("AREA:")));
                    tableObservacion2.AddCell(new Cell().Add(new Paragraph(AreaResponsable)));

                    tableObservacion2.AddCell(new Cell().Add(new Paragraph("PISO:")));
                    tableObservacion2.AddCell(new Cell().Add(new Paragraph(PisoContratista)));


                }
                else
                {
                    var ObservacionMovimiento = asignaciones[0].ObservacionMovimiento;

                    // Verificamos si ObservacionMovimiento tiene datos
                    if (ObservacionMovimiento != null)
                    {
                        tableObservacion2.AddCell(new Cell().Add(new Paragraph(ObservacionMovimiento)));
                    }
                    else
                    {
                        tableObservacion2.AddCell(new Cell(1, 4).Add(new Paragraph("")));
                    }

                    // Crear una celda que ocupe el 100% de la fila (usando colspan=4 para una tabla de 4 columnas)
                    tableObservacion2.AddCell(new Cell(2, 4) // Celda que ocupa 1 fila y 4 columnas
                        .Add(new Paragraph("EN CASO DE HURTO DEBERÉ REMITIR DENUNCIO DE LA FISCALÍA AL CORREO DEL JEFE INMEDIATO, AL COORDINADOR DEL GIT DE SERVICIOS TECNOLÓGICOS Y AL COORDINADOR DEL PARQUE COMPUTACIONAL")
                        .SetTextAlignment(TextAlignment.JUSTIFIED) // Alineación justificada
                        .SetMultipliedLeading(1.2f) // Espaciado entre líneas
                        .SetPadding(5)) // Espacio dentro de la celda
                    );

                }


                // Agregar los items en la primera columna










                // Crear la celda que contiene la tabla anidada
                Cell nestedTableCell = new Cell()
                    .Add(tableObservacion2)
                    .SetPadding(0)  // Eliminar relleno para evitar espacios adicionales
                    .SetMargin(0)   // Eliminar márgenes para evitar espacio alrededor
                    .SetBorder(Border.NO_BORDER);  // Opcional: quitar el borde alrededor de la celda

                // Agregar la celda con la tabla anidada en la segunda columna de la tabla principal
                tableObservacion.AddCell(nestedTableCell);

                // Agregar la tabla principal al documento
                document.Add(tableObservacion);



                // Definir anchos específicos para la tabla principal
                float[] mainTableWidthsAnexos = { 20, 80 }; // Anchos de columnas: 20% para la columna "MORRAL" y 80% para la tabla anidada

                //// Crear la tabla principal con 2 columnas y anchos específicos
                Table tableAnexos = new Table(UnitValue.CreatePercentArray(mainTableWidthsAnexos))
                   .SetWidth(UnitValue.CreatePercentValue(100)) // Ancho total de la tabla
                   .SetFontSize(8);

                tableAnexos.AddCell(new Cell().Add(new Paragraph("Anexos"))
                   .SetTextAlignment(TextAlignment.CENTER)
                   .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                   .SetBackgroundColor(ColorConstants.LIGHT_GRAY).SetBold());
                tableAnexos.AddCell(new Cell().Add(new Paragraph("LOS COMPONENTES SE ENTREGAN EN EXCELENTES CONDICIONES Y EL CLIENTEDECLARA HABERLOS RECIBIDO A SATISFACCIÓN Y ASUME LA RESPONSABILIDAD SOBRELOS MISMOS A PARTIR DE LA FIRMA DE LA PRESENTE ACTA. EL CLIENTE ASUME LA RESPONSABILIDAD DE LA CUSTODIA DE LOS PRODUCTOS, CONDICIONES DEALMACENAMIENTO Y AL FIRMAR ESTA ACTA LIBRA A SUMIMAS S.A.S., DE CUALQUIER RESPONSABILIDAD O RIESGO QUE PUEDAN SUFRIR LOS EQUIPOS."))
                   .SetFontSize(7).SetTextAlignment(TextAlignment.JUSTIFIED));

                document.Add(tableAnexos);




                //float[] mainTableWidthFirmas = { 50, 50 };

                //Table tableFirmas = new Table(UnitValue.CreatePercentArray(mainTableWidthFirmas))
                //   .SetWidth(UnitValue.CreatePercentValue(100)) // Ancho total de la tabla
                //   .SetFontSize(8);

                //tableFirmas.AddCell(new Cell().Add(new Paragraph("*VARIABLE*")).SetHeight(50).SetBorderTop(Border.NO_BORDER));
                //tableFirmas.AddCell(new Cell().Add(new Paragraph("*VARIABLE*")));
                //tableFirmas.AddCell(new Cell().Add(new Paragraph("ENTREGADO Y REALIZADO POR EL TECNICO :")));
                //tableFirmas.AddCell(new Cell().Add(new Paragraph("RECIBIDO O ENTREGADO POR :")));
                //tableFirmas.AddCell(new Cell().Add(new Paragraph("*VARIABLE*")).SetHeight(50));
                //tableFirmas.AddCell(new Cell().Add(new Paragraph("*VARIABLE*")));
                //tableFirmas.AddCell(new Cell().Add(new Paragraph("FIRMA TECNICO SUMIMAS S.A.S")));
                //tableFirmas.AddCell(new Cell().Add(new Paragraph("FIRMA FUNCIONARIO MINTIC")));

                //document.Add(tableFirmas);


                //float[] mainTableWidths2 = { 20, 80 };
                //Table mainTable = new Table(UnitValue.CreatePercentArray(mainTableWidths2)).SetWidth(UnitValue.CreatePercentValue(100));

                //// Añadir celda de "Anexos" en la primera columna
                //Cell annexCell = new Cell(1, 1)
                //    .Add(new Paragraph("Anexos").SetBold())
                //    .SetBackgroundColor(ColorConstants.LIGHT_GRAY)
                //    .SetTextAlignment(TextAlignment.CENTER)
                //    .SetVerticalAlignment(VerticalAlignment.MIDDLE);
                //mainTable.AddCell(annexCell);

                //// Añadir celda de texto largo en la segunda columna
                //Cell descriptionCell = new Cell(1, 1)
                //    .Add(new Paragraph("LOS COMPONENTES SE ENTREGAN EN EXCELENTES CONDICIONES...")) // Texto largo
                //    .SetTextAlignment(TextAlignment.LEFT)
                //    .SetVerticalAlignment(VerticalAlignment.MIDDLE)
                //    .SetPadding(5);
                //mainTable.AddCell(descriptionCell);

                //// Añadir espacio vacío (línea horizontal)
                //mainTable.AddCell(new Cell(1, 2).Add(new Paragraph("")).SetBorder(Border.NO_BORDER));


                // Tabla para las firmas
                float[] signatureTableWidths = { 50, 50 };
                Table signatureTable = new Table(UnitValue.CreatePercentArray(signatureTableWidths))
                    .SetWidth(UnitValue.CreatePercentValue(100))
                    .SetFontSize(10);

                // Crear celdas para "ENTREGADO Y REALIZADO POR EL TECNICO" y "RECIBIDO O ENTREGADO POR"
                signatureTable.AddCell(new Cell().Add(new Paragraph("ENTREGADO Y REALIZADO POR EL TECNICO :")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE))

                    .SetBorderRight(Border.NO_BORDER)

                    );

                signatureTable.AddCell(new Cell().Add(new Paragraph("RECIBIDO O ENTREGADO POR :")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetVerticalAlignment(VerticalAlignment.MIDDLE)

                    ).SetBorderLeft(Border.NO_BORDER));






                var FirmaUsuario = asignaciones[0].FirmaUsuario;
                byte[] imageBytes1 = null;
                if (FirmaUsuario != null)
                {
                    imageBytes1 = Convert.FromBase64String(FirmaUsuario);
                    ImageData imageData1 = ImageDataFactory.Create(imageBytes1);
                    Image firmaImage1 = new Image(imageData1)
                        .ScaleToFit(100, 50) // Escala la imagen
                        .SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    signatureTable.AddCell(new Cell().Add(firmaImage1).SetTextAlignment(TextAlignment.CENTER).SetBorderRight(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER)); // Firma técnica
                }
                else
                {
                    signatureTable.AddCell(new Cell().Add(new Paragraph("Pendiente por firmar"))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorderLeft(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER)); // Mensaje de pendiente
                }




                var FirmaEncargado = asignaciones[0].FirmaFuncionario;
                byte[] imageBytes2 = null;
                if (FirmaEncargado != null)
                {
                    imageBytes2 = Convert.FromBase64String(FirmaEncargado);
                    ImageData imageData2 = ImageDataFactory.Create(imageBytes2);
                    Image firmaImage2 = new Image(imageData2)
                        .ScaleToFit(100, 50) // Escala la imagen
                        .SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    signatureTable.AddCell(new Cell().Add(firmaImage2).SetTextAlignment(TextAlignment.CENTER).SetBorderLeft(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER)); // Firma MINTIC
                }
                else
                {
                    signatureTable.AddCell(new Cell().Add(new Paragraph("Pendiente por firmar"))
                        .SetTextAlignment(TextAlignment.CENTER)
                        .SetBorderLeft(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER)); // Mensaje de pendiente
                }



              




                // Agregar la firma técnica asumiendo que firmaImage1 ya está definido antes



                var NombreEntrega = $"{asignaciones[0].NombreUsuario} {asignaciones[0].ApellidoUsuario}";

                signatureTable.AddCell(new Cell().Add(new Paragraph(NombreEntrega).SetTextAlignment(TextAlignment.CENTER)).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));
                signatureTable.AddCell(new Cell().Add(new Paragraph(NombreEncargado).SetTextAlignment(TextAlignment.CENTER)).SetBorderLeft(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));

                // Añadir nombres de las firmas
                signatureTable.AddCell(new Cell().Add(new Paragraph("FIRMA TECNICO SUMIMAS S.A.S").SetTextAlignment(TextAlignment.CENTER)).SetBorderRight(Border.NO_BORDER));
                signatureTable.AddCell(new Cell().Add(new Paragraph("FIRMA FUNCIONARIO MINTIC").SetTextAlignment(TextAlignment.CENTER)).SetBorderLeft(Border.NO_BORDER));

                // Añadir la tabla de firmas a la tabla principal
                //mainTable.AddCell(new Cell(1, 2).Add(signatureTable).SetBorder(Border.NO_BORDER));

                // Añadir tabla principal al documento
                document.Add(signatureTable);


                document.Close();

                // Devuelve el archivo PDF en un array de bytes

            }
            catch (Exception ex)
            {
                errorLogger.RegistrarError(ex , "Descargar Template Metodo" + nameof(GeneratePdf));
                throw;
            }
            return outputPath;





        }
    
    
    
    
    
    
    
    }
}
