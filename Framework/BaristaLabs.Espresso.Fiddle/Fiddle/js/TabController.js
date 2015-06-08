/**
 * Created by Administrator on 4/8/2015.
 */
ngBaristaFiddle.controller('tabCtrl', ['$scope', '$http', '$timeout',
    function ($scope, $http, $timeout) {
        $scope.model = {
            errorCount: 0,
            isTitleEditing: false
        };

        $scope.editorOptions = {
            lineWrapping: true,
            lineNumbers: true,
            mode: { name: "javascript", json: true },
            onLoad: function(cm) {
                $scope.tab.__editor = cm;

                //Initialize tern
                //TODO: Change this to pull in the current barista state. somehow.
                $http({
                    cache: true,
                    method: "GET",
                    url: "http://ternjs.net/defs/ecma5.json"

                })
                .success(function(code) {
                    $scope.tab.__tern = new CodeMirror.TernServer({
                        defs: [code]
                    });

                    cm.on("cursorActivity", function(cm) {
                        $scope.tab.__tern.updateArgHints(cm);
                    });
                });


                $timeout(function() {
                    cm.refresh();
                    cm.setSize(null, "100%");
                });
            },
            "extraKeys": {
                "Ctrl-Enter": function(cm) {
                    $scope.evaluateScript();
                },
                "Ctrl-Shift-Enter": function(cm) {
                    //$scope.debugScript();
                },
                "Ctrl-S": function(cm) {
                    $scope.saveEditorState()
                },
                "Ctrl-Space": function(cm) {
                    $scope.tab.__tern.complete(cm);
                },
                "Ctrl-K Ctrl-D": function(cm) {
                    $scope.tidyUp($scope.tab);
                },
                "Ctrl-I": function(cm) {
                    $scope.tab.__tern.showType(cm);
                },
                "Ctrl-O": function(cm) {
                    $scope.tab.__tern.showDocs(cm);
                },
                "Alt-.": function(cm) {
                    $scope.tab.__tern.jumpToDef(cm);
                },
                "Alt-,": function(cm) {
                    $scope.tab.__tern.jumpBack(cm);
                },
                "Ctrl-Q": function(cm) {
                    $scope.tab.__tern.rename(cm);
                },
                "Ctrl-.": function(cm) {
                    $scope.tab.__tern.selectName(cm);
                }
            }
            //theme: "rubyblue"
        };

        $scope.startEditTabTitle = function() {
            if ($scope.tab.active) {
                $scope.model.isTitleEditing = true;
            }
        };

        $scope.endEditTabTitle = function() {
            $scope.model.isTitleEditing = false;
            $scope.saveEditorState();
        };

        $scope.saveEditorState = function() {

            //$scope.tab.selection = $scope.tab._editor.getSelection();
        };

        $scope.deselect = function() {
            if ($scope.model.isTitleEditing)
                $scope.model.isTitleEditing = false;

            $scope.saveEditorState();
        };

        $scope.$on("BaristaEditor-Errors", function(e, count) {
            $scope.$apply(function() {
                $scope.model.errorCount = count;
            })
        });

        $scope.$on("BaristaFiddle-Saving", function(e) {
            $scope.saveEditorState();
        });

    }]);