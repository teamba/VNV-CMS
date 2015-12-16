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

function load_content() {
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
            for (i = 0; i < columns.length; i++) {
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

                if (columns[i].ParentID == 0) create_column_node(columns[i], columns, root);
            }
        }
    });

    init_page();

    $('#column_tree').tree({
        onClick: function (node) {
            console.log(node.id + "-" + node.text);
        }
    });
}

$(document).ready(function () {
    load_content();
});
