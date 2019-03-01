<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OperationManage.aspx.cs" Inherits="Certification.OperationManage" %>

<%@ Register Src="~/Controls/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>实操信息管理</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />

    <link href="Styles/global.css" rel="stylesheet" />
    <link href="Styles/Dialog.css" rel="stylesheet" />


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

        .mouseover {
            background-color: gainsboro;
        }

        .imgEdit {
            width: 20px;
            height: 20px;
            text-align: center;
            cursor: pointer;
            margin-top: 2px;
            margin-bottom: -2px;
        }

        .imgDel {
            width: 20px;
            height: 20px;
            text-align: center;
            cursor: pointer;
            margin-top: 2px;
            margin-bottom: -2px;
        }

        #frmOperation input {
            height: 24px;
            margin: 2px;
            padding-left: 2px;
            padding-right: 2px;
            min-width: 80px;
            min-height: 24px;
        }

        #frmOperation table {
            margin: 8px auto 8px auto;
        }


        #divOperation {
            width: 100%;
            text-align: center;
            margin: 10px auto 10px auto;
        }

            #divOperation input {
                width: 80px;
                height: 24px;
                margin: auto 36px auto 36px;
            }
    </style>


    <script>
        $(function () {
            $.ajax.async = false;

            $(".tableDefault tr").mousemove(
                function () {
                    $(this).addClass("mouseover");
                }
            ).mouseout(function () {
                $(this).removeClass("mouseover");
            });

            $("#nav ul > li").eq(4).addClass("active").siblings().removeClass("active");

            //0添加
            $("#aAdd").click(function () {
                if ($("#ddlOpeartionType").val() == "ALL")
                    $("#operationType").val("");
                else
                    $("#operationType").val($("#ddlOpeartionType").val());

                $("#oType").val("0");
                $("#title").html("添加实操信息");
                RemoveDisabled();
                ShowDialog();

            });

            //3查看
            $("#tbody > tr").dblclick(function () {
                $("#divOperation").hide();
                $("#trlist>tr:gt(0)").remove();
                $("#operationType").val($(this).children("td:eq(2)").html());
                GetData($(this).children("td:eq(1)").html());
                SetDisabled();
                $("#title").html("查看信息");
                ShowDialog();
            });

            //1删除
            $(".imgDel").click(function () {
                $("#oType").val("1");
                var oRow = $(this).parent().parent();

                var operationItemID = $(this).parent().parent().children("td:eq(1)").html();
                //var operationType = $(this).parent().parent().children("td:eq(2)").html();
                if (confirm("您确定删除该条考核项目吗?删除后将会连同评分标准一起删除!")) {

                    $.ajax({
                        type: "post",
                        url: "OperationManage.aspx?oType=1&operationItemID=" + operationItemID,
                        success: function (data) {
                            var v = data.toString();
                            if (v == "0")
                                alert("删除失败!");
                            else {
                                alert("删除成功!");
                                $("#tbody>tr:eq(" + $(oRow).index() + ")").remove();
                                window.location.href = "OperationManage.aspx";
                                //alert($(oRow).index()); return;
                            }

                        }

                    });
                }
            });

            //2修改
            $(".imgEdit").click(function () {
                $("#oType").val("2");
                $("#operationItemID").val($(this).parent().parent().children("td:eq(1)").html());
                $("#operationType").val($(this).parent().parent().children("td:eq(2)").html());
                $("#title").text("修改实操信息");
                RemoveDisabled();
                ShowDialog();
                GetData($(this).parent().parent().children("td:eq(1)").html());
                $("#divOperation").show();

            });

            //保存事件
            $("#btnSave").click(function () {

                //验证
                if ($("#txtQuestion").val() == "") { $("#txtQuestion").focus(); return; }
                if ($("#txtScore").val() == "") { $("#txtScore").focus(); return; }
                if ($("input[name='A']").val() == "") { $("input[name='A']").focus(); return; }

                if ($("#QuestionResult").val() == "") { $("#QuestionResult").focus(); return; }

                $("#btnSave").attr("disabled", "disabled");

                $("#frmOperation").submit();
            });

            //取消,关闭事件
            $("#btnCancel,#close").click(function () {
                $("#mask").hide();
                $("#login").hide();
            });

            //新增行事件
            $("#addrow").click(function () {
                addrow();
            });

            //动态添加行
            var trlisthtml = $("#trlist").html();//获取默认的一行tr，用作复制
            function addrow() {//增加
                var num = $("#trlist > tr").length;

                $("#trlist:last").append(trlisthtml);//向tbody最后添加一行tr.
                //设置序号
                $("#trlist>tr:last").children("td:eq(0)").html(num + 1);

                $("#trlist>tr:last").children("td:eq(2)").find("input").show().click(function () {
                    //添加移除行事件
                    $(this).parent().parent().remove();
                    SetSeq();
                });
                SetSeq();


            }

            //获取弹出层的数据
            function GetData(operationItemID) {
                $.ajaxSettings.async = false;
                $.ajax({
                    type: "POST",
                    url: "OperationManage.aspx",
                    data: "oType=3&operationItemID=" + operationItemID,
                    success: function (data) {
                        if (data.toString() == "0")
                            alert("数据获取失败,请稍后在试!");
                        else {
                            var dataJson = $.parseJSON(data);
                            $("#trlist>tr:gt(0)").remove();

                            var trCount = $("#trlist>tr").length;
                            var dataCount = $(dataJson).length;

                            for (var i = trCount; i < dataCount; i++) {
                                addrow();
                            }

                            $(dataJson).each(function (i, e) {
                                $("#iptOperationItem").val(e.OperationItem);
                                $("#iptScore").val(e.OperationScore);
                                $("#trlist>tr:eq(" + i + ")").children("td:eq(1)").find("input").val(e.OperationStandard);
                            });
                        }
                    }
                });
            }

        });

        function ShowDialog() {
            $("#mask").show();
            $("#login").show();
        }

        function SetDisabled() {
            $("#iptOperationItem").attr("disabled", true);
            $("#iptScore").attr("disabled", true);
            $("#operationType").attr("disabled", true);

            $("#trlist>tr").each(function () {
                $(this).children("td:eq(1)").find("input").attr("disabled", "disabled");
                $(this).children("td:eq(2)").find("input").hide();
            });
        }

        function RemoveDisabled() {
            $("#iptOperationItem").removeAttr("disabled");
            $("#iptScore").removeAttr("disabled");
            $("#operationType").removeAttr("disabled");


            $("#trlist>tr").each(function (i) {
                $(this).children("td:eq(1)").find("input").removeAttr("disabled");
                if (i > 0)
                    $(this).children("td:eq(2)").find("input").show();
            });
        }

        function SetSeq() {
            $("#trlist>tr").each(function (i) {
                $(this).children("td:eq(0)").html(i + 1);
                $(this).children("td:eq(1)").find("input").attr("name", "txtOperationStandard" + (i + 1));
            });
        }


    </script>

