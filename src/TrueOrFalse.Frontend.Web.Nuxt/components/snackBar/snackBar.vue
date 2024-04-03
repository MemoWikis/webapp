<script lang="ts" setup>
import { useSnackbarStore } from './snackBarStore'
const snackbarStore = useSnackbarStore()

async function customFunction(id: number, dismiss?: any) {
	const index = snackbarStore.customActions.findIndex(c => c.id == id)
	if (index >= 0) {
		snackbarStore.customActions[index].action()
		await nextTick()
		snackbarStore.customActions.splice(index, 1)
	}
	dismiss()
}
</script>

<template>
    <NuxtSnackbar :duration="4000">
		<template #message-content="{ text, title }">
				<strong v-text="title"></strong>
				<p v-text="text.message"></p>
		</template>
		<template #message-close-icon="{ message, isDismissible, dismiss }">
			<div class="snackbar-content-btn">
				<button v-if="isDismissible" @click="dismiss">
				Close
				</button>
				<div v-if="message.text.buttonId" @click="customFunction(message.text.buttonId, dismiss)" class="snackbar-btn-container" >
					<div class="snackbar-btn">
						{{ message.text.buttonLabel }}
					</div>
				</div>
			</div >
		</template>
	</NuxtSnackbar>
</template>

<style lang="less">
p {
	margin-bottom: 0px;
}

.snackbar-content-btn {
	display: flex;
	justify-content: space-between;
	.snackbar-btn-container {
		display:flex;
		justify-content: center;
		align-items: center;
		padding: 10px;
		cursor: pointer;
		flex-direction: row-reverse;
		.snackbar-btn {
			font-size: 14px;
			text-transform: uppercase;
		}
	}
}
</style>