<script setup lang="ts">
import { PageData } from '~/composables/missionControl/pageData'

interface Props {
    pages: PageData[]
    noPagesText?: string
}

const props = defineProps<Props>()

const { t } = useI18n()


</script>

<template>
    <div class="pages-grid-container">
        <div v-if="pages?.length > 0" class="pages-grid">
            <div v-for="page in pages" :key="page.id" class="page-card-container">
                <MissionControlCard :page="page" />
            </div>
        </div>
        <div v-else class="no-pages">
            <p>{{ noPagesText ? noPagesText : t('missionControl.pageTable.noPages') }}</p>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.pages-grid-container {
    width: 100%;
    margin-top: 1rem;

    .no-pages {
        text-align: center;
        color: @memo-grey-dark;
        font-style: italic;
        padding: 20px 0;
    }
}

.pages-grid {
    display: grid;
    grid-template-columns: repeat(1, 1fr);
    gap: 16px;

    @media (min-width: 576px) {
        grid-template-columns: repeat(2, 1fr);
    }

    @media (min-width: 992px) {
        grid-template-columns: repeat(3, 1fr);
    }

    @media (min-width: 1200px) {
        grid-template-columns: repeat(4, 1fr);
    }

    .page-card-container {
        height: 100%;
    }
}
</style>
