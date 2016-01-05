var current_update_id = 0;
var current_column_id = 0;
var current_resource_id = 0;

function view_update_item_run(res) {
    if (res.flag < 0) {
        alert(res.error);
        return;
    }

    switch (res.items.Type) {
        case 1:
            $('#div_update_item_content').html(res.items.Content);
            break;

        case 2:
            break;

        default:
            break;
    }

    $('#div_update_item_content').show();
}

function view_update_item() {
    if (current_resource_id <= 0) return;

    $('#div_view_update_item').hide();

    if (g_php) {
        $.post("/resource/get_ex", { id: current_resource_id }, function (data) {
            view_update_item_run(data);
        });
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/GetResourceEx",
            data: "{ID:" + current_resource_id + "}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var res = eval('(' + result.d + ')');
                view_update_item_run(res);
            }
        });
    }
}

function view_resource_run(res) {
    if (res.flag < 0) {
        alert(res.error);
        return;
    }

    switch (res.items.Type) {
        case 1:
            $('#div_resource_content').html(res.items.Content);
            break;

        case 2:
            break;

        default:
            break;
    }

    $('#div_resource_content').show();
}

function view_resource() {
    if (current_resource_id2 <= 0) return;

    $('#div_view_resource').hide();

    if (g_php) {
        $.post("/resource/get_ex", { id: current_resource_id2 }, function (data) {
            view_resource_run(data);
        });
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/GetResourceEx",
            data: "{ID:" + current_resource_id2 + "}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var res = eval('(' + result.d + ')');
                view_resource_run(res);
            }
        });
    }
}

function select_update(update_id) {
    //alert(update_id);
    var column_node = $('#column_tree').tree('find', "c:" + current_column_id);
    if (column_node == null) return;

    while ($('#column_tree').tree('isLeaf', column_node.target) == false) {
        var nodes = $('#column_tree').tree('getChildren', column_node.target);
        $('#column_tree').tree('remove', nodes[0].target);
    }

    // clear dg_update_item_sort
    var rows = $('#dg_update_item_sort').datagrid('getRows');
    if (rows) {
        var i, n;
        n = rows.length;
        for (i = 0; i < n; i++) $('#dg_update_item_sort').datagrid('deleteRow', 0);
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
                            id: "r:" + res.items[i].ResourceID,
                            text: res.items[i].Title
                        }
                    });

                    // add item into dg_update_item_sort
                    $('#dg_update_item_sort').datagrid('appendRow', {
                        id: res.items[i].ResourceID,
                        listpoint: res.items[i].ListPoint,
                        title: res.items[i].Title
                    });
                }
            }
        });
    }

    // get the property of current update and display
    $('#div_new_property').show();
    clear_update_property();
    if (g_php) {
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/GetProperties",
            data: "{objectType:4, objectID:" + current_update_id + "}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var res = eval('(' + result.d + ')');
                show_properties(res);
            }
        });
    }
}

function show_properties(result) {
    if (result.flag < 0) {
        alert(result.error);
    }
    else {
        for(var i=0; i<result.items.length; i++ ) {
            $('#dg_update_item_property').datagrid('appendRow',{
	            key: result.items[i].Key,
	            value: result.items[i].Value
            });
        }
    }
}

function clear_update_property() {
    var rows = $('#dg_update_item_property').datagrid('getRows');
    if (rows) {
        var i, n;
        n = rows.length;
        for (i = 0; i < n; i++) $('#dg_update_item_property').datagrid('deleteRow', 0);
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
    //alert("save_current_column_run");
    current_update_id = result.items.ID;
}

function save_checked_columns() {
    alert("save_checked_columns");
}

function clear_current_resource() {
    //alert("clear_current_resource");

    var node = $('#column_tree').tree('getSelected');
    if (node == null || node.id == null) {
        alert("please select a node in the column tree!");
        return;
    }

    var items = node.id.split(':');
    if (items[0] != 'r') {
        alert("Please select the resource node!");
        return;
    }

    if (g_php) {
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/DeleteUpdateItem",
            data: "{ID:"  + items[1] + "}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var res = eval('(' + result.d + ')');
                if (res.flag < 0) alert(res.error);
                else $('#column_tree').tree('remove', node.target);
            }
        });
    }
}

