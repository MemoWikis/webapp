<script lang="ts" setup>
import { useAiCreateStore } from './aiCreateStore'
import { useUserStore } from '~/components/user/userStore'

interface Props {
    disabled: boolean
    buttonLabel: string
}

const props = defineProps<Props>()
const emit = defineEmits<{ action: [] }>()

const aiCreateStore = useAiCreateStore()
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

const groupedModels = computed(() => {
    const groups: { name: string; models: typeof aiCreateStore.availableModels }[] = []
    const providers = new Set(aiCreateStore.availableModels.map(model => model.provider))

    for (const provider of providers) {
        groups.push({
            name: provider,
            models: aiCreateStore.availableModels.filter(model => model.provider === provider)
        })
    }

    return groups
})

const selectedModelDisplayName = computed(() => {
    const model = aiCreateStore.availableModels.find(model => model.modelId === aiCreateStore.selectedModelId)
    return model?.displayName ?? ''
})
</script>

<template>
    <div class="ai-create-footer">
        <!-- AI Model Selection -->
        <div class="ai-model-selection">
            <VDropdown :distance="2" class="model-dropdown" placement="top-start">
                <div class="model-select"
                    :class="{ disabled: aiCreateStore.isGenerating || aiCreateStore.isLoadingModels }">
                    <span v-if="aiCreateStore.isLoadingModels">{{ t('page.ai.createPage.loadingModels') }}</span>
                    <span v-else>{{ selectedModelDisplayName || t('page.ai.createPage.selectModel') }}</span>
                    <font-awesome-icon :icon="['fas', 'chevron-down']" />
                </div>

                <template #popper="{ hide }">
                    <div class="model-dropdown-menu model-dropdown-popper">
                        <template v-for="provider in groupedModels" :key="provider.name">
                            <div class="provider-header">{{ provider.name }}</div>
                            <div v-for="model in provider.models" :key="model.modelId"
                                class="dropdown-row ai-model-option"
                                :class="{ active: aiCreateStore.selectedModelId === model.modelId }"
                                @click="aiCreateStore.setSelectedModel(model.modelId); hide()">
                                <span>{{ model.displayName }}</span>
                                <span class="token-cost-multiplier">{{ model.tokenCostMultiplier }}x</span>
                            </div>
                        </template>
                    </div>
                </template>
            </VDropdown>

            <VDropdown :distance="2" placement="top" class="token-balance-dropdown">
                <div class="token-balance-btn" :title="t('page.ai.createPage.tokenBalance')"
                    :class="{ 'quota-low': (userStore.quotaInfo?.percentageUsed ?? 0) > 80, 'quota-depleted': userStore.quotaInfo?.isQuotaDepleted }"
                    @click="userStore.fetchQuotaInfo()">
                    <font-awesome-icon :icon="['fas', 'chart-pie']" />
                </div>

                <template #popper>
                    <div class="token-balance-popper">
                        <div class="token-balance-header">{{ t('page.ai.createPage.quota.title') }}</div>

                        <div v-if="userStore.isLoadingQuotaInfo" class="loading-state">
                            <font-awesome-icon icon="fa-solid fa-spinner" spin />
                        </div>

                        <template v-else-if="userStore.quotaInfo">
                            <div class="quota-progress-container">
                                <div class="quota-progress-bar">
                                    <div class="quota-progress-fill" :class="{
                                        'low': userStore.quotaInfo.percentageUsed > 80,
                                        'depleted': userStore.quotaInfo.isQuotaDepleted
                                    }" :style="{ width: `${userStore.quotaInfo.percentageUsed}%` }" />
                                </div>
                                <div class="quota-values">
                                    <span class="quota-remaining">
                                        {{ userStore.quotaInfo.tokensUsedThisWeek.toLocaleString() }}
                                    </span>
                                    <span class="quota-separator">/</span>
                                    <span class="quota-total">
                                        {{ userStore.quotaInfo.weeklyLimit.toLocaleString() }}
                                    </span>
                                </div>
                            </div>

                            <div v-if="userStore.quotaInfo.hasActiveSubscription && userStore.quotaInfo.nextResetDate"
                                class="quota-reset">
                                <font-awesome-icon :icon="['fas', 'calendar-alt']" class="reset-icon" />
                                <span>{{ t('page.ai.createPage.quota.resetDate') }}: {{
                                    formatResetDate(userStore.quotaInfo.nextResetDate) }}</span>
                            </div>

                            <div v-if="userStore.quotaInfo.isQuotaDepleted" class="quota-depleted-warning">
                                <font-awesome-icon :icon="['fas', 'exclamation-triangle']" />
                                <span>{{ t('page.ai.createPage.quota.depleted') }}</span>
                            </div>

                            <NuxtLink :to="localePath('/Einstellungen?tab=ai-usage')" class="quota-settings-link">
                                <font-awesome-icon :icon="['fas', 'cog']" />
                                {{ t('page.ai.createPage.quota.settingsLink') }}
                            </NuxtLink>
                        </template>

                        <template v-else>
                            <div class="token-balance-value">—</div>
                        </template>
                    </div>
                </template>
            </VDropdown>
        </div>

        <!-- Quota Warning -->
        <NuxtLink v-if="userStore.quotaInfo?.isQuotaDepleted" :to="localePath('/Einstellungen?tab=ai-usage')"
            class="quota-warning-indicator depleted">
            <font-awesome-icon :icon="['fas', 'exclamation-circle']" />
            <span>{{ t('page.ai.createPage.quotaWarning.depleted') }}</span>
        </NuxtLink>
        <NuxtLink v-else-if="userStore.quotaInfo && userStore.quotaInfo.percentageUsed > 80"
            :to="localePath('/Einstellungen?tab=ai-usage')" class="quota-warning-indicator low">
            <font-awesome-icon :icon="['fas', 'exclamation-triangle']" />
            <span>{{ t('page.ai.createPage.quotaWarning.low', {
                percent: (100 - userStore.quotaInfo.percentageUsed).toFixed(0)
            }) }}</span>
        </NuxtLink>

        <div class="buttons">
            <button class="memo-button btn btn-primary" :disabled="props.disabled" @click="emit('action')">
                <font-awesome-icon :icon="['fas', 'wand-magic-sparkles']" class="generate-icon" />
                {{ props.buttonLabel }}
            </button>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.ai-create-footer {
    display: flex;
    justify-content: space-between;
    align-items: center;

    .ai-model-selection {
        display: flex;
        align-items: center;
        flex-direction: row;
        color: @memo-grey-light;

        * {
            color: @memo-grey-dark;
            cursor: pointer;
            user-select: none;

            &:hover {
                filter: brightness(0.9);
            }
        }

        .model-select {
            display: flex;
            gap: 8px;
            align-items: center;
        }

        .token-balance-btn {
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 0 8px;
            cursor: pointer;
            color: @memo-grey-dark;
            transition: all 0.2s ease;
            height: 46px;
            width: 46px;

            &:hover {
                color: @memo-blue;
            }

            &.quota-low {
                color: @memo-yellow;
            }

            &.quota-depleted {
                color: #B13A48;
            }
        }
    }

    .quota-warning-indicator {
        display: flex;
        align-items: center;
        gap: 8px;
        padding: 8px 14px;
        border-radius: 6px;
        font-size: 13px;
        font-weight: 500;
        text-decoration: none;
        transition: all 0.2s ease;

        &.low {
            background: fade(@memo-yellow, 15%);
            color: darken(@memo-yellow, 25%);
            border: 1px solid fade(@memo-yellow, 40%);

            &:hover {
                background: fade(@memo-yellow, 25%);
            }

            svg {
                color: @memo-yellow;
            }
        }

        &.depleted {
            background: fade(#B13A48, 10%);
            color: #B13A48;
            border: 1px solid fade(#B13A48, 30%);

            &:hover {
                background: fade(#B13A48, 18%);
            }

            svg {
                color: #B13A48;
            }
        }
    }

    .buttons {
        display: flex;
        gap: 12px;

        .memo-button {
            display: flex;
            align-items: center;
            gap: 8px;
            font-weight: 600;
            padding: 10px 24px;
            border-radius: 8px;

            .generate-icon {
                font-size: 14px;
            }
        }
    }
}
</style>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.ai-model-option {
    display: flex;
    justify-content: space-between;
    align-items: center;

    .token-cost-multiplier {
        font-size: 12px;
        color: @memo-grey;
    }
}

.model-dropdown-popper {
    width: 100%;
    min-width: 300px;
    max-height: 400px;
    overflow-y: auto;
    margin-top: -12px;
    margin-bottom: -12px;

    .provider-header {
        padding: 12px;
        font-weight: 600;
        color: @memo-grey-dark;
        background: @memo-grey-lighter;
        font-size: 12px;
        text-transform: uppercase;
        letter-spacing: 0.5px;
    }

    .dropdown-row {
        padding: 10px 16px;
        cursor: pointer;
        transition: background 0.15s ease;

        &:hover {
            background: fade(@memo-blue, 10%);
        }

        &.active {
            background: @memo-grey-lightest;
            color: @memo-grey-darker;
            font-weight: 600;
        }
    }
}

.token-balance-popper {
    padding: 12px 16px;
    min-width: 200px;

    .token-balance-header {
        font-size: 12px;
        font-weight: 600;
        color: @memo-grey-dark;
        margin-bottom: 8px;
    }

    .token-balance-value {
        font-size: 18px;
        font-weight: 600;
        color: @memo-blue;
    }

    .loading-state {
        text-align: center;
        padding: 8px;
        color: @memo-grey-dark;
    }

    .quota-progress-container {
        margin-bottom: 12px;

        .quota-progress-bar {
            height: 8px;
            background: @memo-grey-lighter;
            border-radius: 4px;
            overflow: hidden;
            margin-bottom: 4px;

            .quota-progress-fill {
                height: 100%;
                background: linear-gradient(90deg, @memo-green, @memo-green);
                border-radius: 4px;
                transition: width 0.3s ease;

                &.low {
                    background: @memo-yellow;
                }

                &.depleted {
                    background: #B13A48;
                    width: 0 !important;
                }
            }
        }

        .quota-values {
            display: flex;
            align-items: baseline;
            font-size: 14px;

            .quota-remaining {
                font-weight: 600;
                color: @memo-blue;
                font-size: 16px;
            }

            .quota-separator {
                margin: 0 4px;
                color: @memo-grey-dark;
            }

            .quota-total {
                color: @memo-grey-dark;
            }
        }
    }

    .quota-reset {
        display: flex;
        align-items: center;
        gap: 6px;
        font-size: 12px;
        color: @memo-grey-dark;
        margin-bottom: 8px;

        .reset-icon {
            color: @memo-blue;
        }
    }

    .quota-depleted-warning {
        display: flex;
        align-items: center;
        gap: 6px;
        padding: 8px;
        background: fade(#B13A48, 10%);
        border-radius: 4px;
        color: #B13A48;
        font-size: 12px;
        font-weight: 500;
        margin-bottom: 8px;
    }

    .quota-settings-link {
        display: flex;
        align-items: center;
        gap: 6px;
        font-size: 12px;
        color: @memo-blue-link;
        text-decoration: none;
        padding-top: 8px;
        border-top: 1px solid @memo-grey-lighter;
        margin-top: 4px;

        &:hover {
            text-decoration: underline;
        }
    }
}
</style>
