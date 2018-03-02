<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Consultas.aspx.cs" Inherits="Consultas" MasterPageFile="~/MasterMenus.master" %>



<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>



<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2 class="anchoCompleto">Consultas</h2>
            <asp:Panel ID="Panel1" runat="server" Visible="false">
                <asp:Label ID="Label1" runat="server"></asp:Label>
                <asp:Label ID="Label2" runat="server"></asp:Label>
                <asp:Label ID="Label3" runat="server"></asp:Label>
                <asp:Label ID="Label5" runat="server"></asp:Label>
                <asp:Label ID="Label6" runat="server"></asp:Label>
                <asp:Label ID="Label8" runat="server"></asp:Label>
                <asp:Label ID="Label10" runat="server"></asp:Label>
                <asp:Label ID="Label14" runat="server"></asp:Label>
                <asp:Label ID="Label15" runat="server"></asp:Label>
                <asp:Label ID="Label16" runat="server"></asp:Label>
                <asp:Label ID="Label17" runat="server"></asp:Label>
                <asp:Label ID="Label18" runat="server"></asp:Label>
            </asp:Panel>


            <div class="anchoCompleto center">
                <asp:RadioButtonList ID="rdbOpcion" runat="server" CssClass="centardo" ForeColor="#002c81" AutoPostBack="True" RepeatDirection="Horizontal" RepeatColumns="3" OnSelectedIndexChanged="rdbOpcion_SelectedIndexChanged">
                    <%--<asp:ListItem Value="0" Text="Pago Sucursal"  />
                    <asp:ListItem Value="1" Text="Usuarios" />--%>
                    <asp:ListItem Value="2" Text="Socios" Selected="True" />
                </asp:RadioButtonList>
            </div>

            <asp:Panel ID="pnlPagos" runat="server" Visible="false">
                <div class="anchoCompleto centardo textocentradoh">
                    <table class="anchoCompleto">
                        <tr>
                            <td class="textoIzquierda">
                                <asp:Label ID="Label29" runat="server" Text="Año:" ForeColor="#002c81" CssClass="textobold"></asp:Label>
                                <asp:DropDownList ID="ddlAño" runat="server" OnSelectedIndexChanged="ddlAño_SelectedIndexChanged" AutoPostBack="true">
                                </asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                                    SelectCommand="select tabla.periodo from (select distinct SUBSTRING(periodo,1,4) as periodo from detalle_cliente where id_empresa=@id_empresa and cast(SUBSTRING(periodo,1,4) as int)<>cast(substring(convert(char(10),getdate(),126),1,4) as int) union all select cast(substring(convert(char(10),getdate(),126),1,4) as int) as perido) as tabla order by tabla.periodo desc " ConnectionString="<%$ ConnectionStrings:eBills %>">
                                    <SelectParameters>
                                        <asp:QueryStringParameter DefaultValue="0" Name="id_empresa" QueryStringField="e" Type="Int32" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                            <td class="textocentradoh">
                                <asp:Label ID="Label25" runat="server" Text="Mes:" ForeColor="#002c81" CssClass="textobold"></asp:Label>
                                <asp:DropDownList ID="ddlMes" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlMes_SelectedIndexChanged">
                                    <asp:ListItem Value="01">Enero</asp:ListItem>
                                    <asp:ListItem Value="02">Febrero</asp:ListItem>
                                    <asp:ListItem Value="03">Marzo</asp:ListItem>
                                    <asp:ListItem Value="04">Abril</asp:ListItem>
                                    <asp:ListItem Value="05">Mayo</asp:ListItem>
                                    <asp:ListItem Value="06">Junio</asp:ListItem>
                                    <asp:ListItem Value="07">Julio</asp:ListItem>
                                    <asp:ListItem Value="08">Agosto</asp:ListItem>
                                    <asp:ListItem Value="09">Septiembre</asp:ListItem>
                                    <asp:ListItem Value="10">Octubre</asp:ListItem>
                                    <asp:ListItem Value="11">Noviembre</asp:ListItem>
                                    <asp:ListItem Value="12">Diciembre</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="textocentradoh">
                                <asp:Label ID="Label26" runat="server" Text="Pago:" ForeColor="#002c81" CssClass="textobold"></asp:Label>
                                <asp:DropDownList ID="ddlPago" runat="server" OnSelectedIndexChanged="ddlPago_SelectedIndexChanged" AutoPostBack="true">
                                    <asp:ListItem Value="R" Selected="True">Rechazados</asp:ListItem>
                                    <asp:ListItem Value="P">Realizado</asp:ListItem>
                                    <asp:ListItem Value="M">Sucursal</asp:ListItem>
                                    <asp:ListItem Value="0">Pendiente</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="textocentradoh">
                                <asp:Label ID="Label27" runat="server" Text="Estatus:" ForeColor="#002c81" CssClass="textobold"></asp:Label>
                                <asp:DropDownList ID="ddlEstatus" runat="server" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlEstatus_SelectedIndexChanged">
                                    <asp:ListItem Value="A" Selected="True">Activo</asp:ListItem>
                                    <asp:ListItem Value="I">Inactivo</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="textocentradoh">
                                <asp:RadioButtonList ID="rbtnTipo" runat="server" RepeatDirection="Horizontal" AutoPostBack="true"
                                    OnSelectedIndexChanged="rbtnTipo_SelectedIndexChanged" ForeColor="#002c81" CssClass="textobold" RepeatColumns="2">
                                    <asp:ListItem Selected="True" Text="TarjetaHabiente" Value="T" />
                                    <asp:ListItem Text="Cliente" Value="C" />
                                </asp:RadioButtonList>
                            </td>
                            <td class="textoright ">
                                <asp:Label ID="Label28" runat="server" Text="Filtro:" ForeColor="#002c81" CssClass="textobold"></asp:Label>
                                <asp:DropDownList ID="ddlBuscar" runat="server">
                                    <asp:ListItem Value="0">Todos</asp:ListItem>
                                    <asp:ListItem Value="1">Referencia</asp:ListItem>
                                    <asp:ListItem Value="2">Nombre</asp:ListItem>
                                </asp:DropDownList>
                                <asp:TextBox ID="txtFiltro" runat="server" CssClass="textboxlog ancho100px center" MaxLength="10" />
                                <cc1:TextBoxWatermarkExtender ID="txtFiltro_TextBoxWatermarkExtender"
                                    runat="server" BehaviorID="txtFiltro_TextBoxWatermarkExtender"
                                    TargetControlID="txtFiltro" WatermarkCssClass="water ancho100px textboxlog" WatermarkText="Buscar..." />

                                <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/IMG/buscar.png" AlternateText="Buscar" OnClick="btnFiltrarGrid_Click" CssClass="iconitos40 center" ToolTip="Buscar" />
                            </td>
                            <td>
                                <asp:LinkButton ID="lbtnLimpiar" runat="server" CssClass="linkLimpia colorAzul"
                                    Text="Limpiar Búsqueda" OnClick="lbtnLimpiar_Click" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="anchoCompleto centardo textocentradoh">
                    <asp:Panel ID="PanGridRec" runat="server" CssClass="sombra ancho850px centardo">
                        <table class="tablaheadgrids textobold textocentradoh ancho850px centardo">
                            <tr>
                                <td class="ancho100px">
                                    <asp:Label ID="Label9" runat="server" Text="Referencia" />
                                </td>
                                <td class="ancho300px">
                                    <asp:Label ID="Label11" runat="server" Text="Nombre" />
                                </td>
                                <td class="ancho150px">
                                    <asp:Label ID="Label12" runat="server" Text="Fecha" />
                                </td>
                                <td class="ancho250px">
                                    <asp:Label ID="Label13" runat="server" Text="Motivo" />
                                </td>
                                <td class="ancho50px"></td>
                            </tr>
                        </table>
                        <asp:Panel ID="Panel2" runat="server" CssClass="altoConsultas ancho850px centardo" ScrollBars="Auto">
                            <asp:GridView ID="GridRec" runat="server" EmptyDataRowStyle-ForeColor="Red" EmptyDataText="No existe información para mostrar"
                                AutoGenerateColumns="False" ShowHeader="False" GridLines="None"
                                CssClass="colorNegro textocentradoh" DataKeyNames="id_cliente"
                                OnRowDataBound="GridRec_RowDataBound">
                                <Columns>
                                    <asp:TemplateField ItemStyle-CssClass="ancho100px textocentradoh">
                                        <ItemTemplate>
                                            <asp:Label ID="lblReferencia" runat="server" Text='<%# Eval("referencia") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="ancho300px alto40px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("nombre") %>' />
                                            <asp:Label ID="lblTarjetahabiente" runat="server" Text='<%# Eval("tarjetahabiente") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="textocentradoh ancho150px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPagoRechazado" runat="server" Text='<%# Eval("fecha_pago_rechazado","{0:d MMM yyyy}") %>' />
                                            <asp:Label ID="lblPago" runat="server" Text='<%# Eval("fecha_pago","{0:d MMM yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="textocentradoh ancho250px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMotivoRechazo" runat="server" Text='<%# Eval("motivo_rechzado") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField ItemStyle-CssClass="textocentradoh ancho50px">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="btnEditar" runat="server" CommandArgument='<%# Eval("id_cliente") %>' AlternateText="Realizar Pago" ToolTip="Realizar Pago" ImageUrl="~/IMG/edit.png" CssClass="iconitos40" OnClick="btnEditar_Click" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataRowStyle ForeColor="Red" />
                                <PagerSettings Position="Bottom" PageButtonCount="20" Mode="NumericFirstLast" />
                                <PagerStyle CssClass="colorNegro textobold textocentradoh" Font-Underline="false" BackColor="#002C81" HorizontalAlign="Center" VerticalAlign="Middle" ForeColor="White" />
                            </asp:GridView>
                        </asp:Panel>
                        <table class="tablaFootergrids textobold ancho850px centardo">
                            <tr>
                                <td class="ancho760px textoright pading8px">
                                    <asp:Label ID="lblTotales" runat="server" ForeColor="Red" />
                                </td>
                            </tr>
                            <tr>
                                <td class="textocentradoh textocentradov">
                                    <asp:Label ID="lblError" runat="server" ForeColor="Red" />
                                </td>
                            </tr>
                        </table>

                    </asp:Panel>
                    <br />
                    <asp:Panel ID="PanelMask" runat="server" Visible="false" CssClass="mask" />
                    <asp:Panel ID="PanelMeses" runat="server" CssClass="centardo ancho340px popup2 rendondeoPopupPanel" Visible="false">
                        <table class="anchoCompleto">
                            <tr class="tablaheadgrids">
                                <td>
                                    <asp:Label ID="Label4" runat="server" Text="Pagos Atrasados" CssClass="textobold textocentradoh colorBlanco"></asp:Label></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblClientePago" runat="server" Visible="false"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:DataList ID="DataList1" runat="server" CssClass="anchoCompleto"
                                        GridLines="None" DataKeyField="periodo">
                                        <ItemTemplate>
                                            <asp:Label ID="clave" runat="server" Text='<%# Eval("periodo") %>' CssClass="colorNegro center"></asp:Label>
                                            <asp:CheckBox ID="chkPago" runat="server" CssClass="colorNegro center" />
                                            <asp:TextBox ID="txtFolioP" runat="server" CssClass="center"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:DataList>
                                    <asp:Label ID="Label7" runat="server" ForeColor="Red"></asp:Label>
                                </td>
                                <tr class="tablaFootergrids textocentradoh">
                                    <td colspan="2" class="textoright">
                                        <asp:ImageButton ID="btnAgregarPagoM" runat="server" ToolTip="Agregar Pago" OnClick="btnAgregarPagoM_Click" ImageUrl="~/IMG/agregar.png" CssClass="iconitos40" />&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:ImageButton ID="btnCancelarPago" runat="server" ToolTip="Cancelar" ImageUrl="~/IMG/cancelar.png" CssClass="iconitos40" OnClick="btnCancelarPago_Click" />
                                    </td>
                                </tr>
                        </table>
                    </asp:Panel>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlConsultaUsuarios" runat="server" Visible="false">
                <div class="anchoCompleto centardo textocentradoh">
                    <table class="ancho100 centardo">
                        <tr class="textocentradoh textobold centardo">
                            <td class="ancho50 textocentradoh centardo">
                                <asp:Label ID="Label20" runat="server" Text="Año:" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                                <asp:DropDownList ID="ddlAños" runat="server" DataSourceID="SqlDataSource2" DataTextField="valor" DataValueField="año" AutoPostBack="true" OnSelectedIndexChanged="ddlAños_SelectedIndexChanged"></asp:DropDownList>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" SelectCommand="select distinct tabla.año, tabla.valor from (select year(fecha_inicio) as año,cast(year(fecha_inicio) as char(4)) as valor  from clientes) as tabla order by tabla.año desc"></asp:SqlDataSource>
                            </td>
                            <td class="ancho50 textocentradoh centardo">
                                <asp:Label ID="Label19" runat="server" Text="Periodo:" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                                <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="center"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlPeriodo_SelectedIndexChanged">
                                    <asp:ListItem Value="0">Todos</asp:ListItem>
                                    <asp:ListItem Value="1">Enero</asp:ListItem>
                                    <asp:ListItem Value="2">Febrero</asp:ListItem>
                                    <asp:ListItem Value="3">Marzo</asp:ListItem>
                                    <asp:ListItem Value="4">Abril</asp:ListItem>
                                    <asp:ListItem Value="5">Mayo</asp:ListItem>
                                    <asp:ListItem Value="6">Junio</asp:ListItem>
                                    <asp:ListItem Value="7">Julio</asp:ListItem>
                                    <asp:ListItem Value="8">Agosto</asp:ListItem>
                                    <asp:ListItem Value="9">Septiembre</asp:ListItem>
                                    <asp:ListItem Value="10">Octubre</asp:ListItem>
                                    <asp:ListItem Value="11">Noviembre</asp:ListItem>
                                    <asp:ListItem Value="12">Diciembre</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>
                    <asp:DataList ID="dtUsuarios" runat="server" RepeatColumns="1" CssClass="centardo ancho100" DataSourceID="SqlDataSourceUsuarios" OnItemDataBound="dtUsuarios_ItemDataBound">
                        <ItemTemplate>
                            <div class="ancho100">
                                <asp:Label ID="Label14" runat="server" Text="Usuario: " ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                                <asp:Label ID="lblIdUsuario" runat="server" Text='<%# Eval("USUARIO") %>' ForeColor="#002c81" CssClass="textobold center" Visible="false"></asp:Label>
                                <asp:Label ID="lblUsuario" runat="server" Text='<%# Eval("NOMBRE") %>' ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                            </div>
                            <div class="ancho100">
                                <table>
                                    <tr>
                                        <td class="ancho340px">
                                            <telerik:RadHtmlChart runat="server" ID="DonutChartUsuario" Width="300" Height="300" Transitions="true" Skin="Silk"></telerik:RadHtmlChart>
                                        </td>
                                        <td class="ancho900px">
                                            <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" Skin="MetroTouch" Align="Justify" CssClass="ancho900px">
                                                <Tabs>
                                                    <telerik:RadTab PageViewID="RadPageView1" Text="Altas" Selected="true" />
                                                    <telerik:RadTab PageViewID="RadPageView2" Text="Bajas" />
                                                    <telerik:RadTab PageViewID="RadPageView3" Text="Bajas Inmediatas" />
                                                </Tabs>
                                            </telerik:RadTabStrip>
                                            <telerik:RadMultiPage ID="RadMultiPage1" runat="server" CssClass="ancho900px">
                                                <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true" CssClass="ancho900px">
                                                    <telerik:RadGrid RenderMode="Lightweight" ID="radGridAltaUser" runat="server" EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" CssClass="ancho900px"
                                                        AllowPaging="true" ShowFooter="true" PagerStyle-AlwaysVisible="true" AllowSorting="true" PageSize="1000" OnPageIndexChanged="radGridAltaUser_PageIndexChanged" OnSortCommand="radGridAltaUser_SortCommand" OnPageSizeChanged="radGridAltaUser_PageSizeChanged">
                                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_cliente">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="año" HeaderText="Año" SortExpression="año" UniqueName="año" Visible="false" />
                                                                <telerik:GridBoundColumn DataField="referencia" HeaderText="Referencia" SortExpression="referencia" UniqueName="referencia" />
                                                                <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" SortExpression="cliente" UniqueName="cliente" />
                                                                <telerik:GridBoundColumn DataField="nombre" HeaderText="Tarjeta Habiente" SortExpression="nombre" UniqueName="nombre" />
                                                                <telerik:GridBoundColumn DataField="fecha_inicio" HeaderText="Fecha" SortExpression="fecha_inicio" UniqueName="fecha_inicio" DataFormatString="{0:d MMM yyyy}" />
                                                                <telerik:GridBoundColumn DataField="valorEstatus" HeaderText="Estatus" SortExpression="valorEstatus" UniqueName="valorEstatus" />
                                                            </Columns>
                                                            <NoRecordsTemplate>
                                                                <asp:Label ID="lblVacioAlta" runat="server" Text="No existe información de altas" ForeColor="Red"></asp:Label>
                                                            </NoRecordsTemplate>
                                                        </MasterTableView>
                                                        <ClientSettings>
                                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                        </ClientSettings>
                                                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>

                                                    </telerik:RadGrid>
                                                </telerik:RadPageView>
                                                <telerik:RadPageView ID="RadPageView2" runat="server" CssClass="ancho900px">
                                                    <telerik:RadGrid RenderMode="Lightweight" ID="radGridBajasUser" runat="server" EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" CssClass="ancho900px"
                                                        AllowPaging="true" ShowFooter="true" PagerStyle-AlwaysVisible="true" AllowSorting="true" PageSize="1000" OnPageIndexChanged="radGridBajasUser_PageIndexChanged" OnSortCommand="radGridBajasUser_SortCommand" OnPageSizeChanged="radGridBajasUser_PageSizeChanged">
                                                        <MasterTableView AutoGenerateColumns="False">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="referencia" HeaderText="Referencia" SortExpression="referencia" UniqueName="referencia" />
                                                                <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" SortExpression="cliente" UniqueName="cliente" />
                                                                <telerik:GridBoundColumn DataField="tarjeta_habiente" HeaderText="Tarjeta Habiente" SortExpression="tarjeta_habiente" UniqueName="tarjeta_habiente" />
                                                                <telerik:GridBoundColumn DataField="fecha_baja" HeaderText="Fecha" SortExpression="fecha_baja" UniqueName="fecha_baja" DataFormatString="{0:d MMM yyyy}" />
                                                                <telerik:GridBoundColumn DataField="usuarioBaja" HeaderText="Usuario Baja" SortExpression="usuarioBaja" UniqueName="usuarioBaja" />
                                                                <telerik:GridBoundColumn DataField="usuarioAutorizo" HeaderText="Usuario Autorizo" SortExpression="usuarioAutorizo" UniqueName="usuarioAutorizo" />
                                                                <telerik:GridBoundColumn DataField="motivo" HeaderText="Motivo" SortExpression="motivo" UniqueName="motivo" />
                                                            </Columns>
                                                            <NoRecordsTemplate>
                                                                <asp:Label ID="lblVacioBaja" runat="server" Text="No existe información de bajas" ForeColor="Red"></asp:Label>
                                                            </NoRecordsTemplate>
                                                        </MasterTableView>
                                                        <ClientSettings>
                                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                        </ClientSettings>
                                                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                    </telerik:RadGrid>
                                                </telerik:RadPageView>
                                                <telerik:RadPageView ID="RadPageView3" runat="server" CssClass="ancho900px">
                                                    <telerik:RadGrid RenderMode="Lightweight" ID="radGridBajasInmediatasUser" runat="server" EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" CssClass="ancho900px"
                                                        AllowPaging="true" ShowFooter="true" PagerStyle-AlwaysVisible="true" AllowSorting="true" PageSize="1000" OnPageIndexChanged="radGridBajasInmediatasUser_PageIndexChanged" OnSortCommand="radGridBajasInmediatasUser_SortCommand" OnPageSizeChanged="radGridBajasInmediatasUser_PageSizeChanged">
                                                        <MasterTableView AutoGenerateColumns="False">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="referencia" HeaderText="Referencia" SortExpression="referencia" UniqueName="referencia" />
                                                                <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" SortExpression="cliente" UniqueName="cliente" />
                                                                <telerik:GridBoundColumn DataField="tarjeta_habiente" HeaderText="Tarjeta Habiente" SortExpression="tarjeta_habiente" UniqueName="tarjeta_habiente" />
                                                                <telerik:GridBoundColumn DataField="fecha_baja" HeaderText="Fecha" SortExpression="fecha_baja" UniqueName="fecha_baja" DataFormatString="{0:d MMM yyyy}" />
                                                                <telerik:GridBoundColumn DataField="usuarioBaja" HeaderText="Usuario Baja" SortExpression="usuarioBaja" UniqueName="usuarioBaja" />
                                                                <telerik:GridBoundColumn DataField="usuarioAutorizo" HeaderText="Usuario Autorizo" SortExpression="usuarioAutorizo" UniqueName="usuarioAutorizo" />
                                                                <telerik:GridBoundColumn DataField="motivo" HeaderText="Motivo" SortExpression="motivo" UniqueName="motivo" />
                                                            </Columns>
                                                            <NoRecordsTemplate>
                                                                <asp:Label ID="lblVacioBaja" runat="server" Text="No existe información de bajas" ForeColor="Red"></asp:Label>
                                                            </NoRecordsTemplate>
                                                        </MasterTableView>
                                                        <ClientSettings>
                                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                        </ClientSettings>
                                                        <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                    </telerik:RadGrid>
                                                </telerik:RadPageView>
                                            </telerik:RadMultiPage>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ItemTemplate>
                    </asp:DataList>
                    <asp:SqlDataSource ID="SqlDataSourceUsuarios" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlConsultaSocios" runat="server" Visible="false" CssClass="ancho95 centardo">
                <div class="ancho100 centardo textocentradoh">
                    <table>
                        <tr class="ancho100 pading8px">
                            <td class="ancho100">
                                <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" DataSourceID="SqlDataSource5"
                                     ShowFooter="true" AllowFilteringByColumn="True" EnableHeaderContextMenu="true" Skin="Metro" OnSelectedIndexChanged="RadGrid1_SelectedIndexChanged">
                                    <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                                        <Selecting AllowRowSelect="true"></Selecting>
                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2"></Scrolling>
                                    </ClientSettings>
                                    <MasterTableView DataKeyNames="id_cliente,referencia,cliente,tarjetaHabiente" AutoGenerateColumns="False" AllowFilteringByColumn="True">
                                        <Columns>
                                            <telerik:GridBoundColumn DataField="id_cliente" DataType="System.Int32" HeaderText="id_cliente" ReadOnly="True" SortExpression="id_cliente" UniqueName="id_cliente" Visible="false" />
                                            <telerik:GridBoundColumn DataField="referencia" ItemStyle-Width="50px" DataType="System.String" HeaderText="Referencia" ReadOnly="True" SortExpression="referencia" UniqueName="referencia" FilterCheckListEnableLoadOnDemand="false" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                                            <telerik:GridBoundColumn DataField="cliente" ItemStyle-Width="300px" ItemStyle-CssClass="textoIzquierda" DataType="System.String" HeaderText="Cliente" ReadOnly="True" SortExpression="cliente" UniqueName="cliente" FilterCheckListEnableLoadOnDemand="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                                            <telerik:GridBoundColumn DataField="tarjetaHabiente" ItemStyle-Width="300px" ItemStyle-CssClass="textoIzquierda" DataType="System.String" HeaderText="Tarjeta Habiente" ReadOnly="True" SortExpression="tarjetaHabiente" UniqueName="tarjetaHabiente" FilterCheckListEnableLoadOnDemand="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                                            <telerik:GridBoundColumn DataField="fecha_inicio" Visible="false" ItemStyle-Width="80px" ItemStyle-CssClass="textoIzquierda" DataType="System.DateTime" HeaderText="Fecha Alta" ReadOnly="True" SortExpression="fecha_inicio" UniqueName="fecha_inicio" DataFormatString="{0:d MMM yyyy}" />
                                            <telerik:GridBoundColumn DataField="usuario_alta" Visible="false" ItemStyle-Width="80px" ItemStyle-CssClass="textoIzquierda" DataType="System.String" HeaderText="Usuario Alta" ReadOnly="True" SortExpression="usuario_alta" UniqueName="usuario_alta" />
                                            <telerik:GridBoundColumn DataField="telefono" ItemStyle-Width="40px" ItemStyle-CssClass="textoIzquierda" DataType="System.String" HeaderText="Teléfono" ReadOnly="True" SortExpression="telefono" UniqueName="telefono" FilterCheckListEnableLoadOnDemand="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                                            <telerik:GridBoundColumn DataField="celular" ItemStyle-Width="40px" ItemStyle-CssClass="textoIzquierda" DataType="System.String" HeaderText="Celular" ReadOnly="True" SortExpression="celular" UniqueName="celular" FilterCheckListEnableLoadOnDemand="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                                            <telerik:GridBoundColumn DataField="correo" ItemStyle-Width="100px" ItemStyle-CssClass="textoIzquierda" DataType="System.String" HeaderText="Correo" ReadOnly="True" SortExpression="correo" UniqueName="correo" FilterCheckListEnableLoadOnDemand="true" AutoPostBackOnFilter="true" CurrentFilterFunction="Contains" ShowFilterIcon="true" />
                                            <telerik:GridBoundColumn DataField="no_cuenta" Visible="false" ItemStyle-Width="100px" ItemStyle-CssClass="textoIzquierda" DataType="System.String" HeaderText="Cuenta" ReadOnly="True" SortExpression="no_cuenta" UniqueName="no_cuenta" />
                                            <telerik:GridBoundColumn DataField="vigencia_cuenta" Visible="false" ItemStyle-Width="50px" DataType="System.String" HeaderText="Vigencia" ReadOnly="True" SortExpression="vigencia_cuenta" UniqueName="vigencia_cuenta" />
                                            <telerik:GridBoundColumn DataField="nombre" Visible="false" ItemStyle-Width="80px" ItemStyle-CssClass="textoIzquierda" DataType="System.String" HeaderText="Banco" ReadOnly="True" SortExpression="nombre" UniqueName="nombre" />
                                        </Columns>
                                    </MasterTableView>                                    
                                </telerik:RadGrid>
                                <asp:SqlDataSource ID="SqlDataSource5" ConnectionString="<%$ ConnectionStrings:eBills %>" ProviderName="System.Data.SqlClient"
                                    SelectCommand="select c.id_cliente,c.referencia,(c.nombre+' '+c.apellido_paterno+' '+c.apellido_materno) as cliente,
                                        (c.nombre_tarjetahabiente+ ' '+c.ap_pat_tarjetahabiente+' '+c.ap_mat_tarjetahabiente) as tarjetaHabiente,
                                        c.fecha_inicio,c.usuario_alta,c.telefono,c.celular,c.correo,c.no_cuenta,c.vigencia_cuenta,b.nombre
                                        from clientes c 
                                        left join bancos b on b.id_banco=c.id_banco
                                        where c.estatus_cliente='A' AND c.id_empresa=@empresa
                                        order by c.referencia asc"
                                    runat="server">
                                    <SelectParameters>
                                        <asp:QueryStringParameter Name="empresa" QueryStringField="e" DefaultValue="0" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>
                        <tr class=" ancho100 pading8px">
                            <td class="ancho100">
                                <asp:Panel ID="PanelDetalle" runat="server" CssClass="ancho100" Visible="false">
                                    <table>
                                        <tr>
                                            <td class="ancho50">
                                                <div class="ancho100">
                                                    <asp:Label ID="lblidCliente" runat="server" Visible="false"></asp:Label>
                                                    <asp:FormView ID="FormView1" runat="server" DataSourceID="SqlDataSource4">
                                                        <ItemTemplate>
                                                            <table class="textoIzquierda">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label25" runat="server" ForeColor="#002c81" CssClass="textobold" Text="Referencia: "></asp:Label></td>
                                                                    <td colspan="3">
                                                                        <asp:Label Text='<%# Bind("referencia") %>' runat="server" ID="referenciaLabel" ForeColor="#002c81" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label24" runat="server" ForeColor="#002c81" CssClass="textobold" Text="Cliente: "></asp:Label></td>
                                                                    <td colspan="3">
                                                                        <asp:Label Text='<%# Bind("cliente") %>' runat="server" ID="clienteLabel" ForeColor="#002c81" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label30" runat="server" ForeColor="#002c81" CssClass="textobold" Text="Tarjeta Habiente: "></asp:Label></td>
                                                                    <td colspan="3">
                                                                        <asp:Label Text='<%# Bind("tarjetahabiente") %>' runat="server" ID="tarjetahabienteLabel" ForeColor="#002c81" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label31" runat="server" ForeColor="#002c81" CssClass="textobold" Text="Teléfono: "></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label Text='<%# Bind("telefono") %>' runat="server" ID="telefonoLabel" ForeColor="#002c81" /></td>
                                                                    <td>
                                                                        <asp:Label ID="Label32" runat="server" ForeColor="#002c81" CssClass="textobold" Text="Celular: "></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label Text='<%# Bind("celular") %>' runat="server" ID="celularLabel" ForeColor="#002c81" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label33" runat="server" ForeColor="#002c81" CssClass="textobold" Text="Correo: "></asp:Label></td>
                                                                    <td colspan="3">
                                                                        <asp:Label Text='<%# Bind("correo") %>' runat="server" ID="correoLabel" ForeColor="#002c81" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label34" runat="server" ForeColor="#002c81" CssClass="textobold" Text="Inicio: "></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label Text='<%# Bind("fecha_inicio","{0:d MMM yyyy}") %>' runat="server" ID="fecha_inicioLabel" ForeColor="#002c81" /></td>
                                                                    <td>
                                                                        <asp:Label ID="Label35" runat="server" ForeColor="#002c81" CssClass="textobold" Text="Usuario Alta: "></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label Text='<%# Bind("usuario_alta") %>' runat="server" ID="usuario_altaLabel" ForeColor="#002c81" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="Label38" runat="server" ForeColor="#002c81" CssClass="textobold" Text="Banco: "></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label Text='<%# Bind("banco") %>' runat="server" ID="bancoLabel" ForeColor="#002c81" /></td>
                                                                    <td>
                                                                        <asp:Label ID="Label36" runat="server" ForeColor="#002c81" CssClass="textobold" Text="Vigencia: "></asp:Label></td>
                                                                    <td>
                                                                        <asp:Label Text='<%# Bind("vigencia_cuenta") %>' runat="server" ID="vigencia_cuentaLabel" ForeColor="#002c81" /></td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:FormView>
                                                    <asp:SqlDataSource runat="server" ID="SqlDataSource4" ConnectionString='<%$ ConnectionStrings:eBills %>' SelectCommand="select c.referencia,(c.nombre+' '+c.apellido_paterno+' '+c.apellido_materno) as cliente,c.fecha_inicio,b.nombre as banco,
                                                            c.vigencia_cuenta,(c.nombre_tarjetahabiente+' '+c.ap_pat_tarjetahabiente+' '+c.ap_mat_tarjetahabiente) as tarjetahabiente,c.usuario_alta,c.telefono,c.celular,c.correo from clientes c left join bancos b on b.id_banco=c.id_banco where c.id_empresa=@empresa and c.id_cliente=@cliente">
                                                        <SelectParameters>
                                                            <asp:QueryStringParameter QueryStringField="e" DefaultValue="0" Name="empresa"></asp:QueryStringParameter>
                                                            <asp:ControlParameter ControlID="lblidCliente" PropertyName="Text" Name="cliente"></asp:ControlParameter>
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                </div>
                                                <div class="ancho100">
                                                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid2" runat="server" Height="300" DataSourceID="SqlDataSource3" OnItemDataBound="RadGrid2_ItemDataBound"
                                                        Skin="Metro" OnSelectedIndexChanged="RadGrid2_SelectedIndexChanged">
                                                        <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                                                            <Selecting AllowRowSelect="true"></Selecting>
                                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                        </ClientSettings>
                                                        <MasterTableView DataKeyNames="ano" AutoGenerateColumns="False">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="id_cliente" DataType="System.Int32" HeaderText="id_cliente" ReadOnly="True" SortExpression="id_cliente" UniqueName="id_cliente" Visible="false" />
                                                                <telerik:GridBoundColumn DataField="ano" HeaderText="Año" UniqueName="ano" />
                                                                <telerik:GridBoundColumn DataField="Enero" HeaderText="Ene" UniqueName="Enero" />
                                                                <telerik:GridBoundColumn DataField="Febrero" HeaderText="Feb" UniqueName="Febrero" />
                                                                <telerik:GridBoundColumn DataField="Marzo" HeaderText="Mar" UniqueName="Marzo" />
                                                                <telerik:GridBoundColumn DataField="Abril" HeaderText="Abr" UniqueName="Abril" />
                                                                <telerik:GridBoundColumn DataField="Mayo" HeaderText="May" UniqueName="Mayo" />
                                                                <telerik:GridBoundColumn DataField="Junio" HeaderText="Jun" UniqueName="Junio" />
                                                                <telerik:GridBoundColumn DataField="Julio" HeaderText="Jul" UniqueName="Julio" />
                                                                <telerik:GridBoundColumn DataField="Agosto" HeaderText="Ago" UniqueName="Agosto" />
                                                                <telerik:GridBoundColumn DataField="Septiembre" HeaderText="Sep" UniqueName="Septiembre" />
                                                                <telerik:GridBoundColumn DataField="Octubre" HeaderText="Oct" UniqueName="Octubre" />
                                                                <telerik:GridBoundColumn DataField="Noviembre" HeaderText="Nov" UniqueName="Noviembre" />
                                                                <telerik:GridBoundColumn DataField="Diciembre" HeaderText="Dic" UniqueName="Diciembre" />
                                                            </Columns>
                                                        </MasterTableView>
                                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                    </telerik:RadGrid>
                                                    <asp:SqlDataSource ID="SqlDataSource3" ConnectionString="<%$ ConnectionStrings:eBills %>" ProviderName="System.Data.SqlClient" SelectCommand="select id_cliente,ano,p1 as Enero,p2 as Febrero,p3 as Marzo, p4 as Abril ,p5 as Mayo, p6 as Junio,p7 as Julio, p8 as Agosto, p9 as Septiembre,p10 as Octubre, p11 as Noviembre, p12 as Diciembre from periodos_pagos where id_cliente=@cliente order by ano desc" runat="server">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="lblidCliente" Name="cliente" PropertyName="Text" DefaultValue="0" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                </div>
                                                <div class="ancho100">
                                                    <telerik:RadHtmlChart runat="server" ID="DonutChart" Width="300" Height="300" Transitions="true" Skin="Silk"></telerik:RadHtmlChart>
                                                </div>
                                            </td>
                                            <td class="ancho50">
                                                <div class="ancho100">
                                                    <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid3" runat="server" DataSourceID="SqlDataSource6" Skin="Metro" AllowPaging="true" PageSize="20"
                                                        AllowSorting="true" ShowFooter="true" PagerStyle-AlwaysVisible="true" EnableHeaderContextMenu="true">
                                                        <ClientSettings AllowKeyboardNavigation="true" EnablePostBackOnRowClick="true">
                                                            <Selecting AllowRowSelect="true"></Selecting>
                                                            <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                        </ClientSettings>
                                                        <MasterTableView AutoGenerateColumns="False">
                                                            <Columns>
                                                                <telerik:GridBoundColumn DataField="mesAplicado" HeaderText="Periodo" UniqueName="mesAplicado" />
                                                                <telerik:GridBoundColumn DataField="fechaPago" HeaderText="Fecha" UniqueName="fechaPago" DataFormatString="{0:d MMM yyyy}" />
                                                                <telerik:GridBoundColumn DataField="movimiento" HeaderText="Movimineto" UniqueName="movimiento" />
                                                                <telerik:GridBoundColumn DataField="motivo_rechzado" HeaderText="Detalle" UniqueName="motivo_rechzado" />
                                                                <telerik:GridBoundColumn DataField="id_usuario" HeaderText="Usuario" UniqueName="id_usuario" />
                                                                <telerik:GridCheckBoxColumn DataField="noReconocido" HeaderText="No Reconocido" UniqueName="noReconocido" />
                                                                <telerik:GridBoundColumn DataField="id_usuario_noReconocido" HeaderText="Usuario No Reconocido" UniqueName="id_usuario_noReconocido" />
                                                            </Columns>
                                                            <NoRecordsTemplate>
                                                                <asp:Label ID="lblVacio" runat="server" Text="Seleccione un año para mostrar sus movimientos" ForeColor="Red"></asp:Label>
                                                            </NoRecordsTemplate>
                                                        </MasterTableView>
                                                        <PagerStyle Mode="NextPrevAndNumeric"></PagerStyle>
                                                    </telerik:RadGrid>
                                                    <asp:SqlDataSource ID="SqlDataSource6" ConnectionString="<%$ ConnectionStrings:eBills %>" ProviderName="System.Data.SqlClient" SelectCommand="select periodo,consecutivo,
