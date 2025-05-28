export interface PageAnalyticsSection {
    id: string
    translationKey: string
}

const KNOWLEDGE_SECTION: PageAnalyticsSection = {
    id: 'AnalyticsKnowledgeSummary',
    translationKey: 'page.analytics.yourKnowledgeStatus',
}
const CONTENT_SECTION: PageAnalyticsSection = {
    id: 'AnalyticsContent',
    translationKey: 'page.analytics.contentTitle',
}
const VIEWS_SECTION: PageAnalyticsSection = {
    id: 'AnalyticsViews',
    translationKey: 'page.analytics.viewsTitle',
}
const PAGE_VIEWS_SECTION: PageAnalyticsSection = {
    id: 'AnalyticsPageViews',
    translationKey: 'page.analytics.pageViewsLast90Days',
}
const QUESTION_VIEWS_SECTION: PageAnalyticsSection = {
    id: 'AnalyticsQuestionViews',
    translationKey: 'page.analytics.questionViewsLast90Days',
}
const LEARN_CALENDAR_SECTION: PageAnalyticsSection = {
    id: 'AnalyticsLearnCalendar',
    translationKey: 'page.analytics.learnActivity',
}

const SECTIONS: PageAnalyticsSection[] = [
    KNOWLEDGE_SECTION,
    LEARN_CALENDAR_SECTION,
    CONTENT_SECTION,
    VIEWS_SECTION,
    PAGE_VIEWS_SECTION,
    QUESTION_VIEWS_SECTION,
]

export const PAGE_ANALYTICS_SECTIONS = SECTIONS

export default {
    SECTIONS,
    KNOWLEDGE_SECTION,
    CONTENT_SECTION,
    VIEWS_SECTION,
    PAGE_VIEWS_SECTION,
    QUESTION_VIEWS_SECTION,
    LEARN_CALENDAR_SECTION,
}
