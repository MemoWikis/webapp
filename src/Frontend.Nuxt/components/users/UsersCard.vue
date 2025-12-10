<script lang="ts" setup>
import { ImageFormat } from '../image/imageFormatEnum'
import { UserResult } from './userResult'

interface Props {
    user: UserResult
}
const props = defineProps<Props>()
const { $urlHelper } = useNuxtApp()
const { t } = useI18n()

const getCountryCode = (language: string) => {
    if (language === "en")
        return "gb"
    else return language

}
</script>

<template>
    <div class="col-md-6 col-xs-12 user-card-container">
        <div class="user-card">
            <Image :src="props.user.imgUrl" :format="ImageFormat.Author" class="user-image" />
            <div class="user-content">
                <div class="user-header">
                    <div class="user-name">
                        <NuxtLink v-if="props.user.id > 0" :to="$urlHelper.getUserUrl(props.user.name, props.user.id)">
                            {{ props.user.name }}
                        </NuxtLink>
                    </div>
                </div>
                <div class="user-body">
                    <div class="">
                        <b>
                            {{ t('usersOverview.card.reputation', {
                                points: props.user.reputationPoints,
                                rank: props.user.rank
                            }) }}
                        </b>
                    </div>

                    <div>
                        {{ t('usersOverview.card.created', {
                            questions: props.user.createdQuestionsCount,
                            pages: props.user.createdPagesCount
                        }) }}
                    </div>
                    <div v-if="props.user.showWishKnowledge" class="mb-8">
                        {{ t('usersOverview.card.wishKnowledge', {
                            questions: props.user.wishKnowledgeQuestionsCount,
                            pages: props.user.wishKnowledgePagesCount
                        }) }}
                    </div>
                    <div v-else class="inactive "><font-awesome-icon icon="fa-solid fa-lock" />
                        {{ t('usersOverview.card.privateWishKnowledge', { name: props.user.name }) }}
                    </div>

                    <div class="mb-8">
                        <div class="content-languages-container">
                            <div class="language-label">
                                {{ t('usersOverview.card.languages') }}:
                            </div>
                            <CircleFlags v-for="language in props.user.contentLanguages" :country="getCountryCode(language)" class="country-flag" />
                        </div>
                    </div>

                    <div v-if="props.user.wikiId != -1">
                        <NuxtLink :to="$urlHelper.getPageUrl(`${props.user.name}s-Wiki`, props.user.wikiId)">
                            {{ t('usersOverview.card.toWiki', { name: props.user.name }) }}
                        </NuxtLink>
                    </div>



                </div>

                <!-- <div class="user-body">
                    <div class="user-body-element">
                        <div class="user-body-card">
                            <b>
                                {{ t('usersOverview.card.reputation', {
                                    points: props.user.reputationPoints,
                                    rank: props.user.rank
                                }) }}
                            </b>
                        </div>

                    </div>

                    <div class="user-body-element">
                        <div class="user-body-card">
                            {{ t('usersOverview.card.created', {
                                questions: props.user.createdQuestionsCount,
                                pages: props.user.createdPagesCount
                            }) }}
                        </div>

                    </div>
                    <div v-if="props.user.showWishKnowledge" class="mb-8 user-body-element">
                        <div class="user-body-card">
                            {{ t('usersOverview.card.wishKnowledge', {
                                questions: props.user.wishKnowledgeQuestionsCount,
                                pages: props.user.wishKnowledgePagesCount
                            }) }}
                        </div>

                    </div>
                    <div v-else class="inactive mb-8 user-body-element">
                        <div class="user-body-card">
                            <font-awesome-icon icon="fa-solid fa-lock" />
                            {{ t('usersOverview.card.privateWishKnowledge', { name: props.user.name }) }}
                        </div>

                    </div>

                    <div class="user-body-element">
                        <div class="user-body-card">
                            <div>
                                {{ t('usersOverview.card.languages') }}:
                            </div>

                            <div class="content-languages-container ">
                                <CircleFlags v-for="language in props.user.contentLanguages" :country="getCountryCode(language)" class="country-flag" />

                            </div>

                        </div>
                    </div>

                    <div v-if="props.user.wikiId != -1" class="user-body-element">
                        <div class="user-body-card">
                            <NuxtLink :to="$urlHelper.getPageUrl(`${props.user.name}s-Wiki`, props.user.wikiId)">
                                {{ t('usersOverview.card.toWiki', { name: props.user.name }) }}
                            </NuxtLink>
                        </div>

                    </div>

                </div> -->
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
                // display: flex;

                .inactive {
                    color: @memo-grey-dark;
                }

                .content-languages-container {
                    display: flex;
                    align-items: center;

                    .language-label {
                        margin-right: 8px;
                    }

                    .country-flag {
                        height: 1.5rem;
                        width: 1.5rem;
                        margin-right: 8px;
                    }
                }

                .user-body-card {
                    padding: 1em;
                    height: 100%;
                    margin: 0.5em;
                    border: solid 1px @memo-grey-light;
                    border-radius: 4px;
                    width: 40%;

                    .user-body-card-title {
                        font-size: 14px;
                        color: @memo-grey-dark;
                    }

                    .user-body-card-content {
                        display: flex;
                        justify-content: space-between;
                        align-items: center;
                        font-size: 14px;
                        color: @memo-grey-darker;
                    }
                }
            }
        }
    }
}
</style>

<!-- <style lang="less" scoped>
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
                display: flex;
                flex-wrap: wrap;

                .inactive {
                    color: @memo-grey-dark;
                }



                .content-languages-container {
                    display: flex;
                    align-items: center;

                    .country-flag {
                        height: 2rem;
                        width: 2rem;
                        margin-right: 1rem;
                    }
                }

                .user-body-element {
                    width: 50%;
                    padding: 0.5em;

                    .user-body-card {
                        padding: 0.5m;
                        height: 100%;
                        border: solid 1px @memo-grey-light;
                        border-radius: 4px;

                        .user-body-card-title {
                            font-size: 14px;
                            color: @memo-grey-dark;
                        }

                        .user-body-card-content {
                            display: flex;
                            justify-content: space-between;
                            align-items: center;
                            font-size: 14px;
                            color: @memo-grey-darker;
                        }
                    }
                }


            }
        }
    }
}
</style> -->