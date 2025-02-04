<script setup lang="ts">
import { HeroContentAlignEnum, HeroImgPosEnum } from './HeroBlockEnums'

const slots = useSlots()

interface Props {
    align?: HeroContentAlignEnum,
    imagePos?: HeroImgPosEnum
}

const props = withDefaults(defineProps<Props>(), {
    align: HeroContentAlignEnum.Left,
    imagePos: HeroImgPosEnum.Left
})

</script>

<template>
    <section>
        <div class="hero-container" :class="`${align}`">
            <div class="hero-image" v-if="slots.image && imagePos === HeroImgPosEnum.Left">
                <slot name="image" />
            </div>
            <div class="hero-content" :class="{ 'no-image': !slots.image, 'left': imagePos === HeroImgPosEnum.Left && slots.image, 'right': imagePos === HeroImgPosEnum.Right && slots.image }">
                <div class="hero-header" v-if="slots.header">
                    <slot name="header" />
                </div>
                <div class="hero-text" v-if="slots.text">
                    <slot name="text" />
                </div>
                <div class="hero-actions" v-if="slots.buttons">
                    <slot name="buttons" />
                </div>
            </div>
            <div class="hero-image" v-if="slots.image && imagePos === HeroImgPosEnum.Right">
                <slot name="image" />
            </div>

        </div>
    </section>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

section {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding-top: 2rem;
    padding-bottom: 2rem;
    min-height: 100px;
}

.hero-container {
    display: flex;
    align-items: center;
    justify-content: space-between;
    width: 100%;
    // max-width: 1160px;
    max-width: 860px;
    margin: auto;

    &.center {
        text-align: center;
        justify-content: center;

        .hero-content {
            align-items: center;
            padding-left: 0;
        }
    }

    &.left {
        text-align: left;
        justify-content: flex-start;

        .hero-content {
            align-items: flex-start;
        }
    }

    &.right {
        text-align: right;
        justify-content: flex-end;

        .hero-content {
            align-items: flex-end;
        }
    }

    @media (max-width: 768px) {
        flex-direction: column;
    }
}

.hero-image {
    width: 50%;

    @media (max-width: 768px) {
        width: 100%;

        :slotted(img) {
            width: 100%;
        }
    }
}

.hero-content {
    display: flex;
    flex-direction: column;
    padding: 0 4rem;
    // align-items: center;

    width: 50%;

    &.no-image {
        width: 100%;
        padding: 0;
    }

    @media (max-width: 768px) {
        width: 100%;
        padding: 0;
    }

    &.left {
        padding-left: 10rem;
    }

    &.right {
        padding-right: 10rem;
    }
}

.hero-actions {
    margin-top: 1rem;
    display: flex;
}

.hero-header {
    :slotted(h1) {
        font-size: 5rem;
    }
}

.hero-text {

    :slotted(h3) {
        font-size: 3rem;
        font-weight: 400;
    }

    :slotted(h2) {
        font-size: 3rem;
    }

    :slotted(h4) {
        font-size: 2.4rem;
        font-weight: 600;
    }

    :slotted(p) {
        font-size: 3rem;
    }

    :slotted(a) {
        text-decoration: none;

        &:hover {
            color: @memo-blue;
        }
    }

    :slotted(ul) {
        font-size: 2rem;
        list-style: none;
        padding-left: 0;

        li {
            padding-bottom: 1rem;
            position: relative;
            padding-left: 3rem;

            &:before {
                content: "✦";
                color: @memo-green;
                position: absolute;
                left: 0;
                top: 0;
            }
        }
    }
}
</style>