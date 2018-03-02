using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

public partial class Afiliacion : System.Web.UI.Page
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
        }
    }
    public void cargadatos()
    {
        lblError.Text = "";
        Datos conectar = new Datos();
        DataSet datos = new DataSet();
        datos = conectar.cargaAfiliacion();
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
        string claveafiliacion = lknEliminar.CommandArgument;
        bool existeRelacion = datos.obtieneRelacion(claveafiliacion);
        
        if (!existeRelacion)
        {
            bool borrado = datos.borraAfiliacion(claveafiliacion);
            if (borrado)
                cargadatos();
            else
            {
                lblError.Text = "No se pudo borrar la afiliación verifique su conexión.";
                cargadatos();
            }
        }
        else
            lblError.Text = "No se puede eliminar la afiliación, se encuentra en uso en otro proceso.";
    }
    protected void btnAgregar_Click(object sender, ImageClickEventArgs e)
    {        
        if (ddlBank.SelectedValue == "0")        
            lblError.Text = "Seleccione el banco";        
        else
        {
            string afi = txtNoafi.Text;
            int criterio;
            double comision;
            try{ criterio = Convert.ToInt32(txtCriterio.Text); }
            catch (Exception) { criterio = 0; }
            int dias;
            try { dias = Convert.ToInt32(txtDias.Text); }
            catch (Exception) { dias = 0; }            
            if (fecha.Text == "1900-01-01" || fecha.Text == "")
                lblError.Text = "Debe indicar una fecha válida";
            else
            {
                try { comision = Convert.ToDouble(txtComision.Text); }
                catch (Exception) { comision = 0; }
                string banco = ddlBank.SelectedValue;
                bool existe = false;
                existe = datos.existeAfi(afi, 0);
                if (!existe)
                {
                    bool agregado = datos.agregaAfiliacion(afi, criterio, dias, fecha.Text, banco, comision);
                    if (agregado)
                    {
                        ddlBank.SelectedValue = "0";
                        txtNoafi.Text = "";
                        txtCriterio.Text = "";
                        txtDias.Text = "";
                        fecha.Text = "";
                        txtComision.Text = "";
                        lblError.Text = "";
                        cargadatos();
                    }
                    else
                    {
                        lblError.Text = "No se pudo agregar la afiliación verifique su conexión.";
                        cargadatos();
                    }
                }
                else                
                    lblError.Text = "Esa afiliación ya existe, ingrese otra afiliación";
            }
        }
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string afiliacion = DataBinder.Eval(e.Row.DataItem, "id_afiliacion").ToString();
            if (e.Row.RowState.ToString() == "Normal" || e.Row.RowState.ToString() == "Alternate")
            {
                GridView1.Columns[6].Visible = true;
                txtNoafi.Visible = true;
                txtCriterio.Visible = true;
                txtDias.Visible = true;
                fecha.Visible = true;
                txtComision.Visible = true;
                ddlBank.Visible = true;
                btnAgregar.Visible = true;
            }           
        }
    }

    protected void btnCancelar_Click(object sender, ImageClickEventArgs e)
    {
        Panel3.Visible = false;
        Panel4.Visible = false;
    }

    protected void btneditar_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btneditar = (ImageButton)sender;
        string[] objs = btneditar.CommandArgument.ToString().Split(new char[] { ';' });
        string claveafi = objs[0];
        string afi = objs[1];
        string cri = objs[2];
        string dias = objs[3];
        string fecha = objs[4];
        string banco = objs[5];
        string com = objs[6];
        DateTime fechaMod;
        try
        {
            fechaMod = Convert.ToDateTime(fecha);
        }
        catch (Exception) { fechaMod = Convert.ToDateTime("1900-01-01"); }
        lblId.Text = claveafi.ToString();
        lblAfi.Text = afi.ToString();
        txtCri.Text = cri.ToString();
        txtDays.Text = dias.ToString();
        txtDate.Text = fechaMod.ToString("yyyy-MM-dd");                
        try
        {
            ddlBanco.SelectedValue = banco.ToString();
        }
        catch (Exception) {
            ddlBanco.SelectedValue = "0";
        }
        txtComi.Text = com.ToString();

        Panel3.Visible = true;
        Panel4.Visible = true;
    }

    protected void btnActualizar_Click(object sender, ImageClickEventArgs e)
    {
        int afiliacion = Convert.ToInt32(lblId.Text); 
        string afi = lblAfi.Text;
        int cri;
        try { cri = Convert.ToInt32(txtCri.Text); }
        catch (Exception) { cri = 0; }
        int dias;
        try { dias = Convert.ToInt32(txtDays.Text); }
        catch (Exception) { dias = 0; }
        string fecha = txtDate.Text;
        string id_banco = ddlBanco.SelectedValue;
        int banco;
        try { banco = Convert.ToInt32(id_banco); }
        catch { banco = 0; }
        double com;
        try { com = Convert.ToDouble(txtComi.Text); }
        catch (Exception) { com = 0; }
        if (ddlBanco.SelectedValue == "0")
        {
            lblErrorMod.Text = "Seleccione el banco";
            Panel3.Visible = true;
            Panel4.Visible = true;
        }
        else
        {
            bool actualizado = false;
            actualizado = datos.actualizaAfiliacion(afiliacion, afi, cri, dias, fecha, banco, com);
            if (actualizado)
            {
                lblErrorMod.Text = "";
                Panel3.Visible = false;
                Panel4.Visible = false;
                cargadatos();                
            }
            else
            {
                lblErrorMod.Text = "No se pudo actualizar la afiliación verifique su conexión.";
                Panel3.Visible = true;
                Panel4.Visible = true;
            }
        }
    }
}