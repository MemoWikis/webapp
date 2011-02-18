<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
	Inherits="System.Web.Mvc.ViewPage<TrueOrFalse.Frontend.Web.Models.QuestionHomeModel>"
    Title="Willkommen bei True Or False" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">

       <h2><%= Html.Encode(ViewData["Message"]) %></h2>

       <div class="span-16">
       
       </div>

       <hr/>

        <div class="span-8 colborder">  
            <h3>Spannende Fragen 1</h3>  

		    <%foreach(var question in Model.MostPopular){ %>
		
			    <div><%= question.Text %></div>
		
		    <%} %>
        </div>  
  
        <div class="span-7 last">  
            <h3>Spannende Fragen 2</h3>  

		    <%foreach(var question in Model.MostPopular){ %>
		
			    <div><%= question.Text %></div>
		
		    <%} %>        
        </div>  


</asp:Content>
