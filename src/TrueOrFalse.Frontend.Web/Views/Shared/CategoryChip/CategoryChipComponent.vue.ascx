<categorychip-component :category="category">
    <div class="category-chip-container">
        <a :href="category.Url">
            <div class="category-chip show-tooltip" :title="category.Name">

                <img v-if="showImage" :src="category.MiniImageUrl"/>

                <span>
                    <template v-if="category.IconHtml.length > 0" v-html="category.IconHtml"></template>{{category.Name}}
                </span>
                <i v-if="category.Visibility == 1" class="fas fa-lock"></i>
                <span class="remove-category-chip"></span>
            </div>
        </a>
    </div>
</categorychip-component>
