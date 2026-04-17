/// <binding ProjectOpened='sass-watch' />
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
        sassSource: 'Sass/site.scss',
        sassFiles: 'Sass/**/*.scss',
        output: 'bundle.css',
        dest: 'wwwroot/lib'
    },
    cssLib: {
        files: [
            'tower-bootstrap-theme/dist/tower-bootstrap-theme.css',
            '@mdi/font/css/materialdesignicons.css'
        ],
        workingDirectory: 'node_modules',
        output: 'bundle-lib.css',
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

gulp.task('bundle-css', () => {
    return gulp.src(options.css.sassSource)
        .pipe(sass({
            errLogToConsole: true
        }).on('error', sass.logError))
        .pipe(concat(options.css.output))
        .pipe(gulp.dest(options.css.dest))
        .pipe(cleancss())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(options.css.dest));
});

gulp.task('bundle-lib-css', () => {
    return gulp.src(options.cssLib.files, { cwd: options.cssLib.workingDirectory })
        //.pipe(sass({
        //    errLogToConsole: true
        //}).on('error', sass.logError))
        .pipe(concat(options.cssLib.output))
        .pipe(gulp.dest(options.cssLib.dest))
        .pipe(cleancss())
        .pipe(rename({ suffix: '.min' }))
        .pipe(gulp.dest(options.cssLib.dest));
});

gulp.task('copy-fonts', function () {
    return gulp.src(options.fonts.files)
        .pipe(gulp.dest(options.fonts.dest));
});

gulp.task('sass-watch', function () {
    gulp.watch(options.css.sassFiles, gulp.parallel('bundle-css'));
});

gulp.task('default', gulp.parallel('bundle-JS', 'bundle-css', 'bundle-lib-css', 'copy-fonts'));
