import { ChartData } from '~/components/chart/chartData'

export enum KnowledgeSummaryType {
    // WishKnowledge (wishKnowledge) types
    SolidWishKnowledge = 'solidWishKnowledge',
    NeedsConsolidationWishKnowledge = 'needsConsolidationWishKnowledge',
    NeedsLearningWishKnowledge = 'needsLearningWishKnowledge',
    NotLearnedWishKnowledge = 'notLearnedWishKnowledge',
    
    // Not in wishKnowledge types
    SolidNotInWishKnowledge = 'solidNotInWishKnowledge',
    NeedsConsolidationNotInWishKnowledge = 'needsConsolidationNotInWishKnowledge',
    NeedsLearningNotInWishKnowledge = 'needsLearningNotInWishKnowledge',
    NotLearnedNotInWishKnowledge = 'notLearnedNotInWishKnowledge',

    // Note: 'NotInWishKnowledge' type for aggregated slice is not included here
    NotInWishKnowledge = 'notInWishKnowledgeCount'
}

export interface KnowledgeStatusCounts {
    notLearned: number
    needsLearning: number
    needsConsolidation: number
    solid: number
    // Percentages relative to this group total (adds up to 100% within this group)
    notLearnedPercentage: number
    needsLearningPercentage: number
    needsConsolidationPercentage: number
    solidPercentage: number
    // Percentages relative to grand total (InWishKnowledge + NotInWishKnowledge)
    notLearnedPercentageOfTotal: number
    needsLearningPercentageOfTotal: number
    needsConsolidationPercentageOfTotal: number
    solidPercentageOfTotal: number
    // Aggregated not-in-wish-knowledge data (only present in Total object)
    notInWishKnowledgeCount?: number
    notInWishKnowledgePercentage?: number
}

export interface KnowledgeSummary {
    totalCount: number

    // New nested structure
    inWishKnowledge: KnowledgeStatusCounts
    notInWishKnowledge: KnowledgeStatusCounts
    total: KnowledgeStatusCounts

    knowledgeStatusPoints: number
    knowledgeStatusPointsTotal: number
}

/**
 * Converts knowledge summary to chart data showing both wishKnowledge and not-in-wishKnowledge sections
 * (uses the new nested structure to distinguish between wishKnowledge and non-wishKnowledge)
 */
export const convertKnowledgeSummaryToChartData = (knowledgeSummary: KnowledgeSummary): ChartData[] => {
    const chartData: ChartData[] = []
    
    // Add wishKnowledge (wishKnowledge) categories first
    if ('inWishKnowledge' in knowledgeSummary && knowledgeSummary.inWishKnowledge) {
        const wishKnowledgeStatusOrder = ['solid', 'needsConsolidation', 'needsLearning', 'notLearned'] as const
        
        for (const statusClass of wishKnowledgeStatusOrder) {
            const value = knowledgeSummary.inWishKnowledge[statusClass]
            if (value && value > 0) {
                chartData.push({
                    value,
                    class: `${statusClass}WishKnowledge`
                })
            }
        }
    }
    
    // Add not-in-wishKnowledge (not in wishKnowledge) categories
    if ('notInWishKnowledge' in knowledgeSummary && knowledgeSummary.notInWishKnowledge) {
        const notInWishKnowledgeStatusOrder = ['solid', 'needsConsolidation', 'needsLearning', 'notLearned'] as const
        
        for (const statusClass of notInWishKnowledgeStatusOrder) {
            const value = knowledgeSummary.notInWishKnowledge[statusClass]
            if (value && value > 0) {
                chartData.push({
                    value,
                    class: `${statusClass}NotInWishKnowledge`
                })
            }
        }
    }
    
    return chartData
}

/**
 * Converts knowledge summary to chart data using only wishKnowledge counts
 * (excludes questions not in wishKnowledge)
 */
export const convertWishKnowledgeSummaryToChartData = (knowledgeSummary: KnowledgeSummary): ChartData[] => {
    const wishKnowledgeStatusOrder = ['solid', 'needsConsolidation', 'needsLearning', 'notLearned'] as const
    const chartData: ChartData[] = []
    
    for (const statusClass of wishKnowledgeStatusOrder) {
        const value = knowledgeSummary.inWishKnowledge[statusClass]
        if (value && value > 0) {
            chartData.push({
                value,
                class: statusClass
            })
        }
    }
    
    return chartData
}

