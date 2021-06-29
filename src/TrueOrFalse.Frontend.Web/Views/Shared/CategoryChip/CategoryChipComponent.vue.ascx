<categorychip-component :category="category" :index="index" inline-template v-on:remove-category-chip="removeCategory">
    <div class="category-chip-component">
        <div class="category-chip-container"  @mouseover="hover = true" @mouseleave="hover = false">
            <a :href="category.Url">
                <div class="category-chip show-tooltip" :title="category.Name">

                    <img v-if="showImage" :src="category.MiniImageUrl"/>

                    <div :href="category.Url" class="category-chip-label">
                        <i v-if="category.IconHtml.length > 0" v-html="category.IconHtml"></i>{{name}}
                    </div>
                    <i v-if="category.Visibility == 1" class="fas fa-lock"></i>
                </div>
            </a>
        </div>
        <div class="category-chip-deleteBtn" v-show="$parent.selectedCategories.length > 1" @click="removeCategory()">
            <i class="fas fa-times" ></i>
        </div>
    </div>

</categorychip-component>
