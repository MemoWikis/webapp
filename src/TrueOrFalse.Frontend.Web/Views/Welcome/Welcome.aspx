<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" 
	Inherits="ViewPage<WelcomeModel>"%>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <title>Richtig-oder-Falsch</title>
    <link href="/Views/Welcome/Welcome.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
<div class="row">
    <div class="col-md-12">
        <div class="row">
            <div class="col-md-8">
                <div class="box box_darkblue">
                    <div class="box-content">
                        <h1>
                            <a href="#teaser1">Richtig-oder-Falsch ist eine Lern- und Wissensplattform. </a>
                            <ul style="margin-top: 10px;">
                                
                                <li><a href="#teaser1" style="font-size: 16px;">Wie hilft Dir Richtig-oder-Falsch? <i class="fa fa-arrow-circle-right" style="" ></i></a></li>
                                <li><a href="#teaser2" style="font-size: 16px;">Wikipedia Prinzip und mehr (prinzipielles) <i class="fa fa-arrow-circle-right" style="" ></i></a></li>
                                <li><a href="#teaser3" style="font-size: 16px;">Wer sind wir? <i class="fa fa-arrow-circle-right" style="" ></i></a></li>
                            </ul>
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
                                RIOFA analysiert Dein Lernverhalten und 
                                versucht den optimalen Zeitpunkt für eine 
                                benötigte Wiederholung von Wissen zu ermitteln. 
                                So kannst Du schneller lernen.
                            </p>
                            <p>
                                (Die Analyse von Lernverhalten funktioniert nur, wenn wir viele Daten sammeln: 
                                Zu diesem Thema mehr hier: <a href="/Hilfe/DatenSicherheit">Hilfe Datenansicht</a>)
                            </p>
                        </li>
                        <li>
                            <b>Zu überblicken Was Du weist und was Du wissen möchtest.</b>
                            <p>
                                Du möchtest Dir 50, 500, 5000 (oder mehr) Fakten merken. 
                                Wir helfen Dir dabei den Überblick zu behalten. 
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
                                informiert Dich darüber, was es noch zu lernen gibt. 
                            </p>
                        </li>
                    </ul>
                </p>

                <h3><a name="teaser2">Unsere Prinzipien</a></h3>
                <ul>
                    <li>
                        <b>Wikipedia Prinzip</b>
                        <p>
                            In RIOFA unterliegen Öffentlichen Inhalte einer
                            Creative Commons Lizenz. Genau wie die Einträge bei Wikipedia.
                            
                            Öffentliche RIOFA-Inhalte können also von jedermann 
                            kostenfrei und ohne Einschränkungen verwendet werden. 
                
                            <a rel="license" href="http://creativecommons.org/licenses/by/3.0/deed.de">Zum Lizenztext: 
                                <img alt="Creative Commons Lizenzvertrag" style="border-width:0" src="http://i.creativecommons.org/l/by/3.0/80x15.png" />
                            </a>
                        </p>
                    </li>
                    <li>
                        <b>Wir werden Deine Daten niemals verkaufen.</b>
                        <p>Wir Nutzen Daten dafür, um Richtig-oder-Falsch besser zu machen.</p>
                    </li>
                    <li>
                        <b>Gemeinwohlorientierung</b>
                    </li>
                    <li>
                        <b>Transparenz!</b>
                    </li>
                    <li>
                        <b>Open-Source</b>
                        <p>
                            Die Software unter der RIOFA läuft, steht unter einer Open Source Lizenz.
                            Die Quelltexte findest Du auf <a href="https://github.com/TrueOrFalse/TrueOrFalse"><i class="fa fa-github"></i> Github</a>. 
                        </p>
                    </li>        
                </ul>
                <div style="height: 10px;"></div>        
                
                <h3><a name="teaser3">Team</a></h3>
                <div style="width: 280px;">
                    
                
                    <div style="width: 128px; float: left; margin: 0px 20px 0 0px; ">
                        <img src="http://www.gravatar.com/avatar/b937ba0e44b611a418f38cb24a8e18ea?s=128"/>
                         <br/> <b>Robert</b> (Gründer) <br/>

                    </div>
                
                    <div style="width: 128px; float: left; ">
                        <img src="/Images/no-profile-picture-128.png"/>  
                        <br/> <b>Jule </b>(Gründerin) <br/> 
                    </div>
                    <div style="clear:both"></div>
                    <div style="margin-top: 10px;">
                        <p>
                            Eine spannende Reise: Wir möchten nicht nur <a href="#teaser1">das Lernen von Faktenwissen vereinfachen</a>, 
                            sondern auch ein stabiles <a href="#teaser2">gemeinwohlorientiertes Unternehmen</a> aufbauen. 
                        </p>
                        <p>
                            Wir konzepieren, programmieren und gestalten.
                        </p>
                    </div>
                </div>
                
            </div>
            
            <div class="col-md-4">
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
