import type { RouterConfig } from '@nuxt/schema'
import { Tab as TopicTab } from '~~/components/topic/tabs/tabsStore'
import { Tab as UsersTab } from '~~/components/users/tabsEnum'

// https://router.vuejs.org/api/interfaces/routeroptions.html
export default <RouterConfig>{
    routes: (_routes) => [
        {
            name: 'welcomePage',
            path: '/',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Topic, redirectFromWelcomePage: true },
            meta: {
                middleware: ['topic-loader'],
            },
        },
        {
            name: 'termsPage',
            path: '/AGB',
            component: () => import('~/pages/terms.vue')
        },
        {
            name: 'imprintPage',
            path: '/Impressum',
            component: () => import('~/pages/imprint.vue')
        },
        {
            name: 'pricePage',
            path: '/Preise',
            component: () => import('~/pages/membership/price.vue')
        },
        {
            name: 'registerPage',
            path: '/Registrieren',
            component: () => import('~/pages/user/register.vue')
        },
        {
            name: 'messagesPage',
            path: '/Nachrichten',
            component: () => import('~/pages/user/messages.vue')
        },
        {
            name: 'questionPage',
            path: '/Fragen/:title/:id(\\d+)',
            component: () => import('~/pages/question/[title]/[id].vue')
        },
        {
            name: 'questionPage2',
            path: '/Fragen/:title/:id(\\d+)/:step',
            component: () => import('~/pages/question/[title]/[id].vue')
        },
        {
            name: 'usersPage',
            path: '/Nutzer',
            component: () => import('~/pages/user/users.vue'),
            props: { tab: UsersTab.AllUsers }
        },
        {
            name: 'userPage',
            path: '/Nutzer/:name/:id(\\d+)',
            component: () => import('~/pages/user/[name]/[id].vue')
        },
        {
            name: 'userSettingsPage',
            path: '/Nutzer/:name/:id/Einstellungen',
            component: () => import('~/pages/user/[name]/[id].vue'),
            props: { isSettingsPage: true }
        },
        {
            name: 'directUserSettingsPage',
            path: '/Nutzer/Einstellungen',
            component: () => import('~/pages/user/[name]/[id].vue'),
            props: { isSettingsPage: true }
        },
        {
            name: 'topicContentPage',
            path: '/:topic/:id(\\d+)',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Topic },
            meta: {
                middleware: ['topic-loader'],
            },
        },
        {
            name: 'topicLearningPage',
            path: '/:topic/:id(\\d+)/Lernen',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Learning },
            meta: {
                middleware: ['topic-loader'],
            },
        },
        {
            name: 'topicLearningPageWithQuestion',
            path: '/:topic/:id(\\d+)/Lernen/:questionId(\\d+)',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Learning },
            meta: {
                middleware: ['topic-loader'],
            },
        },
        {
            name: 'topicFeedPage',
            path: '/:topic/:id(\\d+)/Feed',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Feed },
            meta: {
                middleware: ['topic-loader'],
            },
        },
        {
            name: 'topicAnalyticsPage',
            path: '/:topic/:id(\\d+)/Analytics',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Analytics },
            meta: {
                middleware: ['topic-loader'],
            },
        },
        {
            name: 'allTopicHistoryOverview',
            path: '/Historie/Themen',
            component: () => import('~~/pages/history/topic/allTopicsOverview.vue'),
        },
        {
            name: 'topicHistoryOverview',
            path: '/Historie/Thema/:id(\\d+)',
            component: () => import('~~/pages/history/topic/overview.vue'),
        },
        {
            name: 'topicHistoryDetail',
            path: '/Historie/Thema/:topicId/:currentRevisionId/:firstEditId',
            component: () => import('~/pages/history/topic/detail.vue'),
        },
        {
            name: 'topicHistoryDetailWithPrevRev',
            path: '/Historie/Thema/:topicId/:currentRevisionId/',
            component: () => import('~/pages/history/topic/detail.vue'),
        },
        {
            name: 'maintenantePage',
            path: '/Maintenance',
            component: () => import('~/pages/maintenance.vue'),
            meta: {
                middleware: ['auth'],
            },
        }
    ],
}