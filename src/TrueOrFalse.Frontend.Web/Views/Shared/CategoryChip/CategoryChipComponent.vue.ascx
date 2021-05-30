<categorychip-component :category="category" :index="index" inline-template v-on:remove-category-chip="removeCategory">
    <div>
        <div class="category-chip-container"  @mouseover="hover = true" @mouseleave="hover = false">
            <a>
                <div class="category-chip show-tooltip" :title="category.Name">

                    <img v-if="showImage" :src="category.MiniImageUrl"/>

                    <span :href="category.Url">
                        <i v-if="category.IconHtml.length > 0" v-html="category.IconHtml"></i>{{category.Name}}
                    </span>
                    <i v-if="category.Visibility == 1" class="fas fa-lock"></i>
                </div>
            </a>
        </div>
        <i v-show="$parent.selectedCategories.length > 1" class="fas fa-times" @click="removeCategory()"></i>
    </div>

</categorychip-component>
