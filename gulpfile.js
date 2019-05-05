/// <binding ProjectOpened='default' />
const { watch, src, dest } = require('gulp');

const sourceFolder = 'Our.Umbraco.BackOfficeThemes/App_Plugins/';

const source = sourceFolder + '**/*';
const destination = 'UmbracoTheme.Site/App_Plugins/';


function copy(path) {

    return src(path, { base: sourceFolder })
        .pipe(dest(destination));
}

function time() {
    return '[' + new Date().toISOString().slice(11, -5) + ']';
}

exports.default = function () {
    watch(source, { ignoreInitial: false })
        .on('change', function (path, stats) {
            console.log(time(), path, 'changed');
            copy(path);
        })
        .on('add', function (path, stats) {
            console.log(time(), path, 'added');
            copy(path);
        });
};
    