function clear_current_column_run(res) {
    if (res.flag < 0) {
        alert(res.error);
    }
    else {
        clear_children_nodes(current_column_id);
    }
}

function clear_current_column() {
    //alert("clear_current_column");
    if (current_update_id <= 0) return;

    if (g_php) {
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/DeleteUpdateItems",
            data: "{updateID:" + current_update_id + "}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var res = eval('(' + result.d + ')');
                clear_current_column_run(res);
            }
        });
    }
}

function reload_current_column() {
    alert("reload_current_column");
}

function save_items_properties() {
    var node = $('#column_tree').tree('getSelected');
    if (node == null || node.id == null) return;

    var items = node.id.split(':');
    if (items[0] == 'r') {
        save_items_properties_run(5, items[1]);
    }
    else {
        if (is_end_column(node)) {
            if (current_update_id > 0) save_items_properties_run(4, current_update_id);
        }
    }
}

function save_items_properties_run(type, id) {
    var rows = $('#dg_update_item_property').datagrid('getRows');

    var i, counter=0; count=rows.length;
    var property;
    var properties = new Array();
    for (i=0; i<count; i++) {
        if (rows[i].value == "") continue;

        property = new Object();
        property.ID = 0;
        property.ObjectType = type;
        property.ObjectID = id;
        property.Key = rows[i].key;
        property.Value = rows[i].value;

        properties[counter] = property;
        counter += 1;
    }

    var str = JSON.stringify(properties);

    //alert(str);
    if (g_php) {
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/SaveProperties",
            data: "{objectType:" + type + ",objectID:" + id + ",strProperties:'" + str + "'}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var ret = eval('(' + result.d + ')');
                
            }
        });
    }
}

function preview_current_column() {
    alert("preview_current_column");
}

function publish_current_column_run(res) {
    if (res.flag<0) {
        alert(res.error);
    }
    else {
        if (g_php) {
        }
        else {
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "../vnv.asmx/GetUpdates",
                data: "{columnID:" + current_column_id + "}", // {parentID:0}
                dataType: 'json',
                success: function (result) {
                    var ret = eval('(' + result.d + ')');
                    show_update_list(ret);
                }
            });
        }
    }
}

function publish_current_column() {
    //alert("publish_current_column");
    if (current_column_id <= 0) return;

    if (g_php) {
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/PublishColumn",
            data: "{columnID:" + current_column_id + "}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var res = eval('(' + result.d + ')');
                publish_current_column_run(res);
            }
        });
    }
}

