<template>
    <LazyModal :show="showModal" :primary-btn-label="primaryBtnLabel" @primary-btn="handlePrimaryAction" @close="showModal = false">
        <template v-slot:header>
            <h4 class="modal-title">
                <!-- Header content -->
                // ...existing code...
            </h4>
        </template>
        <template v-slot:body>
            <div class="create-modal">
                <!-- Body content -->
                // ...existing code...
            </div>
        </template>
        <template v-slot:footer>
            <!-- Footer content if needed -->
            // ...existing code...
        </template>
    </LazyModal>
</template>

<script lang="ts" setup>
const showModal = ref(false)
const primaryBtnLabel = ref('Create Wiki')

function handlePrimaryAction() {
    // Action when primary button is clicked
    // ...existing code...
}
</script>

<style scoped lang="less">
@import '~~/assets/shared/search.less';
@import (reference) '~~/assets/includes/imports.less';

.create-modal {
    // Styles similar to .delete-modal in modal.vue
    // ...existing styles...
}

/* ...existing styles... */
</style>
