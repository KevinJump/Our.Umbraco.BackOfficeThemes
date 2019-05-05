(function () {
    'use strict';

    angular.module('umbraco').run(['assetsService', 'eventsService', 'themeResource',
        function (assetsService, eventsService, themeResource) {

            // fired when a user is authenticated and logged in.
            eventsService.on("app.ready", function (evt, data) {
                themeResource.getCurrentTheme(data.user)
                    .then(function (theme) {
                        loadTheme(theme);
                    });
            });

            // load the css/js for the theme. 
            function loadTheme(themeName) {
                if (themeName !== undefined) {
                    var themeFolder = Umbraco.Sys.ServerVariables.application.applicationPath + 'App_Plugins/BackOfficeThemes/themes/';

                    var theme = themeFolder + themeName;
                    assetsService.load([theme + '/theme.css', theme + '/theme.js'])
                        .then(function () {
                            // loaded 
                        });
                }
            }
        }]);
})();