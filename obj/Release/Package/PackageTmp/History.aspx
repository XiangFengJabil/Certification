<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="History.aspx.cs" Inherits="Certification.History" %>

<%@ Register Src="~/Controls/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SkillSN Log</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="Styles/global.css" rel="stylesheet" type="text/css" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />

    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>

    <script>
        $(function () {
            $("#nav ul > li").eq(2).addClass("active").siblings().removeClass("active");
            //处理body顶部宽度

        });
    </script>
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />
    <div style="margin: 10px;">

        <div style="margin-bottom: 4px;">
            <form runat="server">
                <input type="hidden" runat="server" value='<%=handleTable %>' id="iptHandleTable" />
                <asp:DropDownList runat="server" ID="ddlHandleType" AutoPostBack="true" class="form-control" Style="width: 100px;" OnSelectedIndexChanged="ddlHandleType_SelectedIndexChanged">
                </asp:DropDownList>
            </form>
        </div>

        <table class="table table-bordered table-hover">
            <tr  class="success">
                <th>NTID</th>
                <th>DisplayName</th>
                <th>HandleType</th>
                <th>HandleTable</th>
                <th>HandleTableItem</th>
                <th>HandleDateTime</th>
            </tr>
            <asp:Repeater runat="server" ID="rptHandle">
                <ItemTemplate>
                    <tr>
                        <td><%# Eval("NTID") %></td>
                        <td><%# Eval("DisplayName") %></td>
                        <td><%# Eval("HandleType") %></td>
                        <td><%# Eval("HandleTable") %></td>
                        <td><%# Eval("HandleTableItem") %></td>
                        <td><%# Convert.ToDateTime(Eval("HandleDateTime")).ToString("yyyy-MM-dd HH:mm:ss") %></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>

    </div>
</body>
</html>
