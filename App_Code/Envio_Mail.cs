using System;
using System.Collections.Generic;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

/// <summary>
/// Descripción breve de Envio_Mail
/// </summary>
public class Envio_Mail
{
    Ejecucion ejecuta = new Ejecucion();
    public Envio_Mail()
    {
        //
        // TODO: Agregar aquí la lógica del constructor
        //
    }
    public bool obtieneDatosServidor(string correo, string mensaje, string contraseña, string asunto, ListBox adjuntos, int empresa, string CC, string CCO)
    {
        bool enviado = false;
        SqlConnection conexion = new SqlConnection();
        conexion.ConnectionString = ConfigurationManager.ConnectionStrings["eBills"].ToString();
        string sql = "select usuario+contrasena+host+cast(puerto as char(10)) from empresas where id_empresa=" + empresa;
        
            string parametros = ejecuta.scalarString(sql);
        if (parametros != "")
        {
            sql = "select isnull(usuario,''),isnull(contrasena,''),isnull(host,''),isnull(puerto,0),isnull(ssl_habilitado,0),isnull(mensajeCorreo,'') from empresas where id_empresa=" + empresa;
            try
            {
                conexion.Open();
                SqlCommand cmd = new SqlCommand(sql, conexion);
                SqlDataReader lectura = cmd.ExecuteReader();
                string usuariohost = "", contrasena = "", host = "";
                int puerto = 0;
                int ssl = 0;
                object[] param = new object[6];
                while (lectura.Read())
                {
                    param[0] = lectura.GetValue(0);
                    param[1] = lectura.GetValue(1);
                    param[2] = lectura.GetValue(2);
                    param[3] = lectura.GetValue(3);
                    param[4] = lectura.GetValue(4);
                    param[5] = lectura.GetValue(5);
                }
                usuariohost = param[0].ToString();
                contrasena = param[1].ToString();
                host = param[2].ToString();
                puerto = Convert.ToInt32(param[3]);
                ssl = Convert.ToInt32(param[4]);
                if (mensaje == "")
                    mensaje = param[5].ToString();
               
                enviado = EnviarCorreo(correo, usuariohost, contrasena, puerto, ssl, host, mensaje, contraseña, asunto, adjuntos, CC, CCO);
            }
            catch (Exception x)
            {
                enviado = false;
            }
        }
        else
            enviado = false;
        
        conexion.Dispose();
        conexion.Close();
        return enviado;
    }

    private bool EnviarCorreo(string correoEnviar, string usuariohost, string contrasena, int puerto, int ssl, string host, string mensaje, string contraseña, string asunto, ListBox adjuntos, string CC, string CCO)
    {
        bool envio = false;
        /*-------------------------MENSAJE DE CORREO----------------------*/
        //Creamos un nuevo Objeto de mensaje
        System.Net.Mail.MailMessage mmsg = new System.Net.Mail.MailMessage();
        //Direccion de correo electronico a la que queremos enviar el mensaje
        mmsg.To.Add(correoEnviar);
        //Nota: La propiedad To es una colección que permite enviar el mensaje a más de un destinatario
        //Asunto
        
        mmsg.Subject = asunto;// "Asunto del correo";
        mmsg.SubjectEncoding = System.Text.Encoding.UTF8;
        if (CC != "")
            mmsg.CC.Add(CC);
        if (CCO != "")
            mmsg.Bcc.Add(CCO);
        //Cuerpo del Mensaje
        mmsg.Body = mensaje;//Texto del contenio del mensaje de correo
        if (adjuntos != null)
        {
            //Adjuntos
            foreach (ListItem l in adjuntos.Items)
            {
                mmsg.Attachments.Add(new System.Net.Mail.Attachment(l.Value));
            }
        }


        mmsg.BodyEncoding = System.Text.Encoding.UTF8;
        mmsg.IsBodyHtml = true; //Si no queremos que se envíe como HTML
        //Correo electronico desde la que enviamos el mensaje
        mmsg.From = new System.Net.Mail.MailAddress(usuariohost);//"juan@formulasistemas.com");//"micuenta@servidordominio.com");
        /*-------------------------CLIENTE DE CORREO----------------------*/
        //Creamos un objeto de cliente de correo
        System.Net.Mail.SmtpClient cliente = new System.Net.Mail.SmtpClient();
        //Hay que crear las credenciales del correo emisor
        cliente.Credentials = new System.Net.NetworkCredential(usuariohost, contrasena);//"juan@formulasistemas.com", "juanFS2014");//"micuenta@servidordominio.com", "micontraseña");
        //Lo siguiente es obligatorio si enviamos el mensaje desde Gmail
        ///*
        cliente.Port = puerto;
        bool ssl_obtenido = false;
        if (ssl == 1)
            ssl_obtenido = true;
        cliente.EnableSsl = ssl_obtenido;
        //*/

        cliente.Host = host; //"mail.formulasistemas.com";// "mail.servidordominio.com"; //Para Gmail "smtp.gmail.com";


        /*-------------------------ENVIO DE CORREO----------------------*/

        try
        {
            //Enviamos el mensaje      
            cliente.Send(mmsg);
            envio = true;
        }
        catch (System.Net.Mail.SmtpException ex)
        {
            envio = false;//Aquí gestionamos los errores al intentar enviar el correo
        }
        return envio;
    }
}