function create_audio_node(group, groups, parent) {
    $('#audio_tree').tree('append', {
        parent: parent.target,
        data: {
            id: group.ID,
            text: group.Name
        }
    });

    var node;
    node = $('#audio_tree').tree('find', group.ID);
    
    var i;
    if (node) for (i = 0; i < groups.length; i++) if (groups[i].ParentID == group.ID) create_audio_node(groups[i], groups, node);
}

function init_audio() {
    $.ajax({
        type: "POST",
        contentType: "application/json",
        url: "../vnv.asmx/GetGroupAll",
        data: "{Type:3}", // 
        dataType: 'json',
        success: function (result) {
            var groups = eval('(' + result.d + ')');

            var i;
            var root = $('#audio_tree').tree('getRoot');
            for (i = 0; i < groups.items.length; i++) {
                if (groups.items[i].ParentID == 0) create_audio_node(groups.items[i], groups.items, root);
            }
        }
    });
}