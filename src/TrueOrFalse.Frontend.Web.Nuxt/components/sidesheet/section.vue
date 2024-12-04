<script lang="ts" setup>

</script>

<template>
    <div class="sidesheet-section">
        <div class="header">
            <slot name="header"></slot>
        </div>
        <div class="content">
            <slot name="content"></slot>
        </div>
        <div class="footer">
            <slot name="footer"></slot>
        </div>

    </div>
</template>

<style lang="less">
@import (reference) '~~/assets/includes/imports.less';

.sidesheet-section {
    padding-bottom: 20px;
    background: @memo-grey-lightest;

    .header,
    .content,
    .footer {
        padding: 0 24px;
    }

    .header {
        .header-container {
            color: @memo-grey-darker;
            background: @memo-grey-lightest;
            border-radius: 4px;
            padding: 8px;
            margin-left: -4px;
            margin-right: -4px;
            font-size: 14px;
            margin-bottom: 0px;
            text-transform: uppercase;

            .svg-inline--fa {
                font-size: 14px;
                margin-right: 4px;
                transition: all 0.3s ease-in-out;
                width: 20px;
                display: flex;
                justify-content: center;
                align-items: center;
                text-align: center;
            }

            &:hover {
                filter: brightness(0.95);
            }

            &:active {
                filter: brightness(0.9);
            }

            &.no-hover {
                &:hover {
                    filter: brightness(1);
                }

                &:active {
                    filter: brightness(1);
                }
            }

        }

        .header-title {
            padding: 8px 0p;
            font-weight: 600;
        }
    }

    .content {

        .content-item {
            display: flex;
            flex-wrap: nowrap;
            align-items: center;
            justify-content: space-between;
            border-radius: 4px;
            padding: 2px 16px;
            background: @memo-grey-lightest;
            color: @memo-grey-dark;
            text-overflow: ellipsis;
            overflow: hidden;
            white-space: nowrap;
            user-select: none;
            padding-right: 2px;
            height: 26px;

            &:hover {
                filter: brightness(0.95);
            }

            &:active {
                filter: brightness(0.9);
            }

            a {
                display: block;
                text-decoration: none;
                color: @memo-grey-dark;
                flex-grow: 2;
                max-width: 100%;

                .link {
                    text-overflow: ellipsis;
                    overflow: hidden;
                    white-space: nowrap;
                    color: @memo-grey-dark;

                    &:hover {
                        color: @memo-grey-darker;
                    }
                }

                &.is-here {
                    color: @memo-blue-link;
                    font-weight: 600;

                    .link {
                        color: @memo-blue-link;
                    }
                }
            }

            .content-item-options {
                display: flex;
                justify-content: center;
                align-items: center;
                cursor: pointer;
                opacity: 0;
                border-radius: 4px;
                margin: 2px;
                height: 20px;
                width: 20px;
                padding: 2px;
                background: @memo-grey-lightest;

                .svg-inline--fa {
                    color: @memo-grey-dark;


                }

                &:hover {
                    filter: brightness(0.9);
                }
            }

            &:hover {
                .content-item-options {
                    opacity: 1;
                }
            }
        }

    }

    .footer {
        display: flex;
        justify-content: center;
        align-items: center;
        padding-top: 16px;

        .sidesheet-button {
            border-radius: 4px;
            padding: 8px 16px;
            background: @memo-grey-lightest;
            color: @memo-grey-dark;
            text-overflow: ellipsis;
            overflow: hidden;
            white-space: nowrap;
            cursor: pointer;
            flex-grow: 2;
            border: 1px solid @memo-grey-light;
            text-align: center;
            margin-left: 16px;
            // margin-right: 16px;

            * {
                margin-right: 8px;
            }

            &:hover {
                filter: brightness(0.95);
            }

            &:active {
                filter: brightness(0.9);
            }

            &.disabled {
                cursor: not-allowed;
                opacity: 0.5;

                &:hover {
                    filter: brightness(1);
                }

                &:active {
                    filter: brightness(1);
                }
            }
        }
    }
}

#SideSheet {

    .no-b-padding {
        .footer {
            padding-top: 0px;
        }
    }

    #SideSheetContainer,
    #SideSheetFooter {
        .header-container {
            display: flex;
            flex-wrap: nowrap;
            align-items: center;
            flex-direction: row;
            cursor: pointer;
            user-select: none;

            .angle-icon {
                color: @memo-grey-light;
                width: 16px;
                margin-right: 0px;
            }
        }
    }

    &.collapsed {
        .footer {
            padding-top: 0px;
            display: none;
        }

        #SideSheetContainer,
        #SideSheetFooter {
            .footer {
                display: none;
            }

            .header-container {
                flex-direction: column;
                text-align: center;
                padding: 4px 0;
                font-size: 14px;

                .svg-inline--fa {
                    font-size: 14px;
                    margin-right: 0px;
                }

                .header-title {
                    font-size: 1rem;
                    margin-top: 4px;
                }
            }


        }



    }

    &.animate-header {
        .header-container {
            transform-origin: left;
            animation: grow 0.3s ease-in-out;
        }
    }
}

@keyframes grow {
    0% {
        transform: scale(0);
        opacity: 0;
    }

    100% {
        transform: scale(1);
        opacity: 1;
    }
}
</style>