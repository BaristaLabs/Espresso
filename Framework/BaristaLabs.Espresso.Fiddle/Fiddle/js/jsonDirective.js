/**
 * Created by Administrator on 4/15/2015.
 */
ngBaristaFiddle.directive("bfJson", [function (){
    return {
        restrict: "EA",
        replace: true,
        scope: {
            json: "=json",
            indent: "=?"
        },
        link: function (scope, element, attrs) {
            scope.formattedJson = "";

            scope.$watch('json', function(value) {
                scope.formattedJson = JSON.stringify(value, null, scope.indent ? scope.indent : 4);
            });
        },
        template: "<pre style='height: 100%; border-radius: 0;'>{{formattedJson}}</pre>"
    }
}]);