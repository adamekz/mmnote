<%@ Page Title="About Project" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="About.aspx.cs" Inherits="NETOCR.About" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        About
    </h2>
    <p>
      You can find <a href="http://go.microsoft.com/fwlink/?LinkID=152368&amp;clcid=0x409"
            title="MSDN ASP.NET Docs">documentation on ASP.NET at MSDN</a>.<br />
            In need of AForge.NET docs <a href="http://www.aforgenet.com/framework/docs/"
            title="AForge.NET Docs">this link will be helpful</a>.<br />
            OCR engine by <a href="http://code.google.com/p/tesseract-ocr/" title="Tessaract OCR">Tessaract OCR</a> through <a href="http://www.emgu.com/wiki/index.php/Main_Page" title="Emgu">EMGU CV</a>.<br />
            Text editing provided by <a href="http://www.freetextbox.com/" title="FreeTextBox">FreeTextBox</a>.
    </p>
</asp:Content>
