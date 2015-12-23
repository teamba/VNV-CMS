var g_php = 1; // 1 -- using PHP, 0 -- using .net
var g_Load = '<span align="center"><img src="/BackEnd/images/loading.gif" /></span>';
var m_login = '../user/login';

function PageLoad(pageID) {
    if (isLogin())
    //个页面的初始化方法
        Initialize(pageID);
    else window.parent.location.href = "/backend/login.html";
}

function onEnterDown() {
    if (window.event.keyCode == 13) {
        login();
    }
}


function login() {
    if (!validate()) return;

    var name = document.getElementById("txtName").value.replace(/^\s+|\s+$/g, "");
    var pwd = document.getElementById("txtPwd").value.replace(/^\s+|\s+$/g, "");

    if (name == "") {
        alert("请输入用户名！");
        return;
    }

    if (pwd == "") {
        alert("请输入密码！");
        return;
    }

    if (validate() == false) return;

    if (name == "admin" && pwd == "123456") {
        //$.cookie('userid') = 1;
        SetCookie('userid', '1', '', '', '');
        SetCookie('username', 'admin', '', '', '')
        window.location.href = "index.html"; 
    }
    else {
        alert("用户名和密码错误！");
    }

    /*
    m_parameters = { 'account': name, 'password': pwd };
    new Ajax.Request(m_login + '', {
        method: "post",
        parameters: m_parameters,
        onSuccess: function (xmlhttprequest) {
            onLogin(xmlhttprequest.responseText);
        }
    });*/
}

// http://www.cnblogs.com/mizzle/archive/2012/02/10/2345891.html
function onLogin(responseText) {
    //alert(responseText);
/*    if (responseText != "true") {
        alert("用户名/密码不匹配！");
        document.getElementById("txtPwd").value = "";
        return;
    }
    window.location.href = "index.aspx"; */
    var ret = JSON.parse(responseText);
    if (ret.ok=='1')
    {
        SetCookie('userid', ret.ID);
        SetCookie('username', ret.Name)
        window.location.href = "index.html"; 
    }
    else
    {
        alert(ret.msg);
    }
}

function isLogin() {return true;
    new Ajax.Request(m_login + '?command=isLogin', {
        method: "post",
        parameters: m_parameters,
        onSuccess: function (xmlhttprequest) {
            var admin = xmlhttprequest.responseText;
            if (admin == "") return false;
        }
    });
    return true;
}

function shownode(contentId, nodeId) {
    var sbtitle = document.getElementById(contentId);
    var icon = document.getElementById(nodeId);
    if (sbtitle) {
        if (sbtitle.style.display == 'block') {
            sbtitle.style.display = 'none';
            icon.src = '/BackEnd/images/tree-ul-li_2.gif';
        } else {
            sbtitle.style.display = 'block';
            icon.src = '/BackEnd/images/tree-ul-li_1.gif';
        }
    }
}

var g_url;
var g_code;
var g_size;
var g_id;

function assignment(u,c,i) {
    g_url = u;
    g_code = c;
    var s = format(g_code);
    g_size = s.w + "x" + s.h;
    g_id = i;
}

function showUpload() {
//    if (m_t == undefined) return;

    var params = {
        uploadServerUrl: "/backend/u.aspx", //上传响应页面(必须设置)
        jsFunction: "onSuccessUpload", 		//上传成功后回调JS
        filter: "*.jpg;*.png", 		//上传文件类型限制
        size: g_size,
        code: g_code,
//        type: m_t,
        objID: g_id
    };
    swfobject.embedSWF("/BackEnd/flash/u.swf", "myContent", "450", "210", "10.0.0", "/BackEnd/flash/expressInstall.swf", params);

}

// cookie function 

function SetCookie(cookieName, cookieValue, path, domain, secure){ 
 var expires = new Date(); 
 expires.setTime(expires.getTime() + 100000000); 
 document.cookie = escape(cookieName) + '=' + escape(cookieValue) 
 + (expires ? '; expires=' + expires.toGMTString() : '') 
 + (path ? '; path=' + path : '/') 
 + (domain ? '; domain=' + domain : '') 
 + (secure ? '; secure' : ''); 
} 

function GetCookie(name){  
 var cookie_start = document.cookie.indexOf(name); 
 var cookie_end = document.cookie.indexOf(";", cookie_start); 
 return cookie_start == -1 ? '' : unescape(document.cookie.substring(cookie_start + name.length + 1, (cookie_end > cookie_start ? cookie_end : document.cookie.length))); 
}  

function DelCookie(cookieName, cookieValue, path, domain, secure){  
 var cookieValue="hello"; 
 var expires = new Date(); 
 expires.setTime(expires.getTime() - 100000); 
 document.cookie = escape(cookieName) + '=' + escape(cookieValue) 
 + (expires ? '; expires=' + expires.toGMTString() : '') 
 + (path ? '; path=' + path : '/') 
 + (domain ? '; domain=' + domain : '') 
 + (secure ? '; secure' : ''); 
}

// added in 2015-11-21
function jump(url) {
    //parent.document.getElementById("mianFrame").src = url;
    //        parent.window.location = url;
    //console.log("jump:" + url);

    $("#div_foot").hide();
    $("#div_right_content").hide();
    $("#div_right_content").empty();
    $("#div_right_content").removeClass("right");

    $("#div_right_content").css('left', '0px');
    $("#div_right_content").show();
    $("#div_foot").show();

    $("#div_right_content").load(url + " #div_right_content", function () {
        // do something
        switch (url) {
            case "audio.html":
                init_audio();
                break;

            case "article.html":
                init_article();
                break;

            case "columnRelease.html":
                break;

            case "columnSet.html":
                init_columnSet();

                break;

            case "dealer.html":
                break;

            case "file.html":
                break;

            case "param.html":
                break;

            case "photo.html":
                init_photo();
                break;

            case "product.html":
                break;

            case "sysSet.html":
                break;

            case "sysUser.html":
                break;

            case "userInfo.html":
                break;

            case "video.html":
                break;

            default:
                break;
        }
        $.parser.parse();

    });
}

function jump1() {
    parent.window.location = "../index.html";
}

function jump2() {
    DelCookie('userid');
    parent.window.location = "../login.html";
}

function init_page() {
    $("#left").load("../html/left_menu.html #menu_item", function () {
        // do something
        initMenu();
    });

    $("#div_foot").load("../html/foot.html #foot_item", function () {
        // do something
    });
}

