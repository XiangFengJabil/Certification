<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OperationRecord.aspx.cs" Inherits="Certification.OperationRecord" %>

<%@ Register Src="~/Controls/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>实操记录</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="Styles/global.css" rel="stylesheet" />

    <script src="Scripts/jquery-1.9.1.min.js"></script>

    <style>
        .divTable {
            width: 99.6%;
        }

        .tableDefault {
            margin: 4px;
            padding: 4px;
            border: 1px;
            border-collapse: collapse;
            word-break: keep-all;
            white-space: normal;
        }

            .tableDefault tr th {
                padding-top: 2px;
                padding-bottom: 2px;
                color: White;
                background-color: #006699;
                font-weight: bold;
                border: 1px solid #A0A0A0;
                text-align: left;
                word-break: keep-all;
                white-space: nowrap;
            }

            .tableDefault tr td {
                border: 1px;
                padding: 2px;
                border: 1px solid #A0A0A0;
                word-break: keep-all;
                white-space: nowrap;
            }

        .txtUserRole {
            text-align: center;
            margin-left: 20px;
            color: #006699;
        }

        .mouseover {
            background-color: gainsboro;
        }
    </style>
    
    <script>
        $(function () {
            $("#nav ul > li").eq(4).addClass("active").siblings().removeClass("active");
            
            $(".tableDefault tr").mousemove(
                function () {
                    $(this).addClass("mouseover");
                }
            ).mouseout(function () {
                $(this).removeClass("mouseover");
            });

            $("#aOperationManage").click(function () {
                window.location.href = "OperationManage.aspx";
            });
        });


    </script>
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />
    <div style="padding: 4px;">
        <input type="hidden" value='<%=userRole%>' id="iptUserRole" />
        <form runat="server">
            <div runat="server" id="divRole" >
                <a href="javascript:;" class="txtUserRole" id="aOperationManage">实操管理</a>
            </div>
            <div>

                <asp:DropDownList ID="ddlSkillSN" Height="20" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSkillSN_SelectedIndexChanged">
                </asp:DropDownList>
                <span>姓名 :</span>
                <asp:TextBox runat="server" ID="txtUserName" Width="100" Height="18"></asp:TextBox>
                <asp:Button runat="server" ID="btnSearch" Text=" 查 询 " Height="20" OnClick="btnSearch_Click" />
            </div>
        </form>
    </div>

    <div class="divTable">
        <table class="tableDefault" cellpadding="0" border="0">
            <tr>
                <th>序号</th>
                <th>NTID</th>
                <th>姓名</th>
                <th>部门</th>
                <th>SkillSn</th>
                <th>考试分</th>
                <th>考试时间</th>
                <th>实操分</th>
                <th>精益分</th>
                <th>其它分</th>
                <% if (userRole == 1) Response.Write("<th>实操评分</th>"); %>
            </tr>
            <asp:Repeater runat="server" ID="rptOperationItem">
                <ItemTemplate>
                    <tr class="table-hover">
                        <td><%# Container.ItemIndex+1 %></td>
                        <td><%# Eval("NTID") %></td>
                        <td><%# Eval("DisplayName") %></td>
                        <td><%# Eval("Department") %></td>
                        <td><%# Eval("SkillSn") %></td>
                        <td><%# Eval("ExamTotalScore") %></td>
                        <td><%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd HH:mm:ss") %></td>
                        <td><%# Eval("OperationTotalScore") %></td>
                        <td><%# Eval("LeanTotalScore") %></td>
                        <td><%# Eval("OtherTotalScore") %></td>
                        <%# OutHtmlTd(Eval("id"),Eval("IsApproved")) %>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>

</body>
</html>
