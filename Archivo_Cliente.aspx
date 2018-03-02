<%@ Page Title="" Language="C#" MasterPageFile="~/MasterMenus.master" AutoEventWireup="true" CodeFile="Archivo_Cliente.aspx.cs" Inherits="Archivo_Cliente" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="anchoCompleto">Importa Clientes</h2>
  <div class="anchoCompleto centardo textocentradoh">      
    <asp:Panel ID="Panel1" runat="server" CssClass="centardo ancho794px sombra" >  
        
    <table class="tablaheadgrids textobold textocentradoh">
            <tr>
                <td class="ancho69px"><asp:Label ID="Label1" runat="server" Text="Referencia"></asp:Label></td>
                <td class="ancho230px" colspan="3"><asp:Label ID="Label2" runat="server" Text="Nombre"></asp:Label></td>                
                <td class="ancho95px"><asp:Label ID="Label5" runat="server" Text="Monto"></asp:Label></td>
                <td class="ancho150px"><asp:Label ID="Label6" runat="server" Text="Cuenta"></asp:Label></td>
                <td class="ancho100px"><asp:Label ID="Label7" runat="server" Text="Fecha Inicio"></asp:Label></td>
                <td class="ancho100px"><asp:Label ID="Label8" runat="server" Text="Fecha Final"></asp:Label></td>
                <td class="ancho50px"><asp:Label ID="Label10" runat="server" Text="Periodo Pago"></asp:Label></td>
            </tr>
         </table>
        <asp:Panel ID="Panel2" runat="server" CssClass="altoConsultas" ScrollBars="Auto">        
            <asp:GridView ID="GridView1" runat="server"  CssClass="colorNegro textocentradoh" ShowHeader="False" AutoGenerateColumns="False"  GridLines="None"  EmptyDataText="No ha subido un archivo"  >
                <Columns>
                    <asp:TemplateField>                
                        <ItemTemplate>
                            <asp:Label ID="Label11" runat="server" Text='<%# Eval("referencia") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho69px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="Label12" runat="server" Text='<%# Eval("nombre") %>'></asp:Label>&nbsp;
                            <asp:Label ID="Label13" runat="server" Text='<%# Eval("apellidoPat") %>'></asp:Label>&nbsp;
                            <asp:Label ID="Label14" runat="server" Text='<%# Eval("apellidoMat") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho230px"></ItemStyle>
                    </asp:TemplateField>                                        
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="Label15" runat="server" Text='<%# Eval("monto") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho95px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="Label16" runat="server" Text='<%# Eval("cuenta") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho150px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="Label17" runat="server" Text='<%# Eval("fechaIni") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho100px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="Label18" runat="server" Text='<%# Eval("fechaFin") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho100px"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="Label19" runat="server" Text='<%# Eval("Periodo") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle CssClass="ancho50px"></ItemStyle>
                    </asp:TemplateField>            
                </Columns>
                 <EmptyDataRowStyle ForeColor="Red" />
            </asp:GridView>
          </asp:Panel>
     <table class="tablaFootergrids anchoCompleto textocentradoh">
         <tr>
             <td>
                <asp:FileUpload ID="FileUpload1" runat="server" CssClass="center" /> &nbsp;<asp:ImageButton 
                     ID="btnAplicar" runat="server" AlternateText="Procesar Archivo" 
                     ImageUrl="~/IMG/aceptar.png" CssClass="iconitos40 center" 
                     onclick="btnAplicar_Click" />
             </td>
             <td class="ancho80px">                
                    <asp:ImageButton ID="btnGuardar" runat="server" CssClass="iconitos40"
                     ImageUrl="~/IMG/activa.png" ToolTip="Guardar" AlternateText="Guardar" 
                     onclick="btnGuardar_Click" Visible="false" />
             </td>
         </tr>
     </table>
        
      </asp:Panel>
      <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
      <br/>
      <asp:Label ID="lblInsert" runat="server"></asp:Label>
      
      <h2 class="anchoCompleto">Clientes No Importados</h2>
       
      <asp:Panel ID="Panel3" runat="server" Visible="false" width="300px" CssClass="centardo  sombra">
        
        <table class="tablaheadgrids textobold textocentradoh">
            <tr>
                <td class="ancho69px"><asp:Label ID="Label3" runat="server" Text="Referencia"></asp:Label></td>
                <td class="ancho230px" colspan="3"><asp:Label ID="Label4" runat="server" Text="Nombre"></asp:Label></td>                
                </tr>
         </table>
        
      <asp:Panel ID="Panel4" runat="server"  CssClass="altoConsultas" ScrollBars="Auto">
          <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" CssClass="colorNegro textocentradoh" ShowHeader="False" GridLines="None">
          
              <Columns>
                  <asp:TemplateField>
                      <ItemTemplate>
                          <asp:Label ID="lblRefrec" runat="server" Text='<%# Eval("referenciaRech") %>'></asp:Label>
                      </ItemTemplate>
                      <ItemStyle CssClass="ancho69px"></ItemStyle>
                  </asp:TemplateField>
                  <asp:TemplateField>
                      <ItemTemplate>
                          <asp:Label ID="lblNomrec" runat="server" Text='<%# Eval("nombreRech") %>'></asp:Label>                          
                      </ItemTemplate>
                      <ItemStyle CssClass="ancho230px"></ItemStyle>
                  </asp:TemplateField>           
                </Columns>
          
          </asp:GridView>
          <table class="tablaFootergrids anchoCompleto textocentradoh">
         <tr>
             <td>
               
             </td>
             <td class="ancho80px">                
                    
             </td>
         </tr>
     </table>
      </asp:Panel>   
      </asp:Panel> 
      </div>
</asp:Content>

