var current_group;
var current_photo;
var current_index;

function show_photo_property() {
    if (current_photo == null) return;
}

function get_photo_property() {
    if (current_photo == null) return;
}

function delete_photo() {
    if (current_photo == null) return;

    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/DeleteResource",
        data: "{ID:" + current_photo.ID + "}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            var res = eval('(' + result.d + ')');

            if (res.flag < 0) {
                // error
                //console.log("delete_photo: error");
            }
            else {
                current_photo = null;
                $('#dg').datagrid('deleteRow', current_index);
                 
            }
        }
    });
}

function add_photo() {
    if (current_group == null) return;

    current_photo = {
        ID: 0,
        GroupID: current_group.ID,
        Type: 2,
        Title: "new photo",
        SubTitle: "",
        Author: "",
        Source: "",
        Status: 1,
        CreateDate: "2015-12-20",
        Content: "",
        UID: "",
        ParentUID: "",
        GroupUID: "",
        Brief: ""
    };

    var content = "";
    get_photo_property();
    if (current_photo.Title == "") {
        alert("Please input the title");
        return;
    }

    var str = JSON.stringify(current_photo);
    //console.log(str);
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/AddResource",
        data: "{groupID:" + current_group.ID + ",strResourceEx:'" + str + "',content:'" + content + "'}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            var res = eval('(' + result.d + ')');

            if (res.flag < 0) {
                // error
                //console.log("update_column: error");
            }
            else {
                current_photo.ID = res.flag;
                $('#dg').datagrid('appendRow', {
                    id: current_photo.ID,
                    title: current_photo.Title,
                    date: current_photo.CreateDate
                });

                var rows = $('#dg').datagrid('getRows');
                current_index = rows.length - 1;
            }
        }
    });
}

function save_photo() {
    if (current_photo == null) return;

    current_photo.Content = "";
    var content = "";
    get_photo_property();
    if (current_photo.Title == "") {
        alert("Please input the title");
        return;
    }

    var str = JSON.stringify(current_photo);
    //console.log(str);
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/UpdateResource",
        data: "{strResourceEx:'" + str + "',content:'" + content + "'}", // {parentID:0}
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

function load_photo(photoID) {
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/GetResourceEx",
        data: "{ID:" + photoID + "}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            var res = eval('(' + result.d + ')');

            if (res.flag < 0) {
                // error
                //console.log("delete_group: error");
            }
            else {
                //alert(res.items.Title);
                
                current_photo = res.items;

                show_photo_property();
            }
        }
    });
}

function delete_group(group_id) {
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/DeleteGroup",
        data: "{groupID:" + group_id + "}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            var result = eval('(' + result.d + ')');

            if (result.flag < 0) {
                // error
                //console.log("delete_group: error");
            }
            else {
                var node = $('#photo_tree').tree('find', group_id);

                $('#photo_tree').tree('remove', node.target);

                show_group_property("Group ID", "");
                show_group_property("Group Name", "");
                show_group_property("Group Code", "");
                show_group_property("Brief", "");
            }
        }
    });
}

function update_group() {
    var group = { ID: 0, Type: 1, Name: "", Code: "", Brief: "" };

    var rows = $('#pg_group').propertygrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        switch (rows[i].name) {
            case "Group Name":
                group.Name = rows[i].value;
                break;

            case "Group Code":
                group.Code = rows[i].value;
                break;

            case "Brief":
                group.Brief = rows[i].value;
                break;

            case "Group ID":
                group.ID = rows[i].value;
                break;

            default:
                break;
        }
    }

    var str = JSON.stringify(group);
    //console.log(str);
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/UpdateGroup",
        data: "{strGroupEx:'" + str + "'}", // {parentID:0}
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

function add_group(parent_id) {
    var group = { ID: 0, Type: 1, Name: "", Code: "", Brief: "" };

    var rows = $('#pg_group').propertygrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        switch (rows[i].name) {
            case "Group Name":
                group.Name = rows[i].value;
                break;

            case "Group Code":
                group.Code = rows[i].value;
                break;

            case "Brief":
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
                if (parent_id == 0) parent_node = $("#photo_tree").tree("getRoot");
                else parent_node = $('#photo_tree').tree('find', parent_id);

                $('#photo_tree').tree('append', {
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
    var rows = $('#pg_group').propertygrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].name == name) {
            rows[i].value = value;
            break;
        }
    }
}

function show_resource_list(groupID) {
    // clear the previos list
    var rows = $('#dg').datagrid('getRows');
    if (rows) {
        var i, n;
        n = rows.length;
        for (i = 0; i < n; i++) $('#dg').datagrid('deleteRow', 0);
    }

    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/GetResources",
        data: "{groupID:" + groupID + "}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            var res = eval('(' + result.d + ')');
            if (res.flag <= 0) {

            }
            else {
                //alert("resource num:"+res.flag);
                for (var i = 0; i < res.items.length; i++) {
                    $('#dg').datagrid('appendRow', {
                        id: res.items[i].ID,
                        title: res.items[i].Title,
                        date: res.items[i].CreateDate
                    });
                }
            }
        }
    });
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
            else {
                current_group = group.items;

                var rows = $('#pg_group').propertygrid('getRows'); // getChanges
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

                        case "Brief":
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

function create_photo_node(group, groups, parent) {
    $('#photo_tree').tree('append', {
        parent: parent.target,
        data: {
            id: group.ID,
            text: group.Name
        }
    });

    var node;
    node = $('#photo_tree').tree('find', group.ID);
    
    var i;
    if (node) for (i = 0; i < groups.length; i++) if (groups[i].ParentID == group.ID) create_photo_node(groups[i], groups, node);
}

function init_photo() {
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/GetGroupAll",
        data: "{Type:2}", // 
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

    $('#photo_tree').tree({
        onClick: function (node) {
            if (node.id) {
                show_group(node.id);
                show_resource_list(node.id);

                // empty photo list

            }
        }
    });

    $('#btn_add_group').click(function () {
        var node = $('#photo_tree').tree('getSelected');
        if (node) {
            var parent = $('#photo_tree').tree('getParent', node.target);
            if (parent && parent.id) add_group(parent.id);
            else add_group(0);
        }
        else {
            add_group(0);
        }
    });


    $('#btn_modify_group').click(function () {
        update_group();
    });


    $('#btn_add_subgroup').click(function () {
        var node = $('#photo_tree').tree('getSelected');
        if (node && node.id) {
            add_group(node.id);
        }
    });

    $('#btn_delete_group').click(function () {
        var node = $('#photo_tree').tree('getSelected');
        if (node && node.id) {
            delete_group(node.id);
        }
    });

    $('#dg').datagrid({
        onClickRow: function (index, row) {
            //alert(row.title);
            load_photo(row.id);
            current_index = index;
        }
    });

    var pager = $('#dg').datagrid().datagrid('getPager');    // get the pager of datagrid
    pager.pagination({
        buttons: [{
            iconCls: 'icon-search',
            handler: function () {
                alert('search');
            }
        }, {
            iconCls: 'icon-add',
            handler: function () {
                //alert('add');
                add_photo();
            }
        }, {
            iconCls: 'icon-save',
            handler: function () {
                //alert('save');
                save_photo();
            }
        }, {
            iconCls: 'icon-remove',
            handler: function () {
                //alert('delete');
                delete_photo();
            }
        }]
    });         

}