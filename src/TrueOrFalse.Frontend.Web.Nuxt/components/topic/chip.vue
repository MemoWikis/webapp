<script lang="ts" setup>
import { TopicItem } from '../search/searchHelper'

interface Props {
    topic: TopicItem
    removableChip?: boolean
    index?: number
    isSpoiler?: boolean
    hideLabel?: boolean
}
const props = defineProps<Props>()
const emit = defineEmits(['removeTopic'])

const hover = ref(false)
const name = ref('')

const showImage = ref(false)
const showName = ref(true)

// onBeforeMount(() => {
//     showImage.value = !props.topic.MiniImageUrl.includes('no-category-picture')
//     name.value = props.topic.Name.length > 30 ? props.topic.Name.substring(0, 26) + ' ...' : props.topic.Name
//     if (props.isSpoiler)
//         showName.value = false
// })

showImage.value = !props.topic.miniImageUrl.includes('no-category-picture')
name.value = props.topic.name.length > 30 ? props.topic.name.substring(0, 26) + ' ...' : props.topic.name
if (props.isSpoiler)
    showName.value = false

const { $urlHelper } = useNuxtApp()
</script>

<template>
    <div class="topic-chip-component">
        <div class="topic-chip-container" @mouseover="hover = true" @mouseleave="hover = false">
            <NuxtLink :to="$urlHelper.getTopicUrl(topic.name, topic.id)" v-if="showName">
                <div class="topic-chip" :v-tooltip="topic.name" :class="{ 'label-hidden': props.hideLabel }">

                    <img v-if="showImage" :src="topic.miniImageUrl" :alt="`image for ${topic.name}`" />

                    <div class="topic-chip-label" v-if="!props.hideLabel">
                        {{ name }}
                    </div>
                    <font-awesome-icon v-if="topic.visibility == 1" icon="fa-solid fa-lock" class="lock" />
                </div>
            </NuxtLink>
            <div class="topic-chip spoiler" v-else @click="showName = true">
                <div class="topic-chip-label">
                    Spoiler anzeigen
                </div>
            </div>
        </div>
        <div class="topic-chip-deleteBtn" v-if="props.removableChip"
            @click="emit('removeTopic', { index: props.index, topicId: props.topic.id })">
            <font-awesome-icon icon="fa-solid fa-xmark" />
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.topic-chip-component {
    display: flex;
    align-items: center;
    margin-right: 15px;
    overflow: hidden;

    .topic-chip-container {
        padding: 4px 8px 4px 0;
        font-size: 13px;
        max-width: 100%;

        .topic-chip {
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

            .topic-chip-label {
                overflow: hidden;
                text-overflow: ellipsis;
            }

            .lock {
                margin-left: 4px;
            }
        }
    }

    .topic-chip-container {
        padding: 4px 0;
    }

    .topic-chip-deleteBtn {
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