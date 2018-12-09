function getNews() {
    $.ajax({
        type:'get',
        url:'Home/GetNews',
        success: function (data) {
            if (data.code == 200) {
    $("#home_news").empty();

                for (var i = 0; i < data.data.length; i++) {
                    var m = data.data[i];
                    var news;
                    news = ‘<article class="excerpt excerpt-‘+i+’" style="">
                <a class="focus" href="/GetOneNews?id='+m.Id+'" title="m.Title" target="_blank"><img class="thumb" data-original="m.image" src="m.image" alt="央企风电三大巨头联手打造东营海上风电装备制造基地" style="display: inline;"></a>
                <header>
                    <h2>
                        <a href="show.html" title="央企风电三大巨头联手打造东营海上风电装备制造基地" target="_blank">央企风电三大巨头联手打造东营海上风电装备制造基地</a>
                    </h2>
                </header>
                <p class="meta">
                    <time class="time"><i class="glyphicon glyphicon-time"></i> 2016-10-14</time>
                    <a class="comment" title="评论" target="_blank"><i class="glyphicon glyphicon-comment"></i>4</a>
                </p>
                <p class="note">大众网东营6月27日讯（通讯员 张学荣）6月23日，山东（河口）风电产业园首批机组下线既二期项目开工仪式在河口经济开发区隆重举行，标志着河口经济开发区在实施新旧动能转换重大工程中又迈出了可喜的一步。山东（河口）风电产业园建设将对加快东营新旧动能转换，推动海洋经济深度融合发展具有现实意义和深远影响。</p>
            </article>’；
                     $("#home_news").append(news);
 
                }
            }
        }
    })
}