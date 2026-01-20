<script lang="ts" setup>
interface Props {
    show: boolean
    title?: string
    message?: string
    confirmText?: string
    cancelText?: string
    confirmButtonClass?: string
}

const props = withDefaults(defineProps<Props>(), {
    title: 'Confirm',
    message: 'Are you sure?',
    confirmText: 'Confirm',
    cancelText: 'Cancel',
    confirmButtonClass: 'btn-primary'
})

const emit = defineEmits<{
    (event: 'confirm' | 'cancel'): void
    (event: 'update:show', value: boolean): void
}>()

const handleConfirm = () => {
    emit('confirm')
    emit('update:show', false)
}

const handleCancel = () => {
    emit('cancel')
    emit('update:show', false)
}

const handleOverlayClick = (event: MouseEvent) => {
    if (event.target === event.currentTarget) {
        handleCancel()
    }
}
</script>

<template>
    <Teleport to="body">
        <Transition name="modal">
            <div v-if="props.show" class="modal-overlay" @click="handleOverlayClick">
                <div class="confirm-modal" role="dialog" aria-modal="true" :aria-labelledby="title">
                    <h4 v-if="title" class="modal-title">{{ props.title }}</h4>
                    <div class="modal-body">
                        <slot>
                            <p>{{ props.message }}</p>
                        </slot>
                    </div>
                    <div class="modal-actions">
                        <button class="memo-button btn" :class="props.confirmButtonClass" @click="handleConfirm">
                            {{ props.confirmText }}
                        </button>
                        <button class="memo-button btn btn-secondary" @click="handleCancel">
                            {{ props.cancelText }}
                        </button>
                    </div>
                </div>
            </div>
        </Transition>
    </Teleport>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.modal-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 1000;
}

.confirm-modal {
    background: white;
    padding: 24px;
    border-radius: 8px;
    max-width: 400px;
    width: 90%;
    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);

    .modal-title {
        margin-top: 0;
        margin-bottom: 16px;
        font-size: 18px;
        font-weight: 600;
    }

    .modal-body {
        margin-bottom: 20px;
        color: @memo-grey-darker;

        p {
            margin: 0;
        }
    }

    .modal-actions {
        display: flex;
        gap: 12px;
        justify-content: flex-end;
    }
}

// Transition animations
.modal-enter-active,
.modal-leave-active {
    transition: opacity 0.2s ease;

    .confirm-modal {
        transition: transform 0.2s ease;
    }
}

.modal-enter-from,
.modal-leave-to {
    opacity: 0;

    .confirm-modal {
        transform: scale(0.95);
    }
}
</style>
