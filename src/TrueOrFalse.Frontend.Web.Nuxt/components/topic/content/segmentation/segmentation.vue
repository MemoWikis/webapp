<script lang="ts">
import { useUserStore } from "~~/components/user/userStore";
import { useTopicStore } from "~~/components/topic/topicStore";
import {
  useEditTopicRelationStore,
  EditTopicRelationType,
  EditRelationData,
} from "../../relation/editTopicRelationStore";
import _ from "underscore";

interface Segment {
  CategoryId: number;
  Title: string;
  ChildCategoryIds: Array<number>;
}

export default defineNuxtComponent({
  props: {
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
      segmentId: "SegmentationComponent",
      hover: false,
      showHover: false,
      addCategoryId: "AddToCurrentCategoryCard",
      dropdownId: "MainSegment-Dropdown",
      controlWishknowledge: false,
      loadComponents: true,
      currentChildCategoryIds: [] as number[],
      segments: [] as Segment[],
      categories: [] as any[],
      isHistoric: this.isHistoricString == "True",
      userStore: useUserStore(),
      topicStore: useTopicStore(),
      editTopicRelationStore: useEditTopicRelationStore(),
      childCategoryIds: "",
      segmentJson: "",
      categoryId: 0,
    };
  },
  mounted() {
    const topicStore = useTopicStore();
    this.categoryId = topicStore.id;
    this.initSegments();
    // eventBus.$on('add-category-card',
    //     (e) => {
    //         if (e.parentId == this.categoryId)
    //             this.addNewCategoryCard(e.newCategoryId);
    //     });

    // eventBus.$on('category-data-is-loading', () => {
    //     this.loaded = false;
    // });
    // eventBus.$on('category-data-finished-loading', () => this.showComponents());
    // eventBus.$on('add-category',
    //     () => {
    //         this.$nextTick(() => {
    //             var categoriesToFilter = this.setCategoriesToFilter();
    //             eventBus.$emit('set-categories-to-filter', categoriesToFilter);
    //         });
    //     });
  },

  watch: {
    hover(val) {
      this.showHover = !!val;
    },
    // currentChildCategoryIds() {
    //     var categoriesToFilter = this.setCategoriesToFilter();
    //     eventBus.$emit('set-categories-to-filter', categoriesToFilter);
    // },
    // segments() {
    //     var categoriesToFilter = this.setCategoriesToFilter();
    //     eventBus.$emit('set-categories-to-filter', categoriesToFilter);
    // }
  },
  methods: {
    async initSegments() {
      var data = {
        id: this.topicStore.id,
      };
      var result = await $fetch<any>(
        "/apiVue/NuxtSegmentation/GetSegmentation",
        {
          method: "POST",
          body: data,
          mode: "cors",
          credentials: "include",
        }
      );

      if (
        result.childCategoryIds != null &&
        result.childCategoryIds.length > 0
      ) {
        this.currentChildCategoryIds = JSON.parse(result.childCategoryIds);
        this.getCategoriesData();
      }
      if (result.segmentJson != null && result.segmentJson.length > 0) {
        let textArea = document.createElement("textarea");
        textArea.innerHTML = result.segmentJson;
        this.segments = JSON.parse(textArea.value);
        this.hasCustomSegment = true;
      }
    },
    setCategoriesToFilter(): number[] {
      var categoriesToFilter = Array.from(
        this.currentChildCategoryIds
      ) as number[];
      categoriesToFilter.push(this.topicStore.id);
      this.segments.forEach((s) => {
        categoriesToFilter.push(s.CategoryId);
      });

      return categoriesToFilter;
    },
    async addNewCategoryCard(id: number) {
      var self = this;
      var data = {
        categoryId: id,
      };

      var category = await $fetch<any>(
        "/apiVue/NuxtSegmentation/GetCategoryData",
        {
          method: "POST",
          body: data,
          mode: "cors",
          credentials: "include",
        }
      );
      if (category) {
        self.categories.push(category);
        self.currentChildCategoryIds.push(category.Id);
      }
    },
    async getCategoriesData() {
      var self = this;
      if (self.currentChildCategoryIds.length <= 0) return;
      var data = {
        categoryIds: self.currentChildCategoryIds,
      };
      var categories;
      categories = await $fetch<any>(
        "/apiVue/NuxtSegmentation/GetCategoriesData",
        {
          method: "POST",
          body: data,
          mode: "cors",
          credentials: "include",
        }
      );
      if (categories) {
        categories.forEach((c: any) => self.categories.push(c));
      }
    },
    async getCategory(id: number) {
      var self = this;
      var data = {
        id: id,
      };

      var category = await $fetch<any>(
        "/apiVue/NuxtSegmentation/GetCategoryData",
        {
          method: "POST",
          body: data,
          mode: "cors",
          credentials: "include",
        }
      );
      if (category) {
        self.categories.push(category);
        // self.$nextTick(() => Images.ReplaceDummyImages());
      }
    },
    async loadSegment(id: number) {
      if (!this.userStore.isLoggedIn) {
        this.userStore.showLoginModal = true;
        return;
      }
      var idExists = (segment: { CategoryId: any }) =>
        segment.CategoryId === id;
      if (this.segments.some(idExists)) return;

      this.currentChildCategoryIds = this.currentChildCategoryIds.filter(
        (c) => c != id
      );
      this.categories = this.categories.filter((c) => c.Id != id);

      var self = this;
      var data = { CategoryId: id };

      var segment = await $fetch<any>("/apiVue/NuxtSegmentation/GetSegment", {
        method: "POST",
        body: data,
        mode: "cors",
        credentials: "include",
      });
      if (segment) {
        self.hasCustomSegment = true;
        var index = self.segments.indexOf(segment);
        if (index == -1) self.segments.push(segment);
        this.saveSegments();
      }
    },
    addCategory(val: boolean) {
      if (!this.userStore.isLoggedIn) {
        this.userStore.showLoginModal = true;
        return;
      }

      var self = this;
      var categoriesToFilter = this.setCategoriesToFilter();

      var parent: EditRelationData = {
        parentId: self.topicStore.id,
        editCategoryRelation: val
          ? EditTopicRelationType.Create
          : EditTopicRelationType.AddChild,
        categoriesToFilter,
      };
      this.editTopicRelationStore.openModal(parent);
    },
    async removeChildren() {
      if (!this.userStore.isLoggedIn) {
        this.userStore.showLoginModal = true;
        return;
      }
      var self = this;
      var data = {
        parentCategoryId: self.categoryId,
        childCategoryIds: self.selectedCategories,
      };

      var result = await $fetch<any>("/apiVue/Topic/RemoveChildren", {
        method: "POST",
        body: data,
        mode: "cors",
        credentials: "include",
      });
      if (result == true) {
        var removedChildCategoryIds = JSON.parse(
          result.removedChildCategoryIds
        );
        self.filterChildren(removedChildCategoryIds);
      }
    },
    moveToNewCategory() {
      if (!this.userStore.isLoggedIn) {
        this.userStore.showLoginModal = true;
        return;
      }
      var self = this;
      var parent: EditRelationData = {
        parentId: self.topicStore.id,
        editCategoryRelation: EditTopicRelationType.Move,
        selectedCategories: self.selectedCategories,
      };
      this.editTopicRelationStore.openModal(parent);
    },
    showComponents: _.debounce((self = this as any) => {
      self.loaded = true;
    }, 1000),
    filterChildren(selectedCategoryIds: any) {
      let filteredCurrentChildCategoryIds = this.currentChildCategoryIds.filter(
        (e, index) => {
          return index < 0;
        },
        selectedCategoryIds
      );
      this.currentChildCategoryIds = filteredCurrentChildCategoryIds;
      this.saveSegments();
    },
    removeSegment(id: number) {
      this.segments = this.segments.filter((s) => s.CategoryId != id);
      this.currentChildCategoryIds.push(id);
      this.hasCustomSegment = this.segments.length > 0;
      this.saveSegments();
    },
    async saveSegments() {
      if (!this.userStore.isLoggedIn) {
        this.userStore.showLoginModal = true;
        return;
      }
      var self = this;
      var segmentation: any[] = [];
      this.segments.map((s: any) => {
        var sEl = this.$refs["segment" + s.CategoryId] as any;
        var childCategoryIds = sEl.currentChildCategoryIdsString;
        var segment =
          childCategoryIds != null
            ? {
              CategoryId: s.CategoryId,
              ChildCategoryIds: childCategoryIds,
            }
            : {
              CategoryId: s.CategoryId,
            };
        segmentation.push(segment);
      });

      var data = {
        categoryId: self.categoryId,
        segmentation: segmentation,
      };
      var result = await $fetch("/apiVue/Topic/SaveSegments", {
        method: "POST",
        body: data,
        mode: "cors",
        credentials: "include",
      });
      // Needs refer msg to editbar
      // if (result == true) {
      //     this.saveSuccess = true;
      //     this.saveMessage = "Das Thema wurde gespeichert.";
      // } else {
      //     this.saveSuccess = false;
      //     this.saveMessage = "Das Speichern schlug fehl.";
      // };
    },
    removeCard(id: any) { },
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
  <div id="Segmentation" class="col-xs-12">
    <div class="segmentationHeader overline-m">
      Untergeordnete Themen
      <template v-if="categories.length > 0">({{ categories.length }})</template>
      <div class="toRoot" id="SegmentationLinkToGlobalWiki" style="display: none">
        <!-- <% Html.RenderPartial("CategoryLabel", RootCategory.Get); %> -->
      </div>
    </div>

    <div id="CustomSegmentSection">
      <TopicContentSegmentationSegment v-for="s in segments" :ref="'segment' + s.CategoryId" :title="s.Title.toString()"
        :child-category-ids="s.ChildCategoryIds" :category-id="parseInt(s.CategoryId.toString())"
        :is-historic="isHistoric" :parent-id="categoryId" @remove-segment="removeSegment(s.CategoryId)"
        @filter-children="filterChildren" />
    </div>
    <div id="GeneratedSegmentSection" @mouseover="hover = true" @mouseleave="hover = false"
      :class="{ hover: showHover && !isHistoric }">
      <div class="segmentHeader" v-if="hasCustomSegment">
        <div class="segmentTitle">
          <h2>Weitere untergeordnete Themen</h2>
        </div>
      </div>

      <div class="topicNavigation row">
        <TopicContentSegmentationCard v-for="(category, index) in categories" :ref="'card' + category.Id"
          :is-custom-segment="isCustomSegment" :category-id="category.Id" :selected-categories="selectedCategories"
          :segment-id="segmentId" hide="false" :key="index" :category="category" :is-historic="isHistoric"
          @remove-card="removeCard(category.Id)" :parent-topic-id="categoryId" @remove-category="removeCategory"
          @load-segment="loadSegment" />
        <div v-if="!isHistoric" class="col-xs-6 topic">
          <div class="addCategoryCard memo-button row" :id="addCategoryId">
            <div class="col-xs-3"></div>
            <div class="col-xs-9 addCategoryLabelContainer">
              <div class="addCategoryCardLabel" @click="addCategory(true)">
                <font-awesome-icon icon="fa-solid fa-plus" />
                Neues Thema
              </div>
              <div class="addCategoryCardLabel" @click="addCategory(false)">
                <font-awesome-icon icon="fa-solid fa-plus" /> Bestehendes Thema
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped lang="less">
@import (reference) "~~/assets/includes/imports.less";
// @memo-blue-link: #18A0FB;
// @memo-blue: #203256;

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
    font-family: "Open Sans";
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
          transition: opacity 0.1s ease-in-out;
          transition: color 0.1s ease-in-out;

          &.show {
            opacity: 1;
            transition: opacity 0.1s ease-in-out;
            transition: color 0.1s ease-in-out;
          }

          &.selected {
            color: @memo-green;
            opacity: 1;
            transition: opacity 0.1s ease-in-out;
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
    transition: all 0.1s ease-in-out;
  }

  .segmentDropdown,
  .dropdown {
    &.hover {
      opacity: 1;
      transition: all 0.1s ease-in-out;
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
      background: #ffffffe6;
      border-radius: 50%;
      height: 40px;
      width: 40px;
      text-align: center;
      padding: 6px;
      transition: all 0.3s ease-in-out;

      i.fa-ellipsis-v {
        color: @memo-grey-dark;
        transition: all 0.3s ease-in-out;
      }
    }
  }

  .hover {
    .DropdownButton {
      a.dropdown-toggle {
        &:hover {
          background: #efefefe6;

          i.fa-ellipsis-v {
            color: @memo-blue;
          }
        }
      }
    }
  }

  .set-question-count {
    a.sub-label {
      color: @memo-grey-dark;
    }
  }

  .segmentCardLock,
  .segmentLock {
    cursor: pointer;
    display: inline-flex;
    align-items: center;
    margin-right: 10px;

    i.fa-unlock {
      display: none !important;
    }

    i.fa-lock {
      display: unset !important;
    }

    &:hover {
      i.fa-lock {
        display: none !important;
        color: @memo-blue;
      }

      i.fa-unlock {
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
