<script setup lang="ts">
import { useUserStore } from '~/components/user/userStore'
const userStore = useUserStore()

const config = useRuntimeConfig()

const { locale, setLocale } = useI18n()

onBeforeMount(() => {
    if (locale.value != 'en' && !userStore.isLoggedIn) {
        setLocale('en')
    }
})

watch(() => locale.value, async (newLocale) => {
    switch (newLocale) {
        case 'de':
            await navigateTo('/AGB')
            break
        case 'en':
            await navigateTo('/TermsOfUse')
            break
        case 'fr':
            await navigateTo('/ConditionsGenerales')
            break
        case 'es':
            await navigateTo('/CondicionesDeUso')
            break
    }
})

useHead(() => ({
    htmlAttrs: {
        lang: 'en'
    },
    link: [
        {
            rel: 'canonical',
            href: `${config.public.officialBase}/TermsOfUse`
        },
    ],
    meta: [
        {
            name: 'description',
            content: 'Read the Terms and Conditions for memoWikis.'
        },
        {
            property: 'og:title',
            content: 'Terms and Conditions | memoWikis'
        },
        {
            property: 'og:url',
            content: `${config.public.officialBase}/TermsOfUse`
        },
        {
            property: 'og:type',
            content: `website`
        },
    ]
}))
</script>

<template>
    <div class="container">
        <div class="row main-page" style="padding-top:30px;">
            <div class="form-horizontal col-md-12 main-content">

                <h2 class="PageHeader">Terms of Use</h2>

                <p style="font-style: italic; font-size: 120%;">
                    Our goal is to make learning simpler and more enjoyable. A key aspect of this is promoting open educational
                    resources to ensure the world’s knowledge is accessible to everyone. <strong>We’re happy you’re taking part
                        in memoWikis</strong> and hope you discover plenty of exciting content while sharing your own knowledge
                    with others. The few rules you need to be aware of are gathered in these Terms of Use, and you must accept
                    them in order to register on memoWikis.
                </p>

                <div class="alert alert-info" role="alert" style="margin: 25px 0;">
                    <h4>The most important points in brief:</h4>
                    <ul>
                        <li>Be kind to other memoWikis users!</li>
                        <li>If you add content (e.g., questions), ensure you are not violating any copyrights.</li>
                        <li>
                            When posting content, ensure it is accurate and properly spelled. Do not post anything false, offensive,
                            prohibited, or purely advertising.
                        </li>
                        <li>Only private individuals may register (i.e., not companies).</li>
                    </ul>
                    <p style="font-style: italic; margin-top: 20px;">
                        This summary is not part of the legally binding Terms of Use. It is solely provided for clarity and information.
                    </p>
                </div>

                <h3>1) Rules of Conduct</h3>
                <p>
                    memoWikis is a social knowledge platform. It thrives on many people coming together, sharing their knowledge,
                    learning together, and supporting each other. However, whenever many people interact, conflicts can arise.
                    That is why it’s important to follow certain rules. We only have one main rule:<br />
                    <strong>Be nice!</strong> Whatever may irritate you, always assume others have good intentions.
                    Remember there is a real person on the other side of the internet connection. Share criticism in a factual
                    and respectful manner, and offer your help if you can.
                </p>

                <h3>2) Copyright & Intellectual Property</h3>
                <p>
                    Copyright and intellectual property are sensitive topics on the Internet. <strong>You, as the user,
                        are solely responsible for the content</strong> you create. memoWikis is not responsible for the content
                    you or other users create; it merely provides the platform.
                </p>
                <p>
                    If you post text or images on memoWikis, you must ensure they are not protected by copyright. You confirm
                    this every time you create or edit a question, but it also applies to all other areas (pages, learning sets, etc.).
                    Remember that content on memoWikis is generally published under the Creative Commons license “CC BY 4.0.”
                    Once you have posted it, anyone can freely use it under certain conditions. Exercise caution, for example,
                    with school textbooks—you’re not allowed to copy them. If in doubt, it’s better to refrain from uploading a
                    certain question or image. You are liable for violations. If we become aware of copyright or related rights
                    infringements, we will remove the offending content immediately. If you see any content that appears to
                    infringe someone’s copyright, please let us know.
                </p>
                <p>
                    You can, of course, use your own images and texts, as you are the author. (This only applies if they really
                    are your own original formulations and not copied from elsewhere.) Many texts and images from Wikipedia and
                    Wikimedia Commons are under open licenses (e.g., Creative Commons or GNU licenses) or are even in the
                    public domain (they belong to no one), so you can freely reuse them on memoWikis.
                </p>

                <h3>3) The Content: What, How & What Not</h3>
                <p>
                    Content that isn’t relevant to the general public must be marked as private questions (for instance, when
                    your aunt’s birthday is, your friend’s phone number, or your favorite cake).
                </p>
                <p>
                    Keep in mind that all other content you post can be viewed and used by every memoWikis user!
                    <strong>Please, therefore, ensure</strong>
                </p>
                <ul>
                    <li>the accuracy and clarity of your questions and answers,</li>
                    <li>correct spelling, and</li>
                    <li>that you provide sources when possible, so others can verify the answer or learn more if needed.</li>
                </ul>
                <p>
                    memoWikis is an open knowledge platform, intended to include all the world’s knowledge! However, the
                    platform must not be misused for other purposes. Therefore, you <strong>must not post content that</strong>
                </p>
                <ul>
                    <li>is clearly false,</li>
                    <li>is evidently and primarily for (commercial, religious, etc.) advertising,</li>
                    <li>deliberately insults others, or</li>
                    <li>violates applicable law (obscene content, child pornography, etc.).</li>
                </ul>

                <h3>4) Users & Membership</h3>
                <p>
                    Only private individuals may register as normal users—e.g., not companies. Other types of membership models
                    can be arranged for companies, non-profit organizations, etc.
                </p>

            </div>
        </div>
    </div>
</template>
