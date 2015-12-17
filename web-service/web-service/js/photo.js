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
        data: "{Type:3}", // 
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
    /*
    $('#column_tree').tree({
        onClick: function (node) {
            if (node.id) show_column(node.id);
        }
    });

    $('#btn_modify_column').click(function () {
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
    */
}