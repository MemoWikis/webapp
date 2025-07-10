<script lang="ts" setup>
import { BreadcrumbItem } from './searchHelper'

interface Props {
    breadcrumbPath: BreadcrumbItem[]
    itemName: string
}

const props = defineProps<Props>()
</script>

<template>
    <div v-if="props.breadcrumbPath && props.breadcrumbPath.length > 0" class="breadcrumb-path">
        <span v-for="(crumb) in props.breadcrumbPath" :key="crumb.id" class="breadcrumb-item">
            <span v-tooltip="crumb.name">{{ crumb.name }}</span>
            <span class="breadcrumb-separator"> › </span>
        </span>
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

    .breadcrumb-item {
        display: inline;

        .breadcrumb-separator {
            margin: 0 4px;
            color: @memo-grey;
        }
    }
}
</style>
