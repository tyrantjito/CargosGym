using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

public partial class Login : System.Web.UI.Page
{
    Datos datos = new Datos();
    string usuario, nombre;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            eliminaArchivosGenerados();
        }
        if (Request.QueryString["e"] == null || Request.QueryString["e"]=="")
        {
            lblEtiquetaEmpresa.Visible = false;
            lblEmpresa.Visible = false;
            lblEmpresa.Text = "";
            Response.Redirect("Default.aspx");
        }
        else if (Request.QueryString["e"].ToString() == "0")
        {
            lblEtiquetaEmpresa.Visible = false;
            lblEmpresa.Visible = false;
            lblEmpresa.Text = "";
        }
        else
        {
            lblEtiquetaEmpresa.Visible = true;
            lblEmpresa.Visible = true;
            try
            {
                int empresa;
                try
                {
                    empresa = Convert.ToInt32(Request.QueryString["e"].ToString());
                }
                catch (Exception)
                {
                    empresa = 0;
                }
                lblEmpresa.Text = datos.obtieneNombreEmpresa(empresa);
            }
            catch (Exception)
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    private void eliminaArchivosGenerados()
    {        
        PdfViewer1.ObtieneFechaActual fechaActual = new PdfViewer1.ObtieneFechaActual();
        DateTime fecha = fechaActual.obtieneFechaLocal();
        string ruta = Server.MapPath("~/TMP/");
        string[] archivos = Directory.GetFiles(ruta);
        foreach (string fileName in archivos) {            
            FileInfo file = new FileInfo(fileName);
            if (file.Exists) {
                DateTime fechaCreacion = File.GetCreationTime(fileName);
                if (fechaCreacion >= fecha.AddDays(1) || fechaCreacion <= fecha.AddDays(-1)) {
                    file.Delete();
                }
            }
        }

    }
    protected void btnLog_Click(object sender, EventArgs e)
    {
        try
        {
            usuario = txtUsuarioLog.Text;
            string empresaVal = "";
            string contraseña = txtContraseñaLog.Text;
            btnBack.Visible = false;
            lblBack.Visible = false;
            bool existe = false;
            int empresa;
            bool[] arregloPermisos = new bool[22];
            for (int i = 0; i < 22; i++)
            {
                arregloPermisos[i] = false;
            }
            object[] obtieneInfo = datos.existeUsuarioVal(usuario);
            if (Convert.ToBoolean(obtieneInfo[0]))
            {
                existe = Convert.ToBoolean(obtieneInfo[1]);
                lblErrorLog.Text = "";
                if (existe)
                {
                    try
                    {
                        empresa = Convert.ToInt32(Request.QueryString["e"].ToString());
                    }
                    catch (Exception)
                    {
                        empresa = 0;
                    }
                    empresaVal = datos.validaEmpresaUsuario(usuario, empresa);
                    if (empresaVal == "")
                        empresaVal = "0";
                    if (empresaVal == Request.QueryString["e"].ToString() || usuario == "Supervisor")
                    {
                        bool usuarioValido = datos.verificaContraseña(usuario, contraseña);
                        if (usuarioValido)
                        {                            
                            nombre = datos.obtieneNombreUsuario(txtUsuarioLog.Text);
                            lblU.Text = nombre;
                            Panel2.Visible = true;
                            PanelMask.Visible = true;
                        }
                        else
                        {
                            lblErrorLog.Text = "La contraseña es incorrecta, verifique sus datos";
                        }
                    }
                    else
                    {
                        btnBack.Visible = true;
                        lblBack.Visible = true;
                        lblErrorLog.Text = "No corresponde a la empresa selecciona, regrese y verifique la empresa en la que fue ingresado";
                    }
                }
                else
                {
                    lblErrorLog.Text = "El usuario no existe, verifique sus datos";
                }
            }
            else
                lblErrorLog.Text = "Error: " + Convert.ToString(obtieneInfo[1]);
        }
        catch (Exception ex) {
            lblErrorLog.Text = "ERROR: " + ex.Message;
        }
    }
    protected void btnBack_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
    protected void lnkRecPas_Click(object sender, EventArgs e)
    {
        Response.Redirect("RecuperaContraseña.aspx");
    }
    protected void btnAceptar_Click(object sender, ImageClickEventArgs e)
    {
        Session["e"] = Request.QueryString["e"];
        Session["u"] = txtUsuarioLog.Text;
        Session["nu"] = datos.obtieneNombreUsuario(txtUsuarioLog.Text);
        Session["C"] = 1;
        Session["aspx"] = "Clientes1.aspx";
        Response.Redirect("Clientes1.aspx");
       
        //Response.Redirect("Clientes1.aspx?e=" + Request.QueryString["e"] + "&u=" + txtUsuarioLog.Text + "&nu=" + datos.obtieneNombreUsuario(txtUsuarioLog.Text));
    }
}