<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecuperaContraseña.aspx.cs" Inherits="RecuperaContraseña" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Recuperar Contraseña</title>
    <link href="CSS/CSS.css" type="text/css" rev="Stylesheet" rel="stylesheet" />
</head>
<body>
<form id="form1" runat="server">
    <div class="encabezado">
        <table class="anchoCompleto">
            <tr>
                <td class="textoright">
                    <asp:Image ID="Image1" AlternateText="e-Bills" CssClass="imagenLogo titulo" ImageUrl="~/IMG/logo.png" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    <h2 class="ancho95">Recuperar Contrase&ntilde;a</h2>    
    <table class="anchoCompleto">
        <tr>
            <td align="center" class="ancho35Porc">
                <asp:Panel ID="Panel1" runat="server" CssClass="ancho35Porc pading8px pangrids sombra" BorderColor="#002c81">
                    <table class="anchoCompleto textocentradoh textobold">
                        <tr>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="Usuario: " ForeColor="#002c81"/>
                            </td>
                            <td>
                                <asp:TextBox ID="txtUsuario" runat="server" MaxLength="20" CssClass="textboxlog center" />
                                <asp:RequiredFieldValidator ControlToValidate="txtUsuario" CssClass="center" ID="frv4" ForeColor="Red" Text="*" runat="server" ErrorMessage="Necesita escribir el Usuario" ValidationGroup="contra" />
                                <asp:RegularExpressionValidator ID="rev1" runat="server" CssClass="center" ErrorMessage="El Usuario debe contener como minimo 3 y un maximo de 20 caracteres" ControlToValidate="txtUsuario" ValidationGroup="contra" ForeColor="Red" Text="*" ValidationExpression="[a-zA-Z0-9]{3,20}" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Text="Correo: " ForeColor="#002c81"/>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorreo" runat="server" MaxLength="500" CssClass="textboxlog center" />
                                <asp:RequiredFieldValidator ControlToValidate="txtCorreo" CssClass="center" ID="RequiredFieldValidator1" ForeColor="Red" Text="*" runat="server" ErrorMessage="Necesita indicar el correo electronico con el que se encuentra registrado en el sistema" ValidationGroup="contra" />                                
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblErrorRecContraseña" runat="server" ForeColor="Red"/>
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="contra" ForeColor="Red" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:ImageButton ID="btnConfirmar" runat="server" ValidationGroup="contra" ImageUrl="~/IMG/aceptar.png"
                                    CssClass="iconitos40"  onclick="btnConfirmar_Click" AlternateText="Recuperar Contraseña" ToolTip="Recuperar Contraseña" />
                                <asp:ImageButton ID="btnRegresar" runat="server" ImageUrl="~/IMG/back.png" AlternateText="Regresar" ToolTip="Regresar" PostBackUrl="~/Default.aspx" CssClass="iconitos40" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <asp:Label ID="lblContraseña" runat="server" ForeColor="#002c81"/>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</form>
</body>
</html>