<script lang="ts" setup>

interface MethodData {
    url: string
    label: string
}

interface Props {
    title: string
    description?: string
    methods: MethodData[]
}
const props = defineProps<Props>()
const emit = defineEmits(['methodClicked'])

</script>

<template>
    <div class="maintenance-section col-xs-12 col-lg-6">
        <h3>
            {{ props.title }}
        </h3>
        <div v-if="props.description">
            {{ props.description }}
        </div>
        <div class="maintenance-method-container">
            <div v-for="m in props.methods">
                <button class="btn btn-link" @click="emit('methodClicked', m.url)">
                    {{ m.label }}
                </button>
            </div>
            <slot></slot>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.maintenance-section {
    border: solid 1px @memo-grey-lighter;
    padding: 8px;
    margin: 8px;
}
</style>