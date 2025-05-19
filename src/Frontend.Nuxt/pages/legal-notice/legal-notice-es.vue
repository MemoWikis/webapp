<script lang="ts" setup>
import { SiteType } from '~/components/shared/siteEnum'
import { useUserStore } from '~/components/user/userStore'
const userStore = useUserStore()

const config = useRuntimeConfig()

const { locale, setLocale } = useI18n()
const emit = defineEmits(['setPage'])
emit('setPage', SiteType.Imprint)

function openEmail() {
    window.location.href = `mailto:${config.public.teamEmail}`
}

onBeforeMount(() => {
    if (locale.value !== 'es' && !userStore.isLoggedIn) {
        setLocale('es')
    }
})

const { t } = useI18n()
watch(
    () => locale.value,
    async () => {
        const localeUrl = `/${t('url.missionControl')}`
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
            href: `${config.public.officialBase}/AvisoLegal`
        }
    ],
    meta: [
        {
            name: 'description',
            content: 'Información legal y datos de la empresa para memoWikis.'
        },
        {
            property: 'og:title',
            content: 'Aviso Legal | memoWikis'
        },
        {
            property: 'og:url',
            content: `${config.public.officialBase}/AvisoLegal`
        },
        {
            property: 'og:type',
            content: `website`
        }
    ]
}))
</script>

