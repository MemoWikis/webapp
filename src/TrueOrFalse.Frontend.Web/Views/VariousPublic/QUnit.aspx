<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div id="qunit"></div>
    <div id="qunit-fixture"></div>    

    <script type="text/javascript">
        test("hello test", function () {
            ok(1 == "1", "Passed!");              
        });

        DateParserTests.Run();

    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link rel="stylesheet" href="//code.jquery.com/qunit/qunit-1.12.0.css" type="text/css" media="screen" />
    <script src="http://code.jquery.com/qunit/qunit-1.12.0.js"></script>
</asp:Content>
