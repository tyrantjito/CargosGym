<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Cargas.aspx.cs" Inherits="Cargas" MasterPageFile="~/MasterMenus.master"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:Label ID="lblCargas" runat="server" Visible="false"></asp:Label>
        <asp:Label ID="lblClientAnt" runat="server" Visible="false"></asp:Label>
            <h2 class="anchoCompleto">Cargas</h2>    
            <div class="anchoCompleto centardo textocentradoh">
                <asp:Panel ID="Panel1" runat="server" Visible="false">
                    <asp:Label ID="Label1" runat="server" ></asp:Label>
                    <asp:Label ID="Label2" runat="server" ></asp:Label>
                    <asp:Label ID="Label3" runat="server" ></asp:Label>
                    <asp:Label ID="Label4" runat="server" ></asp:Label>
                    <asp:Label ID="Label5" runat="server" ></asp:Label>
                    <asp:Label ID="Label6" runat="server" ></asp:Label>
                    <asp:Label ID="Label7" runat="server" ></asp:Label>
                    <asp:Label ID="Label8" runat="server" ></asp:Label>
                    <asp:Label ID="Label9" runat="server" ></asp:Label>
                    <asp:Label ID="Label10" runat="server" ></asp:Label>
                    <asp:Label ID="Label11" runat="server" ></asp:Label>
                    <asp:Label ID="Label12" runat="server" ></asp:Label>
                    <asp:Label ID="lblEmpresa" runat="server" ></asp:Label>
                    <asp:Label ID="lblCondicion" runat="server"></asp:Label>                    
                </asp:Panel>
                <table class="ancho100 centardo">
                    <tr class="textocentradoh textobold centardo">
                        <td class="ancho10 textocentradoh centardo">
                            <asp:Label ID="Label14" runat="server" Text="Año:" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                            <asp:DropDownList ID="ddlAño" runat="server" CssClass="center" 
                                DataSourceID="SqlDataSource1" DataTextField="ano" DataValueField="ano">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:eBills %>" 
                                SelectCommand="select ano from periodos_pagos group by ano order by ano desc">
                            </asp:SqlDataSource>
                        </td> 
                        <td class="ancho10 textocentradoh centardo">
                            <asp:Label ID="Label13" runat="server" Text="Periodo:" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                            <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="center" 
                                AutoPostBack="True" onselectedindexchanged="ddlPeriodo_SelectedIndexChanged" >                                
                                <asp:ListItem Value="01">Enero</asp:ListItem>
                                <asp:ListItem Value="02">Febrero</asp:ListItem>
                                <asp:ListItem Value="03">Marzo</asp:ListItem>
                                <asp:ListItem Value="04">Abril</asp:ListItem>
                                <asp:ListItem Value="05">Mayo</asp:ListItem>
                                <asp:ListItem Value="06">Junio</asp:ListItem>
                                <asp:ListItem Value="07">Julio</asp:ListItem>
                                <asp:ListItem Value="08">Agosto</asp:ListItem>
                                <asp:ListItem Value="09">Septiembre</asp:ListItem>
                                <asp:ListItem Value="10">Octubre</asp:ListItem>
                                <asp:ListItem Value="11">Noviembre</asp:ListItem>
                                <asp:ListItem Value="12">Diciembre</asp:ListItem>
                            </asp:DropDownList>
                        </td>   
                        <td class="ancho10 textocentradoh centardo">
                            <asp:Label ID="Label15" runat="server" Text="Estatus:" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                            <asp:DropDownList ID="ddlEstatus" runat="server" CssClass="center" 
                                DataSourceID="SqlDataSource2" DataTextField="estatus" 
                                DataValueField="movimiento" AutoPostBack="true" 
                                onselectedindexchanged="ddlEstatus_SelectedIndexChanged" >
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" 
                                SelectCommand="select distinct DC.movimiento, case dc.movimiento when 'P'then 'Pagado' when 'R' then 'Rechazado' when 'M' then 'Sucursal' else '' end as estatus from detalle_cliente dc where substring(dc.periodo,1,4)=@ano and substring(dc.periodo,5,2)=@periodo and (select top 1 movimiento from detalle_cliente where periodo=dc.periodo and id_cliente=dc.id_cliente order by consecutivo desc)=dc.movimiento and dc.id_empresa=@idEmpresa">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlAño" Name="ano" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="ddlPeriodo" Name="periodo" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="lblEmpresa" Name="idEmpresa" PropertyName="Text" />
                                </SelectParameters>
                            </asp:SqlDataSource>                            
                        </td> 
                        <td class="ancho10 textocentradoh centardo">
                            <asp:Label ID="Label16" runat="server" Text="Carga:" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                            <asp:DropDownList ID="ddlFechas" runat="server" CssClass="center" 
                                DataSourceID="SqlDataSource3" DataTextField="fecha" 
                                DataValueField="fecha" AutoPostBack="True" 
                                onselectedindexchanged="ddlFechas_SelectedIndexChanged">
                            </asp:DropDownList>                                                       
                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" SelectCommand="select distinct case dc.movimiento when 'R' then Convert(char(10),dc.fecha_pago_rechazado,126) else convert(char(10),dc.fecha_pago,126) end as fecha from detalle_cliente dc where substring(dc.periodo,1,4)=@ano and substring(dc.periodo,5,2)=@periodo and (select top 1 movimiento from detalle_cliente where periodo=dc.periodo and id_cliente=dc.id_cliente order by consecutivo desc)=dc.movimiento and dc.movimiento=@movimiento and dc.id_empresa=@idEmpresa order by 1 desc">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="ddlAño" Name="ano" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="ddlPeriodo" Name="periodo" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="ddlEstatus" Name="movimiento" PropertyName="SelectedValue" />
                                    <asp:ControlParameter ControlID="lblEmpresa" Name="idEmpresa" PropertyName="Text" />
                                </SelectParameters>
                            </asp:SqlDataSource>                                                       
                        </td> 
                    </tr>
                </table>
                 <asp:Panel ID="PanGridRec" runat="server" CssClass="sombra ancho850px centardo" >
                    <table class="tablaheadgrids textobold textocentradoh ancho850px centardo" >
                        <tr>
                            <td class="ancho100px">
                                <asp:Label ID="Label17" runat="server" Text="Referencia"/>
                            </td>
                            <td class="ancho300px">
                                <asp:Label ID="Label18" runat="server" Text="Nombre"/>
                            </td>
                            <td class="ancho150px">
                                <asp:Label ID="Label19" runat="server" Text="Fecha"/>
                            </td>
                            <td class="ancho300px">
                                <asp:Label ID="Label20" runat="server" Text="Motivo"/>
                            </td>                            
                        </tr>
                    </table>
              
                    <asp:Panel ID="Panel3" runat="server" CssClass="altoConsultas" ScrollBars="Auto">
                        <asp:GridView ID="GridView1" runat="server" ShowHeader="False" EmptyDataText="No existe Información" 
                                GridLines="None" AutoGenerateColumns="False" CssClass="colorNegro ancho100"
                                EmptyDataRowStyle-ForeColor="Red" DataSourceID="SqlDataSource4" OnRowDataBound="GridView1_RowDataBound">            
                                <Columns>
                                    <asp:BoundField DataField="id_cliente" HeaderText="id_cliente" SortExpression="id_cliente" Visible="false" />
                                    <asp:BoundField DataField="referencia" HeaderText="Referencia" 
                                        SortExpression="referencia" ItemStyle-CssClass="ancho100px" >
                                    <ItemStyle CssClass="ancho100px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Nombre" HeaderText="Cliente" ReadOnly="True" 
                                        SortExpression="Nombre" ItemStyle-CssClass="ancho300px" >
                                    <ItemStyle CssClass="ancho300px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="fecha_pago" HeaderText="Fecha" ReadOnly="True" 
                                        SortExpression="fecha_pago" ItemStyle-CssClass="ancho150px">                                    
                                    <ItemStyle CssClass="ancho150px" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Motivo" SortExpression="motivo_rechzado">
                                        <ItemTemplate>
                                            <asp:Label ID="lblMotivo" runat="server" Text='<%# Bind("motivo_rechzado") %>'></asp:Label>
                                        </ItemTemplate>                                        
                                        <ItemStyle CssClass="ancho300px" />
                                    </asp:TemplateField>

                                </Columns>
                            <EmptyDataRowStyle ForeColor="Red" />
                        </asp:GridView>                             
                            <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" ></asp:SqlDataSource>
                    </asp:Panel>
                    
                </asp:Panel>
                <br />
                    <div class="ancho850px centardo textoright colorNegro">
                        <asp:Label ID="lblTotal" runat="server"></asp:Label>
                    </div>
                            
                             
                     
                
            </div>
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" >
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
