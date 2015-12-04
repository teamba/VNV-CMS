var isIe = (document.all) ? true : false;
function mousePosition(ev) {
    if (ev.pageX || ev.pageY) {
        return { x: ev.pageX, y: ev.pageY };
    }
    return {
        x: ev.clientX + document.body.scrollLeft - document.body.clientLeft, y: ev.clientY + document.body.scrollTop - document.body.clientTop
    };
}
function showMessageBox(wTitle, content, pos, wWidth) {
    closeWindow();
    var bWidth = parseInt(document.documentElement.scrollWidth);
    var bHeight = parseInt(document.documentElement.scrollHeight);
    if (isIe) {
        setSelectState('hidden');
    }
    var back = document.createElement("div");
    back.id = "back";
    var styleStr = "top:0px;left:0px;position:absolute;background:#666;width:" + bWidth + "px;height:" + bHeight + "px;";
    styleStr += (isIe) ? "filter:alpha(opacity=0);" : "opacity:0;";
    back.style.cssText = styleStr;
    document.body.appendChild(back);
    showBackground(back, 50);
    var mesW = document.createElement("div");
    mesW.id = "mesWindow";
    mesW.className = "mesWindow";
    mesW.innerHTML = "<div class='mesWindowTop'><table width='100%' height='100%'><tr><td>" + wTitle + "</td><td style='width:1px;'><input type='button' onclick='closeWindow();' title='关闭窗口' class='close' value='X' /></td></tr></table></div><div class='mesWindowContent' id='mesWindowContent'>" + content + "</div><div class='mesWindowBottom'></div>";
    styleStr = "top:50%;left:50%;margin:-150px 0 0 -200px;position:absolute;width:" + wWidth + "px;";
    mesW.style.cssText = styleStr;
    document.body.appendChild(mesW);
}

function showMessageBox1(wTitle, content, pos, wWidth) {
   
    closeWindow();
    
    var bWidth = parseInt(document.documentElement.scrollWidth);
    var bHeight = parseInt(document.documentElement.scrollHeight);
    if (isIe) {
        setSelectState('hidden');
    }
    var back = document.createElement("div");
    back.id = "back";
    var styleStr = "top:0px;left:0px;position:absolute;background:#666;width:" + bWidth + "px;height:" + bHeight + "px;";
    styleStr += (isIe) ? "filter:alpha(opacity=0);" : "opacity:0;";
    back.style.cssText = styleStr;
    document.body.appendChild(back);
    //showBackground(back, 50);
    var mesW = document.createElement("div");
    mesW.id = "mesWindow";
    mesW.className = "mesWindow";
    mesW.innerHTML = "<div class='mesWindowTop'><table width='100%' height='100%'><tr><td>" + wTitle + "</td><td style='width:1px;'><input type='button' onclick='closeWindow();' title='关闭窗口' class='close' value='X' /></td></tr></table></div><div class='mesWindowContent' id='mesWindowContent'>" + content + "</div><div class='mesWindowBottom'></div>";
    styleStr = "top:" + (parseInt(pos.y)-100) + "px;left:30%;margin:-150px 0 0 -200px;position:absolute;width:" + wWidth + "px;";
    mesW.style.cssText = styleStr;
    document.body.appendChild(mesW);
}
function showBackground(obj, endInt) {
    if (isIe) {
        obj.filters.alpha.opacity += 10;
        if (obj.filters.alpha.opacity < endInt) {
            setTimeout(function () { showBackground(obj, endInt) }, 0);
        }
    }

    else {
        var al = parseFloat(obj.style.opacity); al += 0.1;
        obj.style.opacity = al;
        if (al < (endInt / 100))
        { setTimeout(function () { showBackground(obj, endInt) }, 0); }
    }
}
function closeWindow() {
    if (document.getElementById('back') != null) {
        document.getElementById('back').parentNode.removeChild(document.getElementById('back'));
    }
    if (document.getElementById('mesWindow') != null) {
        document.getElementById('mesWindow').parentNode.removeChild(document.getElementById('mesWindow'));
    }
    if (isIe) {
        setSelectState('');
    }
}
function setSelectState(state) {
    var objl = document.getElementsByTagName('select');
    for (var i = 0; i < objl.length; i++) {
        objl[i].style.visibility = state;
    }
}