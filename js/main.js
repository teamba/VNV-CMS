$(document).ready(function(){

	// search the article group
	$.post("/group/search", {
		parentid: 0,
		type: 1
	}, function (data, textStatus) {

	  	if (data.ok == 1) {
	  		 
	  		show_groups(data.groups);
	  	}
	  	else {
	  		alert(data.msg);
	  	};

	}, "json");
	
	$("#btn_modify_group").click(function() {modify_group();});
	$("#btn_add_group").click(function() {add_group();});
	$("#btn_add_subgroup").click(function() {add_subgroup();});
	$("#btn_delete_group").click(function() {delete_group();});

	$("#btn_edit_article_group").click(function() {edit_article_group();});
	$("#btn_list_article").click(function() {list_article();});
	$("#btn_edit_article_priolity").click(function() {edit_article_priolity();});
});

// http://blog.csdn.net/hurryjiang/article/details/7453235
// http://www.codeweblog.com/jstree-the-demo-how-to-create-a-child-node/
// http://peterknolle.com/hierarchies-with-remote-objects-and-jstree/

function show_groups(groups) {
	// create the tree
	$('#jstree').jstree({
  		"core" : {
		    "animation" : 0,
		    "check_callback" : true,
		    "themes" : { "stripes" : true, "variants": true },
		    'data' : null
  		},		
	});

	var i;
	for(i=0; i<groups.length; i++) {
		create_group_node(groups[i]);
	};

	// bind the event
	$('#jstree').on("changed.jstree", function (e, data) {
	    select_group(data.selected);
	});	
};

function create_group_node(group) {
	var newNode = {
		id: "group_node_" + group.ID,
		text: group.Name
	};
	 
	if (group.ParentID == "0") {
		$("#jstree").jstree(true).create_node("#", newNode, "last", null, true);
	}
	else {
		var parent_node = $("#jstree").jstree(true).get_node("group_node_" + group.ParentID);
		$("#jstree").jstree(true).create_node(parent_node, newNode, "last", null, true);
	}

	if (group.groups != null) {
		var i;
		for(i=0; i<group.groups.length; i++) create_group_node(group.groups[i]);
	}
};

function select_group(group_id) {
	var items = group_id.toString().split("_");
	if (items.length < 3) return;

	// apply the group by id -- items[2]
	$.post("/group/get", {
		id: items[2]
	}, function (data, textStatus) {

	  	if (data.ok == 1) {
	  		show_group(data);
	  	}
	  	else {
	  		alert(data.msg);
	  	};

	}, "json");
};

function show_group(group) {
	//alert(group.Name);
	$("#txt_group_name").val(group.Name);
	$("#txt_group_id").val(group.ID);
	$("#txt_group_brief").val(group.Brief);
};

function modify_group() {
	//alert("modify_group!");
	var name = $("#txt_group_name").val();
	var id = $("#txt_group_id").val();
	var brief = $("#txt_group_brief").val();

	$.post("/group/modify", {
		id: id,
		name: name,
		brief: brief
	}, function (data, textStatus) {

	  	if (data.ok == 1) {
	  		modify_group_node(id, name);
	  		//alert("ok");
	  	}
	  	else {
	  		alert(data.msg);
	  	};

	}, "json");	
};

function modify_group_node(id, name) {
	//alert("modify_group_node:"+id);
	var node_id = "group_node_" + id;
	var node = $("#jstree").jstree(true).get_node(node_id);
	if (node != null) $("#jstree").jstree(true).set_text(node, name);
};

