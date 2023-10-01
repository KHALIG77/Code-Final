

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
    $(document).on("click", ".wish-remove", function (e) {
        e.preventDefault();

        let url = $(this).attr("data-fetch");
        $.get(url, function (data) {
            $(".wishlist-parent").html(data);
            $(".count-wishlist").html($(".wish-total").val())
        });
    });
  
   

    
});

let wishBtns = document.querySelectorAll(".wish-btn")
wishBtns.forEach((btn) => {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        let url = btn.getAttribute("data-fetch")

        fetch(url)
            .then(data => data.json())
            .then(response => {
                ToastWish(response.status)
                let totalCount = document.querySelector(".count-wishlist")
                totalCount.innerHTML = response.total;
            })
    })
})
//let wishRemoveBtns = document.querySelectorAll(".wish-remove")

//wishRemoveBtns.forEach((btn) => {
    
//    btn.addEventListener("click", (e) => {
       
//        e.preventDefault();
//        let url = btn.getAttribute("data-fetch")
//        console.log(url)
        

//        fetch(url)
//            .then(data => data.text())
//            .then(response => {
//                /*ToastWish(response.status)*/
//                let totalCount = document.querySelector(".count-wishlist")
//                totalCount.innerHTML = document.querySelector(".wish-total").value;
//            })
//    })
//})














