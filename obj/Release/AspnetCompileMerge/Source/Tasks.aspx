<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tasks.aspx.cs" Inherits="Certification.Tasks" %>

<%@ Register Src="Controls/menu.ascx" TagName="menu" TagPrefix="uc1" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Certification List</title>
    <link href="Styles/global.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <uc1:menu ID="menu1" runat="server" />
    <p class="LocationTag t-l">
        <asp:Label ID="lbLocationTag" runat="server">Home >> My List</asp:Label>
    </p>
    <form id="form1" runat="server">
        <div id="tablebody" style="overflow-y: auto; overflow-x: hidden; width:99%; margin:0 auto;">
            <asp:GridView ID="dgvRequests" runat="server" CssClass="dataTable" AutoGenerateColumns="False" EnableViewState="False">
                <Columns>
                    <asp:TemplateField HeaderText="ID">
                        <ItemTemplate>
                            <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# string.Format("~/Request.aspx?reqid={0}&taskId={1}",Eval("ReqID"),Eval("Sno")) %>' Text='<%# Eval("ReqID", "{0:d6}") %>'></asp:HyperLink>
                        </ItemTemplate>
                        <HeaderStyle Width="120px" />
                    </asp:TemplateField>
                    <asp:BoundField DataField="SiteName" HeaderText="Site" SortExpression="SiteName">
                        <HeaderStyle Width="80px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="FormName" HeaderText="Form" SortExpression="FormName">
                        <HeaderStyle Width="40%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ExpectDay" HeaderText="ExpectDay" DataFormatString="{0:d}"
                        SortExpression="ExpectDay">
                        <HeaderStyle Width="120px"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Requestor" HeaderText="Requestor" SortExpression="Requestor">
                        <HeaderStyle Width="15%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status">
                        <HeaderStyle Width="20%"></HeaderStyle>
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
