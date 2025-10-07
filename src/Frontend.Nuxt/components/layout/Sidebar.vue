<script setup lang="ts">
const { isDesktop } = useDevice()

interface Props {
    siteClass?: string
    hideDivider?: boolean
}

const props = withDefaults(defineProps<Props>(), {
    siteClass: '',
    hideDivider: false
})
</script>

<template>
    <div id="Sidebar" :class="siteClass" v-if="isDesktop">
        <div id="SidebarDivider" v-if="!hideDivider"></div>
        <div id="SidebarContent">
            <div id="SidebarSpacer"></div>

            <!-- Header/Default content slot -->
            <div id="DefaultSidebar">
                <slot name="header" v-if="$slots.header" />
            </div>

            <!-- Outline content slot -->
            <template v-if="$slots.outline">
                <div class="sidebarcard-divider-container" v-if="$slots.header">
                    <div class="sidebarcard-divider"></div>
                </div>

                <slot name="outline" />
            </template>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

#Sidebar {
    display: flex;
    align-items: stretch;
    flex-grow: 1;
    height: 100%;
    width: 100%;
    max-width: 338px;

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

    &.is-user {
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
        margin-left: 1rem;
        display: flex;
        justify-content: center;
        align-items: center;

        .sidebarcard-divider {
            height: 1px;
            background-color: @memo-grey-light;
            width: 100%;
        }
    }

    #OutlineSection {
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
