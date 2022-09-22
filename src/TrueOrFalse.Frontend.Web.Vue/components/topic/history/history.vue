<script lang="ts" setup>

const model = ref(null)
</script>

<template>

    <div class="row" id="HeaderCategoryHistory">
        <div class="col-12">
            <h1 class="hidden-sm hidden-xs hidden-xxs">Bearbeitungshistorie '{{model.categoryName}}'</h1>
            <h3 class="hidden-md hidden-lg">Bearbeitungshistorie '{{model.categoryName}}'</h3>
        </div>
    </div>
    <a class="btn btn-sm btn-default" href="<%= Links.CategoryChangesOverview(1) %>">
        Zur Bearbeitungshistorie aller Themen
    </a>
    <TopicHistoryDay :day="model.day" />

</template>