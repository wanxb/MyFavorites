var modal;
var closeBtn;
$(function () {
    loadData();
    $(document).keyup(function (event) {
        var kw = document.getElementById("searchKey").value;
        var modalInputs = document.querySelectorAll('.modal-input'); // 获取所有模态框中的输入框

        // 检查事件的目标元素是否是模态框中的输入框
        if (Array.from(modalInputs).some(input => input === event.target)) {
            event.stopPropagation(); // 阻止事件传播
            return;
        }

        if (kw && kw != null && kw != "null") {
            if (event.keyCode == 13) {
                goSearch();
            }
        }
        if (!kw || kw == null || kw == "null") {
            if (event.keyCode == 8) {
                window.location.href = "../../";
            }
        }
    });

    // 获取模态框和关闭按钮
    modal = document.getElementById("myModal");
    closeBtn = document.getElementsByClassName("close")[0];
});

function loadData() {
    var url = '/favorites/get';
    var kw = decodeURI(getUrlParam('kw'));
    if (kw && kw != null && kw != "null") {
        $("#searchKey").val(kw);
        $(".close-del").show();
        //getSearch(data, kw);
        url = url + '?keyWord=' + kw;
    }
    $.getJSON(url, function (data) {
        if (data != null) {
            // 排序方法
            function jsonSort(a, b) {
                return a.sort - b.sort;
            }
            // 第一层排序
            data = data.sort(jsonSort);
            for (var i = 0; i < data.length; i++) {
                // 第二层排序
                data[i].items = data[i].items.sort(jsonSort);
            }
            // 渲染HTML
            var outhtml = template2_setdata($('#template_content').html(), data);
            $('.main-box').html(outhtml);
        }
    });
}

function delItem(event) {
    var blockBox = event.target.parentElement;
    // 获取 .block-box 元素中的 .tit 元素的文本内容（即 data[i].items[j].name 的值）
    var id = blockBox.querySelector('.id').textContent;
    var uid = blockBox.querySelector('.uid').textContent;
    var data = {
        Id: id,
        Uid: uid,
    };
    // 使用SweetAlert2创建确认提示框
    Swal.fire({
        title: '确认删除吗？',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: '确定',
        cancelButtonText: '取消',
        customClass: {
            popup: 'custom-class', // 自定义提示框的CSS类名
        }
    }).then((result) => {
        // 如果用户点击了确认按钮
        if (result.isConfirmed) {
            fetch('/favorites/delete', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(data)
            })
                .then(response => {
                    if (response.ok) {
                        blockBox.remove();
                    } else {
                        console.error('Error deleting resource:', response.status);
                    }
                })
                .catch(error => {
                    console.error('Error deleting resource:', error);
                });
        }
    });
}

function getUrlParam(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

function clearSearch() {
    window.location.href = "../../";
}

function goSearch() {
    var kw = document.getElementById("searchKey").value;
    if (kw && kw != null && kw != "null") {
        window.location.href = encodeURI(encodeURI("../Favorites/list?kw=" + kw));
    } else {
        window.location.href = "../../";
    }
}

function getSearch(data, kw) {
    for (var i in data) {
        var newItems = new Array();
        var jData = data[i];
        if (!jData) {
            continue;
        }
        var items = jData.items;
        for (var j in items) {
            var item = items[j];
            if (!item) {
                continue;
            }
            if ((jData.type.toLowerCase()).indexOf(kw.toLowerCase()) > -1) {
                //匹配分类名称
                newItems.push(item);
            } else if ((jData.description.toLowerCase()).indexOf(kw.toLowerCase()) > -1) {
                //匹配分类详情
                newItems.push(item);
            } else if ((item.name.toLowerCase()).indexOf(kw.toLowerCase()) > -1) {
                //匹配名称
                newItems.push(item);
            } else if ((item.url.toLowerCase()).indexOf(kw.toLowerCase()) > -1) {
                //匹配链接地址
                newItems.push(item);
            } else if ((item.description.toLowerCase()).indexOf(kw.toLowerCase()) > -1) {
                //匹配详情
                newItems.push(item);
            }
        }
        data[i].items = newItems;
    }
}

// 当用户点击 "addItem" 按钮时显示模态框
function addItem() {
    modal.style.display = "block";
}

// 当用户点击保存按钮时执行保存操作
function saveItem() {
    // 获取用户输入的数据
    var type = document.getElementById("type").value;
    var description = document.getElementById("description").value;
    var url = document.getElementById("url").value;
    var name = document.getElementById("name").value;

    // 验证输入项是否为空
    if (type === "" || name === "" || url === "") {
        document.getElementById("errMsg").style.display = "block";
        return;
    }
    // 在这里可以将数据保存到后端，或者根据需求进行其他操作
    // 构建请求体数据
    var data = {
        Type: type,
        Description: description,
        Url: url,
        Name: name
    };

    // 发送 POST 请求
    fetch('/favorites/post', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify(data)
    })
        .then(response => response.json())
        .then(data => {
            // 请求成功处理逻辑
            console.log('Item saved successfully!');
            // 显示Toast通知
            var toastNotification = document.getElementById("toastNotification");
            toastNotification.textContent = "保存成功！";
            toastNotification.style.display = "block";
            // 2秒后隐藏Toast通知
            setTimeout(function () {
                toastNotification.style.display = "none";
            }, 2000);
            closeModal(); // 关闭模态框或执行其他操作
            window.location.href = "../../";
        })
        .catch(error => {
            // 请求异常处理逻辑
            console.error('Error:', error);
        });
}

// 当用户点击取消按钮或模态框外部时关闭模态框
function closeModal() {
    modal.style.display = "none";
    document.getElementById("errMsg").style.display = "none";
}