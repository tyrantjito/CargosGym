using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.IO;
using System.Drawing;
using Telerik.Web.UI;

public partial class Empresas : System.Web.UI.Page
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
            cargadatos();
    }
    
    public void cargadatos()
    {
        Datos conectar = new Datos();
        DataSet datos = new DataSet();
        datos = conectar.cargaEmpresas();
        if (datos.Tables.Count != 0)
        {
            GridView1.DataSource = datos;
            GridView1.DataBind();
        }
        string id_empresa = "0";
        if (Session["e"] == null || (Session["e"].ToString() == "0" && Session["u"].ToString() != "Supervisor"))
            Response.Redirect("Default.aspx");
        else
        {
            try { id_empresa = Session["e"].ToString(); }
            catch (Exception) { Response.Redirect("Default.aspx"); }
        }
    }

    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        lblError.Text = "";
        int empresa = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Value.ToString());
        TextBox txtNom = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtModnom");
        FileUpload img = (FileUpload)GridView1.Rows[e.RowIndex].FindControl("FileUpload1");
        TextBox txtMonto = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtMensualidadMod");
        TextBox txtUsuario = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtUsuarioMod");
        TextBox txtContraseña = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtContraseñaMod");
        TextBox txtHost = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtHostMod");
        RadNumericTextBox txtPuerto = (RadNumericTextBox)GridView1.Rows[e.RowIndex].FindControl("radPuerto");
        DropDownList ddlservidor = (DropDownList)GridView1.Rows[e.RowIndex].FindControl("ddlTipoServidor");
        CheckBox chkSSL = (CheckBox)GridView1.Rows[e.RowIndex].FindControl("chkSSlMod");
        TextBox txtMensaje = (TextBox)GridView1.Rows[e.RowIndex].FindControl("txtMensajeCorreo");

        string nombre = txtNom.Text;
        byte[] imagen = img.FileBytes;
        decimal monto = 0;
        try { monto = Convert.ToDecimal(txtMonto.Text); } catch (Exception) { monto = 0; }
        if (monto != 0)
        {
            bool actualizado = false;
            actualizado = datos.actualizaEmpresa(empresa, nombre, imagen, monto, txtUsuario.Text, txtContraseña.Text, txtPuerto.Value.ToString(), ddlservidor.SelectedValue, chkSSL.Checked, txtMensaje.Text, txtHost.Text);
            if (actualizado)
            {
                GridView1.EditIndex = -1;
                cargadatos();
            }
            else
            {
                lblError.Text = "No se pudo actualizar la empresa verifique su conexión.";
                cargadatos();
            }
        }
        else
            lblError.Text = "El monto debe ser mayor a cero";
    }

    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        cargadatos();

    }
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        cargadatos();
    }

    protected void btnAgregar_Click1(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";
        string[] extensiones = { "bmp", "jpg", "png", "gif", "tiff", "jpeg" };
        byte[] imagen = FileUpload2.FileBytes;
        if (FileUpload2.FileBytes.Length > 0)
        {
            char[] separador = { '.' };
            string[] datosNombre = FileUpload2.FileName.Split(separador);
            string extension = datosNombre[datosNombre.Length - 1].ToString();
            bool archivoValido = false;
            for (int i = 0; i < 6; i++)
            {
                if (extensiones[i] == extension)
                {
                    archivoValido = true;
                    break;
                }
            }

            if (archivoValido)
            {
                string nombre = txtNombre.Text;
                bool agregado = false;
                decimal monto = 0;
                try { monto = Convert.ToDecimal(txtMensualidad.Text); } catch (Exception) { monto = 0; }
                if (monto != 0)
                {
                    agregado = datos.agregaEmpresa(nombre, imagen, monto);
                    if (agregado)
                    {
                        txtNombre.Text = "";
                        cargadatos();
                    }
                    else
                    {
                        lblError.Text = "No se pudo agregar la empresa verifique su conexión.";
                        cargadatos();
                    }
                }
                else
                {
                    lblError.Text = "El monto debe ser mayor a cero";
                    cargadatos();
                }
            }
            else
            {
                lblError.Text = "Solo es posible cargar archivos con extensión .bmp, .jpg, .png, .gif, .tiff, .jpeg";
                cargadatos();
            }
        }
        else
        {
            lblError.Text = "Debe indicar el logo de la empresa";
            cargadatos();
        }
    }

    protected void btnaut_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        ImageButton btnaut = (ImageButton)sender;
        string empresa = btnaut.CommandArgument;
        bool autestatus = false;
        autestatus = datos.autEstatus(empresa);
        if (autestatus)
            cargadatos();
        else
        {
            lblError.Text = "No se pudo cambiar estatus verifique su conexión.";
            cargadatos();
        }
    }

    protected void btndes_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        ImageButton btndes = (ImageButton)sender;
        string empresa = btndes.CommandArgument;
        bool desestatus = false;
        desestatus = datos.desEstatus(empresa);
        if (desestatus)
            cargadatos();
        else
        {
            lblError.Text = "No se pudo cambiar estatus verifique su conexión.";
            cargadatos();
        }
    }

    protected void btnActualizarMod_Click(object sender, ImageClickEventArgs e)
    {
        byte[] imagen = FileUploadPopup.FileBytes;
        lblErrorMod.Text = "";
        lblError.Text = "";
        lblErrorCargaLogoAsterisco.Visible = false;
        bool archivoValido = false;
        try
        {
            if (imagen.Length != 0)
            {
                string[] extensiones = { "bmp", "jpg", "png", "gif", "tiff", "jpeg" };
                string[] datosNombre = FileUploadPopup.FileName.Split(new char[] { '.' });
                string extension = datosNombre[datosNombre.Length - 1].ToString();
                for (int i = 0; i < 6; i++)
                {
                    if (extensiones[i] == extension)
                    {
                        archivoValido = true;
                        break;
                    }
                }
                if (!archivoValido)
                    lblErrorMod.Text = "Formato de imagen incorrecto, debe indicar imagenes de tipo 'bmp, jpg, png, gif, tiff, jpeg'";
                else {
                    if (archivoValido)
                    {
                        lblErrorCargaLogoAsterisco.Visible = false;
                        string nombre = txtNombreMod.Text.TrimEnd();
                        int id_empresa = 0;
                        decimal monto = 0;
                        try { id_empresa = Convert.ToInt32(Session["e"]); }
                        catch (Exception) { id_empresa = 0; }
                        try { monto = Convert.ToDecimal(txtMensualidadMod.Text); } catch (Exception) { monto = 0; }
                        if (monto != 0)
                        {
                            bool actualiza = false;
                            actualiza = datos.actualizaEmpresa(id_empresa, nombre, imagen, monto, txtUsuarioMod.Text, txtContraseñaMod.Text, radPuerto.Value.ToString(), ddlTipoServidor.SelectedValue, chkSSlMod.Checked, txtMensajeCorreo.Text, txtHostMod.Text);
                            if (actualiza)
                            {
                                PanPopUpDiv.Visible = false;
                                PanPopUp.Visible = false;
                                lblErrorMod.Text = "";
                                lblErrorCargaLogoAsterisco.Visible = false;
                                cargadatos();
                            }
                            else
                                lblErrorMod.Text = "Hubo un problema en la actualización verifique su conexión e intentelo nuevamente mas tarde";
                        }
                        else
                            lblErrorMod.Text = "El monto debe ser mayo a cero";
                    }
                }
            }
            else
            {
                archivoValido = true;
                if (archivoValido)
                {
                    lblErrorCargaLogoAsterisco.Visible = false;
                    string nombre = txtNombreMod.Text.TrimEnd();
                    int id_empresa = 0;
                    decimal monto = 0;
                    try { id_empresa = Convert.ToInt32(Session["e"]); }
                    catch (Exception) { id_empresa = 0; }
                    try { monto = Convert.ToDecimal(txtMensualidadMod.Text); } catch (Exception) { monto = 0; }
                    if (monto != 0)
                    {
                        bool actualiza = false;
                        actualiza = datos.actualizaEmpresa(id_empresa, nombre, imagen, monto, txtUsuarioMod.Text, txtContraseñaMod.Text, radPuerto.Value.ToString(), ddlTipoServidor.SelectedValue, chkSSlMod.Checked, txtMensajeCorreo.Text, txtHostMod.Text);
                        if (actualiza)
                        {
                            PanPopUpDiv.Visible = false;
                            PanPopUp.Visible = false;
                            lblErrorMod.Text = "";
                            lblErrorCargaLogoAsterisco.Visible = false;
                            cargadatos();
                        }
                        else
                        {
                            lblErrorMod.Text = "Hubo un problema en la actualización verifique su conexión e intentelo nuevamente mas tarde";
                        }
                    }
                    else
                        lblErrorMod.Text = "El monto debe ser mayo a cero";
                }
            }                        
        }
        catch (Exception)
        {
            lblErrorMod.Text = "Debe indicar el Nombre";
            lblErrorCargaLogoAsterisco.Visible = true;
        }
    }

    protected void btnCancelarMod_Click(object sender, ImageClickEventArgs e)
    {
        PanPopUpDiv.Visible = false;
        PanPopUp.Visible = false;
        lblError.Text = "";
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Modificar")
        {
            string[] valores = e.CommandArgument.ToString().Split(new char[]{';'});
            
            txtNombreMod.Text = valores[1];
            txtMensualidadMod.Text = valores[2];
            try
            {
                DataSet info = datos.obtieneInfoEmpresaParametrosCorreo(valores[0]);
                foreach (DataRow r in info.Tables[0].Rows)
                {
                    txtUsuarioMod.Text = r[0].ToString();
                    txtContraseñaMod.Text = r[1].ToString();
                    txtHostMod.Text = r[2].ToString();
                    radPuerto.Value = Convert.ToDouble(r[3]);
                    ddlTipoServidor.SelectedValue = r[4].ToString();
                    chkSSlMod.Checked = Convert.ToBoolean(r[5]);
                    txtMensajeCorreo.Text = r[6].ToString();
                }
            }
            catch (Exception) {
                txtUsuarioMod.Text = txtContraseñaMod.Text = txtHostMod.Text = txtMensajeCorreo.Text = "";
                radPuerto.Value = 0;
                ddlTipoServidor.SelectedValue = "0";
                chkSSlMod.Checked = false;                 
            }
            PanPopUpDiv.Visible = true;
            PanPopUp.Visible = true;
        }
        if (e.CommandName == "Eliminar")
        {
            lblError.Text = "";
            int idEmpresa = Convert.ToInt32(e.CommandArgument);
            bool borrado = false;
            int relaciones = 1;
            int IdEmpresaLog = 0;
            try { IdEmpresaLog = Convert.ToInt32(Session["e"]); }
            catch (Exception) { Response.Redirect("Default.aspx"); }
            if (idEmpresa != IdEmpresaLog)
            {
                relaciones = datos.obtieneCoincidenciasEmpresas(idEmpresa);
                if (relaciones == 0)
                {
                    borrado = datos.borraEmpresa(idEmpresa.ToString());
                    if (borrado)
                        cargadatos();
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "No se pudo borrar la empresa verifique su conexión.";
                        cargadatos();
                    }
                }
                else
                {
                    lblError.Visible = true;
                    lblError.Text = "No se logro eliminar la empresa ya que se encuentra relacionada";
                }
            }
            else
            {
                lblError.Visible = true;
                lblError.Text = "No puede eliminar la empresa en la que a ingresado ";
            }
        }
    }
}