<script lang="ts" setup>
const props = defineProps(['day', 'model'])
</script>

<template>
    <div class="category-change-day">
        <div class="row">
            <div class="col-md-12">
                <h3>
                    {{day.Date}}
                </h3>
            </div>
        </div>
        <TopicHistoryItem v-for="i in props.day.Items" :item="i" :model="props.model" />
    </div>

</template>