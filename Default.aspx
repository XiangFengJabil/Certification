<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Certification.Default" %>

<%@ Register Src="~/Controls/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Training & Certification</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="Styles/common.css" rel="stylesheet" />
    <link href="Styles/global.css" rel="stylesheet" />
    <link href="Styles/jquery-ui.min.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/jquery-1.9.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#nav ul > li").eq(0).addClass("active").siblings().removeClass("active");

            if ($("#iptUserRole").val() == "0") {
                $(".txtUserRole").hide();
            }

            //$(".divContent a").click(function () {

            //    if ($(this).children("img").attr("title") == "Training") {
            //        //window.location.href = "";
            //    } else if ($(this).children("img").attr("title") == "Certification")
            //        window.location.href = "Certification.aspx";
            //    else if ($(this).children("img").attr("title") == "") {
            //        //window.location.href = "";
            //    } else if ($(this).children("img").attr("title") == "Exam") {
            //        window.location.href = "Exam.aspx";
            //    } else if ($(this).children("img").attr("title") == "ExamQuestionManage") {
            //        window.location.href = "ExamQuestionManage.aspx";
            //    }


            //});

            $("#divReport").load("Report.aspx");

        });

    </script>
    <style type="text/css">
        .divMain {
            width: 90%;
            margin: 10px auto;
            text-align: center;
        }

        .divContent {
            background: url("images/home/blockbkgrd.png") no-repeat 4px 5px;
            width: 300px;
            height: 270px;
            text-align: center;
            display: inline-block;
            *zoom: 1;
            *display: inline;
        }

            .divContent a {
                margin: 10px auto;
                display: block;
                text-align: center;
                color: rgb(102, 102, 102);
                font-size: small;
                font-weight: bold;
                text-align: center;
            }

            .divContent img {
                padding: 0;
                margin: 30px auto;
                display: inline;
            }

            .divContent a span {
                display: block;
            }

            .divContent a:hover {
                color: #bc2c2c;
            }
    </style>
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />
    <%--<input type="hidden" value='<%=userRole %>' id="iptUserRole" />
    <div class="divMain">
        <div class="divContent">
            <a href="javascript:;">
                <img alt="Training" title="Training" src="images/home/folder_documents.png" />
                <span>Training</span>
            </a>
        </div>
        <div class="divContent">
            <a href="javascript:;">
                <img alt="Certification" title="Certification" src="images/home/Edit.png" />
                <span>Certification</span>
            </a>
        </div>
        <div class="divContent">
            <a href="javascript:;">
                <img alt="Management" title="Management" src="images/home/Cofigure.png" />
                <span>Management</span>
            </a>
        </div>
        <div class="divContent">
            <a href="javascript:;">
                <img alt="Exam" title="Exam" src="images/home/exam.png" />
                <span>Exam</span>
            </a>
        </div>
        <div class="divContent txtUserRole">
            <a href="javascript:;">
                <img alt="ExamQuestionManage" title="ExamQuestionManage" src="images/home/ExamQuestionManage.png" />
                <span>Exam Question Manage</span>
            </a>
        </div>
    </div>--%>
    
    <div id="divReport">

    </div>
</body>
</html>
