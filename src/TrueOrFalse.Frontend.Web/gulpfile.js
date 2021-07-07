/// <binding AfterBuild='copyScripts' />
var deps = {
    "vue": {
        "dist/vue.js": ""
    },
    "vue-textarea-autosize": {
        "dist/vue-textarea-autosize.umd.js": ""
    },
    "vue-sticky-directive": {
        "vue-sticky-directive.js": ""
    },
    "postscribe": {
        "dist/postscribe.js": ""
    },
    "d3": {
        "dist/d3.js": ""
    },
    "vue-slider-component" : {
        "dist/vue-slider-component.umd.js": ""
    },
    "@tiptap" : {
        "core/dist/tiptap-core.umd.js": "",
        "core/node_modules/prosemirror-commands/dist/index.js": "prosemirror-commands",
        "core/node_modules/prosemirror-model/dist/index.js": "prosemirror-model",
        "core/node_modules/prosemirror-state/dist/index.js": "prosemirror-state",
        "core/node_modules/prosemirror-view/dist/index.js": "prosemirror-view",
        "extension-blockquote/dist/tiptap-extension-blockquote.umd.js": "tiptap-extension",
        "extension-bold/dist/tiptap-extension-bold.umd.js": "tiptap-extension",
        "extension-bullet-list/dist/tiptap-extension-bullet-list.umd.js": "tiptap-extension",
        "extension-code/dist/tiptap-extension-code.umd.js": "tiptap-extension",
        "extension-code-block/dist/tiptap-extension-code-block.umd.js": "tiptap-extension",
        "extension-document/dist/tiptap-extension-document.umd.js": "tiptap-extension",
        "extension-dropcursor/dist/tiptap-extension-dropcursor.umd.js": "tiptap-extension",
        "extension-gapcursor/dist/tiptap-extension-gapcursor.umd.js": "tiptap-extension",
        "extension-hard-break/dist/tiptap-extension-hard-break.umd.js": "tiptap-extension",
        "extension-heading/dist/tiptap-extension-heading.umd.js": "tiptap-extension",
        "extension-history/dist/tiptap-extension-history.umd.js": "tiptap-extension",
        "extension-horizontal-rule/dist/tiptap-extension-horizontal-rule.umd.js": "tiptap-extension",
        "extension-italic/dist/tiptap-extension-italic.umd.js": "tiptap-extension",
        "extension-list-item/dist/tiptap-extension-list-item.umd.js": "tiptap-extension",
        "extension-ordered-list/dist/tiptap-extension-ordered-list.umd.js": "tiptap-extension",
        "extension-paragraph/dist/tiptap-extension-paragraph.umd.js": "tiptap-extension",
        "extension-strike/dist/tiptap-extension-strike.umd.js": "tiptap-extension",
        "extension-text/dist/tiptap-extension-text.umd.js": "tiptap-extension",
        "starter-kit/dist/tiptap-starter-kit.umd.js": ""
    }
};

var merge = require('merge-stream'); 
var gulp = require('gulp');

function copyScripts(cb) {
    var streams = [];

    for (var prop in deps) {
        console.log("Prepping Scripts for: " + prop);

        //console.log(gulp.dest("scripts/npm/"));

        for (var itemProp in deps[prop]) {
            streams.push(gulp.src("node_modules/" + prop + "/" + itemProp)
                .pipe(gulp.dest("scripts/npm/" + prop + "/" + deps[prop][itemProp])));
        }
    }
    merge(streams);

    cb();
}

exports.copyScripts = copyScripts;

function buildTiptap(cb) {
    var run = require('gulp-run');
    run('npm run build:tiptap');
    cb();
}

exports.buildTiptap = buildTiptap;