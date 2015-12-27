var current_update_id = 0;
var current_column_id = 0;

function select_update(update_id) {
    //alert(update_id);
    var column_node = $('#column_tree').tree('find', "c:" + current_column_id);
    if (column_node == null) return;

    while ($('#column_tree').tree('isLeaf', column_node.target) == false) {
        var nodes = $('#column_tree').tree('getChildren', column_node.target);
        $('#column_tree').tree('remove', nodes[0].target);
    }

    current_update_id = update_id;
    if (g_php) {
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/GetUpdateItems",
            data: "{updateID:" + current_update_id + "}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var res = eval('(' + result.d + ')');
                for (var i = 0; i < res.items.length; i++) {
                    $('#column_tree').tree('append', {
                        parent: column_node.target,
                        data: {
                            id: "r:" + res.items[i].ID,
                            text: res.items[i].Title
                        }
                    });
                }
            }
        });
    }
}

function save_current_column() {
    //alert("save_current_column");
    var node = $('#column_tree').tree('getSelected');
    if (node == null || node.id == null) {
        alert("please select a column!");
        return;
    }

    var items = node.id.split(':');
    if (items[0] != "c") {
        alert("please select a column node");
        return;
    }
    var column_id = items[1];

    var nodes;
    if ($('#column_tree').tree('isLeaf', node.target) == true) {
        alert("no resource in this column");
        return;
    }
    else {
        nodes = $('#column_tree').tree('getChildren', node.target);
        items = nodes[0].id.split(':');
        if (items[0] != "r") {
            alert("it's not the end column");
            return;
        }
    }

    var strUpdateItem = "";
    for (var i = 0; i < nodes.length; i++) {
        items = nodes[i].id.split(':');
        if (strUpdateItem != "") strUpdateItem += "|";
        strUpdateItem += items[1];
    }

    //alert(strUpdateItem);
    if (g_php) {
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/SaveEditUpdate",
            data: "{updateID:" + current_update_id + ", columnID:" + column_id + ", strUpdateItem:'" + strUpdateItem + "'}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var res = eval('(' + result.d + ')');
                save_current_column_run(res);
            }
        });
    }
}

function save_current_column_run(result) {
}

function save_checked_columns() {
}

function clear_current_resource() {
}

function clear_current_column() {
}

function reload_current_column() {
}

function preview_current_column() {
}

function publish_current_column() {
}

function publish_checked_columns() {
}

function add_article_to_column() {
    var node = $('#column_tree').tree('getSelected');
    if (node == null || node.id == null) {
        alert("please select a column!");
        return;
    }

    var items = node.id.split(':');
    if (items[0] != "c") {
        alert("please select a column node");
        return;
    }

    var ok = 0;
    if ($('#column_tree').tree('isLeaf', node.target) == true) {
        ok = 1;
    }
    else {
        var nodes = $('#column_tree').tree('getChildren', node.target);
        items = nodes[0].id.split(':');
        if (items[0] == "r") ok = 1;
    }

    if (ok == 0) {
        alert("please select a leaf column");
        return;
    }

    var nodes_source = $('#article_tree').tree('getChecked');
    if (nodes_source == null) {
        alert("please select articles in the article tree");
        return;
    }

    var nodes_target;
    for (var i = 0; i < nodes_source.length; i++) {
        items = nodes_source[i].id.split(':');
        if (items[0] != "r") continue;

        ok = 1;
        if ($('#column_tree').tree('isLeaf', node.target) == false) {
            nodes_target = $('#column_tree').tree('getChildren', node.target);
            for (var j = 0; j < nodes_target.length; j++) {
                if (nodes_source[i].id == nodes_target[j].id) {
                    ok = 0;
                    break;
                }
            }
        }
        if (ok == 0) continue;

        $('#column_tree').tree('append', {
            parent: node.target,
            data: {
                id: nodes_source[i].id,
                text: nodes_source[i].text
            }
        });
    }
}

function add_photo_to_column() {
    var node = $('#column_tree').tree('getSelected');
    if (node == null || node.id == null) {
        alert("please select a column!");
        return;
    }

    var items = node.id.split(':');
    if (items[0] != "c") {
        alert("please select a column node");
        return;
    }

    var ok = 0;
    if ($('#column_tree').tree('isLeaf', node.target) == true) {
        ok = 1;
    }
    else {
        var nodes = $('#column_tree').tree('getChildren', node.target);
        items = nodes[0].id.split(':');
        if (items[0] == "r") ok = 1;
    }

    if (ok == 0) {
        alert("please select a leaf column");
        return;
    }

    var nodes_source = $('#photo_tree').tree('getChecked');
    if (nodes_source == null) {
        alert("please select photos in the photo tree");
        return;
    }

    var nodes_target;
    for (var i = 0; i < nodes_source.length; i++) {
        items = nodes_source[i].id.split(':');
        if (items[0] != "r") continue;

        ok = 1;
        if ($('#column_tree').tree('isLeaf', node.target) == false) {
            nodes_target = $('#column_tree').tree('getChildren', node.target);
            for (var j = 0; j < nodes_target.length; j++) {
                if (nodes_source[i].id == nodes_target[j].id) {
                    ok = 0;
                    break;
                }
            }
        }
        if (ok == 0) continue;

        $('#column_tree').tree('append', {
            parent: node.target,
            data: {
                id: nodes_source[i].id,
                text: nodes_source[i].text
            }
        });
    }
}

