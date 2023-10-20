

$(document).on("click", ".wish-remove",function (e) {
    e.preventDefault();
    let url = $(this).attr("href");

    $.get(url, function (data) {
        $(".wishlist-parent").html(data);
        $(".count-wishlist").html($(".wish-total").val()) 
    });
});