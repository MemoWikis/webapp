<script lang="ts" setup>
import { ImageStyle } from '../image/imageStyleEnum'
import { UserResult } from './userResult'
import { useUserStore } from '../user/userStore'
const userStore = useUserStore()

interface Props {
    user: UserResult
}
const props = defineProps<Props>()
const followed = ref(false)
onBeforeMount(() => {
    followed.value = props.user.followed
})

const emit = defineEmits(['refreshNetwork'])

async function follow() {
    if (await userStore.follow(props.user.id)) {
        followed.value = true
        emit('refreshNetwork')
    }
}

async function unfollow() {
    if (await userStore.unfollow(props.user.id)) {
        followed.value = false
        emit('refreshNetwork')
    }
}
</script>

<template>
    <div class="col-sm-6 col-xs-12 user-card-container">
        <div class="user-card">
            <Image :url="props.user.imgUrl" :style="ImageStyle.Author" class="user-image" />
            <div class="user-content">
                <div class="user-header">
                    <div class="user-name">
                        <NuxtLink :to="`/Nutzer/${props.user.encodedName}/${props.user.id}`">
                            {{ props.user.name }}
                        </NuxtLink>
                    </div>
                    <button class="follow-btn btn btn-link">
                        <div v-if="followed" @click="unfollow()" class="inactive"><font-awesome-icon
                                icon="fa-solid fa-user-minus" /> Entfolgen
                        </div>
                        <div v-else @click="follow()"><font-awesome-icon icon="fa-solid fa-user-plus" />
                            Folgen</div>
                    </button>
                </div>
                <div class="user-body">
                    <div><b> Reputation: {{ props.user.reputationPoints }} (Rang {{ props.user.rank }})</b></div>
                    <div> Erstellt: {{ props.user.createdQuestionsCount }} Fragen / {{ props.user.createdTopicsCount }}
                        Themen</div>

                    <div v-if="props.user.showWuwi"> Wunschwissen: {{ props.user.wuwiQuestionsCount }} Fragen / {{
                        props.user.wuwiTopicsCount }} Themen</div>
                    <div v-else class="inactive"><font-awesome-icon icon="fa-solid fa-lock" /> {{ props.user.name }}s
                        Wunschwissen ist privat
                    </div>
                    <div v-if="props.user.wikiId != -1">
                        <NuxtLink :to="`/${props.user.encodedName}s-Wiki/${props.user.wikiId}`">
                            Zu {{ props.user.name }}s Wiki
                        </NuxtLink>
                    </div>
                </div>


            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';


.inactive {
    color: @memo-grey-dark;
}

.follow-btn {
    outline: none;
    border: none;
    text-decoration: none;
    background: white;
    border-radius: 24px;

    &:hover {
        filter: brightness(0.95)
    }

    &:active {
        outline: none;
        border: none;
        filter: brightness(0.85)
    }
}

.user-card-container {
    padding: 4px 12px;

    .user-image {
        width: 105px;
        height: 105px;
        display: flex;
        justify-content: center;
        align-items: center;
    }

    .user-card {
        border: solid 1px @memo-grey-light;
        padding: 12px;
        display: flex;
        justify-content: space-between;

        .user-content {
            padding-left: 24px;
            flex-grow: 2;
            display: flex;
            flex-direction: column;
            height: 100%;
            justify-content: flex-start;

            .user-header {
                display: flex;
                justify-content: space-between;
                flex-wrap: nowrap;
                align-items: center;

                .user-name {
                    font-size: 18px;
                }
            }
        }
    }
}
</style>