<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TitleModel>"  %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>        

<header id="MasterHeader">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="row">
                    <div class="col-sm-6 col-title">
                        <a class="block title" href="/">
                            <h1><span id="m">M</span>EM<span id="uch">uch</span>O<span id="logo"></span></h1>
                        </a>
                    </div>
                    <div class="col-sm-6 col-LoginAndHelp clearfix">
            	        <div id="loginAndHelp" >
                            <% Html.RenderPartial(UserControls.Logon); %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</header>