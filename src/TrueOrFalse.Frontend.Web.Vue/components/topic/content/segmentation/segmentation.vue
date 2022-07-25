<script lang="ts">
import { useUserStore } from '~~/components/user/userStore'
import { useTopicStore } from '~~/components/topic/topicStore'
import { EditTopicRelationType } from '../../relation/editTopicRelationEnum'
const userStore = useUserStore()
const topicStore = useTopicStore()
interface Segment {
    CategoryId: Number,
    Title: String,
    ChildCategoryIds: Array<Number>,
};


export default {
    props: {
        categoryId: [String, Number],
        editMode: Boolean,
        childCategoryIds: String,
        segmentJson: String,
        isHistoricString: String,
    },

    data() {
        return {
            baseCategoryList: [],
            componentKey: 0,
            selectedCategoryId: null,
            isCustomSegment: false,
            hasCustomSegment: false,
            selectedCategories: [],
            segmentId: 'SegmentationComponent',
            hover: false,
            showHover: false,
            addCategoryId: "AddToCurrentCategoryCard",
            dropdownId: "MainSegment-Dropdown",
            controlWishknowledge: false,
            loadComponents: true,
            currentChildCategoryIds: [],
            segments: [] as Segment[],
            categories: [],
            isHistoric: this.isHistoricString == 'True',
        };
    },
    mounted() {
        if (this.childCategoryIds != null)
            this.currentChildCategoryIds = JSON.parse(this.childCategoryIds);
        if (this.segmentJson.length > 0)
            this.segments = JSON.parse(this.segmentJson);
        this.hasCustomSegment = this.segments.length > 0;
        if (this.currentChildCategoryIds.length > 0)
            this.getCategoriesData();
        eventBus.$on('add-category-card',
            (e) => {
                if (e.parentId == this.categoryId)
                    this.addNewCategoryCard(e.newCategoryId);
            });

        eventBus.$on('category-data-is-loading', () => {
            this.loaded = false;
        });
        eventBus.$on('category-data-finished-loading', () => this.showComponents());
        eventBus.$on('add-category',
            () => {
                this.$nextTick(() => {
                    var categoriesToFilter = this.setCategoriesToFilter();
                    eventBus.$emit('set-categories-to-filter', categoriesToFilter);
                });
            });

    },

    watch: {
        hover(val) {
            this.showHover = !!(val && this.editMode);
        },
        currentChildCategoryIds() {
            var categoriesToFilter = this.setCategoriesToFilter();
            eventBus.$emit('set-categories-to-filter', categoriesToFilter);
        },
        segments() {
            var categoriesToFilter = this.setCategoriesToFilter();
            eventBus.$emit('set-categories-to-filter', categoriesToFilter);
        }
    },
    methods: {
        setCategoriesToFilter() {
            var categoriesToFilter = Array.from(this.currentChildCategoryIds);
            categoriesToFilter.push(parseInt(this.categoryId));
            this.segments.forEach(s => {
                categoriesToFilter.push(s.CategoryId);
            });

            return categoriesToFilter;
        },
        addNewCategoryCard(id) {
            var self = this;
            var data = {
                categoryId: id,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetCategoryData',
                data: JSON.stringify(data),
                success: function (c) {
                    self.categories.push(c);
                    self.currentChildCategoryIds.push(c.Id);

                },
            });
        },
        getCategoriesData() {
            var self = this;
            var data = {
                categoryIds: self.currentChildCategoryIds,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetCategoriesData',
                data: JSON.stringify(data),
                success: function (data) {
                    data.forEach(c => self.categories.push(c));
                    self.$nextTick(() => Images.ReplaceDummyImages());
                },
            });
        },
        getCategory(id) {
            var self = this;
            var data = {
                id: id,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetCategoryData',
                data: JSON.stringify(data),
                success(c) {
                    self.categories.push(c);
                    self.$nextTick(() => Images.ReplaceDummyImages());
                },
            });
        },
        loadSegment(id) {
            if (!userStore.isLoggedIn) {
                userStore.showLoginModal = true
                return;
            }
            var idExists = (segment) => segment.CategoryId === id;
            if (this.segments.some(idExists))
                return;

            this.currentChildCategoryIds = this.currentChildCategoryIds.filter(c => c != id);
            this.categories = this.categories.filter(c => c.Id != id);

            var self = this;
            var data = { CategoryId: id }

            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/Segmentation/GetSegment',
                data: JSON.stringify(data),
                success: function (segment) {
                    if (segment) {
                        self.hasCustomSegment = true;
                        var index = self.segments.indexOf(segment);
                        if (index == -1)
                            self.segments.push(segment);
                        this.saveSegments();
                    }
                },
            });
        },
        addCategory(val) {
            if (!userStore.isLoggedIn) {
                userStore.showLoginModal = true
                return;
            }

            var self = this;
            var categoriesToFilter = this.setCategoriesToFilter();
            var parent = {
                id: self.categoryId,
                addCategoryBtnId: $("#AddToCurrentCategoryBtn"),
                editCategoryRelation: val ? EditTopicRelationType.Create : EditTopicRelationType.AddChild,
                categoriesToFilter,
            }

            $('#AddCategoryModal').data('parent', parent).modal('show');
        },
        removeChildren() {
            if (!userStore.isLoggedIn) {
                userStore.showLoginModal = true
                return;
            }
            var self = this;
            var data = {
                parentCategoryId: self.categoryId,
                childCategoryIds: self.selectedCategories,
            };
            $.ajax({
                type: 'Post',
                contentType: "application/json",
                url: '/EditCategory/RemoveChildren',
                data: JSON.stringify(data),
                success: function (result) {
                    eventBus.$emit('content-change');
                    var removedChildCategoryIds = JSON.parse(result.removedChildCategoryIds);
                    self.filterChildren(removedChildCategoryIds);
                },
            });
        },
        moveToNewCategory() {
            if (!userStore.isLoggedIn) {
                userStore.showLoginModal = true
                return;
            }
            var self = this;
            var parent = {
                id: self.categoryId,
                addCategoryBtnId: $("#AddToCurrentCategoryBtn"),
                categoryChange: EditTopicRelationType.Move,
                selectedCategories: self.selectedCategories,
            }
            $('#AddCategoryModal').data('parent', parent).modal('show');
        },
        showComponents: _.debounce(function () {
            this.loaded = true;
        }, 1000),
        filterChildren(selectedCategoryIds) {
            let filteredCurrentChildCategoryIds = this.currentChildCategoryIds.filter(
                function (e) {
                    return this.indexOf(e) < 0;
                },
                selectedCategoryIds
            );
            this.currentChildCategoryIds = filteredCurrentChildCategoryIds;
            this.saveSegments();
        },
        removeSegment(id) {
            this.segments = this.segments.filter(s => s.CategoryId != id);
            this.currentChildCategoryIds.push(id);
            this.hasCustomSegment = this.segments.length > 0;
            this.saveSegments();
        },
        async saveSegments() {
            if (!userStore.isLoggedIn) {
                userStore.showLoginModal = true
                return;
            }
            var self = this;
            var segmentation = [];
            this.segment.map(function (s) {
                var childCategoryIds = this.$refs['segment' + s.CategoryId].currentChildCategoryIdsString
                var segment = childCategoryIds != null ? {
                    CategoryId: s.CategoryId,
                    ChildCategoryIds: childCategoryIds
                } :
                    {
                        CategoryId: s.CategoryId,
                    }
                segmentation.push(segment);
            })

            var data = {
                categoryId: self.categoryId,
                segmentation: segmentation
            }
            var result = await $fetch('/api/Topic/SaveSegments', {
                method: 'POST', body: data, mode: 'cors', credentials: 'include'
            })
            if (result == true) {
                this.saveSuccess = true;
                this.saveMessage = "Das Thema wurde gespeichert.";
            } else {
                this.saveSuccess = false;
                this.saveMessage = "Das Speichern schlug fehl.";
            };
        },
        removeCard(id) {

        }
    },
}
</script>


