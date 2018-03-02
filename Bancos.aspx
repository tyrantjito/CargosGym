<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMenus.master" AutoEventWireup="true" CodeFile="Bancos.aspx.cs" Inherits="Bancos" %>


<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
<h2 class="anchoCompleto">Bancos</h2> 
<div class="ancho100 centardo textocentradoh">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
    <asp:Label ID="lblIdBanco" runat="server" Visible="false"></asp:Label>
        <asp:Panel ID="Panel1" runat="server" CssClass="centardo sombra ancho440px" >
            <table class="tablaheadgrids textocentradoh textobold">
                <tr>                
                    <td class="ancho180px"><asp:Label ID="Label2" runat="server" Text="Nombre"></asp:Label></td>
                    <td class="ancho180px"><asp:Label ID="Label3" runat="server" Text="Clave"></asp:Label></td>
                    <td class="iconitos40">&nbsp;</td>
                    <td class="iconitos40">&nbsp;</td>
                </tr>
            </table>
            <asp:Panel ID="Panel2" runat="server" CssClass="altoConsultas" ScrollBars="Auto">
                <asp:GridView ID="GridView1" runat="server" ShowHeader="False"
                        DataKeyNames="id_banco"
                        EmptyDataText="No hay registros de bancos" 
                        AutoGenerateColumns="False" 
                        GridLines="None" 
                        EmptyDataRowStyle-ForeColor="Red" onrowcommand="GridView1_RowCommand" >            
                    <Columns>
                        <asp:TemplateField Visible="false">
                            
                            <ItemTemplate>
                                <asp:Label ID="lblId" runat="server" Text='<%# Eval("id_banco") %>'></asp:Label>
                            </ItemTemplate>                        
                        </asp:TemplateField>
                        <asp:TemplateField>
                            
                            <ItemTemplate>
                                <asp:Label ID="lblnombre" runat="server" Text='<%# Eval("nombre") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho180px colorNegro"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            
                            <ItemTemplate>
                                <asp:Label ID="lblClave" runat="server" Text='<%# Eval("clave") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho180px colorNegro"></ItemStyle>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="lknEliminar" runat="server" 
                                    CommandArgument='<%# Eval("id_banco") %>'  CssClass="iconitos40" AlternateText="Eliminar" ToolTip="Eliminar"
                                    ImageUrl="~/IMG/eliminar.png"  OnClick="lknEliminar_Click" OnClientClick="return confirm('¿Está seguro de eliminar el Banco?')"/>
                            </ItemTemplate>
                            <ItemStyle CssClass="iconitos40" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="btnEditar" CommandArgument='<%# Eval("id_banco")+";"+Eval("nombre")+";"+Eval("clave") %>' 
                                    CommandName="Modificar" ImageUrl="~/IMG/edit.png" CssClass="iconitos40" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataRowStyle ForeColor="Red" />
                </asp:GridView>
            </asp:Panel>
             <table class="tablaFootergrids ancho440px">
                <tr>
                    <td class="ancho180px">
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="textboxlog center ancho100px"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtNombre_TextBoxWatermarkExtender" 
                            runat="server" BehaviorID="txtNombre_TextBoxWatermarkExtender" 
                            TargetControlID="txtNombre" WatermarkCssClass="water textboxlog ancho100px" WatermarkText="Nombre" />
                        <asp:RequiredFieldValidator
                            ID="RequiredFieldValidator1" runat="server" CssClass="center" ErrorMessage="Debe indicar el banco" ControlToValidate="txtNombre" ValidationGroup="crear" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                      </td>  
                    <td class="ancho180px">
                        <asp:TextBox ID="txtClave" runat="server" CssClass="textboxlog center ancho100px"></asp:TextBox>
                        <cc1:TextBoxWatermarkExtender ID="txtClave_TextBoxWatermarkExtender" 
                            runat="server" BehaviorID="txtClave_TextBoxWatermarkExtender" 
                            TargetControlID="txtClave" WatermarkCssClass="water textboxlog ancho100px" WatermarkText="Clave" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" CssClass="center" runat="server" ErrorMessage="Debe indicar la clave de banco" ControlToValidate="txtClave" ValidationGroup="crear" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <td class="ancho80px" colspan="2">
                        <asp:ImageButton ID="btnAgregar" runat="server" ImageUrl="~/IMG/aceptar.png" OnClick="btnAgregar_Click" ValidationGroup="crear" CssClass="iconitos40" AlternateText="Agregar" ToolTip="Agregar" />
                    </td>                                
                </tr>
            </table>
        </asp:Panel>
        <br />
        <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="crear" runat="server" ForeColor="Red" DisplayMode="List" />
        
        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>

        <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
            <ProgressTemplate>
                <div class="ontopDiv">
                    <asp:Panel ID="PanGif2" runat="server" CssClass="zen4 anchoCompleto center textocentradoh padingTop200px">
                        <asp:Image ID="gif2" runat="server" ImageUrl="~/IMG/25-1.gif" AlternateText="Cargando. . ." Width="300px" />
                    </asp:Panel>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

    </ContentTemplate>
    </asp:UpdatePanel>
</div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" >
        <ContentTemplate>
            <asp:Panel ID="PanPopUpDiv" runat="server" Visible="false" CssClass="mask"></asp:Panel>
                <asp:Panel ID="PanPopUp" runat="server" CssClass="zen4 redondeoPopPanel ancho30 popup" Visible="false">
                    <table class="anchoCompleto">
                        <tr class="headerPanelPop tablaheadgrids">
                            <td class="textocentradoh pading8px headerPanelPop" colspan="3"><asp:Label ID="Label15" runat="server" Text="Modifica Banco" ForeColor="White" CssClass="textobold pading8px"/></td>
                        </tr>
                        <tr>
                            <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label1" runat="server" Text="Nombre:" CssClass="colorNegro" /></td>
                            <td class="textoIzquierda">
                                <asp:TextBox ID="txtNombreMod" runat="server" MaxLength="150" CssClass="textboxlog ancho180px" />
                            </td>
                            <td><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Necesita indicar el Nombre" ControlToValidate="txtNombreMod" ForeColor="Red" Text="*" ValidationGroup="PopMod" /></td>
                        </tr>
                        <tr>
                            <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label4" runat="server" Text="Clave:" CssClass="colorNegro" /></td>
                            <td class="textoIzquierda">
                               <asp:TextBox ID="txtClaveMod" runat="server" MaxLength="15" CssClass="textboxlog ancho180px" />
                            </td>
                            <td><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Necesita indicar la Clave" ControlToValidate="txtClaveMod" ForeColor="Red" Text="*" ValidationGroup="PopMod" /></td>
                        </tr>
                        <tr>
                            <td class="textocentradoh" colspan="3">
                                <asp:ValidationSummary ID="ValidationSummary3" runat="server" ValidationGroup="PopMod" ForeColor="Red" />
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

</asp:Content>

