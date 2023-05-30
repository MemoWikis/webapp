<script lang="ts" setup>
import { useUserStore } from '../userStore'
import { loadStripe, Stripe } from '@stripe/stripe-js'
import * as Subscription from '~~/components/user/membership/subscription'

const userStore = useUserStore()
const config = useRuntimeConfig()

const stripePromise = loadStripe(config.public.stripeKey)
const sessionId = ref<string>('');

interface CheckoutSessionResult {
    success: boolean
    id?: string
}
const createOrUpdateSubscription = async (id: string): Promise<string> => {
    const result = await $fetch<CheckoutSessionResult>('/apiVue/StripeAdminstration/CompletedSubscription', {
        method: 'POST',
        body: { priceId: id },
        credentials: 'include'
    });
    if (result.success)
        return result.id ? result.id : ''
    else return ''
}



const redirectToCheckout = async (sessionId: string): Promise<void> => {
    const stripe: Stripe | null = await stripePromise;

    if (stripe) {
        const { error } = await stripe.redirectToCheckout({
            sessionId,
        });

        if (error) {
            console.log('Fehler beim Weiterleiten zur Checkout-Seite:', error);
        }
    }
}


const handleCheckout = async (type: Subscription.Type): Promise<void> => {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    let priceId = '';

    if (type == Subscription.Type.Plus)
        priceId = config.public.stripePlusPriceId
    else if (type == Subscription.Type.Team)
        priceId = config.public.stripeTeamPriceId

    sessionId.value = await createOrUpdateSubscription(priceId);
    if (sessionId.value)
        await redirectToCheckout(sessionId.value);
}
</script>

<template>
    <div class="">
        <div class="subscription-plans">

            <UserMembershipPriceCard :plan="Subscription.plans.basic" :selected="false"
                :class="{ 'selected': userStore.isLoggedIn && userStore.subscriptionType == Subscription.Type.Basic }">
                <template v-slot:button>
                    <button class="memo-button btn-primary btn"
                        v-if="userStore.isLoggedIn && userStore.subscriptionType != Subscription.Type.Basic">
                        <NuxtLink to="/Nutzer/Einstellungen/Mitgliedschaft">
                            Downgrade
                        </NuxtLink>
                    </button>
                    <button class="memo-button btn-success btn"
                        v-else-if="userStore.isLoggedIn && userStore.subscriptionType == Subscription.Type.Basic">
                        <NuxtLink to="/Nutzer/Einstellungen/Mitgliedschaft">
                            Deine Mitgliedschaft
                        </NuxtLink>
                    </button>
                    <button class="memo-button btn-primary btn" v-else>
                        <NuxtLink to="/Registrieren">
                            Kostenlos registrieren
                        </NuxtLink>
                    </button>


                </template>
            </UserMembershipPriceCard>

            <UserMembershipPriceCard :plan="Subscription.plans.plus" :selected="false"
                :class="{ 'recommended': !userStore.isLoggedIn, 'selected': userStore.isLoggedIn && userStore.subscriptionType == Subscription.Type.Plus }">
                <template v-slot:button>
                    <button class="memo-button btn-primary btn"
                        v-if="userStore.isLoggedIn == false">
                        <NuxtLink to="/Registrieren">
                            Kostenlos registrieren
                        </NuxtLink>
                    </button>
                    <button class="memo-button btn-primary btn"
                        v-if="userStore.isLoggedIn && userStore.subscriptionType != Subscription.Type.Plus"
                        @click="handleCheckout(Subscription.Type.Plus)">
                        Auswählen
                    </button>
                    <button class="memo-button btn-success"
                        v-else-if="userStore.isLoggedIn && userStore.subscriptionType == Subscription.Type.Plus">
                        <NuxtLink to="/Nutzer/Einstellungen/Mitgliedschaft">
                            Deine Mitgliedschaft
                        </NuxtLink>
                    </button>
                </template>
            </UserMembershipPriceCard>
            
            <UserMembershipPriceCard :plan="Subscription.plans.team" :selected="false"
                :class="{ 'selected': userStore.isLoggedIn && userStore.subscriptionType == Subscription.Type.Team }">
                <template v-slot:button>
                    <button class="memo-button btn-primary btn" disabled>
                        In Planung
                    </button>
                </template>
            </UserMembershipPriceCard>

            <UserMembershipPriceCard :plan="Subscription.plans.organisation" :selected="false"
                :class="{ 'selected': userStore.isLoggedIn && userStore.subscriptionType == Subscription.Type.Organisation }">
                <template v-slot:button>
                    <button class="memo-button btn-link">Kontaktieren</button>
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