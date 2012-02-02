<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" 
	Inherits="ViewPage<WelcomeModel>"
    Title="Willkommen bei True Or False" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    
    
      <!-- Main hero unit for a primary marketing message or call to action -->
      <div class="hero-unit">
        <h1>Willkommen</h1>
        <p>Schön das Du da bist. Entscheide was Dir wichtig ist und merke es Dir - Für immer!</p>
        <p><a class="btn primary large">Mehr Erfahren &raquo;</a></p>
      </div>
      


       <hr/>

        <div class="span-6 colborder">  
            <h3>Spannende Fragen 1</h3>  

		    <%foreach(var question in Model.MostPopular){ %>
		
			    <div><%= question.Text %></div>
		
		    <%} %>
        </div>  
  
        <div class="span-6 last">  
            <h3>Spannende Fragen 2</h3>  

		    <%foreach(var question in Model.MostPopular){ %>
		
			    <div><%= question.Text %></div>
		
		    <%} %>        
        </div>  


</asp:Content>
