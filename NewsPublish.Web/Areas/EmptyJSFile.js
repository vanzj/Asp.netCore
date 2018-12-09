function getBanner() {
    $.ajax({
        type:'get',
        url:'Home/GetBanner',
        success: function (data) {
            if (data.code == 200) {
                $(".carousel-inner").empty();
    $(".carousel-indicators").empty();
                for (var i = 0; i < data.data.length; i++) {
                    var m = data.data[i];
                    var banner;
   var indicators;
                    if(i == 0)
                    {
      indicators =' <li data-target="#focusslide" data-slide-to="'+i+'" class="active"></li>'

                        banner ='<div class="item active"><a href="'+m.url+'" target="_blank"><img src="'+m.image+'" class="img-responsive"></a></div>';
                    }
                    else
                    {
     indicators =' <li data-target="#focusslide" data-slide-to="'+i+'" ></li>'

                        banner = '<div class="item"><a href="'+m.url+'" target="_blank"><img src="'+m.image+'" class="img-responsive"> </a></div>';

                    }
                     $(".carousel-inner").append(banner);
     $(".carousel-indicators").append(indicators);

                }
            }
        }
    })
}