<script lang="ts" setup>
import { FullSearch, QuestionItem, SearchType, TopicItem, UserItem } from './searchHelper'
import { ImageFormat } from '../image/imageFormatEnum'

interface Props {
    searchType: SearchType
    showSearchIcon?: boolean
    showSearch: boolean
    placement?: string
    topicIdsToFilter?: number[]
    autoHide?: boolean
    placeholderLabel?: string
    showDefaultSearchIcon?: boolean
    mainSearch?: boolean
    distance?: number
}

const props = withDefaults(defineProps<Props>(), {
    showSearchIcon: true,
    placement: 'bottom-start',
    placeholderLabel: 'Suche',
    topicIdsToFilter: () => [],
    distance: 6
})

const emit = defineEmits(['selectItem', 'navigateToUrl'])

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
        case SearchType.category:
            searchUrl.value = '/apiVue/Search/Topic'
            break;
        case SearchType.categoryInWiki:
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
const { $urlHelper, $logger } = useNuxtApp()

async function search() {

    type BodyType = {
        term: string
        topicIdsToFilter?: number[]
    }

    let data: BodyType = {
        term: searchTerm.value,
    }
    if ((props.searchType == SearchType.category || props.searchType == SearchType.categoryInWiki))
        data = { ...data, topicIdsToFilter: props.topicIdsToFilter }

    const result = await $fetch<FullSearch>(searchUrl.value, {
        method: 'POST',
        body: data,
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText} `, [{ response: context.response, host: context.request }])
        }
    })
    if (result != null) {
        if (result.topics) {
            topics.value = result.topics
            topics.value.forEach((t) => t.type = 'TopicItem')
            topicCount.value = result.topicCount
        }
        if (result.questions) {
            questions.value = result.questions
            questions.value.forEach((q) => q.type = 'QuestionItem')

            questionCount.value = result.questionCount
        }
        if (result.users) {
            users.value = result.users
            users.value.forEach((u) => u.type = 'UserItem')
            userCount.value = result.userCount
        }
        noResults.value = result.topics?.length + result.questions?.length + result.users?.length <= 0
        userSearchUrl.value = result.userSearchUrl ? result.userSearchUrl : ''
    }
}

function selectItem(item: TopicItem | QuestionItem | UserItem) {
    switch (item.type) {
        case 'TopicItem':
            item.url = $urlHelper.getTopicUrl(item.name, item.id)
            break;
        case 'QuestionItem':
            item.url = $urlHelper.getTopicUrlWithQuestionId(item.primaryTopicName, item.primaryTopicId, item.id)
            break;
        case 'UserItem':
            item.url = $urlHelper.getUserUrl(item.name, item.id)
            break;
    }
    selectedItem.value = item
    searchTerm.value = ''
}

function openUsers() {
    return navigateTo(userSearchUrl.value)
}

onMounted(() => {
    if (typeof window !== 'undefined') {
        window.addEventListener('scroll', () => { showDropdown.value = false })
    }
})

const searchInput = ref()

watch(() => props.showSearch, (val) => {
    if (val && searchInput.value)
        searchInput.value.focus()
})
</script>

<template>
    <LazyClientOnly>
        <div class="search-category-component">
            <form v-on:submit.prevent :class="{ 'main-search': props.mainSearch, 'open': props.showSearch }">
                <div class="form-group searchAutocomplete">
                    <div class="searchInputContainer">
                        <input class="form-control search" :class="{ 'hasSearchIcon': props.showSearchIcon }"
                            type="text" v-bind:value="searchTerm" @input="event => inputValue(event)" autocomplete="off"
                            :placeholder="props.placeholderLabel" ref="searchInput" />
                        <font-awesome-icon icon="fa-solid fa-magnifying-glass" class="default-search-icon"
                            v-if="props.showDefaultSearchIcon" />

                    </div>
                </div>
            </form>

            <VDropdown :distance="props.distance" v-model:shown="showDropdown" no-auto-focus :auto-hide="true"
                :placement="props.placement">
                <template #popper>
                    <div class="searchDropdown">
                        <div v-if="topics.length > 0" class="searchBanner">
                            <div>Themen </div>
                            <div>{{ topicCount }} Treffer</div>
                        </div>
                        <div class="searchResultItem" v-for="t in topics" @click="selectItem(t)" v-tooltip="t.name">
                            <Image :src="t.imageUrl" :format="ImageFormat.Topic" />
                            <div class="searchResultLabelContainer">
                                <div class="searchResultLabel body-m">{{ t.name }}</div>
                                <div class="searchResultSubLabel body-s">{{ t.questionCount }} Frage<template
                                        v-if="t.questionCount != 1">n</template></div>
                            </div>
                        </div>
                        <div v-if="questions.length > 0" class="searchBanner">
                            <div>Fragen </div>
                            <div>{{ questionCount }} Treffer</div>
                        </div>
                        <div class="searchResultItem" v-for="q in questions" @click="selectItem(q)" v-tooltip="q.name">
                            <Image :src="q.imageUrl" />
                            <div class="searchResultLabelContainer">
                                <div class="searchResultLabel body-m">{{ q.name }}</div>
                                <div class="searchResultSubLabel body-s"></div>
                            </div>
                        </div>
                        <div v-if="users.length > 0" class="searchBanner">
                            <div>Nutzer </div>
                            <div class="link" @click="openUsers()">zeige {{ userCount }} Treffer</div>
                        </div>
                        <div class="searchResultItem" v-for="u in users" @click="selectItem(u)" v-tooltip="u.name">
                            <Image :src="u.imageUrl" :format="ImageFormat.Author" />
                            <div class="searchResultLabelContainer">
                                <div class="searchResultLabel body-m">{{ u.name }}</div>
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

    </LazyClientOnly>
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

.main-search {
    position: relative;

    .searchInputContainer {
        width: 0px;
        min-width: 0px;
    }

    &.open {
        .searchInputContainer {
            width: 250px;
        }
    }

    .searchAutocomplete {
        position: absolute;
        right: 0;
        top: -17px;
    }
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

.search-button {
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