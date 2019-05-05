(function () {
    'use strict';

    function themeDashboardController(userService, backofficeThemeService, themeResource) {

        var vm = this;
        vm.loading = true;

        vm.apply = apply;
        vm.reset = reset;

        userService.getCurrentUser()
            .then(function (user) {
                themeResource.getCurrentTheme(user)
                    .then(function (data) {
                        vm.current = data;
                    });
            });


        // init
        backofficeThemeService.getThemes()
            .then(function (result) {
                vm.themes = result.data;
                vm.loading = false;
            });

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
    }

    angular.module('umbraco')
        .controller('backofficeThemeDashboardController', themeDashboardController);
})();