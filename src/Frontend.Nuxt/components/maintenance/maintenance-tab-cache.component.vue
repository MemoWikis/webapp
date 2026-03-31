<script lang="ts" setup>
import type {
    MethodData,
    MmapCacheStatusData,
    MmapCacheFileStatus,
} from './maintenance.types'

const props = defineProps<{
    cacheMethods: MethodData[]
    mmapCacheStatus: MmapCacheStatusData | null
    mmapCacheStatusLoaded: boolean
    loadDurationMs: number | null
}>()

const emit = defineEmits<{
    (event: 'executeMaintenanceOperation', url: string): void
    (event: 'loadMmapCacheStatus'): void
}>()

const { t: $t } = useI18n()

const loadedAtFormatted = ref('')

watch(() => props.loadDurationMs, (newVal) => {
    if (newVal != null) {
        loadedAtFormatted.value = new Date().toLocaleTimeString()
    }
})

const formatSize = (status: MmapCacheFileStatus): string => {
    if (status.sizeMb) {
        return `${status.sizeMb} MB`
    }
    return `${status.sizeKb} KB`
}

const formatAge = (savedAtUtc: string | null): string => {
    if (!savedAtUtc) {
        return '—'
    }
    const saved = new Date(savedAtUtc)
    const now = new Date()
    const diffMs = now.getTime() - saved.getTime()
    const diffHours = Math.floor(diffMs / (1000 * 60 * 60))
    const diffMinutes = Math.floor((diffMs % (1000 * 60 * 60)) / (1000 * 60))

    if (diffHours >= 24) {
        const days = Math.floor(diffHours / 24)
        const remainingHours = diffHours % 24
        return `${days}d ${remainingHours}h ago`
    }
    if (diffHours > 0) {
        return `${diffHours}h ${diffMinutes}m ago`
    }
    return `${diffMinutes}m ago`
}

const cacheCards = computed(() => {
    if (!props.mmapCacheStatus) {
        return []
    }
    return [
        {
            key: 'pageViews',
            label: $t('maintenance.cacheTab.pageViews'),
            status: props.mmapCacheStatus.pageViewsCache,
        },
        {
            key: 'questionViews',
            label: $t('maintenance.cacheTab.questionViews'),
            status: props.mmapCacheStatus.questionViewsCache,
        },
        {
            key: 'pageChanges',
            label: $t('maintenance.cacheTab.pageChanges'),
            status: props.mmapCacheStatus.pageChangesCache,
        },
    ]
})
</script>

