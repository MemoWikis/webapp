<script lang="ts" setup>
import { useSideSheetStore, type SideSheetChildPage } from './sideSheetStore'
import { usePageStore } from '../page/pageStore'

interface Props {
    pageId: number
    depth: number
    section: string
}

const props = defineProps<Props>()

const sideSheetStore = useSideSheetStore()
const pageStore = usePageStore()
const { $urlHelper } = useNuxtApp()

const children = computed(() => {
    return sideSheetStore.childrenMap.get(props.pageId) ?? []
})

const isLoading = ref(false)

const pagePlaceholderUrl = '/Images/Placeholders/placeholder-page-50.png'

const onThumbError = (event: Event) => {
    (event.target as HTMLImageElement).src = pagePlaceholderUrl
}

const loadChildren = async () => {
    if (!sideSheetStore.childrenMap.has(props.pageId)) {
        isLoading.value = true
        const result = await $api<SideSheetChildPage[]>(`/apiVue/SideSheet/GetChildPages/${props.pageId}`)
        sideSheetStore.setChildren(props.pageId, result)
        isLoading.value = false
    }
}

onMounted(() => {
    loadChildren()
})

const toggleChild = async (childId: number) => {
    sideSheetStore.toggleExpanded(props.section, childId)

    if (sideSheetStore.isExpanded(props.section, childId) && !sideSheetStore.childrenMap.has(childId)) {
        const result = await $api<SideSheetChildPage[]>(`/apiVue/SideSheet/GetChildPages/${childId}`)
        sideSheetStore.setChildren(childId, result)
    }
}
</script>

<template>
    <div class="sidesheet-children">
        <div v-for="child in children" :key="child.id">
            <div class="content-item" :style="{ paddingLeft: `${8 + depth * 16}px` }">
                <div v-if="child.childrenCount > 0" class="expand-toggle" @click.stop="toggleChild(child.id)">
                    <font-awesome-icon
                        :icon="sideSheetStore.isExpanded(section, child.id) ? ['fas', 'angle-down'] : ['fas', 'angle-right']" />
                </div>
                <div v-else class="expand-toggle-space" />
                <NuxtLink :to="$urlHelper.getPageUrl(child.name, child.id)"
                    :class="{ 'is-here': child.id === pageStore.id }">
                    <div class="link">
                        <img :src="child.imgUrl || pagePlaceholderUrl" class="sidesheet-thumb" @error="onThumbError" />
                        <span class="link-text">{{ child.name }}</span>
                    </div>
                </NuxtLink>
            </div>
            <SideSheetChildren v-if="sideSheetStore.isExpanded(section, child.id)" :page-id="child.id"
                :depth="depth + 1" :section="section" />
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.sidesheet-children {
    .content-item {
        display: flex;
        flex-wrap: nowrap;
        align-items: center;
        border-radius: 4px;
        padding: 2px 8px 2px 16px;
        background: @memo-grey-lightest;
        color: @memo-grey-dark;
        min-height: 30px;
        user-select: none;

        &:hover {
            filter: brightness(0.95);
        }

        .expand-toggle {
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            width: 16px;
            flex-shrink: 0;
            color: @memo-grey-dark;
            font-size: 12px;
            margin-right: 4px;

            &:hover {
                color: @memo-blue-link;
            }
        }

        .expand-toggle-space {
            width: 16px;
            flex-shrink: 0;
            margin-right: 4px;
        }

        a {
            display: block;
            text-decoration: none;
            color: @memo-grey-dark;
            flex-grow: 2;
            max-width: 100%;
            overflow: hidden;

            .link {
                display: flex;
                align-items: center;
                gap: 8px;
                text-overflow: ellipsis;
                overflow: hidden;
                white-space: nowrap;
                color: @memo-grey-dark;

                .sidesheet-thumb {
                    width: 20px;
                    height: 20px;
                    border-radius: 3px;
                    object-fit: cover;
                    flex-shrink: 0;
                }

                .link-text {
                    text-overflow: ellipsis;
                    overflow: hidden;
                    white-space: nowrap;
                    font-size: 13px;
                }

                &:hover {
                    color: @memo-grey-darker;
                }
            }

            &.is-here {
                color: @memo-blue-link;
                font-weight: 600;

                .link {
                    color: @memo-blue-link;
                }
            }
        }
    }
}

.expand-toggle {
    display: flex;
    align-items: center;
    justify-content: center;
    cursor: pointer;
    width: 16px;
    flex-shrink: 0;
    color: @memo-grey-dark;
    font-size: 11px;

    &:hover {
        color: @memo-blue-link;
    }
}

.expand-toggle-space {
    width: 16px;
    flex-shrink: 0;
}
</style>
