import type { RouterConfig } from '@nuxt/schema'
import { Tab } from '~~/components/topic/tabs/tabsStore'

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
            props: { tab: Tab.Topic }
        },
        {
            name: 'topicLearningPage',
            path: '/:topic/:id/Lernen',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: Tab.Learning }
        },
        {
            name: 'topicLearningPageWithQuestion',
            path: '/:topic/:id/Lernen/:questionId',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: Tab.Learning }
        },
        {
            name: 'topicFeedPage',
            path: '/:topic/:id/Feed',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: Tab.Feed }
        },
        {
            name: 'topicAnalyticsPage',
            path: '/:topic/:id/Analytics',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: Tab.Analytics }
        },
    ],
}