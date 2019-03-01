<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Menu.ascx.cs" Inherits="Certification.Controls.Menu" %>

<div id="Header">
    <hr id="topHR" />
    <div id="logoDiv">
        <a href="http://jabilweb.corp.jabil.org/" style="cursor: pointer; float: left">
            <img src="images/jabil_log.jpg" style="width: 179px; height: 48px; border: none" alt="JABIL" />
        </a><%--<span style="float: right; position:relative;top:20px; margin-right: 350px; display: block; font-family: Tahoma; font-style: normal; font-size: 11px;">v 1.0</span><span id="appName">Certification List</span>--%>

        <span style="float: right; position: relative; top: 20px; margin-right: 1%; display: inline; font-family: Tahoma; font-style: normal; font-size: 11px;">v 1.0</span>
        <span style="float: right; position: relative; top: 0px; margin-right: 1%; display: inline;" id="appName">Training & Certification</span>
        <div style="clear: both">
        </div>
    </div>
    <div id="nav">
        <ul>

            <li><a href="Default.aspx">Home</a></li>
            <li><a href="Certification.aspx">Certification</a></li>
            <li><a href="Exam.aspx">Exam</a></li>
            <li><a href="DeptHead.aspx">Dept Person Liable</a></li>
            <li><a href="OperationRecord.aspx">Operation Record</a></li>
            <%--            <li><a href='<%= GetUrl("RequestList.aspx?act=3")  %>'>Get Child Link Data</a></li>
            <li><a href='<%= GetUrl("DeptHead.aspx")  %>'>Get Child Link Data</a></li>--%>
        </ul>
        <asp:Label ID="lbUser" Style="float: right; position: relative; top: 0px; margin-right: 1%; display: block; font-family: Tahoma; font-style: normal; font-size: 11px;" CssClass="userInfo" runat="server"></asp:Label>

    </div>
</div>
