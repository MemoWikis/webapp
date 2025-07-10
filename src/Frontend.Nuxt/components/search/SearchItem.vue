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
    if (props.item.type === 'UserItem') return false
    return (props.item as PageItem | QuestionItem).languageCode !== locale.value
})

const languageCode = computed(() => {
    if (props.item.type === 'UserItem') return ''
    return (props.item as PageItem | QuestionItem).languageCode
})

const subLabelText = computed(() => {
    switch (props.item.type) {
        case 'PageItem':
            return t('search.countedQuestions', (props.item as PageItem).questionCount)
        case 'QuestionItem':
            return ''
        case 'UserItem':
            return ''
        default:
            return ''
    }
})

const itemTypeClass = computed(() => {
    return `search-item-${props.item.type}`
})
</script>

<template>
    <div class="searchResultItem" :class="itemTypeClass">
        <Image :src="props.item.imageUrl" :format="imageFormat" />
        <div class="searchResultLabelContainer">
            <div class="searchResultLabel body-m">{{ props.item.name }}</div>
            <div class="searchResultSubLabel body-s">
                <span v-if="showLanguageTag" class="language-tag">{{ languageCode }}</span>
                {{ subLabelText }}
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
    height: 70px;
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
        color: @memo-grey-darker;
        text-overflow: ellipsis;
        margin: 0;
        overflow: hidden;
        max-width: 408px;
        white-space: normal;
    }

    .searchResultSubLabel {
        color: @memo-grey-light;
        font-style: italic;
        height: 20px;
        line-height: normal;
    }

    .language-tag {
        background-color: @memo-blue-link;
        color: white;
        font-size: 0.8em;
        padding: 2px 6px;
        border-radius: 4px;
        margin-right: 6px;
        text-transform: uppercase;
        font-style: normal;
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
