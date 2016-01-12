function save_column_properties() {
    var node = $('#column_tree').tree('getSelected');
    if (node == null || node.id == null) return;

    var rows = $('#dg_column_property').datagrid('getRows');

    var i, counter = 0; count = rows.length;
    var property;
    var properties = new Array();
    for (i = 0; i < count; i++) {
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
            data: "{objectType:1,objectID:" + node.id + ",strProperties:'" + str + "'}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var ret = eval('(' + result.d + ')');

            }
        });
    }
}

function add_column_property() {
    //alert("add_new_property");
    var key = $('#txt_property_name').val();

    if (key == "") {
        alert("please input property name and value");
        return;
    }

    var rows = $('#dg_column_property').propertygrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        if (rows[i].key == key) {
            alert("the key has exist");
            return;
        }
    }

    $('#dg_column_property').datagrid('appendRow', {
        key: key,
        value: ""
    });
}

function clear_column_property() {
    var rows = $('#dg_column_property').datagrid('getRows');
    if (rows) {
        var i, n;
        n = rows.length;
        for (i = 0; i < n; i++) $('#dg_column_property').datagrid('deleteRow', 0);
    }
}

function create_column_node_set(column, columns, parent) {
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
    if (node) for (i = 0; i < columns.length; i++) if (columns[i].ParentID == column.ID) create_column_node_set(columns[i], columns, node);
}

function show_column_run(column) {
	//console.log("Enter:show_column_run");
    if (column.flag < 0) {
        // error
        //console.log("show_column: error--"+column.error);
    }
    else {
        //console.log("column name:" + column.items.Name);
        /*
        $("#txt_column_name").val(column.items.Name);
        $("#txt_column_code").val(column.items.Code);
        $("#txt_column_id").val(column.items.ID);
        $("#txt_column_brief").val(column.items.Brief);

        $('#sel_column_type').val(column.items.ContentType);
        $('#txt_column_template').val(column.items.Template);
        $('#txt_column_seo_description').val(column.items.SEO_Description);
        $('#txt_column_seo_keyword').val(column.items.SEO_Keyword);
        $('#txt_column_seo_title').val(column.items.SEO_Title);
        */
        $('#txt_column_name').textbox('setValue', column.items.Name);
        $('#txt_column_code').textbox('setValue', column.items.Code);
        $('#txt_column_id').textbox('setValue', column.items.ID);
        $('#txt_column_brief').textbox('setValue', column.items.Brief);
        $('#txt_column_seo_description').textbox('setValue', column.items.SEO_Description);
        $('#txt_column_seo_keyword').textbox('setValue', column.items.SEO_Keyword);
        $('#txt_column_seo_title').textbox('setValue', column.items.SEO_Title);

        $('#sel_column_type').combobox('setValue', column.items.ContentType);
    }
}

function show_column_properties(result) {
    if (result.flag < 0) {
        alert(result.error);
    }
    else {
        for (var i = 0; i < result.items.length; i++) {
            $('#dg_column_property').datagrid('appendRow', {
                key: result.items[i].Key,
                value: result.items[i].Value
            });
        }
    }
}

function show_column(columnID) {
    //console.log("Enter:show_column");
    if (g_php) {
        $.post("/column/get", {id:columnID}, function(data) {
            show_column_run(data);  
        });
    }
    else {
         $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/GetColumnEx",
            data: "{columnID:" + columnID + "}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var column = eval('(' + result.d + ')');
                show_column_run(column);
            }
        });

        // show column property
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/GetProperties",
            data: "{objectType:1, objectID:" + columnID + "}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var res = eval('(' + result.d + ')');
                show_column_properties(res);
            }
        });            
    }

}

function update_column() {
    var column = { ID: 0, Name: "", Code: "", Brief: "", ContentType: 0, Template: "", SEO_Description: "", SEO_Keyword: "", SEO_Title: "" };
    /*
    column.ID = $("#txt_column_id").val();
    column.Name = $("#txt_column_name").val();
    column.Code = $("#txt_column_code").val();
    column.Brief = $("#txt_column_brief").val();

    column.ContentType = $('#sel_column_type').val();
    column.SEO_Description = $('#txt_column_seo_description').val();
    column.SEO_Keyword = $('#txt_column_seo_keyword').val();
    column.SEO_Title = $('#txt_column_seo_title').val();
    */
    column.ID = $('#txt_column_id').textbox('getValue');
    column.Name = $('#txt_column_name').textbox('getValue');
    column.Code = $('#txt_column_code').textbox('getValue');
    column.Brief = $('#txt_column_brief').textbox('getValue');
    column.SEO_Description = $('#txt_column_seo_description').textbox('getValue');
    column.SEO_Keyword = $('#txt_column_seo_keyword').textbox('getValue');
    column.SEO_Title = $('#txt_column_seo_title').textbox('getValue');

    column.ContentType = $('#sel_column_type').combobox('getValue');

    var str = JSON.stringify(column);
    //console.log(str);
    if (g_php) {
        $.post("/column/update", {strColumnEx:str}, function(data) {
             
        });
    }
    else {
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

}

function add_column_run(parent_id, result) {
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
                text: result.items.Name
            }
        });

        $("#txt_column_id").val(result.flag);
    }
}

function add_column(parent_id) {
    var column = { ID: 0, Name: "", Code: "", Brief: "", CreateDate: "2015-12-12", ContentType: 0, Template: "", SEO_Description: "", SEO_Keyword: "", SEO_Title: "" };
    /*
    column.Name = $("#txt_column_name").val();
    column.Code = $("#txt_column_code").val();
    column.Brief = $("#txt_column_brief").val();

    column.ContentType = $('#sel_column_type').val();
    column.SEO_Description = $('#txt_column_seo_description').val();
    column.SEO_Keyword = $('#txt_column_seo_keyword').val();
    column.SEO_Title = $('#txt_column_seo_title').val();
    */

    column.Name = $('#txt_column_name').textbox('getValue');
    column.Code = $('#txt_column_code').textbox('getValue');
    column.Brief = $('#txt_column_brief').textbox('getValue');
    column.SEO_Description = $('#txt_column_seo_description').textbox('getValue');
    column.SEO_Keyword = $('#txt_column_seo_keyword').textbox('getValue');
    column.SEO_Title = $('#txt_column_seo_title').textbox('getValue');

    column.ContentType = $('#sel_column_type').combobox('getValue');

    var str = JSON.stringify(column);

    if (g_php) {
        $.post("/column/add", {parentID:parent_id, strColumnEx:str}, function(data) {
             add_column_run(parent_id, data);
        });
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/AddColumn",
            data: "{parentID:" + parent_id + ",strColumnEx:'" + str + "'}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var result = eval('(' + result.d + ')');
                add_column_run(parent_id, result);
            }
        });        
    }

}

function delete_column_run(result) {
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

function delete_column(column_id) {
    if (g_php) {
        $.post("/column/delete", {id:column_id}, function(data) {
             delete_column_run(data);
        });
    }
    else {
        $.ajax({
            type: "POST",
            contentType: "application/json",
            url: "../vnv.asmx/DeleteColumn",
            data: "{columnID:" + column_id + "}", // {parentID:0}
            dataType: 'json',
            success: function (result) {
                var result = eval('(' + result.d + ')');
                delete_column_run(result);
            }
        });        
    }

}

function init_columnSet() {
    // create the column tree
    if (g_php) {
        $.post("/column/get_all", {}, function(data) {
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

    $('#btn_add_property').click(function () { add_column_property(); });
    $('#btn_save_property').click(function () { save_column_property(); });
}