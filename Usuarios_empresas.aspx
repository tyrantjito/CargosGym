<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMenus.master" AutoEventWireup="true" CodeFile="Usuarios_empresas.aspx.cs" Inherits="Usuarios_empresas" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2 class="anchoCompleto">Usuarios Empresas</h2> 
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
<div class="centardo textocentradoh">
    <asp:Panel ID="Panel1" runat="server" CssClass="centardo sombra ancho340px">
        <table class="tablaheadgrids textocentradoh textobold anchoCompleto">
            <tr>
                <td class="ancho150px"><asp:Label ID="Label1" runat="server" Text="Usuario"></asp:Label></td>
                <td class="ancho150px"><asp:Label ID="Label2" runat="server" Text="Empresa"></asp:Label></td>
                <td class="iconitos40">&nbsp;</td>                    
            </tr>
        </table>
        <asp:Panel ID="Panel2" runat="server" ScrollBars="Auto" CssClass="altoConsultas">
            <asp:GridView ID="GridView1" runat="server" ShowHeader="False" GridLines="None"
            EmptyDataText="No existe información" AutoGenerateColumns="False"
            EmptyDataRowStyle-ForeColor="Red" CssClass="textocentradoh">                
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("id_usuario") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho150px colorNegro"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>                            
                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("nombre") %>'></asp:Label>                            
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho150px colorNegro"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="lknEliminar" runat="server"  ImageUrl="~/IMG/eliminar.png" OnClick="lknEliminar_Click" CommandArgument='<%# Eval("id_usuario")+";"+Eval("id_empresa")%>' ToolTip="Eliminar" AlternateText="Eliminar" CssClass="iconitos40" OnClientClick="return confirm('¿Está seguro de eliminar el usuario?')"  />
                        </ItemTemplate>
                        <ItemStyle CssClass="iconitos40"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataRowStyle ForeColor="Red" />
            </asp:GridView>
        </asp:Panel>
        <table class="tablaFootergrids textocentradoh anchoCompleto">
           <tr>
               <td class="ancho150px">
                   <asp:DropDownList ID="ddlUsuario" runat="server" DataSourceID="SqlDataSource1" 
                                    DataTextField="id_usuario" DataValueField="id_usuario" 
                                AppendDataBoundItems="True" CssClass="center ancho150px">
                       <asp:ListItem Value="0">Seleccione Usuario</asp:ListItem>
                   </asp:DropDownList>
               </td>
               <td class="ancho150px">
                   <asp:DropDownList ID="ddlEmpresa" runat="server"  DataSourceID="SqlDataSource2" 
                                    DataTextField="nombre" DataValueField="id_empresa" 
                                AppendDataBoundItems="True" CssClass="center ancho150px">
                       <asp:ListItem Value="0">Seleccione empresa</asp:ListItem>
                   </asp:DropDownList>                   
               </td>
               <td class="iconitos40">
                   <asp:ImageButton ID="btnagregar" runat="server" ImageUrl="~/IMG/agregar.png" OnClick="btnagregar_Click"  ValidationGroup="crear" AlternateText="Agregar" ToolTip="Agregar" CssClass="iconitos40 center"/>
               </td>
           </tr>
        </table>
       
    </asp:Panel>
     <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                       ConnectionString="<%$ ConnectionStrings:eBills %>"
                       SelectCommand="select id_usuario from usuarios">
                   </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
                       ConnectionString="<%$ ConnectionStrings:eBills %>"
                       SelectCommand="select id_empresa, nombre from empresas ">
                   </asp:SqlDataSource>
                  
    
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






