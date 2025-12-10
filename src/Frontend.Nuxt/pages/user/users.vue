<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'
import { BreadcrumbItem } from '~~/components/header/breadcrumbItems'
import { useLoadingStore } from '~/components/loading/loadingStore'
import { UserResult } from '~~/components/users/userResult'

const { t, locales } = useI18n()
const loadingStore = useLoadingStore()

const userCount = ref(200)
const currentPage = ref(1)
const usersPerPageCount = ref(20)
const searchTerm = ref('')

enum SearchUsersOrderBy {
    None = -1,
    Rank = 0,
    WishCount = 1
}
const orderBy = ref(SearchUsersOrderBy.Rank)

interface GetResponse {
    users: UserResult[]
    totalItems: number
}

const headers = useRequestHeaders(['cookie']) as HeadersInit
const config = useRuntimeConfig()

const { data: totalUserCount } = await useLazyFetch<number>('/apiVue/Users/GetTotalUserCount', {
    credentials: 'include',
    mode: 'cors',
    onRequest({ options }) {
        if (import.meta.server) {
            options.headers = new Headers(headers)
            options.baseURL = config.public.serverBase
        }
    },
    default: () => null
})

watch(totalUserCount, (newTotalUserCount) => {
    if (newTotalUserCount != null)
        userCount.value = newTotalUserCount
})
const { $logger } = useNuxtApp()
const selectedLanguages = ref<string[]>(locales.value.map(locale => locale.code))
const debouncedSearchTerm = ref('')

const { data: pageData, status } = await useFetch<GetResponse>('/apiVue/Users/Get', {
    query: {
        page: currentPage,
        pageSize: usersPerPageCount,
        languages: selectedLanguages,
        searchTerm: debouncedSearchTerm,
        orderBy: orderBy
    },
    credentials: 'include',
    mode: 'cors',
    watch: [debouncedSearchTerm, currentPage, orderBy, selectedLanguages],
    onRequest({ options }) {
        if (import.meta.server) {
            options.headers = new Headers(headers)
            options.baseURL = config.public.serverBase
        }
    },
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    },
})

// Manual debounce implementation
let searchTimeout: NodeJS.Timeout | null = null

watch(searchTerm, (newSearchTerm) => {
    if (searchTimeout) {
        clearTimeout(searchTimeout)
    }

    searchTimeout = setTimeout(() => {
        debouncedSearchTerm.value = newSearchTerm
        currentPage.value = 1
    }, 300)
})

watch(pageData, (newPageData) => {
    if (newPageData == null || newPageData == undefined)
        return
    if (newPageData != null && newPageData.totalItems > 0) {
        userCount.value = newPageData.totalItems
    }
})

watch(searchTerm, () => {
    currentPage.value = 1
})

watch(status, (newStatus) => {
    if (newStatus == null || newStatus == undefined)
        return

    if (newStatus === 'pending')
        loadingStore.startLoading()
    else loadingStore.stopLoading()
})

const emit = defineEmits(['setBreadcrumb', 'setPage'])

onMounted(() => {
    emit('setPage', SiteType.Default)
    const breadcrumbItem: BreadcrumbItem = {
        name: t('usersOverview.title'),
        url: t('url.users')
    }
    emit('setBreadcrumb', [breadcrumbItem])
})

const getSelectedOrderLabel = computed(() => {
    switch (orderBy.value) {
        case SearchUsersOrderBy.Rank:
            return t('usersOverview.sort.options.rank')
        case SearchUsersOrderBy.WishCount:
            return t('usersOverview.sort.options.wishKnowledge')
        default:
            return t('usersOverview.sort.options.notSelected')
    }
})

useHead(() => ({
    link: [
        {
            rel: 'canonical',
            href: `${config.public.officialBase}/Nutzer`
        },
    ],
    meta: [
        {
            name: 'description',
            content: t('usersOverview.meta.description')
        },
        {
            property: 'og:title',
            content: t('usersOverview.meta.title')
        },
        {
            property: 'og:url',
            content: `${config.public.officialBase}/Nutzer`
        },
    ]
}))

const ariaId = useId()
const ariaId2 = useId()

const toggleLanguage = (code: string) => {
    const index = selectedLanguages.value.indexOf(code)
    if (index === -1) {
        selectedLanguages.value.push(code)
    } else {
        selectedLanguages.value.splice(index, 1)
    }
    currentPage.value = 1
}
</script>

