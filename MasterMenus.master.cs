using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class MasterMenus : System.Web.UI.MasterPage
{
    Datos datos = new Datos();

    protected void Page_Load(object sender, EventArgs e)
    {
        int empre = Convert.ToInt32(Session["e"]);
        string usua = Session["u"].ToString();

        if ( Session["e"] == null || Session["e"].ToString() == "0")
        {
            label.Visible = false;
            lblEmpresa.Visible = false;
            lblEmpresa.Text = "";

        }
        else
        {            
            label.Visible = true;
            lblEmpresa.Visible = true;
            try
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

                bool[] arregloPermisos = null;

                if (datos.obtienePermisosTotales(Session["u"].ToString()) != 0)
                    arregloPermisos = datos.obtienePermisos(Session["u"].ToString());
                else
                {
                    arregloPermisos = new bool[20];
                    for (int i = 0; i < 20; i++)
                    {
                        arregloPermisos[i] = false;
                    }                    
                }

                lblEmpresa.Text = datos.obtieneNombreEmpresa(empresa);
                imgEmp.ImageUrl = "~/ImgEmpresas.ashx?id=" + empresa + "";
                string usuario = Session["u"].ToString();

                lblUsuario.Text = usuario;
                if (arregloPermisos != null)
                {                    
                    bool[] permisos = arregloPermisos;
                    
                    activaMenu(permisos);                    
                }
            }
            catch (Exception)
            {
                Response.Redirect("Default.aspx");
            }
        }
    }

    private void cargaUrls() {

        MenuItem menuCatalogos = Menu1.Items[5];
        MenuItem menuClientes = Menu1.Items[0];
        MenuItem menuConsultas = Menu1.Items[1];
        MenuItem menuContraseña = Menu1.Items[2];
        MenuItem menuArchvios = Menu1.Items[3];
        MenuItem menuAvanzadas = Menu1.Items[4];
        MenuItem menuEstadisticos = Menu1.Items[6];
        MenuItem submenEmpresas = Menu1.Items[5].ChildItems[0];
        MenuItem submenUsuarios = Menu1.Items[5].ChildItems[1];
        MenuItem submenUsuariosEmp = Menu1.Items[5].ChildItems[2];
        MenuItem submenUsuariosPerm = Menu1.Items[5].ChildItems[3];
        MenuItem submenBancos = Menu1.Items[5].ChildItems[4];
        MenuItem submenAfiliacion = Menu1.Items[5].ChildItems[5];
        MenuItem submenUsuariosAfil = Menu1.Items[5].ChildItems[6];
        MenuItem submenArchivoCob = Menu1.Items[3].ChildItems[0];
        MenuItem submenPagos = Menu1.Items[3].ChildItems[1];
        MenuItem submenImpClientes = Menu1.Items[3].ChildItems[2];

        submenEmpresas.NavigateUrl = "Empresas.aspx";
        submenUsuarios.NavigateUrl = "Usuarios.aspx";
        submenUsuariosEmp.NavigateUrl = "Usuarios_empresas.aspx";
        submenUsuariosPerm.NavigateUrl = "Usuarios_permisos.aspx";
        submenBancos.NavigateUrl = "Bancos.aspx";
        submenAfiliacion.NavigateUrl = "Afiliacion.aspx";
        submenUsuariosAfil.NavigateUrl = "Usuarios_afiliacion.aspx";
        submenArchivoCob.NavigateUrl = "GeneraArchivo.aspx";
        submenPagos.NavigateUrl = "Lectura.aspx";
        submenImpClientes.NavigateUrl = "Archivo_Cliente.aspx";
        menuClientes.NavigateUrl = "Clientes1.aspx";        
        menuAvanzadas.NavigateUrl = "Avanzadas.aspx";
        menuEstadisticos.NavigateUrl = "Estadisticos.aspx";
        menuConsultas.NavigateUrl = "Consultas.aspx";
        menuContraseña.NavigateUrl = "CambiarContraseña.aspx";
    }

    private void activaMenu(bool[] permisos)
    {
        try
        {
            MenuItem[] quitar = new MenuItem[7];
            bool[] quitarGral = new bool[7] { false, false, false, false, false, false, false };
            MenuItemCollection menus = Menu1.Items;
            MenuItemCollection submenusCat =null;
            MenuItem[] quitarCAT = new MenuItem[7];
            bool[] quitarCat = new bool[7] { false, false, false, false, false, false, false };
            MenuItemCollection submenusArc = null;
            MenuItem[] quitarARC = new MenuItem[3];
            bool[] quitarArc = new bool[3] { false, false, false };
            int i = 0;
            foreach (MenuItem menu in menus) {
                switch (menu.ToolTip) { 
                    case "Clientes":
                        menu.NavigateUrl = "Clientes1.aspx";
                        quitar[0] = menu;
                        if (!permisos[1])
                            //menus.Remove(menu);
                            quitarGral[0] = true;
                        break;
                    case "Consultas":
                        quitar[1] = menu;
                        menu.NavigateUrl = "Consultas.aspx";
                        break;
                    case "Cambiar Contraseña":
                        quitar[2] = menu;
                        menu.NavigateUrl = "CambiarContraseña.aspx";
                        break;
                    case "Archivos":
                        quitar[3] = menu;
                        submenusArc = Menu1.Items[i].ChildItems;
                        if (!permisos[2])
                            quitarGral[3] = true;
                        break;
                    case "Avanzadas":
                        quitar[4] = menu;
                        menu.NavigateUrl = "Avanzadas.aspx"; 
                        if (!permisos[3])
                            quitarGral[4] = true;
                        break;
                    case "Catálogos":
                        quitar[5] = menu;
                        submenusCat = Menu1.Items[i].ChildItems;
                        if (!permisos[0])
                            quitarGral[5] = true;                       
                        break;
                    case "Estadísticos":
                        quitar[6] = menu;
                        menu.NavigateUrl = "Estadisticos.aspx"; 
                        if (!permisos[16])
                            quitarGral[6] = true;
                        break;
                    default:
                        break;
                }
                i++;
            }

            

            if (submenusCat != null)
            {
                foreach (MenuItem menu in submenusCat)
                {
                    switch (menu.ToolTip)
                    {
                        case "Empresas":
                            quitarCAT[0] = menu;
                            menu.NavigateUrl = "Empresas.aspx";
                            if (!permisos[4])
                                quitarCat[0] = true;
                            break;
                        case "Usuarios":
                            quitarCAT[1] = menu;
                            menu.NavigateUrl = "Usuarios.aspx";
                            if (!permisos[5])
                                quitarCat[1] = true;
                            break;
                        case "Usuarios Empresas":
                            quitarCAT[2] = menu;
                            menu.NavigateUrl = "Usuarios_empresas.aspx";
                            if (!permisos[6])
                                quitarCat[2] = true;
                            break;
                        case "Usuarios Permisos":
                            quitarCAT[3] = menu;
                            menu.NavigateUrl = "Usuarios_permisos.aspx";
                            if (!permisos[7])
                                quitarCat[3] = true;
                            break;
                        case "Bancos":
                            quitarCAT[4] = menu;
                            menu.NavigateUrl = "Bancos.aspx";
                            if (!permisos[8])
                                quitarCat[4] = true;
                            break;
                        case "Afiliaciones":
                            quitarCAT[5] = menu;
                            menu.NavigateUrl = "Afiliacion.aspx";
                            if (!permisos[9])
                                quitarCat[5] = true;
                            break;
                        case "Usuarios Afiliación":
                            quitarCAT[6] = menu;
                            menu.NavigateUrl = "Usuarios_afiliacion.aspx";
                            if (!permisos[10])
                                quitarCat[6] = true;
                            break;
                        default:
                            break;
                    }
                }

                for (int j = 0; j < 7; j++) {
                    if (quitarCat[j])
                        submenusCat.Remove(quitarCAT[j]);
                }
            }

            if (submenusArc != null)
            {
                foreach (MenuItem menu in submenusArc)
                {
                    switch (menu.ToolTip)
                    {
                        case "Generación de Archivo de Cobranza":
                            quitarARC[0] = menu;
                            menu.NavigateUrl = "GeneraArchivo.aspx";
                            if (!permisos[11])
                                quitarArc[0] = true;
                            break;
                        case "Importación de Pagos":
                            quitarARC[1] = menu;
                            menu.NavigateUrl = "Lectura.aspx";
                            if (!permisos[12])
                                quitarArc[1] = true;
                            break;
                        case "Importación de Clientes":
                            quitarARC[2] = menu;
                            menu.NavigateUrl = "Archivo_Cliente.aspx";
                            if (!permisos[13])
                                quitarArc[2] = true;
                            break;                        
                        default:
                            break;
                    }
                }

                for (int j = 0; j < 3; j++)
                {
                    if (quitarArc[j])
                        submenusArc.Remove(quitarARC[j]);
                }
            }


            for (int j = 0; j < 7; j++)
            {
                if (quitarGral[j])
                    Menu1.Items.Remove(quitar[j]);
            }

            /*try
            {
                MenuItem menuCatalogos = Menu1.Items[5]; if (!permisos[0])
                    Menu1.Items.Remove(menuCatalogos);
            }
            catch (Exception) {}
            try
            {
                MenuItem menuClientes = Menu1.Items[0]; 
                menuClientes.NavigateUrl = "Clientes1.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; 
                if (!permisos[1])
                    Menu1.Items.Remove(menuClientes);
            }
            catch (Exception) { }
            try { MenuItem menuConsultas = Menu1.Items[1]; menuConsultas.NavigateUrl = "Consultas.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; }
            catch (Exception) { }
            try
            {
                MenuItem menuArchvios = Menu1.Items[3]; if (!permisos[2])
                    Menu1.Items.Remove(menuArchvios);
            }
            catch (Exception) { }
            try
            {
                MenuItem menuAvanzadas = Menu1.Items[4]; menuAvanzadas.NavigateUrl = "Avanzadas.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; if (!permisos[3])
                    Menu1.Items.Remove(menuAvanzadas);
            }
            catch (Exception) { }
            try
            {
                MenuItem menuEstadisticos = Menu1.Items[6]; menuEstadisticos.NavigateUrl = "Estadisticos.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; if (!permisos[16])
                    Menu1.Items.Remove(menuEstadisticos);
            }
            catch (Exception) { }
            try
            {
                MenuItem submenEmpresas = Menu1.Items[5].ChildItems[0]; submenEmpresas.NavigateUrl = "Empresas.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; if (!permisos[4])
                    Menu1.Items[5].ChildItems.Remove(submenEmpresas);
            }
            catch (Exception) { }
            try
            {
                MenuItem submenUsuarios = Menu1.Items[5].ChildItems[1]; submenUsuarios.NavigateUrl = "Usuarios.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; if (!permisos[5])
                    Menu1.Items[5].ChildItems.Remove(submenUsuarios);
            }
            catch (Exception) { }
            try
            {
                MenuItem submenUsuariosEmp = Menu1.Items[5].ChildItems[2]; submenUsuariosEmp.NavigateUrl = "Usuarios_empresas.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; if (!permisos[6])
                    Menu1.Items[5].ChildItems.Remove(submenUsuariosEmp);
            }
            catch (Exception) { }
            try
            {
                MenuItem submenUsuariosPerm = Menu1.Items[5].ChildItems[3]; submenUsuariosPerm.NavigateUrl = "Usuarios_permisos.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; if (!permisos[7])
                    Menu1.Items[5].ChildItems.Remove(submenUsuariosPerm);
            }
            catch (Exception) { }
            try
            {
                MenuItem submenBancos = Menu1.Items[5].ChildItems[4]; submenBancos.NavigateUrl = "Bancos.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; if (!permisos[8])
                    Menu1.Items[5].ChildItems.Remove(submenBancos);
            }
            catch (Exception) { }
            try
            {
                MenuItem submenAfiliacion = Menu1.Items[5].ChildItems[5]; submenAfiliacion.NavigateUrl = "Afiliacion.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; if (!permisos[9])
                    Menu1.Items[5].ChildItems.Remove(submenAfiliacion);
            }
            catch (Exception) { }
            try
            {
                MenuItem submenUsuariosAfil = Menu1.Items[5].ChildItems[6]; submenUsuariosAfil.NavigateUrl = "Usuarios_afiliacion.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; if (!permisos[10])
                    Menu1.Items[5].ChildItems.Remove(submenUsuariosAfil);
            }
            catch (Exception) { }
            try
            {
                MenuItem submenArchivoCob = Menu1.Items[3].ChildItems[0]; submenArchivoCob.NavigateUrl = "GeneraArchivo.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; if (!permisos[11])
                    Menu1.Items[3].ChildItems.Remove(submenArchivoCob);
            }
            catch (Exception) { }
            try
            {
                MenuItem submenPagos = Menu1.Items[3].ChildItems[1]; submenPagos.NavigateUrl = "Lectura.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; if (!permisos[12])
                    Menu1.Items[3].ChildItems.Remove(submenPagos);
            }
            catch (Exception) { }
            try
            {
                MenuItem submenImpClientes = Menu1.Items[3].ChildItems[2]; submenImpClientes.NavigateUrl = "Archivo_Cliente.aspx?e=" + Request.QueryString["e"] + "&u=" + Request.QueryString["u"] + "&nu=" + Request.QueryString["nu"]; if (!permisos[13])
                    Menu1.Items[3].ChildItems.Remove(submenImpClientes);
            }
            catch (Exception) { }  */
        }
        catch (Exception ex) { }
    }

    protected void btnSalir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}
