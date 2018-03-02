using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;


public partial class Usuarios_afiliacion : System.Web.UI.Page
{
    Datos datos = new Datos();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            cargadatos();
        }
    }

    public void cargadatos()
    {
        Datos conectar = new Datos();
        DataSet datos = new DataSet();
        datos = conectar.cargaUsuariosafi();
        GridView1.DataSource = datos;
        GridView1.DataBind();

    }
    protected void lknEliminar_Click(object sender, EventArgs e)
    {
        ImageButton lknEliminar = (ImageButton)sender;
        char[] separador = { ';' };
        string[] valores = lknEliminar.CommandArgument.ToString().Split(separador);
        string claveusuario = valores[0];
        int afi;
        try { afi = Convert.ToInt32(valores[1].ToString()); }
        catch (Exception) { afi = 0; }
        bool borrado = false;
        
            borrado = datos.borraUsuarioafi(claveusuario, afi);
            if (borrado)
                cargadatos();
            else
            {
                lblError.Text = "No se pudo borrar el usuario verifique su conexión.";
                cargadatos();
            }
        
    }
    protected void btnagregar_Click(object sender, ImageClickEventArgs e)
    {
        if (ddlUsuario.SelectedValue == "0" && ddlAfi.SelectedValue == "0")
        {
            lblError.Text = "Seleccione el usuario y afiliación";
        }

        else if (ddlUsuario.SelectedValue == "0")
        {
            lblError.Text = "Seleccione usuario";
        }

        else if (ddlAfi.SelectedValue == "0")
        {
            lblError.Text = "Seleccione afiliación";
        }
        else
        {
            string id_usuario = ddlUsuario.SelectedValue;
            string usuario;
            try
            {
                usuario = Convert.ToString(id_usuario);
            }
            catch
            {
                usuario = "0";
            }

            string id_afi = ddlAfi.SelectedValue;
            int afi;
            try
            {
                afi = Convert.ToInt32(id_afi);
            }
            catch
            {
                afi = 0;
            }
            bool existe = false;
            bool existeu = false;
            existe = datos.existeAfiu(usuario, afi);
            existeu = datos.existeUsaafi(usuario);
            if (!existe && !existeu)
            {
                bool agregado = false;
                agregado = datos.agregaUsuarioafi(usuario.ToString(), afi);
                if (agregado)
                {
                    ddlUsuario.SelectedValue = "0";
                    ddlAfi.SelectedValue = "0";
                    cargadatos();
                    lblError.Text = "";
                }
                else
                {
                    lblError.Text = "No se pudo agregar el usuario verifique su conexión.";
                    cargadatos();
                }

            }
            else
            {
                lblError.Text = "El usuario ya cuenta con esa afiliación y/o el usuario ya cuenta con una afiliación";
            }
        }
    }
}

