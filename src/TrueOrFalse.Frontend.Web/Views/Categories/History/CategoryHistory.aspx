<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Sidebar.Master" Inherits="ViewPage<CategoryHistoryModel>" %>
<%@ Import Namespace="System.Web.Optimization" %>
<%@ Import Namespace="TrueOrFalse.Frontend.Web.Code" %>
<%@ Import Namespace="NHibernate.Criterion" %>

<asp:Content ID="Content2" ContentPlaceHolderID="Head" runat="server">
    <%= Styles.Render("~/bundles/CategoryHistory") %>
    <%
        Model.TopNavMenu.BreadCrumb.Add(new TopNavMenuItem { Text = Model.CategoryName, Url = Model.CategoryUrl, ToolTipText = Model.CategoryName });
        Model.TopNavMenu.IsCategoryBreadCrumb = false;
    %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="row" id="HeaderCategoryHistory">
        <div class="col-12">
            <h1 class="hidden-sm hidden-xs hidden-xxs">Bearbeitungshistorie '<%= Model.CategoryName %>'</h1>
            <h3 class="hidden-md hidden-lg">Bearbeitungshistorie '<%= Model.CategoryName %>'</h3>
        </div>
    </div>
    <a class="btn btn-sm btn-default" href="<%= Links.CategoryChangesOverview(1) %>">
        Zur Bearbeitungshistorie aller Themen
    </a>
    <%
        var d = 1;
        foreach (var day in Model.Days)
        {
            if (day.Items.Count > 0 && day.Items.Any(i => i.IsVisibleToCurrentUser()))
            {
    %>
        <div class="category-change-day">
            <div class="row">
                <div class="col-md-12">
                    <h3><%= day.Date %></h3>
                </div>
            </div>

            <% foreach (var model in day.Items)
               {
                   var item = model;
                   var i = 1;
                   var panelId = Guid.NewGuid();
                   var relationChangeItem = Model.GetRelationChange(item);

                   if (item.Type == CategoryChangeType.Relations)
                       item.SetLabelAndVisibility(relationChangeItem);

                   if (item.IsVisibleToCurrentUser() && item.RelationIsVisibleToCurrentUser)
                   {
                       if (item.AggregatedCategoryChangeDetailModel.Count > 1 && PermissionCheck.IsAuthorOrAdmin(item.Author))
                       {
            %>
                        <div class="panel-group row change-detail-model" id="accordion<%= panelId %>" role="tablist" aria-multiselectable="true">
                            <div class="panel panel-default">
                                <div class="panel-heading row collapsed" role="tab" id="heading<%= panelId %>" role="button" data-toggle="collapse" data-parent="#accordion<%= panelId %>" href="#collapse<%= panelId %>" aria-controls="collapse<%= panelId %>" expanded="false">
                                    <div class="col-xs-3">
                                        <a class="history-link" href="<%= Links.UserDetail(item.Author) %>">
                                            <img class="history-author" src="<%= item.AuthorImageUrl %>" height="20"/>
                                        </a>
                                        <a class="history-link" href="<%= Links.UserDetail(item.Author) %>"><%= item.AuthorName %></a>
                                    </div>
                                    <div class="col-xs-3 col-sm-2 show-tooltip" data-toggle="tooltip" data-placement="left" title="<%= item.DateTime %>">
                                        vor <%= item.ElapsedTime %>
                                        um <%= item.Time %>
                                    </div>
                                    <div class="col-xs-6 col-sm-7 pull-right change-detail">
                                        <div class="change-detail-label pointer"><%= item.Label %></div>
                                        <div class="pointer"></div>
                                        <a class="btn btn-sm btn-default btn-primary display-changes pull-right memo-button history-link" role="button" href="<%= Links.CategoryHistoryDetail(Model.CategoryId, item.AggregatedCategoryChangeDetailModel.Last().CategoryChangeId, item.CategoryChangeId) %>">
                                            Ansehen
                                        </a>
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
                                                    <a class="history-link"href="<%= Links.UserDetail(ai.Author) %>">
                                                        <img class="history-author" src="<%= ai.AuthorImageUrl %>" height="20"/>
                                                    </a>
                                                    <a class="history-link" href="<%= Links.UserDetail(ai.Author) %>"><%= ai.AuthorName %></a>
                                                </div>
                                                <div class="col-xs-3 col-sm-2 show-tooltip" data-toggle="tooltip" data-placement="left" title="<%= ai.DateTime %>">
                                                    vor <%= ai.ElapsedTime %> um <%= ai.Time %>
                                                </div>
                                                <div class="col-xs-6 col-sm-7 pull-right change-detail">
                                                    <div class="change-detail-label"><%= item.Label %></div>

                                                    <a class="btn btn-sm btn-default btn-primary display-changes pull-right memo-button history-link" href="<%= Links.CategoryHistoryDetail(Model.CategoryId, ai.CategoryChangeId) %>">
                                                        Ansehen
                                                    </a>
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
                                <a class="history-link" href="<%= Links.UserDetail(item.Author) %>">
                                    <img class="history-author" src="<%= item.AuthorImageUrl %>" height="20"/>
                                </a>
                                <a class="history-link"href="<%= Links.UserDetail(item.Author) %>"><%= item.AuthorName %></a>
                            </div>
                            <div class="col-xs-3 col-sm-2 show-tooltip" data-toggle="tooltip" data-placement="left" title="<%= item.DateTime %>">
                                vor <%= item.ElapsedTime %> um <%= item.Time %>
                            </div>
                            <div class="col-xs-6 col-sm-7 pull-right change-detail <%= item.Type == CategoryChangeType.Relations ? "relation-detail" : "" %>">
                                <div class="change-detail-label"><%= item.Label %></div>
                                <%
                                    if (item.Type == CategoryChangeType.Relations)
                                    { %>
                                    <div class="related-category-name">
                                        <a class="history-link" href="<%= Links.CategoryDetail(relationChangeItem.RelatedCategory) %>">
                                            <%= relationChangeItem.RelatedCategory.Name %>
                                        </a>

                                        <%= RelationChangeItem.GetRelationChangeLabel(relationChangeItem) %>
                                    </div>
                                <%
                                    }
                                    else
                                    { %>
                                    <a class="btn btn-sm btn-default btn-primary display-changes pull-right memo-button history-link" href="<%= Links.CategoryHistoryDetail(Model.CategoryId, item.AggregatedCategoryChangeDetailModel.Last().CategoryChangeId, item.CategoryChangeId) %>">
                                        Ansehen
                                    </a>
                                    <div class="change-detail-spacer">
                                    </div>
                                <% } %>
                            </div>
                        </div>

            <% }
                   }
                   i++;
               }
            } %>
        </div>

    <% d++;
        } %>

    <%= Scripts.Render("~/bundles/js/CategoryHistory") %>

</asp:Content>