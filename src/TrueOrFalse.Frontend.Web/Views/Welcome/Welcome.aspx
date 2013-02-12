<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" 
	Inherits="ViewPage<WelcomeModel>"
    Title="Richtig-oder-Falsch" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <link href="/Views/Welcome/Welcome.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
        <div class="span8">
            <div class="box box_darkblue">
                <h1>
                    <a href="#teaser1">Richtig-oder-Falsch ist eine Lern- und Wissensplattform. Sammle Wissen und teile es mit anderen. <i class="icon-circle-arrow-right" style="color: white;" ></i></a>
                </h1>
            </div>
            <div class="box box_darkblue">
                <h1>
                    <a href="#teaser2">Wikipedia-Prinzip:<br />
                    Open-Source, gemeinfreie Lizenz für Fragen und Inhalte <i class="icon-circle-arrow-right" style="color: white;" ></i></a>
                </h1>
            </div>
            <div class="box">
                <h2 style="padding-left: 5px;">Das sollte jeder wissen:</h2>
                <div class="box-content">
                    <% foreach (var question in Model.MostPopular){ %>
                        <div class="question-short-row">
                            <a class="question-short"><%= question.Text %></a>
                        </div>                
                    <% } %>
                </div>
            </div>

            <div class="box">
                <h2>Das könnte Ihr Leben retten:</h2>
                <div class="box-content">
                    <% foreach (var question in Model.MostPopular){ %>
                        <div class="question-short-row">
                              <a class="question-short"><%= question.Text %></a>
                        </div>                
                    <% } %>
                </div>
            </div>
            
            <div class="box">
                <h2>Die schwierigsten Fragen (am häufigsten falsch beantwortet):</h2>
                <div class="box-content">
                    <% foreach (var question in Model.MostPopular){ %>
                        <div class="question-short-row">
                            <a class="question-short"><%= question.Text %></a>
                        </div>                
                    <% } %>
                </div>
            </div>
         
        </div>
        <div class="span4">
            <div class="box" style="padding: 20px; height: 180px;">
                <!--<h2>Login</h2>-->           
                <div  style="margin-bottom: 5px;">
                    <input type="text" placeholder="Email-Adresse">
                    <input type="text" placeholder="Passwort" style="margin-bottom: 0;">
                    <br/><a>Passwort vergessen?</a>
                </div>
                
                <div style="height: 40px; border-bottom: solid 1px #A6BADE;">
                    <label class="checkbox" style="width: 120px; margin-top: 3px;">
                        <input type="checkbox"> angemeldet bleiben
                    </label>
                
                <div style="float: right">
                    <button class="btn btn-primary">Anmelden</button>
                </div>
                </div>
                <div style="clear: both; margin-top: 10px;">
                    <div style="padding-top: 3px; margin-right: 5px; float: left;">Oder:</div> 
                    <div style="padding-top: 5px;">
                    <a class="zocial icon facebook"></a>
                    <a class="zocial icon google"></a>
                    <a class="zocial icon twitter"></a>
                    </div>
                </div>
            </div>
            
            <div class="box box-green">
                <h2>Top Fragesätze</h2>
                <div class="box-content">
                    <div class="question-short-row">
                        <a class="question-short">"Wenn ich über die steuer- und erbrechtliche 
                            Anerkennung von homosexuellen Paaren diskutiere, kann ich gleich über Teufelsanbetung diskutieren." Wer hat das gesagt?
                        </a>
                    </div>
                </div>
            </div>
            <div class="box box-green">
                <h2>Top Kategorien</h2>
                
                <div class="box-content">
                    <div class="question-short-row">
                        <a class="question-short">"Wenn ich über die steuer- und erbrechtliche 
                            Anerkennung von homosexuellen Paaren diskutiere, kann ich gleich über Teufelsanbetung diskutieren." Wer hat das gesagt?
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
    
    <h1><a name="teaser1">Über Richtig-oder-Falsch</a></h1>
    <p>
        <ul>
            <li>Effizientes Lernen</li>
            <li>Persönliches Wissensmanagement</li>
        </ul>
    </p>
    <h1><a name="teaser2">Wikipedia Prinzip</a></h1>
    
    <div style="height: 100px;"></div>

</asp:Content>
