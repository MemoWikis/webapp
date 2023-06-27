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
            this.name = topic.Name
            this.id = topic.Id
            if (topic.ImageUrl.length > 0)
                this.imgUrl = topic.ImageUrl
        }
    }
})