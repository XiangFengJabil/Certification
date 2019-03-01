<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExamOline.aspx.cs" Inherits="Certification.ExamOline" %>

<%@ Register Src="~/Controls/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>在线考试</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="Styles/global.css" rel="stylesheet" />

    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <link href="Scripts/layui/css/layui.css" rel="stylesheet" />
    <script type="text/javascript">
        $(function () {
            $("#nav ul > li").eq(2).addClass("active").siblings().removeClass("active");

            $("#btnCommit").click(function () {
                if (confirm("您确定交卷吗？")) {
                    $("#frmExam").submit();
                }
            });
        });

        var startTime = new Date('<% Response.Write(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));%>');  //开始时间
        var endTime = new Date('<% Response.Write(Convert.ToDateTime(Session["examTime"]).ToString("yyyy/MM/dd HH:mm:ss"));%>'); //结束时间
        //两个时间相差的秒数
        var maxtime = parseInt(endTime - startTime) / 1000;
        function CountDown() {
            if (maxtime >= 0) {
                minutes = Math.floor(maxtime / 60);
                seconds = Math.floor(maxtime % 60);
                msg = "距离考试结束还有" + minutes + "分" + seconds + "秒";
                $("#examTime").html(msg);
                if (maxtime == 5 * 60) {
                    layer.open({
                        title: '提示'
                        , content: '还剩5分钟'
                        , icon: 0
                    });
                }
                --maxtime;
            } else {
                clearInterval(timer);

                layer.open({
                    title: ''
                    , content: '时间到，结束!'
                    , icon: 0
                });
                //时间到，自动提交
                $("#frmExam").submit();
            }
        }
        timer = setInterval(CountDown, 1000);
        
    </script>


</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />

    <form id="frmExam" class="layui-form" method="post" action="?Action=<%=Request["ID"] %>">
        <div class="layui-elem-quote" style="font-size: larger;">
            <span style="margin-left: 20px;"><%=Request["ID"] %></span>
            <span style="margin-left: 20px; color: red;">总分:<%= totalScore %>分</span>
            <span style="margin-left: 20px; float: right; color: #005288;" id="examTime"></span>
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

                        <div class="layui-colla-content layui-show">
                            <div class="layui-input-block" style="margin-left: 20px;">
                                <asp:Repeater runat="server" ID="rptQuestionOption">
                                    <ItemTemplate>
                                        <%# ConvertIsMultiple(DataBinder.Eval(((RepeaterItem)Container.Parent.Parent).DataItem, "IsMultiple"),Eval("QuestionID"),Eval("QuestionOption")) %>
                                    </ItemTemplate>
                                </asp:Repeater>

                            </div>

                        </div>
                    </div>
                </div>

            </ItemTemplate>
        </asp:Repeater>
    </form>

    <div style="text-align: center; margin: 10px 0 10px 0;">
        <input id="btnCommit" type="button" value="确定交卷" class="layui-btn layui-btn-lg" />
    </div>

    <ul class="layui-fixbar">
        <li onclick='$("html,body").animate({"scrollTop":0});' class=" layui-icon layui-fixbar-top" style="background-color: rgb(0, 150, 136); display: list-item;"></li>
    </ul>


    <script src="Scripts/layui/layui.all.js"></script>
    <script>
        layui.use('form', function () {
            var form = layui.form;
        });
    </script>

</body>
</html>
