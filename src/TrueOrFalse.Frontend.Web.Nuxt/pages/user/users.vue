<script lang="ts" setup>

const userCount = ref(200)
const currentPage = ref(1)
const usersPerPageCount = ref(20)

enum SearchUsersOrderBy {
        None = -1,
        Rank = 0,
        WishCount = 1
}

interface User {
        name: string
        id: number
        encodedName: string
        reputationPoints: number
        rank: number
        createdQuestionsCount: number
        createdTopicsCount: number
        showWuwi: boolean
        wuwiQuestionsCount: number
        wuwiTopicsCount: number
}
interface UsersResult {
        users: User[]
        currentPage: number
        totalPages: number
        totalItems: number
}
const headers = useRequestHeaders(['cookie']) as HeadersInit
const config = useRuntimeConfig()
const { data: users } = await useFetch<UsersResult>('/apiVue/VueUsers/Get/', {
        method: 'POST',
        credentials: 'include',
        mode: 'cors',
        body: {
                page: currentPage.value,
                usersPerPageCount: usersPerPageCount.value
        },
        onRequest({ options }) {
                if (process.server) {
                        options.headers = headers
                        options.baseURL = config.public.serverBase
                }
        }
})
watch(currentPage, () => {
        refreshNuxtData()
})
</script>

<template>
        <div class="container" v-if="users">
                <div class="row main-page">
                        <div class="col-xs-12">
                                <h1>Alle Nutzer</h1>
                        </div>
                        <div class="col-xs-12">
                                <div class="pagination">
                                        <vue-awesome-paginate :total-items="userCount" :items-per-page="20" :max-pages-shown="5"
                                                v-model="currentPage" :show-ending-buttons="false"
                                                :show-breakpoint-buttons="false" prev-button-content="Vorherige"
                                                next-button-content="Nächste" first-page-content="Erste"
                                                last-page-content="Letzte" />
                                </div>
                        </div>

                </div>
        </div>
</template>