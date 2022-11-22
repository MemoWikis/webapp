<script lang="ts" setup>
const props = defineProps(['tabBadgesModel'])

</script>

<template>
    <div class="row well" style="margin-top: 18px; padding: 10px;">
        <div class="col-lg-12">
            <h4>Du hast 0 von {{props.tabBadgesModel.AllBadgeCount}}Badges.</h4>
        </div>
    </div>
    <template v-for="btg in props.tabBadgesModel.BadgeTypeGroup">
        <div class="row">
            <div class="col-md-12">
                <h4>
                    {{btg.Title}}
                </h4>
            </div>
        </div>
        <div v-for="b in btg" class="row">
            <div class="col-md-1"></div>
            <div class="col-md-4">
                {{b.Name}}
            </div>
        </div>
    </template>
</template>