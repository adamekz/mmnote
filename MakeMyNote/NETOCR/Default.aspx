<%@ Page Title="OCR.NET" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="NETOCR._Default" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<script runat="server">
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (FileUpload1.HasFile)
            try
            {
                //FileUpload1.SaveAs("H:\\Uploads\\" + FileUpload1.FileName);
                FileUpload1.SaveAs(Server.MapPath("~/img/" + FileUpload1.FileName));
                //pic_file = "H:\\Uploads\\" + FileUpload1.FileName;
                pic_file = Server.MapPath("~/img/" + FileUpload1.FileName);
                Label1.Text = "File name: " +
                     FileUpload1.PostedFile.FileName + "<br>" +
                     FileUpload1.PostedFile.ContentLength + " b<br>" +
                     "Content type: " +
                     FileUpload1.PostedFile.ContentType;
                Panel2.Visible = false;
                pre_ocr(pic_file);
               
            }
            catch (Exception ex)
            {
                Label1.Text = "ERROR: " + ex.Message.ToString();
            }
        else
        {
            Label1.Text = "You have not specified a file.";
        }
    }
</script>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">   
    <asp:Panel ID="Panel0" runat="server">
     <h2>
        Welcome to OCR.NET!
    </h2>
    </asp:Panel>
    <asp:Panel ID="Panel2" runat="server">
        <p id="Paragraph1">
            To start Your journey select file&nbsp;
            <asp:FileUpload ID="FileUpload1" runat="server" />
            &nbsp;and press this button
            <asp:Button ID="Button1" runat="server" onClick="Button1_Click" 
            Text="Click me!" />
        </p>
    </asp:Panel>
    <p>
        <asp:Label ID="Label1" runat="server"></asp:Label>
    </p>
    <asp:Panel ID="Panel1" runat="server" Visible="False">
    
        <asp:Image ID="Image1" runat="server" Height="200px" Width="200px" 
            ImageAlign="Middle" />
        <br />
        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/rleft.bmp" 
            onclick="ImageButton1_Click" />
        <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="~/rright.bmp" 
            onclick="ImageButton2_Click" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button2" runat="server" Text="Continue" 
            onclick="Button2_Click" />
    </asp:Panel>
    
    <asp:Panel ID="Panel3" runat="server" Height="23px" Width="910px" Visible="False">
        <center><asp:Label ID="Label2" runat="server" Text=""></asp:Label>
            
        </center>
    </asp:Panel>

    <asp:Panel ID="Panel4" runat="server" Height="254px" Visible="False">
               <center>
                <FTB:FreeTextBox ID="PastOCRBox" runat="server" Height="350px" ToolbarStyleConfiguration="NotSet" />
                <asp:Panel ID="Panel6" runat="server" >
                    <asp:Button ID="SaveButton" runat="server" Text="Save" 
                        onclick="SaveButton_Click" />
                    <asp:Button ID="DwnButton" runat="server" Text="Download" 
                        onclick="DwnButton_Click" />
                    <asp:Label Text="Title:" runat="server"></asp:Label>
                    <asp:TextBox ID="TitleBox" Text="" runat="server"></asp:TextBox>
                    <asp:Label Text="Tags:" runat="server"></asp:Label>
                    <asp:TextBox ID="TagsBox" Text="" runat="server"></asp:TextBox>
                    <asp:Label ID="PastStatus" Text="" runat="server"></asp:Label>
                </asp:Panel>
                </center>
            </asp:Panel>

    <asp:Panel ID="Panel5" runat="server">
   
    </asp:Panel>
</asp:Content>