<template>
    <div class="main-content">
        <div class="users-header">
            <h1>{{ t('usersOverview.title') }}</h1>
        </div>

        <div class="row content">
            <div class="col-xs-12 col-sm-12 users-title">
                <div class="overline-s no-line" v-if="pageData && pageData.totalItems != null && pageData.totalItems <= 0 && searchTerm.length > 0">
                    {{ t('usersOverview.search.noResults', { term: searchTerm }) }}
                </div>
                <div class="overline-s no-line" v-else-if="pageData && pageData.totalItems != null && pageData.totalItems > 0 && searchTerm.length > 0">
                    {{ t('usersOverview.search.results', { term: searchTerm, count: pageData.totalItems }) }}
                </div>
                <div class="overline-s no-line" v-else>
                    {{ t('usersOverview.search.allUsers') }}
                    <template v-if="totalUserCount != null"> ({{ totalUserCount }})</template>
                </div>
            </div>
            <div class="col-xs-12 col-sm-12 users-options">
                <div class="search-section">
                    <div class="search-container">
                        <input type="text" v-model="searchTerm" class="search-input" :placeholder="t('usersOverview.search.placeholder')" />
                        <div class="search-icon reset-icon" v-if="searchTerm.length > 0" @click="searchTerm = ''">
                            <font-awesome-icon icon="fa-solid fa-xmark" />
                        </div>
                        <div class="search-icon" v-else>
                            <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
                        </div>
                    </div>
                </div>
                <div class="filter-options">
                    <div class="language-section">

                        <div class="language-dropdown">
                            <VDropdown :aria-id="ariaId" :distance="0">
                                <div class="language-select">
                                    <div class="select-label">
                                        <font-awesome-icon icon="fa-solid fa-language" />
                                        <div class="language-label">{{ t('usersOverview.contentLanguageLabel')
                                        }}</div>
                                    </div>

                                    <font-awesome-icon icon="fa-solid fa-chevron-down" class="chevron" />

                                </div>

                                <template #popper>
                                    <div class="dropdown-row select-row" v-for="locale in locales"
                                        :key="locale.code">
                                        <div class="language-checkbox" @click="toggleLanguage(locale.code)"
                                            @keydown.space.prevent="toggleLanguage(locale.code)"
                                            @keydown.enter.prevent="toggleLanguage(locale.code)"
                                            :class="{ 'active': selectedLanguages.includes(locale.code) }"
                                            role="checkbox"
                                            :aria-checked="selectedLanguages.includes(locale.code)"
                                            tabindex="0">
                                            <font-awesome-icon
                                                :icon="selectedLanguages.includes(locale.code) ? 'fa-solid fa-square-check' : 'fa-regular fa-square'"
                                                class="checkbox-icon" />
                                            <span class="checkbox-text">{{ locale.name }}</span>
                                        </div>
                                    </div>
                                </template>
                            </VDropdown>
                        </div>
                    </div>
                    <div class="sort-section">
                        <font-awesome-icon icon="fa-solid fa-sort" />
                        <div class="sort-label">{{ t('usersOverview.sort.label') }}</div>
                        <div class="orderby-dropdown">
                            <VDropdown :aria-id="ariaId2" :distance="0">
                                <div class="orderby-select">
                                    <div>
                                        {{ getSelectedOrderLabel }}
                                    </div>
                                    <font-awesome-icon icon="fa-solid fa-chevron-down" class="chevron" />
                                </div>

                                <template #popper="{ hide }">
                                    <div class="dropdown-row select-row"
                                        @click="orderBy = SearchUsersOrderBy.Rank; hide()"
                                        :class="{ 'active': orderBy === SearchUsersOrderBy.Rank }">
                                        <div class="dropdown-label select-option">
                                            {{ t('usersOverview.sort.options.rank') }}
                                        </div>
                                    </div>
                                    <div class="dropdown-row"
                                        @click="orderBy = SearchUsersOrderBy.WishCount; hide()"
                                        :class="{ 'active': orderBy === SearchUsersOrderBy.WishCount }">
                                        <div class="dropdown-label select-option">
                                            {{ t('usersOverview.sort.options.wishKnowledge') }}
                                        </div>
                                    </div>
                                </template>
                            </VDropdown>
                        </div>
                    </div>
                </div>
            </div>

            <template v-if="pageData">
                <div class="row usercard-container">
                    <TransitionGroup name="usercard">
                        <UsersCard v-for="u in pageData.users" :user="u" :key="u.id" />
                    </TransitionGroup>
                </div>

                <div class="col-xs-12 empty-page-container" v-if="pageData.users.length <= 0 && searchTerm.length > 0">
                    <div class="empty-page">
                        {{ t('usersOverview.search.noUserWithName', { term: searchTerm }) }}
                    </div>
                </div>

                <div class="col-xs-12" v-if="searchTerm.length === 0 && pageData.users.length > 0">
                    <div class="pagination hidden-xs">
                        <vue-awesome-paginate v-if="currentPage > 0 && totalUserCount != null && totalUserCount > 0" :total-items="totalUserCount" :items-per-page="20" :max-pages-shown="5" v-model="currentPage" :show-ending-buttons="true"
                            :show-breakpoint-buttons="false">
                            <template #first-page-button>
                                <font-awesome-layers>
                                    <font-awesome-icon :icon="['fas', 'chevron-left']" transform="left-3" />
                                    <font-awesome-icon :icon="['fas', 'chevron-left']" transform="right-3" />
                                </font-awesome-layers>
                            </template>
                            <template #prev-button>
                                <font-awesome-icon :icon="['fas', 'chevron-left']" />
                            </template>
                            <template #next-button>
                                <font-awesome-icon :icon="['fas', 'chevron-right']" />
                            </template>
                            <template #last-page-button>
                                <font-awesome-layers>
                                    <font-awesome-icon :icon="['fas', 'chevron-right']" transform="left-3" />
                                    <font-awesome-icon :icon="['fas', 'chevron-right']" transform="right-3" />
                                </font-awesome-layers>
                            </template>
                        </vue-awesome-paginate>
                    </div>
                    <div class="pagination hidden-sm hidden-md hidden-lg">
                        <vue-awesome-paginate v-if="currentPage > 0 && userCount != null && userCount > 0" :total-items="userCount" :items-per-page="20" :max-pages-shown="3" v-model="currentPage" :show-ending-buttons="true"
                            :show-breakpoint-buttons="false">
                            <template #first-page-button>
                                <font-awesome-layers>
                                    <font-awesome-icon :icon="['fas', 'chevron-left']" transform="left-3" />
                                    <font-awesome-icon :icon="['fas', 'chevron-left']" transform="right-3" />
                                </font-awesome-layers>
                            </template>
                            <template #prev-button>
                                <font-awesome-icon :icon="['fas', 'chevron-left']" />
                            </template>
                            <template #next-button>
                                <font-awesome-icon :icon="['fas', 'chevron-right']" />
                            </template>
                            <template #last-page-button>
                                <font-awesome-layers>
                                    <font-awesome-icon :icon="['fas', 'chevron-right']" transform="left-3" />
                                    <font-awesome-icon :icon="['fas', 'chevron-right']" transform="right-3" />
                                </font-awesome-layers>
                            </template>
                        </vue-awesome-paginate>
                    </div>
                </div>
                <div class="info-bar" v-else-if="pageData.users.length < pageData.totalItems">
                    {{ t('usersOverview.search.limitedResults') }}
                </div>
            </template>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.users-header {
    height: 54px;
    margin-top: 20px;
    margin-bottom: 10px;
}

