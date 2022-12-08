<script lang="ts" setup>
import { UserModel } from '~~/components/user/shared/userModel'
import { ImageStyle } from '~~/components/image/imageStyleEnum'

const route = useRoute()
const config = useRuntimeConfig()

const { data: model } = await useFetch<UserModel>(`/apiVue/VueUser/GetUser/${route.params.id}`, {
})

function follow() {

}
function unfollow() {

}

const { data: tabBadgesModel } = await useFetch<any>(`/apiVue/VueUser/GetUser/${route.params.id}`, {
})

const { data: tabKnowledgeModel } = await useFetch<any>(`/apiVue/VueUser/GetUser/${route.params.id}`, {
})

const showTab = ref('wuwi')
</script>

<template>
    <div class="row" v-if="model">
        <div class="xxs-stack col-xs-12">
            <div class="row">
                <div class="col-xs-9 xxs-stack" style="margin-bottom: 10px;">
                    <h1 class="pull-left ColoredUnderline User"
                        style="margin-bottom: 10px; margin-top: 0px;  font-size: 30px;">
                        <VTooltip>
                            <font-awesome-icon icon="fa-solid fa-star" />

                            <template #popper>
                                {{ model.Name }} unterstützt memucho als Fördermitglied. Danke!
                            </template>
                        </VTooltip>
                        {{ model.Name }}
                        <span style="display: inline-block; font-size: 20px; font-weight: normal;">
                            &nbsp;(Reputation: {{ model.ReputationTotal }} - Rang {{ model.ReputationRank }})
                        </span>
                    </h1>
                </div>
                <div class="col-xs-3 xxs-stack">
                    <div class="navLinks">
                        <NuxtLink>
                            <font-awesome-icon icon="fa-solid fa-list" />
                            &nbsp;zur Übersicht
                        </NuxtLink>
                        <!-- <a href="<%= Url.Action(" Users", "Users" )%>" style="font-size: 12px; margin: 0px;">
              <font-awesome-icon icon="fa-solid fa-list" />
              &nbsp;zur Übersicht
            </a> -->
                        <NuxtLink v-if="model.IsCurrentUser">
                            <font-awesome-icon icon="fa-solid fa-pencil" /> bearbeiten
                        </NuxtLink>
                        <!-- <% if (Model.IsCurrentUser) { %>
              <a href="<%= Url.Action(Links.UserSettingsAction, Links.UserSettingsController) %>"
                style="font-size: 12px; margin: 0px;"><i class="fa fa-pencil"></i>&nbsp;bearbeiten</a>
              <% } %> -->
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-10 col-xs-9 xxs-stack">
            <div class="box-content" style="min-height: 120px; clear: both; ">

                <div class="column">
                    <h4 style="margin-top: 0;">Reputation</h4>
                    <div>- {{ model.Reputation.ForQuestionsCreated }} für erstellte Fragen</div>
                    <div>- {{ model.User.Id != 1 ? model.Reputation.ForQuestionsInOtherWishKnowledge : 0 }} für eigene
                        Fragen im
                        Wunschwissen anderer </div>
                    <div>- {{ model.User.Id != 1 ? model.Reputation.ForPublicWishknowledge : 0 }} für die
                        Veröffentlichung des
                        eigenen Wunschwissens</div>
                    <div>- {{ model.Reputation.ForUsersFollowingMe }} für folgende Nutzer</div>
                </div>
                <div class="column">
                    <h4 style="margin-top: 0;">Erstellte Inhalte</h4>
                    <div>
                        {{ model.AmountCreatedQuestions }} öffentliche Fragen erstellt
                    </div>
                    <div>
                        {{ model.AmountCreatedCategories }} Themen erstellt
                    </div>

                    <NuxtLink v-if="model.ShowWiki" :to="`${model.UserWikiName}/${model.UserWikiId}`">
                        Zu {{ model.Name }}s Wiki
                    </NuxtLink>
                </div>

                <div class="column">
                    <h4 style="margin-top: 0;">Wunschwissen</h4>
                    <div>7
                        {{ model.AmountWishCountQuestions }} Fragen gemerkt
                    </div>
                </div>

            </div>
        </div>

        <div class="col-lg-2 col-xs-3 xxs-stack">
            <Image :style="ImageStyle.Author" :url="model.ImageUrl_250" />
            <div style="text-align: center; margin-top: 10px;">

                <template v-if="!model.IsCurrentUser && model.IsMember">
                    <button v-if="model.DoIFollow" class="btn btn-warning btn-sm">
                        Entfolgen
                    </button>
                    <button v-else class="btn btn-default btn-sm">
                        Folgen
                    </button>
                </template>
            </div>
        </div>
    </div>

    <div class="row" id="user-main">

        <div id="MobileSubHeader" class="MobileSubHeader DesktopHide" style="margin-top: 20px;">
            <div class="MainFilterBarWrapper">
                <div id="MainFilterBarBackground" class="btn-group btn-group-justified">
                    <div class="btn-group">
                        <a class="btn btn-default disabled">.</a>
                    </div>
                </div>
                <div class="container">
                    <div id="MainFilterBar" class="btn-group btn-group-justified JS-Tabs">

                        <div class="btn-group" :class="{ 'active': showTab == 'wuwi' }">
                            <div type="button" class="btn btn-default" @click="showTab = 'wuwi'">
                                Wunsch<span class="hidden-xxs">wissen</span>
                            </div>
                        </div>

                        <div class="btn-group" :class="{ 'active': showTab == 'badges' }">
                            <div type="button" class="btn btn-default" @click="showTab = 'badges'">
                                Badges
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-lg-12">
            <div class="boxtainer-outlined-tabs" style="margin-top: 20px;">
                <div class="boxtainer-header MobileHide">
                    <ul class="nav nav-tabs">
                        <li :class="{ 'active': showTab == 'wuwi' }">
                            <div class="btn-link" @click="showTab == 'wuwi'">
                                Wunschwissen
                            </div>
                        </li>
                        <li :class="{ 'active': showTab == 'badges' }">
                            <div class="btn-link" @click="showTab == 'badges'">
                                Badges (0 von {{ tabBadgesModel.Count }})
                            </div>
                        </li>
                    </ul>
                </div>
                <div class="boxtainer-content">
                    <UserUserProfileTabsTabKnowledge v-if="showTab == 'wuwi'" :user-model="model"
                        :tab-knowledge-model="''" />
                    <UserUserProfileTabsTabBadges v-else-if="showTab == 'badges'" />
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>

</style>