</head>

<body>
    <div id="mask"></div>
    <uc1:Menu runat="server" ID="Menu" />

    <div id="divMain">

        <div id="login" style="padding: 4px;">
            <span id="close" title="close">X</span>
            <h2 id="title">查看信息</h2>
            <form id="frmOperation" action="OperationManage.aspx">
                <input type="hidden" value="" name="oType" id="oType" />
                <input type="hidden" value="" name="operationItemID" id="operationItemID" />
                <div>
                    <table>
                        <tr>
                            <td>考核项目:</td>
                            <td colspan="2">
                                <input type="text" value="" name="operationType" id="operationType" /></td>
                        </tr>
                        <tr>
                            <td>具体内容:</td>
                            <td colspan="2">
                                <input id="iptOperationItem" style="width: 86%;" type="text" name="operationItem" /><span style="color: red; margin-left: 4px;">*</span></td>
                        </tr>
                        <tr>
                            <td>分数:</td>
                            <td colspan="2">
                                <input onkeyup="(this.v=function(){this.value=this.value.replace(/[^0-9-]+/,'');}).call(this)" onblur="this.v();" type="text" id="iptScore" name="Score" style="width: 50px;" maxlength="5" /><span style="color: red; margin-left: 4px;">*</span></td>
                        </tr>
                        <tr>
                            <td colspan="3">评分标准:
                            </td>
                        </tr>

                        <tbody id="trlist">
                            <tr style="margin: 10px;">
                                <td style="width: 70px; text-align: center;">1</td>
                                <td>
                                    <input type="text" style="width: 94%;" name="txtOperationStandard1" value="" /></td>
                                <td style="width: 80px;">
                                    <input style="display: none; height: 24px;" onclick="SetSeq()" type="button" value=" 移 除 " /></td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <div id="divOperation">
                    <div style="margin-left: 2px;">
                        <input type="button" id="addrow" value="新增评分标准" />
                    </div>
                    <div style="text-align: center;">
                        <input type="button" id="btnSave" value="保存" />
                        <input type="button" id="btnCancel" value="取消" />
                    </div>
                </div>
            </form>
        </div>

        <div style="padding: 4px 0 0 0;">
            <a id="aAdd" style="margin-left: 20px; color: #006699;" href="javascript:;">添加实操信息</a>
        </div>

        <div style="padding: 4px;">
            <form runat="server">
                <asp:DropDownList ID="ddlOpeartionType" Height="20" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlOpeartionType_SelectedIndexChanged">
                </asp:DropDownList>
            </form>
        </div>

        <div class="divTable">
            <table class="tableDefault" cellpadding="0" border="0">
                <tr>
                    <th>序号</th>
                    <th style="display: none;">ID</th>
                    <th>考核项目</th>
                    <th>具体内容</th>
                    <th>评分标准(条数)</th>
                    <th>分数</th>
                    <th style="text-align: center;">操作</th>
                </tr>
                <tbody id="tbody">
                    <asp:Repeater runat="server" ID="rptOperationManage">
                        <ItemTemplate>
                            <tr>
                                <td><%# Container.ItemIndex+1 %></td>
                                <td style="display: none;"><%# Eval("ID") %></td>
                                <td><%# Eval("OperationType") %></td>
                                <td><%# Eval("OperationItem") %></td>
                                <td><%# Eval("COUNTITEM") %></td>
                                <td><%# Eval("OperationScore") %></td>
                                <td style="text-align: center; width: 100px;">
                                    <img title='Edit <%# Eval("ID") %>' class="imgEdit" src="images/exam_edit_48_48.png" />
                                    <img title='Delete <%# Eval("ID") %>' class="imgDel" src="images/exam_delete_24_24.png" />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>

    </div>

</body>
</html>
