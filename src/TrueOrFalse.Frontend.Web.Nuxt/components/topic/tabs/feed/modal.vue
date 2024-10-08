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
            <div class="feed-modal-content-change" v-if="oldContent && newContent">
                <div class="feed-modal-old-content feed-modal-content" v-html="props.oldContent"></div>
                <div class="feed-modal-new-content feed-modal-content" v-html="props.newContent"></div>
            </div>
        </template>
    </Modal>
</template>


<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.feed-modal-content-change {
    display: flex;
    justify-content: space-between;
    margin-top: 20px;
    .feed-modal-old-content {
        border-right: 1px solid @memo-grey-lighter;
    }
    .feed-modal-new-content {
    }

    .feed-modal-content {
        width: 50%;
        padding: 16px;
    }
}
</style>