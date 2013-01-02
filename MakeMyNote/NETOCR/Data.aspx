<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Data.aspx.cs" Inherits="NETOCR.Data" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>
<%@ Register TagPrefix="FTB" Namespace="FreeTextBoxControls" Assembly="FreeTextBox" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server" EnableViewState="true">
   <center>
   <asp:Panel ID="GridPanel" runat="server">
    <asp:GridView ID="GridView1" runat="server" AllowSorting="True" 
        AutoGenerateColumns="False" CellPadding="4" DataKeyNames="FID" 
        DataSourceID="SqlDataSource1" EnableModelValidation="True" ForeColor="#333333" 
        GridLines="None" meta:resourcekey="GridView1Resource1" 
           onrowcreated="GridView1_RowCreated" onrowcommand="GridView1_RowCommand" 
           Width="912px" >
        <AlternatingRowStyle BackColor="White" />
        <Columns>
            <asp:ButtonField ButtonType="Link" CommandName="Text" Text="Text" />

            <asp:BoundField DataField="FID" HeaderText="FID" InsertVisible="False" 
                meta:resourcekey="BoundFieldResource1" ReadOnly="True" SortExpression="FID" />
            <asp:BoundField DataField="Name" HeaderText="Name" 
                meta:resourcekey="BoundFieldResource2" SortExpression="Name" />
            <asp:BoundField DataField="date" HeaderText="date" 
                meta:resourcekey="BoundFieldResource3" SortExpression="date" />
            <asp:BoundField DataField="Tags" HeaderText="Tags" 
                meta:resourcekey="BoundFieldResource4" SortExpression="Tags" />
            <asp:CommandField ShowEditButton="True" />
        </Columns>
        <EditRowStyle BackColor="#2461BF" />
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <RowStyle BackColor="#EFF3FB" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
    </asp:GridView>    
    </asp:Panel>
    </center>
    <center>
    <asp:Panel ID="TextPanel" runat="server" Visible="False" Width="600px" 
        meta:resourcekey="TextPanelResource1">
        
        <asp:Label ID="TitleLabel" runat="server" Text=""></asp:Label>
        <FTB:FreeTextBox ID="DataViewBox" runat="server" AllowHtmlMode="False" 
            AssemblyResourceHandlerPath="" AutoConfigure="" 
            AutoGenerateToolbarsFromString="True" AutoHideToolbar="True" 
            AutoParseStyles="True" BackColor="158, 190, 245" BaseUrl="" 
            BreakMode="Paragraph" ButtonDownImage="False" ButtonFileExtention="gif" 
            ButtonFolder="Images" ButtonHeight="20" ButtonImagesLocation="InternalResource" 
            ButtonOverImage="False" ButtonPath="" ButtonSet="Office2003" ButtonWidth="21" 
            ClientSideTextChanged="" ConvertHtmlSymbolsToHtmlCodes="False" 
            DesignModeBodyTagCssClass="" DesignModeCss="" DisableIEBackButton="False" 
            DownLevelCols="50" DownLevelMessage="" DownLevelMode="TextArea" 
            DownLevelRows="10" EditorBorderColorDark="128, 128, 128" 
            EditorBorderColorLight="128, 128, 128" EnableHtmlMode="True" EnableSsl="False" 
            EnableToolbars="True" Focus="False" FormatHtmlTagsToXhtml="True" 
            GutterBackColor="129, 169, 226" GutterBorderColorDark="128, 128, 128" 
            GutterBorderColorLight="255, 255, 255" Height="350px" HelperFilesParameters="" 
            HelperFilesPath="" HtmlModeCss="" HtmlModeDefaultsToMonoSpaceFont="True" 
            ImageGalleryPath="~/images/" 
            ImageGalleryUrl="ftb.imagegallery.aspx?rif={0}&amp;cif={0}" 
            InstallationErrorMessage="InlineMessage" JavaScriptLocation="InternalResource" 
            Language="en-US" PasteMode="Default" ReadOnly="False" 
            RemoveScriptNameFromBookmarks="True" RemoveServerNameFromUrls="True" 
            RenderMode="NotSet" ScriptMode="External" ShowTagPath="False" SslUrl="/." 
            StartMode="DesignMode" StripAllScripting="False" 
            SupportFolder="/aspnet_client/FreeTextBox/" TabIndex="-1" 
            TabMode="InsertSpaces" Text="" TextDirection="LeftToRight" 
            ToolbarBackColor="Transparent" ToolbarBackgroundImage="True" 
            ToolbarImagesLocation="InternalResource" 
            ToolbarLayout="ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu|Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,InsertImage,InsertRule|Cut,Copy,Paste;Undo,Redo,Print" 
            ToolbarStyleConfiguration="NotSet" UpdateToolbar="True" 
            UseToolbarBackGroundImage="True" Width="600px" />
        <asp:Button ID="SaveButton" runat="server" Text="Save" 
            onclick="SaveButton_Click" meta:resourcekey="SaveButtonResource1" />
        <asp:Button ID="CancelButton" runat="server" Text="Cancel" 
            onclick="CancelButton_Click" />
            
    </asp:Panel>
</center>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:FilesDatabaseConnectionString %>" 
        SelectCommand="SELECT [FID], [Name], [date], [Tags] FROM [Files]">
    </asp:SqlDataSource>
</asp:Content>
