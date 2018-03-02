<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CambiarContraseña.aspx.cs" Inherits="CambiarContraseña" MasterPageFile="~/MasterMenus.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <h2 class="anchoCompleto">Cambiar Contrase&ntilde;a</h2>
    <table class="anchoCompleto padingTop">
        <tr>
            <td align="center">
                <asp:Panel ID="Panel1" runat="server" CssClass="ancho35Porc pading8px pangrids sombra textobold" BorderColor="#002c81">
                    <table>
                        <tr>
                            <td style="color:#002c81">
                                Contrase&ntilde;a Actual:&nbsp;&nbsp;<asp:Label ID="lblContraseña" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="center" style="color:#002c81">
                                Nueva Contrase&ntilde;a:&nbsp;&nbsp;<asp:TextBox ID="txtContraseña" runat="server" MaxLength="20" TextMode="Password" CssClass="textboxlog center" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Necesita colocar la nueva contraseña" ControlToValidate="txtContraseña" ValidationGroup="contraseña" Text="*" ForeColor="Red" CssClass="center" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="La contraseña debe contener como mínimo 5 y un míximo de 20 caractéres" ControlToValidate="txtContraseña" ValidationGroup="contraseña" ForeColor="Red" Text="*" ValidationExpression="[a-zA-Z0-9]{5,20}" CssClass="center"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblErrorContraseña" runat="server" ForeColor="Red" />
                                <asp:ValidationSummary ID="vs" DisplayMode="List" ForeColor="Red" runat="server" ValidationGroup="contraseña" />
                            </td>
                        </tr>
                        <tr>
                            <td class="textocentradoh">
                                <asp:ImageButton ID="btnConfirmar" runat="server" AlternateText="Aceptar" ImageUrl="~/IMG/aceptar.png"
                                    ValidationGroup="contraseña" CssClass="iconitos40" onclick="btnConfirmar_Click" ToolTip="Aceptar" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
