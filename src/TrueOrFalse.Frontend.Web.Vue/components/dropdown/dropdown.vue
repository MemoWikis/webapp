<script lang="ts" setup>
const props = defineProps(['distance'])
</script>

<template>
    <VDropdown :distance="props.distance">
        <slot name="button"></slot>
        <template #popper>
            <ul class="dropdown-menu">

                <slot name="dropdown"></slot>

            </ul>

        </template>
    </VDropdown>
</template>

<style lang="less">
</style>