/**
 * Converts knowledge summary to chart data using only non-wishKnowledge counts
 */
export const convertNotInWishKnowledgeSummaryToChartData = (knowledgeSummary: KnowledgeSummary): ChartData[] => {
    const statusOrder = ['solid', 'needsConsolidation', 'needsLearning', 'notLearned'] as const
    const chartData: ChartData[] = []
    
    for (const statusClass of statusOrder) {
        const value = knowledgeSummary.notInWishKnowledge[statusClass]
        if (value && value > 0) {
            chartData.push({
                value,
                class: statusClass
            })
        }
    }
    
    return chartData
}

/**
 * Converts knowledge summary to chart data using percentages of total
 * (shows actual proportions relative to all questions)
 */
export const convertKnowledgeSummaryToTotalPercentageChartData = (knowledgeSummary: KnowledgeSummary): ChartData[] => {
    const chartData: ChartData[] = []
    
    // Add wishKnowledge (wishKnowledge) categories with their percentage of total
    if (knowledgeSummary.inWishKnowledge) {
        const wishKnowledgeStatusOrder = ['solid', 'needsConsolidation', 'needsLearning', 'notLearned'] as const
        
        for (const statusClass of wishKnowledgeStatusOrder) {
            const value = knowledgeSummary.inWishKnowledge[`${statusClass}PercentageOfTotal`] || 0
            if (value > 0) {
                chartData.push({
                    value,
                    class: `${statusClass}WishKnowledge`
                })
            }
        }
    }
    
    // Add not-in-wishKnowledge categories with their percentage of total
    if (knowledgeSummary.notInWishKnowledge) {
        const notInWishKnowledgeStatusOrder = ['solid', 'needsConsolidation', 'needsLearning', 'notLearned'] as const
        
        for (const statusClass of notInWishKnowledgeStatusOrder) {
            const value = knowledgeSummary.notInWishKnowledge[`${statusClass}PercentageOfTotal`] || 0
            if (value > 0) {
                chartData.push({
                    value,
                    class: `${statusClass}NotInWishKnowledge`
                })
            }
        }
    }
    
    return chartData
}

/**
 * Gets the percentage of total for a specific knowledge status and category
 */
export const getKnowledgeStatusPercentageOfTotal = (
    knowledgeSummary: KnowledgeSummary, 
    status: 'solid' | 'needsConsolidation' | 'needsLearning' | 'notLearned',
    category: 'inWishKnowledge' | 'notInWishKnowledge'
): number => {
    const categoryData = knowledgeSummary[category]
    if (!categoryData) return 0
    
    return categoryData[`${status}PercentageOfTotal`] || 0
}
/**
 * Converts KnowledgeStatusCounts directly to chart data
 * (efficient method that works with any KnowledgeStatusCounts object)
 */
export const convertKnowledgeStatusCountsToChartData = (statusCounts: KnowledgeStatusCounts): ChartData[] => {
    const chartData: ChartData[] = []
    const statusOrder = ['solid', 'needsConsolidation', 'needsLearning', 'notLearned'] as const

    // Add in-wish-knowledge status items
    for (const statusClass of statusOrder) {
        const value = statusCounts[statusClass]
        if (value > 0) {
            chartData.push({
                value,
                class: statusClass
            })
        }
    }

    // Add aggregated not-in-wish-knowledge slice if available
    if (statusCounts.notInWishKnowledgeCount && statusCounts.notInWishKnowledgeCount > 0) {
        chartData.push({
            value: statusCounts.notInWishKnowledgeCount,
            class: 'notInWishKnowledge'
        })
    }

    return chartData
}

/**
 * Converts knowledge summary to chart data using total counts (legacy behavior)
 * (combines wishKnowledge and not-in-wishKnowledge into single categories)
 */
export const convertKnowledgeSummaryToTotalChartData = (knowledgeSummary: KnowledgeSummary): ChartData[] => {
    // Use the efficient method with the backend-calculated total that includes aggregated not-in-wish-knowledge
    return convertKnowledgeStatusCountsToChartData(knowledgeSummary.total)
}
