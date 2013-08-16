<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" 
	Inherits="ViewPage<WelcomeModel>"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <title>Richtig-oder-Falsch</title>
    <link href="/Views/Welcome/Welcome.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="row">
    <div class="span12">
        <div class="row">
            <div class="span8">
                <div class="box box_darkblue">
                    <div class="box-content">
                        <h1>
                            <a href="#teaser1">
                                Richtig-oder-Falsch ist eine Lern- und Wissensplattform. 
                                Sammle Wissen und teile es mit anderen. <i class="icon-circle-arrow-right" style="" ></i>
                            </a>
                        </h1>
                    </div>
                </div>
                <div class="box box_darkblue">
                    <div class="box-content">
                        <h1>
                            <a href="#teaser2">Wikipedia-Prinzip:<br />
                            Open-Source, gemeinfreie Lizenz für Fragen und Inhalte <i class="icon-circle-arrow-right" style="" ></i></a>
                        </h1>
                    </div>
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


                <h3><a name="teaser1">Wie hilft Dir Richtig-oder-Falsch</a></h3>
                <p>
                    RIOFA hilft Dir dabei: 
                    <ul>
                        <li>
                            <b>Schneller zu lernen:</b>
                            <p>
                                RIOFA analyisert Dein Lernverhalten und 
                                versucht den optimalen Zeitpunkt, für eine 
                                benötigte Wiederholung von Wissen zu ermitteln. 
                                So kannst Du schneller lernen.
                            </p>
                        </li>
                        <li>
                            <b>Zu überblicken Was Du weist und was Du wissen möchtest.</b>
                            <p>
                            </p>
                        </li>
                        <li>
                            <b>Zu einen genauern Termin zu lernen:</b>
                            <p>
                                Eine Klassenarbeit, eine Prüfung, ein wichtiges Gespräch steht an? 
                                Du kannst Termine anlegen und bestimmen, was Du zu diesem Termin
                                als Faktenwissen abrufen möchtest.
                            </p>
                            <p>
                                Dabei hast Du genau im Blick, welches Wissen Du schon sicher kannst und
                                wo Du noch weiter üben musst. 
                            </p> 
                            <p>
                                RIOFA erinnert Dich an Termine und 
                                informiert Dich darüber was es noch zu lernen gibt. 
                            </p>
                        </li>
                        <li>
                            <b></b>
                        </li>
                    </ul>
                </p>
                <h3><a name="teaser2"></a></h3>
                <h3><a name="teaser2">Unsere Prinzipien</a></h3>
                <ul>
                    <li>
                        <b>Wikipedia Prinzip</b>
                        <p>
                            Öffentlichen Inhalte unterliegen, wie zum Beispiel Artikel bei Wikipedia, unter einer
                            Creative Commons Lizenz. Öffentliche Inhalten können also von jedermann 
                            kostenfrei und ohne Einschränkungen verwendet werden. 
                
                            <a rel="license" href="http://creativecommons.org/licenses/by/3.0/deed.de">Zum Lizenztext: 
                                <img alt="Creative Commons Lizenzvertrag" style="border-width:0" src="http://i.creativecommons.org/l/by/3.0/80x15.png" />
                            </a>
                        </p>
                    </li>
                    <li>
                        <b>Gemeinwohlorientierung</b>
                    </li>
                    <li>
                        <b>Wir werden Deine Daten niemals verkaufen.</b>
                        <p>Wir Nutzen Daten dafür, um Richtig-oder-Falsch besser zu machen.</p>
                    </li>
                    <li>
                        <b>Transparenz!</b>
                    </li>
                    <li>
                        <b>Open-Source</b>
                        <p>
                            Die Software unter der RIOFA läuft, steht unter einer Open Source Lizenz.
                            Die Quelltexte findest Du auf <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="icon-github"></i> Github</a>. 
                        </p>
                    </li>        
                </ul>
                <div style="height: 10px;"></div>        
            </div>
            
            <div class="span4">
                <div class="box" style="padding: 20px; height: 180px;">
                    <div style="width: 222px; margin-left: auto; margin-right: auto;" >
                        <!--<h2>Login</h2>-->           
                        <div  style="margin-bottom: 5px; text-align: center;">
                            <input type="text" placeholder="Email-Adresse">
                            <input type="text" placeholder="Passwort" style="margin-bottom: 0;">
                    
                        </div>
                
                        <label class="checkbox" style="width: 180px; margin-top: 3px;">
                                <input type="checkbox" style="position: relative; top: -2px;"> angemeldet bleiben
                            </label>
                
                        <div style="height: 40px; border-bottom: solid 1px #A6BADE; clear: both;">
                            <div style="float: right">
                                <button class="btn btn-primary">Anmelden</button>
                            </div>
                            <div>
                                <a style="font-size: 90%; position: relative; top: 6px;">Passwort vergessen?</a>
                            </div>
                        </div>

                        <div style="clear: both; margin-top: 5px;">
                            <div style="padding-top: 3px; margin-right: 5px; float: left;">Alternativ mit:</div> 
                            <div style="padding-top: 5px; float: right;">
                            <a class="zocial icon facebook"></a>
                            <a class="zocial icon google"></a>
                            <a class="zocial icon twitter"></a>
                            </div>
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
    </div>
</div>

</asp:Content>
