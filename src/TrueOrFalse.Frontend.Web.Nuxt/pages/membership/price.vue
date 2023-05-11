<script setup lang="ts">
import { loadStripe, Stripe } from '@stripe/stripe-js';
import * as Subscription from '~~/components/user/membership/subscription';
import { useUserStore } from '~~/components/user/userStore';

const userStore = useUserStore()
interface FaqItem {
    question: string
    answer: string
}
const faqItems = ref<FaqItem[]>([
    {
        question: 'Wie kann ich bezahlen?',
        answer: 'Kreditkarte und Vorabüberweisung'
    },
    {
        question: 'Kann ich jederzeit kündigen?',
        answer: 'Ja du kannst jederzeit kündigen, dein Abo endet dann automatisch zum nächsten Abrechnungstermin'
    },
    {
        question: 'Wie uneingeschränkt sind uneingeschränkte Inhalte?',
        answer: 'Genauso wie wir es sagen: Uneingeschränkt'
    },
    {
        question: 'Verlängert sich mein Abonnement automatisch?',
        answer: 'Um es mit Goethes Faust, Teil 2, Vorspiel auf dem Theater, Vers 1265 zu sagen: Ja!'
    },
])

//stripe
const config = useRuntimeConfig()

const stripePromise = loadStripe(config.public.stripeKey);
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

const emit = defineEmits(['setBreadcrumb'])

onBeforeMount(() => {
    emit('setBreadcrumb', [{ name: 'Preise', url: '/Preise' }])
})

</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="main-content">
                <div class="col-md-12 header">
                    <div class="top-label">MITGLIED WERDEN UND FREIE BILDUNG UNTERSTÜTZEN!</div>
                    <div class="title">Öffentlich ist kostenlos – für immer und alle!</div>
                    <div class="bottom-label">
                        Öffentlichen Inhalte sind auf memucho uneingeschränkter nutzbar – Freie Daten!
                        Freie
                        Software! (open data open access)<br />
                        Du möchtest memucho privat nutzen? Hier findest unsere Pläne:
                    </div>
                </div>
                <div class="container">
                    <div class="row subscription-plans">

                        <UserMembershipPriceCard :plan="Subscription.plans.basic" :selected="false"
                            :class="{ 'selected': userStore.isLoggedIn && userStore.subscriptionType == Subscription.Type.Basic }">
                            <template v-slot:button>
                                <button class="memo-button btn-default"
                                    v-if="userStore.isLoggedIn && userStore.subscriptionType != Subscription.Type.Basic">
                                    <NuxtLink to="/Nutzer/Einstellungen/Subscription">
                                        Downgrade
                                    </NuxtLink>
                                </button>
                                <button class="memo-button btn-success" disabled
                                    v-else-if="userStore.isLoggedIn && userStore.subscriptionType == Subscription.Type.Basic">
                                    <NuxtLink to="/Registrieren">
                                        Deine Mitgliedschaft
                                    </NuxtLink>
                                </button>
                                <button class="memo-button btn-default" v-else>
                                    <NuxtLink to="/Registrieren">
                                        Kostenlos registrieren
                                    </NuxtLink>
                                </button>


                            </template>
                        </UserMembershipPriceCard>

                        <UserMembershipPriceCard :plan="Subscription.plans.plus" :selected="false"
                            :class="{ 'recommended': !userStore.isLoggedIn }">
                            <template v-slot:button>
                                <button class="memo-button btn-primary btn" v-if="userStore.isLoggedIn && userStore.subscriptionType != Subscription.Type.Plus" @click="handleCheckout(Subscription.Type.Plus)">
                                    Auswählen
                                </button>
                                <button class="memo-button btn-success" disabled v-else-if="userStore.isLoggedIn && userStore.subscriptionType == Subscription.Type.Plus">
                                    <NuxtLink to="/Registrieren">
                                        Deine Mitgliedschaft
                                    </NuxtLink>
                                </button>
                            </template>
                        </UserMembershipPriceCard>

                        <UserMembershipPriceCard :plan="Subscription.plans.team" :selected="false">
                            <template v-slot:button>
                                <button class="memo-button btn-primary btn" disabled>
                                    In Planung
                                </button>
                            </template>
                        </UserMembershipPriceCard>

                        <UserMembershipPriceCard :plan="Subscription.plans.organisation" :selected="false">
                            <template v-slot:button>
                                <button class="memo-button btn-link">Kontaktieren</button>
                            </template>
                        </UserMembershipPriceCard>
                    </div>
                </div>
            </div>

        </div>
    </div>
    <div class="full-width-row">
        <div id="FaqHeaderOuter">
            <div class="faq-top-label">Häufig gestellte</div>
            <div>Fragen</div>
        </div>

        <div id="QuestionsOuter">

            <UserMembershipFaqItem v-for="item in faqItems" :question="item.question" :answer="item.answer" />

            <div id="NotFound">
                <div class="not-found-header">Deine Frage nicht gefunden?</div>
                <a class="memucho-contact" href="mailto:abc@example.com">memucho kontaktieren</a>
                <div class="email">team@memucho.de</div>
            </div>
        </div>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.container {
    .header {

        display: flex;
        flex-wrap: wrap;
        flex-direction: column;

        color: @memo-grey-darker;

        .top-label {
            font-weight: 600;
        }

        .title {
            margin-top: 20px;
            font-size: 45px;
            font-weight: 400;
            color: @memo-blue;
        }

        .bottom-label {
            margin-top: 20px;
            font-weight: 400;
            font-size: 18px;
        }
    }

    .subscription-plans {
        button {
            width: 100%;
            display: flex;
            align-items: center;
            justify-content: center;
        }
    }
}

#FaqHeaderOuter {

    color: @memo-blue;
    font-weight: 400;
    font-size: 45px;
    display: flex;
    align-items: center;
    flex-direction: column;
}

.full-width-row {
    margin-top: 60px;
    background-color: @memo-grey-lighter;
    padding-top: 100px;

    #QuestionsOuter {
        margin-top: 31px;
        background-color: @memo-grey-lighter;
        margin-left: auto;
        margin-right: auto;
        font-weight: 400;
        font-size: 16px;
        max-width: 1000px;

        .not-found {
            margin-top: 112px;
            color: @memo-blue;
        }
    }
}

#NotFound {
    display: flex;
    flex-direction: column;
    align-items: center;
    margin-top: 112px;
    font-weight: 400;

    .not-found-header {
        color: @memo-blue;
        font-size: 18px;

    }

    .memucho-contact {
        margin-top: 26px;
        color: @memo-blue-link;
        font-size: 16px;
    }

    .email {
        font-size: 12px;
        margin-top: 12px;
        color: @memo-grey-dark;
        margin-bottom: 43px;

    }

}
</style> 

