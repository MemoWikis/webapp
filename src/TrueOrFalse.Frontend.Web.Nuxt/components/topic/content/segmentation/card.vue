<script lang="ts">
import { useUserStore } from '~~/components/user/userStore'
import { useAlertStore, AlertType, messages } from '~~/components/alert/alertStore'
import { useEditTopicRelationStore, EditRelationData, EditTopicRelationType } from '../../relation/editTopicRelationStore'
import { usePublishTopicStore } from '../../publish/publishTopicStore'

export default defineNuxtComponent({
    props: {
        categoryId: { type: Number, required: true },
        isCustomSegment: Boolean,
        selectedCategories: Array,
        segmentId: [String, Number],
        hide: String,
        category: { type: Object, required: true },
        isHistoric: Boolean,
        parentTopicId: Number
    },
    data() {
        return {
            visible: true,
            hover: false,
            dropdownId: null as null | string,
            id: parseInt(this.$props.categoryId!.toString()),
            isSelected: false,
            checkboxId: '',
            showHover: false,
            imgHtml: null,
            linkToCategory: null,
            visibility: 0,
            categoryTypeHtml: null,
            childCategoryCount: 0,
            questionCount: 0,
            categoryName: null,
            knowledgeBarHtml: null,
        };
    },
    mounted() {
        // eventBus.$on('publish-category',
        //     id => {
        //         if (this.id == id)
        //             this.visibility = 0;
        //     });
        // $('.show-tooltip').tooltip();
    },
    watch: {
        selectedCategories() {
            if (this.selectedCategories != null)
                this.isSelected = this.selectedCategories.includes(this.id);
        },
        hover(val) {
            this.showHover = val;
        },
        categoryId() {
            this.init();
        }
    },
    created() {
        this.init();
    },
    methods: {
        mouseOver(event: any) {
            event.stopPropagation();
            this.hover = true;
        },
        mouseLeave(event: any) {
            event.stopPropagation();
            this.hover = false;
        },
        goToCategory() {
            window.location.href = this.category!.LinkToCategory;
        },
        init() {
            this.dropdownId = this.segmentId + '-Dropdown' + this.id;
            this.checkboxId = this.segmentId + '-Checkbox' + this.id;
            if (this.isCustomSegment)
                this.dropdownId += this.parentTopicId;

            this.visibility = this.category!.Visibility;
        },

        thisToSegment(hide: any) {
            hide()
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }
            if (!this.isCustomSegment) {
                this.$emit('load-segment', this.id)
            }
        },
        async removeParent(hide: any) {
            hide()
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }
            var self = this;
            var data = {
                parentCategoryIdToRemove: self.parentTopicId,
                childCategoryId: self.categoryId,
            };

            var result = await $fetch<any>('/apiVue/Card/RemoveParent', {
                method: 'POST',
                body: data,
            })
            if (result) {
                const alertStore = useAlertStore()
                if (result.success == true) {
                    this.$emit('remove-category', self.categoryId)
                    alertStore.openAlert(AlertType.Success, {
                        text: messages.success.category[result.key]
                    })
                }
                else {
                    alertStore.openAlert(AlertType.Error, {
                        text: messages.error.category[result.key]
                    })
                }
            }
        },
        openMoveCategoryModal(hide: any) {
            hide()
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }
            var self = this;
            var data = {
                parentId: self.$parentz.categoryId,
                childId: self.categoryId,
                editCategoryRelation: EditTopicRelationType.Move,
            } as EditRelationData

            const editTopicRelationStore = useEditTopicRelationStore()
            editTopicRelationStore.openModal(data)
        },
        hideCategory() {
            this.$emit('filter-children', [this.categoryId])
        },
        openPublishModal(hide: any) {
            hide()
            const publishTopicStore = usePublishTopicStore()
            publishTopicStore.openModal(this.categoryId)
        },
        openAddToWikiModal(hide: any) {
            hide()
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }
            var data = {
                parentId: this.categoryId,
                editCategoryRelation: EditTopicRelationType.AddToPersonalWiki
            } as EditRelationData

            const editTopicRelationStore = useEditTopicRelationStore()
            editTopicRelationStore.openModal(data)
        }

    }
})
</script>

