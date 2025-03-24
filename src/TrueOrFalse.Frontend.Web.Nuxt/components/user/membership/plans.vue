<script lang="ts" setup>
import { AlertType, useAlertStore } from '~/components/alert/alertStore'
import { useUserStore } from '../userStore'
import * as Subscription from '~~/components/user/membership/subscription'

const userStore = useUserStore()
const config = useRuntimeConfig()
const alertStore = useAlertStore()
const selectedPriceId = ref<string>('')
const { t } = useI18n()
const redirectingDialogTitle = t('user.membership.plans.stripe.redirectDialog.title')

interface CheckoutSessionResult {
    success: boolean
    id?: string
}
const { $logger } = useNuxtApp()
const getStripeSessionId = async (priceId: string): Promise<string> => {
    const result = await $api<CheckoutSessionResult>('/apiVue/StripeAdminstration/CompletedSubscription', {
        method: 'POST',
        body: { priceId: priceId },
        credentials: 'include',
        onResponseError(context) {
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, host: context.request }])
        },
    })
    if (result.success)
        return result.id ? result.id : ''
    else return ''
}

onMounted(() => {
    alertStore.$onAction(({ name, after }) => {
        if (name === 'closeAlert')
            after((result) => {
                if (result.id === redirectingDialogTitle && !result.cancelled) {
                    consentForStripeGiven.value = true
                    redirectToCheckout()
                }
            })
    })
})

const consentForStripeGiven = ref(userStore.hasStripeCustomerId)

const redirectToCheckout = async (): Promise<void> => {

    if (!consentForStripeGiven) return

    const sessionId = await getStripeSessionId(selectedPriceId.value)

    if (!sessionId || sessionId === '') {
        alertStore.openAlert(AlertType.Error, { text: t('user.membership.plans.stripe.error') })
        return
    }

    const { loadStripe } = await import('@stripe/stripe-js')

    const stripe = await loadStripe(config.public.stripeKey)

    const { $logger } = useNuxtApp()

    if (stripe) {
        const { error } = await stripe.redirectToCheckout({
            sessionId,
        })

        if (error) {
            $logger.error('Error when forwarding to the checkout page', error)
        }
    } else {
        $logger.error('Error while loading stripe')
    }
}

const initStripeCheckout = (type: Subscription.Type) => {

    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }

    if (type === Subscription.Type.Plus)
        selectedPriceId.value = config.public.stripePlusPriceId
    else if (type === Subscription.Type.Team)
        selectedPriceId.value = config.public.stripeTeamPriceId

    if (consentForStripeGiven.value) {
        redirectToCheckout()
    } else {
        //Is handled as close event:
        alertStore.openAlert(
            AlertType.Default,
            { text: t('user.membership.plans.stripe.redirectDialog.message') },
            t('user.membership.plans.stripe.redirectDialog.confirm'),
            true,
            redirectingDialogTitle,
            redirectingDialogTitle
        )
    }
}

function contact() {
    window.location.href = "mailto:team@memucho.de"
}

const plans = ref()

async function setPlanData() {
    const limit = await $api<Subscription.BasicLimits>(`/apiVue/UserMembershipPlans/GetBasicLimits`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            const { $logger } = useNuxtApp()
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
        }
    })
    if (limit != null)
        plans.value = Subscription.plans(limit)
}

onBeforeMount(() => {
    setPlanData()
})
</script>

<template>
    <div class="">
        <div class="subscription-plans" v-if="plans">

            <UserMembershipPriceCard :plan="plans.basic" :selected="false"
                :class="{ 'selected': userStore.isLoggedIn && userStore.subscriptionType === Subscription.Type.Basic }">
                <template v-slot:button>
                    <button class="memo-button btn-primary btn"
                        v-if="userStore.isLoggedIn && userStore.subscriptionType != Subscription.Type.Basic">
                        <NuxtLink to="/Nutzer/Einstellungen/Mitgliedschaft">
                            {{ t('user.membership.plans.downgrade') }}
                        </NuxtLink>
                    </button>
                    <button class="memo-button btn-success btn"
                        v-else-if="userStore.isLoggedIn && userStore.subscriptionType === Subscription.Type.Basic">
                        <NuxtLink to="/Nutzer/Einstellungen/Mitgliedschaft">
                            {{ t('user.membership.plans.yourMembership') }}
                        </NuxtLink>
                    </button>
                    <button class="memo-button btn-primary btn" v-else>
                        <NuxtLink to="/Registrieren">
                            {{ t('user.membership.plans.registerFree') }}
                        </NuxtLink>
                    </button>


                </template>
            </UserMembershipPriceCard>

            <UserMembershipPriceCard :plan="plans.plus" :selected="false" :class="{ 'recommended': !userStore.isLoggedIn, 'selected': userStore.isLoggedIn && userStore.subscriptionType === Subscription.Type.Plus }">
                <template v-slot:button>
                    <button class="memo-button btn-primary btn" v-if="userStore.isLoggedIn === false">
                        <NuxtLink to="/Registrieren">
                            {{ t('user.membership.plans.registerFree') }}
                        </NuxtLink>
                    </button>
                    <button class="memo-button btn-primary btn"
                        v-if="userStore.isLoggedIn && userStore.subscriptionType != Subscription.Type.Plus"
                        @click="initStripeCheckout(Subscription.Type.Plus)">
                        {{ t('user.membership.plans.select') }}
                    </button>
                    <button class="memo-button btn-success"
                        v-else-if="userStore.isLoggedIn && userStore.subscriptionType === Subscription.Type.Plus">
                        <NuxtLink to="/Nutzer/Einstellungen/Mitgliedschaft">
                            {{ t('user.membership.plans.yourMembership') }}
                        </NuxtLink>
                    </button>
                </template>
            </UserMembershipPriceCard>

            <UserMembershipPriceCard :plan="plans.team" :selected="false" :class="{ 'selected': userStore.isLoggedIn && userStore.subscriptionType === Subscription.Type.Team }">
                <template v-slot:button>
                    <button class="memo-button btn-primary btn" disabled>
                        {{ t('user.membership.plans.inPlanning') }}
                    </button>
                </template>
            </UserMembershipPriceCard>

            <UserMembershipPriceCard :plan="plans.organisation" :selected="false" :class="{ 'selected': userStore.isLoggedIn && userStore.subscriptionType === Subscription.Type.Organisation }">
                <template v-slot:button>
                    <button @click="contact" class="memo-button btn-link">{{ t('user.membership.plans.contact') }}</button>
                </template>
            </UserMembershipPriceCard>
        </div>
    </div>
</template>

<style lang="less" scoped>
.subscription-plans {
    button {
        width: 100%;
        display: flex;
        align-items: center;
        justify-content: center;
    }

}
</style>