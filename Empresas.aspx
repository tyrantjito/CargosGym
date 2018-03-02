<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Empresas.aspx.cs" MasterPageFile="~/MasterMenus.master" Inherits="Empresas" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="anchoCompleto">Empresas</h2>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
            <div class="anchoCompleto centardo textocentradoh">
                <asp:Panel ID="Panel1" runat="server" CssClass="centardo ancho530px sombra">
                    <table class="tablaheadgrids textocentradoh textobold">
                        <tr>                
                            <td class="ancho250px"><asp:Label ID="Label2" runat="server" Text="Nombre"></asp:Label></td>
                            <td class="ancho150px"><asp:Label ID="Label3" runat="server" Text="Logo"></asp:Label></td>
                            <td class="ancho100px"><asp:Label ID="Label4" runat="server" Text="Monto"></asp:Label></td>    
                            <td class="iconitos40">&nbsp;</td>
                            <td class="iconitos40">&nbsp;</td>
                        </tr>
                    </table>
                    <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" CssClass="altoConsultas" >
                        <asp:GridView ID="GridView1" runat="server" ShowHeader="False"
                        EmptyDataText="No existen empresas registradas" AutoGenerateColumns="False"
                        EmptyDataRowStyle-ForeColor="Red" DataKeyNames="id_empresa" 
                            onrowediting="GridView1_RowEditing" 
                            onrowcancelingedit="GridView1_RowCancelingEdit" GridLines="None"
                            onrowupdating="GridView1_RowUpdating" CssClass="colorNegro textocentradoh" 
                            onrowcommand="GridView1_RowCommand" >                
                            <Columns>
                                <asp:TemplateField Visible="false">
                                    <EditItemTemplate>
                                        <asp:Label ID="Label6" runat="server" Text='<%# Eval("id_empresa") %>'></asp:Label>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label5" runat="server" Text='<%# Eval("id_empresa") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>                   
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtModnom" runat="server" Text='<%# Eval("nombre") %>' MaxLength="200"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                                            ControlToValidate="txtModnom" ErrorMessage="Ingrese el nombre de la empresa" ForeColor="Red" 
                                            ValidationGroup="editar" Text="*"></asp:RequiredFieldValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label7" runat="server" Text='<%# Eval("nombre") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="ancho250px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:Image ID="Image2" runat="server" ImageUrl='<%# Eval("id_empresa","ImgEmpresas.ashx?id={0}") %>' CssClass="imagenesEmpresasGrid"  />
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" 
                                            ControlToValidate="FileUpload1" ErrorMessage="Solo puede seleccionar imágenes jpg, jpeg, png, gif, tiff y bmp" 
                                            ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))(.png|.PNG|.jpg|.JPG|.jpeg|.JPEG|.tiff|.TIFF|.bmp|.BMP|.gif|.GIF)$" 
                                            ValidationGroup="editar" Text="*" ForeColor="Red"></asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("id_empresa", "ImgEmpresas.ashx?id={0}") %>' CssClass="imagenesEmpresasGrid"/>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="ancho150px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txtMensualidadMod" runat="server" Text='<%# Eval("mensualidad") %>' MaxLength="13" CssClass="textboxlog center ancho69px"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" CssClass="center"
                                            ControlToValidate="txtMensualidadMod" ErrorMessage="Debe indicar el Monto" ForeColor="Red"
                                            Text="*" ValidationGroup="editar"></asp:RequiredFieldValidator>
                                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtMensualidadMod"
                                            ErrorMessage="Debe indicar una cantidad válida" ForeColor="Red" Text="*" CssClass="center"
                                            ValidationExpression="\d*[0-9]{1,10}\.?\d[0-9]{0,2}|\d*[0-9]{1,10}" ValidationGroup="editar"></asp:RegularExpressionValidator>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Label71" runat="server" Text='<%# Eval("mensualidad") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle CssClass="ancho100px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="lknEliminar" runat="server" ImageUrl="~/IMG/eliminar.png" CommandName="Eliminar"
                                            CommandArgument='<%# Eval("id_empresa")%>' CssClass="iconitos40" ToolTip="Eliminar" 
                                            OnClientClick="return confirm('¿Está seguro de eliminar la Empresa?, tome en cuenta que todo su historial y registro se eliminará permanentemente')" />
                                    </ItemTemplate>
                                    <ItemStyle CssClass="iconitos40"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="btnEditar" runat="server" ImageUrl="~/IMG/edit.png" CommandArgument='<%# Eval("id_empresa")+";"+Eval("nombre")+";"+Eval("mensualidad")%>' CssClass="iconitos40" ToolTip="Modificar" CommandName="Modificar" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataRowStyle ForeColor="Red" />
                        </asp:GridView>
                    </asp:Panel>
                    <table class="tablaFootergrids textocentradoh anchoCompleto ancho530px">
                        <tr>    
                            <td class="ancho250px">
                                <asp:TextBox ID="txtNombre" runat="server" MaxLength="200" CssClass="textboxlog center ancho180px"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtNombre_TextBoxWatermarkExtender" 
                                    runat="server" BehaviorID="txtNombre_TextBoxWatermarkExtender" 
                                    TargetControlID="txtNombre" WatermarkCssClass="water textboxlog ancho180px" WatermarkText="Nombre" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" CssClass="center"
                                    ControlToValidate="txtNombre" ErrorMessage="Ingrese el nombre de la empresa" ForeColor="Red" 
                                    ValidationGroup="up"></asp:RequiredFieldValidator>
                            </td>
                            <td class="ancho150px">
                                <asp:FileUpload ID="FileUpload2" runat="server" CssClass="center ancho150px"  />                        
                            </td>
                            <td class="ancho100px">
                                <asp:TextBox ID="txtMensualidad" runat="server" MaxLength="13" CssClass="textboxlog center ancho69px"></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="txtMensualidadWatermarkExtender1" 
                                    runat="server" BehaviorID="txtMensualidad_TextBoxWatermarkExtender" 
                                    TargetControlID="txtMensualidad" WatermarkCssClass="water textboxlog ancho69px" WatermarkText="Monto" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidatortxtMensualidad" runat="server" CssClass="center"
                                    ControlToValidate="txtMensualidad" ErrorMessage="Debe indicar el Monto" ForeColor="Red"
                                    Text="*" ValidationGroup="up"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidatortxtMensualidad" runat="server" ControlToValidate="txtMensualidad"
                                    ErrorMessage="Debe indicar una cantidad válida" ForeColor="Red" Text="*" CssClass="center"
                                    ValidationExpression="\d*[0-9]{1,10}\.?\d[0-9]{0,2}|\d*[0-9]{1,10}" ValidationGroup="up"></asp:RegularExpressionValidator>
                            </td>   
                            <td class="ancho126px" colspan="2">
                                <asp:ImageButton ID="btnAgregar" runat="server" ValidationGroup="up" ImageUrl="~/IMG/agregar.png" onclick="btnAgregar_Click1" CssClass="iconitos40" ToolTip="Agregar" AlternateText="Agregar" />
                            </td>           
                        </tr>
                    </table>
                </asp:Panel>    
            </div>
            <br />
            <div class="textocentradoh pading8px">
                <asp:Label ID="lblError" runat="server" ForeColor="Red" />
                <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="up" runat="server" ForeColor="Red"  DisplayMode="List"/>
                <asp:ValidationSummary ID="ValidationSummary2" ValidationGroup="editar" runat="server"  ForeColor="Red" DisplayMode="List"/>
            </div>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnActualizarMod" />
        </Triggers>
        <ContentTemplate>
            <asp:Panel ID="PanPopUpDiv" runat="server" Visible="false" CssClass="mask"></asp:Panel>
                <asp:Panel ID="PanPopUp" runat="server" CssClass="zen4 redondeoPopPanel ancho30 popup" Visible="false">
                    <table class="anchoCompleto">
                        <tr class="headerPanelPop tablaheadgrids">
                            <td class="textocentradoh pading8px headerPanelPop" colspan="3"><asp:Label ID="Label15" runat="server" Text="Modifica Empresa" ForeColor="White" CssClass="textobold pading8px"/></td>
                        </tr> 
                        <tr>
                            <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label11" runat="server" Text="Nombre" CssClass="colorNegro" /></td>
                            <td class="textoIzquierda">
                                <asp:TextBox ID="txtNombreMod" runat="server" MaxLength="150" CssClass="textboxlog ancho180px" />
                            </td>
                            <td><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Debe ingresar el nombre" ControlToValidate="txtNombreMod" ForeColor="Red" Text="*" ValidationGroup="PopMod" /></td>
                        </tr>
                        <tr>
                             <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label14" runat="server" Text="Logo"  CssClass="colorNegro"/></td>
                             <td class="textoIzquierda"><asp:FileUpload ID="FileUploadPopup" runat="server" CssClass="ancho180px" /></td>
                             <td class="textoIzquierda"><asp:Label ID="lblErrorCargaLogoAsterisco" runat="server" Text="*" Visible="false" ForeColor="Red"  /></td>
                        </tr>
                        <tr>       
                            <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label1" runat="server" Text="Monto Mensualidad"  CssClass="colorNegro"/></td>
                            <td class="textoIzquierda"><asp:TextBox ID="txtMensualidadMod" runat="server" MaxLength="13" CssClass="textboxlog center ancho69px" ></asp:TextBox></td>
                            <td class="textoIzquierda">
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" CssClass="center"
                                            ControlToValidate="txtMensualidadMod" ErrorMessage="Debe indicar el Monto" ForeColor="Red"
                                            Text="*" ValidationGroup="editar"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ControlToValidate="txtMensualidadMod"
                                            ErrorMessage="Debe indicar una cantidad válida" ForeColor="Red" Text="*" CssClass="center"
                                            ValidationExpression="\d*[0-9]{1,10}\.?\d[0-9]{0,2}|\d*[0-9]{1,10}" ValidationGroup="editar"></asp:RegularExpressionValidator>

                            </td>
                        </tr>
                        <tr class="headerPanelPop tablaheadgrids">
                            <td class="textocentradoh pading8px headerPanelPop" colspan="3"><asp:Label ID="Label13" runat="server" Text="Parametros Correo" ForeColor="White" CssClass="textobold pading8px"/></td>
                        </tr>
                        <tr>       
                            <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label8" runat="server" Text="Usuario"  CssClass="colorNegro"/></td>
                            <td class="textoIzquierda"><asp:TextBox ID="txtUsuarioMod" runat="server" MaxLength="200" CssClass="textboxlog center ancho180px" ></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender1" runat="server" BehaviorID="txtUsuarioMod_TextBoxWatermarkExtender" TargetControlID="txtUsuarioMod" WatermarkCssClass="water textboxlog ancho180px" WatermarkText="Usuario" />
                            </td>                            
                        </tr>
                        <tr>       
                            <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label9" runat="server" Text="Contraseña"  CssClass="colorNegro"/></td>
                            <td class="textoIzquierda"><asp:TextBox ID="txtContraseñaMod" runat="server" MaxLength="30" CssClass="textboxlog center ancho180px" ></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender2" runat="server" BehaviorID="txtContraseñaMod_TextBoxWatermarkExtender" TargetControlID="txtContraseñaMod" WatermarkCssClass="water textboxlog ancho180px" WatermarkText="Contraseña" />
                            </td>                            
                        </tr>
                        <tr>       
                            <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label10" runat="server" Text="Servidor"  CssClass="colorNegro"/></td>
                            <td class="textoIzquierda"><asp:TextBox ID="txtHostMod" runat="server" MaxLength="50" CssClass="textboxlog center ancho180px" ></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender3" runat="server" BehaviorID="txtHostMod_TextBoxWatermarkExtender" TargetControlID="txtHostMod" WatermarkCssClass="water textboxlog ancho180px" WatermarkText="Servidor" />
                            </td>                            
                        </tr>
                        <tr>       
                            <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label12" runat="server" Text="Puerto"  CssClass="colorNegro"/></td>
                            <td class="textoIzquierda"><telerik:RadNumericTextBox ID="radPuerto" runat="server" MinValue="0" MaxValue="32500" ShowSpinButtons="true" NumberFormat-GroupSizes="4" NumberFormat-GroupSeparator=" "  EmptyMessage="Puerto" NumberFormat-DecimalDigits="0" Skin="MetroTouch"></telerik:RadNumericTextBox></td>
                        </tr>
                        <tr>       
                            <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label16" runat="server" Text="Tipo Servidor"  CssClass="colorNegro"/></td>
                            <td class="textoIzquierda">
                                <asp:DropDownList ID="ddlTipoServidor" runat="server" CssClass="textboxlog center ancho180px">
                                    <asp:ListItem Text="Seleccione tipo de Servidor" Value="0"></asp:ListItem>
                                    <asp:ListItem Text="SMTP" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="POP3" Value="2"></asp:ListItem>
                                    <asp:ListItem Text="IMAP" Value="3"></asp:ListItem>
                                </asp:DropDownList>
                            </td>                            
                        </tr>
                        <tr>       
                            <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label17" runat="server" Text="SSL"  CssClass="colorNegro"/></td>
                            <td class="textoIzquierda"><asp:CheckBox ID="chkSSlMod" runat="server" Text="" CssClass="textboxlog center ancho180px"/></td>
                        </tr>
                        <tr>       
                            <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label18" runat="server" Text="Mensaje Correo"  CssClass="colorNegro"/></td>
                            <td class="textoIzquierda"><asp:TextBox ID="txtMensajeCorreo" runat="server" MaxLength="500" CssClass="textboxlog center ancho180px" TextMode="MultiLine" Rows="10"  ></asp:TextBox>
                                <cc1:TextBoxWatermarkExtender ID="TextBoxWatermarkExtender4" runat="server" BehaviorID="txtMensajeCorreo_TextBoxWatermarkExtender" TargetControlID="txtMensajeCorreo" WatermarkCssClass="water textboxlog ancho180px" WatermarkText="Mensaje" />
                            </td>                            
                        </tr>
                        <tr>
                            <td class="textocentradoh" colspan="3">
                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="PopMod" ForeColor="Red" DisplayMode="List" />
                                <asp:Label ID="lblErrorMod" runat="server" ForeColor="Red"/>
                            </td>
                        </tr>
                        <tr class="piePanelPop tablaFootergrids">
                                <td class="textoright tablaFootergrids" colspan="3" >
                                    <asp:ImageButton ID="btnActualizarMod" runat="server" ValidationGroup="PopMod" 
                                        ImageUrl="~/IMG/aceptar.png" CssClass="iconitos40" onclick="btnActualizarMod_Click"/>&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:ImageButton ID="btnCancelarMod" runat="server" ValidationGroup="PopMod" 
                                        CssClass="iconitos40" ImageUrl="~/IMG/cancelar.png" OnClick="btnCancelarMod_Click" />
                                </td>
                        </tr>                                                    
                </table>
            </asp:Panel>        
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
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