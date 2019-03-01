<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Operation.aspx.cs" Inherits="Certification.Operation" %>

<%@ Register Src="~/Controls/Menu.ascx" TagPrefix="uc1" TagName="Menu" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>实操考核</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
    <link href="Styles/global.css" rel="stylesheet" />
    <link href="Scripts/layui/css/layui.css" rel="stylesheet" />

    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/layui/layui.js"></script>

    <style>
        .marginTop4px {
            margin-top: 6px;
        }
    </style>

    <script>
        $(function () {
            $("#nav ul > li").eq(4).addClass("active").siblings().removeClass("active");

            $.ajaxSettings.async = false;
            $("#btnSaveBasicInfo").click(function () {
                $.post("Operation.aspx",
                    {
                        basis: randomString(32),
                        ID: $("#iptID").val(),
                        Customer: $("#frmBasic :input[name=Customer]").val(),
                        ProductType: $("#frmBasic :input[name=ProductType]").val(),
                        ProductPnOrSn: $("#frmBasic :input[name=ProductPnOrSn]").val(),
                        Quantity: $("#frmBasic :input[name=Quantity]").val(),
                        InspectorName: $("#frmBasic :input[name=InspectorName]").val(),
                        AssessmentDate: $("#frmBasic :input[name=AssessmentDate]").val()
                    },
                    function (data) {
                        var v = data.toString();

                        layui.use('layer', function () {
                            var layer = layui.layer;
                            
                            if (v == "-1") {
                                layer.msg('出错,保存失败!', { icon: 5 });
                            } else {
                                layer.msg('保存成功!', { icon: 1 });
                                $("#frmBasic :input").addClass("layui-btn-disabled").unbind("click");
                            }
                        });
                    });

            });

            //保存实操
            $("#btnSaveOperation").click(function () {
                var params = "operation=" + randomString(32) + "&recordID=" + $("#iptID").val();

                $("#frmOperation :input[type=hidden]").each(function (i, e) {
                    //itemID
                    params += "&item" + i + "=" + $(e).val();
                });


                $("#frmOperation :input[type=text]").each(function (i, e) {
                    //itemScore
                    params += "&score" + i + "=" + $(e).val();
                });

                $.ajax({
                    url: "Operation.aspx",
                    type: "post",
                    data: params,
                    success: function (data) {
                        var v = data.toString();
                        layui.use('layer', function () {
                            var layer = layui.layer;

                            if (v == "-1") {
                                layer.msg('出错,保存失败!', { icon: 5 });
                            } else {
                                layer.msg('保存成功!', { icon: 1 });
                                $("#btnSaveOperation").addClass("layui-btn-disabled").unbind("click");
                            }
                        });

                    }
                });

            });

            //分数统计
            $("#o1 :input[type=text]").keyup(function () {
                sumValue("#o1 :input[type=text]", "#lblO1");
                $("#lblOpearationScore").text($("#lblO1").text());
                sumTotalScore();
            });

            $("#o2 :input[type=text]").keyup(function () {
                sumValue("#o2 :input[type=text]", "#lblO2");
                $("#lblLeanScore").text($("#lblO2").text());
                sumTotalScore();
            });

            $("#o3 :input[type=text]").keyup(function () {
                sumValue("#o3 :input[type=text]", "#lblO3");
                $("#lblOtherScore").text($("#lblO3").text());
                sumTotalScore();
            });
        });


        function sumValue(tabChildrenInput, sumText) {
            var sumValue = 0;
            $(tabChildrenInput).each(function () {
                if ($(this).val() != "")
                    sumValue += parseFloat($(this).val());
            });
            $(sumText).text(sumValue.toFixed(2));

            //lblTotlaScore计算总分examScore,lblOpearationScore,lblLeanScore,lblOtherScore
        }

        function sumTotalScore() {
            var exam = $("#examScore").text();
            var opearationScore = $("#lblOpearationScore").text();
            var leanScore = $("#lblLeanScore").text();
            var otherScore = $("#lblOtherScore").text();

            var totalScore = (exam * 0.15) + (opearationScore * 0.5) + (leanScore * 0.2) + (otherScore * 0.15);
            $("#lblTotlaScore").text(totalScore.toFixed(2));
        }

        function randomString(len) {
            len = len || 32;
            var $chars = 'ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678';
            var maxPos = $chars.length;
            var pwd = '';
            for (i = 0; i < len; i++) {
                pwd += $chars.charAt(Math.floor(Math.random() * maxPos));
            }
            return pwd;
        }

    </script>
