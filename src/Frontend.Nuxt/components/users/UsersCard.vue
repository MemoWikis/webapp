<script lang="ts" setup>
import { ImageFormat } from '../image/imageFormatEnum'
import { UserResult } from './userResult'

interface Props {
    user: UserResult
}
const props = defineProps<Props>()
const { $urlHelper } = useNuxtApp()
const { t } = useI18n()
</script>

<template>
    <NuxtLink :to="$urlHelper.getUserUrl(props.user.name, props.user.id)" class="user-card">
        <div class="user-avatar-container">
            <Image :src="props.user.imgUrl" :format="ImageFormat.Author" class="user-avatar" />
        </div>
        <div class="user-info">
            <div class="user-name">{{ props.user.name }}</div>
            <div class="user-stats">
                <span class="stat-badge rank">
                    <font-awesome-icon icon="fa-solid fa-trophy" class="stat-icon" />
                    {{ t('usersOverview.card.rank') }} {{ props.user.rank }}
                </span>
                <span v-if="props.user.createdPagesCount > 0" class="stat-badge">
                    {{ props.user.createdPagesCount }} {{ t('wikisPage.pages') }}
                </span>
                <span v-if="props.user.createdQuestionsCount > 0" class="stat-badge">
                    {{ props.user.createdQuestionsCount }} {{ t('wikisPage.questions') }}
                </span>
            </div>
            <div class="user-footer">
                <div class="content-languages">
                    <CircleFlags v-for="language in props.user.contentLanguages" :key="language"
                        :country="getCountryCode(language)" class="country-flag" />
                </div>
                <div v-if="props.user.wikiId !== -1" class="wiki-link">
                    <font-awesome-icon icon="fa-solid fa-book" class="wiki-icon" />
                    Wiki
                </div>
            </div>
        </div>
    </NuxtLink>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.user-card {
    display: flex;
    align-items: flex-start;
    gap: 12px;
    padding: 16px;
    border: 1px solid @memo-grey-light;
    border-radius: 8px;
    text-decoration: none;
    color: inherit;
    transition: border-color 0.15s, box-shadow 0.15s;

    &:hover {
        border-color: @memo-blue;
        box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
    }

    .user-avatar-container {
        flex-shrink: 0;

        .user-avatar {
            width: 48px;
            height: 48px;
            border-radius: 50%;
            object-fit: cover;
        }
    }

    .user-info {
        min-width: 0;
        flex: 1;

        .user-name {
            font-size: 15px;
            font-weight: 600;
            color: @memo-grey-darkest;
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
        }

        .user-stats {
            display: flex;
            flex-wrap: wrap;
            gap: 6px;
            margin-top: 6px;

            .stat-badge {
                font-size: 12px;
                color: @memo-grey-dark;
                background: @memo-grey-lightest;
                padding: 2px 8px;
                border-radius: 4px;

                .stat-icon {
                    font-size: 10px;
                    margin-right: 2px;
                    color: @memo-yellow;
                }

                &.rank {
                    color: @memo-grey-darker;
                    font-weight: 500;
                }
            }
        }

        .user-footer {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-top: 8px;

            .content-languages {
                display: flex;
                gap: 4px;

                .country-flag {
                    height: 18px;
                    width: 18px;
                }
            }

            .wiki-link {
                font-size: 12px;
                color: @memo-blue-link;

                .wiki-icon {
                    font-size: 11px;
                    margin-right: 2px;
                }
            }
        }
    }
}
</style>