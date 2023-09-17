

$(document).ready(function () {
    $(".add-basket").on("click", function (e) {
        e.preventDefault();
        let url = $(this).attr("href");
        $.get(url, function (data) {
            $(".basket-view").html(data);
            var totalCount = $(".total-count").val()
            $(".cart-count").html(totalCount)
        });
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






