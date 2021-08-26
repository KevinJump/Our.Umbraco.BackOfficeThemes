(function () {
    'use strict';

    angular.module('umbraco').run(['assetsService', 'eventsService', 'backofficeThemeService',
        function (assetsService, eventsService, backofficeThemeService) {

            // fired when a user is authenticated and logged in.
            eventsService.on("app.ready", function (evt, data) {
                backofficeThemeService.getCurrentTheme()
                    .then(function (theme) {
                        loadTheme(theme.data);
                    });
            });

            // load the css/js for the theme. 
            function loadTheme(themeName) {
                if (!_.isUndefined(themeName) && !_.isEmpty(themeName)) {

                    var themeFolder = Umbraco.Sys.ServerVariables.backOfficeThemes.themeFolder;
                    var theme = themeFolder + '/' + themeName;

                    assetsService.load([theme + '/theme.css', theme + '/theme.js'])
                        .then(function () {
                            // loaded
                        });
                }
            }
        }]);
})();