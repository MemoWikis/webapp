<script setup lang="ts">
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
                        Öffentliche Inhalte sind auf memucho uneingeschränkt nutzbar – freie Daten!
                        Freie Software! (open data open access)<br />
                        Du möchtest memucho privat nutzen? Hier findest unsere Pläne:
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