function create_column_node(column, columns, parent) {
    $('#column_tree').tree('append', {
        parent: parent.target,
        data: {
            id: "c:"+column.ID,
            text: column.Name
        }
    });

    //$.parser.parse('#column_tree');

    var node;
    node = $('#column_tree').tree('find', "c:"+column.ID);
    //console.log("new node:" + node.id + "--" + node.text);
    var i;
    if (node) for (i = 0; i < columns.length; i++) if (columns[i].ParentID == column.ID) create_column_node(columns[i], columns, node);
}

function show_update_list(result) {
    if (result.flag < 0) {
        //
        return;
    }

    var rows = $('#dg_update').datagrid('getRows');
    while (rows.length > 0) {
        $('#dg_update').datagrid('deleteRow', 0);
        rows = $('#dg_update').datagrid('getRows');
    }

    var status = "";
    for (var i = 0; i < result.items.length; i++) {
        switch (result.items[i].Status) {
            case 0:
                status = "编辑中";
                break;

            case 1:
                status = "已发布";
                break;

            case 2:
                status = "旧内容";
                break;

            default:
                break;
        }

        $('#dg_update').datagrid('appendRow', {
            id: result.items[i].ID,
            createdate: result.items[i].CreateDate,
            status: status
        });
    }
}

function is_end_column(node) {
    var items = node.id.split(':');
    if (items[0] != "c") return false;

    if ($('#column_tree').tree('isLeaf', node.target)) return true;

    var nodes = $('#column_tree').tree('getChildren', node.target);
    items = nodes[0].id.split(':');
    if (items[0] == "r") return true;

    return false;
}

function proc_column_click(node) {
    if (node.id == null) return;

    var items = node.id.split(':');
    //var ret = $('#column_tree').tree('isLeaf', node.target);
    //if (items[0] == "c" && $('#column_tree').tree('isLeaf', node.target)) {
    if (is_end_column(node)) {
        //alert("is leaf!");
        current_column_id = items[1];

        if (g_php) {
        }
        else {
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "../vnv.asmx/GetUpdates",
                data: "{columnID:" + items[1] + "}", // {parentID:0}
                dataType: 'json',
                success: function (result) {
                    var res = eval('(' + result.d + ')');
                    show_update_list(res);
                }
            });
        }
    }
}

function proc_article_click(node) {
    if (node.id == null) return;

    var items = node.id.split(':');
    if (items[0] == "g" && $('#article_tree').tree('isLeaf', node.target)) {
        if (g_php) {
            $.post("/resource/get_list", { groupid: items[1] }, function (data) {
                //var res = eval('(' + data + ')');
                show_article_list(node, data);
            });
        }
        else {
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "../vnv.asmx/GetResources",
                data: "{groupID:" + items[1] + "}", // {parentID:0}
                dataType: 'json',
                success: function (result) {
                    var res = eval('(' + result.d + ')');
                    show_article_list(node, res);
                }
            });
        }
    }
}

function show_article_list(node,res) {
    if (res.flag <= 0) {

    }
    else {
        for (var i = 0; i < res.items.length; i++) {
            $('#article_tree').tree('append', {
                parent: node.target,
                data: {
                    id: "r:" + res.items[i].ID,
                    text: res.items[i].Title
                }
            });
        }
    }
}

function show_photo_list(node, res) {
    if (res.flag <= 0) {

    }
    else {
        for (var i = 0; i < res.items.length; i++) {
            $('#photo_tree').tree('append', {
                parent: node.target,
                data: {
                    id: "r:" + res.items[i].ID,
                    text: res.items[i].Title
                }
            });
        }
    }
}

function proc_photo_click(node) {
    var items = node.id.split(':');
    if (items[0] == "g" && $('#photo_tree').tree('isLeaf', node.target)) {
        if (g_php) {
            $.post("/resource/get_list", { groupid: items[1] }, function (data) {
                //var res = eval('(' + data + ')');
                show_photo_list(node, data);
            });
        }
        else {
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "../vnv.asmx/GetResources",
                data: "{groupID:" + items[1] + "}", // {parentID:0}
                dataType: 'json',
                success: function (result) {
                    var res = eval('(' + result.d + ')');
                    show_photo_list(node, res);
                }
            });
        }
    }
}

