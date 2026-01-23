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

    if (type === Subscription.Type.Smart || type === Subscription.Type.Plus)
        selectedPriceId.value = config.public.stripeSmartPriceId || config.public.stripePlusPriceId
    else if (type === Subscription.Type.Expert || type === Subscription.Type.Team)
        selectedPriceId.value = config.public.stripeExpertPriceId || config.public.stripeTeamPriceId

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
    window.location.href = `mailto:${config.public.teamEmail}`
}

const plans = ref()

async function setPlanData() {
    const limits = await $api<Subscription.PlanLimits>(`/apiVue/UserMembershipPlans/GetPlanLimits`, {
        method: 'GET',
        mode: 'cors',
        credentials: 'include',
        onResponseError(context) {
            const { $logger } = useNuxtApp()
            $logger.error(`fetch Error: ${context.response?.statusText}`, [{ response: context.response, req: context.request }])
        }
    })
    if (limits != null)
        plans.value = Subscription.plans(limits)
}

onBeforeMount(() => {
    setPlanData()
})

// Check if user has Smart or Expert subscription (including legacy Plus/Team)
const hasSmartSubscription = computed(() =>
    userStore.subscriptionType === Subscription.Type.Smart ||
    userStore.subscriptionType === Subscription.Type.Plus
)

const hasExpertSubscription = computed(() =>
    userStore.subscriptionType === Subscription.Type.Expert ||
    userStore.subscriptionType === Subscription.Type.Team
)
</script>

<template>
    <div class="subscription-plans" v-if="plans">

        <div class="subscription-section">
            <UserMembershipPriceCard :plan="plans.basic" :selected="false"
                :class="{ 'selected': userStore.isLoggedIn && userStore.subscriptionType === Subscription.Type.Basic }">
                <template v-slot:button>
                    <button class="memo-button btn-primary btn"
                        v-if="userStore.isLoggedIn && userStore.subscriptionType != Subscription.Type.Basic">
                        <NuxtLink to="/User/Settings/Membership">
                            {{ t('user.membership.plans.downgrade') }}
                        </NuxtLink>
                    </button>
                    <button class="memo-button btn-success btn"
                        v-else-if="userStore.isLoggedIn && userStore.subscriptionType === Subscription.Type.Basic">
                        <NuxtLink to="/User/Settings/Membership">
                            {{ t('user.membership.plans.yourMembership') }}
                        </NuxtLink>
                    </button>
                    <button class="memo-button btn-primary btn" v-else>
                        <NuxtLink :to="`/${t('url.register')}`">
                            {{ t('user.membership.plans.registerFree') }}
                        </NuxtLink>
                    </button>
                </template>
            </UserMembershipPriceCard>

            <UserMembershipPriceCard :plan="plans.smart" :selected="false"
                :class="{ 'selected': userStore.isLoggedIn && hasSmartSubscription }">
                <template v-slot:button>
                    <button class="memo-button btn-primary btn" v-if="userStore.isLoggedIn === false">
                        <NuxtLink :to="`/${t('url.register')}`">
                            {{ t('user.membership.plans.registerFree') }}
                        </NuxtLink>
                    </button>
                    <button class="memo-button btn-primary btn"
                        v-if="userStore.isLoggedIn && !hasSmartSubscription && !hasExpertSubscription"
                        @click="initStripeCheckout(Subscription.Type.Smart)">
                        {{ t('user.membership.plans.select') }}
                    </button>
                    <button class="memo-button btn-success" v-else-if="userStore.isLoggedIn && hasSmartSubscription">
                        <NuxtLink to="/User/Settings/Membership">
                            {{ t('user.membership.plans.yourMembership') }}
                        </NuxtLink>
                    </button>
                    <button class="memo-button btn-primary btn"
                        v-else-if="userStore.isLoggedIn && hasExpertSubscription">
                        <NuxtLink to="/User/Settings/Membership">
                            {{ t('user.membership.plans.downgrade') }}
                        </NuxtLink>
                    </button>
                </template>
            </UserMembershipPriceCard>
        </div>

        <div class="subscription-section">
            <UserMembershipPriceCard :plan="plans.expert" :selected="false"
                :class="{ 'recommended': !userStore.isLoggedIn || (!hasSmartSubscription && !hasExpertSubscription), 'selected': userStore.isLoggedIn && hasExpertSubscription }">
                <template v-slot:button>
                    <button class="memo-button btn-primary btn" v-if="userStore.isLoggedIn === false">
                        <NuxtLink :to="`/${t('url.register')}`">
                            {{ t('user.membership.plans.startNow') }}
                        </NuxtLink>
                    </button>
                    <button class="memo-button btn-primary btn filled"
                        v-if="userStore.isLoggedIn && !hasExpertSubscription"
                        @click="initStripeCheckout(Subscription.Type.Expert)">
                        {{ t('user.membership.plans.startNow') }}
                    </button>
                    <button class="memo-button btn-success" v-else-if="userStore.isLoggedIn && hasExpertSubscription">
                        <NuxtLink to="/User/Settings/Membership">
                            {{ t('user.membership.plans.yourMembership') }}
                        </NuxtLink>
                    </button>
                </template>
            </UserMembershipPriceCard>

            <UserMembershipPriceCard :plan="plans.organisation" :selected="false"
                :class="{ 'selected': userStore.isLoggedIn && userStore.subscriptionType === Subscription.Type.Organisation }">
                <template v-slot:button>
                    <button @click="contact" class="memo-button btn-link">{{ t('user.membership.plans.contact')
                        }}</button>
                </template>
            </UserMembershipPriceCard>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

.subscription-plans {
    padding-top: 30px;
    display: flex;
    justify-content: center;
    flex-direction: row;

    flex-wrap: wrap;
    width: 100%;

    .subscription-section {
        width: calc(50% - 0.5rem);
        display: flex;
        flex-direction: row;
        flex-wrap: wrap;
        justify-content: center;
        gap: 0 1rem;

        @media (max-width: 1400px) {
            width: 100%;
        }
    }

    button {
        width: 100%;
        display: flex;
        align-items: center;
        justify-content: center;

        &.filled {
            background: @memo-blue;
            color: white;
            font-weight: 600;
        }
    }
}
</style>