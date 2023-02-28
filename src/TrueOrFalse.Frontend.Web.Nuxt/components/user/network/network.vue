<script lang="ts" setup>
import { UserResult } from '~~/components/users/userResult'

interface Props {
    following: UserResult[],
    followers: UserResult[]
}
const props = defineProps<Props>()
const searchFollowing = ref('')
const currentFollowingPage = ref(1)

const searchFollowers = ref('')
const currentFollowersPage = ref(1)

const emit = defineEmits(['refreshNetwork'])
</script>

<template>
    <div class="col-xs-12 row">
        <div class="col-xs-12">
            <div class="overline-s no-line">Du folgst {{ props.following.length == 1 ? '1 Nutzer' :
                `${props.following.length} Nutzer` }}</div>
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
        <UsersCard v-for="u in props.following" :user="u" @refresh-network="emit('refreshNetwork')" />
        <div class="col-xs-12">
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
            <div class="overline-s no-line">Dir {{ props.followers.length == 1 ? 'folgt 1' :
                `folgen ${props.followers.length}` }} Nutzer</div>
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

        <UsersCard v-for="u in props.followers" :user="u" />

        <div class="col-xs-12">
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