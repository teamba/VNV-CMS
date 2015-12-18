function add_group(parent_id) {
    var group = { ID: 0, Type:1, Name: "", Code: "", Brief: "" };

    var rows = $('#pg_group').propertygrid('getChanges');
    for (var i = 0; i < rows.length; i++) {
        switch (rows[i].name) {
            case "Group Name":
                group.Name = rows[i].value;
                break;

            case "Group Code":
                group.Code = rows[i].value;
                break;

            case "Brief(Memo)":
                group.Brief = rows[i].value;
                break;

            default:
                break;
        }
    }

    var str = JSON.stringify(group);
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/AddGroup",
        data: "{parentID:" + parent_id + ",strGroupEx:'" + str + "'}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            var result = eval('(' + result.d + ')');

            if (result.flag < 0) {
                // error
                //console.log("add_group: error");
            }
            else {
                var parent_node;
                if (parent_id == 0) parent_node = $("#article_tree").tree("getRoot");
                else parent_node = $('#article_tree').tree('find', parent_id);

                $('#article_tree').tree('append', {
                    parent: parent_node.target,
                    data: {
                        id: result.flag,
                        text: group.Name
                    }
                });

                //$("#txt_column_id").val(result.flag);
                show_group_property("Group ID", result.flag);
            }
        }
    });
}

function show_group_property(name, value) {
    var rows = $('#pg_group').propertygrid('getChanges');
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].name == name) {
            rows[i].value = value;
            break;
        }
    }
}

function show_group(groupID) {
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/GetGroupEx",
        data: "{groupID:" + groupID + "}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            var group = eval('(' + result.d + ')');
            if (group.flag < 0) {
                // error
                //console.log("show_column: error");
            }
            else {/*
                $("#txt_column_name").val(column.items.Name);
                $("#txt_column_code").val(column.items.Code);
                $("#txt_column_id").val(column.items.ID);
                $("#txt_column_brief").val(column.items.Brief);*/
                var rows = $('#pg_group').propertygrid('getChanges');
                for (var i = 0; i < rows.length; i++) {
                    switch (rows[i].name) {
                        case "Group Name":
                            rows[i].value = group.items.Name;
                            break;

                        case "Group Code":
                            rows[i].value = group.items.Code;
                            break;

                        case "Group ID":
                            rows[i].value = group.items.ID;
                            break;

                        case "Brief(Memo)":
                            rows[i].value = group.items.Brief;
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    });
}

function create_article_node(group, groups, parent) {
    $('#article_tree').tree('append', {
        parent: parent.target,
        data: {
            id: group.ID,
            text: group.Name
        }
    });

    var node;
    node = $('#article_tree').tree('find', group.ID);
    
    var i;
    if (node) for (i = 0; i < groups.length; i++) if (groups[i].ParentID == group.ID) create_article_node(groups[i], groups, node);
}

function init_article() {
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

    // init group property
   var row = {
        name: 'Group Name',
        value: '',
        group: 'Basic',
        editor: 'text'
    };
    $('#pg_group').propertygrid('appendRow', row);
    row = {
        name: 'Group Code',
        value: '',
        group: 'Basic',
        editor: 'text'
    };
    $('#pg_group').propertygrid('appendRow', row);
    row = {
        name: 'Group ID',
        value: '',
        group: 'Basic',
        editor: 'text'
    };
    $('#pg_group').propertygrid('appendRow', row);
    row = {
        name: 'Brief(Memo)',
        value: '',
        group: 'Basic',
        editor: 'text'
    };
    $('#pg_group').propertygrid('appendRow', row);
    $('#pg_group').propertygrid({
        showHeader: false
    });

    $('#article_tree').tree({
        onClick: function (node) {
            if (node.id) show_group(node.id);
        }
    });

    $('#btn_add_group').click(function () {
        var node = $('#column_tree').tree('getSelected');
        if (node) {
            var parent = $('#article_tree').tree('getParent', node.target);
            if (parent && parent.id) add_group(parent.id);
            else add_group(0);
        }
        else {
            add_group(0);
        }
    });

    /*
    $('#btn_modify_column').click(function () {
        update_column();
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
    */
}