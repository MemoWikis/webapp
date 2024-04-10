import { defineStore } from 'pinia'
import { Topic } from '../topic/topicStore'
export const useRootTopicChipStore = defineStore('rootTopicChipStore', {
    state: () => {
        return {
            showRootTopicChip: false,
            name: 'Globales Wiki',
            id: 1,
            imgUrl: '/Images/Categories/1_50s.jpg'
        }
    },
    actions: {
        setRootTopicData(topic: Topic) {
            this.name = topic.name
            this.id = topic.id
            if (topic.imageUrl.length > 0)
                this.imgUrl = topic.imageUrl
        }
    }
})