<script lang="ts" setup></script>

<template>
  <div id="MasterFooter">
    <div class="row">
      <div class="container">
<div class="row Promoter">
    <div class="col-xs-12">
    </div>
</div>

<div class="row footer-links-memucho">

    <div class="FooterCol xxs-stack col-xs-6 col-md-3">
        <div id="MasterFooterLogoContainer">
            <a href="/" id="MasterFooterLogo">
                <Image url="/Images/Logo/LogoIconText.svg"/>
            </a>

            <div class="overline-s no-line">
                Alles an einem Ort –
                <br/>
                Wiki und Lernwerkzeug vereint!
            </div>
        </div>
        <div class="footer-group">
            <a href="<%= Links.TermsAndConditions %>">Nutzungsbedingungen (AGBs)</a><br/>
            <a href="<%= Links.Imprint %>">Impressum & Datenschutz</a><br/>
            <!-- <a href="<%= Links.TermsAndConditions %>">Nutzungsbedingungen (AGBs)</a><br/>
            <a href="<%= Links.Imprint %>">Impressum & Datenschutz</a><br/> -->
        </div>
    </div>

    <div class="FooterCol xxs-stack col-xs-6 col-md-3">
        <div class="footer-group">
            <div class="overline-m no-line">
                <!-- <a href="<%= Links.CategoryDetail("memucho-Wiki", RootCategory.MemuchoWikiId) %>">Memucho Wiki</a> -->
                <a href="">Memucho Wiki</a>

            </div>
            <!-- <% foreach (var id in RootCategory.MemuchoCategoryIds)
               {
                   var category = EntityCache.GetCategory(id, getDataFromEntityCache: true);
            %>
                <a class="" href="<%= Links.CategoryDetail(category) %>"><%= category.Name %></a><br/>
            <% } %> -->
        </div>
        <div class="footer-group">
            <div class="overline-m no-line">Software</div>
            <a href="https://github.com/TrueOrFalse/TrueOrFalse" target="_blank"><i class="fa fa-github">&nbsp;</i>Github</a>
            <br/>
            <a href="http://teamcity.memucho.de:8080/project.html?projectId=TrueOrFalse&guest=1" target="_blank">
                <font-awesome-icon icon="fa fa-cogs"></font-awesome-icon> Teamcity
            </a>
            <br/>
            <!-- <% if (Request.IsLocal)
               { %>
                <%= Html.ActionLink("Algorithmus-Einblick", "Forecast", "AlgoInsight") %><br/>
            <% } %>
            <% var assembly = Assembly.Load("TrueOrFalse"); %>
            <span style="color: darkgray">
                (Build: <%= assembly.GetName().Version.Major %> am
                <%= Html.Raw(AssemblyLinkerTimestamp.Get(assembly).ToString("dd.MM.yyyy 'um' HH:mm")) %>)
            </span> -->
        </div>

    </div>
    <div class="visible-xs visible-sm" style="clear: both"></div>

    <div class="FooterCol xxs-stack col-xs-6 col-md-3">
        <div class="footer-group">
            <div class="overline-m no-line">Hilfe & Kontakt</div>
            <!-- <% foreach (var id in RootCategory.MemuchoHelpIds)
               {
                   var category = EntityCache.GetCategory(id, getDataFromEntityCache: true);
            %>
                <a class="" href="<%= Links.CategoryDetail(category) %>"><%= category.Name %></a><br/>
            <% } %> -->
            <br/>

            <a href="https://discord.com/invite/nXKwGrN" target="_blank"><i class="fab fa-discord" aria-hidden="true">&nbsp;</i>Discord</a><br/>
            <a href="https://twitter.com/memuchoWissen" target="_blank"><i class="fa fa-twitter" aria-hidden="true">&nbsp;</i>auf Twitter</a><br/>
        </div>
    </div>

    <div class="FooterCol xxs-stack col-xs-6 col-md-3">
        <div class="footer-group">
            <div class="overline-m no-line">
                <!-- <a href="<%= Links.CategoryDetail("globales-wiki", RootCategory.RootCategoryId) %>">Globales Wiki</a> -->
                <a href="">Globales Wiki</a>

            </div>
            <!-- <% foreach (var id in RootCategory.MainCategoryIds)
               {
                   var category = EntityCache.GetCategory(id, getDataFromEntityCache: true);
            %>
                <a class="" href="<%= Links.CategoryDetail(category) %>"><%= category.Name %></a><br/>
            <% } %> -->
        </div>
        <div class="footer-group">
            <div class="overline-m no-line">Beliebte Themen</div>
            <!-- <% foreach (var id in RootCategory.PopularCategoryIds)
               {
                   var category = EntityCache.GetCategory(id, getDataFromEntityCache: true);
            %>
                <a class="" href="<%= Links.CategoryDetail(category) %>"><%= category.Name %></a><br/>
            <% } %> -->
        </div>
    </div>

    <div id="FooterEndContainer" class="col-xs-12 col-lg-12">
        <div id="FooterEnd">
            <div>
                Entwickelt von:
            </div>
            <a href="https://bitwerke.de/">
                <Image url="/Images/Logo/BitwerkeLogo.svg" />
            </a>
            <a href="https://bitwerke.de/">
                <div>
                    Individualsoftware, UX/UI, Entwicklung und Beratung
                </div>
            </a>
        </div>
    </div>
</div>

      </div>
    </div>
  </div>
</template>

<style scoped></style>
