<script setup lang="ts">
import { KnowledgebarData } from '~/components/page/content/grid/knowledgebar/knowledgebarData'

interface WikiData {
    id: number
    name: string
    imgUrl?: string
    questionCount: number
    knowledgebarData: KnowledgebarData
}

interface Props {
    wikis: WikiData[]
}

defineProps<Props>()

const { t } = useI18n()

</script>

<template>
    <div class="wikis-grid">
        <div v-if="wikis.length" class="row">
            <div v-for="wiki in wikis" :key="wiki.id" class="col-12 col-md-6 mb-4">
                <MissionControlWikiCard :wiki="wiki" />
            </div>
        </div>
        <div v-else class="no-wikis">
            <p>{{ t('missionControl.wiki.noWikis') }}</p>
        </div>
    </div>
</template>

<style lang="less" scoped>
.wikis-grid {

    .no-wikis {
        text-align: center;
        color: #666;
        font-style: italic;
    }
}
</style>
