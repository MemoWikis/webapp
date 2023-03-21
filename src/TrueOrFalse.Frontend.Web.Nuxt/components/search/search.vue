<script lang="ts" setup>
import _ from 'underscore'
import { FullSearch, QuestionItem, SearchType, TopicItem, UserItem } from './searchHelper'
import { ImageStyle } from '../image/imageStyleEnum'

interface Props {
    searchType: SearchType
    showSearchIcon?: boolean
    showSearch: boolean
    placement?: string
    topicIdsToFilter?: number[]
    autoHide?: boolean
    placeholderLabel?: string
    showDefaultSearchIcon?: boolean
}

const props = withDefaults(defineProps<Props>(), {
    showSearchIcon: true,
    placement: 'bottom-start',
    placeholderLabel: 'Suche',
    topicIdsToFilter: () => []
})


const emit = defineEmits(['selectItem'])

const selectedItem = ref(null as TopicItem | QuestionItem | UserItem | null)
watch(selectedItem, (item: TopicItem | QuestionItem | UserItem | null) => {
    emit('selectItem', item)
})

const showDropdown = ref(false)
const searchTerm = ref('')

watch([searchTerm, () => props.showSearch], () => {
    if (searchTerm.value.length > 0 && props.showSearch) {
        showDropdown.value = true
        search()
    }
    else
        showDropdown.value = false
})

function inputValue(e: Event) {
    const target = e.target as HTMLTextAreaElement
    searchTerm.value = target.value
}

onBeforeMount(() => {
    switch (props.searchType) {
        case SearchType.Category:
            searchUrl.value = '/apiVue/Search/Topic'
            break;
        case SearchType.CategoryInWiki:
            searchUrl.value = '/apiVue/Search/TopicInPersonalWiki'
            break
        default:
            searchUrl.value = '/apiVue/Search/All'
    }
})

const searchUrl = ref('')
const noResults = ref(false)
const topicCount = ref(0)
const questionCount = ref(0)
const userCount = ref(0)
const userSearchUrl = ref('')

const topics = ref([] as TopicItem[])
const questions = ref([] as QuestionItem[])
const users = ref([] as UserItem[])

async function search() {
    var result = await $fetch<FullSearch>(`${searchUrl.value}?term=${encodeURIComponent(searchTerm.value)}`, {
        mode: 'no-cors',
        credentials: 'include'
    })
    if (result != null) {
        if (result.topics) {
            topics.value = result.topics
            topicCount.value = result.topicCount
        }
        if (result.questions) {
            questions.value = result.questions
            questionCount.value = result.questionCount
        }
        if (result.users) {
            users.value = result.users
            userCount.value = result.userCount
        }
        noResults.value = result.topics?.length + result.questions?.length + result.users?.length <= 0
        userSearchUrl.value = result.userSearchUrl ? result.userSearchUrl : ''
    }
}

function selectItem(item: TopicItem | QuestionItem | UserItem) {
    selectedItem.value = item
}
function openUsers() {
    navigateTo(userSearchUrl.value)
}

onMounted(() => {
    if (typeof window !== 'undefined') {
        window.addEventListener('scroll', () => { showDropdown.value = false })
    }
})
function hide() {
    showDropdown.value = false
}
</script>