<template>
    <div id="Segmentation">
        <div class="segmentationHeader overline-m">
            Untergeordnete Themen <template v-if="categories.length > 0">({{ categories.length }})</template>
            <div class="toRoot" id="SegmentationLinkToGlobalWiki" style="display:none">
                <!-- <% Html.RenderPartial("CategoryLabel", RootCategory.Get); %> -->
            </div>
        </div>

        <div id="CustomSegmentSection">
            <TopicContentSegmentationSegment v-for="s in segments" :ref="'segment' + s.CategoryId" :title="s.Title"
                :child-category-ids="s.ChildCategoryIds" :category-id="s.CategoryId" :is-historic="isHistoric"
                :parent-id="categoryId" @remove-segment="removeSegment(s.CategoryId)" />
        </div>
        <div id="GeneratedSegmentSection" @mouseover="hover = true" @mouseleave="hover = false"
            :class="{ hover: showHover && !isHistoric }">
            <div class="segmentHeader" v-if="hasCustomSegment">
                <div class="segmentTitle">
                    <h2>
                        Weitere untergeordnete Themen
                    </h2>
                </div>
            </div>

            <div class="topicNavigation row">
                <TopicContentSegmentationCard v-for="(category, index) in categories" @select-category="selectCategory"
                    @unselect-category="unselectCategory" :ref="'card' + category.Id"
                    :is-custom-segment="isCustomSegment" :category-id="category.Id"
                    :selected-categories="selectedCategories" :segment-id="segmentId" hide="false" :key="index"
                    :category="category" :is-historic="isHistoric" @remove-card="removeCard(category.Id)" />
                <div v-if="!isHistoric" class="col-xs-6 topic">
                    <div class="addCategoryCard memo-button row" :id="addCategoryId">
                        <div class="col-xs-3">
                        </div>
                        <div class="col-xs-9 addCategoryLabelContainer">
                            <div class="addCategoryCardLabel" @click="addCategory(true)">
                                <i class="fas fa-plus"></i> Neues Thema
                            </div>
                            <div class="addCategoryCardLabel" @click="addCategory(false)">
                                <i class="fas fa-plus"></i> Bestehendes Thema
                            </div>
                        </div>

                    </div>
                </div>

            </div>

        </div>
    </div>
</template>