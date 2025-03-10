<script lang="ts" setup>
import { Site } from '~/components/shared/siteEnum'
import { BreadcrumbItem } from '~~/components/header/breadcrumbItems'
import { useLoadingStore } from '~/components/loading/loadingStore'
import { UserResult } from '~~/components/users/userResult'

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
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    },
    default: () => null
})
const { $logger } = useNuxtApp()

const { data: pageData, status } = await useFetch<GetResponse>('/apiVue/Users/Get', {
    query: {
        page: currentPage,
        pageSize: usersPerPageCount,
        searchTerm: searchTerm,
        orderBy: orderBy
    },
    credentials: 'include',
    mode: 'cors',
    onRequest({ options }) {
        if (import.meta.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    },
    onResponseError(context) {
        $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
    },
    immediate: true
})

watch(pageData, (e) => {
    if (e != null) {
        userCount.value = e.totalItems
    }
})

watch(searchTerm, (e) => currentPage.value = 1)

watch(status, (s) => {
    if (s === 'pending')
        loadingStore.startLoading()
    else loadingStore.stopLoading()
})

const emit = defineEmits(['setBreadcrumb', 'setPage'])


onMounted(() => {
    emit('setPage', Site.Default)
    const breadcrumbItem: BreadcrumbItem = {
        name: 'Alle Nutzer',
        url: '/Nutzer'
    }
    emit('setBreadcrumb', [breadcrumbItem])
})
const getSelectedOrderLabel = computed(() => {
    switch (orderBy.value) {
        case SearchUsersOrderBy.Rank:
            return 'Rang'
        case SearchUsersOrderBy.WishCount:
            return 'Wunschwissen'
        default:
            return 'Nicht ausgewählt'
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
            content: 'List of all users'
        },
        {
            property: 'og:title',
            content: 'Users'
        },
        {
            property: 'og:url',
            content: `${config.public.officialBase}/Nutzer`
        },
    ]
}))

const ariaId = useId()

</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="col-xs-12 container">
                <div class="users-header">
                    <h1>Alle Nutzer</h1>
                </div>

                <div class="row content" v-if="pageData">
                    <div class="col-xs-12 col-sm-12">

                        <div class="overline-s no-line" v-if="pageData.totalItems <= 0 && searchTerm.length > 0">
                            Kein Nutzer mit dem Namen "{{ searchTerm }}"
                        </div>
                        <div class="overline-s no-line" v-else-if="pageData.totalItems > 0 && searchTerm.length > 0">
                            Ergebnisse für "{{ searchTerm }}" ({{ pageData.totalItems }})
                        </div>
                        <div class="overline-s no-line" v-else>
                            Alle Nutzer <template v-if="totalUserCount != null"> ({{ totalUserCount }})</template>
                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-12 users-options">
                        <div class="search-section">
                            <div class="search-container">
                                <input type="text" v-model="searchTerm" class="search-input" placeholder="Suche" />
                                <div class="search-icon reset-icon" v-if="searchTerm.length > 0" @click="searchTerm = ''">
                                    <font-awesome-icon icon="fa-solid fa-xmark" />
                                </div>
                                <div class="search-icon" v-else>
                                    <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
                                </div>
                            </div>
                        </div>

                        <div class="sort-section">

                            <font-awesome-icon icon="fa-solid fa-sort" />
                            <div class="sort-label">Sortieren nach: </div>
                            <div class="orderby-dropdown">
                                <VDropdown :aria-id="ariaId" :distance="0">
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
                                                Rang
                                            </div>
                                        </div>
                                        <div class="dropdown-row"
                                            @click="orderBy = SearchUsersOrderBy.WishCount; hide()"
                                            :class="{ 'active': orderBy === SearchUsersOrderBy.WishCount }">
                                            <div class="dropdown-label select-option">
                                                Wunschwissen
                                            </div>
                                        </div>
                                    </template>
                                </VDropdown>
                            </div>
                        </div>
                    </div>

                    <div class="row usercard-container">
                        <TransitionGroup name="usercard">
                            <UsersCard v-for="u in pageData.users" :user="u" :key="u.id" />
                        </TransitionGroup>
                    </div>

                    <div class="col-xs-12 empty-page-container" v-if="pageData.users.length <= 0 && searchTerm.length > 0">
                        <div class="empty-page">
                            Leider gibt es keinen Nutzer mit "{{ searchTerm }}"
                        </div>
                    </div>

                    <div class="col-xs-12" v-if="searchTerm.length === 0">
                        <div class="pagination hidden-xs">
                            <vue-awesome-paginate v-if="currentPage > 0" :total-items="userCount" :items-per-page="20" :max-pages-shown="5" v-model="currentPage" :show-ending-buttons="true" :show-breakpoint-buttons="false">
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
                            <vue-awesome-paginate v-if="currentPage > 0" :total-items="userCount" :items-per-page="20" :max-pages-shown="3" v-model="currentPage" :show-ending-buttons="true" :show-breakpoint-buttons="false">
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
                        Wir zeigen nur die
                        ersten 100, für mehr/andere Ergebnisse verfeinern Sie die Suche
                    </div>
                </div>
            </div>
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

.users-options {
    display: flex;
    justify-content: flex-start;
    align-items: center;
    margin-bottom: 24px;
    z-index: 2;
    flex-wrap: wrap;


    .search-section {
        @media (max-width: 630px) {
            width: 100%;
            margin-bottom: 8px;
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

    .sort-section {
        display: flex;
        align-items: center;
        // margin: auto;
        margin-left: auto;

        .sort-label {
            margin: 0 8px;
        }
    }
}

.usercard-container {
    display: flex;
    flex-wrap: wrap;
    padding: 0 10px;
}

.orderby-dropdown {
    width: 150px;
}


.v-popper--shown {

    .orderby-select {

        .chevron {
            transform: rotate(180deg)
        }
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

.orderby-select {
    padding: 6px 12px;
    height: 34px;
    cursor: pointer;
    border: solid 1px @memo-grey-light;
    background: white;
    display: flex;
    justify-content: space-between;
    align-items: center;
    user-select: none;
    width: 150px;

    &:hover {
        color: @memo-blue;
        filter: brightness(0.95)
    }

    &:active {
        filter: brightness(0.85)
    }
}
</style>