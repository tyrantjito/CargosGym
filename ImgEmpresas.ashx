<%@ WebHandler Language="C#" Class="ImgEmpresas" %>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class ImgEmpresas : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.Clear();
        if (!String.IsNullOrEmpty(context.Request.QueryString["id"]))
        {
            string usuario = context.Request.QueryString["id"];
            int empresa = Convert.ToInt32(usuario);
            Image imagen = GetImagen(empresa);
            context.Response.ContentType = "image/jpeg";
            if (imagen != null)
                imagen.Save(context.Response.OutputStream, ImageFormat.Jpeg);
        }
        else
        {
            context.Response.ContentType = "text/html";
            context.Response.Write("&nbsp;");
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private Image GetImagen(int empresa)
    {
        Image logo = null;
        MemoryStream memoryStream = new MemoryStream();        
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = ConfigurationManager.ConnectionStrings["eBills"].ToString();
        string sql = "select imagen_logo from Empresas where id_empresa=" + empresa.ToString();
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
            }
        }
        catch (Exception)
        {

        }
        finally
        {
            conexion.Dispose();
            conexion.Close();
        }
        try
        {
            logo = Image.FromStream(memoryStream);
        }
        catch (Exception)
        {
            logo = null;
        }   
        return logo;
    }
}