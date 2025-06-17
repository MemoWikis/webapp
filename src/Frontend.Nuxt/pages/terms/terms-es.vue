<script setup lang="ts">
import { SiteType } from '~/components/shared/siteEnum'
import { useUserStore } from '~/components/user/userStore'
const userStore = useUserStore()

const config = useRuntimeConfig()

const { locale, setLocale } = useI18n()
const emit = defineEmits(['setPage'])
emit('setPage', SiteType.Terms)

onBeforeMount(() => {
    if (locale.value != 'es' && !userStore.isLoggedIn) {
        setLocale('es')
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
        lang: 'es'
    },
    link: [
        {
            rel: 'canonical',
            href: `${config.public.officialBase}/CondicionesDeUso`
        },
    ],
    meta: [
        {
            name: 'description',
            content: 'Lee las Condiciones de Uso de memoWikis.'
        },
        {
            property: 'og:title',
            content: 'Condiciones de Uso | memoWikis'
        },
        {
            property: 'og:url',
            content: `${config.public.officialBase}/CondicionesDeUso`
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

        <h2 class="PageHeader">Condiciones de Uso</h2>

        <p style="font-style: italic; font-size: 120%;">
            Nuestro objetivo es hacer que el aprendizaje sea más sencillo y divertido. Una base importante para
            ello
            es promover contenidos educativos abiertos,
            de modo que el conocimiento de este mundo sea accesible para todos. <strong>Nos alegra que
                participes en memoWikis</strong> y esperamos
            que descubras muchos contenidos interesantes y compartas tus propios conocimientos con los demás.
            Los
            pocos aspectos que debes tener en cuenta
            se recopilan en estas Condiciones de Uso. Debes aceptarlas para registrarte en memoWikis.
        </p>

        <div class="alert alert-info" role="alert" style="margin: 25px 0;">
            <h4>Lo más importante de un vistazo:</h4>
            <ul>
                <li>¡Sé amable con otros usuarios de memoWikis!</li>
                <li>Si publicas contenidos (por ejemplo, preguntas), asegúrate de no infringir derechos de
                    autor.</li>
                <li>Si publicas contenidos, presta atención a la exactitud y ortografía. No publiques contenidos
                    que sean falsos, ofensivos, ilegales o publicitarios.</li>
                <li>Solo pueden registrarse personas particulares, por ejemplo, no empresas.</li>
            </ul>
            <p style="font-style: italic; margin-top: 20px;">
                Este resumen no forma parte de las Condiciones de Uso legalmente vinculantes. Sirve únicamente
                como visión general y tiene carácter informativo.
            </p>
        </div>

        <h3>1) Normas de conducta</h3>
        <p>
            memoWikis es una plataforma social de conocimiento. Se nutre del hecho de que muchas personas se
            unan,
            aporten sus conocimientos,
            aprendan juntas y se apoyen mutuamente. Donde se encuentran muchas personas, también pueden surgir
            conflictos,
            por eso es importante respetar ciertas reglas. Solo tenemos una regla:<br />
            <strong>¡Sé amable!</strong> Independientemente de lo que te moleste, asume siempre que los demás
            tienen buenas intenciones.
            Ten en cuenta que al otro lado de la línea de Internet hay una persona. Expresa tus críticas de
            forma
            objetiva y respetuosa, y ofrece tu ayuda
            cuando puedas.
        </p>

        <h3>2) Derechos de autor y propiedad intelectual</h3>
        <p>
            Los derechos de autor y la propiedad intelectual son ámbitos delicados en Internet.
            <strong>Tú eres personalmente responsable de los contenidos</strong> que creas. memoWikis no es
            responsable de los contenidos que tú u otros usuarios hayan creado, sino que solamente los pone a tu
            disposición.
        </p>
        <p>
            Cuando publiques textos o imágenes en memoWikis, debes asegurarte de que no estén protegidos por
            derechos de autor.
            Esto lo confirmas cada vez que creas o modificas una pregunta, pero también se aplica a todos los
            demás ámbitos
            (páginas, sets de aprendizaje, etc.). Ten en cuenta que los contenidos en memoWikis están, en
            general, bajo la licencia Creative Commons “CC BY 4.0”.
            Una vez que los hayas publicado, los demás pueden utilizarlos libremente (bajo ciertas condiciones).
            Ten especial cuidado, por ejemplo, con libros de texto:
            no se te permite copiar su contenido. Si no estás seguro, es mejor abstenerte de publicar
            determinada pregunta o imagen. Eres responsable de cualquier infracción.
            Si nos percatamos de que hay una infracción a los derechos de autor o derechos de explotación,
            eliminaremos inmediatamente los contenidos afectados.
            Si detectas algún contenido que consideres que infringe los derechos de autor, infórmanos por favor.
        </p>
        <p>
            Tus propias imágenes y tus propios textos puedes usarlos, ya que tú eres el autor. (Esto, por
            supuesto, solo se aplica mientras
            realmente sean tus propias formulaciones y no hayas copiado nada.) También en Wikipedia y
            Wikicommons,
            casi todos los textos e imágenes están bajo licencias abiertas (por ejemplo, una licencia Creative
            Commons o una licencia GNU) o
            incluso son de dominio público (por lo que no pertenecen a nadie), así que puedes utilizarlos
            fácilmente en memoWikis.
        </p>

        <h3>3) Los contenidos: qué, cómo y qué no</h3>
        <p>
            Los contenidos que no sean relevantes para el público en general deben marcarse como preguntas
            privadas (por ejemplo,
            la fecha de cumpleaños de tu tía, el número de teléfono de tu amigo o cuál es tu pastel favorito).
        </p>
        <p>
            Ten en cuenta que todos tus demás contenidos pueden ser vistos y utilizados por todos los usuarios
            de
            memoWikis.
            <strong>Por eso, presta atención a</strong>
        </p>
        <ul>
            <li>la exactitud y claridad de tus preguntas y respuestas,</li>
            <li>la ortografía, y</li>
            <li>cuando sea posible, cita fuentes para que otros puedan verificar la respuesta en caso de duda o
                informarse más
            </li>
        </ul>

        <p>
            memoWikis es una plataforma abierta de conocimiento. ¡Aquí debería tener cabida todo el conocimiento
            del mundo! Sin embargo,
            la plataforma no debe utilizarse indebidamente con otros fines. Por lo tanto, <strong>no debes
                publicar contenidos que</strong>
        </p>
        <ul>
            <li>sean claramente falsos,</li>
            <li>sirvan de manera evidente y principal a la publicidad (comercial, religiosa, etc.),</li>
            <li>tengan por objeto insultar deliberadamente a otros, o</li>
            <li>infrinjan la legislación vigente (contenido obsceno, pornografía infantil, etc.).</li>
        </ul>


        <h3>4) Usuarios y membresía</h3>
        <p>
            Solo pueden registrarse como usuarios normales las personas particulares y, por ejemplo, no las
            empresas.
            Para las empresas, asociaciones sin ánimo de lucro, etc., existen otros modelos de membresía tras
            acordarlo previamente.
        </p>
    </div>
</template>
