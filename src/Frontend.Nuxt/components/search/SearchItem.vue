<script lang="ts" setup>
import { PageItem, QuestionItem, UserItem, BreadcrumbItem } from './searchHelper'
import { ImageFormat } from '../image/imageFormatEnum'
import SearchItemBreadcrumb from './SearchItemBreadcrumb.vue'

interface Props {
    item: PageItem | QuestionItem | UserItem
}

const props = defineProps<Props>()
const { t, locale } = useI18n()

const imageFormat = computed(() => {
    return props.item.type === 'UserItem' ? ImageFormat.Author : ImageFormat.Page
})

const showLanguageTag = computed(() => {
    if (props.item.type === 'UserItem') {
        const userItem = props.item as UserItem
        return userItem.languageCodes && userItem.languageCodes.length > 0
    }
    return (props.item as PageItem | QuestionItem).languageCode !== locale.value
})

const languageCodes = computed(() => {
    if (props.item.type === 'UserItem') {
        const userItem = props.item as UserItem
        return userItem.languageCodes
    }
    return [(props.item as PageItem | QuestionItem).languageCode]
})

const subLabelText = computed(() => {
    switch (props.item.type) {
        case 'PageItem':
            const pageItem = props.item as PageItem
            if (pageItem.questionCount > 0) {
                return t('search.countedQuestions', pageItem.questionCount)
            }
        case 'QuestionItem':
        case 'UserItem':
        default:
            return ''
    }
})

const itemTypeClass = computed(() => {
    return `search-item-${props.item.type}`
})

const showCreatorName = computed(() => {
    return props.item.type !== 'UserItem' && (props.item as PageItem | QuestionItem).creatorName
})
</script>

<template>
    <div class="search-result-item" :class="itemTypeClass" v-tooltip="props.item.name">
        <Image :src="props.item.imageUrl" :format="imageFormat" />
        <div class="search-result-label-container">
            <div class="search-result-main-label-container">
                <SearchItemBreadcrumb
                    v-if="props.item.type === 'PageItem'"
                    :breadcrumbPath="(props.item as PageItem).breadcrumbPath || []"
                    :itemName="props.item.name" />
                <div v-if="props.item.type === 'UserItem'" class="search-user-id">{{ t('search.userId', { id: (props.item as UserItem).id }) }}</div>
                <div class="search-result-label body-m">{{ props.item.name }}</div>
            </div>
            <div class="search-result-sub-label body-s">
                <div>
                    <span v-if="showCreatorName" class="creator-name">
                        {{ (props.item as PageItem | QuestionItem).creatorName }}
                    </span>
                    <template v-if="showCreatorName && (showLanguageTag || subLabelText)">
                        •
                    </template>
                    <span v-if="showLanguageTag" v-for="langCode in languageCodes" :key="langCode" class="language-tag" :class="{ 'current-locale': langCode === locale }">{{ langCode }}</span>
                    <template v-if="showLanguageTag && subLabelText">
                        •
                    </template>
                    <span v-if="subLabelText">{{ subLabelText }}</span>

                </div>
            </div>
        </div>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.search-result-item {
    padding: 4px 8px;
    display: flex;
    width: 100%;
    transition: .2s ease-in-out;
    cursor: pointer;
    border-bottom: 1px solid @memo-grey-lighter;

    &:last-child {
        border-bottom: none;
    }

    &:hover {
        background: @memo-grey-lighter;
        color: @memo-blue;
    }

    .search-result-label-container {
        width: 100%;
        height: 100%;
        // overflow: hidden;
        display: flex;
        flex-direction: column;
    }

    .search-result-main-label-container {
        display: flex;
        flex-direction: column;
        overflow: hidden;
        margin-bottom: 4px;

        .search-user-id {
            font-size: 0.85em;
            color: @memo-grey;
        }
    }

    .search-result-label {
        line-height: normal;
        color: @memo-blue;
        text-overflow: ellipsis;
        margin: 0;
        overflow: hidden;
        max-width: 408px;
        white-space: normal;
        font-weight: 600;
    }

    .search-result-sub-label {
        display: flex;
        flex-direction: column-reverse;
        color: @memo-grey-dark;
        // font-style: italic;
        line-height: normal;
        margin-bottom: 0px;
        font-size: 0.85em;

        .creator-name {
            text-overflow: ellipsis;
            overflow: hidden;
            white-space: nowrap;
            max-width: 100px;
        }
    }

    .language-tag {
        background-color: @memo-grey;
        color: white;
        font-size: 0.8em;
        padding: 2px 6px;
        border-radius: 4px;
        margin-right: 6px;
        text-transform: uppercase;
        font-style: normal;

        &.current-locale {
            background-color: @memo-blue-link;
        }
    }

    .img-container {
        max-height: 62px;
        max-width: 62px;
        height: auto;
        margin-right: 10px;
        width: 100%;
        margin-top: 0px !important;
    }
}
</style>
