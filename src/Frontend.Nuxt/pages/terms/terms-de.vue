<script setup lang="ts">
import { SiteType } from '~/components/shared/siteEnum'
import { useUserStore } from '~/components/user/userStore'
const userStore = useUserStore()

const config = useRuntimeConfig()

const { locale, setLocale } = useI18n()
const emit = defineEmits(['setPage'])
emit('setPage', SiteType.Terms)

onBeforeMount(() => {
    if (locale.value != 'de' && !userStore.isLoggedIn) {
        setLocale('de')
    }
})

const { t } = useI18n()
watch(
    () => locale.value,
    async () => {
        const localeUrl = `/${t('url.termsOfUse')}`
        await navigateTo(localeUrl)
    }
)

useHead(() => ({
    htmlAttrs: {
        lang: 'de'
    },
    link: [
        {
            rel: 'canonical',
            href: `${config.public.officialBase}/AGB`
        },
    ],
    meta: [
        {
            name: 'description',
            content: 'Lies die Nutzungsbedingungen für memoWikis.'
        },
        {
            property: 'og:title',
            content: 'Nutzungsbedingungen | memoWikis'
        },
        {
            property: 'og:url',
            content: `${config.public.officialBase}/AGB`
        },
        {
            property: 'og:type',
            content: `website`
        },
    ]
}))
</script>


