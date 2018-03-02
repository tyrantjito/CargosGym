using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using PdfViewer1;
using E_Utilities;

public partial class Estadisticos : System.Web.UI.Page
{
    ObtieneFechaActual fechaLocal = new ObtieneFechaActual();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
            int CONTA=1;
            for (int i = 0; i < 12; i++) {
                periodos[i].Text = fechaLocal.obtieneFechaLocal().Year.ToString() + CONTA.ToString().PadLeft(2, '0');
                CONTA++;
            }
            BarChartPagos.Visible = false;
        }
    }

    protected void dlEstadistico_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
        if (dlEstadistico.SelectedValue == "0")
        {
            obtienePagos(periodos,ddlPeriodo.SelectedValue);
            pnlMontos.Visible = false;
            pnlPagos.Visible = true;
            pnlAltas.Visible = false;
            pnlBajas.Visible = false;
            pnlBancos.Visible = false;
            panelUsuarios.Visible = false;
        }
        else if (dlEstadistico.SelectedValue == "1")
        {
            obtieneMontos(periodos, ddlPeriodo.SelectedValue);
            pnlMontos.Visible = true;
            pnlPagos.Visible = false;
            pnlAltas.Visible = false;
            pnlBajas.Visible = false;
            pnlBancos.Visible = false;
            panelUsuarios.Visible = false;
        }
        else if (dlEstadistico.SelectedValue == "2") {
            obtieneAltas(periodos, ddlPeriodo.SelectedValue);
            pnlMontos.Visible = false;
            pnlPagos.Visible = false;
            pnlAltas.Visible = true;
            pnlBajas.Visible = false;
            pnlBancos.Visible = false;
            panelUsuarios.Visible = false;
        }
        else if (dlEstadistico.SelectedValue == "3")
        {
            obtieneBajas(periodos, ddlPeriodo.SelectedValue);
            pnlMontos.Visible = false;
            pnlPagos.Visible = false;
            pnlAltas.Visible = false;
            pnlBajas.Visible = true;
            pnlBancos.Visible = false;
            panelUsuarios.Visible = false;
        }
        else if (dlEstadistico.SelectedValue == "4")
        {
            //obtieneBancos(periodos, ddlPeriodo.SelectedValue);            
            dtBancos.DataBind();
            pnlMontos.Visible = false;
            pnlPagos.Visible = false;
            pnlAltas.Visible = false;
            pnlBajas.Visible = false;
            pnlBancos.Visible = true;
            panelUsuarios.Visible = false;
        }
        else if (dlEstadistico.SelectedValue == "5")
        {
            //ddlUsuario.Items.Clear();
            //ddlUsuario.DataBind();
            ddlAños.Items.Clear();
            ddlAños.DataBind();
            llenaDdlUsuarios();
            //obtieneInfoDetalles(ddlUsuario.SelectedValue, ddlPeriodo.SelectedValue, ddlAños.SelectedValue, periodos, "A");
            //obtieneInfoDetalles(ddlUsuario.SelectedValue, ddlPeriodo.SelectedValue, ddlAños.SelectedValue, periodos, "B");
            llenaDtUsuarios();            
            pnlMontos.Visible = false;
            pnlPagos.Visible = false;
            pnlAltas.Visible = false;
            pnlBajas.Visible = false;
            pnlBancos.Visible = false;
            panelUsuarios.Visible = true;
            //RadTabStrip1.SelectedIndex = RadMultiPage1.SelectedIndex = 0;
        }

    }

    private void llenaDtUsuarios()
    {
        int empresa = 0;
        try { empresa = Convert.ToInt32(Request.QueryString["e"]); }
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
            fecha_inicio = Convert.ToDateTime(año+"-"+ periodo_indicado.PadLeft(2, '0') + "-01");
            fecha_final = fecha_inicio.AddYears(1);
            fecha_final = fecha_final.AddDays(-1);
        }
        else
        {
            fecha_inicio = Convert.ToDateTime(año + "-" + periodo.PadLeft(2, '0') + "-01");
            fecha_final = fecha_inicio.AddMonths(1);
            fecha_final = fecha_final.AddDays(-1);
        }

        string usuarioCondicion = "";
        if (ddlUsuarios.SelectedValue == "T" || ddlUsuarios.SelectedValue == "")
            usuarioCondicion = "";
        else
            usuarioCondicion = " and TABLA.USUARIO='" + ddlUsuarios.SelectedValue + "' ";


        SqlDataSourceUsuarios.SelectCommand = "declare @empresa int set @empresa=" + empresa + " SELECT DISTINCT TABLA.USUARIO,CASE TABLA.NOMBRE WHEN '' THEN (CASE TABLA.USUARIO WHEN '' THEN 'SIN USUARIO' ELSE TABLA.USUARIO END) ELSE TABLA.NOMBRE END AS NOMBRE FROM(" +
