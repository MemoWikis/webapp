<script lang="ts" setup>
import { useUserStore } from '~~/components/user/userStore'
import { useLearningSessionConfigurationStore } from './learningSessionConfigurationStore'

const userStore = useUserStore()
interface Props {
    openFilter?: boolean
    cookieName: string
    expiryDate?: Date
}
const props = defineProps<Props>()

const learningSessionConfigurationStore = useLearningSessionConfigurationStore()
const showFilterDropdown = ref(false)
const showQuestionFilterOptionsDropdown = ref(false)
function closeQuestionFilterDropdown() {
    showQuestionFilterOptionsDropdown.value = false
}

const showModeSelectionDropdown = ref(false)
function closeModeSelectionDropdown() {
    showModeSelectionDropdown.value = false
}

const showKnowledgeSummaryDropdown = ref(false)

function closeKnowledgeSummaryDropdown() {
    showKnowledgeSummaryDropdown.value = false
}
onBeforeMount(() => {
    if (props.openFilter)
        showFilterDropdown.value = true
})
onMounted(() => learningSessionConfigurationStore.checkQuestionFilterSelection())
watch(() => props.openFilter, (val) => {
    if (val)
        showFilterDropdown.value = true
})
watch(showFilterDropdown, (val) => {
    const cookieOptions = props.expiryDate ? { expires: props.expiryDate } : {}
    const cookie = useCookie(props.cookieName, cookieOptions)
    cookie.value = val.toString()
})

const { isMobile } = useDevice()
const { t } = useI18n()
</script>

