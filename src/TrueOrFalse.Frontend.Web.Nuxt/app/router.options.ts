import type { RouterConfig } from '@nuxt/schema'
import { Content } from '~/components/user/settings/contentEnum'
import { Tab as TopicTab } from '~~/components/topic/tabs/tabsStore'
import { Tab as UsersTab } from '~~/components/users/tabsEnum'
import { Tab as UserTab } from '~~/components/user/tabs/tabsEnum'

// https://router.vuejs.org/api/interfaces/routeroptions.html
export default <RouterConfig>{
    routes: (_routes) => [
        {
            name: 'welcomePage',
            path: '/',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Text },
            meta: {
                middleware: ['startpage'],
            },
        },
        {
            name: 'termsPage',
            path: '/AGB',
            component: () => import('~/pages/terms.vue')
        },
        {
            name: 'userSettingsSubscription',
            path: '/Nutzer/Einstellungen/Subscription',
            component: () => import('~/pages/user/[name]/[id].vue'),
            props: { isSettingsPage: true, activeContentProp: "Subscription" }
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
            name: 'cancel',
            path: '/Abbruch',
            component: () => import('~/pages/membership/cancel.vue')
        },
        {
            name: 'success',
            path: '/Erfolgreich',
            component: () => import('~/pages/membership/success.vue')
        },
        {
            name: 'registerPage',
            path: '/Registrieren',
            component: () => import('~/pages/user/register.vue')
        },
        {
            name: 'resetPasswordPage',
            path: '/NeuesPasswort/:token',
            component: () => import('~/pages/user/resetPassword.vue')
        },
        {
            name: 'confirmEmailPage',
            path: '/EmailBestaetigen/:token',
            component: () => import('~/pages/user/confirmEmail.vue')
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
            props: { tab: UserTab.Settings },
            meta: {
                key: route => `/${route.params.name}/${route.params.id}`
            },
        },
        {
            name: 'userWuwiPage',
            path: '/Nutzer/:name/:id/Wunschwissen',
            component: () => import('~/pages/user/[name]/[id].vue'),
            props: { tab: UserTab.Wishknowledge },
            meta: {
                key: route => `/${route.params.name}/${route.params.id}`
            },
        },
        {
            name: 'directUserSettingsPage',
            path: '/Nutzer/Einstellungen',
            component: () => import('~/pages/user/[name]/[id].vue'),
            props: { tab: UserTab.Settings },
            meta: {
                key: route => `/${route.params.name}/${route.params.id}`
            },
        },
        {
            name: 'userSubscriptionPage',
            path: '/Nutzer/Einstellungen/Mitgliedschaft',
            component: () => import('~/pages/user/[name]/[id].vue'),
            props: { tab: UserTab.Settings, content: Content.Membership },
            meta: {
                key: route => `/${route.params.name}/${route.params.id}`
            },
        },
        {
            name: 'topicContentPage',
            path: '/:topic/:id(\\d+)',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Text },
            meta: {
                key: route => `/${route.params.topic}/${route.params.id}`
            },
        },
        {
            name: 'topicLearningPage',
            path: '/:topic/:id(\\d+)/Lernen',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Learning },
            meta: {
                key: route => `/${route.params.topic}/${route.params.id}`
            },
        },
        {
            name: 'topicLearningPageWithQuestion',
            path: '/:topic/:id(\\d+)/Lernen/:questionId(\\d+)',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Learning },
            meta: {
                key: route => `/${route.params.topic}/${route.params.id}`
            },
        },
        {
            name: 'topicFeedPage',
            path: '/:topic/:id(\\d+)/Feed',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Feed },
            meta: {
                key: route => `/${route.params.topic}/${route.params.id}`
            },
        },
        {
            name: 'topicAnalyticsPage',
            path: '/:topic/:id(\\d+)/Analytics',
            component: () => import('~/pages/[topic]/[id].vue'),
            props: { tab: TopicTab.Analytics },
            meta: {
                key: route => `/${route.params.topic}/${route.params.id}`
            },
        },
        {
            name: 'maintenantePage',
            path: '/Maintenance',
            component: () => import('~/pages/maintenance.vue'),
            meta: {
                middleware: ['admin-auth'],
            },
        },
        {
            name: 'catchAll',
            path: '/:catchAll(.*)',
            component: () => import('~/pages/catchAll.vue'),
            hidden: true
        },
        {
            name: 'metrics',
            path: '/Metriken',
            component: () => import('~/pages/metrics.vue'),
            meta: {
                middleware: ['admin-auth'],
            },
        },
    ],
}   