<template>

    <div class="form-horizontal main-content">

        <h2 class="PageHeader">Nutzungsbedingungen</h2>

        <p style="font-style: italic; font-size: 120%;">
            Unser Ziel ist es, dass Lernen einfacher wird und mehr Spaß macht. Eine Grundlage dafür ist die
            Förderung von offenen Bildungsinhalten,
            damit das Wissen dieser Welt für alle zugänglich ist. <strong>Wir freuen uns daher, dass du bei
                memoWikis mitmachst</strong> und wünschen dir,
            dass du viele spannende Inhalte entdeckst und dein eigenes Wissen mit anderen teilst. Die wenigen
            Dinge, die du dabei beachten musst,
            sind in diesen Nutzungsbedingungen gesammelt. Du musst sie akzeptieren, um dich bei memoWikis zu
            registrieren.
        </p>

        <div class="alert alert-info" role="alert" style="margin: 25px 0;">
            <h4>Das Wichtigste in Kürze:</h4>
            <ul>
                <li>Sei nett zu anderen memoWikis-Nutzern!</li>
                <li>Wenn du Inhalte (z.B. Fragen) selber einstellst, stelle sicher, dass du keine Urheberrechte
                    verletzt.</li>
                <li>Wenn du Inhalte einstellst, achte auf Richtigkeit und Rechtschreibung. Stelle keine Inhalte
                    ein, die falsch, beleidigend, verboten oder Werbung sind.</li>
                <li>Registrieren dürfen sich nur Privatpersonen und z.B. keine Unternehmen.</li>
            </ul>
            <p style="font-style: italic; margin-top: 20px;">Diese Zusammenfassung ist nicht Teil der
                rechtsverbindlichen Nutzungsbedingungen. Sie dient allein dem Überblick und ist informativ.</p>
        </div>

        <h3>1) Verhaltensregeln</h3>
        <p>
            memoWikis ist eine soziale Wissensplattform. Sie lebt davon, dass viele Menschen sich zusammentun,
            ihr
            Wissen bereitstellen,
            zusammen lernen und sich gegenseitig unterstützen. Wo viele Menschen aufeinandertreffen, kann es
            aber auch zu Konflikten kommen,
            deswegen ist es wichtig, verschiedene Regeln zu beachten. Wir haben nur eine Regel:<br />
            <strong>Sei nett!</strong> Egal, was dich ärgert, gehe immer von guten Intentionen der anderen aus.
            Denke daran, dass am anderen Ende der Internetleitung ein Mensch sitzt. Äußere Kritik sachlich und
            mit Respekt und biete deine Hilfe an,
            wenn du helfen kannst.
        </p>


        <h3>2) Urheberrecht & Geistiges Eigentum</h3>
        <p>
            Urheberrecht und geistiges Eigentum sind sensible Seiten im Internet. <strong>Du bist als Nutzer
                selbst verantwortlich für die Inhalte</strong>,
            die du erstellst. memoWikis ist nicht für die Inhalte verantwortlich, die du oder andere Nutzer
            erstellt haben, sondern stellt diese lediglich bereit.
        </p>
        <p>
            Wenn du Texte oder Bilder bei memoWikis einstellst, musst du sicher sein, dass diese nicht
            urheberrechtlich geschützt sind.
            Dies bestätigst du jedesmal, wenn du eine Frage erstellst oder änderst, es gilt aber auch bei allen
            anderen Bereichen
            (Seiten, Lernsets etc.). Denke daran, dass Inhalte bei memoWikis generell unter der Creative
            Commons-Lizenz “CC BY 4.0” stehen.
            Wenn du sie einmal eingestellt hast, können sie von allen anderen (unter bestimmten Bedingungen)
            frei verwendet werden. Besondere
            Vorsicht ist z.B. bei Schulbüchern geboten, hier darfst du nicht abschreiben. Wenn du dir nicht
            sicher bist, verzichte im Zweifelsfall
            lieber auf eine bestimmte Frage oder ein Bild. Für Verstöße bist du verantwortlich. Erfahren wir von
            solchen Verstöße gegen das
            Urheberrecht oder Verwertungsrechte, entfernen wir die betroffenen Inhalte umgehend. Solltest du auf
            Inhalte stoßen, bei denen du
            ein Verstoß gegen das Urheberrecht vermutest, sage uns bitte Bescheid.
        </p>
        <p>
            Deine eigenen Bilder und deine eigenen Texte kannst du verwenden, hier bist du der Urheber. (Das
            gilt natürlich nur,
            solange es wirklich deine eigenen Formulierungen sind und du nicht abgeschrieben hast.) Auch bei
            Wikipedia und Wikicommons
            stehen fast alle Texte und Bilder unter offen Lizenzen (z.B. einer Creative Commons-Lizenz oder
            einer GNU-Lizenz) oder
            sind sogar gemeinfrei (sie gehören also niemandem), so dass du sie sehr gut bei memoWikis nutzen
            kannst.
        </p>


        <h3>3) Die Inhalte: Was, wie & was nicht</h3>
        <p>
            Inhalte, die für die Allgemeinheit nicht relevant sind, müssen als private Fragen markiert sein (zum
            Beispiel, wann deine
            Tante Geburtstag hat, wie die Telefonnummer von deinem Freund lautet oder welches dein
            Lieblingskuchen ist).
        </p>
        <p>
            Denke daran, dass alle anderen Inhalte von dir von allen memoWikis-Nutzern gesehen und genutzt
            werden
            können! <strong>Achte deshalb auf</strong>
        </p>
        <ul>
            <li>Richtigkeit und Eindeutigkeit deiner Fragen und Antworten,</li>
            <li>Rechtschreibung, und</li>
            <li>Gib, wo dies möglich ist, Quellen an, damit andere im Zweifel die Antwort nachprüfen oder sich
                weiter informieren können</li>
        </ul>

        <p>
            memoWikis ist eine offene Wissensplattform. Alles Wissen der Welt soll hier Platz haben! Allerdings
            darf die Plattform
            nicht für andere Zwecke missbraucht werden. Daher darfst du <strong>keine Inhalte einstellen,
                die</strong>

        </p>
        <ul>
            <li>offensichtlich falsch sind,</li>
            <li>offensichtlich und primär der (kommerziellen, religiösen etc.) Werbung dienen,</li>
            <li>absichtlich andere beleidigen oder</li>
            <li>gegen geltendes Recht verstoßen (öbszöne Inhalte, Kinderpornografie u.ä.).</li>
        </ul>


        <h3>4) Nutzer & Mitgliedschaft</h3>
        <p>
            Als normale Nutzer registrieren dürfen sich nur Privatpersonen und z.B. keine Unternehmen. Für
            Unternehmen, gemeinnützige Vereine etc.
            gibt es nach Absprache andere Modelle der Mitgliedschaft.
        </p>


    </div>

</template>