<template>
    <div class="tab-content">
        <MaintenanceSection :title="$t('maintenance.cacheTab.operations')" :methods="props.cacheMethods"
            :icon="['fas', 'retweet']" @method-clicked="emit('executeMaintenanceOperation', $event)" />

        <LayoutPanel :title="$t('maintenance.cacheTab.title')">
            <div class="cache-controls">
                <button class="memo-button btn btn-primary" @click="emit('loadMmapCacheStatus')">
                    <font-awesome-icon :icon="['fas', 'sync-alt']" />
                    {{ $t('maintenance.cacheTab.loadStatus') }}
                </button>

                <span v-if="props.mmapCacheStatusLoaded && props.loadDurationMs != null" class="load-feedback">
                    <font-awesome-icon :icon="['fas', 'check-circle']" />
                    {{ $t('maintenance.cacheTab.loadedIn', { ms: props.loadDurationMs }) }}
                    ({{ loadedAtFormatted }})
                </span>
            </div>

            <div v-if="props.mmapCacheStatus" class="cache-grid">
                <div v-for="card in cacheCards" :key="card.key" class="cache-card"
                    :class="{ 'cache-card--invalid': !card.status.isValid && card.status.exists }">
                    <div class="cache-card__header">
                        <span class="cache-card__name">{{ card.label }}</span>
                        <span v-if="card.status.exists" class="cache-card__badge"
                            :class="card.status.isValid ? 'badge--valid' : 'badge--invalid'">
                            {{ card.status.isValid ? $t('maintenance.cacheTab.valid') :
                                $t('maintenance.cacheTab.invalid') }}
                        </span>
                        <span v-else class="cache-card__badge badge--missing">
                            {{ $t('maintenance.cacheTab.missing') }}
                        </span>
                    </div>

                    <template v-if="card.status.exists">
                        <div class="cache-card__row">
                            <span class="cache-card__label">{{ $t('maintenance.cacheTab.size') }}</span>
                            <span class="cache-card__value">{{ formatSize(card.status) }}</span>
                        </div>
                        <div class="cache-card__row">
                            <span class="cache-card__label">{{ $t('maintenance.cacheTab.entries') }}</span>
                            <span class="cache-card__value">{{ card.status.entryCount?.toLocaleString() ?? '—' }}</span>
                        </div>
                        <div class="cache-card__row">
                            <span class="cache-card__label">{{ $t('maintenance.cacheTab.age') }}</span>
                            <span class="cache-card__value">{{ formatAge(card.status.savedAtUtc) }}</span>
                        </div>
                        <div class="cache-card__row">
                            <span class="cache-card__label">{{ $t('maintenance.cacheTab.savedAt') }}</span>
                            <span class="cache-card__value">{{ card.status.savedAtUtc ? new
                                Date(card.status.savedAtUtc).toLocaleString() : '—' }}</span>
                        </div>
                        <div class="cache-card__row">
                            <span class="cache-card__label">{{ $t('maintenance.cacheTab.schema') }}</span>
                            <span class="cache-card__value">v{{ card.status.schemaVersion ?? '?' }}</span>
                        </div>
                        <div v-if="card.status.validationError" class="cache-card__error">
                            <font-awesome-icon :icon="['fas', 'exclamation-triangle']" />
                            {{ card.status.validationError }}
                        </div>
                    </template>
                    <div v-else class="cache-card__empty">
                        {{ $t('maintenance.cacheTab.noCacheFile') }}
                    </div>
                </div>
            </div>

            <div v-else-if="props.mmapCacheStatusLoaded" class="no-data-message">
                {{ $t('maintenance.cacheTab.noData') }}
            </div>
        </LayoutPanel>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.cache-grid {
    display: grid;
    grid-template-columns: repeat(3, 1fr);
    gap: 16px;
    width: 100%;
}

@media (max-width: 900px) {
    .cache-grid {
        grid-template-columns: 1fr;
    }
}

.cache-card {
    border: 1px solid @memo-grey-light;
    border-radius: 8px;
    padding: 16px;
    background: white;

    &--invalid {
        border-color: @memo-red-wrong;
        background: #fdf2f3;
    }

    &__header {
        display: flex;
        align-items: center;
        justify-content: space-between;
        margin-bottom: 12px;
        padding-bottom: 8px;
        border-bottom: 1px solid @memo-grey-lighter;
    }

    &__name {
        font-weight: 600;
        font-size: 15px;
        color: @memo-blue;
    }

    &__badge {
        font-size: 11px;
        font-weight: 600;
        padding: 2px 8px;
        border-radius: 10px;
        text-transform: uppercase;
        letter-spacing: 0.5px;
    }

    .badge--valid {
        background: #e6f4ea;
        color: #1e7e34;
    }

    .badge--invalid {
        background: #fce8e8;
        color: @memo-red-wrong;
    }

    .badge--missing {
        background: @memo-grey-lighter;
        color: @memo-grey-dark;
    }

    &__row {
        display: flex;
        padding: 4px 0;
        font-size: 13px;
    }

    &__label {
        color: @memo-grey-dark;
        width: 120px;
        flex-shrink: 0;
    }

    &__value {
        font-weight: 500;
        font-family: 'Courier New', monospace;
        text-align: right;
        flex: 1;
    }

    &__error {
        margin-top: 8px;
        padding: 8px;
        background: #fce8e8;
        border-radius: 4px;
        font-size: 12px;
        color: @memo-red-wrong;

        svg {
            margin-right: 4px;
        }
    }

    &__empty {
        color: @memo-grey;
        font-style: italic;
        padding: 8px 0;
    }
}

.cache-controls {
    display: flex;
    align-items: center;
    gap: 12px;
    width: 100%;
}

.load-feedback {
    font-size: 13px;
    color: #1e7e34;
    display: inline-flex;
    align-items: center;
    gap: 6px;
}

.no-data-message {
    padding: 16px;
    color: @memo-grey-dark;
    font-style: italic;
}
</style>
