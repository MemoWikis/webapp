import type { RouterConfig } from '@nuxt/schema'
import { Tab as TopicTab } from '~~/components/topic/tabs/tabsStore'
import { Tab as UsersTab } from '~~/components/users/tabsEnum'

// https://router.vuejs.org/api/interfaces/routeroptions.html
export default <RouterConfig>{
    routes: (_routes) => [
        {
            name: 'welcomePage',
            path: '/',
            component: () => import('~/pages/index.vue')
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
            path: '/Fragen/:title/:id',
            component: () => import('~/pages/question/[title]/[id].vue')
        },
        {
            name: 'usersPage',
            path: '/Nutzer',
            component: () => import('~/pages/user/users.vue')
        },
        {
            name: 'networkPage',
            path: '/Netzwerk',
            component: () => import('~/pages/user/users.vue'),
            props: { tab: UsersTab.Network }
        },
        {
            name: 'userPage',
            path: '/Nutzer/:name/:id',
            component: () => import('~/pages/user/[name]/[id].vue')
        },
        {
            name: 'userSettingsPage',
            path: '/Nutzer/:name/:id/Einstellungen',
            component: () => import('~/pages/user/[name]/[id].vue'),
            props: { isSettingsPage: true }
        },
        {
            name: 'userSettingsPage',
            path: '/Nutzer/Einstellungen',
            component: () => import('~/pages/user/[name]/[id].vue'),
            props: { isSettingsPage: true }
        },
        {
            name: 'topicContentPage',
            path: '/:topic/:id',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Topic }
        },
        {
            name: 'topicLearningPage',
            path: '/:topic/:id/Lernen',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Learning }
        },
        {
            name: 'topicLearningPageWithQuestion',
            path: '/:topic/:id/Lernen/:questionId',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Learning }
        },
        {
            name: 'topicFeedPage',
            path: '/:topic/:id/Feed',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Feed }
        },
        {
            name: 'topicAnalyticsPage',
            path: '/:topic/:id/Analytics',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Analytics }
        },
    ],
}