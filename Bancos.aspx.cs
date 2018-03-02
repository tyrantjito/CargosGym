using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Text;

public partial class Bancos : System.Web.UI.Page
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

        if (!Page.IsPostBack)
        {
            cargadatos();
            lblIdBanco.Text = "";
        }
    }

    public void cargadatos()
    {
        Datos conectar = new Datos();
        DataSet datos = new DataSet();
        datos = conectar.cargaBancos();
        GridView1.DataSource = datos;
        GridView1.DataBind();
        string id_empresa = "0";
        if (Session["e"] == null || (Session["e"].ToString() == "0" && Session["u"].ToString() != "Supervisor"))
            Response.Redirect("Default.aspx");
        else
            id_empresa = Session["e"].ToString();
    }

    

    protected void lknEliminar_Click(object sender, EventArgs e)
    {
        ImageButton lknEliminar = (ImageButton)sender;
        string idBanco = lknEliminar.CommandArgument;
        bool borrado = false;
        int existe = 1;
        existe = datos.existeRelacionBancos(idBanco);
        if (existe == 0)
        {
            borrado = datos.borraBanco(idBanco);
            if (borrado)
                cargadatos();
            else
            {
                lblError.Text = "No se pudo borrar el usuario verifique su conexión.";
                cargadatos();
            }
        }
        else
            lblError.Text = "No es posible eliminar el banco ya que esta siendo usado en otro(s) proceso(s)";
    }

    protected void btnAgregar_Click(object sender, ImageClickEventArgs e)
    {
        string clave = txtClave.Text;
        string nombre = txtNombre.Text;

        bool agregado = false;
        agregado = datos.agregaBanco(nombre, clave);
        if (agregado)
        {
            txtClave.Text = "";
            txtNombre.Text = "";
            cargadatos();
        }
        else
        {
            lblError.Text = "No se pudo agregar el banco verifique su conexión.";
            cargadatos();
        }
    }

    

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Modificar")
        {
            lblErrorMod.Text = "";
            string[] nomClav = e.CommandArgument.ToString().Split(new char[] { ';' });
            lblIdBanco.Text = nomClav[0].TrimEnd();
            txtClaveMod.Text = nomClav[2].TrimEnd();
            txtNombreMod.Text = nomClav[1].TrimEnd();
            PanPopUpDiv.Visible = true;
            PanPopUp.Visible = true;
        }
    }

    protected void btnActualizarMod_Click(object sender, ImageClickEventArgs e)
    {
        string nombre = "";
        string clave = "";
        try
        {
            nombre = txtNombreMod.Text;
            clave = txtClaveMod.Text;
        }
        catch (Exception) { lblErrorMod.Text = "Necesita llenar todos los campos"; }
        int id_banco = 0; 
        try { id_banco = Convert.ToInt32(lblIdBanco.Text.Trim().ToString()); }
        catch (Exception) { id_banco = 0; }
        bool actualizado = datos.actualizaBanco(id_banco, nombre, clave);
        if (!actualizado)
            lblErrorMod.Text = "Hubo un problema en la actualización verifique su conexion e intentelo nuevamente";
        else
        {
            lblErrorMod.Text = "Actualización exitosa";
            lblIdBanco.Text = "";
        }
        cargadatos();
        PanPopUpDiv.Visible = false;
        PanPopUp.Visible = false;
    }

    protected void btnCancelarMod_Click(object sender, ImageClickEventArgs e)
    {
        PanPopUpDiv.Visible = false;
        PanPopUp.Visible = false;
    }
}
