<script lang="ts" setup>
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
</script>
<template>
    <div class="container">
        <div class="main-page">
            <div class="row header">
                <div class="col-md-12">
                    <div class="first-header">MITGLID WERDEN UND FREIE BILDUNG UNTERSTÜTZEN!</div>
                    <div class="second-header">Öffentlich ist kostenlos – für immer und alle!</div>
                    <div class="third-header">Öffentlichen Inhalte sind auf memucho uneingeschränkter nutzbar – Freie Daten!
                        Freie
                        Software! (open data open access)<br />
                        Du möchtest memucho privat nutzen? Hier findest unsere Pläne:</div>
                </div>
            </div>
            <div id="PricesOuter" class="row">
                <div class="price-inner" id="Free">
                    <div class="head-line">Kostenlos</div>
                    <div class="price">0€</div>
                    <div class="first-text">Kostenlos für immer!</div>
                    <button>KOSTENLOS REGISTRIEREN</button>
                    <div class="second-text">
                        <p>Für einzelne Personen, die schnell und unkompliziert mit dem Lernen beginnen wollen.</p>
                        <p> Erstelle Dein persönliches Wiki und verwalte erste persönliche Dokumente und kleine
                            Lerneinheiten.</p>
                    </div>
                    <div class="enumeration">
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Unbeschränkt öffentliche Inhalte<br />
                            <span class="second-row">(ansehen, erstellen und lernen)</span>
                        </div>
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> 10 private Themen </div>
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> 50 private Fragen</div>
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> max 150 Fragen im Wunschwissen</div>
                    </div>
                </div>
                <div class="price-inner" id="Plus">
                    <div class="head-line">Plus</div>
                    <div class="price">5€</div>
                    <div class="first-text">pro Monat bei monatlicher Zahlung</div>
                    <button>Auswählen</button>
                    <div class="second-text">
                        <p>Für einzelne Personen, die täglich Lernen und sich Wissen zu einer Vielzahl an Themen aneignen
                            wollen.</p>
                        <p>Verwalten Sie unbegrenzt private Inhalte.</p>
                    </div>
                    <div class="enumeration">
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Unbeschränkt private Themen</div>
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Unbeschränkt private Fragen</div>
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Unbeschränktes Wunschwissen</div>
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Private Inhalte teilen </div>
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Gruppen Verwalten </div>
                    </div>
                </div>
                <div class="price-inner" id="Team">
                    <div class="head-line">Team</div>
                    <div class="price">7€</div>
                    <div class="first-text">pro Monat bei monatlicher Zahlung</div>
                    <button class="disabled" disabled>In Planung</button>
                    <div class="second-text">
                        <p>Für Lehrende und Lerngruppen, die sich gemeinsam Themen erarbeiten und gemeinsam Lernen wollen.
                        </p>
                        <p>Verwalten Sie Lerngruppen und unbegrenzt Inhalte.</p>
                    </div>
                    <div class="enumeration">
                        <div>Alles aus Personal, plus: <br /></div>
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Private Inhalte im Team bearbeiten</div>
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Teams verwalten</div>
                    </div>
                </div>
                <div class="price-inner" id="Organisation">
                    <div class="head-line">Organisation</div>
                    <div class="price-organisation">Auf Anfrage</div>
                    <div class="first-text">Individuelle Preise. Bitte Kontaktieren Sie uns für mehr Informationen.</div>
                    <button class="contact">KONTAKTIEREN</button>
                    <div class="second-text">
                        <p>Für Unternehmen und Institutionen, die zusätzliche Unterstützung benötigen.</p>
                        <p>Bringen Sie Ihr Team auf den neuesten Stand.</p>
                    </div>
                    <div class="enumeration">
                        <div>Alles aus Team, plus: <br />
                        </div>
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Individuelle Anbindung an Infrastruktur
                        </div>
                        <div><font-awesome-icon :icon="['fa-solid', 'fa-check']" /> Installation on premise oder gehostet
                        </div>
                    </div>
                </div>
            </div>

            <div class="row" id="FaqHeaderOuter">

                <div class="faq-first-header">Häufig gestellte</div>
                <div>Fragen</div>

            </div>

            <div class="row" id="QuestionsOuter">

                <div class="question">
                    <div class="question-inner">
                        <div class="text">Wird Memucho teurer?</div>
                        <div class="icon"><font-awesome-icon :icon="['fa-solid', 'fa-chevron-down']" /></div>
                    </div>
                    <div class="answer">Ja Memucho wird sich mit der Zeit an die Gegebenheiten anpassen allerdings werden
                        wir laufende
                        Verträge nicht ehöhen</div>
                </div>
                <div class="question">
                    <div class="question-inner col-md-12">
                        <div class="text">Wird Memucho teurer?</div>
                        <div class="icon"><font-awesome-icon :icon="['fa-solid', 'fa-chevron-down']" /></div>
                    </div>
                    <div class="answer">Ja Memucho wird sich mit der Zeit an die Gegebenheiten anpassen allerdings werden
                        wir laufende
                        Verträge nicht ehöhen</div>
                </div>
                <div class="question">
                    <div class="question-inner">
                        <div class="text">Wird Memucho teurer?</div>
                        <div class="icon"><font-awesome-icon :icon="['fa-solid', 'fa-chevron-down']" /></div>
                    </div>
                    <div class="answer">Ja Memucho wird sich mit der Zeit an die Gegebenheiten anpassen allerdings werden
                        wir laufende
                        Verträge nicht ehöhen</div>
                </div>
                <div class="question">
                    <div class="question-inner">
                        <div class="text">Wird Memucho teurer?</div>
                        <div class="icon"><font-awesome-icon :icon="['fa-solid', 'fa-chevron-down']" /></div>
                    </div>
                    <div class="answer">Ja Memucho wird sich mit der Zeit an die Gegebenheiten anpassen allerdings werden
                        wir laufende
                        Verträge nicht ehöhen</div>
                </div>

                <div class="not-find">Deine Frage nicht gefunden</div>
            </div>
        </div>
    </div>
