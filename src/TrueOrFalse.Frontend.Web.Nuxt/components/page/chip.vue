<script lang="ts" setup>
import { PageItem } from '../search/searchHelper'

interface Props {
    page: PageItem
    removableChip?: boolean
    index?: number
    isSpoiler?: boolean
    hideLabel?: boolean
}
const props = defineProps<Props>()
const emit = defineEmits(['removePage'])

const hover = ref(false)
const name = ref('')

const showImage = ref(false)
const showName = ref(true)

// onBeforeMount(() => {
//     showImage.value = !props.page.MiniImageUrl.includes('no-category-picture')
//     name.value = props.page.Name.length > 30 ? props.page.Name.substring(0, 26) + ' ...' : props.page.Name
//     if (props.isSpoiler)
//         showName.value = false
// })

showImage.value = !props.page.miniImageUrl.includes('no-category-picture')
name.value = props.page.name.length > 30 ? props.page.name.substring(0, 26) + ' ...' : props.page.name
if (props.isSpoiler)
    showName.value = false

const { $urlHelper } = useNuxtApp()
</script>

<template>
    <div class="page-chip-component">
        <div class="page-chip-container" @mouseover="hover = true" @mouseleave="hover = false">
            <NuxtLink :to="$urlHelper.getPageUrl(page.name, page.id)" v-if="showName">
                <div class="page-chip" :v-tooltip="page.name" :class="{ 'label-hidden': props.hideLabel }">

                    <img v-if="showImage" :src="page.miniImageUrl" :alt="`image for ${page.name}`" />

                    <div class="page-chip-label" v-if="!props.hideLabel">
                        {{ name }}
                    </div>
                    <font-awesome-icon v-if="page.visibility == 1" icon="fa-solid fa-lock" class="lock" />
                </div>
            </NuxtLink>
            <div class="page-chip spoiler" v-else @click="showName = true">
                <div class="page-chip-label">
                    Spoiler anzeigen
                </div>
            </div>
        </div>
        <div class="page-chip-deleteBtn" v-if="props.removableChip"
            @click="emit('removePage', { index: props.index, pageId: props.page.id })">
            <font-awesome-icon icon="fa-solid fa-xmark" />
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.page-chip-component {
    display: flex;
    align-items: center;
    margin-right: 15px;
    overflow: hidden;

    .page-chip-container {
        padding: 4px 8px 4px 0;
        font-size: 13px;
        max-width: 100%;

        .page-chip {
            max-width: 100%;
            height: 32px;
            display: inline-flex;
            border-radius: 16px;
            background: @memo-grey-lighter;
            padding: 0 12px;
            white-space: nowrap;
            line-height: 32px;
            color: @memo-grey-darker;
            transition: background 0.8s;
            display: flex;
            flex-wrap: nowrap;
            justify-content: center;
            align-items: center;
            cursor: pointer;
            min-width: 32px;

            img {
                margin-left: -8px;
                margin-right: 4px;
                border-radius: 50%;
                height: 26px
            }

            &.label-hidden {
                padding: 0px;

                img {
                    margin-left: 0px;
                    margin-right: 0px;
                }
            }

            &:hover {
                filter: brightness(0.95)
            }

            &:active {
                filter: brightness(0.85)
            }

            .page-chip-label {
                overflow: hidden;
                text-overflow: ellipsis;
            }

            .lock {
                margin-left: 4px;
            }
        }
    }

    .page-chip-container {
        padding: 4px 0;
    }

    .page-chip-deleteBtn {
        display: flex;
        justify-content: center;
        align-items: center;
        transition: all 0.2s ease-in-out;
        width: 16px;
        height: 100%;
        cursor: pointer;
        color: @memo-salmon;
    }

    img {
        display: flex;
        justify-content: center;
        align-items: center;
        border-radius: 12px;
    }
}
</style>