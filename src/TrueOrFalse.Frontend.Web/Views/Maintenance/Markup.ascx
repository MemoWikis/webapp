<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<ImageMaintenanceInfo>" %>

<div>
    <%= !String.IsNullOrEmpty(Model.MetaData.Markup) ? Regex.Replace(Model.MetaData.Markup, @"\r\n?|\n", "<br />") : "Kein Markup vorhanden." %>
</div>
         





