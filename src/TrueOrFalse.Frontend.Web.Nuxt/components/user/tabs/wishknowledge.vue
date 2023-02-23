<script lang="ts" setup>

interface Props {
    questions?: {
        title: string
        encodedPrimaryTopicName: string
        primaryTopicId: number
        id: number
    }[],
    topics?: {
        name: string
        encodedName: string
        id: number
        questionCount: number
    }[]
}
const props = defineProps<Props>()

const searchQuestion = ref('')
const currentQuestionPage = ref(1)
const questionsPerPage = ref(30)
const filteredQuestions = computed(() => {
    const rangeStart = (currentQuestionPage.value - 1) * questionsPerPage.value
    const rangeEnd = currentQuestionPage.value * questionsPerPage.value - 1

    if (searchQuestion.value.length > 0) {
        return props.questions?.filter(q => q.title.includes(searchQuestion.value)).slice(rangeStart, rangeEnd)
    }
    else {
        return props.questions?.slice(rangeStart, rangeEnd)
    }
})

const searchTopic = ref('')
const currentTopicPage = ref(1)
const topicsPerPage = ref(30)
const filteredTopics = computed(() => {
    const rangeStart = (currentTopicPage.value - 1) * topicsPerPage.value
    const rangeEnd = currentTopicPage.value * topicsPerPage.value - 1

    if (searchTopic.value.length > 0) {
        return props.topics?.filter(t => t.name.includes(searchTopic.value)).slice(rangeStart, rangeEnd)
    }
    else {
        return props.topics?.slice(rangeStart, rangeEnd)
    }
})
</script>

<template>
    <div class="wuwi-partial">
        <h4>Fragen im Wunschwissen ({{ props.questions?.length }})</h4>
        <input type="text" v-model="searchQuestion" class="search-input" />
        <div class="wuwi-list">
            <div v-for="q in filteredQuestions">
                <NuxtLink :to="`/${q.encodedPrimaryTopicName}/${q.primaryTopicId}/Lernen/${q.id}`">
                    {{ q.title }}
                </NuxtLink>
            </div>

        </div>
        <div class="pagination"> <vue-awesome-paginate v-if="props.questions" :total-items="props.questions?.length"
                :items-per-page="questionsPerPage" :max-pages-shown="5" v-model="currentQuestionPage"
                :show-ending-buttons="false" :show-breakpoint-buttons="false" prev-button-content="Vorherige"
                next-button-content="Nächste" first-page-content="Erste" last-page-content="Letzte" /></div>
    </div>

    <div class="wuwi-partial">
        <h4>Themen mit Wunschwissen ({{ props.topics?.length }})</h4>

        <input type="text" v-model="searchTopic" class="search-input" />
        <div class="wuwi-list">
            <div v-for="t in filteredTopics">
                <NuxtLink :to="`/${t.name}/${t.id}/`">
                    {{ t.name }}
                </NuxtLink>
                <span> mit {{ t.questionCount }} Fragen</span>
            </div>

        </div>
        <div class="pagination">
            <vue-awesome-paginate v-if="props.topics" :total-items="props.topics?.length" :items-per-page="topicsPerPage"
                :max-pages-shown="5" v-model="currentTopicPage" :show-ending-buttons="false"
                :show-breakpoint-buttons="false" prev-button-content="Vorherige" next-button-content="Nächste"
                first-page-content="Erste" last-page-content="Letzte" />
        </div>

    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.wuwi-partial {
    padding-top: 20px;
    padding-bottom: 20px;

    .search-input {
        border-radius: 24px;
        border: solid 1px @memo-grey-light;
        height: 34px;
        width: 360px;
        padding: 4px 12px;

        &:focus {
            border: solid 1px @memo-green;
        }
    }
}

.wuwi-partial,
.wuwi-list {
    display: flex;
    flex-direction: column;
}
</style>