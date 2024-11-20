<script lang="ts" setup>

import { debounce } from 'underscore'
import { usePageStore } from '../page/pageStore'
import { useSideSheetStore } from './sideSheetStore'

const pageStore = usePageStore()
const sideSheetStore = useSideSheetStore()

const windowWidth = ref(0)
const windowHeight = ref(0)
const resize = () => {
    if (window) {
        windowWidth.value = window.innerWidth
        windowHeight.value = window.innerHeight
    }
}

const handleWidth = (newWidth: number) => {
    if (newWidth < 901) {
        hidden.value = true
        collapsed.value = true
        previouslyCollapsed.value = true
        sideSheetStore.showSideSheet = false
        return
    }

    if (newWidth < 1701 && newWidth > 900) {
        hidden.value = false
        collapsed.value = true
        previouslyCollapsed.value = true
    } else {
        hidden.value = false
        collapsed.value = false
        previouslyCollapsed.value = false
    }
    sideSheetStore.showSideSheet = true
}

onMounted(() => {
    if (window) {
        resize()

        window.addEventListener('resize', debounce(resize, 20))
        handleWidth(windowWidth.value)
    }

})

const collapsed = ref(false)
const hidden = ref(false)

watch(windowWidth, (oldWidth, newWidth) => {

    if (newWidth) {
        handleWidth(newWidth)
    }


}, { immediate: true })

onMounted(() => {
    sideSheetStore.favoriteWikis = [
        {
            name: 'Home',
            id: 3999
        },
        {
            name: 'Globales Wiki',
            id: 1
        },
        {
            name: 'Home',
            id: 3999
        },
        {
            name: 'About',
            id: 1876
        },
        {
            name: 'Dokumentation',
            id: 1864
        },
        {
            name: 'Home',
            id: 3999
        },
        {
            name: 'About',
            id: 1876
        },
        {
            name: 'Dokumentation',
            id: 1864
        },
        {
            name: 'Home',
            id: 3999
        },
        {
            name: 'About',
            id: 1876
        },
        {
            name: 'Dokumentation',
            id: 1864
        },
    ]
})

onMounted(() => {
    sideSheetStore.favoritePages = [
        {
            name: 'Home',
            id: 3999
        },
        {
            name: 'About',
            id: 1876
        },
        {
            name: 'Dokumentation',
            id: 1864
        },
        {
            name: 'Home',
            id: 3999
        },
        {
            name: 'About',
            id: 1876
        },
        {
            name: 'Dokumentation',
            id: 1864
        },
        {
            name: 'Home',
            id: 3999
        },
        {
            name: 'About',
            id: 1876
        },
        {
            name: 'Dokumentation',
            id: 1864
        },
        {
            name: 'Home',
            id: 3999
        },
        {
            name: 'About',
            id: 1876
        },
        {
            name: 'Dokumentation',
            id: 1864
        },
    ]
})

const isFavourite = computed(() => {
    return sideSheetStore.favoritePages.some((page) => page.id === pageStore.id)
})

const previouslyCollapsed = ref(false)

watch(() => pageStore.id, (id) => {
    if (id)
        sideSheetStore.handleRecentPage(pageStore.name, pageStore.id)
})

onMounted(() => {
    if (pageStore.id)
        sideSheetStore.handleRecentPage(pageStore.name, pageStore.id)
})

const showWikis = ref(true)
const showFavorites = ref(true)
const showRecents = ref(true)
const { $urlHelper } = useNuxtApp()

const animate = ref(false)

watch(collapsed, () => {
    animate.value = true
    setTimeout(() => {
        animate.value = false
    }, 300)
})

watch(() => sideSheetStore.showSideSheet, (show) => {
    if (show) {
        hidden.value = false
        collapsed.value = false
    } else {
        handleWidth(windowWidth.value)
    }
}, {
    immediate: true
})

const handleMouseOver = () => {
    collapsed.value = false
}
const handleMouseLeave = () => {
    if (sideSheetStore.showSideSheet && windowWidth.value > 900)
        collapsed.value = previouslyCollapsed.value
}

const showWikisChevron = ref(false)

