<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMenus.master" AutoEventWireup="true" CodeFile="Afiliacion.aspx.cs" Inherits="Afiliacion" Culture="es-MX" UICulture="es-MX" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2 class="anchoCompleto">Afiliaciones</h2> 
<div class="ancho100 centardo tdgrids textocentradoh">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True">
</asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
     <ContentTemplate>
        <asp:Panel ID="Panel1" runat="server" CssClass="centardo ancho900px sombra" >       
        <table class="tablaheadgrids textobold textocentradoh">
            <tr>
                <td class="ancho180px"><asp:Label ID="Label1" runat="server" Text="No. Afiliación"></asp:Label></td>
                <td class="ancho180px"><asp:Label ID="Label2" runat="server" Text="Criterio"></asp:Label></td>
                <td class="ancho100px"><asp:Label ID="Label3" runat="server" Text="Dias"></asp:Label></td>
                <td class="ancho100px"><asp:Label ID="Label4" runat="server" Text="Fecha"></asp:Label></td>
                <td class="ancho180px"><asp:Label ID="Label5" runat="server" Text="Banco"></asp:Label></td>
                <td class="ancho80px"><asp:Label ID="Label6" runat="server" Text="Comisión"></asp:Label></td>
                <td class="iconitos40">&nbsp;</td>
                <td class="iconitos40">&nbsp;</td>
            </tr>
        </table>
      
        <asp:Panel ID="Panel2" runat="server" CssClass="altoConsultas" ScrollBars="Auto">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>                                
               <asp:GridView ID="GridView1" runat="server" ShowHeader="False"
                DataKeyNames="id_afiliacion" EmptyDataText="No existen afiliaciones registradas" 
                AutoGenerateColumns="False"  
                CssClass="textocentradoh colorNegro"
                OnRowDataBound="GridView1_RowDataBound"
                GridLines="None" EmptyDataRowStyle-ForeColor="Red">
                <Columns>
                    <asp:TemplateField>
                        
                        <ItemTemplate>
                            <asp:Label ID="lblNoafi" runat="server" Text='<%# Eval("no_afiliacion") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho180px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                       
                        <ItemTemplate>
                            <asp:Label ID="lblcriterio" runat="server" Text='<%# Eval("criterio") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho180px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        
                        <ItemTemplate>
                            <asp:Label ID="lblDias" runat="server" Text='<%# Eval("dias") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho100px"></ItemStyle>
                    </asp:TemplateField>

                    <asp:TemplateField>
                        
                        <ItemTemplate>
                            <asp:Label ID="lblFecha" runat="server" Text='<%# Eval("fecha","{0:d MMM yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle CssClass="ancho100px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        
                        <ItemTemplate>
                            <asp:Label ID="lblBanco" runat="server" Text='<%# Eval("nombre") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle CssClass="ancho180px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        
                        <ItemTemplate>
                            <asp:Label ID="lblComision" runat="server" Text='<%# Eval("comision") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle CssClass="ancho80px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="lknEliminar" runat="server" CssClass="iconitos40"
                                CommandArgument='<%# Eval("id_afiliacion") %>'  
                                ImageUrl="~/IMG/eliminar.png" OnClick="lknEliminar_Click" ToolTip="Eliminar" AlternateText="Eliminar" OnClientClick="return confirm('¿Esta seguro de borrar este registro?');"/>
                        </ItemTemplate>
                        <ItemStyle CssClass="iconitos40"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="btneditar" runat="server" OnClick="btneditar_Click"
                                CommandArgument='<%# Eval("id_afiliacion")+";"+ Eval("no_afiliacion")+";"+Eval("criterio")+";"+Eval("dias")+";"+Eval("fecha")+";"+ Eval("id_banco")+";"+ Eval("comision")%>' 
                                CssClass="iconitos40" ImageUrl="~/IMG/edit.png" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataRowStyle ForeColor="Red" />
            </asp:GridView>                           
            </ContentTemplate>
            </asp:UpdatePanel>            
                       
        </asp:Panel>                        
          <table class="tablaFootergrids textocentradoh ancho900px">
            <tr>
                <td class="ancho150px">
                    <asp:TextBox ID="txtNoafi" runat="server" CssClass="textboxlog ancho80px center" MaxLength="10"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtNoafi_TextBoxWatermarkExtender" 
                        runat="server" BehaviorID="txtNoafi_TextBoxWatermarkExtender" 
                        TargetControlID="txtNoafi" WatermarkCssClass="water textboxlog ancho80px" WatermarkText="Afiliación" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" CssClass="center" runat="server" ErrorMessage="Debe indicar el No. de afiliación" ControlToValidate="txtNoafi" ValidationGroup="crear" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>  
                <td class="ancho150px">
                    <asp:TextBox ID="txtCriterio" runat="server" CssClass="textboxlog ancho30px" MaxLength="3"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtCriterio_TextBoxWatermarkExtender" 
                        runat="server" BehaviorID="txtCriterio_TextBoxWatermarkExtender" 
                        TargetControlID="txtCriterio" WatermarkCssClass="water textboxlog ancho30px" WatermarkText="Criterio" />
                </td>
                <td class="ancho100px">
                    <asp:TextBox ID="txtDias" runat="server" CssClass="textboxlog ancho30px" MaxLength="2"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtDias_TextBoxWatermarkExtender" 
                        runat="server" BehaviorID="txtDias_TextBoxWatermarkExtender" 
                        TargetControlID="txtDias" WatermarkCssClass="water textboxlog ancho30px" WatermarkText="Días"/>
                  </td>  
                  <td class="ancho100px">
                    <asp:TextBox ID="fecha" runat="server" CssClass="textboxlog ancho80px center"  MaxLength="10" Enabled="True" ></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="fecha_TextBoxWatermarkExtender" 
                          runat="server" BehaviorID="fecha_TextBoxWatermarkExtender" 
                          TargetControlID="fecha" WatermarkCssClass="water textboxlog ancho80px" WatermarkText="aaaa-mm-dd" />
                     
                    <asp:RequiredFieldValidator
                        ID="RequiredFieldValidator7" runat="server" CssClass="center" ErrorMessage="Debe indicar la fecha" ControlToValidate="fecha" ValidationGroup="crear" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                      <cc1:CalendarExtender ID="CalendarExtender1" runat="server"  Format="yyyy-MM-dd" TargetControlID="fecha" />
                  </td>  
                  <td class="ancho180px">
                      <asp:DropDownList ID="ddlBank" runat="server" DataSourceID="SqlDataSource2" 
                          ValidationGroup="crear" DataTextField="nombre" DataValueField="id_banco"  CssClass="textboxlog"
                        AppendDataBoundItems="True">
                          <asp:ListItem Value="0">Seleccione el Banco</asp:ListItem>
                      </asp:DropDownList> 
                      <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" 
                            SelectCommand="select id_banco,nombre from bancos"></asp:SqlDataSource>
                  </td>  
                  <td class="ancho80px">
                    <asp:TextBox ID="txtComision" runat="server" MaxLength="5" CssClass="textboxlog ancho69px" ></asp:TextBox>
                      <cc1:TextBoxWatermarkExtender ID="txtComision_TextBoxWatermarkExtender" 
                          runat="server" BehaviorID="txtComision_TextBoxWatermarkExtender" 
                          TargetControlID="txtComision" WatermarkCssClass="water textboxlog ancho69px" WatermarkText="Comisión" />
                  </td>  
                <td class="ancho80px" colspan="2">
                    <asp:ImageButton ID="btnAgregar" runat="server" ImageUrl="~/IMG/aceptar.png" CssClass="iconitos40"
                        OnClick="btnAgregar_Click" ValidationGroup="crear" ToolTip="Agregar" AlternateText="Agregar" />
                </td>                                
            </tr>
        </table>  
        <asp:Panel ID="Panel4" runat="server" CssClass="mask" Visible="false">
        </asp:Panel>
        <asp:Panel ID="Panel3" runat="server" CssClass="centardo ancho340px popup zen4 rendondeoPopupPanel"  Visible="false">
          <table class="redondeoPopPanel">
            <tr class="headerPanelPop">
                <td class="ancho340px pading8px headerPanelPop textobold" style="background-color:#002c81;" colspan="2">
                    <asp:Label ID="Label7" runat="server" Text="Modifica Afiliación" CssClass="colorBlanco"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label8" runat="server" Text="No Afiliación : " CssClass="colorNegro"></asp:Label></td> 
                <td class="textoIzquierda">
                     <asp:Label ID="lblAfi" runat="server" CssClass="colorNegro" ></asp:Label>
                     <asp:Label ID="lblId" runat="server"  Visible="False"></asp:Label>
                </td>
             </tr>
             <tr>
                 <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label9" runat="server" Text="Criterio" CssClass="colorNegro"></asp:Label> </td>
                 <td class="textoIzquierda"><asp:TextBox ID="txtCri" runat="server" MaxLength="3" CssClass="ancho30px textboxlog"></asp:TextBox> </td>
            </tr>
            <tr>
                <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label10" runat="server" Text="Días" CssClass="colorNegro" ></asp:Label> </td>
                <td class="textoIzquierda"><asp:TextBox ID="txtDays" runat="server" MaxLength="2" CssClass="ancho30px textboxlog"></asp:TextBox> </td>
            </tr>
            <tr>
                <td class="textoIzquierda padingLeft30px">
                    <asp:Label ID="Label11" runat="server" Text="Fecha" CssClass="colorNegro"></asp:Label>
                </td>
                <td class="textoIzquierda">
                    <asp:TextBox ID="txtDate" runat="server" MaxLength="10" CssClass="ancho80px textboxlog center"></asp:TextBox>           
                    <cc1:CalendarExtender ID="CalendarExtender3" runat="server"  Format="yyyy-MM-dd" TargetControlID="txtDate"  />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="center" ErrorMessage="Debe indicar la fecha" ControlToValidate="txtDate" ValidationGroup="edit" Text="*" ForeColor="Red"></asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label12" runat="server" Text="Banco" CssClass="colorNegro"></asp:Label></td>
                <td class="textoIzquierda"><asp:DropDownList ID="ddlBanco" runat="server" AppendDataBoundItems="True" DataSourceID="SqlDataSource3" DataTextField="nombre" DataValueField="id_banco" CssClass="textboxlog">
                 <asp:ListItem Value="0" Selected="True">Seleccione Banco</asp:ListItem>
                </asp:DropDownList> 
                <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" 
                            SelectCommand="select id_banco,nombre from bancos"></asp:SqlDataSource>
            </td>
            </tr>
            <tr>
                <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label13" runat="server" Text="Comisión" CssClass="colorNegro"></asp:Label> </td>
                <td class="textoIzquierda"><asp:TextBox ID="txtComi" runat="server" MaxLength="5" CssClass="ancho69px textboxlog"></asp:TextBox> </td>
            </tr>
            <tr>
                <td colspan="3" class="textocentradoh">
                    <asp:ValidationSummary ID="ValidationSummary2" ValidationGroup="edit" runat="server" ForeColor="Red" DisplayMode="List"/>
                    <asp:Label ID="lblErrorMod" runat="server" align="center" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr class="piePanelPop">
                <td colspan="2" class="textoright piePanelPop" style="background-color:#002c81;">
                    <asp:ImageButton ID="btnActualizar" runat="server" ToolTip="Actualizar" ValidationGroup="editar" OnClick="btnActualizar_Click" ImageUrl="~/IMG/aceptar.png" CssClass="iconitos40" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="btnCancelar" runat="server" ToolTip="Cancelar" ImageUrl="~/IMG/cancelar.png" CssClass="iconitos40" OnClick="btnCancelar_Click" />
                </td>
            </tr>
          </table>          
        </asp:Panel>
    </asp:Panel>
    <br />
    <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="crear" runat="server" ForeColor="Red" DisplayMode="List" />    
    <asp:Label ID="lblError" runat="server" align="center" ForeColor="Red"></asp:Label>
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
</div>
</asp:Content>

