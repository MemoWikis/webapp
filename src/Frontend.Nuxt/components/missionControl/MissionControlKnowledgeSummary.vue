<script lang="ts" setup>

interface Props {
    knowledgeStatus: KnowledgeSummary
}
const sumWishKnowledge = computed(() => {
    const inWishKnowledge = props.knowledgeStatus.inWishKnowledge
    return inWishKnowledge.solid + inWishKnowledge.needsConsolidation + inWishKnowledge.needsLearning + inWishKnowledge.notLearned
})

const props = defineProps<Props>()
</script>

<template>
    <div class="knowledge-summary">
        <SharedKnowledgeSummaryPie :knowledge-status-counts="props.knowledgeStatus.inWishKnowledge" :total-count="sumWishKnowledge" />
        <SharedKnowledgeSummary :knowledge-summary="props.knowledgeStatus" :use-total="false" />
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.knowledge-summary {
    display: flex;
    flex-direction: column;
    border-radius: 8px;
    padding: 16px 20px;
    gap: 2rem 4rem;

    @media (max-width:576px) {
        min-width: unset;
    }

    @media (min-width: @screen-sm) {
        flex-direction: row;
        align-items: center;
        flex-wrap: wrap;
        justify-content: center;
    }
}
</style>

<style lang="less">
.sidesheet-open {
    .knowledge-summary {
        @media (max-width:976px) {
            flex-direction: row;
            align-items: center;
            min-width: unset;
            flex-wrap: wrap;
            justify-content: center;     }
    }
}
</style>