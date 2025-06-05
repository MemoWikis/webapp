<script lang="ts" setup>
import { usePageStore } from '../page/pageStore'
import { useTabsStore, Tab } from '../page/tabs/tabsStore'
import { SiteType } from '../shared/siteEnum'
import { ImageFormat } from '../image/imageFormatEnum'

const pageStore = usePageStore()
const tabsStore = useTabsStore()

const { isDesktop } = useDevice()
interface Props {
    showOutline?: boolean
    site: SiteType
}
const props = defineProps<Props>()

const { $urlHelper } = useNuxtApp()

</script>

<template>
    <div id="Sidebar" class="" v-if="isDesktop">
        <div id="SidebarDivider"></div>
        <div id="SidebarContent">
            <div id="SidebarSpacer"></div>
            <div id="DefaultSidebar">

                <SidebarCard v-if="props.site === SiteType.Page && pageStore.currentWiki && pageStore.id !== pageStore.currentWiki.id">
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

            </div>
            <template v-if="props.showOutline && pageStore.id && pageStore.name">

                <div class="sidebarcard-divider-container" v-show="tabsStore?.activeTab === Tab.Text && props.site === SiteType.Page && pageStore.currentWiki && pageStore.id !== pageStore.currentWiki.id">
                    <div class="sidebarcard-divider"></div>
                </div>

                <SidebarCard id="PageOutline" v-show="tabsStore?.activeTab === Tab.Text && !pageStore.textIsHidden">
                    <template v-slot:body>
                        <SidebarOutline />
                    </template>
                </SidebarCard>
                <SidebarCard id="PageOutline" v-show="tabsStore?.activeTab === Tab.Analytics">
                    <template v-slot:body>
                        <SidebarAnalyticsOutline />
                    </template>
                </SidebarCard>
            </template>

        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';
@import (reference) '~~/assets/sidebar.less';

#Sidebar {
    display: flex;
    align-items: stretch;
    flex-grow: 1;
    height: 100%;
    max-width: 300px;

    @media (max-width: 900px) {
        display: none;
    }

    #SidebarDivider {
        border-left: 1px solid @memo-grey-light;
        top: 0;
        flex-grow: 0;
    }

    #SidebarContent {
        flex-grow: 2;

        #SidebarSpacer {
            height: 55px;
        }
    }

    &.is-page {
        #SidebarDivider {
            margin-top: 20px;
            margin-bottom: 20px;
        }

        #SidebarContent {
            flex-grow: 2;

            #SidebarSpacer {
                height: 25px;
            }
        }

    }

    .sidebar-link-container {
        max-height: 17px;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
        max-width: 239px;

        &.wiki-container {
            display: -webkit-box;
            line-clamp: 2;
            -webkit-line-clamp: 2;
            -webkit-box-orient: vertical;

            justify-content: center;
            align-items: center;
            text-align: center;
            max-height: 36px;
            text-overflow: ellipsis;
            white-space: wrap;
        }
    }

    .sidebar-link {
        color: @memo-grey-dark;
        text-decoration: none;

        &:hover {
            color: @memo-blue-link;
        }
    }

    .sidebarcard-divider-container {
        margin-left: 20px;
        display: flex;
        justify-content: center;
        align-items: center;

        .sidebarcard-divider {
            height: 1px;
            background-color: @memo-grey-light;
            width: 100%;

        }
    }

    #PageOutline {
        margin-top: 20px;
        position: sticky;
        top: 60px;

        .outline-title {
            cursor: pointer;
            transition: all 0.1s ease-in-out;

            &:hover {
                color: @memo-blue-link;
            }

            &.current-heading {
                color: @memo-blue;
            }
        }
    }
}

#DefaultSidebar {
    height: 145px;
    // :deep(.sidebar-title) {
    //     padding-bottom: 0px;
    //     height: 30px;
    // }

    #SidebarHelpBody {
        display: flex;
        align-items: center;
        padding-bottom: 0px;

        .link-divider-container {
            height: 20px;
            width: 25px;
            display: flex;
            justify-content: center;
            align-items: center;

            .link-divider {
                width: 1px;
                height: 100%;
                background: @memo-grey-light;
            }
        }
    }
}
</style>

<style lang="less">
.sidesheet-open {
    @media (max-width: 1209px) {

        #Sidebar {
            display: none;
        }
    }
}
</style>