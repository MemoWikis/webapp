<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" 
	Inherits="ViewPage<WelcomeModel>"
    Title="Willkommen bei True Or False" %>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
                      
    <div class="hero-unit row" style="padding: 50px 50px 30px 50px; margin-top: -20px;" >
        <h1 style="font-size:50px;">Willkommen</h1>
        <p>Schön das Du da bist. Entscheide was Dir wichtig ist und merke es Dir - Für immer!</p>
        <p><a class="btn primary large">Mehr Erfahren &raquo;</a></p>
    </div>
    
    <div class="row" style="padding-left:50px; padding-top: 0px;">
        <h3>
            "Wenn ich über die steuer- und erbrechtliche 
            Anerkennung von homosexuellen Paaren diskutiere, kann ich gleich über Teufelsanbetung diskutieren."<br/>
        </h3>
        Wer hat das gesagt: 
        <span class="label label-success">Jörg Heider</span>
        <span class="label label-success">Helmut Kohl</span>
        <span class="label label-success">Edmund Stoiber</span>
    </div>
    
    <div class="row" style="padding-left:50px; padding-top:30px;">
        <h3>
            Welches Auto ist in der Grundausstattung teurer:
        </h3>
        
        <span class="label label-info">BMW 318i</span> oder 
        <span class="label label-info">Golf GTI</span> ?
    </div>
    
    <div class="row" style="padding-left:50px; padding-top:30px;">
        <h3>
            Wieviele Mitarbeiter arbeiten bei der Deutschen-Bahn?
        </h3>
        
        <form class="form-search">
            <div class="control-group">
                Deine Antwort:
                <input type="text" class="input-medium" style="width: 50px;">
                <button class="btn">Antworten</button> (bis zu 20% Abweichung zugelassen)
             </div>
         </form>        
    </div>
    
    <div class="row" style="padding-left:50px; padding-top:30px;">
        <h3>
            Welcher Politiker hat die 1. deutsche Autobahn eingeweiht?
        </h3>
        
        <span class="label label-important">Adolf Hitler</span>, 
        <span class="label label-important">Konrad Adenauer</span> oder
        <span class="label label-important">Helmut Ebron</span> ?
    </div>


<%--    <div class="row">
        <h3>Spannende Fragen 1</h3>  

		<%foreach(var question in Model.MostPopular){ %>
			<div><%= question.Text %></div>
		<%} %>
    </div>

    <div class="row">
        <h3>Spannende Fragen 2</h3>  
		<%foreach(var question in Model.MostPopular){ %>
			<div><%= question.Text %></div>
		<%} %>        
    </div>  --%>


</asp:Content>
