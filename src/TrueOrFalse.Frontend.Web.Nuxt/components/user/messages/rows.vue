<script lang="ts" setup>
const props = defineProps(['msg'])
</script>

<template>
    <div class="row msgRow rowBase>" :class="{ 'isRead': msg.IsRead }">
        <div class="msg">
            <div class="col-xs-12 header">
                <h4>
                    {{ msg.Subject }}
                </h4>
            </div>

            <div class="col-xs-12 body" v-html="msg.Body">
            </div>

            <div class="col-sm-5 footer">
                <span class="show-tooltip" :title="msg.WhenDatetime">vor {{ msg.When }}</span>
            </div>
            <div class="col-sm-7  footer" v-if="msg.MessageId != 0">
                <span class="pull-right" v-if="msg.IsRead">
                    <div class="TextLinkWithIcon markAsRead">
                        <span class="TextSpan">als gelesen makieren</span>
                        &nbsp; <i class="fa fa-square-o show-tooltip" style="color:sandybrown;"
                            title="Die Frage ist ungelesen"></i>

                    </div>
                </span>

                <span class="pull-right" v-else>
                    <div class="TextLinkWithIcon markAsUnRead">
                        <span class="TextSpan">als ungelesen makieren</span>
                        &nbsp; <i class="fa fa-check-square-o show-tooltip" style="color:green"
                            title="Die Frage ist gelesen"></i>
                    </div>
                </span>
            </div>
        </div>
    </div>

</template>