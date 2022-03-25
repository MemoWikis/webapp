const path = require('path');

module.exports = {
    entry: {
        'tiptapBundle': [
            '/Scripts/npm_builder/tiptap-build/tiptap-vue2.js',
            '/Scripts/npm_builder/tiptap-build/tiptap-starterkit.js',
            '/Scripts/npm_builder/tiptap-build/tiptap-codeblocklowlight.js',
            '/Scripts/npm_builder/tiptap-build/tiptap-image.js',
            '/Scripts/npm_builder/tiptap-build/tiptap-Link.js',
            '/Scripts/npm_builder/tiptap-build/tiptap-placeholder.js',
            '/Scripts/npm_builder/tiptap-build/tiptap-underline.js',
            '/Scripts/npm_builder/tiptap-build/lowlight.js',
            '/Scripts/npm_builder/tiptap-build/hast-util-to-html.js',
        ]
    },
    output: {
        path: path.resolve(__dirname, 'Scripts/npm/tiptap-build/'),
        filename: '[name].js',
      },
    optimization: {
        minimize: true,
        runtimeChunk: 'single',
    }
}