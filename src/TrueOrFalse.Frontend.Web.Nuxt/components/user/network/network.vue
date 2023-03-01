<script lang="ts" setup>
import { UserResult } from '~~/components/users/userResult'

interface Props {
    following: UserResult[],
    followers: UserResult[]
}
const props = defineProps<Props>()
const searchFollowing = ref('')
const currentFollowingPage = ref(1)
const currentFollowingPageUsers = computed(() => {
    let filteredResults = props.following.filter(u => u.name.replace(/\s/g, '').trim().toLowerCase().includes(searchFollowing.value.replace(/\s/g, '').trim().toLowerCase()))
    return filteredResults.slice(10 * (currentFollowingPage.value - 1), 10 * currentFollowingPage.value - 1)
})

const searchFollowers = ref('')
const currentFollowersPage = ref(1)
const currentFollowersPageUsers = computed(() => {
    let filteredResults = props.followers.filter(u => u.name.replace(/\s/g, '').trim().toLowerCase().includes(searchFollowers.value.replace(/\s/g, '').trim().toLowerCase()))
    return filteredResults.slice(10 * (currentFollowersPage.value - 1), 10 * currentFollowersPage.value - 1)
})

const emit = defineEmits(['refreshNetwork', 'tabToAllUsers'])
</script>

<template>
    <div class="col-xs-12 row">
        <div class="col-xs-12">
            <div class="overline-s no-line">
                Du folgst {{ props.following.length == 1 ? '1 Nutzer' : `${props.following.length} Nutzern` }}
            </div>
        </div>

        <div class="col-xs-12 search-section">
            <div class="search-container" v-if="props.following.length > 0">
                <input type="text" v-model="searchFollowing" class="search-input" placeholder="Suche" />
                <div class="search-icon reset-icon" v-if="searchFollowing.length > 0" @click="searchFollowing = ''">
                    <font-awesome-icon icon="fa-solid fa-xmark" />
                </div>
                <div class="search-icon" v-else>
                    <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
                </div>
            </div>
        </div>

        <TransitionGroup name="usercard">
            <UsersCard v-for="u in currentFollowingPageUsers" :user="u" @refresh-network="emit('refreshNetwork')" />
        </TransitionGroup>
        <div v-if="currentFollowingPageUsers.length <= 0" class="empty-page-container col-xs-12">
            <div v-if="props.following?.length <= 0" class="empty-page">
                <h3>Noch folgst du niemanden</h3>
                Um Nutzern zu folgen, gehe zur <button @click="emit('tabToAllUsers')"> "Alle Nutzer"</button> Seite und
                verwende
                den "Folgen"-Button.
            </div>
            <div v-else-if="currentFollowingPageUsers.length <= 0 && searchFollowing.length > 0" class="empty-page">
                Leider gibt es keinen Nutzer mit "{{ searchFollowing }}"
            </div>

        </div>


        <div class="col-xs-12 pager">
            <div class="pagination">
                <vue-awesome-paginate v-if="props.following.length > 10" :total-items="props.following.length"
                    :items-per-page="10" :max-pages-shown="5" v-model="currentFollowingPage" :show-ending-buttons="false"
                    :show-breakpoint-buttons="false" prev-button-content="Vorherige" next-button-content="Nächste"
                    first-page-content="Erste" last-page-content="Letzte" />
            </div>
        </div>
    </div>

    <div class="col-xs-12">
        <div class="divider"></div>
    </div>

    <div class="col-xs-12 row">
        <div class="col-xs-12">
            <div class="overline-s no-line">
                Dir {{ props.followers.length == 1 ? 'folgt 1' : `folgen ${props.followers.length}` }} Nutzer
            </div>
        </div>

        <div class="col-xs-12 search-section">
            <div class="search-container" v-if="props.followers.length > 0">
                <input type="text" v-model="searchFollowers" class="search-input" placeholder="Suche" />
                <div class="search-icon reset-icon" v-if="searchFollowers.length > 0" @click="searchFollowers = ''">
                    <font-awesome-icon icon="fa-solid fa-xmark" />
                </div>
                <div class="search-icon" v-else>
                    <font-awesome-icon icon="fa-solid fa-magnifying-glass" />
                </div>
            </div>
        </div>

        <TransitionGroup name="usercard">
            <UsersCard v-for="u in currentFollowersPageUsers" :user="u" @refresh-network="emit('refreshNetwork')" />
        </TransitionGroup>
        <div class="col-xs-12 empty-page-container" v-if="currentFollowersPageUsers.length <= 0">
            <div v-if="props.followers.length <= 0" class="empty-page">
                Dir folgt noch kein Nutzer.
            </div>
            <div v-else-if="currentFollowersPageUsers.length <= 0 && searchFollowers.length > 0" class="empty-page">
                Leider gibt es keinen Nutzer mit "{{ searchFollowers }}"
            </div>
        </div>


        <div class="col-xs-12 pager">
            <div class="pagination">
                <vue-awesome-paginate v-if="props.followers.length > 10" :total-items="props.followers.length"
                    :items-per-page="10" :max-pages-shown="5" v-model="currentFollowersPage" :show-ending-buttons="false"
                    :show-breakpoint-buttons="false" prev-button-content="Vorherige" next-button-content="Nächste"
                    first-page-content="Erste" last-page-content="Letzte" />
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.pager {
    margin-bottom: 30px;
}

.empty-page-container {
    padding: 4px 12px;

    .empty-page {
        border: solid 1px @memo-grey-light;
        padding: 24px;
    }
}


.divider {
    height: 1px;
    background: @memo-grey-lighter;
    width: 100%;
    margin-bottom: 10px;
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