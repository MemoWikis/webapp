export interface MissionControlSection {
    id: string
    translationKey: string
    tooltipKey?: string
}

const KNOWLEDGE_STATUS_SECTION: MissionControlSection = {
    id: 'KnowledgeStatus',
    translationKey: 'missionControl.sections.knowledgeStatus',
}

const WIKIS_SECTION: MissionControlSection = {
    id: 'Wikis',
    translationKey: 'missionControl.sections.wikis',
}

const FAVORITES_SECTION: MissionControlSection = {
    id: 'Favorites',
    translationKey: 'missionControl.sections.favorites',
}

const LEARN_CALENDAR_SECTION: MissionControlSection = {
    id: 'LearnCalendar',
    translationKey: 'missionControl.sections.learnCalendar',
}

const SECTIONS: MissionControlSection[] = [
    KNOWLEDGE_STATUS_SECTION,
    WIKIS_SECTION,
    FAVORITES_SECTION,
    LEARN_CALENDAR_SECTION,
]

export const MISSION_CONTROL_SECTIONS = SECTIONS

export default {
    SECTIONS,
    KNOWLEDGE_STATUS_SECTION,
    WIKIS_SECTION,
    FAVORITES_SECTION,
    LEARN_CALENDAR_SECTION,
}