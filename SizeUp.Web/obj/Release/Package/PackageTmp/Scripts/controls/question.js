(function () {
    sizeup.core.namespace('sizeup.controls');
    sizeup.controls.question = function (opts) {

        var defaults = {
            answers: [],
            clearingButtons: [],
            answerClicked: function (index) { },
            answerCleared: function () { }
        };
        var me = {};
        me.opts = $.extend(true, defaults, opts);
        me.questionContainer = me.opts.questionContainer;
        me.data = {};
        me.data.activeIndex = null;

        var init = function () {
            for (var a in me.opts.answers) {
                var i = me.opts.answers[a];
                i.answer.hide().removeClass('hidden');
                i.question.bind('click', i.index, function (e) {
                    answerClicked(e.data);
                });
            }

            for (var a in me.opts.clearingButtons) {
                var i = me.opts.clearingButtons[a];
                i.click(function () { clearAnswer(); });
            }
        };

       
        var answerClicked = function (index) {
            showAnswer(index);
            me.opts.answerClicked(index);
        };

        var findAnswer = function (index) {
            var obj = null;
            for (var a in me.opts.answers) {
                var i = me.opts.answers[a];
                if (i.index == index) {
                    obj =  i;
                }
            }
            return obj;
        };

        var setAnswer = function (a) {
            me.questionContainer.hide();
            a.answer.show().removeClass('collapsed', 500);
        };

        var showAnswer = function (index) {
            if (me.data.activeIndex != index) {
                hideAnswers();
                var a = findAnswer(index);
                if (a) {
                    setAnswer(a);
                    me.data.activeIndex = index;
                }
            }
        };

        var hideAnswers = function () {
            for (var a in me.opts.answers) {
                var i = me.opts.answers[a];
                i.answer.hide();
                i.answer.addClass('collapsed');
            }
            me.data.activeIndex = null;
        };

        var clearAnswer = function () {
            me.data.activeIndex = null;
            for (var a in me.opts.answers) {
                var i = me.opts.answers[a];
                if (i.answer.is(':visible')) {
                    i.answer.addClass('collapsed', 500, function(){
                        me.questionContainer.fadeIn(500);
                        $(this).hide();
                    });
                }
            }
            me.opts.answerCleared();
            
            
        };

        var publicObj = {
            showAnswer: function (index) {
                showAnswer(index);
            },
            clearAnswer: function () {
                clearAnswer();
            }
        };
        init();
        return publicObj;

    };
})();