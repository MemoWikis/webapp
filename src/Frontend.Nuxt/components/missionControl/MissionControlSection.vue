<script setup lang="ts">
defineProps({
    title: {
        type: String,
        default: '',
    }
})
</script>

<template>
    <div class="mission-control-section">
        <div class="section-header" v-if="title">
            <h2 class="section-title">{{ title }}</h2>
            <div class="section-actions" v-if="$slots.actions">
                <slot name="actions"></slot>
            </div>
        </div>
        <div class="section-content">
            <slot></slot>
        </div>
    </div>
</template>

<style lang="less" scoped>
.mission-control-section {
    background-color: white;
    border-radius: 8px;
    box-shadow: 0 2px 8px rgba(0, 0, 0, 0.08);
    margin-bottom: 24px;
    overflow: hidden;

    .section-header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 16px 20px;
        border-bottom: 1px solid #eaeaea;

        .section-title {
            font-size: 18px;
            font-weight: 600;
            color: #333;
            margin: 0;
        }

        .section-actions {
            display: flex;
            align-items: center;
            gap: 8px;
        }
    }
}
</style>