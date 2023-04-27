<script lang="ts" setup>
const props = defineProps(['item', 'model'])
const relationChangeItem = ref(null)

function getRelationChangeLabel() {

}


</script>

<template>
    <!-- <template v-if="props.item.IsVisibleToCurrentUser">
        <div class="panel-group row change-detail-model" id="accordion<%= panelId %>" role="tablist"
            aria-multiselectable="true">
            <div class="panel panel-default">
                <div class="panel-heading row collapsed" id="heading<%= panelId %>" role="button" data-toggle="collapse"
                    data-parent="#accordion<%= panelId %>" href="#collapse<%= panelId %>"
                    aria-controls="collapse<%= panelId %>" expanded="false">
                    <div class="col-xs-3">
                        <a class="history-link" href="<%= Links.UserDetail(item.Author) %>">
                            <img class="history-author" src="<%= item.AuthorImageUrl %>" height="20" />
                        </a>
                        <a class="history-link" href="<%= Links.UserDetail(item.Author) %>">
                            {{props.item.AuthorName}}
                        </a>
                    </div>
                    <div class="col-xs-3 col-sm-2 show-tooltip" data-toggle="tooltip" data-placement="left"
                        title="<%= item.DateTime %>">
                        vor {{props.item.ElapsedTime}}
                        um {{props.item.Time}}
                    </div>
                    <div class="col-xs-6 col-sm-7 pull-right change-detail">
                        <div class="change-detail-label pointer">
                            {{props.item.Label}}
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
                        <TopicHistoryAggregatedItem v-for="ai in props.item.AggregatedCategoryChangeDetailModel"
                            :aggregated-item="ai" />
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
                    {{props.item.AuthorName}}
                </a>
            </div>
            <div class="col-xs-3 col-sm-2 show-tooltip" data-toggle="tooltip" data-placement="left"
                title="<%= item.DateTime %>">
                vor {{props.item.ElapsedTime}} um {{props.item.Time}}
            </div>
            <div class="col-xs-6 col-sm-7 pull-right change-detail <%= item.Type == CategoryChangeType.Relations ? '
                            relation-detail' : '" %>">

                <div class="change-detail-sub">
                    <div class="change-detail-label">
                        {{props.item.Label}}
                    </div>

                    <div v-if="props.item.CategoryId != props.model.CategoryId" class="change-detail-additional-info">
                        Unterkategorie: <a href="/<%=item.CategoryName %>/<%=item.CategoryId %>">
                            {{props.item.CategoryName}}
                        </a>
                    </div>
                    <div v-if="props.item.AffectedParents != null" class="change-detail-additional-info">
                        Von
                        <a href="/<%=item.AffectedParents[1].Name %>/<%=item.AffectedParents[1].Id %>">
                            {{props.item.AffectedParents[1].Name}}
                        </a>
                        nach
                        <a href="/<%=item.AffectedParents[0].Name %>/<%=item.AffectedParents[0].Id %>">
                            {{props.item.AffectedParents[0].Name}}
                        </a>
                    </div>
                </div>


                <div v-if="props.item.Type == TopicChangeTypeEnum.Relations" class="related-category-name">
                    <a class="history-link" href="<%= Links.CategoryDetail(relationChangeItem.RelatedCategory) %>">

                                        {{relationChangeItem.RelatedCategory.Name}}
                    </a>
                    {{getRelationChangeLabel(relationChangeItem)}}
                            </div>
                                <template v-else>
                        <a class="btn btn-sm btn-default btn-primary display-changes pull-right memo-button history-link"
                                        href="<%= Links.CategoryHistoryDetail(Model.CategoryId, item.AggregatedCategoryChangeDetailModel.Last().CategoryChangeId, item.CategoryChangeId) %>">
                                        Ansehen
                                    </a>
                                    <div class="change-detail-spacer"></div>
                                            </template>

                                                            </div>
                                                                        </div>

                                                                    </template> -->
    <!-- if (item.IsVisibleToCurrentUser() &&
                                                                    item.RelationIsVisibleToCurrentUser) {
                                                                    if (item.AggregatedCategoryChangeDetailModel.Count> 1 && PermissionCheck.IsAuthorOrAdmin(item.Author)) { %>

                                                                    <% } else { %>
        
                                                                                        <% } %> -->
</template>