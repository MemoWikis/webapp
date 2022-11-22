<script lang="ts" setup>
const props = defineProps(['aggregatedItem'])


</script>

<template>
    <li class="list-group-item row">
        <div class="col-xs-3">
            <a class="history-link" href="<%= Links.UserDetail(ai.Author) %>">
                <img class="history-author" src="<%= ai.AuthorImageUrl %>" height="20" />
            </a>
            <a class="history-link" href="<%= Links.UserDetail(ai.Author) %>">
                {{props.aggregatedItem.AuthorName}}
            </a>
        </div>
        <div class="col-xs-3 col-sm-2 show-tooltip" data-toggle="tooltip" data-placement="left"
            title="<%= ai.DateTime %>">
            vor {{props.aggregatedItem.ElapsedTime}}
            um {{props.aggregatedItem.Time}}
        </div>
        <div class="col-xs-6 col-sm-7 pull-right change-detail">
            <div class="change-detail-label">
                {{props.aggregatedItem.Label}}

            </div>

            <a class="btn btn-sm btn-default btn-primary display-changes pull-right memo-button history-link"
                href="<%= Links.CategoryHistoryDetail(Model.CategoryId, ai.CategoryChangeId) %>">
                Ansehen
            </a>
            <div class="change-detail-spacer">
            </div>
        </div>
    </li>

</template>