"select DISTINCT ISNULL(UPPER(C.USUARIO_ALTA), '') AS USUARIO, ISNULL(UPPER(U.NOMBRE), '') AS NOMBRE from clientes C LEFT JOIN USUARIOS U ON U.ID_USUARIO = C.USUARIO_ALTA where C.id_empresa = @empresa AND C.ESTATUS_CLIENTE = 'A' and convert(char(10), c.fecha_inicio, 120) between '" + fecha_inicio.ToString("yyyy-MM-dd") + "' and '" + fecha_final.ToString("yyyy-MM-dd") + "' " +
"UNION ALL select DISTINCT ISNULL(UPPER(C.USUARIO_INACTIVA), '') AS USUARIO, ISNULL(UPPER(U.NOMBRE), '') AS NOMBRE from clientes C LEFT JOIN USUARIOS U ON U.ID_USUARIO = C.USUARIO_INACTIVA where C.id_empresa = @empresa AND C.ESTATUS_CLIENTE = 'I' and convert(char(10), c.fecha_inicio, 120) between '" + fecha_inicio.ToString("yyyy-MM-dd") + "' and '" + fecha_final.ToString("yyyy-MM-dd") + "' " +
"UNION ALL SELECT DISTINCT ISNULL(UPPER(B.USUARIO_BAJA), '') AS USUARIO, ISNULL(UPPER(U.NOMBRE), '') AS NOMBRE FROM BAJAS_INMEDIATAS B LEFT JOIN USUARIOS U ON U.ID_USUARIO = B.USUARIO_BAJA WHERE B.ID_EMPRESA = @empresa  and convert(char(10), b.fecha_baja, 120) between '" + fecha_inicio.ToString("yyyy-MM-dd") + "' and '" + fecha_final.ToString("yyyy-MM-dd") + "' UNION ALL " +
"SELECT '' AS USUARIO, '' AS NOMBRE UNION ALL SELECT UPPER(ID_USUARIO)AS USUARIO, UPPER(NOMBRE) AS NOMBRE FROM USUARIOS ) AS TABLA WHERE TABLA.USUARIO <> 'SUPERVISOR' " + usuarioCondicion + " ORDER BY TABLA.USUARIO";
        dtUsuarios.DataSourceID = "SqlDataSourceUsuarios";
        dtUsuarios.DataBind();
    }

    private void llenaDdlUsuarios()
    {
        int empresa = 0;
        try { empresa = Convert.ToInt32(Request.QueryString["e"]); }
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
            fecha_inicio = Convert.ToDateTime(año + "-" + periodo_indicado.PadLeft(2, '0') + "-01");
            fecha_final = fecha_inicio.AddYears(1);
            fecha_final = fecha_final.AddDays(-1);
        }
        else
        {
            fecha_inicio = Convert.ToDateTime(año + "-" + periodo.PadLeft(2, '0') + "-01");
            fecha_final = fecha_inicio.AddMonths(1);
            fecha_final = fecha_final.AddDays(-1);
        }
        SqlDataSource3.SelectCommand = "declare @empresa int set @empresa=" + empresa + " SELECT DISTINCT TABLA.USUARIO,CASE TABLA.NOMBRE WHEN '' THEN (CASE TABLA.USUARIO WHEN '' THEN 'SIN USUARIO' ELSE TABLA.USUARIO END) ELSE TABLA.NOMBRE END AS NOMBRE FROM(" +
