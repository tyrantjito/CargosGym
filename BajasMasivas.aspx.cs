using E_Utilities;
using PdfViewer1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using System.Globalization;


public partial class BajasMasivas : System.Web.UI.Page
{
    Fechas fechas = new Fechas();
    Datos datos = new Datos();
    ObtieneFechaActual fechaLocal = new ObtieneFechaActual();
    bool[] altaCliente = new bool[20];
    string Clientes = "";
    string Referencias = "";
    bool tienePermiso;
    byte[] imagen;
    string extension;
    int cont = 0;
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


        /*   string url = Request.Url.ToString();


           if (url.Contains("?"))
           {

               url = url.Split(new Char[] { '?' })[0];

           }

           if (aspx.Contains("Clientes1.aspx"))
           {
               Session["C"] = cont + 1;
               if (Convert.ToInt32(Session["C"]) > 2)
               {
                   Session["C"] = 1;
                   Response.Redirect("Default.aspx");

               }
           }*/

        if (!IsPostBack)
        {
            llenaAñoDdl();
            cargaDatos();
            PanelImgZoom.Visible = false;
        }
    }

    private bool verificaVigencia(string valor)
    {
        bool valido = false;
        string[] vigen = valor.Split(new char[] { '/' });
        int mes, año;
        try { mes = Convert.ToInt32(vigen[0]); }
        catch (Exception) { mes = 0; }
        try { año = Convert.ToInt32(vigen[1]); }
        catch (Exception) { año = 0; }
        if (mes == 0 || año == 0)
            valido = false;
        else
        {
            string periodoVigencia = "2" + año.ToString().PadLeft(3, '0') + "-" + mes.ToString().PadLeft(2, '0');
            DateTime fechaVigencia = Convert.ToDateTime(periodoVigencia);
            DateTime fechaActual = Convert.ToDateTime(fechas.obtieneFechaLocal().ToString("yy") + "-" + fechas.obtieneFechaLocal().ToString("MM"));
            if (fechaVigencia <= fechaActual)
                valido = false;
            else
                valido = true;
        }
        Session["C"] = 1;
        return valido;
    }

    public void llenaAñoDdl()
    {
        int idEmpresa = 0;
        try { idEmpresa = Convert.ToInt32(Session["e"]); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        DataSet data = new DataSet();
        try { data = datos.llenaAñoClien(idEmpresa); }
        catch (Exception) { data = null; }
        this.ddlAño.DataSource = data;
        this.ddlAño.DataValueField = "ano";
        this.ddlAño.DataTextField = "anio";
        this.ddlAño.DataBind();
        Session["C"] = 1;
    }
    private void cargaDatos()
    {
        lblError.Text = "";
        int empresa;
        string usuario = "";
        try
        {
            empresa = Convert.ToInt32(Session["e"]);
            usuario = Session["u"].ToString();
        }
        catch (Exception)
        {
            empresa = 0;
            Response.Redirect("Default.aspx");
        }



        altaCliente = datos.obtienePermisos(usuario);
        tienePermiso = altaCliente[18];
        string condicion = "";
        string nombreCliente = "C";
        string nombreCompleto = "";
        if (nombreCliente == "T")
            nombreCompleto = "(c.nombre_tarjetahabiente+' '+c.ap_pat_tarjetahabiente+' '+c.ap_mat_tarjetahabiente) as nombre_completo";
        else
            nombreCompleto = "(c.nombre+' '+c.apellido_paterno+' '+c.apellido_materno) as nombre_completo";

        int op = Convert.ToInt32(ddlBuscar.SelectedValue);
        switch (op)
        {
            case 1:
                condicion = " c.referencia like '%" + txtBuscar.Text.Trim() + "%'";
                break;
            case 2:
                if (nombreCliente == "T")
                    condicion = " c.nombre_tarjetahabiente like '%" + txtBuscar.Text.Trim() + "%' or  c.ap_pat_tarjetahabiente like '%" + txtBuscar.Text.Trim() + "%' or  c.ap_mat_tarjetahabiente like '%" + txtBuscar.Text.Trim() + "%'  or (c.nombre_tarjetahabiente+' '+c.ap_pat_tarjetahabiente+' '+c.ap_mat_tarjetahabiente) like '%" + txtBuscar.Text + "%' ";
                else
                    condicion = " c.nombre like '%" + txtBuscar.Text.Trim() + "%' or  c.apellido_paterno like '%" + txtBuscar.Text.Trim() + "%' or  c.apellido_materno like '%" + txtBuscar.Text.Trim() + "%' or (c.nombre+' '+c.apellido_paterno+' '+c.apellido_materno) like '%" + txtBuscar.Text + "%' ";
                break;
            case 3:
                condicion = " b.nombre like '%" + txtBuscar.Text.Trim() + "%'";
                break;
            case 4:
                condicion = " c.no_cuenta like'%" + txtBuscar.Text.Trim() + "%'";
                break;
            default:
                if (nombreCliente == "T")
                    condicion = " c.referencia like '%" + txtBuscar.Text.Trim() + "%' or c.nombre_tarjetahabiente like '%" + txtBuscar.Text.Trim() + "%' or  c.ap_pat_tarjetahabiente like '%" + txtBuscar.Text.Trim() + "%' or  c.ap_mat_tarjetahabiente like '%" + txtBuscar.Text.Trim() + "%' or b.nombre like '%" + txtBuscar.Text.Trim() + "%' or c.no_cuenta like'%" + txtBuscar.Text.Trim() + "%' or (c.nombre_tarjetahabiente+' '+c.ap_pat_tarjetahabiente+' '+c.ap_mat_tarjetahabiente) like '%" + txtBuscar.Text + "%' ";
                else
                    condicion = " c.referencia like '%" + txtBuscar.Text.Trim() + "%' or c.nombre like '%" + txtBuscar.Text.Trim() + "%' or  c.apellido_paterno like '%" + txtBuscar.Text.Trim() + "%' or  c.apellido_materno like '%" + txtBuscar.Text.Trim() + "%' or b.nombre like '%" + txtBuscar.Text.Trim() + "%' or c.no_cuenta like'%" + txtBuscar.Text.Trim() + "%' or (c.nombre+' '+c.apellido_paterno+' '+c.apellido_materno) like '%" + txtBuscar.Text + "%' ";
                break;
        }
        DataSet ds = datos.obtieneClientes(empresa, "M", txtBuscar.Text, ddlEstatus.SelectedValue, condicion, nombreCompleto, ddlAño.SelectedValue);
        RadGrid1.DataSource = ds;
        //GridView1.DataSource = ds;
        try
        {
            //GridView1.DataBind();
            RadGrid1.DataBind();
        }
        catch (Exception)
        {
            RadGrid1.DataSource = null;
            RadGrid1.DataBind();
            //GridView1.DataSource = null;
            //GridView1.DataBind();
        }
        lblClientesActivos.Text = datos.obtieneClientesActivosInactivos("A", empresa);
        lblClientesInactivos.Text = datos.obtieneClientesActivosInactivos("I", empresa);
        Session["C"] = 1;
    }

    private bool validaFechas(DateTime fechaInicial, DateTime fechaFinal)
    {
        if (fechaFinal < fechaInicial)
            return false;
        else if (fechaInicial > fechaFinal)
            return false;
        else if (fechaFinal.ToString("yyyy-MM-dd") == fechaInicial.ToString("yyyy-MM-dd"))
            return false;
        else if (fechaFinal < fechaInicial.AddMonths(1))
            return false;
        else if (fechaFinal < fechaLocal.obtieneFechaLocal())
            return false;
        else
            return true;
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string usuario = DataBinder.Eval(e.Row.DataItem, "id_cliente").ToString();
            string fechaIngreso = DataBinder.Eval(e.Row.DataItem, "fecha_ini").ToString();
            string vigencia = DataBinder.Eval(e.Row.DataItem, "vigencia_cuenta").ToString();
            int noReconocidos = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "noReconocido").ToString());
            int mesBaja = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "mesBaja").ToString());
            int anoBaja = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "anoBaja").ToString());
            string argumentos = DataBinder.Eval(e.Row.DataItem, "extra").ToString();

            int empresa = 0;
            try { empresa = Convert.ToInt32(Session["e"]); }
            catch (Exception) { Response.Redirect("Default.aspx"); }
            var botonEliminar = e.Row.Cells[6].Controls[1].FindControl("lknEliminar") as ImageButton;
            var botonActiva = e.Row.Cells[7].Controls[1].FindControl("lknActiva") as ImageButton;
            var botonInactiva = e.Row.Cells[7].Controls[1].FindControl("lknInactiva") as ImageButton;
            var botonEditar = e.Row.Cells[8].Controls[1].FindControl("lknEditar") as ImageButton;
            var p1 = e.Row.Cells[9].Controls[1].FindControl("P1") as Label;
            var p2 = e.Row.Cells[10].Controls[1].FindControl("P2") as Label;
            var p3 = e.Row.Cells[11].Controls[1].FindControl("P3") as Label;
            var p4 = e.Row.Cells[12].Controls[1].FindControl("P4") as Label;
            var p5 = e.Row.Cells[13].Controls[1].FindControl("P5") as Label;
            var p6 = e.Row.Cells[14].Controls[1].FindControl("P6") as Label;
            var p7 = e.Row.Cells[15].Controls[1].FindControl("P7") as Label;
            var p8 = e.Row.Cells[16].Controls[1].FindControl("P8") as Label;
            var p9 = e.Row.Cells[17].Controls[1].FindControl("P9") as Label;
            var p10 = e.Row.Cells[18].Controls[1].FindControl("P10") as Label;
            var p11 = e.Row.Cells[19].Controls[1].FindControl("P11") as Label;
            var p12 = e.Row.Cells[20].Controls[1].FindControl("P12") as Label;
            var lblClienteGrid = e.Row.Cells[2].Controls[1].FindControl("Label10") as Label;

            var imgComentario = e.Row.Cells[1].Controls[1].FindControl("imgComentario") as System.Web.UI.WebControls.Image;
            var imgInformacion = e.Row.Cells[1].Controls[1].FindControl("imgInformacion") as System.Web.UI.WebControls.Image;
            var imgDocumentos = e.Row.Cells[1].Controls[1].FindControl("imgDocumentos") as System.Web.UI.WebControls.Image;


            string[] valoresFaltantes = argumentos.Split(new char[] { ';' });

            if (Convert.ToInt32(valoresFaltantes[0].Trim()) > 0)
                imgComentario.Visible = true;
            else
                imgComentario.Visible = false;

            if (Convert.ToInt32(valoresFaltantes[1].Trim()) > 0)
                imgInformacion.Visible = true;
            else
                imgInformacion.Visible = false;

            if (Convert.ToInt32(valoresFaltantes[2].Trim()) == 0)
                imgDocumentos.Visible = true;
            else
                imgDocumentos.Visible = false;


            if (noReconocidos > 0)
                lblClienteGrid.BackColor = Color.Yellow;


            string[] periodos = new string[12];
            int periodoRegistro = 1;
            for (int valores = 0; valores < 12; valores++)
            {
                string periodo = "p" + periodoRegistro.ToString();
                periodos[valores] = DataBinder.Eval(e.Row.DataItem, periodo).ToString();
                periodoRegistro++;
            }

            if (e.Row.RowState.ToString() == "Normal" || e.Row.RowState.ToString() == "Alternate")
            {
                if (e.Row.RowState.ToString() == "Alternate")
                    e.Row.BackColor = Color.WhiteSmoke;
                else
                    e.Row.BackColor = Color.White;

                string status = DataBinder.Eval(e.Row.DataItem, "estatus_cliente").ToString();

                string[] pagos = null;
                Color[] colores = null;
                Label[] elementos = { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12 };
                for (int i = 0; i < 12; i++)
                {
                    elementos[0].Visible = true;
                }


                DataSet ultimoMov = new DataSet();

                pagos = new string[12] { "E", "F", "M", "A", "M", "J", "J", "A", "S", "O", "N", "D" };
                colores = new Color[12];
                for (int i = 0; i < 12; i++)
                {
                    colores[i] = defineColor(periodos[i]);
                }

                string usuarioLog = "";
                try { usuarioLog = Session["u"].ToString(); }
                catch (Exception) { Response.Redirect("Default.aspx"); }
                bool permisoAlta = altaCliente[18];
                if (status == "I")
                {
                    e.Row.ForeColor = Color.Red;
                    if (permisoAlta)
                        botonEliminar.Visible = false;
                    else
                        botonEliminar.Visible = true;

                    bool permiteInactivar = altaCliente[14];
                    if (permiteInactivar)
                        botonActiva.Visible = true;
                    else
                        botonActiva.Visible = false;
                    botonInactiva.Visible = false;
                    botonEditar.Visible = false;
                }
                else
                {
                    e.Row.ForeColor = Color.Black;
                    botonEliminar.Visible = false;
                    botonActiva.Visible = false;
                    bool permiteInactivar = altaCliente[14];
                    if (permiteInactivar)
                        botonInactiva.Visible = true;
                    else
                        botonInactiva.Visible = false;
                    if (!permisoAlta)
                        botonEditar.Visible = false;
                    else
                        botonEditar.Visible = true;
                }


                int contador = 0;

                DateTime fechaPeriodo, fechaIngresoD;
                fechaIngresoD = Convert.ToDateTime("01/" + fechaIngreso.Substring(5, 2).PadLeft(2, '0') + "/" + fechaIngreso.Substring(0, 4));

                if (vigencia.Trim() != "")
                {
                    int diferencia = obtieneMesesdeVigencia(vigencia);
                    Label banco = (Label)e.Row.FindControl("Label19");

                    if (diferencia <= 2 && diferencia >= 1)
                    {
                        banco.BackColor = Color.Yellow;
                        if (status == "I")
                            banco.ForeColor = Color.Black;
                    }
                    else if (diferencia < 1)
                    {
                        banco.BackColor = Color.Red;
                        if (status == "I")
                            banco.ForeColor = Color.White;
                    }
                    else
                        banco.BackColor = Color.Transparent;

                }
                foreach (Label etiqueta in elementos)
                {
                    etiqueta.Text = pagos[contador];
                    try
                    {
                        fechaPeriodo = Convert.ToDateTime("01/" + (contador + 1).ToString().PadLeft(2, '0') + "/" + ddlAño.SelectedValue);
                    }
                    catch (Exception)
                    {
                        fechaPeriodo = Convert.ToDateTime("01/" + (contador + 1).ToString().PadLeft(2, '0') + "/" + fechaLocal.obtieneFechaLocal().Year.ToString());
                    }
                    if (fechaPeriodo >= fechaIngresoD)
                    {
                        if (status == "I")
                        {
                            if (anoBaja.ToString() == ddlAño.SelectedValue)
                                if (contador + 1 > mesBaja)
                                    etiqueta.Visible = false;
                                else
                                    etiqueta.Visible = true;
                            else if (anoBaja < Convert.ToInt32(ddlAño.SelectedValue))
                                etiqueta.Visible = false;
                            else
                                etiqueta.Visible = true;
                        }
                        else
                            etiqueta.Visible = true;
                        //etiqueta.ToolTip = obtieneDetalle(usuario, empresa, fechaPeriodo.ToString("yyyyMM"));
                    }
                    else
                    {
                        etiqueta.Visible = false;
                    }
                    etiqueta.BackColor = colores[contador];
                    contador++;
                }

                Label lblcuenta = (Label)e.Row.FindControl("Label16");
                lblcuenta.Text = lblcuenta.Text.Substring(12, 4).PadLeft(16, '*');


            }
        }
        Session["C"] = 1;
    }

    private string obtieneDetalle(string usuario, int empresa, string periodo)
    {
        return datos.obtieneinfoDetalleCliente(usuario, empresa, periodo);
        Session["C"] = 1;
    }

    private int obtieneMesesdeVigencia(string vigencia)
    {
        int año, mes, mesV, añoV, diferencia = 0;
        DateTime fechaVigencia = Convert.ToDateTime("01/" + vigencia.Substring(0, 2).PadLeft(2, '0') + "/" + vigencia.Substring(3, 2));
        DateTime fechaActual = Convert.ToDateTime("01/" + fechaLocal.obtieneFechaLocal().Month.ToString().PadLeft(2, '0') + "/" + fechaLocal.obtieneFechaLocal().Year.ToString());
        mesV = fechaVigencia.Month;
        añoV = fechaVigencia.Year;
        año = fechaLocal.obtieneFechaLocal().Year;
        mes = fechaLocal.obtieneFechaLocal().Month;
        TimeSpan s = fechaVigencia - fechaActual;

        diferencia = s.Days / 30;
        Session["C"] = 1;
        return diferencia;
    }

    private Color defineColor(string valor)
    {
        if (valor == "P")
            return Color.LimeGreen;
        else if (valor == "R")
            return Color.Red;
        else if (valor == "M")
            return Color.LightSkyBlue;
        else if (valor == "C")
            return Color.Yellow;
        else if (valor == "A")
            return Color.DarkOrange;
        else
            return Color.Transparent;

    }

    protected void rbtnPeriodicidad_SelectedIndexChanged(object sender, EventArgs e)
    {
        Panel4.Visible = false;
        Panel6.Visible = false;
        Session["C"] = 1;
        cargaDatos();
    }

    protected void lknEliminar_Click(object sender, EventArgs e)
    {
        ImageButton lknInactiva = (ImageButton)sender;
        string cliente = lknInactiva.CommandArgument;
        lblClienteInactiva.Text = cliente;
        lblErrorBaja.Text = lblErrorAutenticacion.Text = txtUsuario.Text = txtContraseñaLog.Text = txtFechaBaja.Text = txtMotivoBaja.Text = "";
        chkBajaInmediata.Checked = true;
        chkBajaInmediata.Enabled = false;

        string script1 = "cierraWinAut()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "CierraAutorizacion", script1, true);
        Session["C"] = 1;
    }

    protected void lknActiva_Click(object sender, EventArgs e)
    {
        ImageButton lknActiva = (ImageButton)sender;
        string cliente = lknActiva.CommandArgument;
        lblCliente.Text = cliente;
        btnAutentica.CommandArgument = "3";
        lblErrorAutenticacion.Text = "";
        txtUsuario.Text = "";
        txtContraseñaLog.Text = "";
        string script1 = "abreWinAut()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "Autorizacion", script1, true);
        Session["C"] = 1;
    }

    protected void lknInactiva_Click(object sender, EventArgs e)
    {
        foreach (GridDataItem item in RadGrid1.Items)
        {
            CheckBox inaCheck = (CheckBox)item.FindControl("ina");
            if (inaCheck.Checked)
            {
                Label cliente = (Label)item.FindControl("Label7");
                string cliente2 = cliente.Text;
                if (Clientes == "")
                {
                    Clientes = Clientes + cliente2;
                }
                else
                {
                    Clientes = Clientes + "," + cliente2;
                }
            }
        }
        lblClienteInactiva.Text = Clientes;
        lblErrorBaja.Text = lblErrorAutenticacion.Text = txtUsuario.Text = txtContraseñaLog.Text = txtFechaBaja.Text = txtMotivoBaja.Text = "";
        chkBajaInmediata.Checked = false;
        chkBajaInmediata.Enabled = true;
        txtFechaBaja.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
        int año = Convert.ToInt32(ddlAño.SelectedValue);
        int periodo = fechas.obtieneFechaLocal().Month;
        /*bool darBaja = obtieneSiPuedeDarBaja(año, periodo, 1, Convert.ToInt32(Clientes));
        if (!darBaja)
        {
            lblErrorBaja.Text = "El cliente aun cuenta con pagos atrasados y no es posible darlo de baja";
            txtFechaBaja.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
            btnBaja.Visible = false;
        }
        else {
            lblErrorBaja.Text = "";
            btnBaja.Visible = true;
        }*/

        string script1 = "abreWinBaj()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BajaCliente", script1, true);
        Session["C"] = 1;
    }

    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        Session["C"] = 1;
        cargaDatos();
    }

    protected void txtBuscar_TextChanged(object sender, EventArgs e)
    {
        Session["C"] = 1;
        cargaDatos();
    }

    protected void btnReferencia_Click(object sender, EventArgs e)
    {
        LinkButton lknReferencia = (LinkButton)sender;
        string[] argumentos = lknReferencia.CommandArgument.ToString().Split(new char[] { ';' });
        lblCliente.Text = argumentos[0];
        lblRefernciaCliente.Text = argumentos[1];
        lblClienteNombre.Text = datos.obtieneNombreCliente(argumentos[0], "C");

        string[] usuFAlta = datos.obtieneNombreUsuarioAlta(argumentos[0]).Split(';');
        lblUsuarioAlta.Text = "Usuario Alta: " + usuFAlta[0];
        lblFechaInicio.Text = "Fecha Inicio: " + usuFAlta[1];

        obtieneSDetalle(argumentos[0]);
        int empresa;
        try
        {
            empresa = Convert.ToInt32(Session["e"]);
        }
        catch (Exception)
        {
            empresa = 0;
            Response.Redirect("Default.aspx");
        }
        DataTable data = llenaDataList(Convert.ToInt32(argumentos[0]), empresa);
        DataList1.DataSource = data;
        DataList1.DataBind();

        if (DataList1.Items.Count > 0)
        {
            btnAgregarPagoM.Visible = true;
            lblErrorSucursal.Text = "";
        }
        else
        {
            btnAgregarPagoM.Visible = false;
            lblErrorSucursal.Text = "No existe información de pagos atrasados";
        }

        GridView3.DataBind();
        lblErroresComentarios.Text = "";
        RadTabStrip1.SelectedIndex = 0;
        RadMultiPage1.SelectedIndex = 0;
        string script1 = "abreWinDet()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "DetallesCliente", script1, true);
        Session["C"] = 1;
    }

    private DataTable llenaDataList(int cliente, int empresa)
    {
        DataTable datSet = new DataTable();
        Fechas fechas = new Fechas();
        string periodo = ddlAño.SelectedValue + fechas.obtieneFechaLocal().Month.ToString().PadLeft(2, '0');
        int año = Convert.ToInt32(ddlAño.SelectedValue);
        datSet = datos.obtienePagosRetrasados(empresa, cliente, año, periodo);
        Session["C"] = 1;
        return datSet;
    }

    protected void btnAgregarPagoM_Click(object sender, ImageClickEventArgs e)
    {
        DataList lista = DataList1;
        string fecha = fechaLocal.obtieneFechaLocal().ToString("yyyy-MM-dd");
        int cliente = Convert.ToInt32(lblCliente.Text);
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
                    string strPeriodos = string.Empty;
                    int numPerPaga = 0;
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
                            strPeriodos = strPeriodos + Imprime_Ticket.obtenerNombreMes(int.Parse(mess)) + " " + año + ". \n\r";
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
                            numPerPaga++;
                        }
                    }
                    DataTable data = llenaDataList(cliente, empresa);
                    DataList1.DataSource = data;
                    DataList1.DataBind();
                    obtieneSDetalle(cliente.ToString());

                    string referencia = datos.obtieneReferencia(cliente.ToString());
                    Ejecucion ejec = new Ejecucion();
                    string strQry = "SELECT monto FROM clientes WHERE id_cliente=" + cliente + " AND id_empresa=" + empresa;
                    decimal monto = decimal.Parse(ejec.scalarString(strQry));
                    decimal total = numPerPaga * monto;
                    Imprime_Ticket impTicket = new Imprime_Ticket();
                    bool genraTicket = impTicket.generarTicketBD(empresa, usuario, cliente, "M", total, strPeriodos, "");
                    GenerarTicket("M", monto, numPerPaga, null, null, cliente, referencia, strPeriodos);
                    cargaDatos();
                }
                else
                    lblError.Text = "Uno o mas peridodos marcados para pago no tienen el folio correspondiente ingresado, verifique";
            }
            else
                lblError.Text = "Debe indicar al menos un periodo para aplicar el pago";
        }
        Session["C"] = 1;
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
        Session["C"] = 1;
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

    private void obtieneSDetalle(string cliente)
    {
        int empresa;
        try
        {
            empresa = Convert.ToInt32(Session["e"]);
        }
        catch (Exception)
        {
            empresa = 0;
        }

        string usuario = "";
        try { usuario = Session["u"].ToString(); }
        catch (Exception) { Response.Redirect("Default.aspx"); }

        DataSet valoresDetalle = datos.obtieneDetalleCliente(cliente, empresa);
        if (valoresDetalle.Tables.Count != 0)
        {
            GridView2.DataSource = valoresDetalle;
            bool[] permisos = datos.obtienePermisos(usuario);
            if (permisos[21])
                GridView2.Columns[6].Visible = GridView2.Columns[7].Visible = true;
            else
                GridView2.Columns[6].Visible = GridView2.Columns[7].Visible = false;
            GridView2.DataBind();
            ///RadGrid2.DataBind();
        }
        Session["C"] = 1;
    }

    protected void btnClose_Click(object sender, ImageClickEventArgs e)
    {
        Panel4.Visible = false;
        Panel6.Visible = false;
        Session["C"] = 1;
    }

    protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        ddlBuscar.SelectedValue = "0";
        txtBuscar.Text = "";
        Session["C"] = 1;
        cargaDatos();
    }

    protected void btnCancelar_Click(object sender, ImageClickEventArgs e)
    {
        Panel6.Visible = false;
        Panel5.Visible = false;
        txtFechaBaja.Text = "";
        txtMotivoBaja.Text = "";
        lblErrorBaja.Text = "";
        Session["C"] = 1;
    }

    protected void btnBaja_Click(object sender, ImageClickEventArgs e)
    {
        foreach (GridDataItem item in RadGrid1.Items)
        {
            CheckBox inaCheck = (CheckBox)item.FindControl("ina");
            if (inaCheck.Checked)
            {
                Label cliente = (Label)item.FindControl("Label7");
                string cliente2 = cliente.Text;
                if (Clientes == "")
                {
                    Clientes = Clientes + cliente2;
                }
                else
                {
                    Clientes = Clientes + "," + cliente2;
                }
            }
        }
        string[] arg = Clientes.ToString().Split(new char[] { ',' });
        lblErrorBaja.Text = "";
        for (int i = 0; i < arg.Length; i++)
        {
            lblClienteInactiva.Text = arg[i];
            string referencia = datos.obtieneReferencia(lblClienteInactiva.Text);
            DateTime fechaBaja, fechaActual;
            int empresa = 0;
            fechaActual = Convert.ToDateTime(fechaLocal.obtieneFechaLocal().ToShortDateString());
            try { fechaBaja = Convert.ToDateTime(txtFechaBaja.Text + " 00:00:00"); }
            catch (Exception) { fechaBaja = Convert.ToDateTime("1900-01-01"); }
            if (fechaBaja.DayOfYear <= fechaActual.DayOfYear)
            {
                if (fechaBaja.ToString("yyyy-MM-dd") == "1900-01-01")
                    lblErrorBaja.Text = "La fecha de baja no es una fecha válida.";
                else if (fechaBaja.DayOfYear > fechaLocal.obtieneFechaLocal().DayOfYear)
                    lblErrorBaja.Text = "La Fecha de Baja no pueder ser mayor a la fecha actual";
                else
                {
                    string usuarioBaja = "";
                    try { empresa = Convert.ToInt32(Session["e"]); usuarioBaja = Session["u"].ToString(); }
                    catch (Exception) { Response.Redirect("Default.aspx"); }
                   
                    if (chkBajaInmediata.Checked)
                    {
                        cargaDatos();

                        int año = Convert.ToInt32(ddlAño.SelectedValue);
                        int periodo = fechas.obtieneFechaLocal().Month;
                        if (!chkBajaInmediata.Checked)
                        {
                            bool darBaja = obtieneSiPuedeDarBaja(año, periodo, 1, Convert.ToInt32(Clientes));
                            if (!darBaja)
                                lblErrorBaja.Text = "El cliente aun cuenta con pagos atrasados y no es posible darlo de baja";
                        }
                        else
                        {
                            btnAutentica.CommandArgument = "1";
                            lblErrorAutenticacion.Text = "";
                            txtUsuario.Text = "";
                            txtContraseñaLog.Text = "";
                            string script1 = "cierraWinBaj()";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "CierraBaja", script1, true);
                            script1 = "abreWinAut()";
                            ScriptManager.RegisterStartupScript(this, typeof(Page), "Autorizacion", script1, true);
                        }
                    }
                    else
                    {
                        btnAutentica.CommandArgument = "2";
                        lblErrorAutenticacion.Text = "";
                        txtUsuario.Text = "";
                        txtContraseñaLog.Text = "";
                        string script1 = "cierraWinBaj()";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "CierraBaja", script1, true);
                        script1 = "abreWinAut()";
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "Autorizacion", script1, true);
                    }

                }
            }
            else
                lblErrorBaja.Text = "La fecha de baja no puede ser mayor a la fecha actual.";
        }
        Session["C"] = 1;
    }

    private bool obtieneSiPuedeDarBaja(int año, int periodo, int opcion, int cliente)
    {
        int mesesAtrasados = 0;
        int contador = 1;

        if (opcion == 1)
            contador = 2;
        else
            contador = 1;


        if (periodo == 1)
        {
            periodo = 12;
            año = año - 1;
            for (int i = 0; i < contador; i++)
            {
                if (periodo == 13)
                {
                    periodo = 1;
                    año = Convert.ToInt32(ddlAño.SelectedValue);
                }
                mesesAtrasados = mesesAtrasados + datos.obtienePeriodos(año, periodo, cliente);
                periodo++;
            }
        }
        else {
            if (opcion == 1)
                periodo = periodo - 1;

            for (int i = 0; i < contador; i++)
            {
                if (periodo == 13)
                {
                    periodo = 1;
                    año = Convert.ToInt32(ddlAño.SelectedValue);
                }
                mesesAtrasados = mesesAtrasados + datos.obtienePeriodos(año, periodo, cliente);
                periodo++;
            }
        }

        if (mesesAtrasados == 0)
            return true;
        else
            return false;

    }

   


    protected void rblCliente_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["C"] = 1;
        cargaDatos();
    }

   

    

    protected void lnkLimpiar_Click(object sender, EventArgs e)
    {
        ddlBuscar.SelectedValue = "0";
        txtBuscar.Text = "";
        Session["C"] = 1;
        cargaDatos();
    }

    protected void btnAutentica_Click(object sender, ImageClickEventArgs e)
    {
        btnCancAut.CommandArgument = btnAutentica.CommandArgument;

        try
        {
            string ruta = Server.MapPath("~/TMP");

            // Si el directorio no existe, crearlo
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);

            bool archivoValido = false;
            bool existeUsuario = datos.existeUsuario(txtUsuario.Text);
            if (existeUsuario)
            {
                bool valido = datos.verificaContraseña(txtUsuario.Text, txtContraseñaLog.Text);
                if (valido)
                {
                    switch (btnAutentica.CommandArgument.ToString())
                    {
                        case "0":
                            // Alta
                            break;
                        case "1":
                            // Inactiva
                            baja1(archivoValido, ruta);
                            break;
                        case "2":
                            //Baja Inmediata
                            baja2();
                            break;
                        case "3":
                            // Reactiva
                            reactiva();
                            break;
                    }


                    string script1 = "cierraWinAut()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "CierraAutorizacion", script1, true);
                }
                else
                    lblErrorAutenticacion.Text = "El usuario o contraseña incorrectos, por favor verifique";
            }
            else
                lblErrorAutenticacion.Text = "El usuario no cuenta con los privilegios para autorizar la baja";
        }
        catch (Exception ex)
        {
            lblErrorAutenticacion.Text = "Error:" + ex.Message;
        }
        Session["C"] = 1;
    }

    private void reactiva()
    {
        string usuarioAtiva = "";
        int empresa = 0;
        try { usuarioAtiva = Session["u"].ToString(); empresa = Convert.ToInt32(Session["e"]); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        bool activado = datos.inactivaCliente(lblCliente.Text, "A", "1900-01-01", "", "", null, empresa, usuarioAtiva, "", txtUsuario.Text);
        if (activado)
        {
            string fecha = fechaLocal.obtieneFechaLocal().ToString("yyyy-MM-dd");
            int cliente = Convert.ToInt32(lblCliente.Text);
            string periodoPago = fechas.obtieneFechaLocal().Year.ToString() + fechas.obtieneFechaLocal().Month.ToString().PadLeft(2, '0');
            int consecutivo = datos.obtieneIdConsecutivoDetalleCliente(cliente) + 1;
            bool actualizado = datos.agregaPagMan(periodoPago, fecha, "Pago Mes Reingreso", empresa, consecutivo, cliente, usuarioAtiva);

            string ref_cliente = datos.obtieneReferencia(cliente.ToString());
            if (actualizado)
            {

                decimal monto = 0;
                try
                {
                    monto = datos.obtieneMontoEmpresa(Convert.ToInt32(Session["e"]));
                }
                catch (Exception) { monto = 0; }
                if (monto != 0)
                {
                    Imprime_Ticket impTicket = new Imprime_Ticket();
                    bool genraTicket = impTicket.generarTicketBD(empresa, usuarioAtiva, cliente, "R", Convert.ToDecimal(monto), null, "");
                    GenerarTicket("R", Convert.ToDecimal(monto), 1, fechas.obtieneFechaLocal().Year.ToString(), fechas.obtieneFechaLocal().Month.ToString().PadLeft(2, '0'), cliente, ref_cliente, null);
                    int cuentaPeriodos = 0;
                    cuentaPeriodos = datos.cuentaPeriodosPagosPorAño(cliente, fechas.obtieneFechaLocal().Year.ToString());
                    bool insertado = false;
                    if (cuentaPeriodos > 0)
                        insertado = datos.actualizarPeriodosPagos(cliente, "A", fechas.obtieneFechaLocal().Year.ToString(), fechas.obtieneFechaLocal().Month.ToString().PadLeft(2, '0'));
                    else if (cuentaPeriodos == 0)
                        insertado = datos.agregarPeriodosPagos(cliente, "A", fechas.obtieneFechaLocal().Year.ToString(), fechas.obtieneFechaLocal().Month.ToString().PadLeft(2, '0'));
                    if (!insertado)
                    {
                        datos.borraDetallePago(empresa, cliente, periodoPago);
                        lblError.Text = "No fue posible registrar el pago en el sistema del cliente nuevo con referencia " + ref_cliente + ", por favor proceda a registrar el pago en consultas";
                    }
                }
                else {
                    datos.borraDetallePago(empresa, cliente, periodoPago);
                }
            }
            else
            {
                lblError.Text = "No fue posible registrar el pago en el sistema del cliente nuevo con referencia " + ref_cliente + ", por favor proceda a registrar el pago en consultas";
            }
            Session["C"] = 1;
            cargaDatos();
        }
        else
        {
            lblError.Text = "No se pudo activar el cliente verifique su conexión.";
            Session["C"] = 1;
            cargaDatos();
        }
    }

    

    private void baja2()
    {
        string usuarioBaja = "";
        int empresa = 0;
        foreach (GridDataItem item in RadGrid1.Items)
        {
            CheckBox inaCheck = (CheckBox)item.FindControl("ina");
            if (inaCheck.Checked)
            {
                Label cliente = (Label)item.FindControl("Label7");
                string cliente2 = cliente.Text;
                if (Clientes == "")
                {
                    Clientes = Clientes + cliente2;
                }
                else
                {
                    Clientes = Clientes + "," + cliente2;
                }
            }
        }
        string[] arg = Clientes.ToString().Split(new char[] { ',' });
        lblErrorBaja.Text = "";
        for (int J = 0; J < arg.Length; J++)
        {
            lblClienteInactiva.Text= arg[J];
            string cliente = lblClienteInactiva.Text;
            string referencia = datos.obtieneReferencia(lblClienteInactiva.Text);
    
                    if (Referencias == "")
                    {
                        Referencias = Referencias + referencia;
                    }
                    else
                    {
                        Referencias = Referencias + "," + referencia;
                    }
                
                string usuarioAutoriza = txtUsuario.Text;
            DateTime fechaBaja, fechaActual;
            fechaActual = Convert.ToDateTime(fechaLocal.obtieneFechaLocal().ToShortDateString());
            try { fechaBaja = Convert.ToDateTime(txtFechaBaja.Text + " 00:00:00"); }
            catch (Exception) { fechaBaja = Convert.ToDateTime("1900-01-01"); }
            try { empresa = Convert.ToInt32(Session["e"]); usuarioBaja = Session["u"].ToString(); }
            catch (Exception) { Response.Redirect("Default.aspx"); }
            int año = Convert.ToInt32(ddlAño.SelectedValue);
            int periodo = fechas.obtieneFechaLocal().Month;
            bool darBaja = obtieneSiPuedeDarBaja(año, periodo, 1, Convert.ToInt32(cliente));
            if (!darBaja)
                lblErrorBaja.Text = "El cliente aun cuenta con pagos atrasados y no es posible darlo de baja";
            else
            {
                bool activado = datos.inactivaCliente(cliente, "I", fechaBaja.ToString("yyyy-MM-dd"), txtMotivoBaja.Text, referencia, null, empresa, usuarioBaja, "", usuarioAutoriza);
                if (activado)
                {
                    string scriptBa = "cierraWinBaj()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "CierraBaja", scriptBa, true);
                  
                }

                lblErrorBaja.Text = "";
                string scriptAl = "cierraWinBaj()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "CierraBaja", scriptAl, true); 
            }
           
        }
        Imprime_Ticket impTicket = new Imprime_Ticket();
        bool genraTicket = impTicket.generarTicketBD2(empresa, usuarioBaja, Clientes, "I", 0, null, txtMotivoBaja.Text);
        GenerarTicketMasivo("I", 0, 1, null, null, Clientes, Referencias, null);
        Session["C"] = 1;
        cargaDatos();
    }

    private void baja1(bool archivoValido, string ruta)
    {
        bool[] tienePermiso = datos.obtienePermisos(txtUsuario.Text);
        if (tienePermiso[17])
        {
            bool registraBaja = false;
            int empresa;
            string usuario = "";
            try
            {
                empresa = Convert.ToInt32(Session["e"]);
                usuario = Session["u"].ToString();
            }
            catch (Exception)
            {
                empresa = 0;
                Response.Redirect("Default.aspx");
            }
            string cliente = lblClienteInactiva.Text;
            string tarhetaHabiente = datos.obtieneNombreCliente(lblClienteInactiva.Text, "C");
            string referencia = datos.obtieneReferencia(lblClienteInactiva.Text);
            //Telerik.Web.UI.RadAsyncUpload radAsyc = (Telerik.Web.UI.RadAsyncUpload)Session["imagen"];
            RadAsyncUpload radAsyc = (RadAsyncUpload)Session["RadAsyncUpload"];
            extension = "";
            DateTime fechaBaja = Convert.ToDateTime(txtFechaBaja.Text);
            registraBaja = datos.registraBaja(empresa, referencia, cliente, tarhetaHabiente, fechaBaja.ToString("yyyy-MM-dd"), txtMotivoBaja.Text, usuario, txtUsuario.Text, lblClienteInactiva.Text, null, "");
            if (registraBaja)
            {
                bool borrado = false;
                borrado = datos.borraCliente(lblClienteInactiva.Text);
                if (borrado)
                {
                    Imprime_Ticket impTicket = new Imprime_Ticket();
                    bool genraTicket = impTicket.generarTicketBD(empresa, usuario, int.Parse(cliente), "B", 0, null, txtMotivoBaja.Text);
                    GenerarTicket("B", 0, 1, null, null, int.Parse(cliente), referencia, null);
                    string script1 = "cierraWinAut()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "CierraAutorizacion", script1, true);
                    cargaDatos();
                }
                else
                {
                    lblErrorBaja.Text = lblErrorAutenticacion.Text = "No se pudo borrar el cliente verifique su conexión.";
                    string script1 = "cierraWinAut()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "CierraAutorizacion", script1, true);
                }
                try
                {
                    string filename = "";
                    int documentos = radAsyc.UploadedFiles.Count;
                    int guardados = 0;
                    for (int i = 0; i < documentos; i++)
                    {
                        filename = radAsyc.UploadedFiles[i].FileName;
                        string[] segmenatado = filename.Split(new char[] { '.' });
                        archivoValido = validaArchivo(segmenatado[1]);
                        extension = segmenatado[1];
                        string archivo = String.Format("{0}\\{1}", ruta, filename);
                        try
                        {
                            FileInfo file = new FileInfo(archivo);
                            // Verificar que el archivo no exista
                            if (File.Exists(archivo))
                                file.Delete();

                            if (extension.ToUpper() != "PDF")
                            {
                                System.Drawing.Image img = System.Drawing.Image.FromStream(radAsyc.UploadedFiles[i].InputStream);
                                img.Save(archivo);
                                imagen = Imagen_A_Bytes(archivo);
                            }
                            else
                            {
                                Telerik.Web.UI.UploadedFile up = radAsyc.UploadedFiles[i];
                                up.SaveAs(archivo);
                                imagen = File.ReadAllBytes(archivo);
                            }
                        }
                        catch (Exception) { imagen = null; }
                        if (imagen != null)
                        {
                            bool agregado = datos.agregaDocumentosCliente(Convert.ToInt32(lblClienteInactiva.Text), imagen, extension, referencia, empresa, "B");
                            if (agregado)
                                guardados++;
                        }
                    }
                    string scriptAl = "cierraWinBaj()";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "CierraBaja", scriptAl, true);
                }
                catch (Exception) { imagen = null; }
            }
            else
            {
                lblErrorBaja.Text = lblErrorAutenticacion.Text = "No se pudo borrar el cliente verifique su conexión.";
                string script1 = "cierraWinAut()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "CierraAutorizacion", script1, true);
            }
        }
        else
            lblErrorAutenticacion.Text = "El usuario no cuenta con los privilegios para autorizar la baja";
        Session["C"] = 1;
    }


    protected void lnkCargas_Click(object sender, EventArgs e)
    {
        Session["C"] = 1;
        Response.Redirect("Cargas.aspx?e=" + Convert.ToInt32(Session["e"]) + "&u=" + Session["u"].ToString() + "&nu=" + Request.QueryString["nu"]);

    }

    protected void ddlAño_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["C"] = 1;
        cargaDatos();
    }

    private Byte[] Imagen_A_Bytes(String ruta)
    {
        try
        {
            FileStream foto = new FileStream(ruta, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            Byte[] arreglo = new Byte[foto.Length];
            BinaryReader reader = new BinaryReader(foto);
            arreglo = reader.ReadBytes(Convert.ToInt32(foto.Length));
            return arreglo;
        }
        catch (Exception)
        {
            return null;
        }
    }

    protected void DataListFotosDanos_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "zoom")
        {
            PanelImgZoom.Visible = true;
            PanelMod.Visible = false;
            btnCerrarImgZomm1.Visible = false;
            btnCerrarImgZomm.Visible = true;
            string[] valores = e.CommandArgument.ToString().Split(';');
            try
            {
                int id_empresa = Convert.ToInt32(Session["e"]);
                int id = Convert.ToInt32(valores[0]);
                string referencia = valores[1];
                int idCliente = Convert.ToInt32(valores[2]);
                string extension = valores[3].ToUpper();
                if (extension != "PDF")
                {
                    Panel8.Visible = true;
                    pnlPdf.Visible = false;
                    imgZoom.ImageUrl = "~/ImgDoctos.ashx?id=" + id + ";" + referencia + ";" + id_empresa + ";" + idCliente;
                }
                else
                {
                    byte[] docto = datos.obtienePdf(id_empresa, idCliente, referencia, id);
                    if (docto != null)
                    {
                        string ruta = Server.MapPath("~/TMP");

                        // Si el directorio no existe, crearlo
                        if (!Directory.Exists(ruta))
                            Directory.CreateDirectory(ruta);

                        string archivo = ruta + "\\" + idCliente.ToString() + referencia + id.ToString() + ".pdf";
                        File.WriteAllBytes(archivo, docto);

                        Panel8.Visible = false;
                        pnlPdf.Visible = true;
                        ShowPdf1.FilePath = "TMP/" + idCliente.ToString() + referencia + id.ToString() + ".pdf";
                    }
                    else
                    {
                        Panel8.Visible = true;
                        pnlPdf.Visible = false;
                        imgZoom.ImageUrl = "IMG/pdflogo.jpg";
                    }
                }
            }
            catch (Exception) { }
        }
        Session["C"] = 1;
    }

    protected void btnCerrarImgZomm_Click(object sender, ImageClickEventArgs e)
    {
        PanelImgZoom.Visible = false;
        PanelMod.Visible = true;
        Session["C"] = 1;
    }

    protected void btnCerrarImgZomm1_Click(object sender, ImageClickEventArgs e)
    {
        PanelImgZoom.Visible = false;
        Panel6.Visible = false;
        Session["C"] = 1;
    }

    private bool validaArchivo(string extencion)
    {
        string[] extenciones = { "jpeg", "jpg", "png", "gif", "bmp", "tiff", "pdf" };
        bool valido = false;
        for (int i = 0; i < extenciones.Length; i++)
        {
            if (extencion.ToUpper() == extenciones[i].ToUpper())
            {
                valido = true;
                break;
            }
        }
        Session["C"] = 1;
        return valido;
    }

    protected void GridView2_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView2.EditIndex = -1;
        obtieneSDetalle(lblCliente.Text);
        Session["C"] = 1;
    }
    protected void GridView2_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView2.EditIndex = e.NewEditIndex;
        obtieneSDetalle(lblCliente.Text);
        Session["C"] = 1;
    }
    protected void GridView2_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        string usuario = "";
        try { usuario = Session["u"].ToString(); }
        catch (Exception) { usuario = ""; Response.Redirect("Default.aspx"); }
        int consecutivo = Convert.ToInt32(GridView2.DataKeys[e.RowIndex].Value.ToString());
        CheckBox check = GridView2.Rows[e.RowIndex].FindControl("chkNoReconocidoMod") as CheckBox;

        Label lblFechaCargo = GridView2.Rows[e.RowIndex].FindControl("lblFechaMovimientoMod") as Label;
        Label lblPeriodoCargo = GridView2.Rows[e.RowIndex].FindControl("Label24") as Label;

        string[] periodoPMesSplit = generaPeriodo(lblPeriodoCargo.Text).Split(';');
        DateTime periodoAñoDate = Convert.ToDateTime(lblFechaCargo.Text);
        string periodoP = periodoPMesSplit[0];
        string periodoAño = periodoAñoDate.Year.ToString();
        string periodoMes = periodoPMesSplit[1];
        int valor = 0;
        if (check.Checked)
            valor = 1;
        else
            valor = 0;
        bool actualizado = datos.actualizaNoReconocido(lblCliente.Text, consecutivo, valor, usuario, periodoP, periodoAño, periodoMes);
        if (actualizado)
        {
            GridView2.EditIndex = -1;
            obtieneSDetalle(lblCliente.Text);
            Session["C"] = 1;
            cargaDatos();
        }
        Session["C"] = 1;
    }

    private string generaPeriodo(string periodoP)
    {
        string periodo = "";
        switch (periodoP)
        {
            case "Enero":
                periodo = "p1;01";
                break;
            case "Febrero":
                periodo = "p2;02";
                break;
            case "Marzo":
                periodo = "p3;03";
                break;
            case "Abril":
                periodo = "p4;04";
                break;
            case "Mayo":
                periodo = "p5;05";
                break;
            case "Junio":
                periodo = "p6;06";
                break;
            case "Julio":
                periodo = "p7;07";
                break;
            case "Agosto":
                periodo = "p8;08";
                break;
            case "Septiembre":
                periodo = "p9;09";
                break;
            case "Octubre":
                periodo = "p10;10";
                break;
            case "Noviembre":
                periodo = "p11;11";
                break;
            case "Diciembre":
                periodo = "p12;12";
                break;
        }
        return periodo;
    }

   

    protected void lnkAgregaComentario_Click(object sender, ImageClickEventArgs e)
    {
        lblErroresComentarios.Text = "";
        string comentario = txtComentario.Text;
        string usuario;
        int empresa;
        if (comentario != "")
        {
            try
            {
                empresa = Convert.ToInt32(Session["e"]);
                usuario = Session["u"].ToString();
            }
            catch (Exception)
            {
                empresa = 0;
                usuario = "";
                Response.Redirect("Default.aspx");
            }
            Fechas fechas = new Fechas();
            bool agregado = datos.agregaComentario(lblCliente.Text, txtComentario.Text, usuario, fechas.obtieneFechaLocal(), empresa);
            if (agregado)
            {
                lblErroresComentarios.Text = "Se agrego un comentario satisfactoriamente";
                GridView3.DataBind();
                cargaDatos();
                txtComentario.Text = "";
            }
            else
                lblErroresComentarios.Text = "Error al agregar el comentario, vuelva a intentar";
        }
        else
            lblErroresComentarios.Text = "Debe indicar el comentario";
        Session["C"] = 1;
    }

    protected void lnkLeido_Click(object sender, EventArgs e)
    {
        lblErroresComentarios.Text = "";
        LinkButton btn = (LinkButton)sender;
        string consecutivo = btn.CommandArgument.ToString();
        string usuario;
        int empresa;
        int cliente;
        try
        {
            empresa = Convert.ToInt32(Session["e"]);
            usuario = Session["u"].ToString();
            cliente = Convert.ToInt32(lblCliente.Text);
        }
        catch (Exception)
        {
            empresa = 0;
            usuario = "";
            cliente = 0;
            Response.Redirect("Default.aspx");
        }
        bool agregado = datos.actualizaComentario(lblCliente.Text, empresa, consecutivo, usuario);
        if (agregado)
        {
            lblErroresComentarios.Text = "Se ha colocado el comentario como leido";
            GridView3.DataBind();
            cargaDatos();
        }
        else
            lblErroresComentarios.Text = "Error al agregar el comentario, vuelva a intentar";
        Session["C"] = 1;
    }

    protected void GridView3_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string leido = DataBinder.Eval(e.Row.DataItem, "leido").ToString().ToUpper();
            var btn = e.Row.Cells[9].Controls[1].FindControl("lnkLeido") as LinkButton;
            if (leido == "TRUE")
                btn.Visible = false;
            else
                btn.Visible = true;
            var lblFechaLectura = e.Row.Cells[9].Controls[1].FindControl("lblFechaLectura") as Label;
            if (lblFechaLectura.Text == "00:00:00")
                lblFechaLectura.Visible = false;
        }
        Session["C"] = 1;
    }

    protected void chkBajaInmediata_CheckedChanged(object sender, EventArgs e)
    {
        Session["C"] = 1;
        checaCheckBajaInmediata();
        Session["C"] = 1;
    }

    private void checaCheckBajaInmediata()
    {
        if (chkBajaInmediata.Checked)
            btnBaja.Visible = true;
        else
        {
            int año = Convert.ToInt32(ddlAño.SelectedValue);
            int periodo = fechas.obtieneFechaLocal().Month;
            bool darBaja = obtieneSiPuedeDarBaja(año, periodo, 1, Convert.ToInt32(lblClienteInactiva.Text));
            if (!darBaja)
            {
                lblErrorBaja.Text = "El cliente aun cuenta con pagos atrasados y no es posible darlo de baja";
                txtFechaBaja.Text = fechas.obtieneFechaLocal().ToString("yyyy-MM-dd");
                btnBaja.Visible = false;
            }
            else
            {
                lblErrorBaja.Text = "";
                btnBaja.Visible = true;
            }
        }
        Session["C"] = 1;
    }

    public void GenerarTicket(string tipo, decimal monto, int cantidad, string año, string mes, int cliente, string refCliente, string periodo)
    {
        string usuario;
        int empresa;
        try
        {
            try
            {
                empresa = Convert.ToInt32(Session["e"]);
                usuario = Session["u"].ToString();
            }
            catch (Exception)
            {
                empresa = 0;
                usuario = "";
                Response.Redirect("Default.aspx");
            }

            lblError.Text = "";
            Imprime_Ticket impTicket = new Imprime_Ticket();
            int ticket = impTicket.obtieneTicket(empresa, cliente);
            string nomEmpresa = datos.obtieneNombreEmpresa(empresa);
            string Archivo = impTicket.GenerarTicket(empresa, usuario, cliente, tipo, monto, cantidad, año, mes, ticket, nomEmpresa, refCliente, periodo);
            if (Archivo != "")
            {
                try
                {
                    System.IO.FileInfo filename = new System.IO.FileInfo(Archivo);
                    if (filename.Exists)
                    {
                        string url = "TicketPdf.aspx?a=" + filename.Name;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "pdfs", "window.open('" + url + "', 'nuevo', 'directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, titlebar=no, width=856px, height=550px');", true);
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                }
            }
            else
                lblError.Text = "Error al generar el ticket del servicio";
        }
        catch (Exception ex)
        {
            lblError.Text = "Error al generar el ticket del servicio " + ex.Message;
        }
        Session["C"] = 1;
    }
    public void GenerarTicketMasivo(string tipo, decimal monto, int cantidad, string año, string mes, string cliente, string refCliente, string periodo)
    {
        string usuario;
        int empresa;
        try
        {
            try
            {
                empresa = Convert.ToInt32(Session["e"]);
                usuario = Session["u"].ToString();
            }
            catch (Exception)
            {
                empresa = 0;
                usuario = "";
                Response.Redirect("Default.aspx");
            }

            lblError.Text = "";
            Imprime_Ticket impTicket = new Imprime_Ticket();
            int ticket = impTicket.obtieneTicket2(empresa, cliente);
            string nomEmpresa = datos.obtieneNombreEmpresa(empresa);
            string Archivo = impTicket.GenerarTicket2(empresa, usuario, cliente, tipo, monto, cantidad, año, mes, ticket, nomEmpresa, refCliente, periodo);
            if (Archivo != "")
            {
                try
                {
                    System.IO.FileInfo filename = new System.IO.FileInfo(Archivo);
                    if (filename.Exists)
                    {
                        string url = "TicketPdf.aspx?a=" + filename.Name;
                        ScriptManager.RegisterStartupScript(this, typeof(Page), "pdfs", "window.open('" + url + "', 'nuevo', 'directories=no, location=no, menubar=no, scrollbars=yes, statusbar=no, titlebar=no, width=856px, height=550px');", true);
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = ex.Message;
                }
            }
            else
                lblError.Text = "Error al generar el ticket del servicio";
        }
        catch (Exception ex)
        {
            lblError.Text = "Error al generar el ticket del servicio " + ex.Message;
        }
        Session["C"] = 1;
    }

    protected void RadGrid1_PageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        Session["C"] = 1;
        cargaDatos();
    }

    protected void RadGrid1_PageSizeChanged(object sender, GridPageSizeChangedEventArgs e)
    {
        Session["C"] = 1;
        cargaDatos();
    }

    protected void GridView2_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string movimiento = DataBinder.Eval(e.Row.DataItem, "movs").ToString();
            if (e.Row.RowState.ToString() == "Normal" || e.Row.RowState.ToString() == "Alternate")
            {
                if (e.Row.RowState.ToString() == "Alternate")
                    e.Row.BackColor = Color.WhiteSmoke;
                else
                    e.Row.BackColor = Color.White;

                switch (movimiento)
                {
                    case "R":
                        e.Row.Cells[1].BackColor = Color.Red;
                        break;
                    case "M":
                        e.Row.Cells[1].BackColor = Color.LightSkyBlue;
                        break;
                    case "C":
                        e.Row.Cells[1].BackColor = Color.Yellow;
                        break;
                    case "P":
                        e.Row.Cells[1].BackColor = Color.LimeGreen;
                        break;
                    default:
                        e.Row.Cells[1].BackColor = Color.Transparent;
                        break;
                }

            }
        }
        Session["C"] = 1;
    }

    protected void btnCancAut_Click(object sender, ImageClickEventArgs e)
    {
        string argumento = btnAutentica.CommandArgument.ToString();
        string script1 = "";
        script1 = "cierraWinAut()";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "CierraAutorizacion", script1, true);
        switch (argumento)
        {
            case "0":
                script1 = "abreWinAlt()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "AbreAlta", script1, true);
                break;
            case "1":
                script1 = "abreWinBaj()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "AbreBaja", script1, true);
                break;
            case "2":
                script1 = "abreWinBaj()";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "AbreBaja", script1, true);
                break;
            default:
                break;
        }
        Session["C"] = 1;
    }

    protected void lknEditar_Click(object sender, ImageClickEventArgs e)
    {

    }
    protected void RadGrid1_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item.ItemType == GridItemType.Item || e.Item.ItemType == GridItemType.AlternatingItem)
        {
            string usuario = DataBinder.Eval(e.Item.DataItem, "id_cliente").ToString();
            string fechaIngreso = DataBinder.Eval(e.Item.DataItem, "fecha_ini").ToString();
            string vigencia = DataBinder.Eval(e.Item.DataItem, "vigencia_cuenta").ToString();
            int noReconocidos = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "noReconocido").ToString());
            int mesBaja = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "mesBaja").ToString());
            int anoBaja = Convert.ToInt32(DataBinder.Eval(e.Item.DataItem, "anoBaja").ToString());
            string argumentos = DataBinder.Eval(e.Item.DataItem, "extra").ToString();

            int empresa = 0;
            try { empresa = Convert.ToInt32(Session["e"]); }
            catch (Exception) { Response.Redirect("Default.aspx"); }
            var botonEliminar = e.Item.FindControl("lknEliminar") as ImageButton;
            var botonActiva = e.Item.FindControl("lknActiva") as ImageButton;
            var botonInactiva = e.Item.FindControl("lknInactiva") as ImageButton;
            var botonEditar = e.Item.FindControl("lknEditar") as ImageButton;
            var p1 = e.Item.FindControl("P1") as Label;
            var p2 = e.Item.FindControl("P2") as Label;
            var p3 = e.Item.FindControl("P3") as Label;
            var p4 = e.Item.FindControl("P4") as Label;
            var p5 = e.Item.FindControl("P5") as Label;
            var p6 = e.Item.FindControl("P6") as Label;
            var p7 = e.Item.FindControl("P7") as Label;
            var p8 = e.Item.FindControl("P8") as Label;
            var p9 = e.Item.FindControl("P9") as Label;
            var p10 = e.Item.FindControl("P10") as Label;
            var p11 = e.Item.FindControl("P11") as Label;
            var p12 = e.Item.FindControl("P12") as Label;
            var lblClienteGrid = e.Item.FindControl("Label10") as Label;

            var imgComentario = e.Item.FindControl("imgComentario") as System.Web.UI.WebControls.Image;
            var imgInformacion = e.Item.FindControl("imgInformacion") as System.Web.UI.WebControls.Image;
            var imgDocumentos = e.Item.FindControl("imgDocumentos") as System.Web.UI.WebControls.Image;

            string[] valoresFaltantes = argumentos.Split(new char[] { ';' });

            if (Convert.ToInt32(valoresFaltantes[0].Trim()) > 0)
                imgComentario.Visible = true;
            else
                imgComentario.Visible = false;

            if (Convert.ToInt32(valoresFaltantes[1].Trim()) > 0)
                imgInformacion.Visible = true;
            else
                imgInformacion.Visible = false;

            if (Convert.ToInt32(valoresFaltantes[2].Trim()) == 0)
                imgDocumentos.Visible = true;
            else
                imgDocumentos.Visible = false;


            if (noReconocidos > 0)
            {
                lblClienteGrid.BackColor = Color.Yellow;
                e.Item.Cells[4].BackColor = Color.Yellow;
            }


            string[] periodos = new string[12];
            int periodoRegistro = 1;
            for (int valores = 0; valores < 12; valores++)
            {
                string periodo = "p" + periodoRegistro.ToString();
                periodos[valores] = DataBinder.Eval(e.Item.DataItem, periodo).ToString();
                periodoRegistro++;
            }

            /*if (e.Row.RowState.ToString() == "Normal" || e.Row.RowState.ToString() == "Alternate")
            {*/
            /*if (e.Row.RowState.ToString() == "Alternate")
                e.Row.BackColor = Color.WhiteSmoke;
            else
                e.Row.BackColor = Color.White;*/

            string status = DataBinder.Eval(e.Item.DataItem, "estatus_cliente").ToString();

            string[] pagos = null;
            Color[] colores = null;
            Label[] elementos = { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12 };
            for (int i = 0; i < 12; i++)
            {
                elementos[0].Visible = true;
            }


            DataSet ultimoMov = new DataSet();

            pagos = new string[12] { "E", "F", "M", "A", "M", "J", "J", "A", "S", "O", "N", "D" };
            colores = new Color[12];
            for (int i = 0; i < 12; i++)
            {
                colores[i] = defineColor(periodos[i]);
            }

            string usuarioLog = "";
            try { usuarioLog = Session["u"].ToString(); }
            catch (Exception) { Response.Redirect("Default.aspx"); }
            bool permisoAlta = altaCliente[18];
            if (status == "I")
            {
                e.Item.ForeColor = Color.Red;
                if (permisoAlta)
                    botonEliminar.Visible = false;
                else
                    botonEliminar.Visible = true;

                bool permiteInactivar = altaCliente[14];
                if (permiteInactivar)
                    botonActiva.Visible = true;
                else
                    botonActiva.Visible = false;
                botonInactiva.Visible = false;
                botonEditar.Visible = false;
            }
            else
            {
                e.Item.ForeColor = Color.Black;
                botonEliminar.Visible = false;
                botonActiva.Visible = false;
                bool permiteInactivar = altaCliente[14];
                if (permiteInactivar)
                    botonInactiva.Visible = true;
                else
                    botonInactiva.Visible = false;
                if (!permisoAlta)
                    botonEditar.Visible = false;
                else
                    botonEditar.Visible = true;
            }


            int contador = 0;

            DateTime fechaPeriodo, fechaIngresoD;
            fechaIngresoD = Convert.ToDateTime("01/" + fechaIngreso.Substring(5, 2).PadLeft(2, '0') + "/" + fechaIngreso.Substring(0, 4));

            if (vigencia.Trim() != "")
            {
                int diferencia = obtieneMesesdeVigencia(vigencia);
                Label banco = (Label)e.Item.FindControl("Label19");

                if (diferencia <= 2 && diferencia >= 1)
                {
                    banco.BackColor = Color.Yellow;
                    e.Item.Cells[7].BackColor = Color.Yellow;
                    if (status == "I")
                        banco.ForeColor = Color.Black;
                }
                else if (diferencia < 1)
                {
                    banco.BackColor = Color.Red;
                    e.Item.Cells[7].BackColor = Color.Red;
                    if (status == "I")
                        banco.ForeColor = Color.White;
                }
                else
                {
                    banco.BackColor = Color.Transparent;
                    e.Item.Cells[7].BackColor = Color.Transparent;
                }

            }
            int celda = 11;
            foreach (Label etiqueta in elementos)
            {
                etiqueta.Text = pagos[contador];
                try
                {
                    fechaPeriodo = Convert.ToDateTime("01/" + (contador + 1).ToString().PadLeft(2, '0') + "/" + ddlAño.SelectedValue);
                }
                catch (Exception)
                {
                    fechaPeriodo = Convert.ToDateTime("01/" + (contador + 1).ToString().PadLeft(2, '0') + "/" + fechaLocal.obtieneFechaLocal().Year.ToString());
                }
                if (fechaPeriodo >= fechaIngresoD)
                {
                    if (status == "I")
                    {
                        if (anoBaja.ToString() == ddlAño.SelectedValue)
                            if (contador + 1 > mesBaja)
                                etiqueta.Visible = false;
                            else
                                etiqueta.Visible = true;
                        else if (anoBaja < Convert.ToInt32(ddlAño.SelectedValue))
                            etiqueta.Visible = false;
                        else
                            etiqueta.Visible = true;
                    }
                    else
                        etiqueta.Visible = true;
                    //etiqueta.ToolTip = obtieneDetalle(usuario, empresa, fechaPeriodo.ToString("yyyyMM"));
                }
                else
                {
                    etiqueta.Visible = false;
                }



                e.Item.Cells[celda].BackColor = colores[contador];
                etiqueta.ForeColor = Color.Black;
                etiqueta.Font.Bold = true;//colores[contador];
                etiqueta.BackColor = colores[contador];
                contador++;
                celda++;

            }

            Label lblcuenta = (Label)e.Item.FindControl("Label16");
            lblcuenta.Text = lblcuenta.Text.Substring(12, 4).PadLeft(16, '*');


            //}
        }
        Session["C"] = 1;
    }
}