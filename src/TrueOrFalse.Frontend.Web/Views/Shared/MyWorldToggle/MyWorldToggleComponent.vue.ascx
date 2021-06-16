<%@ Control Language="C#" AutoEventWireup="true" 
Inherits="System.Web.Mvc.ViewUserControl<CategoryModel>" %>

<my-world-toggle-component inline-template is-my-world="<%= Model.IsMyWorld %>">
    <div class="toggle-container" @click="toggleMyWorld()" >
        <input type="checkbox" id="mwcbx" style="display:none" v-model="showMyWorld" :disabled="disabled"/>
        <label for="mwcbx" class="toggle">
            <span></span>
        </label>
    </div>
</my-world-toggle-component>
