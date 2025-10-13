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

const PAGES_SECTION: UserSection = {
    id: 'Pages',
    translationKey: 'user.sidebar.pages',
    tooltipKey: 'user.sidebar.tooltips.pages'
}

const QUESTIONS_SECTION: UserSection = {
    id: 'Questions',
    translationKey: 'user.sidebar.questions',
    tooltipKey: 'user.sidebar.tooltips.questions'
}

const SKILLS_PLACEHOLDER_SECTION: UserSection = {
    id: 'Skills-placeholder',
    translationKey: 'user.sidebar.skills',
    tooltipKey: 'user.sidebar.tooltips.skills'
}

const WIKIS_PLACEHOLDER_SECTION: UserSection = {
    id: 'Wikis-placeholder',
    translationKey: 'user.sidebar.wikis',
    tooltipKey: 'user.sidebar.tooltips.wikis'
}

const PAGES_PLACEHOLDER_SECTION: UserSection = {
    id: 'Pages-placeholder',
    translationKey: 'user.sidebar.pages',
    tooltipKey: 'user.sidebar.tooltips.pages'
}

const QUESTIONS_PLACEHOLDER_SECTION: UserSection = {
    id: 'Questions-placeholder',
    translationKey: 'user.sidebar.questions',
    tooltipKey: 'user.sidebar.tooltips.questions'
}

const SECTIONS: UserSection[] = [
    STATS_SECTION,
    SKILLS_SECTION,
    WIKIS_SECTION,
    PAGES_SECTION,
    QUESTIONS_SECTION,
    SKILLS_PLACEHOLDER_SECTION,
    WIKIS_PLACEHOLDER_SECTION,
    PAGES_PLACEHOLDER_SECTION,
    QUESTIONS_PLACEHOLDER_SECTION,
]

export const USER_SECTIONS = SECTIONS

export default {
    SECTIONS,
    STATS_SECTION,
    SKILLS_SECTION,
    WIKIS_SECTION,
    PAGES_SECTION,
    QUESTIONS_SECTION,
    SKILLS_PLACEHOLDER_SECTION,
    WIKIS_PLACEHOLDER_SECTION,
    PAGES_PLACEHOLDER_SECTION,
    QUESTIONS_PLACEHOLDER_SECTION,
}
