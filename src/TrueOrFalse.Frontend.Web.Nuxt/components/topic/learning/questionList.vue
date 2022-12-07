<script lang="ts" setup>
import { QuestionListItem } from './questionListItem'
import _ from 'underscore'
const props = defineProps([
    'categoryId',
    'isAdmin',
    'expandQuestion',
    'activeQuestionId',
    'selectedPageFromActiveQuestion',
    'questionCount'])

const questions = ref([] as QuestionListItem[])

const pages = ref(0)
const pageArray = ref([] as number[])
const selectedPage = ref(1)

const pageIsLoading = ref(false)
const itemCountPerPage = ref(25)


const emit = defineEmits(['updateQuestionCount'])
const showLeftSelectionDropUp = ref(false)
const showRightSelectionDropUp = ref(false)
const questionCount = ref(0)

const hideLeftPageSelector = ref(false)
const hideRightPageSelector = ref(false)

const showLeftPageSelector = ref(false)
const showRightPageSelector = ref(false)

const centerArray = ref([] as number[])
const leftSelectorArray = ref([] as number[])
const rightSelectorArray = ref([] as number[])

function setPaginationRanges(sP: number) {
    if ((sP - 2) <= 2) {
        hideLeftPageSelector.value = true
    };
    if ((sP + 2) >= pageArray.value.length) {
        hideRightPageSelector.value = true
    };

    let leftArray = [];
    let cA = [];
    let rightArray = [];

    if (pageArray.value.length >= 8) {

        cA = _.range(sP - 2, sP + 3);
        cA = cA.filter(e => e >= 2 && e <= pageArray.value.length - 1)

        leftArray = _.range(2, cA[0]);
        rightArray = _.range(cA[cA.length - 1] + 1, pageArray.value.length)

        centerArray.value = cA
        leftSelectorArray.value = leftArray
        rightSelectorArray.value = rightArray

    } else {
        centerArray.value = pageArray.value
    }
}
async function updatePageCount(sP: number) {
    selectedPage.value = sP
    showLeftSelectionDropUp.value = false
    showRightSelectionDropUp.value = false

    if (typeof questions.value[0] != "undefined")
        pages.value = Math.ceil(questionCount.value / itemCountPerPage.value)
    else
        pages.value = 1

    await nextTick()
    setPaginationRanges(sP)
    pageIsLoading.value = false
}

async function loadQuestions(page: Number) {
    pageIsLoading.value = true

    var result = await $fetch<any>('/apiVue/VueQuestionList/LoadQuestions/', {
        method: 'POST', body: {
            itemCountPerPage: itemCountPerPage.value,
            pageNumber: selectedPage.value,
        }, mode: 'cors', credentials: 'include'
    })
    if (result != null) {
        questions.value = result
        updatePageCount(selectedPage.value)
        emit('updateQuestionCount')
    }
}
function loadPreviousQuestions() {
    if (selectedPage.value != 1)
        loadQuestions(selectedPage.value - 1)
}

function loadNextQuestions() {
    if (selectedPage.value != pageArray.value.length)
        loadQuestions(selectedPage.value + 1)
}
</script>

