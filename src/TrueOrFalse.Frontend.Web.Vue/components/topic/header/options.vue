<script lang="ts" setup>
import { ref } from 'vue';
const msg = ref('hello')
</script>

<template>
    <VDropdown :distance="0">
        <!-- This will be the popover reference (for the events and position) -->
        <button>Click me</button>

        <!-- This will be the content of the popover -->
        <template #popper>
            <input class="tooltip-content" v-model="msg" placeholder="Tooltip content" />
            <p>
                {{ msg }}
            </p>

            <!-- You can put other components too -->
        </template>
    </VDropdown>
</template>