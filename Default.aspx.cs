using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Text;

public partial class _Default : System.Web.UI.Page
{
    Datos datos = new Datos();
    protected void Page_Load(object sender, EventArgs e)
    {        
        int empresas = datos.obtieneEmpresasTotales();
        int empresasActivas = datos.obtieneEmpresasTotalesActivas();
        if (empresasActivas != 0)
        {
            if (empresas != 0)
            {
                if (empresas == 1)
                {
                    int id_empresa = datos.obtieneIdEmpresa();
                    Session["id_empresa"] = id_empresa;
                    Response.Redirect("Login.aspx?e=" + id_empresa);
                }                
                DataSet data = new DataSet();
                data = datos.llenaEmpresas();
                DataList1.DataSource = data;
                DataList1.DataBind();
            }
            else
                Response.Redirect("Login.aspx?e=0");
        }
        else
            Response.Redirect("Login.aspx?e=0");
    }
    protected void logoEmpresas_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton logo = (ImageButton)sender;
        int id_empresa = Convert.ToInt32(logo.CommandArgument);        
        Response.Redirect("Login.aspx?e=" + id_empresa);
    }


    private void cierraSesiones() {
        Session["id_empresa"] = null;
        Session["usuario"] = null;
        Session["nomUser"] = null;
    }
    protected void lblEmpresa_Click(object sender, EventArgs e)
    {
        LinkButton btnNombre = (LinkButton)sender;
        int id_empresa = Convert.ToInt32(btnNombre.CommandArgument);        
        Response.Redirect("Login.aspx?e=" + id_empresa);
    }
    protected void DataList1_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        ImageButton ibtn = (ImageButton)e.Item.FindControl("logoEmpresas") as ImageButton;
        int id_empresa = Convert.ToInt32(ibtn.CommandArgument);
        bool imagenExiste = datos.existeImagenEmpresa(id_empresa);
        if (imagenExiste)
            ibtn.Visible = true;
        else
            ibtn.Visible = false;
    }
}