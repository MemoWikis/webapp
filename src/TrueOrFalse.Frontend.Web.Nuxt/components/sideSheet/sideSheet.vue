<script lang="ts" setup>

import { debounce } from 'underscore'
import { usePageStore } from '../page/pageStore'
import { useSideSheetStore } from './sideSheetStore'
import { useUserStore } from '../user/userStore'

const pageStore = usePageStore()
const sideSheetStore = useSideSheetStore()
const userStore = useUserStore()

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

interface GetWikisResponse {
    id: number
    name: string
    hasParents: boolean
}

interface GetPageResponse {
    id: number
    name: string
}

const init = async () => {
    sideSheetStore.wikis = await $api<GetWikisResponse[]>('/apiVue/SideSheet/GetWikis')
    sideSheetStore.favorites = await $api<GetPageResponse[]>('/apiVue/SideSheet/GetFavorites')
    sideSheetStore.recentPages = await $api<GetPageResponse[]>('/apiVue/SideSheet/GetRecentPages')
}

onBeforeMount(async () => {
    if (userStore.isLoggedIn) {
        init()
    }
})

const isFavorite = computed(() => {
    return sideSheetStore.favorites.some((page) => page.id === pageStore.id)
})

const previouslyCollapsed = ref(false)

watch(() => pageStore.id, (id) => {
    if (userStore.isLoggedIn) {
        init()
    }
    else if (id)
        sideSheetStore.handleRecentPage(pageStore.name, pageStore.id)
})

onMounted(() => {
    if (userStore.isLoggedIn) {
        init()
    }
    else if (pageStore.id)
        sideSheetStore.handleRecentPage(pageStore.name, pageStore.id)
})

watch(() => userStore.isLoggedIn, (isLoggedIn) => {
    if (isLoggedIn) {
        init()
    }
    else {
        sideSheetStore.wikis = []
        sideSheetStore.favorites = []
        sideSheetStore.recentPages = []

        if (pageStore.id)
            sideSheetStore.handleRecentPage(pageStore.name, pageStore.id)
    }
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
}, { immediate: true })

const handleMouseOver = () => {
    collapsed.value = false
}
const handleMouseLeave = () => {
    if (sideSheetStore.showSideSheet && windowWidth.value > 900)
        collapsed.value = previouslyCollapsed.value
}

const ariaId = useId()

