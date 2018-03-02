<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login</title>
    <link href="CSS/CSS.css" type="text/css" rev="Stylesheet" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div class="encabezado">
                <table class="anchoCompleto">
                    <tr>
                        <td class="textoIzquierda textobold">
                            <asp:Label ID="lblEtiquetaEmpresa" runat="server" Text="Empresa:" CssClass="colorObscuro" />&nbsp;
                            <asp:Label ID="lblEmpresa" runat="server" CssClass="colorMedio" />
                        </td>
                        <td>
                            <asp:Image AlternateText="e-Bills" CssClass="imagenLogo titulo" ImageUrl="~/IMG/logo.png" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table class="divLog anchoCompleto">
                    <tr class="ancho35Porc centardo">
                        <td colspan="2" class="centardo ancho35Porc" align="center">
                            <asp:Panel ID="Panel1" runat="server" CssClass="ancho35Porc sombra">
                                <table class="anchoCompleto">
                                    <tr><td class="encabezadoLogin borderBottom"><asp:Label ID="Label1" runat="server" Text="Ingreso" /></td></tr>
                                    <tr>
                                        <td class="textobold textoIzquierda pading8px top padingleft">
                                            <asp:Label ID="Label2" runat="server" Text="Usuario:" /><br />
                                            <asp:Image ID="Image1" runat="server" ImageUrl="~/IMG/user.png" CssClass="iconitos40 center"/>&nbsp;
                                            <asp:TextBox ID="txtUsuarioLog" runat="server" CssClass="textboxlog ancho150px center" MaxLength="20" placeholder="Usuario" />
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ControlToValidate="txtUsuarioLog" ErrorMessage="Necesita llenar el usuario." />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ErrorMessage="El usuario debe un máximo de 20 caracteres." ControlToValidate="txtUsuarioLog" ValidationExpression="[a-zA-Z0-9]{3,20}" />
                                            </td>
                                        </tr>
                                    <tr >
                                        <td class="textobold textoIzquierda pading8px top padingleft">
                                            <asp:Label ID="Label3" runat="server" Text="Contraseña:" /><br />
                                            <asp:Image ID="Image2" runat="server" ImageUrl="~/IMG/pass.png" CssClass="iconitos40 center"/>&nbsp;
                                            <asp:TextBox ID="txtContraseñaLog" runat="server" CssClass="textboxlog ancho150px center" TextMode="Password" MaxLength="20" placeholder="Contraseña" />                                            
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ControlToValidate="txtContraseñaLog" ErrorMessage="Necesita llenar la contraseña." />
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" Text="*" ForeColor="Red" ValidationGroup="log" ErrorMessage="La contraseña debe contener como mínimo 5 y un máximo de 20 caracteres." ControlToValidate="txtContraseñaLog" ValidationExpression="[a-zA-Z0-9]{5,20}" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="textocentradoh">
                                            <asp:Label ID="lblErrorLog" runat="server" ForeColor="Red" />
                                            <asp:ValidationSummary ID="ValidationSummary1" ValidationGroup="log" ForeColor="Red" runat="server" DisplayMode="List" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="textocentradoh pading8px">
                                            <asp:ImageButton ID="btnBack" runat="server" CssClass="iconitos40" ImageUrl="~/IMG/back.png" Visible="false" onclick="btnBack_Click" /><br />
                                            <asp:Label ID="lblBack" runat="server" Text="Regresar" Visible="false" CssClass="colorMedio textobold"/>
                                        </td>
                                    </tr>
                                    <tr><td class="textocentradoh"><asp:LinkButton ID="lnkRecPas" runat="server" Text="¿Olvidaste tu Contraseña?" CssClass="link colorAzul" OnClick="lnkRecPas_Click" ></asp:LinkButton></td></tr>
                                    <tr ><td class="pieLogin textocentradoh borderTop textobold texto20pt"><asp:ImageButton ID="btnLog" runat="server" OnClick="btnLog_Click" ImageUrl="~/IMG/iniciar.png" AlternateText="Iniciar" ValidationGroup="log" /></td></tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>                
                    </table>
                <asp:Panel ID="Panel2" runat="server" Visible="false" CssClass="popupw">
                    <table class="anchoCompleto">
                        <tr>
                            <td class="pading8px textobold textocentradoh texto20pt colorAzul borderBottom"><asp:Label ID="Label4" runat="server" Text="Bienvenido"></asp:Label></td>
                        </tr>
                        <tr> 
                        <td class="textocentradoh pading8px">                       
                            <asp:Label ID="lblU" runat="server" CssClass="textobold colorNegro"></asp:Label></td>    
                        </tr>
                        <tr class="tablaFootergrids textocentradoh">
                            <td class="textocentradoh">
                                <asp:ImageButton ID="btnAceptar" runat="server" CssClass="iconitos40" 
                                    ImageUrl="~/IMG/aceptar.png" OnClick="btnAceptar_Click" ToolTip="Continuar" AlternateText="Continuar" />
                            </td>
                        </table>
                </asp:Panel>
                <asp:Panel ID="PanelMask" runat="server" Visible="false" CssClass="mask">
                </asp:Panel>
                </div>
                <asp:UpdateProgress ID="UpdateProgress1" runat="server">
                    <ProgressTemplate>
                        <div class="mask zen3">
                            <asp:Panel ID="PanGif" runat="server" CssClass="zen4 anchoCompleto center textocentradoh padingTop200px">
                                <asp:Image ID="gif" runat="server" ImageUrl="~/IMG/25-1.gif" AlternateText="Cargando. . ." Width="40%" />
                            </asp:Panel>
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
