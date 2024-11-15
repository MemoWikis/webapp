<script lang="ts" setup>

import { debounce } from 'underscore'
import { TinyPageModel, usePageStore } from '../page/pageStore'

const pageStore = usePageStore()
const windowWidth = ref(0)

const resize = () => {
    if (window) {
        windowWidth.value = window.innerWidth
    }
}


const handleWidth = (newWidth: number) => {
    if (newWidth < 901) {
        hidden.value = true
        collapsed.value = true
        previouslyCollapsed.value = true
    }
    else if (newWidth < 1701 && newWidth > 900) {
        hidden.value = false
        collapsed.value = true
        previouslyCollapsed.value = true
    } else {
        hidden.value = false
        collapsed.value = false
        previouslyCollapsed.value = false
    }
}

onMounted(() => {
    if (window) {
        windowWidth.value = window.innerWidth
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

const mockData = ref(
    {
        title: 'Favourites',
        links: [
            {
                title: 'Home',
                url: '/'
            },
            {
                title: 'About',
                url: '/Ueber-uns/1876'
            },
            {
                title: 'Contact',
                url: '/Impressum'
            }
        ]
    }
)

const previouslyCollapsed = ref(false)
const recentPages = ref<TinyPageModel[]>()

const handleRecentPage = () => {
    const tinyPageModel = {
        id: pageStore.id,
        name: pageStore.name,
        imgUrl: pageStore.imgUrl
    } as TinyPageModel

    if (recentPages.value) {
        recentPages.value = recentPages.value.filter((page) => page.id !== tinyPageModel.id)

        if (recentPages.value.length > 5) {
            recentPages.value.pop()
        }

        recentPages.value.unshift(tinyPageModel)
    } else {
        recentPages.value = [tinyPageModel]
    }

}
watch(() => pageStore.id, (id) => {
    if (id) {
        handleRecentPage()
    }
})

onMounted(() => {
    handleRecentPage()
})

const { $urlHelper } = useNuxtApp()

</script>
<template>
    <div id="SideSheet" :class="{ 'collapsed': collapsed, 'hide': hidden }" @mouseover="collapsed = false" @mouseleave="collapsed = previouslyCollapsed">
        <perfect-scrollbar>

            <SidesheetSection>
                <template #header>
                    <h4>
                        <font-awesome-icon :icon="['fas', 'star']" class="header-icon" />
                        <div v-show="!hidden" :class="{ 'smol': collapsed }" class="header-title">
                            {{ mockData.title }}

                        </div>

                    </h4>
                </template>

                <template #content>
                    <NuxtLink v-for="link in mockData.links" :to="link.url">
                        <div class="link">
                            {{ link.title }}
                        </div>
                    </NuxtLink>
                </template>
            </SidesheetSection>

            <SidesheetSection>
                <template #header>
                    <h4>
                        <font-awesome-icon :icon="['fas', 'clock-rotate-left']" class="header-icon" />
                        <div v-show="!hidden" :class="{ 'smol': collapsed }" class="header-title">
                            Recent
                        </div>

                    </h4>
                </template>

                <template #content>
                    <NuxtLink v-for="recentPage in recentPages" :to="$urlHelper.getPageUrl(recentPage.name, recentPage.id)">
                        <div class="link">
                            {{ recentPage.name }}
                        </div>
                    </NuxtLink>
                </template>
            </SidesheetSection>

        </perfect-scrollbar>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';
@memo-grey-lightest: #f9f9f9;

#SideSheet {
    height: 100%;
    background: @memo-grey-lightest;
    width: 400px;
    position: fixed;
    z-index: 2000;
    transition: width 0.3s ease-in-out;
    padding-top: 40px;

    h4 {
        display: flex;
        flex-wrap: nowrap;
        align-items: center;
        flex-direction: row;

        .smol {
            font-size: 14px;
        }
    }

    .header-icon {
        font-size: 24px;
        margin-right: 12px;
        transition: all 0.3s ease-in-out;
    }

    &.collapsed {
        width: 100px;

        .header-icon {
            font-size: 36px;
            margin-right: 0px;
        }

        h4 {
            flex-direction: column;
        }
    }

    &.hide {
        width: 0px;
    }
}
</style>