<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OperationRecord.aspx.cs" Inherits="Certification.OperationRecord" %>

<%@ Register Src="~/Controls/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<!DOCTYPE html>

<html lang="zh-CN" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>实操记录</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache" />
    <meta http-equiv="Expires" content="0" />

    <link href="Styles/global.css" rel="stylesheet" type="text/css" />
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <link href="Scripts/jquery-ui-1.12.1/jquery-ui.css" rel="stylesheet" />

    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/jquery-ui-1.12.1/jquery-ui.min.js"></script>


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
                text-align: center;
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

        function getData(requestType) {
            var val = "";
            $.ajax({
                type: "GET",
                url: "Handler/GetAutocompleteData.ashx",
                async: false,
                data: "requestType=" + requestType,
                success: function (data) {
                    if (data != "" && data != undefined)
                        val = data;
                }
            });
            return val;
        }

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

            $("#ddlTotal").attr("title", "ALL(包括在线考试合格、实操已评分、未评分)，未评分(在线考试合格，实操未评分)");


            var jsonName = JSON.parse(getData("OperationRecordDisplayName"));
            var arrName = new Array();
            $.each(jsonName, function (i, e) {
                arrName.push(e.DisplayName);
            });

            $("#txtUserName").autocomplete({ source: arrName, autoFocus: true, minLength: 1, delay: 300 });


        });


    </script>
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />
    <div style="padding: 4px;">
        <form runat="server" class="form-inline">
            <div runat="server" id="divRole">
                <a href="javascript:;" class="btn btn-primary" id="aOperationManage">实操管理</a>
                <a href="javascript:;" class="btn btn-primary" data-toggle='modal' data-target='#divTheoryExamFailed'>理论不及格</a>
                <%--<a href="javascript:;" class="btn btn-primary" id="aApprovalHistory">审批记录</a>--%>
                <div class="form-group" style="margin-top: 4px;">
                    <label>考核结果：</label>
                    <asp:DropDownList ID="ddlTotal" CssClass="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlTotal_SelectedIndexChanged">
                        <asp:ListItem Value="0">ALL</asp:ListItem>
                        <asp:ListItem Value="1">合格</asp:ListItem>
                        <asp:ListItem Value="2">优秀</asp:ListItem>
                        <asp:ListItem Value="3">不合格</asp:ListItem>
                        <asp:ListItem Value="4">未评分</asp:ListItem>
                    </asp:DropDownList>

                    <label>证书查询：</label>
                    <asp:DropDownList ID="ddlSkillSN" CssClass="form-control" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlSkillSN_SelectedIndexChanged">
                    </asp:DropDownList>
                    <label>姓名查询：</label>
                    <asp:TextBox runat="server" ID="txtUserName" CssClass="form-control"></asp:TextBox>
                    <asp:Button runat="server" ID="btnSearch" Text="查询" CssClass="btn btn-default" OnClick="btnSearch_Click" />


                </div>
                <div style="margin-top: 10px; float: right;">
                    <span>最终得分 = 理论得分 X 15% + 实际操作得分 X 50% + 精益分 X 20% + 其它分 X 15% <span style="color: red;">< 73 不合格</span><span style="color: orange;"> 73~87.5 合格</span> <span style="color: green;">> 87.5 优秀</span><span style="margin-right: 10px; float: right;">理论得分 >= 80 分才会显示到此界面</span></span>
                </div>
            </div>
        </form>
    </div>

    <%--TheoryExamFailed modal--%>
    <div id="divTheoryExamFailed" class="modal fade" tabindex="-1" role="dialog" aria-hidden="true">
        <div class="modal-dialog modal-lg ">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title" id="dialogAddOrUpdateTitle">理论不级格</h4>
                </div>
                <div class="modal-body">
                    <table class="table table-hover table-bordered">
                        <tr>
                            <td>序号</td>
                            <td>NTID</td>
                            <td>姓名</td>
                            <td>部门</td>
                            <td>SkillSN</td>
                            <td>理论得分</td>
                            <td>考试时间</td>
                        </tr>
                        <asp:Repeater runat="server" ID="rptTheoryExamFailed">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Container.ItemIndex+1 %></td>
                                    <td><%# Eval("NTID") %></td>
                                    <td><%# Eval("DisplayName") %></td>
                                    <td><%# Eval("Department") %></td>
                                    <td><%# Eval("SkillSn") %></td>
                                    <td><%# Eval("ExamTotalScore") %></td>
                                    <td><%# Convert.ToDateTime(Eval("CreateDate")).ToString("yyyy-MM-dd") %></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </table>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                </div>
            </div>
        </div>
    </div>
    <%--end TheoryExamFailed--%>

    <div class="divTable">
        <table class="tableDefault" cellpadding="0" border="0">
            <tr>
                <th>序号</th>
                <th>NTID</th>
                <th>姓名</th>
                <th>部门</th>
                <th>SkillSN</th>
                <th>理论得分</th>
                <th>考试时间</th>
                <th>实操得分</th>
                <th>精益得分</th>
                <th>其它得分</th>
                <th>最终得分</th>
                <%= (userRole == 1)?"<th style='text-align:center;'>考核结果</th>":"" %>
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
                        <td><%# Eval("TotalScore") %></td>
                        <%# OutHtmlTd(Eval("id"),Eval("IsOperationApproved")) %>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
        </table>
    </div>

</body>
</html>
