/**
* This jQuery plugin displays pagination links inside the selected elements.
*
* @version 1.2
* @param {Object} opts Several options (see README for documentation)
* @return {Object} jQuery Object Api
*/
(function ($) {
    /**
    @class 工具类
    */
    var util = function () {
        return {
            getUrlVars: function () {
                var vars = [], hash;
                var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
                for (var i = 0; i < hashes.length; i++) {
                    hash = hashes[i].split('=');
                    vars.push(hash[0]);
                    vars[hash[0]] = hash[1];
                }
                return vars;
            },
            getUrlVar: function (name) {
                return this.getUrlVars()[name];
            },
            /**
            @return {Int32}
            @description 通过当前分页和显示标签的个数属性，计算开始节点到结束节点之间的位置
            */
            getInterval: function () {
                var neHalf = Math.ceil(opts.numDisplayEntries / 2);
                var np = util.numPages();
                var upperLimit = np - opts.numDisplayEntries;
                var start = currentPageIndex > neHalf ? Math.max(Math.min(currentPageIndex - neHalf, upperLimit), 0) : 0;
                var end = currentPageIndex > neHalf ? Math.min(currentPageIndex + neHalf, np) : Math.min(opts.numDisplayEntries, np);
                return [start, end];
            },
            /**
            @return {Int32}
            @description 获取分页数
            */
            numPages: function () {
                var numPages = Math.ceil(count / opts.pageSize);
                return numPages <= 0 ? 1 : numPages;
            },
            /**
            @static
            @param {String} url URL
            @param {String} name 名称
            @param {String} value 值
            @return {String}
            @description 添加URL参数
            */
            addUrlParam: function (url, name, value) {
                var str = name + "=" + escape(value);
                var strArray = url.split("?");
                var urlSegment = strArray[0];
                var querySegment = (strArray.length > 1 ? strArray[1] : "");
                var strArray2;
                if (querySegment.length > 0) {
                    if (querySegment.indexOf(name) == -1)
                        return (url + "&" + str);
                    strArray2 = querySegment.split("&");
                } else
                    return (url + "?" + str);

                var i = 0;
                var buffer = "";

                while (true) {
                    if (i >= strArray2.length) {
                        buffer += str;

                        return (urlSegment + "?" + buffer);
                    }

                    var str4 = strArray2[i];

                    if (str4.indexOf(name + "=") != 0 && (str4.length != 0))
                        buffer += (str4 + "&");

                    ++i;
                }
            },
            /**
            @static
            @param {String} pagingUrl 分页链接
            @param {Int32} pageIndex 当前页索引
            @param {Array} formDomIds 表单元素标识
            @return {String}
            @description 制作分页链接
            */
            makePagingUrl: function (pagingUrl, pageIndex, pageSize, formDomIds) {
                var newPagingUrl = pagingUrl;
                var map = {};
                map[opts.pageIndexParamName] = pageIndex;
                map[opts.pageSizeParamName] = pageSize;

                for (var i = 0; i < formDomIds.length; ++i) {
                    var domId = formDomIds[i];
                    var dom = document.getElementById(domId);

                    if (dom == null)
                        dom = document.getElementsByName(domId);

                    if (dom != null)
                        map[domId] = $(dom).val();
                }
                for (var name in map)
                    newPagingUrl = this.addUrlParam(newPagingUrl, name, map[name]);
                return newPagingUrl;
            }
        };
    } ();
    /**
    * 分页Link or span 标签的点击事件
    */
    function pageSelected(pageIndex, evt) {
        pageIndex = pageIndex + parseInt(opts.mode);
        //判断是否自动提交
        if (opts.autoSubmit) {
            var curIndex = util.getUrlVar(opts.pageIndexParamName);
            var curPageSize = util.getUrlVar(opts.pageSizeParamName);
            if (curIndex != pageIndex || curPageSize != opts.pageSize) {

                if ($.isFunction(pageIndexChanging)) {
                    pageIndexChanging.call(this, pageIndex, opts);
                } else {
                    var formFirst = $("form:first");
                    if (formFirst.length == 0)
                        type = httpMethod.GET;
                    if (type == httpMethod.POST)
                        formFirst.attr({ "method": type, "action": util.makePagingUrl(opts.linkTo, pageIndex, opts.pageSize, opts.urlEncodedDomIds) }).submit();
                    else if (type == httpMethod.GET)
                        window.location.href = util.makePagingUrl(opts.linkTo, pageIndex, opts.pageSize, opts.urlEncodedDomIds);
                }
            }
        }
        currentPageIndex = pageIndex;
        drawLinks(pageIndex);
        var continuePropagation = opts.callback(pageIndex, opts.pageSize, panel);
        if (!continuePropagation) {
            if (evt != undefined) {
                if (evt.stopPropagation) {
                    evt.stopPropagation();
                } else {
                    evt.cancelBubble = true;
                }
            }
        }
        return continuePropagation;
    }

    /**
    * 设置分页详细显示信息
    * @param {Int} 当前分页
    */
    function pageString(pageIndex) {
        if (!opts.pageDetail) {
            return;
        }
        var pageStr = opts.pageString, maxPage = util.numPages();
        pageStr = pageStr
                         .replace("{currentPageIndex}", pageIndex + 1)
                         .replace("{maxPage}", maxPage)
                         .replace("{pageSize}", opts.pageSize)
                         .replace("{total_count}", count);
        var pageSkip = $(panel).find('.pageString');
        pageSkip.html(pageStr);
    }

    /**
    * 设置每页显示记录条数
    */
    function setPageActiveSize() {
        if (!opts.pageActiveSize)
            return;
        var pageSize = $(panel).find('select').eq(0);
        pageSize.bind("change", function () {
            opts.pageSize = $(this).val();
            //pageSelected(0, event);
            var e = window.event || arguments.callee.caller.arguments[0];
            pageSelected(0, e);
        });
    }

    /**
    * 分页跳转函数
    */
    function setPageSkip(pageIndex) {
        if (!opts.pageSkip) {
            return;
        }
        var maxPage = util.numPages() - 1;
        setSkipString(pageIndex);

        var pageSkip = $(panel).find('input').eq(0);
        pageSkip.bind('focus', function (event) {
            if (event.type === 'focus') {
                var curPageIndex = parseInt(pageIndex + 1, 10);
                $(this).val(curPageIndex).select();
            }
            if (event.type === 'mouseup') {
                return false;
            }
            return false;
        });
        //绑定blur事件
        pageSkip.bind('blur', function (e) {
            var $self = $(this);
            if ($.browser.msie) { // 判断浏览器
                if (event.keyCode === 27) {
                    $self.val(parseInt(opts.currentPageIndex, 10));
                    $self.blur();
                }
                if (event.keyCode === 13) {
                    $self.blur();
                }
                if (event.type === 'blur') {
                    var curPage = parseInt($self.val()) - 1;
                    if (curPage != pageIndex) {
                        if (curPage < 1)
                            curPage = 0;
                        //跳转到指定分页
                        var evt1 = window.event || arguments.callee.caller.arguments[0];
                        pageSelected(curPage <= maxPage ? curPage : maxPage, evt1);
                    } else {
                        setSkipString(pageIndex);
                    }
                }
            }
            else {
                if (e.which === 27) {
                    $self.val(parseInt(opts.currentPageIndex, 10));
                    $self.blur();
                }
                if (e.which === 13) {
                    $self.blur();
                }
                if (e.type === 'blur') {
                    var curPage = parseInt($self.val()) - 1;
                    if (curPage != pageIndex) {
                        if (curPage < 1)
                            curPage = 0;
                        //跳转到指定分页
                        var evt1 = window.event || arguments.callee.caller.arguments[0];
                        pageSelected(curPage <= maxPage ? curPage : maxPage, evt1);
                    } else {
                        setSkipString(pageIndex);
                    }
                }
            }

        });
    }
    /**
    *设置pageString
    */
    function setSkipString(pageIndex) {
        var skipString = opts.skipString, maxPage = util.numPages();
        skipString = skipString
                         .replace("{currentPageIndex}", parseInt(pageIndex) + 1)
                         .replace("{maxPage}", maxPage);
        var pageSkip = $(panel).find('input').eq(0);
        pageSkip.val(skipString);
    }

    /**
    * 绘制分页控件
    */
    function drawLinks(curPageIndex) {
        curPageIndex = curPageIndex - parseInt(opts.mode);
        currentPageIndex = curPageIndex;
        //活动分页缓存
        var objActiveSize;
        if (opts.pageActiveSize) {
            objActiveSize = $(panel).find("select").parent();
        }
        panel.empty();
        var interval = util.getInterval();
        var np = util.numPages();
        var getClickHandler = function (pageIndex) {
            return function (evt) {
                return pageSelected(pageIndex, evt);
            };
        };
        var appendItem = function (pageIndex, appendopts) {
            pageIndex = pageIndex < 0 ? 0 : (pageIndex < np ? pageIndex : np - 1);
            appendopts = $.extend({ text: pageIndex + 1, classes: "" }, appendopts || {});
            var lnk;
            if (pageIndex == curPageIndex) {
                lnk = $("<span class='current'>" + (appendopts.text) + "</span>");
            } else {
                lnk = $("<a>" + (appendopts.text) + "</a>")
                                 .bind("click", getClickHandler(pageIndex))
                                 .attr('href', opts.linkTo.replace(/__id__/, pageIndex));
            }
            if (appendopts.classes) {
                lnk.addClass(appendopts.classes);
            }
            panel.append(lnk);
        };
        //生成Page_stirng 详细信息
        if (opts.pageString) {
            $("<span class='pageString'></span>").appendTo(panel);
        }

        // 生成上一页链接
        if (opts.prevText && (curPageIndex > 0 || opts.prev_show_always)) {
            appendItem(curPageIndex - 1, { text: opts.prevText, classes: "prev" });
        }
        // 生成起始点链接标签
        var i;
        if (interval[0] > 0 && opts.numEdgeEntries > 0) {
            var end = Math.min(opts.numEdgeEntries, interval[0]);
            for (i = 0; i < end; i++) {
                appendItem(i);
            }
            if (opts.numEdgeEntries < interval[0] && opts.ellipseText) {
                $("<span>" + opts.ellipseText + "</span>").appendTo(panel);
            }
        }
        // 生成间隔中的标签按钮
        for (i = interval[0]; i < interval[1]; i++) {
            appendItem(i);
        }
        // 生成节点数标签
        if (interval[1] < np && opts.numEdgeEntries > 0) {
            if (np - opts.numEdgeEntries > interval[1] && opts.ellipseText) {
                $("<span>" + opts.ellipseText + "</span>").appendTo(panel);
            }
            var begin = Math.max(np - opts.numEdgeEntries, interval[1]);
            for (i = begin; i < np; i++) {
                appendItem(i);
            }

        }
        // 生成下一页链接按钮
        if (opts.nextText && (currentPageIndex < np - 1 || opts.nextShowAlways)) {
            appendItem(curPageIndex + 1, { text: opts.nextText, classes: "next" });
        }
        //生成跳转按钮
        if (opts.pageSkip) {
            $("<input type='text' class='pageSkip' ></input>").appendTo(panel);
        }
        //生成 每页分页大小
        if (opts.pageActiveSize) {
            if (objActiveSize != undefined && objActiveSize.length > 0)
                $(objActiveSize).appendTo(panel);
            else {
                var getPagerNvaSize = util.getUrlVar(opts.pageSizeParamName);
                var option;
                var optionValue;
                var item = opts.activeSize.length;
                var curPageSize = opts.pageSize;
                if (getPagerNvaSize == undefined)
                    getPagerNvaSize = curPageSize;
                var isAddCurPageIndex = true;
                for (i = 0; i < item; i++) {
                    optionValue = opts.activeSize[i];
                    option += genActiveSizeOption(getPagerNvaSize, optionValue);
                    if (curPageSize == optionValue)
                        isAddCurPageIndex = false;
                }
                //限制每页最大不要操作100条数据
                var maxPageCount = 100;
                if (getPagerNvaSize != undefined) {
                    if (getPagerNvaSize > maxPageCount || getPagerNvaSize > count) {
                        curPageSize = 100;
                        if (curPageSize > count)
                            curPageSize = count;
                    }
                }

                //添加当前默认项目
                if (isAddCurPageIndex)
                    option = genActiveSizeOption(curPageSize, curPageSize) + option;
                option = "<span style='padding:0;'>每页 <select class='pageSize'>" + option + "</select> 条</span>";
                $(option).appendTo(panel);
            }
        }
        //设置分页控件的水平对齐方式
        if (opts.align) {
            if (opts.align == "center")
                panel.css('text-align', 'center');
            else if (opts.align == "right")
                panel.css('text-align', 'right');
            else
                panel.css('text-align', 'left');
        }

        setPageSkip(curPageIndex);
        pageString(curPageIndex);
        setPageActiveSize();
    }

    /**
    @param {Int} curPageSize 当前url传入的参数
    @param {String} optValue 当前传入的option value值
    @return {String} 活动分页
    @description 生成活动分页的选项
    */
    function genActiveSizeOption(curPageSize, optValue) {
        var option;
        var selected = false;
        if (curPageSize != undefined && curPageSize == optValue) {
            selected = true;
        }
        if (selected)
            option = "<option value='" + optValue + "' selected='selected'>" + optValue + "</option>";
        else
            option = "<option value='" + optValue + "'>" + optValue + "</option>";
        return option;
    }


    //选项
    var opts;
    //当前分页
    var currentPageIndex;
    //分页回调
    var pageIndexChanging;
    //页面提交方式
    var type;
    //分页总记录数
    var count;
    //分页面板
    var panel;

    // 数据传输方法枚举
    var httpMethod = { GET: "get", POST: "post" };
    /**
    *  @namespace 分页插件
    *  @class 分页插件
    *  @exports $.fn.pager as Pager
    *  @constructor
    *  @param {Object} option 分页控制选项
    *
    *  @example 示例1 精简版  
    *  <div id="pager"></div>
    *  $("#pager").pager({
    *      count: parseInt('@Model.TotalCount'),
    *      pageSize: parseInt('@Model.PageSize'),
    *      currentPageIndex: parseInt('@Model.PageIndex')
    *  });

    *  @example 示例2 标准版  
    *   <div id="pager"></div>
    *   $("#pager").pager({
    *       count: parseInt('@Model.TotalCount'),
    *       pageSize: parseInt('@Model.PageSize'),
    *       currentPageIndex: parseInt('@Model.PageIndex'),
    *       pageSkip: true,
    *       pageActiveSize: true,
    *       pageDetail: true,
    *   });
    *  @example 示例3 无刷新更新数据  
    *   var pager = eval("[{'PageSize':10,'TotalCount':0,'PageIndex':0}]")[0];//分页json对象
    *   function genPager() {
    *      //分页
    *      $("#pager").pager({
    *          count: pager.TotalCount,
    *          pageSize: pager.PageSize,
    *          currentPageIndex: pager.PageIndex,
    *          autoSubmit: false,//手工处理分页数据
    *          linkTo: '#',
    *          callback: loadContents
    *      });
    *   }
    *   function loadContents(currentPage, currentSize, panel) {
    *       //这里手工刷新分页区域内容
    *   }
    * @return {Object} jQuery Object Api
    * @version 2013 beta release
    */
    $.fn.pager = function (option) {
        opts = $.extend({}, $.fn.pager.defaults, option || {});
        if (!opts.autoSubmit)
            opts.linkTo = "#";
        pageIndexChanging = opts.pageIndexChanging;
        //获取当前分页
        var optsCurPageIndex = util.getUrlVar(opts.pageIndexParamName);
        currentPageIndex = (optsCurPageIndex != undefined ? parseInt(optsCurPageIndex) : opts.currentPageIndex + opts.mode);
        type = opts.type.toLowerCase();
        //总记录数
        count = opts.count;
        //做一下记录条数的容错
        count = (!count || count < 0) ? 0 : count;
        opts.pageSize = (!opts.pageSize || opts.pageSize < 0) ? 1 : opts.pageSize;
        var curNavPageSize = util.getUrlVar(opts.pageSizeParamName);
        if (opts.autoSubmit)
            if (curNavPageSize != undefined)
                opts.pageSize = curNavPageSize;
        // 全局对象
        panel = $(this);
        //绑定分页样式
        panel.addClass("pagination");
        /**
        *   @description 对外API函数
        *   @namespace 分页插件API函数
        *   @class 分页插件API
        *   @example
        *   var pager=$("#pager").pager({...});
        *   pager.pageSelected(1);
        *   @version 2013 beta release
        */
        var evt1 = window.event || arguments.callee.caller.arguments[0];
        var methods = {
            /**
            *   @description 分页控件初始化
            */

            init: function () {
                //对来自主函数，并进入每个方法中的选择器其中的每个单独的元素都执行代码
                return this.each(function () {
                    //绑定事件
                    this.selectPage = function (pageIndex) { pageSelected(pageIndex); };
                    this.prevPage = function () {
                        if (currentPageIndex > 0) {
                            pageSelected(currentPageIndex - 1);
                            return true;
                        } else {
                            return false;
                        }
                    };
                    this.nextPage = function () {
                        if (currentPageIndex < util.numPages() - 1) {
                            pageSelected(currentPageIndex + 1);
                            return true;
                        } else {
                            return false;
                        }
                    };
                    // 绘制分页控件elements
                    drawLinks(currentPageIndex);

                    // 回调函数
                    opts.callback(currentPageIndex, opts.pageSize, this);
                });
            },
            /**
            *   @param {Int} pageIndex 当前分页
            *   @description 分页Link or span 标签的点击事件
            */
            pageSelected: function (pageIndex) {
                return pageSelected(pageIndex - 1, evt1);
            },
            /**
            *   @param {Int} pageIndex 当前分页
            *   @description 设置分页详细显示信息
            */
            pageString: function (pageIndex) {
                pageString(pageIndex);
            },
            /**
            *   @description 设置每页显示记录条数
            */
            setPageActiveSize: function () {
                setPageActiveSize();
            },
            /**
            *   @param {Int} pageIndex 当前分页
            *   @description 分页跳转函数
            */
            setPageSkip: function (pageIndex) {
                setPageSkip(pageIndex);
            },
            /** 
            *   @param {Int} pageIndex 当前分页            
            *   @description 设置跳转到(setSkipString)显示信息
            */
            setSkipString: function (pageIndex) {
                setSkipString(pageIndex);
            },
            /**
            *   @param {Int} curPageSize 当前url传入的参数
            *   @param {String} optValue 当前传入的option value值
            *   @return {String} 活动分页
            *   @description 生成活动分页的选项
            */
            genActiveSizeOption: function (curPageSize, optValue) {
                return genActiveSizeOption(curPageSize, optValue);
            }
        };
        var method = arguments[0];
        if (methods[method]) {
            method = methods[method];
            arguments = Array.prototype.slice.call(arguments, 1);
        } else if (typeof (method) == 'object' || !method) {
            method = methods.init;
        } else {
            $.error('Method' + method + '方法不存在或者参数没有传入');
            return this;
        }
        // 用apply方法来调用我们的方法并传入参数 
        method.apply(this, arguments);
        //返回对外api
        return methods;
    };
    /**
    * @namespace 分页插件默认配置
    * @class 分页插件默认配置选项
    * @version 2013 beta release
    */
    $.fn.pager.defaults = {
        /**
        @description 分页记录总数
        @default 0
        */
        count: 0,
        /**
        @description 分页大小，每页记录条数
        @default 15
        */
        pageSize: 15,
        /**
        @description 当前页
        @default 0
        */
        currentPageIndex: 0,
        /**
        @description 第一页pageIndex,（0-1）
        @default 0
        */
        mode: 0,
        /**
        @description 分页回调函数相比较于callback，是只会在触发分页时才执行
        主要用来兼容老版本
        @default null
        @return {Object} pageIndexChanging(panel, pageIndex, opts);
        */
        pageIndexChanging: null,
        /**
        @description 链接到
        @default window.location.href
        */
        linkTo: window.location.href,
        /**
        @description 是否启用跳转到
        @default true
        */
        pageSkip: true,
        /**
        @description 是否启用分页详细信息
        @default true
        */
        pageDetail: true,
        /**
        @description 是否启用活动分页大小
        @default true
        */
        pageActiveSize: true,
        /**
        @description 指定的活动分页大小
        @default new Array(10, 15, 20, 45)
        */
        activeSize: new Array(10, 15, 20, 45),
        /**
        @description 是否显示上一页
        @default true
        */
        prev_show_always: true,
        /**
        @description 是否显示下一页
        @default true
        */
        nextShowAlways: true,
        /**
        @default true
        @description 是否自动提交页面[兼容老版分页控件,默认为true]
        (如果设置为false,则分页不会刷新页面，同时页面内容需要手动在回调函数中更新)
        */
        autoSubmit: true,
        /**
        @description 页面提交方式
        @default httpMethod.Post
        */
        type: httpMethod.POST,
        /**
        @description 跳转显示值
        @default '当前页 {currentPageIndex} of {maxPage}'
        */
        skipString: '当前页 {currentPageIndex} of {maxPage}',
        /**
        @description  页面详细信息显示值
        @default '当前页:{currentPageIndex} 总页数:{maxPage} 每页:{pageSize} 总记录数:{total_count}'
        */
        pageString: '总记录数:{total_count}',
        /**
        @description 分页控件水平对齐方式[left,center,right]
        @default center
        */
        align: 'center',
        /**
        @description 上一页显示值
        @default 上一页
        */
        prevText: "上一页",
        /**
        @description 下一页显示值
        @default 下一页
        */
        nextText: "下一页",
        /**
        @description 默认省略显示值
        @default "..."
        */
        ellipseText: "...",
        /**
        @description 当中显示的分页标签个数（中间部分）
        @default 5
        */
        numDisplayEntries: 5,
        /**
        @description 边缘显示的分页标签个数(两边部分)
        @default 2
        */
        numEdgeEntries: 2,
        /**
        @description 附件url参数的元素标识
        @default []
        */
        urlEncodedDomIds: [],
        /**
        @description 当前页参数名称
        @default "pageIndex"
        */
        pageIndexParamName: "pageIndex",
        /**
        @description 当前页大小参数名称
        @default "pageSize"
        */
        pageSizeParamName: "pageSize",
        /**
        @description 回调函数-返回当前分页(currentPageIndex,pageSize,this)
        @default funtion(){return false}
        @return {Object} callback(currentPageIndex, pageSize, this);
        */
        callback: function () { return false; }
    };
})(jQuery);