</template>
<style scoped lang="less">
@media screen and (min-width: 1666px) {
    .container {
        width: 1366px;
    }
}

.container {


    .header {

        display: flex;
        flex-wrap: wrap;
        flex-direction: column;
        margin-top: 100px;

        color: #003264;

        .first-header {
            font-weight: 600;


        }

        .second-header {
            margin-top: 20px;
            font-size: 45px;
            font-weight: 400;
        }

        .third-header {
            margin-top: 20px;
            font-weight: 400;
            font-size: 18px;
            margin-bottom: 20px;
        }
    }

    #PricesOuter {
        display: flex;
        flex-wrap: wrap;


        #Free {
            width: 341px;
            margin-right: 10px;
        }

        #Plus {
            width: 320px;
            margin-right: 10px;
        }

        #Organisation {
            width: 305px;
            margin-right: 10px;
        }

        #Team {
            width: 320px;
            margin-right: 10px;
        }

        .price-inner {
            border: #DDDDDD 1px solid;
            padding: 20px;

            .head-line {
                font-size: 32px;
                margin-right: 20px;
                font-weight: 400;
            }

            .contact {
                color: #0065CA;
                background: #E5E5E5;
                font-size: 14px;

            }

            .disabled {
                background: linear-gradient(0deg, rgba(255, 255, 255, 0.7), rgba(255, 255, 255, 0.7)), #0065CA;
            }

            .price {
                font-size: 45px;
                font-weight: 700;
                margin-top: 50px;
            }

            .price-organisation {
                margin-top: 50px;
                font-weight: 700;
                font-size: 18px;

            }

            .first-text {
                margin-top: 10px;
            }

            button {
                margin-top: 10px;
                padding: 8px 16px 8px 16px;
                background: #0065CA;
                font-weight: 600;
                font-size: 14px;
                color: #FFFFFF;
            }

            .second-text {
                margin-top: 50px;
                color: #555555;
            }

            .enumeration {
                font-weight: 400;
                font-size: 14px;
                color: #003264;

                .fa-check {
                    font-size: 18px;
                    font-weight: 900;
                }

                .second-row {
                    margin-left: 22px;
                }
            }
        }
    }

    #FaqHeaderOuter {
        margin-top: 725px;
        color: #074EE8;
        font-weight: 400;
        font-size: 45px;
        display: flex;
        align-items: center;
        flex-direction: column;
    }

    #QuestionsOuter {
        margin-top: 31px;
        background-color: #EFEFEF;

        .question {
            min-height: 60px;
            background-color: white;
            margin-top: 19px;

            .question-inner {
                width: 100;

                .icon {
                    margin-left: auto;
                    margin-right: 10px;

                }


            }

            .answer {
                margin-top: 20px;
                margin-bottom: 20px;
                display: none;
            }
        }


        .not-find {
            margin-top: 112px;
            color: #074EE8;

        }
    }

}
</style> 

