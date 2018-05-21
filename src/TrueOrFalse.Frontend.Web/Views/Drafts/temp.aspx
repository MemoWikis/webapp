<%@ Page Title="temp" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<WelcomeModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Questions/Questions.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/questions") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div style="margin-top: 20px;">
        <h4>Flexbox</h4>
        <div class="flexboxx">
            <div></div>
            <div></div>
            <div></div>
        </div>
    </div>
    <div style="margin-top: 20px;">
        <h4>Dropdown ohne Control</h4>
        
        <div class="dropdown">
          <div class="dropdown-toggle" type="" id="dropdownMenu1" data-toggle="dropdown">
            Dropdown
            <span class="caret"></span>
          </div>
          <ul class="dropdown-menu" role="menu" aria-labelledby="dropdownMenu1">
            <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Action</a></li>
            <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Another action</a></li>
            <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Something else here</a></li>
            <li role="presentation" class="divider"></li>
            <li role="presentation"><a role="menuitem" tabindex="-1" href="#">Separated link</a></li>
          </ul>
        </div>
    </div>

    <div class="GradientBunt" style="height: 50px; width: 50px;">
        <i class="fa fa-bars" style="color: white; font-size: 40px; display: block; margin-left: 15%; margin-top: 20px;"></i>
    </div>
    <div style="margin-top: 20px;">
        <h4>Z-Index-Problem</h4>
        <div class="" style="width: 50px; height: 50px; background-color: red; position: relative; z-index: 1;">D1</div>
        <div class="" style="width: 50px; height: 50px; background-color: green; margin-top: 0px; position: relative; z-index: 2;">
            <div class="" style="width: 20px; height: 20px; background-color: blue; top: -10px; position: relative; z-index: auto;">D3</div>
        </div>
    </div>
    <div style="margin-top: 20px;">
        <h4>Statistik mit Icons</h4>
        <div class="StatsRow">
            <div class="StatsIcon">
                <i class="fa fa-user"> </i>
            </div>
            <div class="StatsValue">
                <span class="sparklineTotals" data-answerstrue="3" data-answersfalse="2"><canvas width="16" height="16" style="display: inline-block; width: 16px; height: 16px; vertical-align: top;"></canvas></span>
            </div>
            <div class="StatsDescription">
                
            </div>
        </div>
        <div class="StatsRow">
            <div class="StatsIcon">
                <i class="fa fa-users"> </i>
            </div>
            <div class="StatsValue">
                <span class="sparklineTotals" data-answerstrue="3" data-answersfalse="3"><canvas width="16" height="16" style="display: inline-block; width: 16px; height: 16px; vertical-align: top;"></canvas></span>
            </div>
            <div class="StatsDescription">
                
            </div>
        </div>
        <div class="StatsRow">
            <div class="StatsIcon">
                <i class="fa fa-eye"> </i>
            </div>
            <div class="StatsValue">
                52
            </div>
            <div class="StatsDescription">
               x gesehen
            </div>
        </div>
        <div class="StatsRow">
            <div class="StatsIcon">
                <i class="fa fa-thumb-tack"> </i>
            </div>
            <div class="StatsValue">
                10
            </div>
            <div class="StatsDescription">
                x in Wunschwissen aufgenommen
            </div>
        </div>
        <div class="StatsRow">
            <div class="StatsIcon">
                <i class="fa fa-star"> </i>
            </div>
            <div class="StatsValue">
                4,7
            </div>
            <div class="StatsDescription">
                durchschnittliche Wertung (Anzahl?)
            </div>
        </div>
        <div class="StatsRow">
            <div class="StatsIcon">
                <i class="fa fa-comment"> </i>
            </div>
            <div class="StatsValue">
                4
            </div>
            <div class="StatsDescription">
                Kommentare
            </div>
        </div>
    </div>

        
</asp:Content>