.empty-page-container {
    padding: 4px 12px;

    .empty-page {
        border: solid 1px @memo-grey-light;
        padding: 24px;
    }
}


.content {
    padding-top: 30px;
    padding-bottom: 30px;

    .info-bar {
        padding: 12px;
        background: @memo-yellow;
        margin-top: 24px;
        display: flex;
        justify-content: center;
        width: 100%;
    }
}

.users-title {
    min-height: 22px;
}

.users-options {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 24px;
    z-index: 2;
    flex-wrap: wrap;

    @media (max-width: 1091px) {
        flex-direction: row-reverse;
    }

    .search-section {
        @media (max-width: 1091px) {
            width: 100%;
        }

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
                outline: none;

                @media (max-width: 630px) {
                    width: 100%;
                    flex-grow: 2;
                }

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

    .filter-options {
        display: flex;
        justify-content: flex-end;
        align-items: center;
        flex-wrap: wrap;
    }

    .sort-section,
    .language-section {
        display: flex;
        align-items: center;
        // margin: auto;
        margin-left: auto;
        cursor: pointer;
        margin-top: 8px;

        @media (min-width: 1092px) {
            margin-top: 0px;
        }


        .sort-label,
        .language-label {
            margin: 0 8px;
            cursor: pointer;
        }
    }

    .sort-section {
        margin-left: 8px;
    }
}

.usercard-container {
    display: flex;
    flex-wrap: wrap;
    padding: 0 10px;
}

.orderby-dropdown {
    width: 230px;
}

.language-dropdown {
    width: auto;
}

.v-popper--shown {

    .orderby-select,
    .language-select {

        .chevron {
            transform: rotate(180deg)
        }
    }
}

.v-popper--shown {

    .language-select {
        display: flex;
    }
}

.v-popper--theme-dropdown {
    .v-popper__inner {
        .dropdown-row {

            .select-option {
                min-width: 110px;
            }
        }

    }
}

.orderby-select,
.language-select {
    padding: 6px 12px;
    height: 34px;
    cursor: pointer;
    border: solid 1px @memo-grey-light;
    background: white;
    display: flex;
    justify-content: space-between;
    align-items: center;
    user-select: none;
    width: 100%;

    .select-label {
        display: flex;
        align-items: center;
        flex-wrap: nowrap;
    }

    &:hover {
        color: @memo-blue;
        filter: brightness(0.95)
    }

    &:active {
        filter: brightness(0.85)
    }
}

.language-select {
    width: auto;
}

.language-checkbox {
    cursor: pointer;

    .checkbox-icon {
        margin-right: 8px;
        color: @memo-grey-dark;
    }

    &.active {
        &.checkbox-text {
            font-weight: 500;
        }

        .checkbox-icon {
            color: @memo-blue-link;
        }
    }
}
</style>
