var g_provinces = null;

function create_district_tree() {
    var nodes = $('#district_tree').tree('getRoots');
    if (nodes != null) for (var i = 0; i < nodes.length; i++) $('#district_tree').tree('remove', nodes[i].target);

    if (g_provinces == null) return;

    for (var i = 0; i < g_provinces.length; i++) {
        $('#district_tree').tree('append', {
            parent: null,
            data: {
                id: g_provinces[i].Code,
                text: g_provinces[i].Name
            }
        });
    }
}

function init_dealer() {
    if (g_php) {
        $.post("/column/get_all", {}, function (data) {
            var i;
            var root = $('#column_tree').tree('getRoot');
            for (i = 0; i < data.items.length; i++) {
                if (data.items[i].ParentID == 0) create_column_node_set(data.items[i], data.items, root);
            }
        });
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/GetColumnAll", // GetColumnAll
            data: "", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var columns = eval('(' + result.d + ')');
                var i;
                var root = $('#column_tree').tree('getRoot');
                for (i = 0; i < columns.items.length; i++) {
                    if (columns.items[i].ParentID == 0) create_column_node_set(columns.items[i], columns.items, root);
                }
            }
        });
    }

    create_district_tree();
}