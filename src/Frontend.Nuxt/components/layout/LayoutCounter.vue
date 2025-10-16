<script setup lang="ts">

interface Props {
    value: string | number
    label: string
    icon?: string | string[]
    color?: string
    iconColor?: string
    formatNumber?: boolean
    urlValue?: string
    labelTooltip?: string
}

const props = withDefaults(defineProps<Props>(), {
    formatNumber: true
})

const formattedValue = computed(() => {
    if (!props.formatNumber) {
        return props.value
    }

    return getFormattedNumber(props.value)
})
</script>

<template>
    <div class="layout-counter">
        <div class="counter-header">
            <div v-if="icon" class="counter-icon">
                <font-awesome-icon :icon="icon" :style="{ color: iconColor }" />
            </div>
            <div class="counter-label" v-tooltip="labelTooltip">{{ label }}</div>
        </div>
        <NuxtLink class="link-to-all-users" :to="props.urlValue" v-if="props.urlValue">
            <div class="counter-value" :style="{ color: color }">{{ formattedValue }}</div>
        </NuxtLink>
        <div v-else class="counter-value" :style="{ color: color }">{{ formattedValue }}</div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.layout-counter {
    height: 100%;
    width: 100%;
    padding: 8px 0;
    display: flex;
    flex-direction: column;
    justify-content: space-between;

    .counter-header {
        display: flex;
        align-items: center;
        margin-bottom: 4px;

        .counter-icon {
            margin-right: 8px;
            font-size: 1.5rem;
            color: @memo-grey;
        }

        .counter-label {
            font-size: 14px;
            font-weight: 500;
            color: @memo-grey;
            overflow-wrap: break-word;
        }
    }

    .counter-value {
        font-size: 28px;
        font-weight: 700;
        color: @memo-grey-darker;
        line-height: 1.2;
    }
}
</style>
