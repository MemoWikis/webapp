<script lang="ts">
import { useUserStore } from '~~/components/user/userStore'
import { useAlertStore, AlertType, messages } from '~~/components/alert/alertStore'
import { useEditTopicRelationStore } from '../../relation/editTopicRelationStore'

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

            var result = await $fetch<any>('/api/EditCategory/RemoveParent', {
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
                parentCategoryIdToRemove: self.$parent.categoryId,
                childCategoryId: self.categoryId,
            };

            const editTopicRelationStore = useEditTopicRelationStore()
            // editTopicRelationStore.
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
                        <font-awesome-icon icon="fa-solid fa-lock" />
                        <font-awesome-icon icon="fa-solid fa-unlock" />
                    </div>
                </div>

                <div v-if="!isHistoric" class="Button dropdown DropdownButton"
                    :class="{ hover: showHover && !isHistoric }">
                    <VDropdown :distance="6">
                        <div :id="dropdownId" class="dropdown-toggle btn btn-link btn-sm ButtonEllipsis" type="button"
                            data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
                            <font-awesome-icon icon="fa-regular fa-ellipsis-vertical" />
                        </div>
                        <template #popper>
                            <ul class="dropdown-menu dropdown-menu-right" :aria-labelledby="dropdownId">
                                <li v-if="!isCustomSegment">
                                    <div @click="thisToSegment" class="dropdown-item">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon icon="fa-solid fa-sitemap" />
                                        </div>
                                        Unterthemen einblenden
                                    </div>
                                </li>
                                <li>
                                    <div @click="removeParent" class="dropdown-item">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon icon="fa-solid fa-link-slash" />
                                        </div>Verknüpfung entfernen
                                    </div>
                                </li>
                                <li v-if="visibility == 1">
                                    <div @click="openPublishModal" class="dropdown-item">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon icon="fa-solid fa-unlock" />
                                        </div>Thema veröffentlichen
                                    </div>
                                </li>
                                <li>
                                    <div @click="openMoveCategoryModal()" class="dropdown-item">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon icon="fa-solid fa-circle-right" />
                                        </div>Thema verschieben
                                    </div>
                                </li>
                                <li>
                                    <div @click="openAddToWikiModal()" data-allowed="logged-in" class="dropdown-item">
                                        <div class="dropdown-icon">
                                            <font-awesome-icon icon="fa-solid fa-square-plus" />
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
li {
    .dropdown-item {
        cursor: pointer;
        display: flex;
        flex-wrap: nowrap;
    }
}
</style>