function add_group() {
	console.log("add_group");
	var id = $("#txt_group_id").val();

	var parent_id = "0";
	if (id == '') {
		console.log("create root group.");
	}
	else {
		var node = $("#jstree").jstree(true).get_node("group_node_" + id);
		if (node != null) {
			var parent_node_id = $("#jstree").jstree(true).get_parent(node);
			if (parent_node_id != null) {
				console.log(parent_node_id);
				var items = parent_node_id.toString().split("_");
				if (items.length == 3) parent_id = items[2];				
			}
		}
	}

	var name = $("#txt_group_name").val();
	var brief = $("#txt_group_brief").val();

	console.log("add_group: parent_id=" + parent_id);
	$.post("/group/add", {
		parentid: parent_id,
		name: name,
		brief: brief,
		type: 1
	}, function (data, textStatus) {

	  	if (data.ok == 1) {
	  		add_group_node(data);
	  	}
	  	else {
	  		alert(data.msg);
	  	};

	}, "json");		
};

function add_group_node(group) {
	var tree = $("#jstree").jstree(true);

	var newNode = {
		id: "group_node_" + group.ID,
		text: group.Name
	}

	if (group.ParentID == '0') {
		$("#jstree").jstree(true).create_node("#", newNode, "last", null, true);
	}
	else {
		var parent_node = $("#jstree").jstree(true).get_node("group_node_" + group.ParentID);
		$("#jstree").jstree(true).create_node(parent_node, newNode, "last", null, true);
	}
	
};

function add_subgroup() {
	console.log("add_subgroup");
	var id = $("#txt_group_id").val();

	var parent_id = "0";
	if (id == '') {
		alert('Please select the parent node!');
		return;
	}

	parent_id = id;

	var name = $("#txt_group_name").val();
	var brief = $("#txt_group_brief").val();

	console.log("add_group: parent_id=" + parent_id);
	$.post("/group/add", {
		parentid: parent_id,
		name: name,
		brief: brief,
		type: 1
	}, function (data, textStatus) {

	  	if (data.ok == 1) {
	  		add_group_node(data);
	  	}
	  	else {
	  		alert(data.msg);
	  	};

	}, "json");		

};

function delete_group() {
	console.log("delete_group");
	var id = $("#txt_group_id").val();

	if (id == '') {
		alert('Please select the group!');
		return;
	}	

	$.post("/group/delete", {
		id: id
	}, function (data, textStatus) {

	  	if (data.ok == 1) {
	  		var node = $("#jstree").jstree(true).get_node("group_node_" + id);
	 		$('#jstree').jstree(true).delete_node(node);

	 		$('#txt_group_id').val('');
	 		$('#txt_group_brief').val('');
	 		$('#txt_group_name').val('');
	  	}
	  	else {
	  		alert(data.msg);
	  	};

	}, "json");		

};

// -------------- article group, list, priolity -----------------

function edit_article_group() {
	$("#div_article_group").show();
	$("#div_article_list").hide();
	$("#div_article_priolity").hide();
};

