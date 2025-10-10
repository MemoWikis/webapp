import { ChartData } from '~/components/chart/chartData'

export enum KnowledgeSummaryType {
    Solid = 'solid',
    NeedsConsolidation = 'needsConsolidation',
    NeedsLearning = 'needsLearning',
    NotLearned = 'notLearned',
    // NotInWishknowledge = 'notInWishknowledge'
}

export interface KnowledgeSummary {
    total: number

    needsLearning: number
    needsLearningPercentage: number

    needsConsolidation: number
    needsConsolidationPercentage: number

    solid: number
    solidPercentage: number

    notLearned: number
    notLearnedPercentage: number

    knowledgeStatusPoints: number
    knowledgeStatusPointsTotal: number
}

export interface KnowledgeSummarySlim {
    solid: number
    needsConsolidation: number
    needsLearning: number
    notLearned: number
}

type KnowledgeSummaryInput = KnowledgeSummary | KnowledgeSummarySlim

export const convertKnowledgeSummaryToChartData = (knowledgeSummary: KnowledgeSummaryInput): ChartData[] => {
    const knowledgeStatusOrder = ['solid', 'needsConsolidation', 'needsLearning', 'notLearned'] as const
    const chartData: ChartData[] = []
    
    for (const statusClass of knowledgeStatusOrder) {
        const value = knowledgeSummary[statusClass]
        if (value > 0) {
            chartData.push({
                value,
                class: statusClass
            })
        }
    }
    
    return chartData
}