<template>
    <div class="session-configurator">
        <div class="session-config-header-container">
            <div class="filter-button selectable-item session-title" @click="showFilterDropdown = !showFilterDropdown"
                :class="[showFilterDropdown ? 'open' : 'closed', learningSessionConfigurationStore.activeCustomSettings ? 'activeCustomSettings' : '', isMobile ? 'is-mobile' : '']">
                <template v-if="isMobile">
                    <div class="mobile-filter-icon">
                        <font-awesome-icon :icon="['fas', 'filter']" :class="{ 'is-active': showFilterDropdown }" />
                    </div>
                </template>
                <template v-else>
                    {{ t('page.learningSessionConfiguration.filter') }}
                    <div>
                        <font-awesome-icon v-if="showFilterDropdown" icon="fa-solid fa-chevron-up"
                            class="filter-button-icon" />
                        <font-awesome-icon v-else icon="fa-solid fa-chevron-down" class="filter-button-icon" />
                    </div>
                </template>

            </div>
            <slot></slot>
        </div>

        <div v-show="showFilterDropdown" class="session-config-dropdown row">
            <div class="dropdown-container col-xs-12 col-sm-6" v-click-outside="closeQuestionFilterDropdown" @click.self="closeQuestionFilterDropdown">
                <div class="sub-header" @click="closeQuestionFilterDropdown">{{ t('page.learningSessionConfiguration.questions') }}</div>
                <div class="question-filter-options-button selectable-item" @click="showQuestionFilterOptionsDropdown = !showQuestionFilterOptionsDropdown" :class="{ 'is-open': showQuestionFilterOptionsDropdown }">
                    <div v-if="learningSessionConfigurationStore.allQuestionFilterOptionsAreSelected">
                        {{ t('page.learningSessionConfiguration.allQuestions') }}
                    </div>
                    <div v-else-if="learningSessionConfigurationStore.selectedQuestionFilterOptionsDisplay.length === 0" class="button-placeholder">
                        {{ t('page.learningSessionConfiguration.chooseYourQuestions') }}
                    </div>
                    <div v-else class="question-filter-options-icon-container">
                        <template v-for="o in learningSessionConfigurationStore.selectedQuestionFilterOptionsDisplay">
                            <font-awesome-icon v-if="o.isSelected" :icon="o.icon" class="filter-icon" />
                        </template>
                        <div class="icon-counter" v-if="learningSessionConfigurationStore.selectedQuestionFilterOptionsExtraCount >= 2">
                            +{{ learningSessionConfigurationStore.selectedQuestionFilterOptionsExtraCount }}</div>
                    </div>

                    <font-awesome-icon v-if="showQuestionFilterOptionsDropdown" icon="fa-solid fa-chevron-up" />
                    <font-awesome-icon v-else icon="fa-solid fa-chevron-down" />

                </div>
                <div v-if="showQuestionFilterOptionsDropdown" class="question-filter-options-dropdown">
                    <div @click="learningSessionConfigurationStore.selectAllQuestionFilter()" class="selectable-item dropdown-item" :class="{ 'item-disabled': !userStore.isLoggedIn }">

                        <font-awesome-icon icon="fa-solid fa-square-check" class="session-select active" v-if="learningSessionConfigurationStore.allQuestionFilterOptionsAreSelected" />
                        <font-awesome-icon icon="fa-regular fa-square" class="session-select" v-else />
                        <div class="selectable-item" :class="{ 'item-disabled': !userStore.isLoggedIn }">
                            {{ t('page.learningSessionConfiguration.chooseAll') }}
                        </div>
                    </div>
                    <div class="dropdown-divider"></div>

                    <div v-for="q in learningSessionConfigurationStore.questionFilterOptions" @click="learningSessionConfigurationStore.selectQuestionFilter(q)" class="dropdown-item selectable-item"
                        :class="{ 'item-disabled': !userStore.isLoggedIn }">
                        <font-awesome-icon icon="fa-solid fa-square-check" class="session-select active" v-if="q.isSelected" />
                        <font-awesome-icon icon="fa-regular fa-square" class="session-select" v-else />
                        <font-awesome-icon class="dropdown-filter-icon" :icon="q.icon" />

                        <div class="selectable-item dropdown-item-label" :class="{ 'item-disabled': !userStore.isLoggedIn }">
                            {{ t(q.label) }} ({{ q.count }})
                        </div>
                    </div>

                </div>
            </div>

            <div class="col-xs-12 col-sm-6 question-counter-container">
                <div class="sub-header">{{ t('page.learningSessionConfiguration.maxQuestionsLabel') }}</div>
                <div class="question-counter"
                    :class="{ 'input-is-active': learningSessionConfigurationStore.questionCountInputFocused, 'input-error': learningSessionConfigurationStore.selectedQuestionCount < 1 && learningSessionConfigurationStore.userHasChangedMaxCount }">
                    <input type="number" min="0" v-model="learningSessionConfigurationStore.selectedQuestionCount" @input="(event: any) => learningSessionConfigurationStore.setSelectedQuestionCount(event.target.value)"
                        @focus="learningSessionConfigurationStore.questionCountInputFocused = true" @blur="learningSessionConfigurationStore.questionCountInputFocused = false" />
                    <div class="question-counter-selector-container">
                        <div class="question-counter-selector selectable-item" @click="learningSessionConfigurationStore.selectQuestionCount(1)">
                            <font-awesome-icon icon="fa-solid fa-chevron-up" />
                        </div>
                        <div class="question-counter-selector  selectable-item" @click="learningSessionConfigurationStore.selectQuestionCount(-1)">
                            <font-awesome-icon icon="fa-solid fa-chevron-down" />
                        </div>
                    </div>

                </div>
                <div v-if="learningSessionConfigurationStore.selectedQuestionCount < 1 && learningSessionConfigurationStore.userHasChangedMaxCount" class="input-error-label">
                    {{ t('page.learningSessionConfiguration.chooseAtLeastOne') }}
                </div>
            </div>

            <div class="dropdown-container col-xs-12 col-sm-6" v-click-outside="closeKnowledgeSummaryDropdown" @click.self="closeKnowledgeSummaryDropdown">
                <div class="sub-header" @click="closeKnowledgeSummaryDropdown">
                    {{ t('page.learningSessionConfiguration.knowledgeStatus') }}
                </div>

                <div class="knowledge-summary-button selectable-item" @click="showKnowledgeSummaryDropdown = !showKnowledgeSummaryDropdown" :class="{ 'is-open': showKnowledgeSummaryDropdown }">
                    <div v-if="learningSessionConfigurationStore.knowledgeSummaryCount === 0" class="button-placeholder">
                        {{ t('page.learningSessionConfiguration.chooseKnowledgeStatus') }}
                    </div>
                    <div class="knowledge-summary-chip-container">
                        <template v-for="s in learningSessionConfigurationStore.knowledgeSummary">
                            <div v-if="s.isSelected" class="knowledge-summary-chip" :class="s.colorClass">
                                <template v-if="learningSessionConfigurationStore.knowledgeSummaryCount === 1">
                                    {{ t(s.label) }}
                                </template>
                            </div>
                        </template>
                    </div>

                    <font-awesome-icon v-if="showKnowledgeSummaryDropdown" icon="fa-solid fa-chevron-up" />
                    <font-awesome-icon v-else icon="fa-solid fa-chevron-down" />
                </div>
                <div v-if="showKnowledgeSummaryDropdown" class="knowledge-summary-dropdown">
                    <div class="selectable-item dropdown-item" @click="learningSessionConfigurationStore.selectAllKnowledgeSummary()" :class="{ 'item-disabled': !userStore.isLoggedIn }">
                        <font-awesome-icon icon="fa-solid fa-square-check" class="session-select active" v-if="learningSessionConfigurationStore.allKnowledgeSummaryOptionsAreSelected" />
                        <font-awesome-icon icon="fa-regular fa-square" class="session-select" v-else />
                        <div class="selectable-item" :class="{ 'item-disabled': !userStore.isLoggedIn }">
                            {{ t('page.learningSessionConfiguration.chooseAll') }}
                        </div>
                    </div>
                    <div class="dropdown-divider"></div>
                    <div v-for="k in learningSessionConfigurationStore.knowledgeSummary" class="dropdown-item selectable-item" :class="{ 'item-disabled': !userStore.isLoggedIn }"
                        @click="learningSessionConfigurationStore.selectKnowledgeSummary(k)">
                        <font-awesome-icon icon="fa-solid fa-square-check" class="session-select active" v-if="k.isSelected" />
                        <font-awesome-icon icon="fa-regular fa-square" class="session-select" v-else />
                        <div :class="k.colorClass" class="knowledge-summary-chip">
                            {{ t(k.label) }} ({{ k.count }})
                        </div>
                    </div>

                </div>
            </div>

            <div class="dropdown-container col-xs-12 col-sm-6" v-click-outside="closeModeSelectionDropdown" @click.self="closeModeSelectionDropdown">
                <div class="sub-header" @click="closeModeSelectionDropdown">Modus</div>

                <div class="mode-change-button selectable-item" @click="showModeSelectionDropdown = !showModeSelectionDropdown" :class="{ 'is-open': showModeSelectionDropdown }">
                    <div v-if="learningSessionConfigurationStore.isTestMode">
                        <font-awesome-icon icon="fa-solid fa-graduation-cap" class="dropdown-filter-icon" /> {{ t('page.learningSessionConfiguration.test') }}
                    </div>
                    <div v-if="learningSessionConfigurationStore.isPracticeMode">
                        <font-awesome-icon icon="fa-solid fa-lightbulb" class="dropdown-filter-icon" /> {{ t('page.learningSessionConfiguration.learn') }}
                    </div>

                    <font-awesome-icon v-if="showModeSelectionDropdown" icon="fa-solid fa-chevron-up" />
                    <font-awesome-icon v-else icon="fa-solid fa-chevron-down" />
                </div>
                <div v-if="showModeSelectionDropdown" class="mode-change-dropdown">
                    <div>
                        <div class="mode-group-container" @click="learningSessionConfigurationStore.selectPracticeMode"
                            :class="{ 'selectable-item': !learningSessionConfigurationStore.isPracticeMode, 'no-pointer': learningSessionConfigurationStore.isPracticeMode }">
                            <div class="dropdown-item mode-change-header" :class="{ 'no-pointer': learningSessionConfigurationStore.isPracticeMode }">
                                <div class="dropdown-item" :class="{ 'no-pointer': learningSessionConfigurationStore.isPracticeMode }">
                                    <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-select active" v-if="learningSessionConfigurationStore.isPracticeMode" />
                                    <font-awesome-icon icon="fa-regular fa-circle" class="session-select" v-else />

                                    <div>
                                        <font-awesome-icon icon="fa-solid fa-lightbulb" class="dropdown-filter-icon" />
                                        {{ t('page.learningSessionConfiguration.learn') }}
                                    </div>
                                </div>
                                <font-awesome-icon v-if="learningSessionConfigurationStore.isPracticeMode" icon="fa-solid fa-chevron-up" />
                                <font-awesome-icon v-else icon="fa-solid fa-chevron-down" />
                            </div>
                            <div class="mode-item-container">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.learnDescription') }}
                                </div>
                            </div>
                        </div>
                        <div v-if="learningSessionConfigurationStore.isPracticeMode" class="mode-group-container">

                            <div class="mode-item-container selectable-item" @click="learningSessionConfigurationStore.selectPracticeOption('questionOrder', 0)">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.sortByEasy') }}
                                </div>
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-mini-select active" v-if="learningSessionConfigurationStore.practiceOptions.questionOrder === 0" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="session-mini-select" v-else />
                            </div>
                            <div class="mode-item-container selectable-item" @click="learningSessionConfigurationStore.selectPracticeOption('questionOrder', 1)">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.sortByHard') }}
                                </div>
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-mini-select active" v-if="learningSessionConfigurationStore.practiceOptions.questionOrder === 1" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="session-mini-select" v-else />
                            </div>
                            <div class="mode-item-container selectable-item" @click="learningSessionConfigurationStore.selectPracticeOption('questionOrder', 2)" :class="{ 'item-disabled': !userStore.isLoggedIn }">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.sortByNotKnown') }}
                                </div>
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-mini-select active" v-if="learningSessionConfigurationStore.practiceOptions.questionOrder === 2" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="session-mini-select" v-else />
                            </div>
                            <div class="mode-item-container selectable-item" @click="learningSessionConfigurationStore.selectPracticeOption('questionOrder', 3)">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.sortByRandom') }}
                                </div>
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-mini-select active" v-if="learningSessionConfigurationStore.practiceOptions.questionOrder === 3" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="session-mini-select" v-else />
                            </div>

                            <div class="dropdown-spacer"></div>

                            <div class="mode-item-container selectable-item" @click="learningSessionConfigurationStore.selectPracticeOption('repetition', 0)">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.noRepeat') }}
                                </div>
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-mini-select active" v-if="learningSessionConfigurationStore.practiceOptions.repetition === 0" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="session-mini-select" v-else />
                            </div>
                            <div class="mode-item-container selectable-item" @click="learningSessionConfigurationStore.selectPracticeOption('repetition', 1)">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.repeatWrong') }}
                                </div>
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-mini-select active" v-if="learningSessionConfigurationStore.practiceOptions.repetition === 1" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="session-mini-select" v-else />
                            </div>
                            <div class="mode-item-container item-disabled selectable-item">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.repeatByLeitner') }} <i>({{ t('page.learningSessionConfiguration.comingSoon') }})</i>
                                </div>
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-mini-select active" v-if="learningSessionConfigurationStore.practiceOptions.repetition === 2" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="session-mini-select" v-else />
                            </div>

                            <div class="dropdown-spacer"></div>
                        </div>

                    </div>
                    <div class="dropdown-divider"></div>

                    <div>
                        <div class="mode-group-container" @click="learningSessionConfigurationStore.selectTestMode"
                            :class="{ 'selectable-item': !learningSessionConfigurationStore.isTestMode, 'no-pointer': learningSessionConfigurationStore.isTestMode }">
                            <div class="dropdown-item mode-change-header" :class="{ 'no-pointer': learningSessionConfigurationStore.isTestMode }">
                                <div class="dropdown-item" :class="{ 'no-pointer': learningSessionConfigurationStore.isTestMode }">
                                    <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-select active" v-if="learningSessionConfigurationStore.isTestMode" />
                                    <font-awesome-icon icon="fa-regular fa-circle" class="session-select" v-else />
                                    <div>
                                        <font-awesome-icon icon="fa-solid fa-graduation-cap" class="dropdown-filter-icon" />
                                        {{ t('page.learningSessionConfiguration.test') }}
                                    </div>
                                </div>
                                <font-awesome-icon v-if="learningSessionConfigurationStore.isTestMode" icon="fa-solid fa-chevron-up" />
                                <font-awesome-icon v-else icon="fa-solid fa-chevron-down" />
                            </div>
                            <div class="mode-item-container">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.testDescription') }}
                                </div>
                            </div>
                        </div>

                        <div v-if="learningSessionConfigurationStore.isTestMode" class="mode-group-container">
                            <div class="mode-item-container  selectable-item" @click="learningSessionConfigurationStore.selectTestOption('questionOrder', 3)">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.sortByRandom') }}
                                </div>
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-mini-select active" v-if="learningSessionConfigurationStore.testOptions.questionOrder === 3" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="session-mini-select" v-else />
                            </div>
                            <div class="mode-item-container  selectable-item" @click="learningSessionConfigurationStore.selectTestOption('questionOrder', 0)">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.SortByEasy') }}
                                </div>
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-mini-select active" v-if="learningSessionConfigurationStore.testOptions.questionOrder === 0" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="session-mini-select" v-else />
                            </div>
                            <div class="mode-item-container  selectable-item" @click="learningSessionConfigurationStore.selectTestOption('questionOrder', 1)">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.sortByHard') }}
                                </div>
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-mini-select active" v-if="learningSessionConfigurationStore.testOptions.questionOrder === 1" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="session-mini-select" v-else />
                            </div>
                            <div class="mode-item-container  selectable-item" @click="learningSessionConfigurationStore.selectTestOption('questionOrder', 2)">
                                <div class="mode-sub-label">
                                    {{ t('page.learningSessionConfiguration.sortByNotKnown') }}
                                </div>
                                <font-awesome-icon icon="fa-solid fa-circle-dot" class="session-mini-select active" v-if="learningSessionConfigurationStore.testOptions.questionOrder === 2" />
                                <font-awesome-icon icon="fa-regular fa-circle" class="session-mini-select" v-else />
                            </div>
                            <div class="dropdown-spacer"></div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-xs-12 reset-session-button-container">
                <div class="reset-session-button" @click="learningSessionConfigurationStore.reset" :class="{ 'disabled': !learningSessionConfigurationStore.activeCustomSettings }">
                    <font-awesome-icon icon="fa-solid fa-xmark" class="reset-icon" />
                    <div>
                        {{ t('page.learningSessionConfiguration.resetAllFilters') }}
                    </div>
                </div>

            </div>
            <div v-if="learningSessionConfigurationStore.showSelectionError" class="session-config-error fade in col-xs-12">
                <div>
                    {{ t('page.learningSessionConfiguration.noQuestionsFoundChangeFilters') }}
                </div>
                <button class="close-alert-btn" @click="learningSessionConfigurationStore.showSelectionError = false">
                    <font-awesome-icon icon="fa-solid fa-xmark" />
                </button>
            </div>
        </div>

    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';
