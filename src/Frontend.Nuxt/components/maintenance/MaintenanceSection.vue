<script lang="ts" setup>

interface MethodData {
    url: string
    translationKey: string
}

interface Props {
    title: string
    description?: string
    methods: MethodData[]
    icon?: string[]
}
const props = defineProps<Props>()
const emit = defineEmits(['methodClicked'])

</script>
<template>
    <LayoutPanel :title="props.title">
        <template #description>
            <div v-if="props.description" class="description">
                {{ props.description }}
            </div>
            <div class="methods-buttons">
                <button v-for="method in props.methods" :key="method.url" class="btn btn-link" @click="emit('methodClicked', method.url)">
                    <font-awesome-icon :icon="props.icon" v-if="props.icon" />
                    {{ $t(method.translationKey) }}
                </button>
            </div>
        </template>
        <slot></slot>
    </LayoutPanel>
</template>

<style lang="less" scoped>
.methods-buttons {
    display: flex;
    flex-wrap: wrap;
    gap: 10px;
    margin-top: 10px;

    .btn {
        margin: 0;
    }
}

.description {
    margin-bottom: 10px;
}
</style>
