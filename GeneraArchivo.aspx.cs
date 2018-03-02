using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Configuration;
using System.Net;
using System.Drawing;
using PdfViewer1;

public partial class GeneraArchivo : System.Web.UI.Page
{
    Datos datos = new Datos();
    DataTable dt = new DataTable();
    ObtieneFechaActual fechaLocal = new ObtieneFechaActual();
    int total, clientes; double montoTotal;

    protected void Page_Load(object sender, EventArgs e)
    {
        int empresa = Convert.ToInt32(Session["e"]);
        if (empresa == 0)
        {
            Response.Redirect("Default.aspx");

        }
        else
        {

            string usuario = Session["u"].ToString();
            string nombre = Session["nu"].ToString();
            int cont = Convert.ToInt32(Session["C"]);
            string aspx = Session["aspx"].ToString();
        }

        string url = Request.Url.ToString();


        if (url.Contains("?"))
        {

            url = url.Split(new Char[] { '?' })[0];

        }
        if (!IsPostBack) {            
            GridView1.DataSource = null;
            GridView1.DataBind();
            btnGenerar.Visible = false;
            lblerror.Text = "";
        }
    }

    private void cargaDatos()
    {        
        dt.Clear();
        total = clientes = 0;
        montoTotal = 0;
        string usuario = ""; 
        int empresa = 0;
        try { usuario = Convert.ToString(Session["u"].ToString()); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        try { empresa = Convert.ToInt32(Session["e"].ToString()); }
        catch (Exception) { Response.Redirect("Default.aspx"); }

        DateTime periodoProceso = Convert.ToDateTime(ddlAno.SelectedValue.PadLeft(4, '0') + "-" + ddlPeriodo.SelectedValue.PadLeft(2, '0') + "-01");


        datos.creaTabla();
        datos.borraTmp();
        dt = new DataTable();
        dt.Columns.Add("afiliacion");
        dt.Columns.Add("referencia");
        dt.Columns.Add("cuenta");
        dt.Columns.Add("monto");        
        DataRow filas;
        DataSet ds = new DataSet();
        int conteos = 0;
        while (dt.Rows.Count == 0)
        {
            if (conteos < 5)
            {
                ds = datos.obtieneCuentas(usuario, empresa, "M");
                if (ds != null)
                {
                    int contador = 0;
                    foreach (DataRow fila in ds.Tables[0].Rows)
                    {
                        DataSet tieneMasRegistros = new DataSet();
                        tieneMasRegistros = datos.obtieneRegistros(fila[1].ToString(), "M", empresa, periodoProceso);
                        if (tieneMasRegistros.Tables[0].Rows.Count > 0)
                        {
                            string referencias = "";
                            double montos = 0;
                            int i = 0;
                            int numRef = 0;
                            foreach (DataRow filaInfo in tieneMasRegistros.Tables[0].Rows)
                            {
                                if (numRef < 4)
                                {
                                    if (i == 0)
                                    {
                                        referencias = filaInfo[0].ToString();
                                        clientes++;
                                        numRef++;
                                    }
                                    else
                                    {
                                        referencias = referencias.Trim() + "-" + filaInfo[0].ToString();
                                        clientes++;
                                        numRef++;
                                    }
                                }
                                montos = montos + Convert.ToDouble(filaInfo[1].ToString());
                                i++;
                            }
                            
                            datos.agregaPago(fila[0].ToString(), referencias, fila[1].ToString(), montos.ToString());
                            filas = dt.NewRow();
                            filas["afiliacion"] = fila[0].ToString();
                            filas["referencia"] = referencias;
                            filas["cuenta"] = fila[1].ToString();
                            filas["monto"] = montos.ToString("0.00");
                            dt.Rows.Add(filas);
                            contador++;
                        }
                    }

                    dt = datos.obtieneTmpPagos();

                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    if (total == 0)
                        btnGenerar.Visible = false;
                    else
                        btnGenerar.Visible = true;
                    lblTotal.Text = total.ToString();
                    
                    lblMontoTotal.Text = montoTotal.ToString("C2");
                    lblClientes.Text = clientes.ToString();
                }
                else
                    break;
                conteos++;
            }
            else
                break;
        }

        if (conteos > 4)
            lblerror.Text = "Se ha detectado un problema de conexión con la base de datos, por favor verifique su conexión e intente más tarde";
    }

    private string[] obtieneDatos(object obj)
    {
        string[] datos = new string[2];
        IEnumerable<object> coleccion = (IEnumerable<object>)obj;
        int valor = 0;
        foreach (object item in coleccion)
        {
            datos[valor] = Convert.ToString(item);
            valor++;
        }
        return datos;
    }

    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {        
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            total++;
            double monto = Convert.ToDouble( DataBinder.Eval(e.Row.DataItem, "monto").ToString());
            montoTotal = montoTotal + monto;            
        }
    }

