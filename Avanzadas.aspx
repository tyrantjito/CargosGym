<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Avanzadas.aspx.cs" Inherits="Avanzadas"
    MasterPageFile="~/MasterMenus.master" %>

<%@ Register Assembly="PdfViewer1" Namespace="PdfViewer1" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <h2 class="anchoCompleto">Avanzadas</h2>
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="ancho50 centardo">        
                <asp:RadioButtonList ID="rdlOpcion" runat="server" ForeColor="#002c81" RepeatColumns="4"
                    RepeatDirection="Horizontal" AutoPostBack="True" CssClass="ancho100 centardo textocentradoh"
                    OnSelectedIndexChanged="rdlOpcion_SelectedIndexChanged">
                    <asp:ListItem Value="A">Actualización Monto Clientes Registrados</asp:ListItem>
                    <asp:ListItem Value="C">Cambio de Referencia</asp:ListItem>
                    <asp:ListItem Value="D">Documentos</asp:ListItem>
                    <asp:ListItem Value="V">Bajas Masivas</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                <ProgressTemplate>
                    <div class="ontopDiv">
                        <asp:Panel ID="PanGif1" runat="server" CssClass="zen4 anchoCompleto center textocentradoh padingTop200px">
                            <asp:Image ID="gif1" runat="server" ImageUrl="~/IMG/25-1.gif" AlternateText="Cargando. . ."
                                Width="40%" />
                        </asp:Panel>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Cambio de Montos -->
            <asp:Panel ID="Panel1" runat="server" CssClass="ancho30 sombra montoPanel textobold"
                Visible="false">
                <table class="anchoCompleto tablaheadgrids textobold colorBlanco">
                    <tr>
                        <td style="height: 35px; text-align: center; vertical-align: middle;">
                            <asp:Label ID="Label61" runat="server" Text="Actualización de Monto a Clientes Registrados" />
                        </td>
                    </tr>
                </table>
                <table class="pading8px anchoCompleto">
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Monto" ForeColor="#002c81" />
                        </td>
                        <td class="textocentradoh">
                            <asp:TextBox ID="txtMonto" runat="server" CssClass="textboxlog ancho100px" MaxLength="9"
                                ToolTip="Nuevo Monto" ValidationGroup="monto" />
                            <cc1:TextBoxWatermarkExtender ID="txtMonto_TextBoxWatermarkExtender" runat="server"
                                BehaviorID="txtMonto_TextBoxWatermarkExtender" TargetControlID="txtMonto" WatermarkCssClass="water ancho100px textboxlog "
                                WatermarkText="Indique monto" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe ingresar el monto nuevo a modificar"
                                ControlToValidate="txtMonto" Text="*" ForeColor="Red" ValidationGroup="monto" />
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Solo puede ingresar dígitos con dos decimales"
                                ControlToValidate="txtMonto" Text="*" ForeColor="Red" ValidationExpression="\d*[0-9]{1,10}\.?\d[0-9]{0,2}|\d*[0-9]{1,10}"
                                ValidationGroup="monto" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="textocentradoh">
                            <asp:ValidationSummary ID="ValidationSummary1" ForeColor="Red" runat="server" ValidationGroup="monto"
                                DisplayMode="List" />
                            <asp:Label ID="lblError" runat="server" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
                <table class="anchoCompleto tablaFootergrids">
                    <tr>
                        <td class="textoright">
                            <asp:ImageButton ID="btnActualizar" runat="server" CssClass="iconitos40" ValidationGroup="monto"
                                ImageUrl="~/IMG/aceptar.png" OnClick="btnActualizar_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <!-- Cambio de Referencia -->
            <asp:Panel ID="Panel2" runat="server" CssClass="ancho30 sombra montoPanel textobold"
                Visible="false">
                <table class="anchoCompleto tablaheadgrids textobold colorBlanco">
                    <tr>
                        <td style="height: 35px; text-align: center; vertical-align: middle;">
                            <asp:Label ID="Label3" runat="server" Text="Cambio de Referencia" />
                        </td>
                    </tr>
                </table>
                <table class="pading8px anchoCompleto">
                    <tr>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Referencia Actual:" ForeColor="#002c81" />
                        </td>
                        <td class="textocentradoh">
                            <asp:TextBox ID="txtAltaReferencia" runat="server" CssClass="ancho140px textboxlog"
                                MaxLength="100"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="txtAltaReferencia_FilteredTextBoxExtender" runat="server"
                                BehaviorID="txtAltaReferencia_FilteredTextBoxExtender" TargetControlID="txtAltaReferencia"
                                FilterType="Numbers,Custom" ValidChars="-" />
                            <cc1:TextBoxWatermarkExtender ID="txtAltaReferencia_TextBoxWatermarkExtender" runat="server"
                                BehaviorID="txtAltaReferencia_TextBoxWatermarkExtender" TargetControlID="txtAltaReferencia"
                                WatermarkText="Referencia" WatermarkCssClass="water ancho140px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Referencia Nueva:" ForeColor="#002c81" />
                        </td>
                        <td class="textocentradoh">
                            <asp:TextBox ID="txtRefNuevo" runat="server" CssClass="ancho140px textboxlog" MaxLength="100"></asp:TextBox>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" BehaviorID="txtRefNuevo_FilteredTextBoxExtender"
                                TargetControlID="txtRefNuevo" FilterType="Numbers" />
                            <cc1:TextBoxWatermarkExtender ID="txtRefNuevoWatermarkExtender1" runat="server" BehaviorID="txtRefNuevo_TextBoxWatermarkExtender"
                                TargetControlID="txtRefNuevo" WatermarkText="Referencia" WatermarkCssClass="water ancho140px" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ErrorMessage="Debe indicar la nueva referencia"
                                ControlToValidate="txtRefNuevo" ValidationGroup="alta" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="textocentradoh">
                            <asp:ValidationSummary ID="ValidationSummary2" ForeColor="Red" runat="server" ValidationGroup="alta"
                                DisplayMode="List" />
                            <asp:Label ID="lblErrorRefe" runat="server" ForeColor="Red" />
                        </td>
                    </tr>
                </table>
                <table class="anchoCompleto tablaFootergrids">
                    <tr>
                        <td class="textoright">
                            <asp:ImageButton ID="btnCambiar" runat="server" CssClass="iconitos40" ValidationGroup="alta"
                                ImageUrl="~/IMG/aceptar.png" OnClick="btnCambiar_Click" />
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <!-- Documentos -->
            <asp:Panel ID="Panel3" runat="server" CssClass="ancho95 centardo textobold">
                <table class="pading8px ancho50 centardo">
                    <tr>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Estatus" ForeColor="#002c81" />
                        </td>
                        <td class="textocentradoh">
                            <asp:DropDownList ID="ddlEstatus" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlEstatus_SelectedIndexChanged">
                                <asp:ListItem Selected="True" Value="A">Activos</asp:ListItem>
                                <asp:ListItem Value="I">Inactivos</asp:ListItem>
                                <asp:ListItem Value="B">Bajas</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td class="textoright center colorMedio textobold">
                            <asp:Label ID="Label28" runat="server" Text="Filtros:" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                            <asp:DropDownList ID="ddlBuscar" runat="server" CssClass="center">
                                <asp:ListItem Selected="True" Value="0">Todos</asp:ListItem>
                                <asp:ListItem Value="1">Referencia</asp:ListItem>
                                <asp:ListItem Value="2">Cliente</asp:ListItem>
                                <asp:ListItem Value="3">Tarjeta Habiente</asp:ListItem>
                            </asp:DropDownList>
                            &nbsp;
                            <asp:TextBox ID="txtBuscar" runat="server" OnTextChanged="txtBuscar_TextChanged"
                                CssClass="textboxlog ancho100px center"></asp:TextBox>
                            <cc1:TextBoxWatermarkExtender ID="txtBuscar_TextBoxWatermarkExtender" runat="server"
                                BehaviorID="txtBuscar_TextBoxWatermarkExtender" TargetControlID="txtBuscar" WatermarkCssClass="water ancho100px textboxlog"
                                WatermarkText="Buscar..." />
                            <asp:ImageButton ID="btnBuscar" runat="server" AlternateText="Buscar" ImageUrl="~/IMG/buscar.png"
                                ToolTip="Buscar" CssClass="center iconitos40" OnClick="btnBuscar_Click" />
                            <asp:LinkButton ID="lnkLimpiar" runat="server" Text="Limpiar Búsqueda" CssClass="linkLimpia colorAzul"
                                OnClick="lnkLimpiar_Click"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <table class="tablaheadgrids textocentradoh textobold colorMedio ancho1040px">
                    <tr>
                        <td class="ancho100px">
                            <asp:Label ID="Label7" runat="server" Text="Referencia"></asp:Label>
                        </td>
                        <td class="ancho230px">
                            <asp:Label ID="Label8" runat="server" Text="Tarjeta Habiente"></asp:Label>
                        </td>
                        <td class="ancho230px">
                            <asp:Label ID="Label9" runat="server" Text="Cliente"></asp:Label>
                        </td>
                        <td class="ancho100px">
                            <asp:Label ID="Label10" runat="server" Text="Fecha"></asp:Label>
                        </td>
                        <td class="ancho230px">
                            <asp:Label ID="Label29" runat="server" Text="Motivo"></asp:Label>
                        </td>
                        <td class="ancho100px">
                            <asp:Label ID="lblusuario" runat="server" Text="Usuario Registro"></asp:Label>
                        </td>
                        <td class="iconitos40"></td>
                    </tr>
                </table>
                <asp:Panel ID="Panel5" runat="server" CssClass="altoConsultas ancho1040px inline sombra"
                    ScrollBars="Auto">
                    <asp:GridView ID="grdClientes" runat="server" AutoGenerateColumns="False" ShowHeader="false"
                        DataKeyNames="id_cliente" DataSourceID="SqlDataSource1" CssClass="colorNegro ancho1040px"
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="referencia" HeaderText="Referencia" ItemStyle-CssClass="ancho100px textocentradoh"
                                SortExpression="referencia">
                                <ItemStyle CssClass="ancho100px textocentradoh" />
                            </asp:BoundField>
                            <asp:BoundField DataField="tarjetahabiente" HeaderText="Tarjeta Habiente" ItemStyle-CssClass="ancho230px"
                                ReadOnly="True" SortExpression="tarjetahabiente">
                                <ItemStyle CssClass="ancho230px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="cliente" HeaderText="Cliente" ReadOnly="True" ItemStyle-CssClass="ancho230px"
                                SortExpression="cliente">
                                <ItemStyle CssClass="ancho230px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="fecha" HeaderText="Fecha" ReadOnly="True" ItemStyle-CssClass="ancho100px textocentradoh"
                                SortExpression="fecha" DataFormatString="{0:d MMM yyyy}">
                                <ItemStyle CssClass="ancho100px textocentradoh" />
                            </asp:BoundField>
                            <asp:BoundField DataField="motivo_baja" HeaderText="Motivo" ItemStyle-CssClass="ancho230px"
                                ReadOnly="True" SortExpression="motivo_baja">
                                <ItemStyle CssClass="ancho230px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="usuario" HeaderText="Usuario" ItemStyle-CssClass="ancho100px"
                                ReadOnly="True" SortExpression="usuario">
                                <ItemStyle CssClass="ancho100px" />
                            </asp:BoundField>
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnSelecciona" runat="server" CausesValidation="False" CommandName="Select"
                                        ImageUrl="~/IMG/aceptar.png" CommandArgument='<%# Eval("referencia")+";"+Eval("id_cliente")  %>'
                                        ToolTip="Seleccionar" CssClass="iconitos40" OnClick="btnSelecciona_Click" />
                                </ItemTemplate>
                                <ItemStyle CssClass="iconitos40" />
                                <ControlStyle CssClass="iconitos40 textoIzquierda" />
                            </asp:TemplateField>
                        </Columns>
                        <SelectedRowStyle BackColor="LightGray" />
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>"
                        SelectCommand="select id_cliente,referencia,
