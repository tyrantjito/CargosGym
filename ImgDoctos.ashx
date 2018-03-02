<%@ WebHandler Language="C#" Class="ImgDoctos" %>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class ImgDoctos : IHttpHandler {

    private string extensiones;
    public void ProcessRequest(HttpContext context)
    {
        context.Response.Clear();
        if (!String.IsNullOrEmpty(context.Request.QueryString["id"]))
        {
            string[] datos = context.Request.QueryString["id"].ToString().Split(new char[] { ';' });
            int empresa = Convert.ToInt32(datos[2]);
            string referencia = datos[1];
            int id = Convert.ToInt32(datos[0]);
            int idCliente = Convert.ToInt32(datos[3]);
            Image imagen = GetImagen(empresa, referencia, id, idCliente);

            string contexto = "image/jpeg";
            ImageFormat formato = ImageFormat.Jpeg;

            switch (extensiones) { 
                case "png":
                    contexto = "image/png";
                    formato = ImageFormat.Png;
                    break;                
                case "gif":
                    contexto = "image/gif";
                    formato = ImageFormat.Gif;
                    break;
                default:
                    contexto = "image/jpeg";
                    formato = ImageFormat.Jpeg;
                    break;
            }

            context.Response.ContentType = contexto;
            if (imagen != null)
                imagen.Save(context.Response.OutputStream, formato);            
        }
        else
        {
            context.Response.ContentType = "text/html";
            context.Response.Write("&nbsp;");
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private Image GetImagen(int empresa, string referencia, int id, int idCliente)
    {
        Image logo = null;
        MemoryStream memoryStream = new MemoryStream();
        SqlConnection conexion = new SqlConnection();
        string extension = "jpg";
        conexion.ConnectionString = ConfigurationManager.ConnectionStrings["eBills"].ToString();
        string sql = "select documento,extension from documentos where id_empresa=" + empresa.ToString() + " and referencia='" + referencia + "' and id_documento=" + id.ToString() + " and id_cliente=" + idCliente.ToString();
        try
        {
            conexion.Open();
            SqlCommand cmd = new SqlCommand(sql, conexion);
            SqlDataReader lectura = cmd.ExecuteReader();
            if (lectura.HasRows)
            {
                lectura.Read();
                byte[] imagenPerfil = (byte[])lectura[0];
                memoryStream = new MemoryStream(imagenPerfil, false);
                extension = lectura[1].ToString();
                extensiones = extension;
            }
        }
        catch (Exception)
        {
            extension = "jpg";
            extensiones = "jpg";
        }
        finally
        {
            conexion.Dispose();
            conexion.Close();
        }

        if (extension != "pdf")
        {
            try
            {
                logo = Image.FromStream(memoryStream);
                extensiones = extension;
            }
            catch (Exception)
            {
                logo = Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/IMG/pdflogo.jpg"));
                extensiones = "jpg";
            }
        }
        else
        {
            logo = Image.FromFile(System.Web.HttpContext.Current.Server.MapPath("~/IMG/pdflogo.jpg"));
            extensiones = "pdf";
        }
        
        return logo;
    }

}