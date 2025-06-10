<script lang="ts" setup>

import { debounce } from 'underscore'
import { FooterPages, usePageStore } from '../page/pageStore'
import { useSideSheetStore } from './sideSheetStore'
import { useUserStore } from '../user/userStore'
import { useDeletePageStore } from '../page/delete/deletePageStore'
import { useConvertStore } from '../page/convert/convertStore'
import { useSnackbar } from 'vue3-snackbar'

interface Props {
    footerPages?: FooterPages | null
}
const props = defineProps<Props>()

const pageStore = usePageStore()
const sideSheetStore = useSideSheetStore()
const userStore = useUserStore()
const deletePageStore = useDeletePageStore()
const convertStore = useConvertStore()
const { t } = useI18n()

const snackbar = useSnackbar()

// Initialize side sheet state with cookie persistence
useSideSheetState()

const windowWidth = ref(0)
const windowHeight = ref(0)
const resize = () => {
    if (window) {
        windowWidth.value = window.innerWidth
        windowHeight.value = window.innerHeight
        handleWidth(window.innerWidth)
    }
}

const handleWidth = (newWidth: number) => {
    if (newWidth < 901) {
        if (!sideSheetStore.showSideSheet) {
            hidden.value = true
        }
        previouslyCollapsed.value = true
        return
    }

    hidden.value = false
    previouslyCollapsed.value = true
}

onMounted(() => {
    if (window) {
        resize()

        window.addEventListener('resize', debounce(resize, 20))
        handleWidth(windowWidth.value)
    }
})

const collapsed = computed(() => {
    return !hover.value && !sideSheetStore.showSideSheet && windowWidth.value > 900
})

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
    sideSheetStore.sharedPages = await $api<GetPageResponse[]>('/apiVue/SideSheet/GetSharedPages')
}

onBeforeMount(async () => {
    if (userStore.isLoggedIn) {
        await init()
    }
})

const isFavorite = computed(() => {
    return sideSheetStore.favorites.some((page) => page.id === pageStore.id)
})

const previouslyCollapsed = ref(false)

