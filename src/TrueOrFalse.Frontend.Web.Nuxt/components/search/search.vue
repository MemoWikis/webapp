<script lang="ts" setup>
import { FullSearch, QuestionItem, SearchType, PageItem, UserItem } from './searchHelper'
import { ImageFormat } from '../image/imageFormatEnum'
import { useUserStore } from '../user/userStore'

interface Props {
    searchType: SearchType
    showSearchIcon?: boolean
    showSearch: boolean
    placement?: string
    pageIdsToFilter?: number[]
    autoHide?: boolean
    placeholderLabel?: string
    showDefaultSearchIcon?: boolean
    mainSearch?: boolean
    distance?: number
    publicOnly?: boolean
    hideCurrentUser?: boolean
}

const props = withDefaults(defineProps<Props>(), {
    showSearchIcon: true,
    placement: 'bottom-start',
    pageIdsToFilter: () => [],
    distance: 6
})

const userStore = useUserStore()

const { t, locale } = useI18n()
const placeHolderText = ref()

const setPlaceholderText = () => {
    if (props.placeholderLabel)
        placeHolderText.value = props.placeholderLabel
    else placeHolderText.value = t('label.search')
}

onBeforeMount(() => {
    setPlaceholderText()
})

watch(locale, () => {
    setPlaceholderText()
})

const emit = defineEmits(['selectItem', 'navigateToUrl'])

const selectedItem = ref(null as PageItem | QuestionItem | UserItem | null)
watch(selectedItem, (item: PageItem | QuestionItem | UserItem | null) => {
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
        case SearchType.page:
            searchUrl.value = '/apiVue/Search/Page'
            break
        case SearchType.pageInWiki:
            searchUrl.value = '/apiVue/Search/PageInPersonalWiki'
            break
        case SearchType.users:
            searchUrl.value = '/apiVue/Search/Users'
            break
        default:
            searchUrl.value = '/apiVue/Search/All'
    }
})

const searchUrl = ref('')
const noResults = ref(false)
const pageCount = ref(0)
const questionCount = ref(0)
const userCount = ref(0)
const userSearchUrl = ref('')

const pages = ref([] as PageItem[])
const questions = ref([] as QuestionItem[])
const users = ref([] as UserItem[])
const { $urlHelper, $logger } = useNuxtApp()

