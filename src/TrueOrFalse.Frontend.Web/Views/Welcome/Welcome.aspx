﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.MenuNo.Master" 
	Inherits="ViewPage<WelcomeModel>"
    Title="Richtig-oder-Falsch" %>

<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="Head">
    <link href="/Views/Welcome/Welcome.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row-fluid">
        <div class="span8">
            <div class="box">
                <h1>
                    <a href="#teaser1">Richtig-oder-Falsch ist eine Lernplattform. Sammle Wissen und teile es mit anderen. <i class="icon-circle-arrow-right" style="color: white;" ></i></a>
                </h1>
            </div>
            <div class="box">
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
            <div class="teaser box1">
                <h2>Box-Template 1</h2>
                    <div class="box-content1" style="height:40px;"></div>
            </div>
            <div class="teaser box2">
                <h2>Box-Template 2</h2>
                    <div class="box-content1" style="height:40px;"></div>
            </div>
            <div class="teaser box2">
                <h2>Box-Template 3</h2>
                    <div class="box-content1 box3" style="height:40px;"></div>
            </div>
            <div class="teaser box2">
                <h2>Box-Template 4</h2>
                    <div class="box-content1 box4" style="height:40px;"></div>
            </div>
            <div class="teaser box2">
                <h2>Box-Template 5</h2>
                    <div class="box-content1 box5" style="height:40px;"></div>
            </div>
            <div class="teaser box1">
                <h2>Box-Template 6</h2>
                    <div class="box-content1 box4" style="height:40px;"></div>
            </div>
        </div>
        <div class="span4">
            <div class="box">
                <h2>Login</h2>                
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
            
            <div class="box">
                <h2>Top Fragesätze</h2>
                <div class="box-content">
                    <div class="question-short-row">
                        <a class="question-short">"Wenn ich über die steuer- und erbrechtliche 
                            Anerkennung von homosexuellen Paaren diskutiere, kann ich gleich über Teufelsanbetung diskutieren." Wer hat das gesagt?
                        </a>
                    </div>
                </div>
            </div>
            <div class="box">
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
        djlköasdf
        adfads
        fasdfasdfasdfasdkjfölk
    </p>
    <h1><a name="teaser2">Wikipedia Prinzip</a></h1>
    
    <div style="height: 100px;"></div>

</asp:Content>
