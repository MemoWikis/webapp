
<template>
    <div class="container">
        <div class="main-page row">
            <div class="col-md-12 header">
                <div class="top-label">MITGLID WERDEN UND FREIE BILDUNG UNTERSTÜTZEN!</div>
                <div class="title">Öffentlich ist kostenlos – für immer und alle!</div>
                <div class="bottom-label">
                    Öffentlichen Inhalte sind auf memucho uneingeschränkter nutzbar – Freie Daten!
                    Freie
                    Software! (open data open access)<br />
                    Du möchtest memucho privat nutzen? Hier findest unsere Pläne:
                </div>
            </div>
            <div id="PricesOuter" class="row">

                <UserMembershipPriceCard :plan="Membership.plans.basic" :selected="false">
                    <template v-slot:button>
                        <NuxtLink to="/Registrieren">
                            <button class="memo-button btn-primary btn">
                                Kostenlos registrieren
                            </button>
                        </NuxtLink>
                    </template>
                </UserMembershipPriceCard>

                <UserMembershipPriceCard :plan="Membership.plans.plus" :selected="false">
                    <template v-slot:button>
                        <button class="memo-button btn-primary btn" @click="handleCheckout(Membership.Type.Plus)">
                            Auswählen
                        </button>
                    </template>
                </UserMembershipPriceCard>

                <UserMembershipPriceCard :plan="Membership.plans.team" :selected="false">
                    <template v-slot:button>
                        <button class="disabled memo-button btn-primary btn" disabled>
                            In Planung
                        </button>
                    </template>
                </UserMembershipPriceCard>

                <UserMembershipPriceCard :plan="Membership.plans.organisation" :selected="false">
                    <template v-slot:button>
                        <button class="memo-button btn-link">Kontaktieren</button>
                    </template>
                </UserMembershipPriceCard>
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
        </div>
    </div>
</template>

<script setup lang="ts">
import { loadStripe, Stripe } from '@stripe/stripe-js';
import * as Membership from '~~/components/user/membership/membership';
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
    const result = await $fetch<CheckoutSessionResult>('/apiVue/Price/CreateCheckoutSession', {
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


const handleCheckout = async (type: Membership.Type): Promise<void> => {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    let priceId = '';

    if (type == Membership.Type.Plus)
        priceId = config.public.stripePlusPriceId
    else if (type == Membership.Type.Team)
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
<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

@media screen and (max-width: 768px) {
    .second-row {
        height: auto;
    }
}

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
            margin-bottom: 20px;
        }
    }

    #PricesOuter {
        .second-row {
            height: 590px;
        }

        @media screen and (max-width: 768px) {
            .second-row {
                height: auto;
                margin-top: 10px;
            }

            .third-row {
                margin-top: 10px;
            }

            .fourth-row {
                margin-top: 10px;
            }


        }

        .price-inner {
            border: #DDDDDD 1px solid;
            padding: 20px;

            .head-line {
                font-size: 32px;
                margin-right: 20px;
                font-weight: 400;
            }

            .contact {
                color: #0065CA;
                background: white;
                font-size: 14px;

            }

            .disabled {
                background: linear-gradient(0deg, rgba(255, 255, 255, 0.7), rgba(255, 255, 255, 0.7)), #0065CA;
            }

            .price {
                font-size: 45px;
                font-weight: 700;
                margin-top: 50px;
            }

            .price-organisation {
                margin-top: 50px;
                font-weight: 700;
                font-size: 18px;

            }

            .first-text {
                margin-top: 10px;
            }

            button {
                width: 100%;
                display: flex;
                justify-content: center;
                align-items: center;
            }

            .second-text {
                margin-top: 50px;
                color: #555555;
            }

            .enumeration {
                font-weight: 400;
                font-size: 14px;
                color: #003264;

                .fa-check {
                    font-size: 18px;
                    font-weight: 900;
                }

                .second-row {
                    margin-left: 22px;
                }
            }
        }
    }

    #FaqHeaderOuter {

        color: #074EE8;
        font-weight: 400;
        font-size: 45px;
        display: flex;
        align-items: center;
        flex-direction: column;
    }

    .full-width-row {
        margin-top: 60px;
        width: 100vw;
        position: relative;
        left: 50%;
        right: 50%;
        margin-left: -50vw;
        margin-right: -50vw;
        background-color: #EFEFEF;
        padding-top: 100px;

        #QuestionsOuter {
            margin-top: 31px;
            background-color: #EFEFEF;
            margin-left: auto;
            margin-right: auto;
            font-weight: 400;
            font-size: 16px;
            max-width: 1000px;

            .not-find {
                margin-top: 112px;
                color: #074EE8;

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
            color: #074EE8;
            font-size: 18px;

        }

        .memucho-contact {
            margin-top: 26px;
            color: #FA00FF;
            font-size: 16px;
        }

        .email {
            font-size: 12px;
            margin-top: 12px;
            color: #FA00FF;
            margin-bottom: 43px;

        }

    }
}
</style> 