    protected void btnGenerar_Click(object sender, ImageClickEventArgs e)
    {
        lblerror.Text = "";
        int empresa =0;
        try{empresa=Convert.ToInt32(Session["e"].ToString());}catch(Exception){Response.Redirect("Default.aspx");}
        string usuario = "";
        try{usuario=Convert.ToString(Session["u"].ToString());}catch(Exception){Response.Redirect("Default.aspx");}
        double montoTotalArchivo = 0;
        string montoSesion = "0";

        try { montoSesion = datos.obtieneMontoPagar().ToString(); }
        catch (Exception) { montoSesion = "0"; }
        string VALORmONTO = montoSesion.Trim();
        //string VALORmONTO = montoSesion.Replace('$',' ');
        //VALORmONTO = VALORmONTO.Replace(',', ' ');
        //VALORmONTO = VALORmONTO.Trim();
        try { montoTotalArchivo = Convert.ToDouble(VALORmONTO); }
        catch (Exception) { montoTotalArchivo = 0; }
        int elementos = 0;
        DateTime periodoProceso = Convert.ToDateTime(ddlAno.SelectedValue.PadLeft(4, '0') + "-" + ddlPeriodo.SelectedValue.PadLeft(2, '0') + "-01");
        string periodo = periodoProceso.ToString("yyyyMM");
        int consecutivo = datos.obtieneConsecutivo(periodo, empresa);

        if (consecutivo == 99)
            consecutivo = 1;
        else
            consecutivo++;


        // Se carga la ruta física de la carpeta temp del sitio
        string digitosAfiliacion=datos.obtieneDigitosAfiliacion(usuario);
        digitosAfiliacion = digitosAfiliacion.Substring(digitosAfiliacion.Length-4,4);
        string fecha = fechaLocal.obtieneFechaLocal().ToString("yyMMdd");
        string fechaArchivo =fechaLocal.obtieneFechaLocal().ToString("ddMMyyyy")+fechaLocal.obtieneFechaLocal().ToString("HHmm")+"00";
        string ruta = Server.MapPath("~/Doctos/Emp" + empresa.ToString());
        string rutaVirtual = "~/Doctos/Emp" + empresa.ToString();
        //Nombre Archivo
        string nombre = ConfigurationManager.AppSettings["prefijo"].ToString() + ConfigurationManager.AppSettings["digitos"].ToString() + "D" + fecha + ConfigurationManager.AppSettings["sufijo"].ToString() + consecutivo.ToString().PadLeft(2, '0') + "." + ConfigurationManager.AppSettings["extension"].ToString();
        // Si el directorio no existe, crearlo
        if (!Directory.Exists(ruta))
            Directory.CreateDirectory(ruta);
        /* base para descarga */
        string archivoDescarga = ruta + "\\" + ConfigurationManager.AppSettings["prefijo"].ToString() + ConfigurationManager.AppSettings["digitos"].ToString() + "D" + fecha + ConfigurationManager.AppSettings["sufijo"].ToString() + consecutivo.ToString().PadLeft(2, '0') + "." + ConfigurationManager.AppSettings["extension"].ToString();
        FileInfo descarga = new FileInfo(archivoDescarga);
        if (descarga.Exists)
            descarga.Delete();
        StreamWriter archivo = new StreamWriter(archivoDescarga);
        string linea="";
        linea = fechaArchivo.PadRight(14, ' ') + lblTotal.Text.PadLeft(6, ' ') + montoTotalArchivo.ToString("0.00").PadLeft(16,' ');
        linea = linea.PadRight(67, ' ');
        archivo.WriteLine(linea);

        dt = datos.obtienePagosArchivo();
        foreach (DataRow fila in dt.Rows) 
        {
            string afiliacion, referencia, cuenta, monto ="";
            afiliacion = fila[1].ToString().Trim().PadRight(9, ' ');            
            referencia = fila[2].ToString().Trim().PadRight(23, ' ');
            cuenta = fila[3].ToString().Trim().PadRight(16,' ');            
            monto = fila[4].ToString().Trim() + "00";
            monto = monto.PadLeft(19, ' ');
            linea = afiliacion + referencia + cuenta + monto;            
            elementos++;
            archivo.WriteLine(linea);
        }        
        archivo.Close();
        string ruta2 = ruta.Replace('\\', '/');
        bool documentoRegistrado = datos.llenaRegistroCobranza(periodo, consecutivo, nombre, elementos, montoTotalArchivo, ruta2, empresa.ToString());
        if (documentoRegistrado)
        {
            try
            {
                System.IO.FileInfo file = new System.IO.FileInfo(archivoDescarga);
                if (file.Exists)
                {
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"; //"application/text"; //vnd.msword.document.12"; //vnd.openxmlformats-officedocument.wordprocessingml.document";//vnd.ms-word.document"; //x-zip-compressed";
                    Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);
                    Response.WriteFile(archivoDescarga);
                    Response.End();
                }
            }
            catch (Exception)
            {

            }
            lblerror.Text = "Archivo Generado";
        }
    }
    
    protected void btnGenerarArchivo_Click(object sender, ImageClickEventArgs e)
    {
        cargaDatos();        
    }
    protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblerror.Text = "";
        GridView1.DataSource = null;
        GridView1.DataBind();
        lblTotal.Text = 0.ToString();
        
        lblMontoTotal.Text = 0.ToString("C2");
        lblClientes.Text = 0.ToString();
        total = clientes = 0; montoTotal = 0;
        dt.Rows.Clear();
        btnGenerar.Visible = false;
    }
    protected void btnConsulta_Click(object sender, EventArgs e)
    {       
        GridView2.DataBind();
        Panel4.Visible = true;
        Panel6.Visible = true;
    }
    protected void btnCerrar_Click(object sender, ImageClickEventArgs e)
    {
        Panel4.Visible = false;
        Panel6.Visible = false;
    }
}