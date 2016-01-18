/// <binding Clean='clean' ProjectOpened='watch' />
"use strict";

var gulp = require("gulp"),
    del = require("del"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify"),
    rename = require("gulp-rename"),
    sass = require("gulp-sass");

var paths = {
    webroot: "./wwwroot/"
};

paths.js = paths.webroot + "js/**/*.js";
paths.jsDest = paths.webroot + "js";
paths.concatJsDest = paths.webroot + "js/site.js";
paths.minJsDest = paths.webroot + "js/site.min.js";
paths.css = paths.webroot + "css/**/*.scss";
paths.cssDest = paths.webroot + "css";
paths.concatCssDest = paths.webroot + "css/site.css";
paths.minCssDest = paths.webroot + "css/site.min.css";

gulp.task("clean:js", function (cb) {
    del([paths.concatJsDest, paths.minJsDest]);
});

gulp.task("clean:css", function (cb) {
    del([paths.concatCssDest, paths.minCssDest]);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("concat:js", function () {
    return gulp.src([paths.js, "!" + paths.minJsDest, "!" + paths.concatJsDest])
        .pipe(concat("site.js"))
        .pipe(gulp.dest(paths.jsDest));
});

gulp.task("concat:css", function () {
    return gulp.src([paths.css, "!" + paths.minCssDest, "!" + paths.concatCssDest])
        .pipe(sass())
        .pipe(concat("site.css"))
        .pipe(gulp.dest(paths.cssDest));
});

gulp.task("concat", ["concat:js", "concat:css"]);

gulp.task("min:js", ["concat:js"], function () {
    return gulp.src(paths.concatJsDest)
        .pipe(uglify())
        .pipe(rename("site.min.js"))
        .pipe(gulp.dest(paths.jsDest));
});

gulp.task("min:css", ["concat:css"], function () {
    return gulp.src(paths.concatCssDest)
        .pipe(cssmin())
        .pipe(rename("site.min.css"))
        .pipe(gulp.dest(paths.cssDest));
});

gulp.task("min", ["min:js", "min:css"]);

gulp.task("watch", ["build"], function () {
    gulp.watch(paths.js, ["min:js"]);
    gulp.watch(paths.css, ["min:css"]);
});

gulp.task("build", ["clean", "min"]);

gulp.task("default", ["build"]);