<my-world-toggle-component inline-template>
    <div class="toggle-container">
        <input type="checkbox" id="mwcbx" style="display:none" data-allowed="logged-in" v-model="showMyWorld" @click="toggleMyWorld()" />
        <label for="mwcbx" class="toggle">
            <span></span>
        </label>
    </div>
</my-world-toggle-component>
