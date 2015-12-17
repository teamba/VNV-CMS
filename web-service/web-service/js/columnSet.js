function create_column_node(column, columns, parent) {
    $('#column_tree').tree('append', {
        parent: parent.target,
        data: {
            id: column.ID,
            text: column.Name
        }
    });

    //$.parser.parse('#column_tree');

    var node;
    node = $('#column_tree').tree('find', column.ID);
    //console.log("new node:" + node.id + "--" + node.text);
    var i;
    if (node) for (i = 0; i < columns.length; i++) if (columns[i].ParentID == column.ID) create_column_node(columns[i], columns, node);
}

function show_column(columnID) {
    //console.log("Enter:show_column");
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/GetColumnEx",
        data: "{columnID:" + columnID + "}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            //alert(result.d);
            //console.log(result.d);
            var column = eval('(' + result.d + ')');
            //console.log(columns.length);
            if (column.flag < 0) {
                // error
                //console.log("show_column: error");
            }
            else {
                //console.log("column name:" + column.items.Name);
                $("#txt_column_name").val(column.items.Name);
                $("#txt_column_code").val(column.items.Code);
                $("#txt_column_id").val(column.items.ID);
                $("#txt_column_brief").val(column.items.Brief);
            }
        }
    });
}

function update_column() {
    var column = { ID: 0, Name: "", Code: "", Brief: "" };
    column.ID = $("#txt_column_id").val();
    //console.log("update_column:" + column.ID);
    column.Name = $("#txt_column_name").val();
    column.Code = $("#txt_column_code").val();
    column.Brief = $("#txt_column_brief").val();

    var str = JSON.stringify(column);
    //console.log(str);
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/UpdateColumn",
        data: "{strColumnEx:'" + str + "'}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            var result = eval('(' + result.d + ')');

            if (result.flag < 0) {
                // error
                //console.log("update_column: error");
            }
            else {

            }
        }
    });
}

function add_column(parent_id) {
    var column = { ID: 0, Name: "", Code: "", Brief: "" };

    column.Name = $("#txt_column_name").val();
    column.Code = $("#txt_column_code").val();
    column.Brief = $("#txt_column_brief").val();

    var str = JSON.stringify(column);

    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/AddColumn",
        data: "{parentID:" + parent_id + ",strColumnEx:'" + str + "'}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            var result = eval('(' + result.d + ')');

            if (result.flag < 0) {
                // error
                //console.log("add_column: error");
            }
            else {
                var parent_node;
                if (parent_id == 0) parent_node = $("#column_tree").tree("getRoot");
                else parent_node = $('#column_tree').tree('find', parent_id);

                $('#column_tree').tree('append', {
                    parent: parent_node.target,
                    data: {
                        id: result.flag,
                        text: column.Name
                    }
                });

                $("#txt_column_id").val(result.flag);
            }
        }
    });
}

function delete_column(column_id) {
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/DeleteColumn",
        data: "{columnID:" + column_id + "}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            var result = eval('(' + result.d + ')');

            if (result.flag < 0) {
                // error
                //console.log("delete_column: error");
            }
            else {
                var node = $('#column_tree').tree('find', column_id);

                $('#column_tree').tree('remove', node.target);

                $("#txt_column_name").val("");
                $("#txt_column_id").val("");
                $("#txt_column_code").val("");
                $("#txt_column_brief").val("");
            }
        }
    });
}

function init_columnSet() {
    // create the column tree
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/GetColumnAll",
        data: "", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            //alert(result.d);
            //console.log(result.d);
            var columns = eval('(' + result.d + ')');
            //console.log(columns.length);
            var i;
            var root = $('#column_tree').tree('getRoot');
            for (i = 0; i < columns.items.length; i++) {
                /*
                console.log(columns[i].ID + '-' + columns[i].Name);
                $('#column_tree').tree('append', {
                parent: root.target,
                data: {
                id: columns[i].ID,
                text: columns[i].Name
                }
                });
                */

                if (columns.items[i].ParentID == 0) create_column_node(columns.items[i], columns.items, root);
            }
        }
    });

    //init_page();

    $('#column_tree').tree({
        onClick: function (node) {
            //console.log("onClick node:" + node.id + "-" + node.text);
            if (node.id) show_column(node.id);
        }
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