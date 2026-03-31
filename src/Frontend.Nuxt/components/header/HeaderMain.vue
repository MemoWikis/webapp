<script lang="ts" setup>
import { VueElement } from 'vue'
import { useUserStore } from '../user/userStore'
import { QuestionItem, SearchType, PageItem, UserItem } from '~~/components/search/searchHelper'
import { SiteType } from '../shared/siteEnum'
import { BreadcrumbItem } from './breadcrumbItems'
import { useSideSheetStore } from '../sideSheet/sideSheetStore'

interface Props {
    site: SiteType
    questionPageData?: {
        primaryPageName: string
        primaryPageUrl: string
        title: string
    }
    breadcrumbItems?: BreadcrumbItem[]
}
const props = defineProps<Props>()

const { t } = useI18n()

const userStore = useUserStore()
const sideSheetStore = useSideSheetStore()

const showSearch = ref(false)

const openUrl = async (val: PageItem | QuestionItem | UserItem) => {
    if (isMobile || window?.innerWidth < 480)
        showSearch.value = false
    return await navigateTo(val.url)
}

const showRegisterButton = ref(false)
const handleScroll = () => {
    showSearch.value = false

    var scrollTop = document.documentElement.scrollTop
    if (scrollTop > 59)
        showRegisterButton.value = true
    else
        showRegisterButton.value = false
}

const handleResize = () => {
    if (showSearch.value)
        return
    if (window.innerWidth < 769) {
        showSearch.value = false
    }
}

const { isDesktopOrTablet, isMobile } = useDevice()
const distance = computed(() => {
    return userStore.isLoggedIn ? 24 : 6
})

onBeforeMount(() => {
    if (isMobile)
        showSearch.value = false
})
const headerContainer = ref<VueElement>()
const headerExtras = ref<VueElement>()

onMounted(async () => {
    if (!userStore.isLoggedIn || window?.innerWidth < 769 || isMobile) {
        showSearch.value = false
    }
    if (typeof window != "undefined") {
        window.addEventListener('resize', handleResize)
        window.addEventListener('scroll', handleScroll)
    }
})

const partialLeft = ref()
const navOptions = ref()

const { $vfm } = useNuxtApp()
const { openedModals } = $vfm
const modalIsOpen = ref(false)

watch(openedModals, (val) => {
    if (val.length > 0)
        modalIsOpen.value = true
    else
        modalIsOpen.value = false
}, { deep: true, immediate: true })

const hidePartial = computed(() => {
    if (typeof window != "undefined" && window.scrollY > 59)
        return false
    else if (userStore.isLoggedIn)
        return false
    else return true
})


onMounted(() => {
    if (import.meta.client) {
        handleScroll()
    }
})

const { sideSheetOpen } = useSideSheetState()

</script>

<template>
    <div id="Navigation" class="breadcrumb-bar">
        <div class="nav-container" :class="{ 'sidesheet-open': sideSheetOpen }">
            <div class="header-container" ref="headerContainer">
                <div class="main-container logged-in">
                    <div class="partial start" ref="partialLeft">
                        <HeaderBreadcrumb :site="props.site" :show-search="false"
                            :question-page-data="props.questionPageData"
                            :custom-breadcrumb-items="props.breadcrumbItems" :partial-left="partialLeft" />
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#Navigation.breadcrumb-bar {
    height: 36px;
    min-height: 36px;
    font-size: 13px;
    overflow: hidden;
    line-height: 21px;
    background-color: @memo-grey-lighter;
    border-bottom: 1px solid @memo-grey-light;
    position: relative;
    z-index: 98;
    white-space: nowrap;
    width: 100%;
    display: flex;
    justify-content: center;
    align-items: center;

    .nav-container {
        display: flex;
        justify-content: center;
        align-items: center;
        min-height: 36px;
        width: 100%;
        max-width: 1600px;

        @media (min-width: 900px) {
            padding-left: 80px;
        }

        &.sidesheet-open {
            padding-left: 410px;

            @media (max-width: 900px) {
                padding-left: 0;
            }

            @media (min-width: 1980px) {
                padding-left: clamp(80px, calc(410px - (100vw - 1980px)), 410px);
            }
        }
    }

    .header-container,
    .main-container {
        display: flex;
        align-items: center;
        height: 100%;
        overflow: hidden;
        width: 100%;
    }

    .header-container {
        width: 100%;
    }

    .main-container {
        padding: 0 16px;
    }

    .partial {
        height: 100%;
        display: flex;
        align-items: center;
        flex-grow: 1;

        &.start {
            align-items: center;
        }
    }
}
</style>