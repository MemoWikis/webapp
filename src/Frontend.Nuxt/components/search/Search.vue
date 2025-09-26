<script lang="ts" setup>
import { FullSearch, QuestionItem, SearchType, PageItem, UserItem } from './searchHelper'
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
                const removedCount = usersBeforeFilter.length - usersAfterFilter.length
                if (removedCount > 0) {
                    result.userCount -= removedCount
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

const myPages = computed(() => {
    if (!userStore.isLoggedIn) {
        return []
    }
    return pages.value.filter(p => p.creatorName === userStore.name)
})

const pagesInCurrentLanguage = computed(() => {
    if (!userStore.isLoggedIn) {
        return pages.value.filter(p => p.languageCode === locale.value)
    }
    return pages.value.filter(p => p.languageCode === locale.value && p.creatorName !== userStore.name)
})

const pagesInOtherLanguages = computed(() => {
    if (!userStore.isLoggedIn) {
        return pages.value.filter(p => p.languageCode !== locale.value)
    }
    return pages.value.filter(p => p.languageCode !== locale.value && p.creatorName !== userStore.name)
})

const myQuestions = computed(() => {
    if (!userStore.isLoggedIn) {
        return []
    }
    return questions.value.filter(q => q.creatorName === userStore.name)
})

const questionsInCurrentLanguage = computed(() => {
    if (!userStore.isLoggedIn) {
        return questions.value.filter(q => q.languageCode === locale.value)
    }
    return questions.value.filter(q => q.languageCode === locale.value && q.creatorName !== userStore.name)
})

const questionsInOtherLanguages = computed(() => {
    if (!userStore.isLoggedIn) {
        return questions.value.filter(q => q.languageCode !== locale.value)
    }
    return questions.value.filter(q => q.languageCode !== locale.value && q.creatorName !== userStore.name)
})

</script>

<template>
    <LazyClientOnly>
        <div class="search-page-component">
            <form v-on:submit.prevent :class="{ 'main-search': props.mainSearch, 'open': props.showSearch }">
                <div class="form-group searchAutocomplete">
                    <div class="searchInputContainer">
                        <input class="form-control search" :class="{ 'hasSearchIcon': props.showSearchIcon }"
                            type="text" v-bind:value="searchTerm" @input="inputValue($event)" autocomplete="off"
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
                        <div v-if="myPages.length > 0" class="searchBanner">
                            <div>{{ t('search.myPages') }} </div>
                            <div>{{ t('search.results', { count: myPages.length }) }}</div>
                        </div>
                        <SearchItem v-for="p in myPages"
                            :item="p"
                            @click="selectItem(p)" />

                        <div v-if="pagesInCurrentLanguage.length > 0" class="searchBanner">
                            <div>{{ t('search.pages') }} </div>
                            <div>{{ t('search.resultsInCurrentLanguage', { count: pagesInCurrentLanguage.length }) }}</div>
                        </div>
                        <SearchItem v-for="p in pagesInCurrentLanguage"
                            :item="p"
                            @click="selectItem(p)" />

                        <div v-if="pagesInOtherLanguages.length > 0" class="searchBanner" :class="{ 'subBanner': pagesInCurrentLanguage.length > 0 }">
                            <div>{{ t('search.pagesInOtherLanguages') }} </div>
                            <div>{{ t('search.results', { count: pagesInOtherLanguages.length }) }}</div>
                        </div>
                        <SearchItem v-for="p in pagesInOtherLanguages"
                            :item="p"
                            @click="selectItem(p)" />

                        <div v-if="myQuestions.length > 0" class="searchBanner">
                            <div>{{ t('search.myQuestions') }} </div>
                            <div>{{ t('search.results', { count: myQuestions.length }) }}</div>
                        </div>
                        <SearchItem v-for="q in myQuestions"
                            :item="q"
                            @click="selectItem(q)"
                            v-tooltip="q.name" />

                        <div v-if="questionsInCurrentLanguage.length > 0" class="searchBanner">
                            <div>{{ t('search.questions') }} </div>
                            <div>{{ t('search.resultsInCurrentLanguage', { count: questionsInCurrentLanguage.length }) }}</div>
                        </div>
                        <SearchItem v-for="q in questionsInCurrentLanguage"
                            :item="q"
                            @click="selectItem(q)"
                            v-tooltip="q.name" />

                        <div v-if="questionsInOtherLanguages.length > 0" class="searchBanner" :class="{ 'subBanner': questionsInCurrentLanguage.length > 0 }">
                            <div>{{ t('search.questionsInOtherLanguages') }} </div>
                            <div>{{ t('search.results', { count: questionsInOtherLanguages.length }) }}</div>
                        </div>
                        <SearchItem v-for="q in questionsInOtherLanguages"
                            :item="q"
                            @click="selectItem(q)" />

                        <div v-if="users.length > 0" class="searchBanner">
                            <div>{{ t('search.users') }}</div>
                            <div class="link" @click="openUsers()">{{ t('search.showUserResults', userCount) }}</div>
                        </div>
                        <SearchItem v-for="u in users"
                            :item="u"
                            @click="selectItem(u)" />
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

@dropdownWidth: calc(100vw - 32px);

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
    max-width: min(500px, @dropdownWidth);
    padding: 0;
    width: @dropdownWidth;

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
</style>