// http://www.jeasyui.com/documentation/datagrid.php
function list_article() {
    $("#div_article_list").html('');

	$("#div_article_list").show();
	$("#div_article_group").hide();
	$("#div_article_priolity").hide();

	var group_id = $("#txt_group_id").val();
//	if (group_id == '') return;

    var w = $("#div_article_list").width();
    var h = $("#div_article_list").height();
    //alert(w);

    var w1=w*1/18, w2=w*10/18, w3=w*3/18, w4=w*3/18, w5=w-w1-w2-w3-w4-1;
    var strHTML = "";
    strHTML = strHTML + "";
	strHTML = strHTML + "       <table id='dg_list' class=\"easyui-datagrid\" title=\"文章列表\" style=\"width:" + w + "px;height:" + h + "px\" \n";
	strHTML = strHTML + "		        data-options=\"singleSelect:true,collapsible:true,url:'/resource/get_list?type=1&groupid=" + group_id + "',method:'get',toolbar:toolbar\"> \n";
	strHTML = strHTML + "	        <thead> \n";
	strHTML = strHTML + "		        <tr> \n";
	strHTML = strHTML + "			        <th data-options=\"field:'ID',width:" + w1 + "\">ID</th> \n";
	strHTML = strHTML + "			        <th data-options=\"field:'Title',width:" + w2 + "\">标题</th> \n";
	strHTML = strHTML + "			        <th data-options=\"field:'Author',width:" + w3 + ",align:'right'\">作者</th> \n";
	strHTML = strHTML + "			        <th data-options=\"field:'Source',width:" + w4 + ",align:'right'\">来源</th> \n";
	strHTML = strHTML + "			        <th data-options=\"field:'Status',width:" + w5 + "\">状态</th> \n";
	strHTML = strHTML + "		        </tr> \n";
	strHTML = strHTML + "	        </thead> \n";
	strHTML = strHTML + "        </table> \n";

	strHTML = strHTML + "        <script type=\"text/javascript\"> \n";
	strHTML = strHTML + "            var toolbar = [{ \n";
	strHTML = strHTML + "                text: '新增', \n";
	strHTML = strHTML + "                iconCls: 'icon-add', \n";
	strHTML = strHTML + "                handler: function () { add_article(); } \n";
	strHTML = strHTML + "            }, { \n";
	strHTML = strHTML + "                text: '浏览', \n";
	strHTML = strHTML + "                iconCls: 'icon-cut', \n";
	strHTML = strHTML + "                handler: function () { view_article(); } \n";
	strHTML = strHTML + "            }, '-', { \n";
	strHTML = strHTML + "                text: '编辑', \n";
	strHTML = strHTML + "                iconCls: 'icon-save', \n";
	strHTML = strHTML + "                handler: function () { edit_article(); } \n";
	strHTML = strHTML + "            }]; \n";
	strHTML = strHTML + "        </script> \n";

    $("#div_article_list").html(strHTML);
    $.getScript('easyui/jquery.easyui.min.js');
};

function add_article() {
    alert('enter add_article');
};

function view_article() {
    alert('enter view_article');
};

function edit_article() {
    //alert('edit_article');
    var row = $("#dg_list").datagrid('getSelected');
    if (row != null) {
        alert(row.Title);
    }
    else {
        alert('Please select an article!');
    }
};

function edit_article_priolity() {
    $("#div_article_priolity").html('');

	$("#div_article_priolity").show();
	$("#div_article_group").hide();
	$("#div_article_list").hide();

    // http://blog.sina.com.cn/s/blog_51048da70101djoy.html
    var w = $("#div_article_priolity").width();
    var h = $("#div_article_priolity").height();
    //alert(w);

    var w1=w*8/74, w2=w*10/74, w3=w*8/74, w4=w*8/74, w5=w*24/74, w6=w-w1-w2-w3-w4-w5-1;
    var strHTML = "";
    strHTML = strHTML + "";
	strHTML = strHTML + "       <table class=\"easyui-datagrid\" title=\"Basic DataGrid\" style=\"width:" + w + "px;height:" + h + "px\" \n";
	strHTML = strHTML + "		        data-options=\"singleSelect:true,collapsible:true,url:'datagrid_data1.json',method:'get'\"> \n";
	strHTML = strHTML + "	        <thead> \n";
	strHTML = strHTML + "		        <tr> \n";
	strHTML = strHTML + "			        <th data-options=\"field:'itemid',width:" + w1 + "\">Item ID</th> \n";
	strHTML = strHTML + "			        <th data-options=\"field:'productid',width:" + w2 + "\">Product</th> \n";
	strHTML = strHTML + "			        <th data-options=\"field:'listprice',width:" + w3 + ",align:'right'\">List Price</th> \n";
	strHTML = strHTML + "			        <th data-options=\"field:'unitcost',width:" + w4 + ",align:'right'\">Unit Cost</th> \n";
	strHTML = strHTML + "			        <th data-options=\"field:'attr1',width:" + w5 + "\">Attribute</th> \n";
	strHTML = strHTML + "			        <th data-options=\"field:'status',width:" + w6 + ",align:'center'\">Status</th> \n";
	strHTML = strHTML + "		        </tr> \n";
	strHTML = strHTML + "	        </thead> \n";
	strHTML = strHTML + "        </table> \n";

    $("#div_article_priolity").html(strHTML);
    $.getScript('easyui/jquery.easyui.min.js');
};