<template>
    <div class="col-xs-12 questionListComponent" id="QuestionListComponent">

        <TopicLearningQuestion v-for="(q, index) in questions" :question="q"
            :is-last-item="index == (questions.length - 1)" :session-index="index"
            :expand-question="props.expandQuestion" />

        <div id="QuestionListPagination" v-show="questions.length > 0">
            <ul class="pagination col-xs-12 row justify-content-xs-center" v-if="pageArray.length <= 8">
                <li class="page-item page-btn" :class="{ disabled: selectedPage == 1 }">
                    <span class="page-link" @click="loadPreviousQuestions()">Vorherige</span>
                </li>
                <li class="page-item" v-for="(p, key) in pageArray" @click="loadQuestions(p)"
                    :class="{ selected: selectedPage == p }">
                    <span class="page-link">{{ p }}</span>
                </li>
                <li class="page-item page-btn" :class="{ disabled: selectedPage == pageArray.length }">
                    <span class="page-link" @click="loadNextQuestions()">Nächste</span>
                </li>
            </ul>

            <ul class="pagination col-xs-12 row justify-content-xs-center" v-else>
                <li class="page-item col-auto page-btn" :class="{ disabled: selectedPage == 1 }">
                    <span class="page-link" @click="loadPreviousQuestions()">Vorherige</span>
                </li>
                <li class="page-item col-auto" @click="loadQuestions(1)" :class="{ selected: selectedPage == 1 }">
                    <span class="page-link">1</span>
                </li>
                <li class="page-item col-auto" v-show="selectedPage == 5">
                    <span class="page-link">2</span>
                </li>
                <li class="page-item col-auto" v-show="showLeftPageSelector" data-toggle="dropdown" aria-haspopup="true"
                    aria-expanded="true">
                    <span class="page-link" @click.this="showLeftSelectionDropUp = !showLeftSelectionDropUp">
                        <div class="dropup" @click.this="showLeftSelectionDropUp = !showLeftSelectionDropUp">
                            <div class="dropdown-toggle" type="button" id="DropUpMenuLeft" data-toggle="dropdown"
                                aria-haspopup="true" aria-expanded="false"
                                @click="showLeftSelectionDropUp = !showLeftSelectionDropUp">
                                ...
                            </div>
                            <ul id="DropUpMenuLeftList" class="pagination dropdown-menu"
                                aria-labelledby="DropUpMenuLeft" v-show="showLeftSelectionDropUp">
                                <li class="page-item" v-for="p in leftSelectorArray" @click="loadQuestions(p)">
                                    <span class="page-link">{{ p }}</span>
                                </li>
                            </ul>
                        </div>
                    </span>
                </li>
                <li class="page-item col-auto" v-for="(p, key) in centerArray" @click="loadQuestions(p)"
                    :class="{ selected: selectedPage == p }">
                    <span class="page-link">{{ p }}</span>
                </li>

                <li class="page-item col-auto" v-show="showRightPageSelector" data-toggle="dropdown"
                    aria-haspopup="true" aria-expanded="true">
                    <span class="page-link" @click.this="showRightSelectionDropUp = !showRightSelectionDropUp">
                        <div class="dropup" @click.this="showRightSelectionDropUp = !showRightSelectionDropUp">
                            <div class="dropdown-toggle" type="button" id="DropUpMenuRight" data-toggle="dropdown"
                                aria-haspopup="true" aria-expanded="false"
                                @click="showRightSelectionDropUp = !showRightSelectionDropUp">
                                ...
                            </div>
                            <ul id="DropUpMenuRightList" class="pagination dropdown-menu"
                                aria-labelledby="DropUpMenuLeft" v-show="showRightSelectionDropUp">
                                <li class="page-item" v-for="p in rightSelectorArray" @click="loadQuestions(p)">
                                    <span class="page-link">{{ p }}</span>
                                </li>
                            </ul>
                        </div>
                    </span>
                </li>
                <li class="page-item col-auto" v-show="selectedPage == pageArray.length - 4">
                    <span class="page-link">{{ pageArray.length - 1 }}</span>
                </li>
                <li class="page-item col-auto" @click="loadQuestions(pageArray.length)"
                    :class="{ selected: selectedPage == pageArray.length }">
                    <span class="page-link">{{ pageArray.length }}</span>
                </li>
                <li class="page-item col-auto page-btn" :class="{ disabled: selectedPage == pageArray.length }">
                    <span class="page-link" @click="loadNextQuestions()">Nächste</span>
                </li>
            </ul>
        </div>
    </div>
</template>

<style lang="less" scoped>
.drop-down-question-sort {
    display: flex;
    flex-wrap: wrap;
    font-size: 18px;
    justify-content: space-between;
    padding-right: 0;
}
</style>