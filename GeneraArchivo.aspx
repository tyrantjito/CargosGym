<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GeneraArchivo.aspx.cs" Inherits="GeneraArchivo" MasterPageFile="~/MasterMenus.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="anchoCompleto">Generación Archivo Cobranza</h2> 
     <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <div class="anchoCompleto">
         <table class="centardo ancho50 textocentradoh">
                <tr>
                    <td class=" ancho30 textoright center colorObscuro textobold">
                        <asp:Label ID="Label6" runat="server" Text="Periodo a Generar:" ForeColor="#002c81"></asp:Label>
                    </td>  
                    <td class="ancho50 textoIzquierda">
                        <asp:DropDownList ID="ddlAno" runat="server" DataSourceID="SqlDataSource2" DataTextField="ano" DataValueField="ano"></asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" SelectCommand="select distinct tabla.ano from(select distinct ano from periodos_pagos union all select year(getdate()) as ano ) as tabla order by tabla.ano desc"></asp:SqlDataSource>&nbsp;&nbsp;&nbsp;
                        <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="center" 
                            AutoPostBack="True" onselectedindexchanged="ddlPeriodo_SelectedIndexChanged">
                            <asp:ListItem Value="0" Selected="True">Seleccione Periodo</asp:ListItem>
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
                    <td class="ancho30 textocentradoh">
                        <asp:ImageButton ID="btnGenerarArchivo" runat="server" 
                            ImageUrl="~/IMG/aceptar.png" CssClass="iconitos40" 
                            AlternateText="Cargar Información" ToolTip="Cargar Información" 
                            ValidationGroup="genera" onclick="btnGenerarArchivo_Click"/>
                    </td>                 
                 </tr>
                 <tr>
                    <td class="ancho100 textocentradoh" colspan="3">
                        <asp:Label ID="lblerror" runat="server" ForeColor="red"></asp:Label>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="genera" ForeColor="Red" DisplayMode="List" />
                    </td>
                 </tr>
              </table>
        <asp:Panel ID="Panel5" runat="server" CssClass="ancho440px centardo">
        <asp:Panel ID="Panel1" runat="server" CssClass="ancho440px centardo sombra ">
        
            <table class="tablaheadgrids textocentradoh textobold">
                <tr>
                    <td class="ancho110px">Afiliaci&oacute;n</td>
                    <td class="ancho110px">Referencia</td>
                    <td class="ancho110px">Cuenta</td>
                    <td class="ancho110px">Monto</td>
                </tr>
            </table>
            <asp:Panel ID="Panel2" runat="server" CssClass="altoConsultas" ScrollBars="Auto">
                <asp:GridView ID="GridView1" runat="server" 
                    CssClass="anchoCompleto textocentradoh colorNegro" ShowHeader="false"
                    AutoGenerateColumns="False" GridLines="None" 
                    onrowdatabound="GridView1_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="Afiliación">
                            <ItemTemplate>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("afiliacion") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho110px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Referencia">
                            <ItemTemplate>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("referencia") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho110px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cuenta">
                            <ItemTemplate>
                                <asp:Label ID="Label3" runat="server" Text='<%# Eval("cuenta") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho110px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Monto">
                            <ItemTemplate>
                                <asp:Label ID="Label4" runat="server" Text='<%# Eval("monto") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle CssClass="ancho110px" />
                        </asp:TemplateField>                        
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Label ID="Label5" runat="server" ForeColor="Red" 
                            Text="No existe información para enviar"></asp:Label>
                    </EmptyDataTemplate>
                </asp:GridView>
            </asp:Panel>            
            <table class="tablaFootergrids textobold">
                <tr>
                    <td class="ancho110px">Movimientos:</td>
                    <td class="ancho110px">
                        <asp:Label ID="lblTotal" runat="server"></asp:Label>&nbsp;(
                        <asp:Label ID="lblClientes" runat="server"></asp:Label>)
                    </td>
                    <td class="ancho110px">Monto Total:</td>
                    <td class="ancho110px">
                        <asp:Label ID="lblMontoTotal" runat="server"></asp:Label></td>
                </tr>
            </table>
            
        </asp:Panel>
        <asp:Panel ID="Panel3" runat="server" CssClass="ancho440px centardo textoright">
            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                <ContentTemplate>
                    <table class="ancho100 textocentradoh center">
                        <tr>
                            <td class="ancho50 textoIzquierda">
                                <asp:LinkButton ID="btnConsulta" runat="server" CssClass="link" 
                                    ForeColor="#3ba5d1" Text="Consulta Archivos Generados" 
                                    onclick="btnConsulta_Click"></asp:LinkButton>
                            </td>
                            <td class="ancho50 textoright">
                                <asp:ImageButton ID="btnGenerar" runat="server" AlternateText="Generar Archivo" 
                                    ToolTip="Generar Archivo" ImageUrl="~/IMG/aceptar.png" CssClass="ancho50px" 
                                    onclick="btnGenerar_Click" />
                            </td>
                        </tr>
                    </table>
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
                    <ProgressTemplate>
                        <div class="ontopDiv2">
                            <asp:Panel ID="progress2" runat="server" CssClass="zen4 anchoCompleto center textocentradoh padingTop200px">
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/IMG/25-1.gif" Width="40%" />
                            </asp:Panel>
                        </div>
                    </ProgressTemplate>
                    </asp:UpdateProgress>
                    </ContentTemplate>
                     <Triggers>
                        <asp:PostBackTrigger ControlID="btnGenerar" />
                    </Triggers>
                    </asp:UpdatePanel>
                
                
            
            
        </asp:Panel>
        </asp:Panel>
        
        <!-- Popup Consulta -->         
            <asp:Panel ID="Panel4" runat="server" style="position: fixed; left: 400px; top: 100px; z-index:2; background-color:White; margin:0 auto; text-align:center; width:50%;" CssClass="redondeoPopPanel" Visible="false">
                <table class="tablaheadgrids textocentradoh textobold ancho100 centardo">
                <tr>
                    <td colspan="4" class="textoright">
                        <asp:ImageButton ID="btnCerrar" runat="server" ImageUrl="~/IMG/close.png" 
                            AlternateText="Cerrar" ToolTip="Cerrar" CssClass="iconitos40" 
                            onclick="btnCerrar_Click" /></td>
                </tr>                    
                </table>
                <asp:Panel ID="Panel7" runat="server" CssClass="altoConsultas ancho100 centardo" ScrollBars="Auto">
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
                        DataSourceID="SqlDataSource1" 
                        CssClass="anchoCompleto textocentradoh colorNegro"  
                        GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="periodo" HeaderText="Periodo" ItemStyle-CssClass="ancho110px"
                                SortExpression="periodo" >
                            <ControlStyle CssClass="ancho110px" />
                            <ItemStyle CssClass="ancho110px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="nombre_del_archivo" HeaderText="Archivo" 
                                SortExpression="nombre_del_archivo" ItemStyle-CssClass="ancho110px" >
                            <ControlStyle CssClass="ancho110px" />
                            <ItemStyle CssClass="ancho110px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="cuentas" HeaderText="Cuentas" ItemStyle-CssClass="ancho110px"
                                SortExpression="cuentas" >
                            <ControlStyle CssClass="ancho110px" />
                            <ItemStyle CssClass="ancho110px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="monto" HeaderText="Monto" SortExpression="monto" 
                                ItemStyle-CssClass="ancho110px" DataFormatString="{0:C2}">
                            <ControlStyle CssClass="ancho110px" />
                            <ItemStyle CssClass="ancho110px" />
                            </asp:BoundField>
                        </Columns>
                        <HeaderStyle CssClass=" tablaheadgrids textocentradoh textobold colorObscuro" />
                        <EmptyDataTemplate>
                        <asp:Label ID="lblinfo" runat="server" ForeColor="Red" 
                            Text="No existe información para mostrar"></asp:Label>
                    </EmptyDataTemplate>
                    </asp:GridView>
                    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:eBills %>" 
                        SelectCommand="select periodo,nombre_del_archivo,elementos as cuentas, monto from archivo_cobranza order by periodo desc,consecutivo desc">
                    </asp:SqlDataSource>
                </asp:Panel>
            </asp:Panel>
            <asp:Panel ID="Panel6" runat="server" Visible="false" CssClass="mask"></asp:Panel>

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