function init_column() {
    // create the column tree
    if (g_php) {
        $.post("/column/get_all", {}, function(data) {
            var i;
            var root = $('#column_tree').tree('getRoot');
            for (i = 0; i < data.items.length; i++) {
                if (data.items[i].ParentID == 0) create_column_node(data.items[i], data.items, root);
            }       
        });
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/GetColumnAll",
            data: "", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var columns = eval('(' + result.d + ')');
                var i;
                var root = $('#column_tree').tree('getRoot');
                for (i = 0; i < columns.items.length; i++) {
                    if (columns.items[i].ParentID == 0) create_column_node(columns.items[i], columns.items, root);
                }
            }
        });
    }

    create_article_tree();
    create_photo_tree();

    //init_page();

    $('#column_tree').tree({
        onClick: function (node) {
            proc_column_click(node);
        }
    });

    $('#article_tree').tree({
        onClick: function (node) {
            proc_article_click(node);
        }
    });

    $('#photo_tree').tree({
        onClick: function (node) {
            proc_photo_click(node);
        }
    });

    $('#column_tree').tree({
        onBeforeDrag:function(node) {return false;}
    });

    $('#article_tree').tree({
        onBeforeDrop: function (node) { return false; }
    });

    $('#photo_tree').tree({
        onBeforeDrop: function (node) { return false; }
    });

    $('#dg_update').datagrid({
        onSelect: function (index, row) {select_update(row.id); }
    });

    $('#btn_modify_column').click(function () {
        //console.log("btn_modify_column click");
        update_column();
    });

    $('#btn_add_column').click(function () {
        var node = $('#column_tree').tree('getSelected');
        if (node) {
            var parent = $('#column_tree').tree('getParent', node.target);
            if (parent && parent.id) add_column(parent.id);
            else add_column(0);
        }
        else {
            add_column(0);
        }
    });

    $('#btn_add_subcolumn').click(function () {
        var node = $('#column_tree').tree('getSelected');
        if (node && node.id) {
            add_column(node.id);
        }
    });

    $('#btn_delete_column').click(function () {
        var node = $('#column_tree').tree('getSelected');
        if (node && node.id) {
            delete_column(node.id);
        }
    });
}

/* ---------- function for group tree ---------*/
function create_article_node(group, groups, parent) {
    $('#article_tree').tree('append', {
        parent: parent.target,
        data: {
            id: "g:" + group.ID,
            text: group.Name
        }
    });

    var node;
    node = $('#article_tree').tree('find', "g:" + group.ID);

    var i;
    if (node) for (i = 0; i < groups.length; i++) if (groups[i].ParentID == group.ID) create_article_node(groups[i], groups, node);
}

function create_photo_node(group, groups, parent) {
    $('#photo_tree').tree('append', {
        parent: parent.target,
        data: {
            id: "g:" + group.ID,
            text: group.Name
        }
    });

    var node;
    node = $('#photo_tree').tree('find', "g:" + group.ID);

    var i;
    if (node) for (i = 0; i < groups.length; i++) if (groups[i].ParentID == group.ID) create_photo_node(groups[i], groups, node);
}

function create_article_tree() {
    if (g_php) {
        $.post("/group/get_all", { Type: 1 }, function (data) {
            //alert(data);
            var groups = data; //eval('(' + data + ')');
            var i;
            var root = $('#article_tree').tree('getRoot');
            for (i = 0; i < groups.items.length; i++) {
                if (groups.items[i].ParentID == 0) create_article_node(groups.items[i], groups.items, root);
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/GetGroupAll",
            data: "{Type:1}", // 
            dataType: 'json',
            success: function (result) {
                var groups = eval('(' + result.d + ')');

                var i;
                var root = $('#article_tree').tree('getRoot');
                for (i = 0; i < groups.items.length; i++) {
                    if (groups.items[i].ParentID == 0) create_article_node(groups.items[i], groups.items, root);
                }

            }
        });
    }
}

function create_photo_tree() {
    if (g_php) {
        $.post("/group/get_all", { Type: 1 }, function (data) {
            //alert(data);
            var groups = data; //eval('(' + data + ')');
            var i;
            var root = $('#photo_tree').tree('getRoot');
            for (i = 0; i < groups.items.length; i++) {
                if (groups.items[i].ParentID == 0) create_photo_node(groups.items[i], groups.items, root);
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/GetGroupAll",
            data: "{Type:1}", // 
            dataType: 'json',
            success: function (result) {
                var groups = eval('(' + result.d + ')');

                var i;
                var root = $('#photo_tree').tree('getRoot');
                for (i = 0; i < groups.items.length; i++) {
                    if (groups.items[i].ParentID == 0) create_photo_node(groups.items[i], groups.items, root);
                }

            }
        });
    }
}

/* ------------------------------------------- */
