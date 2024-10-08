<script lang="ts" setup>
interface Props {
    show: boolean,
    oldContent?: string,
    newContent?: string,
    diffContent?: string
}

const props = defineProps<Props>()
const emit = defineEmits(['close'])
</script>

<template>

    <Modal :show="props.show" @close="emit('close')">
        <template #body>
            <!-- <div class="feed-modal-content-change" v-if="oldContent && newContent">
                <div class="feed-modal-old-content feed-modal-content" v-html="props.oldContent"></div>
                <div class="feed-modal-new-content feed-modal-content" v-html="props.newContent"></div>
            </div> -->

            <div class="feed-modal-content-change" v-if="props.diffContent">
                <div class="feed-modal-diff-content" v-html="props.diffContent"></div>
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

    .feed-modal-diff-content {
        padding: 16px;
    }
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.feed-modal-content-change {
    ins {
        background: fade(@memo-green, 10%);
        border-radius: 4px;
        color: @memo-green;

        img {
            border: solid 4px @memo-green;
        }
    }
    del {
        background: fade(@memo-wuwi-red, 10%);
        text-decoration: line-through;
        border-radius: 4px;
        color: @memo-wuwi-red;

        img {
            border: solid 4px @memo-wuwi-red;
        }
    }
}
</style>