<template>
    <div class="search-category-component" v-click-outside="hide">
        <form v-on:submit.prevent>
            <div class="form-group searchAutocomplete">
                <div class="searchInputContainer">
                    <input class="form-control search" :class="{ 'hasSearchIcon': props.showSearchIcon }" type="text"
                        v-bind:value="searchTerm" @input="event => inputValue(event)" autocomplete="off"
                        :placeholder="props.placeholderLabel" />
                    <font-awesome-icon icon="fa-solid fa-magnifying-glass" class="default-search-icon"
                        v-if="props.showDefaultSearchIcon" />

                </div>
            </div>
        </form>

        <VDropdown :distance="6" :triggers="[]" v-model:shown="showDropdown" no-auto-focus :auto-hide="false"
            :placement="props.placement">
            <template #popper>
                <div class="searchDropdown">
                    <div v-if="topics.length > 0" class="searchBanner">
                        <div>Themen </div>
                        <div>{{ topicCount }} Treffer</div>
                    </div>
                    <div class="searchResultItem" v-for="t in topics" @click="selectItem(t)" v-tooltip="t.Name">
                        <Image :url="t.ImageUrl" :style="ImageStyle.Topic" />
                        <div class="searchResultLabelContainer">
                            <div class="searchResultLabel body-m">{{ t.Name }}</div>
                            <div class="searchResultSubLabel body-s">{{ t.QuestionCount }} Frage<template
                                    v-if="t.QuestionCount != 1">n</template></div>
                        </div>
                    </div>
                    <div v-if="questions.length > 0" class="searchBanner">
                        <div>Fragen </div>
                        <div>{{ questionCount }} Treffer</div>
                    </div>
                    <div class="searchResultItem" v-for="q in questions" @click="selectItem(q)" v-tooltip="q.Name">
                        <Image :url="q.ImageUrl" />
                        <div class="searchResultLabelContainer">
                            <div class="searchResultLabel body-m">{{ q.Name }}</div>
                            <div class="searchResultSubLabel body-s"></div>
                        </div>
                    </div>
                    <div v-if="users.length > 0" class="searchBanner">
                        <div>Nutzer </div>
                        <div class="link" @click="openUsers()">zeige {{ userCount }} Treffer</div>
                    </div>
                    <div class="searchResultItem" v-for="u in users" @click="selectItem(u)" v-tooltip="u.Name">
                        <Image :url="u.ImageUrl" :style="ImageStyle.Author" />
                        <div class="searchResultLabelContainer">
                            <div class="searchResultLabel body-m">{{ u.Name }}</div>
                            <div class="searchResultSubLabel body-s"></div>
                        </div>
                    </div>
                    <div v-if="noResults" class="noResult">
                        <div>Kein Treffer</div>
                    </div>
                </div>

            </template>
        </VDropdown>
    </div>
</template>

<style scoped lang="less">
@import '~~/assets/shared/search.less';
@import (reference) '~~/assets/includes/imports.less';

.default-search-icon {
    position: absolute;
    margin-top: 10px;
    margin-right: 15px;
    right: 0;
    color: @memo-grey-dark;
}

.searchAutocomplete {
    z-index: 105;
    margin: 0;
}

.searchDropdown {
    max-width: 360px;
    padding: 0;
    width: calc(100vw - 32px);

    .dropdownFooter {
        line-height: 20px;
        padding: 8px;
        text-align: end;

        .dropdownLink {
            color: @memo-blue-link;
            cursor: pointer;

            &:hover {
                text-decoration: underline;
            }
        }
    }
}

.searchBanner {
    background: @memo-blue;
    color: white;
    display: flex;
    justify-content: space-between;
    align-items: center;
    flex-direction: row;
    cursor: unset;
    height: 32px;

    div {
        padding: 0 12px;
        cursor: unset;

        &.link {
            cursor: pointer;
            color: white;

            &:hover {
                text-decoration: underline;
            }
        }
    }
}

.searchInputContainer {
    display: flex;
    flex-direction: row-reverse;
    flex-wrap: nowrap;
    min-width: 250px;

    @media (max-width: 330px) {
        min-width: 200px;
    }

    .hasSearchIcon {
        padding-right: 54px;
    }

    .searchIconContainer {
        height: 34px;
        font-size: 25px;
        position: relative;
        left: -40px;
    }
}

.searchResultItem {
    padding: 4px 8px;
    display: flex;
    width: 100%;
    height: 70px;
    transition: .2s ease-in-out;
    cursor: pointer;

    &:hover {
        background: @memo-grey-lighter;
        color: @memo-blue;
    }

    .searchResultLabelContainer {
        width: 100%;
        height: 100%;
        overflow: hidden;
    }

    .searchResultLabel {
        height: 40px;
        line-height: normal;
        color: @memo-grey-darker;
        text-overflow: ellipsis;
        margin: 0;
        overflow: hidden;
        max-width: 408px;
        white-space: normal;
    }

    .searchResultSubLabel {
        color: @memo-grey-light;
        font-style: italic;
        height: 20px;
        line-height: normal;
    }


    .img-container {
        max-height: 62px;
        max-width: 62px;
        height: auto;
        margin-right: 10px;
        width: 100%;
        margin-top: 0px !important;
    }
}

.searchButton {
    svg.fa-xmark {
        color: @memo-blue;

        &:hover {
            color: @memo-green;
        }
    }
}

input,
.form-control {
    border-radius: 24px;
}

.noResult {
    height: 60px;
    display: flex;
    justify-content: center;
    align-items: center;
}
</style>