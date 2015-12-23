var current_group;
var current_article;
var current_index;

function show_article_property() {
    if (current_article == null) return;

    var rows = $('#pg_article').propertygrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        switch (rows[i].name) {
            case "Title":
                rows[i].value = current_article.Title;
                break;

            case "Author":
                rows[i].value = current_article.Author;
                break;

            case "Source":
                rows[i].value = current_article.Source;
                break;

            case "Create Date":
                rows[i].value = current_article.CreateDate;
                break;

            case "Sub Title":
                rows[i].value = current_article.SubTitle;
                break;

            case "key word":
                break;

            case "Description":
                break;

            default:
                break;
        }
    }

    $("#txt_brief").val(current_article.Brief);
}

function get_article_property() {
    if (current_article == null) return;

    var rows = $('#pg_article').propertygrid('getRows');
    for (var i = 0; i < rows.length; i++) {
        switch (rows[i].name) {
            case "Title":
                current_article.Title = rows[i].value;
                break;

            case "Author":
                current_article.Author = rows[i].value;
                break;

            case "Source":
                current_article.Source = rows[i].value;
                break;

            case "Create Date":
                current_article.CreateDate = rows[i].value;
                break;

            case "Sub Title":
                current_article.SubTitle = rows[i].value;
                break;

            case "key word":
                break;

            case "Description":
                break;

            default:
                break;
        }
    }

   current_article.Brief = $("#txt_brief").val();
}

function delete_article() {
    if (current_article == null) return;

    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/DeleteResource",
        data: "{ID:" + current_article.ID + "}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            var res = eval('(' + result.d + ')');

            if (res.flag < 0) {
                // error
                //console.log("delete_article: error");
            }
            else {
                current_article = null;
                $('#dg').datagrid('deleteRow', current_index);
                CKEDITOR.instances.editor01.setData("");
            }
        }
    });
}

function add_article() {
    if (current_group == null) return;

    current_article = {
        ID: 0,
        GroupID: current_group.ID,
        Type: 1,
        Title: "new article",
        SubTitle: "",
        Author: "",
        Source:"",
        Status: 1,
        CreateDate: "2015-12-20",
        Content: "",
        UID: "",
        ParentUID: "",
        GroupUID:"",
        Brief: ""
    };

    var content = CKEDITOR.instances.editor01.getData();
    get_article_property();
    if (current_article.Title == "") {
        alert("Please input the title");
        return;
    }

    var str = JSON.stringify(current_article);
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
                current_article.ID = res.flag;
                $('#dg').datagrid('appendRow', {
                    id: current_article.ID,
                    title: current_article.Title,
                    date: current_article.CreateDate
                });

                var rows = $('#dg').datagrid('getRows');
                current_index = rows.length-1;
            }
        }
    });
}

function save_article() {
    if (current_article == null) return;

    current_article.Content = "";
    var content = CKEDITOR.instances.editor01.getData();
    get_article_property();
    if (current_article.Title == "") {
        alert("Please input the title");
        return;
    }

    var str = JSON.stringify(current_article);
    //console.log(str);
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/UpdateResource",
        data: "{strResourceEx:'" + str + "',content:'"+content +"'}", // {parentID:0}
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

function load_article(articleID) {
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/GetResourceEx",
        data: "{ID:" + articleID + "}", // {parentID:0}
        dataType: 'json',
        success: function (result) {
            var res = eval('(' + result.d + ')');

            if (res.flag < 0) {
                // error
                //console.log("delete_group: error");
            }
            else {
                //alert(res.items.Title);
                CKEDITOR.instances.editor01.setData(res.items.Content);
                current_article = res.items;

                show_article_property();
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
                var node = $('#article_tree').tree('find', group_id);

                $('#article_tree').tree('remove', node.target);

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
    var group = { ID: 0, Type:1, Name: "", Code: "", Brief: "" };

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
        for (i = 0; i < n; i++) $('#dg').datagrid('deleteRow',0);
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
	if (g_php) {
		$.post("/group/get_all", {Type:1}, function(data) {
			//alert(data);
			var groups = eval('(' + data + ')');
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
	
    $('#article_tree').tree({
        onClick: function (node) {
            if (node.id) {
                show_group(node.id);
                show_resource_list(node.id);

                // empty current article
                current_article = null;
                CKEDITOR.instances.editor01.setData("");
            }
        }
    });

    $('#btn_add_group').click(function () {
        var node = $('#article_tree').tree('getSelected');
        if (node) {
            var parent = $('#article_tree').tree('getParent', node.target);
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
        var node = $('#article_tree').tree('getSelected');
        if (node && node.id) {
            add_group(node.id);
        }
    });

    $('#btn_delete_group').click(function () {
        var node = $('#article_tree').tree('getSelected');
        if (node && node.id) {
            delete_group(node.id);
        }
    });

    $('#dg').datagrid({
        onClickRow: function (index, row) {
            //alert(row.title);
            load_article(row.id);
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
                add_article();
            }
        }, {
            iconCls: 'icon-save',
            handler: function () {
                //alert('save');
                save_article();
            }
        }, {
            iconCls: 'icon-remove',
            handler: function () {
                //alert('delete');
                delete_article();
            }
        }]
    });         
}