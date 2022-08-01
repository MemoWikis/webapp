<script lang="ts">
export default {
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
            id: parseInt(this.categoryId),
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
        this.$nextTick(() => {
            // Images.ReplaceDummyImages();
        });
        // eventBus.$on('publish-category',
        //     id => {
        //         if (this.id == id)
        //             this.visibility = 0;
        //     });
        // $('.show-tooltip').tooltip();
    },
    watch: {
        selectedCategories() {
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
        mouseOver(event) {
            event.stopPropagation();
            this.hover = true;
        },
        mouseLeave(event) {
            event.stopPropagation();
            this.hover = false;
        },
        goToCategory() {
            window.location.href = this.category.LinkToCategory;
        },
        init() {
            this.dropdownId = this.segmentId + '-Dropdown' + this.id;
            this.checkboxId = this.segmentId + '-Checkbox' + this.id;
            if (this.isCustomSegment)
                this.dropdownId += this.$parent.id;

            this.visibility = this.category.Visibility;
        },

        thisToSegment() {
            // if (NotLoggedIn.Yes()) {
            //     NotLoggedIn.ShowErrorMsg("MoveToSegment");
            //     return;
            // }
            if (!this.isCustomSegment) {
                this.$parent.loadSegment(this.id);
            }
        },
        removeParent() {
            // if (NotLoggedIn.Yes()) {
            //     NotLoggedIn.ShowErrorMsg("RemoveParent");
            //     return;
            // }
            var self = this;
            var data = {
                parentCategoryIdToRemove: self.$parent.categoryId,
                childCategoryId: self.categoryId,
            };
            // $.ajax({
            //     type: 'Post',
            //     contentType: "application/json",
            //     url: '/EditCategory/RemoveParent',
            //     data: JSON.stringify(data),
            //     success: function (data) {
            //         if (data.success == true) {
            //             self.$parent.currentChildCategoryIds = self.$parent.currentChildCategoryIds.filter((id) => {
            //                 return id != self.categoryId;
            //             });
            //             self.$parent.categories = self.$parent.categories.filter((c) => {
            //                 return c.Id != self.categoryId;
            //             });

            //             Alerts.showSuccess({
            //                 text: messages.success.category[data.key]
            //             });
            //         }
            //         else {
            //             Alerts.showError({
            //                 text: messages.error.category[data.key]
            //             });
            //         }
            //     },
            // });
        },
        openMoveCategoryModal() {
            // if (NotLoggedIn.Yes()) {
            //     NotLoggedIn.ShowErrorMsg("MoveCategory");
            //     return;
            // }

            var self = this;
            var data = {
                parentCategoryIdToRemove: self.$parent.categoryId,
                childCategoryId: self.categoryId,
            };
            // eventBus.$emit('open-move-category-modal', data);
        },
        hideCategory() {
            this.$parent.filterChildren([this.categoryId]);
        },
        openPublishModal() {
            // eventBus.$emit('open-publish-category-modal', this.categoryId);
        },
        openAddToWikiModal() {
            // eventBus.$emit('add-to-wiki', this.categoryId);
        }

    }
}

</script>

<template>
    <div class="col-xs-6 topic segmentCategoryCard" v-if="visible" @mouseover="mouseOver" @mouseleave="mouseLeave"
        :class="{ hover: showHover }">
        <div class="row" v-on:click.self="goToCategory()">
            <div class="col-xs-3">
                <a :href="category.LinkToCategory">
                    <div class="ImageContainer" v-html="category.ImgHtml">
                    </div>
                </a>
            </div>
            <div class="col-xs-9">
                <div class="topic-name">

                    <NuxtLink :href="category.LinkToCategory">
                        <template v-html="category.CategoryTypeHtml"></template> {{ category.Name }}
                    </NuxtLink>

                    <div v-if="visibility == 1" class="segmentCardLock" @click="openPublishModal" data-toggle="tooltip"
                        title="Thema ist privat. Zum Veröffentlichen klicken.">
                        <i class="fas fa-lock"></i>
                        <i class="fas fa-unlock"></i>
                    </div>
                </div>
                <div v-if="!isHistoric" class="Button dropdown DropdownButton"
                    :class="{ hover: showHover && !isHistoric }">
                    <a href="#" :id="dropdownId" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis"
                        type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                        <li v-if="!isCustomSegment">
                            <a @click="thisToSegment">
                                <div class="dropdown-icon">
                                    <i class="fas fa-sitemap"></i>
                                </div>Unterthemen einblenden
                            </a>
                        </li>
                        <li>
                            <a @click="removeParent">
                                <div class="dropdown-icon">
                                    <i class="fas fa-unlink"></i>
                                </div>Verknüpfung entfernen
                            </a>
                        </li>
                        <li v-if="visibility == 1">
                            <a @click="openPublishModal">
                                <div class="dropdown-icon">
                                    <i class="fas fa-unlock"></i>
                                </div>Thema veröffentlichen
                            </a>
                        </li>
                        <li>
                            <a @click="openMoveCategoryModal()">
                                <div class="dropdown-icon">
                                    <i class="fa fa-arrow-circle-right"></i>
                                </div>Thema verschieben
                            </a>
                        </li>
                        <li>
                            <a @click="openAddToWikiModal()" data-allowed="logged-in">
                                <div class="dropdown-icon">
                                    <i class="fa fa-plus-square"></i>
                                </div>
                                Zu meinem Wiki hinzufügen
                            </a>
                        </li>
                    </ul>
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