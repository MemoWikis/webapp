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
        <template #description v-if="props.description" class="description">
            {{ props.description }}
        </template>
        <LayoutCard :size="LayoutContentSize.Flex">
            <div v-for="method in props.methods" :size="LayoutContentSize.Flex">
                <button class="btn btn-link" @click="emit('methodClicked', method.url)">
                    <font-awesome-icon :icon="props.icon" v-if="props.icon" />
                    {{ $t(method.translationKey) }}
                </button>
            </div>
        </LayoutCard>
        <slot></slot>

    </LayoutPanel>
</template>
