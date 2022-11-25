<script>
import { useUserStore } from '~~/components/user/userStore'
export default {
    props: {
        title: String,
        description: String,
        childCategoryIds: Array,
        categoryId: Number,
        editMode: Boolean,
        isHistoric: Boolean,
        parentId: Number
    },

    data() {
        return {
            categories: [],
            segmentId: null,
            cardsKey: null,
            isCustomSegment: true,
            selectedCategories: [],
            currentChildCategoryIds: [],
            currentChildCategoryIdsString: "",
            hover: false,
            showHover: false,
            addCategoryId: "",
            dropdownId: null,
            timer: null,
            linkToCategory: null,
            visibility: 0,
            segmentTitle: null,
            knowledgeBarHtml: null,
            disabled: true,
            knowledgeBarData: null
        };
    },
    mounted() {
        this.getSegmentData();
        this.segmentId = "Segment-" + this.categoryId;
        if (this.childCategoryIds != null) {
            var baseChildCategoryIds = JSON.parse(this.childCategoryIds);
            this.currentChildCategoryIds = baseChildCategoryIds;
        }
        this.addCategoryId = "AddCategoryTo-" + this.segmentId + "-Btn";
        this.dropdownId = this.segmentId + '-Dropdown';

        this.$on('select-category', (id) => this.selectCategory(id));
        this.$on('unselect-category', (id) => this.unselectCategory(id));
        eventBus.$on('add-category-card',
            (e) => {
                if (this.categoryId == e.parentId)
                    this.addNewCategoryCard(e.newCategoryId);
            });
        if (this.currentChildCategoryIds.length > 0)
            this.getCategoriesData();
    },

    watch: {
        hover(val) {
            if (val && this.editMode)
                this.showHover = true;
            else
                this.showHover = false;
        },
        currentChildCategoryIds() {
            this.currentChildCategoryIdsString = this.currentChildCategoryIds.join(',');
        },
        selectedCategoryIds(val) {
            this.disabled = val.length <= 0;
        }
    },

    updated() {
    },

    methods: {
        async addNewCategoryCard(id) {
            var self = this;
            var data = {
                categoryId: id,
            };

            let c = await $fetch('/Segmentation/GetCategoryData', {
                body: data,
                method: 'Post',
                credentials: 'include',
                mode: 'no-cors',
                headers: useRequestHeaders(['cookie']),
            })
            if (c) {
                self.categories.push(c)
                self.currentChildCategoryIds.push(c.Id)
                // self.$nextTick(() => Images.ReplaceDummyImages());
            }

        },
        async getCategoriesData() {
            var self = this;
            var data = {
                categoryIds: self.currentChildCategoryIds,
            };

            let result = await $fetch('/apiVue/Segmentation/GetCategoriesData', {
                body: data,
                method: 'Post',
                credentials: 'include',
                mode: 'no-cors',
                headers: useRequestHeaders(['cookie']),
            })
            if (result)
                result.forEach(c => self.categories.push(c))
        },
        async getSegmentData() {
            var self = this;
            var data = {
                categoryId: parseInt(self.categoryId),
            }

            let result = await $fetch < any > ('/apiVue/Segmentation/GetSegmentData', {
                body: data,
                method: 'Post',
                credentials: 'include',
            })
            if (result) {
                self.linkToCategory = result.linkToCategory;
                self.visibility = result.visibility;
                self.knowledgeBarHtml = result.knowledgeBarHtml;
                self.knowledgeBarData = result.knowledgeBarData;
                if (self.title)
                    self.segmentTitle = self.title;
                else self.segmentTitle = result.categoryName;
            }

        },
        selectCategory(id) {
            if (this.selectedCategories.includes(id))
                return;
            else this.selectedCategories.push(id);
        },
        unselectCategory(id) {
            if (this.selectedCategories.includes(id)) {
                var index = this.selectedCategories.indexOf(id);
                this.selectedCategories.splice(index, 1);
            }
        },
        updateCategoryOrder() {
            this.categories = $("#" + this.segmentId + " > .topic").map((idx, elem) => $(elem).attr("category-id")).get();
        },
        removeSegment() {
            if (NotLoggedIn.Yes()) {
                NotLoggedIn.ShowErrorMsg("RemoveSegment");
                return;
            }
            var data = {
                parentId: this.parentId,
                newCategoryId: this.categoryId
            };
            eventBus.$emit('remove-segment', parseInt(this.categoryId));
            eventBus.$emit('add-category-card', data);
        },
        addCategory(val) {
            // if (NotLoggedIn.Yes()) {
            //     NotLoggedIn.ShowErrorMsg("CreateCategory");
            //     return;
            // }
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                return
            }
            var self = this;
            var categoriesToFilter = Array.from(self.currentChildCategoryIds);
            categoriesToFilter.push(parseInt(self.categoryId));

            var parent = {
                id: self.categoryId,
                addCategoryBtnId: $("#" + self.addCategoryId),
                moveCategories: false,
                categoriesToFilter,
                editCategoryRelation: val ? EditTopicRelationType.Create : EditTopicRelationType.AddChild,

            }

            this.editTopicRelationStore.openModal(parent)
        },

        async removeChildren() {
            // if (NotLoggedIn.Yes()) {
            //     NotLoggedIn.ShowErrorMsg("RemoveChildren");
            //     return;
            // }
            const userStore = useUserStore()
            if (!userStore.isLoggedIn) {
                return
            }
            var self = this
            var data = {
                parentCategoryId: self.categoryId,
                childCategoryIds: self.selectedCategories,
            }

            let result = await $fetch < any > ('/api/EditCategory/RemoveChildren', {
                body: data,
                method: 'Post',
                credentials: 'include',
                mode: 'no-cors',
                headers: useRequestHeaders(['cookie']),
            })

            if (result) {
                var removedChildCategoryIds = JSON.parse(result.removedChildCategoryIds);
                self.filterChildren(removedChildCategoryIds);
            }
        },
        filterChildren(selectedCategoryIds) {
            let filteredCurrentChildCategoryIds = this.currentChildCategoryIds.filter(
                function (e) {
                    return this.indexOf(e) < 0;
                },
                selectedCategoryIds
            );
            this.currentChildCategoryIds = filteredCurrentChildCategoryIds;
            this.selectedCategoryIds = [];
            eventBus.$emit('save-segments');
        },
        hideChildren() {
            this.filterChildren(this.selectedCategories);
        },
        openPublishModal() {
            eventBus.$emit('open-publish-category-modal', this.categoryId);
        },
        openMoveCategoryModal() {
            var self = this;
            var data = {
                parentCategoryIdToRemove: self.$parent.categoryId,
                childCategoryId: self.categoryId,
            };
            eventBus.$emit('open-move-category-modal', data);
        },
        openAddToWikiModal() {
            eventBus.$emit('add-to-wiki', this.categoryId);
        }
    },
}
</script>

