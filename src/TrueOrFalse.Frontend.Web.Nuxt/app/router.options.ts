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
            path: '/Frage/:title/:id',
            component: () => import('~/pages/question/[title]/[id].vue')
        },
        {
            name: 'userPage',
            path: '/Nutzer/:name/:id',
            component: () => import('~/pages/user/[name]/[id].vue')
        },
        {
            name: 'topicContentPage',
            path: '/:topic/:id',
            component: () => import('~/pages/[topic]/[id].vue')
        },
        {
            name: 'topicLearningPage',
            path: '/:topic/:id/Lernen',
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