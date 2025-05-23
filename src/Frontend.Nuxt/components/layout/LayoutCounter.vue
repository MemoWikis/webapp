<script setup lang="ts">
interface Props {
    value: string | number
    label: string
    icon?: string
    color?: string
    iconColor?: string
    formatNumber?: boolean
}

const props = withDefaults(defineProps<Props>(), {
    formatNumber: true
})

const { t } = useI18n()

const formattedValue = computed(() => {
    if (!props.formatNumber || typeof props.value !== 'number') {
        return props.value
    }

    const value = props.value

    if (value >= 1000000) {
        // Format as millions
        const millions = (value / 1000000).toFixed(1).replace(/\.0$/, '')
        return t('counter.millions', { value: millions })
    } else if (value >= 1000) {
        // Format as thousands 
        const thousands = (value / 1000).toFixed(1).replace(/\.0$/, '')
        return t('counter.thousands', { value: thousands })
    }

    return value.toString()
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
    width: 100%;
    padding: 8px 0;
    display: flex;
    flex-direction: column;

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
            white-space: nowrap;
            overflow: hidden;
            text-overflow: ellipsis;
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
