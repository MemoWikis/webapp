<script lang="ts">
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import { TopicContentSegmentationCard } from "~~/.nuxt/components";
import { useUserStore } from "~~/components/user/userStore";
import {
  EditTopicRelationType,
  useEditTopicRelationStore,
} from "../../relation/editTopicRelationStore";
import { CategoryCardData } from "./CategoryCardData";
export default defineNuxtComponent({
  props: {
    title: String,
    description: String,
    childCategoryIds: Array,
    categoryId: {
      type: Number,
      required: true,
    },
    editMode: Boolean,
    isHistoric: Boolean,
    parentId: Number,
  },

  data() {
    return {
      categories: [] as any[],
      segmentId: null as null | string,
      cardsKey: null as null | string,
      isCustomSegment: true,
      selectedCategories: [] as any[],
      currentChildCategoryIds: [] as number[],
      currentChildCategoryIdsString: "",
      hover: false,
      showHover: false,
      addCategoryId: "",
      dropdownId: null as null | string,
      timer: null,
      linkToCategory: null,
      visibility: 0,
      segmentTitle: null as null | string,
      knowledgeBarHtml: null,
      disabled: true,
      knowledgeBarData: null,
    };
  },
  mounted() {
    this.getSegmentData();
    this.segmentId = "Segment-" + this.categoryId;
    if (this.childCategoryIds != null) {
      var baseChildCategoryIds = JSON.parse(this.childCategoryIds.toString());
      this.currentChildCategoryIds = baseChildCategoryIds;
    }
    this.addCategoryId = "AddCategoryTo-" + this.segmentId + "-Btn";
    this.dropdownId = this.segmentId + "-Dropdown";

    this.$on("select-category", (id: number) => this.selectCategory(id));
    this.$on("unselect-category", (id: number) => this.unselectCategory(id));
    this.$on("add-category-card", (e: any) => {
      if (this.categoryId == e.parentId)
        this.addNewCategoryCard(e.newCategoryId);
    });
    if (this.currentChildCategoryIds.length > 0) 
    this.getCategoriesData();
  },

  watch: {
    hover(val) {
      if (val && this.editMode) this.showHover = true;
      else this.showHover = false;
    },
    currentChildCategoryIds() {
      this.currentChildCategoryIdsString =
        this.currentChildCategoryIds.join(",");
    },
    selectedCategoryIds(val) {
      this.disabled = val.length <= 0;
    },
  },

  updated() {},

  methods: {
    async addNewCategoryCard(id: number) {
      var self = this;
      var data = {
        categoryId: id,
      };

      let result = await $fetch<CategoryCardData>(
        "/Segmentation/GetCategoryData",
        {
          body: data,
          method: "Post",
          credentials: "include",
          mode: "no-cors",
        }
      );
      if (result) {
        self.categories.push(result);
        self.currentChildCategoryIds.push(result.Id);
        // self.$nextTick(() => Images.ReplaceDummyImages());
      }
    },
    async getCategoriesData() {
      var self = this;
      var data = {
        categoryIds: self.currentChildCategoryIds,
      };

      let result = await $fetch<CategoryCardData[]>(
        "/apiVue/Segmentation/GetCategoriesData",
        {
          body: data,
          method: "Post",
          credentials: "include",
          mode: "no-cors",
        }
      );
      if (result) {
        result.forEach((c) => self.categories.push(c));
      }
    },
    async getSegmentData() {
      var self = this;
      var data = {
        categoryId: self.categoryId,
      };

      let result = await $fetch<any>("/apiVue/Segmentation/GetSegmentData", {
        body: data,
        method: "Post",
        credentials: "include",
      });
      if (result) {
        self.linkToCategory = result.linkToCategory;
        self.visibility = result.visibility;
        self.knowledgeBarHtml = result.knowledgeBarHtml;
        self.knowledgeBarData = result.knowledgeBarData;
        if (self.title) self.segmentTitle = self.title;
        else self.segmentTitle = result.categoryName;
      }
    },
    selectCategory(id: number) {
      if (this.selectedCategories.includes(id)) return;
      else this.selectedCategories.push(id);
    },
    unselectCategory(id: number) {
      if (this.selectedCategories.includes(id)) {
        var index = this.selectedCategories.indexOf(id);
        this.selectedCategories.splice(index, 1);
      }
    },
    updateCategoryOrder() {
      if (this.segmentId != null) {
        let topicElements = document
          .getElementById(this.segmentId)
          ?.getElementsByClassName("topic");
        let categoryIds: number[] = [];
        if (topicElements) {
          for (let i = 0; i < topicElements.length; i++) {
            let id = topicElements[i].getAttribute("category-id");
            if (id) {
              categoryIds.push(parseInt(id));
            }
          }
        }
      }
    },
    removeSegment() {
      const userStore = useUserStore();

      if (!userStore.isLoggedIn) {
        // NotLoggedIn.ShowErrorMsg("RemoveSegment");
        return;
      }
      var data = {
        parentId: this.parentId,
        newCategoryId: this.categoryId,
      };
      this.$emit("remove-segment", this.categoryId);
      this.$emit("add-category-card", data);
    },
    addCategory(val: boolean) {
      // if (NotLoggedIn.Yes()) {
      //     NotLoggedIn.ShowErrorMsg("CreateCategory");
      //     return;
      // }
      const userStore = useUserStore();
      if (!userStore.isLoggedIn) {
        return;
      }

      var categoriesToFilter = Array.from(this.currentChildCategoryIds);
      categoriesToFilter.push(this.categoryId);

      var parent = {
        id: this.categoryId,
        addCategoryButtonExists: document.getElementById(this.addCategoryId)
          ? true
          : false,
        moveCategories: false,
        categoriesToFilter,
        editCategoryRelation: val
          ? EditTopicRelationType.Create
          : EditTopicRelationType.AddChild,
      };
      const editTopicRelationStore = useEditTopicRelationStore();
      editTopicRelationStore.openModal(parent);
    },

    async removeChildren() {
      // if (NotLoggedIn.Yes()) {
      //     NotLoggedIn.ShowErrorMsg("RemoveChildren");
      //     return;
      // }
      const userStore = useUserStore();
      if (!userStore.isLoggedIn) {
        return;
      }
      var self = this;
      var data = {
        parentCategoryId: self.categoryId,
        childCategoryIds: self.selectedCategories,
      };

      let result = await $fetch<any>("/apiVue/EditCategory/RemoveChildren", {
        body: data,
        method: "Post",
        credentials: "include",
        mode: "no-cors",
      });

      if (result) {
        var removedChildCategoryIds = JSON.parse(
          result.removedChildCategoryIds
        );
        self.filterChildren(removedChildCategoryIds);
      }
    },
    filterChildren(selectedCategoryIds: number[]) {
      let filteredCurrentChildCategoryIds = this.currentChildCategoryIds.filter(
        (c) => selectedCategoryIds.some((s) => s != c)
      );
      this.currentChildCategoryIds = filteredCurrentChildCategoryIds;
      this.$nuxt.$emit("save-segments");
    },
    hideChildren() {
      this.filterChildren(this.selectedCategories);
    },
    openPublishModal() {
      this.$nuxt.$emit("open-publish-category-modal", this.categoryId);
    },
    openMoveCategoryModal() {
      var data = {
        parentCategoryIdToRemove: this.parentId,
        childCategoryId: this.categoryId,
      };
      this.$nuxt.$emit("open-move-category-modal", data);
    },
    openAddToWikiModal() {
      this.$nuxt.$emit("add-to-wiki", this.categoryId);
    },
    removeCategory(id: number) {
      this.currentChildCategoryIds = this.currentChildCategoryIds.filter(
        (c) => {
          return c != id;
        }
      );
      this.categories = this.categories.filter((c) => {
        return c.Id != id;
      });
    },
  },
});
</script>

