<script lang="ts" setup>
import { CustomPino } from '~/logs/logger';
import { Topic } from '../topic/topicStore'

const { isDesktop } = useDevice()
interface Props {
    documentation: Topic
}
const props = defineProps<Props>()
const config = useRuntimeConfig()

const discordBounce = ref(false)

function doTestLog() {
    const logger = new CustomPino()
    logger.log("Logging Test from Sidebar")
}

const showTestLogButton = config.public.showTestLogButton
const { $urlHelper } = useNuxtApp()
</script>

<template>
    <div id="Sidebar" class="col-lg-3 hidden-md hidden-sm hidden-xs container" v-if="isDesktop">
        <div id="SidebarDivider"></div>
        <div id="SidebarContent">
            <div id="SidebarSpacer"></div>
            <SidebarCard>
                <template v-slot:header>
                    <NuxtLink :to="$urlHelper.getTopicUrl(props.documentation.Name, props.documentation.Id)"
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
            <SidebarCard v-if="showTestLogButton">
                <template v-slot:body>
                    <button class="memo-button btn-link" @click="doTestLog">Logging-Test</button>
                </template>
            </SidebarCard>
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
            height: 60px;
        }
    }

    .sidebar-link {
        color: @memo-grey-dark;
        text-decoration: none;

        &:hover {
            color: @memo-blue-link;
        }
    }
}
</style>