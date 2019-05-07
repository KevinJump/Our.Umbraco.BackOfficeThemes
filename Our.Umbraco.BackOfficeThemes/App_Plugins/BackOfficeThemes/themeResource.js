(function () {
    'use strict';

    function themeResource($q, backofficeThemeService) {

        var resource = {
            getCurrentTheme: getCurrentTheme
        };

        return resource;

        ///////////////

        function getCurrentTheme(user) {

            var defer = $q.defer();

            var userInfo = user;

            backofficeThemeService.getConfig()
                .then(function (result) {
                    defer.resolve(locateTheme(result.data));
                }, function (error) {
                    defer.reject(error);
                });

            return defer.promise; 

            ///////////////

            function locateTheme(config) {
                if (config === undefined) return undefined;

                var theme = checkUsers(config.users);
                if (theme !== undefined) return theme;

                theme = checkGroups(config.groups);
                if (theme !== undefined) return theme;

                return checkServer(config.servers);
            }

            function checkUsers(config) {
                if (config === undefined) return undefined;

                for (var i in config) {
                    if (config[i].userId === userInfo.id) {
                        return config[i].theme;
                    }
                }
            }

            function checkGroups(config) {
                if (config === undefined) return undefined;

                console.log(userInfo);
                for (var i in config) {
                    if (userInfo.userGroups.indexOf(config[i].group) !== -1) {
                        return config[i].theme;
                    }
                }
            }

            function checkServer(config) {
                if (config === undefined) return undefined;

                // Umbraco Enviroment Indicator pattern matching code. 
                // https://github.com/leekelleher/umbraco-environment-indicator
                for (var i in config) {
                    if (new RegExp(config[i].pattern, "i").test(location.host)) {
                        return config[i].theme;
                    }
                }
            }
        }
    }

    angular.module('umbraco.resources')
        .factory('themeResource', themeResource);
})();