function publish_checked_columns() {
    alert("publish_checked_columns");
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

function create_column_node_parent(columns, parent) {
    if (parent == null) parent = $('#column_tree').tree('getRoot');
    if (parent == null) return;

    for (var i = 0; i < columns.length; i++) {
        $('#column_tree').tree('append', {
            parent: parent.target,
            data: {
                id: "c:" + columns[i].ID,
                text: columns[i].Name
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
            status: status,
            createdate: result.items[i].CreateDate
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

function clear_children_nodes(column_id) {
    var node = $('#column_tree').tree('find', "c:" + column_id);
    if (node == null) return;

    var nodes = $('#column_tree').tree('getChildren', node.target);
    while (nodes != null && nodes.length>0) {
        $('#column_tree').tree('remove', nodes[0].target);
        nodes = $('#column_tree').tree('getChildren', node.target);
    }
}

function proc_column_click(node) {
    $('#div_view_update_item').hide();
    $('#div_update_item_content').hide();
    $('#div_new_property').hide();
    clear_update_property();

    if (node.id == null) return;

    var items = node.id.split(':');
    //var ret = $('#column_tree').tree('isLeaf', node.target);
    //if (items[0] == "c" && $('#column_tree').tree('isLeaf', node.target)) {
    //current_column_id = 0;
    //current_update_id = 0;

    if (items[0] == "r") {
        $('#div_view_update_item').show();
        current_resource_id = items[1];

        //todo: get the property of current update item and display
        $('#div_new_property').show();
        if (g_php) {
        }
        else {
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "../vnv.asmx/GetProperties",
                data: "{objectType:5, objectID:" + current_resource_id + "}", // {parentID:0}
                dataType: 'json',
                success: function (result) {
                    var res = eval('(' + result.d + ')');
                    show_properties(res);
                }
            });
        }

        return;
    }

    if (is_end_column(node)) {
        //alert("is leaf!");
        current_column_id = items[1];

        if (g_php) {
        }
        else {
            //check the children columns
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "../vnv.asmx/GetColumns",
                data: "{parentID:" + items[1] + "}", // {parentID:0}
                dataType: 'json',
                success: function (result) {
                    var columns = eval('(' + result.d + ')');
                    create_column_node_parent(columns.items, node);
                }
            });

            if (is_end_column(node) == false) return;

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

var current_resource_id2 = 0;

function proc_article_click(node) {
    $('#div_view_resource').hide();
    $('#div_resource_content').hide();

    if (node.id == null) return;

    var items = node.id.split(':');

    if (items[0] == "r") {
        $('#div_view_resource').show();
        current_resource_id2 = items[1];
    }

    if (items[0] == "g" && $('#article_tree').tree('isLeaf', node.target)) {
        if (g_php) {
            $.post("/resource/get_list", { groupid: items[1] }, function (data) {
                //var res = eval('(' + data + ')');
                show_article_list(node, data);
            });
        }
        else {
            // check the children group
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "../vnv.asmx/GetGroups",
                data: "{Type:1, parentID:" + items[1] + "}", // {parentID:0}
                dataType: 'json',
                success: function (result) {
                    var res = eval('(' + result.d + ')');
                    create_article_node_col_parent(res.items, node);
                }
            });

            if ($('#article_tree').tree('isLeaf', node.target)==false) return;

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
    $('#div_view_resource').hide();
    $('#btn_view_resource').hide();

    if (node.id == null) return;

    var items = node.id.split(':');

    if (items[0] == "r") {
        $('#btn_view_resource').show();
        current_resource_id2 = items[1];
    }

    if (items[0] == "g" && $('#photo_tree').tree('isLeaf', node.target)) {
        if (g_php) {
            $.post("/resource/get_list", { groupid: items[1] }, function (data) {
                //var res = eval('(' + data + ')');
                show_photo_list(node, data);
            });
        }
        else {
            // check the children group
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: "../vnv.asmx/GetGroups",
                data: "{Type:2, parentID:" + items[1] + "}", // {parentID:0}
                dataType: 'json',
                success: function (result) {
                    var res = eval('(' + result.d + ')');
                    create_photo_node_col_parent(res.items, node);
                }
            });

            if ($('#photo_tree').tree('isLeaf', node.target) == false) return;

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

function pro_item_sort(sort, order) {
    if (current_column_id <= 0 || current_update_id <= 0) return;
    if (sort != "listpoint") return;

    clear_children_nodes(current_column_id);

    var node = $('#column_tree').tree('find', "c:" + current_column_id);
    if (node == null) return;

    var rows = $('#dg_update_item_sort').datagrid('getRows');

    for (var i = 0; i < rows.length; i++) {
        $('#column_tree').tree('append', {
            parent: node.target,
            data: {
                id: "r:" + rows[i].id,
                text: rows[i].title
            }
        });
    }
}

function add_new_property() {
    //alert("add_new_property");
    var key  = $('#txt_property_name').val();

    if (key == "") {
        alert("please input property name and value");
        return;
    }

    var rows = $('#dg_update_item_property').propertygrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].key == key) {
            alert("the key has exist");
            return;
        }
    }

    $('#dg_update_item_property').datagrid('appendRow', {
        key: key,
        value: ""
    });
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
            url: "../vnv.asmx/GetColumns",
            data: "{parentID:0}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var columns = eval('(' + result.d + ')');
                create_column_node_parent(columns.items, null);
                /*
                var i;
                var root = $('#column_tree').tree('getRoot');
                for (i = 0; i < columns.items.length; i++) {
                if (columns.items[i].ParentID == 0) create_column_node(columns.items[i], columns.items, root);
                }
                */
            }
        });
    }

    create_article_tree_col();
    create_photo_tree_col();

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

    $('#btn_view_update_item').click(function () { view_update_item(); });
    $('#btn_view_resource').click(function () { view_resource(); });
    $('#btn_add_property').click(function () { add_new_property(); });

    $('#dg_update_item_sort').datagrid().datagrid('enableCellEditing');
    $('#dg_update_item_property').datagrid().datagrid('enableCellEditing');

    $('#dg_update_item_sort').datagrid({
        onSortColumn: function (sort, order) {
            //alert("sort:" + sort + ", order:" + order);
            pro_item_sort(sort, order);
        }
    });
}

