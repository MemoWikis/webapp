<script setup lang="ts">

interface Props {
    value: string | number
    label: string
    icon?: string
    color?: string
    iconColor?: string
    formatNumber?: boolean
}

const { formattedNumber } = useFormatNumber()

const props = withDefaults(defineProps<Props>(), {
    formatNumber: true
})

const formattedValue = computed(() => {
    if (!props.formatNumber) {
        return props.value
    }

    return formattedNumber(props.value)
})
</script>

<template>
    <div class="layout-counter">
        <div class="counter-header">
            <div v-if="icon" class="counter-icon">
                <font-awesome-icon :icon="icon" :style="{ color: iconColor }" />
            </div>
            <div class="counter-label">{{ label }}</div>
        </div>
        <div class="counter-value" :style="{ color: color }">{{ formattedValue }}</div>
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
        margin-bottom: 8px;

        .counter-icon {
            margin-right: 8px;
            font-size: 14px;
            color: @memo-grey-dark;
        }

        .counter-label {
            font-size: 14px;
            font-weight: 500;
            color: @memo-grey-dark;
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
