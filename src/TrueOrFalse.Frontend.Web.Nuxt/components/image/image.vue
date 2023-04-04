<script lang="ts" setup>
import { useImageLicenseStore } from './imageLicenseStore'
import { ImageStyle } from './imageStyleEnum'

interface Props {
	url: string,
	alt?: string,
	square?: boolean,
	class?: string,
	style?: ImageStyle,
	showLicense?: boolean,
	imageId?: number
}

const props = defineProps<Props>()

const imageLicenseStore = useImageLicenseStore()
function openImage() {
	if (props.imageId && props.imageId > 0)
		imageLicenseStore.openImage(props.imageId)
}

const imgUrl = ref('')

if (props.url.startsWith('/Images/Categories/') || props.url.startsWith('/Images/Questions/') || props.url.startsWith('/Images/Users/'))
	imgUrl.value = props.url
else
	imgUrl.value = props.url.replace("/Images", "/img")
onBeforeMount(() => {


})
</script>

<template>
	<div class="img-container" :class="props.class">
		<slot name="top"></slot>

		<img v-if="props.style == ImageStyle.Author" :src="imgUrl" class="author" :alt="props.alt" />
		<img v-else :src="imgUrl" class="topic" :alt="props.alt" />

		<div v-if="props.showLicense && props.imageId != undefined && !props.url.includes('no-category-picture')"
			class="license-btn" @click="openImage()">Lizenzinfos
		</div>
		<slot name="bottom"></slot>
	</div>
</template>

<style scoped lang="less">
@import (reference) '~~/assets/includes/imports.less';

.img-container {
	display: flex;
	flex-direction: column;
	align-items: center;

	&.header-author-icon {
		img {
			border-radius: 24px;

		}
	}

	img {
		height: 100%;
		width: 100%;
	}

	.license-btn {
		cursor: pointer;
		color: @memo-grey-dark;
		line-height: 18px;
		font-size: 10px;
		text-align: center;
		z-index: 2;

		&:hover {
			color: @memo-blue-lighter;
		}
	}
}

.topic {
	border-radius: 0;
}

.author {
	border-radius: 50%;
}
</style>