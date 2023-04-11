<script lang="ts" setup></script>
<template>
    <div class="col-xs-12 container">
        <div class="error-page">
            <Image src="/Images/Error/memo-500_german_600.png" class="error-image" />
            <div class="error-message">
                <p>Oder schicke eine E-Mail an team@memucho.de.</p>
                <ul>
                    <li>Wir wurden per E-Mail informiert.</li>
                    <li>Bei dringenden Fragen kannst du Robert unter 0178-1866848 erreichen.</li>
                    <li>Oder schicke eine E-Mail an team@memucho.de.</li>
                </ul>
            </div>
        </div>
    </div>
</template>
<style lang="less" scoped>
.error-page {
    display: flex;
    justify-content: center;
    align-items: center;
    padding-top: 60px;
    padding-bottom: 60px;
    flex-direction: column;

    .error-image {
        max-width: 600px;
        margin-bottom: 60px;
    }

    .error-message {
        width: 100%;
    }
}
</style>