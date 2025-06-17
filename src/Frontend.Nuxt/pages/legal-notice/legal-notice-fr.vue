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
	if (locale.value !== 'fr' && !userStore.isLoggedIn) {
		setLocale('fr')
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
		lang: 'fr'
	},
	link: [
		{
			rel: 'canonical',
			href: `${config.public.officialBase}/MentionsLegales`
		}
	],
	meta: [
		{
			name: 'description',
			content: 'Informations légales et données de l’entreprise pour memoWikis.'
		},
		{
			property: 'og:title',
			content: 'Mentions légales | memoWikis'
		},
		{
			property: 'og:url',
			content: `${config.public.officialBase}/MentionsLegales`
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
			<div class="form-horizontal main-content">

				<h1 class="PageHeader" id="Impressum">Mentions légales &amp; Politique de confidentialité</h1>

				<h2>1. Mentions légales</h2>

				<h3>Informations conformément au § 5 TMG :</h3>
				memucho GmbH<br />
				Am Moorhof <br />
				Nettgendorfer Str. 7 <br />
				14947 Nuthe-Urstromtal<br />

				<h3>Inscription au registre :</h3>
				Registre du Tribunal : Amtsgericht Cottbus, HRB 13499 CB<br />

				<h3>Représentée par :</h3>
				Gérant :<br />
				Robert Mischke<br />

				<h3 id="Kontakt">Contact :</h3>
				Téléphone :<br />
				+49-178 186 68 48<br />
				<span class="mailme btn-link" @click="openEmail">{{ config.public.teamMail }}</span>

				<h3>Responsable du contenu selon le § 55 Abs. 2 RStV :</h3>
				<p>Robert Mischke</p>

				<h3>Clause de non-responsabilité</h3>
				<p><strong>Responsabilité pour les contenus</strong></p>
				<p>
					Le contenu de nos pages a été élaboré avec le plus grand soin.
					Toutefois, nous ne pouvons garantir l’exactitude, l’exhaustivité
					ni l’actualité de ces contenus. En tant que prestataires de services,
					nous sommes responsables de nos propres contenus sur ces pages conformément
					aux dispositions légales générales (§ 7 al. 1 TMG). Toutefois, selon les
					§§ 8 à 10 TMG, nous ne sommes pas tenus, en tant que prestataires de services,
					de surveiller les informations transmises ou stockées provenant de tiers ni
					de rechercher des circonstances pouvant indiquer une activité illégale.
					Les obligations de supprimer ou de bloquer l’utilisation d’informations
					en vertu des lois générales ne sont pas affectées. Une responsabilité
					à cet égard ne peut être engagée qu’à partir du moment où nous avons
					effectivement connaissance d’une violation précise de la loi.
					Dès lors que nous aurions connaissance d’une telle infraction, nous
					supprimerions immédiatement ces contenus.
				</p>

				<p><strong>Responsabilité pour les liens</strong></p>
				<p>
					Notre offre contient des liens vers des sites web externes de tiers
					dont nous ne maîtrisons pas le contenu. Nous ne pouvons donc assumer
					aucune responsabilité quant à ces contenus étrangers. Le responsable
					de ces sites est toujours le fournisseur ou l’exploitant concerné.
					Les pages liées ont été contrôlées au moment de la mise en lien pour
					vérifier l’existence éventuelle de contenus contraires à la loi.
					Aucun contenu illicite n’a été constaté lors de la liaison.
					Un contrôle permanent du contenu des pages liées n’est cependant
					pas réalisable sans indices concrets d’une violation de la loi.
					Si nous avons connaissance de violations, nous supprimerons immédiatement
					ces liens.
				</p>

				<p><strong>Protection des données</strong></p>
				<p>
					L’utilisation de notre site web est en partie possible sans fournir de
					données personnelles. Dans la mesure où des données personnelles (telles que
					le nom, l’adresse ou les adresses e-mail) sont collectées sur nos pages,
					cela se fait, autant que possible, toujours sur une base volontaire. Ces données
					ne sont pas transmises à des tiers sans votre accord explicite.
				</p>
				<p>
					Nous attirons votre attention sur le fait que la transmission de données
					sur Internet (par exemple lors de la communication par e-mail) peut
					présenter des failles de sécurité. Une protection complète des données
					contre l’accès par des tiers n’est pas possible.
				</p>
				<p>
					Nous nous opposons expressément à l’utilisation par des tiers des coordonnées
					publiées dans le cadre de l’obligation des mentions légales à des fins d’envoi
					de publicité et d’informations non sollicitées expressément. Les exploitants
					de ces pages se réservent expressément le droit d’intenter une action en justice
					en cas d’envoi non sollicité d’informations publicitaires, par exemple par
					des spams.
				</p>

				<p id="under16"><strong>Utilisateurs de moins de 16 ans</strong></p>
				<p>
					Si tu as moins de 16 ans, tu ne peux t’inscrire sur memoWikis qu’avec
					le consentement de tes parents.
				</p>
				<p>
					Ce consentement peut nous être transmis par e-mail ou par téléphone.
					Tes parents peuvent nous joindre ici :
					<span class="mailme btn-link" @click="openEmail">{{ config.public.teamMail }}</span>
				</p>
				<p>
					Tu peux bien entendu utiliser le site de façon anonyme, c’est-à-dire
					sans t’inscrire.
				</p>

				<p>
					(Une) source : <i>
						<NuxtLink
							to="http://www.e-recht24.de/muster-disclaimer.htm"
							target="_blank"
							:external="true">
							Disclaimer
						</NuxtLink> de
						eRecht24, le portail de droit internet du
						<NuxtLink
							to="http://www.e-recht24.de/"
							target="_blank"
							:external="true">
							Rechtsanwalt
						</NuxtLink>
						Sören Siebert.
					</i>
				</p>

				<br />

				<h1>Déclaration de confidentialité</h1>

				<h2>Protection des données</h2>
				<p>
					Nous avons rédigé cette déclaration de confidentialité (version du
					15.11.2019-311128432) afin de vous expliquer, conformément aux exigences du
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/FR/TXT/?uri=CELEX%3A32016R0679&tid=311128432">
						Règlement général sur la protection des données (UE) 2016/679
					</NuxtLink>,
					quelles informations nous recueillons, comment nous utilisons ces données et
					quelles possibilités de décision vous avez en tant que visiteur de ce site web.
				</p>
				<p>
					Malheureusement, de par la nature du sujet, ces explications peuvent sembler
					très techniques. Nous nous sommes toutefois efforcés de décrire les points
					essentiels de la manière la plus simple et la plus claire possible.
				</p>

				<h2>Enregistrement automatique des données</h2>
				<p>
					Lorsque vous consultez des sites web – y compris notre site – certaines
					informations sont automatiquement créées et enregistrées. Cela se produit
					aussi avec notre site web.
				</p>
				<p>
					Lorsque vous visitez notre site web, notre serveur web (l’ordinateur sur lequel
					ce site est hébergé) enregistre automatiquement des données telles que :
				</p>

				<ul>
					<li>L’adresse (URL) de la page consultée</li>
					<li>Le navigateur et la version du navigateur</li>
					<li>Le système d’exploitation utilisé</li>
					<li>L’adresse (URL) du site précédemment visité (URL de référence)</li>
					<li>Le nom d’hôte et l’adresse IP de l’appareil depuis lequel l’accès est effectué</li>
					<li>La date et l’heure</li>
				</ul>

				<p>dans des fichiers de type « Logfiles » du serveur web.</p>
				<p>
					En règle générale, ces Logfiles sont conservés pendant deux semaines
					avant d’être supprimés automatiquement. Nous ne transmettons pas ces
					données mais nous ne pouvons exclure qu’elles soient consultées en cas
					de comportement illicite.
				</p>
				<p>
					La base légale découle de
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/FR/TXT/HTML/?uri=CELEX:32016R0679&from=FR&tid=311128432">
						l’article 6, paragraphe 1, point f) du RGPD
					</NuxtLink>
					(licéité du traitement) : nous avons un intérêt légitime à rendre possible
					un fonctionnement correct de ce site web en collectant les Logfiles du serveur web.
				</p>

				<h2>Cookies</h2>
				<p>
					Notre site web utilise des cookies HTTP pour enregistrer des données spécifiques
					à l’utilisateur.
				</p>
				<p>
					Vous trouverez ci-après des explications sur ce que sont les cookies et
					pourquoi ils sont utilisés, afin que vous puissiez mieux comprendre la suite
					de cette déclaration de confidentialité.
				</p>

				<h3>Qu’est-ce qu’un cookie ?</h3>
				<p>
					Lorsque vous naviguez sur Internet, vous utilisez un navigateur.
					Les navigateurs les plus connus sont Chrome, Safari, Firefox, Internet Explorer
					ou Microsoft Edge. La plupart des sites web enregistrent de petits fichiers
					de texte dans votre navigateur, appelés cookies.
				</p>
				<p>
					On ne peut nier l’utilité des cookies : ils sont de véritables petits
					« assistants ». Presque tous les sites web utilisent des cookies.
					Techniquement, il s’agit de cookies HTTP, même s’il existe différents cookies
					pour d’autres utilisations. Les cookies HTTP sont de petits fichiers que
					notre site enregistre sur votre ordinateur. Ils sont automatiquement stockés
					dans le dossier cookies de votre navigateur. Un cookie est composé d’un
					nom et d’une valeur. Lors de la définition d’un cookie, un ou plusieurs attributs
					doivent également être indiqués.
				</p>
				<p>
					Les cookies enregistrent certaines données d’utilisation (par ex. la langue ou
					les paramètres personnels du site). Lorsque vous revisitez notre site, votre
					navigateur renvoie les informations « relatives à l’utilisateur » à notre site.
					Grâce aux cookies, notre site web sait qui vous êtes et vous affiche vos
					paramètres habituels. Dans certains navigateurs, chaque cookie dispose
					d’un fichier propre, dans d’autres (par ex. Firefox), tous les cookies
					sont enregistrés dans un seul fichier.
				</p>
				<p>
					Il existe à la fois des cookies first-party (internes) et des cookies third-party
					(tiers). Les cookies internes sont créés directement par notre site,
					tandis que les cookies tiers sont créés par des sites web partenaires
					(par ex. Google Analytics). Chaque cookie doit être considéré individuellement
					puisqu’il enregistre des données différentes. Leur durée de vie varie d’une
					poignée de minutes à plusieurs années. Les cookies ne sont pas des programmes
					informatiques et ne contiennent pas de virus, chevaux de Troie ou autres
					« nuisibles ». Ils ne peuvent pas non plus accéder aux informations de
					votre PC.
				</p>

				<p>Voici un exemple de données qu’un cookie peut contenir :</p>
				<ul>
					<li>Nom : _ga</li>
					<li>Durée de vie : 2 ans</li>
					<li>Utilisation : distinction des visiteurs du site web</li>
					<li>Valeur : GA1.2.1326744211.152311128432</li>
				</ul>

				<p>Un navigateur devrait supporter les tailles minimales suivantes :</p>
				<ul>
					<li>Un cookie doit pouvoir comporter au moins 4096 bytes</li>
					<li>Au moins 50 cookies doivent pouvoir être enregistrés par domaine</li>
					<li>Au total, au moins 3000 cookies doivent pouvoir être enregistrés</li>
				</ul>

				<h3>Quels types de cookies existe-t-il ?</h3>
				<p>
					Les cookies que nous utilisons concrètement dépendent des services
					que nous utilisons et sont présentés dans la suite de cette déclaration
					de confidentialité. À ce stade, nous allons simplement expliquer brièvement
					les différents types de cookies HTTP.
				</p>
				<p>On peut distinguer 4 types de cookies :</p>

				<p><strong>Cookies strictement nécessaires</strong></p>
				<p>
					Ces cookies sont indispensables pour assurer les fonctions de base du site.
					Par exemple, ces cookies sont nécessaires pour qu’un utilisateur conserve
					son panier, même s’il ferme la fenêtre du navigateur.
				</p>

				<p><strong>Cookies fonctionnels</strong></p>
				<p>
					Ces cookies collectent des informations sur le comportement des utilisateurs
					et sur d’éventuels messages d’erreur. Ils permettent également de mesurer
					les temps de chargement et le comportement du site sous différents navigateurs.
				</p>

				<p><strong>Cookies à finalité</strong></p>
				<p>
					Ces cookies assurent une meilleure convivialité. Ils enregistrent par exemple
					des entrées (localisations, tailles de police, données de formulaire) pour
					simplifier l’utilisation du site.
				</p>

				<p><strong>Cookies publicitaires</strong></p>
				<p>
					Aussi appelés « cookies de ciblage ». Ils servent à proposer des publicités
					personnalisées à l’utilisateur. Cela peut être très pratique, mais aussi
					très gênant.
				</p>

				<p>
					Lors de votre première visite d’un site web, il vous est souvent demandé
					quels cookies vous souhaitez autoriser. Évidemment, ce choix est ensuite
					lui-même enregistré dans un cookie.
				</p>

				<h3>Comment puis-je supprimer les cookies ?</h3>
				<p>
					C’est vous qui décidez si vous voulez utiliser ou non des cookies.
					Quelle que soit la provenance des cookies (notre site ou un autre),
					vous pouvez toujours les supprimer, les autoriser partiellement ou
					les désactiver. Par exemple, vous pouvez bloquer uniquement les cookies
					tiers et autoriser les autres.
				</p>
				<p>
					Si vous souhaitez savoir quels cookies sont enregistrés dans votre navigateur,
					si vous voulez modifier ou supprimer les paramètres des cookies, vous pouvez
					le faire dans les paramètres de votre navigateur :
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.google.com/chrome/answer/95647?tid=311128432">
						Chrome : supprimer, activer et gérer les cookies
					</NuxtLink>
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.apple.com/fr-fr/guide/safari/sfri11471/mac?tid=311128432">
						Safari : gérer les cookies et les données de site web
					</NuxtLink>
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.mozilla.org/fr/kb/effacer-cookies-donnees-site-firefox?tid=311128432">
						Firefox : supprimer les cookies pour supprimer les données que les sites web ont placées sur votre ordinateur
					</NuxtLink>
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.microsoft.com/fr-fr/help/17442/windows-internet-explorer-delete-manage-cookies?tid=311128432">
						Internet Explorer : supprimer et gérer les cookies
					</NuxtLink>
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.microsoft.com/fr-fr/help/4027947/windows-delete-cookies?tid=311128432">
						Microsoft Edge : supprimer et gérer les cookies
					</NuxtLink>
				</p>
				<p>
					Si vous ne souhaitez pas de cookies, vous pouvez configurer votre navigateur
					pour être averti à chaque fois qu’un cookie doit être placé, et décider au
					cas par cas de l’accepter ou non. La procédure varie selon le navigateur.
					Il est souvent plus simple de chercher sur Google, par exemple avec
					« supprimer cookies Chrome » ou « désactiver cookies Chrome »
					(et de remplacer « Chrome » par le nom de votre navigateur, p. ex. Edge,
					Firefox, Safari).
				</p>

				<h3>Qu’en est-il de ma protection des données ?</h3>
				<p>
					Depuis 2009, il existe les « directives relatives aux cookies ». Celles-ci
					stipulent que le stockage de cookies requiert l’accord du visiteur d’un site web.
					Dans l’UE, l’implémentation de cette directive varie selon les pays. En Allemagne,
					elle n’a pas été transposée en droit national, mais principalement
					mise en œuvre dans le § 15 al. 3 du TMG.
				</p>
				<p>
					Pour en savoir plus sur les cookies et si vous n’avez pas peur de la
					documentation technique, nous vous recommandons :
					<NuxtLink :external="true" to="https://tools.ietf.org/html/rfc6265">
						https://tools.ietf.org/html/rfc6265
					</NuxtLink>
					(Request for Comments de l’IETF, intitulé « HTTP State Management Mechanism »).
				</p>

				<h2>Enregistrement de données personnelles</h2>
				<p>
					Les données personnelles que vous nous transmettez par voie électronique
					sur ce site web – par ex. nom, adresse e-mail, adresse, etc. dans le cadre
					d’un formulaire ou de commentaires dans le blog – sont utilisées
					exclusivement aux fins indiquées, stockées en toute sécurité et non transmises
					à des tiers.
				</p>
				<p>
					Nous utilisons donc vos données personnelles uniquement pour communiquer
					avec les visiteurs qui souhaitent expressément être contactés, et pour le
					traitement des services et produits proposés sur ce site. Nous ne transmettons
					pas vos données personnelles sans votre consentement, mais nous ne pouvons
					pas exclure qu’elles soient consultées en cas de comportement illicite.
				</p>
				<p>
					Si vous nous envoyez des données personnelles par e-mail – c’est-à-dire
					en dehors de ce site web –, nous ne pouvons pas garantir une transmission
					entièrement sécurisée ni la protection de vos données. Nous vous recommandons
					donc de ne jamais transmettre de données confidentielles non cryptées par e-mail.
				</p>
				<p>
					La base légale résulte de
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/FR/TXT/HTML/?uri=CELEX:32016R0679&from=FR&tid=311128432">
						l’article 6, paragraphe 1, point a) du RGPD
					</NuxtLink> :
					vous nous donnez votre consentement pour traiter les données que vous
					avez saisies. Vous pouvez à tout moment révoquer ce consentement – un e-mail
					informel suffit. Vous trouverez nos coordonnées dans les mentions légales.
				</p>

				<h2>Droits selon le Règlement général sur la protection des données</h2>
				<p>
					Conformément au RGPD, vous disposez en principe des droits suivants :
				</p>
				<ul>
					<li>Droit de rectification (article 16 RGPD)</li>
					<li>Droit à l’effacement (« droit à l’oubli », article 17 RGPD)</li>
					<li>Droit à la limitation du traitement (article 18 RGPD)</li>
					<li>
						Droit à la notification : obligation de notification en lien avec la
						rectification ou l’effacement de données personnelles ou la limitation
						du traitement (article 19 RGPD)
					</li>
					<li>Droit à la portabilité des données (article 20 RGPD)</li>
					<li>Droit d’opposition (article 21 RGPD)</li>
					<li>
						Droit de ne pas faire l’objet d’une décision fondée exclusivement
						sur un traitement automatisé – y compris le profilage
						(article 22 RGPD)
					</li>
				</ul>
				<p>
					Si vous estimez que le traitement de vos données enfreint la législation
					sur la protection des données, ou que vos droits en la matière ont été violés
					d’une quelconque manière, vous pouvez vous adresser au
					<NuxtLink
						:external="true"
						to="https://www.bfdi.bund.de/EN/Buerger/Inhalte/Allgemein/Datenschutz/BeschwerdeBeiDatenschutzbehoerden.html?tid=311128432">
						Commissaire fédéral à la protection des données et à la liberté
						d’information (BfDI)
					</NuxtLink>.
				</p>

				<h2>Analyse du comportement des visiteurs</h2>
				<p>
					Dans la présente déclaration de confidentialité, nous vous informons
					si et comment nous analysons les données de votre visite de ce site web.
					Les données collectées sont généralement analysées de manière anonyme et
					nous ne pouvons pas déduire votre identité à partir de votre comportement
					sur ce site web.
				</p>
				<p>
					Vous trouverez plus d’informations sur la possibilité de vous opposer
					à cette analyse dans cette déclaration de confidentialité.
				</p>

				<p>&nbsp;</p>

				<h3>5 Matomo (anciennement « Piwik »)</h3>
				<p><strong>5.1 Étendue et description du traitement des données personnelles</strong></p>
				<p>
					Notre site web utilise « Matomo » (anciennement « Piwik »), un service
					d’analyse web de la société InnoCraft Ltd., 150 Willis St, 6011 Wellington,
					Nouvelle-Zélande. Matomo enregistre des cookies sur votre appareil, qui
					permettent une analyse de l’utilisation de notre site par vous. Les
					informations ainsi recueillies sont stockées exclusivement sur notre
					serveur et comprennent notamment :
				</p>
				<p>- deux octets de l’adresse IP du système utilisateur qui consulte le site</p>
				<p>- la page web consultée</p>
				<p>- la page web depuis laquelle l’utilisateur est arrivé sur la page consultée (référant)</p>
				<p>- les sous-pages consultées depuis la page en question</p>
				<p>- la durée de consultation de la page</p>
				<p>- la fréquence de consultation de la page</p>
				<p>
					Notre site web utilise Matomo avec le paramètre « Anonymize Visitors’ IP addresses »,
					de sorte que les adresses IP ne soient traitées que sous forme abrégée,
					excluant tout lien direct avec une personne. Le logiciel est configuré de
					façon à ce que les adresses IP ne soient pas stockées complètement,
					mais que 2 octets de l’adresse IP soient masqués (par ex. 192.168.xxx.xxx).
					Ainsi, il est impossible de relier l’adresse IP abrégée à l’ordinateur
					de l’utilisateur. L’adresse IP transmise par votre navigateur via Matomo
					ne sera pas recoupée avec d’autres données que nous collectons.
				</p>

				<p><strong>5.2 Base juridique du traitement des données personnelles</strong></p>
				<p>
					La base juridique du traitement des données de l’utilisateur est
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/FR/TXT/HTML/?uri=CELEX:32016R0679&from=FR&tid=311128432">
						l’article 6, paragraphe 1, point f) du RGPD
					</NuxtLink>
					ou le § 15, paragraphe 3 TMG.
				</p>

				<p><strong>5.3 Finalité du traitement</strong></p>
				<p>
					Avec Matomo, nous analysons l’utilisation de notre site web et de chacune
					de ses fonctionnalités afin d’améliorer en continu l’expérience de l’utilisateur.
					L’évaluation statistique du comportement de navigation nous permet
					d’optimiser notre offre et de la rendre plus intéressante pour les visiteurs.
				</p>

				<p><strong>5.4 Durée de conservation</strong></p>
				<p>
					Les données relatives à ce traitement sont supprimées après une durée
					de conservation de 90 jours.
				</p>

				<p><strong>5.5 Droit d’opposition et possibilité de suppression</strong></p>
				<p>
					Vous pouvez empêcher l’évaluation en supprimant les cookies existants
					et en désactivant la sauvegarde des cookies dans les paramètres de
					votre navigateur. Nous signalons que, le cas échéant, vous ne pourrez pas
					utiliser toutes les fonctionnalités de ce site web dans leur intégralité.
				</p>
				<p>
					Matomo est un projet open-source d’InnoCraft Ltd., 150 Willis St, 6011
					Wellington, Nouvelle-Zélande.
				</p>
				<p>
					Vous trouverez plus d’informations sur la protection des données
					dans la déclaration de confidentialité à l’adresse :
					<NuxtLink :external="true" to="https://matomo.org/privacy-policy/">
						matomo.org/privacy-policy/
					</NuxtLink>
				</p>

				<p>&nbsp;</p>

				<h2>Chiffrement TLS via https</h2>
				<p>
					Nous utilisons https afin de transmettre nos données de manière sécurisée
					sur Internet (<em>protection des données par la conception technique</em>,
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/FR/TXT/HTML/?uri=CELEX:32016R0679&from=FR&tid=311128432#d1e2349-1-1">
						article 25, paragraphe 1 du RGPD
					</NuxtLink>).
					Grâce à TLS (Transport Layer Security), un protocole de cryptage permettant
					la transmission sécurisée de données sur Internet, nous pouvons assurer
					la protection de données confidentielles. Vous pouvez reconnaître l’utilisation
					de ce dispositif de sécurité au petit symbole de cadenas situé en haut à gauche
					de votre navigateur ainsi qu’à la mention « https » dans notre adresse web
					(au lieu de « http »).
				</p>

				<h2>Newsletter – Déclaration de confidentialité</h2>
				<p>
					Si vous vous inscrivez à notre newsletter, vous nous transmettez
					les données personnelles mentionnées et nous donnez le droit de
					vous contacter par e-mail. Les données enregistrées lors de l’inscription
					à la newsletter sont utilisées exclusivement pour notre newsletter
					et ne sont pas transmises à des tiers.
				</p>
				<p>
					Si vous souhaitez vous désinscrire de la newsletter, vous trouverez
					un lien de désinscription en bas de chaque newsletter.
					Après votre désinscription, toutes les données enregistrées
					lors de l’inscription sont supprimées.
				</p>

				<h2>Google Fonts locales – Déclaration de confidentialité</h2>
				<p>
					Nous utilisons Google Fonts de la société Google Inc.
					(1600 Amphitheatre Parkway, Mountain View, CA 94043, USA)
					sur notre site. Nous avons intégré les polices Google localement sur
					notre serveur – pas sur les serveurs de Google. Cela signifie qu’il
					n’y a aucune connexion aux serveurs de Google et donc aucune transmission
					ni aucun stockage de données.
				</p>

				<h3>Qu’est-ce que Google Fonts ?</h3>
				<p>
					Google Fonts (anciennement Google Web Fonts) est un répertoire
					interactif de plus de 800 polices de caractères mis à disposition
					par
					<NuxtLink :external="true" to="https://fr.wikipedia.org/w/index.php?title=Google&tid=311128432">
						Google LLC
					</NuxtLink>.
					Grâce à Google Fonts, on peut utiliser des polices de caractères
					sans avoir à les télécharger sur son propre serveur. Cependant,
					afin d’éviter toute transmission de données à Google, nous avons
					téléchargé ces polices sur notre propre serveur. Nous agissons ainsi
					conformément aux règles de protection des données et n’envoyons aucune
					donnée à Google Fonts.
				</p>
				<p>
					Contrairement à d’autres polices web, Google nous autorise un accès
					illimité à toutes les polices de caractères. Nous pouvons ainsi exploiter
					pleinement un large éventail de polices et optimiser la présentation
					de notre site. Pour plus d’informations sur Google Fonts et d’autres
					questions, consultez
					<NuxtLink :external="true" to="https://developers.google.com/fonts/faq?tid=311128432">
						https://developers.google.com/fonts/faq?tid=311128432
					</NuxtLink>.
				</p>

				<h2>Google Fonts – Déclaration de confidentialité</h2>
				<p>
					Nous utilisons également Google Fonts de la société Google Inc.
					(1600 Amphitheatre Parkway, Mountain View, CA 94043, USA) sur notre site web.
				</p>
				<p>
					Pour utiliser ces polices Google, vous n’avez pas besoin de vous inscrire
					ni de définir un mot de passe. Aucune cookie n’est non plus enregistrée
					dans votre navigateur. Les fichiers (CSS, polices) sont sollicités via
					les domaines fonts.googleapis.com et fonts.gstatic.com. Selon Google,
					ces requêtes CSS et polices sont complètement séparées de tous les
					autres services Google. Si vous possédez un compte Google, vous n’avez
					pas à craindre que vos données de compte Google soient transmises à Google
					lors de l’utilisation de Google Fonts. Google enregistre l’utilisation
					de CSS et des polices utilisées et stocke ces données en toute sécurité.
					Nous examinerons plus en détail comment sont stockées ces données.
				</p>

				<h3>Qu’est-ce que Google Fonts ?</h3>
				<p>
					Google Fonts (anciennement Google Web Fonts) est un répertoire
					interactif de plus de 800 polices de caractères.
				</p>
				<p>
					Beaucoup de polices sont publiées sous la licence SIL Open Font,
					d’autres sous la licence Apache. Il s’agit de licences libres,
					ce qui nous permet de les utiliser gratuitement.
				</p>

				<h3>Pourquoi utilisons-nous Google Fonts sur notre site web ?</h3>
				<p>
					Google Fonts nous permet d’utiliser des polices de caractères
					sur notre propre site sans avoir à les télécharger sur notre serveur.
					Google Fonts est un élément essentiel pour maintenir la qualité de
					notre site. Toutes les polices Google sont optimisées pour le web,
					ce qui réduit la taille des fichiers et est particulièrement avantageux
					pour les appareils mobiles. Lorsque vous visitez notre site, la taille
					réduite des fichiers permet un temps de chargement rapide. De plus, Google Fonts
					est un service web sûr, qui évite les problèmes de compatibilité
					entre navigateurs et systèmes.
				</p>
				<p>
					Nous utilisons Google Fonts pour afficher notre contenu en ligne
					de manière attrayante et cohérente. Selon
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/FR/TXT/HTML/?uri=CELEX:32016R0679&from=FR&tid=311128432">
						l’article 6, paragraphe 1, point f) du RGPD
					</NuxtLink>,
					cela constitue un « intérêt légitime » au traitement des données personnelles.
					Dans ce contexte, un « intérêt légitime » peut être aussi bien juridique
					qu’économique ou idéologique, pour autant qu’il soit reconnu par le système juridique.
				</p>

				<h3>Quelles données Google enregistre-t-il ?</h3>
				<p>
					Lorsque vous visitez notre site web, les polices sont chargées
					depuis les serveurs de Google. Cette requête externe transfère
					automatiquement des données vers les serveurs de Google, comme votre
					adresse IP ou les paramètres de langue, la résolution de l’écran,
					la version du navigateur et son nom. L’API Google Fonts a été conçue
					pour réduire au minimum la collecte, le stockage et l’utilisation
					des données de l’utilisateur final. Toutefois, Google n’explique
					pas clairement si ces données sont effectivement enregistrées et, si oui, comment.
				</p>
				<p>
					Google Fonts stocke en toute sécurité les requêtes CSS et les polices
					sur ses serveurs et peut ainsi déterminer la popularité des polices
					utilisées. Les résultats de ces analyses sont publiés dans des rapports
					internes, par exemple sur Google Analytics. De plus, Google utilise
					son propre crawler pour déterminer quels sites web utilisent Google Fonts,
					et enregistre ces données dans la base de données BigQuery. BigQuery est
					un service de Google permettant aux entreprises de traiter et d’analyser
					de grandes quantités de données.
				</p>
				<p>
					Notez que chaque requête Google Fonts envoie également des informations
					comme l’adresse IP, les paramètres de langue, la résolution de l’écran,
					la version et le nom du navigateur. On ignore si et comment Google stocke
					ces données, car la société ne communique pas clairement à ce sujet.
				</p>

				<h3>Combien de temps et où les données sont-elles conservées ?</h3>
				<p>
					Les requêtes relatives aux fichiers CSS sont conservées un jour sur
					les serveurs de Google, qui se trouvent principalement en dehors de l’UE.
					Cela nous permet d’utiliser un Google-Stylesheet pour les polices.
					Les fichiers de polices sont stockés sur les serveurs de Google pendant
					un an. Ainsi, Google cherche à améliorer les temps de chargement
					de manière générale. Si des millions de pages renvoient aux mêmes polices,
					elles sont mémorisées après la première visite et s’affichent immédiatement
					sur les autres sites visités ultérieurement. Parfois, Google met à jour
					les polices pour réduire la taille des fichiers, étendre la couverture linguistique
					ou améliorer le design.
				</p>

				<h3>Comment puis-je supprimer mes données ou empêcher leur stockage ?</h3>
				<p>
					Vous ne pouvez pas simplement supprimer les données que Google enregistre
					pendant un jour ou un an. Elles sont transmises automatiquement lors
					de la consultation. Pour les supprimer à l’avance, vous devriez contacter
					le support Google à
					<NuxtLink :external="true" to="https://support.google.com/?hl=fr&amp;tid=311128432">
						https://support.google.com/?hl=fr
					</NuxtLink>.
					Dans ce cas, la seule manière d’éviter la conservation des données
					est de ne pas consulter notre site.
				</p>
				<p>
					Contrairement à d’autres polices web, Google nous autorise un accès
					illimité à l’ensemble de ses polices, ce qui nous permet d’optimiser
					notre site. Pour plus d’informations sur Google Fonts, consultez
					<NuxtLink :external="true" to="https://developers.google.com/fonts/faq?tid=311128432">
						https://developers.google.com/fonts/faq?tid=311128432
					</NuxtLink>.
					Google y aborde des questions relatives à la protection des données,
					mais les informations stockées ne sont pas présentées de manière très détaillée.
					Il est relativement difficile (voire impossible) d’obtenir des informations
					précises de la part de Google.
				</p>
				<p>
					Vous pouvez également consulter
					<NuxtLink
						:external="true"
						to="https://policies.google.com/privacy?hl=fr&amp;tid=311128432">
						https://www.google.com/intl/policies/privacy/
					</NuxtLink>
					pour savoir quelles données Google collecte en général et à quelles fins.
				</p>

				<h2>Politique de confidentialité de YouTube</h2>
				<p>
					Nous avons intégré sur notre site des vidéos YouTube pour vous présenter
					du contenu intéressant directement sur nos pages. YouTube est un portail
					de vidéos devenu filiale de Google LLC en 2006. Il est exploité par YouTube, LLC,
					901 Cherry Ave., San Bruno, CA 94066, USA. Lorsque vous accédez à l’une
					de nos pages qui intègre une vidéo YouTube, votre navigateur se connecte
					automatiquement aux serveurs de YouTube ou de Google. Selon les paramètres,
					différentes données sont transmises. Google est responsable de l’intégralité
					du traitement de ces données, et la politique de confidentialité de Google
					s’applique donc.
				</p>
				<p>
					Nous allons vous expliquer plus en détail quelles données sont traitées,
					pourquoi nous avons intégré des vidéos YouTube et comment vous pouvez
					gérer ou supprimer vos données.
				</p>

				<h3>Qu’est-ce que YouTube ?</h3>
				<p>
					Sur YouTube, les utilisateurs peuvent regarder, évaluer, commenter et
					télécharger des vidéos gratuitement. Au fil des années, YouTube est devenu
					l’une des plateformes de médias sociaux les plus importantes. Afin de vous
					afficher des vidéos directement sur notre site, YouTube met à notre disposition
					un extrait de code que nous avons intégré.
				</p>

				<h3>Pourquoi utilisons-nous des vidéos YouTube sur notre site web ?</h3>
				<p>
					YouTube étant la plateforme de vidéos la plus populaire, avec le meilleur contenu,
					nous souhaitons vous offrir la meilleure expérience utilisateur possible.
					Le contenu vidéo ne saurait manquer pour y parvenir. Grâce aux vidéos intégrées,
					nous mettons à votre disposition un contenu complémentaire utile en plus
					de nos textes et images. De plus, notre site est mieux référencé sur Google.
					Et si nous faisons de la publicité via Google Ads, Google peut – grâce
					aux données collectées – afficher ces annonces uniquement à des utilisateurs
					vraiment intéressés.
				</p>

				<h3>Quelles données YouTube enregistre-t-il ?</h3>
				<p>
					Dès que vous visitez l’une de nos pages comportant une vidéo YouTube intégrée,
					YouTube place au moins un cookie qui enregistre votre adresse IP et l’URL
					de notre page. Si vous êtes connecté à votre compte YouTube, YouTube peut
					souvent attribuer vos interactions sur notre site à votre profil, comme
					la durée de la session, le taux de rebond, votre position approximative,
					des informations techniques sur votre navigateur, la résolution de l’écran
					ou votre fournisseur d’accès internet. D’autres données peuvent être
					des coordonnées, d’éventuelles évaluations, le partage de contenu sur
					des réseaux sociaux ou l’ajout de vidéos à vos favoris sur YouTube.
				</p>
				<p>
					Si vous n’êtes pas connecté à un compte Google ou YouTube, Google
					enregistre des données identifiées de façon unique, associées à votre
					appareil, votre navigateur ou votre application (p. ex. vos paramètres
					linguistiques). Toutefois, une partie des données d’interaction
					ne peut pas être enregistrée.
				</p>
				<p>
					Voici quelques cookies détectés lors d’un test en navigation. Certains
					sont placés sans être connecté à YouTube, d’autres sont placés en étant
					connecté. Cette liste n’est pas exhaustive, car tout dépend de
					vos interactions :
				</p>
				<ul>
					<li>Nom : YSC</li>
					<li>Valeur : b9-CV6ojI5Y</li>
					<li>Utilisation : Ce cookie enregistre un ID unique pour stocker des statistiques sur les vidéos vues.</li>
					<li>Expiration : fin de la session</li>
				</ul>
				<ul>
					<li>Nom : PREF</li>
					<li>Valeur : f1=50000000</li>
				</ul>
				<p>
					Utilisation : Ce cookie enregistre également votre ID unique. Google
					obtient via PREF des statistiques sur la manière dont vous utilisez
					des vidéos YouTube sur notre site.
				</p>
				<p>Expiration : après 8 mois</p>

				<ul>
					<li>Nom : GPS</li>
					<li>Valeur : 1</li>
				</ul>
				<p>
					Utilisation : Ce cookie enregistre votre ID unique sur des appareils
					mobiles pour suivre votre position GPS.
				</p>
				<p>Expiration : après 30 minutes</p>

				<ul>
					<li>Nom : VISITOR_INFO1_LIVE</li>
					<li>Valeur : 95Chz8bagyU</li>
				</ul>
				<p>
					Utilisation : Ce cookie tente d’estimer la bande passante de l’utilisateur
					sur nos pages (avec des vidéos YouTube intégrées).
				</p>
				<p>Expiration : après 8 mois</p>

				<p>
					D’autres cookies sont placés si vous êtes connecté à votre compte YouTube.
					Par exemple :
				</p>
				<ul>
					<li>Nom : APISID</li>
					<li>Valeur : zILlvClZSkqGsSwI/AU1aZI6HY7311128432-</li>
				</ul>
				<p>
					Utilisation : Ce cookie est utilisé pour établir un profil de vos centres
					d’intérêt afin de proposer de la publicité personnalisée.
				</p>
				<p>Expiration : après 2 ans</p>

				<ul>
					<li>Nom : CONSENT</li>
					<li>Valeur : YES+AT.de+20150628-20-0</li>
				</ul>
				<p>
					Utilisation : Le cookie enregistre le statut du consentement d’un utilisateur
					concernant l’utilisation de différents services Google. CONSENT sert
					également à la sécurité, pour vérifier les utilisateurs et protéger
					les données contre toute attaque non autorisée.
				</p>
				<p>Expiration : après 19 ans</p>

				<ul>
					<li>Nom : HSID</li>
					<li>Valeur : AcRwpgUik9Dveht0I</li>
				</ul>
				<p>
					Utilisation : Ce cookie est utilisé pour établir un profil de vos centres
					d’intérêt, afin d’afficher de la publicité personnalisée.
				</p>
				<p>Expiration : après 2 ans</p>

				<ul>
					<li>Nom : LOGIN_INFO</li>
					<li>Valeur : AFmmF2swRQIhALl6aL…</li>
				</ul>
				<p>
					Utilisation : Ce cookie contient des informations sur vos données de connexion.
				</p>
				<p>Expiration : après 2 ans</p>

				<ul>
					<li>Nom : SAPISID</li>
					<li>Valeur : 7oaPxoG-pZsJuuF5/AnUdDUIsJ9iJz2vdM</li>
				</ul>
				<p>
					Utilisation : Ce cookie identifie de manière unique votre navigateur
					et votre appareil, afin d’établir un profil de vos centres d’intérêt.
				</p>
				<p>Expiration : après 2 ans</p>

				<ul>
					<li>Nom : SID</li>
					<li>Valeur : oQfNKjAsI311128432-</li>
				</ul>
				<p>
					Utilisation : Ce cookie enregistre votre ID de compte Google et
					l’heure de votre dernière connexion sous forme chiffrée.
				</p>
				<p>Expiration : après 2 ans</p>

				<ul>
					<li>Nom : SIDCC</li>
					<li>Valeur : AN0-TYuqub2JOcDTyL</li>
				</ul>
				<p>
					Utilisation : Ce cookie enregistre des informations sur la manière
					dont vous utilisez le site et sur les publicités que vous avez pu
					voir avant de venir sur notre site.
				</p>
				<p>Expiration : après 3 mois</p>

				<h3>Combien de temps et où sont stockées les données ?</h3>
				<p>
					Les données que YouTube collecte et traite sont stockées sur les serveurs
					de Google, pour la plupart aux États-Unis. Vous pouvez consulter
					l’emplacement exact des centres de données Google ici :
					<NuxtLink
						:external="true"
						to="https://www.google.com/about/datacenters/inside/locations/?hl=fr">
						https://www.google.com/about/datacenters/inside/locations/?hl=fr
					</NuxtLink>.
					Vos données sont réparties sur les serveurs, afin d’être plus facilement
					accessibles et mieux protégées contre toute manipulation.
				</p>
				<p>
					Google conserve les données collectées pendant des durées variables.
					Vous pouvez en supprimer certaines, d’autres sont automatiquement effacées
					après un certain temps et d’autres encore sont conservées plus longtemps
					par Google. Certains éléments (comme ceux de « Mon activité », photos
					ou documents) stockés dans votre compte Google y restent jusqu’à ce que
					vous les supprimiez. Même si vous n’êtes pas connecté à un compte Google,
					vous pouvez parfois supprimer des données liées à votre appareil,
					votre navigateur ou votre application.
				</p>

				<h3>Comment puis-je supprimer mes données ou empêcher leur stockage ?</h3>
				<p>
					En principe, vous pouvez supprimer manuellement vos données dans
					votre compte Google. La fonction d’effacement automatique introduite en 2019
					supprime les données relatives à votre position et à votre activité
					après 3 ou 18 mois, selon votre choix.
				</p>
				<p>
					Que vous ayez ou non un compte Google, vous pouvez configurer votre navigateur
					de manière à ce que les cookies Google soient effacés ou désactivés. Selon
					le navigateur utilisé, la procédure varie. Les liens suivants expliquent
					comment gérer les cookies :
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.google.com/chrome/answer/95647?tid=311128432">
						Chrome : supprimer, activer et gérer les cookies
					</NuxtLink>
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.apple.com/fr-fr/guide/safari/sfri11471/mac?tid=311128432">
						Safari : gérer les cookies et les données de sites web
					</NuxtLink>
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.mozilla.org/fr/kb/effacer-cookies-donnees-site-firefox?tid=311128432">
						Firefox : supprimer les cookies pour supprimer des données que des sites web ont placées sur votre ordinateur
					</NuxtLink>
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.microsoft.com/fr-fr/help/17442/windows-internet-explorer-delete-manage-cookies?tid=311128432">
						Internet Explorer : supprimer et gérer les cookies
					</NuxtLink>
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.microsoft.com/fr-fr/help/4027947/windows-delete-cookies?tid=311128432">
						Microsoft Edge : supprimer et gérer les cookies
					</NuxtLink>
				</p>

				<p>
					Si vous ne voulez absolument pas de cookies, vous pouvez configurer
					votre navigateur pour être informé à chaque fois qu’un cookie
					doit être placé. Vous décidez alors au cas par cas de l’accepter ou non.
					Étant donné que YouTube appartient à Google, il existe une déclaration
					de confidentialité commune. Pour en savoir plus sur la gestion de
					vos données, nous vous recommandons la politique de confidentialité
					de Google :
					<NuxtLink :external="true" to="https://policies.google.com/privacy?hl=fr">
						https://policies.google.com/privacy?hl=fr
					</NuxtLink>.
				</p>

				<h2>Bouton d’abonnement YouTube – Déclaration de confidentialité</h2>
				<p>
					Nous avons intégré sur notre site le bouton « S’abonner » de YouTube.
					Vous reconnaîtrez généralement ce bouton à l’emblématique logo YouTube :
					les mots « S’abonner » ou « YouTube » en blanc sur fond rouge, précédés
					du symbole « Play » en blanc. Le design peut également varier.
				</p>
				<p>
					Notre chaîne YouTube propose régulièrement des vidéos amusantes, intéressantes
					ou captivantes. En intégrant ce bouton d’abonnement, vous pouvez vous abonner
					directement à notre chaîne depuis notre site, sans devoir vous rendre sur
					le site YouTube. Ainsi, nous vous facilitons l’accès à notre contenu.
					Notez que YouTube peut, ce faisant, enregistrer et traiter des données vous
					concernant.
				</p>
				<p>
					Lorsque vous affichez notre bouton d’abonnement, YouTube place – selon Google –
					au moins un cookie. Ce cookie stocke votre adresse IP et l’URL de notre page.
					Il peut également enregistrer des informations sur votre navigateur, votre
					localisation approximative et votre langue par défaut. Lors de notre test,
					les quatre cookies suivants ont été placés sans être connecté à YouTube :
				</p>
				<ul>
					<li>Nom : YSC</li>
					<li>Valeur : b9-CV6ojI5311128432Y</li>
					<li>
						Utilisation : Ce cookie enregistre un ID unique pour conserver des
						statistiques sur les vidéos visionnées.
					</li>
					<li>Expiration : fin de la session</li>
				</ul>
				<ul>
					<li>Nom : PREF</li>
					<li>Valeur : f1=50000000</li>
				</ul>
				<p>
					Utilisation : Ce cookie enregistre également votre ID unique. Google
					obtient des statistiques via PREF, par exemple comment vous utilisez
					les vidéos de YouTube sur notre site.
				</p>
				<p>Expiration : après 8 mois</p>

				<ul>
					<li>Nom : GPS</li>
					<li>Valeur : 1</li>
				</ul>
				<p>
					Utilisation : Ce cookie enregistre votre ID unique sur les appareils mobiles,
					pour suivre votre position GPS.
				</p>
				<p>Expiration : après 30 minutes</p>

				<ul>
					<li>Nom : VISITOR_INFO1_LIVE</li>
					<li>Valeur : 31112843295Chz8bagyU</li>
				</ul>
				<p>
					Utilisation : Ce cookie tente d’estimer la bande passante de l’utilisateur
					sur nos pages (avec vidéo YouTube intégrée).
				</p>
				<p>Expiration : après 8 mois</p>

				<p>
					Remarque : ces cookies ont été placés lors d’un test et ne prétendent pas
					à l’exhaustivité.
				</p>
				<p>
					Si vous êtes connecté à votre compte YouTube, la plateforme peut enregistrer
					de nombreuses interactions (clics, durée, etc.) sur notre site via les cookies
					et les associer à votre compte. Ainsi, YouTube apprend par exemple combien
					de temps vous naviguez sur notre page, quel type de navigateur vous utilisez,
					quelle résolution d’écran vous préférez et quelles actions vous réalisez.
				</p>
				<p>
					YouTube utilise ces données pour améliorer ses propres services et offres,
					et pour fournir des analyses et statistiques aux annonceurs utilisant Google Ads.
				</p>

				<h4>Candidatures en ligne / Publication d’offres d’emploi</h4>
				<p>
					Nous vous offrons la possibilité de postuler chez nous via notre présence
					sur Internet. Lors de ces candidatures numériques, vos données de candidature
					sont collectées et traitées électroniquement afin de mener à bien
					le processus de recrutement.
				</p>
				<p>
					La base légale pour ce traitement est le § 26 al. 1 phrase 1 BDSG
					conjointement à
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/FR/TXT/HTML/?uri=CELEX:32016R0679&from=FR&tid=311128432">
						l’article 88, paragraphe 1 du RGPD
					</NuxtLink>.
				</p>
				<p>
					Si, à l’issue de ce processus, nous concluons un contrat de travail,
					nous conservons les données transmises dans votre dossier personnel
					afin de mener à bien l’organisation et la gestion du personnel, en
					respectant les obligations légales.
				</p>
				<p>
					La base légale pour ce traitement est également le § 26 al. 1 phrase 1 BDSG
					conjointement à l’article 88 al. 1 RGPD.
				</p>
				<p>
					En cas de refus d’une candidature, nous supprimons automatiquement les données
					transmises deux mois après la notification du refus. Toutefois, les données
					ne sont pas supprimées si elles s’avèrent nécessaires pour respecter des obligations
					légales (par ex. obligations de preuve prévues par l’AGG), exigeant une conservation
					plus longue pouvant aller jusqu’à quatre mois ou jusqu’à la fin d’une procédure
					judiciaire.
				</p>
				<p>
					La base légale est alors
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/FR/TXT/HTML/?uri=CELEX:32016R0679&from=FR&tid=311128432">
						l’article 6, paragraphe 1, point f) du RGPD
					</NuxtLink>
					ainsi que le § 24 al. 1 n° 2 BDSG. Notre intérêt légitime réside dans la défense en
					justice ou la revendication de droits.
				</p>
				<p>
					Si vous consentez expressément à ce que vos données soient conservées plus
					longtemps (par exemple pour être intégré(e) dans une base de candidats ou
					de personnes intéressées), vos données seront traitées sur la base de votre
					consentement. La base légale est alors
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/FR/TXT/HTML/?uri=CELEX:32016R0679&from=FR&tid=311128432">
						l’article 6, paragraphe 1, point a) du RGPD
					</NuxtLink>.
					Vous pouvez bien sûr révoquer à tout moment votre consentement pour l’avenir
					(art. 7, paragraphe 3 du RGPD) en nous en faisant la demande.
				</p>

				<p>
					<NuxtLink target="_blank" :external="true" to="https://www.adsimple.de/datenschutz-generator/">
						Modèle de déclaration de confidentialité
					</NuxtLink>
					de
					<NuxtLink target="_blank" :external="true" to="https://www.adsimple.de/">
						adsimple.de
					</NuxtLink>
				</p>

				<hr />
				<p style="padding-top: 20px">
					memoWikis est financé dans le cadre du programme EXIST par le Ministère fédéral
					de l’Économie et de l’Énergie ainsi que par le Fonds social européen.
				</p>

			</div>
		</div>
	</div>
</template>
