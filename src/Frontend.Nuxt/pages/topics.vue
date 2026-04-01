<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'

const { t } = useI18n()

const emit = defineEmits(['setPage'])

onBeforeMount(() => {
    emit('setPage', SiteType.Default)
})

useHead(() => ({
    title: t('topicsPage.meta.title'),
    meta: [
        {
            name: 'description',
            content: t('topicsPage.meta.description')
        }
    ]
}))
</script>

<template>
    <div class="topics-page">
        <div class="topics-container">
            <div class="topics-header">
                <h1>{{ t('topicsPage.title') }}</h1>
                <p class="topics-subtitle">{{ t('topicsPage.subtitle') }}</p>
            </div>

            <div class="topics-empty">
                <font-awesome-icon :icon="['fas', 'layer-group']" class="empty-icon" />
                <p>{{ t('topicsPage.comingSoon') }}</p>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.topics-page {
    display: flex;
    justify-content: center;
    padding: 40px 20px;
}

.topics-container {
    max-width: 1200px;
    width: 100%;
}

.topics-header {
    margin-bottom: 32px;

    h1 {
        font-size: 28px;
        font-weight: 600;
        color: @memo-grey-darkest;
        margin-bottom: 4px;
    }

    .topics-subtitle {
        font-size: 15px;
        color: @memo-grey-dark;
    }
}

.topics-empty {
    text-align: center;
    padding: 80px 20px;
    color: @memo-grey-dark;

    .empty-icon {
        font-size: 48px;
        margin-bottom: 16px;
        opacity: 0.3;
    }

    p {
        font-size: 15px;
    }
}
</style>
