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
@memo-grey-lightest: #f9f9f9;

.sidesheet-section {
    padding-bottom: 20px;
    background: @memo-grey-lightest;

    .header,
    .content,
    .footer {
        padding: 0 10px;
    }

    .content,
    .footer {
        padding-left: 24px;
    }

    .header {
        h4 {
            color: @memo-grey-darker;
            background: @memo-grey-lightest;
            border-radius: 4px;
            padding: 8px;
            margin-left: -4px;
            margin-right: -4px;
            font-size: 14px;

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

        }

        .header-title {
            padding: 8px 0p;
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

            &:hover {
                filter: brightness(0.95);
                color: @memo-blue-link;
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

                .svg-inline--fa {
                    color: @memo-grey-dark;

                    &:hover {
                        color: @memo-blue-link;
                    }
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
        // justify-content: center;
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

            border: 1px solid @memo-grey-light;

            * {
                margin-right: 8px;
            }

            &:hover {
                filter: brightness(0.95);
            }

            &:active {
                filter: brightness(0.9);
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

    &.collapsed {
        .footer {
            padding-top: 0px;
        }
    }
}
</style>