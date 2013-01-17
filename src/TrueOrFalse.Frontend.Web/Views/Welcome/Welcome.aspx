<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" 
	Inherits="ViewPage<WelcomeModel>"
    Title="Richtig-oder-Falsch ist eine Lernplattform. Sammle Wissen und teile es mit anderen. bei True Or False" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <link href="/Views/Welcome/Welcome.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="well well-large well-transparent lead">
        <i class="icon-spinner icon-spin icon-2x pull-left"></i> Spinner icon when loading content...
      </div>
    
    <div class="row-fluid">
        <div class="span8">
            <div class="teaser">
                <h1>Richtig-oder-Falsch ist eine Lernplattform. Sammle Wissen und teile es mit anderen.</h1>
                <a href="#">Mehr erfahren</a>
            </div>
            <div class="teaser">
                <h1 style="font-size:25px;">Wikipedia-Prinzip:</h1>
                <h2>Open-Source, gemeinfreie Lizenz für Fragen und Inhalte</h2>
                <a href="#">Mehr erfahren</a>
            </div>
            <div class="teaser">
                <div class="question-short-row">
                    <a class="question-short">"Wenn ich über die steuer- und erbrechtliche 
                        Anerkennung von homosexuellen Paaren diskutiere, kann ich gleich über Teufelsanbetung diskutieren." Wer hat das gesagt?
                    </a>
                </div>
                <div class="question-short-row">
                    <a class="question-short">
                        Wieviele Mitarbeiter arbeiten bei der Deutschen Bahn?
                    </a>
                </div>
            </div>
        </div>
        <div class="span4 teaser">
            <div>
                <input type="text" placeholder="Email-Adresse">
                <input type="text" placeholder="Passwort">
            </div>
            <div>
                <a class="zocial icon facebook"></a>
                <a class="zocial icon google"></a>
                <a class="zocial icon twitter"></a>
            </div>
        </div>
    </div>
    
    

    
     <div style="height: 100px;"></div> 

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
