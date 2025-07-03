//based on: https://github.com/bae-sh/tiptap-extension-resize-image

import Image from '@tiptap/extension-image'
import { dom } from '@fortawesome/fontawesome-svg-core'
import { color } from '~/constants/colors'

const borderStyle = `1px dashed ${color.memoGreen}`
const resizeHandle = `position: absolute; width: 9px; height: 9px; border: 2px solid ${color.memoGreyDarker}; border-radius: 50%;`
const fontColor = `${color.memoGreyLighter}`
const defaultIconStyle = `
            background: white;
            border: hidden;
            font-size: 18px;
            width: 36px;
            height: 36px;
            margin: 0px;
            color: ${color.memoGreyDarker};
            text-align: center;
            padding: 0px 21px;
            display: flex;
            justify-content: center;
            align-items: center;
            transition: filter 0.1s;        
            `

const ImageResize = Image.extend({
    name: 'figure',

    addAttributes() {
        return {
            src: {
                default: null,
            },
            alt: {
                default: null,
            },
            style: {
                default: 'width: 100%; height: auto; cursor: pointer;',
                parseHTML: (element) => {
                    // For figure elements, get style from the figure itself
                    if (element.tagName === 'FIGURE') {
                        const width = element.getAttribute('width')
                        return width
                            ? `width: ${width}px; height: auto; cursor: pointer;`
                            : element.style.cssText || 'width: 100%; height: auto; cursor: pointer;'
                    }
                    // For legacy img elements, get style from the img
                    const width = element.getAttribute('width')
                    return width
                        ? `width: ${width}px; height: auto; cursor: pointer;`
                        : element.style.cssText || 'width: 100%; height: auto; cursor: pointer;'
                },
            },
            caption: {
                default: null,
                parseHTML: (element) => {
                    const figcaption = element.querySelector('figcaption')
                    return figcaption?.textContent || null
                },
            },
            license: {
                default: null,
                parseHTML: (element) => {
                    const figcaption = element.querySelector('figcaption')
                    return figcaption?.getAttribute('data-license') || null
                },
            },
            title: {
                default: null,
            },
            loading: {
                default: null,
            },
            srcset: {
                default: null,
            },
            sizes: {
                default: null,
            },
            crossorigin: {
                default: null,
            },
            usemap: {
                default: null,
            },
            ismap: {
                default: null,
            },
            width: {
                default: null,
            },
            height: {
                default: null,
            },
            referrerpolicy: {
                default: null,
            },
            longdesc: {
                default: null,
            },
            decoding: {
                default: null,
            },
            class: {
                default: null,
            },
            id: {
                default: null,
            },
            name: {
                default: null,
            },
            draggable: {
                default: true,
            },
            tabindex: {
                default: null,
            },
            'aria-label': {
                default: null,
            },
            'aria-labelledby': {
                default: null,
            },
            'aria-describedby': {
                default: null,
            },
        }
    },

    parseHTML() {
        return [
            {
                tag: 'figure',
                getAttrs: (element) => {
                    const img = element.querySelector('img')
                    const figcaption = element.querySelector('figcaption')
                    
                    if (!img) return false
                    
                    return {
                        src: img.getAttribute('src'),
                        alt: img.getAttribute('alt'),
                        title: img.getAttribute('title'),
                        caption: figcaption?.textContent || null,
                        license: figcaption?.getAttribute('data-license') || null,
                        style: element.style.cssText || 'width: 100%; height: auto; cursor: pointer;'
                    }
                }
            },
            {
                tag: 'img',
                getAttrs: (element) => {
                    return {
                        src: element.getAttribute('src'),
                        alt: element.getAttribute('alt'),
                        title: element.getAttribute('title'),
                        caption: null,
                        style: element.style.cssText || 'width: 100%; height: auto; cursor: pointer;'
                    }
                }
            }
        ]
    },

    renderHTML({ HTMLAttributes }) {
        const { caption, license, src, alt, style, ...imgAttrs } = HTMLAttributes
        
        const imgElement = ['img', { src, alt, class: 'tiptap-image', ...imgAttrs }]
        
        if (caption) {
            const figcaptionAttrs = license ? { 'data-license': license, class: 'tiptap-figcaption' } : { class: 'tiptap-figcaption' }
            return [
                'figure',
                { style, class: 'tiptap-figure' },
                imgElement,
                ['figcaption', figcaptionAttrs, caption]
            ]
        } else {
            return [
                'figure',
                { style, class: 'tiptap-figure' },
                imgElement
            ]
        }
    },

    addNodeView() {
        return ({ node, editor, getPos }) => {
            const {
                view,
                options: { editable },
            } = editor
            const { src, alt, caption, license, style } = node.attrs
            
            // Create wrapper and container elements
            const $wrapper = document.createElement('div')
            const $container = document.createElement('figure')
            const $img = document.createElement('img')
            const iconStyle = `
                background: white;
                border: hidden;
                font-size: 18px;
                width: 36px;
                height: 36px;
                margin: 0px;
                color: ${color.memoGreyDarker};
                text-align: center;
                padding: 0px 21px;
                display: flex;
                justify-content: center;
                align-items: center;
                transition: filter 0.1s;        
            `

            const dispatchNodeView = () => {
                if (typeof getPos === 'function') {
                    const newAttrs = {
                        ...node.attrs,
                        style: `${$container.style.cssText}`,
                    }
                    view.dispatch(view.state.tr.setNodeMarkup(getPos(), null, newAttrs))
                }
            }

            const showCaptionModal = () => {
                // Create modal overlay
                const $modalOverlay = document.createElement('div')
                $modalOverlay.setAttribute('style', `
                    position: fixed;
                    top: 0;
                    left: 0;
                    width: 100%;
                    height: 100%;
                    background-color: rgba(0, 0, 0, 0.5);
                    z-index: 10000;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                `)

                // Create modal dialog
                const $modal = document.createElement('div')
                $modal.setAttribute('style', `
                    background: white;
                    border-radius: 8px;
                    padding: 20px;
                    width: 400px;
                    max-width: 90vw;
                    box-shadow: 0 4px 20px rgba(0, 0, 0, 0.15);
                `)

                // Create form elements
                const $title = document.createElement('h3')
                $title.textContent = 'Edit Image Caption and License'
                $title.setAttribute('style', 'margin: 0 0 20px 0; color: #333; font-size: 18px;')

                const $captionLabel = document.createElement('label')
                $captionLabel.textContent = 'Caption:'
                $captionLabel.setAttribute('style', 'display: block; margin-bottom: 5px; font-weight: bold; color: #555; font-size: 14px;')

                const $captionInput = document.createElement('textarea')
                $captionInput.value = node.attrs.caption || ''
                $captionInput.setAttribute('style', `
                    width: 100%;
                    height: 80px;
                    border: 1px solid #ddd;
                    border-radius: 4px;
                    padding: 8px;
                    margin-bottom: 15px;
                    font-family: inherit;
                    font-size: 14px;
                    resize: vertical;
                    box-sizing: border-box;
                    transition: border-color 0.2s;
                `)
                $captionInput.placeholder = 'Enter image caption...'

                // Add focus styles
                $captionInput.addEventListener('focus', () => {
                    $captionInput.style.borderColor = color.memoGreen
                    $captionInput.style.outline = 'none'
                })
                $captionInput.addEventListener('blur', () => {
                    $captionInput.style.borderColor = '#ddd'
                })

                const $licenseLabel = document.createElement('label')
                $licenseLabel.textContent = 'License:'
                $licenseLabel.setAttribute('style', 'display: block; margin-bottom: 5px; font-weight: bold; color: #555; font-size: 14px;')

                const $licenseInput = document.createElement('input')
                $licenseInput.type = 'text'
                $licenseInput.value = node.attrs.license || ''
                $licenseInput.setAttribute('style', `
                    width: 100%;
                    border: 1px solid #ddd;
                    border-radius: 4px;
                    padding: 8px;
                    margin-bottom: 20px;
                    font-family: inherit;
                    font-size: 14px;
                    box-sizing: border-box;
                    transition: border-color 0.2s;
                `)
                $licenseInput.placeholder = 'Enter license information (e.g., CC BY 4.0, © Author Name)...'

                // Add focus styles
                $licenseInput.addEventListener('focus', () => {
                    $licenseInput.style.borderColor = color.memoGreen
                    $licenseInput.style.outline = 'none'
                })
                $licenseInput.addEventListener('blur', () => {
                    $licenseInput.style.borderColor = '#ddd'
                })

                // Create button container
                const $buttonContainer = document.createElement('div')
                $buttonContainer.setAttribute('style', 'display: flex; justify-content: flex-end; gap: 10px;')

                const $cancelButton = document.createElement('button')
                $cancelButton.textContent = 'Cancel'
                $cancelButton.setAttribute('style', `
                    padding: 10px 20px;
                    border: 1px solid #ddd;
                    border-radius: 4px;
                    background: white;
                    cursor: pointer;
                    font-family: inherit;
                    font-size: 14px;
                    transition: background-color 0.2s, border-color 0.2s;
                `)

                const $saveButton = document.createElement('button')
                $saveButton.textContent = 'Save'
                $saveButton.setAttribute('style', `
                    padding: 10px 20px;
                    border: none;
                    border-radius: 4px;
                    background: ${color.memoGreen};
                    color: white;
                    cursor: pointer;
                    font-family: inherit;
                    font-size: 14px;
                    font-weight: bold;
                    transition: background-color 0.2s;
                `)

                // Add hover effects
                $cancelButton.addEventListener('mouseenter', () => {
                    $cancelButton.style.backgroundColor = '#f5f5f5'
                    $cancelButton.style.borderColor = '#bbb'
                })
                $cancelButton.addEventListener('mouseleave', () => {
                    $cancelButton.style.backgroundColor = 'white'
                    $cancelButton.style.borderColor = '#ddd'
                })

                $saveButton.addEventListener('mouseenter', () => {
                    $saveButton.style.backgroundColor = '#45a049'
                })
                $saveButton.addEventListener('mouseleave', () => {
                    $saveButton.style.backgroundColor = color.memoGreen
                })

                // Add event listeners
                $cancelButton.addEventListener('click', () => {
                    document.body.removeChild($modalOverlay)
                })

                $saveButton.addEventListener('click', () => {
                    const newCaption = $captionInput.value.trim() || null
                    const newLicense = $licenseInput.value.trim() || null
                    
                    // Update node attributes
                    if (typeof getPos === 'function') {
                        const newAttrs = {
                            ...node.attrs,
                            caption: newCaption,
                            license: newLicense
                        }
                        view.dispatch(view.state.tr.setNodeMarkup(getPos(), null, newAttrs))
                    }

                    // Update figcaption in DOM
                    const existingFigcaption = $container.querySelector('figcaption')
                    if (existingFigcaption) {
                        $container.removeChild(existingFigcaption)
                    }

                    if (newCaption) {
                        const $figcaption = document.createElement('figcaption')
                        $figcaption.className = 'tiptap-figcaption'
                        $figcaption.textContent = newCaption
                        if (newLicense) {
                            $figcaption.setAttribute('data-license', newLicense)
                        }
                        $container.appendChild($figcaption)
                    }

                    document.body.removeChild($modalOverlay)
                })

                // Close modal on overlay click
                $modalOverlay.addEventListener('click', (e) => {
                    if (e.target === $modalOverlay) {
                        document.body.removeChild($modalOverlay)
                    }
                })

                // Escape key closes modal, Enter saves (if not in textarea)
                const escapeHandler = (e) => {
                    if (e.key === 'Escape') {
                        document.body.removeChild($modalOverlay)
                        document.removeEventListener('keydown', escapeHandler)
                    } else if (e.key === 'Enter' && e.target !== $captionInput && !e.shiftKey) {
                        e.preventDefault()
                        $saveButton.click()
                        document.removeEventListener('keydown', escapeHandler)
                    }
                }
                document.addEventListener('keydown', escapeHandler)

                // Assemble modal
                $buttonContainer.appendChild($cancelButton)
                $buttonContainer.appendChild($saveButton)

                $modal.appendChild($title)
                $modal.appendChild($captionLabel)
                $modal.appendChild($captionInput)
                $modal.appendChild($licenseLabel)
                $modal.appendChild($licenseInput)
                $modal.appendChild($buttonContainer)

                $modalOverlay.appendChild($modal)
                document.body.appendChild($modalOverlay)

                // Focus on caption input
                $captionInput.focus()
            }

            const paintPositionController = () => {
                const $positionController = document.createElement('div')

                const $leftController = document.createElement('div')
                const $centerController = document.createElement('div')
                const $rightController = document.createElement('div')

                const controllerMouseOver = (e) => {
                    e.target.style.opacity = 0.3
                }

                const controllerMouseOut = (e) => {
                    e.target.style.opacity = 1
                }

                $positionController.setAttribute('class', 'position-controller')

                // Set up left alignment button
                $leftController.classList.add('menubar_button')
                const leftIcon = document.createElement('i')
                leftIcon.classList.add('fa-solid', 'fa-align-left')
                $leftController.appendChild(leftIcon)
                $leftController.setAttribute('style', defaultIconStyle)
                $leftController.addEventListener('mouseover', controllerMouseOver)
                $leftController.addEventListener('mouseout', controllerMouseOut)
                $leftController.addEventListener('click', () => {
                    $container.setAttribute('style', `${$container.style.cssText} margin: 0 auto 0 0;`)
                    dispatchNodeView()
                })

                // Set up center alignment button  
                $centerController.classList.add('menubar_button')
                const centerIcon = document.createElement('i')
                centerIcon.classList.add('fa-solid', 'fa-align-center')
                $centerController.appendChild(centerIcon)
                $centerController.setAttribute('style', defaultIconStyle)
                $centerController.addEventListener('mouseover', controllerMouseOver)
                $centerController.addEventListener('mouseout', controllerMouseOut)
                $centerController.addEventListener('click', () => {
                    $container.setAttribute('style', `${$container.style.cssText} margin: 0 auto;`)
                    dispatchNodeView()
                })

                // Set up right alignment button
                $rightController.classList.add('menubar_button')
                const rightIcon = document.createElement('i')
                rightIcon.classList.add('fa-solid', 'fa-align-right')
                $rightController.appendChild(rightIcon)
                $rightController.setAttribute('style', defaultIconStyle)
                $rightController.addEventListener('mouseover', controllerMouseOver)
                $rightController.addEventListener('mouseout', controllerMouseOut)
                $rightController.addEventListener('click', () => {
                    $container.setAttribute('style', `${$container.style.cssText} margin: 0 0 0 auto;`)
                    dispatchNodeView()
                })

                $positionController.appendChild($leftController)
                $positionController.appendChild($centerController)
                $positionController.appendChild($rightController)

                $container.appendChild($positionController)

                // Add separate caption/license editing button
                const $captionController = document.createElement('div')
                $captionController.classList.add('menubar_button')
                const captionIcon = document.createElement('i')
                captionIcon.classList.add('fa-solid', 'fa-pen')
                $captionController.appendChild(captionIcon)
                $captionController.setAttribute('style', `
                    position: absolute;
                    top: 30px;
                    left: 50%;
                    transform: translateX(-50%);
                    ${defaultIconStyle}
                    font-size: 14px;
                    border-radius: 4px;
                    z-index: 999;
                `)
                $captionController.title = 'Edit caption and license'
                $captionController.addEventListener('mouseover', controllerMouseOver)
                $captionController.addEventListener('mouseout', controllerMouseOut)
                $captionController.addEventListener('click', (e) => {
                    e.preventDefault()
                    e.stopPropagation()
                    showCaptionModal()
                })

                $container.appendChild($captionController)

                // Convert Font Awesome icons to SVG
                if (dom && dom.i2svg) {
                    dom.i2svg({ node: $container })
                }
            }

            // Set up wrapper and container
            $wrapper.setAttribute('style', `display: flex;`)
            $wrapper.appendChild($container)

            $container.setAttribute('style', `${style}`)
            $container.className = 'tiptap-figure'
            
            // Set up image
            $img.setAttribute('src', src || '')
            $img.className = 'tiptap-image'
            if (alt) $img.setAttribute('alt', alt)
            $container.appendChild($img)

            // Add figcaption if caption exists
            if (caption) {
                const $figcaption = document.createElement('figcaption')
                $figcaption.className = 'tiptap-figcaption'
                $figcaption.textContent = caption
                if (license) {
                    $figcaption.setAttribute('data-license', license)
                }
                $container.appendChild($figcaption)
            }

            // If not editable, return simple view
            if (!editable) return { dom: $container }

            // Add floating caption button on hover
            const $floatingButton = document.createElement('button')
            const floatingIcon = document.createElement('i')
            floatingIcon.classList.add('fa-solid', 'fa-pen')
            $floatingButton.appendChild(floatingIcon)
            $floatingButton.title = 'Edit caption and license'
            $floatingButton.setAttribute('style', `
                position: absolute;
                top: 10px;
                right: 10px;
                width: 30px;
                height: 30px;
                border-radius: 50%;
                border: none;
                background: rgba(255, 255, 255, 0.9);
                color: ${color.memoGreyDarker};
                font-size: 12px;
                cursor: pointer;
                box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
                opacity: 0;
                transition: opacity 0.2s;
                z-index: 1000;
                display: flex;
                align-items: center;
                justify-content: center;
            `)

            $floatingButton.addEventListener('click', (e) => {
                e.preventDefault()
                e.stopPropagation()
                showCaptionModal()
            })

            $container.appendChild($floatingButton)

            // Convert Font Awesome icons to SVG for floating button
            if (dom && dom.i2svg) {
                dom.i2svg({ node: $floatingButton })
            }

            // Show/hide floating button on hover
            $container.addEventListener('mouseenter', () => {
                $floatingButton.style.opacity = '1'
            })

            $container.addEventListener('mouseleave', () => {
                $floatingButton.style.opacity = '0'
            })

            // Add resize functionality for editable mode
            const isMobile = document.documentElement.clientWidth < 768
            const dotPosition = isMobile ? '-8px' : '-4px'
            const dotsPosition = [
                `top: ${dotPosition}; left: ${dotPosition}; cursor: nwse-resize;`,
                `top: ${dotPosition}; right: ${dotPosition}; cursor: nesw-resize;`,
                `bottom: ${dotPosition}; left: ${dotPosition}; cursor: nesw-resize;`,
                `bottom: ${dotPosition}; right: ${dotPosition}; cursor: nwse-resize;`,
            ]

            let isResizing = false
            let startX, startWidth

            $container.addEventListener('click', (e) => {
                // Remove remaining dots and position controller (but keep floating button)
                const isMobile = document.documentElement.clientWidth < 768
                isMobile && (document.querySelector('.ProseMirror-focused')?.blur())

                // Count base elements: img + optional figcaption + floating button
                const baseElementCount = (caption ? 2 : 1) + 1 // +1 for floating button
                if ($container.childElementCount > baseElementCount) {
                    // Remove all controls except the floating button and base elements
                    const elementsToKeep = [$img, $floatingButton]
                    if (caption) {
                        const figcaption = $container.querySelector('figcaption')
                        if (figcaption) elementsToKeep.push(figcaption)
                    }
                    
                    const children = Array.from($container.children)
                    children.forEach(child => {
                        if (!elementsToKeep.includes(child)) {
                            $container.removeChild(child)
                        }
                    })
                }

                paintPositionController()

                // Add resize handles to the figure
                $container.setAttribute(
                    'style',
                    `position: relative; border: ${borderStyle}; ${style} cursor: pointer;`
                )

                Array.from({ length: 4 }, (_, index) => {
                    const $dot = document.createElement('div')
                    $dot.setAttribute(
                        'style',
                        `${resizeHandle} ${dotsPosition[index]}`
                    )

                    $dot.addEventListener('mousedown', (e) => {
                        e.preventDefault()
                        isResizing = true
                        startX = e.clientX
                        startWidth = $container.offsetWidth

                        const onMouseMove = (e) => {
                            if (!isResizing) return
                            const deltaX = index % 2 === 0 ? -(e.clientX - startX) : e.clientX - startX
                            const newWidth = startWidth + deltaX

                            $container.style.width = newWidth + 'px'
                        }

                        const onMouseUp = () => {
                            if (isResizing) {
                                isResizing = false
                            }
                            dispatchNodeView()

                            document.removeEventListener('mousemove', onMouseMove)
                            document.removeEventListener('mouseup', onMouseUp)
                        }

                        document.addEventListener('mousemove', onMouseMove)
                        document.addEventListener('mouseup', onMouseUp)
                    })

                    // Touch support for mobile
                    $dot.addEventListener(
                        'touchstart',
                        (e) => {
                            e.cancelable && e.preventDefault()
                            isResizing = true
                            startX = e.touches[0].clientX
                            startWidth = $container.offsetWidth

                            const onTouchMove = (e) => {
                                if (!isResizing) return
                                const deltaX = index % 2 === 0 
                                    ? -(e.touches[0].clientX - startX) 
                                    : e.touches[0].clientX - startX
                                const newWidth = startWidth + deltaX

                                $container.style.width = newWidth + 'px'
                            }

                            const onTouchEnd = () => {
                                if (isResizing) {
                                    isResizing = false
                                }
                                dispatchNodeView()

                                document.removeEventListener('touchmove', onTouchMove)
                                document.removeEventListener('touchend', onTouchEnd)
                            }

                            document.addEventListener('touchmove', onTouchMove)
                            document.addEventListener('touchend', onTouchEnd)
                        },
                        { passive: false }
                    )
                    
                    $container.appendChild($dot)
                })
            })

            // Click outside to remove controls
            document.addEventListener('click', (e) => {
                const $target = e.target
                const isClickInside = $container.contains($target) || $target.style.cssText === iconStyle

                if (!isClickInside) {
                    const containerStyle = $container.getAttribute('style')
                    const newStyle = containerStyle?.replace(`border: ${borderStyle};`, '')
                    $container.setAttribute('style', newStyle || '')

                    // Count base elements: img + optional figcaption + floating button
                    const baseElementCount = (caption ? 2 : 1) + 1 // +1 for floating button
                    if ($container.childElementCount > baseElementCount) {
                        // Remove all controls except the floating button and base elements
                        const elementsToKeep = [$img, $floatingButton]
                        if (caption) {
                            const figcaption = $container.querySelector('figcaption')
                            if (figcaption) elementsToKeep.push(figcaption)
                        }
                        
                        const children = Array.from($container.children)
                        children.forEach(child => {
                            if (!elementsToKeep.includes(child)) {
                                $container.removeChild(child)
                            }
                        })
                    }
                }
            })

            return {
                dom: $wrapper,
            }
        }
    },
})

export { ImageResize, ImageResize as default }
