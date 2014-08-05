<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<TitleModel>"  %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>        

<header id="MasterHeader">
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="row HeaderMainRow">
                    <div class="col-xs-6 col-title">
                        <a id="MenuButton"><i class="fa fa-bars"></i><span class="caret"></span></a>
                        <a class="block title" href="/">
                            <%--<h1><span id="m">M</span>EM<span id="uch">uch</span>O<span id="logo"></span></h1>--%>
                            <%--<h1><span id="m">M</span>em<span id="uch">uch</span>o<span id="logo"></span></h1>--%>
                            <h1><span id="m">m</span>em<span id="uch"><span>u</span><span>c</span><span>h</span></span>o<span id="logo"></span></h1>
                            <img id="memo" src="/Images/Memo.png" />
                        </a>
                    </div>
                    <div class="col-xs-6 col-LoginAndHelp">
            	        <div id="loginAndHelp" >
                            <% Html.RenderPartial(UserControls.Logon); %>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</header>