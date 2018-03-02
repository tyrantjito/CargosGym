<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMenus.master" AutoEventWireup="true" CodeFile="Usuarios.aspx.cs" Inherits="Usuarios" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2 class="anchoCompleto">Usuarios</h2>
<asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
       <ContentTemplate>  
    <div class="anchoCompleto centardo textocentradoh">
   
    <asp:Panel ID="Panel2" runat="server" CssClass="ancho770 centardo sombra" >  
           
            <table class="tablaheadgrids textobold textocentradoh">
                <tr>
                  <td class="ancho180px">
                    <asp:Label ID="Label1" runat="server" Text="Usuario" ForeColor="White"></asp:Label>
                  </td>
                  <td class="ancho180px">
                    <asp:Label ID="Label2" runat="server" Text="Contraseña" ForeColor="White"></asp:Label>
                  </td>
                  <td class="ancho180px">
                    <asp:Label ID="Label3" runat="server" Text="Nombre" ForeColor="White"></asp:Label>
                  </td>
                  <td class="ancho150px">
                    <asp:Label ID="Label4" runat="server" Text="Usuario Afiliación" ForeColor="White"></asp:Label>
                  </td>                
                  <td class="iconitos40">&nbsp;</td>
                  <td class="iconitos40">&nbsp;</td>
                </tr>
           </table>
          <asp:Panel ID="Panel1" runat="server" CssClass="altoConsultas" ScrollBars="Auto">    
        
            <asp:GridView ID="GridView1" runat="server" ShowHeader="False"
            DataKeyNames="id_usuario" CssClass="textocentradoh colorNegro"
                 EmptyDataText="No hay registros de usuarios" 
            AutoGenerateColumns="False" 
            onrowdatabound="GridView1_RowDataBound" GridLines="None" 
            EmptyDataRowStyle-ForeColor="Red"  >
                <Columns>
                <asp:TemplateField>
                   
                 <ItemTemplate>
                    <asp:Label ID="lblClave" runat="server"  Text='<%# Eval("id_usuario") %>'></asp:Label>
                 </ItemTemplate>
                <ItemStyle CssClass="ancho180px"></ItemStyle>
                </asp:TemplateField>                
                <asp:TemplateField>
                    
                 <ItemTemplate>
                <asp:Label ID="lblContraseña" runat="server"  Text='<%# Eval("contrasena") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="ancho180px"></ItemStyle>
                </asp:TemplateField>               
                <asp:TemplateField>
                   
                 <ItemTemplate>
                 <asp:Label ID="lblNombre" runat="server" Text='<%# Eval("nombre") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle CssClass="ancho180px"></ItemStyle>
                </asp:TemplateField>               
                <asp:TemplateField>
                    
                 <ItemTemplate>
                    <asp:CheckBox ID="CheckUsua" Enabled ="false" runat="server"  />
                 </ItemTemplate>
                <ItemStyle CssClass="ancho150px"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField >
                    <ItemTemplate>
                        <asp:ImageButton ID="lknEliminar" runat="server" ImageUrl="~/IMG/eliminar.png" OnClick="lknEliminar_Click" CommandArgument='<%# Eval("id_usuario")%>' CssClass="iconitos40" AlternateText="Eliminar" ToolTip="Eliminar" OnClientClick="return confirm('¿Está seguro de eliminar el Usuario?')" />
                    </ItemTemplate>
                    <ItemStyle CssClass="iconitos40"></ItemStyle>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:ImageButton ID="btneditar" runat="server" CssClass="iconitos40" OnClick="btneditar_Click" CommandArgument='<%# Eval("id_usuario")+";"+ Eval("contrasena")+";"+Eval("nombre")+";"+Eval("usuario_afiliacion")+";"+Eval("correo")%>' ImageUrl="~/IMG/edit.png" />
                             
                    </ItemTemplate>
                     <ItemStyle CssClass="iconitos40"></ItemStyle>
                </asp:TemplateField>
            </Columns>
            <EmptyDataRowStyle ForeColor="Red"></EmptyDataRowStyle>
        </asp:GridView>
       
        </asp:Panel>
        <table class="tablaFootergrids textocentradoh">
            <tr>
                <td class="ancho180px">
                    <asp:TextBox ID="txtIdu" runat="server" CssClass="textboxlog center ancho100px" MaxLength="20"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtIdu_TextBoxWatermarkExtender" 
                        runat="server" BehaviorID="txtIdu_TextBoxWatermarkExtender" 
                        TargetControlID="txtIdu" WatermarkCssClass="water ancho100px textboxlog" WatermarkText="Usuario"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Debe indicar el usuario" ControlToValidate="txtIdu" ValidationGroup="crear" Text="*" ForeColor="Red" CssClass="center"></asp:RequiredFieldValidator>
                  </td>  
                <td class="ancho180px">
                    <asp:TextBox ID="txtContraseña" runat="server"  CssClass="textboxlog ancho100px center" MaxLength="20" ></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtContraseña_TextBoxWatermarkExtender" 
                        runat="server" BehaviorID="txtContraseña_TextBoxWatermarkExtender" 
                        TargetControlID="txtContraseña" WatermarkCssClass="water ancho100px textboxlog" WatermarkText="Contaseña" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Debe indicar la contraseña" ControlToValidate="txtContraseña" ValidationGroup="crear" Text="*" ForeColor="Red" CssClass="center"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" Text="*" ForeColor="Red" ValidationGroup="crear" ErrorMessage="La contraseña debe contener como mínimo 5 y un máximo de 20 caracteres." ControlToValidate="txtContraseña" ValidationExpression="[a-zA-Z0-9]{5,20}" />
                </td>
                <td class="ancho180px">
                    <asp:TextBox ID="txtNombre" runat="server"  CssClass="textboxlog ancho100px center" MaxLength="150"></asp:TextBox>
                    <cc1:TextBoxWatermarkExtender ID="txtNombre_TextBoxWatermarkExtender" 
                        runat="server" BehaviorID="txtNombre_TextBoxWatermarkExtender" 
                        TargetControlID="txtNombre" WatermarkCssClass="water ancho100px textboxlog" WatermarkText="Nombre"/>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Debe indicar el nombre" ControlToValidate="txtNombre" ValidationGroup="crear" Text="*" ForeColor="Red" CssClass="center"></asp:RequiredFieldValidator>
                </td>
                <td class="ancho150px">
                    <asp:CheckBox ID="CheckBox1" runat="server" />
                </td>
                <td class="ancho80px textocentradoh" colspan="2">
                    <asp:ImageButton ID="btnAgregar" runat="server" ImageUrl="~/IMG/aceptar.png" OnClick="btnAgregar_Click" ValidationGroup="crear" CssClass="iconitos40 center" AlternateText="Agregar" ToolTip="Agregar" />
                </td>                                
            </tr>
        </table>        
        
        <asp:Panel ID="Panel4" runat="server" CssClass="mask" Visible="false"></asp:Panel>
        <asp:Panel ID="Panel3" runat="server" CssClass="centardo ancho340px popup rendondeoPopupPanel"  Visible="false">
         <table class="anchoCompleto">
            <tr class="tablaheadgrids" >
                <td class="pading8px textobold" colspan="3">
                    <asp:Label ID="Label6" runat="server" Text="Modifica Usuario" ForeColor="White"></asp:Label>
                </td>
           </tr>        
            <tr>
                <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label5" runat="server" Text="Usuario:" CssClass="colorNegro"></asp:Label></td>   
                <td class="textoIzquierda" colspan="2"><asp:Label ID="lblUser" runat="server" CssClass="colorNegro"></asp:Label></td>        
            </tr>
            <tr>
                <td class="textoIzquierda padingLeft30px"><asp:Label ID="lblPass" runat="server" Text="Contraseña: " CssClass="colorNegro"></asp:Label></td>
                <td class="textoIzquierda"><asp:TextBox ID="txtPass" runat="server" MaxLength="20" CssClass="textboxlog ancho100px"></asp:TextBox></td>
                <td class="textoIzquierda">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPass" ErrorMessage="Debe indicar la Contraseña" ForeColor="Red" ValidationGroup="editar" Text="*"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Text="*" ForeColor="Red" ValidationGroup="editar" ErrorMessage="La contraseña debe contener como mínimo 5 y un máximo de 20 caracteres." ControlToValidate="txtPass" ValidationExpression="[a-zA-Z0-9]{5,20}" />                    
                </td>
            </tr>
            <tr>
                <td class="textoIzquierda padingLeft30px"><asp:Label ID="lblName" runat="server" Text="Nombre: " CssClass="colorNegro"></asp:Label></td>    
                <td class="textoIzquierda"><asp:TextBox ID="txtNombrea" runat="server" MaxLength="150" CssClass="textboxlog ancho100px"></asp:TextBox></td>
                <td class="textoIzquierda"><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtNombrea" ErrorMessage="Debe indicar el Nombre" ForeColor="Red" ValidationGroup="editar" Text="*"></asp:RequiredFieldValidator></td>
            </tr>
            <tr>
                <td class="textoIzquierda padingLeft30px"><asp:Label ID="lblAfi" runat="server" Text="Usuario Afiliación: " CssClass="colorNegro"></asp:Label></td>
                <td class="textoIzquierda" colspan="2"><asp:CheckBox ID="chkUsua" runat="server" /></td>
            </tr>            
            <tr>
                <td class="textoIzquierda padingLeft30px"><asp:Label ID="Label9" runat="server" Text="Correo: " CssClass="colorNegro"></asp:Label></td>
                <td class="textoIzquierda" colspan="2"><asp:TextBox ID="txtCorreoMod" runat="server" MaxLength="500" CssClass="textboxlog ancho100px"></asp:TextBox></td>                
            </tr>
            <tr>
                <td class="textocentradoh" colspan="3">
                    <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="editar" DisplayMode="List" ForeColor="Red" align="center"/>
                     <asp:Label ID="lblErrorMod" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr class="tablaFootergrids textocentradoh">
                <td colspan="3" class="textoright">
                    <asp:ImageButton ID="btnActualizar" runat="server" ToolTip="Actualizar" ValidationGroup="editar" OnClick="btnActualizar_Click" ImageUrl="~/IMG/aceptar.png" CssClass="iconitos40" />&nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:ImageButton ID="btnCancelar" runat="server" ToolTip="Cancelar" ImageUrl="~/IMG/cancelar.png" CssClass="iconitos40" OnClick="btnCancelar_Click" />
                </td>
            </tr>
        </table>        
            
        </asp:Panel>
          
            
        </asp:Panel> 
        <br /> 
        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="crear" DisplayMode="List" ForeColor="Red" align="center"/>   
        <asp:Label ID="lblError" runat="server"  ForeColor="Red"></asp:Label>
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
</asp:Content>

