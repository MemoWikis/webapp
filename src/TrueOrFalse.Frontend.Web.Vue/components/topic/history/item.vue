<script lang="ts" setup>
const props = defineProps(['item'])

</script>

<template>
    <template v-if="props.item.IsVisibleToCurrentUser">
        <div class="panel-group row change-detail-model" id="accordion<%= panelId %>" role="tablist"
            aria-multiselectable="true">
            <div class="panel panel-default">
                <div class="panel-heading row collapsed" role="tab" id="heading<%= panelId %>" role="button"
                    data-toggle="collapse" data-parent="#accordion<%= panelId %>" href="#collapse<%= panelId %>"
                    aria-controls="collapse<%= panelId %>" expanded="false">
                    <div class="col-xs-3">
                        <a class="history-link" href="<%= Links.UserDetail(item.Author) %>">
                            <img class="history-author" src="<%= item.AuthorImageUrl %>" height="20" />
                        </a>
                        <a class="history-link" href="<%= Links.UserDetail(item.Author) %>">
                            <%= item.AuthorName %>
                        </a>
                    </div>
                    <div class="col-xs-3 col-sm-2 show-tooltip" data-toggle="tooltip" data-placement="left"
                        title="<%= item.DateTime %>">
                        vor <%= item.ElapsedTime %>
                            um <%= item.Time %>
                    </div>
                    <div class="col-xs-6 col-sm-7 pull-right change-detail">
                        <div class="change-detail-label pointer">
                            <%= item.Label %>
                        </div>
                        <div class="pointer"></div>
                        <a class="btn btn-sm btn-default btn-primary display-changes pull-right memo-button history-link"
                            role="button"
                            href="<%= Links.CategoryHistoryDetail(Model.CategoryId, item.AggregatedCategoryChangeDetailModel.Last().CategoryChangeId, item.CategoryChangeId) %>">
                            Ansehen
                        </a>
                        <div class="chevron-container pointer">
                            <i class="fas fa-chevron-down pull-right"></i>
                            <i class="fas fa-chevron-up pull-right"></i>
                        </div>

                    </div>
                </div>
                <div id="collapse<%= panelId %>" class="panel-collapse collapse" role="tabpanel"
                    aria-labelledby="heading<%= panelId %>">
                    <ul class="list-group">

                        <% foreach (var ai in item.AggregatedCategoryChangeDetailModel) { %>
                            <li class="list-group-item row">
                                <div class="col-xs-3">
                                    <a class="history-link" href="<%= Links.UserDetail(ai.Author) %>">
                                        <img class="history-author" src="<%= ai.AuthorImageUrl %>" height="20" />
                                    </a>
                                    <a class="history-link" href="<%= Links.UserDetail(ai.Author) %>">
                                        <%= ai.AuthorName %>
                                    </a>
                                </div>
                                <div class="col-xs-3 col-sm-2 show-tooltip" data-toggle="tooltip" data-placement="left"
                                    title="<%= ai.DateTime %>">
                                    vor <%= ai.ElapsedTime %> um <%= ai.Time %>
                                </div>
                                <div class="col-xs-6 col-sm-7 pull-right change-detail">
                                    <div class="change-detail-label">
                                        <%= item.Label %>
                                    </div>

                                    <a class="btn btn-sm btn-default btn-primary display-changes pull-right memo-button history-link"
                                        href="<%= Links.CategoryHistoryDetail(Model.CategoryId, ai.CategoryChangeId) %>">
                                        Ansehen
                                    </a>
                                    <div class="change-detail-spacer">
                                    </div>
                                </div>

                            </li>
                            <% } %>
                    </ul>
                </div>
            </div>
        </div>
    </template>

    <template v-else>
        <div class="row change-detail-model">
            <div class="col-xs-3">
                <a class="history-link" href="<%= Links.UserDetail(item.Author) %>">
                    <img class="history-author" src="<%= item.AuthorImageUrl %>" height="20" />
                </a>
                <a class="history-link" href="<%= Links.UserDetail(item.Author) %>">
                    <%= item.AuthorName %>
                </a>
            </div>
            <div class="col-xs-3 col-sm-2 show-tooltip" data-toggle="tooltip" data-placement="left"
                title="<%= item.DateTime %>">
                vor <%= item.ElapsedTime %> um <%= item.Time %>
            </div>
            <div class="col-xs-6 col-sm-7 pull-right change-detail <%= item.Type == CategoryChangeType.Relations ? '
                            relation-detail' : '" %>">

                <div class="change-detail-sub">
                    <div class="change-detail-label">
                        <%= item.Label %>
                    </div>

                    <% if (item.CategoryId !=Model.CategoryId) { %>
                        <div class="change-detail-additional-info">Unterkategorie: <a
                                href="/<%=item.CategoryName %>/<%=item.CategoryId %>">
                                <%= item.CategoryName %>
                            </a></div>
                        <% } %>
                            <% if (item.AffectedParents.Any()) { %>
                                <div class="change-detail-additional-info">Von <a
                                        href="/<%=item.AffectedParents[1].Name %>/<%=item.AffectedParents[1].Id %>">
                                        <%= item.AffectedParents[1].Name %>
                                    </a> nach <a
                                        href="/<%=item.AffectedParents[0].Name %>/<%=item.AffectedParents[0].Id %>">
                                        <%= item.AffectedParents[0].Name %>
                                    </a></div>
                                <% } %>
                </div>


                <% if (item.Type==CategoryChangeType.Relations) { %>
                    <div class="related-category-name">
                        <a class="history-link" href="<%= Links.CategoryDetail(relationChangeItem.RelatedCategory) %>">
                            <%= relationChangeItem.RelatedCategory.Name %>
                        </a>

                        <%= RelationChangeItem.GetRelationChangeLabel(relationChangeItem) %>
                    </div>
                    <% } else { %>
                        <a class="btn btn-sm btn-default btn-primary display-changes pull-right memo-button history-link"
                            href="<%= Links.CategoryHistoryDetail(Model.CategoryId, item.AggregatedCategoryChangeDetailModel.Last().CategoryChangeId, item.CategoryChangeId) %>">
                            Ansehen
                        </a>
                        <div class="change-detail-spacer">
                        </div>
    </template>
    <!-- if (item.IsVisibleToCurrentUser() &&
    item.RelationIsVisibleToCurrentUser) {
    if (item.AggregatedCategoryChangeDetailModel.Count> 1 && PermissionCheck.IsAuthorOrAdmin(item.Author)) { %>

    <% } else { %>
        
                        <% } %> -->
</template>