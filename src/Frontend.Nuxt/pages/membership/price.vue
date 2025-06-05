<script setup lang="ts">
interface FaqItem {
    question: string
    answer: string
    answerParams?: Record<string, string>
}
const { t, locale } = useI18n()

const faqItems = ref<FaqItem[]>([
    {
        "question": 'user.membership.faq.billing.question',
        "answer": 'user.membership.faq.billing.answer'
    },
    {
        "question": 'user.membership.faq.privateContent.question',
        "answer": 'user.membership.faq.privateContent.answer'
    },
    {
        "question": 'user.membership.faq.unlimitedContent.question',
        "answer": 'user.membership.faq.unlimitedContent.answer'
    },
    {
        "question": 'user.membership.faq.payment.question',
        "answer": 'user.membership.faq.payment.answer',
        "answerParams": { stripePrivacyUrl: `https://stripe.com/at/privacy` }
    },
    {
        "question": 'user.membership.faq.cardDeclined.question',
        "answer": 'user.membership.faq.cardDeclined.answer'
    },
])

const emit = defineEmits(['setBreadcrumb'])

onBeforeMount(() => {
    if (locale.value === 'de') {
        emit('setBreadcrumb', [{ name: t('user.membership.breadcrumb'), url: '/Preise' }])
    } else {
        emit('setBreadcrumb', [{ name: t('user.membership.breadcrumb'), url: '/Prices' }])
    }
})

const config = useRuntimeConfig()

const contact = () => {
    window.location.href = `mailto:${config.public.teamEmail}`
}
</script>

<template>

    <div>
        <div class="main-content row">
            <div class="col-md-12 header">
                <div class="top-label">{{ t('user.membership.header.topLabel') }}</div>
                <div class="title">{{ t('user.membership.header.title') }}</div>
                <div class="bottom-label">
                    {{ t('user.membership.header.bottomLabel1') }} <br /><br />
                    {{ t('user.membership.header.bottomLabel2') }}
                </div>
            </div>
            <div class="col-xs-12">
                <UserMembershipPlans class="plans-container" />
            </div>

        </div>
        <div class="faq-content row ">
            <div id="FaqHeaderOuter">
                <div class="faq-top-label">{{ t('user.membership.faq.title') }}</div>
            </div>

            <div id="QuestionsOuter">
                <UserMembershipFaqItem v-for="item in faqItems" :question="item.question" :answer="item.answer" :answer-params="item.answerParams" />

                <div id="NotFound">
                    <div class="not-found-header">{{ t('user.membership.faq.notFound') }}</div>
                    <div class="memoWikis-contact" @click="contact()">{{ t('user.membership.faq.contact') }}</div>
                    <div class="email">{{ config.public.teamMail }}</div>
                </div>
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

.faq-content {
    margin-top: 60px;
    background-color: @memo-grey-lighter;
    padding-top: 100px;
    margin-bottom: 40px;
    border-radius: 8px;

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
