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

export const plans: { basic: Plan, plus: Plan, team: Plan, organisation: Plan } = {
    basic: {
        name: 'Kostenlos',
        price: 0,
        priceLabel: 'Kostenlos für immer!',
        description: ['Du kannst alles machen. Nur die Anzahl der privaten Inhalte ist begrenzt.'],
        list: ['Unbeschränkt öffentliche Inhalte (ansehen, erstellen und lernen)', '10 private Themen', '20 private Fragen', 'max 500 Fragen im Wunschwissen'],
    },
    plus: {
        name: 'Plus',
        price: 3,
        priceLabel: 'pro Monat',
        description: ['Du unterstützt die Weiterentwicklung! Uneingeschränkt private Inhalte!'],
        list: ['Du unterstützt uns :-)','Unbeschränkt private Themen', 'Unbeschränkt private Fragen', 'Unbeschränktes Wunschwissen'],
    },
    team: {
        name: 'Team',
        price: 7,
        priceLabel: 'pro Monat & Nutzer',
        description: ['Für Teams, die eine zentrale Addministration und private Inhalte benötigen.'],
        listLabel: 'Alles aus Plus und zusätzlich:',
        list: ['Private Inhalte im Team bearbeiten', 'Teams verwalten'],
    },
    organisation: {
        name: 'Organisation',
        price: 'Auf Anfrage',
        priceLabel: 'Individuelle Preise. Bitte kontaktiert uns für mehr Informationen.',
        description: ['Für Unternehmen und Institutionen, die zusätzliche Unterstützung benötigen.', 'Bringen Sie Ihr Team auf den neuesten Stand.'],
        listLabel: 'Alles aus Team, plus:',
        list: ['SSO / AD-Integration', 'Eigener Server / Hosting in Eurem Rechenzentrium','Customizing'],
    }
}