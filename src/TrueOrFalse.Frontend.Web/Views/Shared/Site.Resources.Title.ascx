<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TitleModel>"  %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>        

<div class="row">
    <div id="header"  class="col-md-12" style="margin-bottom: 20px;">
        <div class="container">
            <div class="pull-left">
                <a class="block" href="/"><h1>MEM<span id="uch">uch</span><span id="logo"></span></h1></a>
            </div>
        
            <div class="pull-right" style="padding-top: 15px; ">
                <% Html.RenderPartial(UserControls.Logon); %>
            </div>

        </div>
    </div>
</div>