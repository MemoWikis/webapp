<script setup lang="ts">
const slots = useSlots()
const props = defineProps({
    align: {
        type: String,
        default: 'left'
    },
    imagePos: {
        type: String,
        default: 'left'
    }

})
</script>

<template>
    <section>
        <div class="hero-container" :class="{ 'center': align === 'center' }">
            <div class="hero-image" v-if="slots.image && imagePos === 'left'">
                <slot name="image" />
            </div>
            <div class="hero-content" :class="{ 'no-image': !slots.image }">
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
            <div class="hero-image" v-if="slots.image && imagePos === 'right'">
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
    width: 100vw;
    // max-width: 1160px;
    max-width: 860px;
    margin: auto;

    &.center {
        justify-content: center;

        .hero-content {
            align-items: center;
            padding-left: 0;
        }
    }
}

.hero-image {
    width: 50%;
}

.hero-content {
    display: flex;
    flex-direction: column;
    padding: 0 4rem;
    // align-items: center;

    width: 50%;

    &.no-image {
        width: 100%;
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
}
</style>