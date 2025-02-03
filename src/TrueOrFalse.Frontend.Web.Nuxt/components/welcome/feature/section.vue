<script setup lang="ts">
import { FeatureImgPosEnum } from './featureImgPosEnum'
const props = defineProps<{
    imagePosition?: FeatureImgPosEnum
}>()

const slots = useSlots()
const { isMobile } = useDevice()

const hasImage = computed(() => props.imagePosition !== FeatureImgPosEnum.None || !!slots.image)
</script>

<template>
    <section>
        <div class="feature" :class="[imagePosition, { mobile: isMobile }]">
            <template v-if="hasImage">
                <div v-if="imagePosition === FeatureImgPosEnum.Left || imagePosition === FeatureImgPosEnum.Top" class="feature-image">
                    <slot name="image" />
                </div>
                <div class="feature-content">
                    <h4 v-if="slots.eyebrow">
                        <slot name="eyebrow" />
                    </h4>
                    <h2 v-if="slots.header">
                        <slot name="header" />
                    </h2>
                    <p v-if="slots.content">
                        <slot name="content" />
                    </p>
                </div>
                <div v-if="imagePosition === FeatureImgPosEnum.Right || imagePosition === FeatureImgPosEnum.Bottom" class="feature-image">
                    <slot name="image" />
                </div>
            </template>
            <template v-else>
                <div class="feature-content">
                    <h4 v-if="slots.eyebrow">
                        <slot name="eyebrow" />
                    </h4>
                    <h2 v-if="slots.header">
                        <slot name="header" />
                    </h2>
                    <template v-if="slots.content">
                        <slot name="content" />
                    </template>
                </div>
            </template>
        </div>
    </section>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

section {
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 3rem 0 6rem;
}

.feature {
    margin: auto;
    width: 100vw;
    max-width: 860px;

    &.mobile {
        flex-direction: column;
        padding: 0 1rem;
    }

    &.left,
    &.right {
        display: flex;
        flex-direction: row;
        justify-content: space-between;
        align-items: center;

        .feature-image {
            width: 50%;
            border-radius: 24px;
            text-align: center;
            overflow: hidden;

            :slotted(img) {
                width: 100%;
                object-fit: cover;
            }
        }

        .feature-content {
            width: 50%;
            padding: 0 1rem;

            :slotted(h1) {
                margin-top: 0;
            }

            :slotted(h2) {
                margin-top: 0;
            }

            :slotted(h3) {
                margin-top: 0;
            }

            :slotted(h4) {
                margin-top: 0;
            }

            :slotted(ul) {
                font-size: 1.6rem;
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

        &.left {
            .feature-content {
                padding-left: 10rem;
            }
        }

        &.right {
            .feature-content {
                padding-right: 10rem;
            }
        }

        &.mobile {
            flex-direction: column;

            .feature-image,
            .feature-content {
                width: 100%;
                padding: 0;
            }

            .feature-image {
                margin-bottom: 1rem;
            }
        }
    }

    &.top,
    &.bottom {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;

        .feature-image {
            width: 50%;
            border-radius: 24px;
            text-align: center;
            overflow: hidden;

            :slotted(img) {
                width: 100%;
                object-fit: cover;
            }

            margin-bottom: 1rem;
        }

        .feature-content {
            width: 100%;
            padding: 0 1rem;
        }

        &.mobile {
            .feature-image {
                width: 100%;
            }
        }

        &.top {
            .feature-content {
                padding-top: 4rem;
            }
        }

        &.bottom {
            .feature-content {
                padding-bottom: 4rem;
            }
        }
    }

    &.none {
        display: block;

        .feature-content {
            width: 100%;
        }
    }
}
</style>
