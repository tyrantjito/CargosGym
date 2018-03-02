using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Text;
using System.Security;
using System.Collections;
using System.Configuration;

public partial class Archivo_Cliente : System.Web.UI.Page
{
    Datos datos = new Datos();
    DataTable dt;
    DataTable dt1;
    int id_empresa;
    string name;

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
            File.Delete(archivo);
        
        file.SaveAs(archivo);
    }

    public void CargaArchivo()
    {
        btnGuardar.Visible = false;
        lblError.Text="";
        lblInsert.Text = "";
        string ruta = Server.MapPath("~/TMP");
        try
        {
            if (FileUpload1.HasFile)
            {
                // Se verifica que la extensión sea de un formato válido
                string ext = FileUpload1.PostedFile.FileName;
                ext = ext.Substring(ext.LastIndexOf(".") + 1).ToLower();
                string[] formatos = new string[] { "txt", "csv" };
                if (Array.IndexOf(formatos, ext) < 0)
                    lblError.Text = ("Formato del Documento inválido.");
                else
                {
                    GuardarArchivo(FileUpload1.PostedFile);
                    name = ruta + "\\" + (FileUpload1.FileName);
                    Session["name"] = name;
                    try { id_empresa = Convert.ToInt32(Session["e"]); }
                    catch (Exception)
                    {
                        Response.Redirect("Default.aspx");
                    }
                    convertirArchivo();
                    dt = new DataTable();

                    string[] datatext = new string[File.ReadAllLines(name).Length];
                    int contador = 0;
                    foreach (string linea in File.ReadLines(name, Encoding.UTF8))
                    {
                        datatext[contador] = linea;
                        contador++;
                    }

                    string[] text = null;
                    dt.Columns.Add("referencia");
                    dt.Columns.Add("nombre");
                    dt.Columns.Add("apellidoPat");
                    dt.Columns.Add("apellidoMat");
                    dt.Columns.Add("monto");
                    dt.Columns.Add("cuenta");
                    dt.Columns.Add("fechaIni");
                    dt.Columns.Add("fechaFin");
                    dt.Columns.Add("periodo");

                    foreach (string csvrow in datatext)
                    {
                        text = csvrow.Split(',');
                        DataRow dr = dt.NewRow();
                        dr.ItemArray = text;
                        dt.Rows.Add(dr);
                    }
                    GridView1.DataSource = dt;
                    GridView1.DataBind();
                    if(dt.Rows.Count!=0)
                        btnGuardar.Visible = true;
                }
            }
            else
                lblError.Text = ("Seleccione un archivo.");
        }
        catch (Exception)
        {
            lblError.Text = "Se perdio la conexión, recargue la página e intentelo nuevamente";
        }        
    }

    public void convertirArchivo()
    {
        byte[] ansiBytes = File.ReadAllBytes(name);
        var utf8String = Encoding.UTF8.GetString(ansiBytes);
        File.WriteAllText(name, utf8String);
    }

    protected void btnGuardar_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "Procesando información, Por Favor Espere ...";
        
        try
        {
            name = Session["name"].ToString();
            id_empresa = Convert.ToInt32(Session["e"]);
        }
        catch (Exception) { Response.Redirect("Default.aspx"); }

        string[] datatext = new string[File.ReadAllLines(name).Length];
        int contador = 0;
        foreach (string linea in File.ReadLines(name, Encoding.UTF8))
        {
            datatext[contador] = linea;
            contador++;
        }
        dt = new DataTable();

        string[] text = null;

        dt.Columns.Add("referencia");
        dt.Columns.Add("nombre");
        dt.Columns.Add("apellidoPat");
        dt.Columns.Add("apellidoMat");
        dt.Columns.Add("monto");
        dt.Columns.Add("cuenta");
        dt.Columns.Add("fechaIni");
        dt.Columns.Add("fechaFin");
        dt.Columns.Add("periodo");

        foreach (string csvrow in datatext)
        {
            text = csvrow.Split(',');
            DataRow dr = dt.NewRow();
            dr.ItemArray = text;
            dt.Rows.Add(dr);
        }

        int regInsertados = 0;
        int regRechazados = 0;
        DataTable rechazados = new DataTable();
        rechazados.Columns.Add("referenciaRech");
        rechazados.Columns.Add("nombreRech");
        foreach (DataRow fila in dt.Rows)
        {
            string referencia, nombre, apPat, apMat, cuenta, fechaIni, fechaFin, periodo;
            double monto;
            referencia = fila[0].ToString();
            nombre = fila[1].ToString();
            apPat = fila[2].ToString();
            apMat = fila[3].ToString();
            monto = Convert.ToDouble(fila[4].ToString());
            cuenta = fila[5].ToString();
            fechaIni = fila[6].ToString();
            fechaFin = fila[7].ToString();
            periodo = fila[8].ToString();

            DateTime fechaIncial, fechaFinal;
            try
            {
                fechaIncial = Convert.ToDateTime(fechaIni);
            }
            catch (Exception) { fechaIncial = Convert.ToDateTime("1900-01-01"); }
            try
            {
                fechaFinal = Convert.ToDateTime(fechaFin);
            }
            catch (Exception) { fechaFinal = Convert.ToDateTime("1900-01-01"); }
            if (fechaIncial.ToString("yyyy-MM-dd") == "1900-01-01" || fechaFinal.ToString("yyyy-MM-dd") == "1900-01-01")
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
                lblError.Text = "Uno o más clientes del archivo cuentan con fechas de inicio o de fin de contrato inválidas; por favor verifique que las fechas sean correctas";
            }
            else
            {                
                
                bool existe = datos.existeCliente(referencia);
                if (!existe)
                {
                    bool registrado = datos.agregaClientesMasivos(referencia, nombre, apPat, apMat, monto, cuenta, fechaIncial.ToString("yyyy-dd-MM"), fechaFinal.ToString("yyyy-dd-MM"), periodo, id_empresa);
                    if (!registrado)
                    {
                        DataRow drRechazado = rechazados.NewRow();
                        string[] valores = new string[2];
                        valores[0] = referencia;
                        valores[1] = nombre + " " + apPat + " " + apMat;
                        drRechazado.ItemArray = valores;
                        rechazados.Rows.Add(drRechazado);
                        regRechazados++;
                    }
                    else
                        regInsertados++;
                }
                else {
                    DataRow drRechazado = rechazados.NewRow();
                    string[] valores = new string[2];
                    valores[0] = referencia;
                    valores[1] = nombre + " " + apPat + " " + apMat;
                    drRechazado.ItemArray = valores;
                    rechazados.Rows.Add(drRechazado);
                    regRechazados++;
                }
            }
        }


        if (regRechazados > 0)
        {
            GridView2.DataSource = rechazados;
            GridView2.DataBind();
            Panel3.Visible = true;
        }
        else {
            GridView2.DataSource = null;
            GridView2.DataBind();
            Panel3.Visible = false;
        }
        lblError.Text = "";
        lblInsert.Text = regInsertados + " Cliente(s) insertados";
        lblError.Text = regRechazados + " Cliente(s) no se registraron";
        GridView1.DataBind();
        borraArchivo();
    }  
     
     public void borraArchivo()
    {       
        if(File.Exists(name))
        {
            try
            {
                File.Delete(name);
            }
            catch (Exception)
            {
                lblError.Text="No se pudo borrar el archivo";
            }
        }

    }
    protected void btnAplicar_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "Cargando Información, Por Favor Espere ...";
        GridView1.DataSource = null;
        GridView1.DataBind();
        CargaArchivo();        
    }
}