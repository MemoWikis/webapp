<script lang="ts" setup>
import { useSnackbarStore } from './snackBarStore'
const snackbarStore = useSnackbarStore()

async function customFunction(id: number) {
	const index = snackbarStore.customActions.findIndex(c => c.id == id)
	console.log(index)
	if (index >= 0) {
		snackbarStore.customActions[index].action()
		await nextTick()
		snackbarStore.splice(index, 1)
	}
}
</script>

<template>
    <NuxtSnackbar bottom right :duration="40000">
		<template #message-content="{ text, title }">
			<template v-if="text.buttonId">
				<div class="snackbar-content-btn">
					<div class="">
						<strong v-text="title"></strong>
						<p v-text="text.message"></p>
					</div>
					<div @click="customFunction(text.buttonId)" class="snackbar-btn-container">
						<div class="snackbar-btn">
							{{ text.buttonLabel }}
						</div>
					</div>
				</div >
			</template>
			<template v-else>
				<strong v-text="title"></strong>
				<p v-text="text.message"></p>
			</template>
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
		.snackbar-btn {
			font-size: 14px;
			text-transform: uppercase;
		}
	}
}
</style>