</script>
<template>
    <div v-if="windowWidth > 0" id="SideSheet" :class="{ 'collapsed': collapsed, 'hide': hidden, 'animate-header': animate }" @mouseover="handleMouseOver" @mouseleave="handleMouseLeave" :style="`height: ${windowHeight}px`">
        <perfect-scrollbar :suppress-scroll-x="true" @ps-scroll-y.stop>

            <div id="SideSheetContainer" :style="`max-height: calc(${windowHeight}px - 156px)`">
                <SideSheetSection :collapsed="collapsed" :class="{ 'no-b-padding': !showWikis }">
                    <template #header>
                        <h4 @click="showWikis = !showWikis" @mouseover="showWikisChevron = true" @mouseleave="showWikisChevron = false">
                            <template v-if="!collapsed && showWikisChevron">
                                <font-awesome-icon v-if="showWikis" :icon="['fas', 'chevron-down']" class="chevron" />
                                <font-awesome-icon v-else :icon="['fas', 'chevron-right']" class="chevron" />
                            </template>
                            <font-awesome-icon :icon="['far', 'folder-open']" v-if="!showWikisChevron && !collapsed" />
                            <div v-show="!hidden" class="header-title">
                                Meine Wikis
                            </div>
                        </h4>
                    </template>

                    <template #content v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showWikis">
                                <div v-for="wiki in sideSheetStore.favoriteWikis" class="content-item">
                                    <NuxtLink :to="$urlHelper.getPageUrl(wiki.name, wiki.id)">
                                        <div class="link">
                                            {{ wiki.name }}
                                        </div>
                                    </NuxtLink>
                                    <div class="content-item-options">
                                        <font-awesome-icon :icon="['fas', 'ellipsis']" />
                                    </div>
                                </div>
                            </div>
                        </Transition>
                    </template>

                    <template #footer v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showWikis">
                                <div class="sidesheet-button" @click="sideSheetStore.addToFavoriteWikis(pageStore.name, pageStore.id)">
                                    <font-awesome-icon :icon="['far', 'square-plus']" />
                                    {{ collapsed ? '' : 'Wiki erstellen' }} <!-- Modal öffnen -->
                                </div>
                            </div>
                        </Transition>
                    </template>

                </SideSheetSection>

                <SideSheetSection :class="{ 'no-b-padding': !showFavorites }">
                    <template #header>
                        <h4 @click="showFavorites = !showFavorites">
                            <template v-if="!collapsed">
                                <font-awesome-icon v-if="showFavorites" :icon="['fas', 'chevron-down']" />
                                <font-awesome-icon v-else :icon="['fas', 'chevron-right']" />
                            </template>
                            <font-awesome-icon :icon="['fas', 'star']" />
                            <div v-show="!hidden" class="header-title">
                                Favoriten
                            </div>
                        </h4>
                    </template>

                    <template #content v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showFavorites">
                                <div v-for="page in sideSheetStore.favoritePages" class="content-item">
                                    <NuxtLink :to="$urlHelper.getPageUrl(page.name, page.id)">
                                        <div class="link">
                                            {{ page.name }}
                                        </div>
                                    </NuxtLink>
                                    <div class="content-item-options">
                                        <font-awesome-icon :icon="['fas', 'ellipsis']" />
                                    </div>
                                </div>

                            </div>
                        </Transition>
                    </template>

                    <template #footer v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showFavorites && !isFavourite">
                                <div class="sidesheet-button" @click="sideSheetStore.addToFavoritePages(pageStore.name, pageStore.id)">
                                    <font-awesome-icon :icon="['fas', 'plus']" />
                                    {{ collapsed ? '' : 'Zu Favoriten hinzufügen' }}
                                </div>
                            </div>
                        </Transition>
                    </template>

                </SideSheetSection>

                <SideSheetSection :class="{ 'no-b-padding': !showRecents }">
                    <template #header>
                        <h4 @click="showRecents = !showRecents">
                            <template v-if="!collapsed">
                                <font-awesome-icon v-if="showRecents" :icon="['fas', 'chevron-down']" />
                                <font-awesome-icon v-else :icon="['fas', 'chevron-right']" />
                            </template>
                            <font-awesome-icon :icon="['fas', 'clock-rotate-left']" />
                            <div v-show="!hidden" class="header-title">
                                Zuletzt besucht
                            </div>
                        </h4>
                    </template>

                    <template #content v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showRecents">
                                <div v-for="recent in sideSheetStore.recentPages" class="content-item">
                                    <NuxtLink :to="$urlHelper.getPageUrl(recent.name, recent.id)">
                                        <div class="link">
                                            {{ recent.name }}
                                        </div>
                                    </NuxtLink>
                                    <div class="content-item-options">
                                        <font-awesome-icon :icon="['fas', 'ellipsis']" />
                                    </div>
                                </div>
                            </div>
                        </Transition>
                    </template>
                </SideSheetSection>
            </div>

        </perfect-scrollbar>

        <div id="SideSheetFooter">
            <div class="bg-fade"></div>
            <div class="sidesheet-content">

            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';
@memo-grey-lightest: #f9f9f9;

#SideSheet {
    background: @memo-grey-lightest;
    width: 400px;
    position: fixed;
    z-index: 51;
    transition: all 0.3s ease-in-out;
    padding-top: 30px;

    #SideSheetContainer {
        height: 100%;

        h4 {
            display: flex;
            flex-wrap: nowrap;
            align-items: center;
            flex-direction: row;
            cursor: pointer;
            user-select: none;

            .chevron {
                color: @memo-grey-dark;
            }
        }
    }

    #SideSheetFooter {
        height: 100px;
        position: fixed;
        width: 400px;
        bottom: 0px;
        transition: all 0.3s ease-in-out;

        .bg-fade {
            height: 40px;
            width: 100%;
            background: linear-gradient(180deg, rgba(249, 249, 249, 0) 0%, rgba(249, 249, 249, 1) 100%);
        }

        .sidesheet-content {
            border-top: 1px solid @memo-grey-light;
            background: @memo-grey-lightest;
        }
    }

    &.collapsed {
        width: 100px;

        #SideSheetContainer {
            h4 {
                flex-direction: column;
                text-align: center;
                padding: 4px 0;
                font-size: 14px;

                .svg-inline--fa {
                    font-size: 14px;
                    margin-right: 0px;
                }

                .header-title {
                    margin-top: 4px;
                    font-size: 12px;
                }
            }
        }

        #SideSheetFooter {
            width: 100px;
        }
    }

    &.hide {
        width: 0px;
    }

    &:hover {
        box-shadow: 0px 0px 10px 0px rgba(0, 0, 0, 0.1);
    }

    @media (max-width: 900px) {
        box-shadow: 0px 0px 10px 0px rgba(0, 0, 0, 0.1);
    }

    &.animate-header {
        h4 {
            transform-origin: left;
            animation: grow 0.3s ease-in-out;
        }
    }

    .no-b-padding {
        padding-bottom: 0px;


        .footer {
            padding-top: 0px;
        }
    }
}

@keyframes grow {
    0% {
        transform: scale(0);
        opacity: 0;
    }

    100% {
        transform: scale(1);
        opacity: 1;
    }
}
</style>