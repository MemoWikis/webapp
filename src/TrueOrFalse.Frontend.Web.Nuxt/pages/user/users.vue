<script lang="ts" setup>
import { BreadcrumbItem } from '~~/components/header/breadcrumbItems'
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { UserResult } from '~~/components/users/userResult'

const spinnerStore = useSpinnerStore()

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

interface Network {
    following: UserResult[]
    followers: UserResult[]
}
interface UsersResult {
    users: UserResult[]
    totalItems: number
}

const headers = useRequestHeaders(['cookie']) as HeadersInit
const config = useRuntimeConfig()

const { data: totalUserCount } = await useLazyFetch<number>('/apiVue/VueUsers/GetTotalUserCount', {
    credentials: 'include',
    mode: 'cors',
    onRequest({ options }) {
        if (process.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    },
    default: () => 0
})

const url = computed(() => {
    return `/apiVue/VueUsers/Get?page=${currentPage.value}&pageSize=${usersPerPageCount.value}&searchTerm=${searchTerm.value}&orderBy=${orderBy.value}`
})
// pageData gets refreshed by executing the request again whenever data changes in the computed url value
// nuxt uses the url in useFetch/useLazyFetch 
const { data: pageData, pending: pageDataPending } = await useFetch<UsersResult>(url, {
    credentials: 'include',
    mode: 'cors',
    onRequest({ options }) {
        if (process.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    }
})

watch(pageData, (e) => {
    if (e != null) {
        userCount.value = e.totalItems
    }
})

watch(searchTerm, (e) => currentPage.value = 1)

watch(pageDataPending, (p) => {
    if (p)
        spinnerStore.showSpinner()
    else spinnerStore.hideSpinner()
})

const emit = defineEmits(['setBreadcrumb'])


onMounted(() => {
    const breadcrumbItem: BreadcrumbItem = {
        name: 'Nutzer',
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
</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="col-xs-12 container">
                <div class="users-header">
                    <h1>Alle Nutzer</h1>
                </div>

                <div class="row">
                    <UsersTabs :all-user-count="totalUserCount!" />
                </div>

                <div class="row content" v-if="pageData">
                    <div class="col-xs-12 col-sm-12 ">

                        <div class="overline-s no-line" v-if="pageData.totalItems <= 0 && searchTerm.length > 0">
                            Kein Nutzer mit dem Namen "{{ searchTerm }}"
                        </div>
                        <div class="overline-s no-line" v-else-if="pageData.totalItems > 0 && searchTerm.length > 0">
                            Ergebnisse für "{{ searchTerm }}" ({{ pageData.totalItems }})
                        </div>
                        <div class="overline-s no-line" v-else>
                            Alle Nutzer ({{ totalUserCount }})
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
                                <V-Dropdown :distance="0">
                                    <div class="orderby-select">
                                        <div>
                                            {{ getSelectedOrderLabel }}
                                        </div>
                                        <font-awesome-icon icon="fa-solid fa-chevron-down" class="chevron" />
                                    </div>

                                    <template #popper="{ hide }">
                                        <div class="dropdown-row select-row"
                                            @click="orderBy = SearchUsersOrderBy.Rank; hide()"
                                            :class="{ 'active': orderBy == SearchUsersOrderBy.Rank }">
                                            <div class="dropdown-label select-option">
                                                Rang
                                            </div>
                                        </div>
                                        <div class="dropdown-row" @click="orderBy = SearchUsersOrderBy.WishCount; hide()"
                                            :class="{ 'active': orderBy == SearchUsersOrderBy.WishCount }">
                                            <div class="dropdown-label select-option">
                                                Wunschwissen
                                            </div>
                                        </div>
                                    </template>
                                </V-Dropdown>
                            </div>
                        </div>
                    </div>

                    <div class="row usercard-container">
                        <TransitionGroup name="usercard">
                            <UsersCard v-for="u in pageData.users" :user="u" />
                        </TransitionGroup>
                    </div>

                    <div class="col-xs-12 empty-page-container" v-if="pageData.users.length <= 0 && searchTerm.length > 0">
                        <div class="empty-page">
                            Leider gibt es keinen Nutzer mit "{{ searchTerm }}"
                        </div>
                    </div>

                    <div class="col-xs-12">
                        <div class="pagination hidden-xs">
                            <vue-awesome-paginate :total-items="userCount" :items-per-page="20" :max-pages-shown="5"
                                v-model="currentPage" :show-ending-buttons="false" :show-breakpoint-buttons="false"
                                prev-button-content="Vorherige" next-button-content="Nächste" first-page-content="Erste"
                                last-page-content="Letzte" />
                        </div>
                        <div class="pagination hidden-sm hidden-md hidden-lg">
                            <vue-awesome-paginate :total-items="userCount" :items-per-page="20" :max-pages-shown="3"
                                v-model="currentPage" :show-ending-buttons="false" :show-breakpoint-buttons="false" />
                        </div>
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