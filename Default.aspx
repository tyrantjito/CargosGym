<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Empresas</title>
    <link href="CSS/CSS.css" type="text/css" rev="Stylesheet" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="encabezado">        
    </div>
    <div class="centardo textocentradoh">
       <h1>Bienvenido a e-Bills</h1>
       <asp:Image ID="logo" runat="server" AlternateText="e-Bills" ImageUrl="~/IMG/logo.png" CssClass="imagenLogoGrande titulo" />
        <h2>Seleccione una Empresa para Continuar</h2>
        <asp:DataList ID="DataList1" runat="server" RepeatColumns="3" onitemdatabound="DataList1_ItemDataBound" CssClass="centardo">
            <ItemTemplate>                   
                <div class="empresas">
                    <asp:LinkButton ID="lblEmpresa" runat="server" ForeColor="#002c81" Text='<%# Bind("nombre") %>' CommandArgument='<%# Eval("id_empresa") %>' onclick="lblEmpresa_Click" CssClass="link" />
                    <br />   
                    <asp:ImageButton ID="logoEmpresas" runat="server" onclick="logoEmpresas_Click" 
                    ImageUrl='<%# "~/ImgEmpresas.ashx?id="+Eval("id_empresa") %>' CssClass="textoGris imagenesEmpresas" 
                    CommandArgument='<%# Eval("id_empresa") %>'/>
                </div>                                        
            </ItemTemplate>                       
        </asp:DataList>
    </div>
    </form>
</body>
</html>
