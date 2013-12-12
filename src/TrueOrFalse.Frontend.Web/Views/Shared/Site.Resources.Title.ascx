<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TitleModel>"  %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>        

<header id="MasterHeader">
    <div class="row">
        <div class="col-md-12">
            <div class="container">
                <div class="row">
                    <div class="col-md-6">
                        <div class="pull-left">
                            <a class="block" href="/"><h1><span id="m">M</span>EM<span id="uch">uch</span>O<span id="logo"></span></h1></a>
                        </div>
                    </div>
                    <div class="col-md-6">
            	        <div id="loginAndHelp" class="pull-right">
                            <% Html.RenderPartial(UserControls.Logon); %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</header>