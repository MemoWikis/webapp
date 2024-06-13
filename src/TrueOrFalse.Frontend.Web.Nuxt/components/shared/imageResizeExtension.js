//based on: https://github.com/bae-sh/tiptap-extension-resize-image

import Image from '@tiptap/extension-image'
import { color } from '~~/components/shared/colors'

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
                parseHTML: element => {
                    const width = element.getAttribute('width');
                    return width
                        ? `width: ${width}px; height: auto; cursor: pointer;`
                        : `${element.style.cssText}`;
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
        };
    },
    addNodeView() {
        return ({ node, editor, getPos }) => {            
            const { view, options: { editable }, } = editor;
            const { style } = node.attrs;
            const $wrapper = document.createElement('div');
            const $container = document.createElement('div');
            const $img = document.createElement('img');

            const dispatchNodeView = () => {
                if (typeof getPos === 'function') {
                    const newAttrs = Object.assign(Object.assign({}, node.attrs), { style: `${$img.style.cssText}` });

                    view.dispatch(view.state.tr.setNodeMarkup(getPos(), null, newAttrs));
                }
            };

            const paintPositionContoller = () => {
                const $postionController = document.createElement('div');
                $postionController.classList.add('position-controller');

                const $leftController = document.createElement('div');
                $leftController.classList.add('menubar_button')
                const leftIcon = document.createElement('i');
                leftIcon.classList.add('fa-solid', 'fa-align-left');
                $leftController.appendChild(leftIcon);

                const $centerController = document.createElement('div');
                $centerController.classList.add('menubar_button')
                const centerIcon = document.createElement('i');
                centerIcon.classList.add('fa-solid', 'fa-align-center');
                $centerController.appendChild(centerIcon);

                const $rightController = document.createElement('div');
                $rightController.classList.add('menubar_button')
                const rightIcon = document.createElement('i');
                rightIcon.classList.add('fa-solid', 'fa-align-right');
                $rightController.appendChild(rightIcon);
                
                $leftController.addEventListener('click', () => {
                    $img.setAttribute('style', `${$img.style.cssText} margin: 0 auto 0 0;`);
                    dispatchNodeView();
                });
                $centerController.addEventListener('click', () => {
                    $img.setAttribute('style', `${$img.style.cssText} margin: 0 auto;`);
                    dispatchNodeView();
                });
                $rightController.addEventListener('click', () => {
                    $img.setAttribute('style', `${$img.style.cssText} margin: 0 0 0 auto;`);
                    dispatchNodeView();
                });
                $postionController.appendChild($leftController);
                $postionController.appendChild($centerController);
                $postionController.appendChild($rightController);
                $container.appendChild($postionController);
            };
            $wrapper.setAttribute('style', `display: flex;`);
            $wrapper.appendChild($container);
            $container.setAttribute('style', `${style}`);
            $container.appendChild($img);
            Object.entries(node.attrs).forEach(([key, value]) => {
                if (value === undefined || value === null)
                    return;
                $img.setAttribute(key, value);
            });
            if (!editable)
                return { dom: $img };
            const dotsPosition = [
                'top: -4px; left: -4px; cursor: nwse-resize;',
                'top: -4px; right: -4px; cursor: nesw-resize;',
                'bottom: -4px; left: -4px; cursor: nesw-resize;',
                'bottom: -4px; right: -4px; cursor: nwse-resize;',
            ];
            let isResizing = false;
            let startX, startWidth, startHeight;
            $container.addEventListener('click', () => {
                //remove remaining dots and position controller
                if ($container.childElementCount > 3) {
                    for (let i = 0; i < 5; i++) {
                        $container.removeChild($container.lastChild);
                    }
                }
                paintPositionContoller();
                
                $container.setAttribute('style', `position: relative; outline: ${borderStyle}; ${style} cursor: pointer;`);
                Array.from({ length: 4 }, (_, index) => {
                    const $dot = document.createElement('div');
                    $dot.setAttribute('style', `${resizeHandle} ${dotsPosition[index]}`);
                    $dot.addEventListener('mousedown', e => {
                        e.preventDefault();
                        isResizing = true;
                        startX = e.clientX;
                        startWidth = $container.offsetWidth;
                        startHeight = $container.offsetHeight;
                        const onMouseMove = (e) => {
                            if (!isResizing)
                                return;
                            const deltaX = index % 2 === 0 ? -(e.clientX - startX) : e.clientX - startX;
                            const aspectRatio = startWidth / startHeight;
                            const newWidth = startWidth + deltaX;
                            const newHeight = newWidth / aspectRatio;
                            $container.style.width = newWidth + 'px';
                            $container.style.height = newHeight + 'px';
                            $img.style.width = newWidth + 'px';
                            $img.style.height = newHeight + 'px';
                        };
                        const onMouseUp = () => {
                            if (isResizing) {
                                isResizing = false;
                            }
                            dispatchNodeView();
                            document.removeEventListener('mousemove', onMouseMove);
                            document.removeEventListener('mouseup', onMouseUp);
                        };
                        document.addEventListener('mousemove', onMouseMove);
                        document.addEventListener('mouseup', onMouseUp);
                    });
                    $container.appendChild($dot);
                });
            });
            document.addEventListener('click', (e) => {
                const $target = e.target;
                if ($target){
                    const isClickInside = $container.contains($target) || $target.style.cssText === defaultIconStyle;
                    if (!isClickInside) {
                        const containerStyle = $container.getAttribute('style');
                        const newStyle = containerStyle === null || containerStyle === void 0 ? void 0 : containerStyle.replace(`outline: ${borderStyle}`, '');
                        $container.setAttribute('style', newStyle);
                        if ($container.childElementCount > 3) {
                            for (let i = 0; i < 5; i++) {
                                $container.removeChild($container.lastChild);
                            }
                        }
                    }
                }
            });
            return {
                dom: $wrapper,
            };
        };
    },
});

export { ImageResize, ImageResize as default };