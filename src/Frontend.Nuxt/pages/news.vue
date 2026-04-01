<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'

const { t } = useI18n()

const emit = defineEmits(['setPage'])

onBeforeMount(() => {
    emit('setPage', SiteType.Default)
})

useHead(() => ({
    title: t('newsPage.meta.title'),
    meta: [
        {
            name: 'description',
            content: t('newsPage.meta.description')
        }
    ]
}))
</script>

<template>
    <div class="news-page">
        <div class="news-container">
            <div class="news-header">
                <h1>{{ t('newsPage.title') }}</h1>
                <p class="news-subtitle">{{ t('newsPage.subtitle') }}</p>
            </div>

            <div class="news-empty">
                <font-awesome-icon :icon="['fas', 'newspaper']" class="empty-icon" />
                <p>{{ t('newsPage.comingSoon') }}</p>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.news-page {
    display: flex;
    justify-content: center;
    padding: 40px 20px;
}

.news-container {
    max-width: 1200px;
    width: 100%;
}

.news-header {
    margin-bottom: 32px;

    h1 {
        font-size: 28px;
        font-weight: 600;
        color: @memo-grey-darkest;
        margin-bottom: 4px;
    }

    .news-subtitle {
        font-size: 15px;
        color: @memo-grey-dark;
    }
}

.news-empty {
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
