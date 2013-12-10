﻿<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TitleModel>"  %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>        

<header>
    <div class="row">
        <div id="header"  class="col-md-12">
            <div class="container">
                <div class="pull-left">
                    <a class="block" href="/"><h1><span id="m">M</span>EM<span id="uch">uch</span>O<span id="logo"></span></h1></a>
                </div>
        
            	<div id="loginAndHelp" class="pull-right">
                    <% Html.RenderPartial(UserControls.Logon); %>
                </div>

            </div>
        </div>
    </div>
</header>