<script lang="ts" setup>
import { Site } from '~/components/shared/siteEnum'
import { FeatureImgPosEnum } from '~/components/welcome/feature/featureImgPosEnum'
import { HeroContentAlignEnum, HeroImgPosEnum } from '~/components/welcome/hero/HeroBlockEnums'

const emit = defineEmits(['setPage'])
onBeforeMount(() => {
    emit('setPage', Site.Welcome)
})
</script>

<template>

    <div id="Welcome">
        <WelcomeHeroBlock :align="HeroContentAlignEnum.Center">

            <template #header>
                <h1>Wissensmanagement</h1>
            </template>
            <template #text>
                <p>memoWikis denkt Wissensmanagement neu</p>

            </template>
            <template #buttons>
                <!-- <button navigate class="btn memo-button cta-btn">
                    <NuxtLink to="/Registrieren" class="">
                        Kostenlos registrieren!
                    </NuxtLink>
                </button> -->
                <!-- <button navigate class="btn memo-button toWiki-btn">
                    <NuxtLink to="/Gl/1" class="">
                        Secondary Button / Wiki
                    </NuxtLink>
                </button> -->
            </template>
        </WelcomeHeroBlock>

        <WelcomeHeroBlock>
            <template #image>
                <img src="~/assets/images/WelcomePage/1t.png" alt="Welcome Hero" />
            </template>
            <template #text>
                <!-- <p>memoWikis denkt Wissensmanagement neu</p> -->
                <div class="hero-content-text">
                    <h2> Inhalt: </h2>
                    <ul class="feature-list">
                        <li>
                            <NuxtLink to="#Sammeln & Organisieren">
                                Sammeln & Organisieren
                            </NuxtLink>
                        </li>
                        <!-- <li>
                            <NuxtLink to="#Feature-2">
                                Organisieren
                            </NuxtLink>
                        </li> -->
                        <li>Teilen</li>
                        <li>Veröffentlichen</li>
                        <li>Durchsuchen</li>
                        <li>Verknüpfen</li>
                        <li>Wissensstandanzeige</li>
                        <li>Lernen</li>
                        <li>Karteikarten</li>
                        <li>Analytics</li>
                        <li>Feed</li>
                        <li>AI-generierte Karteikarten</li>
                    </ul>
                </div>
            </template>
        </WelcomeHeroBlock>

        <!-- <div class="spacer"></div> -->

        <div class="spacer star"></div>


        <WelcomeFeatureSection :image-position="FeatureImgPosEnum.Left" id="Sammeln & Organisieren">
            <template #image>
                <img src="~/assets/images/WelcomePage/3.png" />
            </template>
            <template #header>
                Sammeln & Organisieren
            </template>
            <template #content>
                <ul>
                    <li><strong>Sammle</strong> deine Wikis, <strong>erstelle</strong> Dokumente und halte alle Notizen an einem zentralen Ort bereit.</li>
                    <li>Bringe <strong>Struktur</strong> in deine Inhalte, indem du sie thematisch oder projektbezogen gruppierst.</li>
                </ul>
            </template>
        </WelcomeFeatureSection>

        <WelcomeFeatureSection :image-position="FeatureImgPosEnum.Left" id="Feature-2">
            <template #image>
                <img src="~/assets/images/WelcomePage/3.png" />
            </template>
            <template #header>
                Organisieren
            </template>
            <template #content>
                <ul>
                    <li><strong>Sammle</strong> deine Wikis, <strong>erstelle</strong> Dokumente und halte alle Notizen an einem zentralen Ort bereit.</li>
                    <li>Bringe <strong>Struktur</strong> in deine Inhalte, indem du sie thematisch oder projektbezogen gruppierst.</li>
                </ul>
            </template>
        </WelcomeFeatureSection>

        <WelcomeFeatureSection :image-position="FeatureImgPosEnum.Right">
            <template #image>
                <img src="~/assets/images/WelcomePage/2.png" />
            </template>
            <template #header>
                Wissen ist Teamwork
            </template>
            <template #content>
                <ul>
                    <li>Gewinne Einblicke in deine Lerngewohnheiten und decke Potenziale für Verbesserungen auf.</li>
                    <li>Bleib durch den <strong>Feed</strong> über neue Inhalte und Diskussionen auf dem Laufenden.</li>
                    <li><strong>Kollaboriere</strong> in Echtzeit an Dokumenten, Wikis und Notizen - bleib stets auf demselben Stand wie dein Team.</li>
                </ul>
            </template>
        </WelcomeFeatureSection>

        <WelcomeFeatureSection :image-position="FeatureImgPosEnum.Right">
            <template #image>
                <img src="~/assets/images/WelcomePage/2.png" />
            </template>
            <template #eyebrow>
                Ladada
            </template>
            <template #header>
                Lorem Ipsum tada undso
            </template>
            <template #content>
                Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at
                Hampden-Sydney
                College in Virginia,
            </template>
        </WelcomeFeatureSection>

        <WelcomeFeatureSection :image-position="FeatureImgPosEnum.Top">
            <template #image>
                <img src="~/assets/images/WelcomePage/1t.png" />
            </template>
            <template #header>
                Lernen leicht gemacht
            </template>
            <template #content>
                Gewinne Einblicke in deine Lerngewohnheiten und decke Potenziale für Verbesserungen auf.
                Bleib durch den Feed über neue Inhalte und Diskussionen auf dem Laufenden.
                Kollaboriere in Echtzeit an Dokumenten, Wikis und Notizen - bleib stets auf demselben Stand wie dein Team.
            </template>
        </WelcomeFeatureSection>


        <WelcomeFeatureSection :image-position="FeatureImgPosEnum.Bottom">
            <template #image>
                <img src="~/assets/images/WelcomePage/2.png" />
            </template>
            <template #eyebrow>
                Ladada
            </template>
            <template #header>
                Lorem Ipsum tada undso
            </template>
            <template #content>
                Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at
                Hampden-Sydney
                College in Virginia,
            </template>
        </WelcomeFeatureSection>

        <WelcomeHeroBlock :align="HeroContentAlignEnum.Center">
            <template #text>
                <p>Blahblah catchphrase ding</p>
            </template>
            <template #buttons>
                <button navigate class="btn memo-button cta-btn">
                    <NuxtLink to="/Registrieren" class="">
                        Kostenlos registrieren!
                    </NuxtLink>
                </button>
            </template>
        </WelcomeHeroBlock>
    </div>

</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

#Welcome {
    margin-bottom: 4em;

    .spacer {
        height: 6rem;
        width: 100%;

        &.star {
            display: flex;
            justify-content: center;
            align-items: center;
            height: 12rem;

            &:before {
                content: "✦";
                color: @memo-green;
                position: absolute;
                left: calc(50% - 1.5rem);
                font-size: 3rem;
            }

        }
    }

    .border-small {
        max-width: 1160px;

        .border-inner {
            border-bottom: 1px solid @memo-grey-light;
            width: 100%;
        }
    }
}

.cta-btn {
    background: @memo-green;
    border-radius: 24px;
    font-size: 1.75rem;
    height: 48px !important;
    padding: 0 2rem !important;

    :deep(a) {
        color: @memo-blue;
    }
}

.toWiki-btn {
    background: white;
    border-radius: 24px;
    border: solid 2px @memo-grey-light;
    font-size: 1.75rem;
    height: 48px !important;
    padding: 0 2rem !important;

    :deep(a) {
        color: @memo-blue;
    }
}
</style>