<template>
    <div class="col-xs-6 topic segmentCategoryCard" v-show="visible" @mouseover="mouseOver" @mouseleave="mouseLeave"
        :class="{ hover: showHover }">
        <div class="row" v-on:click.self="goToCategory()">
            <div class="col-xs-4 col-sm-3">
                <NuxtLink :to="category.LinkToCategory">
                    <div class="ImageContainer">
                        <Image :url="category.ImgUrl" :square="true" />
                    </div>
                </NuxtLink>
            </div>
            <div class="col-xs-8 col-sm-9">
                <div class="topic-name">

                    <NuxtLink :href="$props.category.LinkToCategory">
                        <template v-html="$props.category.CategoryTypeHtml"></template> {{ $props.category.Name }}
                    </NuxtLink>

                    <div v-if="visibility == 1" class="segmentCardLock" @click="openPublishModal" data-toggle="tooltip"
                        title="Thema ist privat. Zum Veröffentlichen klicken.">
                        <font-awesome-icon :icon="['fa-solid', 'lock']" />
                        <font-awesome-icon :icon="['fa-solid', 'unlock']" />
                    </div>
                </div>
                <div class="Button dropdown DropdownButton" :class="{ hover: showHover && !isHistoric }">

                    <VDropdown :distance="1">
                        <div class="btn btn-link btn-sm ButtonEllipsis">
                            <font-awesome-icon :icon="['fa-solid', 'ellipsis-vertical']" />
                        </div>
                        <template #popper="{ hide }">
                            <div v-if="!isCustomSegment" @click="thisToSegment(hide)" class="dropdown-row">
                                <div class="dropdown-icon">
                                    <font-awesome-icon :icon="['fa-solid', 'sitemap']" />
                                </div>
                                <div class="dropdown-label"> Unterthemen einblenden</div>
                            </div>
                            <div @click="removeParent(hide)" class="dropdown-row">
                                <div class="dropdown-icon">
                                    <font-awesome-icon :icon="['fa-solid', 'link-slash']" />
                                </div>
                                <div class="dropdown-label">Verknüpfung entfernen </div>
                            </div>
                            <div v-if="visibility == 1" @click="openPublishModal(hide)" class="dropdown-row">
                                <div class="dropdown-icon">
                                    <font-awesome-icon :icon="['fa-solid', 'unlock']" />
                                </div>
                                <div class="dropdown-label">Thema veröffentlichen</div>
                            </div>
                            <div @click="openMoveCategoryModal(hide)" class="dropdown-row">
                                <div class="dropdown-icon">
                                    <font-awesome-icon :icon="['fa-solid', 'circle-right']" />
                                </div>
                                <div class="dropdown-label">Thema verschieben</div>
                            </div>
                            <div @click="openAddToWikiModal(hide)" data-allowed="logged-in" class="dropdown-row">
                                <div class="dropdown-icon">
                                    <font-awesome-icon :icon="['fa-solid', 'plus']" />
                                </div>
                                <div class="dropdown-label">Zu meinem Wiki hinzufügen</div>
                            </div>
                        </template>
                    </VDropdown>
                </div>
                <div class="set-question-count">

                    <NuxtLink :href="$props.category.LinkToCategory" class="sub-label">
                        <template v-if="$props.category.ChildCategoryCount == 1">1 Unterthema </template>
                        <template v-else-if="$props.category.ChildCategoryCount > 1">{{ category.ChildCategoryCount }}
                            Unterthemen </template>
                        <span v-if="$props.category.QuestionCount > 0">
                            {{ category.QuestionCount + ' Frage' + ($props.category.QuestionCount == 1 ? '' : 'n') }}
                        </span>
                    </NuxtLink>

                </div>
                <div v-if="$props.category.QuestionCount > 0" class="KnowledgeBarWrapper">

                    <NuxtLink :href="$props.category.LinkToCategory">
                        <div class="knowledge-bar">
                            <div v-if="$props.category.KnowledgeBarData.NeedsLearningPercentage > 0" class="needs-learning"
                                v-tooltip="'Solltest du lernen:' + $props.category.KnowledgeBarData.NeedsLearning + ' Fragen (' + $props.category.KnowledgeBarData.NeedsLearningPercentage + '%)'"
                                :style="{ 'width': $props.category.KnowledgeBarData.NeedsLearningPercentage + '%' }">
                            </div>

                            <div v-if="$props.category.KnowledgeBarData.NeedsConsolidationPercentage > 0"
                                class="needs-consolidation"
                                v-tooltip="'Solltest du lernen:' + $props.category.KnowledgeBarData.NeedsConsolidation + ' Fragen (' + $props.category.KnowledgeBarData.NeedsConsolidationPercentage + '%)'"
                                :style="{ 'width': $props.category.KnowledgeBarData.NeedsConsolidationPercentage + '%' }">
                            </div>

                            <div v-if="$props.category.KnowledgeBarData.SolidPercentage > 0" class="solid-knowledge"
                                v-tooltip="'Solltest du lernen:' + $props.category.KnowledgeBarData.Solid + ' Fragen (' + $props.category.KnowledgeBarData.SolidPercentage + '%)'"
                                :style="{ 'width': $props.category.KnowledgeBarData.SolidPercentage + '%' }"></div>

                            <div v-if="$props.category.KnowledgeBarData.NotLearnedPercentage > 0" class="not-learned"
                                v-tooltip="'Solltest du lernen:' + $props.category.KnowledgeBarData.NotLearned + ' Fragen (' + $props.category.KnowledgeBarData.NotLearnedPercentage + '%)'"
                                :style="{ 'width': $props.category.KnowledgeBarData.NotLearnedPercentage + '%' }"></div>
                        </div>
                        <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                    </NuxtLink>

                </div>
            </div>
        </div>
    </div>
