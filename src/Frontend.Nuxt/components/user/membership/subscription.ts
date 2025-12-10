export interface Plan {
    name: string
    price: number | string
    priceLabel: string
    description: string[]
    listLabel?: string
    list: string[]
}

export enum Type {
    Basic,
    Plus,
    Team,
    Organisation,
}

export interface BasicLimits {
    maxPrivatePageCount: number
    maxPrivateQuestionCount: number
    maxWishKnowledgeCount: number
}

export const plans = (limit: BasicLimits) => {
    const nuxtApp = useNuxtApp()
    const { $i18n } = nuxtApp

    return {
        basic: {
            name: $i18n.t("user.membership.plans.basic.name"),
            price: 0,
            priceLabel: $i18n.t("user.membership.plans.basic.priceLabel"),
            description: [$i18n.t("user.membership.plans.basic.description")],
            list: [
                $i18n.t("user.membership.plans.basic.list.publicContent"),
                $i18n.t("user.membership.plans.basic.list.privatePages", {
                    count: limit.maxPrivatePageCount,
                }),
                $i18n.t("user.membership.plans.basic.list.privateQuestions", {
                    count: limit.maxPrivateQuestionCount,
                }),
                $i18n.t("user.membership.plans.basic.list.wishKnowledge", {
                    count: limit.maxWishKnowledgeCount,
                }),
            ],
        } as Plan,
        plus: {
            name: $i18n.t("user.membership.plans.plus.name"),
            price: 3,
            priceLabel: $i18n.t("user.membership.plans.plus.priceLabel"),
            description: [$i18n.t("user.membership.plans.plus.description")],
            list: [
                $i18n.t("user.membership.plans.plus.list.support"),
                $i18n.t("user.membership.plans.plus.list.unlimitedPages"),
                $i18n.t("user.membership.plans.plus.list.unlimitedQuestions"),
                $i18n.t(
                    "user.membership.plans.plus.list.unlimitedWishKnowledge"
                ),
            ],
        } as Plan,
        team: {
            name: $i18n.t("user.membership.plans.team.name"),
            price: 7,
            priceLabel: $i18n.t("user.membership.plans.team.priceLabel"),
            description: [$i18n.t("user.membership.plans.team.description")],
            listLabel: $i18n.t("user.membership.plans.team.listLabel"),
            list: [
                $i18n.t("user.membership.plans.team.list.teamPrivateContent"),
                $i18n.t("user.membership.plans.team.list.manageTeams"),
            ],
        } as Plan,
        organisation: {
            name: $i18n.t("user.membership.plans.organisation.name"),
            price: $i18n.t("user.membership.plans.organisation.price"),
            priceLabel: $i18n.t(
                "user.membership.plans.organisation.priceLabel"
            ),
            description: [
                $i18n.t(
                    "user.membership.plans.organisation.description.support"
                ),
                $i18n.t("user.membership.plans.organisation.description.team"),
            ],
            listLabel: $i18n.t("user.membership.plans.organisation.listLabel"),
            list: [
                $i18n.t("user.membership.plans.organisation.list.sso"),
                $i18n.t("user.membership.plans.organisation.list.hosting"),
                $i18n.t("user.membership.plans.organisation.list.customizing"),
            ],
        } as Plan,
    }
}
