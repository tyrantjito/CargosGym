<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Estadisticos.aspx.cs" Inherits="Estadisticos" MasterPageFile="~/MasterMenus.master" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <h2 class="anchoCompleto">Estad&iacute;sticos</h2>    
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
                </asp:Panel>
                <table class="ancho100 centardo">
                    <tr class="textocentradoh textobold centardo">
                        <td class="ancho50 textocentradoh centardo">
                            <asp:RadioButtonList ID="dlEstadistico" runat="server" CssClass="centardo" 
                                ForeColor="#002c81" AutoPostBack="True" 
                                onselectedindexchanged="dlEstadistico_SelectedIndexChanged" RepeatColumns="6" 
                                RepeatDirection="Horizontal">
                                <asp:ListItem Value="0">Pagos</asp:ListItem>
                                <asp:ListItem Value="1">Montos</asp:ListItem>
                                <asp:ListItem Value="2">Altas</asp:ListItem>
                                <asp:ListItem Value="3">Bajas</asp:ListItem>
                                <asp:ListItem Value="4">Bancos</asp:ListItem>
                                <asp:ListItem Value="5">Usuarios</asp:ListItem>
                            </asp:RadioButtonList>
                        </td> 
                        <td class="ancho50 textocentradoh centardo">
                            <asp:Label ID="Label13" runat="server" Text="Periodo:" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                            <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="center" 
                                AutoPostBack="true" onselectedindexchanged="ddlPeriodo_SelectedIndexChanged">
                                <asp:ListItem Value="0">Todos</asp:ListItem>
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
                    </tr>
                    <tr>                
                        <td class="ancho100" colspan="2">
                            <asp:Panel ID="pnlPagos" runat="server" CssClass="ancho100 centardo" Visible="false">
                                <telerik:RadHtmlChart runat="server" ID="BarChartPagos" CssClass="anchoCompleto centardo"  Transitions="true" Skin="Silk">
                                    <PlotArea>                        
                                        <Appearance>
                                            <FillStyle BackgroundColor="Transparent"></FillStyle>
                                        </Appearance>
                                        <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside" Reversed="false">                            
                                            <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                            <TitleAppearance Position="Center" RotationAngle="0" Text="Periodos"></TitleAppearance>
                                        </XAxis>
                                        <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside"
                                            MinorTickType="None" Reversed="false" >
                                            <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1" ></LabelsAppearance>                            
                                        </YAxis>
                                    </PlotArea>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <ChartTitle Text="Pagos">
                                        <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                    </ChartTitle>
                                    <Legend>
                                        <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                    </Legend>
                                </telerik:RadHtmlChart>
                            </asp:Panel>
                            <asp:Panel ID="pnlMontos" runat="server" CssClass="ancho100 centardo" Visible="false">                               
                                <telerik:RadHtmlChart runat="server" ID="BarChartMontos" CssClass="ancho100 centardo"  Transitions="true" Skin="Silk">
                                    <PlotArea>                        
                                        <Appearance>
                                            <FillStyle BackgroundColor="Transparent"></FillStyle>
                                        </Appearance>
                                        <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside" Reversed="false">                            
                                            <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                            <TitleAppearance Position="Center" RotationAngle="0" Text="Periodos"></TitleAppearance>
                                        </XAxis>
                                        <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside"
                                            MinorTickType="None" Reversed="false">
                                            <LabelsAppearance DataFormatString="{0:C2}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>                            
                                        </YAxis>
                                    </PlotArea>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <ChartTitle Text="Montos">
                                        <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                    </ChartTitle>
                                    <Legend>
                                        <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                    </Legend>
                                </telerik:RadHtmlChart>    
                            </asp:Panel>
                             <asp:Panel ID="pnlAltas" runat="server" CssClass="ancho100 centardo" Visible="false">
                                 <telerik:RadHtmlChart runat="server" ID="BarChartAltas" CssClass="ancho100 centardo"  Transitions="true" Skin="Silk">
                                    <PlotArea>                        
                                        <Appearance>
                                            <FillStyle BackgroundColor="Transparent"></FillStyle>
                                        </Appearance>
                                        <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside" Reversed="false">                            
                                            <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                            <TitleAppearance Position="Center" RotationAngle="0" Text="Periodos"></TitleAppearance>
                                        </XAxis>
                                        <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside"
                                            MinorTickType="None" Reversed="false">
                                            <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>                            
                                        </YAxis>
                                    </PlotArea>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <ChartTitle Text="Altas">
                                        <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                    </ChartTitle>
                                    <Legend>
                                        <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                    </Legend>
                                </telerik:RadHtmlChart> 
                             </asp:Panel>
                             <asp:Panel ID="pnlBajas" runat="server" CssClass="ancho100 centardo" Visible="false">
                                 <telerik:RadHtmlChart runat="server" ID="BarChartBajas" CssClass="ancho100 centardo"  Transitions="true" Skin="Silk">
                                    <PlotArea>                        
                                        <Appearance>
                                            <FillStyle BackgroundColor="Transparent"></FillStyle>
                                        </Appearance>
                                        <XAxis AxisCrossingValue="0" Color="black" MajorTickType="Outside" MinorTickType="Outside" Reversed="false">                            
                                            <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>
                                            <TitleAppearance Position="Center" RotationAngle="0" Text="Periodos"></TitleAppearance>
                                        </XAxis>
                                        <YAxis AxisCrossingValue="0" Color="black" MajorTickSize="1" MajorTickType="Outside"
                                            MinorTickType="None" Reversed="false">
                                            <LabelsAppearance DataFormatString="{0}" RotationAngle="0" Skip="0" Step="1"></LabelsAppearance>                            
                                        </YAxis>
                                    </PlotArea>
                                    <Appearance>
                                        <FillStyle BackgroundColor="Transparent"></FillStyle>
                                    </Appearance>
                                    <ChartTitle Text="Bajas">
                                        <Appearance Align="Center" BackgroundColor="Transparent" Position="Top"></Appearance>
                                    </ChartTitle>
                                    <Legend>
                                        <Appearance BackgroundColor="Transparent" Position="Bottom"></Appearance>
                                    </Legend>
                                </telerik:RadHtmlChart>
                             </asp:Panel>
                             <asp:Panel ID="pnlBancos" runat="server" CssClass="ancho100 centardo" Visible="false" >
                                  <asp:DataList ID="dtBancos" runat="server" RepeatColumns="3" onitemdatabound="dtBancos_ItemDataBound" CssClass="centardo ancho50" DataSourceID="SqlDataSource2">
                                    <ItemTemplate>                   
                                        <div class="ancho30">
                                            <asp:Label ID="lblIdBanco" runat="server" Text='<%# Eval("id_banco") %>' Visible="false"></asp:Label>
                                            <asp:Label ID="Label15" runat="server" Text='<%# Eval("nombre") %>' ForeColor="#002c81" CssClass="ancho100"></asp:Label>
                                             <telerik:RadHtmlChart runat="server" ID="DonutChart1" Width="300" Height="300" Transitions="true" Skin="Silk">                                                                                            
                                            </telerik:RadHtmlChart>
                                        </div>                                        
                                    </ItemTemplate>                       
                                </asp:DataList>
                                <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" SelectCommand="select c.id_banco,b.nombre from clientes c inner join bancos b on b.id_banco=c.id_banco where c.id_banco is not null group by c.id_banco,b.nombre"></asp:SqlDataSource>                                 
                             </asp:Panel>
                            <asp:Panel ID="panelUsuarios" runat="server" CssClass="ancho100 centardo" Visible="false">
                                <br />
                                <table class="ancho100">
                                    <tr class="ancho100">
                                        <td class="ancho50">
                                            <asp:Label ID="Label16" runat="server" Text="Año:" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                                            <asp:DropDownList ID="ddlAños" runat="server" DataSourceID="SqlDataSource1" DataTextField="valor" DataValueField="año" AutoPostBack="true" OnSelectedIndexChanged="ddlAños_SelectedIndexChanged"></asp:DropDownList>
                                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" SelectCommand="select distinct tabla.año, tabla.valor from (select year(fecha_inicio) as año,cast(year(fecha_inicio) as char(4)) as valor  from clientes) as tabla order by tabla.año desc"></asp:SqlDataSource>
                                        </td>
                                        <td class="ancho50">
                                            <asp:Label ID="Label17" runat="server" Text="Usuario:" ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                                            <asp:DropDownList ID="ddlUsuarios" runat="server" DataSourceID="SqlDataSource3" DataTextField="NOMBRE" DataValueField="USUARIO" AutoPostBack="true" OnSelectedIndexChanged="ddlUsuarios_SelectedIndexChanged" AppendDataBoundItems="true">
                                                <asp:ListItem Value="T" Text="Todos"></asp:ListItem>
                                            </asp:DropDownList> 
                                            <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" ></asp:SqlDataSource>                                          
                                        </td>
                                    </tr>
                                </table>                                
                                <br />
                                <asp:DataList ID="dtUsuarios" runat="server" RepeatColumns="1" CssClass="centardo ancho100" DataSourceID="SqlDataSourceUsuarios" OnItemDataBound="dtUsuarios_ItemDataBound">
                                    <ItemTemplate>
                                        <div class="ancho100">
                                            <asp:Label ID="Label14" runat="server" Text="Usuario: " ForeColor="#002c81" CssClass="textobold center"></asp:Label>                                            
                                            <asp:Label ID="lblIdUsuario" runat="server" Text='<%# Eval("USUARIO") %>' ForeColor="#002c81" CssClass="textobold center" Visible="false"></asp:Label>
                                            <asp:Label ID="lblUsuario" runat="server" Text='<%# Eval("NOMBRE") %>' ForeColor="#002c81" CssClass="textobold center"></asp:Label>
                                        </div>
                                        <div class="ancho100">
                                            <table>
                                                <tr>
                                                    <td class="ancho340px">
                                                        <telerik:RadHtmlChart runat="server" ID="DonutChartUsuario" Width="300" Height="300" Transitions="true" Skin="Silk"></telerik:RadHtmlChart>
                                                    </td>
                                                    <td class="ancho900px">
                                                        <telerik:RadTabStrip RenderMode="Lightweight" ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1" Skin="MetroTouch" Align="Justify" CssClass="ancho900px">
                                                            <Tabs>
                                                                <telerik:RadTab PageViewID="RadPageView1" Text="Altas" Selected="true" />
                                                                <telerik:RadTab PageViewID="RadPageView2" Text="Bajas" />
                                                                <telerik:RadTab PageViewID="RadPageView3" Text="Bajas Inmediatas" />                                                        
                                                            </Tabs>
                                                        </telerik:RadTabStrip>
                                                        <telerik:RadMultiPage ID="RadMultiPage1" runat="server" CssClass="ancho900px" >
                                                            <telerik:RadPageView ID="RadPageView1" runat="server" Selected="true" CssClass="ancho900px">                                        
                                                                <telerik:RadGrid RenderMode="Lightweight" ID="radGridAltaUser" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro" CssClass="ancho900px"
                                                                    AllowPaging="true" ShowFooter="true" PagerStyle-AlwaysVisible="true" AllowSorting="true" PageSize="1000" OnPageIndexChanged="radGridAltaUser_PageIndexChanged" OnSortCommand="radGridAltaUser_SortCommand" OnPageSizeChanged="radGridAltaUser_PageSizeChanged" >                                            
                                                                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="id_cliente">                                                    
                                                                        <Columns>            
                                                                            <telerik:GridBoundColumn DataField="año" HeaderText="Año" SortExpression="año" UniqueName="año" Visible="false"/>                                        
                                                                            <telerik:GridBoundColumn DataField="referencia" HeaderText="Referencia" SortExpression="referencia" UniqueName="referencia"/>  
                                                                            <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" SortExpression="cliente" UniqueName="cliente"/>                                                                                                                                             
                                                                            <telerik:GridBoundColumn DataField="nombre" HeaderText="Tarjeta Habiente" SortExpression="nombre" UniqueName="nombre"/>
                                                                            <telerik:GridBoundColumn DataField="fecha_inicio" HeaderText="Fecha" SortExpression="fecha_inicio" UniqueName="fecha_inicio" DataFormatString="{0:d MMM yyyy}"/>                                
                                                                            <telerik:GridBoundColumn DataField="valorEstatus" HeaderText="Estatus" SortExpression="valorEstatus" UniqueName="valorEstatus"/>                                                        
                                                                        </Columns>
                                                                        <NoRecordsTemplate>
                                                                            <asp:Label ID="lblVacioAlta" runat="server" Text="No existe información de altas" ForeColor="Red"></asp:Label>
                                                                        </NoRecordsTemplate>
                                                                    </MasterTableView>
                                                                    <ClientSettings>
                                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                                    </ClientSettings>                        
                                                                    <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                            
                                                                </telerik:RadGrid>
                                                            </telerik:RadPageView>
                                                            <telerik:RadPageView ID="RadPageView2" runat="server" CssClass="ancho900px">
                                                                <telerik:RadGrid RenderMode="Lightweight" ID="radGridBajasUser" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro"  CssClass="ancho900px"
                                                                    AllowPaging="true" ShowFooter="true" PagerStyle-AlwaysVisible="true" AllowSorting="true" PageSize="1000" OnPageIndexChanged="radGridBajasUser_PageIndexChanged" OnSortCommand="radGridBajasUser_SortCommand" OnPageSizeChanged="radGridBajasUser_PageSizeChanged" >                                            
                                                                    <MasterTableView AutoGenerateColumns="False">                                                    
                                                                        <Columns>                                                                
                                                                            <telerik:GridBoundColumn DataField="referencia" HeaderText="Referencia" SortExpression="referencia" UniqueName="referencia"/>                                                                                                                                               
                                                                            <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" SortExpression="cliente" UniqueName="cliente"/>
                                                                            <telerik:GridBoundColumn DataField="tarjeta_habiente" HeaderText="Tarjeta Habiente" SortExpression="tarjeta_habiente" UniqueName="tarjeta_habiente"/>
                                                                            <telerik:GridBoundColumn DataField="fecha_baja" HeaderText="Fecha" SortExpression="fecha_baja" UniqueName="fecha_baja" DataFormatString="{0:d MMM yyyy}"/>                                
                                                                            <telerik:GridBoundColumn DataField="usuarioBaja" HeaderText="Usuario Baja" SortExpression="usuarioBaja" UniqueName="usuarioBaja"/>
                                                                            <telerik:GridBoundColumn DataField="usuarioAutorizo" HeaderText="Usuario Autorizo" SortExpression="usuarioAutorizo" UniqueName="usuarioAutorizo"/>
                                                                            <telerik:GridBoundColumn DataField="motivo" HeaderText="Motivo" SortExpression="motivo" UniqueName="motivo"/>                                                                            
                                                                        </Columns>
                                                                        <NoRecordsTemplate>
                                                                            <asp:Label ID="lblVacioBaja" runat="server" Text="No existe información de bajas" ForeColor="Red"></asp:Label>
                                                                        </NoRecordsTemplate>
                                                                    </MasterTableView>
                                                                    <ClientSettings>
                                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                                    </ClientSettings>                        
                                                                    <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                                </telerik:RadGrid>
                                                            </telerik:RadPageView>                                                   
                                                        
                                                            <telerik:RadPageView ID="RadPageView3" runat="server" CssClass="ancho900px">
                                                                <telerik:RadGrid RenderMode="Lightweight" ID="radGridBajasInmediatasUser" runat="server"  EnableHeaderContextMenu="true" Culture="es-Mx" Skin="Metro"  CssClass="ancho900px"
                                                                    AllowPaging="true" ShowFooter="true" PagerStyle-AlwaysVisible="true" AllowSorting="true" PageSize="1000" OnPageIndexChanged="radGridBajasInmediatasUser_PageIndexChanged" OnSortCommand="radGridBajasInmediatasUser_SortCommand" OnPageSizeChanged="radGridBajasInmediatasUser_PageSizeChanged" >                                            
                                                                    <MasterTableView AutoGenerateColumns="False">                                                    
                                                                        <Columns>                                                                
                                                                            <telerik:GridBoundColumn DataField="referencia" HeaderText="Referencia" SortExpression="referencia" UniqueName="referencia"/>                                                                                                                                               
                                                                            <telerik:GridBoundColumn DataField="cliente" HeaderText="Cliente" SortExpression="cliente" UniqueName="cliente"/>
                                                                            <telerik:GridBoundColumn DataField="tarjeta_habiente" HeaderText="Tarjeta Habiente" SortExpression="tarjeta_habiente" UniqueName="tarjeta_habiente"/>
                                                                            <telerik:GridBoundColumn DataField="fecha_baja" HeaderText="Fecha" SortExpression="fecha_baja" UniqueName="fecha_baja" DataFormatString="{0:d MMM yyyy}"/>                                
                                                                            <telerik:GridBoundColumn DataField="usuarioBaja" HeaderText="Usuario Baja" SortExpression="usuarioBaja" UniqueName="usuarioBaja"/>
                                                                            <telerik:GridBoundColumn DataField="usuarioAutorizo" HeaderText="Usuario Autorizo" SortExpression="usuarioAutorizo" UniqueName="usuarioAutorizo"/>
                                                                            <telerik:GridBoundColumn DataField="motivo" HeaderText="Motivo" SortExpression="motivo" UniqueName="motivo"/>                                                                            
                                                                        </Columns>
                                                                        <NoRecordsTemplate>
                                                                            <asp:Label ID="lblVacioBaja" runat="server" Text="No existe información de bajas" ForeColor="Red"></asp:Label>
                                                                        </NoRecordsTemplate>
                                                                    </MasterTableView>
                                                                    <ClientSettings>
                                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true"></Scrolling>
                                                                    </ClientSettings>                        
                                                                    <PagerStyle PageSizeControlType="RadDropDownList" Mode="NextPrevAndNumeric"></PagerStyle>
                                                                </telerik:RadGrid>
                                                            </telerik:RadPageView>                                                   
                                                        </telerik:RadMultiPage>
                                                    </td>
                                                </tr>
                                            </table>                                            
                                        </div>
                                    </ItemTemplate>
                                </asp:DataList>
                                <asp:SqlDataSource ID="SqlDataSourceUsuarios" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" />
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="anchoCompleto centardo textocentradoh">                                
                
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