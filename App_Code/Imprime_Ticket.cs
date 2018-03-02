using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using E_Utilities;
using System.Data;
using Telerik.Web.UI;
using System.Globalization;
using E_Utilities;

/// <summary>
/// Descripción breve de Imprime_Ticket
/// </summary>
public class Imprime_Ticket
{
    Fechas fechas = new Fechas();
    Ejecucion ejecuta = new Ejecucion();
    public Imprime_Ticket()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }

    public string GenerarTicket(int empresa, string usuario, int cliente, string tipo, decimal monto, int cantidad,string año,string mes,int ticket,string nomEmpresa, string refCliente, string periodo)
    {
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.B6, 0.5f, 0.5f, 0.5f, 0.5f);
        documento.AddTitle("Ticket de Servicio");
        documento.AddCreator("e-Bills");

        string ruta = HttpContext.Current.Server.MapPath("~/Tickets");
        string archivo = ruta + "\\Ticket_" + empresa.ToString() + "_" + ticket.ToString() + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
        {
            Directory.CreateDirectory(ruta);
        }

        if (archivo.Trim() != "")
        {
            FileStream file = new FileStream(archivo,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite);
            PdfWriter.GetInstance(documento, file);
            // Abrir documento.
            documento.Open();

            //Insertar logo o imagen            
            string rutaLogo = "";
            iTextSharp.text.Image logo = null;
            iTextSharp.text.Font standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font standardFont1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            try
            {
                rutaLogo = HttpContext.Current.Server.MapPath("~/img/Logo.jpg");
                logo = iTextSharp.text.Image.GetInstance(rutaLogo);
                logo.ScaleToFit(200, 100);
                logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                documento.Add(logo);
                documento.Add(new Paragraph(" "));
                Paragraph titulo = new Paragraph(nomEmpresa, standardFont);
                titulo.Alignment = Element.ALIGN_CENTER;
                documento.Add(new Paragraph(titulo));
            }
            catch (Exception) { }

            Paragraph encabezado = new Paragraph("Av. Huehuetoca s/n esq. Dr. Jiménez Cantú" + Environment.NewLine + 
                "Ex Hacienda San Miguel, Cuatitlán Izcalli" + Environment.NewLine +
                "Edo. de Méx. México, C.P. 54715" + Environment.NewLine +
                "Tel. (55) 1676-1723", standardFont1);
            encabezado.Alignment = Element.ALIGN_CENTER;
            documento.Add(encabezado);
            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));

            Paragraph numTicket = new Paragraph("Ticket # " + ticket, standardFont1);
            numTicket.Alignment = Element.ALIGN_CENTER;
            documento.Add(numTicket);

            string tdcTitular = "SELECT (nombre_tarjetahabiente +' '+ ap_pat_tarjetahabiente +' '+  ap_mat_tarjetahabiente) AS tdcNombre FROM clientes WHERE referencia=" + refCliente;
            if(tipo == "B")
                tdcTitular = "SELECT tarjeta_habiente FROM bajas_inmediatas WHERE referencia=" + refCliente;
            tdcTitular = ejecuta.scalarString(tdcTitular);
            fechas.tipoFormato = 4;
            Paragraph impFecha = new Paragraph("Fecha: " + fechas.obtieneFechaLocal() + Environment.NewLine + "Socio # " + refCliente + Environment.NewLine + "TarjetaHabiente: " + tdcTitular, standardFont1);
            impFecha.Alignment = Element.ALIGN_CENTER;
            documento.Add(impFecha);            
            Paragraph impAten = new Paragraph("Atendió: " + usuario, standardFont1);
            impAten.Alignment = Element.ALIGN_CENTER;
            documento.Add(impAten);            
            documento.Add(new Paragraph(" "));

            generaContenido(documento,cliente,empresa, monto, tipo);

            if (tipo == "A" || tipo == "M" || tipo=="R")
            {

                string strTot = (monto * cantidad).ToString();
                documento.Add(new Paragraph(" "));
                documento.Add(new Paragraph(" "));
                Paragraph ltotal = new Paragraph("Total: " + Convert.ToDecimal(strTot).ToString("C2"), standardFont);
                ltotal.Alignment = Element.ALIGN_CENTER;
                documento.Add(ltotal);
                documento.Add(new Paragraph(" "));
                documento.Add(new Paragraph(" "));

                //importe con letra
                Convertidores conversion = new Convertidores();
                conversion._importe = strTot;
                string textoLetras = conversion.convierteMontoEnLetras();
                Paragraph importeLetra = new Paragraph(textoLetras, standardFont);
                importeLetra.Alignment = Element.ALIGN_CENTER;
                documento.Add(importeLetra);
            }

            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            iTextSharp.text.Font fntPie = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Paragraph pie = new Paragraph("http://www.sportsclubmexico.com" + Environment.NewLine +
                "Siguenos en facebook.com/sportsclubmexico", fntPie);
            pie.Alignment = Element.ALIGN_CENTER;
            documento.Add(pie);


            documento.Close();
        }
        return archivo;
    }
    public string GenerarTicket2(int empresa, string usuario, string cliente, string tipo, decimal monto, int cantidad, string año, string mes, int ticket, string nomEmpresa, string refCliente, string periodo)
    {
        // Crear documento
        Document documento = new Document(iTextSharp.text.PageSize.B6, 0.5f, 0.5f, 0.5f, 0.5f);
        documento.AddTitle("Ticket de Servicio");
        documento.AddCreator("e-Bills");

        string ruta = HttpContext.Current.Server.MapPath("~/Tickets");
        string archivo = ruta + "\\Ticket_" + empresa.ToString() + "_" + ticket.ToString() + ".pdf";

        //si no existe la carpeta temporal la creamos 
        if (!(Directory.Exists(ruta)))
        {
            Directory.CreateDirectory(ruta);
        }

        if (archivo.Trim() != "")
        {
            FileStream file = new FileStream(archivo,
            FileMode.OpenOrCreate,
            FileAccess.ReadWrite,
            FileShare.ReadWrite);
            PdfWriter.GetInstance(documento, file);
            // Abrir documento.
            documento.Open();

            //Insertar logo o imagen            
            string rutaLogo = "";
            iTextSharp.text.Image logo = null;
            iTextSharp.text.Font standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            iTextSharp.text.Font standardFont1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
            try
            {
                documento.Add(new Paragraph(" "));
                documento.Add(new Paragraph(" "));
                rutaLogo = HttpContext.Current.Server.MapPath("~/img/Logo.jpg");
                logo = iTextSharp.text.Image.GetInstance(rutaLogo);
                logo.ScaleToFit(200, 100);
                logo.Alignment = iTextSharp.text.Image.ALIGN_CENTER;
                documento.Add(logo);
                documento.Add(new Paragraph(" "));
                Paragraph titulo = new Paragraph(nomEmpresa, standardFont);
                titulo.Alignment = Element.ALIGN_CENTER;
                documento.Add(new Paragraph(titulo));
            }
            catch (Exception) { }

            Paragraph encabezado = new Paragraph("Av. Huehuetoca s/n esq. Dr. Jiménez Cantú" + Environment.NewLine +
                "Ex Hacienda San Miguel, Cuatitlán Izcalli" + Environment.NewLine +
                "Edo. de Méx. México, C.P. 54715" + Environment.NewLine +
                "Tel. (55) 1676-1723", standardFont1);
            encabezado.Alignment = Element.ALIGN_CENTER;
            documento.Add(encabezado);
            documento.Add(new Paragraph(" "));

            Paragraph impFecha = new Paragraph("Fecha: " + fechas.obtieneFechaLocal() + Environment.NewLine + "Socio # " + refCliente + Environment.NewLine + "TarjetaHabiente: " , standardFont1);
            impFecha.Alignment = Element.ALIGN_CENTER;
            documento.Add(impFecha);
            string tdcTitular = "";
            string[] arg = refCliente.ToString().Split(new char[] { ',' });
            for (int J = 0; J < arg.Length; J++)
            {
                string Nombres = arg[J];
                tdcTitular = "SELECT (nombre_tarjetahabiente +' '+ ap_pat_tarjetahabiente +' '+  ap_mat_tarjetahabiente) AS tdcNombre FROM clientes WHERE referencia=" + Nombres;
            if (tipo == "B")
                tdcTitular = "SELECT tarjeta_habiente FROM bajas_inmediatas WHERE referencia=" + refCliente;
            tdcTitular = ejecuta.scalarString(tdcTitular);

                Paragraph nombre = new Paragraph(tdcTitular, standardFont1);
                nombre.Alignment = Element.ALIGN_CENTER;
                documento.Add(nombre);

            }

            fechas.tipoFormato = 4;
            
            Paragraph impAten = new Paragraph("Atendió: " + usuario, standardFont1);
            impAten.Alignment = Element.ALIGN_CENTER;
            documento.Add(impAten);
            documento.Add(new Paragraph(" "));

          //  generaContenido2(documento, cliente, empresa, monto, tipo);

            if (tipo == "A" || tipo == "M" || tipo == "R")
            {

                string strTot = (monto * cantidad).ToString();
                documento.Add(new Paragraph(" "));
                documento.Add(new Paragraph(" "));
                Paragraph ltotal = new Paragraph("Total: " + Convert.ToDecimal(strTot).ToString("C2"), standardFont);
                ltotal.Alignment = Element.ALIGN_CENTER;
                documento.Add(ltotal);
                documento.Add(new Paragraph(" "));
                documento.Add(new Paragraph(" "));

                //importe con letra
                Convertidores conversion = new Convertidores();
                conversion._importe = strTot;
                string textoLetras = conversion.convierteMontoEnLetras();
                Paragraph importeLetra = new Paragraph(textoLetras, standardFont);
                importeLetra.Alignment = Element.ALIGN_CENTER;
                documento.Add(importeLetra);
            }

            documento.Add(new Paragraph(" "));
            documento.Add(new Paragraph(" "));
            iTextSharp.text.Font fntPie = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);
            Paragraph pie = new Paragraph("http://www.sportsclubmexico.com" + Environment.NewLine +
                "Siguenos en facebook.com/sportsclubmexico", fntPie);
            pie.Alignment = Element.ALIGN_CENTER;
            documento.Add(pie);


            documento.Close();
        }
        return archivo;
    }

    private void generaContenido(Document documento, int cliente, int empresa, decimal pUnitario, string tipo)
    {
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        if (tipo == "A" || tipo == "M" || tipo=="R")
        {
            PdfPTable tblProductos = new PdfPTable(3);
            
            PdfPCell cld2 = new PdfPCell(new Phrase("DESCRIPCION", fuente1));
            cld2.BorderWidthBottom = 1;
            cld2.HorizontalAlignment = 1;
            cld2.VerticalAlignment = 1;
            cld2.Padding = 1;

            PdfPCell cld3 = new PdfPCell(new Phrase("P. U.", fuente1));
            cld3.BorderWidthBottom = 1;
            cld3.HorizontalAlignment = 1;
            cld3.VerticalAlignment = 1;
            cld3.Padding = 1;

            PdfPCell cld4 = new PdfPCell(new Phrase("IMPORTE", fuente1));
            cld4.BorderWidthBottom = 1;
            cld4.HorizontalAlignment = 1;
            cld4.VerticalAlignment = 1;
            cld4.Padding = 1;
                        
            tblProductos.AddCell(cld2);
            tblProductos.AddCell(cld3);
            tblProductos.AddCell(cld4);

            int tamañodatos = 0;
            DataTable dt = new DataTable();
            string strSql = "SELECT TOP 1 ticket, descripcion, monto FROM ticket WHERE id_empresa=" + empresa + " AND id_cliente=" + cliente + " Order by ticket desc";
            dt = ejecuta.obtieneInformacionDt(strSql);
            foreach (DataRow fila in dt.Rows)
            {
                try
                {                    
                    PdfPCell desc = new PdfPCell(new Phrase(fila[1].ToString().ToUpper(), fuente2));
                    PdfPCell prec = new PdfPCell(new Phrase(pUnitario.ToString("C2"), fuente2));
                    PdfPCell tot = new PdfPCell(new Phrase(Convert.ToDecimal(fila[2].ToString()).ToString("C2"), fuente2));
                    
                    desc.BorderWidth = 0;
                    desc.HorizontalAlignment = 1;
                    desc.VerticalAlignment = 1;
                    desc.Padding = 1;
                    prec.BorderWidth = 0;
                    prec.HorizontalAlignment = 1;
                    prec.VerticalAlignment = 1;
                    prec.Padding = 1;
                    tot.BorderWidth = 0;
                    tot.HorizontalAlignment = 1;
                    tot.VerticalAlignment = 1;
                    tot.Padding = 1;
                    
                    tblProductos.AddCell(desc);
                    tblProductos.AddCell(prec);
                    tblProductos.AddCell(tot);
                    tamañodatos++;
                }
                catch (Exception ex)
                { string err = ex.Message; }
            }
            
            documento.Add(tblProductos);
        }
        else {
            PdfPTable tblProductos = new PdfPTable(2);

            PdfPCell cld2 = new PdfPCell(new Phrase("DESCRIPCION", fuente1));
            cld2.BorderWidthBottom = 1;
            cld2.HorizontalAlignment = 1;
            cld2.VerticalAlignment = 1;
            cld2.Padding = 1;

            PdfPCell cld4 = new PdfPCell(new Phrase("IMPORTE", fuente1));
            cld4.BorderWidthBottom = 1;
            cld4.HorizontalAlignment = 1;
            cld4.VerticalAlignment = 1;
            cld4.Padding = 1;

            tblProductos.AddCell(cld2);            
            tblProductos.AddCell(cld4);

            int tamañodatos = 0;
            DataTable dt = new DataTable();
            string strSql = "SELECT TOP 1 ticket, descripcion, monto FROM ticket WHERE id_empresa=" + empresa + " AND id_cliente=" + cliente + " Order by ticket desc";
            dt = ejecuta.obtieneInformacionDt(strSql);
            foreach (DataRow fila in dt.Rows)
            {
                try
                {
                    PdfPCell desc = new PdfPCell(new Phrase(fila[1].ToString().ToUpper(), fuente2));                    
                    PdfPCell tot = new PdfPCell(new Phrase(Convert.ToDecimal(fila[2].ToString()).ToString("C2"), fuente2));

                    desc.BorderWidth = 0;
                    desc.HorizontalAlignment = 1;
                    desc.VerticalAlignment = 1;
                    desc.Padding = 1;                   
                    tot.BorderWidth = 0;
                    tot.HorizontalAlignment = 1;
                    tot.VerticalAlignment = 1;
                    tot.Padding = 1;

                    tblProductos.AddCell(desc);                    
                    tblProductos.AddCell(tot);
                    tamañodatos++;
                }
                catch (Exception ex)
                { string err = ex.Message; }
            }            
            documento.Add(tblProductos);

        }
            
        if(tipo == "A" || tipo == "M" || tipo=="R")
        {
            documento.Add(new Paragraph(" "));
            Paragraph proxPago = new Paragraph("Recuerde que su próximo pago es en: " + obtenerNombreMes(fechas.obtieneFechaLocal().AddMonths(1).Month) + " " + fechas.obtieneFechaLocal().AddMonths(1).Year.ToString(), fuente1);
            proxPago.Alignment = Element.ALIGN_CENTER;
            documento.Add(proxPago);
        }
    }
    private void generaContenido2(Document documento, string cliente, int empresa, decimal pUnitario, string tipo)
    {
        iTextSharp.text.Font fuente1 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.BOLD, BaseColor.BLACK);
        iTextSharp.text.Font fuente2 = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 9, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

        if (tipo == "A" || tipo == "M" || tipo == "R")
        {
            PdfPTable tblProductos = new PdfPTable(3);

            PdfPCell cld2 = new PdfPCell(new Phrase("DESCRIPCION", fuente1));
            cld2.BorderWidthBottom = 1;
            cld2.HorizontalAlignment = 1;
            cld2.VerticalAlignment = 1;
            cld2.Padding = 1;

            PdfPCell cld3 = new PdfPCell(new Phrase("P. U.", fuente1));
            cld3.BorderWidthBottom = 1;
            cld3.HorizontalAlignment = 1;
            cld3.VerticalAlignment = 1;
            cld3.Padding = 1;

            PdfPCell cld4 = new PdfPCell(new Phrase("IMPORTE", fuente1));
            cld4.BorderWidthBottom = 1;
            cld4.HorizontalAlignment = 1;
            cld4.VerticalAlignment = 1;
            cld4.Padding = 1;

            tblProductos.AddCell(cld2);
            tblProductos.AddCell(cld3);
            tblProductos.AddCell(cld4);

            int tamañodatos = 0;
            DataTable dt = new DataTable();
            string strSql = "SELECT TOP 1 ticket, descripcion, monto FROM ticket WHERE id_empresa=" + empresa + " AND id_cliente=" + cliente + " Order by ticket desc";
            dt = ejecuta.obtieneInformacionDt(strSql);
            foreach (DataRow fila in dt.Rows)
            {
                try
                {
                    PdfPCell desc = new PdfPCell(new Phrase(fila[1].ToString().ToUpper(), fuente2));
                    PdfPCell prec = new PdfPCell(new Phrase(pUnitario.ToString("C2"), fuente2));
                    PdfPCell tot = new PdfPCell(new Phrase(Convert.ToDecimal(fila[2].ToString()).ToString("C2"), fuente2));

                    desc.BorderWidth = 0;
                    desc.HorizontalAlignment = 1;
                    desc.VerticalAlignment = 1;
                    desc.Padding = 1;
                    prec.BorderWidth = 0;
                    prec.HorizontalAlignment = 1;
                    prec.VerticalAlignment = 1;
                    prec.Padding = 1;
                    tot.BorderWidth = 0;
                    tot.HorizontalAlignment = 1;
                    tot.VerticalAlignment = 1;
                    tot.Padding = 1;

                    tblProductos.AddCell(desc);
                    tblProductos.AddCell(prec);
                    tblProductos.AddCell(tot);
                    tamañodatos++;
                }
                catch (Exception ex)
                { string err = ex.Message; }
            }

            documento.Add(tblProductos);
        }
        else {
            PdfPTable tblProductos = new PdfPTable(2);

            PdfPCell cld2 = new PdfPCell(new Phrase("DESCRIPCION", fuente1));
            cld2.BorderWidthBottom = 1;
            cld2.HorizontalAlignment = 1;
            cld2.VerticalAlignment = 1;
            cld2.Padding = 1;

            PdfPCell cld4 = new PdfPCell(new Phrase("IMPORTE", fuente1));
            cld4.BorderWidthBottom = 1;
            cld4.HorizontalAlignment = 1;
            cld4.VerticalAlignment = 1;
            cld4.Padding = 1;

            tblProductos.AddCell(cld2);
            tblProductos.AddCell(cld4);

            int tamañodatos = 0;
            DataTable dt = new DataTable();
            string strSql = "SELECT TOP 1 ticket, descripcion, monto FROM ticket WHERE id_empresa=" + empresa + " AND id_cliente=" + cliente + " Order by ticket desc";
            dt = ejecuta.obtieneInformacionDt(strSql);
            foreach (DataRow fila in dt.Rows)
            {
                try
                {
                    PdfPCell desc = new PdfPCell(new Phrase(fila[1].ToString().ToUpper(), fuente2));
                    PdfPCell tot = new PdfPCell(new Phrase(Convert.ToDecimal(fila[2].ToString()).ToString("C2"), fuente2));

                    desc.BorderWidth = 0;
                    desc.HorizontalAlignment = 1;
                    desc.VerticalAlignment = 1;
                    desc.Padding = 1;
                    tot.BorderWidth = 0;
                    tot.HorizontalAlignment = 1;
                    tot.VerticalAlignment = 1;
                    tot.Padding = 1;

                    tblProductos.AddCell(desc);
                    tblProductos.AddCell(tot);
                    tamañodatos++;
                }
                catch (Exception ex)
                { string err = ex.Message; }
            }
            documento.Add(tblProductos);

        }

        if (tipo == "A" || tipo == "M" || tipo == "R")
        {
            documento.Add(new Paragraph(" "));
            Paragraph proxPago = new Paragraph("Recuerde que su próximo pago es en: " + obtenerNombreMes(fechas.obtieneFechaLocal().AddMonths(1).Month) + " " + fechas.obtieneFechaLocal().AddMonths(1).Year.ToString(), fuente1);
            proxPago.Alignment = Element.ALIGN_CENTER;
            documento.Add(proxPago);
        }
    }

    public int obtieneTicketSiguiente(int empresa, int cliente)
    {
        string sql = "select isnull((select top 1 ticket from ticket_enc where id_empresa=" + empresa + " and id_cliente=" + cliente + " order by ticket desc),0)+1";
        return ejecuta.scalarInt(sql);
    }

    public bool generarTicketBD(int empresa, string usuario, int cliente, string tipo, decimal monto, string Periodo, string detalle)
    {        
        string servicio = "";
        switch (tipo)
        {
            case "A":
                servicio = "Pago por primer mensualidad";
                break;
            case "I":
                servicio = "Comprobante de Baja Temporal por el siguiente motivo: " + detalle;
                break;
            case "B":
                servicio = "Comprobante de Baja por el siguiente motivo: " + detalle;
                break;
            case "M":
                servicio = "Pago correspondiente a el o los periodos:\n\r" + Periodo;
                break;
            case "R":
                servicio = "Pago de mensualidad por reactivacion";
                break;
            default:
                servicio = "";
                break;
        }            
        string sql = "declare @ticket int " +
                     "declare @i int " +                     
                     "set @i = 0 " +
                     "set @ticket = (select isnull((select top 1 ticket from ticket where id_empresa = " + empresa + " order by ticket desc),0)+1) " +
                     "insert into ticket(ticket,id_empresa,id_cliente,id_usuario,tipo,fecha,hora,monto,descripcion,periodo_proximo_pago) " +
                     "values(@ticket," + empresa + "," + cliente + ",'" + usuario + "','" + tipo + "','" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "'," + monto.ToString() + ",'" + servicio + "','" + fechas.obtieneFechaLocal().AddMonths(1).ToString("yyyy-MM-dd") + "') ";
                     
        return ejecuta.update_insert_delete(sql);
    }
    public bool generarTicketBD2(int empresa, string usuario, string cliente, string tipo, decimal monto, string Periodo, string detalle)
    {
        string servicio = "";
        switch (tipo)
        {
            case "A":
                servicio = "Pago por primer mensualidad";
                break;
            case "I":
                servicio = "Comprobante de Baja Temporal por el siguiente motivo: " + detalle;
                break;
            case "B":
                servicio = "Comprobante de Baja por el siguiente motivo: " + detalle;
                break;
            case "M":
                servicio = "Pago correspondiente a el o los periodos:\n\r" + Periodo;
                break;
            case "R":
                servicio = "Pago de mensualidad por reactivacion";
                break;
            default:
                servicio = "";
                break;
        }
        string sql = "declare @ticket int " +
                     "declare @i int " +
                     "set @i = 0 " +
                     "set @ticket = (select isnull((select top 1 ticket from ticket where id_empresa = " + empresa + " order by ticket desc),0)+1) " +
                     "insert into ticket(ticket,id_empresa,id_cliente,id_usuario,tipo,fecha,hora,monto,descripcion,periodo_proximo_pago) " +
                     "values(@ticket," + empresa + ",'" + cliente + "','" + usuario + "','" + tipo + "','" + fechas.obtieneFechaLocal().ToString("yyyy-MM-dd") + "','" + fechas.obtieneFechaLocal().ToString("HH:mm:ss") + "'," + monto.ToString() + ",'" + servicio + "','" + fechas.obtieneFechaLocal().AddMonths(1).ToString("yyyy-MM-dd") + "') ";

        return ejecuta.update_insert_delete(sql);
    }

    public int obtieneTicket(int empresa, int cliente)
    {
        string sql = "select top 1 ticket from ticket where id_empresa = " + empresa + " and id_cliente = " + cliente + " order by ticket desc";
        return ejecuta.scalarInt(sql);
    }
    public int obtieneTicket2(int empresa, string cliente)
    {
        string sql = "select top 1 ticket from ticket where id_empresa = " + empresa + " and id_cliente = " + cliente + " order by ticket desc";
        return ejecuta.scalarInt(sql);
    }

    public static string obtenerNombreMes(int numeroMes)
    {
        try
        {
            DateTimeFormatInfo formatoFecha = CultureInfo.CurrentCulture.DateTimeFormat;
            string nombreMes = formatoFecha.GetMonthName(numeroMes);
            return nombreMes;
        }
        catch
        {
            return "Desconocido";
        }
    }
}
