<script lang="ts" setup>
import { Topic, useTopicStore } from '../topic/topicStore'
import { useTabsStore, Tab } from '../topic/tabs/tabsStore'

const topicStore = useTopicStore()
const tabsStore = useTabsStore()

const { isDesktop } = useDevice()
interface Props {
    documentation: Topic
    showOutline?: boolean
}
const props = defineProps<Props>()
const config = useRuntimeConfig()

const discordBounce = ref(false)
const { $urlHelper } = useNuxtApp()
</script>

<template>
    <div id="Sidebar" class="col-lg-3 hidden-md hidden-sm hidden-xs container" v-if="isDesktop">
        <div id="SidebarDivider"></div>
        <div id="SidebarContent">
            <div id="SidebarSpacer"></div>
            <SidebarCard>
                <template v-slot:header>
                    <NuxtLink :to="$urlHelper.getTopicUrl(props.documentation.name, props.documentation.id)"
                        class="sidebar-link">
                        Zur Dokumentation
                    </NuxtLink>
                </template>
            </SidebarCard>
            <SidebarCard>
                <template v-slot:header>
                    <NuxtLink :to="config.public.discord" class="sidebar-link" @mouseover="discordBounce = true"
                        @mouseleave="discordBounce = false">
                        <font-awesome-icon :icon="['fab', 'discord']" :bounce="discordBounce" /> Discord
                    </NuxtLink>
                </template>
                <template v-slot:body>
                    Du willst dich mit uns unterhalten?
                    <br />
                    Dann triff dich mit uns auf Discord!

                </template>
            </SidebarCard>
            <template v-if="props.showOutline && topicStore.id && topicStore.name">
                <div class="sidebarcard-divider-container">
                    <div class="sidebarcard-divider"></div>
                </div>
                <SidebarCard id="TopicOutline" v-show="tabsStore?.activeTab == Tab.Topic">
                    <template v-slot:header>{{ topicStore.name }}</template>
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

    &.is-topic {
        #SidebarDivider {
            margin-top: 20px;
            margin-bottom: 20px;
        }

    }

    #SidebarContent {
        flex-grow: 2;

        #SidebarSpacer {
            height: 25px;
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
    }
}
</style>