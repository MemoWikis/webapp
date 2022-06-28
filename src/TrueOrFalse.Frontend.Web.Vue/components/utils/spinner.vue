    <script lang="ts" setup>
        import {useUtilsStore} from './utilsStore'
        const utilsStore = useUtilsStore()
    </script>

    <template>
        <div class="spinner" v-show="utilsStore.showSpinner">
            <div class="double-bounce1"></div>
            <div class="double-bounce2"></div>
        </div>
    </template>