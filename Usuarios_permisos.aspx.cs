using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class Usuarios_permisos : System.Web.UI.Page
{
    Datos conectar = new Datos();
    string usuariolog;
    bool[] permisos = { false, false, false, false, false, false, false, false, false, false, false, false, false };
    protected void Page_Load(object sender, EventArgs e)
    {

        int empresa = Convert.ToInt32(Session["e"]);
        if (empresa == 0)
        {
            Response.Redirect("Login.aspx");

        }
        else {

            string usuario = Session["u"].ToString();
            string nombre = Session["nu"].ToString();
            int cont = Convert.ToInt32(Session["C"]);
            string aspx = Session["aspx"].ToString();
        }
        if (!IsPostBack)
        {
            llenadatos();
            cargadatos();            
        }
    }

    public void llenadatos()
    {
        Datos conectar = new Datos();
        try
        {
            usuariolog = Convert.ToString(Session["u"]);
        }
        catch (Exception)
        {
            Response.Redirect("Default.aspx");
        }
        bool llenado = conectar.llenaPermisos(usuariolog,ddlUsuario.SelectedValue);
        if (!llenado)
        {            
            lblError.Text = "No se cargaron los datos";
        }
    }

    public void cargadatos()
    {
        Datos conectar = new Datos();
        try
        {
            usuariolog = Convert.ToString(Session["u"]);
        }
        catch (Exception) {
            Response.Redirect("Default.aspx");
        }
        DataSet datos = new DataSet();        

        if (ddlUsuario.SelectedValue != "0")
        {
            datos = conectar.cargaPermisos(usuariolog);
            ListBox1.DataSource = datos;
            ListBox1.DataBind();

            datos = conectar.cargaUPermisos(ddlUsuario.SelectedValue, usuariolog);
            ListBox2.DataSource = datos;
            ListBox2.DataBind();
        }
        else
        {
            ListBox1.Items.Clear();
            ListBox2.Items.Clear();
        }
    }
    protected void btnBorrartodos_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        try { usuariolog = Session["u"].ToString(); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        string usuario= Convert.ToString(ddlUsuario.SelectedValue.ToString());
        Datos datos = new Datos();
        bool borrado = datos.borraPermisos(usuario,usuariolog);
        if (borrado)
        {
            cargadatos();
        }
        else
        {
            lblError.Text = "No se han quitado los permisos, revise su conexión";
            cargadatos();
        }

    }

    protected void btnBorrar_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        try { usuariolog = Session["u"].ToString(); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        string usuario = Convert.ToString(ddlUsuario.SelectedValue.ToString());
        int permiso = Convert.ToInt32(ListBox2.SelectedValue);
        Datos datos = new Datos();
        bool borrado = datos.borraPermiso(usuario, usuariolog, permiso);
        if (borrado)
        {
            cargadatos();
        }
        else
        {
            lblError.Text = "No se han quitado los permisos, revise su conexión";
            cargadatos();
        }

    }


    protected void btnAgregar_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        if (ddlUsuario.SelectedValue == "0")
        {
            lblError.Text = "Seleccione usuario";
        }
        else
        {
            Datos datos = new Datos();
            string usuarioLog = "";
            try { usuarioLog = Session["u"].ToString(); }
            catch (Exception) { Response.Redirect("Default.aspx"); }
            if (usuariolog != "")
            {
                string usuario = Convert.ToString(ddlUsuario.SelectedValue.ToString());
                int permiso = Convert.ToInt32(ListBox1.SelectedValue);
                bool agregado = datos.agregaPermiso(usuario, permiso, usuarioLog);
                if (agregado)
                {
                    cargadatos();
                }
                else
                {
                    lblError.Text = "No se han agregado los permisos, revise su conexión";
                }
            }else
                Response.Redirect("Default.aspx");
        }
    }

    protected void btnAgregartodos_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        if (ddlUsuario.SelectedValue == "0")
        {
            lblError.Text = "Seleccione usuario";
        }
        else
        {
            Datos datos = new Datos();
            string usuarioLog = "";
            try { usuarioLog = Session["u"].ToString(); }
            catch (Exception) { Response.Redirect("Default.aspx"); }
            if (usuariolog != "")
            {
                string usuario = Convert.ToString(ddlUsuario.SelectedValue.ToString());

                bool agregado = datos.agregaPermisos(usuario, usuarioLog);
                if (agregado)
                {
                    cargadatos();

                }
                else
                {
                    lblError.Text = "No se han agregado los permisos, revise su conexión";
                    cargadatos();
                }
            }else
                Response.Redirect("Default.aspx");
        }
    }
    protected void btnAplicar_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        Datos datos = new Datos();
        string usuarioLog = "";
        try { usuarioLog = Session["u"].ToString(); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        if (usuarioLog != "")
        {
            string usuario = Convert.ToString(ddlUsuario.SelectedValue.ToString());
            if (usuario != "0")
            {
                bool agregado = datos.validaPermiso(usuario, usuarioLog);
                if (agregado)
                    cargadatos();
                else
                {
                    lblError.Text = "No se han aplicado los permisos, revise su conexión";
                    cargadatos();
                }
            }
            else
            {
                lblError.Text = "Debe indicar un usuario y/o debe indicar permisos a asignar";
                cargadatos();
            }
        }else
            Response.Redirect("Default.aspx");
    }
    protected void ddlUsuario_SelectedIndexChanged(object sender, EventArgs e)
    {
        llenadatos();
        cargadatos();
    }
}