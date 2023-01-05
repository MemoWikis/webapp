<script setup lang="ts">

</script>

<template>
    <LazyModal>
        <template v-slot:header>
            Bilddetails
        </template>
        <template v-slot:body>
            <div class="ImageContainer">
                <img />
                <%= Model.RenderHtmlImageBasis(1000, false, Model.ImageMetaData.Type) %>
                    <div class="ImageInfo">
                        <% if (!String.IsNullOrEmpty(Model.AttributionHtmlString)){ %>
                            <div>
                                <%= Model.AttributionHtmlString %>
                            </div>
                            <% } %>
                                <% if (!String.IsNullOrEmpty(WebUtility.HtmlEncode(Model.Description))){ %>
                                    <div style="margin-top: 10px;"><span class="InfoLabel">Beschreibung:</span>
                                        <%= WebUtility.HtmlEncode(Model.Description) %>
                                    </div>
                                    <% } %>
                    </div>
            </div>
        </template>
        <template v-slot:footer>

        </template>
    </LazyModal>

</template>