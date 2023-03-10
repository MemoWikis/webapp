<script lang="ts" setup>

const model = ref(null as any)
onMounted(() => {
})
</script>

<template>
    <div class="row" id="HeaderCategoryHistory">
        <div class="col-12">
            <h1 class="hidden-sm hidden-xs hidden-xxs">Bearbeitungshistorie '{{ model.categoryName }}'</h1>
            <h3 class="hidden-md hidden-lg">Bearbeitungshistorie '{{ model.categoryName }}'</h3>
        </div>
    </div>
    <LazyNuxtLink class="btn btn-sm btn-default" to="/Historie/Themen">
        Zur Bearbeitungshistorie aller Themen
    </LazyNuxtLink>

    <div v-for="day in model.day" class="category-change-day">
        <div class="row">
            <div class="col-md-12">
                <h3>
                    {{ day.Date }}
                </h3>
            </div>
        </div>
        <TopicHistoryItem v-for="i in day.Items" :item="i" :model="model" />
    </div>
</template>