(function () {
    'use strict';

    function themeDashboardController(userService, notificationsService,
        backofficeThemeService, themeResource) {

        var vm = this;
        vm.loading = true;

        vm.apply = apply;
        vm.reset = reset;
        vm.saveConfig = saveConfig;

        vm.isAdmin = false; 
        vm.viewAdmin = false;

        vm.buttonState = 'init';

        vm.config = {};

        init();

        //////////////////////

        function apply(alias) {
            backofficeThemeService.applyTheme(alias)
                .then(function (result) {
                    location.reload();
                });
        }

        function reset() {
            backofficeThemeService.resetTheme()
                .then(function (result) {
                    location.reload();
                });
        }

        function saveConfig() {
            backofficeThemeService.saveConfig(vm.config)
                .then(function (result) {
                    notificationsService.success('Saved', 'Admin settings saved');
                }, function (error) {
                        notificationsService.error('error', error.data.ExceptionMessage);
                });
        }


        //////////////////////


        function init() {

            userService.getCurrentUser()
                .then(function (user) {
                    vm.isAdmin = user.userGroups.includes('admin');

                    themeResource.getCurrentTheme(user)
                        .then(function (data) {
                            vm.current = data;
                        });
                });

            backofficeThemeService.getConfig()
                .then(function (result) {
                    vm.config = result.data;
                });

            backofficeThemeService.getThemes()
                .then(function (result) {
                    vm.themes = result.data;
                    vm.loading = false;
                });
        }

    }

    angular.module('umbraco')
        .controller('backofficeThemeDashboardController', themeDashboardController);
})();