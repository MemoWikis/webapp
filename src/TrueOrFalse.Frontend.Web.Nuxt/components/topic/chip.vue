<script lang="ts" setup>

const props = defineProps(['topic', 'selectedTopics', 'index'])
const emit = defineEmits(['removeTopic'])

const hover = ref(false)
const name = ref('')

const showImage = ref(false)

onMounted(() => {
    if (props.topic.MiniImageUrl.includes('no-category-picture'))
        showImage.value = true

    name.value = props.topic.Name.length > 30 ? props.topic.Name.substring(0, 26) + ' ...' : props.topic.Name;

})

</script>

<template>
    <div class="category-chip-component">
        <div class="category-chip-container" @mouseover="hover = true" @mouseleave="hover = false">
            <a :href="topic.Url">
                <div class="category-chip show-tooltip" :title="topic.Name">

                    <img v-if="showImage" :src="topic.MiniImageUrl" />

                    <div :href="topic.Url" class="category-chip-label">
                        <i v-if="topic.IconHtml.length > 0" v-html="topic.IconHtml"></i>{{ name }}
                    </div>
                    <font-awesome-icon v-if="topic.Visibility == 1" icon="fa-solid fa-lock" />
                </div>
            </a>
        </div>
        <div class="category-chip-deleteBtn" v-show="props.selectedTopics.length > 1"
            @click="emit('removeTopic', { index: props.index, topicId: props.topic.Id })">
            <font-awesome-icon icon="fa-solid fa-xmark" />
        </div>
    </div>
</template>