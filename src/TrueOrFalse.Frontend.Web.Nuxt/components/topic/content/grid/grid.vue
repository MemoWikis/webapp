<script lang="ts" setup>
import { useTopicStore } from '~/components/topic/topicStore'
import { ToggleState } from './toggleStateEnum'
import { GridTopicItem } from './item/gridTopicItem'
import { useRootTopicChipStore } from '~/components/header/rootTopicChipStore'
import { TopicItem } from '~/components/search/searchHelper'
import { EditRelationData, EditTopicRelationType, useEditTopicRelationStore } from '~/components/topic/relation/editTopicRelationStore'
import { useUserStore } from '~/components/user/userStore'

const topicStore = useTopicStore()
const rootTopicChipStore = useRootTopicChipStore()
const editTopicRelationStore = useEditTopicRelationStore()
const userStore = useUserStore()

interface Props {
    children: GridTopicItem[]
}
const props = defineProps<Props>()

const toggleState = ref(ToggleState.Collapsed)
const { $urlHelper } = useNuxtApp()

const rootTopicItem = ref<TopicItem>()

onMounted(() => {
    rootTopicItem.value = {
        Type: 'TopicItem',
        Id: rootTopicChipStore.id,
        Name: rootTopicChipStore.name,
        Url: $urlHelper.getTopicUrl(rootTopicChipStore.name, rootTopicChipStore.id),
        QuestionCount: 0,
        ImageUrl: rootTopicChipStore.imgUrl,
        MiniImageUrl: rootTopicChipStore.imgUrl,
        Visibility: 0,
    }
})

const topicsToFilter = computed<number[]>(() => {

    let topicsToFilter = process.server ? props.children.map(c => c.id) : topicStore.gridItems.map(c => c.id)
    topicsToFilter.push(topicStore.id)

    return topicsToFilter
})

function addTopic(newTopic: boolean) {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    const parent: EditRelationData = {
        parentId: topicStore.id,
        editCategoryRelation: newTopic
            ? EditTopicRelationType.Create
            : EditTopicRelationType.AddChild,
        categoriesToFilter: topicsToFilter.value,
    }
    editTopicRelationStore.openModal(parent)
}

</script>

<template>
    <div class="row">
        <div class="col-xs-12">
            <div class="grid-container">
                <div class="grid-header ">
                    <div class="grid-title overline-m no-line">
                        Untergeordnete Themen ({{ topicStore.directChildTopicCount }})
                    </div>

                    <div class="grid-options">
                        <div class="grid-divider"></div>
                        <div class="grid-option">
                            <button @click="addTopic(true)">
                                <font-awesome-icon :icon="['fas', 'plus']" />
                            </button>
                        </div>
                        <div class="grid-divider"></div>
                        <div class="grid-option">
                            <button @click="addTopic(false)">
                                <font-awesome-icon :icon="['fas', 'link']" />
                            </button>
                        </div>

                        <template v-if="rootTopicChipStore.showRootTopicChip && rootTopicItem">
                            <div class="grid-divider"></div>
                            <div class="root-chip grid-option">
                                <TopicChip :topic="rootTopicItem" class="no-margin" />
                            </div>
                        </template>

                    </div>
                </div>

                <TopicContentGridItem v-for="c in props.children" :topic="c" :toggle-state="toggleState"
                    :parent-id="topicStore.id" />

                <div class="grid-footer">
                    <div class="grid-option overline-m no-line no-margin">
                        <button @click="addTopic(true)">
                            <font-awesome-icon :icon="['fas', 'plus']" />
                            <span class="button-label">
                                Neues Thema erstellen
                            </span>
                        </button>
                    </div>
                    <div class="grid-divider"></div>
                    <div class="grid-option overline-m no-line no-margin">
                        <button @click="addTopic(false)">
                            <font-awesome-icon :icon="['fas', 'link']" />
                            <span class="button-label">
                                Bestehendes Thema verknüpfen
                            </span>
                        </button>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.grid-container {
    margin-bottom: 45px;
}

.no-margin {
    margin-right: 0px;
    margin-left: 0px;
    margin-top: 0px;
    margin-bottom: 0px;
}

.grid-header {
    justify-content: space-between;

    .grid-options {
        display: flex;
        justify-content: flex-end;
        align-items: center;

        .root-chip {
            align-items: center;
            color: @memo-grey-darker;

        }
    }
}

.grid-header,
.grid-footer {
    display: flex;
    flex-wrap: nowrap;
    align-items: center;

    button {
        color: @memo-grey-darker;
        background: white;
        height: 32px;
        border-radius: 32px;
        min-width: 32px;

        .button-label {
            color: @memo-grey-dark;
            padding-right: 4px;
        }

        &:hover {
            filter: brightness(0.95)
        }

        &:active {
            filter: brightness(0.85)
        }
    }

    .grid-divider {
        margin: 0 8px;
        height: 22px;
        border-left: solid 1px @memo-grey-light;
    }
}

.grid-footer {
    border-top: solid 1px @memo-grey-light;
    padding-top: 4px;
}
</style>

<style lang="less">
.open-modal {

    .grid-container,
    .grid-header {
        button {
            background: none;
        }
    }
}
</style>