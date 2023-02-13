<script lang="ts" setup>
import { handleNewLine } from '~~/components/shared/utils'
import { Message } from './message'

interface Props {
    message: Message,
    forceShow: boolean
}
const props = defineProps<Props>()
const read = ref(false)
onBeforeMount(() => {
    read.value = props.message.read
    if (!props.message.read)
        showMessage.value = true
})

async function markAsRead() {
    $fetch(`/apiVue/MessageRow/MarkAsRead?id=${props.message.id}`, {
        credentials: 'include',
        mode: 'cors'
    })
    read.value = true
}

async function markAsUnread() {
    $fetch(`/apiVue/MessageRow/MarkAsUnread?id=${props.message.id}`, {
        credentials: 'include',
        mode: 'cors'
    })
    read.value = false
}
const showMessage = ref(false)
watch(() => props.forceShow, (val) => showMessage.value = val)
</script>

<template>
    <div class="row msgRow rowBase" :class="{ 'isRead': read }" v-show="showMessage">
        <div class="msg">
            <div class="col-xs-12 header">
                <h4>
                    {{ props.message.subject }}
                </h4>
            </div>

            <div class="col-xs-12 body" v-html="handleNewLine(props.message.body)">
            </div>

            <div class="col-sm-5 footer">
                <span class="show-tooltip" :title="props.message.date">vor {{
                    props.message.timeElapsed
                }}</span>
            </div>
            <div class="col-sm-7  footer" v-if="props.message.id != 0">
                <span class="pull-right" v-if="read">
                    <div class="TextLinkWithIcon" @click="markAsRead()">
                        <span class="TextSpan">als gelesen makieren</span>
                        <font-awesome-icon icon="fa-regular fa-square" v-tooltip="'Die Frage ist ungelesen'" />
                    </div>
                </span>

                <span class="pull-right" v-else>
                    <div class="TextLinkWithIcon" @click="markAsUnread()">
                        <span class="TextSpan">als ungelesen makieren</span>
                        <font-awesome-icon icon="fa-solid fa-square-check" v-tooltip="'Die Frage ist gelesen'" />
                    </div>
                </span>
            </div>
        </div>
    </div>

</template>

<style lang="less" scoped>
@import '~~/assets/views/answerQuestion.less';

.msgRow {
    margin-bottom: 14px;
    border-radius: 0px;

    &.rowBase {
        margin-left: 0;
        margin-right: 0;
    }

    &.isRead {

        opacity: 0.6;
        //background-color:  #fcfcfc;
        //color: lighten(@global-text-color, 30);

        .header,
        .body,
        .footer {
            border-color: @grey-10;
        }
    }

    &:hover {
        background-color: #ebeff7;
        opacity: 1;
    }

    .header {
        padding-top: 9px;
        padding-bottom: 9px;

        h4 {
            margin: 0;
        }
    }

    .body {
        min-height: 60px;
        padding-top: 9px;
        padding-bottom: 9px;
    }

    .footer {
        padding-top: 9px;
        padding-bottom: 9px;
    }
}
</style>