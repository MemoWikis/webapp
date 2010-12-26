<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
	Inherits="System.Web.Mvc.ViewPage<TrueOrFalse.Frontend.Web.Models.QuestionHomeModel>" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Willkommen bei True Or False
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%= Html.Encode(ViewData["Message"]) %></h2>
    <p>

		<%foreach(var question in Model.MostPopular){ %>
		
			<div><%= question.Text %></div>
		
		<%} %>
        
    </p>
</asp:Content>