@import '~~/assets/shared/register.less';

.close-alert-btn {
    background: none;
    margin-left: 6px;
    padding: 12px 6px;
    cursor: pointer;
}

.opacity {
    opacity: 0.5;
}

.session-configurator {
    width: 100%;
    margin-top: 30px;
    margin-bottom: 30px;
    color: @memo-grey-darker;

    .filter-button,
    .knowledge-summary-button,
    .mode-change-button,
    .question-counter,
    .question-filter-options-button {
        position: relative;
        border: solid 1px @memo-grey-light;
        background: white;
        display: flex;
    }

    .knowledge-summary-button,
    .mode-change-button,
    .question-counter,
    .question-filter-options-button {
        z-index: 6;
        height: 40px;
        align-items: center;
        padding: 0 10px;
        justify-content: space-between;
    }

    .question-counter {
        display: flex;
        flex-wrap: nowrap;
        justify-content: space-between;
        padding-right: 5px;


        input {
            width: 100%;
            border: none !important;
            color: @memo-grey-dark;
        }

        input:focus,
        textarea:focus,
        select:focus {
            outline: none;
            color: @memo-blue;
        }

        .question-counter-selector-container {
            display: flex;
            flex-wrap: nowrap;
            align-items: center;

            .question-counter-selector {
                padding: 0 5px;
            }
        }
    }

    .filter-button {
        padding-top: 5px;
        height: 30px;
        min-width: 105px;
        justify-content: center;
        text-transform: uppercase;
        z-index: 5;

        &:hover {
            color: @memo-blue;
        }

        &.is-mobile {
            min-width: 50px;

            .mobile-filter-icon {
                color: @memo-blue;

                .is-active {
                    color: @memo-blue-link;
                }
            }
        }
    }

    .dropdown-container,
    .question-counter-container {
        margin-bottom: 10px;

        .sub-header {
            font-size: 12px;
            font-style: normal;
            font-weight: 600;
            line-height: 18px;
            letter-spacing: 1.25px;
            text-transform: uppercase;
            padding: 10px 0 5px 0;
        }

        .dropdown-item {
            min-height: 40px;
            font-family: Open Sans;
            font-size: 14px;
            font-style: normal;
            font-weight: 400;
            line-height: 19px;
            letter-spacing: 0em;
            display: flex;
            align-items: center;
        }

        .button-placeholder {
            float: left;
            color: #aaa;
            font-style: italic;
        }
    }

    .session-config-dropdown {
        position: relative;
        padding: 10px;
        padding-top: 11px;
        border: 1px solid @memo-grey-light;
        margin-top: -1px;
        z-index: 3;
        margin-left: 0px;
        margin-right: 0px;
        background: white;

        .question-filter-options-dropdown,
        .knowledge-summary-dropdown,
        .mode-change-dropdown {
            border: solid 1px @memo-grey-light;
            border-top: none;
            position: absolute;
            right: 0;
            margin-right: 10px;
            left: 0;
            margin-left: 10px;
            background: white;
            z-index: 10;

            .dropdown-item {
                display: flex;
                flex-direction: row;
                cursor: pointer;
                user-select: none;
                padding: 0 6px;
                flex-wrap: wrap;

                .dropdown-item-label {
                    padding-left: 4px;
                }
            }

            .dropdown-divider {
                border-bottom: solid 1px @memo-grey-light;
                width: 100%;
            }

            .dropdown-spacer {
                height: 14px;
                width: 100%;
            }
        }

        .session-select {
            font-size: 18px;
            margin-right: 15px;
            margin-left: 10px;

            &.active {
                color: @memo-blue-link;
            }
        }

        .item-disabled {
            .session-select {
                &.active {
                    color: @memo-grey-dark;
                }
            }
        }

        .session-mini-select {
            &.active {
                color: @memo-blue-link;
            }
        }
    }

    /* Chrome, Safari, Edge, Opera */
    input::-webkit-outer-spin-button,
    input::-webkit-inner-spin-button {
        -webkit-appearance: none;
        margin: 0;
    }

    /* Firefox */
    input[type=number] {
        -moz-appearance: textfield;
        appearance: textfield;
        border: none;
    }
}

