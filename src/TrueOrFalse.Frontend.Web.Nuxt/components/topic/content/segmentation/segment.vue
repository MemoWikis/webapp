<script lang="ts">
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";
import { TopicContentSegmentationCard } from "~~/.nuxt/components";
import { useUserStore } from "~~/components/user/userStore";
import {
	EditTopicRelationType,
	useEditTopicRelationStore,
} from "../../relation/editTopicRelationStore";
import { CategoryCardData } from "./CategoryCardData";
import { usePublishTopicStore } from "../../publish/publishTopicStore";

export default defineNuxtComponent({
	props: ['title', 'categoryId', 'childCategoryIds', 'editMode', 'isHistoric', 'parentId', 'childTopics', 'segmentData'],
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
			linkToCategory: '',
			visibility: 0,
			segmentTitle: null as null | string,
			disabled: true,
			knowledgeBarData: null,
		};
	},
	mounted() {
		this.setSegmentData(this.segmentData);
		this.segmentId = "Segment-" + this.categoryId;
		if (this.childCategoryIds != null) {
			this.childTopics?.forEach((c: any) => this.categories.push(c));
			this.currentChildCategoryIds = this.childCategoryIds;
		}
		this.addCategoryId = "AddCategoryTo-" + this.segmentId + "-Btn";
		this.dropdownId = this.segmentId + "-Dropdown";

		const editTopicRelationStore = useEditTopicRelationStore()
		const self = this;
		editTopicRelationStore.$onAction(({
			name,
			after
		}) => {
			if (name == 'addTopicCard') {
				after((result) => {
					if (result.parentId == self.categoryId) {
						self.addNewCategoryCard(result.childId)
					}
				})
			}
		})
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
		segmentData(val) {
			this.setSegmentData(val);
		},
		childCategoryIds(val) {
			if (val != null) {
				this.categories = []
				this.currentChildCategoryIds = []
				this.childTopics?.forEach((c: any) => this.categories.push(c));
				this.currentChildCategoryIds = this.childCategoryIds;
			}
		}
	},
	methods: {
		addCategoryCardEvent(e: any) {
			if (this.categoryId == e.parentId)
				this.addNewCategoryCard(e.newCategoryId);
		},
		async addNewCategoryCard(id: number) {
			var self = this;
			var data = {
				categoryId: id,
			};

			let result = await $fetch<CategoryCardData>(
				"/apiVue/VueSegmentation/GetCategoryData",
				{
					body: data,
					method: "POST",
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
				"/apiVue/VueSegmentation/GetCategoriesData",
				{
					body: data,
					method: "POST",
					credentials: "include",
					mode: "cors",
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

			let result = await $fetch<any>("/apiVue/VueSegmentation/GetSegmentData", {
				body: data,
				method: "POST",
				credentials: "include",
			});
			if (result) {
				this.setSegmentData(result);
			}
		},
		setSegmentData(data: any) {
			this.linkToCategory = data.linkToCategory;
			this.visibility = data.visibility;
			this.knowledgeBarData = data.knowledgeBarData;
			if (this.title) this.segmentTitle = this.title;
			else this.segmentTitle = data.categoryName;
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
				method: "POST",
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
			// this.$nuxt.$emit("save-segments");
		},
		hideChildren() {
			this.filterChildren(this.selectedCategories);
		},
		openPublishModal() {

			const publishTopicStore = usePublishTopicStore();
			publishTopicStore.openModal(this.categoryId);

			const self = this;
			publishTopicStore.$onAction(({ name, after }) => {
				if (name == 'publish') {
					after((result) => {
						if (result.id == self.categoryId && result.success) {
							this.visibility = 0
						}
					})
				}
			})
		},
		openMoveCategoryModal() {
			const data = {
				topicIdToRemove: this.parentId,
				childId: this.categoryId,
				editCategoryRelation: EditTopicRelationType.Move,
			}
			const editTopicRelationStore = useEditTopicRelationStore()
			editTopicRelationStore.openModal(data)
		},
		openAddToWikiModal() {
			const data = {
				childId: this.categoryId,
				editCategoryRelation: EditTopicRelationType.AddToPersonalWiki,
			}
			const editTopicRelationStore = useEditTopicRelationStore()
			editTopicRelationStore.openModal(data)
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
		mouseEnter() {
			this.showHover = true
		},
		mouseLeave() {
			this.showHover = false
		}
	},
});
</script>

<template>
	<div class="segment" @mouseover="mouseEnter" @mouseleave="mouseLeave" :class="{ hover: showHover }">
		<div class="segmentSubHeader">
			<div class="segmentHeader">
				<div class="segmentTitle">
					<NuxtLink :to="linkToCategory">
						<h2>
							{{ segmentTitle }}
						</h2>
					</NuxtLink>
					<div v-if="visibility != 0" class="segmentLock" @click="openPublishModal" data-toggle="tooltip"
						title="Thema ist privat. Zum Veröffentlichen klicken.">
						<font-awesome-icon :icon="['fa-solid', 'lock']" />
						<font-awesome-icon :icon="['fa-solid', 'unlock']" />
					</div>
				</div>


				<div v-if="!isHistoric" class="Button dropdown DropdownButton segmentDropdown"
					:class="{ hover: showHover && !isHistoric }">
					<VDropdown :distance="1">
						<div class="btn btn-link btn-sm ButtonEllipsis">
							<font-awesome-icon :icon="['fa-solid', 'ellipsis-vertical']" />
						</div>
						<template #popper="{ hide }">
							<div @click="removeSegment()" class="dropdown-row">
								<div class="dropdown-icon">
									<font-awesome-icon :icon="['fa-solid', 'sitemap']" />
								</div>
								<div class="dropdown-label"> Unterthema ausblenden</div>
							</div>
							<div @click="openPublishModal(); hide()" class="dropdown-row" v-if="visibility == 1">
								<div class="dropdown-icon">
									<font-awesome-icon :icon="['fa-solid', 'unlock']" />
								</div>
								<div class="dropdown-label">Thema veröffentlichen</div>
							</div>
							<div @click="openMoveCategoryModal(); hide()" class="dropdown-row">
								<div class="dropdown-icon">
									<font-awesome-icon :icon="['fa-solid', 'circle-right']" />
								</div>
								<div class="dropdown-label">Thema verschieben</div>
							</div>
							<div @click="openAddToWikiModal(); hide()" data-allowed="logged-in" class="dropdown-row">
								<div class="dropdown-icon">
									<font-awesome-icon :icon="['fa-solid', 'plus']" />
								</div>
								<div class="dropdown-label">Zu meinem Wiki hinzufügen</div>
							</div>
						</template>
					</VDropdown>
				</div>
			</div>

			<div class="segmentKnowledgeBar">
				<div class="KnowledgeBarWrapper">
					<div class="knowledge-bar">
						<div v-if="$props.segmentData.knowledgeBarData.NeedsLearningPercentage > 0" class="needs-learning"
							v-tooltip="`Solltest du lernen: ${$props.segmentData.knowledgeBarData.NeedsLearning} Fragen (${$props.segmentData.knowledgeBarData.NeedsLearningPercentage})`"
							:style="{ 'width': $props.segmentData.knowledgeBarData.NeedsLearningPercentage + '%' }">
						</div>

						<div v-if="$props.segmentData.knowledgeBarData.NeedsConsolidationPercentage > 0"
							class="needs-consolidation"
							v-tooltip="`Solltest du lernen: ${$props.segmentData.knowledgeBarData.NeedsConsolidation} Fragen (${$props.segmentData.knowledgeBarData.NeedsConsolidationPercentage})`"
							:style="{ 'width': $props.segmentData.knowledgeBarData.NeedsConsolidationPercentage + '%' }">
						</div>

						<div v-if="$props.segmentData.knowledgeBarData.SolidPercentage > 0" class="solid-knowledge"
							v-tooltip="`Solltest du lernen: ${$props.segmentData.knowledgeBarData.Solid} Fragen (${$props.segmentData.knowledgeBarData.SolidPercentage})`"
							:style="{ 'width': $props.segmentData.knowledgeBarData.SolidPercentage + '%' }"></div>

						<div v-if="$props.segmentData.knowledgeBarData.NotLearnedPercentage > 0" class="not-learned"
							v-tooltip="`Solltest du lernen: ${$props.segmentData.knowledgeBarData.NotLearned} Fragen (${$props.segmentData.knowledgeBarData.NotLearnedPercentage})`"
							:style="{ 'width': $props.segmentData.knowledgeBarData.NotLearnedPercentage + '%' }"></div>
					</div>
					<div class="KnowledgeBarLegend">Dein Wissensstand</div>
				</div>
			</div>
		</div>
		<div class="topicNavigation row" :key="cardsKey!">
			<TopicContentSegmentationCard v-for="(category, index) in categories" @select-category="selectCategory"
				@unselect-category="unselectCategory" inline-template :ref="'card' + category.Id"
				:is-custom-segment="isCustomSegment" :category-id="category.Id" :selected-categories="selectedCategories"
				:segment-id="segmentId!" hide="false" :key="index" :category="category" :is-historic="isHistoric"
				@filter-children="filterChildren" :parent-topic-id="categoryId" @remove-category="removeCategory"
				@add-category-card="addCategoryCardEvent" />
		</div>
	</div>
</template>
<style lang="less" scoped>
@import (reference) "~~/assets/includes/imports.less";

.topic {
	@media (max-width: 649px) {
		width: 100%;
	}
}

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

	.segment {
		transition: 0.2s;

		.row {
			margin-bottom: 25px;
		}

		&.hover {
			cursor: pointer;
		}
	}

	.topicNavigation {
		margin-top: 0px;

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
				background: white;
				border-radius: 15px;

				&:hover {
					filter: brightness(0.85);
				}

				&:active {
					filter: brightness(0.7);
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

	.segment {
		.segmentSubHeader {
			.segmentKnowledgeBar {
				max-width: 420px;

				.knowledge-bar {
					display: inline-flex;
					margin-top: 15px;
					height: 10px;
					min-width: 150px;
					width: 100%;

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
	}
}
}

.segmentHeader {
	display: inline-flex;
	width: 100%;
	justify-content: space-between;
	margin-top: 20px;

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

	.ButtonEllipsis {
		font-size: 18px;
		color: @memo-grey-dark;
		border-radius: 50%;
		height: 40px;
		width: 40px;
		display: flex;
		justify-content: center;
		align-items: center;
		background: white;

		&:hover {
			filter: brightness(0.85);
		}

		&:active {
			filter: brightness(0.7);
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
	.dropdown-toggle {
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

& .hover {
	.DropdownButton {
		.dropdown-toggle {
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
	font-size: 16px;

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
</style>

<style lang="less">
@import (reference) "~~/assets/includes/imports.less";

.segmentKnowledgeBar {
	.category-knowledge-bar {
		height: 10px;
		width: 100%;

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
</style>