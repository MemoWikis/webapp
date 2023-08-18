<script lang="ts" setup>

export interface KnowledgebarData {
    needsLearning: number
    needsLearningPercentage: number

    needsConsolidation: number
    needsConsolidationPercentage: number

    solid: number
    solidPercentage: number

    notLearned: number
    notLearnedPercentage: number
}

interface Props {
    knowledgebarData: KnowledgebarData
}
const props = defineProps<Props>()
</script>

<template>
    <div v-if="knowledgebarData.needsLearningPercentage > 0" class="needs-learning"
        v-tooltip="`Solltest du lernen: ${knowledgebarData.needsLearning} Fragen (${knowledgebarData.needsLearningPercentage})`"
        :style="{ 'width': knowledgebarData.needsLearningPercentage + '%' }">
    </div>

    <div v-if="knowledgebarData.needsConsolidationPercentage > 0" class="needs-consolidation"
        v-tooltip="`Solltest du lernen: ${knowledgebarData.needsConsolidation} Fragen (${knowledgebarData.needsConsolidationPercentage})`"
        :style="{ 'width': knowledgebarData.needsConsolidationPercentage + '%' }">
    </div>

    <div v-if="knowledgebarData.solidPercentage > 0" class="solid-knowledge"
        v-tooltip="`Solltest du lernen: ${knowledgebarData.solid} Fragen (${knowledgebarData.solidPercentage})`"
        :style="{ 'width': knowledgebarData.solidPercentage + '%' }">
    </div>

    <div v-if="knowledgebarData.notLearnedPercentage > 0" class="not-learned"
        v-tooltip="`Solltest du lernen: ${knowledgebarData.notLearned} Fragen (${knowledgebarData.notLearnedPercentage})`"
        :style="{ 'width': knowledgebarData.notLearnedPercentage + '%' }">
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.knowledge-bar {
    display: inline-flex;
    margin-top: 15px;
    height: 10px;
    min-width: 150px;
    width: 100%;
    max-width: 180px;

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