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
using System.Drawing;
using PdfViewer1;
using E_Utilities;

public partial class Consultas : System.Web.UI.Page
{
    Datos datos = new Datos();
    int id_empresa, i;    
    int registros;
    ObtieneFechaActual fechaLocal = new ObtieneFechaActual();
    Fechas fechas = new Fechas();
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

        if (Session["u"] == null || Session["u"].ToString() == "" || Session["e"] == null || Session["e"].ToString() == "")
            Response.Redirect("Default.aspx");
        /*if (Session["clientes"] != null) {
            Response.Redirect("Clientes1.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]);
        }*/

        //Response.Redirect("Clientes1.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]);

        if (!IsPostBack)
        {
            ddlMes.SelectedValue = fechaLocal.obtieneFechaLocal().Month.ToString();
            if (i == 0)
            {
                //cargaDatos();
            }
            i = 0;

            Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
            int CONTA = 1;
            for (int j = 0; j < 12; j++)
            {
                periodos[j].Text = fechaLocal.obtieneFechaLocal().Year.ToString() + CONTA.ToString().PadLeft(2, '0');
                CONTA++;
            }

            /*Agregado*/

            switch (rdbOpcion.SelectedValue)
            {
                case "0":
                    pnlPagos.Visible = true;
                    pnlConsultaUsuarios.Visible = pnlConsultaSocios.Visible = false;
                    break;
                case "1":
                    pnlConsultaUsuarios.Visible = true;
                    pnlPagos.Visible = pnlConsultaSocios.Visible = false;
                    ddlAños.Items.Clear();
                    ddlAños.DataBind();
                    llenaDtUsuarios();
                    break;
                case "2":
                    pnlConsultaSocios.Visible = true;
                    pnlPagos.Visible = pnlConsultaUsuarios.Visible = false;
                    PanelDetalle.Visible = false;
                    break;
                default:
                    pnlPagos.Visible = pnlConsultaUsuarios.Visible = pnlConsultaSocios.Visible = false;
                    break;
            }
        }
    }

    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {        
        cargaDatos();
    }

    private void cargaDatos()
    {
        string usuario = Session["u"].ToString();
        bool[] permisos = datos.obtienePermisos(usuario);
        bool permisoBool = permisos[16];       
        registros = 0;
        try { id_empresa = Convert.ToInt32(Session["e"]); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        string mes = "";
        string mesInicio = fechaLocal.obtieneFechaLocal().ToString("MM");
        if (!IsPostBack)
        {
            mes = mesInicio;
            llenaAñoDdl();
        }
        else
            mes = ddlMes.SelectedValue.ToString();
        ddlMes.SelectedValue = mes;
        string año = ddlAño.SelectedValue.ToString();
        string periodoActual = fechaLocal.obtieneFechaLocal().Year.ToString() + fechaLocal.obtieneFechaLocal().Month.ToString().PadLeft(2, '0');
        string periodoEnDdls = año + mes;
        int periodoActualInt = 0, periodoEnDdlsInt = 0;
        string estatusCliente = ddlEstatus.SelectedValue;
        string periodicidad = "M";
        string estatusPago = ddlPago.SelectedValue.ToString();
        string nombreStatus = rbtnTipo.SelectedValue.ToString();
        string filtroCampo = ddlBuscar.SelectedValue.ToString();
        string filtroTetxo = txtFiltro.Text;
        string clienteTarjeta = rbtnTipo.SelectedValue;
        DataSet data = new DataSet();
        try
        {
            periodoActualInt = Convert.ToInt32(periodoActual);
            periodoEnDdlsInt = Convert.ToInt32(periodoEnDdls);
        }
        catch (Exception) { periodoActualInt = 1; periodoEnDdlsInt = 0; }
        // Tiene permiso totales y muestra info siguente mes o información por pagar 
        /*if (periodoActualInt < periodoEnDdlsInt && permisoBool)
        {            
            data = datos.llenaSuperConsultasMesNuevo(id_empresa, año, mes);
        }
        else
        {*/
            /*
            lblTextoMontoTotalMes.Visible = true;
            lblTextoActivos.Visible = true;
            lblActivos.Visible = true;
            lblActivos.Text = datos.cuentaClientesEstatus(id_empresa, "A").ToString();
            lblTextoInactivos.Visible = true;
            lblInactivos.Visible = true;
            lblInactivos.Text = datos.cuentaClientesEstatus(id_empresa, "I").ToString();
            lblTextoTotalClientes.Visible = true;
            lblTotalClientes.Visible = true;
            lblTotalClientes.Text = datos.cuentaTotalClientes(id_empresa);
            lblTextoPagoRechazado.Visible = true;
            lblRechazados.Visible = true;
            lblRechazados.Text = datos.cuentaClientesMovimientoPeriodoEstatus(id_empresa, año, mes, estatusCliente, "R").ToString();
            lblTextoPagoRealizado.Visible = true;
            lblPagados.Visible = true;
            lblPagados.Text = datos.cuentaClientesMovimientoPeriodoEstatus(id_empresa, año, mes, estatusCliente, "P").ToString(); 
            lblMontoPagados.Visible = true;
            lblMontoPagados.Text = Convert.ToDecimal(datos.calculaMontoPagoRealizadoPeriodo(id_empresa, estatusCliente, año, mes, "P")).ToString("C2");
            lblTextoPagoManual.Visible = true;
            lblManual.Visible = true;
            lblManual.Text = datos.cuentaClientesMovimientoPeriodoEstatus(id_empresa, año, mes, estatusCliente, "M").ToString(); ;
            lblMontoManual.Visible = true;
            lblMontoManual.Text = Convert.ToDecimal(datos.calculaMontoPagoRealizadoPeriodo(id_empresa, estatusCliente, año, mes, "M")).ToString("C2");
            lblTextoPagoPendiente.Visible = true;
            lblPendiente.Visible = true;
            lblPendiente.Text = datos.cuentaClientesPendientesPeriodoEstatus(id_empresa, año, mes, estatusCliente, estatusPago).ToString();
            lblMontoTotalMes.Text = (Convert.ToDouble(datos.calculaMontoPagoRealizadoPeriodo(id_empresa, estatusCliente, año, mes, "M")) + Convert.ToDouble(datos.calculaMontoPagoRealizadoPeriodo(id_empresa, estatusCliente, año, mes, "P"))).ToString("C2");
            lblMontoTotalMes.Visible = true;*/
            data = datos.llenaSuperConsultas(año, mes, estatusCliente, periodicidad, estatusPago, filtroCampo, filtroTetxo, clienteTarjeta, id_empresa);
            /*if (ddlPago.SelectedValue == "R")
                lblTotales.Text = "Clientes Pago Rechazado: " + lblRechazados.Text;
            else if (ddlPago.SelectedValue == "P")
                lblTotales.Text = "Clientes Pago Realizado: " + lblPagados.Text;
            else if (ddlPago.SelectedValue == "M")
                lblTotales.Text = "Clientes Pago Sucursal: " + lblManual.Text;
            else if (ddlPago.SelectedValue == "0")
                lblTotales.Text = "Clientes Pago Pendiente: " + lblPendiente.Text;
            else
                lblTotales.Text = "";*/
        //}
        try
        {
            GridRec.DataSource = data; registros = 0;
            GridRec.DataBind();
        }
        catch (Exception)
        {
            GridRec.DataSource = null; registros = 0;
            GridRec.DataBind();
        }
    }

    public void llenaAñoDdl()
    {
        int idEmpresa = 0;
        try { idEmpresa = Convert.ToInt32(Session["e"]); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        DataSet data = new DataSet();
        try { data = datos.llenaAñoConsultas(id_empresa); }
        catch (Exception) { data = null; }
        this.ddlAño.DataSource = data;
        this.ddlAño.DataValueField = "periodo";
        this.ddlAño.DataTextField = "periodo";
        this.ddlAño.DataBind();
    }

    protected void btnFiltrarGrid_Click(object sender, EventArgs e)
    {
        cargaDatos();
    }

    protected void txtFiltro_TextChanged(object sender, EventArgs e)
    {
        cargaDatos();
    }

    protected void ddlMes_SelectedIndexChanged(object sender, EventArgs e)
    {        
        cargaDatos();
    }

    protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
    {        
        cargaDatos();
    }

    protected void ddlAño_SelectedIndexChanged(object sender, EventArgs e)
    {
        cargaDatos();
    }

    protected void rbtnTipo_SelectedIndexChanged(object sender, EventArgs e)
    {        
        cargaDatos();
    }

    protected void ddlPago_SelectedIndexChanged(object sender, EventArgs e)
    {        
        cargaDatos();
    }

    protected void lbtnLimpiar_Click(object sender, EventArgs e)
    {
        ddlBuscar.SelectedValue = "0";
        txtFiltro.Text = "";
        cargaDatos();
    }

    protected void GridRec_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.RowState.ToString() == "Alternate")
                e.Row.BackColor = Color.WhiteSmoke;
            else
                e.Row.BackColor = Color.White;
            string usuario = "";
            try{ usuario = Session["u"].ToString(); } catch(Exception){Response.Redirect("Default.aspx");}
            try
            {
                ImageButton btnEditar = e.Row.FindControl("btnEditar") as ImageButton;
                Label lblNombre = e.Row.FindControl("lblNombre") as Label;
                Label lblTarjetahabiente = e.Row.FindControl("lblTarjetahabiente") as Label;
                Label lblPagoRechazado = e.Row.FindControl("lblPagoRechazado") as Label;
                Label lblPago = e.Row.FindControl("lblPago") as Label;
                Label lblMotivoRechazo = e.Row.FindControl("lblMotivoRechazo") as Label;
                string estatusPago = ddlPago.SelectedValue.ToString();
                string names = rbtnTipo.SelectedValue.ToString();//t y c
                lblTotales.ForeColor = Color.White;
                if (estatusPago == "0")
                {
                    bool[] permisos = datos.obtienePermisos(usuario);
                    if (permisos[15])
                        btnEditar.Visible = true;
                    else
                        btnEditar.Visible = false;
                    if (names == "T")
                    {
                        lblNombre.Visible = false;
                        lblTarjetahabiente.Visible = true;
                    }
                    else if (names == "C")
                    {
                        lblNombre.Visible = true;
                        lblTarjetahabiente.Visible = false;
                    }
                    lblPagoRechazado.Visible = false;
                    lblPago.Visible = false;
                    lblMotivoRechazo.Visible = false;
                    registros++;
                    
                }
                else if (estatusPago == "P")
                {
                    btnEditar.Visible = false;
                    if (names == "T")
                    {
                        lblNombre.Visible = false;
                        lblTarjetahabiente.Visible = true;
                    }
                    else if (names == "C")
                    {
                        lblNombre.Visible = true;
                        lblTarjetahabiente.Visible = false;
                    }
                    lblPagoRechazado.Visible = false;
                    lblPago.Visible = true;
                    lblMotivoRechazo.Visible = false;
                    registros++;
                    
                }
                else if (estatusPago == "M")
                {
                    btnEditar.Visible = false;
                    if (names == "T")
                    {
                        lblNombre.Visible = false;
                        lblTarjetahabiente.Visible = true;
                    }
                    else if (names == "C")
                    {
                        lblNombre.Visible = true;
                        lblTarjetahabiente.Visible = false;
                    }
                    lblPagoRechazado.Visible = false;
                    lblPago.Visible = true;
                    lblMotivoRechazo.Visible = true;
                    registros++;
                    
                }
                else if (estatusPago == "R")
                {
                    bool[] permisos = datos.obtienePermisos(usuario);
                    if(permisos[15])
                        btnEditar.Visible = true;
                    else
                        btnEditar.Visible = false;
                    if (names == "T")
                    {
                        lblNombre.Visible = false;
                        lblTarjetahabiente.Visible = true;
                    }
                    else if (names == "C")
                    {
                        lblNombre.Visible = true;
                        lblTarjetahabiente.Visible = false;
                    }
                    lblPagoRechazado.Visible = true;
                    lblPago.Visible = false;
                    lblMotivoRechazo.Visible = true;
                    registros++;                    
                }
                if (ddlEstatus.SelectedValue == "I")
                    btnEditar.Visible = false;
            }
            catch (Exception) { }
        }
        else
        {
            registros = 0;
        }
    }

    protected void btnEditar_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btnEditar = (ImageButton)sender;
        lblClientePago.Text = btnEditar.CommandArgument.ToString();
        DataTable data = new DataTable();
        int cliente = Convert.ToInt32(lblClientePago.Text);
        int empresa = 0;
        try { empresa = Convert.ToInt32(Session["e"]); }
        catch (Exception) { empresa = 0; }
        if (empresa != 0)
        {
            data = llenaDataList(cliente, empresa);
            DataList1.DataSource = data;
            DataList1.DataBind();
            PanelMask.Visible = true;
            PanelMeses.Visible = true;
        }
    }
    
    private DataTable llenaDataList( int cliente, int empresa)
    {
        DataTable datSet = new DataTable();        
        string periodo = ddlAño.SelectedValue + ddlMes.SelectedValue.PadLeft(2, '0');
        int año = Convert.ToInt32(ddlAño.SelectedValue);
        datSet = datos.obtienePagosRetrasados(empresa, cliente, año, periodo);
        return datSet;
    }

    protected void btnAgregarPagoM_Click(object sender, ImageClickEventArgs e)
    {
        DataList lista = DataList1;
        string fecha = fechaLocal.obtieneFechaLocal().ToString("yyyy-dd-MM");
        int cliente = Convert.ToInt32(lblClientePago.Text);
        int empresa = 0;
        try { empresa = Convert.ToInt32(Session["e"]); }
        catch (Exception) { empresa = 0; Response.Redirect("Default.aspx"); }
        string usuario = "";
        try { usuario = Session["u"].ToString(); }
        catch (Exception) { usuario = ""; Response.Redirect("Default.aspx"); }
        if (empresa != 0)
        {
            int marcados = verificaCambio(lista.Items);
            if (marcados != lista.Items.Count)
            {
                bool validos = verificaValidos(lista.Items);
                if (validos)
                {
                    foreach (DataListItem item in lista.Items)
                    {
                        Label mes = (Label)item.FindControl("clave");
                        CheckBox chk = (CheckBox)item.FindControl("chkPago");
                        TextBox folio = (TextBox)item.FindControl("txtFolioP");
                        string periodoPago = mes.Text;
                        if (chk.Checked && folio.Text != "")
                        {
                            int consecutivo = datos.obtieneIdConsecutivoDetalleCliente(cliente) + 1;
                            bool actualizado = datos.agregaPagMan(periodoPago, fecha, folio.Text, empresa, consecutivo, cliente, usuario);
                            string año = periodoPago.Substring(0, 4);
                            string mess = periodoPago.Substring(4, 2);
                            int cuentaPeriodos = 0;
                            if (actualizado)
                            {
                                cuentaPeriodos = datos.cuentaPeriodosPagosPorAño(cliente, año);
                                bool insertado = false;
                                if (cuentaPeriodos > 0)
                                    insertado = datos.actualizarPeriodosPagos(cliente, "M", año, mess);
                                else if (cuentaPeriodos == 0)
                                    insertado = datos.agregarPeriodosPagos(cliente, "M", año, mess);
                                if (!insertado)
                                    datos.borraDetallePago(empresa, cliente, periodoPago);
                            }
                        }
                    }
                    
                    PanelMask.Visible = false;
                    PanelMeses.Visible = false;
                    cargaDatos();
                }
                else
                    Label7.Text = "Uno o mas peridodos marcados para pago no tienen el folio correspondiente ingresado, verifique";
            }
            else
                Label7.Text = "Debe indicar al menos un periodo para aplicar el pago";
        }        
    }

    private int verificaCambio(DataListItemCollection items)
    {
        int validos = 0;
        foreach (DataListItem item in items)
        {
            Label mes = (Label)item.FindControl("clave");
            CheckBox chk = (CheckBox)item.FindControl("chkPago");
            TextBox folio = (TextBox)item.FindControl("txtFolioP");
            if (!chk.Checked)
                validos++;
        }
        return validos;
    }

    private bool verificaValidos(DataListItemCollection items)
    {
        int validos = 0;
        foreach (DataListItem item in items)
        {
            Label mes = (Label)item.FindControl("clave");
            CheckBox chk = (CheckBox)item.FindControl("chkPago");
            TextBox folio = (TextBox)item.FindControl("txtFolioP");
            if (chk.Checked && folio.Text == "")
                validos++;
        }
        if (validos == 0)
            return true;
        else
            return false;   
    }
        
    protected void btnCancelarPago_Click(object sender, ImageClickEventArgs e)
    {        
        PanelMeses.Visible = false;
        PanelMask.Visible = false;
    }
    
    protected void rdbOpcion_SelectedIndexChanged(object sender, EventArgs e)
    {
        switch (rdbOpcion.SelectedValue) {
            case "0":
                pnlPagos.Visible = true;
                pnlConsultaUsuarios.Visible = pnlConsultaSocios.Visible = false;
                break;
            case "1":
                pnlConsultaUsuarios.Visible = true;
                pnlPagos.Visible = pnlConsultaSocios.Visible = false;
                ddlAños.Items.Clear();
                ddlAños.DataBind();                
                llenaDtUsuarios();
                break;
            case "2":
                pnlConsultaSocios.Visible = true;
                pnlPagos.Visible = pnlConsultaUsuarios.Visible = false;
                PanelDetalle.Visible = false;
                break;
            default:
                pnlPagos.Visible = pnlConsultaUsuarios.Visible = pnlConsultaSocios.Visible = false;
                break;
        }
    }

    private void llenaDtUsuarios()
    {
        int empresa = 0;
        try { empresa = Convert.ToInt32(Session["e"]); }
        catch (Exception)
        {
            Response.Redirect("Default.aspx");
        }
        string periodo_indicado = "1";
        DateTime fecha_inicio;
        DateTime fecha_final;
        Fechas fechas = new Fechas();
        string año = ddlAños.SelectedValue;
        string periodo = ddlPeriodo.SelectedValue;
        if (año == "")
            año = fechas.obtieneFechaLocal().Year.ToString();
        if (periodo == "0")
        {
            fecha_inicio = Convert.ToDateTime("01/" + periodo_indicado.PadLeft(2, '0') + "/" + año);
            fecha_final = fecha_inicio.AddYears(1);
            fecha_final = fecha_final.AddDays(-1);
        }
        else
        {
            fecha_inicio = Convert.ToDateTime("01/" + periodo.PadLeft(2, '0') + "/" + año);
            fecha_final = fecha_inicio.AddMonths(1);
            fecha_final = fecha_final.AddDays(-1);
        }
        SqlDataSourceUsuarios.SelectCommand = "declare @empresa int set @empresa=" + empresa + " SELECT DISTINCT TABLA.USUARIO,CASE TABLA.NOMBRE WHEN '' THEN (CASE TABLA.USUARIO WHEN '' THEN 'SIN USUARIO' ELSE TABLA.USUARIO END) ELSE TABLA.NOMBRE END AS NOMBRE FROM(" +
"select DISTINCT ISNULL(UPPER(C.USUARIO_ALTA), '') AS USUARIO, ISNULL(UPPER(U.NOMBRE), '') AS NOMBRE from clientes C LEFT JOIN USUARIOS U ON U.ID_USUARIO = C.USUARIO_ALTA where C.id_empresa = @empresa AND C.ESTATUS_CLIENTE = 'A' and convert(char(10), c.fecha_inicio, 120) between '" + fecha_inicio.ToString("yyyy-MM-dd") + "' and '" + fecha_final.ToString("yyyy-MM-dd") + "' " +
"UNION ALL select DISTINCT ISNULL(UPPER(C.USUARIO_INACTIVA), '') AS USUARIO, ISNULL(UPPER(U.NOMBRE), '') AS NOMBRE from clientes C LEFT JOIN USUARIOS U ON U.ID_USUARIO = C.USUARIO_INACTIVA where C.id_empresa = @empresa AND C.ESTATUS_CLIENTE = 'I' and convert(char(10), c.fecha_inicio, 120) between '" + fecha_inicio.ToString("yyyy-MM-dd") + "' and '" + fecha_final.ToString("yyyy-MM-dd") + "' " +
"UNION ALL SELECT DISTINCT ISNULL(UPPER(B.USUARIO_BAJA), '') AS USUARIO, ISNULL(UPPER(U.NOMBRE), '') AS NOMBRE FROM BAJAS_INMEDIATAS B LEFT JOIN USUARIOS U ON U.ID_USUARIO = B.USUARIO_BAJA WHERE B.ID_EMPRESA = @empresa  and convert(char(10), b.fecha_baja, 120) between '" + fecha_inicio.ToString("yyyy-MM-dd") + "' and '" + fecha_final.ToString("yyyy-MM-dd") + "' UNION ALL " +
"SELECT '' AS USUARIO, '' AS NOMBRE UNION ALL SELECT UPPER(ID_USUARIO)AS USUARIO, UPPER(NOMBRE) AS NOMBRE FROM USUARIOS ) AS TABLA WHERE TABLA.USUARIO <> 'SUPERVISOR' ORDER BY TABLA.USUARIO";
        dtUsuarios.DataSourceID = "SqlDataSourceUsuarios";
        dtUsuarios.DataBind();
    }

    protected void ddlAños_SelectedIndexChanged(object sender, EventArgs e)
    {
        llenaDtUsuarios();
    }

    protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        llenaDtUsuarios();
    }

    protected void radGridAltaUser_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        llenaDtUsuarios();
    }

    protected void radGridAltaUser_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        llenaDtUsuarios();
    }

    protected void radGridAltaUser_PageSizeChanged(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
    {
        llenaDtUsuarios();
    }

    protected void radGridBajasUser_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        llenaDtUsuarios();
    }

    protected void radGridBajasUser_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        llenaDtUsuarios();
    }

    protected void radGridBajasUser_PageSizeChanged(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
    {
        llenaDtUsuarios();
    }

    protected void radGridBajasInmediatasUser_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        llenaDtUsuarios();
    }

    protected void radGridBajasInmediatasUser_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        llenaDtUsuarios();
    }

    protected void radGridBajasInmediatasUser_PageSizeChanged(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
    {
        llenaDtUsuarios();
    }

    protected void dtUsuarios_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        Fechas fechas = new Fechas();
        Telerik.Web.UI.RadHtmlChart grafica = (Telerik.Web.UI.RadHtmlChart)e.Item.FindControl("DonutChartUsuario") as Telerik.Web.UI.RadHtmlChart;
        Telerik.Web.UI.RadGrid radGridAltaUser = (Telerik.Web.UI.RadGrid)e.Item.FindControl("radGridAltaUser") as Telerik.Web.UI.RadGrid;
        Telerik.Web.UI.RadGrid radGridBajasUser = (Telerik.Web.UI.RadGrid)e.Item.FindControl("radGridBajasUser") as Telerik.Web.UI.RadGrid;
        Telerik.Web.UI.RadGrid radGridBajasInmediatasUser = (Telerik.Web.UI.RadGrid)e.Item.FindControl("radGridBajasInmediatasUser") as Telerik.Web.UI.RadGrid;

        Telerik.Web.UI.RadTabStrip RadTabStrip1 = (Telerik.Web.UI.RadTabStrip)e.Item.FindControl("RadTabStrip1") as Telerik.Web.UI.RadTabStrip;
        Telerik.Web.UI.RadMultiPage RadMultiPage1 = (Telerik.Web.UI.RadMultiPage)e.Item.FindControl("RadMultiPage1") as Telerik.Web.UI.RadMultiPage;

        Label lblIdUsuario = (Label)e.Item.FindControl("lblIdUsuario") as Label;
        string usuario = lblIdUsuario.Text;
        Datos datos = new Datos();
        int empresa = 0;
        try { empresa = Convert.ToInt32(Session["e"]); }
        catch (Exception)
        {
            Response.Redirect("Default.aspx");
        }
        DataTable dtA = new DataTable();
        DataTable dtB = new DataTable();
        DataTable dtBI = new DataTable();

        DateTime fecha_inicio;
        DateTime fecha_final;

        string periodo = ddlPeriodo.SelectedValue;
        Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
        string año = ddlAños.SelectedValue;

        string periodo_indicado = "1";

        if (año == "")
            año = fechas.obtieneFechaLocal().Year.ToString();
        if (periodo == "0")
        {
            fecha_inicio = Convert.ToDateTime("01/" + periodo_indicado.PadLeft(2, '0') + "/" + año);
            fecha_final = fecha_inicio.AddYears(1);
            fecha_final = fecha_final.AddDays(-1);
        }
        else
        {
            fecha_inicio = Convert.ToDateTime("01/" + periodo.PadLeft(2, '0') + "/" + año);
            fecha_final = fecha_inicio.AddMonths(1);
            fecha_final = fecha_final.AddDays(-1);
        }


        dtA = datos.obtieneInformacionAltaUsuarios(usuario, empresa.ToString(), fecha_inicio, fecha_final);
        radGridAltaUser.DataSource = dtA;
        radGridAltaUser.DataBind();

        dtB = datos.obtieneInformacionBajasUsuarios(usuario, empresa.ToString(), fecha_inicio, fecha_final);
        radGridBajasUser.DataSource = dtB;
        radGridBajasUser.DataBind();

        dtBI = datos.obtieneInformacionBajasInmeditasUsuarios(usuario, empresa.ToString(), fecha_inicio, fecha_final);
        radGridBajasInmediatasUser.DataSource = dtBI;
        radGridBajasInmediatasUser.DataBind();

        int altas = dtA.Rows.Count;
        int bajas = dtB.Rows.Count;
        int bajasInm = dtBI.Rows.Count;



        if (altas == 0 && bajas != 0 && bajasInm == 0)
            RadTabStrip1.SelectedIndex = RadMultiPage1.SelectedIndex = 1;
        else if (altas != 0 && bajas == 0 && bajasInm == 0)
            RadTabStrip1.SelectedIndex = RadMultiPage1.SelectedIndex = 0;
        else if (altas == 0 && bajas == 0 && bajasInm != 0)
            RadTabStrip1.SelectedIndex = RadMultiPage1.SelectedIndex = 2;
        else
            RadTabStrip1.SelectedIndex = RadMultiPage1.SelectedIndex = 0;

        int dato = Convert.ToInt32(periodo);


        grafica.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
        grafica.PlotArea.Series.Clear();

        Telerik.Web.UI.DonutSeries serieRec = new Telerik.Web.UI.DonutSeries();
        serieRec.StartAngle = 0;
        serieRec.HoleSize = 0;

        serieRec.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.OutsideEnd;
        serieRec.LabelsAppearance.DataFormatString = "{0}";
        serieRec.LabelsAppearance.Visible = true;

        serieRec.TooltipsAppearance.BackgroundColor = Color.Black;
        serieRec.TooltipsAppearance.Color = Color.White;
        serieRec.TooltipsAppearance.DataFormatString = "{0}";

        Telerik.Web.UI.PieSeriesItem cRec = new Telerik.Web.UI.PieSeriesItem();
        Telerik.Web.UI.PieSeriesItem cPag = new Telerik.Web.UI.PieSeriesItem();
        Telerik.Web.UI.PieSeriesItem cOt = new Telerik.Web.UI.PieSeriesItem();

        cRec.Y = Convert.ToDecimal(bajas.ToString());
        cPag.Y = Convert.ToDecimal(altas.ToString());
        cOt.Y = Convert.ToDecimal(bajasInm.ToString());

        cRec.Exploded = cPag.Exploded = cOt.Exploded = false;
        cRec.BackgroundColor = Color.FromArgb(255, 69, 0);
        cPag.BackgroundColor = Color.FromArgb(50, 205, 50);
        cOt.BackgroundColor = Color.FromArgb(135, 206, 250);
        cRec.Name = "Bajas";
        cPag.Name = "Altas";
        cOt.Name = "Bajas Inmediatas";

        serieRec.SeriesItems.Add(cRec);
        serieRec.SeriesItems.Add(cPag);
        serieRec.SeriesItems.Add(cOt);

        grafica.PlotArea.Series.Add(serieRec);

    }



    protected void RadGrid1_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblidCliente.Text = RadGrid1.SelectedValues["id_cliente"].ToString();       
        PanelDetalle.Visible = true;        
        FormView1.DataBind();
        RadGrid2.DataBind();
    }

    private void generaGrafica(string cliente, string año, string empresa)
    {

        DonutChart.Legend.Appearance.Position = Telerik.Web.UI.HtmlChart.ChartLegendPosition.Bottom;
        DonutChart.PlotArea.Series.Clear();

        Telerik.Web.UI.DonutSeries serieRec = new Telerik.Web.UI.DonutSeries();
        serieRec.StartAngle = 0;
        serieRec.HoleSize = 0;

        serieRec.LabelsAppearance.Position = Telerik.Web.UI.HtmlChart.PieAndDonutLabelsPosition.OutsideEnd;
        serieRec.LabelsAppearance.DataFormatString = "{0:F2} %";
        serieRec.LabelsAppearance.Visible = true;

        serieRec.TooltipsAppearance.BackgroundColor = Color.Black;
        serieRec.TooltipsAppearance.Color = Color.White;
        serieRec.TooltipsAppearance.DataFormatString = "{0:F2} %";

        Telerik.Web.UI.PieSeriesItem cRec = new Telerik.Web.UI.PieSeriesItem();
        Telerik.Web.UI.PieSeriesItem cPag = new Telerik.Web.UI.PieSeriesItem();
        Telerik.Web.UI.PieSeriesItem cOt = new Telerik.Web.UI.PieSeriesItem();

        
        decimal pagados = 0, rechazados = 0, sucursal = 0;
        try
        {
            
            DataSet info = datos.obtienePorcentajes(cliente, empresa, año);
            foreach (DataRow r in info.Tables[0].Rows)
            {
                pagados = (Convert.ToDecimal(r[0]) / Convert.ToDecimal(r[3])) * 100;
                rechazados = (Convert.ToDecimal(r[1]) / Convert.ToDecimal(r[3])) * 100;
                sucursal = (Convert.ToDecimal(r[2]) / Convert.ToDecimal(r[3])) * 100;
            }
           
        }
        catch (Exception ex) { pagados = rechazados = sucursal = 0; }

        cRec.Y = Convert.ToDecimal(rechazados.ToString());
        cPag.Y = Convert.ToDecimal(pagados.ToString());
        cOt.Y = Convert.ToDecimal(sucursal.ToString());

        cRec.Exploded = cPag.Exploded = cOt.Exploded = false;
        cRec.BackgroundColor = Color.FromArgb(255, 69, 0);
        cPag.BackgroundColor = Color.FromArgb(50, 205, 50);
        cOt.BackgroundColor = Color.FromArgb(135, 206, 250);
        cRec.Name = "Pagado";
        cPag.Name = "Rechazado";
        cOt.Name = "Pago Sucursal";

        serieRec.SeriesItems.Add(cRec);
        serieRec.SeriesItems.Add(cPag);
        serieRec.SeriesItems.Add(cOt);

        DonutChart.PlotArea.Series.Add(serieRec);
    }

    protected void RadGrid2_SelectedIndexChanged(object sender, EventArgs e)
    {
        RadGrid3.DataBind();
        int empresa = 0;
        try { empresa = Convert.ToInt32(Session["e"]); }
        catch (Exception)
        {
            Response.Redirect("Default.aspx");
        }
        generaGrafica(lblidCliente.Text, RadGrid2.SelectedValue.ToString(), empresa.ToString());
    }

    protected void RadGrid2_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item.ItemType == Telerik.Web.UI.GridItemType.Item || e.Item.ItemType == Telerik.Web.UI.GridItemType.AlternatingItem) {
            DataRowView dv = (DataRowView)e.Item.DataItem;            
            DataRow r = dv.Row;
            for(int i = 2; i < 14; i++)
            {
                string status = r[i].ToString();
                switch (status) {
                    case "P":
                        e.Item.Cells[i+2].BackColor = Color.Green;
                        e.Item.Cells[i+2].ForeColor = Color.Green;
                        break;
                    case "R":
                        e.Item.Cells[i+2].BackColor = Color.OrangeRed;
                        e.Item.Cells[i+2].ForeColor = Color.OrangeRed;
                        break;
                    case "C":
                        e.Item.Cells[i+2].BackColor = Color.Orange;
                        e.Item.Cells[i+2].ForeColor = Color.Orange;
                        break;
                    case "M":
                        e.Item.Cells[i+2].BackColor = Color.LightSkyBlue;
                        e.Item.Cells[i+2].ForeColor = Color.LightSkyBlue;
                        break;
                    default:
                        if (Convert.ToInt32(r[1].ToString()) == fechas.obtieneFechaLocal().Year)
                        {
                            e.Item.Cells[i + 2].BackColor = Color.White;
                            e.Item.Cells[i + 2].ForeColor = Color.White;
                        }
                        else {
                            e.Item.Cells[i + 2].BackColor = Color.LightGray;
                            e.Item.Cells[i + 2].ForeColor = Color.LightGray;
                        }
                        break;
                }
            }
        }
    }
}