/* ---------- function for group tree ---------*/
function create_article_node_col_parent(groups, parent) {
    if (parent == null) parent = $('#article_tree').tree('getRoot');
    if (parent == null) return;

    for (var i = 0; i < groups.length; i++) {
        $('#article_tree').tree('append', {
            parent: parent.target,
            data: {
                id: "g:" + groups[i].ID,
                text: groups[i].Name
            }
        });
    }
}

function create_article_node_col(group, groups, parent) {
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
    if (node) for (i = 0; i < groups.length; i++) if (groups[i].ParentID == group.ID) create_article_node_col(groups[i], groups, node);
}

function create_photo_node_col_parent(groups, parent) {
    if (parent == null) parent = $('#photo_tree').tree('getRoot');
    if (parent == null) return;

    for (var i = 0; i < groups.length; i++) {
        $('#photo_tree').tree('append', {
            parent: parent.target,
            data: {
                id: "g:" + groups[i].ID,
                text: groups[i].Name
            }
        });
    }
}

function create_photo_node_col(group, groups, parent) {
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
    if (node) for (i = 0; i < groups.length; i++) if (groups[i].ParentID == group.ID) create_photo_node_col(groups[i], groups, node);
}

function create_article_tree_col() {
    if (g_php) {
        $.post("/group/get_all", { Type: 1 }, function (data) {
            //alert(data);
            var groups = data; //eval('(' + data + ')');
            var i;
            var root = $('#article_tree').tree('getRoot');
            for (i = 0; i < groups.items.length; i++) {
                if (groups.items[i].ParentID == 0) create_article_node_col(groups.items[i], groups.items, root);
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/GetGroups",
            data: "{Type:1,parentID:0}", // 
            dataType: 'json',
            success: function (result) {
                var groups = eval('(' + result.d + ')');
                create_article_node_col_parent(groups.items, null);
                /*
                var i;
                var root = $('#article_tree').tree('getRoot');
                for (i = 0; i < groups.items.length; i++) {
                if (groups.items[i].ParentID == 0) create_article_node_col(groups.items[i], groups.items, root);
                }
                */
            }
        });
    }
}

function create_photo_tree_col() {
    if (g_php) {
        $.post("/group/get_all", { Type: 1 }, function (data) {
            //alert(data);
            var groups = data; //eval('(' + data + ')');
            var i;
            var root = $('#photo_tree').tree('getRoot');
            for (i = 0; i < groups.items.length; i++) {
                if (groups.items[i].ParentID == 0) create_photo_node_col(groups.items[i], groups.items, root);
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/GetGroups",
            data: "{Type:1,parentID:0}", // 
            dataType: 'json',
            success: function (result) {
                var groups = eval('(' + result.d + ')');
                create_photo_node_col_parent(groups.items, null);
                /*
                var i;
                var root = $('#photo_tree').tree('getRoot');
                for (i = 0; i < groups.items.length; i++) {
                if (groups.items[i].ParentID == 0) create_photo_node_col(groups.items[i], groups.items, root);
                }
                */
            }
        });
    }
}

/* ------------------------------------------- */
