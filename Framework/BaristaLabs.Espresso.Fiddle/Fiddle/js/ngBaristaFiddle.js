/**
 * Created by Oceanswave on 10/7/14.
 */

var ngBaristaFiddle = angular.module("ngBaristaFiddle", [ "ui.bootstrap", "ui.router", "ui.codemirror", "kendo.directives", "LocalStorageModule", "focusOn"])

ngBaristaFiddle.config(['localStorageServiceProvider', function(localStorageServiceProvider){
        localStorageServiceProvider.setPrefix('baristaFiddle');
    }]);

ngBaristaFiddle.controller('MainCtrl', ['$scope', '$http', '$timeout', 'localStorageService', 'focus',
    function($scope, $http, $timeout, localStorageService, focus) {
        $scope.model = {
            tabs: null
        };

        $scope.getWorkspaceHeight = function() {
            return ($(window).height() - $('#fiddle-topnav').height() - $('ul.nav.nav-tabs').height() - 5) + "px";
        };

        $scope.initTabs = function () {
            $scope.model.tabs = [
                {
                    title: "Tab 1",
                    active: true,
                    result: null,
                    code: "var helloWorld = function() {\n\
    return(\"Hello, World!\");\n\
};\n\
\n\
helloWorld();"
                }
            ];
        };

        $scope.addTab = function() {
            if (angular.isArray($scope.model.tabs) == false) {
                $scope.initTabs();
                return;
            }

            var count = 0;
            angular.forEach($scope.model.tabs, function(t){
                var tabTitleRegExp = new RegExp("^Tab (\\d+)$", "i");
                if (tabTitleRegExp.test(t.title))
                    count = parseInt(tabTitleRegExp.exec(t.title)[1])
            });

            $scope.model.tabs.push({
                title: "Tab " + (count + 1),
                code: "",
                active: true
            });

            $scope.saveTabs();
        };

        $scope.getActiveTab = function() {
            return _.find($scope.model.tabs, {active: true});
        };

        $scope.closeTab = function() {
            if (angular.isArray($scope.model.tabs) === false) {
                $scope.initTabs();
                return;
            }

            var activeTab = $scope.getActiveTab();
            activeTab.active = false;
            var ix = _.indexOf($scope.model.tabs, activeTab);
            console.log(ix);
            _.pull($scope.model.tabs, activeTab);
            //$scope.model.tabs = _.without($scope.model.tabs, activeTab);
           // _.last($scope.model.tabs).active = true;

            //$scope.model.tabs = tabs;

            $scope.saveTabs();
            console.log(activeTab);
            return false;
        };

        $scope.saveTabs = function() {
            $scope.$broadcast("BaristaFiddle-Saving");

            var tabsState = [];

            angular.forEach($scope.model.tabs, function(tab) {
                var tabState = _.pick(tab, function(value, key) {
                    return key.charAt(0) != '_';
                });

                tabsState.push(tabState);
            });

            localStorageService.set("tabs", tabsState);
        };

        $scope.evaluateScript = function() {
            if ($scope.model.errorCount > 0)
                return;

            $scope.saveTabs();

            var activeTab = $scope.getActiveTab();

            $scope.result = null;

            $http({
                method: "POST",
                url: "/api",
                headers: {
                    "Content-Type": "text/plain; charset=UTF-8",
                    "X-Espresso-Debug": "true"
                },
                data: activeTab.code
            }).success(function(data) {
                activeTab.result = data;
            }).error(function(data, status, headers, config) {
                activeTab.result = data;
                //TODO: Set a line marker at the error line.
                //if (data.lineNumber)
                //    activeTab._editor.addLineErrorMarker(data.lineNumber - 1, data.message);
            })["finally"](function(data) {
                focus("resultUpdated");
                $scope.saveTabs();
            })
        };

        $scope.tidyUp = function(tab) {
            if (!tab)
                tab = $scope.getActiveTab();

            tab.code = js_beautify(tab.code);
        };

        $scope.tabSelected = function(tab) {
            $scope.$broadcast("Tab-Selected", tab);
        };

        $scope.tabDeselected = function(tab) {
            $scope.$broadcast("Tab-Deselected", tab);
        };

        //Initialization
        $scope.model.tabs = localStorageService.get("tabs");

        if (angular.isArray($scope.model.tabs) == false) {
            $scope.initTabs();
        }
    }]);