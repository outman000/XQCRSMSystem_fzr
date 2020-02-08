//单任务节点是否多选 true为多选
var isMulti = true;
//End
function addSelectUser(treeNode) {
    $('<li id="' + treeNode.id + '" userName ="' + treeNode.name + '"></li>')
        .html('<a href="#"><input type="checkbox" name ="selectedUser" nodeid="' + treeNode.id + '" />' + treeNode.name +
        ' <img src="../../Content/themes/base/img/btnicon/icon_g_btn_delete.png" style="height:12px;" onclick="clickTreeNode(' +
        treeNode.id + ')" /></a>【' + treeNode.UnitName + '】')
        .appendTo($("#selectUser"));
}

function addViewUser(treeNode) {
    $('<span id="' + treeNode.id + '_X" userName ="' + treeNode.name + '"></span>')
        .html('<a href="#">' + treeNode.name + '</a>【' + treeNode.UnitName + '】')
        .appendTo($("#viewUser"));

}

function clickTreeNode(treeNode) {
    var treeObj = $.fn.zTree.getZTreeObj("tree_Unit");
    var node = treeObj.getNodeByParam("id", treeNode.id, null);
    treeObj.checkNode(node, false, false);
    setCheckEvent(null, node.id, node);
}


function clearAllSelectedUsers() {
    var seleted = $("#selectUser").find("li img");
    for (var k = 0; k < seleted.length; k++) {
        $(seleted[k]).click();
    }
}

function beforeCheck() {
    if (multi == false && !isMulti) {
        clearAllSelectedUsers();
    }
}

$(document).ready(function () {
    $("#clearSelectedUsers").click(clearAllSelectedUsers);
    $("#deleteSelectedUsers").click(function () {
        var selected = $("input[name='selectedUser']");
        for (var k = 0; k < selected.length; k++) {
            if ($(selected[k]).attr("checked")) {
                $(selected[k]).parent().find("img").click();
            }
        }
    });
});