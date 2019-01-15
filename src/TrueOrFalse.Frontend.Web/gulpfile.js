/// <binding AfterBuild='copyScripts' />
var deps = {
    "tiptap": {
        "dist/tiptap.js": ""
    },
    "tiptap-extensions": {
        "dist/extensions.js": ""
    },
    "vue": {
        "dist/vue.js": ""
    },
    "vue-nestable": {
        "dist/vue-nestable.js": ""
    },
    "d3": {
        "dist/d3.js": ""
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