<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DeptHead.aspx.cs" Inherits="Certification.DeptHead" %>

<%@ Register Src="~/Controls/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Dept Head List</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="Styles/global.css" rel="stylesheet" type="text/css" />
    <link href="Styles/common.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-1.9.1.min.js"></script>

    <style type="text/css">
        body, html {
            padding: 0;
            margin: 0;
        }

        .divTable {
            width: 99.6%;
        }

        .tableDefault {
            width: 100%;
            margin: 4px;
            padding: 4px;
            border: 1px;
            border-collapse: collapse;
        }

            .tableDefault tr th {
                color: White;
                background-color: #006699;
                font-weight: bold;
                border: 1px solid #A0A0A0;
                text-align: left;
            }

            .tableDefault tr td {
                border: 1px;
                padding: 2px;
                border: 1px solid #A0A0A0;
            }

        input {
            height: 24px;
        }

        select {
            height: 24px;
        }

        .divSearch {
            padding-top: 4px;
            padding-left: 4px;
        }

        .mouseover {
            background-color: gainsboro;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            $("#nav ul > li").eq(3).addClass("active").siblings().removeClass("active");

            $("#tabDeptHead tr").mousemove(
                function () {
                    $(this).addClass("mouseover");
                }
            ).mouseout(function () {
                $(this).removeClass("mouseover");
            });

        });
    </script>
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />

    <div id="divMain">
        <form id="form1" runat="server">
            <div class="divSearch">
                <span>部门 : </span>
                <asp:DropDownList ID="ddl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_SelectedIndexChanged">
                </asp:DropDownList>
                <span runat="server" id="spanLiable" class="txtUserRole" style="margin-left: 10px;">Upload Person Liable :</span>
                <asp:FileUpload ID="FileUploadDeptHead" class="txtUserRole" runat="server" Visible="False" />
                <asp:Button ID="btnUploadDeptHead" class="txtUserRole" runat="server" Text=" Upload Person Liable " OnClick="btnUploadDeptHead_Click" Visible="False" />
            </div>
        </form>
        <div class="divTable">
            <table id="tabDeptHead" class="tableDefault" border="0">
                <tr>
                    <th>部门</th>
                    <th>姓名</th>
                    <th>工号</th>
                    <th>NTID</th>
                    <th>邮箱</th>
                    <th>是否责任人</th>
                </tr>
                <asp:Repeater ID="rptDeptHead" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%# Eval("部门") %></td>
                            <td><%# Eval("姓名") %></td>
                            <td><%# Eval("工号") %></td>
                            <td><%# Eval("NTID") %></td>
                            <td><%# Eval("邮箱") %></td>
                            <td><%# Eval("是否责任人") %></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
        </div>
    </div>
</body>
</html>