<template>
  <div class="segment" @mouseover="hover = true" @mouseleave="hover = false" :class="{ hover: showHover }">
    <div class="segmentSubHeader">
      <div class="segmentHeader">
        <div class="segmentTitle">
          <a :href?="linkToCategory">
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
          <a href="#" :id?="dropdownId" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" type="button"
            data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
            <i class="fa fa-ellipsis-v"></i>
          </a>
          <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby?="dropdownId">
            <li @click="removeSegment()">
              <a>
                <div class="dropdown-icon">
                  <img class="fas" src="/Images/Icons/sitemap-disable.svg" />
                </div>
                Unterthema ausblenden
              </a>
            </li>
            <li v-if="visibility == 1">
              <a @click="openPublishModal">
                <div class="dropdown-icon">
                  <i class="fas fa-unlock"></i>
                </div>
                Thema veröffentlichen
              </a>
            </li>
            <li>
              <a @click="openMoveCategoryModal()" data-allowed="logged-in">
                <div class="dropdown-icon">
                  <i class="fa fa-arrow-circle-right"></i>
                </div>
                Thema verschieben
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
        <div class="KnowledgeBarWrapper" v-html="knowledgeBarHtml"></div>
      </div>
    </div>
    <div class="topicNavigation row" :key?="cardsKey">
      <TopicContentSegmentationCard v-for="(category, index) in categories" @select-category="selectCategory"
        @unselect-category="unselectCategory" inline-template :ref="'card' + category.Id"
        :is-custom-segment="isCustomSegment" :category-id="category.Id" :selected-categories="selectedCategories"
        :segment-id?="segmentId" hide="false" :key="index" :category="category" :is-historic="isHistoric"
        @filter-children="filterChildren" :parent-topic-id="categoryId" @remove-category="removeCategory" />
      <div v-if="!isHistoric" class="col-xs-6 topic">
        <div class="addCategoryCard memo-button row" :id="addCategoryId">
          <div class="col-xs-3"></div>
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
