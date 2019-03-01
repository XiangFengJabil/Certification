<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TempDataImport.aspx.cs" Inherits="Certification.TempDataImport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:FileUpload ID="FileUpload1" runat="server" />
            <asp:Button ID="btnUploadExcel" runat="server" Text="UploadExcel" OnClick="btnUploadExcel_Click" />
            <asp:Button ID="Button1" runat="server" Text="清除全部数据(测试使用)" OnClick="Button1_Click" />
        </div>
    </form>
</body>
</html>
