export interface UserSection {
    id: string
    translationKey: string
    tooltipKey?: string
}

const STATS_SECTION: UserSection = {
    id: 'Stats',
    translationKey: 'user.sidebar.stats',
}

const SKILLS_SECTION: UserSection = {
    id: 'Skills',
    translationKey: 'user.sidebar.skills',
    tooltipKey: 'user.sidebar.tooltips.skills'
}

const WIKIS_SECTION: UserSection = {
    id: 'Wikis',
    translationKey: 'user.sidebar.wikis',
    tooltipKey: 'user.sidebar.tooltips.wikis'
}

const QUESTIONS_SECTION: UserSection = {
    id: 'Questions',
    translationKey: 'user.sidebar.questions',
    tooltipKey: 'user.sidebar.tooltips.questions'
}

const SECTIONS: UserSection[] = [
    STATS_SECTION,
    SKILLS_SECTION,
    WIKIS_SECTION,
    QUESTIONS_SECTION,
]

export const USER_SECTIONS = SECTIONS

export default {
    SECTIONS,
    STATS_SECTION,
    SKILLS_SECTION,
    WIKIS_SECTION,
    QUESTIONS_SECTION,
}
