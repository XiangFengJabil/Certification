<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamResultDetail.aspx.cs" Inherits="Certification.ExamResultDetail" %>

<%@ Register Src="~/Controls/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ON-Line Exam Detail</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="Styles/global.css" rel="stylesheet" type="text/css" />
    <link href="Styles/Dialog.css" rel="stylesheet" />

    <script src="Scripts/jquery-1.9.1.min.js"></script>

    <link href="Scripts/layui/css/layui.css" rel="stylesheet" />

    <script>
        $(function () {
            $("#nav ul > li").eq(2).addClass("active").siblings().removeClass("active");
            //处理body顶部宽度
        });
    </script>

</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />

    <form id="frmExam" class="layui-form">
        <div class="layui-elem-quote" style="font-size: larger;">
            <span style="margin-left: 20px;"><%=skillsn %></span>
            <span style="margin-left: 20px; color: red;">总分:<%= totalScore %>分</span>
            <span style="margin-left: 20px;">考试日期：<%=dateTm.ToString("yyyy-MM-dd") %></span>
            <span style="margin-left: 20px;">得分：<%= GetUserScore()%></span>
        </div>
        <asp:Repeater runat="server" ID="rptExamQuestion" OnItemDataBound="rptExamQuestion_ItemDataBound">
            <ItemTemplate>
                <div class="layui-collapse">
                    <div class="layui-colla-item">
                        <h2 class="layui-colla-title">
                            <b>第<%# Container.ItemIndex+1 %>题：<%# Eval("Question") %>
                                <span style="margin-left: 20px; color: #005288;">
                                    <%# ConvertQuestionType(Eval("IsMultiple"),Eval("QuestionResult")) %></span>
                                <span style="margin-left: 20px;">分数：<label style="color: red;"><%# Eval("Score") %></label></span>
                            </b>
                        </h2>
                        <div style="height: 22px; line-height: 22px; padding-left: 30px; background-color: darkgray; color: white;">
                            正确答案： <%# GetQuestionResult(Eval("QuestionResult")) %>
                        </div>
                        <div class="layui-colla-content layui-show">
                            <div class="layui-input-block" style="margin-left: 20px;">
                                <asp:Repeater runat="server" ID="rptQuestionOption">
                                    <ItemTemplate>
                                        <%# ConvertIsMultiple(DataBinder.Eval(((RepeaterItem)Container.Parent.Parent).DataItem, "IsMultiple"),Eval("QuestionID"),Eval("QuestionOption"),GetUserResult(Eval("QuestionID"))) %>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <%# GetExamStandardResult(Eval("ID")) %>
                            </div>
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </form>
    <script src="Scripts/layui/layui.all.js"></script>
    <script>
        layui.use('form', function () {
            var form = layui.form;
        });
    </script>

</body>
</html>
