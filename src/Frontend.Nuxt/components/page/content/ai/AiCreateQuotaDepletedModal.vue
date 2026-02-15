<script lang="ts" setup>
import { useUserStore } from '~/components/user/userStore'

interface Props {
    show: boolean
}

defineProps<Props>()
const emit = defineEmits<{ close: [] }>()

const userStore = useUserStore()
const { t, locale } = useI18n()
const localePath = useLocalePath()

function formatResetDate(date: Date): string {
    return date.toLocaleDateString(locale.value, {
        weekday: 'long',
        day: 'numeric',
        month: 'long',
        year: 'numeric'
    })
}
</script>

<template>
    <Teleport to="body">
        <Transition name="modal-fade">
            <div v-if="show" class="quota-depleted-overlay" role="dialog" aria-modal="true"
                :aria-label="t('page.ai.createPage.quotaDepleted.title')" @click.self="emit('close')">
                <div class="quota-depleted-modal">
                    <div class="modal-icon">
                        <font-awesome-icon :icon="['fas', 'hourglass-half']" />
                    </div>
                    <h3 class="modal-title">{{ t('page.ai.createPage.quotaDepleted.title') }}</h3>
                    <p class="modal-message">{{ t('page.ai.createPage.quotaDepleted.message') }}</p>

                    <div v-if="userStore.quotaInfo?.nextResetDate" class="reset-info">
                        <font-awesome-icon :icon="['fas', 'calendar-check']" />
                        <span>{{ t('page.ai.createPage.quotaDepleted.resetInfo', {
                            date: formatResetDate(userStore.quotaInfo.nextResetDate)
                        }) }}</span>
                    </div>

                    <div class="modal-actions">
                        <NuxtLink :to="localePath('/Einstellungen?tab=ai-usage')" class="btn btn-primary settings-btn">
                            <font-awesome-icon :icon="['fas', 'chart-pie']" />
                            {{ t('page.ai.createPage.quotaDepleted.viewUsage') }}
                        </NuxtLink>
                        <button class="btn btn-secondary" @click="emit('close')">
                            {{ t('page.ai.createPage.quotaDepleted.close') }}
                        </button>
                    </div>
                </div>
            </div>
        </Transition>
    </Teleport>
</template>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.quota-depleted-overlay {
    position: fixed;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background: rgba(0, 0, 0, 0.5);
    display: flex;
    align-items: center;
    justify-content: center;
    z-index: 10000;
    padding: 20px;
}

.quota-depleted-modal {
    background: white;
    border-radius: 16px;
    padding: 32px;
    max-width: 420px;
    width: 100%;
    text-align: center;
    box-shadow: 0 20px 60px rgba(0, 0, 0, 0.2);

    .modal-icon {
        width: 64px;
        height: 64px;
        background: linear-gradient(135deg, @memo-yellow 0%, darken(@memo-yellow, 15%) 100%);
        border-radius: 50%;
        display: flex;
        align-items: center;
        justify-content: center;
        margin: 0 auto 20px;

        svg {
            font-size: 28px;
            color: white;
        }
    }

    .modal-title {
        font-size: 20px;
        font-weight: 700;
        color: @memo-grey-darker;
        margin: 0 0 12px;
    }

    .modal-message {
        font-size: 14px;
        color: @memo-grey-dark;
        line-height: 1.6;
        margin: 0 0 20px;
    }

    .reset-info {
        display: flex;
        align-items: center;
        justify-content: center;
        gap: 8px;
        padding: 12px 16px;
        background: fade(@memo-green, 10%);
        border-radius: 8px;
        color: darken(@memo-green, 20%);
        font-size: 13px;
        font-weight: 500;
        margin-bottom: 24px;

        svg {
            color: @memo-green;
        }
    }

    .modal-actions {
        display: flex;
        flex-direction: column;
        gap: 10px;

        .settings-btn {
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            text-decoration: none;
        }

        .btn {
            padding: 12px 20px;
            border-radius: 24px;
            font-weight: 500;
        }

        .btn-secondary {
            background: @memo-grey-lighter;
            color: @memo-grey-dark;
            border: none;

            &:hover {
                background: darken(@memo-grey-lighter, 5%);
            }
        }
    }
}

.modal-fade-enter-active,
.modal-fade-leave-active {
    transition: opacity 0.2s ease;

    .quota-depleted-modal {
        transition: transform 0.2s ease;
    }
}

.modal-fade-enter-from,
.modal-fade-leave-to {
    opacity: 0;

    .quota-depleted-modal {
        transform: scale(0.95);
    }
}
</style>
