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

            <div class="feed-modal-content-change" v-if="oldContent && newContent">
                <div class="feed-modal-old-content" v-html="props.oldContent"></div>
                <div class="feed-modal-new-content" v-html="props.newContent"></div>
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
        width: 50%;
        border-right: 1px solid @memo-grey-lighter;
    }
    .feed-modal-new-content {
        width: 50%;
    }
}
</style>