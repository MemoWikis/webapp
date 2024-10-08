<script lang="ts" setup>
interface Props {
    show: boolean,
    oldContent?: string,
    newContent?: string
}

const props = defineProps<Props>()
const emit = defineEmits(['close'])
</script>

<template>

    <Modal :show="props.show" @close="emit('close')">
        <template #body>
            test
        </template>
    </Modal>
</template>