watch(() => pageStore.id, async (id) => {
    if (userStore.isLoggedIn) {
        await init()
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
const showShared = ref(true)
const { $urlHelper } = useNuxtApp()

watch(() => sideSheetStore.showSideSheet, (show) => {
    if (show) {
        hidden.value = false
    } else {
        if (windowWidth.value < 901 || isMobile) {
            hidden.value = true
        } else {
            hidden.value = false
        }
    }
}, { immediate: true })

const hover = ref(false)
const mouseOverTimer = ref()
const delayedMouseLeaveTimeOut = ref()
const lastMousePosition = ref({ x: 0, y: 0 })
const mouseMoveTimer = ref()

const handleMouseOver = (event: MouseEvent) => {
    if (windowWidth.value > 900) {
        clearTimeout(delayedMouseLeaveTimeOut.value)
        clearTimeout(mouseOverTimer.value)
        clearTimeout(mouseMoveTimer.value)

        const currentX = event.clientX
        const currentY = event.clientY

        clearTimeout(mouseOverTimer.value)

        mouseMoveTimer.value = setTimeout(() => {
            const deltaX = Math.abs(currentX - lastMousePosition.value.x)
            const deltaY = Math.abs(currentY - lastMousePosition.value.y)

            if (deltaX <= 10 && deltaY <= 10) {
                mouseOverTimer.value = setTimeout(() => {
                    hover.value = true
                }, 200)
            }
        }, 100)

        lastMousePosition.value = { x: currentX, y: currentY }
    }
}

const handleMouseLeave = () => {
    if (windowWidth.value > 900) {
        clearTimeout(delayedMouseLeaveTimeOut.value)
        clearTimeout(mouseOverTimer.value)
        clearTimeout(mouseMoveTimer.value)
        delayedMouseLeaveTimeOut.value = setTimeout(() => {
            hover.value = false
        }, 500)
    }
}

const ariaId = useId()

const addToFavorites = async (name: string, id: number) => {

    if (isFavorite.value) {
        return
    }

    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    interface Result {
        success: boolean,
        messageKey?: string
    }
    const result = await $api<Result>(`/apiVue/SideSheet/AddToFavorites/${id}`, {
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
    })

    if (result.success) {
        sideSheetStore.addToFavoritePages(name, id)
    } else if (result.messageKey) {
        snackbar.add({
            text: t(result.messageKey),
            type: 'error'
        })
    }
}

const removeFromFavorites = async (id: number) => {
    interface Result {
        success: boolean,
        messageKey?: string
    }
    const result = await $api<Result>(`/apiVue/SideSheet/RemoveFromFavorites/${id}`, {
        method: 'POST',
        mode: 'cors',
        credentials: 'include',
    })

    if (result.success) {
        const name = sideSheetStore.favorites.find(f => f.id === id)?.name
        sideSheetStore.removeFromFavoritePages(id)
        snackbar.add({
            text: t('sideSheet.removedFromFavorites', { name: name }),
            type: 'success'
        })
    } else if (result.messageKey) {
        snackbar.add({
            text: t(result.messageKey),
            type: 'error'
        })
    }
}

const showCreateWikiModal = ref(false)

const openCreateWikiModal = () => {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    showCreateWikiModal.value = true
}
const config = useRuntimeConfig()

const discordBounce = ref(false)

const handleWikiCreated = async () => {
    showCreateWikiModal.value = false
    sideSheetStore.wikis = await $api<GetWikisResponse[]>('/apiVue/SideSheet/GetWikis')
}

deletePageStore.$onAction(({ after, name }) => {
    if (name === 'deletePage') {
        after((result) => {
            if (result && result.id && (sideSheetStore.wikis.some(w => w.id === result.id) || sideSheetStore.favorites.some(f => f.id === result.id))) {
                init()
            }
        })
    }
})

convertStore.$onAction(({ after, name }) => {
    if (name === 'confirmConversion') {
        after(() => {
            init()
        })
    }
})

const cancelMouseLeave = () => {
    clearTimeout(delayedMouseLeaveTimeOut.value)
}

pageStore.$onAction(({ after, name }) => {
    if (name === 'saveName') {
        after(() => {
            init()
        })
    }
})

const route = useRoute()
const { isMobile } = useDevice()
onMounted(() => {
    if (isMobile) {
        watch(() => route.path, () => {
            sideSheetStore.showSideSheet = false
            hidden.value = true
        })

        if (!sideSheetStore.showSideSheet) {
            hidden.value = true
        }
    }
})

const hoverWikiButton = ref(false)
const hoverFavoritesButton = ref(false)

const handleClick = (key?: string) => {
    if (collapsed.value)
        sideSheetStore.showSideSheet = !sideSheetStore.showSideSheet
    else if (key) {
        switch (key) {
            case 'showWikis':
                showWikis.value = !showWikis.value
                break
            case 'showFavorites':
                showFavorites.value = !showFavorites.value
                break
            case 'showRecents':
                showRecents.value = !showRecents.value
                break
            case 'showShared':
                showShared.value = !showShared.value
                break
        }
    }
}

</script>
<template>
    <div id="SideSheet"
        :class="{ 'collapsed': collapsed, 'hide': hidden, 'not-logged-in': !userStore.isLoggedIn }" @mouseleave="handleMouseLeave">
        <perfect-scrollbar :suppress-scroll-x="true" @ps-scroll-y.stop>

            <div id="SideSheetContainer" :style="`max-height: calc(${windowHeight}px - 156px)`">
                <SideSheetSection class="no-b-padding" @mouseover="handleMouseOver">
                    <template #header>
                        <NuxtLink :to="`/${t('url.missionControl')}`" class="mission-control-link">
                            <div class="header-container no-hover">
                                <template v-if="!collapsed">
                                    <div class="angle-icon-space"></div>
                                </template>
                                <font-awesome-icon :icon="['fas', 'rocket']" />
                                <div v-show="!hidden" class="header-title">
                                    {{ t('sideSheet.missionControl') }}
                                </div>
                            </div>
                        </NuxtLink>
                    </template>
                </SideSheetSection>

                <SideSheetSection :class="{ 'no-b-padding': !showWikis }" @mouseover="handleMouseOver">
                    <template #header>
                        <div class="header-container" @click="handleClick('showWikis')" :class="{ 'no-hover': hoverWikiButton }">
                            <template v-if="!collapsed">
                                <font-awesome-icon v-if="showWikis" :icon="['fas', 'angle-down']" class="angle-icon" />
                                <font-awesome-icon v-else :icon="['fas', 'angle-right']" class="angle-icon" />
                            </template>
                            <font-awesome-icon :icon="['fas', 'folder']" />
                            <div v-show="!hidden" class="header-title">
                                {{ t('sideSheet.myWikis') }}
                            </div>
                            <div class="header-btn" v-show="!hidden && !collapsed" @click.stop="openCreateWikiModal" v-tooltip="t('label.createWiki')" @mouseenter="hoverWikiButton = true" @mouseleave="hoverWikiButton = false">
                                <font-awesome-icon :icon="['fas', 'plus']" />
                            </div>
                        </div>
                    </template>

                    <template #content v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showWikis">
                                <div v-for="wiki in sideSheetStore.wikis" class="content-item">
                                    <NuxtLink :to="$urlHelper.getPageUrl(wiki.name, wiki.id)"
                                        :class="{ 'is-here': wiki.id === pageStore.id }">
                                        <div class="link">
                                            {{ wiki.name }}
                                        </div>
                                    </NuxtLink>

                                    <VDropdown :aria-id="`${ariaId}-w-${wiki.id}`" :distance="0">
                                        <div class="content-item-options">
                                            <font-awesome-icon :icon="['fas', 'ellipsis']" />
                                        </div>
                                        <template #popper="{ hide }">
                                            <div class="sidesheet-wikioptions" @mouseenter="cancelMouseLeave">
                                                <p class="breadcrumb-dropdown dropdown-row"
                                                    @click="deletePageStore.openModal(wiki.id, false); hide()">
                                                    {{ t('sideSheet.deleteWiki') }}
                                                </p>
                                                <p v-if="wiki.hasParents" class="breadcrumb-dropdown dropdown-row"
                                                    @click="convertStore.openModal(wiki.id)">
                                                    {{ t('sideSheet.convertToPage') }}
                                                </p>
                                            </div>
                                        </template>
                                    </VDropdown>
                                </div>
                                <div v-if="sideSheetStore.wikis.length === 0" class="empty-pages">
                                    {{ userStore.isLoggedIn ? t('sideSheet.noWikis') : t('sideSheet.noWikisNotLoggedIn') }}
                                </div>
                            </div>
                        </Transition>
                    </template>

                </SideSheetSection>

                <SideSheetSection :class="{ 'no-b-padding': !showFavorites }" @mouseover="handleMouseOver">
                    <template #header>
                        <div class="header-container" @click="handleClick('showFavorites')" :class="{ 'no-hover': hoverFavoritesButton }">
                            <template v-if="!collapsed">
                                <font-awesome-icon v-if="showFavorites" :icon="['fas', 'angle-down']"
                                    class="angle-icon" />
                                <font-awesome-icon v-else :icon="['fas', 'angle-right']" class="angle-icon" />
                            </template>
                            <font-awesome-icon :icon="['fas', 'star']" />
                            <div v-show="!hidden" class="header-title">
                                {{ t('sideSheet.favorites') }}
                            </div>
                            <div v-show="!hidden && !collapsed" @click.stop="addToFavorites(pageStore.name, pageStore.id)" class="header-btn" :class="{ 'disabled': isFavorite }"
                                v-tooltip="isFavorite ? t('label.addedAsFavorite') : t('label.addToFavorites')"
                                @mouseenter="hoverFavoritesButton = true" @mouseleave="hoverFavoritesButton = false">
                                <font-awesome-icon :icon="['fas', 'plus']" />
                            </div>
                        </div>
                    </template>

                    <template #content v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showFavorites">
                                <div v-for="favorite in sideSheetStore.favorites" class="content-item">
                                    <NuxtLink :to="$urlHelper.getPageUrl(favorite.name, favorite.id)"
                                        :class="{ 'is-here': favorite.id === pageStore.id }">
                                        <div class="link">
                                            {{ favorite.name }}
                                        </div>
                                    </NuxtLink>
                                    <div class="content-item-options" @click="removeFromFavorites(favorite.id)">
                                        <font-awesome-layers>
                                            <font-awesome-icon :icon="['far', 'star']" transform="left-1" />
                                            <font-awesome-icon :icon="['fas', 'slash']" transform="down-2 left-2"
                                                class="slash-bg" />
                                            <font-awesome-icon :icon="['fas', 'slash']" transform="left-2 shrink-2" />
                                        </font-awesome-layers>
                                    </div>
                                </div>
                                <div v-if="sideSheetStore.favorites.length === 0" class="empty-pages">
                                    {{ userStore.isLoggedIn ? t('sideSheet.noFavorites') : t('sideSheet.noFavoritesNotLoggedIn') }}
                                </div>

                            </div>
                        </Transition>
                    </template>

                </SideSheetSection>

                <SideSheetSection :class="{ 'no-b-padding': !showShared }" @mouseover="handleMouseOver">
                    <template #header>
                        <div class="header-container" @click="handleClick('showShared')">
                            <template v-if="!collapsed">
                                <font-awesome-icon v-if="showShared" :icon="['fas', 'angle-down']" class="angle-icon" />
                                <font-awesome-icon v-else :icon="['fas', 'angle-right']" class="angle-icon" />
                            </template>
                            <font-awesome-icon :icon="['fas', 'share-nodes']" />
                            <div v-show="!hidden" class="header-title">
                                {{ t('sideSheet.sharedWithMe') }}
                            </div>
                        </div>
                    </template>

                    <template #content v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showShared">
                                <div v-for="page in sideSheetStore.sharedPages" class="content-item">
                                    <NuxtLink :to="$urlHelper.getPageUrl(page.name, page.id)"
                                        :class="{ 'is-here': page.id === pageStore.id }">
                                        <div class="link">
                                            {{ page.name }}
                                        </div>
                                    </NuxtLink>
                                </div>
                                <div v-if="sideSheetStore.sharedPages.length === 0" class="empty-pages">
                                    {{ t('sideSheet.noSharedPages') }}
                                </div>
                            </div>
                        </Transition>
                    </template>
                </SideSheetSection>

                <SideSheetSection :class="{ 'no-b-padding': !showRecents }" @mouseover="handleMouseOver">
                    <template #header>
                        <div class="header-container" @click="handleClick('showRecents')">
                            <template v-if="!collapsed">
                                <font-awesome-icon v-if="showRecents" :icon="['fas', 'angle-down']"
                                    class="angle-icon" />
                                <font-awesome-icon v-else :icon="['fas', 'angle-right']" class="angle-icon" />
                            </template>
                            <font-awesome-icon :icon="['fas', 'clock-rotate-left']" />
                            <div v-show="!hidden" class="header-title">
                                {{ t('sideSheet.lastVisited') }}
                            </div>
                        </div>
                    </template>

                    <template #content v-if="!collapsed">
                        <Transition name="collapse">
                            <div v-if="showRecents">
                                <div v-for="recent in sideSheetStore.recentPages" class="content-item">
                                    <NuxtLink :to="$urlHelper.getPageUrl(recent.name, recent.id)"
                                        :class="{ 'is-here': recent.id === pageStore.id }">
                                        <div class="link">
                                            {{ recent.name }}
                                        </div>
                                    </NuxtLink>
                                </div>
                            </div>
                        </Transition>
                    </template>
                </SideSheetSection>
            </div>

        </perfect-scrollbar>

        <div id="SideSheetFooter">
            <div class="bg-fade"></div>
            <div class="sidesheet-content footer">
                <SideSheetSection class="no-b-padding help-section" @mouseover="handleMouseOver">
                    <template #header>
                        <div class="header-container no-hover help-header" @click="handleClick()">

                            <font-awesome-icon :icon="['fas', 'circle-question']" />
                            <div v-show="!hidden" class="header-title">
                                {{ t('sideSheet.help') }}
                            </div>
                        </div>
                    </template>

                    <template #content v-if="!collapsed && props.footerPages">
                        <div class="help-links">
                            <NuxtLink
                                :to="$urlHelper.getPageUrl(props.footerPages.documentation.name, props.footerPages.documentation.id)"
                                class="sidebar-link">
                                {{ t('sideSheet.documentation') }}
                            </NuxtLink>
                            <div class="link-divider-container">
                                <div class="link-divider"></div>
                            </div>
                            <NuxtLink :to="config.public.discord" class="sidebar-link" @mouseover="discordBounce = true"
                                @mouseleave="discordBounce = false">
                                <font-awesome-icon :icon="['fab', 'discord']" :bounce="discordBounce" /> {{
                                    t('label.discord') }}
                            </NuxtLink>
                        </div>
                    </template>

                </SideSheetSection>
            </div>
        </div>

        <ClientOnly>
            <SideSheetCreateWikiModal :show-modal="showCreateWikiModal" @close-wiki-modal="showCreateWikiModal = false" @wiki-created="handleWikiCreated" />
        </ClientOnly>
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
    height: 100%;

    &.not-logged-in {
        padding-top: 131px;
    }

    #SideSheetContainer {
        height: 100%;
        overscroll-behavior: none;
    }

    #SideSheetFooter {
        height: 110px;
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

        :deep(.header) {
            padding: 0px 12px;

            .header-title {
                padding: 0px 8px;
            }
        }

        #SideSheetFooter {
            width: 80px;

            .sidesheet-content {
                &.footer {
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    height: 70px;
                }
            }
        }
    }

    &.hide {
        width: 0px;
        overflow: hidden;
        visibility: hidden;
    }

    &:hover {
        box-shadow: 0px 0px 10px 0px rgba(0, 0, 0, 0.1);
    }

    @media (max-width: 900px) {
        box-shadow: 0px 0px 10px 0px rgba(0, 0, 0, 0.1);

        &.hide {
            transform: translateX(-100%);
            box-shadow: none;
        }
    }

    .no-b-padding {
        padding-bottom: 0px;

        .footer {
            padding-top: 0px;
        }
    }

    &.collapsed {
        .no-b-padding {
            margin-bottom: 16px;
        }
    }
}

.angle-icon-space {
    width: 16px;
    display: inline-block;
}

.help-links {
    color: @memo-grey-dark;
    display: flex;
    align-items: center;
    text-overflow: ellipsis;
    white-space: nowrap;
    overflow: hidden;

    .sidebar-link {
        color: @memo-grey-dark;
        text-decoration: none;
        display: flex;
        align-items: center;
        padding: 0 8px;
        font-size: 14px;

        &:hover {
            color: @memo-blue-link;
        }
    }
}

.empty-pages {
    padding: 8px 16px;
    color: @memo-grey-dark;
    font-style: italic;
    font-size: 14px;
}
</style>

<style lang="less">
svg.slash-bg {
    color: white !important;
}

.sidesheet-wikioptions {
    padding: 12px 0;
}

#SideSheet {
    #SideSheetFooter {
        .help-section {
            .help-header {
                padding-bottom: 0px;
            }
        }
    }

    &.collapsed {
        #SideSheetFooter {
            .help-section {
                margin-bottom: 0px;
            }
        }
    }
}

.help-links {
    height: 36px;

    .fa-discord {
        margin-right: 4px;
    }
}
</style>