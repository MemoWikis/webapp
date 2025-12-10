<script lang="ts" setup>

interface Props {
    questions?: {
        title: string
        primaryPageName: string
        primaryPageId: number
        id: number
    }[],
    pages?: {
        name: string
        id: number
        questionCount: number
    }[]
}
const props = defineProps<Props>()
const { t } = useI18n() // Auto-imported by Nuxt

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

const searchPage = ref('')
const currentPagePage = ref(1)
const pagesPerPage = ref(20)
const filteredPages = computed(() => {
    if (searchPage.value.length > 0) {
        return props.pages?.filter(t => t.name.includes(searchPage.value))
    }
    else {
        return props.pages
    }
})

const filteredPagesOnPage = computed(() => {
    const rangeStart = (currentPagePage.value - 1) * pagesPerPage.value
    const rangeEnd = currentPagePage.value * pagesPerPage.value - 1
    return filteredPages.value?.slice(rangeStart, rangeEnd)
})

const { $urlHelper } = useNuxtApp()
</script>

<template>
    <div class="wish-knowledge-partial">
        <h4>{{ t('user.wishKnowledge.questions.title', { count: props.questions?.length || 0 }) }}</h4>
        <div class="search-section" v-if="(props.questions ?? []).length > 0">
            <div class="search-container">
                <input type="text" v-model="searchQuestion" class="search-input"
                    :placeholder="t('user.wishKnowledge.questions.search')" />
                <div class="search-icon reset-icon" v-if="searchQuestion.length > 0" @click="searchQuestion = ''">
                    <font-awesome-icon icon="fa-solid fa-xmark" />
                </div>
                <div class="search-icon" v-else>
                    <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
                </div>
            </div>
        </div>
        <div class="wish-knowledge-list">
            <div v-for="q in filteredQuestionsOnPage" class="wish-knowledge-link">
                <NuxtLink :to="`${$urlHelper.getPageUrl(q.primaryPageName, q.primaryPageId)}/Lernen/${q.id}`">
                    {{ q.title }}
                </NuxtLink>
            </div>
            <div v-if="filteredQuestions?.length === 0 && searchQuestion.length > 0" class="search-error">
                {{ t('user.wishKnowledge.questions.noResults', { term: searchQuestion }) }}
            </div>
        </div>
        <div class="pagination">
            <vue-awesome-paginate v-if="filteredQuestions" :total-items="filteredQuestions?.length"
                :items-per-page="questionsPerPage" :max-pages-shown="5" v-model="currentQuestionPage"
                :show-ending-buttons="false" :show-breakpoint-buttons="false"
                :prev-button-content="t('user.wishKnowledge.pagination.previous')"
                :next-button-content="t('user.wishKnowledge.pagination.next')"
                :first-page-content="t('user.wishKnowledge.pagination.first')"
                :last-page-content="t('user.wishKnowledge.pagination.last')" class="hidden-xs" />

            <vue-awesome-paginate v-if="filteredQuestions" :total-items="filteredQuestions?.length"
                :items-per-page="questionsPerPage" :max-pages-shown="3" v-model="currentQuestionPage"
                :show-ending-buttons="false" :show-breakpoint-buttons="false"
                :prev-button-content="t('user.wishKnowledge.pagination.previous')"
                :next-button-content="t('user.wishKnowledge.pagination.next')" class="hidden-sm hidden-lg hidden-md" />
        </div>
    </div>

    <div class="divider"></div>

    <div class="wish-knowledge-partial">
        <h4>{{ t('user.wishKnowledge.pages.title', { count: props.pages?.length || 0 }) }}</h4>
        <div class="search-section" v-if="(props.pages ?? []).length > 0">
            <div class="search-container">
                <input type="text" v-model="searchPage" class="search-input"
                    :placeholder="t('user.wishKnowledge.pages.search')" />
                <div class="search-icon reset-icon" v-if="searchPage.length > 0" @click="searchPage = ''">
                    <font-awesome-icon icon="fa-solid fa-xmark" />
                </div>
                <div class="search-icon" v-else>
                    <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
                </div>
            </div>
        </div>
        <div class="wish-knowledge-list">
            <div v-for="f in filteredPagesOnPage" class="wish-knowledge-link">
                <NuxtLink :to="`/${f.name}/${f.id}/`">
                    {{ f.name }}
                </NuxtLink>
                <span> {{ t('user.wishKnowledge.pages.withQuestions', { count: f.questionCount }) }}</span>
            </div>
            <div v-if="filteredPages?.length === 0 && searchPage.length > 0" class="search-error">
                {{ t('user.wishKnowledge.pages.noResults', { term: searchPage }) }}
            </div>
        </div>
        <div class="pagination">
            <vue-awesome-paginate v-if="filteredPages" :total-items="filteredPages?.length"
                :items-per-page="pagesPerPage" :max-pages-shown="5" v-model="currentPagePage"
                :show-ending-buttons="false" :show-breakpoint-buttons="false"
                :prev-button-content="t('user.wishKnowledge.pagination.previous')"
                :next-button-content="t('user.wishKnowledge.pagination.next')"
                :first-page-content="t('user.wishKnowledge.pagination.first')"
                :last-page-content="t('user.wishKnowledge.pagination.last')" class="hidden-xs" />

            <vue-awesome-paginate v-if="filteredPages" :total-items="filteredPages?.length"
                :items-per-page="pagesPerPage" :max-pages-shown="3" v-model="currentPagePage"
                :show-ending-buttons="false" :show-breakpoint-buttons="false"
                :prev-button-content="t('user.wishKnowledge.pagination.previous')"
                :next-button-content="t('user.wishKnowledge.pagination.next')"
                :first-page-content="t('user.wishKnowledge.pagination.first')"
                :last-page-content="t('user.wishKnowledge.pagination.last')" class="hidden-sm hidden-md hidden-lg" />
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

.wish-knowledge-partial {
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

.wish-knowledge-partial,
.wish-knowledge-list {
    display: flex;
    flex-direction: column;

    .wish-knowledge-link {
        overflow-wrap: break-word;
        margin-bottom: 12px;
        font-size: 18px;
        background: white;
    }
}
</style>