<script lang="ts" setup>
import { FooterTopics, useTopicStore } from '../topic/topicStore'
import { useTabsStore, Tab } from '../topic/tabs/tabsStore'
import { useUserStore } from '../user/userStore'

const topicStore = useTopicStore()
const tabsStore = useTabsStore()
const userStore = useUserStore()

const { isDesktop } = useDevice()
interface Props {
    footerTopics: FooterTopics
    showOutline?: boolean
}
const props = defineProps<Props>()
const config = useRuntimeConfig()

const discordBounce = ref(false)
const { $urlHelper } = useNuxtApp()


function scrollToTitle() {
    document.getElementById("TopicTitle")?.scrollIntoView({ behavior: 'smooth', block: 'end' })
}
</script>

<template>
    <div id="Sidebar" class="col-lg-3 hidden-md hidden-sm hidden-xs container" v-if="isDesktop">
        <div id="SidebarDivider"></div>
        <div id="SidebarContent">
            <div id="SidebarSpacer"></div>
            <div id="DefaultSidebar">

                <SidebarCard>
                    <template v-slot:body>

                        <div class="overline-s no-line">
                            <NuxtLink
                                :to="$urlHelper.getTopicUrl(props.footerTopics.rootWiki.name, props.footerTopics.rootWiki.id)"
                                class="sidebar-link">
                                {{ props.footerTopics.rootWiki.name }}
                            </NuxtLink>
                        </div>
                        <div v-if="userStore.isLoggedIn && userStore.personalWiki" class="overline-s no-line">
                            <NuxtLink
                                :to="$urlHelper.getTopicUrl(userStore.personalWiki.name, userStore.personalWiki.id)"
                                class="sidebar-link">
                                Dein Wiki
                            </NuxtLink>
                        </div>
                        <div v-else class="overline-s no-line">
                            <NuxtLink
                                :to="$urlHelper.getTopicUrl(props.footerTopics.memoTopics[0].name, props.footerTopics.memoTopics[0].id)"
                                class="sidebar-link">
                                {{ props.footerTopics.memoTopics[0].name }}
                            </NuxtLink>
                        </div>

                    </template>
                </SidebarCard>

                <SidebarCard>
                    <template v-slot:header>Hilfe</template>
                    <template v-slot:body>
                        <div id="SidebarHelpBody">
                            <NuxtLink
                                :to="$urlHelper.getTopicUrl(props.footerTopics.documentation.name, props.footerTopics.documentation.id)"
                                class="sidebar-link">
                                Doku
                            </NuxtLink>
                            <div class="link-divider-container">
                                <div class="link-divider"></div>
                            </div>
                            <NuxtLink :to="config.public.discord" class="sidebar-link" @mouseover="discordBounce = true"
                                @mouseleave="discordBounce = false">
                                <font-awesome-icon :icon="['fab', 'discord']" :bounce="discordBounce" /> Discord
                            </NuxtLink>
                        </div>
                    </template>
                </SidebarCard>

            </div>
            <template v-if="props.showOutline && topicStore.id && topicStore.name">

                <div class="sidebarcard-divider-container" v-show="tabsStore?.activeTab == Tab.Topic">
                    <div class="sidebarcard-divider"></div>
                </div>

                <SidebarCard id="TopicOutline" v-show="tabsStore?.activeTab == Tab.Topic">
                    <template v-slot:header>
                        <div @click="scrollToTitle" class="outline-title">{{ topicStore.name }}</div>
                    </template>
                    <template v-slot:body>

                        <SidebarOutline />
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

    &.is-topic {
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

    #TopicOutline {
        margin-top: 20px;
        position: sticky;
        top: 60px;

        .outline-title {
            cursor: pointer;
            transition: all 0.1s ease-in-out;

            &:hover {
                color: @memo-blue-link;
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