ltrim(rtrim(nombre_tarjetahabiente))+' '+ltrim(rtrim(ap_pat_tarjetahabiente))+' '+ltrim(rtrim(isnull(ap_mat_tarjetahabiente,''))) as tarjetahabiente,
ltrim(rtrim(nombre))+' '+ltrim(rtrim(apellido_paterno))+' '+ltrim(rtrim(isnull(apellido_materno,''))) as cliente,
fecha_inicio as fecha, '' as motivo_baja, usuario_alta as usuario  from clientes where id_empresa=@empresa and estatus_cliente='A' order by referencia">
                        <SelectParameters>
                            <asp:QueryStringParameter Name="empresa" QueryStringField="e" DefaultValue="0" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                </asp:Panel>
                <asp:Panel ID="Panel6" runat="server" CssClass="padingLeft10px altoConsultas ancho200px inline sombra"
                    ScrollBars="Auto">
                    <asp:Label ID="Label1" runat="server" ForeColor="#002c81" Text="Referencia: "></asp:Label><asp:Label
                        ID="lblReferencia" runat="server" ForeColor="#002c81"></asp:Label>
                    <asp:Label ID="lblIdCliente" runat="server" Visible="false"></asp:Label>
                    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
                    <telerik:RadAsyncUpload RenderMode="Lightweight" runat="server" CssClass="async-attachment" MultipleFileSelection="Automatic" Culture="es-Mx"
                        ID="AsyncUpload1" HideFileInput="true" MaxFileInputsCount="10" AllowedFileExtensions=".jpeg,.jpg,.png,.gif,.bmp,.tiff,.JPEG,.JPG,.PNG,.GIF,.BMP,.TIFF,.PDF,.pdf">
                    </telerik:RadAsyncUpload>
                    <asp:ImageButton ID="btnAgregarImagen" runat="server" ToolTip="Agregar" CssClass="iconitos40"
                        ImageUrl="~/IMG/agregar.png" OnClick="btnAgregarImagen_Click" />
                    <asp:DataList ID="DataListFotosDanos" runat="server" RepeatColumns="2" RepeatDirection="Horizontal"
                        DataKeyField="id_documento" OnItemCommand="DataListFotosDanos_ItemCommand" OnItemDataBound="DataListFotosDanos_ItemDataBound">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnLogo" runat="server" CommandName="zoom" CommandArgument='<%# Eval("id_documento")+";"+Eval("referencia")+";"+Eval("id_cliente")+";"+Eval("extension")+";"+Eval("identificador_proceso") %>'>
                                <asp:Image ID="Image1" runat="server" Width="100px" ImageUrl='<%# "~/ImgDoctos.ashx?id="+Eval("id_documento")+";"+Eval("referencia")+";"+Eval("id_empresa")+";"+Eval("id_cliente") %>' />
                            </asp:LinkButton>
                            <asp:ImageButton ID="btnEliminaFotoDanos" runat="server" CommandName="elimina" ToolTip="Eliminar"
                                CommandArgument='<%# Eval("id_documento")+";"+Eval("referencia")+";"+Eval("id_cliente")+";"+Eval("identificador_proceso") %>'
                                OnClientClick="return confirm('¿Esta seguro de eliminar la fotografía?');" CssClass="iconitos40"
                                ImageUrl="~/IMG/eliminar.png" />
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho180px textoCentrado" />
                    </asp:DataList>
                    <br /><br />
                <div class="textoright ancho100">
                    <asp:Label ID="Label12" runat="server" Text="Alta" ForeColor="Green" CssClass="textobold"></asp:Label>&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label13" runat="server" Text="Baja" ForeColor="Red" CssClass="textobold"></asp:Label>&nbsp;&nbsp;&nbsp;
                    <asp:Label ID="Label14" runat="server" Text="Inactivo" ForeColor="Orange" CssClass="textobold"></asp:Label>
                    </div>
                </asp:Panel>

                
                    <br />

                <asp:Panel ID="PanelMascara" runat="server" CssClass="mask" />
                <asp:Panel ID="PanelImgZoom" runat="server" Style="position: fixed; left: 2px; top: 2px; z-index: 2; background-color: White; margin: 0 auto; text-align: center; width: 100%;"
                    CssClass="redondeoPopPanel" ScrollBars="Auto">
                    <table class="ancho100">
                        <tr class="tablaheadgrids textocentradoh textobold colorMedio">
                            <td class="ancho95 textocentradoh ">
                                <asp:Label ID="Label11" runat="server" Text="Documento" CssClass="t22 colorMorado textoBold" />
                                <asp:ImageButton ID="btnCerrarImgZomm" runat="server" ToolTip="Cerrar" OnClick="btnCerrarImgZomm_Click"
                                    ImageUrl="~/IMG/close.png" CssClass="iconitos40" />
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
            </asp:Panel>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                <ProgressTemplate>
                    <div class="ontopDiv">
                        <asp:Panel ID="PanGif" runat="server" CssClass="zen4 anchoCompleto center textocentradoh padingTop200px">
                            <asp:Image ID="gif" runat="server" ImageUrl="~/IMG/25-1.gif" AlternateText="Cargando. . ." Width="40%" />
                        </asp:Panel>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
