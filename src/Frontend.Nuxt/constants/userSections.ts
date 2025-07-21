export interface UserSection {
    id: string
    translationKey: string
}

const STATS_SECTION: UserSection = {
    id: 'Stats',
    translationKey: 'user.sidebar.stats',
}

const SKILLS_SECTION: UserSection = {
    id: 'Skills',
    translationKey: 'user.sidebar.skills',
}

const WIKIS_SECTION: UserSection = {
    id: 'Wikis',
    translationKey: 'user.sidebar.wikis',
}

const QUESTIONS_SECTION: UserSection = {
    id: 'Questions',
    translationKey: 'user.sidebar.questions',
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
