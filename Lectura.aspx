<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Lectura.aspx.cs" Inherits="Lectura" MasterPageFile="~/MasterMenus.master" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content runat="server" ID="content" ContentPlaceHolderID="ContentPlaceHolder1" >
    <h2 class="anchoCompleto">Carga de Respuesta</h2>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    
                        
     <asp:Panel ID="Panel1" runat="server" CssClass="anchoCompleto centardo">  
           
        <table class="ancho100 centardo textocentradoh">            
            <tr>                
                <td class="textocentradoh center colorObscuro textobold">
                    <asp:Label ID="Label6" runat="server" Text="Periodo a Generar:" ForeColor="#002c81"></asp:Label>
                    <asp:DropDownList ID="ddlAno" runat="server" DataSourceID="SqlDataSource2" DataTextField="ano" DataValueField="ano"></asp:DropDownList>
                    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:eBills %>" SelectCommand="select distinct tabla.ano from(select distinct ano from periodos_pagos union all select year(getdate()) as ano ) as tabla order by tabla.ano desc"></asp:SqlDataSource>&nbsp;&nbsp;&nbsp;
                    <asp:DropDownList ID="ddlPeriodo" runat="server" CssClass="center" 
                        AutoPostBack="True" onselectedindexchanged="ddlPeriodo_SelectedIndexChanged" >
                        <asp:ListItem Value="0">Seleccione Periodo</asp:ListItem>
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
                <td rowspan="2" class="textocentradoh padingLeft30px textocentradov ancho10">
                    <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                    <ContentTemplate>
                        <asp:Timer ID="Timer1" runat="server" Interval="1000" ontick="Timer1_Tick" ></asp:Timer>
                        <asp:ImageButton ID="btnCargarDoc" runat="server" AlternateText="Cargar Documento" ImageUrl="~/IMG/aceptar.png" CssClass="iconitos40 center" ToolTip="Cargar Documento" onclick="btnCargarDoc_Click" OnClientClick="return confirm('¿Está seguro de realizar la carga del archivo indicado en el periodo señalado?')" />
                        <asp:ImageButton ID="btnProceso" runat="server" 
                            AlternateText="Cargar Documento" ImageUrl="~/IMG/aceptar.png" 
                            CssClass="iconitos40 center" ToolTip="Cargar Documento" 
                            onclick="btnProceso_Click" Visible="false"  /><br />
                            <asp:Label ID="lblError" runat="server" ForeColor="Red" CssClass="center" />
                            <asp:TextBox ID="txtProceso" runat="server" Visible="false"></asp:TextBox>
                        <asp:Panel ID="Panel4" runat="server" CssClass="ontopDiv" Visible="false" onload="Panel4_Load">
                                    <asp:Panel ID="progress" Visible="false"  runat="server" CssClass="zen4 anchoCompleto center textocentradoh padingTop200px">
                                         
                                        <asp:Image ID="Image1" Visible="false"  runat="server" ImageUrl="~/IMG/25-1.gif" Width="40%" />
                                    </asp:Panel>
                                </asp:Panel>
                                <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel3">
            <ProgressTemplate>
                <div class="ontopDiv">
                    <asp:Panel ID="progress3" Visible="false" runat="server" CssClass="zen4 anchoCompleto center textocentradoh padingTop200px">
                        <asp:Image ID="Image13" Visible="false" runat="server" ImageUrl="~/IMG/25-1.gif" Width="40%" />
                    </asp:Panel>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress> 
                    </ContentTemplate>
                    <Triggers><asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" /></Triggers>
                    </asp:UpdatePanel>
                                
                                
                    
                </td>               
            </tr> 
            <tr>
                <td class="textocentradoh padingLeft30px textocentradov ancho50">                    
                    <cc1:AjaxFileUpload ID="AjaxFileUpload1" runat="server" MaximumNumberOfFiles="1" onuploadcomplete="AjaxFileUpload1_UploadComplete" />                    
                </td>
                
            </tr>                               
        </table>
        
        <asp:Panel ID="Panel2" runat="server" >            
            
           
                    
                     <table class="tablaheadgrids textocentradoh textobold ancho744px centardo">
                <tr>
                    <td class="ancho69px">
                        <asp:Label ID="Label1" runat="server" Text="Referencia" />
                    </td>
                    <td class="ancho100px;">
                        <asp:Label ID="Label2" runat="server" Text="Periodo" />
                    </td>
                    <td class="ancho100px;">
                        <asp:Label ID="Label3" runat="server" Text="Fecha Pago" />
                    </td>
                    <td class="ancho150px;">
                        <asp:Label ID="Label7" runat="server" Text="No. Cuenta" />
                    </td>                                
                    <td class="ancho230px">
                        <asp:Label ID="Label5" runat="server" Text="Motivo Rechazo" />
                    </td>
                    <td class="ancho95px">
                        <asp:Label ID="Label4" runat="server" Text="Monto" />
                    </td>
                </tr>
            </table> 
             <asp:UpdatePanel ID="UpdatePanel2" runat="server" >
                <ContentTemplate>               
                <asp:Panel ID="Panel3" runat="server" CssClass="altoConsultas centardo ancho744px" ScrollBars="Auto">                                        
                    <asp:GridView ID="GridCargaRespuesta" runat="server" GridLines="None" EmptyDataRowStyle-ForeColor="Red" 
                        EmptyDataText="No se a cargado documento de respuesta" ShowHeader="false" 
                        onrowdatabound="GridCargaRespuesta_RowDataBound" AutoGenerateColumns="false" CssClass="colorNegro textocentradoh">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblReferencia" runat="server" Text='<%# Eval("referencia") %>'/>                                        
                                </ItemTemplate>
                                <ItemStyle CssClass="ancho69px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblPeriodo" runat="server" Text='<%# Eval("periodo") %>'/>
                                </ItemTemplate>
                                <ItemStyle CssClass="ancho100px" />
                            </asp:TemplateField>                                     
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblFecha" runat="server" Text='<%# Eval("fecha","{0:d MMM yyyy}") %>'/>
                                </ItemTemplate>
                                    <ItemStyle CssClass="ancho100px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblCuenta" runat="server" Text='<%# Eval("cuenta") %>'/>
                                </ItemTemplate>
                                <ItemStyle CssClass="ancho150px" />
                            </asp:TemplateField>                                    
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblMotivoRechazo" runat="server" Text='<%# Eval("motivo_rechazo") %>'/>
                                </ItemTemplate>
                                <ItemStyle CssClass="ancho230px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblMonto" runat="server" Text='<%# Eval("monto") %>'/>
                                </ItemTemplate>
                                <ItemStyle CssClass="ancho95px" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>                
                </asp:Panel>
                <table class="tablaFootergrids ancho744px textocentradoh textobold centardo">
                    <tr class="pading8px">
                        <td>Realizados</td>
                        <td>Rechazados</td>
                    </tr>
                    <tr>
                        <td class="colorBlanco">Operaciones:&nbsp;<asp:Label ID="lblCtaReal" runat="server" CssClass="colorBlanco" ></asp:Label></td>
                        <td style="color:Red">Operaciones:&nbsp;<asp:Label ID="lblCtaRec" runat="server" ForeColor="Red"></asp:Label></td>
                    </tr>
                    <tr><td colspan="2" class="pading8px">Total Operaciones:&nbsp;<asp:Label ID="lblTotalCuentas" runat="server"></asp:Label></td></tr>
                    <tr>
                        <td class="colorBlanco">Clientes:&nbsp;<asp:Label ID="lblAceptados" runat="server" CssClass="colorBlanco" ></asp:Label></td>
                        <td style="color:Red">Clientes:&nbsp;<asp:Label ID="lblRechazados" runat="server" ForeColor="Red"></asp:Label></td>
                    </tr>
                    <tr><td colspan="2" class="pading8px">Total Clientes:&nbsp;<asp:Label ID="lblTotalClientes" runat="server"></asp:Label></td></tr>
                    <tr>
                        <td class="colorBlanco">Monto Operaciones:&nbsp;<asp:Label ID="lblMontoReal" runat="server" CssClass="colorBlanco" ></asp:Label></td>
                        <td style="color:Red">Monto Operaciones:&nbsp;<asp:Label ID="lblMontoRec" runat="server" ForeColor="Red"></asp:Label></td>
                    </tr>
                    <tr><td colspan="2" class="pading8px">Monto Total Operaciones:&nbsp;<asp:Label ID="lblTotal" runat="server"></asp:Label></td></tr>
                </table>
                    <br />
                    <div class="anchoCompleto centardo textocentradoh">
                        <asp:LinkButton ID="lnkNotificar" runat="server" Text="Notificar Rechazos" CssClass="link colorAzul" OnClick="lnkNotificar_Click"></asp:LinkButton>
                    </div>
                    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
            <ProgressTemplate>
                <div class="ontopDiv">
                    <asp:Panel ID="progress2" runat="server" Visible="false"  CssClass="zen4 anchoCompleto center textocentradoh padingTop200px">
                        <asp:Image ID="Image12" runat="server" Visible="false"  ImageUrl="~/IMG/25-1.gif" Width="40%" />
                    </asp:Panel>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress> 
                </ContentTemplate>
            </asp:UpdatePanel>
               
            
        </asp:Panel>



        
    </asp:Panel>
    
    
        

   
</asp:Content>