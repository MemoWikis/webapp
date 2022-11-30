<script lang="ts">
import { useUserStore } from '~~/components/user/userStore'
import { useAlertStore, AlertType, messages } from '~~/components/alert/alertStore'
import { useEditTopicRelationStore, EditRelationData, EditTopicRelationType } from '../../relation/editTopicRelationStore'

export default defineNuxtComponent({
    props: {
        categoryId: [String, Number],
        isCustomSegment: Boolean,
        selectedCategories: Array,
        segmentId: [String, Number],
        hide: String,
        category: Object,
        isHistoric: Boolean,
    },

    data() {
        return {
            visible: true,
            hover: false,
            dropdownId: null,
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
                this.dropdownId += this.$parent.id;

            this.visibility = this.category!.Visibility;
        },

        thisToSegment() {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }
            if (!this.isCustomSegment) {
                this.$parent.loadSegment(this.id);
            }
        },
        async removeParent() {
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                userStore.openLoginModal()
                return
            }
            var self = this;
            var data = {
                parentCategoryIdToRemove: self.$parent.categoryId,
                childCategoryId: self.categoryId,
            };

            var result = await $fetch<any>('/apiVue/EditCategory/RemoveParent', {
                method: 'POST',
                body: data,
            })
            if (result) {
                const alertStore = useAlertStore()
                if (result.success == true) {
                    self.$parent.currentChildCategoryIds = self.$parent.currentChildCategoryIds.filter((id) => {
                        return id != self.categoryId;
                    });
                    self.$parent.categories = self.$parent.categories.filter((c) => {
                        return c.Id != self.categoryId;
                    });
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
        openMoveCategoryModal() {
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
            this.$parent.filterChildren([this.categoryId]);
        },
        openPublishModal() {
            // eventBus.$emit('open-publish-category-modal', this.categoryId);
        },
        openAddToWikiModal() {
            var data = {
                parentId: this.categoryId,
                editCategoryRelation: EditTopicRelationType.AddToWiki
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
            <div class="col-xs-3">
                <NuxtLink :to="category.LinkToCategory">
                    <div class="ImageContainer">
                        <Image :url="category.ImgUrl" :square="true" />
                    </div>
                </NuxtLink>
            </div>
            <div class="col-xs-9">
                <div class="topic-name">

                    <NuxtLink :href="category.LinkToCategory">
                        <template v-html="category.CategoryTypeHtml"></template> {{ category.Name }}
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
                        <template #popper>
                            <ul>
                                <li v-if="!isCustomSegment">
                                    <div @click="thisToSegment" class="dropdown-item">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon :icon="['fa-solid', 'sitemap']" />
                                        </div>
                                        Unterthemen einblenden
                                    </div>
                                </li>
                                <li>
                                    <div @click="removeParent" class="dropdown-item">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon :icon="['fa-solid', 'link-slash']" />
                                        </div>Verknüpfung entfernen
                                    </div>
                                </li>
                                <li v-if="visibility == 1">
                                    <div @click="openPublishModal" class="dropdown-item">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon :icon="['fa-solid', 'unlock']" />
                                        </div>Thema veröffentlichen
                                    </div>
                                </li>
                                <li>
                                    <div @click="openMoveCategoryModal()" class="dropdown-item">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon :icon="['fa-solid', 'circle-right']" />
                                        </div>Thema verschieben
                                    </div>
                                </li>
                                <li>
                                    <div @click="openAddToWikiModal()" data-allowed="logged-in" class="dropdown-item">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon :icon="['fa-solid', 'plus']" />
                                        </div>
                                        Zu meinem Wiki hinzufügen
                                    </div>
                                </li>
                            </ul>
                        </template>
                    </VDropdown>


                </div>
                <div class="set-question-count">
                    <a :href="category.LinkToCategory" class="sub-label">
                        <template v-if="category.ChildCategoryCount == 1">1 Unterthema</template>
                        <template v-else-if="category.ChildCategoryCount > 1">{{ category.ChildCategoryCount }}
                            Unterthemen</template>
                        <span v-if="category.QuestionCount > 0">{{ category.QuestionCount }} Frage<template
                                v-if="category.QuestionCount != 1">n</template></span>
                    </a>
                </div>
                <a :href="category.LinkToCategory">
                    <div v-if="category.QuestionCount > 0" class="KnowledgeBarWrapper">
                        <div v-html="category.KnowledgeBarHtml"></div>
                        <div class="KnowledgeBarLegend">Dein Wissensstand</div>
                    </div>
                </a>
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

        &.hover {
            cursor: pointer;
        }
    }

    #CustomSegmentSection,
    #GeneratedSegmentSection {
        .topicNavigation {
            margin-top: 20px;

            .segmentCategoryCard {

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

            .addCategoryCard {
                display: flex;
                border: solid 1px @memo-grey-light;
                transition: 0.2s;
                align-items: center;
                min-height: 150px;
                color: @memo-grey-dark;
                cursor: pointer;
                margin-left: 0;
                margin-right: 0;

                @media (max-width: 649px) {
                    width: 100%;
                }

                .addCategoryLabelContainer {
                    padding: 0;
                }

                &:hover {
                    border-color: @memo-green;
                }

                .addCategoryCardLabel {
                    transition: 0.2s;

                    &:hover {
                        color: @memo-green;
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
        margin-right: 10px;

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
        }
    }

    .segmentLock {
        height: 20px;

        i {
            font-size: 18px;
        }
    }
}
</style>