.knowledge-summary-chip-container {
    display: flex;
    flex-wrap: nowrap;
}

.knowledge-summary-chip {
    height: 20px;
    min-width: 20px;
    border-radius: 10px;
    margin-right: 4px;
    font-size: 10px;
    line-height: 20px;
    padding: 0 6px;
    font-size: 10px;
    font-style: normal;
    font-weight: 600;
    letter-spacing: 0px;

}

.session-configurator {
    .not-learned {
        background-color: @not-learned-color;
    }

    .needs-learning {
        background-color: @needs-learning-color;
    }

    .needs-consolidation {
        background-color: @needs-consolidation-color;
    }

    .solid {
        background-color: @solid-knowledge-color;
    }

    .input-is-active,
    .is-open {
        border: solid 1px @memo-green;
    }

    .input-error {
        border: solid 1px @memo-salmon;
    }

    .input-error-label {
        position: absolute;
        font-size: 12px;
        color: @memo-salmon;
    }
}


.filter-button {
    &.open {
        border-bottom: none !important;
    }

    &.closed {
        border-bottom: 1px solid @memo-grey-light;
    }

    &.activeCustomSettings {
        color: @memo-blue-link;

        i {
            color: @memo-blue-link;
        }
    }

    .filter-button-icon {
        margin-left: 4px;
    }
}

