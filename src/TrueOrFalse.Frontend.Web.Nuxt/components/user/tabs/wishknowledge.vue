<script lang="ts" setup>

interface Props {
    questions?: {
        title: string
        primaryTopicName: string
        primaryTopicId: number
        id: number
    }[],
    topics?: {
        name: string
        id: number
        questionCount: number
    }[]
}
const props = defineProps<Props>()

const searchQuestion = ref('')
const currentQuestionPage = ref(1)
const questionsPerPage = ref(20)
const filteredQuestions = computed(() => {
    if (searchQuestion.value.length > 0) {
        return props.questions?.filter(q => q.title.includes(searchQuestion.value))
    }
    else {
        return props.questions
    }
})
const filteredQuestionsOnPage = computed(() => {
    const rangeStart = (currentQuestionPage.value - 1) * questionsPerPage.value
    const rangeEnd = currentQuestionPage.value * questionsPerPage.value - 1
    return filteredQuestions.value?.slice(rangeStart, rangeEnd)
})

const searchTopic = ref('')
const currentTopicPage = ref(1)
const topicsPerPage = ref(20)
const filteredTopics = computed(() => {
    if (searchTopic.value.length > 0) {
        return props.topics?.filter(t => t.name.includes(searchTopic.value))
    }
    else {
        return props.topics
    }
})

const filteredTopicsOnPage = computed(() => {
    const rangeStart = (currentTopicPage.value - 1) * topicsPerPage.value
    const rangeEnd = currentTopicPage.value * topicsPerPage.value - 1
    return filteredTopics.value?.slice(rangeStart, rangeEnd)
})

const { $urlHelper } = useNuxtApp()
</script>

<template>
    <div class="wuwi-partial">
        <h4>Fragen im Wunschwissen ({{ props.questions?.length }})</h4>
        <div class="search-section">
            <div class="search-container">
                <input type="text" v-model="searchQuestion" class="search-input" placeholder="Suche" />
                <div class="search-icon reset-icon" v-if="searchQuestion.length > 0" @click="searchQuestion = ''">
                    <font-awesome-icon icon="fa-solid fa-xmark" />
                </div>
                <div class="search-icon" v-else>
                    <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
                </div>
            </div>
        </div>
        <div class="wuwi-list">
            <div v-for="q in filteredQuestionsOnPage" class="wuwi-link">
                <NuxtLink :to="`${$urlHelper.getTopicUrl(q.primaryTopicName, q.primaryTopicId)}/Lernen/${q.id}`">
                    {{ q.title }}
                </NuxtLink>
            </div>
            <div v-if="filteredQuestions?.length == 0" class="search-error">
                Hmmm..., leider gibt es keine Frage mit "{{ searchQuestion }}"
            </div>
        </div>
        <div class="pagination">
            <vue-awesome-paginate v-if="filteredQuestions" :total-items="filteredQuestions?.length"
                :items-per-page="questionsPerPage" :max-pages-shown="5" v-model="currentQuestionPage"
                :show-ending-buttons="false" :show-breakpoint-buttons="false" prev-button-content="Vorherige"
                next-button-content="Nächste" first-page-content="Erste" last-page-content="Letzte" class="hidden-xs" />

            <vue-awesome-paginate v-if="filteredQuestions" :total-items="filteredQuestions?.length"
                :items-per-page="questionsPerPage" :max-pages-shown="3" v-model="currentQuestionPage"
                :show-ending-buttons="false" :show-breakpoint-buttons="false" prev-button-content="Vorherige"
                next-button-content="Nächste" class="hidden-sm hidden-lg hidden-md" />
        </div>

    </div>
    <div class="divider"></div>
    <div class="wuwi-partial">
        <h4>Themen mit Wunschwissen ({{ props.topics?.length }})</h4>
        <div class="search-section">

            <div class="search-container">
                <input type="text" v-model="searchTopic" class="search-input" placeholder="Suche" />
                <div class="search-icon reset-icon" v-if="searchTopic.length > 0" @click="searchTopic = ''">
                    <font-awesome-icon icon="fa-solid fa-xmark" />
                </div>
                <div class="search-icon" v-else>
                    <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
                </div>
            </div>
        </div>
        <div class="wuwi-list">
            <div v-for="t in filteredTopicsOnPage" class="wuwi-link">
                <NuxtLink :to="`/${t.name}/${t.id}/`">
                    {{ t.name }}
                </NuxtLink>
                <span> mit {{ t.questionCount }} Fragen</span>
            </div>
            <div v-if="filteredTopics?.length == 0" class="search-error">
                Huch! Wir haben kein Thema mit "{{ searchTopic }}" gefunden.
            </div>
        </div>
        <div class="pagination">
            <vue-awesome-paginate v-if="filteredTopics" :total-items="filteredTopics?.length"
                :items-per-page="topicsPerPage" :max-pages-shown="5" v-model="currentTopicPage" :show-ending-buttons="false"
                :show-breakpoint-buttons="false" prev-button-content="Vorherige" next-button-content="Nächste"
                first-page-content="Erste" last-page-content="Letzte" class="hidden-xs" />
            <vue-awesome-paginate v-if="filteredTopics" :total-items="filteredTopics?.length"
                :items-per-page="topicsPerPage" :max-pages-shown="3" v-model="currentTopicPage" :show-ending-buttons="false"
                :show-breakpoint-buttons="false" prev-button-content="Vorherige" next-button-content="Nächste"
                first-page-content="Erste" last-page-content="Letzte" class="hidden-sm hidden-md hidden-lg" />
        </div>

    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.divider {
    height: 1px;
    background: @memo-grey-lighter;
    width: 100%;
    margin-bottom: 10px;
}

.wuwi-partial {
    padding-top: 20px;
    padding-bottom: 20px;

    .search-section {
        display: flex;
        justify-content: flex-start;
        align-items: center;
        margin-bottom: 24px;

        .search-container {
            display: flex;
            justify-content: flex-end;
            align-items: center;

            .search-input {
                border-radius: 24px;
                border: solid 1px @memo-grey-light;
                height: 34px;
                width: 300px;
                padding: 4px 12px;

                &:focus {
                    border: solid 1px @memo-green;
                }
            }

            .search-icon {
                position: absolute;
                padding: 4px;
                font-size: 18px;
                margin-right: 2px;
                border-radius: 24px;
                background: white;
                height: 32px;
                width: 32px;
                display: flex;
                justify-content: center;
                align-items: center;
                color: @memo-grey-light;

                &.reset-icon {
                    color: @memo-grey-darker;
                    cursor: pointer;

                    &:hover {
                        color: @memo-blue;
                        filter: brightness(0.95)
                    }

                    &:active {
                        filter: brightness(0.85)
                    }
                }
            }
        }
    }

}

.wuwi-partial,
.wuwi-list {
    display: flex;
    flex-direction: column;

    .wuwi-link {
        overflow-wrap: break-word;
        margin-bottom: 12px;
        font-size: 18px;
        background: white;
    }
}
</style>