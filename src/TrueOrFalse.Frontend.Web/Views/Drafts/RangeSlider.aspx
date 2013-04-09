<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" Inherits="System.Web.Mvc.ViewPage<WelcomeModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    
    <link href="/Style/site.css" rel="stylesheet" type="text/css" />
    <link href="/Views/Drafts/RangeSlider.css" rel="stylesheet" />
    <!-- include the Tools -->
  <script src="http://cdn.jquerytools.org/1.2.7/full/jquery.tools.min.js"></script>
   

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    
  
<!-- Rangeslider from http://jquerytools.org/documentation/rangeinput/ --> 
 
  <!-- HTML5 range input -->
  <input type="range" name="test" min="0" max="10" value="5" step="0.1"/>

<input type="range" name="test" min="0" max="10" value="5" step="0.1"/> 
  
<!-- make it happen -->
  <script>
      $(":range").rangeinput();
  </script>
    
  
 
</asp:Content>
