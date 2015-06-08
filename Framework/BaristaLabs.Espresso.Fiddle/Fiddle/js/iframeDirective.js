/**
 * Created by Administrator on 4/15/2015.
 */

ngBaristaFiddle.directive("bfIframe", [function (){
    return {
        restrict: "A",
        replace: true,
        scope: {
            html: "=ngHtml",
            callback: "&ngOnload"
        },
        link: function (scope, element, attrs) {
            // hooking up the onload event - calling the callback on load event
            element.on("load", function() { scope.callback() });

            scope.$watch(scope.html, function(value){
                if (value) {
                    if (angular.isFunction(value))
                        element.html(value());
                    else if (angular.isObject(value) || angular.isArray(value))
                        element.html(JSON.stringify(value, null, 4));
                    else
                        element.html(value);
                }
            });
        }
    }
}]);