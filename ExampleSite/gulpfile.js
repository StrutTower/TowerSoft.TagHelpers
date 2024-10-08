﻿/// <binding ProjectOpened='sass-watch' />
var gulp = require('gulp'),
    concat = require('gulp-concat'),
    sass = require('gulp-dart-sass'),
    cleancss = require('gulp-clean-css'),
    terser = require('gulp-terser'),
    rename = require('gulp-rename'),
    stripComments = require('gulp-strip-comments'),
    merge = require('merge2');

//#region Options
var options = {
    js: {
        libFiles: [
            'bootstrap/dist/js/bootstrap.bundle.js',
        ],
        workingDirectory: 'node_modules',
        libOutput: 'lib.js',
        appFiles: ['wwwroot/js/**/*.js'],
        output: 'bundle.min.js',
        dest: 'wwwroot/lib'
    },
    css: {
        libFiles: [
            '@mdi/font/css/materialdesignicons.css'
        ],
        workingDirectory: 'node_modules',
        sassSource: 'Sass/site.scss',
        sassFiles: 'Sass/**/*.scss',
        output: 'bundle.css',
        dest: 'wwwroot/lib'
    },
    fonts: {
        files: [
            'node_modules/@mdi/font/fonts/*.*'
        ],
        dest: 'wwwroot/fonts'
    }
};
//#endregion

//#region Tasks
gulp.task('bundle-JS', function () {
    return gulp.src(options.js.libFiles, { cwd: options.js.workingDirectory })
        .pipe(concat(options.js.libOutput))
        .pipe(stripComments())
        .pipe(gulp.dest(options.js.dest));
});

gulp.task('bundle-CSS', function () {
    var libCSS = gulp.src(options.css.libFiles, { cwd: options.css.workingDirectory });

    var siteCSS = gulp.src(options.css.sassSource)
        .pipe(sass({
            errLogToConsole: true
        }).on('error', sass.logError));

    return merge(libCSS, siteCSS)
        .pipe(concat(options.css.output))
        .pipe(gulp.dest(options.css.dest))
        .pipe(cleancss({ level: { 1: { specialComments: 0 } } }))
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(options.css.dest));
});

gulp.task('copy-fonts', function () {
    return gulp.src(options.fonts.files)
        .pipe(gulp.dest(options.fonts.dest));
});

gulp.task('sass-watch', function () {
    gulp.watch(options.css.sassFiles, gulp.parallel('bundle-CSS'));
});

gulp.task('default', gulp.parallel('bundle-JS', 'bundle-CSS', 'copy-fonts'));
//#endregion