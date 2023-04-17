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
        description: ['Für einzelne Personen, die schnell und unkompliziert mit dem Lernen beginnen wollen.', 'Erstelle Dein persönliches Wiki und verwalte erste persönliche Dokumente und kleine Lerneinheiten.'],
        list: ['<b>Unbeschränkt</b> öffentliche Inhalte (ansehen, erstellen und lernen)', '<b>10</b> private Themen', '<b>50</b> private Fragen', 'max <b>150</b> Fragen im Wunschwissen'],
    },
    plus: {
        name: 'Plus',
        price: 3,
        priceLabel: 'pro Monat bei monatlicher Zahlung',
        description: ['Für einzelne Personen, die täglich Lernen und sich Wissen zu einer Vielzahl an Themen aneignen wollen.', 'Verwalten Sie unbegrenzt private Inhalte.'],
        list: ['<b>Unbeschränkt</b> private Themen', '<b>Unbeschränkt</b> private Fragen', '<b>Unbeschränktes</b> Wunschwissen', 'Private Inhalte teilen', 'Gruppen Verwalten'],
    },
    team: {
        name: 'Team',
        price: 7,
        priceLabel: 'pro Monat bei monatlicher Zahlung',
        description: ['Für Lehrende und Lerngruppen, die sich gemeinsam Themen erarbeiten und gemeinsam Lernen wollen.', 'Verwalten Sie Lerngruppen und unbegrenzt Inhalte.'],
        listLabel: 'Alles aus Plus und zusätzlich:',
        list: ['Private Inhalte im Team bearbeiten', 'Teams verwalten'],
    },
    organisation: {
        name: 'Organisation',
        price: 'Auf Anfrage',
        priceLabel: 'Individuelle Preise. <br/> Bitte Kontaktieren Sie uns für mehr Informationen.',
        description: ['Für Unternehmen und Institutionen, die zusätzliche Unterstützung benötigen.', 'Bringen Sie Ihr Team auf den neuesten Stand.'],
        listLabel: 'Alles aus Team, plus:',
        list: ['Individuelle Anbindung an Infrastruktur', 'Installation on premise oder gehostet'],
    }
}