</template>

<style lang="less" scoped>
@import (reference) '~~/assets/includes/imports.less';

li {
    .dropdown-item {
        cursor: pointer;
        display: flex;
        flex-wrap: nowrap;
    }
}

@memo-blue-link: #18A0FB;
@memo-blue: #203256;

#Segmentation {
    margin-top: 80px;
    margin-bottom: 40px;

    .toRoot {
        align-items: center;
        color: @memo-grey-darker;

        .category-chip-container {
            margin-top: -10px;
            margin-left: 4px;
            text-transform: initial;
            font-weight: initial;
            letter-spacing: normal;
        }
    }

    .overline-m {
        margin-bottom: 15px;
    }

    .segmentationHeader {
        font-family: 'Open Sans';
        display: flex;
        justify-content: space-between;
    }

    #GeneratedSegmentSection,
    .segment,
    .segmentCategoryCard {
        transition: 0.2s;

        .row {
            margin-top: 20px;
            margin-bottom: 25px;
        }

        &.hover {
            cursor: pointer;
        }
    }

    #CustomSegmentSection,
    #GeneratedSegmentSection {
        .topicNavigation {
            margin-top: 20px;

            .segmentCategoryCard {

                .ButtonEllipsis {
                    font-size: 18px;
                    color: @memo-grey-dark;
                    border-radius: 24px;
                    height: 30px;
                    width: 30px;
                    display: flex;
                    justify-content: center;
                    align-items: center;

                    &:hover {
                        background: @memo-grey-lighter;
                        color: @memo-blue;
                    }

                    &:active {
                        background: @memo-grey-light;
                    }
                }

                .topic-name {
                    padding: 0;
                }

                .checkBox {
                    position: absolute;
                    z-index: 3;
                    line-height: 0;
                    background: white;
                    color: @memo-green;
                    opacity: 0;
                    transition: opacity .1s ease-in-out;
                    transition: color .1s ease-in-out;


                    &.show {
                        opacity: 1;
                        transition: opacity .1s ease-in-out;
                        transition: color .1s ease-in-out;
                    }


                    &.selected {
                        color: @memo-green;
                        opacity: 1;
                        transition: opacity .1s ease-in-out;
                    }
                }
            }
        }
    }

    #CustomSegmentSection {
        .segment {
            .segmentSubHeader {
                .segmentKnowledgeBar {
                    max-width: 420px;
                }
            }
        }
    }

    .segmentHeader {
        display: inline-flex;
        width: 100%;
        justify-content: space-between;
        margin-top: 20px;
        margin-bottom: 10px;

        .segmentTitle {
            display: inline-flex;
            align-items: center;

            a {
                color: @memo-blue;
                transition: 0.2s;
                padding-right: 10px;

                &:hover {
                    text-decoration: none;
                    color: @memo-blue-link;
                }
            }

            h2 {
                margin: 0;
            }

            span.Button {
                padding-top: 10px;
                margin-left: 10px;
            }
        }
    }

    .segmentDropdown,
    .dropdown {
        font-size: 35px;
        opacity: 0;
        transition: all .1s ease-in-out;
    }

    .segmentDropdown,
    .dropdown {
        &.hover {
            opacity: 1;
            transition: all .1s ease-in-out;
        }
    }

    .DropdownButton {
        position: absolute;
        right: 10px;
        top: -10px;

        &.segmentDropdown {
            position: relative;
        }

        a.dropdown-toggle {
            background: #FFFFFFE6;
            border-radius: 50%;
            height: 40px;
            width: 40px;
            text-align: center;
            padding: 6px;
            transition: all .3s ease-in-out;

            .fa-ellipsis-vertical {
                color: @memo-grey-dark;
                transition: all .3s ease-in-out;
            }
        }
    }

    .hover {
        .DropdownButton {
            a.dropdown-toggle {
                &:hover {
                    background: #EFEFEFE6;

                    .fa-ellipsis-v {
                        color: @memo-blue;
                    }
                }
            }
        }
    }

    .set-question-count {
        .sub-label {
            color: @memo-grey-dark;
        }
    }

    .segmentCardLock,
    .segmentLock {
        cursor: pointer;
        display: inline-flex;
        align-items: center;
        margin-right: 4px;
        margin-left: 4px;
        background: white;
        width: 24px;
        height: 24px;
        justify-content: center;
        border-radius: 15px;

        .fa-unlock {
            display: none !important;
        }

        .fa-lock {
            display: unset !important;
        }

        &:hover {

            .fa-lock {
                display: none !important;
                color: @memo-blue;
            }

            .fa-unlock {
                display: unset !important;
                color: @memo-blue;
            }

            filter: brightness(0.95)
        }

        &:active {
            filter: brightness(0.85)
        }
    }

    .segmentLock {
        height: 20px;

        i {
            font-size: 18px;
        }
    }

    .topicNavigation,
    .setCardMiniList {
        display: flex;
        flex-wrap: wrap;
        align-content: space-between;
        justify-content: flex-start;
        margin-bottom: 20px;

        &.row:before,
        &.row:after {
            display: inline-block;
        }

        img {
            border-radius: 0;
        }

        a {
            color: @global-text-color;

            &:hover,
            &:active,
            &:focus {
                text-decoration: none;
            }
        }

        .set-question-count {
            color: @gray-light;
            margin-top: 8px;
            line-height: 22px;
        }

        .topic,
        .setCardMini {

            .row {
                margin-top: 20px;
                margin-bottom: 25px;
            }

            .stack-below(@extra-breakpoint-cards);

            .ImageContainer {
                max-width: 80px;
                min-width: 70px;

                .LicenseInfo {
                    text-align: center;
                    color: @gray-light;

                    &:after {
                        content: "Lizenz";
                    }
                }
            }

            .topic-name {
                max-height: 65px;
                display: flex;
                align-items: center;
                height: 100%;
                overflow: hidden;

                @media (max-width: (@extra-breakpoint-cards - 1px)) {
                    max-height: none;
                }
            }

            .KnowledgeBarLegend {
                .greyed;
                font-size: 12px;
                line-height: 1.5em;
                //text-transform: uppercase;
                opacity: 0;
                transition: opacity 0.2s linear;

                .media-below-sm ({
                    opacity: 1;
                });
        }

        &:hover {

            //show on hover over navigation tile
            .KnowledgeBarLegend {
                opacity: 1;
            }
        }
    }

    .knowledge-bar {
        display: inline-flex;
        margin-top: 15px;
        height: 10px;
        min-width: 150px;
        width: 100%;
        max-width: 180px;

        .solid-knowledge,
        .needs-learning,
        .needs-consolidation,
        .not-learned,
        .not-in-wish-knowledge {
            height: inherit;
            float: left;
        }

        .needs-learning {
            background-color: @needs-learning-color;
        }

        .needs-consolidation {
            background-color: @needs-consolidation-color;
        }

        .solid-knowledge {
            background-color: @solid-knowledge-color;
        }

        .not-learned {
            background-color: @not-learned-color;
        }

        .not-in-wish-knowledge {
            background-color: @not-in-wish-knowledge-color;
        }
    }
}
}
</style>