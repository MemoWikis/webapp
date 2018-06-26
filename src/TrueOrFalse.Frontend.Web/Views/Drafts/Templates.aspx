<%@ Page Title="temp" Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<WelcomeModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <link href="/Views/Questions/Questions.css" rel="stylesheet" />
    <%= Scripts.Render("~/bundles/questions") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Link with Icon -->
    <div>
        <h4>Link with Icon</h4>
            For not having the icon underlined at hover.<br/>
            Use classes a.TextLinkWithIcon and span.TextSpan.<br/>
        <br/>
            <a class="TextLinkWithIcon">
                <i class="fa fa-question-circle"></i>
                <span class="TextSpan">Hilfe & mehr</span>
            </a> 
    </div>
    <!-- Header -->
    <h1>Lorem ipsum - h1 plain</h1>
    <h1><span class="ColoredUnderline">Lorem ipsum - h1 underlined</span></h1>
    <h1 class="ColoredUnderline">Lorem ipsum - h1 underlined complete</h1>
    <br/>
    <h2>Lorem ipsum - h2 plain</h2>
    <h2><span class="ColoredUnderline">Lorem ipsum - h2 underlined</span></h2>
    <h2 class="ColoredUnderline">Lorem ipsum - h2 underlined complete</h2>
    <br/>
    <h3>Lorem ipsum - h3 plain</h3>
    <h3><span class="ColoredUnderline">Lorem ipsum - h3 underlined</span></h3>
    <h3 class="ColoredUnderline">Lorem ipsum - h3 underlined complete</h3>
    <br/>
    <h4>Lorem ipsum - h4 plain</h4>
    <h4><span class="ColoredUnderline">Lorem ipsum - h4 underlined</span></h4>
    <h4 class="ColoredUnderline">Lorem ipsum - h4 underlined complete</h4>
    <br/>
        
    <div>
        <div class="alert alert-success">Test Success</div>
        <div class="alert alert-info">Test Info</div>
        <div class="alert alert-warning">Test Warning</div>
        <div class="alert alert-danger">Test Danger</div>

    </div>
    <div>
        <h4>Tooltip</h4>
        Text with tooltip                      
        <i class="fa fa-question-circle show-tooltip" title="help text" data-placement="right"></i>
    </div>
        <h4>Temp. Accordion for interaction sketching</h4>
    <div>
        <!-- temp accordion html start-->
                    <div class="panel-group" id="accordion">
                      <div class="panel panel-default">
                        <div class="panel-heading">
                          <h4 class=".ColoredText Category panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapse1">
            Überschrift Code-Abschnitt 1
                            </a>
                          </h4>
                        </div>
                        <div id="collapse1" class="panel-collapse collapse in">
                          <div class="panel-body">
<!-- temp accordion html end-->
Code-Abschnitt 1
<!-- temp accordion html start-->

                          </div>
                        </div>
                      </div>
                      <div class="panel panel-default">
                        <div class="panel-heading">
                          <h4 class=".ColoredText Category panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapse2">
            Überschrift Code-Abschnitt 2
                            </a>
                          </h4>
                        </div>
                        <div id="collapse2" class="panel-collapse collapse">
                          <div class="panel-body">
<!-- temp accordion html end-->
Code-Abschnitt 2                              
<!-- temp accordion html start-->

                          </div>
                        </div>
                      </div>
                      <div class="panel panel-default">
                        <div class="panel-heading">
                          <h4 class=".ColoredText Category panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapse3">
            Überschrift Code-Abschnitt 3
                            </a>
                          </h4>
                        </div>
                        <div id="collapse3" class="panel-collapse collapse">
                          <div class="panel-body">
<!-- temp accordion html end-->
Code-Abschnitt 3                             
<!-- temp accordion html start-->

                          </div>
                        </div>
                      </div>
                      <div class="panel panel-default">
                        <div class="panel-heading">
                          <h4 class=".ColoredText Category panel-title">
                            <a data-toggle="collapse" data-parent="#accordion" href="#collapse4">
Überschrift Code-Abschnitt 4
                            </a>
                          </h4>
                        </div>
                        <div id="collapse4" class="panel-collapse collapse">
                          <div class="panel-body">
<!-- temp accordion html end-->
Code-Abschnitt 4

<!-- temp accordion html start-->

                          </div>
                        </div>
                      </div>
                    </div>
<!-- temp accordion html end-->
    </div>
   
</asp:Content>