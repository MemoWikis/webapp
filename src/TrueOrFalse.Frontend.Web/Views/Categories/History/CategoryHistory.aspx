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
            <h1><i class="fa fa-list-ul"></i>&nbsp; Bearbeitungshistorie '<%= Model.CategoryName %>'</h1>
        </div>
    </div>
    <a class="btn btn-sm btn-default" href="<%= Links.CategoryChangesOverview(1) %>">
        Zur Bearbeitungshistorie aller Themen
    </a>
    <% foreach (var day in Model.Days)
       {
           var afterRelease = ReleaseDate.IsAfterRelease(day.DateTime);
           if (day.Items.Count > 0)
           {
    %>

            <div class="row">
                <div class="col-md-12">
                    <h3><%= day.Date %></h3>
                </div>
            </div>

            <% foreach (var item in day.Items)
               {
                   if (item.IsVisibleToCurrentUser())
                   { %>

                    <div class="row change-detail-model">
                        <div class="col-xs-3">
                            <a href="<%= Links.UserDetail(item.Author) %>">
                                <img src="<%= item.AuthorImageUrl %>" height="20"/>
                            </a>
                            <b>
                                <a href="<%= Links.UserDetail(item.Author) %>"><%= item.AuthorName %></a>
                            </b>
                        </div>
                        <div class="col-xs-3 show-tooltip" data-toggle="tooltip" data-placement="left" title="<%= item.DateTime %>">
                            vor <%= item.ElapsedTime %> um <%= item.Time %>
                        </div>
                        <div class="col-xs-6 pull-right">
                            <%--                            <%if (afterRelease) {%>
                                <a class="btn btn-sm btn-default btn-primary" href="<%= Links.CategoryDetail(Model.CategoryName, Model.CategoryId, item.CategoryChangeId) %>">
                                    <i class="fa fa-desktop"></i> Revision anzeigen
                                </a>&nbsp;
                            <%} %>--%>
                            <% if (item.AggregatedCategoryChangeDetailModel.Count > 1)
                               { %>
                                <a id="DisplayChanges" class="btn btn-sm btn-default btn-primary" href="<%= Links.CategoryHistoryDetail(Model.CategoryId, item.AggregatedCategoryChangeDetailModel.Last().CategoryChangeId, item.CategoryChangeId) %>">
                                    <i class="fa fa-code-fork"></i> Änderungen anzeigen
                                </a>
                            <% }
                               else
                               { %>
                                <a id="DisplayChanges" class="btn btn-sm btn-default btn-primary" href="<%= Links.CategoryHistoryDetail(Model.CategoryId, item.CategoryChangeId) %>">
                                    <i class="fa fa-code-fork"></i> Änderungen anzeigen
                                </a>
                            <% } %>

                        </div>
                    </div>
                    <div> changeCount: <%= item.AggregatedCategoryChangeDetailModel.Count %></div>
                    <div> lastEdit: <%= item.AggregatedCategoryChangeDetailModel.First().DateCreated %></div>
                    <div> firstEdit: <%= item.AggregatedCategoryChangeDetailModel.Last().DateCreated %></div>

                    <% if (item.Type == CategoryChangeType.Relations)
                       {
                           var relationChangeItem = Model.GetRelationChange(item);
                           if (relationChangeItem.IsVisibleToCurrentUser)
                           {
                               if (relationChangeItem.Count > 0)
                               {
                    %>
                                <div> Thema hinzugefuegt</div>

                            <%
                               }
                               else
                               { %>
                                <div> Thema entfernt</div>
                        <% }
                           }

                        %>

                    <% } %>
        <% }
               }
           } %>
    <% } %>
</asp:Content>