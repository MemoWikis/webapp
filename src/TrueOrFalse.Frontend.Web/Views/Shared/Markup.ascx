<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TrueOrFalse.ImageMaintenanceInfo>" %>
<%@ Import Namespace="System.Activities.Statements" %>
<%@ Import Namespace="System.Web.Razor.Parser.SyntaxTree" %>
<%@ Import Namespace="Microsoft.Ajax.Utilities" %>
<%@ Import Namespace="TrueOrFalse" %>

<div>
    <%--<%= !String.IsNullOrEmpty(Model.MetaData.Markup) ? Model.MetaData.Markup.Replace(Environment.NewLine,"<br />") : "Kein Markup vorhanden." %>--%>
    <%= !String.IsNullOrEmpty(Model.MetaData.Markup) ? Regex.Replace(Model.MetaData.Markup, @"\r\n?|\n", "<br />") : "Kein Markup vorhanden." %>
</div>
         





