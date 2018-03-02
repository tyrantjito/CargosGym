using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CambiarContraseña : System.Web.UI.Page
{
    Datos datos = new Datos();
    protected void Page_Load(object sender, EventArgs e)
    {
        int empresa = Convert.ToInt32(Session["e"]);
        if (empresa == 0)
        {
            Response.Redirect("Default.aspx");

        }
        else {

            string usuario = Session["u"].ToString();
            string nombre = Session["nu"].ToString();
            int cont = Convert.ToInt32(Session["C"]);
            string aspx = Session["aspx"].ToString();
        }

        cargaDatso();
    }

    private void cargaDatso()
    {
        string usuario="";
        try { usuario = Session["u"].ToString(); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        lblContraseña.Text = datos.obtieneContraseña(usuario);
    }
    protected void btnConfirmar_Click(object sender, ImageClickEventArgs e)
    {
        if (txtContraseña.Text != "".Trim())
        {
            lblErrorContraseña.Text = "";
            string contraseña = txtContraseña.Text;
            string usuario = "";
            try { usuario = Session["u"].ToString(); }
            catch (Exception) { Response.Redirect("Default.aspx"); }
            bool actualizado = datos.actualizaContraseña(usuario, contraseña);
            if (actualizado)
            {
                lblErrorContraseña.Text = "Contraseña exitosamente modificada";
                cargaDatso();
            }
            else
            {
                lblErrorContraseña.Text = "Hubo un error al cambiar la contraseña intentelo nuevamente ";
                cargaDatso();
            }
        }
    }
}