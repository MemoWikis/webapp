<script lang="ts" setup>
import { KnowledgebarData } from './knowledgebarData'

interface Props {
    knowledgebarData: KnowledgebarData
}
const props = defineProps<Props>()
</script>

<template>
    <div class="knowledge-bar">
        <div v-if="knowledgebarData.solidPercentage > 0" class="solid-knowledge"
            v-tooltip="`Sicheres Wissen: ${knowledgebarData.solid} Fragen (${knowledgebarData.solidPercentage}%)`"
            :style="{ 'width': knowledgebarData.solidPercentage + '%' }">
        </div>
        <div v-if="knowledgebarData.needsConsolidationPercentage > 0" class="needs-consolidation"
            v-tooltip="`Solltest du festigen: ${knowledgebarData.needsConsolidation} Fragen (${knowledgebarData.needsConsolidationPercentage}%)`"
            :style="{ 'width': knowledgebarData.needsConsolidationPercentage + '%' }">
        </div>
        <div v-if="knowledgebarData.needsLearningPercentage > 0" class="needs-learning"
            v-tooltip="`Solltest du lernen: ${knowledgebarData.needsLearning} Fragen (${knowledgebarData.needsLearningPercentage}%)`"
            :style="{ 'width': knowledgebarData.needsLearningPercentage + '%' }">
        </div>
        <div v-if="knowledgebarData.notLearnedPercentage > 0" class="not-learned"
            v-tooltip="`Noch nicht gelernt: ${knowledgebarData.notLearned} Fragen (${knowledgebarData.notLearnedPercentage}%)`"
            :style="{ 'width': knowledgebarData.notLearnedPercentage + '%' }">
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.knowledge-bar {
    display: inline-flex;
    height: 6px;
    min-width: 180px;
    width: 100%;
    max-width: 180px;
    border-radius: 6px;
    overflow: hidden;

    .solid-knowledge,
    .needs-learning,
    .needs-consolidation,
    .not-learned,
    .not-in-wish-knowledge {
        height: inherit;
        float: left;
    }

    .needs-learning {
        background-color: @needs-learning-color;
    }

    .needs-consolidation {
        background-color: @needs-consolidation-color;
    }

    .solid-knowledge {
        background-color: @solid-knowledge-color;
    }

    .not-learned {
        background-color: @not-learned-color;
    }

    .not-in-wish-knowledge {
        background-color: @not-in-wish-knowledge-color;
    }
}
</style>