<template>
    <div class="segment" @mouseover="hover = true" @mouseleave="hover = false" :class="{ hover: showHover }">
        <div class="segmentSubHeader">
            <div class="segmentHeader">
                <div class="segmentTitle">
                    <a :href="linkToCategory">
                        <h2>
                            {{ segmentTitle }}
                        </h2>
                    </a>
                    <div v-if="visibility == 1" class="segmentLock" @click="openPublishModal" data-toggle="tooltip"
                        title="Thema ist privat. Zum Veröffentlichen klicken.">
                        <font-awesome-icon icon="fa-solid fa-lock" />
                        <font-awesome-icon icon="fa-solid fa-lock-open" />
                    </div>

                </div>
                <div v-if="!isHistoric" class="Button dropdown DropdownButton segmentDropdown"
                    :class="{ hover: showHover && !isHistoric }">
                    <a href="#" :id="d
                    opdownId" class="dropdown-toggle  btn btn-link btn-sm ButtonEllipsis"
                        type="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                        <i class="fa fa-ellipsis-v"></i>
                    </a>
                    <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                        <li @click="removeSegment()">
                            <a>
                                <div class="dropdown-icon">
                                    <img class="fas" src="/Images/Icons/sitemap-disable.svg" />
                                </div>Unterthema ausblenden
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
                            <a @click="openMoveCategoryModal()" data-allowed="logged-in">
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
            </div>

            <div class="segmentKnowledgeBar">
                <div class="KnowledgeBarWrapper" v-html="knowledgeBarHtml">
                </div>
            </div>
        </div>
        <div class="topicNavigation row" :key="cardsKey">

            <TopicContentSegmentationCard v-for="(category, index) in categories" @select-category="selectCategory"
                @unselect-category="unselectCategory" inline-template :ref="'card' + category.Id"
                :is-custom-segment="isCustomSegment" :category-id="category.Id"
                :selected-categories="selectedCategories" :segment-id="segmentId" hide="false" :key="index"
                :category="category" :is-historic="isHistoric" />
            <div v-if="!isHistoric" class="col-xs-6 topic">
                <div class="addCategoryCard memo-button row" :id="addCategoryId">
                    <div class="col-xs-3">
                    </div>
                    <div class="col-xs-9 addCategoryLabelContainer">
                        <div class="addCategoryCardLabel" @click="addCategory(true)">
                            <font-awesome-icon icon="fa-solid fa-plus" /> Neues Thema
                        </div>
                        <div class="addCategoryCardLabel" @click="addCategory(false)">
                            <font-awesome-icon icon="fa-solid fa-plus" /> Bestehendes Thema
                        </div>
                    </div>

                </div>
            </div>

        </div>
    </div>
</template>