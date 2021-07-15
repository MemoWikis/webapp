const path = require('path');

module.exports = {
    entry: '/Scripts/npm_builder/tiptap-build/tiptap-build.js',
    output: {
        path: path.resolve(__dirname, 'Scripts/npm/tiptap-build/'),
        filename: 'tiptap-build.js',
      },
    optimization: {
        minimize: true,
    }
}