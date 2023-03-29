<script lang="ts" setup>
import { ImageStyle } from '../image/imageStyleEnum'
import { UserResult } from './userResult'

interface Props {
    user: UserResult
}
const props = defineProps<Props>()

</script>

<template>
    <div class="col-md-6 col-sm-12 user-card-container">
        <div class="user-card">
            <Image :url="props.user.imgUrl" :style="ImageStyle.Author" class="user-image" />
            <div class="user-content">
                <div class="user-header">
                    <div class="user-name">
                        <NuxtLink :to="`/Nutzer/${props.user.encodedName}/${props.user.id}`">
                            {{ props.user.name }}
                        </NuxtLink>
                    </div>
                </div>
                <div class="user-body">
                    <div class="">
                        <b>
                            Reputation: {{ props.user.reputationPoints }} (Rang {{ props.user.rank }})
                        </b>
                    </div>

                    <div>
                        Erstellt: {{ props.user.createdQuestionsCount }} Fragen / {{ props.user.createdTopicsCount }} Themen
                    </div>
                    <div v-if="props.user.showWuwi" class="mb-8">
                        Wunschwissen: {{ props.user.wuwiQuestionsCount }} Fragen / {{ props.user.wuwiTopicsCount }} Themen
                    </div>
                    <div v-else class="inactive mb-8"><font-awesome-icon icon="fa-solid fa-lock" />
                        {{ props.user.name }}s Wunschwissen ist privat
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

.mb-8 {
    margin-bottom: 8px;
}

.user-card-container {
    padding: 4px 12px;

    .user-image {
        width: 105px;
        height: 105px;
        min-width: 105px;
        min-height: 105px;
        display: flex;
        justify-content: center;
        align-items: center;

        @media(max-width: @screen-xxs-max) {
            width: 60px;
            height: 60px;
            min-width: 60px;
            min-height: 60px;
        }
    }

    .user-card {
        border: solid 1px @memo-grey-light;
        padding: 12px;
        display: flex;
        justify-content: space-between;
        min-height: 146px;

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
                flex-wrap: wrap;
                align-items: center;

                .user-name {
                    font-size: 18px;
                }

                .inactive {
                    color: @memo-grey-dark;
                }
            }

            .user-body {
                overflow-wrap: break-word;
            }
        }
    }
}
</style>