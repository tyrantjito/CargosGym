<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMenus.master" AutoEventWireup="true" CodeFile="BajasMasivas.aspx.cs" Inherits="BajasMasivas" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="PdfViewer1" Namespace="PdfViewer1" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
    function abreWinDet() {
        var oWnd = $find("<%=modalPopUpDetalle.ClientID%>");
        oWnd.setUrl('');
        oWnd.show();
    }

    function cierraWinDet() {
        var oWnd = $find("<%=modalPopUpDetalle.ClientID%>");
        oWnd.close();
    }
    function abreWinBaj() {
        var oWnd = $find("<%=modalPopUpBaja.ClientID%>");
        oWnd.setUrl('');
        oWnd.show();
    }

    function cierraWinBaj() {
        var oWnd = $find("<%=modalPopUpBaja.ClientID%>");
        oWnd.close();
    }

    function abreWinAut() {
        var oWnd = $find("<%=modalPopUpAuto.ClientID%>");
        oWnd.setUrl('');
        oWnd.show();
    }

    function cierraWinAut() {
        var oWnd = $find("<%=modalPopUpAuto.ClientID%>");
        oWnd.close();
    }
</script>

    <h2 class="ancho100">Clientes</h2>
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
    </asp:ScriptManager>
    
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div>
                <div class="anchoCompleto textocentradoh">
                    <table class="centardo anchoCompleto">
                                <tr>
                                  <td class="textocentradoh">
                                        <asp:Label ID="Label27" runat="server" Text="Estatus:" Visible="false" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                                        <asp:DropDownList ID="ddlEstatus" runat="server" Visible="false"  AutoPostBack="True" CssClass="center"
                                            OnSelectedIndexChanged="ddlEstatus_SelectedIndexChanged">
                                            <asp:ListItem Value="A" Selected="True">Activo</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                     <td class="textocentradoh">
                                        <asp:Label ID="Label9" runat="server" Text="Año:" Visible="true" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                                        <asp:DropDownList ID="ddlAño" runat="server" Visible="true" AutoPostBack="True" OnSelectedIndexChanged="ddlAño_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                    <td class="textoright center colorMedio textobold">
                                        <asp:Label ID="Label28" runat="server" Text="Filtros:" Visible="false" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                                        <asp:DropDownList ID="ddlBuscar" runat="server" Visible="false" CssClass="center">
                                            <asp:ListItem Selected="True" Value="0">Todos</asp:ListItem>
                                            <asp:ListItem Value="1">Referencia</asp:ListItem>
                                            <asp:ListItem Value="2">Nombre</asp:ListItem>
                                            <asp:ListItem Value="3">Bancos</asp:ListItem>
                                            <asp:ListItem Value="4">Cuenta</asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp;
                                        <asp:TextBox ID="txtBuscar" runat="server" Visible="false" OnTextChanged="txtBuscar_TextChanged"
                                            CssClass="textboxlog ancho100px center"></asp:TextBox>
                                        <cc1:TextBoxWatermarkExtender ID="txtBuscar_TextBoxWatermarkExtender"  runat="server"
                                            BehaviorID="txtBuscar_TextBoxWatermarkExtender" TargetControlID="txtBuscar" WatermarkCssClass="water ancho100px textboxlog"
                                            WatermarkText="Buscar..." />
                                        <asp:ImageButton ID="btnBuscar" runat="server" Visible="false" AlternateText="Buscar" ImageUrl="~/IMG/buscar.png"
                                            ToolTip="Buscar" CssClass="center iconitos40" OnClick="btnBuscar_Click" />
                                        <asp:LinkButton ID="lnkLimpiar" Visible="false" runat="server" Text="Baja Masiva" CssClass="linkLimpia colorAzul"
                                            ></asp:LinkButton>
                                    </td>
                                    <td class="textocentradoh">
                                         <asp:LinkButton ID="lnkBaja"  runat="server" Text="Bajas Masivas" CssClass="linkLimpia colorAzul"
                                          OnClick="lknInactiva_Click"  OnClientClick="return confirm('¿Está seguro de inactivar los clientes?')"   ></asp:LinkButton>
                                     <asp:ImageButton ID="lknInactiva" runat="server" AlternateText="Inactivar" CommandArgument='<%# Eval("id_cliente")%>'
                                                            Width="60px" ToolTip="Inactivar" ImageUrl="~/IMG/inactiva2.png" OnClick="lknInactiva_Click"
                                                            OnClientClick="return confirm('¿Está seguro de inactivar los clientes?')" />
                                         </td>
                                </tr>
                            </table>
                            <br />
                            <asp:Panel ID="Panel1" runat="server" CssClass="anchoCompleto centardo sombra">                               
                                <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="RadGrid1" ShowFooter="true" Skin="Metro" OnItemDataBound="RadGrid1_ItemDataBound">
                                        <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_cliente">
                                            <Columns>
                                               <telerik:GridTemplateColumn HeaderText="Cliente" Visible="false">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("id_cliente") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Referencia" ItemStyle-CssClass="textoIzquierda">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnReferencia" runat="server" Text='<%# Eval("referencia") %>'
                                                        CommandArgument='<%# Eval("id_cliente")+";"+Eval("referencia") %>' OnClick="btnReferencia_Click"
                                                        CssClass="link" ForeColor="#3ba5d1"></asp:LinkButton>
                                                        <asp:Image ID="imgComentario" runat="server" Width="20px" Height="20px" ImageUrl="~/IMG/Chat3.png" />
                                                        <asp:Image ID="imgInformacion" runat="server" Width="20px" Height="20px" ImageUrl="~/IMG/Info2.png" />
                                                        <asp:Image ID="imgDocumentos" runat="server" Width="20px" Height="20px" ImageUrl="~/IMG/Documents Folder Black.PNG" />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Nombre" ItemStyle-CssClass="textoIzquierda">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label10" runat="server" Text='<%# Eval("nombre_completo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Monto">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label15" runat="server" Text='<%# String.Format("{0:C}",Eval("monto")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Cuenta">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label16" runat="server" Text='<%# Eval("cuenta") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Banco" ItemStyle-CssClass="textoIzquierda">
                                                    <ItemTemplate>
                                                        <asp:Label ID="Label19" runat="server" Text='<%# Eval("banco") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false" HeaderText="Elimina">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lknEliminar" runat="server" AlternateText="Eliminar" CommandArgument='<%# Eval("id_cliente")%>'
                                                            Width="20px" ToolTip="Eliminar" ImageUrl="~/IMG/eliminar.png" OnClick="lknEliminar_Click"
                                                            OnClientClick="return confirm('¿Está seguro de eliminar el Cliente?, tome en cuenta que todo su historial y registro se eliminará permanentemente')" />
                                                    </ItemTemplate>                                                    
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false" HeaderText="Inactiva / Reactiva">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lknActiva" runat="server" AlternateText="Activar" CommandArgument='<%# Eval("id_cliente")%>'
                                                            Width="20px" ToolTip="Activar" ImageUrl="~/IMG/activa.png" OnClick="lknActiva_Click"
                                                            OnClientClick="return confirm('¿Está seguro de reactivar el cliente?')" />
                                                        <asp:ImageButton ID="lknInactiva" runat="server" AlternateText="Inactivar" CommandArgument='<%# Eval("id_cliente")%>'
                                                            Width="20px" ToolTip="Inactivar" ImageUrl="~/IMG/inactiva.png" OnClick="lknInactiva_Click"
                                                            OnClientClick="return confirm('¿Está seguro de inactivar el cliente?')" />
                                                    </ItemTemplate>                                                    
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false" HeaderText="Editar">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="lknEditar" runat="server" AlternateText="Editar" CommandArgument='<%# Eval("id_cliente")%>'
                                                            Width="20px" ToolTip="Editar" ImageUrl="~/IMG/edit.png" OnClick="lknEditar_Click" />
                                                    </ItemTemplate>                                                    
                                                </telerik:GridTemplateColumn>
                                      <telerik:GridTemplateColumn HeaderText="Ene">
                                                    <ItemTemplate>
                                                        <asp:Label ID="P1" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Feb">
                                                    <ItemTemplate>
                                                        <asp:Label ID="P2" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Mar">
                                                    <ItemTemplate>
                                                        <asp:Label ID="P3" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Abr">
                                                    <ItemTemplate>
                                                        <asp:Label ID="P4" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="May">
                                                    <ItemTemplate>
                                                        <asp:Label ID="P5" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Jun">
                                                    <ItemTemplate>
                                                        <asp:Label ID="P6" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Jul">
                                                    <ItemTemplate>
                                                        <asp:Label ID="P7" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Ago">
                                                    <ItemTemplate>
                                                        <asp:Label ID="P8" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Sep">
                                                    <ItemTemplate>
                                                        <asp:Label ID="P9" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Oct">
                                                    <ItemTemplate>
                                                        <asp:Label ID="P10" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Nov">
                                                    <ItemTemplate>
                                                        <asp:Label ID="P11" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Dic">
                                                    <ItemTemplate>
                                                        <asp:Label ID="P12" runat="server" Text=""></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                 <telerik:GridTemplateColumn HeaderText="Masiva"  UniqueName="ConfirmOrder" DataField='DataIDValue'>
                                                   <ItemTemplate>
                                                       <asp:CheckBox ID="ina" runat="server"  CommandArgument='<%# Eval("id_cliente") %>' AutoPostBack="true"/>
                                                   </ItemTemplate>
                                               </telerik:GridTemplateColumn>  
                                            </Columns>
                                            <NoRecordsTemplate>
                                                <asp:Label ID="lblVacioBaja" runat="server" Text="No hay clientes registrados" ForeColor="Red"></asp:Label>
                                            </NoRecordsTemplate>
                                        </MasterTableView>
                                        <ClientSettings>
                                            <Scrolling UseStaticHeaders="True" ></Scrolling>                                        
                                        </ClientSettings>                                    
                                    </telerik:RadGrid>                               
                                <table class="anchoCompleto centardo textocentradoh tablaFootergrids sinRedondeo">
                                    <tr>
                                        <td class="ancho50 textobold pading8px">
                                            <asp:Label ID="Label22" runat="server" Text="Total de Clientes Activos:" CssClass="colorObscuro"></asp:Label>&nbsp;<asp:Label
                                                ID="lblClientesActivos" runat="server" CssClass="colorBlanco"></asp:Label>
                                        </td>
                                        <td class="ancho50 textobold pading8px">
                                            <asp:Label ID="Label26" runat="server" Text="Total de Clientes Inactivos:" CssClass="colorObscuro"></asp:Label>&nbsp;<asp:Label
                                                ID="lblClientesInactivos" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr class="textocentradoh">
                                        <td class="ancho100 textobold pading8px" colspan="2">
                                            <asp:LinkButton ID="lnkCargas" runat="server" OnClick="lnkCargas_Click" CssClass="link colorBlanco">Consulta Cargos</asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                                <!-- Crea Cliente-->
                            </asp:Panel>
                            <br />
                            <asp:Label ID="Label36" runat="server" Text="Pagado" ForeColor="LimeGreen"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label37" runat="server" Text="Rechazado" ForeColor="Red"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label38" runat="server" Text="Pago Sucursal" ForeColor="LightSkyBlue"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label39" runat="server" Text="Contracargo" ForeColor="Yellow"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label1" runat="server" Text="Reactivación" ForeColor="DarkOrange"></asp:Label>
                            <br />
                            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                </div>                

                <!-- Popup Detalle Cliente -->
                    <!-- Detalle de Cliente -->
                <asp:Panel ID="Panel4" runat="server" Visible="false" CssClass="ancho95 redondeoPopPanel" Style="position: fixed; left: 10px; top: 10px; z-index: 2; background-color: White; margin: 0 auto; text-align: center;">
                    
                    
                </asp:Panel>
                <!-- Popup modificacion -->
                <asp:Panel ID="PanelMod" runat="server" Style="position: fixed; left: 400px; top: 100px; z-index: 2; background-color: White; margin: 0 auto; text-align: center; width: 40%;"
                    CssClass="redondeoPopPanel" Visible="false">
                    
                </asp:Panel>
                <!-- Popup AltaCliente -->
                <asp:Panel ID="PanelAlta" runat="server" Style="position: fixed; left: 400px; top: 100px; z-index: 2; background-color: White; margin: 0 auto; text-align: center; width: 40%;"
                    CssClass="redondeoPopPanel" Visible="false">
                    
                </asp:Panel>


                <!-- Popup Baja -->
                <asp:Panel ID="Panel5" runat="server" Style="position: fixed; left: 400px; top: 100px; z-index: 2; background-color: White; margin: 0 auto; text-align: center; width: 30%;"
                    CssClass="redondeoPopPanel" Visible="false">
                    
                </asp:Panel>

                <asp:Panel ID="Panel6" runat="server" Visible="false" CssClass="mask" />
                
                <!-- Pop Up zoom-->
                <asp:Panel ID="PanelImgZoom" runat="server" Style="position: fixed; left: 2px; top: 2px; z-index: 4; background-color: White; margin: 0 auto; text-align: center; width: 100%;" CssClass="redondeoPopPanel">
                    <table class="ancho100">
                        <tr class="tablaheadgrids textocentradoh textobold colorMedio headerPanelPop ancho100">
                            <td class="ancho100 textocentradoh headerPanelPop">
                                <asp:Label ID="Label61" runat="server" Text="Documento" CssClass="t22 colorMorado textoBold" />
                                <asp:ImageButton ID="btnCerrarImgZomm" runat="server" ToolTip="Cerrar" OnClick="btnCerrarImgZomm_Click" ImageUrl="~/IMG/close.png" CssClass="iconitos40" />
                                <asp:ImageButton ID="btnCerrarImgZomm1" runat="server" ToolTip="Cerrar" OnClick="btnCerrarImgZomm1_Click" ImageUrl="~/IMG/close.png" CssClass="iconitos40" />
                            </td>
                        </tr>
                    </table>
                    <div class="ancho100">
                        <div class="ancho100 textocentradoh">
                            <asp:Panel ID="Panel8" runat="server" Height="600px" ScrollBars="Auto" CssClass="ancho100">
                                <asp:Image ID="imgZoom" runat="server" CssClass="ancho100" AlternateText="Imagen no disponible" />
                            </asp:Panel>
                            <asp:Panel ID="pnlPdf" runat="server" CssClass="ancho100" ScrollBars="Auto">
                                <cc2:ShowPdf ID="ShowPdf1" runat="server" Height="550px" Width="856px" BorderStyle="Inset" BorderWidth="2px" />
                            </asp:Panel>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Panel de Autorizacion-->
                <asp:Panel ID="pnlAutorizacion" runat="server" Style="position: fixed; left: 400px; top: 100px; z-index: 2; background-color: White; margin: 0 auto; text-align: center; width: 30%;"
                    CssClass="redondeoPopPanel" Visible="false">
                    
                </asp:Panel>

            </div>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
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
    
    <telerik:RadWindow RenderMode="Lightweight" ID="modalPopUpDetalle" Title="Detalle de Cliente" EnableShadow="true" Skin="Metro"
        Behaviors="Close,Maximize,Move,Resize" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="1000px" Height="700px" Style="z-index: 1000;" InitialBehaviors="Maximize">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanelEmi" runat="server">
                <ContentTemplate>
                    <div class="ancho95 centrado">
                        <table class="textocentradoh ancho100 pading8px">
                            <tr>
                                <td colspan="2">
                                    <table class="anchoCompleto">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCliente" runat="server" Visible="false"></asp:Label>
                                                <asp:Label ID="lblRefernciaCliente" runat="server" CssClass="textobold colorNegro center"></asp:Label>&nbsp;&nbsp;&nbsp;
                                                <asp:Label ID="lblClienteNombre" runat="server" CssClass="textobold colorNegro center"></asp:Label><br />
                                                <asp:Label ID="lblUsuarioAlta" runat="server" CssClass="textobold colorNegro center"></asp:Label><br />
                                                <asp:Label ID="lblFechaInicio" runat="server" CssClass="textobold colorNegro center"></asp:Label>
                                            </td>                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <asp:Panel ID="Panel3" runat="server" CssClass="ancho100">
                        <asp:Label ID="lblErroresComentarios" runat="server" ForeColor="Red"></asp:Label>
                            <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" Skin="MetroTouch" Align="Left">
                                <Tabs>
                                    <telerik:RadTab PageViewID="RadPageView1" Text="Detalle de Pagos" />
                                    <telerik:RadTab PageViewID="RadPageView2" Text="Pagos Sucursal y/o Atrasados" />                                
                                    <telerik:RadTab PageViewID="RadPageView4" Text="Comentarios" />
                                </Tabs>
                            </telerik:RadTabStrip>
                            <telerik:RadMultiPage ID="RadMultiPage1" runat="server" CssClass="anchoCompleto">
                                <telerik:RadPageView ID="RadPageView1" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:Panel ID="Panel2" runat="server" CssClass="ancho100" ScrollBars="Auto">                                                                                    
                                                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" GridLines="None"
                                                    HeaderStyle-CssClass="tablaheadgrids" CssClass="textocentradoh anchoCompleto" OnRowDataBound="GridView2_RowDataBound"                                            
                                                    DataKeyNames="CONSECUTIVO" OnRowCancelingEdit="GridView2_RowCancelingEdit"
                                                    OnRowEditing="GridView2_RowEditing" OnRowUpdating="GridView2_RowUpdating">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Fecha Cargo" HeaderStyle-CssClass="colorBlanco">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label21" runat="server" Text='<%# Eval("fecha","{0:d MMM yyyy}") %>'></asp:Label>
                                                                <asp:Label ID="lblFechaMovimientoMod" runat="server" Text='<%# Eval("fecha") %>' Visible="false"></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="colorNegro" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Periodo Aplicado"  HeaderStyle-CssClass="colorBlanco">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label24" runat="server" Text='<%# Eval("periodo") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="colorNegro" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Motivo Rechazo"  HeaderStyle-CssClass="colorBlanco" >
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label23" runat="server" Text='<%# Eval("motivo_rechzado") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="colorNegro" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Estatus"  HeaderStyle-CssClass="colorBlanco">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label241" runat="server" Text='<%# Eval("Estatus") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="colorNegro" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Usuario Cobro"  HeaderStyle-CssClass="colorBlanco">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblUsuarioCobro" runat="server" Text='<%# Eval("usuario") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="colorNegro" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Consecutivo" Visible="false">
                                                            <ItemTemplate>
                                                                <asp:Label ID="Label25" runat="server" Text='<%# Eval("CONSECUTIVO") %>'></asp:Label>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="No Reconocido"  HeaderStyle-CssClass="colorBlanco">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkNoReconocido" runat="server" Checked='<%# Eval("noReconocido") %>' Enabled="false" />
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:CheckBox ID="chkNoReconocidoMod" runat="server" Checked='<%# Eval("noReconocido") %>' />
                                                            </EditItemTemplate>
                                                            <ItemStyle CssClass="colorNegro" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkEditar" runat="server" CausesValidation="False" CommandName="Edit" Text="Editar"></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <EditItemTemplate>
                                                                <asp:LinkButton ID="lnkActualizar" runat="server" CausesValidation="True" CommandArgument='<%# Eval("CONSECUTIVO") %>' CommandName="Update" Text="Actualizar"></asp:LinkButton>
                                                                &nbsp;<asp:LinkButton ID="lnkCancelar" runat="server" CausesValidation="False" CommandName="Cancel" Text="Cancelar"></asp:LinkButton>
                                                            </EditItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="Label25" runat="server" ForeColor="Red" Text="No existe detalle del cliente"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <HeaderStyle CssClass="tablaheadgrids" />
                                                </asp:GridView>
                                            </asp:Panel>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </telerik:RadPageView>
                                <telerik:RadPageView ID="RadPageView2" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                        <ContentTemplate>                                        
                                            <asp:Panel ID="Panel9" runat="server" CssClass="ancho100" ScrollBars="Auto">
                                                <asp:DataList ID="DataList1" runat="server"  CssClass="ancho100" GridLines="None" DataKeyField="periodo" RepeatColumns="3">
                                                    <ItemTemplate>
                                                        <asp:Label ID="clave" runat="server" Text='<%# Eval("periodo") %>' CssClass="colorNegro center"></asp:Label>
                                                        <asp:CheckBox ID="chkPago" runat="server" CssClass="colorNegro center" />
                                                        <asp:TextBox ID="txtFolioP" runat="server" CssClass="center"></asp:TextBox>
                                                    </ItemTemplate>                                                            
                                                </asp:DataList>
                                                <br /><br />
                                                <asp:Label ID="lblErrorSucursal" runat="server" ForeColor="Red"></asp:Label>
                                            </asp:Panel>
                                            <div class="tablaFootergrids textoright">
                                                <asp:ImageButton ID="btnAgregarPagoM" runat="server" ToolTip="Agregar Pago" OnClick="btnAgregarPagoM_Click" ImageUrl="~/IMG/agregar.png" CssClass="iconitos40" />
                                            </div>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </telerik:RadPageView>                            
                                <telerik:RadPageView ID="RadPageView3" runat="server">
                                    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                                        <ContentTemplate>
                                            <table class="ancho10">
                                                <tr>
                                                    <td><asp:TextBox ID="txtComentario" runat="server" TextMode="MultiLine" Rows="5" CssClass="ancho250px ancho70"></asp:TextBox></td>
                                                    <td><asp:ImageButton ID="lnkAgregaComentario" runat="server" ImageUrl="~/IMG/agregar.png" CssClass="iconitos40" AlternateText="Agregar Comentario" ToolTip="Agregar Comentario" OnClick="lnkAgregaComentario_Click" /></td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="Panel10" runat="server" CssClass="ancho100" ScrollBars="Auto">
                                                <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" GridLines="None" HeaderStyle-CssClass="tablaheadgrids" CssClass="textocentradoh anchoCompleto" OnRowDataBound="GridView3_RowDataBound" DataSourceID="SqlDataSource21">
                                                    <Columns>
                                                        <asp:BoundField DataField="consecutivo" HeaderText="Consecutivo" SortExpression="consecutivo" Visible="false" />
                                                        <asp:BoundField DataField="comentario" HeaderText="Comentario" SortExpression="comentario" ControlStyle-ForeColor="Black" ItemStyle-CssClass="colorNegro"  HeaderStyle-CssClass="colorBlanco"/>
                                                        <asp:BoundField DataField="id_usuario" HeaderText="Usuario" SortExpression="id_usuario" ControlStyle-ForeColor="Black" ItemStyle-CssClass="colorNegro"  HeaderStyle-CssClass="colorBlanco"/>
                                                        <asp:BoundField DataField="fecha" HeaderText="Fecha" SortExpression="fecha" DataFormatString="{0:d MMM yyyy}" ControlStyle-ForeColor="Black" ItemStyle-CssClass="colorNegro"  HeaderStyle-CssClass="colorBlanco"/>
                                                        <asp:BoundField DataField="hora" HeaderText="Hora" SortExpression="hora" ControlStyle-ForeColor="Black" ItemStyle-CssClass="colorNegro"  HeaderStyle-CssClass="colorBlanco"/>
                                                        <asp:CheckBoxField DataField="leido" HeaderText="Leido" SortExpression="leido"  HeaderStyle-CssClass="colorBlanco"/>
                                                        <asp:BoundField DataField="usuario_lectura" HeaderText="Usuario Lectura" SortExpression="usuario_lectura" ControlStyle-ForeColor="Black" ItemStyle-CssClass="colorNegro"  HeaderStyle-CssClass="colorBlanco"/>
                                                        <asp:BoundField DataField="fecha_lectura" HeaderText="Fecha Lectura" SortExpression="fecha_lectura" ControlStyle-ForeColor="Black" DataFormatString="{0:d MMM yyyy}" ItemStyle-CssClass="colorNegro"  HeaderStyle-CssClass="colorBlanco"/>
                                                        <asp:TemplateField HeaderText="Hora Lectura" SortExpression="hora_lectura" ControlStyle-ForeColor="Black" ItemStyle-CssClass="colorNegro"  HeaderStyle-CssClass="colorBlanco">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblFechaLectura" runat="server" Text='<%# Bind("hora_lectura") %>' />
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lnkLeido" runat="server" CausesValidation="False"
                                                                    CommandArgument='<%# Eval("consecutivo") %>' Text="Marcar como leido" CssClass="linkLimpia colorAzul" OnClick="lnkLeido_Click"></asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <EmptyDataTemplate>
                                                        <asp:Label ID="Label25" runat="server" ForeColor="Red" Text="No existen comentarios"></asp:Label>
                                                    </EmptyDataTemplate>
                                                    <HeaderStyle CssClass="tablaheadgrids" />
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:SqlDataSource ID="SqlDataSource21" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" 
                                                SelectCommand="select id_comentario as consecutivo,comentario,id_usuario,fecha,hora,leido,isnull(usuario_lectura,'')as usuario_lectura,fecha_lectura,hora_lectura from comentarios where id_cliente=@cliente and id_empresa=@empresa order by id_comentario desc">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="lblCliente" Name="cliente" PropertyName="Text" />
                                                    <asp:QueryStringParameter DefaultValue="0" Name="empresa" QueryStringField="e" />
                                                </SelectParameters>
                                            </asp:SqlDataSource>
                                            <br /><br />                                                                                        
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </telerik:RadPageView>
                            </telerik:RadMultiPage>
                        </asp:Panel>
                    </div>
                    <asp:UpdateProgress ID="UpdateProgress4" runat="server" AssociatedUpdatePanelID="UpdatePanelEmi">
                        <ProgressTemplate>
                            <div class="ontopDiv">
                                <asp:Panel ID="progressEmi" runat="server" CssClass="zen4 anchoCompleto center textocentradoh padingTop200px">
                                    <asp:Image ID="ImageEMi" runat="server" ImageUrl="~/IMG/25-1.gif" Width="40%" />
                                </asp:Panel>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>

    <telerik:RadWindow RenderMode="Lightweight" ID="modalPopUpBaja" Title="Bajas de Clientes" EnableShadow="true" Skin="Metro"
        Behaviors="Close,Maximize,Move,Resize" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="550px" Height="300px" Style="z-index: 1000;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                <ContentTemplate>
                    <div class="ancho95 centrado">
                        <table class="ancho100">                            
                            <tr>
                                <td>
                                    <asp:Label ID="lblClienteInactiva" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="Label40" runat="server" Text="Fecha Bajas:" ForeColor="#002c81" CssClass="textobold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtFechaBaja" runat="server" MaxLength="10" Enabled="false"></asp:TextBox>
                                    <cc1:CalendarExtender ID="txtFechaBaja_CalendarExtender" runat="server" BehaviorID="txtFechaBaja_CalendarExtender"
                                        TargetControlID="txtFechaBaja" Format="yyyy-MM-dd" PopupButtonID="btnFechaBaja" />
                                    <asp:ImageButton ID="btnFechaBaja" runat="server" ImageUrl="~/IMG/calendario.png" CssClass="iconitos40" />
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ErrorMessage="Debe indicar la fecha de baja con formato 'aaaa-mm-dd'"
                                        ControlToValidate="txtFechaBaja" ValidationGroup="baja" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator11" runat="server"
                                        ErrorMessage="Debe indicar una fecha de baja válida con formato 'aaaa-mm-dd'"
                                        ValidationGroup="baja" ControlToValidate="txtFechaBaja" Text="*" ForeColor="Red"
                                        ValidationExpression="^\d{2,4}\-\d{1,2}\-\d{1,2}$"></asp:RegularExpressionValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label41" runat="server" Text="Motivo Bajas:" ForeColor="#002c81" CssClass="textobold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMotivoBaja" runat="server" MaxLength="200" TextMode="MultiLine" Height="100" Width="300"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ErrorMessage="Debe indicar el motivo de la baja" ControlToValidate="txtMotivoBaja" ValidationGroup="baja" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label6" runat="server" Text="Baja Inmediata:" ForeColor="#002c81" CssClass="textobold"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:CheckBox ID="chkBajaInmediata" runat="server" AutoPostBack="true" OnCheckedChanged="chkBajaInmediata_CheckedChanged" />
                                </td>
                            </tr>
                           
                            <tr>
                                <td colspan="3" class="textocentradoh">
                                    <asp:Label ID="lblErrorBaja" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="baja" ForeColor="Red" DisplayMode="List" />
                                </td>
                            </tr>
                            <tr>
                                <td class="textoright" colspan="3" style="background-color: #002c81;">
                                    <asp:ImageButton ID="btnBaja" runat="server" ImageUrl="~/IMG/aceptar.png" CssClass="iconitos40" OnClick="btnBaja_Click" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="btnCancelar" runat="server" ImageUrl="~/IMG/cancelar.png" CssClass="iconitos40" OnClientClick="cierraWinBaj()" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:UpdateProgress ID="UpdateProgress5" runat="server" AssociatedUpdatePanelID="UpdatePanel7">
                        <ProgressTemplate>
                            <div class="ontopDiv">
                                <asp:Panel ID="progress7" runat="server" CssClass="zen4 anchoCompleto center textocentradoh padingTop200px">
                                    <asp:Image ID="Image7" runat="server" ImageUrl="~/IMG/25-1.gif" Width="40%" />
                                </asp:Panel>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>

    <telerik:RadWindow RenderMode="Lightweight" ID="modalPopUpAuto" Title="Autorizaciónes" EnableShadow="true" Skin="Metro"
        Behaviors="Close,Maximize,Move,Resize" VisibleOnPageLoad="false" ShowContentDuringLoad="false" DestroyOnClose="true"
        Animation="Fade" runat="server" Modal="true" Width="400px" Height="200px" Style="z-index: 3000;">
        <ContentTemplate>
            <asp:UpdatePanel ID="UpdatePanel8" runat="server">
                <ContentTemplate>
                    <div class="ancho95 centrado">
                        <table class="ancho100 pading8px">                            
                            <tr>
                                <td>
                                    <asp:Label ID="Label17" runat="server" Text="Usuario:" ForeColor="#002c81" CssClass="textobold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtUsuario" runat="server" MaxLength="20" CssClass="textboxlog ancho150px center"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator50" runat="server" Text="*"
                                        ForeColor="Red" ValidationGroup="aut" ControlToValidate="txtUsuario" ErrorMessage="Necesita llenar el usuario." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator51" runat="server"
                                        Text="*" ForeColor="Red" ValidationGroup="aut" ErrorMessage="El usuario debe un máximo de 20 caracteres."
                                        ControlToValidate="txtUsuario" ValidationExpression="[a-zA-Z0-9]{3,20}" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="Label20" runat="server" Text="Contraseña:" ForeColor="#002c81" CssClass="textobold"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContraseñaLog" runat="server" CssClass="textboxlog ancho150px center"
                                        TextMode="Password" MaxLength="20" />
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator52" runat="server" Text="*"
                                        ForeColor="Red" ValidationGroup="aut" ControlToValidate="txtContraseñaLog" ErrorMessage="Necesita llenar la contraseña." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator53" runat="server"
                                        Text="*" ForeColor="Red" ValidationGroup="aut" ErrorMessage="La contraseña debe contener como mínimo 5 y un máximo de 20 caracteres."
                                        ControlToValidate="txtContraseñaLog" ValidationExpression="[a-zA-Z0-9]{5,20}" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" class="textocentradoh">
                                    <asp:Label ID="lblErrorAutenticacion" runat="server" ForeColor="Red"></asp:Label>
                                    <asp:ValidationSummary ID="ValidationSummary5" runat="server" ValidationGroup="aut"
                                        ForeColor="Red" DisplayMode="List" />
                                </td>
                            </tr>
                            <tr class="ancho100">
                                <td class="textoright" colspan="3" style="background-color: #002c81;">
                                    <asp:ImageButton ID="btnAutentica" runat="server" ImageUrl="~/IMG/aceptar.png" CssClass="iconitos40"
                                        OnClick="btnAutentica_Click" ValidationGroup="aut" />
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="btnCancAut" runat="server" ImageUrl="~/IMG/cancelar.png" CssClass="iconitos40"
                                        OnClick="btnCancAut_Click" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" AssociatedUpdatePanelID="UpdatePanel8">
                        <ProgressTemplate>
                            <div class="ontopDiv">
                                <asp:Panel ID="progress8" runat="server" CssClass="zen4 anchoCompleto center textocentradoh padingTop200px">
                                    <asp:Image ID="Image8" runat="server" ImageUrl="~/IMG/25-1.gif" Width="40%" />
                                </asp:Panel>
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </ContentTemplate>
            </asp:UpdatePanel>
        </ContentTemplate>
    </telerik:RadWindow>
</asp:Content>

