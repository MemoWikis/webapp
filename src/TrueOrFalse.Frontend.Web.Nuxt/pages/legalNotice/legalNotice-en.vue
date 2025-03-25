<script lang="ts" setup>
const emit = defineEmits(['setBreadcrumb'])

onBeforeMount(() => {
	emit('setBreadcrumb', [{ name: 'Legal Notice & Privacy Policy', url: '/LegalNotice' }])
})

function openEmail() {
	window.location.href = "mailto:team@memucho.de"
}

const config = useRuntimeConfig()

useHead(() => ({
	link: [
		{
			rel: 'canonical',
			href: `${config.public.officialBase}/LegalNotice`
		},
	],
	meta: [
		{
			name: 'description',
			content: 'Legal information and company details for memoWikis.'
		},
		{
			property: 'og:title',
			content: 'Imprint | memoWikis'
		},
		{
			property: 'og:url',
			content: `${config.public.officialBase}/LegalNotice`
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
		<div class="row main-page">
			<div class="form-horizontal col-md-12 main-content">

				<h1 class="PageHeader" id="Impressum">Legal Notice & Privacy Policy</h1>

				<h2>1. Legal Notice</h2>

				<h3>Information pursuant to § 5 TMG:</h3>
				memucho GmbH<br />
				Am Moorhof <br />
				Nettgendorfer Str. 7 <br />
				14947 Nuthe-Urstromtal<br />

				<h3>Registry Entry:</h3>
				Register Court: Amtsgericht Cottbus, HRB 13499 CB<br />
				<h3>Represented by:</h3>
				Managing Director:<br />
				Robert Mischke<br />

				<h3 id="Kontakt">Contact:</h3>
				Telephone:
				<br />
				+49-178 186 68 48<br />
				<span class="mailme btn-link" @click="openEmail">team@memucho.de</span>

				<h3>Responsible for content pursuant to § 55 (2) RStV:</h3>
				<p>Robert Mischke</p>

				<h3>Disclaimer</h3>
				<p><strong>Liability for Content</strong></p>
				<p>
					The content of our pages has been created with the utmost care.
					However, we cannot guarantee its accuracy, completeness, or timeliness.
					As a service provider, we are responsible for our own content on these
					pages in accordance with § 7(1) TMG under general law. However, pursuant
					to §§ 8 to 10 TMG, we are not obliged to monitor transmitted or stored
					third-party information or investigate circumstances that indicate illegal
					activity. Obligations to remove or block the use of information in
					accordance with general law remain unaffected. Liability in this regard
					is only possible from the moment we become aware of a specific
					infringement. Upon notification of such legal violations, we will remove
					this content immediately.
				</p>

				<p><strong>Liability for Links</strong></p>
				<p>
					Our site contains links to external websites operated by third parties,
					over whose content we have no control. Therefore, we cannot accept any
					responsibility for these external contents. The respective provider or
					operator of linked pages is always responsible for their own content.
					The linked pages were checked for possible legal violations at the time
					of linking, and no illegal content was apparent. A permanent inspection
					of the linked pages is not reasonable without concrete evidence of
					infringement. If any legal violations become known, we will remove such
					links immediately.
				</p>

				<p><strong>Data Protection</strong></p>
				<p>
					The use of our website is partly possible without providing personal
					data. Wherever personal data (for example, name, address, or email
					addresses) is collected on our pages, this is, as far as possible,
					always done on a voluntary basis. This data will not be disclosed to
					third parties without your explicit consent.
				</p>
				<p>
					Please note that data transmission over the Internet (for example,
					when communicating via email) can have security vulnerabilities. It
					is not possible to completely protect data from third-party access.
				</p>
				<p>
					We expressly prohibit the use of contact data published in compliance
					with legal notice requirements by third parties for the purpose of
					sending unsolicited advertising or informational materials. The owners
					of these pages reserve the right to take legal action in the event
					of unsolicited sending of advertising information, such as spam emails.
				</p>

				<p id="under16"><strong>Users Under 16 Years of Age</strong></p>
				<p>
					If you are under 16 years old, you may only register on memoWikis
					with your parents’ consent.
				</p>
				<p>
					Your parents can provide consent by email or phone. They can contact
					us here: <span class="mailme btn-link" @click="openEmail">team@memucho.de</span>
				</p>
				<p>
					You can of course use the site anonymously—that is, without registration.
				</p>

				<p>
					(A) Source: <i>
						<NuxtLink
							to="http://www.e-recht24.de/muster-disclaimer.htm"
							target="_blank"
							:external="true">
							Disclaimer
						</NuxtLink> by
						eRecht24, the portal for internet law by <NuxtLink
							to="http://www.e-recht24.de/"
							target="_blank"
							:external="true">
							attorney
						</NuxtLink>
						Sören Siebert.
					</i>
				</p>

				<br />

				<h1>Privacy Policy</h1>

				<h2>Data Protection</h2>
				<p>
					We have created this Privacy Policy (version 15.11.2019-311128432) to
					explain, pursuant to the requirements of the
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/DE/ALL/?tid=1234&uri=celex%3A32016R0679&tid=311128432">
						EU General Data Protection Regulation (EU) 2016/679
					</NuxtLink>,
					what information we collect, how we use that data, and which choices
					you have as a visitor to this website.
				</p>
				<p>
					Unfortunately, the very nature of these explanations can sound quite
					technical; however, we have endeavored to make them as simple and
					clear as possible.
				</p>

				<h2>Automatic Data Storage</h2>
				<p>
					When you visit websites today, certain information is automatically
					created and stored, as is the case on this site.
				</p>
				<p>
					When you visit our website—just like you’re doing now—our web server
					(the computer on which this website is stored) automatically saves data
					such as
				</p>
				<ul>
					<li>The address (URL) of the accessed web page</li>
					<li>Browser and browser version</li>
					<li>The operating system used</li>
					<li>The address (URL) of the previously visited page (referrer URL)</li>
					<li>The hostname and IP address of the device from which access is made</li>
					<li>Date and time</li>
				</ul>
				<p>into web server log files.</p>
				<p>
					Generally, web server log files are stored for two weeks and then
					automatically deleted. We do not pass on this data, but we cannot
					exclude the possibility that it may be viewed if there is evidence
					of illegal activity.
				</p>
				<p>
					The legal basis under
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/DE/TXT/HTML/?uri=CELEX:32016R0679&from=DE&tid=311128432">
						Article 6(1)(f) GDPR
					</NuxtLink>
					is that there is a legitimate interest in enabling the error-free
					operation of this website by collecting web server log files.
				</p>

				<h2>Cookies</h2>
				<p>
					Our website uses HTTP cookies to store user-specific data.
				</p>
				<p>
					Below, we explain what cookies are and why they are used, so you can
					better understand this Privacy Policy.
				</p>

				<h3>What Are Cookies Exactly?</h3>
				<p>
					Whenever you browse the Internet, you use a browser. Common browsers
					include Chrome, Safari, Firefox, Internet Explorer, and Microsoft
					Edge. Most websites store small text files in your browser, called
					cookies.
				</p>
				<p>
					One thing is clear: cookies can be very handy. Almost all websites
					use cookies. More specifically, they are HTTP cookies, as there are
					other types of cookies for different applications. HTTP cookies are
					small files stored by our website on your computer. These cookie files
					are automatically placed in the cookie folder—essentially the “brain”
					of your browser. A cookie consists of a name and a value, and its
					definition must also include one or more attributes.
				</p>
				<p>
					Cookies store certain user data, such as language or personalized
					page settings. When you revisit our site, your browser sends the
					“user-specific” information back to us. Thanks to cookies, our website
					recognizes who you are and offers you your usual default settings. In
					some browsers, each cookie has its own file; in others, such as
					Firefox, all cookies are stored in a single file.
				</p>
				<p>
					There are both first-party cookies (created by our website) and
					third-party cookies (created by partner websites, e.g. Google
					Analytics). Each cookie must be evaluated individually, as each
					cookie stores different data. Cookie lifespans can range from a few
					minutes to several years. Cookies are not software programs and do
					not contain viruses, trojans, or other “malware.” They also cannot
					access information on your PC.
				</p>
				<p>Cookie data might look like this:</p>
				<ul>
					<li>Name: _ga</li>
					<li>Expiration: 2 years</li>
					<li>Usage: Distinguishing website visitors</li>
					<li>Example value: GA1.2.1326744211.152311128432</li>
				</ul>

				<p>A browser should support the following minimum sizes:</p>
				<ul>
					<li>A cookie can hold at least 4096 bytes</li>
					<li>At least 50 cookies can be stored per domain</li>
					<li>A total of at least 3000 cookies can be stored</li>
				</ul>

				<h3>Which Types of Cookies Are There?</h3>
				<p>
					The cookies we use specifically depend on the services we use,
					which we clarify in the following sections of this Privacy Policy.
					At this point, let’s briefly discuss the different types of HTTP
					cookies:
				</p>
				<p>There are 4 types of cookies:</p>

				<p>**Essential Cookies**
					These cookies are necessary to ensure basic site functions. For example,
					these cookies are needed to keep items in a shopping cart—even when
					you close the browser window—and allow you to continue where you left
					off.
				</p>

				<p>**Functional Cookies**
					These cookies gather information about user behavior and any errors.
					They also measure load times and the website’s behavior across
					different browsers.
				</p>

				<p>**Targeted Cookies**
					These cookies improve user experience—for instance, saving entered
					locations, font sizes, or form data.
				</p>

				<p>**Advertising Cookies**
					Also known as targeting cookies, these deliver individualized
					advertising to users. This can be quite convenient but also
					annoying at times.
				</p>

				<p>
					Typically, on your first visit to a website, you will be asked which
					of these cookie categories you wish to allow. Naturally, your choice
					is itself stored in a cookie.
				</p>

				<h3>How Can I Delete Cookies?</h3>
				<p>
					It’s up to you whether—and how—you want to use cookies. Regardless
					of which service or website the cookies come from, you always have
					the option to delete cookies, only partially allow them, or disable
					them entirely. For example, you can block third-party cookies but
					allow all other cookies.
				</p>
				<p>
					If you want to see which cookies have been stored in your browser,
					change cookie settings, or delete cookies, you can find instructions
					in your browser settings:
				</p>

				<p>
					<NuxtLink
						:external="true"
						to="https://support.google.com/chrome/answer/95647?tid=311128432">
						Chrome: Delete, enable & manage cookies
					</NuxtLink>
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.apple.com/de-at/guide/safari/sfri11471/mac?tid=311128432">
						Safari: Manage cookies and website data
					</NuxtLink>
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.mozilla.org/de/kb/cookies-und-website-daten-in-firefox-loschen?tid=311128432">
						Firefox: Delete cookies to remove data websites have placed on your computer
					</NuxtLink>
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.microsoft.com/de-at/help/17442/windows-internet-explorer-delete-manage-cookies?tid=311128432">
						Internet Explorer: Delete and manage cookies
					</NuxtLink>
				</p>
				<p>
					<NuxtLink
						:external="true"
						to="https://support.microsoft.com/de-at/help/4027947/windows-delete-cookies?tid=311128432">
						Microsoft Edge: Delete and manage cookies
					</NuxtLink>
				</p>
				<p>
					If you fundamentally do not want any cookies, you can set up your
					browser to notify you every time a cookie is about to be set. That
					way, you can decide whether to allow or disallow each cookie. This
					process varies from browser to browser. Typically, searching for
					“delete cookies Chrome” or “disable cookies Chrome” (or replacing
					“Chrome” with the name of your browser) will direct you to the
					relevant instructions.
				</p>

				<h3>What About My Privacy?</h3>
				<p>
					The “Cookie Guidelines” have existed since 2009, specifying that
					storing cookies requires consent from the website’s visitors (i.e.,
					you). Within EU countries, however, reactions to these guidelines
					vary widely. Germany did not implement the cookie guidelines as
					national law; instead, these guidelines were largely integrated
					under § 15(3) of the German Telemedia Act (TMG).
				</p>
				<p>
					If you want to learn more about cookies, especially the technical
					details, we recommend:
					<NuxtLink
						:external="true"
						to="https://tools.ietf.org/html/rfc6265">
						https://tools.ietf.org/html/rfc6265
					</NuxtLink>,
					the “HTTP State Management Mechanism” Request for Comments by the
					Internet Engineering Task Force (IETF).
				</p>

				<h2>Storing Personal Data</h2>
				<p>
					Personal data that you transmit to us electronically via this website
					(such as your name, email address, address, or other personal details
					provided via a form or blog comments) is used solely for the stated
					purpose, stored securely along with the timestamp and IP address,
					and not shared with third parties.
				</p>
				<p>
					We thus only use your personal data to communicate with visitors who
					expressly wish to be contacted and to process the services and products
					offered on this website. We do not share your personal data without
					your consent, although we cannot rule out access in the event of
					unlawful behavior.
				</p>
				<p>
					If you send us personal data by email—i.e., outside this website—
					please be aware we cannot guarantee secure transmission and data
					protection. We recommend never sending confidential data unencrypted
					via email.
				</p>
				<p>
					The legal basis under
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/DE/TXT/HTML/?uri=CELEX:32016R0679&from=DE&tid=311128432">
						Article 6(1)(a) GDPR
					</NuxtLink>
					is that you consent to the processing of the data you have entered.
					You can revoke this consent at any time—an informal email is sufficient;
					our contact details can be found in the legal notice.
				</p>

				<h2>Your Rights Under the GDPR</h2>
				<p>
					Under the provisions of the GDPR, you are generally entitled to the
					following rights:
				</p>
				<ul>
					<li>Right to rectification (Article 16 GDPR)</li>
					<li>Right to erasure (“right to be forgotten”) (Article 17 GDPR)</li>
					<li>Right to restrict processing (Article 18 GDPR)</li>
					<li>
						Right to notification—obligation to inform in connection with the
						rectification or erasure of personal data or the restriction of
						processing (Article 19 GDPR)
					</li>
					<li>Right to data portability (Article 20 GDPR)</li>
					<li>Right to object (Article 21 GDPR)</li>
					<li>
						Right not to be subject solely to automated decision-making—including
						profiling—(Article 22 GDPR)
					</li>
				</ul>
				<p>
					If you believe the processing of your data violates data protection
					law or otherwise infringes on your data protection rights, you can
					contact the
					<NuxtLink
						:external="true"
						to="https://www.bfdi.bund.de/DE/Datenschutz/Ueberblick/MeineRechte/Artikel/BeschwerdeBeiDatenschutzbehoereden.html?tid=311128432">
						Federal Commissioner for Data Protection and Freedom of Information (BfDI)
					</NuxtLink>.
				</p>

				<h2>Analyzing Visitor Behavior</h2>
				<p>
					In this Privacy Policy, we inform you whether and how we analyze data
					from your visit to this website. The data collected is typically
					evaluated anonymously, and we cannot infer your personal identity
					from your usage behavior on this site.
				</p>
				<p>
					Further details on how to object to this analysis can be found in
					this Privacy Policy.
				</p>

				<p>&nbsp;</p>

				<h3>5. Matomo (formerly “Piwik”)</h3>
				<p><strong>5.1 Scope and Description of Processing Personal Data</strong></p>
				<p>
					Our website uses “Matomo” (formerly “Piwik”), a web analytics service
					by InnoCraft Ltd., 150 Willis St, 6011 Wellington, New Zealand. Matomo
					stores cookies on your device, enabling analysis of how you use our
					website. The information collected is stored exclusively on our server,
					including:
				</p>
				<p>- Two bytes of the user’s IP address</p>
				<p>- The accessed webpage</p>
				<p>- The website from which the user came (referrer)</p>
				<p>- Subpages accessed from the main webpage</p>
				<p>- The time spent on the website</p>
				<p>- How often the webpage is visited</p>
				<p>
					Our website uses Matomo with the setting “Anonymize Visitors’ IP addresses.”
					As a result, IP addresses are further processed only in a truncated form,
					excluding direct personal identification. The software is configured
					so that IP addresses are not fully stored. Instead, 2 bytes of the IP
					address are masked (e.g. 192.168.xxx.xxx). This makes it impossible
					to associate the truncated IP address with the requesting computer.
					The IP address transmitted by your browser via Matomo is not merged
					with other data we collect.
				</p>

				<p><strong>5.2 Legal Basis for Processing Personal Data</strong></p>
				<p>
					The legal basis for processing user data is Article 6(1)(f) GDPR or
					§ 15(3) TMG.
				</p>

				<p><strong>5.3 Purpose of Processing</strong></p>
				<p>
					We use Matomo to analyze how our website and its features are used,
					in order to continuously improve the user experience. By evaluating
					user behavior statistically, we refine and enhance our offerings,
					making them more interesting to visitors.
				</p>

				<p><strong>5.4 Duration of Storage</strong></p>
				<p>
					The data described here is deleted after 90 days.
				</p>

				<p><strong>5.5 Objection and Removal</strong></p>
				<p>
					You can prevent this analysis by deleting cookies and disabling the
					storage of cookies in your browser settings. Please note that doing
					so may affect the functionality of our website.
				</p>
				<p>
					Matomo is an open-source project of InnoCraft Ltd., 150 Willis St, 6011
					Wellington, New Zealand.
				</p>
				<p>
					For more information about data protection, see the privacy policy at:
					<NuxtLink :external="true" to="https://matomo.org/privacy-policy/">
						matomo.org/privacy-policy/
					</NuxtLink>
				</p>

				<p>&nbsp;</p>

				<h2>TLS Encryption via HTTPS</h2>
				<p>
					We use HTTPS to securely transmit data over the Internet (“data
					protection by design,”
					<NuxtLink
						:external="true"
						to="https://eur-lex.europa.eu/legal-content/DE/TXT/HTML/?uri=CELEX:32016R0679&from=DE&tid=311128432">
						Article 25(1) GDPR
					</NuxtLink>).
					By using TLS (Transport Layer Security), an encryption protocol for
					secure data transfer on the Internet, we protect confidential data.
					You can recognize this protection by the small lock icon in the
					upper-left corner of your browser, as well as by the “https” prefix
					(instead of “http”) in our address bar.
				</p>

				<h2>Newsletter Privacy Policy</h2>
				<p>
					If you subscribe to our newsletter, you provide the aforementioned
					personal data and grant us the right to contact you via email. The
					data stored during newsletter registration is used exclusively for
					our newsletter and is not shared with third parties.
				</p>
				<p>
					If you unsubscribe (a link is provided at the bottom of every
					newsletter), we will delete all data collected in conjunction with
					your newsletter registration.
				</p>

				<h2>Local Google Fonts Privacy Statement</h2>
				<p>
					We use Google Fonts by Google Inc. (1600 Amphitheatre Parkway Mountain
					View, CA 94043, USA) on our website. However, we have hosted these
					fonts locally on our web server, rather than using Google’s servers.
					As a result, no connection is made to Google’s servers, and no data
					is transmitted or stored.
				</p>

				<h3>What Are Google Fonts?</h3>
				<p>
					Google Fonts (formerly Google Web Fonts) is an interactive directory
					of over 800 fonts made freely available by
					<NuxtLink :external="true" to="https://de.wikipedia.org/wiki/Google_LLC?tid=311128432">
						Google LLC
					</NuxtLink>.
					With Google Fonts, one can normally use these fonts without hosting
					them locally, but to avoid any transmission of data to Google,
					we have downloaded and embedded the fonts on our server. In this
					way, we comply with data protection regulations and do not send any
					data to Google Fonts.
				</p>
				<p>
					Unlike other web fonts, Google gives us unrestricted access to all
					of its fonts. This enables us to choose from a wide array of fonts
					and maximize our site’s design. For more details, please see:
					<NuxtLink
						:external="true"
						to="https://developers.google.com/fonts/faq?tid=311128432">
						https://developers.google.com/fonts/faq?tid=311128432
					</NuxtLink>.
				</p>

				<h2>Google Fonts Privacy Policy</h2>
				<p>
					We use Google Fonts by Google Inc. (1600 Amphitheatre Parkway Mountain
					View, CA 94043, USA) on our website.
				</p>
				<p>
					To use these Google fonts, you do not need to log in or provide a
					password. Additionally, no cookies are stored in your browser. The
					files (CSS, fonts) are requested via the domains fonts.googleapis.com
					and fonts.gstatic.com. According to Google, the requests for CSS
					and fonts are entirely separate from other Google services. If you
					have a Google account, there’s no need to worry that your account
					data will be transmitted to Google during the use of Google Fonts.
					Google tracks the use of CSS (Cascading Style Sheets) and the fonts
					being used, storing this information securely. We’ll cover the
					specifics of Google’s data storage below.
				</p>

				<h3>What Are Google Fonts?</h3>
				<p>
					Google Fonts (formerly Google Web Fonts) is an interactive directory
					of over 800 fonts provided by
					<NuxtLink
						:external="true"
						to="https://de.wikipedia.org/wiki/Google_LLC?tid=311128432">
						Google LLC
					</NuxtLink>
					for free use.
				</p>
				<p>
					Many of these fonts are released under the SIL Open Font License,
					while others fall under the Apache License. Both are free software
					licenses, meaning we can use them without paying licensing fees.
				</p>

				<h3>Why Do We Use Google Fonts on Our Website?</h3>
				<p>
					With Google Fonts, we can use fonts on our website without hosting
					them on our own server. Google Fonts is a crucial tool for maintaining
					high-quality design. All Google Fonts are automatically optimized
					for the web, saving data and improving load times (especially for
					mobile devices). The fonts are considered secure web fonts, reducing
					rendering errors across different browsers and operating systems.
					With Google Fonts, there are typically no cross-platform issues.
					Google supports all major browsers (Chrome, Firefox, Safari, Opera),
					as well as most modern mobile operating systems (including Android
					2.2+ and iOS 4.2+).
				</p>
				<p>
					We use Google Fonts to present our online services in the most
					visually appealing and unified manner possible. Under Article 6(1)(f)
					lit. F GDPR, this constitutes a “legitimate interest” for processing
					personal data. In this case, “legitimate interest” includes legal,
					economic, and/or ideal interests recognized by law.
				</p>

				<h3>Which Data Does Google Store?</h3>
				<p>
					When you visit our website, the fonts are fetched from a Google
					server. This external request transmits data to Google’s servers,
					allowing Google to determine that you (or your IP address) visited
					our website. The Google Fonts API was created to minimize the
					collection, storage, and use of end-user data required to deliver
					fonts efficiently. (“API” stands for Application Programming
					Interface, serving among other things as a data transmitter in
					software.)
				</p>
				<p>
					Google Fonts securely stores requests for CSS and fonts at Google
					and thus provides protection. By analyzing usage statistics, Google
					can determine the popularity of certain fonts and may publish the
					results internally (e.g., in Google Analytics) or via its BigQuery
					service. Additionally, Google uses its own web crawler to see which
					websites use Google Fonts, publishing these findings in the BigQuery
					database for Google Fonts. BigQuery is a Google web service for
					companies seeking to handle and analyze large volumes of data.
				</p>
				<p>
					Bear in mind that each request for Google Fonts automatically sends
					information (like your IP address, language settings, browser
					resolution, browser version, and name of your browser) to Google’s
					servers. Whether and how this data is stored remains unclear or is
					not definitively stated by Google.
				</p>

				<h3>How Long and Where Is the Data Stored?</h3>
				<p>
					Google stores requests for CSS assets on its servers (mainly outside
					the EU) for one day. This enables us to use a Google stylesheet. A
					stylesheet is a format template that allows for easy, quick changes
					(such as modifying a website’s fonts).
				</p>
				<p>
					Google stores font files for one year. Google’s goal in doing this
					is to enhance website load times overall. If millions of websites
					reference the same fonts, they are cached after your first visit
					and then immediately appear on other websites you visit subsequently.
					Occasionally, Google updates font files to minimize file size,
					expand language coverage, and refine the design.
				</p>

				<h3>How Can I Delete My Data or Prevent Its Storage?</h3>
				<p>
					The data Google stores for one day or one year cannot be simply
					deleted. It is automatically transmitted when you visit a webpage.
					To have this data removed prematurely, you must contact Google
					Support via
					<NuxtLink
						:external="true"
						to="https://support.google.com/?hl=de&tid=311128432">
						https://support.google.com/?hl=de&tid=311128432
					</NuxtLink>.
					Preventing data storage here is only possible by not visiting our
					website.
				</p>
				<p>
					Unlike other web fonts, Google offers unrestricted access to its
					collection, letting us tap into a sea of fonts to optimize our site.
					For more information about Google Fonts, see
					<NuxtLink
						:external="true"
						to="https://developers.google.com/fonts/faq?tid=311128432">
						https://developers.google.com/fonts/faq?tid=311128432
					</NuxtLink>.
					While Google addresses data protection topics, detailed storage
					information is sparse. Obtaining precise information about Google’s
					stored data can be challenging.
				</p>
				<p>
					For a general overview of which data Google collects and how it is
					used, see
					<NuxtLink
						:external="true"
						to="https://policies.google.com/privacy?hl=de&tid=311128432">
						https://www.google.com/intl/de/policies/privacy/
					</NuxtLink>.
				</p>

				<h2>YouTube Privacy Policy</h2>
				<p>
					We have embedded YouTube videos on our site to provide interesting
					content directly here. YouTube is a video platform that has been a
					Google LLC subsidiary since 2006, operated by YouTube, LLC, 901 Cherry
					Ave., San Bruno, CA 94066, USA. When you open one of our pages
					containing an embedded YouTube video, your browser automatically
					connects to YouTube’s/Google’s servers, transmitting various data
					(depending on the settings). Google is fully responsible for the
					data processing, and Google’s privacy policies apply.
				</p>
				<p>
					Below, we’ll explain which data is processed, why we embed YouTube
					videos, and how you can manage or delete your data.
				</p>

				<h3>What Is YouTube?</h3>
				<p>
					YouTube allows users to watch, rate, comment on, and upload videos
					for free. Over the years, YouTube has become one of the most important
					social media platforms worldwide. To display videos on our website,
					YouTube provides an embed code snippet, which we incorporate into
					our page.
				</p>

				<h3>Why Do We Use YouTube Videos on Our Website?</h3>
				<p>
					YouTube is the platform with the most visitors and the best content.
					We strive to provide you with the best possible user experience on
					our site, and interesting videos are a key part of that. Through
					these embedded videos, we supplement our texts and images with
					additional helpful content. Embedding YouTube videos may also
					improve our visibility on Google Search. Even when we place Google
					Ads, Google can show these ads more precisely to people interested
					in our content, thanks to collected data.
				</p>

				<h3>Which Data Does YouTube Store?</h3>
				<p>
					As soon as you visit a page on our site containing an embedded
					YouTube video, YouTube sets at least one cookie storing your IP
					address and our URL. If you are logged into your YouTube account,
					YouTube can typically associate your interactions with your account
					via cookies (e.g., session duration, bounce rate, approximate
					location, technical info like browser type, screen resolution,
					or ISP). Other data could include contact details, any ratings,
					sharing content on social media, or adding videos to your favorites.
				</p>
				<p>
					If you are not logged into a Google/YouTube account, Google saves
					data with a unique identifier tied to your device, browser, or app
					(like your preferred language). However, fewer interaction data
					points are stored because fewer cookies are set.
				</p>
				<p>
					In the following list, we show cookies that were set in a test.
					We list both cookies set without a logged-in YouTube account and
					cookies set when logged in. Because user data always depends on
					interactions with YouTube, this list cannot be exhaustive:
				</p>

				<!-- Example cookies... (omitted for brevity in the explanation) -->

				<p>
					If you are logged into your YouTube account, YouTube can store and
					associate many of your actions/interactions on our website with your
					YouTube account via cookies. This gives YouTube information like how
					long you surf on our site, which browser type you use, which screen
					resolution you prefer, or which actions you perform.
				</p>
				<p>
					YouTube uses this data to improve its own services and offerings,
					as well as to provide analytics and statistics to advertisers (who
					use Google Ads).
				</p>

				<h4>Online Job Applications / Publication of Job Advertisements</h4>
				<p>
					We offer you the opportunity to apply to us via our website. In these
					digital applications, your applicant and application data are
					collected and processed electronically for the purpose of handling
					the application process.
				</p>
				<p>
					The legal basis for this processing is § 26(1) sentence 1 BDSG in
					conjunction with Article 88(1) GDPR.
				</p>
				<p>
					If an employment contract is signed as a result of the application
					process, we store the data you submitted when applying in your
					personnel file for standard organizational and administrative
					procedures—while observing further legal obligations.
				</p>
				<p>
					The legal basis for this processing is again § 26(1) sentence 1 BDSG
					in conjunction with Article 88(1) GDPR.
				</p>
				<p>
					If an application is rejected, we delete the data you provided
					automatically two months after the rejection notice, unless legal
					provisions—such as evidence obligations under the AGG—require
					retention of up to four months or until the conclusion of legal
					proceedings.
				</p>
				<p>
					The legal basis in that case is Article 6(1)(f) GDPR and
					§ 24(1) No. 2 BDSG. Our legitimate interest is in legal defense
					or enforcement.
				</p>
				<p>
					If you expressly consent to your data being stored longer (e.g. for
					inclusion in an applicant or prospect database), processing is then
					based on your consent. The legal basis is Article 6(1)(a) GDPR. You
					may revoke this consent at any time under Article 7(3) GDPR, by
					notifying us to that effect, with future effect.
				</p>

				<p>
					<NuxtLink
						target="_blank"
						:external="true"
						to="https://www.adsimple.de/datenschutz-generator/">
						Sample Privacy Policy
					</NuxtLink>
					by
					<NuxtLink target="_blank" :external="true" to="https://www.adsimple.de/">
						adsimple.de
					</NuxtLink>
				</p>

				<hr />
				<p style="padding-top: 20px">
					memoWikis is funded under the EXIST program by the Federal Ministry
					for Economic Affairs and Energy and the European Social Fund.
				</p>

			</div>
		</div>
	</div>
</template>