<template>
    <div class="container">
        <div class="row main-page">
            <div class="form-horizontal col-md-12 main-content">

                <h1 class="PageHeader" id="Impressum">
                    Aviso legal y política de privacidad
                </h1>

                <h2>1. Aviso legal</h2>

                <h3>Información según el § 5 TMG:</h3>
                memucho GmbH<br />
                Am Moorhof <br />
                Nettgendorfer Str. 7 <br />
                14947 Nuthe-Urstromtal<br />

                <h3>Registro Mercantil:</h3>
                Registro en el Juzgado (Amtsgericht) de Cottbus, HRB 13499 CB<br />

                <h3>Representada por:</h3>
                Director general:<br />
                Robert Mischke<br />

                <h3 id="Kontakt">Contacto:</h3>
                Teléfono:<br />
                +49-178 186 68 48<br />
                <span class="mailme btn-link" @click="openEmail">{{ config.public.teamMail }}</span>

                <h3>Responsable del contenido según § 55 Abs. 2 RStV:</h3>
                <p>Robert Mischke</p>

                <h3>Exención de responsabilidad</h3>
                <p><strong>Responsabilidad por los contenidos</strong></p>
                <p>
                    Los contenidos de nuestras páginas se han creado con la máxima diligencia.
                    Sin embargo, no podemos garantizar la exactitud, integridad y actualidad
                    de dichos contenidos. Como proveedores de servicios, somos responsables de
                    los contenidos propios de estas páginas de acuerdo con las leyes generales
                    (§ 7 párr.1 TMG). No obstante, de conformidad con los §§ 8 a 10 TMG, no estamos
                    obligados como proveedores de servicios a supervisar la información
                    transmitida o almacenada de terceros ni a investigar circunstancias que
                    indiquen una actividad ilegal. Las obligaciones de eliminar o bloquear
                    el uso de la información conforme a las disposiciones legales generales
                    no se verán afectadas. Sin embargo, la responsabilidad solo es posible
                    a partir del momento en el que se tenga conocimiento de una infracción
                    específica. Si tuviéramos conocimiento de tal infracción, eliminaremos
                    inmediatamente dicho contenido.
                </p>

                <p><strong>Responsabilidad por enlaces</strong></p>
                <p>
                    Nuestra oferta incluye enlaces a sitios web de terceros, sobre cuyos contenidos
                    no tenemos influencia. Por tanto, no podemos asumir ninguna responsabilidad
                    por dichos contenidos ajenos. El responsable del contenido de las páginas
                    enlazadas es siempre el proveedor u operador respectivo. Las páginas enlazadas
                    se revisaron en el momento de establecer el enlace para detectar posibles
                    infracciones legales. No se observaron contenidos ilegales en ese momento.
                    No obstante, no es razonable llevar a cabo una supervisión permanente de
                    los contenidos de las páginas enlazadas sin indicios concretos de una
                    infracción legal. Si tenemos conocimiento de una infracción legal, eliminaremos
                    dichos enlaces de inmediato.
                </p>

                <p><strong>Protección de datos</strong></p>
                <p>
                    El uso de nuestro sitio web puede ser posible en parte sin proporcionar
                    datos personales. En la medida en que se recaben datos personales (por
                    ejemplo, nombre, dirección o direcciones de correo electrónico) en nuestras
                    páginas, esto se realiza, en lo posible, siempre de forma voluntaria.
                    Estos datos no se transmitirán a terceros sin su consentimiento expreso.
                </p>
                <p>
                    Advertimos que la transmisión de datos en Internet (por ejemplo, en la
                    comunicación por correo electrónico) puede presentar brechas de seguridad.
                    No es posible proteger completamente los datos ante el acceso de terceros.
                </p>
                <p>
                    Se rechaza expresamente el uso por parte de terceros de los datos de contacto
                    publicados en virtud de la obligación de aviso legal para el envío de
                    publicidad e información no solicitadas expresamente. Los operadores de
                    las páginas se reservan expresamente el derecho a emprender acciones legales
                    en caso de envío no solicitado de información publicitaria, por ejemplo,
                    mediante correos spam.
                </p>

                <p id="under16"><strong>Usuarios menores de 16 años</strong></p>
                <p>
                    Si tienes menos de 16 años, solo puedes registrarte en memoWikis con
                    el consentimiento de tus padres.
                </p>
                <p>
                    Dicho consentimiento puede realizarse por correo electrónico o por teléfono.
                    Aquí tus padres pueden ponerse en contacto con nosotros:
                    <span class="mailme btn-link" @click="openEmail">{{ config.public.teamMail }}</span>
                </p>
                <p>
                    Por supuesto, puedes usar el sitio de forma anónima, es decir,
                    sin registrarte.
                </p>

                <p>
                    (Una) fuente: <i>
                        <NuxtLink
                            to="http://www.e-recht24.de/muster-disclaimer.htm"
                            target="_blank"
                            :external="true">
                            Disclaimer
                        </NuxtLink> de
                        eRecht24, el portal de derecho en Internet de
                        <NuxtLink
                            to="http://www.e-recht24.de/"
                            target="_blank"
                            :external="true">
                            el abogado
                        </NuxtLink>
                        Sören Siebert.
                    </i>
                </p>

                <br />

                <h1>Declaración de privacidad</h1>

                <h2>Protección de datos</h2>
                <p>
                    Hemos redactado esta declaración de protección de datos (versión 15.11.2019-311128432)
                    para informarle, de conformidad con las disposiciones del
                    <NuxtLink
                        :external="true"
                        to="https://eur-lex.europa.eu/legal-content/ES/TXT/?uri=CELEX%3A32016R0679&tid=311128432">
                        Reglamento General de Protección de Datos (UE) 2016/679
                    </NuxtLink>,
                    sobre qué información recopilamos, cómo usamos los datos
                    y qué opciones de decisión tiene como visitante de este sitio web.
                </p>
                <p>
                    Lamentablemente, por la naturaleza misma del asunto, estas explicaciones pueden
                    parecer muy técnicas. Sin embargo, nos hemos esforzado por describir los aspectos
                    más importantes de la forma más simple y clara posible.
                </p>

                <h2>Almacenamiento automático de datos</h2>
                <p>
                    Cuando visita sitios web, incluida esta página, se generan y guardan automáticamente
                    ciertos datos. Lo mismo ocurre con nuestro sitio web.
                </p>
                <p>
                    Cuando visita nuestro sitio web (como lo hace ahora mismo), nuestro servidor web
                    (la computadora donde se aloja este sitio) guarda automáticamente datos tales como:
                </p>

                <ul>
                    <li>La dirección (URL) de la página visitada</li>
                    <li>El navegador y la versión del navegador</li>
                    <li>El sistema operativo utilizado</li>
                    <li>La dirección (URL) de la página visitada anteriormente (URL de referencia)</li>
                    <li>El nombre de host y la dirección IP del dispositivo desde el cual se accede</li>
                    <li>Fecha y hora</li>
                </ul>

                <p>en los archivos de registro (Webserver-Logfiles).</p>
                <p>
                    Generalmente, dichos archivos de registro del servidor web se almacenan durante
                    dos semanas y luego se borran automáticamente. No compartimos estos datos con
                    terceros, pero no podemos excluir que estos datos puedan ser examinados en caso
                    de comportamiento ilegal.
                </p>
                <p>
                    La base legal se encuentra en
                    <NuxtLink
                        :external="true"
                        to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432">
                        el art. 6 párr. 1 letra f) del RGPD
                    </NuxtLink>
                    (legitimidad del tratamiento), dado nuestro interés legítimo en permitir el funcionamiento
                    correcto de este sitio mediante el registro de archivos de registro del servidor web.
                </p>

                <h2>Cookies</h2>
                <p>
                    Nuestro sitio web utiliza cookies HTTP para almacenar datos específicos del usuario.
                </p>
                <p>
                    A continuación, explicamos qué son las cookies y por qué se utilizan, para que
                    pueda entender mejor el resto de la declaración de protección de datos.
                </p>

                <h3>¿Qué son exactamente las cookies?</h3>
                <p>
                    Cuando navega por Internet, utiliza un navegador. Navegadores conocidos incluyen
                    Chrome, Safari, Firefox, Internet Explorer o Microsoft Edge. La mayoría de los
                    sitios web guardan pequeños archivos de texto en su navegador, llamados cookies.
                </p>
                <p>
                    Es innegable que las cookies pueden ser herramientas muy útiles. Casi todos los
                    sitios web utilizan cookies. Técnicamente, son llamadas cookies HTTP, ya que
                    existen otros tipos para otros fines. Las cookies HTTP son pequeños archivos
                    que nuestro sitio web almacena en su ordenador. Estos archivos se guardan
                    automáticamente en la carpeta de cookies de su navegador. Una cookie consta
                    de un nombre y un valor. Al definir una cookie, también deben especificarse
                    uno o varios atributos.
                </p>
                <p>
                    Las cookies guardan ciertos datos del usuario, como el idioma o la configuración
                    personalizada de la página. Cuando vuelve a visitar nuestro sitio, su navegador
                    envía la información “relacionada con el usuario” a nuestro sitio. Gracias a
                    las cookies, nuestro sitio puede reconocerlo y ofrecerle sus configuraciones
                    habituales. En algunos navegadores, cada cookie tiene su propio archivo, mientras
                    que en otros, como Firefox, todas las cookies se almacenan en un único archivo.
                </p>
                <p>
                    Existen tanto cookies de origen (first-party) como cookies de terceros
                    (third-party). Las de origen las crea nuestro sitio web, mientras que
                    las de terceros las crean sitios web asociados (por ejemplo, Google Analytics).
                    Cada cookie debe evaluarse individualmente, ya que cada una almacena datos
                    diferentes. El tiempo de expiración de una cookie varía desde un par de minutos
                    hasta varios años. Las cookies no son programas de software y no contienen virus,
                    troyanos ni otros “elementos dañinos”. Tampoco pueden acceder a información de su PC.
                </p>

                <p>Un ejemplo de datos que puede contener una cookie es:</p>
                <ul>
                    <li>Nombre: _ga</li>
                    <li>Tiempo de expiración: 2 años</li>
                    <li>Uso: Distinguir a los visitantes del sitio web</li>
                    <li>Valor de ejemplo: GA1.2.1326744211.152311128432</li>
                </ul>

                <p>Un navegador debería soportar los siguientes tamaños mínimos:</p>
                <ul>
                    <li>Una cookie puede contener al menos 4096 bytes</li>
                    <li>Se deben poder guardar al menos 50 cookies por dominio</li>
                    <li>En total, se deben poder almacenar al menos 3000 cookies</li>
                </ul>

                <h3>¿Qué tipos de cookies hay?</h3>
                <p>
                    Cuáles cookies utilizamos en concreto depende de los servicios que usemos, y se
                    explica en la presente declaración de privacidad. Aquí proporcionamos una breve
                    descripción de los distintos tipos de cookies HTTP.
                </p>
                <p>Podemos distinguir 4 tipos de cookies:</p>

                <p><strong>Cookies imprescindibles</strong></p>
                <p>
                    Estas cookies son necesarias para que el sitio web funcione de manera básica.
                    Por ejemplo, son esenciales cuando un usuario añade un producto al carrito y
                    luego visita otras páginas y, más tarde, pasa por caja. Gracias a estas cookies,
                    el carrito no se elimina, incluso si el usuario cierra la ventana del navegador.
                </p>

                <p><strong>Cookies funcionales</strong></p>
                <p>
                    Estas cookies recopilan información sobre el comportamiento del usuario y
                    sobre posibles mensajes de error. Además, se utilizan para medir los tiempos de
                    carga y el comportamiento del sitio en diferentes navegadores.
                </p>

                <p><strong>Cookies orientadas a objetivos</strong></p>
                <p>
                    Estas cookies proporcionan una mejor experiencia de usuario, por ejemplo
                    guardando ubicaciones, tamaños de fuente o datos de formularios.
                </p>

                <p><strong>Cookies de publicidad</strong></p>
                <p>
                    También conocidas como “cookies de orientación” (targeting). Se utilizan para
                    mostrar publicidad personalizada al usuario, lo que puede ser muy práctico
                    (o muy molesto).
                </p>

                <p>
                    Generalmente, al visitar un sitio web por primera vez, se le pregunta qué
                    tipos de cookies desea permitir. Naturalmente, esta decisión se guardará en
                    una cookie.
                </p>

                <h3>¿Cómo puedo borrar las cookies?</h3>
                <p>
                    Usted decide si desea usar o no las cookies. Independientemente del servicio o
                    sitio web del que provengan, siempre tiene la opción de eliminarlas, permitirlas
                    solo parcialmente o deshabilitarlas. Por ejemplo, puede bloquear las cookies
                    de terceros y permitir las de origen.
                </p>
                <p>
                    Si desea saber qué cookies están almacenadas en su navegador, si quiere cambiar
                    la configuración de las cookies o eliminarlas, puede encontrar todo en la
                    configuración de su navegador:
                </p>
                <p>
                    <NuxtLink
                        :external="true"
                        to="https://support.google.com/chrome/answer/95647?tid=311128432">
                        Chrome: Eliminar, habilitar y administrar cookies en Chrome
                    </NuxtLink>
                </p>
                <p>
                    <NuxtLink
                        :external="true"
                        to="https://support.apple.com/es-es/guide/safari/sfri11471/mac?tid=311128432">
                        Safari: Administrar cookies y datos de sitios web en Safari
                    </NuxtLink>
                </p>
                <p>
                    <NuxtLink
                        :external="true"
                        to="https://support.mozilla.org/es/kb/Borrar%20cookies?tid=311128432">
                        Firefox: Eliminar cookies para eliminar los datos que sitios web han guardado en tu computadora
                    </NuxtLink>
                </p>
                <p>
                    <NuxtLink
                        :external="true"
                        to="https://support.microsoft.com/es-es/help/17442/windows-internet-explorer-delete-manage-cookies?tid=311128432">
                        Internet Explorer: Eliminar y administrar cookies
                    </NuxtLink>
                </p>
                <p>
                    <NuxtLink
                        :external="true"
                        to="https://support.microsoft.com/es-es/help/4027947/windows-delete-cookies?tid=311128432">
                        Microsoft Edge: Eliminar y administrar cookies
                    </NuxtLink>
                </p>
                <p>
                    Si de manera general no desea cookies, puede configurar su navegador de forma
                    que le informe cada vez que se deba establecer una cookie. Así puede decidir
                    individualmente si desea permitir cada cookie. El procedimiento varía según el
                    navegador. Lo mejor es buscar las instrucciones en Google con, por ejemplo,
                    “eliminar cookies en Chrome” o “deshabilitar cookies en Chrome”, y sustituir
                    la palabra “Chrome” por Edge, Firefox, Safari, etc., si utiliza otro navegador.
                </p>

                <h3>¿Qué pasa con mi privacidad?</h3>
                <p>
                    Desde 2009 existen las llamadas “directivas sobre cookies”, que requieren el
                    consentimiento del visitante del sitio web (es decir, de usted) para el
                    almacenamiento de cookies. Sin embargo, en los países de la UE todavía hay
                    enfoques muy diferentes sobre esta normativa. En Alemania, dicha directiva
                    no se ha convertido en ley nacional, sino que se ha implementado en gran medida
                    mediante el § 15 párr. 3 de la Ley de Telemedios (TMG).
                </p>
                <p>
                    Si desea obtener más información sobre las cookies y no teme la documentación
                    técnica, le recomendamos:
                    <NuxtLink :external="true" to="https://tools.ietf.org/html/rfc6265">
                        https://tools.ietf.org/html/rfc6265
                    </NuxtLink>
                    (Request for Comments de la IETF sobre “HTTP State Management Mechanism”).
                </p>

                <h2>Almacenamiento de datos personales</h2>
                <p>
                    Los datos personales que nos transmita electrónicamente en este sitio web,
                    como por ejemplo nombre, dirección de correo electrónico, dirección postal
                    o cualquier otra información personal proporcionada a través de un formulario
                    o en los comentarios del blog, se utilizarán únicamente con el fin indicado,
                    se guardarán de forma segura y no se transmitirán a terceros.
                </p>
                <p>
                    Así pues, solo utilizaremos sus datos personales para comunicarnos con los
                    visitantes que soliciten expresamente contacto y para procesar los servicios
                    y productos ofrecidos en este sitio web. No compartiremos sus datos personales
                    sin su consentimiento, aunque no podemos descartar que dichos datos
                    se consulten en caso de comportamiento ilegal.
                </p>
                <p>
                    Si nos envía datos personales por correo electrónico –es decir, fuera de este
                    sitio–, no podemos garantizar una transmisión completamente segura ni la
                    protección de sus datos. Por ese motivo, le recomendamos no enviar nunca
                    datos confidenciales sin cifrar por correo electrónico.
                </p>
                <p>
                    La base legal se establece en
                    <NuxtLink
                        :external="true"
                        to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432">
                        el art. 6 párr. 1 letra a) del RGPD
                    </NuxtLink>
                    (legitimidad del tratamiento), dado que nos otorga su consentimiento para procesar
                    los datos que introduzca. Puede revocar este consentimiento en cualquier momento;
                    basta con enviarnos un correo electrónico informal. Encontrará nuestros datos de
                    contacto en el aviso legal.
                </p>

                <h2>Derechos según el Reglamento General de Protección de Datos</h2>
                <p>
                    De acuerdo con el RGPD, en general usted tiene los siguientes derechos:
                </p>
                <ul>
                    <li>
                        Derecho a la rectificación (
                        <NuxtLink
                            :external="true"
                            to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432#d1e2803-1-1">
                            artículo 16 RGPD
                        </NuxtLink>)
                    </li>
                    <li>
                        Derecho a la supresión (“derecho al olvido”) (
                        <NuxtLink
                            :external="true"
                            to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432#d1e2865-1-1">
                            artículo 17 RGPD
                        </NuxtLink>)
                    </li>
                    <li>
                        Derecho a la limitación del tratamiento (
                        <NuxtLink
                            :external="true"
                            to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432#d1e2898-1-1">
                            artículo 18 RGPD
                        </NuxtLink>)
                    </li>
                    <li>
                        Derecho de notificación en relación con la rectificación o supresión
                        de datos personales o la limitación del tratamiento (
                        <NuxtLink
                            :external="true"
                            to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432#d1e2941-1-1">
                            artículo 19 RGPD
                        </NuxtLink>)
                    </li>
                    <li>
                        Derecho a la portabilidad de los datos (
                        <NuxtLink
                            :external="true"
                            to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432#d1e2958-1-1">
                            artículo 20 RGPD
                        </NuxtLink>)
                    </li>
                    <li>
                        Derecho de oposición (
                        <NuxtLink
                            :external="true"
                            to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432#d1e2987-1-1">
                            artículo 21 RGPD
                        </NuxtLink>)
                    </li>
                    <li>
                        Derecho a no ser objeto de una decisión basada únicamente en el tratamiento
                        automatizado, incluida la elaboración de perfiles (
                        <NuxtLink
                            :external="true"
                            to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432#d1e3019-1-1">
                            artículo 22 RGPD
                        </NuxtLink>)
                    </li>
                </ul>
                <p>
                    Si considera que el tratamiento de sus datos infringe la ley de protección
                    de datos o que se han vulnerado sus derechos de protección de datos de otra
                    manera, puede dirigirse al
                    <NuxtLink
                        :external="true"
                        to="https://www.bfdi.bund.de/EN/Buerger/Inhalte/Allgemein/Datenschutz/BeschwerdeBeiDatenschutzbehoerden.html?tid=311128432">
                        Comisionado/a Federal de Protección de Datos y Libertad de Información (BfDI)
                    </NuxtLink>.
                </p>

                <h2>Análisis del comportamiento de los visitantes</h2>
                <p>
                    A continuación, en esta declaración de privacidad le informamos si
                    y cómo evaluamos los datos de su visita a este sitio. Generalmente, los datos
                    recopilados se analizan de forma anónima y no podemos sacar conclusiones sobre
                    su persona a partir de su comportamiento en este sitio web.
                </p>
                <p>
                    Puede encontrar más información sobre las opciones para oponerse
                    a este análisis en la presente declaración de privacidad.
                </p>

                <p>&nbsp;</p>

                <h3>5 Matomo (anteriormente “Piwik”)</h3>
                <p><strong>5.1 Alcance y descripción del tratamiento de datos personales</strong></p>
                <p>
                    Nuestro sitio web utiliza “Matomo” (anteriormente “Piwik”), un servicio de
                    análisis web de la empresa InnoCraft Ltd., 150 Willis St, 6011 Wellington,
                    Nueva Zelanda. Matomo almacena cookies en su dispositivo que permiten analizar
                    cómo utiliza nuestro sitio web. La información recopilada de este modo se
                    guarda exclusivamente en nuestro servidor, concretamente los siguientes datos:
                </p>
                <p>- Dos bytes de la dirección IP del sistema que realiza la consulta</p>
                <p>- La página web visitada</p>
                <p>- El sitio desde el cual el usuario accedió a la página visitada (referrer)</p>
                <p>- Las subpáginas a las que se accede desde la página visitada</p>
                <p>- El tiempo de permanencia en la página web</p>
                <p>- La frecuencia de las visitas a la página web</p>
                <p>
                    Nuestro sitio web utiliza Matomo con la función “Anonymize Visitors’ IP addresses”.
                    Por lo tanto, las direcciones IP se procesan de forma abreviada, impidiendo que
                    se establezca un vínculo directo con ninguna persona. El software está configurado
                    para que las direcciones IP no se almacenen por completo, sino que 2 bytes de
                    la IP se enmascaren (p. ej. 192.168.xxx.xxx). De esta manera, no es posible
                    relacionar la dirección IP abreviada con el dispositivo que realiza la consulta.
                    La dirección IP que su navegador transmite a través de Matomo no se fusionará
                    con otros datos que recopilemos.
                </p>

                <p><strong>5.2 Base legal para el tratamiento de los datos personales</strong></p>
                <p>
                    La base legal para el tratamiento de los datos del usuario es
                    <NuxtLink
                        :external="true"
                        to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432">
                        el art. 6 párr. 1 letra f) del RGPD
                    </NuxtLink>
                    y/o el § 15 párr. 3 TMG.
                </p>

                <p><strong>5.3 Finalidad del tratamiento</strong></p>
                <p>
                    Con Matomo analizamos el uso de nuestro sitio y de cada una de sus funciones
                    para mejorar continuamente la experiencia del usuario. Con la evaluación
                    estadística del comportamiento del usuario, mejoramos nuestra oferta y la hacemos
                    más interesante para los visitantes.
                </p>

                <p><strong>5.4 Duración del almacenamiento</strong></p>
                <p>
                    Los datos recopilados en este contexto se eliminarán tras un periodo de
                    almacenamiento de 90 días.
                </p>

                <p><strong>5.5 Derecho de oposición y posibilidad de eliminación</strong></p>
                <p>
                    Puede evitar este análisis eliminando las cookies existentes y deshabilitando el
                    almacenamiento de cookies en la configuración de su navegador web. Tenga en cuenta
                    que, en ese caso, es posible que no pueda usar todas las funciones de este sitio
                    web de forma completa.
                </p>
                <p>
                    Matomo es un proyecto de código abierto de InnoCraft Ltd., 150 Willis St, 6011
                    Wellington, Nueva Zelanda.
                </p>
                <p>
                    Para más información sobre la protección de datos, consulte la política de
                    privacidad en:
                    <NuxtLink :external="true" to="https://matomo.org/privacy-policy/">
                        matomo.org/privacy-policy/
                    </NuxtLink>
                </p>

                <p>&nbsp;</p>

                <h2>Cifrado TLS con https</h2>
                <p>
                    Usamos https para transmitir datos de forma segura por Internet (<em>protección
                        de datos mediante configuración técnica</em>,
                    <NuxtLink
                        :external="true"
                        to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432">
                        art. 25 párr.1 RGPD
                    </NuxtLink>).
                    Con el uso de TLS (Transport Layer Security), un protocolo de cifrado para la
                    transmisión segura de datos en Internet, podemos garantizar la protección
                    de datos confidenciales. Reconocerá el uso de esta protección en la transmisión
                    de datos al ver el pequeño icono de candado en la esquina superior izquierda
                    de su navegador y la utilización del esquema https (en lugar de http) como parte
                    de nuestra dirección web.
                </p>

                <h2>Newsletter – declaración de privacidad</h2>
                <p>
                    Si se suscribe a nuestro boletín (newsletter), nos transmite los datos personales
                    mencionados anteriormente y nos concede el derecho de ponernos en contacto con
                    usted por correo electrónico. Los datos guardados en el proceso de suscripción
                    al boletín se utilizan exclusivamente para nuestro boletín y no se comparten
                    con terceros.
                </p>
                <p>
                    Si desea darse de baja del boletín, encontrará el enlace para hacerlo
                    al final de cada boletín. Entonces se eliminarán todos los datos que
                    se guardaron al suscribirse.
                </p>

                <h2>Google Fonts local – declaración de privacidad</h2>
                <p>
                    En nuestro sitio web utilizamos Google Fonts de Google Inc. (1600 Amphitheatre
                    Parkway, Mountain View, CA 94043, EE.UU.). Hemos integrado las fuentes de
                    Google localmente en nuestro servidor web, no en los servidores de Google.
                    De esta manera, no hay conexión con los servidores de Google y, por lo tanto,
                    no se produce ninguna transferencia ni almacenamiento de datos.
                </p>

                <h3>¿Qué son Google Fonts?</h3>
                <p>
                    Google Fonts (antes Google Web Fonts) es un directorio interactivo con más de 800
                    tipografías que
                    <NuxtLink
                        :external="true"
                        to="https://es.wikipedia.org/w/index.php?title=Google&tid=311128432">
                        Google LLC
                    </NuxtLink>
                    pone a disposición de forma gratuita. Con Google Fonts uno podría usar las fuentes
                    sin tener que subirlas a su propio servidor. Sin embargo, para impedir cualquier
                    comunicación con los servidores de Google, hemos descargado las fuentes en nuestro
                    servidor, garantizando así que no se envíen datos a Google Fonts.
                </p>
                <p>
                    A diferencia de otras fuentes web, Google nos permite el acceso ilimitado a todas
                    sus tipografías. Podemos, por tanto, acceder sin restricciones a una inmensa
                    variedad y así optimizar nuestro sitio. Para más información sobre Google Fonts
                    y otras cuestiones, visite
                    <NuxtLink :external="true" to="https://developers.google.com/fonts/faq?tid=311128432">
                        https://developers.google.com/fonts/faq?tid=311128432
                    </NuxtLink>.
                </p>

                <h2>Google Fonts – declaración de privacidad</h2>
                <p>
                    En nuestro sitio web también utilizamos Google Fonts de Google Inc. (1600
                    Amphitheatre Parkway, Mountain View, CA 94043, EE.UU.).
                </p>
                <p>
                    Para utilizar estas tipografías de Google no necesita registrarse ni establecer
                    una contraseña. Tampoco se almacenan cookies en su navegador. Los archivos (CSS
                    y tipografías/Fonts) se solicitan a través de los dominios fonts.googleapis.com
                    y fonts.gstatic.com. Según Google, estas solicitudes de CSS y fuentes están
                    completamente separadas de otros servicios de Google. Si tiene una cuenta de Google,
                    no debe preocuparse de que se transmitan datos de su cuenta de Google mientras
                    use Google Fonts. Google recopila la utilización de CSS (Hojas de estilo en cascada)
                    y de las tipografías empleadas y almacena estos datos de manera segura. A
                    continuación examinaremos en detalle cómo es ese almacenamiento de datos.
                </p>

                <h3>¿Qué son Google Fonts?</h3>
                <p>
                    Google Fonts (antes Google Web Fonts) es un directorio interactivo con más de 800
                    tipografías que
                    <NuxtLink
                        :external="true"
                        to="https://de.wikipedia.org/wiki/Google_LLC?tid=311128432">
                        Google LLC
                    </NuxtLink>
                    pone a disposición de forma gratuita.
                </p>
                <p>
                    Muchas de estas fuentes están bajo la licencia SIL Open Font o la licencia Apache.
                    Ambas son licencias de software libre, por lo que podemos usarlas sin pagar
                    royalties.
                </p>

                <h3>¿Por qué utilizamos Google Fonts en nuestro sitio web?</h3>
                <p>
                    Con Google Fonts podemos integrar las fuentes en nuestro sitio sin
                    tener que subirlas a nuestro propio servidor. Google Fonts es un componente
                    clave para mantener la calidad de nuestro sitio. Todas las fuentes de Google
                    están optimizadas para la web, lo que ahorra volumen de datos y es especialmente
                    ventajoso para el uso en dispositivos móviles. Cuando visita nuestra página,
                    el pequeño tamaño de archivo garantiza un tiempo de carga rápido. Además,
                    Google Fonts es seguro y confiable a nivel multiplataforma. Es compatible
                    con la mayoría de navegadores (Google Chrome, Mozilla Firefox, Apple Safari,
                    Opera) y funciona sin problemas en los principales sistemas operativos móviles,
                    incluidos Android 2.2+ e iOS 4.2+ (iPhone, iPad, iPod).
                </p>
                <p>
                    Utilizamos Google Fonts para mostrar nuestro contenido en línea de manera bonita
                    y uniforme. Según
                    <NuxtLink
                        :external="true"
                        to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432">
                        el art. 6 párr. 1 letra f) del RGPD
                    </NuxtLink>,
                    esto constituye un “interés legítimo” en el tratamiento de datos personales. Se
                    entiende por “interés legítimo” en este contexto tanto los intereses legales
                    como económicos o ideológicos reconocidos por el sistema jurídico.
                </p>

                <h3>¿Qué datos almacena Google?</h3>
                <p>
                    Cuando visita nuestro sitio web, las fuentes se cargan desde un servidor de
                    Google. A través de esta solicitud externa, se transmiten datos a los servidores
                    de Google. Así, Google reconoce que su IP ha visitado nuestro sitio. Según
                    Google, la API de Google Fonts está diseñada para reducir la recopilación,
                    el almacenamiento y el uso de datos del usuario final a lo necesario para
                    proporcionar eficientemente las fuentes.
                </p>
                <p>
                    Google Fonts guarda con seguridad las solicitudes de CSS y fuentes en los
                    servidores de Google. Mediante las estadísticas de uso, Google sabe la
                    popularidad de cada fuente. Los resultados se publican en páginas de análisis
                    internas, por ejemplo, Google Analytics. Además, Google utiliza los datos
                    de su propio rastreador web (web-crawler) para determinar qué sitios web
                    usan las Google Fonts. Estos datos se publican en la base de datos de
                    BigQuery de Google Fonts. BigQuery es un servicio web de Google para empresas
                    que deseen mover y analizar grandes volúmenes de datos.
                </p>
                <p>
                    Tenga en cuenta que cada vez que solicita Google Fonts, también se transmiten
                    datos como la dirección IP, la configuración de idioma, la resolución de la
                    pantalla, la versión del navegador y el nombre del navegador a los servidores
                    de Google. No está claro si estos datos también se almacenan, pues Google no
                    lo comunica de forma precisa.
                </p>

                <h3>¿Cuánto tiempo y dónde se almacenan los datos?</h3>
                <p>
                    Las solicitudes de activos de CSS se almacenan durante un día en los servidores
                    de Google, que suelen estar fuera de la UE. Esto nos permite usar un
                    Google-Stylesheet para las fuentes. Los archivos de fuentes se almacenan
                    en Google durante un año. El objetivo de Google es mejorar los tiempos
                    de carga de las páginas web. Cuando millones de páginas se refieren a las
                    mismas fuentes, se guardan tras la primera visita y vuelven a aparecer
                    inmediatamente en todas las demás páginas web que se visiten más tarde.
                    A veces, Google actualiza las tipografías para reducir el tamaño de archivo
                    y ampliar el soporte a idiomas o mejorar el diseño.
                </p>

                <h3>¿Cómo puedo borrar mis datos o evitar que se almacenen?</h3>
                <p>
                    Los datos que Google almacena por un día o por un año no pueden borrarse
                    simplemente. Se transmiten de forma automática al visitar la página. Para
                    borrarlos antes, deberá contactar al soporte de Google en:
                    <NuxtLink
                        :external="true"
                        to="https://support.google.com/?hl=es&amp;tid=311128432">
                        https://support.google.com/?hl=es
                    </NuxtLink>.
                    En este caso, la única forma de evitar el almacenamiento de datos es
                    no visitar nuestro sitio.
                </p>
                <p>
                    A diferencia de otras tipografías web, Google nos permite un acceso ilimitado
                    a todas las fuentes, lo que nos ayuda a obtener lo mejor para nuestro sitio.
                    Para más información sobre Google Fonts y otras consultas, visite:
                    <NuxtLink :external="true" to="https://developers.google.com/fonts/faq?tid=311128432">
                        https://developers.google.com/fonts/faq?tid=311128432
                    </NuxtLink>.
                    Allí, Google aborda aspectos de privacidad, pero no ofrece detalles
                    muy específicos sobre el almacenamiento de datos. Es relativamente difícil
                    (casi imposible) obtener de Google información realmente detallada.
                </p>
                <p>
                    Puede consultar qué datos recopila Google en general y con qué fines los usa
                    en
                    <NuxtLink
                        :external="true"
                        to="https://policies.google.com/privacy?hl=es&amp;tid=311128432">
                        https://policies.google.com/privacy?hl=es
                    </NuxtLink>.
                </p>

                <h2>YouTube – declaración de privacidad</h2>
                <p>
                    En nuestro sitio web hemos integrado videos de YouTube para mostrarle contenido
                    interesante directamente en nuestras páginas. YouTube es un portal de videos,
                    filial de Google LLC desde 2006. Está operado por YouTube, LLC, 901 Cherry Ave.,
                    San Bruno, CA 94066, EE.UU. Cuando visita una de nuestras páginas con un
                    video de YouTube incrustado, su navegador se conecta automáticamente con
                    los servidores de YouTube o Google. Dependiendo de la configuración, se
                    transmiten diversos datos. Google es responsable de todo el tratamiento
                    de datos, por lo que se aplica la política de privacidad de Google.
                </p>
                <p>
                    A continuación, explicamos con más detalle qué datos se procesan, por qué
                    hemos integrado videos de YouTube y cómo puede administrar o borrar sus datos.
                </p>

                <h3>¿Qué es YouTube?</h3>
                <p>
                    En YouTube, los usuarios pueden subir, ver, calificar y comentar videos de
                    forma gratuita. En los últimos años, YouTube se ha convertido en una de las
                    plataformas de redes sociales más importantes del mundo. Para mostrarle videos
                    en nuestro sitio, YouTube proporciona un fragmento de código que hemos
                    incrustado en nuestras páginas.
                </p>

                <h3>¿Por qué utilizamos videos de YouTube en nuestro sitio web?</h3>
                <p>
                    YouTube es la plataforma de videos más visitada y con el mejor contenido.
                    Queremos ofrecerle la mejor experiencia de usuario posible en nuestro sitio y
                    el contenido en video no puede faltar. Con los videos incrustados, le
                    proporcionamos contenido adicional valioso además de nuestros textos e
                    imágenes. Además, nuestro sitio se vuelve más fácil de encontrar en
                    la búsqueda de Google. Aunque utilicemos Google Ads para publicidad,
                    Google puede, gracias a los datos recopilados, mostrar esos anuncios
                    solo a personas interesadas en nuestras ofertas.
                </p>

                <h3>¿Qué datos almacena YouTube?</h3>
                <p>
                    En cuanto visita una de nuestras páginas con un video de YouTube incrustado,
                    YouTube establece al menos una cookie que registra su dirección IP
                    y nuestra URL. Si está conectado en su cuenta de YouTube, esta plataforma
                    puede asociar sus interacciones en nuestra web con su perfil, por ejemplo,
                    duración de la sesión, tasa de rebote, ubicación aproximada, datos técnicos
                    como tipo de navegador, resolución de pantalla o su proveedor de Internet.
                    Pueden recopilarse también datos de contacto, posibles valoraciones,
                    compartir contenidos en redes sociales o añadir a favoritos en YouTube.
                </p>
                <p>
                    Si no tiene una cuenta de Google o no ha iniciado sesión, Google almacena
                    datos con una ID única asociada a su dispositivo, navegador o app. Esto
                    permite retener su configuración de idioma, por ejemplo, aunque no se
                    pueden guardar tantas interacciones como en el caso de los usuarios
                    conectados a su cuenta.
                </p>
                <p>
                    A continuación se muestran algunas cookies que se establecieron en una
                    prueba de navegación. Algunas se colocan sin estar conectado en YouTube,
                    otras se establecen con la sesión iniciada. Esta lista no pretende ser
                    exhaustiva, ya que el usuario puede variar su comportamiento:
                </p>
                <ul>
                    <li>Nombre: YSC</li>
                    <li>Valor: b9-CV6ojI5Y</li>
                    <li>Uso: Registrar una ID única para almacenar estadísticas del video visto.</li>
                    <li>Expira: al finalizar la sesión</li>
                </ul>
                <ul>
                    <li>Nombre: PREF</li>
                    <li>Valor: f1=50000000</li>
                </ul>
                <p>
                    Uso: Este cookie también registra su ID única. Google obtiene estadísticas sobre
                    cómo utiliza los videos de YouTube en nuestro sitio a través de PREF.
                </p>
                <p>Expira: tras 8 meses</p>

                <ul>
                    <li>Nombre: GPS</li>
                    <li>Valor: 1</li>
                </ul>
                <p>
                    Uso: Este cookie registra su ID única en dispositivos móviles para rastrear
                    su ubicación GPS.
                </p>
                <p>Expira: tras 30 minutos</p>

                <ul>
                    <li>Nombre: VISITOR_INFO1_LIVE</li>
                    <li>Valor: 31112843295Chz8bagyU</li>
                </ul>
                <p>
                    Uso: Este cookie intenta estimar el ancho de banda del usuario en nuestras páginas
                    que tienen video incrustado de YouTube.
                </p>
                <p>Expira: tras 8 meses</p>

                <p>
                    Otros cookies se configuran si inicia sesión en su cuenta de YouTube. Entre ellos:
                </p>
                <ul>
                    <li>Nombre: APISID</li>
                    <li>Valor: zILlvClZSkqGsSwI/AU1aZI6HY7311128432-</li>
                </ul>
                <p>
                    Uso: Se usa para crear un perfil sobre sus intereses, con fines de publicidad personalizada.
                </p>
                <p>Expira: tras 2 años</p>

                <ul>
                    <li>Nombre: CONSENT</li>
                    <li>Valor: YES+AT.de+20150628-20-0</li>
                </ul>
                <p>
                    Uso: Almacena el estado del consentimiento del usuario para utilizar distintos
                    servicios de Google. CONSENT sirve también para la seguridad, para verificar
                    usuarios y proteger los datos de accesos no autorizados.
                </p>
                <p>Expira: tras 19 años</p>

                <ul>
                    <li>Nombre: HSID</li>
                    <li>Valor: AcRwpgUik9Dveht0I</li>
                </ul>
                <p>
                    Uso: Se utiliza para crear un perfil de sus intereses y poder mostrar
                    publicidad personalizada.
                </p>
                <p>Expira: tras 2 años</p>

                <ul>
                    <li>Nombre: LOGIN_INFO</li>
                    <li>Valor: AFmmF2swRQIhALl6aL…</li>
                </ul>
                <p>
                    Uso: Aquí se guardan datos relacionados con su inicio de sesión.
                </p>
                <p>Expira: tras 2 años</p>

                <ul>
                    <li>Nombre: SAPISID</li>
                    <li>Valor: 7oaPxoG-pZsJuuF5/AnUdDUIsJ9iJz2vdM</li>
                </ul>
                <p>
                    Uso: Identifica de forma única su navegador y dispositivo, para crear un
                    perfil de sus intereses.
                </p>
                <p>Expira: tras 2 años</p>

                <ul>
                    <li>Nombre: SID</li>
                    <li>Valor: oQfNKjAsI311128432-</li>
                </ul>
                <p>
                    Uso: Almacena su ID de cuenta de Google y su última hora de inicio de sesión
                    de forma cifrada.
                </p>
                <p>Expira: tras 2 años</p>

                <ul>
                    <li>Nombre: SIDCC</li>
                    <li>Valor: AN0-TYuqub2JOcDTyL</li>
                </ul>
                <p>
                    Uso: Almacena información sobre cómo utiliza el sitio y qué anuncios ha visto
                    antes de visitar nuestra página.
                </p>
                <p>Expira: tras 3 meses</p>

                <h3>¿Cuánto tiempo y dónde se almacenan los datos?</h3>
                <p>
                    Los datos que YouTube recopila y procesa se guardan en servidores de Google,
                    en su mayoría en EE.UU. Puede ver la ubicación exacta de los centros de datos
                    de Google en:
                    <NuxtLink :external="true" to="https://www.google.com/about/datacenters/inside/locations/?hl=es">
                        https://www.google.com/about/datacenters/inside/locations/?hl=es
                    </NuxtLink>.
                    Sus datos se distribuyen en los servidores, para protegerlos mejor de la
                    manipulación y garantizar un acceso rápido.
                </p>
                <p>
                    Google almacena los datos recopilados durante diferentes lapsos de tiempo. Muchos
                    de ellos puede borrarlos usted mismo, otros se borran automáticamente tras un
                    periodo limitado y otros son guardados por Google a largo plazo. Algunos datos
                    (como elementos de “Mi actividad”, fotos o documentos) almacenados en su cuenta
                    de Google permanecerán hasta que los elimine. Incluso sin iniciar sesión en una
                    cuenta de Google, puede eliminar algunos datos asociados a su dispositivo,
                    navegador o app.
                </p>

                <h3>¿Cómo puedo borrar mis datos o impedir el almacenamiento?</h3>
                <p>
                    En general, puede borrar datos manualmente en su cuenta de Google. La función
                    de borrado automático introducida en 2019 elimina datos de ubicación y
                    actividad según su decisión, a los 3 o 18 meses.
                </p>
                <p>
                    Independientemente de si tiene o no una cuenta de Google, puede configurar
                    su navegador para borrar o desactivar las cookies de Google. Según
                    el navegador que utilice, este procedimiento será diferente. Las siguientes
                    instrucciones muestran cómo gestionar cookies en su navegador:
                </p>
                <p>
                    <NuxtLink
                        :external="true"
                        to="https://support.google.com/chrome/answer/95647?tid=311128432">
                        Chrome: Eliminar, habilitar y administrar cookies
                    </NuxtLink>
                </p>
                <p>
                    <NuxtLink
                        :external="true"
                        to="https://support.apple.com/es-es/guide/safari/sfri11471/mac?tid=311128432">
                        Safari: Administrar cookies y datos de sitios web
                    </NuxtLink>
                </p>
                <p>
                    <NuxtLink
                        :external="true"
                        to="https://support.mozilla.org/es/kb/Borrar%20cookies?tid=311128432">
                        Firefox: Borrar cookies para eliminar datos guardados por sitios
                    </NuxtLink>
                </p>
                <p>
                    <NuxtLink
                        :external="true"
                        to="https://support.microsoft.com/es-es/help/17442/windows-internet-explorer-delete-manage-cookies?tid=311128432">
                        Internet Explorer: Eliminar y administrar cookies
                    </NuxtLink>
                </p>
                <p>
                    <NuxtLink
                        :external="true"
                        to="https://support.microsoft.com/es-es/help/4027947/windows-delete-cookies?tid=311128432">
                        Microsoft Edge: Eliminar y administrar cookies
                    </NuxtLink>
                </p>

                <p>
                    Si no desea cookies en absoluto, puede configurar su navegador para recibir
                    un aviso cada vez que se vaya a establecer una cookie. Así podrá decidir
                    individualmente si permite la cookie o no. Dado que YouTube es una filial
                    de Google, existe una declaración de privacidad común. Para más información
                    sobre cómo Google maneja sus datos, le recomendamos la política de privacidad
                    en
                    <NuxtLink :external="true" to="https://policies.google.com/privacy?hl=es">
                        https://policies.google.com/privacy?hl=es
                    </NuxtLink>.
                </p>

                <h2>Botón de suscripción de YouTube – declaración de privacidad</h2>
                <p>
                    En nuestro sitio web hemos colocado el botón de suscripción de YouTube
                    (en inglés: “Subscribe”). Por lo general, reconocerá este botón con el clásico
                    logotipo de YouTube, a menudo representado en blanco sobre fondo rojo con
                    el texto “Suscribirse” o “YouTube” a la derecha, junto al icono de “Play”
                    en blanco. El diseño también puede variar.
                </p>
                <p>
                    Nuestro canal de YouTube ofrece regularmente videos divertidos, interesantes
                    o emocionantes. Al incrustar este botón de suscripción, puede suscribirse a
                    nuestro canal directamente desde nuestro sitio web, sin tener que visitar
                    la web de YouTube. Así le facilitamos el acceso a nuestro amplio contenido.
                    Tenga en cuenta que de esta manera, YouTube puede almacenar y procesar datos.
                </p>
                <p>
                    Cuando ve nuestro botón de suscripción, YouTube establece –según Google– al
                    menos una cookie. Esta cookie almacena su dirección IP y la URL de nuestra página.
                    También puede recopilar información sobre su navegador, ubicación aproximada y
                    el idioma predefinido. En nuestra prueba, se establecieron las cuatro cookies
                    siguientes sin estar conectados a YouTube:
                </p>
                <ul>
                    <li>Nombre: YSC</li>
                    <li>Valor: b9-CV6ojI5311128432Y</li>
                    <li>
                        Uso: Este cookie registra una ID única para almacenar estadísticas de
                        los videos vistos.
                    </li>
                    <li>Expira: al terminar la sesión</li>
                </ul>
                <ul>
                    <li>Nombre: PREF</li>
                    <li>Valor: f1=50000000</li>
                </ul>
                <p>
                    Uso: También registra su ID única. Google puede obtener a través de PREF
                    estadísticas sobre cómo utiliza los videos de YouTube en nuestro sitio.
                </p>
                <p>Expira: tras 8 meses</p>

                <ul>
                    <li>Nombre: GPS</li>
                    <li>Valor: 1</li>
                </ul>
                <p>
                    Uso: Este cookie registra su ID única en dispositivos móviles para
                    localizarlo vía GPS.
                </p>
                <p>Expira: tras 30 minutos</p>

                <ul>
                    <li>Nombre: VISITOR_INFO1_LIVE</li>
                    <li>Valor: 31112843295Chz8bagyU</li>
                </ul>
                <p>
                    Uso: Este cookie intenta estimar el ancho de banda del usuario en nuestras páginas
                    que tienen video incrustado de YouTube.
                </p>
                <p>Expira: tras 8 meses</p>

                <p>
                    Nota: Estas cookies fueron establecidas tras una prueba y no representan
                    exhaustivamente todas las cookies que se puedan configurar.
                </p>
                <p>
                    Si ha iniciado sesión en su cuenta de YouTube, la plataforma puede relacionar
                    muchas de sus interacciones (clics, tiempo de permanencia, etc.) con su cuenta
                    usando dichas cookies. Así, YouTube puede saber, por ejemplo, cuánto tiempo
                    navega en nuestra página, qué tipo de navegador usa, qué resolución de pantalla
                    prefiere o qué acciones lleva a cabo.
                </p>
                <p>
                    YouTube utiliza estos datos para mejorar sus propias ofertas y servicios y
                    proporcionar análisis y estadísticas a anunciantes que utilicen Google Ads.
                </p>

                <h4>Solicitudes de empleo en línea / Publicación de ofertas de empleo</h4>
                <p>
                    Le ofrecemos la posibilidad de postularse con nosotros a través de nuestra
                    presencia en Internet. En estas solicitudes digitales, sus datos de postulación
                    serán recopilados y procesados electrónicamente para llevar a cabo el proceso
                    de contratación.
                </p>
                <p>
                    La base legal para este tratamiento es § 26 párr. 1 s. 1 BDSG en relación con
                    <NuxtLink
                        :external="true"
                        to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432">
                        el art. 88 párr. 1 RGPD
                    </NuxtLink>.
                </p>
                <p>
                    Si, tras el proceso de solicitud, se establece una relación laboral, guardaremos
                    los datos que nos proporcionó en su solicitud en su expediente personal con el
                    fin de gestionar y organizar el proceso, cumpliendo con nuestras obligaciones
                    legales.
                </p>
                <p>
                    La base legal para este tratamiento es también § 26 párr. 1 s. 1 BDSG en relación
                    con el art. 88 párr. 1 RGPD.
                </p>
                <p>
                    Si se rechaza una solicitud, borraremos automáticamente los datos transmitidos
                    dos meses después de la notificación de la denegación. Sin embargo, la
                    eliminación no tendrá lugar si se necesitan los datos para cumplir con
                    otras obligaciones legales, p. ej. debido a obligaciones de prueba
                    según la AGG, para una retención de hasta cuatro meses o hasta la
                    finalización de un procedimiento judicial.
                </p>
                <p>
                    En este caso, la base legal es
                    <NuxtLink
                        :external="true"
                        to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432">
                        el art. 6 párr. 1 letra f) RGPD
                    </NuxtLink>
                    y § 24 párr. 1 nº 2 BDSG. Nuestro interés legítimo radica en la defensa o el ejercicio
                    de derechos legales.
                </p>
                <p>
                    Si usted da su consentimiento expreso para que se conserven sus datos
                    (por ejemplo, para una base de datos de candidatos o interesados), sus datos
                    se seguirán procesando en base a dicha autorización. En ese caso, la base legal
                    es
                    <NuxtLink
                        :external="true"
                        to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432">
                        el art. 6 párr. 1 letra a) RGPD
                    </NuxtLink>.
                    Por supuesto, puede revocar su consentimiento en cualquier momento con efecto
                    futuro (
                    <NuxtLink
                        :external="true"
                        to="https://eur-lex.europa.eu/legal-content/ES/TXT/HTML/?uri=CELEX:32016R0679&from=ES&tid=311128432">
                        art. 7 párr. 3 RGPD
                    </NuxtLink>
                    ) simplemente comunicándonoslo.
                </p>

                <p>
                    <NuxtLink
                        target="_blank"
                        :external="true"
                        to="https://www.adsimple.de/datenschutz-generator/">
                        Modelo de declaración de protección de datos
                    </NuxtLink>
                    de
                    <NuxtLink target="_blank" :external="true" to="https://www.adsimple.de/">
                        adsimple.de
                    </NuxtLink>
                </p>

                <hr />
                <p style="padding-top: 20px">
                    memoWikis está financiado en el marco del programa EXIST por el Ministerio Federal
                    de Economía y Energía y el Fondo Social Europeo.
                </p>

            </div>
        </div>
    </div>
</template>
