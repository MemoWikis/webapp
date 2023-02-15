<script lang="ts" setup>
import { TopicItem } from '../search/searchHelper'


interface Props {
    topic: TopicItem
    removableChip?: boolean
    index?: number
    isSpoiler?: boolean
}
const props = defineProps<Props>()
const emit = defineEmits(['removeTopic'])

const hover = ref(false)
const name = ref('')

const showImage = ref(false)
const showName = ref(true)

onBeforeMount(() => {
    showImage.value = props.topic.MiniImageUrl.includes('no-category-picture')
    name.value = props.topic.Name.length > 30 ? props.topic.Name.substring(0, 26) + ' ...' : props.topic.Name
    if (props.isSpoiler)
        showName.value = false
})

</script>

<template>
    <div class="category-chip-component">
        <div class="category-chip-container" @mouseover="hover = true" @mouseleave="hover = false">
            <NuxtLink :to="topic.Url" v-if="showName">
                <div class="category-chip" :v-tooltip="topic.Name">

                    <img v-if="showImage" :src="topic.MiniImageUrl" />

                    <div class="category-chip-label">
                        <i v-if="topic.IconHtml.length > 0" v-html="topic.IconHtml"></i>{{ name }}
                    </div>
                    <font-awesome-icon v-if="topic.Visibility == 1" icon="fa-solid fa-lock" class="lock" />
                </div>
            </NuxtLink>
            <div class="category-chip spoiler" v-else @click="showName = true">
                <div class="category-chip-label">
                    Spoiler anzeigen
                </div>
            </div>
        </div>
        <div class="category-chip-deleteBtn" v-if="props.removableChip"
            @click="emit('removeTopic', { index: props.index, topicId: props.topic.Id })">
            <font-awesome-icon icon="fa-solid fa-xmark" />
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.category-chip-component {
    display: flex;
    align-items: center;
    margin-right: 15px;
    overflow: hidden;

    .category-chip-container {
        padding: 4px 8px 4px 0;
        font-size: 13px;
        max-width: 100%;

        .category-chip {
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

            img {
                margin-left: -8px;
                margin-right: 4px;
                border-radius: 50%;
                height: 26px
            }

            &:hover {
                filter: brightness(0.95)
            }

            &:active {
                filter: brightness(0.85)
            }

            .category-chip-label {
                overflow: hidden;
                text-overflow: ellipsis;
            }

            .lock {
                margin-left: 4px;
            }
        }
    }

    .category-chip-container {
        padding: 4px 0;
    }

    .category-chip-deleteBtn {
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