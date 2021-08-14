(function () {
    'use strict';

    function userDashboardController(editorService, backofficeThemeService) {

        var vm = this;
        vm.mode = 'show';

        vm.pickTheme = pickTheme;
        vm.apply = apply;
        vm.reset = reset;

        vm.enabled = Umbraco.Sys.ServerVariables.backOfficeThemes.enabled;

        vm.theme = {
            name:  Umbraco.Sys.ServerVariables.backOfficeThemes.default,
            image: Umbraco.Sys.ServerVariables.backOfficeThemes.themeFolder + '/default/theme.png',
            description: 'The current default theme'
        };

        init();

        ///////////////
        function init() {
            backofficeThemeService.getCurrentThemeInfo()
                .then(function (result) {
                    if (result.data) {
                        vm.theme = result.data;
                    }
                });

            backofficeThemeService.getThemes()
                .then(function (result) {
                    vm.themes = result.data;
                    vm.loading = false;
                });
        }

        function pickTheme() {
            vm.mode = 'pick';
        }


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
        .controller('backofficeThemeUserDashboardController', userDashboardController);
})();