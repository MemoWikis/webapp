<script lang="ts" setup>
import { SolutionType } from '~/components/question/solutionTypeEnum'

interface Props {
    answerBodyModel: any
}

const props = defineProps<Props>()
</script>

<template>
    <div
        id="MarkdownCol"
        v-if="answerBodyModel.solutionType != SolutionType.Flashcard && !!answerBodyModel.renderedQuestionTextExtended.length">
        <SharedRawHtml
            class="RenderedMarkdown"
            :html="answerBodyModel.renderedQuestionTextExtended"
            id="ExtendedQuestionContainer" />
    </div>
</template>
