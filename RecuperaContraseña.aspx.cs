using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class RecuperaContraseña : System.Web.UI.Page
{
    Datos datos = new Datos();
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnConfirmar_Click(object sender, ImageClickEventArgs e)
    {
        if (txtUsuario.Text != "")
        {
            string usuario = txtUsuario.Text;
            string correo = txtCorreo.Text;
            bool existe = datos.existeUsuarioContraseña(usuario);
            if (existe)
            {
                bool existeCorreo = datos.existeCorreo(usuario, correo);
                if (existeCorreo)
                {
                    string idEmpresa = "";
                    try
                    {
                        idEmpresa = Request.QueryString["e"].ToString();
                    }
                    catch (Exception) { Response.Redirect("Default.aspx"); }
                    Envio_Mail enviaMeail = new Envio_Mail();
                    bool enviado = enviaMeail.obtieneDatosServidor(correo.ToLower().Trim(), "<p><strong>Recuperación de Contraseña SportsClub Cargos:</strong><br/><br/>Usuario:<strong>"+usuario.ToUpper()+ "</strong><br/><br/>Contraseña:<strong>" + datos.obtieneContraseña(usuario).ToUpper() + "</strong></p>", "", "Recuperacion de Contraseña Sistema Cargos SportsClub", null, Convert.ToInt32(idEmpresa), "", "sportsclubmexico@outlook.com");
                    if (enviado)
                    {
                        lblErrorRecContraseña.Text = "";
                        btnConfirmar.Visible = false;
                        btnRegresar.Visible = true;
                        lblContraseña.Text = "Su Contraseña ha sido enviado vía correo.";
                    }
                    else {                        
                        btnConfirmar.Visible = true;
                        btnRegresar.Visible = true;
                        lblErrorRecContraseña.Text = "Se produjo un error al enviar por correo su contraseña, por favor contacte al administrador del sistema para más información";
                    }
                }
                else {
                    lblErrorRecContraseña.Text = "El correo indicado no coincide con el que se registrado en el sistema, debe ingresar el correo correcto.";
                    btnConfirmar.Visible = true;
                    btnRegresar.Visible = true;                    
                    lblContraseña.Text = "";
                }
            }
            else
            {
                btnConfirmar.Visible = true;
                btnRegresar.Visible = true;
                lblErrorRecContraseña.Text = "El Usuario no existe favor de verificar.";
                lblContraseña.Text = "";
            }
        }
        else
        {
            btnConfirmar.Visible = true;
            btnRegresar.Visible = true;
            lblErrorRecContraseña.Text = "Necesita ingresar el Usuario";
        }
    }
}