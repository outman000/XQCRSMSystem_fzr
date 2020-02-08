
//关闭网页，兼容IE/FireFox/Chrome浏览器，前提是通过window.open或新窗口中打开的网页
function CloseWebPage() {
    try {
        window.opener = null;
        window.open('', '_self', '');
        window.top.close();
    } catch (e) {}
    try {
        window.open('', '_parent', '');
        window.close();
    } catch (e) {}
}

String.prototype.format = function (args) {
    if (arguments.length > 0) {
        var result = this;
        if (arguments.length == 1 && typeof (args) == "object") {
            for (var key in args) {
                var reg = new RegExp("({" + key + "})", "g");
                result = result.replace(reg, args[key]);
            }
        }
        else {
            for (var i = 0; i < arguments.length; i++) {
                if (arguments[i] == undefined) {
                    return "";
                }
                else {
                    var reg = new RegExp("({[" + i + "]})", "g");
                    result = result.replace(reg, arguments[i]);
                }
            }
        }
        return result;
    }
    else {
        return this;
    }
}
//两种调用方式，两个结果都是"我是loogn，今年22了"
//var template1="我是{0}，今年{1}了";
//var template2="我是{name}，今年{age}了";
//var result1=template1.format("loogn",22);
//var result2=template1.format({name:"loogn",age:22});

/**
* 从对象数组中删除属性为objPropery，值为objValue元素的对象
* @param Array arr 数组对象
* @param String objPropery 对象的属性
* @param String objPropery 对象的值
* @return Array 过滤后数组
*/
function remove(arr, objPropery, objValue) {
    return $.grep(arr, function (cur, i) {
        return cur[objPropery] != objValue;
    });
}

/**
* 从对象数组中获取属性为objPropery，值为objValue元素的对象
* @param Array arr 数组对象
* @param String objPropery 对象的属性
* @param String objPropery 对象的值
* @return Array 过滤后的数组
*/
function get(arr, objPropery, objValue) {
    return $.grep(arr, function (cur, i) {
        return cur[objPropery] == objValue;
    });
}

//获取URL地址栏参数
function GetURLRequest() {
    var url = location.search;
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = decodeURIComponent(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}
//调用方式
//var request = GetURLRequest();
//var 参数名称1的值 = request["参数名称1"];
//var 参数名称2的值 = request["参数名称2"];

//数字（float、int、double）相加
function addValue(value1, value2) {
    if (value1 == "") value1 = "0";
    if (value2 == "") value2 = "0";
    var temp1 = 0;
    var temp2 = 0;
    if (value1.indexOf(".") != -1)
        temp1 = value1.length - value1.indexOf(".") - 1;
    if (value2.indexOf(".") != -1)
        temp2 = value2.length - value2.indexOf(".") - 1;

    var temp = 0;

    if (temp1 > temp2)
        temp = (parseFloat(value1) + parseFloat(value2)).toFixed(temp1);
    else
        temp = (parseFloat(value1) + parseFloat(value2)).toFixed(temp2);

    return temp;
}