#QuestionListApp {
    .session-configurator {
        margin-bottom: 10px;

        .session-config-dropdown {
            margin-right: 20px;
        }
    }
}

.session-config-header-container {
    display: flex;
    flex-wrap: nowrap;
    align-items: center;
    gap: 1rem;

    .session-progress-bar {
        font-size: @font-size-base;
        line-height: @line-height-base;
        display: flex;
        width: 100%;
        margin-left: 10px;
        height: 30px;
        background: @memo-grey-lighter;
        justify-content: space-between;
        flex-wrap: nowrap;
        position: relative;

        .step-count {
            display: flex;
            padding-left: 10px;
            padding-right: 15px;
            flex-wrap: nowrap;
            align-items: center;
            z-index: 2;
        }

        .progress-percentage {
            display: flex;
            align-items: center;
            padding-right: 10px;
            padding-left: 15px;
            z-index: 2;
        }

        .progress-bar {
            position: absolute;
            background: @memo-green;
            height: 100%;
            left: 0;
            right: 0;
        }
    }
}

.selectable-item {
    cursor: pointer;
    user-select: none;
    color: @memo-grey-dark;
    transition: background ease-in-out 0.1s;

    i {
        color: @memo-grey-dark;
    }

    &:hover {
        color: @memo-blue;
        background: @memo-grey-lighter;

        i {
            color: @memo-blue;
        }
    }

    &.item-disabled {
        cursor: not-allowed !important;
        color: @memo-grey-dark;

        i {
            color: @memo-grey-dark;
        }
    }
}

