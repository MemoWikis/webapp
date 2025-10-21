import { ChartData } from '~/components/chart/chartData'

export enum KnowledgeSummaryType {
    // WishKnowledge (wishknowledge) types
    SolidWishKnowledge = 'solidWishKnowledge',
    NeedsConsolidationWishKnowledge = 'needsConsolidationWishKnowledge',
    NeedsLearningWishKnowledge = 'needsLearningWishKnowledge',
    NotLearnedWishKnowledge = 'notLearnedWishKnowledge',
    
    // Not in wuwi types
    SolidNotInWishKnowledge = 'solidNotInWishKnowledge',
    NeedsConsolidationNotInWishKnowledge = 'needsConsolidationNotInWishKnowledge',
    NeedsLearningNotInWishKnowledge = 'needsLearningNotInWishKnowledge',
    NotLearnedNotInWishKnowledge = 'notLearnedNotInWishKnowledge'
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
    total: number
}

export interface KnowledgeSummary {
    totalCount: number

    // Legacy properties for backward compatibility
    needsLearning: number
    needsLearningPercentage: number
    needsConsolidation: number
    needsConsolidationPercentage: number
    solid: number
    solidPercentage: number
    notLearned: number
    notLearnedPercentage: number

    // New nested structure
    inWishKnowledge: KnowledgeStatusCounts
    notInWishKnowledge: KnowledgeStatusCounts
    totalDetailed: KnowledgeStatusCounts

    knowledgeStatusPoints: number
    knowledgeStatusPointsTotal: number

    notInWishKnowledgePercentage: number
}

export interface KnowledgeSummarySlim {
    solid: number
    needsConsolidation: number
    needsLearning: number
    notLearned: number
    inWishKnowledge?: KnowledgeStatusCounts
    notInWishKnowledge?: KnowledgeStatusCounts
}

type KnowledgeSummaryInput = KnowledgeSummary | KnowledgeSummarySlim

/**
 * Converts knowledge summary to chart data showing both wuwi and not-in-wuwi sections
 * (uses the new nested structure to distinguish between wishknowledge and non-wishknowledge)
 */
export const convertKnowledgeSummaryToChartData = (knowledgeSummary: KnowledgeSummaryInput): ChartData[] => {
    const chartData: ChartData[] = []
    
    // Add wuwi (wishknowledge) categories first
    if ('inWishKnowledge' in knowledgeSummary && knowledgeSummary.inWishKnowledge) {
        const wuwiStatusOrder = ['solid', 'needsConsolidation', 'needsLearning', 'notLearned'] as const
        
        for (const statusClass of wuwiStatusOrder) {
            const value = knowledgeSummary.inWishKnowledge[statusClass]
            if (value && value > 0) {
                chartData.push({
                    value,
                    class: `${statusClass}WishKnowledge`
                })
            }
        }
    }
    
    // Add not-in-wuwi (not in wishknowledge) categories
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
 * Converts knowledge summary to chart data using only wishknowledge counts
 * (excludes questions not in wishknowledge)
 */
export const convertWishKnowledgeSummaryToChartData = (knowledgeSummary: KnowledgeSummary): ChartData[] => {
    const wishknowledgeStatusOrder = ['solid', 'needsConsolidation', 'needsLearning', 'notLearned'] as const
    const chartData: ChartData[] = []
    
    for (const statusClass of wishknowledgeStatusOrder) {
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
 * Converts knowledge summary to chart data using only non-wishknowledge counts
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
    
    // Add wuwi (wishknowledge) categories with their percentage of total
    if (knowledgeSummary.inWishKnowledge) {
        const wuwiStatusOrder = ['solid', 'needsConsolidation', 'needsLearning', 'notLearned'] as const
        
        for (const statusClass of wuwiStatusOrder) {
            const value = knowledgeSummary.inWishKnowledge[`${statusClass}PercentageOfTotal`] || 0
            if (value > 0) {
                chartData.push({
                    value,
                    class: `${statusClass}WishKnowledge`
                })
            }
        }
    }
    
    // Add not-in-wuwi categories with their percentage of total
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
 * Converts knowledge summary to chart data using total counts (legacy behavior)
 * (combines wuwi and not-in-wuwi into single categories)
 */
export const convertKnowledgeSummaryToTotalChartData = (knowledgeSummary: KnowledgeSummaryInput): ChartData[] => {
    const chartData: ChartData[] = []
    const statusOrder = ['solid', 'needsConsolidation', 'needsLearning', 'notLearned'] as const
    
    // Calculate totals by combining wuwi and not-in-wuwi
    for (const statusClass of statusOrder) {
        let totalValue = 0
        
        if ('inWishKnowledge' in knowledgeSummary && knowledgeSummary.inWishKnowledge) {
            totalValue += knowledgeSummary.inWishKnowledge[statusClass] || 0
        }
        
        if ('notInWishKnowledge' in knowledgeSummary && knowledgeSummary.notInWishKnowledge) {
            totalValue += knowledgeSummary.notInWishKnowledge[statusClass] || 0
        }
        
        // Fallback to legacy properties if nested structure not available
        if (totalValue === 0 && knowledgeSummary[statusClass]) {
            totalValue = knowledgeSummary[statusClass]
        }
        
        if (totalValue > 0) {
            chartData.push({
                value: totalValue,
                class: statusClass
            })
        }
    }
    
    return chartData
}
