<script lang="ts" setup>
import { PageItem, QuestionItem, UserItem } from './searchHelper'
import { ImageFormat } from '../image/imageFormatEnum'

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
            return t('search.countedQuestions', pageItem.questionCount)
        case 'QuestionItem':
        case 'UserItem':
        default:
            return ''
    }
})

const itemTypeClass = computed(() => {
    return `search-item-${props.item.type}`
})
</script>

<template>
    <div class="searchResultItem" :class="itemTypeClass" v-tooltip="props.item.name">
        <Image :src="props.item.imageUrl" :format="imageFormat" />
        <div class="searchResultLabelContainer">
            <div class="searchResultLabel body-m">{{ props.item.name }}</div>
            <div class="searchResultSubLabel body-s">
                <p>
                    <span v-if="showLanguageTag" v-for="langCode in languageCodes" :key="langCode" class="language-tag" :class="{ 'current-locale': langCode === locale }">{{ langCode }}</span>
                    <span v-if="subLabelText">{{ subLabelText }}</span>
                </p>
                <p v-if="props.item.type !== 'UserItem' && (props.item as PageItem | QuestionItem).creatorName" class="creator-name">
                    {{ t('search.createdBy', { creator: (props.item as PageItem | QuestionItem).creatorName }) }}
                </p>
            </div>
        </div>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.searchResultItem {
    padding: 4px 8px;
    display: flex;
    width: 100%;
    height: 90px;
    transition: .2s ease-in-out;
    cursor: pointer;

    &:hover {
        background: @memo-grey-lighter;
        color: @memo-blue;
    }

    .searchResultLabelContainer {
        width: 100%;
        height: 100%;
        overflow: hidden;
    }

    .searchResultLabel {
        height: 40px;
        line-height: normal;
        color: @memo-blue;
        text-overflow: ellipsis;
        margin: 0;
        overflow: hidden;
        max-width: 408px;
        white-space: normal;
    }

    .searchResultSubLabel {
        display: flex;
        flex-direction: column-reverse;
        color: @memo-grey-dark;
        font-style: italic;
        height: 40px;
        line-height: normal;
        margin-bottom: 0px;
        max-width: 250px;

        p {
            margin-bottom: 0px;
        }

        .creator-name {
            text-overflow: ellipsis;
            overflow: hidden;
            white-space: nowrap;
            margin-bottom: 2px;
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
        max-height: 82px;
        max-width: 82px;
        height: auto;
        margin-right: 10px;
        width: 100%;
        margin-top: 0px !important;
    }
}
</style>
