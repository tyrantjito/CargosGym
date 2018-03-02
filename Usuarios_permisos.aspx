<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMenus.master" AutoEventWireup="true" CodeFile="Usuarios_permisos.aspx.cs" Inherits="Usuarios_permisos" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2 class="anchoCompleto">Usuarios Permisos</h2> 
        <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div class="ancho100 centardo tdgrids">
        <asp:Panel ID="Panel5" runat="server" CssClass="ancho744px textobold centardo textocentradoh">
            <asp:Label ID="Label2" runat="server" Text="Usuarios: " CssClass="center" ForeColor="#002c81"></asp:Label>
         <asp:DropDownList ID="ddlUsuario" runat="server" AppendDataBoundItems="True" 
            DataSourceID="SqlDataSource1" DataTextField="id_usuario" CssClass="center"
            DataValueField="id_usuario" AutoPostBack="True" 
                onselectedindexchanged="ddlUsuario_SelectedIndexChanged">
            <asp:ListItem Value="0">Seleccione un Usuario</asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="lblError" runat="server" ForeColor="Red" CssClass="centardo"></asp:Label>
         </asp:Panel> 
         <br />
   <table class="ancho744px centardo">
   <tr>
   <td>
    <asp:Panel ID="Panel1" runat="server" CssClass=" pangrids centardo ancho250px sombra" >  
        <table class="tablaheadgrids textocentradoh ancho250px textobold">
            <tr>
                <td><asp:Label ID="Label1" runat="server" Text="Permisos"></asp:Label> </td>
            </tr>
         </table>
        <asp:Panel ID="Panel2" runat="server" CssClass="ancho230px centardo altoConsultas">
            <asp:ListBox ID="ListBox1" runat="server" DataKeys="id_permiso" CssClass="anchoCompleto altoConsultas" DataValueField="id_permiso" DataTextField="nombre" SelectionMode="Multiple">             
            </asp:ListBox>
        </asp:Panel>
    </asp:Panel>
        
    </td>

    <td class=" ancho10 centardo textocentradoh textobold">
        <asp:ImageButton ID="btnAgregar" runat="server" ToolTip="Agregar Permiso" CssClass="iconitos40 opciones" AlternateText=">" onclick="btnAgregar_Click"/><br />
        <asp:ImageButton ID="btnAgregartodos" runat="server" CssClass="iconitos40 opciones" ToolTip="Agregar Todos" AlternateText=">>" onclick="btnAgregartodos_Click"/><br />
        <asp:ImageButton ID="btnBorrartodos" runat="server" ToolTip="Borrar Todos" AlternateText="<<" onclick="btnBorrartodos_Click"  CssClass="iconitos40 opciones"/><br />
        <asp:ImageButton ID="btnBorrar" runat="server" ToolTip="Quitar Permiso" onclick="btnBorrar_Click" CssClass="iconitos40 opciones" AlternateText="<"/>
    </td>

    <td>
    
    <asp:Panel ID="Panel3" runat="server" CssClass=" pangrids centardo ancho250px sombra">
        <table class="tablaheadgrids textocentradoh textobold anchoCompleto">
            <tr>
                <td><asp:Label ID="Label3" runat="server" Text="Usuarios Permisos"></asp:Label></td>
            </tr>
        </table>

            <asp:Panel ID="Panel4" runat="server" CssClass="ancho230px centardo altoConsultas">
                <asp:ListBox ID="ListBox2" runat="server" DataValueField="id_permiso" DataTextField="nombre" SelectionMode="Multiple" CssClass="anchoCompleto altoConsultas">
                </asp:ListBox>                 
            </asp:Panel>
        </asp:Panel>
        </td>
        <td>
            <asp:ImageButton ID="btnAplicar" runat="server" CssClass="iconitos40" 
                AlternateText="Aplicar" ToolTip="Aplicar" ImageUrl="~/IMG/aceptar.png" 
                onclick="btnAplicar_Click" />
        </td>
       </tr>
        </table>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"  ConnectionString="<%$ ConnectionStrings:eBills %>" 
               SelectCommand="Select id_usuario from usuarios where id_usuario<>'Supervisor'"></asp:SqlDataSource>
        
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

