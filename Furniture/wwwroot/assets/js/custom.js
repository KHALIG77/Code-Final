

$(document).ready(function () {

 
    $(".subscribe-btn-index").click(function () {
        var formDataIndex = $(".newsletter-form-index").serialize();

        $.ajax({
            type: "POST",
            url: "/home/subscribe",
            data: formDataIndex,
            dataType: "json",
            success: function (response) {
                if (response.status == 0) {
                    new Toast({
                        message: 'Please write your email',
                        type: 'danger',

                    });
                    console.log(response.status)
                }
                else if (response.status == 1) {
                    new Toast({
                        message: 'Please write your email correctly',
                        type: 'warning',
                    });

                }
                else if (response.status == 3) {
                    new Toast({
                        message: 'Congratulations, you have subscribed',
                        type: 'success',
                    });

                }
                else if (response.status == 2) {
                    new Toast({
                        message: 'This email is already in use',
                        type: 'danger',
                    });

                }


            },

        });
    });

      


    $(".subscribe-btn-footer").click(function () {
        var formDataFooter = $(".newsletter-form-footer").serialize();

        $.ajax({
            type: "POST",
            url: "/home/subscribe",
            data: formDataFooter,
            dataType: "json",
            success: function (response) {
                if (response.status == 0) {
                    new Toast({
                        message: 'Please write your email',
                        type: 'danger',

                    });
                    console.log(response.status)
                }
                else if (response.status == 1) {
                    new Toast({
                        message: 'Please write your email correctly',
                        type: 'warning',
                    });

                }
                else if (response.status == 3) {
                    new Toast({
                        message: 'Congratulations, you have subscribed',
                        type: 'success',
                    });

                }
                else if (response.status == 2) {
                    new Toast({
                        message: 'This email is already in use',
                        type: 'danger',
                    });

                }


            },

        });
    })

     



    $(document).on("click",".add-basket", function (e) {
       
        
        let url = $(this).attr("data-fetch");
        $.get(url, function (data) {
            $(".basket-view").html(data);
            var totalCount = $(".total-count").val()
            $(".cart-count").html(totalCount)
        });
    });
    $(document).on("click", ".add-basket-wishlist", function (e) {
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
                if (response.status !== 2) {
                    let totalCount = document.querySelector(".count-wishlist")
                    totalCount.innerHTML = response.total;
                }
                
            })
    })
})















