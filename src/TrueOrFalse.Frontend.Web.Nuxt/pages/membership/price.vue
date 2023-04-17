
<template>
    <div class="container">
        <div class="main-page">
            <div class="row header">
                <div class="col-md-12">
                    <div class="first-header">MITGLID WERDEN UND FREIE BILDUNG UNTERSTÜTZEN!</div>
                    <div class="second-header">Öffentlich ist kostenlos – für immer und alle!</div>
                    <div class="third-header">Öffentlichen Inhalte sind auf memucho uneingeschränkter nutzbar – Freie Daten!
                        Freie
                        Software! (open data open access)<br />
                        Du möchtest memucho privat nutzen? Hier findest unsere Pläne:</div>
                </div>
            </div>
            <div id="PricesOuter" class="row">
                <div class="col-sm-6 col-md-6 col-lg-3">
                    <div class="price-inner" id="Free">
                        <div class="head-line">Kostenlos</div>
                        <div class="price">0€</div>
                        <div class="first-text">Kostenlos für immer!</div>
                        <button>KOSTENLOS REGISTRIEREN</button>
                        <div class="second-text">
                            <p>Für einzelne Personen, die schnell und unkompliziert mit dem Lernen beginnen wollen.</p>
                            <p> Erstelle Dein persönliches Wiki und verwalte erste persönliche Dokumente und kleine
                                Lerneinheiten.</p>
                        </div>
                        <div class="enumeration">
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Unbeschränkt öffentliche
                                Inhalte<br />
                                <span class="second-row">(ansehen, erstellen und lernen)</span>
                            </div>
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> 10 private Themen </div>
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> 50 private Fragen</div>
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> max 150 Fragen im Wunschwissen</div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-md-6 col-lg-3 second-row">
                    <div class="price-inner" id="Plus">
                        <div class="head-line">Plus</div>
                        <div class="price">5€</div>
                        <div class="first-text">pro Monat bei monatlicher Zahlung</div>
                        <button @click="handleCheckout(SubscriptionType.Plus)">Auswählen</button>
                        <div class="second-text">
                            <p>Für einzelne Personen, die täglich Lernen und sich Wissen zu einer Vielzahl an Themen
                                aneignen
                                wollen.</p>
                            <p>Verwalten Sie unbegrenzt private Inhalte.</p>
                        </div>
                        <div class="enumeration">
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Unbeschränkt private Themen</div>
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Unbeschränkt private Fragen</div>
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Unbeschränktes Wunschwissen</div>
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Private Inhalte teilen </div>
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Gruppen Verwalten </div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-md-6 col-lg-3 third-row">
                    <div class="price-inner" id="Team">
                        <div class="head-line">Team</div>
                        <div class="price">7€</div>
                        <div class="first-text">pro Monat bei monatlicher Zahlung</div>
                        <button class="disabled" disabled>In Planung</button>
                        <div class="second-text">
                            <p>Für Lehrende und Lerngruppen, die sich gemeinsam Themen erarbeiten und gemeinsam Lernen
                                wollen.
                            </p>
                            <p>Verwalten Sie Lerngruppen und unbegrenzt Inhalte.</p>
                        </div>
                        <div class="enumeration">
                            <div>Alles aus Personal, plus: <br /></div>
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Private Inhalte im Team bearbeiten
                            </div>
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Teams verwalten</div>
                        </div>
                    </div>
                </div>
                <div class="col-sm-6 col-md-6 col-lg-3 fourth-row">
                    <div class="price-inner" id="Organisation">
                        <div class="head-line">Organisation</div>
                        <div class="price-organisation">Auf Anfrage</div>
                        <div class="first-text">Individuelle Preise. Bitte Kontaktieren Sie uns für mehr Informationen.
                        </div>
                        <button class="contact">KONTAKTIEREN</button>
                        <div class="second-text">
                            <p>Für Unternehmen und Institutionen, die zusätzliche Unterstützung benötigen.</p>
                            <p>Bringen Sie Ihr Team auf den neuesten Stand.</p>
                        </div>
                        <div class="enumeration">
                            <div>Alles aus Team, plus: <br />
                            </div>
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Individuelle Anbindung an
                                Infrastruktur
                            </div>
                            <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Installation on premise oder
                                gehostet
                            </div>
                        </div>
                    </div>
                </div>
            </div>



            <div class="full-width-row">
                <div id="FaqHeaderOuter">
                    <div class="faq-first-header">Häufig gestellte</div>
                    <div>Fragen</div>
                </div>

                <div id="QuestionsOuter">

                    <FaqItem v-for="item in faqItems" :question="item.question" :answer="item.answer" />

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

enum SubscriptionType {
    Plus,
    Team
}

const handleCheckout = async (type: SubscriptionType): Promise<void> => {
    if (!userStore.isLoggedIn) {
        userStore.openLoginModal()
        return
    }
    let priceId = '';

    if (type == SubscriptionType.Plus)
        priceId = config.public.stripePlusPriceId
    else if (type == SubscriptionType.Team)
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
        margin-top: 100px;

        color: #003264;

        .first-header {
            font-weight: 600;


        }

        .second-header {
            margin-top: 20px;
            font-size: 45px;
            font-weight: 400;
        }

        .third-header {
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
                margin-top: 10px;
                padding: 8px 16px 8px 16px;
                background: #0065CA;
                font-weight: 600;
                font-size: 14px;
                color: #FFFFFF;
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
        margin-top: 725px;
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

