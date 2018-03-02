using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Text;
using System.Web.SessionState;
using System.Data;

public partial class Lectura : System.Web.UI.Page
{
    object[] objetoLectura = new object[100];
    object[] cuentasDesdeLineas = new object[100];
    object[] desgCuentasRechazadas = new object[100];
    object[] desgCuentasPagadas = new object[100];
    string movimiento;
    string cuenta, referencia, name, motivoRechazo, fechaRechazo, fechaPago, monto, periodoConvertido, fechaRechazoConvertida;
    DateTime periodo;
    int clientReal, clientRec, ctasReal, ctasRec;
    int consecutivo, idCliente, PagoORechazo, noExisteUsuario, registrosTotales;
    int totalCuentasReal , totalCuentasRechazadas ;
    double MontoclientReal , MontoclientRec ; 
    Datos datos = new Datos();
    
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

        if (!IsPostBack)
        {
            btnCargarDoc.Visible = false;
            GridCargaRespuesta.DataSource = null;
            GridCargaRespuesta.DataBind();
            Session["iteracion"] = 1;
            Session["ctaReal"] = Session["ctaRec"] = Session["cReal"] = Session["cRec"] = Session["mReal"] = Session["mRec"] = null;
            GridCargaRespuesta.EmptyDataText = "Debe seleccionar un periodo y cargar un archivo para procesar.";
            lnkNotificar.Visible = false;
        }            
    }

    private void cargaDatos()
    {        
        string usuario = "";
        try { usuario = Session["u"].ToString(); }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        lblError.Text = "";
        DataSet data = new DataSet();
        data = datos.llenaGridRespuesta( usuario);

        GridCargaRespuesta.EmptyDataText = "Debe seleccionar un periodo y cargar un archivo para procesar.";
        try
        {
            int totOperaciones = 0, opeR = 0, opeP = 0;
            totOperaciones = Convert.ToInt32(lblTotalCuentas.Text);
            opeR = Convert.ToInt32(lblCtaRec.Text);
            opeP = Convert.ToInt32(lblCtaReal.Text);
            if (opeR == totOperaciones)
                GridCargaRespuesta.EmptyDataText = "Se han rechazado todos los movimientos";
            if (opeP == totOperaciones)
                GridCargaRespuesta.EmptyDataText = "Ya se han pagado todos los movimientos";
            if (opeR + opeP == totOperaciones)
                GridCargaRespuesta.EmptyDataText = "El archivo cargado ya se ha procesado con anterioridad";
        }
        catch (Exception)
        {
            GridCargaRespuesta.EmptyDataText = "Debe seleccionar un periodo y cargar un archivo para procesar.";
        }

        try { GridCargaRespuesta.DataSource = data; }
        catch (Exception) { GridCargaRespuesta.DataSource = null; }
        GridCargaRespuesta.DataBind();

        borraArchivo();
        
    }

    private void actualizaDetalleClienteRechazados(string año, string mes)//rechazado
    {
        bool existeCliente = false;
        bool insertado = false;        
        int pagado = 0;
        int rechazado = 0;
        string[] referenciaArreglo = referencia.Split('-');
        int referencias = Convert.ToInt32(referenciaArreglo.Length);
        string usuario = "";
        string idEmpresa = "";
        try {
            usuario = Session["u"].ToString();
            idEmpresa = Session["e"].ToString();
        }
        catch (Exception) { Response.Redirect("Default.aspx"); }
                
        if (referencias > 1)
        {
            registrosTotales = registrosTotales + referencias;
            registrosTotales = registrosTotales - 1;
            decimal grupoUsuarios = Convert.ToDecimal(referenciaArreglo.Length);
            decimal montoDecimal = Convert.ToDecimal(monto);
            decimal monto2 = montoDecimal / grupoUsuarios;
            int cuentaPeriodos=0;
            bool registrado;
            if(!datos.existeCargaPervia(periodoConvertido, idEmpresa, Session["carga"].ToString(), usuario))
                registrado = datos.generaBitacora(periodoConvertido, idEmpresa, Session["carga"].ToString(), usuario);
            for (int z = 0; z < referenciaArreglo.Length; z++)
            {
                try { clientRec = Convert.ToInt32(Session["cRec"]); clientReal = Convert.ToInt32(Session["cReal"]); }
                catch (Exception) { clientReal = clientRec = 0; }
                existeCliente = datos.existeClientePeriodo(referenciaArreglo[z].ToString(),periodoConvertido);
                if (movimiento == "R")
                {
                    clientRec++;
                    Session["cRec"] = clientRec;
                }
                else if (movimiento == "P")
                {
                    clientReal++;
                    Session["cReal"] = clientReal;
                }

                if (existeCliente)
                {
                    idCliente = datos.obtieneIdCliente(referenciaArreglo[z].ToString());
                    pagado = datos.validaPagoRealizado(idCliente, periodoConvertido);
                    rechazado = datos.validaPagoRechazado(idCliente, periodoConvertido);
                    cuentaPeriodos = datos.cuentaPeriodosPagosPorAño(idCliente, año);
                    if ( rechazado < 1 && movimiento == "R")
                    {
                        if (pagado < 1)
                        {
                            consecutivo = datos.obtieneIdConsecutivoDetalleCliente(idCliente) + 1;
                            insertado = datos.agregaDetalleCliente(idCliente, periodoConvertido, fechaRechazoConvertida, motivoRechazo, movimiento, idEmpresa, consecutivo, usuario);
                            insertado = datos.agregaRegistroTMP(idCliente, referenciaArreglo[z].ToString(), periodoConvertido, fechaRechazoConvertida, cuenta, movimiento, motivoRechazo, monto2.ToString(), usuario);
                            if (cuentaPeriodos>0)
                                insertado = datos.actualizarPeriodosPagos(idCliente, movimiento, año, mes);
                            else if (cuentaPeriodos==0)
                                insertado = datos.agregarPeriodosPagos(idCliente, movimiento, año, mes);
                        }
                        else
                            noExisteUsuario++;
                    }
                    else if (rechazado >= 1 && movimiento == "R") {
                        consecutivo = datos.obtieneIdConsecutivoDetalleCliente(idCliente) + 1;
                        insertado = datos.agregaDetalleCliente(idCliente, periodoConvertido, fechaRechazoConvertida, motivoRechazo, movimiento, idEmpresa, consecutivo, usuario);
                    }
                    else if (pagado < 1 && movimiento == "P")
                    {
                        consecutivo = datos.obtieneIdConsecutivoDetalleCliente(idCliente) + 1;
                        insertado = datos.agregaDetalleCliente(idCliente, periodoConvertido, fechaRechazoConvertida, motivoRechazo, movimiento, idEmpresa, consecutivo, usuario);
                        insertado = datos.agregaRegistroTMP(idCliente, referenciaArreglo[z].ToString(), periodoConvertido, fechaRechazoConvertida, cuenta, movimiento, motivoRechazo, monto2.ToString(), usuario);
                        if (cuentaPeriodos > 0)
                            insertado = datos.actualizarPeriodosPagos(idCliente, movimiento, año, mes);
                        else if (cuentaPeriodos == 0)
                            insertado = datos.agregarPeriodosPagos(idCliente, movimiento, año, mes);
                    }
                    else
                        noExisteUsuario++;
                }
                else
                    noExisteUsuario++;


            }
        }
        else
        {
            bool registrado;
            if (!datos.existeCargaPervia(periodoConvertido, idEmpresa, Session["carga"].ToString(), usuario))
                registrado = datos.generaBitacora(periodoConvertido, idEmpresa, Session["carga"].ToString(), usuario);
            existeCliente = datos.existeClientePeriodo(referenciaArreglo[0].ToString(),periodoConvertido);
            try { clientRec = Convert.ToInt32(Session["cRec"]); clientReal = Convert.ToInt32(Session["cReal"]); }
            catch (Exception) { clientReal = clientRec = 0; }
            if (movimiento == "R")
            {
                clientRec++;
                Session["cRec"] = clientRec;
            }
            else if (movimiento == "P")
            {
                clientReal++;
                Session["cReal"] = clientReal;
            }

            if (existeCliente)
            {                
                
                idCliente = datos.obtieneIdCliente(referenciaArreglo[0].ToString());
                int cuentaPeriodos = datos.cuentaPeriodosPagosPorAño(idCliente, año);
                pagado = datos.validaPagoRealizado(idCliente, periodoConvertido);
                rechazado = datos.validaPagoRechazado(idCliente, periodoConvertido);

                

                if (rechazado < 1 && movimiento == "R")
                {
                    if (pagado < 1)
                    {
                        consecutivo = datos.obtieneIdConsecutivoDetalleCliente(idCliente) + 1;
                        insertado = datos.agregaDetalleCliente(idCliente, periodoConvertido, fechaRechazoConvertida, motivoRechazo, movimiento, idEmpresa, consecutivo, usuario);
                        insertado = datos.agregaRegistroTMP(idCliente, referenciaArreglo[0].ToString(), periodoConvertido, fechaRechazoConvertida, cuenta, movimiento, motivoRechazo, monto, usuario);
                        if (cuentaPeriodos > 0)
                            insertado = datos.actualizarPeriodosPagos(idCliente, movimiento, año, mes);
                        else if (cuentaPeriodos == 0)
                            insertado = datos.agregarPeriodosPagos(idCliente, movimiento, año, mes);
                    }else
                        noExisteUsuario++;
                }
                else if (rechazado >= 1 && movimiento == "R")
                {
                    consecutivo = datos.obtieneIdConsecutivoDetalleCliente(idCliente) + 1;
                    insertado = datos.agregaDetalleCliente(idCliente, periodoConvertido, fechaRechazoConvertida, motivoRechazo, movimiento, idEmpresa, consecutivo, usuario);
                }
                else if (pagado < 1 && movimiento == "P")
                {
                    consecutivo = datos.obtieneIdConsecutivoDetalleCliente(idCliente) + 1;
                    insertado = datos.agregaDetalleCliente(idCliente, periodoConvertido, fechaRechazoConvertida, motivoRechazo, movimiento, idEmpresa, consecutivo, usuario);
                    insertado = datos.agregaRegistroTMP(idCliente, referenciaArreglo[0].ToString(), periodoConvertido, fechaRechazoConvertida, cuenta, movimiento, motivoRechazo, monto, usuario);
                    if (cuentaPeriodos > 0)
                        insertado = datos.actualizarPeriodosPagos(idCliente, movimiento, año, mes);
                    else if (cuentaPeriodos == 0)
                        insertado = datos.agregarPeriodosPagos(idCliente, movimiento, año, mes);
                }
                else
                    noExisteUsuario++;
            }
            else
                noExisteUsuario++;
        }
    }
    
    private void GuardarArchivo(HttpPostedFile file)
    {
        // Se carga la ruta física de la carpeta temp del sitio
        string ruta = Server.MapPath("~/TMP");

        // Si el directorio no existe, crearlo
        if (!Directory.Exists(ruta))
            Directory.CreateDirectory(ruta);

        string archivo = String.Format("{0}\\{1}", ruta, file.FileName);

        // Verificar que el archivo no exista
        if (File.Exists(archivo))
            lblError.Text = (String.Format("Ya existe Documento con nombre\"{0}\".", file.FileName));
        else
        {
            file.SaveAs(archivo);
        }
    }

    public void lectura(string archivo)
    {
        string ruta = Server.MapPath("~/TMP");
        lblError.Text = "";
        datos.borrarTempCargaDatos();
        int i, j, k, m, yy;
        bool inicioCuentas = false;
        bool[] cuentaJ = { false, false };
        bool[] cuentak = { true, true };
        string registroSt;
        string[] registroAr = new string[20];
        //bool temp = true;
        /*try
        {
            if (fupRespuesta.HasFile)
            {
                //validar formato
                //string ext = fupRespuesta.PostedFile.FileName;
                //ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower();
                //string[] formatos = new string[] { "txt" };
                //if (Array.IndexOf(formatos, ext) < 0)
                    //lblError.Text = ("Formato del Documento inválido.");
                GuardarArchivo(fupRespuesta.PostedFile);
            }
            else
                lblError.Text = ("Seleccione un archivo del disco duro.");
        }
        catch (Exception)
        {
            lblError.Text = "Se perdio la conexión, recargue lapagina e intentelo nuevamente";
        }*/
        name = archivo;
        var lines = File.ReadAllLines(name, Encoding.Default).ToList();
        i = 0;
        j = 0;
        k = 0;        
        m = 0;        
        objetoLectura = new object[lines.Count];
        cuentasDesdeLineas = new object[lines.Count];

        

        totalCuentasReal = 0; totalCuentasRechazadas = 0;
        MontoclientReal = 0; MontoclientRec = 0;        
        for (yy = 0; yy < lines.Count; yy++)
        {
            objetoLectura[i] = lines[i];
            inicioCuentas = parseo(objetoLectura[i]);
            cuentaJ[0] = cuentaJ[1];
            cuentaJ[1] = inicioCuentas;
            if (cuentaJ[0] != cuentaJ[1])
                j++;
            if (inicioCuentas)
            {
                if (j != 0)
                {
                    if ((j % 2) != 0)
                    {
                        cuentasDesdeLineas[k] = lines[i];
                        k++;
                    }
                }
            }
            i++;
        }
        char separador = ' ';
        string[] cuentasDesdeLineasString = new string[lines.Count];
        for (i = 0; i < cuentasDesdeLineas.Length; i++)
        {
            if (cuentasDesdeLineas[i] != null)
            {
                cuentasDesdeLineasString[i] = cuentasDesdeLineas[i].ToString();
            }
            else
                break;
        }
        cuentasDesdeLineasString = cuentasDesdeLineasString.Where(x => !string.IsNullOrEmpty(x)).ToArray();//rechazadas
        registrosTotales = cuentasDesdeLineasString.Length;
        lblTotalCuentas.Text = registrosTotales.ToString();
         

        for (i = 0; i < cuentasDesdeLineasString.Length; i++)
        {            
            registroSt = cuentasDesdeLineasString[i].ToString();
            registroAr = registroSt.Split(separador);
            registroAr = registroAr.Where(x => !string.IsNullOrEmpty(x.Trim())).ToArray();
            cuenta = registroAr[0];
            referencia = registroAr[1];
            fechaRechazo = registroAr[2];
            bool rec_pag;
            int autorizacion = 0;
            try
            {
                PagoORechazo = Convert.ToInt32(registroAr[4].ToString());
                rec_pag = false;
            }
            catch (Exception) { rec_pag = true; }
            if (rec_pag)
            {
                try
                {
                    autorizacion = Convert.ToInt32(registroAr[5].ToString());
                    rec_pag = true;
                }
                catch (Exception) { rec_pag = false; }
            }
            if (!rec_pag)
            {
                try
                {
                    m = 5;
                    for (k = 5; k < registroAr.Length; k++)
                    {
                        if (k == 5)
                            motivoRechazo = registroAr[k];
                        else
                            motivoRechazo += " " + registroAr[k];
                        m++;
                        try
                        {
                            PagoORechazo = Convert.ToInt32(registroAr[m]);
                            break;
                        }
                        catch (Exception) { }
                    }
                    monto = registroAr[registroAr.Length - 2];
                    string[] peri = fechaRechazo.Split();
                    fechaRechazo = "20" + fechaRechazo.Substring(0, 2) + "-" + fechaRechazo.Substring(2, 2) + "-" + fechaRechazo.Substring(4, 2);
                    periodo = Convert.ToDateTime(fechaRechazo);
                    string año = ddlAno.SelectedValue.PadLeft(4, '0');
                    string mes = ddlPeriodo.SelectedValue.PadLeft(2, '0');
                    periodoConvertido = año + mes;
                    fechaRechazoConvertida = periodo.ToString("yyyy-MM-dd");
                    movimiento = "R";
                    try { totalCuentasRechazadas = Convert.ToInt32(Session["ctaRec"]); MontoclientRec = Convert.ToDouble(Session["mRec"]); }
                    catch (Exception) { totalCuentasRechazadas = 0; MontoclientRec = 0; }
                    totalCuentasRechazadas++;
                    Session["ctaRec"] = totalCuentasRechazadas;
                    
                    MontoclientRec = MontoclientRec + Double.Parse(monto,System.Globalization.NumberStyles.Currency);
                    Session["mRec"] = MontoclientRec;
                    actualizaDetalleClienteRechazados(año, mes);
                    motivoRechazo = "";
                }
                catch (Exception) { noExisteUsuario++; }
            }
            else
            {
                try
                {
                    motivoRechazo = "";
                    monto = registroAr[registroAr.Length - 2];
                    string[] peri2 = fechaRechazo.Split();
                    fechaPago = "20" + fechaRechazo.Substring(0, 2) + "-" + fechaRechazo.Substring(2, 2) + "-" + fechaRechazo.Substring(4, 2);
                    periodo = Convert.ToDateTime(fechaPago);
                    string año2 = ddlAno.SelectedValue.PadLeft(4, '0');
                    string mes2 = ddlPeriodo.SelectedValue.PadLeft(2, '0');
                    periodoConvertido = año2 + mes2;
                    fechaRechazoConvertida = periodo.ToString("yyyy-MM-dd");
                    movimiento = "P";
                    try { totalCuentasReal = Convert.ToInt32(Session["ctaReal"]); MontoclientReal = Convert.ToDouble(Session["mReal"]); }
                    catch (Exception) { totalCuentasReal = 0; MontoclientReal = 0; }
                    totalCuentasReal++;
                    Session["ctaReal"] = totalCuentasReal;
                    MontoclientReal = MontoclientReal + Double.Parse(monto, System.Globalization.NumberStyles.Currency);
                    Session["mReal"] = MontoclientReal;
                    actualizaDetalleClienteRechazados(año2, mes2);
                }
                catch (Exception) { noExisteUsuario++; }
            }
        }
       
       
        
        ddlPeriodo.SelectedValue = "0";
        btnCargarDoc.Visible = false;
        Panel4.Visible = false;
        txtProceso.Text = "";       
    }

    public bool parseo(object objeto)
    {
        bool numeroCuenta = false;
        char separador=' ';
        string[] arregloSplit = Convert.ToString(objeto).Split(separador);
        string cuenta = arregloSplit[0].ToString();
        int totalCuenta = 16;
        if (totalCuenta == Convert.ToInt32(cuenta.Length))
        {
            numeroCuenta= true;
        }
        return numeroCuenta;
    }

    protected void GridCargaRespuesta_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                Label lblMovimiento = (Label)e.Row.FindControl("lblMovimiento");
                if (rowView[4].ToString() == "R")
                    e.Row.ForeColor = System.Drawing.Color.Red;
            }
        }
        catch (Exception) { }
    }

    protected void btnCargarDoc_Click(object sender, ImageClickEventArgs e) 
    {
        if (ddlPeriodo.SelectedValue == "0")
            lblError.Text = "Debe indicar el periodo a aplicar los pagos";
        else
        {
            string archivoCarga;
            try { archivoCarga = Convert.ToString(Session["carga"]); }
            catch (Exception) { archivoCarga = ""; }
            if (archivoCarga == "")
                lblError.Text = "Debe proporcionar un archivo a cargar";
            else
            {
                string usuario = "", idEmpresa = "";
                try
                {
                    usuario = Session["u"].ToString();
                    idEmpresa = Session["e"].ToString();
                }
                catch (Exception) { Response.Redirect("Default.aspx"); }

                string periodo = ddlAno.SelectedValue.PadLeft(4,'0') + ddlPeriodo.SelectedValue.PadLeft(2, '0');
                if (datos.existeCargaPervia(periodo, idEmpresa, archivoCarga, usuario))
                {
                    string periodoAplicado = datos.obtienePeridoAplicado(idEmpresa, archivoCarga);
                    if (periodo != "")
                    {
                        if (periodo == periodoAplicado)
                        {
                            Panel4.Visible = true;
                            txtProceso.Text = "1";
                            Session["iteracion"] = 1;
                            Session["ctaReal"] = Session["ctaRec"] = Session["cReal"] = Session["cRec"] = Session["mReal"] = Session["mRec"] = null;
                            lblError.Text = "Archivo Procesado Exitosamente";
                        }
                        else
                            lblError.Text = "El archivo ya fue cargado y procesado con anterioridad en el periodo: " + periodoAplicado;
                    }
                    else
                        lblError.Text = "El archivo ya fue cargado y procesado con anterioridad";
                }
                else {
                    Panel4.Visible = true;
                    txtProceso.Text = "1";
                    Session["iteracion"] = 1;
                    Session["ctaReal"] = Session["ctaRec"] = Session["cReal"] = Session["cRec"] = Session["mReal"] = Session["mRec"] = null;
                    lblError.Text = "Archivo Procesado Exitosamente";
                }
            }
        }
    }

    private void procesamiento() {
        
        string archivo = "";
        try { archivo = Convert.ToString(Session["archivo"]); }
        catch (Exception) { archivo = ""; }
        if (archivo == "")
            lblError.Text = "Debe indicar un archivo a cargar";
        else
        {
            if (File.Exists(archivo))
            {
                lectura(archivo);
            }
            else
            {
                lblError.Text = "Debe indicar un archivo a cargar.";
            }
        }
        
    }


    public void borraArchivo()
    {
        if (File.Exists(name))
        {
            try
            {
                File.Delete(name);
            }
            catch (Exception)
            {
                lblError.Text = "No se pudo borrar el archivo";
            }
        }
    }

    protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlPeriodo.SelectedValue != "0")
        {
            btnCargarDoc.Visible = true;
            Session["archivo"] = null;
            Session["carga"] = null;
            AjaxFileUpload1.Dispose();
            lnkNotificar.Visible = true;
        }
        else
            lnkNotificar.Visible = false;
    }
    protected void AjaxFileUpload1_UploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
    {
        string filename = Path.GetFileName(e.FileName);
        string ruta = Server.MapPath("~/TMP");

        // Si el directorio no existe, crearlo
        if (!Directory.Exists(ruta))
            Directory.CreateDirectory(ruta);

        string archivo = String.Format("{0}\\{1}", ruta, filename);

        // Verificar que el archivo no exista
        if (File.Exists(archivo))
            lblError.Text = (String.Format("Ya existe Documento con nombre\"{0}\".", filename));
        else
        {
            AjaxFileUpload1.SaveAs(ruta + "\\" + filename);
        }
        Session["archivo"] = ruta + "\\" + filename;
        Session["carga"] = filename;
        e.DeleteTemporaryData();
    }
    protected void txtProceso_TextChanged(object sender, EventArgs e)
    {
        /*if (txtProceso.Text != "")
            procesamiento();*/
    }
    protected void btnProceso_Click(object sender, ImageClickEventArgs e)
    {
        Panel4.Visible = true;
    }
    protected void Panel4_Load(object sender, EventArgs e)
    {
        if (Panel4.Visible)
        {            
            if (Convert.ToInt32(Session["iteracion"]) == 1)
            {
                procesamiento();
                
            }
            cargaDatos();
            
                try
                {
                    lblMontoReal.Text = Convert.ToDouble(Session["mReal"]).ToString("C2");
                    lblMontoRec.Text = Convert.ToDouble(Session["mRec"]).ToString("C2");
                    lblCtaReal.Text = Session["ctaReal"].ToString();
                    lblCtaRec.Text = Session["ctaRec"].ToString();
                    lblAceptados.Text = Session["cReal"].ToString();
                    lblRechazados.Text = Session["cRec"].ToString();
                }
                catch (Exception)
                {
                    lblMontoReal.Text = Convert.ToDouble("0").ToString("C2");
                    lblMontoRec.Text = Convert.ToDouble("0").ToString("C2");
                    lblCtaReal.Text = "0";
                    lblCtaRec.Text = "0";
                    lblAceptados.Text = "0";
                    lblRechazados.Text = "0";
                }
                try
                {
                    MontoclientReal = Convert.ToDouble(Session["mReal"]);
                    MontoclientRec = Convert.ToDouble(Session["mRec"]);
                    clientReal = Convert.ToInt32(Session["cReal"]);
                    clientRec = Convert.ToInt32(Session["cRec"]);
                    ctasReal = Convert.ToInt32(Session["ctaReal"]);
                    ctasRec = Convert.ToInt32(Session["ctaRec"]);
                    lblTotalClientes.Text = (clientReal + clientRec).ToString();
                    lblTotal.Text = (MontoclientReal + MontoclientRec).ToString("C2");
                    lblTotalCuentas.Text = (ctasReal + ctasRec).ToString();
                }
                catch (Exception)
                {
                    lblTotalClientes.Text = "0";
                    lblTotal.Text = Convert.ToDouble("0").ToString("C2");
                    lblTotalCuentas.Text = "0";
                }
            
            Panel4.Visible = false;
            Timer1.Enabled = false;
            borraArchivo();
            Session["iteracion"] = 2;
        }
    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        if (Panel4.Visible&&txtProceso.Text=="1")
        {
            txtProceso.Text = "";
            Panel4.Visible = false;
        }
    }

    protected void lnkNotificar_Click(object sender, EventArgs e)
    {
        lblError.Text = "";
        string idEmpresa = "";
        try
        {
            idEmpresa = Session["e"].ToString();
        }
        catch (Exception) { Response.Redirect("Default.aspx"); }
        if (ddlPeriodo.SelectedValue != "0")
        {
            DataSet info = datos.obtieneClientesRechazados(idEmpresa, ddlAno.SelectedValue, ddlPeriodo.SelectedValue);
            Envio_Mail enviaMeail = new Envio_Mail();
            int envios = info.Tables[0].Rows.Count;
            int enviados = 0;
            foreach (DataRow r in info.Tables[0].Rows)
            {
                bool enviado = enviaMeail.obtieneDatosServidor(r[1].ToString().ToLower().Trim(), "", "", "Pago Rechazado Sports Club Mexico " + ddlPeriodo.SelectedItem.Text + " " + ddlAno.SelectedValue + ". Socio: " + r[2].ToString(), null, Convert.ToInt32(idEmpresa), "", "sportsclubmexico@outlook.com");
                if (enviado)
                    enviados++;
            }
            lblError.Text = "Se han notificado a " + enviados + " socios de " + envios + " socios rechazados en el periodo " + ddlPeriodo.SelectedItem.Text + " de " + ddlAno.SelectedValue;
        }
        else
            lblError.Text = "Debe indicar un año y un periodo para realizar lso envios de rechazo y que previamente se cargara un documento de pagos";
    }
}