
//let wishRemoveBtn = document.querySelectorAll(".wish-remove")
//wishRemoveBtn.forEach((btn) => {
//    btn.addEventListener("click", (e) => {

//        e.preventDefault();
//        let url = btn.getAttribute("href")

//        fetch(url)
//            .then(data => data.text())
//            .then(response => {
//                let parent = document.querySelector(".wishlist-parent")
//                parent.innerHTML=response
//            })
//    })
//})
$(".wish-remove").on("click", function (e) {
    e.preventDefault();
    let url = $(this).attr("href");

    $.get(url, function (data) {
        $(".wishlist-parent").html(data);
        $(".count-wishlist").html($(".wish-total").val()) 
    });
});