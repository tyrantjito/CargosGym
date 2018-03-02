using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class Usuarios : System.Web.UI.Page
{
    
    bool tienePermiso;
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
        }
    }

    public void cargadatos()
    {
        Datos conectar = new Datos();
        DataSet datos = new DataSet();
        datos = conectar.cargaUsuarios();
        if (datos.Tables.Count != 0)
        {
            GridView1.DataSource = datos;
            GridView1.DataBind();
        }        

        string id_empresa = "0";
        if (Session["e"] == null || (Session["e"].ToString() == "0" && Session["u"].ToString() != "Supervisor"))
            Response.Redirect("Default.aspx");
        else
            id_empresa = Session["e"].ToString();
    
        bool activo = conectar.obtieneUsuarioAfiliacion();
        if (activo)
        {
            CheckBox1.Enabled = false;
        }
        
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                Datos datos = new Datos();
                bool[] permisos = datos.obtienePermisos(Session["u"].ToString());
                tienePermiso = permisos[20];
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string usuario = DataBinder.Eval(e.Row.DataItem, "id_usuario").ToString();
                if (e.Row.RowState.ToString() == "Normal" || e.Row.RowState.ToString() == "Alternate")
                {

                    GridView1.Columns[4].Visible = true;
                    txtContraseña.Visible = true;
                    btnAgregar.Visible = true;
                    txtIdu.Visible = true;
                    txtNombre.Visible = true;
                    CheckBox1.Visible = true;
                    var valores = e.Row.FindControl("CheckUsua") as CheckBox;
                    Datos datos = new Datos();
                    valores.Checked = datos.obtieneUsaAfiliacion(usuario);
                    var img = e.Row.FindControl("lknELiminar") as ImageButton;
                    if (valores.Checked)
                        img.Visible = false;
                    var contra = e.Row.FindControl("lblContraseña") as Label;
                    if (!tienePermiso)
                        contra.Text = "********************";
                }
            }
        }
        catch (Exception) { Response.Redirect("Default.aspx"); }
    }   
    
    protected void btnAgregar_Click(object sender, ImageClickEventArgs e)
    {
        string id_usuario = txtIdu.Text;
        string contraseña = txtContraseña.Text;
        string nombre = txtNombre.Text;
        bool usuario_afiliacion = CheckBox1.Checked;
        int usaAfiliacion;
        if (usuario_afiliacion)
            usaAfiliacion = 1;
        else
            usaAfiliacion = 0;        
       
        bool agregado = false;
        Datos datos = new Datos();
        agregado = datos.agregaUsuario(id_usuario, contraseña, nombre, usaAfiliacion);
        if (agregado)
        {
            txtIdu.Text = "";
            txtContraseña.Text = "";
            txtNombre.Text = "";
            CheckBox1.Checked = false;
            bool activo = datos.obtieneUsuarioAfiliacion();
            if (activo)
            {
                CheckBox1.Enabled = false;
            }            
            cargadatos();
        }
        else
        {
            lblError.Text = "No se pudo agregar el usuario verifique su conexión.";
            cargadatos();
        }
    }
    protected void lknEliminar_Click(object sender, EventArgs e)
    {
        Datos datos = new Datos();
        ImageButton lknEliminar = (ImageButton)sender;
        string claveusuario = lknEliminar.CommandArgument ;
        bool existeRelacion = datos.obtieneRelacionU(claveusuario);
        if (!existeRelacion)
        {
        bool borrado = false;
        borrado = datos.borraUsuario(claveusuario);
        
        if (borrado)
        {

            cargadatos();
        }
        else
        {
            lblError.Text = "No se pudo borrar el usuario verifique su conexión.";
            cargadatos();
        }
      }else
            lblError.Text = "No se puede eliminar el usuario; se encuentra usado en otro proceso";
    }

    
   
    protected void btnCancelar_Click(object sender, ImageClickEventArgs e)
    {
        Panel4.Visible = false;
        Panel3.Visible = false;        
    }
    protected void btneditar_Click(object sender, ImageClickEventArgs e)
    {
        Panel4.Visible = true;
        ImageButton btneditar = (ImageButton)sender;
        Datos datos = new Datos();
        bool[] permisos = datos.obtienePermisos(Session["u"].ToString());
        tienePermiso = permisos[20];
        string[] objs = btneditar.CommandArgument.ToString().Split(new char[] { ';' });
        string claveusuario=objs[0];
        string contraseña = objs[1];
        string nombre = objs[2];
        string usa = objs[3];
        string correo = objs[4];
        if (usa == "True")
            chkUsua.Checked = true;
        else
            chkUsua.Checked = false;
        
        lblUser.Text = claveusuario.ToString();
        if (tienePermiso)
        {
            txtPass.Enabled = true;
            txtPass.TextMode = TextBoxMode.SingleLine;
            RequiredFieldValidator5.Enabled = RegularExpressionValidator2.Enabled = true;
            txtPass.Text = contraseña.ToString();
        }
        else {
            txtPass.Enabled = false;
            txtPass.TextMode = TextBoxMode.Password;
            RequiredFieldValidator5.Enabled = RegularExpressionValidator2.Enabled = false;
            txtPass.Text = contraseña.ToString();
        }
        txtNombrea.Text=nombre.ToString();
        txtCorreoMod.Text = correo;
        Panel3.Visible = true;        
    }
    protected void btnActualizar_Click(object sender, ImageClickEventArgs e)
    {
        Panel4.Visible = true;
        bool actualizado = false;
        int usa = 0;
        if (chkUsua.Checked)
            usa = 1;
        else
            usa = 0;
        Datos datos = new Datos();

        actualizado = datos.actualizausuario(lblUser.Text, txtPass.Text, txtNombrea.Text, usa, txtCorreoMod.Text);
        if (actualizado)
        {
            Panel3.Visible = false;
            Panel4.Visible = false;
            cargadatos();
        }
        else
        {
            lblErrorMod.Text = "No se pudo actualizar el usuario verifique su conexión.";
            cargadatos();
        }
    }

}      

