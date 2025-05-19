<script setup lang="ts">
import { SiteType } from '~/components/shared/siteEnum'
import { useUserStore } from '~/components/user/userStore'
const userStore = useUserStore()

const config = useRuntimeConfig()

const { locale, setLocale } = useI18n()
const emit = defineEmits(['setPage'])
emit('setPage', SiteType.Terms)

onBeforeMount(() => {
    if (locale.value != 'fr' && !userStore.isLoggedIn) {
        setLocale('fr')
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
        lang: 'fr'
    },
    link: [
        {
            rel: 'canonical',
            href: `${config.public.officialBase}/ConditionsGenerales`
        },
    ],
    meta: [
        {
            name: 'description',
            content: 'Lisez les Conditions Générales d’Utilisation de memoWikis.'
        },
        {
            property: 'og:title',
            content: 'Conditions Générales d’Utilisation | memoWikis'
        },
        {
            property: 'og:url',
            content: `${config.public.officialBase}/ConditionsGenerales`
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

                <h2 class="PageHeader">Conditions Générales d’Utilisation</h2>

                <p style="font-style: italic; font-size: 120%;">
                    Notre objectif est de rendre l’apprentissage plus simple et plus amusant. Une des bases pour y
                    parvenir
                    est la promotion des contenus éducatifs libres,
                    afin que le savoir de ce monde soit accessible à tous. <strong>Nous sommes ravis que tu participes à
                        memoWikis</strong> et nous te souhaitons
                    de découvrir de nombreux contenus passionnants et de partager tes connaissances avec d’autres. Les
                    quelques points à respecter
                    figurent dans ces Conditions Générales d’Utilisation. Tu dois les accepter pour t’inscrire sur
                    memoWikis.
                </p>

                <div class="alert alert-info" role="alert" style="margin: 25px 0;">
                    <h4>En bref :</h4>
                    <ul>
                        <li>Sois bienveillant·e envers les autres utilisateurs de memoWikis !</li>
                        <li>Si tu publies des contenus (p. ex. des questions), veille à ne pas enfreindre les droits
                            d’auteur.</li>
                        <li>Si tu publies des contenus, assure-toi qu’ils sont corrects et bien orthographiés. Ne publie
                            pas de contenus qui sont faux, insultants,
                            interdits ou publicitaires.</li>
                        <li>Seules les personnes privées (et pas des entreprises, par exemple) peuvent s’inscrire.</li>
                    </ul>
                    <p style="font-style: italic; margin-top: 20px;">
                        Ce résumé ne fait pas partie des Conditions Générales d’Utilisation juridiquement
                        contraignantes. Il
                        sert uniquement de vue d’ensemble et est fourni à titre informatif.
                    </p>
                </div>

                <h3>1) Règles de comportement</h3>
                <p>
                    memoWikis est une plateforme sociale de savoir. Elle vit du fait que de nombreuses personnes
                    s’unissent,
                    mettent leurs connaissances à disposition,
                    apprennent ensemble et s’entraident mutuellement. Comme dans tout lieu où se rencontrent beaucoup de
                    gens, des conflits peuvent survenir,
                    c’est pourquoi il est important de respecter certaines règles. Nous n’en avons qu’une :<br />
                    <strong>Sois bienveillant·e !</strong> Peu importe ce qui t’agace, pars toujours du principe que les
                    autres ont de bonnes intentions.
                    Souviens-toi qu’il y a un être humain de l’autre côté de la connexion Internet. Exprime tes
                    critiques
                    de manière objective et respectueuse,
                    et offre ton aide quand tu le peux.
                </p>

                <h3>2) Droits d’auteur & propriété intellectuelle</h3>
                <p>
                    Les droits d’auteur et la propriété intellectuelle sont des sujets sensibles sur Internet.
                    <strong>Tu es
                        toi-même responsable des contenus</strong>
                    que tu crées. memoWikis n’est pas responsable des contenus que tu ou d’autres utilisateurs ont
                    créés, mais
                    ne fait que les mettre à disposition.
                </p>
                <p>
                    Quand tu publies des textes ou des images sur memoWikis, tu dois t’assurer qu’ils ne sont pas
                    protégés par
                    des droits d’auteur.
                    Tu le confirmes à chaque fois que tu crées ou modifies une question, mais cela s’applique également
                    à tous
                    les autres domaines
                    (pages, sets d’apprentissage, etc.). Sache que les contenus sur memoWikis sont généralement sous
                    licence
                    Creative Commons « CC BY 4.0 ».
                    Une fois que tu les as publiés, ils peuvent être utilisés librement par tous les autres (sous
                    certaines
                    conditions).
                    Sois particulièrement vigilant·e, par exemple avec les manuels scolaires : tu n’as pas le droit de
                    copier
                    leur contenu.
                    Si tu n’es pas sûr·e, il vaut mieux t’abstenir de publier une question ou une image. En cas
                    d’infraction, tu
                    en es responsable.
                    Si nous apprenons qu’il y a une infraction aux droits d’auteur ou aux droits de propriété, nous
                    supprimerons
                    immédiatement les contenus concernés.
                    Si tu rencontres des contenus dont tu soupçonnes qu’ils enfreignent les droits d’auteur, fais-le
                    nous
                    savoir, s’il te plaît.
                </p>
                <p>
                    Pour tes propres images et tes propres textes, tu peux évidemment les utiliser ici, car tu en es
                    l’auteur·e. (Cela n’est valable, bien sûr, que si ce sont réellement tes formulations originales et
                    que tu
                    n’as pas copié ailleurs.)
                    Sur Wikipédia et Wikicommons, la plupart des textes et des images sont sous licences libres (par ex.
                    une
                    licence Creative Commons ou une licence GNU)
                    ou même dans le domaine public (ils n’appartiennent donc à personne), ce qui te permet également de
                    les
                    utiliser très bien sur memoWikis.
                </p>

                <h3>3) Les contenus : quoi, comment & quoi éviter</h3>
                <p>
                    Les contenus qui ne sont pas pertinents pour le grand public doivent être marqués comme questions
                    privées
                    (par exemple, la date d’anniversaire de ta tante,
                    le numéro de téléphone de ton ami ou ton gâteau préféré).
                </p>
                <p>
                    Garde à l’esprit que tous tes autres contenus peuvent être vus et utilisés par tous les utilisateurs
                    de
                    memoWikis !
                    <strong>Assure-toi donc</strong>
                </p>
                <ul>
                    <li>de l’exactitude et de la clarté de tes questions et réponses,</li>
                    <li>de la bonne orthographe, et</li>
                    <li>d’indiquer des sources quand c’est possible, pour permettre aux autres de vérifier la réponse en
                        cas de
                        doute ou de s’informer davantage
                    </li>
                </ul>

                <p>
                    memoWikis est une plateforme de savoir ouverte. Tout le savoir du monde doit y avoir sa place !
                    Toutefois,
                    la plateforme
                    ne doit pas être détournée à d’autres fins. Par conséquent, tu <strong>ne dois pas publier de
                        contenus
                        qui</strong>
                </p>
                <ul>
                    <li>sont manifestement faux,</li>
                    <li>servent essentiellement (et de façon évidente) à faire de la publicité (commerciale, religieuse,
                        etc.),</li>
                    <li>ont pour but d’insulter sciemment d’autres personnes,</li>
                    <li>enfreignent la loi (contenus obscènes, pédopornographie, etc.).</li>
                </ul>


                <h3>4) Utilisateurs & adhésion</h3>
                <p>
                    Seules les personnes privées (et non des entreprises, par exemple) peuvent s’inscrire comme
                    utilisateurs
                    ordinaires.
                    Pour les entreprises, associations à but non lucratif, etc., d’autres formules d’adhésion sont
                    possibles
                    après concertation.
                </p>
            </div>
        </div>
    </div>
</template>
