 var pageSize = 15;
        var pageIndex = 1;
        var currentPageCount = 0;
        var classifyId = 0;
        var keyword = "";
        var lastPage = 1;
        window.onload = (function () {
            // optional set
            pageNav.pre = "&lt;上一页";
            pageNav.next = "下一页&gt;";
            getNews(pageIndex, pageSize, classifyId, keyword);
        });
        $('.table-sort').dataTable({
            "lengthMenu": false,//显示数量选择
            "bFilter": false,//过滤功能
            "bPaginate": false,//翻页信息
            "bInfo": false,//数量信息
            "aaSorting": [[1, "desc"]],//默认第几个排序
            "bStateSave": true,//状态保存
            "aoColumnDefs": [
                //{"bVisible": false, "aTargets": [ 3 ]} //控制列的隐藏显示
                { "orderable": false, "aTargets": [0, 1, 2, 3, 4, 5, 6, 7] }// 制定列不参与排序
            ]
        });
        function search() {
            var classifyId = $("#classifyId").val();
            var keyword = $("#keyword").val();
            pageIndex = 1;
            getNews(pageIndex, pageSize, classifyId, keyword);
        }
        function getNews(pageIndex, pageSize, classifyId, keyword) {
            $.ajax({
                type: 'get',
                async: true,
                url: '/Admin/News/GetNews',
                data: { pageIndex: pageIndex, pageSize: pageSize, classifyId: classifyId, keyword: keyword },
                success: function (result) {
                    currentPageCount = result.data.length;//当前页的新闻数量
                    var totalPage = parseInt(result.total / pageSize + 1);//总页数
                    // p,当前页码,pn,总页面
                    pageNav.fn = function (p, pn) {
                        $("#pageinfo").text("当前页:" + p + " 总页: " + totalPage);
                        if (p != lastPage)
                            getNews(p, pageSize, classifyId, keyword)
                    };
                    pageNav.go(pageIndex, totalPage);
                    $("#news_contents").empty();
                    for (var i = 0; i < result.data.length; i++) {
                        var trData = result.data[i];
                        var tr = '<tr class="text-c">
        <td>'+trda+'</td>
        <td>时政要闻</td>
        <td>外媒曝光微软内部邮件：可折叠Surface Phone有戏</td>
        <td>现在，外媒The Verge居然曝光了微软的内部邮件，其中还泄漏了Andromeda项目的...</td>
        <td>2014-6-11 11:11:42</td>
        <td>这是一个备注</td>
        <td class="f-14 user-manage">
        <a title="删除" href="javascript:;" onClick="del(this,'1')" class="ml-5" style="text-decoration:none"><i class="icon-trash"></i></a>
        </td>
      </tr>';
                        $("#news_contents").append(tr);
                    }
                    lastPage = pageIndex;
                }
            });
        }
        //删除新闻
        function del(obj, id) {
            layer.confirm('确认要删除吗？', function (index) {
                //$(obj).parents("tr").remove();
                //layer.msg('已删除!', 1);
                $.ajax({
                    type: 'post',
                    async: true,
                    url: '/Admin/News/DelNews',
                    data: { id: id },
                    success: function (result) {
                        if (result.code == 200) {
                            layer.msg('已删除!', 1);
                            if (currentPageCount == 1) {
                                if (pageIndex == 1) {
                                    reload();
                                } else {
                                    getNews(pageIndex - 1, pageSize, classifyId, keyword);
                                }
                            } else {
                                getNews(pageIndex, pageSize, classifyId, keyword);
                                //$(obj).parents("tr").remove();
                            }
                        } else {
                            layer.msg(result.result, 1);
                        }
                    }
                });
            });
        }
        function reload() {
            location.replace(location.href);
        }