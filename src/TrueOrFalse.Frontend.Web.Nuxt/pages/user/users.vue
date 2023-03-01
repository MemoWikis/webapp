<script lang="ts" setup>
import { BreadcrumbItem } from '~~/components/header/breadcrumbItems';
import { useSpinnerStore } from '~~/components/spinner/spinnerStore'
import { Tab } from '~~/components/users/tabsEnum'
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
const orderBy = ref(SearchUsersOrderBy.None)

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

watch(pageDataPending, (p) => {
    if (p)
        spinnerStore.showSpinner()
    else spinnerStore.hideSpinner()
})

const { data: network, refresh: refreshNetwork } = await useLazyFetch<Network>('/apiVue/VueUsers/GetNetwork', {
    credentials: 'include',
    mode: 'cors',
    onRequest({ options }) {
        if (process.server) {
            options.headers = headers
            options.baseURL = config.public.serverBase
        }
    }
})

const tab = ref<Tab>()

interface Props {
    tab?: Tab
}

const props = defineProps<Props>()
const emit = defineEmits(['setBreadcrumb'])
onMounted(() => {
    tab.value = props.tab == Tab.Network ? Tab.Network : Tab.AllUsers
    watch(tab, (t) => {
        if (t == Tab.AllUsers) {
            history.pushState(null, 'Alle Nutzer', `/Nutzer`)
            const breadcrumbItem: BreadcrumbItem = {
                name: 'Alle Nutzer',
                url: '/Nutzer'
            }
            emit('setBreadcrumb', [breadcrumbItem])
        }
        else if (t == Tab.Network) {
            history.pushState(null, 'Mein Netzwerk', `/Netzwerk`)
            const breadcrumbItem: BreadcrumbItem = {
                name: 'Mein Netzwerk',
                url: '/Netzwerk'
            }
            emit('setBreadcrumb', [breadcrumbItem])
        }
    })

})

</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="col-xs-12 container">
                <h1 v-if="tab == Tab.AllUsers">Alle Nutzer</h1>
                <h1 v-else-if="tab == Tab.Network">Mein Netzwerk </h1>

                <div class="row">
                    <UsersTabs :tab="tab" :all-user-count="totalUserCount!" :following-count="network?.following.length"
                        :follower-count="network?.followers.length" @set-tab="tab = $event" />
                </div>

                <div class="row content" v-if="pageData && tab == Tab.AllUsers">
                    <div class="col-xs-12">

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
                    <div class="col-xs-12 search-section">
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
                    <TransitionGroup name="usercard">
                        <UsersCard v-for="u in pageData.users" :user="u" @refresh-network="refreshNetwork" />
                    </TransitionGroup>

                    <div class="col-xs-12 empty-page-container" v-if="pageData.users.length <= 0 && searchTerm.length > 0">
                        <div class="empty-page">
                            Leider gibt es keinen Nutzer mit "{{ searchTerm }}"
                        </div>
                    </div>

                    <div class="col-xs-12">
                        <div class="pagination">
                            <vue-awesome-paginate :total-items="userCount" :items-per-page="20" :max-pages-shown="5"
                                v-model="currentPage" :show-ending-buttons="false" :show-breakpoint-buttons="false"
                                prev-button-content="Vorherige" next-button-content="Nächste" first-page-content="Erste"
                                last-page-content="Letzte" />
                        </div>
                    </div>
                </div>

                <div class="row content" v-else-if="network && tab == Tab.Network">
                    <UserNetwork :following="network.following" :followers="network.followers"
                        @refresh-network="refreshNetwork" @tab-to-all-users="tab = Tab.AllUsers" />
                </div>
            </div>


        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

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
            outline: none;

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
</style>