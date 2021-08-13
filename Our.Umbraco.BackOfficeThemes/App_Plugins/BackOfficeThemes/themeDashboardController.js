(function () {
    'use strict';

    function themeDashboardController(userService, notificationsService,
        backofficeThemeService) {

        var vm = this;
        vm.loading = true;

        vm.apply = apply;
        vm.reset = reset;

        vm.buttonState = 'init';

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


        //////////////////////


        function init() {

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