case movimiento when 'R' then fecha_pago_rechazado else fecha_pago end as fechaPago,
case movimiento when 'R' then 'Rechazado' when 'P' then 'Pagado' when 'M' then 'Pago Sucursal' else '' end as movimiento,
motivo_rechzado,noReconocido,id_usuario,id_usuario_noReconocido
,case substring(periodo,5,2) when '01' then 'Enero' when '02' then 'Febrero' when '03' then 'Marzo' when '04' then 'Abril' when '05' then 'Mayo' when '06' then 'Julio' when '07' then 'Julio' when '08' then 'Agosto' when '09' then 'Septiembre' when '10' then 'Octubre' when '11' then 'Noviembre' when '12' then 'Diciembre' else '' end as mesAplicado,
substring(periodo,5,2) as mes
from detalle_cliente 
where id_cliente=@cliente and id_empresa=@empresa and substring(periodo,1,4)=@ano
order by periodo desc, consecutivo desc"
                                                        runat="server">
                                                        <SelectParameters>
                                                            <asp:ControlParameter ControlID="lblidCliente" Name="cliente" PropertyName="Text" DefaultValue="0" />
                                                            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DefaultValue="0" />
                                                            <asp:ControlParameter Name="ano" ControlID="RadGrid2" DefaultValue="0" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                <ProgressTemplate>
                    <div class="ontopDiv">
                        <asp:Panel ID="progress" runat="server" CssClass="zen4 anchoCompleto center textocentradoh padingTop200px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/IMG/25-1.gif" Width="40%" />
                        </asp:Panel>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

