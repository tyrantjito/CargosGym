using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PdfViewer1;

public partial class Cargas : System.Web.UI.Page
{
    string usuario;
    string idEmpresa;
    int contado;
    int numCargas;
    string cleinet_ant;
    ObtieneFechaActual fechaLocal = new ObtieneFechaActual();
    protected void Page_Load(object sender, EventArgs e)
    {
        int empresa = Convert.ToInt32(Session["e"]);
        if (empresa == 0)
        {
                Response.Redirect("Default.aspx");

        }else { 

        string usuario = Session["u"].ToString();
        string nombre = Session["nu"].ToString();
        int cont =  Convert.ToInt32(Session["C"] );
        string aspx = Session["aspx"].ToString();
        }
        if (!IsPostBack)
        {
            try
            {
                usuario = Session["u"].ToString();
                idEmpresa = Session["e"].ToString();
            }
            catch (Exception) { Response.Redirect("Default.aspx"); }
            Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
            int CONTA = 1;
            for (int i = 0; i < 12; i++)
            {
                periodos[i].Text = fechaLocal.obtieneFechaLocal().Year.ToString() + CONTA.ToString().PadLeft(2, '0');
                CONTA++;
            }
            lblEmpresa.Text = idEmpresa;
            lblCargas.Text = "0";
        }
    }


    protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlEstatus.SelectedValue == "R")
            lblCondicion.Text = " convert(char(10),dc.fecha_pago_rechazado,126) ";
        else
            lblCondicion.Text = " convert(char(10),dc.fecha_pago,126) ";
        try
        {
            usuario = Session["u"].ToString();
            idEmpresa = Session["e"].ToString();
            lblEmpresa.Text = idEmpresa;
        }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        ddlFechas.Items.Clear();
        ddlFechas.DataBind();
        llenaGrid();
    }
    protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            usuario = Session["u"].ToString();
            idEmpresa = Session["e"].ToString();
            lblEmpresa.Text = idEmpresa;
        }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        ddlEstatus.Items.Clear();
        ddlEstatus.DataBind();
        ddlFechas.Items.Clear();
        ddlFechas.DataBind();
        llenaGrid();
    }
    protected void ddlFechas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            usuario = Session["u"].ToString();
            idEmpresa = Session["e"].ToString();
            lblEmpresa.Text = idEmpresa;
        }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        string sql = SqlDataSource4.SelectCommand;
        llenaGrid();


    }

    private void llenaGrid() {
        if (ddlEstatus.SelectedValue == "R")
            lblCondicion.Text = " convert(char(10),dc.fecha_pago_rechazado,126) ";
        else
            lblCondicion.Text = " convert(char(10),dc.fecha_pago,126) ";
        SqlDataSource4.SelectCommand = "select (c.nombre+' '+ c.apellido_paterno+' '+ c.apellido_paterno) as Nombre,dc.id_cliente," +
"c.referencia, case dc.movimiento when 'R' then convert(char(10),dc.fecha_pago_rechazado,126) else convert(char(10),dc.fecha_pago,126) end as fecha_pago," +
"dc.motivo_rechzado,dc.consecutivo " +
"from detalle_cliente dc " +
"left join clientes c on dc.id_cliente = c.id_cliente " +
"where substring(dc.periodo,1,4)='"+ddlAño.SelectedValue+"' and substring(dc.periodo,5,2)='"+ddlPeriodo.SelectedValue+"' and (select top 1 movimiento from detalle_cliente where periodo=dc.periodo and id_cliente=dc.id_cliente order by consecutivo desc)=dc.movimiento " +
"and dc.movimiento='" + ddlEstatus.SelectedValue + "' and dc.id_empresa=" + lblEmpresa.Text + " and " + lblCondicion.Text + " = '" + ddlFechas.SelectedValue + "' order by c.nombre,c.apellido_paterno,c.apellido_materno,dc.id_cliente,c.referencia,dc.consecutivo";     
        
        GridView1.DataBind();

    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e) {
        if (e.Row.RowType == DataControlRowType.Header || e.Row.RowType == DataControlRowType.EmptyDataRow) {
            contado = 0;            
        }
        else if (e.Row.RowType == DataControlRowType.DataRow)
        {
            contado++;
            string usuario = DataBinder.Eval(e.Row.DataItem, "id_cliente").ToString();
            var etiqueta = e.Row.Cells[4].Controls[1].FindControl("lblMotivo") as Label;
            try
            {
                numCargas = Convert.ToInt32(lblCargas.Text.ToString());
                cleinet_ant = Convert.ToString(lblClientAnt.Text);
            }
            catch (Exception) { numCargas = 0; cleinet_ant = ""; }
            if (cleinet_ant != usuario)
            {
                numCargas = 1;
                lblCargas.Text = numCargas.ToString();
            }
            else
            {
                numCargas++;
                lblCargas.Text = numCargas.ToString();
            }

            if (numCargas > 1)
                etiqueta.Text = etiqueta.Text + " (carga " + numCargas.ToString() + ")";

            lblClientAnt.Text = usuario;

        }
        else if (e.Row.RowType == DataControlRowType.Footer) {
            lblTotal.Text = "Total Clientes: " + contado.ToString();
            lblClientAnt.Text = "";
        }
    }
}