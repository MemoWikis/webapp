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
    Organisation
}

export interface BasicLimits {
    maxPrivatePageCount: number
    maxPrivateQuestionCount: number
    maxWishknowledgeCount: number
}

export const plans = (limit: BasicLimits) => {
    return {
        basic: {
            name: 'Kostenlos',
            price: 0,
            priceLabel: 'Kostenlos für immer!',
            description: ['Du kannst alles machen. Nur die Anzahl der privaten Inhalte ist begrenzt.'],
            list: ['Unbeschränkt öffentliche Inhalte (ansehen, erstellen und lernen)', `${limit.maxPrivatePageCount} private Themen`, `${limit.maxPrivateQuestionCount} private Fragen`, `max ${limit.maxWishknowledgeCount} Fragen im Wunschwissen`],
        } as Plan,
        plus: {
            name: 'Plus',
            price: 3,
            priceLabel: 'pro Monat',
            description: ['Du unterstützt die Weiterentwicklung! Uneingeschränkt private Inhalte!'],
            list: ['Du unterstützt uns :-)', 'Unbeschränkt private Themen', 'Unbeschränkt private Fragen', 'Unbeschränktes Wunschwissen'],
        } as Plan,
        team: {
            name: 'Team',
            price: 7,
            priceLabel: 'pro Monat & Nutzer',
            description: ['Für Teams, die eine zentrale Addministration und private Inhalte benötigen.'],
            listLabel: 'Alles aus Plus und zusätzlich:',
            list: ['Private Inhalte im Team bearbeiten', 'Teams verwalten'],
        } as Plan,
        organisation: {
            name: 'Organisation',
            price: 'Auf Anfrage',
            priceLabel: 'Individuelle Preise. Bitte kontaktiert uns für mehr Informationen.',
            description: ['Für Unternehmen und Institutionen, die zusätzliche Unterstützung benötigen.', 'Bringen Sie Ihr Team auf den neuesten Stand.'],
            listLabel: 'Alles aus Team, plus:',
            list: ['SSO / AD-Integration', 'Eigener Server / Hosting in Eurem Rechenzentrium', 'Customizing'],
        } as Plan
    }
}