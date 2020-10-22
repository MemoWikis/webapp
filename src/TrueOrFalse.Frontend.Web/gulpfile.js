/// <binding AfterBuild='copyScripts' />
var deps = {
    "vue": {
        "dist/vue.js": ""
    },
    "vue-sortable": {
        "vue-sortable.js": ""
    },
    "vue-textarea-autosize": {
        "dist/vue-textarea-autosize.umd.js": ""
    },
    "vue-select": {
        "dist/vue-select.js": ""
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
    "sortablejs": {
        "Sortable.js": ""
    },
    "vue-slider-component" : {
        "dist/vue-slider-component.umd.js": ""
    },
    "vue-float-action-button": {
        "dist/vue-fab.js": ""
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


function buildTiptap() {
    var run = require('gulp-run');
    return run('npm run build:tiptap').exec;
}

exports.buildTiptap = buildTiptap;