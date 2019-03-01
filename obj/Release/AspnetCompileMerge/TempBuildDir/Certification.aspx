<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Certification.aspx.cs" Inherits="Certification.RequestList" %>

<%@ Register Src="~/Controls/Menu.ascx" TagPrefix="uc2" TagName="Menu" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Certification List</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="Styles/global.css" rel="stylesheet" type="text/css" />
    <link href="Styles/common.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/jquery-1.9.1.min.js"></script>
    <style type="text/css">
        body {
        overflow-x:auto;
        }
        .txt80 {
            height: 24px;
            width: 80px;
        }

        input {
            height: 24px;
        }

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

        .thMerge {
            padding-left: 10px;
            padding-right: 10px;
        }

        #divFilter {
            margin-top: 4px;
        }

            #divFilter input {
                margin-left: 10px;
            }

        .mouseover {
            background-color: gainsboro;
        }
    </style>

    <script type="text/javascript">
        $(function () {
            $("#nav ul > li").eq(1).addClass("active").siblings().removeClass("active");
            //处理body顶部宽度
            $("#Header").width($("#tabCartification").outerWidth(true));

            $("#tabCartification tr").mousemove(
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
    <uc2:Menu runat="server" ID="Menu" />
    <div id="divMain">
        <form id="form1" runat="server">
            <div id="divFilter">
                <b>查询条件</b>
                <asp:DropDownList ID="ddlItems" CssClass="txt80" runat="server">
                    <asp:ListItem Value="-1">--请选择--</asp:ListItem>
                    <asp:ListItem Value="0">姓名</asp:ListItem>
                    <asp:ListItem Value="1">Workcell</asp:ListItem>
                    <asp:ListItem Value="2">EEID</asp:ListItem>
                    <asp:ListItem Value="3">Category</asp:ListItem>
                    <asp:ListItem Value="4">Skill SN</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtItem" Height="20px" runat="server"></asp:TextBox>
                <asp:Button ID="btnSubmit" runat="server" Text="Select" CssClass="txt80" OnClick="btnSubmit_Click" />
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <asp:Button ID="btnUploadExcel" runat="server" Text=" UploadExcel " OnClick="btnUploadExcel_Click" />
                <asp:Button ID="btnDataExport" runat="server" Text=" Data Export " OnClick="btnDataExport_Click" />
            </div>

            <div class="divTable">
                <table id="tabCartification" class="tableDefault" cellpadding="0" border="0">
                    <tr>
                        <th>Item</th>
                        <th>Name</th>
                        <th>Workcell/ROP</th>
                        <th>EEID</th>
                        <th>NTID</th>
                        <th class="thMerge">On Boarding Date</th>
                        <th>Category</th>
                        <th>Skill SN</th>
                        <th class="thMerge">Certificate Date</th>
                        <th class="thMerge" colspan="2">1st(Re-certificate)</th>
                        <th class="thMerge" colspan="2">2nd(Re-certificate)</th>
                        <th class="thMerge" colspan="2">3rd(Re-certificate)</th>
                        <th class="thMerge" colspan="2">4th(Re-certificate)</th>
                        <th class="thMerge" colspan="2">5th(Re-certificate)</th>
                        <th class="thMerge" colspan="2">6th(Re-certificate)</th>
                        <th class="thMerge" colspan="2">7th(Re-certificate)</th>
                        <th class="thMerge" colspan="2">8th(Re-certificate)</th>
                        <th class="thMerge" colspan="2">9th(Re-certificate)</th>
                        <th class="thMerge" colspan="2">10th(Re-certificate)</th>
                        <th class="thMerge" colspan="2">11th(Re-certificate)</th>
                        <th class="thMerge" colspan="2">12th(Re-certificate)</th>
                        <th>是否发送邮件</th>
                    </tr>
                    <asp:Repeater ID="rptCertificate" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%# Eval("Item") %></td>
                                <td><%# Eval("Name") %></td>
                                <td><%# Eval("Workcell") %></td>
                                <td><%# Eval("EEID") %></td>
                                <td><%# Eval("NTID") %></td>
                                <td><%# DateConvert(Eval("OnBoardingDate")) %></td>
                                <td><%# Eval("Category") %></td>
                                <td><%# Eval("SkillSN") %></td>
                                <td><%# DateConvert(Eval("CertificateDate")) %></td>
                                <td><%# DateConvert(Eval("1st_1")) %></td>
                                <td><%# Eval("1st_2") %></td>
                                <td><%# DateConvert(Eval("2nd_1")) %></td>
                                <td><%# Eval("2nd_2") %></td>
                                <td><%# DateConvert(Eval("3rd_1")) %></td>
                                <td><%# Eval("3rd_2") %></td>
                                <td><%# DateConvert(Eval("4th_1")) %></td>
                                <td><%# Eval("4th_2") %></td>
                                <td><%# DateConvert(Eval("5th_1")) %></td>
                                <td><%# Eval("5th_2") %></td>
                                <td><%# DateConvert(Eval("6th_1")) %></td>
                                <td><%# Eval("6th_2") %></td>
                                <td><%# DateConvert(Eval("7th_1")) %></td>
                                <td><%# Eval("7th_2") %></td>
                                <td><%# DateConvert(Eval("8th_1")) %></td>
                                <td><%# Eval("8th_2") %></td>
                                <td><%# DateConvert(Eval("9th_1")) %></td>
                                <td><%# Eval("9th_2") %></td>
                                <td><%# DateConvert(Eval("10th_1")) %></td>
                                <td><%# Eval("10th_2") %></td>
                                <td><%# DateConvert(Eval("11th_1")) %></td>
                                <td><%# Eval("11th_2") %></td>
                                <td><%# DateConvert(Eval("12th_1")) %></td>
                                <td><%# Eval("12th_2") %></td>
                                <td><%# Eval("IsSendEmail") %></td>
                            </tr>
                        </ItemTemplate>

                    </asp:Repeater>
                </table>
            </div>
        </form>
    </div>
</body>
</html>
