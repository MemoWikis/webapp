<script setup lang="ts">
interface FaqItem {
    question: string
    answer: string
}
const faqItems = ref<FaqItem[]>([
    {
        "question": "Was passiert, wenn mein Abo ausläuft?",
        "answer": "Wenn dein Abo ausläuft, kannst du keine weiteren privaten Inhalte über die Basislimits hinaus mehr anlegen. Allerdings werden deine bestehenden Inhalte nicht gelöscht."
    },
    {
        "question": "Werden private Inhalte gelöscht, wenn mein Abo ausläuft?",
        "answer": "Nein, deine privaten Inhalte werden nicht gelöscht, es sei denn, du möchtest das. Du kannst weiterhin auf sie zugreifen, auch wenn dein Abo ausgelaufen ist."
    },
    {
        "question": "Was bedeutet “uneingeschränkte Inhalte”, gibt es keine Limits?",
        "answer": "Bislang haben wir keine Limits vorgesehen. Wenn jedoch technische Grenzen erreicht werden, informieren wir dich. Bei normaler Nutzung solltest du keine technischen Limits erreichen. Limits für Bilder und Videos werden noch festgelegt."
    },
    {
        "question": "Wie erfolgt die Bezahlung?",
        "answer": "Die Zahlung erfolgt über den Zahlungsdienstleister Stripe. Stripe arbeitet GDRP/DSGVO-konform (https://stripe.com/at/privacy). Wir erhalten von Stripe nur eine Rückmeldung über den Zahlungserfolg. Alle weiteren Vorgänge, wie Rechnungslegung und Verwaltung der Kreditkartendaten, werden von Stripe durchgeführt."
    },
    {
        "question": "Meine Kreditkarte wurde abgelehnt und ich möchte auf meine privaten Inhalte zugreifen. Wie gehe ich vor?",
        "answer": "Auch wenn dein Abo abgelaufen ist oder deine Kreditkarte bei der Verlängerung abgelehnt wurde, kannst du weiterhin auf deine privaten Inhalte zugreifen."
    },
])

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
                    <!-- <div class="top-label"></div> -->
                    <div class="title">Mitgliedschaft</div>
                    <!-- <div class="title">Öffentlich ist kostenlos – für immer und alle!</div> -->
                    <div class="bottom-label">
                        Öffentliche Inhalte sind auf memoWikis uneingeschränkt nutzbar. Freie Daten –
                        freie Software (open data open access)! <br /><br />
                        Du möchtest memoWikis für private Inhalte nutzen? Hier findest unsere Preise:
                    </div>
                </div>
                <div class="col-xs-12">
                    <UserMembershipPlans class="plans-container" />
                </div>
            </div>

        </div>
    </div>
    <div class="full-width-row">
        <div id="FaqHeaderOuter">
            <div class="faq-top-label">Häufig gestellte Fragen</div>
            <!-- <div>Fragen</div> -->
        </div>

        <div id="QuestionsOuter">

            <UserMembershipFaqItem v-for="item in faqItems" :question="item.question" :answer="item.answer" />

            <div id="NotFound">
                <div class="not-found-header">Deine Frage nicht gefunden?</div>
                <a class="memoWikis-contact" href="mailto:abc@example.com">memoWikis kontaktieren</a>
                <div class="email">team@memowikis.net</div>
            </div>
        </div>
    </div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.plans-container {
    margin: 0 -10px;
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
    text-align: center;
    margin-left: 10px;
    margin-right: 10px;
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

    .memoWikis-contact {
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
