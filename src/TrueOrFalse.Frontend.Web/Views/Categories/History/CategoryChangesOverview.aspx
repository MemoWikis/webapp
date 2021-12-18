<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<CategoryChangesOverviewModel>" %>

<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/CategoryHistory") %>
    <%
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem { Text = "Bearbeitungshistorie aller Themen", Url = Links.CategoryChangesOverview(1), ToolTipText = "Bearbeitungshistorie aller Themen" });
        Model.TopNavMenu.IsCategoryBreadCrumb = false;
    %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

<div class="row">
    <div class="col-12">
        <h1 class="hidden-sm hidden-xs hidden-xxs">Bearbeitungshistorie aller Themen</h1>
        <h3 class="hidden-md hidden-lg">Bearbeitungshistorie aller Themen</h3>
    </div>
</div>
<%
    var d = 1;
    foreach (var day in Model.Days)
    {
        var changes = Model.GetCategoryChanges(day);
        if (day.Items.Count > 0 && day.Items.Any(i => i.IsVisibleToCurrentUser()))
        {
%>
        <div class="row">
            <div class="col-md-12">
                <h3><%= day.Date %></h3>
            </div>
        </div>
        <% foreach (var item in day.Items)
           {
               var i = 1;
               var panelId = Guid.NewGuid();
               var itemIsVisibleToCurrentUser = true;
               var relationChangeItem = new RelationChangeItem();
               var label = item.Typ;
               if (item.Type == CategoryChangeType.Relations)
               {
                   relationChangeItem = Model.GetRelationChange(item, changes);
                   itemIsVisibleToCurrentUser = relationChangeItem.IsVisibleToCurrentUser;
                   if (relationChangeItem.RelationAdded)
                       label += " hinzugefügt";
                   else
                       label += " entfernt";
               }

               if (itemIsVisibleToCurrentUser && item.IsVisibleToCurrentUser())
               {
                   if (item.AggregatedCategoryChangeDetailModel.Count > 1 && Model.IsAuthorOrAdmin(item))
                   {
        %>
                    <div class="panel-group row change-detail-model" id="accordion<%= panelId %>" role="tablist" aria-multiselectable="true">
                        <div class="panel panel-default">
                            <div class="panel-heading row collapsed" role="tab" id="heading<%= panelId %>" role="button" data-toggle="collapse" data-parent="#accordion<%= panelId %>" href="#collapse<%= panelId %>" aria-controls="collapse<%= panelId %>" expanded="false">
                                <div class="col-xs-3">
                                    <a class="history-link" href="<%= Links.CategoryDetail(EntityCache.GetCategoryCacheItem(item.CategoryId)) %>">
                                        <img class="history-category" src="<%= item.CategoryImageUrl %>" height="20"/>
                                    </a>
                                    <a class="history-link" href="<%= Links.CategoryDetail(EntityCache.GetCategoryCacheItem(item.CategoryId)) %>"><%= item.CategoryName %></a>
                                </div>
                                <div class="col-xs-3 col-sm-2 show-tooltip history-date" data-toggle="tooltip" data-placement="left" title="<%= item.DateTime %>">
                                    <a class="history-link" href="<%= Links.UserDetail(item.Author) %>"><%= item.AuthorName %></a>
                                    um <%= item.Time %>
                                </div>
                                <div class="col-xs-6 col-sm-7 pull-right change-detail">
                                    <div class="change-detail-label pointer"><%= label %></div>
                                    <div class="pointer"></div>
                                    <a class="btn btn-sm btn-default btn-primary display-changes pull-right memo-button history-link" role="button" href="<%= Links.CategoryHistoryDetail(item.CategoryId, item.AggregatedCategoryChangeDetailModel.Last().CategoryChangeId, item.CategoryChangeId) %>">
                                        Ansehen
                                    </a>

                                    <div class="Button dropdown DropdownButton">
                                        <% var buttonId = Guid.NewGuid(); %>
                                        <a href="#" id="<%= buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                            <i class="fa fa-ellipsis-v"></i>
                                        </a>
                                        <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonId %>">
                                            <li>
                                                <a href="<%= Links.CategoryHistory(item.CategoryId) %>" class="history-link">
                                                    <div class="dropdown-icon">
                                                        <i class="fas fa-history"></i>
                                                    </div>
                                                    Bearbeitungshistorie
                                                </a>
                                            </li>
                                        </ul>
                                    </div>

                                    <div class="chevron-container pointer">
                                        <i class="fas fa-chevron-down pull-right"></i>
                                        <i class="fas fa-chevron-up pull-right"></i>
                                    </div>

                                </div>
                            </div>
                            <div id="collapse<%= panelId %>" class="panel-collapse collapse" role="tabpanel" aria-labelledby="heading<%= panelId %>">
                                <ul class="list-group">

                                    <% foreach (var ai in item.AggregatedCategoryChangeDetailModel)
                                       {
                                    %>
                                        <li class="list-group-item row">
                                            <div class="col-xs-3">
                                                <a class="history-link" href="<%= Links.CategoryDetail(EntityCache.GetCategoryCacheItem(item.CategoryId)) %>">
                                                    <img class="history-category" src="<%= item.CategoryImageUrl %>" height="20"/>
                                                </a>
                                                <a class="history-link" href="<%= Links.CategoryDetail(EntityCache.GetCategoryCacheItem(item.CategoryId)) %>"><%= item.CategoryName %></a>
                                            </div>
                                            <div class="col-xs-3 col-sm-2 show-tooltip  history-date" data-toggle="tooltip" data-placement="left" title="<%= item.DateTime %>">
                                                <a class="history-link" href="<%= Links.UserDetail(item.Author) %>"><%= item.AuthorName %></a>
                                                um <%= item.Time %>
                                            </div>
                                            <div class="col-xs-6 col-sm-7 pull-right change-detail">
                                                <div class="change-detail-label"><%= label %></div>

                                                <a class="btn btn-sm btn-default btn-primary display-changes pull-right memo-button history-link" href="<%= Links.CategoryHistoryDetail(item.CategoryId, ai.CategoryChangeId) %>">
                                                    Ansehen
                                                </a>

                                                <div class="Button dropdown DropdownButton">
                                                    <% var listItemId = Guid.NewGuid(); %>
                                                    <a href="#" id="<%= listItemId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                                        <i class="fa fa-ellipsis-v"></i>
                                                    </a>
                                                    <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= listItemId %>">
                                                        <li>
                                                            <a href="<%= Links.CategoryHistory(item.CategoryId) %>">
                                                                <div class="dropdown-icon">
                                                                    <i class="fas fa-history"></i>
                                                                </div>
                                                                Bearbeitungshistorie
                                                            </a>
                                                        </li>
                                                    </ul>
                                                </div>

                                                <div class="change-detail-spacer">
                                                </div>
                                            </div>

                                        </li>
                                    <%
                                           } %>
                                </ul>
                            </div>
                        </div>
                    </div>
                <%
                       }
                    else
                    { %>
                    <div class="row change-detail-model">
                        <div class="col-xs-3">
                            <a class="history-link" href="<%= Links.CategoryDetail(EntityCache.GetCategoryCacheItem(item.CategoryId)) %>">
                                <img class="history-category" src="<%= item.CategoryImageUrl %>" height="20"/>
                            </a>
                            <a class="history-link" href="<%= Links.CategoryDetail(EntityCache.GetCategoryCacheItem(item.CategoryId)) %>"><%= item.CategoryName %></a>
                        </div>
                        <div class="col-xs-3 col-sm-2 show-tooltip  history-date" data-toggle="tooltip" data-placement="left" title="<%= item.DateTime %>">
                            <a class="history-link" href="<%= Links.UserDetail(item.Author) %>"><%= item.AuthorName %></a>
                            um <%= item.Time %>
                        </div>
                        <div class="col-xs-6 col-sm-7 pull-right change-detail <%= item.Type == CategoryChangeType.Relations ? "relation-detail" : "" %>">
                            <div class="change-detail-label"><%= label %></div>
                            <%
                                if (item.Type == CategoryChangeType.Relations)
                                {
                                    var relationChangeString = "";
                                    if (relationChangeItem.Type == CategoryRelationType.IsChildOf)
                                        relationChangeString = " ist übergeordnet";
                                    else if (relationChangeItem.Type == CategoryRelationType.IncludesContentOf)
                                        relationChangeString = " ist untergeordnet";

                            %>
                                <div class="related-category-name">
                                    <a class="history-link" href="<%= Links.CategoryDetail(relationChangeItem.RelatedCategory) %>">
                                        <%= relationChangeItem.RelatedCategory.Name %>
                                    </a>

                                    <%= relationChangeString %>
                                </div>
                            <%
                                    }
                                    else
                                    { %>
                                <a class="btn btn-sm btn-default btn-primary display-changes pull-right memo-button history-link" href="<%= Links.CategoryHistoryDetail(item.CategoryId, item.AggregatedCategoryChangeDetailModel.Last().CategoryChangeId, item.CategoryChangeId) %>">
                                    Ansehen
                                </a>

                                <div class="Button dropdown DropdownButton">
                                    <% var buttonId = Guid.NewGuid(); %>
                                    <a href="#" id="<%= buttonId %>" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis" type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                                        <i class="fa fa-ellipsis-v"></i>
                                    </a>
                                    <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="<%= buttonId %>">
                                        <li>
                                            <a href="<%= Links.CategoryHistory(item.CategoryId) %>">
                                                <div class="dropdown-icon">
                                                    <i class="fas fa-history"></i>
                                                </div>
                                                Bearbeitungshistorie
                                            </a>
                                        </li>
                                    </ul>
                                </div>

                                <div class="change-detail-spacer">
                                </div>
                            <% } %>
                        </div>
                    </div>

<% }
               }
               i++;
           }
        }
        d++;
    } %>

<br/>
<br/>
<div class="col-md-12 text-center">
    <div class="btn-group" role="group">
        <a type="button" class="btn btn-default" href="<%= Links.CategoryChangesOverview(Model.PageToShow - 1) %>" <%= Model.PageToShow == 1 ? "disabled" : "" %>>
            Neuere Revisionen

        </a>
        <a type="button" class="btn btn-default" href="<%= Links.CategoryChangesOverview(Model.PageToShow + 1) %>">
            Ältere Revisionen
        </a>
    </div>
</div>
<%= Scripts.Render("~/bundles/js/CategoryHistory") %>
</asp:Content>