</head>
<body>
    <uc1:Menu runat="server" ID="Menu" />
    <input type="hidden" id="iptID" value='<%=Request["id"] %>' />
    <div class="layui-elem-quote">
        <form class="layui-form layui-form-pane" id="frmBasic">
            <div class="layui-form-item">
                <label class="layui-form-label marginTop4px">部门</label>
                <div class="layui-input-inline marginTop4px">
                    <input type="text" name="Department" required lay-verify="required" readonly="readonly" value='<%= lstOperationRecordInfo[0].Department %>' autocomplete="off" class="layui-input" />
                </div>

                <label class="layui-form-label marginTop4px">姓名</label>
                <div class="layui-input-inline marginTop4px">
                    <input type="text" name="DisplayName" required lay-verify="required" readonly="readonly" value='<%= lstOperationRecordInfo[0].DisplayName %>' autocomplete="off" class="layui-input" />
                </div>

                <label class="layui-form-label marginTop4px">NTID</label>
                <div class="layui-input-inline marginTop4px">
                    <input type="text" name="NTID" required lay-verify="required" readonly="readonly" value='<%= lstOperationRecordInfo[0].NTID %>' autocomplete="off" class="layui-input" />
                </div>

                <label class="layui-form-label marginTop4px">客户</label>
                <div class="layui-input-inline marginTop4px">
                    <input type="text" name="Customer" required lay-verify="required" value='<%= lstOperationRecordInfo[0].Customer %>' placeholder="客户" autocomplete="off" class="layui-input" />
                </div>

                <label class="layui-form-label marginTop4px">产品型号</label>
                <div class="layui-input-inline marginTop4px">
                    <input type="text" name="ProductType" required lay-verify="required" value='<%= lstOperationRecordInfo[0].ProductType %>' placeholder="产品型号" autocomplete="off" class="layui-input" />
                </div>

                <label class="layui-form-label marginTop4px">产品PN/SN</label>
                <div class="layui-input-inline marginTop4px">
                    <input type="text" name="ProductPnOrSn" required lay-verify="required" value='<%= lstOperationRecordInfo[0].ProductPnOrSn %>' placeholder="产品PN/SN" autocomplete="off" class="layui-input" />
                </div>

                <label class="layui-form-label marginTop4px">数量</label>
                <div class="layui-input-inline marginTop4px">
                    <input type="text" name="Quantity" required lay-verify="number" value='<%= lstOperationRecordInfo[0].Quantity %>' placeholder="数量" autocomplete="off" class="layui-input" />
                </div>

                <label class="layui-form-label marginTop4px">考核员</label>
                <div class="layui-input-inline marginTop4px">
                    <input type="text" name="InspectorName" required lay-verify="required" value='<%=user.DisplayName%>' readonly="readonly" placeholder="考核员" autocomplete="off" class="layui-input" />
                </div>

                <label class="layui-form-label marginTop4px">日期</label>
                <div class="layui-input-inline marginTop4px">
                    <input type="text" name="AssessmentDate" required lay-verify="required" value='<%=DateTime.Now.ToString("yyyy-MM-dd") %>' readonly="readonly" autocomplete="off" class="layui-input" />
                </div>
                <div class="layui-input-inline marginTop4px">
                    <input id="btnSaveBasicInfo" class="layui-btn" type="button" value="保存" />
                </div>
            </div>
        </form>
    </div>

    <div style="padding-left: 20px; padding-right: 20px;">

        <form id="frmOperation">
            <fieldset class="layui-elem-field">
                <legend>实际操作</legend>
                <div class="layui-field-box">
                    <table id="o1" class="layui-table" lay-size="sm">
                        <thead>
                            <tr>
                                <th>序号</th>
                                <th>具体内容</th>
                                <th>实操分数</th>
                                <th>评分标准</th>
                                <th>得分</th>
                            </tr>
                        </thead>
                        <asp:Repeater runat="server" ID="rptOperation" OnItemDataBound="rptOperation_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Container.ItemIndex+1 %></td>
                                    <td><%# Eval("OperationItem") %>
                                    </td>
                                    <td class="layui-colorpicker-lg">
                                        <%# Eval("OperationScore") %>
                                    </td>
                                    <td>
                                        <ul>
                                            <asp:Repeater ID="rptOperationStandard" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <%# Eval("OperationStandard") %>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </td>
                                    <td>
                                        <input type="hidden" value='<%# Eval("ID") %>' />
                                        <input type="text" value='<%# GetUserScore(Eval("ID")) %>' maxlength="5" lay-verify="required|number" onkeyup="(this.v=function(){this.value=this.value.replace(/[^0-9-]+/,'');}).call(this)" placeholder="得分" onblur="this.v();" autocomplete="off" class="layui-input" <%if (lstOperationRecordInfo[0].IsApproved) Response.Write("disabled='disabled'"); %>>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr class="layui-carousel-right">
                            <td colspan="5">
                                <span style="float: right; margin: 8px 10px auto auto;">实操总得分:<label id="lblO1" style="color: red;"><%= lstOperationRecordInfo[0].OperationTotalScore %></label></span>
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>

            <fieldset class="layui-elem-field">
                <legend>精益分(参考精粉分值表)</legend>
                <div class="layui-field-box">
                    <table id="o2" class="layui-table" lay-size="sm">
                        <thead>
                            <tr>
                                <th>序号</th>
                                <th>具体内容</th>
                                <th>分数</th>
                                <th>评分标准</th>
                                <th>得分</th>

                            </tr>
                        </thead>
                        <asp:Repeater runat="server" ID="rptLean" OnItemDataBound="rptOperation_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Container.ItemIndex+1 %></td>
                                    <td><%# Eval("OperationItem") %>
                                    </td>
                                    <td class="layui-colorpicker-lg">
                                        <%# Eval("OperationScore") %>
                                    </td>
                                    <td>
                                        <ul>
                                            <asp:Repeater ID="rptOperationStandard" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <%# Eval("OperationStandard") %>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </td>
                                    <td>
                                        <input type="hidden" value='<%# Eval("ID") %>' />
                                        <input type="text" value='<%# GetUserScore(Eval("ID")) %>' name='<%# Eval("ID") %>' maxlength="5" lay-verify="required|number" onkeyup="(this.v=function(){this.value=this.value.replace(/[^0-9-]+/,'');}).call(this)" placeholder="得分" onblur="this.v();" autocomplete="off" class="layui-input" <%if (lstOperationRecordInfo[0].IsApproved) Response.Write("disabled='disabled'"); %>>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr class="layui-carousel-right">
                            <td colspan="5">
                                <span style="float: right; margin: 8px 10px auto auto;">精益总得分:<label id="lblO2" style="color: red;"><%= lstOperationRecordInfo[0].LeanTotalScore %></label></span>
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>

            <fieldset class="layui-elem-field">
                <legend>其它</legend>
                <div class="layui-field-box">
                    <table id="o3" class="layui-table" lay-size="sm">
                        <thead>
                            <tr>
                                <th>序号</th>
                                <th>具体内容</th>
                                <th>实操分数</th>
                                <th>评分标准</th>
                                <th>得分</th>
                            </tr>
                        </thead>
                        <asp:Repeater runat="server" ID="rptOther" OnItemDataBound="rptOperation_ItemDataBound">
                            <ItemTemplate>
                                <tr>
                                    <td><%# Container.ItemIndex+1 %></td>
                                    <td><%# Eval("OperationItem") %>
                                    </td>
                                    <td class="layui-colorpicker-lg">
                                        <%# Eval("OperationScore") %>
                                    </td>
                                    <td>
                                        <ul>
                                            <asp:Repeater ID="rptOperationStandard" runat="server">
                                                <ItemTemplate>
                                                    <li>
                                                        <%# Eval("OperationStandard") %>
                                                    </li>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </ul>
                                    </td>
                                    <td>
                                        <input type="hidden" value='<%# Eval("ID") %>' />
                                        <input type="text" value='<%# GetUserScore(Eval("ID")) %>' name='<%# Eval("ID") %>' maxlength="5" lay-verify="required|number" onkeyup="(this.v=function(){this.value=this.value.replace(/[^0-9-]+/,'');}).call(this)" placeholder="得分" onblur="this.v();" autocomplete="off" class="layui-input" <%if (lstOperationRecordInfo[0].IsApproved) Response.Write("disabled='disabled'"); %>>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr class="layui-carousel-right">
                            <td colspan="5">
                                <span style="float: right; margin: 8px 10px auto auto;">其它总得分:<label id="lblO3" style="color: red;"><%= lstOperationRecordInfo[0].OtherTotalScore %></label></span>
                            </td>
                        </tr>
                    </table>
                </div>
            </fieldset>

            <div class="layui-form-item">
                <% if (lstOperationRecordInfo[0].IsApproved) Response.Write(""); else Response.Write("<input id='btnSaveOperation' class='layui-btn' value='提交' type='button' />"); %>
            </div>
        </form>
    </div>

    <div class="layui-form-item">
        <fieldset class="layui-elem-field layui-field-title">
            <legend>最终评分标准</legend>
        </fieldset>
        <blockquote class="layui-elem-quote">
            <span style="float: right; margin-right: 20px; font-size: larger; color: red;">总得分:
                <label id="lblTotlaScore"><%=lstOperationRecordInfo[0].TotalScore %></label></span>
            <span style="color: #0B588A;">总分 = 理论知识得分 X 15% + 实际操作得分 X 50% + 精盗分 X 20% + 其它分 X 15%</span>
            <span style="color: orange; margin-left: 30px;">笔试分:<label id="examScore"><%=lstOperationRecordInfo[0].ExamTotalScore %></label></span>
            <span style="color: orange; margin-left: 30px;">实操分:<label id="lblOpearationScore"><%=lstOperationRecordInfo[0].OperationTotalScore %></label></span>
            <span style="color: orange; margin-left: 30px;">精益分:<label id="lblLeanScore"><%=lstOperationRecordInfo[0].LeanTotalScore %></label></span>
            <span style="color: orange; margin-left: 30px;">其它分:<label id="lblOtherScore"><%=lstOperationRecordInfo[0].OtherTotalScore %></label></span>

        </blockquote>
    </div>

    <div class="layui-form-item">
        <fieldset class="layui-elem-field layui-field-title">
            <legend>判断标准</legend>
        </fieldset>
        <blockquote class="layui-elem-quote">
            <table border="1" style="text-align: center;">
                <tr>
                    <th>内容</th>
                    <th>分值区间</th>
                    <th>考核结果</th>
                    <th>总分标准</th>
                    <th>备注</th>
                </tr>
                <tr>
                    <td rowspan="3">理论知识（总分100）</td>
                    <td>< 80</td>
                    <td>不合格</td>
                    <td rowspan="12">< 73 不合格    73~87.5 合格   ﹥87.5 优秀
                    </td>
                    <td rowspan="12">
                        <ul style="list-style-type: decimal;">
                            <li>60~73分的员工，原则上淘汰，但在生产任务紧急，人员急缺的情况下，建议分配到操作简单工序或部门；</li>
                            <li>60分以下，在任何情况下都必须淘汰；</li>
                            <li>73~87.5分，适合熔接工序；</li>
                            <li>﹥87.5分，适合所有生产部门较难的生产操作工序。</li>
                        </ul>
                    </td>
                </tr>
                <tr>
                    <td>80~90</td>
                    <td>合格</td>
                </tr>
                <tr>
                    <td>>90</td>
                    <td>优秀</td>
                </tr>

                <tr>
                    <td rowspan="3">实际操作(总分100)</td>
                    <td>< 70</td>
                    <td>不合格</td>
                </tr>
                <tr>
                    <td>70 ~ 85</td>
                    <td>合格</td>
                </tr>
                <tr>
                    <td>> 85</td>
                    <td>优秀</td>
                </tr>

                <tr>
                    <td rowspan="3">精益分(总分100)</td>
                    <td>< 80</td>
                    <td>不合格</td>
                </tr>
                <tr>
                    <td>80~90</td>
                    <td>合格</td>
                </tr>
                <tr>
                    <td>>90</td>
                    <td>优秀</td>
                </tr>

                <tr>
                    <td rowspan="3">其它(总分100)</td>
                    <td>< 70</td>
                    <td>不合格</td>
                </tr>
                <tr>
                    <td>70~90</td>
                    <td>合格</td>
                </tr>
                <tr>
                    <td>>90</td>
                    <td>优秀</td>
                </tr>

            </table>


        </blockquote>
    </div>

</body>
</html>
