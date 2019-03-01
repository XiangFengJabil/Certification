<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Exam.aspx.cs" Inherits="Certification.Exam" %>

<%@ Register Src="~/Controls/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<!DOCTYPE html>

<html lang="zh-CN" xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ON-Line Exam System</title>
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


    <style type="text/css">
        .imgExam {
        }

        .imgEdit {
        }

        .imgDelete {
        }

        .textAlignCenter {
            text-align: center;
        }

        .mouseOver {
            border-color: #005288;
            background-color: azure;
            font-weight: 700;
        }
    </style>

    <script type="text/javascript">        

        function AddOrUpdate() {
            var handel = $("#iptHandle").val();
            $("#divOperation").show();
            if (handel == "0") {
                $("#iptSkillSN").val("")
                $("#iptSkillDescription").val("")
                $("#dialogAddOrUpdateTitle").html("添加证书");
            } else if ((handel == "1")) {
                $("#dialogAddOrUpdateTitle").html("修改证书");
            }
        }

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
            $("#nav ul > li").eq(2).addClass("active").siblings().removeClass("active");

            //操作记录
            $("#aHandleHistory").click(function () {
                window.location.href = "History.aspx?handle=Skill";
            });



            //考试事件
            $(".imgExam").click(function () {
                var skillSn = $.trim($(this).parent().find("input[type=hidden]").val());

                window.location.href = "ExamOline.aspx?id=" + $.trim(skillSn);

            });

            //0添加
            $("#aAddCertification").click(function () {
                $("#iptHandle").val(0);
                AddOrUpdate();

            });

            //1修改
            $(".imgEdit").click(function () {
                var oldSkillSn = $.trim($(this).parent().find("input[type=hidden]").val());
                $("#oldSkillSN").val(oldSkillSn);
                $.ajaxSettings.async = false;
                $.post("Exam.aspx", { iptHandle: "3", SKillSN: oldSkillSn },
                    function (data) {
                        $(data).each(function (i, e) {
                            $("#iptSkillSN").val(e.SkillSn);
                            $("#iptSkillDescription").val(e.SkillDescription);
                        });

                    }, "json");

                $("#iptHandle").val(1);
                AddOrUpdate();

            });

            //2删除
            $(".imgDelete").click(function () {
                var skillSn = $.trim($(this).parent().find("input[type=hidden]").val());
                if (!confirm("您确定删除 【" + skillSn + "】 该证书吗？ 【" + $.trim(skillSn) + "】 证书下的考题将会一并删除。"))
                    return;
                else {
                    var div = $(this).parent().parent().parent();
                    $.post("Exam.aspx", { iptHandle: "2", SKillSN: skillSn },
                        function (data) {
                            if (data == "0") {
                                alert("出错了，删除失败！");
                            }
                            else {
                                alert("删除成功！");
                                $(div).remove();
                            }
                        });

                }
            });

            //保存。增加、修改共用一个事件，修改的时候需要原来的SkillSN
            $("#btnSave").click(function () {
                var handel = $("#iptHandle").val();
                var skillSN = $("#iptSkillSN").val();
                var oldSkillSN = $("#oldSkillSN").val();

                var skillDescription = $("#iptSkillDescription").val();

                if (skillSN == "") { $("#iptSkillSN").focus(); return; }

                $.ajaxSettings.async = false;
                $.post("Exam.aspx", { SKillSN: skillSN, OldSkillSN: oldSkillSN, SkillDescription: skillDescription, iptHandle: handel },
                    function (data) {
                        if (data == "0") {
                            if (handel == "0")
                                alert("出错了，【证书已存在】或者【存在已被删除】，添加失败！");
                            else
                                alert("出错了，【证书已存在】或者【存在已被删除】，修改失败！");
                        } else {
                            if (handel == "0")
                                alert("添加成功！");
                            else
                                alert("修改成功！");
                            window.location.href = "Exam.aspx";
                        }
                    });
            });

            //autocomplete
            var jsonName = JSON.parse(getData("ExamSkillSN"));
            var arrName = new Array();
            $.each(jsonName, function (i, e) {
                arrName.push(e.SkillSn);
            });

            $("#txtSkillSN").autocomplete({ source: arrName, autoFocus: false, minLength: 1, delay: 300 });

            //thumbnail
            $(".thumbnail").mouseover(function () {
                $(this).addClass("mouseOver");
            }).mouseout(function () {
                $(this).removeClass("mouseOver");
            });



        });

    </script>

