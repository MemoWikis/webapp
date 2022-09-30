<script lang="ts" setup>
const props = defineProps(['tabBadgesModel'])

</script>

<template>
    <!-- <div class="row well" style="margin-top: 18px; padding: 10px;">
        <div class="col-lg-12">
            <h4>Du hast 0 von <%= BadgeTypes.All().Count %> Badges.</h4>
        </div>
    </div>

    <% foreach(var badgeTypeGroup in BadgeTypeGroups.All()) { %>

        <div class="row">
            <div class="col-md-12">
                <h4>
                    <%= badgeTypeGroup.Title %>
                </h4>
            </div>
        </div>

        <% foreach(var badgeType in BadgeTypes.All().ByGroupKey(badgeTypeGroup.Key)) { %>

            <div class="row">
                <div class="col-md-1"></div>
                <div class="col-md-4">
                    <%= badgeType.Name %>
                </div>
            </div>

            <% } %>

                <% } %> -->

</template>