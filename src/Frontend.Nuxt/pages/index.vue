<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'

const { t } = useI18n()

const emit = defineEmits(['setBreadcrumb', 'setPage'])

onBeforeMount(() => {
    emit('setPage', SiteType.Default)
})

useHead(() => ({
    title: t('aboutPage.meta.title'),
    meta: [
        {
            name: 'description',
            content: t('aboutPage.meta.description')
        }
    ]
}))
</script>

<template>
    <div class="about-page">
        <div class="about-container">
            <div class="about-hero">
                <Image src="/Images/Logo/Logo.svg" class="about-logo" alt="memoWikis logo" />
                <h1>{{ t('aboutPage.title') }}</h1>
                <p class="about-subtitle">{{ t('aboutPage.subtitle') }}</p>
            </div>

            <div class="about-content">
                <p class="about-description">{{ t('aboutPage.description') }}</p>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.about-page {
    display: flex;
    justify-content: center;
    padding: 40px 20px;
}

.about-container {
    max-width: 800px;
    width: 100%;
}

.about-hero {
    text-align: center;
    margin-bottom: 40px;

    .about-logo {
        width: 200px;
        margin-bottom: 24px;
    }

    h1 {
        font-size: 32px;
        font-weight: 600;
        color: @memo-grey-darkest;
        margin-bottom: 8px;
    }

    .about-subtitle {
        font-size: 18px;
        color: @memo-grey-dark;
    }
}

.about-content {
    .about-description {
        font-size: 16px;
        line-height: 1.6;
        color: @memo-grey-dark;
        text-align: center;
    }
}
</style>