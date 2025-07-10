<script lang="ts" setup>
import { BreadcrumbItem } from './searchHelper'

interface Props {
    breadcrumbPath: BreadcrumbItem[]
    itemName: string
}

const props = defineProps<Props>()

const rootCrumb = computed(() => {
    return props.breadcrumbPath && props.breadcrumbPath.length > 0 ? props.breadcrumbPath[0] : null
})

const pathCrumbs = computed(() => {
    return props.breadcrumbPath && props.breadcrumbPath.length > 1 ? props.breadcrumbPath.slice(1) : []
})

const ariaId = useId()
</script>

<template>
    <div v-if="props.breadcrumbPath && props.breadcrumbPath.length > 0" class="breadcrumb-path">
        <!-- Show root crumb -->
        <span v-if="rootCrumb" class="breadcrumb-item first-crumb">
            <span class="breadcrumb-name">{{ rootCrumb.name }}</span>
            <span class="breadcrumb-separator"> › </span>
        </span>

        <!-- Show path crumbs: dropdown if more than 2, otherwise show directly -->
        <template v-if="pathCrumbs.length > 2">
            <VDropdown :aria-id="ariaId" :distance="0">
                <span class="breadcrumb-dropdown">
                    <span class="breadcrumb-ellipsis">...</span>
                    <span class="breadcrumb-separator"> › </span>
                </span>

                <template #popper="{ hide }">
                    <div v-for="crumb in pathCrumbs" :key="crumb.id" class="dropdown-item" @click="hide">
                        <span class="breadcrumb-name">{{ crumb.name }}</span>
                    </div>
                </template>
            </VDropdown>
        </template>

        <template v-else>
            <span v-for="(crumb) in pathCrumbs" :key="crumb.id" class="breadcrumb-item">
                <span class="breadcrumb-name">{{ crumb.name }}</span>
                <span class="breadcrumb-separator"> › </span>
            </span>
        </template>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.breadcrumb-path {
    text-overflow: ellipsis;
    overflow: hidden;
    white-space: nowrap;
    font-size: 0.85em;
    color: @memo-grey;
    position: relative;
    display: flex;
    align-items: center;

    .breadcrumb-item {
        display: inline-flex;
        align-items: center;

        .breadcrumb-name {
            max-width: 100px;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }

        &.first-crumb .breadcrumb-name {
            max-width: 100px;
        }

        .breadcrumb-separator {
            margin: 0 4px;
            color: @memo-grey;
            flex-shrink: 0;
        }
    }

    .breadcrumb-dropdown {
        display: inline-flex;
        align-items: center;
        position: relative;
        cursor: pointer;

        .breadcrumb-ellipsis {
            padding: 2px 4px;
            border-radius: 3px;
            transition: background-color 0.2s;

            &:hover {
                background-color: @memo-grey-lighter;
            }
        }

        .breadcrumb-separator {
            margin: 0 4px;
            color: @memo-grey;
            flex-shrink: 0;
        }
    }
}

.dropdown-item {
    padding: 6px 12px;
    cursor: pointer;
    transition: background-color 0.2s;
    display: block;

    &:hover {
        background-color: @memo-grey-lighter;
    }

    .breadcrumb-name {
        max-width: 180px;
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
        color: @memo-grey-dark;
    }
}
</style>
