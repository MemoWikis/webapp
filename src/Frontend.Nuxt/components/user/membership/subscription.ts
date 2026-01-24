export interface Plan {
    name: string
    price: number | string
    priceLabel: string
    description: string[]
    listLabel?: string
    list: string[]
    recommended?: boolean
    tier?: 'basic' | 'smart' | 'expert' | 'organisation'
}

export enum Type {
    Basic,
    Plus, // Legacy - maps to Smart
    Smart,
    Expert,
    Team, // Legacy
    Organisation,
}

export interface PlanLimits {
    maxPrivatePageCount: number
    maxPrivateQuestionCount: number
    maxWishKnowledgeCount: number
    freeWeeklyTokens: number
    smartWeeklyTokens: number
    expertWeeklyTokens: number
    tokensPerA4Page: number
}

// Legacy interface for backward compatibility
export interface BasicLimits {
    maxPrivatePageCount: number
    maxPrivateQuestionCount: number
    maxWishKnowledgeCount: number
}

const formatTokens = (tokens: number): string => {
    if (tokens >= 1_000_000) {
        return `${(tokens / 1_000_000).toFixed(tokens % 1_000_000 === 0 ? 0 : 1)} Mio.`
    }
    return tokens.toLocaleString()
}

export const plans = (limits: PlanLimits) => {
    const nuxtApp = useNuxtApp()
    const { $i18n } = nuxtApp

    const freeA4Pages = Math.floor(
        limits.freeWeeklyTokens / limits.tokensPerA4Page,
    )
    const smartA4Pages = Math.floor(
        limits.smartWeeklyTokens / limits.tokensPerA4Page,
    )
    const expertA4Pages = Math.floor(
        limits.expertWeeklyTokens / limits.tokensPerA4Page,
    )

    return {
        basic: {
            name: $i18n.t('user.membership.plans.basic.name'),
            price: 0,
            priceLabel: $i18n.t('user.membership.plans.basic.priceLabel'),
            description: [$i18n.t('user.membership.plans.basic.description')],
            tier: 'basic',
            list: [
                $i18n.t('user.membership.plans.basic.list.publicContent'),
                $i18n.t('user.membership.plans.basic.list.privatePages', {
                    count: limits.maxPrivatePageCount,
                }),
                $i18n.t('user.membership.plans.basic.list.privateQuestions', {
                    count: limits.maxPrivateQuestionCount,
                }),
                $i18n.t('user.membership.plans.basic.list.wishKnowledge', {
                    count: limits.maxWishKnowledgeCount,
                }),
                $i18n.t('user.membership.plans.basic.list.aiTokens', {
                    tokens: formatTokens(limits.freeWeeklyTokens),
                    pages: freeA4Pages,
                }),
            ],
        } as Plan,
        smart: {
            name: $i18n.t('user.membership.plans.smart.name'),
            price: 3,
            priceLabel: $i18n.t('user.membership.plans.smart.priceLabel'),
            description: [
                $i18n.t('user.membership.plans.smart.description.tokens', {
                    count: formatTokens(limits.smartWeeklyTokens),
                }),
            ],
            tier: 'smart',
            list: [
                $i18n.t('user.membership.plans.smart.list.unlimitedPages'),
                $i18n.t('user.membership.plans.smart.list.unlimitedQuestions'),
                $i18n.t('user.membership.plans.smart.list.support'),
            ],
        } as Plan,
        expert: {
            name: $i18n.t('user.membership.plans.expert.name'),
            price: 7,
            priceLabel: $i18n.t('user.membership.plans.expert.priceLabel'),
            description: [
                $i18n.t('user.membership.plans.expert.description.tokens', {
                    count: formatTokens(limits.expertWeeklyTokens),
                }),
            ],
            tier: 'expert',
            listLabel: $i18n.t('user.membership.plans.expert.listLabel'),
            list: [
                $i18n.t('user.membership.plans.expert.list.topModels'),
                $i18n.t('user.membership.plans.expert.list.priority'),
            ],
        } as Plan,
        organisation: {
            name: $i18n.t('user.membership.plans.organisation.name'),
            price: $i18n.t('user.membership.plans.organisation.price'),
            priceLabel: $i18n.t(
                'user.membership.plans.organisation.priceLabel',
            ),
            description: [
                $i18n.t(
                    'user.membership.plans.organisation.description.support',
                ),
                $i18n.t('user.membership.plans.organisation.description.team'),
            ],
            tier: 'organisation',
            listLabel: $i18n.t('user.membership.plans.organisation.listLabel'),
            list: [
                $i18n.t('user.membership.plans.organisation.list.sso'),
                $i18n.t('user.membership.plans.organisation.list.hosting'),
                $i18n.t('user.membership.plans.organisation.list.customizing'),
            ],
        } as Plan,
        // Legacy aliases
        plus: {
            name: $i18n.t('user.membership.plans.smart.name'),
            price: 3,
            priceLabel: $i18n.t('user.membership.plans.smart.priceLabel'),
            description: [
                $i18n.t('user.membership.plans.smart.description.tokens', {
                    count: formatTokens(limits.smartWeeklyTokens),
                }),
            ],
            tier: 'smart',
            list: [
                $i18n.t('user.membership.plans.smart.list.unlimitedPages'),
                $i18n.t('user.membership.plans.smart.list.unlimitedQuestions'),
                $i18n.t('user.membership.plans.smart.list.support'),
            ],
        } as Plan,
        team: {
            name: $i18n.t('user.membership.plans.expert.name'),
            price: 7,
            priceLabel: $i18n.t('user.membership.plans.expert.priceLabel'),
            description: [
                $i18n.t('user.membership.plans.expert.description.tokens', {
                    count: formatTokens(limits.expertWeeklyTokens),
                }),
            ],
            tier: 'expert',
            listLabel: $i18n.t('user.membership.plans.expert.listLabel'),
            list: [
                $i18n.t('user.membership.plans.expert.list.topModels'),
                $i18n.t('user.membership.plans.expert.list.priority'),
            ],
        } as Plan,
    }
}