</head>
<body>

    <div class="modal fade" id="divAddOrUpdateSkillSN" tabindex="-1" role="dialog" aria-labelledby="dialogAddOrUpdateTitle" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title" id="dialogAddOrUpdateTitle"></h4>
                </div>
                <div class="modal-body">
                    <input type="hidden" id="iptHandle" name="iptHandle" value="" />
                    <input type="hidden" id="oldSkillSN" />

                    <div class="form-group">
                        <label for="iptSkillsn" class="col-sm-3">证书SN：<span style="color: red">*</span></label>
                        <input id="iptSkillSN" class="form-control" type="text" maxlength="50" />
                        <label for="iptSkillDescription" class="col-sm-3">证书描述：</label>
                        <input id="iptSkillDescription" class="form-control" type="text" maxlength="500" />
                    </div>

                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                    <button id="btnSave" type="button" class="btn btn-primary">提交</button>
                </div>
            </div>
        </div>
    </div>

    <uc1:Menu runat="server" ID="Menu" />
    <div id="divMain">

        <div style="padding: 4px;">
            <form runat="server" class="form-inline">
                <a href="javascript:;" title="只显示最后考试20次的记录" class="btn btn-primary" id="aExamHistory" data-toggle="modal" data-target="#divExamHistory">考试记录</a>
                <%= userRole == 1?"<a id='aAddCertification'  data-toggle='modal' data-target='#divAddOrUpdateSkillSN' class='btn btn-primary' href='javascript:;'>添加证书</a>":"" %>
                <%= userRole == 1?" <a class='btn btn-primary' href='ExamQuestionManage.aspx'>题库管理</a>":"" %>
                <%= userRole == 1?"<a href='javascript:;' class='btn btn-primary' id='aHandleHistory'>操作记录</a>":"" %>

                <div class="form-group">
                    <label>SkillSN :</label>
                    <asp:TextBox runat="server" ID="txtSkillSN" CssClass="form-control"></asp:TextBox>
                    <asp:Button runat="server" ID="btnSearch" Text=" 查 询 " CssClass="btn btn-default" OnClick="btnSearch_Click" />
                </div>

                <span style="color: red; float: right; margin: 10px 10px 0 0;">注意：修改或删除证书，会连同考题一起修改或删除，请谨慎操作。</span>
                <span style="margin: 10px 20px 0 0; float: right; color: #005288;">共有：<%=countActiveSkillSN %>个证书，已删除了<%=countDelSkillSN %>个证书(不计入证书中)。</span>
            </form>

            <!-- Exam History Modal -->
            <div class="modal fade" id="divExamHistory" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true">
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title" id="myModalLabel">考试记录</h4>
                        </div>
                        <div class="modal-body">
                            <table class="table table-hover table-bordered">
                                <tr>
                                    <th>证书SN</th>
                                    <th>得分</th>
                                    <th>考试时间</th>
                                    <th>详情记录</th>
                                </tr>
                                <asp:Repeater ID="rptExamHistory" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("SkillSn") %></td>
                                            <td><%# Eval("SUMSCORE") %></td>
                                            <td><%# Convert.ToDateTime(Eval("ExamDate")).ToString("yyyy-MM-dd") %></td>
                                            <td><a class="btn btn-primary btn-sm" role="button" href='ExamResultDetail.aspx?ExamDate=<%# Convert.ToDateTime(Eval("ExamDate")).ToString("yyyy-MM-dd") %>&skillsn=<%# Eval("SkillSn") %>'>查看</a></td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </table>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">关闭</button>
                        </div>
                    </div>
                    <!-- /.modal-content -->
                </div>
                <!-- /.modal-dialog -->
            </div>
            <!-- /.modal -->
        </div>

        <div class="row" style="padding: 10px 20px 20px 20px;">
            <asp:Repeater runat="server" ID="rptSkill">
                <ItemTemplate>

                    <div class="col-sm-2 col-md-2">
                        <div class="thumbnail">
                            <div class="caption textAlignCenter">
                                <h3><%# Eval("SkillSn") %></h3>
                                <p title='<%# Eval("SkillDescription") %>' style="overflow: hidden; text-overflow: ellipsis; white-space: nowrap;"><%# Eval("SkillDescription") %></p>
                                <p>
                                    <span>题数:<%# Eval("QCOUNT") %></span>
                                    <span style="margin-left: 20px;">分数:<%# Eval("QSUM") %></span>
                                </p>
                                <p>
                                    <input type="hidden" value='<%# Eval("SkillSn") %>' />
                                    <a style="background-color: #009688; color: white;" href='javascript:;' class='btn imgExam' role='button'>考试</a>
                                    <%# userRole == 1 ? "<a href='javascript:;'  data-toggle='modal' data-target='#divAddOrUpdateSkillSN' class='btn btn-warning imgEdit' role='button'>修改</a> <a title='Delete " + Eval("SkillSn") + "' href='javascript:;' class='btn btn-danger imgDelete' role='button'>删除</a>" : "" %>
                                </p>
                            </div>
                        </div>
                    </div>

                </ItemTemplate>
            </asp:Repeater>
        </div>

    </div>

</body>
</html>
