

$(document).ready(function () {
    $(document).on("click",".add-basket", function (e) {
        e.preventDefault();
        let url = $(this).attr("href");
        $.get(url, function (data) {
            $(".basket-view").html(data);
            var totalCount = $(".total-count").val()
            $(".cart-count").html(totalCount)
        });
    });
    $(document).on("click", ".quickview-close", function (e) {
     
        $(".quickview-popup").removeClass("active");
          
       
       
    });

     
});
$(".basket-view").on("click", ".remove-basket", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    $.get(url, function (data) {
        $(".basket-view").html(data);
        var totalCount = $(".total-count").val()
        $(".cart-count").html(totalCount)
    });
});

$(".quickview-button").on("click", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");
    $.get(url, function (data) {
        $("#quick-quick").html(data);
      
    });
});








