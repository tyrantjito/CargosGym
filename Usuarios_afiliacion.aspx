﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMenus.master" AutoEventWireup="true" CodeFile="Usuarios_afiliacion.aspx.cs" Inherits="Usuarios_afiliacion" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2 class="anchoCompleto">Usuarios Afiliaci&oacute;n</h2> 
  <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
<div class="ancho100 centardo tdgrids textocentradoh">
    <asp:Panel ID="Panel1" runat="server" CssClass="centardo sombra ancho340px">
    
         
        <table class="tablaheadgrids textobold textocentradoh">
            <tr>
                <td class="ancho150px"><asp:Label ID="Label1" runat="server" Text="Usuario"></asp:Label></td>
                <td class="ancho150px"><asp:Label ID="Label2" runat="server" Text=" Afiliación"></asp:Label></td>
                <td class="iconitos40">&nbsp;</td>                    
            </tr>
        </table>
        <asp:Panel ID="Panel2" runat="server" CssClass="altoConsultas" ScrollBars="Auto">
            <asp:GridView ID="GridView1" runat="server" ShowHeader="False"
            EmptyDataText="No existe Información" AutoGenerateColumns="False" CssClass="colorNegro textocentradoh"
            EmptyDataRowStyle-ForeColor="Red" GridLines="None" >                
                <Columns>
                    <asp:TemplateField>
                             <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("id_usuario") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho150px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>                            
                            <asp:Label ID="Label4" runat="server" Text='<%# Eval("no_afiliacion") %>'></asp:Label>                            
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho150px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="lknEliminar" runat="server" CssClass="iconitos40"
                                ImageUrl="~/IMG/eliminar.png" OnClick="lknEliminar_Click" ToolTip="Borrar" AlternateText="Borrar" OnClientClick="return confirm('¿Esta seguro de borrar este registro?');"
                                CommandArgument='<%# Eval("id_usuario")+";"+Eval("id_afiliacion")%>'/>
                        </ItemTemplate>
                        <ItemStyle CssClass="iconitos40"></ItemStyle>
                    </asp:TemplateField>
                </Columns>
                <EmptyDataRowStyle ForeColor="Red" />
            </asp:GridView>
        </asp:Panel>
        <table class="tablaFootergrids textocentradoh">
       <tr>
       <td class="ancho150px">
           <asp:DropDownList ID="ddlUsuario" runat="server" DataSourceID="SqlDataSource1" 
                            DataTextField="id_usuario" DataValueField="id_usuario" 
                        AppendDataBoundItems="True">
               <asp:ListItem Value="0">Seleccione Usuario</asp:ListItem>
           </asp:DropDownList>
       </td>
       <td class="ancho150px">
           <asp:DropDownList ID="ddlAfi" runat="server"  DataSourceID="SqlDataSource2" 
                            DataTextField="no_afiliacion" DataValueField="id_afiliacion" 
                        AppendDataBoundItems="True">
               <asp:ListItem Value="0">Seleccione afiliacion</asp:ListItem>
           </asp:DropDownList>
       </td>
       <td class="iconitos40">
           <asp:ImageButton ID="btnagregar" runat="server"  CssClass="iconitos40"
               ImageUrl="~/IMG/aceptar.png" OnClick="btnagregar_Click"  ValidationGroup="crear" ToolTip="Agregar" AlternateText="Agregar"/>
       </td>
       </tr>
        </table>
         
    </asp:Panel>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
               ConnectionString="<%$ ConnectionStrings:eBills %>" 
               SelectCommand="select id_usuario from usuarios">
           </asp:SqlDataSource>
           <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
               ConnectionString="<%$ ConnectionStrings:eBills %>"
               SelectCommand="select id_afiliacion, no_afiliacion from afiliaciones ">
           </asp:SqlDataSource>
    <asp:Label ID="lblError" runat="server" Text="" ForeColor="Red"></asp:Label>
    
   
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