.session-title {
    font-size: 14px;
    font-style: normal;
    font-weight: 600;
    line-height: 20px;
    letter-spacing: 1.25px;
    text-align: center;
}

.reset-session-button-container {
    display: flex;
    justify-content: flex-end;

    .reset-session-button {
        text-align: right;
        font-size: 12px;
        font-style: normal;
        font-weight: 400;
        line-height: 18px;
        letter-spacing: 0px;
        display: flex;
        flex-wrap: nowrap;
        justify-content: flex-end;
        align-items: center;
        color: @memo-blue;

        .reset-icon {
            padding-right: 4px;
        }

        &:hover {
            color: @memo-blue-link;
            cursor: pointer;
        }

        &.disabled {
            cursor: not-allowed;
            color: @memo-grey-dark;
        }
    }
}

.question-filter-options-icon-container {
    display: flex;
    flex-wrap: nowrap;
    align-items: center;

    .filter-icon {
        padding-right: 4px;
    }

    .icon-counter {
        border-radius: 50px;
        border: solid 1px @memo-grey-dark;
        width: 25px;
        height: 25px;
        align-items: center;
        justify-content: center;
        display: flex;
        font-size: 12px;
        line-height: 25px;
        font-weight: 600;
    }
}

.dropdown-filter-icon {
    width: 18px;
}

.mode-change-header {
    display: flex;
    justify-content: space-between;
    flex-wrap: nowrap !important;
    padding-right: 10px !important;
}

.mode-group-container {
    padding-bottom: 10px;

    .mode-item-container {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 0 10px 0 54px;

        &:hover {
            color: @memo-blue;
        }

        .mode-sub-label {
            font-size: 14px;
            font-style: normal;
            font-weight: 400;
            letter-spacing: 0px;
            text-align: left;
        }

        input[type=number] {
            text-align: right;
            width: 100px;
        }
    }
}

.item-disabled {
    color: @memo-grey-dark !important;

    &:hover {
        color: @memo-grey-dark !important;
    }
}

.no-pointer {
    cursor: auto !important;
}

.session-config-error {
    width: 100%;
    font-size: 14px;
    margin-top: 20px;
    display: flex;
    justify-content: space-between;
    align-items: center;
    background: #fff7cc;
    padding: 20px;

    .selectable-item {
        &:hover {
            background: none;
        }
    }

    .close-selection-error-button {
        padding-left: 12px;
        text-align: right;
    }
}
</style>