"select DISTINCT ISNULL(UPPER(C.USUARIO_ALTA), '') AS USUARIO, ISNULL(UPPER(U.NOMBRE), '') AS NOMBRE from clientes C LEFT JOIN USUARIOS U ON U.ID_USUARIO = C.USUARIO_ALTA where C.id_empresa = @empresa AND C.ESTATUS_CLIENTE = 'A' and convert(char(10), c.fecha_inicio, 120) between '" + fecha_inicio.ToString("yyyy-MM-dd") + "' and '" + fecha_final.ToString("yyyy-MM-dd") + "' " +
"UNION ALL select DISTINCT ISNULL(UPPER(C.USUARIO_INACTIVA), '') AS USUARIO, ISNULL(UPPER(U.NOMBRE), '') AS NOMBRE from clientes C LEFT JOIN USUARIOS U ON U.ID_USUARIO = C.USUARIO_INACTIVA where C.id_empresa = @empresa AND C.ESTATUS_CLIENTE = 'I' and convert(char(10), c.fecha_inicio, 120) between '" + fecha_inicio.ToString("yyyy-MM-dd") + "' and '" + fecha_final.ToString("yyyy-MM-dd") + "' " +
"UNION ALL SELECT DISTINCT ISNULL(UPPER(B.USUARIO_BAJA), '') AS USUARIO, ISNULL(UPPER(U.NOMBRE), '') AS NOMBRE FROM BAJAS_INMEDIATAS B LEFT JOIN USUARIOS U ON U.ID_USUARIO = B.USUARIO_BAJA WHERE B.ID_EMPRESA = @empresa  and convert(char(10), b.fecha_baja, 120) between '" + fecha_inicio.ToString("yyyy-MM-dd") + "' and '" + fecha_final.ToString("yyyy-MM-dd") + "' UNION ALL " +
"SELECT '' AS USUARIO, '' AS NOMBRE UNION ALL SELECT UPPER(ID_USUARIO)AS USUARIO, UPPER(NOMBRE) AS NOMBRE FROM USUARIOS ) AS TABLA WHERE TABLA.USUARIO <> 'SUPERVISOR' ORDER BY TABLA.USUARIO";
        ddlUsuarios.Items.Clear();
        ListItem todos = new ListItem();
        todos.Value = "T";
        todos.Text = "Todos";
        ddlUsuarios.Items.Add(todos);
        ddlUsuarios.DataSourceID = "SqlDataSource3";
        ddlUsuarios.SelectedIndex = 0;
        ddlUsuarios.DataBind();
    }

    /*private void obtieneInfoDetalles(string usuario, string periodo, string año, Label[] periodos, string status)
    {
        string empresa = "";
        try
        {
            empresa = Convert.ToString(Request.QueryString["e"]);
        }
        catch (Exception ex) { Response.Redirect("Default.aspx"); }
        Datos datos = new Datos();
        DateTime fecha_inicio;
        DateTime fecha_final;
        DataTable dt = new DataTable();
        string periodo_indicado = "1";
        Fechas fechas = new Fechas();
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

        if (status == "A")
        {
            dt = datos.obtieneInformacionAltaUsuarios(usuario, empresa, fecha_inicio, fecha_final);
            radGridAltaUser.DataSource = dt;
            radGridAltaUser.DataBind();
        }
        else if (status == "B")
        {
            dt = datos.obtieneInformacionBajasUsuarios(usuario, empresa, fecha_inicio, fecha_final);
            radGridBajasUser.DataSource = dt;
            radGridBajasUser.DataBind();
        }
    }*/

    protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        cargaInfo();
    }

    private void cargaInfo()
    {
        Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
        if (dlEstadistico.SelectedValue == "0")
        {
            obtienePagos(periodos, ddlPeriodo.SelectedValue);
            pnlMontos.Visible = false;
            pnlAltas.Visible = false;
            pnlBajas.Visible = false;
            pnlPagos.Visible = true;
            pnlBancos.Visible = false;
            panelUsuarios.Visible = false;
        }
        else if (dlEstadistico.SelectedValue == "1")
        {
            obtieneMontos(periodos, ddlPeriodo.SelectedValue);
            pnlMontos.Visible = true;
            pnlPagos.Visible = false;
            pnlAltas.Visible = false;
            pnlBajas.Visible = false;
            pnlBancos.Visible = false;
            panelUsuarios.Visible = false;
        }
        else if (dlEstadistico.SelectedValue == "2")
        {
            obtieneAltas(periodos, ddlPeriodo.SelectedValue);
            pnlMontos.Visible = false;
            pnlPagos.Visible = false;
            pnlAltas.Visible = true;
            pnlBajas.Visible = false;
            pnlBancos.Visible = false;
            panelUsuarios.Visible = false;
        }
        else if (dlEstadistico.SelectedValue == "3")
        {
            obtieneBajas(periodos, ddlPeriodo.SelectedValue);
            pnlMontos.Visible = false;
            pnlPagos.Visible = false;
            pnlAltas.Visible = false;
            pnlBajas.Visible = true;
            pnlBancos.Visible = false;
            panelUsuarios.Visible = false;
        }
        else if (dlEstadistico.SelectedValue == "4")
        {
            //obtieneBancos(periodos, ddlPeriodo.SelectedValue);
            dtBancos.DataBind();
            pnlMontos.Visible = false;
            pnlPagos.Visible = false;
            pnlAltas.Visible = false;
            pnlBajas.Visible = false;
            pnlBancos.Visible = true;
            panelUsuarios.Visible = false;
        }
        else if (dlEstadistico.SelectedValue == "5")
        {
            //obtieneInfoDetalles(ddlUsuario.SelectedValue, ddlPeriodo.SelectedValue, ddlAños.SelectedValue, periodos, "A");
            //obtieneInfoDetalles(ddlUsuario.SelectedValue, ddlPeriodo.SelectedValue, ddlAños.SelectedValue, periodos, "B");
            llenaDdlUsuarios();
            llenaDtUsuarios();
            pnlMontos.Visible = false;
            pnlPagos.Visible = false;
            pnlAltas.Visible = false;
            pnlBajas.Visible = false;
            pnlBancos.Visible = false;
            panelUsuarios.Visible = true;            
        }
    }

    private void obtienePagos(Label[] periodos, string periodo)
    {
        Datos datos = new Datos();
        int empresa = 0;
        try { empresa = Convert.ToInt32(Request.QueryString["e"]); }
        catch (Exception)
        {
            Response.Redirect("Default.aspx");
        }
        DataTable dt = new DataTable();
        int dato = Convert.ToInt32(periodo);
        if (periodo == "0")
        {
            dt = datos.obtienePagosEstadisticos(periodos, empresa);
            BarChartPagos.Height = 500;
        }
        else
        {
            dt = datos.obtienePagoEstadisticos(periodos[dato - 1], empresa, periodo);
            BarChartPagos.Height = 500;
        }
        
        BarChartPagos.Visible = true;
        BarChartPagos.PlotArea.Series.Clear();
        BarChartPagos.PlotArea.XAxis.Items.Clear();

        Telerik.Web.UI.ColumnSeries serieRec = new Telerik.Web.UI.ColumnSeries();
        Telerik.Web.UI.ColumnSeries seriePag = new Telerik.Web.UI.ColumnSeries();
        Telerik.Web.UI.ColumnSeries serieSuc = new Telerik.Web.UI.ColumnSeries();
        Telerik.Web.UI.ColumnSeries seriePen = new Telerik.Web.UI.ColumnSeries();        

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Telerik.Web.UI.CategorySeriesItem cRec = new Telerik.Web.UI.CategorySeriesItem();
            Telerik.Web.UI.CategorySeriesItem cPag = new Telerik.Web.UI.CategorySeriesItem();
            Telerik.Web.UI.CategorySeriesItem cSuc = new Telerik.Web.UI.CategorySeriesItem();
            Telerik.Web.UI.CategorySeriesItem cPen = new Telerik.Web.UI.CategorySeriesItem();

            cRec.Y = Convert.ToDecimal(dt.Rows[i][2].ToString());
            cPag.Y = Convert.ToDecimal(dt.Rows[i][1].ToString());
            cSuc.Y = Convert.ToDecimal(dt.Rows[i][3].ToString());
            //cPen.Y = Convert.ToDecimal(dt.Rows[i][4].ToString());
            cPen.Y = Convert.ToDecimal(dt.Rows[i][1].ToString()) + Convert.ToDecimal(dt.Rows[i][3].ToString());

            serieRec.SeriesItems.Add(cRec);
            seriePag.SeriesItems.Add(cPag);
            serieSuc.SeriesItems.Add(cSuc);
            seriePen.SeriesItems.Add(cPen);

            BarChartPagos.PlotArea.XAxis.Items.Add(new Telerik.Web.UI.AxisItem { LabelText = dt.Rows[i][0].ToString() });
        }

        BarChartPagos.PlotArea.Series.Add(serieRec);
        BarChartPagos.PlotArea.Series.Add(seriePag);
        BarChartPagos.PlotArea.Series.Add(serieSuc);
        BarChartPagos.PlotArea.Series.Add(seriePen);

        serieRec.Name = "Rechazados";
        seriePag.Name = "Pagados";
        serieSuc.Name = "Sucursal";
        seriePen.Name = "Total Pagados";

        

        BarChartPagos.PlotArea.Series[0].Appearance.FillStyle.BackgroundColor = Color.FromArgb(255, 69, 0);//orangered #FF4500
        BarChartPagos.PlotArea.Series[1].Appearance.FillStyle.BackgroundColor = Color.FromArgb(50, 205, 50);//limegreen
        BarChartPagos.PlotArea.Series[2].Appearance.FillStyle.BackgroundColor = Color.FromArgb(135, 206, 250);//lightskyble
        BarChartPagos.PlotArea.Series[3].Appearance.FillStyle.BackgroundColor = Color.FromArgb(211, 211, 211);//lightgray

        for(int j = 0; j < 4; j++)
        {
            BarChartPagos.PlotArea.Series[j].TooltipsAppearance.BackgroundColor = Color.FromArgb(0, 0, 0);
            BarChartPagos.PlotArea.Series[j].TooltipsAppearance.DataFormatString = "{0:F2}";
            BarChartPagos.PlotArea.Series[j].TooltipsAppearance.Color = Color.White;
        }
    }

    private void obtieneMontos(Label[] periodos, string periodo)
    {
        Datos datos = new Datos();
        int empresa = 0;
        try { empresa = Convert.ToInt32(Request.QueryString["e"]); }
        catch (Exception)
        {
            Response.Redirect("Default.aspx");
        }
        DataTable dt = new DataTable();
        int dato = Convert.ToInt32(periodo);
        if (periodo == "0")
        {
            dt = datos.obtieneMontosEstadisticos(periodos, empresa);
            BarChartMontos.Height = 500;
        }
        else
        {
            dt = datos.obtieneMontoEstadisticos(periodos[dato - 1], empresa, periodo);
            BarChartMontos.Height = 500;
        }

        BarChartMontos.Visible = true;
        BarChartMontos.PlotArea.Series.Clear();
        BarChartMontos.PlotArea.XAxis.Items.Clear();

        Telerik.Web.UI.ColumnSeries serieRec = new Telerik.Web.UI.ColumnSeries();
        Telerik.Web.UI.ColumnSeries seriePag = new Telerik.Web.UI.ColumnSeries();
        Telerik.Web.UI.ColumnSeries serieSuc = new Telerik.Web.UI.ColumnSeries();
        Telerik.Web.UI.ColumnSeries seriePen = new Telerik.Web.UI.ColumnSeries();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Telerik.Web.UI.CategorySeriesItem cRec = new Telerik.Web.UI.CategorySeriesItem();
            Telerik.Web.UI.CategorySeriesItem cPag = new Telerik.Web.UI.CategorySeriesItem();
            Telerik.Web.UI.CategorySeriesItem cSuc = new Telerik.Web.UI.CategorySeriesItem();
            Telerik.Web.UI.CategorySeriesItem cPen = new Telerik.Web.UI.CategorySeriesItem();
            
            cRec.Y = Convert.ToDecimal(dt.Rows[i][2].ToString());
            cPag.Y = Convert.ToDecimal(dt.Rows[i][1].ToString());
            cSuc.Y = Convert.ToDecimal(dt.Rows[i][3].ToString());
            //cPen.Y = Convert.ToDecimal(dt.Rows[i][4].ToString());
            cPen.Y = Convert.ToDecimal(dt.Rows[i][1].ToString()) + Convert.ToDecimal(dt.Rows[i][3].ToString());            

            serieRec.SeriesItems.Add(cRec);
            seriePag.SeriesItems.Add(cPag);
            serieSuc.SeriesItems.Add(cSuc);
            seriePen.SeriesItems.Add(cPen);
            
            BarChartMontos.PlotArea.XAxis.Items.Add(new Telerik.Web.UI.AxisItem { LabelText = dt.Rows[i][0].ToString() });
        }

        BarChartMontos.PlotArea.Series.Add(serieRec);
        BarChartMontos.PlotArea.Series.Add(seriePag);
        BarChartMontos.PlotArea.Series.Add(serieSuc);
        BarChartMontos.PlotArea.Series.Add(seriePen);

        serieRec.Name = "Rechazados";
        seriePag.Name = "Pagados";
        serieSuc.Name = "Sucursal";
        seriePen.Name = "Total Pagado";
                

        BarChartMontos.PlotArea.Series[0].Appearance.FillStyle.BackgroundColor = Color.FromArgb(255, 69, 0);//orangered #FF4500
        BarChartMontos.PlotArea.Series[1].Appearance.FillStyle.BackgroundColor = Color.FromArgb(50, 205, 50);//limegreen
        BarChartMontos.PlotArea.Series[2].Appearance.FillStyle.BackgroundColor = Color.FromArgb(135, 206, 250);//lightskyble
        BarChartMontos.PlotArea.Series[3].Appearance.FillStyle.BackgroundColor = Color.FromArgb(211, 211, 211);//lightgray

        for (int j = 0; j < 4; j++)
        {
            BarChartMontos.PlotArea.Series[j].TooltipsAppearance.BackgroundColor = Color.FromArgb(0, 0, 0);
            BarChartMontos.PlotArea.Series[j].TooltipsAppearance.DataFormatString = "{0:C2}";
            BarChartMontos.PlotArea.Series[j].TooltipsAppearance.Color = Color.White;
        }
    }

    private void obtieneAltas(Label[] periodos, string periodo)
    {
        Datos datos = new Datos();
        int empresa = 0;
        try { empresa = Convert.ToInt32(Request.QueryString["e"]); }
        catch (Exception)
        {
            Response.Redirect("Default.aspx");
        }
        DataTable dt = new DataTable();
        int dato = Convert.ToInt32(periodo);
        
        if (periodo == "0")
        {
            dt = datos.obtieneAltasEstadisticos(periodos);
            BarChartAltas.Height = 500;
        }
        else
        {
            dt = datos.obtieneAltasMensuales(periodos[dato - 1], periodo);
            BarChartAltas.Height = 500;
        }

        BarChartAltas.Visible = true;
        BarChartAltas.PlotArea.Series.Clear();
        BarChartAltas.PlotArea.XAxis.Items.Clear();

        Telerik.Web.UI.ColumnSeries serieRec = new Telerik.Web.UI.ColumnSeries();
       
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Telerik.Web.UI.CategorySeriesItem cRec = new Telerik.Web.UI.CategorySeriesItem();
           
            cRec.Y = Convert.ToDecimal(dt.Rows[i][1].ToString());
            
            serieRec.SeriesItems.Add(cRec);

            BarChartAltas.PlotArea.XAxis.Items.Add(new Telerik.Web.UI.AxisItem { LabelText = dt.Rows[i][0].ToString() });
        }

        BarChartAltas.PlotArea.Series.Add(serieRec);
        serieRec.Name = "Altas";
        
        BarChartAltas.PlotArea.Series[0].Appearance.FillStyle.BackgroundColor = Color.FromArgb(50, 205, 50);//limegreen        

        for (int j = 0; j < 1; j++)
        {
            BarChartAltas.PlotArea.Series[j].TooltipsAppearance.BackgroundColor = Color.FromArgb(0, 0, 0);
            BarChartAltas.PlotArea.Series[j].TooltipsAppearance.DataFormatString = "{0}";
            BarChartAltas.PlotArea.Series[j].TooltipsAppearance.Color = Color.White;
        }
    }

    private void obtieneBajas(Label[] periodos, string periodo)
    {
        Datos datos = new Datos();
        int empresa = 0;
        try { empresa = Convert.ToInt32(Request.QueryString["e"]); }
        catch (Exception)
        {
            Response.Redirect("Default.aspx");
        }
        DataTable dt = new DataTable();
        int dato = Convert.ToInt32(periodo);
        if (periodo == "0")
        {
            dt = datos.obtieneBajasEstadisticos(periodos, empresa);
            BarChartBajas.Height = 500;
        }
        else
        {
            dt = datos.obtieneBajasMensuales(periodos[dato - 1], periodo, empresa);
            BarChartBajas.Height = 500;
        }

        BarChartBajas.Visible = true;
        BarChartBajas.PlotArea.Series.Clear();
        BarChartBajas.PlotArea.XAxis.Items.Clear();

        Telerik.Web.UI.ColumnSeries serieRec = new Telerik.Web.UI.ColumnSeries();

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Telerik.Web.UI.CategorySeriesItem cRec = new Telerik.Web.UI.CategorySeriesItem();

            cRec.Y = Convert.ToDecimal(dt.Rows[i][1].ToString());

            serieRec.SeriesItems.Add(cRec);

            BarChartBajas.PlotArea.XAxis.Items.Add(new Telerik.Web.UI.AxisItem { LabelText = dt.Rows[i][0].ToString() });
        }

        BarChartBajas.PlotArea.Series.Add(serieRec);
        serieRec.Name = "Bajas";

        BarChartBajas.PlotArea.Series[0].Appearance.FillStyle.BackgroundColor = Color.FromArgb(255, 69, 0);//orangered #FF4500
        

        for (int j = 0; j < 1; j++)
        {
            BarChartBajas.PlotArea.Series[j].TooltipsAppearance.BackgroundColor = Color.FromArgb(0, 0, 0);
            BarChartBajas.PlotArea.Series[j].TooltipsAppearance.DataFormatString = "{0}";
            BarChartBajas.PlotArea.Series[j].TooltipsAppearance.Color = Color.White;
        }
    }

    /*private void obtieneBancos(Label[] periodos, string periodo)
    {
        Datos datos = new Datos();
        int empresa = 0;
        try { empresa = Convert.ToInt32(Request.QueryString["e"]); }
        catch (Exception)
        {
            Response.Redirect("Default.aspx");
        }
        DataTable dt = new DataTable();
        int dato = Convert.ToInt32(periodo);
        int banco = 0;
        try { banco = Convert.ToInt32(ddlBancos.SelectedValue); }
        catch (Exception) { banco = 0; }
        if (periodo == "0")
            dt = datos.obtienePagosBancos(empresa, banco);
        else
            dt = datos.obtienePagoBanco(periodos[dato - 1], empresa, periodo, banco);

        string[] meses = new string[dt.Rows.Count];
        decimal[] pagados = new decimal[dt.Rows.Count];
        decimal[] rechazados = new decimal[dt.Rows.Count];

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            meses[i] = dt.Rows[i][0].ToString();
            try { pagados[i] = Convert.ToDecimal(dt.Rows[i][1]); }
            catch (Exception) { pagados[i] = Convert.ToDecimal(0); }
            try { rechazados[i] = Convert.ToDecimal(dt.Rows[i][2]); }
            catch (Exception) { rechazados[i] = Convert.ToDecimal(0); }
        }


        BarChart5.Series.Add(new AjaxControlToolkit.BarChartSeries { Name = "Pagados", Data = pagados });
        BarChart5.Series.Add(new AjaxControlToolkit.BarChartSeries { Name = "Rechazados", Data = rechazados });

        BarChart5.Series[0].BarColor = "LimeGreen";
        BarChart5.Series[1].BarColor = "OrangeRed";

        BarChart5.CategoriesAxis = string.Join(",", meses);
        if (periodo == "0")
        {
            pnlBancos.CssClass = "ancho100 centardo";
            BarChart5.ChartWidth = (meses.Length * 90).ToString();
        }
        else
        {
            pnlBancos.CssClass = "ancho100 centardo padingLeft30px";
            BarChart5.ChartWidth = (meses.Length * 300).ToString();
        }
    }
    
    protected void ddlBancos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
        dtBancos.DataBind();
        obtieneBancos(periodos, ddlPeriodo.SelectedValue);
    }*/

    protected void dtBancos_ItemDataBound(object sender, DataListItemEventArgs e)
    {        
        Telerik.Web.UI.RadHtmlChart grafica = (Telerik.Web.UI.RadHtmlChart)e.Item.FindControl("DonutChart1") as Telerik.Web.UI.RadHtmlChart;
        Label lblIdBanco = (Label)e.Item.FindControl("lblIdBanco") as Label;
        Datos datos = new Datos();
        int empresa = 0;
        
        try { empresa = Convert.ToInt32(Request.QueryString["e"]); }
        catch (Exception)
        {
            Response.Redirect("Default.aspx");
        }
        DataTable dt = new DataTable();
        string periodo = ddlPeriodo.SelectedValue;
        Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
        
        int dato = Convert.ToInt32(periodo);
        int banco = 0;
        try { banco = Convert.ToInt32(lblIdBanco.Text); }
        catch (Exception) { banco = 0; }
        if (periodo == "0")
            dt = datos.obtienePagosBancos(empresa, banco);
        else
            dt = datos.obtienePagoBanco(periodos[dato - 1], empresa, periodo, banco);

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

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            Telerik.Web.UI.PieSeriesItem cRec = new Telerik.Web.UI.PieSeriesItem();
            Telerik.Web.UI.PieSeriesItem cPag = new Telerik.Web.UI.PieSeriesItem();
           
            cRec.Y = Convert.ToDecimal(dt.Rows[i][2].ToString());
            cPag.Y = Convert.ToDecimal(dt.Rows[i][1].ToString());

            cRec.Exploded = cPag.Exploded = false;
            cRec.BackgroundColor= Color.FromArgb(255, 69, 0);
            cPag.BackgroundColor = Color.FromArgb(50, 205, 50);
            cRec.Name = "Rechazados";
            cPag.Name = "Pagados";

            serieRec.SeriesItems.Add(cRec);
            serieRec.SeriesItems.Add(cPag);
        }        
        grafica.PlotArea.Series.Add(serieRec);        

    }

    /*protected void ddlUsuario_SelectedIndexChanged(object sender, EventArgs e)
    {
        Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
        obtieneInfoDetalles(ddlUsuario.SelectedValue, ddlPeriodo.SelectedValue, ddlAños.SelectedValue, periodos, "A");
        obtieneInfoDetalles(ddlUsuario.SelectedValue, ddlPeriodo.SelectedValue, ddlAños.SelectedValue, periodos, "B");
    }*/

    protected void ddlAños_SelectedIndexChanged(object sender, EventArgs e)
    {
        llenaDdlUsuarios();
        llenaDtUsuarios();
    }

    protected void radGridAltaUser_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        //Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
        //obtieneInfoDetalles(ddlUsuario.SelectedValue, ddlPeriodo.SelectedValue, ddlAños.SelectedValue, periodos, "A");
        llenaDtUsuarios();
    }

    protected void radGridAltaUser_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        //Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
        //obtieneInfoDetalles(ddlUsuario.SelectedValue, ddlPeriodo.SelectedValue, ddlAños.SelectedValue, periodos, "A");
        llenaDtUsuarios();
    }

    protected void radGridAltaUser_PageSizeChanged(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
    {
        //Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
        //obtieneInfoDetalles(ddlUsuario.SelectedValue, ddlPeriodo.SelectedValue, ddlAños.SelectedValue, periodos,"A");
        llenaDtUsuarios();
    }

    protected void radGridBajasUser_PageIndexChanged(object sender, Telerik.Web.UI.GridPageChangedEventArgs e)
    {
        //Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
        //obtieneInfoDetalles(ddlUsuario.SelectedValue, ddlPeriodo.SelectedValue, ddlAños.SelectedValue, periodos, "B");
        llenaDtUsuarios();
    }

    protected void radGridBajasUser_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
    {
        //Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
        //obtieneInfoDetalles(ddlUsuario.SelectedValue, ddlPeriodo.SelectedValue, ddlAños.SelectedValue, periodos, "B");
        llenaDtUsuarios();
    }

    protected void radGridBajasUser_PageSizeChanged(object sender, Telerik.Web.UI.GridPageSizeChangedEventArgs e)
    {
        //Label[] periodos = { Label1, Label2, Label3, Label4, Label5, Label6, Label7, Label8, Label9, Label10, Label11, Label12 };
        //obtieneInfoDetalles(ddlUsuario.SelectedValue, ddlPeriodo.SelectedValue, ddlAños.SelectedValue, periodos, "B");
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
        try { empresa = Convert.ToInt32(Request.QueryString["e"]); }
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
            fecha_inicio = Convert.ToDateTime(año+"-" + periodo_indicado.PadLeft(2, '0') + "-01");
            fecha_final = fecha_inicio.AddYears(1);
            fecha_final = fecha_final.AddDays(-1);
        }
        else
        {
            fecha_inicio = Convert.ToDateTime(año+"-" + periodo.PadLeft(2, '0') + "-01");
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
        


        if (altas == 0 && bajas != 0 && bajasInm==0)
            RadTabStrip1.SelectedIndex = RadMultiPage1.SelectedIndex = 1;
        else if(altas!=0 && bajas==0 && bajasInm==0)
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

    protected void ddlUsuarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        llenaDtUsuarios();
    }
}