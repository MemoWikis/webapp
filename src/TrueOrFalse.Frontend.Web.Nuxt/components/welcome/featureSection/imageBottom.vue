<script setup lang="ts">
const slots = useSlots()

const { isMobile } = useDevice()

</script>

<template>
    <section>
        <div class="feature-container" :class="{ 'mb': isMobile }">
            <div class="feature-container">
                <slot />
            </div>
            <div v-if="slots.image" class="feature-image">
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
    padding: 3rem 0 6rem;

    .feature-container {
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;

        width: 100%;
        width: 100vw;
        // max-width: 1160px;
        max-width: 860px;
        margin: auto;

        .feature-image {
            flex: 1;
            text-align: center;
            width: 50%;
            border-radius: 24px;
            // border: solid 1px lightgrey;
            overflow: hidden;

            :slotted(img) {
                width: 100%;
                object-fit: cover;
            }
        }

        .feature-container {
            flex: 1;
            padding: 0 1rem;
            width: 100%;
        }

        &.mb {
            flex-direction: column;

            .feature-image {
                padding-bottom: 1rem;
                width: 100%;
            }

            .feature-container {
                width: 100%;
            }
        }

        :slotted(.feature-content) {
            padding-bottom: 4rem;

            p {
                font-size: 1.6rem;
            }

            h2 {
                font-size: 3rem;
                padding-bottom: 2rem;
            }
        }
    }
}
</style>