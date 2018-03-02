using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;


public partial class Usuarios_empresas : System.Web.UI.Page
{Datos datos = new Datos();
    protected void Page_Load(object sender, EventArgs e)
    {
       /* int empresa = Convert.ToInt32(Session["e"]);
        string usuario = Session["u"].ToString();
        string nombre = Session["nu"].ToString();
        int cont = Convert.ToInt32(Session["C"]);
        string aspx = Session["aspx"].ToString();*/

        if (!Page.IsPostBack)
        {
            cargadatos();
        }
    }
    public void cargadatos()
    {
        Datos conectar = new Datos();
        DataSet datos = new DataSet();
        datos = conectar.cargaUsuariosEmpresas();
        GridView1.DataSource = datos;
        GridView1.DataBind();
       
    }
    protected void lknEliminar_Click(object sender, EventArgs e)
    {
        ImageButton lknEliminar = (ImageButton)sender;
        char[] separador = { ';' };
        string argumentos =  lknEliminar.CommandArgument;
        string[] valores = argumentos.Split(separador);
        string claveusuario = valores[0];
        int empresa = Convert.ToInt32(valores[1].ToString());
        bool borrado = false;
        borrado = datos.borraUsuarioEmpresa(claveusuario, empresa);
        
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
        if (ddlUsuario.SelectedValue == "0" && ddlEmpresa.SelectedValue == "0")
        {
            lblError.Text = "Seleccione el usuario y/o la empresa";
        }

        else if (ddlUsuario.SelectedValue == "0")
        {
            lblError.Text = "Seleccione usuario";
        }

        else if (ddlEmpresa.SelectedValue == "0")
        {
            lblError.Text = "Seleccione empresa";
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

            string id_empresa = ddlEmpresa.SelectedValue;
            int empresa;
            try
            {
                empresa = Convert.ToInt32(id_empresa);
            }
            catch
            {
                empresa = 0;
            }

            bool existeUsuarioEmpresa = datos.existeUsuarioEmpresa(usuario, empresa);
            if (!existeUsuarioEmpresa)
            {
                bool agregado = false;
                agregado = datos.agregaUsuarioEmpresa(usuario.ToString(), empresa);
                if (agregado)
                {
                    ddlUsuario.SelectedValue = "0";
                    ddlEmpresa.SelectedValue = "0";
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
                lblError.Text = "El Usuario indicado con la empresa relacionada ya existe";
        }
    }
    }
