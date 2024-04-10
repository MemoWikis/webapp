<script lang="ts" setup>
interface Props {
    name: string
}

const props = defineProps<Props>()
</script>

<template>
    <div class="grid-item-placeholder">
        <div class="left-spacer"></div>
        <div class="image-container">
            <div class="image-placeholder"></div>
        </div>

        <div class="body-placeholder">
            <div class="topic-name-placeholder">
                {{ props.name }}
            </div>

            <div class="label-placeholder"></div>

            <div class="knowledgebar-container">
                <div class="knowledgebar-placeholder"></div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.grid-item-placeholder {
    user-select: none;
    display: flex;
    flex-wrap: nowrap;
    padding: 10px 0px;
    background: white;
    // border-top: solid 1px #DDDDDD;
    position: relative;
    max-height: 78px;

    .left-spacer {
        width: 32px;
        max-width: 32px;
    }

    .image-container {
        max-width: 40px;

        .image-placeholder {
            background: @memo-grey-lighter;
            height: 40px;
            width: 40px;
            min-width: 40px;
            min-height: 40px;
        }
    }

    .body-placeholder {
        display: flex;
        flex-direction: column;
        padding-left: 8px;

        .topic-name-placeholder {
            font-size: 18px;
            color: @memo-green;
            height: 26px;
            width: 100%;
        }

        .label-placeholder {
            min-height: 18px;
            height: 18px;
            width: 250px;
            background: @memo-grey-lighter;
        }

        .knowledgebar-container {
            width: 250px;
            height: 18px;

            padding-top: 6px;

            .knowledgebar-placeholder {
                height: 6px;
                width: 180px;
                background: @memo-grey-lighter;
            }
        }
    }
}
</style>