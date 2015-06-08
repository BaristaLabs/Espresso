/**
 * Created by Administrator on 4/15/2015.
 */
/*
 * Checks every $digest for height changes
 */
ngBaristaFiddle.directive( 'bfHeight', function() {

    return {
        link: function( scope, elem, attrs ) {

            scope.$watch( function() {
                scope[attrs['bfHeight']] = elem.height();
            } );
        }
    }

});
