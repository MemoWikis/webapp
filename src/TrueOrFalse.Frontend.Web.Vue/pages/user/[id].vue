<script lang="ts" setup>
import { UserModel } from '~~/components/user/userModel'

const route = useRoute()
const config = useRuntimeConfig()

const { data: user } = await useFetch<UserModel>(`/User/GetUser/${route.params.id}`, {
  baseURL: config.apiBase,
  headers: useRequestHeaders(['cookie'])
})



</script>

<template>
  <div class="row">
    <div class="xxs-stack col-xs-12">
      <div class="row">
        <div class="col-xs-9 xxs-stack" style="margin-bottom: 10px;">
          <h1 class="pull-left ColoredUnderline User" style="margin-bottom: 10px; margin-top: 0px;  font-size: 30px;">
            <VTooltip>
              <font-awesome-icon icon="fa-solid fa-star" />

              <template #popper>
                {{user.Name}} unterstützt memucho als Fördermitglied. Danke!
              </template>
            </VTooltip>
            {{user.Name}}
            <span style="display: inline-block; font-size: 20px; font-weight: normal;">
              &nbsp;(Reputation: {{user.ReputationTotal}} - Rang {{user.ReputationRank}})
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
            <NuxtLink v-if="user.IsCurrentUser">
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
          <div>- <%= Model.Reputation.ForQuestionsCreated %> für erstellte Fragen</div>
          <div>- <%= Model.User.Id !=-1 ? Model.Reputation.ForQuestionsInOtherWishknowledge : 0 %> für eigene Fragen im
              Wunschwissen anderer </div>
          <div>- <%= Model.User.Id !=-1 ? Model.Reputation.ForPublicWishknowledge : 0 %> für die Veröffentlichung des
              eigenen Wunschwissens</div>
          <div>- <%= Model.Reputation.ForUsersFollowingMe %> für folgende Nutzer</div>
        </div>
        <div class="column">
          <h4 style="margin-top: 0;">Erstellte Inhalte</h4>
          <div>
            <%= Model.AmountCreatedQuestions %> öffentliche Fragen erstellt
          </div>
          <div>
            <%= Model.AmountCreatedCategories %> Themen erstellt
          </div>
          <%if (Model.ShowWiki) {%>
            <a href="<%= Links.CategoryDetail(Model.UserWiki) %>">Zu <%= Model.User.Name %>s Wiki</a>
            <%} %>
        </div>

        <div class="column">
          <h4 style="margin-top: 0;">Wunschwissen</h4>
          <div>
            <%= Model.AmountWishCountQuestions %> Fragen gemerkt
          </div>
        </div>

      </div>
    </div>

    <div class="col-lg-2 col-xs-3 xxs-stack">
      <img style="width:100%; border-radius:5px;" src="<%=Model.ImageUrl_250 %>" /><br />
      <div style="text-align: center; margin-top: 10px;" data-userid="<%=Model.UserIdProfile %>">
        <% if(!Model.IsCurrentUser && Model.IsMember){ %>
          <button class="btn btn-default btn-sm" type="button" data-type="btn-follow"
            style="<%= Html.CssHide(Model.DoIFollow) %> ">
            <i class="fa fa-user-plus"></i>
            Folgen
          </button>

          <i class='fa fa-spinner fa-pulse' data-type="btnFollowSpinner" style="display:none"></i>

          <button class="btn btn-warning btn-sm " type="button" data-type="btn-unfollow"
            style="<%= Html.CssHide(!Model.DoIFollow) %>">
            <i class="fa fa-user-times"></i>
            Entfolgen
          </button>
          <% } %>
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

            <div class="btn-group <%= Model.IsActiveTabKnowledge? " active" : "" %>">
              <a href="<%= Links.UserDetail(Model.User) %>" type="button" class="btn btn-default">
                Wunsch<span class="hidden-xxs">wissen</span>
              </a>
            </div>

            <div class="btn-group  <%= Model.IsActiveTabBadges  ? " active" : "" %>">
              <a href="<%= Links.UserDetailBadges(Model.User.User) %>" type="button" class="btn btn-default">
                Badges
              </a>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="col-lg-12">
      <div class="boxtainer-outlined-tabs" style="margin-top: 20px;">
        <div class="boxtainer-header MobileHide">
          <ul class="nav nav-tabs">
            <li class="<%= Html.IfTrue(Model.IsActiveTabKnowledge, " active") %>">
              <a href="<%= Links.UserDetail(Model.User) %>">
                Wunschwissen
              </a>
            </li>
            <li class="<%= Html.IfTrue(Model.IsActiveTabBadges, " active") %>">
              <a href="<%= Links.UserDetailBadges(Model.User.User) %>">
                Badges (0 von <%= BadgeTypes.All().Count %>)
              </a>
            </li>
          </ul>
        </div>
        <div class="boxtainer-content">
          <% if(Model.IsActiveTabKnowledge) { %>
            <% Html.RenderPartial("~/Views/Users/Detail/TabKnowledge.ascx", new TabKnowledgeModel(Model)); %>
              <% } %>
                <% if(Model.IsActiveTabBadges) { %>
                  <% Html.RenderPartial("~/Views/Users/Detail/TabBadges.ascx", new TabBadgesModel(Model)); %>
                    <% } %>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>

</style>