async function search() {

    type BodyType = {
        term: string
        pageIdsToFilter?: number[]
        includePrivatePages?: boolean
        languages: string[]
    }

    let data: BodyType = {
        term: searchTerm.value,
        languages: [locale.value]
    }
    if ((props.searchType === SearchType.page ||
        props.searchType === SearchType.pageInWiki))
        data = { ...data, pageIdsToFilter: props.pageIdsToFilter }

    if (props.publicOnly)
        data = { ...data, includePrivatePages: false }

    const result = await $api<FullSearch>(searchUrl.value, {
        method: 'POST',
        body: data,
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText} `, [{ response: context.response, host: context.request }])
        }
    })
    if (result != null) {
        if (result.pages) {
            pages.value = result.pages
            pages.value.forEach((t) => t.type = 'PageItem')
            pageCount.value = result.pageCount
        }
        if (result.questions) {
            questions.value = result.questions
            questions.value.forEach((q) => q.type = 'QuestionItem')

            questionCount.value = result.questionCount
        }
        if (result.users) {
            if (props.hideCurrentUser) {
                const usersBeforeFilter = result.users
                const usersAfterFilter = usersBeforeFilter.filter((u) => u.id !== userStore.id)
                result.users = usersAfterFilter
                if (usersAfterFilter.length < usersBeforeFilter.length) {
                    result.userCount = result.userCount - 1
                }
            }

            users.value = result.users
            users.value.forEach((u) => u.type = 'UserItem')
            userCount.value = result.userCount
        }
        noResults.value = result.pages?.length + result.questions?.length + result.users?.length <= 0
        userSearchUrl.value = result.userSearchUrl ? result.userSearchUrl : ''
    }
}

function selectItem(item: PageItem | QuestionItem | UserItem) {
    switch (item.type) {
        case 'PageItem':
            item.url = $urlHelper.getPageUrl(item.name, item.id)
            break
        case 'QuestionItem':
            item.url = $urlHelper.getPageUrlWithQuestionId(item.primaryPageName, item.primaryPageId, item.id)
            break
        case 'UserItem':
            item.url = $urlHelper.getUserUrl(item.name, item.id)
            break
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
const ariaId = useId()

const pagesInCurrentLanguage = computed(() => {
    return pages.value.filter(p => p.languageCode === locale.value)
})

const pagesInOtherLanguages = computed(() => {
    return pages.value.filter(p => p.languageCode !== locale.value)
})

const questionsInCurrentLanguage = computed(() => {
    return questions.value.filter(q => q.languageCode === locale.value)
})

const questionsInOtherLanguages = computed(() => {
    return questions.value.filter(q => q.languageCode !== locale.value)
})

</script>

<template>
    <LazyClientOnly>
        <div class="search-page-component">
            <form v-on:submit.prevent :class="{ 'main-search': props.mainSearch, 'open': props.showSearch }">
                <div class="form-group searchAutocomplete">
                    <div class="searchInputContainer">
                        <input class="form-control search" :class="{ 'hasSearchIcon': props.showSearchIcon }"
                            type="text" v-bind:value="searchTerm" @input="event => inputValue(event)" autocomplete="off"
                            :placeholder="placeHolderText" ref="searchInput" />
                        <font-awesome-icon icon="fa-solid fa-magnifying-glass" class="default-search-icon"
                            v-if="props.showDefaultSearchIcon" />
                    </div>
                </div>
            </form>

            <VDropdown :aria-id="ariaId" :distance="props.distance" v-model:shown="showDropdown" no-auto-focus :auto-hide="true"
                :placement="props.placement">
                <template #popper>
                    <div class="searchDropdown">
                        <div v-if="pagesInCurrentLanguage.length > 0" class="searchBanner">
                            <div>{{ t('search.pages') }} </div>
                            <div>{{ t('search.resultsInCurrentLanguage', { count: pagesInCurrentLanguage.length }) }}</div>
                        </div>
                        <div class="searchResultItem" v-for="p in pagesInCurrentLanguage" @click="selectItem(p)" v-tooltip="p.name">
                            <Image :src="p.imageUrl" :format="ImageFormat.Page" />
                            <div class="searchResultLabelContainer">
                                <div class="searchResultLabel body-m">{{ p.name }}</div>
                                <div class="searchResultSubLabel body-s">{{ t('search.countedQuestions', p.questionCount) }}</div>
                            </div>
                        </div>

                        <div v-if="pagesInOtherLanguages.length > 0" class="searchBanner subBanner">
                            <div>{{ t('search.pagesInOtherLanguages') }} </div>
                            <div>{{ t('search.results', { count: pagesInOtherLanguages.length }) }}</div>
                        </div>
                        <div class="searchResultItem" v-for="p in pagesInOtherLanguages" @click="selectItem(p)" v-tooltip="p.name">
                            <Image :src="p.imageUrl" :format="ImageFormat.Page" />
                            <div class="searchResultLabelContainer">
                                <div class="searchResultLabel body-m">{{ p.name }}</div>
                                <div class="searchResultSubLabel body-s">
                                    <span class="language-tag">{{ p.languageCode }}</span>
                                    {{ t('search.countedQuestions', p.questionCount) }}
                                </div>
                            </div>
                        </div>

                        <div v-if="questionsInCurrentLanguage.length > 0" class="searchBanner">
                            <div>{{ t('search.questions') }} </div>
                            <div>{{ t('search.resultsInCurrentLanguage', { count: questionsInCurrentLanguage.length }) }}</div>
                        </div>
                        <div class="searchResultItem" v-for="q in questionsInCurrentLanguage" @click="selectItem(q)" v-tooltip="q.name">
                            <Image :src="q.imageUrl" />
                            <div class="searchResultLabelContainer">
                                <div class="searchResultLabel body-m">{{ q.name }}</div>
                                <div class="searchResultSubLabel body-s"></div>
                            </div>
                        </div>

                        <div v-if="questionsInOtherLanguages.length > 0" class="searchBanner">
                            <div>{{ t('search.questionsInOtherLanguages') }} </div>
                            <div>{{ t('search.results', { count: questionsInOtherLanguages.length }) }}</div>
                        </div>
                        <div class="searchResultItem" v-for="q in questionsInOtherLanguages" @click="selectItem(q)" v-tooltip="q.name">
                            <Image :src="q.imageUrl" />
                            <div class="searchResultLabelContainer">
                                <div class="searchResultLabel body-m">{{ q.name }}</div>
                                <div class="searchResultSubLabel body-s">
                                    <span class="language-tag">{{ q.languageCode }}</span>
                                </div>
                            </div>
                        </div>

                        <div v-if="users.length > 0" class="searchBanner">
                            <div>{{ t('search.users') }}</div>
                            <div class="link" @click="openUsers()">{{ t('search.showUserResults', userCount) }}</div>
                        </div>
                        <div class="searchResultItem" v-for="u in users" @click="selectItem(u)" v-tooltip="u.name">
                            <Image :src="u.imageUrl" :format="ImageFormat.Author" />
                            <div class="searchResultLabelContainer">
                                <div class="searchResultLabel body-m">{{ u.name }}</div>
                                <div class="searchResultSubLabel body-s"></div>
                            </div>
                        </div>
                        <div v-if="noResults" class="noResult">
                            <div>{{ t('search.noResults') }}</div>
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

    &.subBanner {
        background: @memo-grey-lighter;
        color: @memo-grey-darker;
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

    &::placeholder {
        color: @memo-grey-darker;
    }
}

.noResult {
    height: 60px;
    display: flex;
    justify-content: center;
    align-items: center;
}

.language-tag {
    background-color: @memo-blue-link;
    color: white;
    font-size: 0.8em;
    padding: 2px 6px;
    border-radius: 4px;
    margin-right: 6px;
    text-transform: uppercase;
    font-style: normal;
}
</style>