(function () {
    'use strict';

    angular.module('umbraco').run(['assetsService', 'eventsService', 'backofficeThemeService',
        function (assetsService, eventsService, backofficeThemeService) {

            // fired when a user is authenticated and logged in.
            eventsService.on("app.ready", function (evt, data) {
                backofficeThemeService.getCurrentTheme()
                    .then(function (theme) {


                        console.log(theme.data);

                        if (theme !== '') {
                            loadTheme(theme.data);
                        }
                    });
            });

            // load the css/js for the theme. 
            function loadTheme(themeName) {
                if (themeName !== undefined) {
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