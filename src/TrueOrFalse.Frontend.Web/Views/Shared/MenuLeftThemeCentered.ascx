<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MenuLeftModel>" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<div class="mainMenuContainer">
      <nav id="mainMenuThemeCentered">
        <div class="list-group">           
            <div class="menu-section">
                <div class="sidebar-menu-container">
                    <div class="menu-item" style="height: 21px; ">
                        <a id="mainMenuBtnKnowledge" style="margin-right:3px;padding-right: 12px; border-radius:0px" class="list-group-item menu-section primary-point <%: Model.Active(MenuEntry.Knowledge)%>" href="<%= Links.Knowledge() %>">
                            <i id="mainMenuKnowledgeHeart" class="fa fa-heart fa-2x" style="color: #b13a48;"></i>
                            <span class="primary-point-text">Wissenszentrale</span>
                        </a>
                    </div>
                    <div class="menu-item" style="height: 21px;"><a id="SidebarMenuButton"><i class="fa fa-bars"></i></a></div>
                </div>
            </div>                       
        </div>
  </nav>
</div>
