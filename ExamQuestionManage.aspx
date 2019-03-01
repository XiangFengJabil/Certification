<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamQuestionManage.aspx.cs" Inherits="Certification.ExamQuestionManage" %>

<%@ Register Src="~/Controls/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>题库管理</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="Styles/common.css" rel="stylesheet" />
    <link href="Styles/global.css" rel="stylesheet" />
    <link href="Styles/Dialog.css" rel="stylesheet" />

    <script src="Scripts/jquery-1.9.1.min.js"></script>

    <script type="text/javascript">
        $(function () {
            //$("#mask").hide();
            //$("#login").hide();

            $("#nav ul > li").eq(2).addClass("active").siblings().removeClass("active");
            //处理body顶部宽度
            $("#Header").width(this.body.scrollWidth);

            $("#tabQuestionManage tr").mousemove(
                function () {
                    $(this).addClass("mouseover");
                }
            ).mouseout(function () {
                $(this).removeClass("mouseover");
            });

            //双击行查看信息
            $("#tbodyQuestionList>tr").dblclick(function () {
                var qID = $(this).children("td:eq(0)").html();

                $("#iptQID").val(qID);
                $("#iptOperationType").val(3);
                $("#QuestionTitle").text("考题信息");

                SetData(qID);
                SetDisabled();
            });

            $("#btnCancel,#close").click(function () {
                $("#mask").hide();
                $("#login").hide();
            });

            $("#btnSave").click(function () {
                //验证
                if ($("#txtQuestion").val() == "") { $("#txtQuestion").focus(); return; }
                if ($("#txtScore").val() == "") { $("#txtScore").focus(); return; }
                if ($("input[name='A']").val() == "") { $("input[name='A']").focus(); return; }

                if ($("#QuestionResult").val() == "") { $("#QuestionResult").focus(); return; }
                else {
                    //※€
                    var vA_Z = $("#QuestionResult").val().toUpperCase();
                    var vResult = "";

                    for (var i = 0; i < vA_Z.length; i++) {
                        $("#trlist>tr").each(function () {
                            if (vA_Z.substring(i, i + 1) == $(this).children("td:eq(0)").html()) {
                                vResult += $(this).children("td:eq(1)").find("input").val() + "※€";
                            }
                        });
                    }
                    if (vResult.length > 2) {
                        vResult = vResult.substring(0, vResult.length - 2)
                        $("input[name='QuestionResult']").val(vResult);
                    }

                }

                $("#SkillCN").attr("disabled", false);
                $("#btnSave").attr("disabled", "disabled");
                $("#formQuestionOptions").submit();
            });

            $("#tabQuestionManage tr>td>input").click(function () {
                var qID = $(this).attr("name");
                //0查看，1修改，2删除,3查看
                if ($(this).index() == 0) {
                    //查看
                    $("#iptOperationType").val(3);
                    $("#mask").show();
                    $("#login").show();
                    $("#divOperation").hide();
                    $("#QuestionTitle").text("考题信息");
                    SetData(qID);
                    SetDisabled();
                }
                else if ($(this).index() == 1) {
                    //修改事件
                    $("#iptOperationType").val(1);
                    $("#iptQID").val(qID);
                    $("#mask").show();
                    $("#login").show();
                    $("#QuestionTitle").text("修改考题");
                    SetData(qID);
                    RemoveDisabled();
                    $("#divOperation").show();
                }
                else if ($(this).index() == 2) {
                    //删除
                    $("#iptOperationType").val(2);
                    if (confirm("确定删除这道考题吗?")) {
                        $.post("ExamQuestionManage.aspx", { operationType: "2", qID: qID }, function (data) {
                            var v = eval(data);
                            if (v == "0") {
                                alert("删除失败!");
                            } else {
                                alert("删除成功!");
                                window.location.href = "ExamQuestionManage.aspx";
                            }
                        });
                    }

                }

            });

            $("#aAdd").click(function () {
                //添加事件
                $("#iptOperationType").val(0);
                $("#iptQID").val(0);
                $("#QuestionTitle").html("添加考题");
                $("#trlist>tr:gt(0)").remove();
                $("#trlist>tr:last").children("td:eq(1)").find("input").val("");
                $("#txtQuestion").val("");
                $("#txtScore").val("");
                $("#QuestionResult").val("");
                RemoveDisabled();
                if ($("#ddlQuestion").val() == "ALL") {
                    alert("请选择证书！");
                    return;
                } else {
                    $("#SkillCN").val($("#ddlQuestion").val()).attr("disabled", "disabled");
                }
                $("#mask").show();
                $("#login").show();
                $("#divOperation").show();
            });

            $("#addrow").click(function () {
                addrow();
            });

            //动态添加行
            var trlisthtml = $("#trlist").html();//获取默认的一行tr，用作复制
            function addrow() {//增加
                //字母转成数字：
                var lastRowA_Z = $("#trlist>tr:last").children("td:eq(0)").html();
                var num = lastRowA_Z.charCodeAt() + 1;

                $("#trlist:last").append(trlisthtml);//向tbody最后添加一行tr.

                //数字转换成字母：
                $("#trlist>tr:last").children("td:eq(0)").html(String.fromCharCode(num));
                $("#trlist>tr:last").children("td:eq(1)").find("input").attr("name", String.fromCharCode(num));

                $("#trlist>tr:last").children("td:eq(2)").find("input").show().click(function () {
                    //添加移除行事件
                    $(this).parent().parent().remove();
                    SetIDSeq();
                });
            }

            function SetIDSeq() {
                //从新计算答案顺序
                num = 65;
                $("#trlist>tr").each(function () {
                    $(this).children("td:eq(0)").html(String.fromCharCode(num));
                    $(this).children("td:eq(1)").find("input").attr("name", String.fromCharCode(num));
                    num++;
                });
            }

            function SetDisabled() {
                $("input[name=IsMultiple]").attr("disabled", "disabled");
                $("#txtQuestion").attr("disabled", "disabled");
                $("#txtScore").attr("disabled", "disabled");
                $("#QuestionResult").attr("disabled", "disabled");
                $("#SkillCN").attr("disabled", "disabled");

                $("#divQR").find(".required").hide();
                $("#trlist>tr").each(function () {
                    $(this).children("td:eq(1)").find("input").attr("disabled", "disabled");
                });
            }

            function RemoveDisabled() {
                $("input[name=IsMultiple]").removeAttr("disabled");
                $("#txtQuestion").removeAttr("disabled");
                $("#txtScore").removeAttr("disabled");
                $("#QuestionResult").removeAttr("disabled");
                $("#SkillCN").removeAttr("disabled");

                $("#divQR").find(".required").show();
                $("#trlist>tr").each(function (i) {
                    $(this).children("td:eq(1)").find("input").removeAttr("disabled");
                    if (i > 0)
                        $(this).children("td:eq(2)").find("input").show();
                });
            }

            //获取，设置查看，修改数据
            function SetData(questionID) {
                $("input[name='A']").val("");

                $.ajaxSettings.async = false;
                $.post("ExamQuestionManage.aspx", { operationType: "3", qID: questionID },
                    function (data) {
                        $("#mask").show();
                        $("#login").show();
                        $("#divOperation").hide();
                        $("#trlist>tr:gt(0)").remove();

                        var trCount = $("#trlist>tr").length;
                        var dataCount = $(data).length;

                        for (var i = trCount; i < dataCount; i++) {
                            addrow();
                        }

                        $(data).each(function (i, e) {
                            if (e.IsMultiple)
                                $("input:radio[value='true']").attr('checked', 'true');
                            else
                                $("input:radio[value='false']").attr('checked', 'true');

                            $("#txtQuestion").val(e.Question);
                            $("#txtScore").val(e.Score);
                            //$("#QuestionResult").val(e.QuestionResult);
                            $("input[name='QuestionResult']").val(e.QuestionResult);
                            $("#SkillCN").val(e.QuestionType);

                            $("#trlist>tr:eq(" + i + ")").children("td:eq(1)").find("input").val(e.QuestionOption);

                            if (i > 0) {
                                $("#trlist>tr:eq(" + i + ")").children("td:eq(2)").find("input").hide();
                            }

                        });

                    }, "json");

                //※€
                var vR = $("input[name='QuestionResult']").val();
                var vRL = vR.split("※€");
                var vResult = "";

                for (var i = 0; i < vRL.length; i++) {
                    $("#trlist>tr").each(function () {
                        if (vRL[i] == $(this).children("td:eq(1)").find("input").val()) {
                            vResult += $(this).children("td:eq(0)").html();
                        }
                    });
                }

                $("#QuestionResult").val(vResult);
            }


        });


    </script>

    <style type="text/css">
        .divTable {
            width: 99.6%;
        }

        .divSearch {
            padding-top: 4px;
            padding-left: 4px;
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

                .tableDefault tr td input {
                    width: 40px;
                }

        input {
            height: 20px;
        }

        select {
            height: 18px;
        }

        .divSearch {
            padding-top: 4px;
            padding-left: 4px;
        }

        .mouseover {
            background-color: gainsboro;
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

        #divQ span {
            display: inline-block;
            margin-left: 10px;
            margin-top: 4px;
        }

        #trlist tr td {
            padding: 2px 0px 2px 8px;
            vertical-align: middle;
        }

        #tabR {
            margin: 2px;
        }

        .required {
            margin-left: 4px;
            color: red;
        }
    </style>

