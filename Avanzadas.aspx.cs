using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using System.Design;

public partial class Avanzadas : System.Web.UI.Page
{
    Datos datos = new Datos();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Panel3.Visible = false;
            PanelImgZoom.Visible = false;
            PanelMascara.Visible = false;
            Panel6.Visible = false;
        }
    }
    protected void btnActualizar_Click(object sender, ImageClickEventArgs e)
    {
        double monto = 0.00;
        try
        {
            lblError.Text = "";
            monto = Convert.ToDouble(txtMonto.Text);
        }
        catch (Exception)
        {
            lblError.Text = "El monto nuevo tiene un formato incorrecto";
        }
        if (monto != 0.00)
        {
            bool actualizado = datos.actualizaMonto(monto);
            if (actualizado)
            {
                lblError.Text = "El monto fue actualizado exitósamente";
                txtMonto.Text = "";
            }
        }
        else
            lblError.Text = "El monto debe ser mayor a 0.00";
    }
    protected void rdlOpcion_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (rdlOpcion.SelectedValue == "V")
        {
            Session["C"] = 1;
            Session["e"] = Convert.ToInt32(Session["e"]);
            Session["u"] = Session["u"].ToString();
            Session["nu"] = Session["nu"].ToString();
            Session["C"] = Convert.ToInt32(Session["C"]);
            Session["aspx"] = Session["aspx"].ToString();
            Response.Redirect("BajasMasivas.aspx");
        }

        if (rdlOpcion.SelectedValue == "A")
        {
            Panel2.Visible = false;
            Panel1.Visible = true;
            Panel3.Visible = false;
        }
        else if (rdlOpcion.SelectedValue == "C")
        {
            lblErrorRefe.Text = "";
            Panel1.Visible = false;
            Panel2.Visible = true;
            Panel3.Visible = false;
        }
        else if (rdlOpcion.SelectedValue == "D")
        {
            Panel1.Visible = false;
            Panel2.Visible = false;
            Panel3.Visible = true;
            Panel6.Visible = false;
        }
    }
    protected void btnCambiar_Click(object sender, ImageClickEventArgs e)
    {
        lblErrorRefe.Text = "";
        bool existeReferencia = datos.existeCliente(txtAltaReferencia.Text);
        if (existeReferencia)
        {
            bool existeNuevaRef = datos.existeCliente(txtRefNuevo.Text);
            if (!existeNuevaRef)
            {
                int cliente = datos.obtieneIdCliente(txtAltaReferencia.Text);
                bool actualizado = datos.actualizaReferencia(cliente, txtRefNuevo.Text);
                if (actualizado)
                {
                    lblErrorRefe.Text = "Se actualizo referencia anterior: " + txtAltaReferencia.Text + " a la nueva referencia: " + txtRefNuevo.Text;
                }
            }
            else
                lblErrorRefe.Text = "La referencia nueva ya existe, favor de verificar";
        }
        else
        {
            lblErrorRefe.Text = "La referencia indicada no existe";
        }
    }
    protected void ddlEstatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        cargaDatos();
    }
    protected void btnSelecciona_Click(object sender, ImageClickEventArgs e)
    {
        ImageButton btn = (ImageButton)sender;
        string[] argumentos = btn.CommandArgument.ToString().Split(new char[] { ';' });
        string referencia = argumentos[0];
        lblReferencia.Text = referencia;
        lblIdCliente.Text = argumentos[1];
        int empresa = 0;
        try
        {
            Datos fotos = new Datos();
            empresa = Convert.ToInt32(Request.QueryString["e"].ToString());
            DataSet imagenes = fotos.obtieneDoctos(referencia, empresa, Convert.ToInt32(lblIdCliente.Text));
            DataListFotosDanos.DataSource = imagenes;
            DataListFotosDanos.DataBind();
            Panel6.Visible = true;
        }
        catch (Exception)
        {
            DataListFotosDanos.DataSource = null;
            DataListFotosDanos.DataBind();
            btn.Visible = false;
            Panel6.Visible = false;
        }

    }

    protected void btnAgregarImagen_Click(object sender, ImageClickEventArgs e)
    {
        lblError.Text = "";

        if (rdlOpcion.SelectedValue == "D")
        {
            if (lblReferencia.Text != "")
            {
                byte[] imagen = null;
                bool archivoValido = false;
                string extension = "";

                if (AsyncUpload1.UploadedFiles.Count == 0)
                    lblError.Text = "Debe indicar el o los documentos a cargar";
                else
                {

                    int idEmpresa = 0;
                    int cliente = 0;
                    Datos fotos = new Datos();
                    try { idEmpresa = Convert.ToInt32(Request.QueryString["e"].ToString()); }
                    catch (Exception) { Response.Redirect("Default.aspx"); }
                    try { cliente = Convert.ToInt32(lblIdCliente.Text); }
                    catch (Exception) { cliente = 0; }
                    //Documentos

                    try
                    {
                        string ruta = Server.MapPath("~/TMP");
                        string filename = "";
                        // Si el directorio no existe, crearlo
                        if (!Directory.Exists(ruta))
                            Directory.CreateDirectory(ruta);
                        int documentos = AsyncUpload1.UploadedFiles.Count;
                        int guardados = 0;
                        for (int i = 0; i < documentos; i++)
                        {
                            filename = AsyncUpload1.UploadedFiles[i].FileName;
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
                                    System.Drawing.Image img = System.Drawing.Image.FromStream(AsyncUpload1.UploadedFiles[i].InputStream);
                                    img.Save(archivo);
                                    imagen = Imagen_A_Bytes(archivo);
                                }
                                else
                                {
                                    Telerik.Web.UI.UploadedFile up = AsyncUpload1.UploadedFiles[i];
                                    up.SaveAs(archivo);
                                    imagen = File.ReadAllBytes(archivo);
                                }
                            }
                            catch (Exception) { imagen = null; }
                            if (imagen != null)
                            {
                                string identificadorProceso = ddlEstatus.SelectedValue;
                                bool agregado = datos.agregaDocumentosCliente(cliente, imagen, extension, lblReferencia.Text, idEmpresa, identificadorProceso);
                                if (agregado)
                                    guardados++;
                            }
                        }
                        lblError.Text = "Se guardaron " + guardados.ToString() + " imagenes de " + documentos.ToString() + " seleccionadas.";
                    }
                    catch (Exception) { imagen = null; }
                    int empresa = 0;
                    try
                    {
                        empresa = Convert.ToInt32(Request.QueryString["e"].ToString());
                        DataSet imagenes = fotos.obtieneDoctos(lblReferencia.Text, empresa, cliente);
                        DataListFotosDanos.DataSource = imagenes;
                        DataListFotosDanos.DataBind();
                    }
                    catch (Exception)
                    {
                        DataListFotosDanos.DataSource = null;
                        DataListFotosDanos.DataBind();
                    }
                }
            }
            else
                lblError.Text = "Debe seleccionar un cliente";
        }
        else
            lblError.Text = "La acción de agregar solo es accesible en la opción documentos";
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

    protected void btnCerrarImgZomm_Click(object sender, ImageClickEventArgs e)
    {
        PanelImgZoom.Visible = false;
        PanelMascara.Visible = false;
    }

    protected void DataListFotosDanos_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "zoom")
        {
            PanelMascara.Visible = true;
            PanelImgZoom.Visible = true;
            string[] valores = e.CommandArgument.ToString().Split(';');
            try
            {
                int id_empresa = Convert.ToInt32(Request.QueryString["e"].ToString());
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
                        imgZoom.Visible = true;
                        pnlPdf.Visible = false;
                        imgZoom.ImageUrl = "IMG/pdflogo.jpg";
                    }
                }
            }
            catch (Exception) { }
        }
        else if (e.CommandName == "elimina")
        {
            lblError.Text = "";
            try
            {
                string[] valores = e.CommandArgument.ToString().Split(';');
                int id_empresa = Convert.ToInt32(Request.QueryString["e"].ToString());
                int id = Convert.ToInt32(valores[0]);
                string referencia = valores[1];
                int idCliente = Convert.ToInt32(valores[2]);
                bool eliminado = datos.eliminarFotoDanos(id, referencia, id_empresa, idCliente);
                if (eliminado)
                {
                    try
                    {
                        Datos fotos = new Datos();
                        DataSet imagenes = fotos.obtieneDoctos(referencia, id_empresa, idCliente);
                        DataListFotosDanos.DataSource = imagenes;
                        DataListFotosDanos.DataBind();
                    }
                    catch (Exception)
                    {
                        DataListFotosDanos.DataSource = null;
                        DataListFotosDanos.DataBind();
                    }
                }
                else
                    lblError.Text = "El documento no se logro eliminar, verifique su conexión e intentelo nuevamente mas tarde";
            }
            catch (Exception x)
            {
                lblError.Text = "La carga de los documentos no se logro por un error inesperado: " + x.Message;
            }
        }
    }

    protected void btnBuscar_Click(object sender, ImageClickEventArgs e)
    {
        cargaDatos();
    }

    protected void lnkLimpiar_Click(object sender, EventArgs e)
    {
        ddlBuscar.SelectedValue = "0";
        txtBuscar.Text = "";
        cargaDatos();
    }

    protected void txtBuscar_TextChanged(object sender, EventArgs e)
    {
        cargaDatos();
    }

    private void cargaDatos()
    {
        string condicion = "";
        int op = Convert.ToInt32(ddlBuscar.SelectedValue);
        switch (op)
        {
            case 1:
                condicion = " and referencia like '%" + txtBuscar.Text.Trim() + "%' ";
                break;
            case 2:
                condicion = " and (ltrim(rtrim(nombre)) like '%" + txtBuscar.Text.Trim() + "%' or ltrim(rtrim(apellido_paterno)) like '%" + txtBuscar.Text.Trim() + "%' or ltrim(rtrim(isnull(apellido_materno,''))) like '%" + txtBuscar.Text.Trim() + "%' ) ";
                break;
            case 3:
                condicion = " and (ltrim(rtrim(nombre_tarjetahabiente)) like '%" + txtBuscar.Text.Trim() + "%' or ltrim(rtrim(ap_pat_tarjetahabiente)) like '%" + txtBuscar.Text.Trim() + "%' or ltrim(rtrim(isnull(ap_mat_tarjetahabiente,''))) like '%" + txtBuscar.Text.Trim() + "%' )";
                break;
            default:
                condicion = " and (ltrim(rtrim(nombre_tarjetahabiente)) like '%" + txtBuscar.Text.Trim() + "%' or ltrim(rtrim(ap_pat_tarjetahabiente)) like '%" + txtBuscar.Text.Trim() + "%' or ltrim(rtrim(isnull(ap_mat_tarjetahabiente,''))) like '%" + txtBuscar.Text.Trim() + "%' or ltrim(rtrim(nombre)) like '%" + txtBuscar.Text.Trim() + "%' or ltrim(rtrim(apellido_paterno)) like '%" + txtBuscar.Text.Trim() + "%' or ltrim(rtrim(isnull(apellido_materno,''))) like '%" + txtBuscar.Text.Trim() + "%' or referencia like '%" + txtBuscar.Text.Trim() + "%' ) ";
                break;
        }

        if (ddlEstatus.SelectedValue == "A")
        {
            SqlDataSource1.SelectCommand = "select id_cliente,referencia,"
+ "ltrim(rtrim(nombre_tarjetahabiente))+' '+ltrim(rtrim(ap_pat_tarjetahabiente))+' '+ltrim(rtrim(isnull(ap_mat_tarjetahabiente,''))) as tarjetahabiente,"
+ "ltrim(rtrim(nombre))+' '+ltrim(rtrim(apellido_paterno))+' '+ltrim(rtrim(isnull(apellido_materno,''))) as cliente,"
+ "fecha_inicio as fecha, motivo_baja, usuario_alta as usuario from clientes where id_empresa=@empresa and estatus_cliente='A' " + condicion;
            lblusuario.Text = "Usuario Registro";
        }
        else if (ddlEstatus.SelectedValue == "I")
        {
            SqlDataSource1.SelectCommand = "select id_cliente,referencia,"
+ "ltrim(rtrim(nombre_tarjetahabiente))+' '+ltrim(rtrim(ap_pat_tarjetahabiente))+' '+ltrim(rtrim(isnull(ap_mat_tarjetahabiente,''))) as tarjetahabiente,"
+ "ltrim(rtrim(nombre))+' '+ltrim(rtrim(apellido_paterno))+' '+ltrim(rtrim(isnull(apellido_materno,''))) as cliente,"
+ "fecha_baja as fecha, motivo_baja, usuario_inactiva as usuario from clientes where id_empresa=@empresa and estatus_cliente='I'" + condicion;
            lblusuario.Text = "Usuario Inactivo";
        }
        else if (ddlEstatus.SelectedValue == "B")
        {
            switch (op)
            {
                case 1:
                    condicion = " and referencia like '%" + txtBuscar.Text.Trim() + "%' ";
                    break;
                case 2:
                    condicion = " and cliente like '%" + txtBuscar.Text.Trim() + "%' ";
                    break;
                case 3:
                    condicion = " and tarjeta_habiente like '%" + txtBuscar.Text.Trim() + "%' ";
                    break;
                default:
                    condicion = " and (referencia like '%" + txtBuscar.Text.Trim() + "%' or cliente like '%" + txtBuscar.Text.Trim() + "%' or tarjeta_habiente like '%" + txtBuscar.Text.Trim() + "%') ";
                    break;
            }

            SqlDataSource1.SelectCommand = "select isnull(id_cliente,0) as id_cliente,referencia,tarjeta_habiente as tarjetahabiente,cliente,"
+ "fecha_baja as fecha, motivo as motivo_baja, usuario_baja as usuario  from bajas_inmediatas where id_empresa=@empresa " + condicion;
            lblusuario.Text = "Usuario Baja";
        }
        DataListFotosDanos.DataSource = null;
        DataListFotosDanos.DataBind();
        grdClientes.SelectedIndex = -1;
        grdClientes.DataBind();
        Panel6.Visible = false;
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
        return valido;
    }
    protected void lnkBajasM_Click(object sender, EventArgs e)
    {
      

        // Response.Redirect("BajasMasivas.aspx?e=" + Convert.ToInt32(Session["e"]) + "&u=" + Session["u"].ToString()+ "&nu=" + Request.QueryString["nu"]);
    }
    protected void DataListFotosDanos_ItemDataBound(object sender, DataListItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            LinkButton btnLogo = e.Item.FindControl("btnLogo") as LinkButton;
            if (btnLogo.CommandArgument != "")
            {
                string[] argumentos = btnLogo.CommandArgument.Split(';');
                string idProceso = argumentos[4];
                if (idProceso == "A")
                    e.Item.BackColor = System.Drawing.Color.Green;
                else if (idProceso == "I")
                    e.Item.BackColor = System.Drawing.Color.Orange;
                else if (idProceso == "B")
                    e.Item.BackColor = System.Drawing.Color.Red;
                else
                {
                    e.Item.BackColor = System.Drawing.Color.Transparent;
                    e.Item.BorderStyle = BorderStyle.Solid;
                    e.Item.BorderWidth = 1;
                    e.Item.BorderColor = System.Drawing.Color.Black;
                }
            }
        }
    }
}