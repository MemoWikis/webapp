<script lang="ts" setup>
const { isDesktop } = useDevice()
</script>

<template>
    <div id="Sidebar" class="" v-if="isDesktop">
        <div id="SidebarDivider"></div>
        <div id="SidebarContent">
            <div id="SidebarSpacer"></div>
            <div id="DefaultSidebar">
            </div>
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
    margin: 0;

    @media (max-width: 1300px) {
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