</head>
<body>
    <div id="mask"></div>
    <uc1:Menu runat="server" ID="Menu" />
    <div id="divMain">
        <div id="login">
            <form id="formQuestionOptions" method="post" action="ExamQuestionManage.aspx">
                <input type="hidden" id="iptQID" name="qID" value="0" />
                <input type="hidden" id="iptOperationType" name="operationType" value="0" />
                <div id="divQR" style="padding: 4px;">
                    <span id="close" title="close">X</span>
                    <h2 id="QuestionTitle" style="margin-left: 6px; height: 32px; display: inline-block; *display: inline; zoom: 1;">Info</h2>
                    <span style="margin-left: 15px; color: #006699;">如果是多选题，答案例如ABC，ABC之间不需要有任何符号。</span>
                    <span style="display: block; margin-left: 10px;">问题:<input type="text" id="txtQuestion" name="Question" style="width: 90%; margin-left: 10px;" /><span class="required">*</span></span>
                    <div id="divQ">
                        <span>分数:<input onkeyup="(this.v=function(){this.value=this.value.replace(/[^0-9-]+/,'');}).call(this)" onblur="this.v();" type="text" id="txtScore" name="Score" style="width: 50px; margin-left: 10px;" />
                            <span class="required">*</span></span>
                        <span>正确答案:<input onkeyup="this.value=this.value.replace(/[^a-zA-Z]/g,'')" type="text" id="QuestionResult" style="width: 100px; text-transform: uppercase;" />
                            <input name="QuestionResult" type="hidden" />
                            <span class="required">*</span></span>

                        <select id="SkillCN" name="SkillCN" runat="server">
                        </select>

                        <span>是否选择题:<label style="margin-left: 10px;">是:<input type="radio" name="IsMultiple" value="true" checked="checked" /></label>
                            <label>
                                否:
                            <input type="radio" name="IsMultiple" value="false" /></label>
                        </span>

                    </div>
                    <table id="tabR" border="0">
                        <tbody id="trlist">
                            <tr>
                                <td style="width: 30px; text-align: center;">A</td>
                                <td>
                                    <input type="text" name="A" value="" style="width: 96%;" /><span class="required">*</span>
                                </td>
                                <td style="width: 10%;">
                                    <input type="button" class="removeBtn" style="width: 50px; height: 24px; display: none;" value="移除" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                    <div id="divOperation">
                        <div style="margin-left: 10px;">
                            <input type="button" id="addrow" value="新增答案" />
                        </div>
                        <input type="button" id="btnSave" value="保存" />
                        <input type="button" id="btnCancel" value="取消" />
                    </div>
                </div>
            </form>
        </div>

        <form id="form1" runat="server">
            <div class="divSearch">
                <span style="margin-right: 20px; float: right; display: inline; color: #005288;">题库中共有：<%=countActiveQuestion %>道题，已删除了<%=countDelQuestion %>道题(不计入题库中)。</span>

                <asp:Button runat="server" ID="btnSearchAll" Text="所有考题" OnClick="btnSearchAll_Click" />
                <span>证书 : </span>
                <asp:DropDownList ID="ddlQuestion" AutoPostBack="true" runat="server" OnSelectedIndexChanged="ddlQuestion_SelectedIndexChanged">
                </asp:DropDownList>
                <%= userRole==1?"<a id='aAdd' style='margin-left: 20px; color: #006699;' href='javascript:;'>添加考题</a>":"" %>
                <%= userRole==1?"<a style='margin-left: 20px; color: #006699;' href='TempDataImport.aspx'>批量添加题目、答案(测试使用)</a>":"" %>
            </div>
        </form>
        <div class="divTable">
            <table id="tabQuestionManage" class="tableDefault" border="0">
                <tr>
                    <th style="width: 40px;">ID</th>
                    <th>问题</th>
                    <th style="width: 80px;">所属证书</th>
                    <th>正确答案</th>
                    <th style="width: 40px;">分数</th>
                    <th style="width: 80px;">是否选择题</th>
                    <th style="text-align: center; width: 150px;">操作</th>
                </tr>
                <tbody id="tbodyQuestionList">
                    <asp:Repeater ID="rptExamQuestion" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("ID") %></td>
                                <td style="text-overflow: ellipsis; width: 40%;"><%# Eval("Question") %></td>
                                <td><%# Eval("QuestionType") %></td>
                                <td style="text-overflow: ellipsis;"><%# Eval("QuestionResult").ToString().Replace("※€",",") %></td>
                                <td><%# Eval("Score") %></td>
                                <td><%# ConvertString(Eval("IsMultiple")) %></td>
                                <td style="text-align: center;">
                                    <input type="button" name='<%# Eval("ID") %>' value="查看" />
                                    <input type="button" name='<%# Eval("ID") %>' value="修改" />
                                    <input type="button" name='<%# Eval("ID") %>' value="删除" /></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div>
    </div>

</body>
</html>
