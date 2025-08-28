<script setup lang="ts">
import { usePageStore } from '../page/pageStore'
import { useTabsStore, Tab } from '../page/tabs/tabsStore'
import { SiteType } from '../shared/siteEnum'
import { ImageFormat } from '../image/imageFormatEnum'

const pageStore = usePageStore()
const tabsStore = useTabsStore()
const { $urlHelper } = useNuxtApp()

interface Props {
    showOutline?: boolean
}

const props = withDefaults(defineProps<Props>(), {
    showOutline: true
})
</script>

<template>
    <LayoutSidebar site-class="is-page">
        <template #header v-if="pageStore.currentWiki && pageStore.id !== pageStore.currentWiki.id">
            <SidebarCard>
                <template v-slot:body>
                    <Image :src="pageStore.currentWiki.imgUrl" class="page-header-image" :format="ImageFormat.WikiLogo" :show-license="false" :min-height="80" :min-width="80" :alt="`${pageStore.currentWiki.name}'s image'`" />

                    <div class="overline-s no-line sidebar-link-container wiki-container">
                        <NuxtLink
                            :to="$urlHelper.getPageUrl(pageStore.currentWiki.name, pageStore.currentWiki.id)"
                            class="sidebar-link">
                            {{ pageStore.currentWiki.name }}
                        </NuxtLink>
                    </div>
                </template>
            </SidebarCard>
        </template>

        <template #outline v-if="props.showOutline && pageStore.id && pageStore.name">
            <SidebarCard id="OutlineSection" v-show="tabsStore?.activeTab === Tab.Text && !pageStore.textIsHidden">
                <template #body>
                    <SidebarPageOutline />
                </template>
            </SidebarCard>

            <SidebarCard id="OutlineSection" v-show="tabsStore?.activeTab === Tab.Analytics">
                <template #body>
                    <SidebarAnalyticsOutline />
                </template>
            </SidebarCard>
        </template>
    </LayoutSidebar>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';
</style>