const addToFavorites = async (name: string, id: number) => {

    if (isFavorite.value) {
        return
    }

    interface Result {
        success: boolean,
        messageKey?: string
    }
    const result = await $api<Result>(`/apiVue/SideSheet/AddToFavorites/${id}`, {
        method: 'POST'
    })

    if (result.success) {
        sideSheetStore.addToFavoritePages(name, id)
    } else if (result.messageKey) {
        console.log(result.messageKey)
    }
}
</script>
<template>
    <div v-if="windowWidth > 0" id="SideSheet" :class="{ 'collapsed': collapsed, 'hide': hidden, 'animate-header': animate, 'not-logged-in': !userStore.isLoggedIn }" @mouseover="handleMouseOver" @mouseleave="handleMouseLeave"
        :style="`height: ${windowHeight}px`">
        <perfect-scrollbar :suppress-scroll-x="true" @ps-scroll-y.stop>

            <div id="SideSheetContainer" :style="`max-height: calc(${windowHeight}px - 156px)`">
                <SideSheetSection :class="{ 'no-b-padding': !showWikis }">
                    <template #header>
                        <div class="header-container" @click="showWikis = !showWikis">
                            <template v-if="!collapsed">
                                <font-awesome-icon v-if="showWikis" :icon="['fas', 'angle-down']" class="angle-icon" />
                                <font-awesome-icon v-else :icon="['fas', 'angle-right']" class="angle-icon" />
                            </template>
                            <font-awesome-icon :icon="['far', 'folder-open']" />
                            <div v-show="!hidden" class="header-title">
                                Meine Wikis
                            </div>
                        </div>
                    </template>

                    <template #content v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showWikis">
                                <div v-for="wiki in sideSheetStore.wikis" class="content-item">
                                    <NuxtLink :to="$urlHelper.getPageUrl(wiki.name, wiki.id)" :class="{ 'is-here': wiki.id === pageStore.id }">
                                        <div class="link">
                                            {{ wiki.name }}
                                            <font-awesome-icon :icon="['fas', 'caret-left']" v-if="wiki.id === pageStore.id" v-tooltip="'Du bist hier'" />
                                        </div>
                                    </NuxtLink>

                                    <VDropdown :aria-id="`${ariaId}-w-${wiki.id}`" :distance="0">
                                        <div class="content-item-options">
                                            <font-awesome-icon :icon="['fas', 'ellipsis']" />
                                        </div>
                                        <template #popper="{ hide }">
                                            <p class="breadcrumb-dropdown dropdown-row" @click="hide">
                                                Wiki löschen
                                            </p>
                                            <p class="breadcrumb-dropdown dropdown-row" @click="hide">
                                                In Seite umwandeln
                                            </p>
                                        </template>
                                    </VDropdown>
                                </div>
                            </div>
                        </Transition>
                    </template>

                    <template #footer v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showWikis" class="sidesheet-button" @click="console.log('test')">
                                <font-awesome-icon :icon="['far', 'square-plus']" />
                                {{ collapsed ? '' : 'Wiki erstellen' }} <!-- Modal öffnen -->
                            </div>
                        </Transition>
                    </template>

                </SideSheetSection>

                <SideSheetSection :class="{ 'no-b-padding': !showFavorites }">
                    <template #header>
                        <div class="header-container" @click="showFavorites = !showFavorites">
                            <template v-if="!collapsed">
                                <font-awesome-icon v-if="showFavorites" :icon="['fas', 'angle-down']" class="angle-icon" />
                                <font-awesome-icon v-else :icon="['fas', 'angle-right']" class="angle-icon" />
                            </template>
                            <font-awesome-icon :icon="['fas', 'star']" />
                            <div v-show="!hidden" class="header-title">
                                Favoriten
                            </div>
                        </div>
                    </template>

                    <template #content v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showFavorites">
                                <div v-for="favorite in sideSheetStore.favorites" class="content-item">
                                    <NuxtLink :to="$urlHelper.getPageUrl(favorite.name, favorite.id)" :class="{ 'is-here': favorite.id === pageStore.id }">
                                        <div class="link">
                                            {{ favorite.name }}
                                            <font-awesome-icon :icon="['fas', 'caret-left']" v-if="favorite.id === pageStore.id" v-tooltip="'Du bist hier'" />
                                        </div>
                                    </NuxtLink>
                                    <div class="content-item-options">
                                        <font-awesome-layers>
                                            <font-awesome-icon :icon="['far', 'star']" />
                                            <font-awesome-icon :icon="['fas', 'slash']" transform="rotate-20 flip-v" class="slash-bg" />
                                            <font-awesome-icon :icon="['fas', 'slash']" transform="rotate-20 flip-v" />
                                        </font-awesome-layers>
                                    </div>
                                </div>

                            </div>
                        </Transition>
                    </template>

                    <template #footer v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showFavorites" class="sidesheet-button" @click="addToFavorites(pageStore.name, pageStore.id)" :class="{ 'disabled': isFavorite }">
                                <font-awesome-icon :icon="['fas', 'plus']" />
                                {{ isFavorite ? 'Als Favorit hinzugefügt' : 'Zu Favoriten hinzufügen' }}
                            </div>
                        </Transition>
                    </template>

                </SideSheetSection>

                <SideSheetSection :class="{ 'no-b-padding': !showRecents }">
                    <template #header>
                        <div class="header-container" @click="showRecents = !showRecents">
                            <template v-if="!collapsed">
                                <font-awesome-icon v-if="showRecents" :icon="['fas', 'angle-down']" class="angle-icon" />
                                <font-awesome-icon v-else :icon="['fas', 'angle-right']" class="angle-icon" />
                            </template>
                            <font-awesome-icon :icon="['fas', 'clock-rotate-left']" />
                            <div v-show="!hidden" class="header-title">
                                Zuletzt besucht
                            </div>
                        </div>
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

#SideSheet {
    background: @memo-grey-lightest;
    width: 400px;
    position: fixed;
    z-index: 51;
    transition: all 0.3s ease-in-out;
    padding-top: 71px;
    overscroll-behavior: none;

    &.not-logged-in {
        padding-top: 131px;
    }

    #SideSheetContainer {
        height: 100%;
        overscroll-behavior: none;
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
        width: 80px;

        #SideSheetFooter {
            width: 80px;
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
        .header-container {
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

.slash-bg {
    color: white;
}
</style>