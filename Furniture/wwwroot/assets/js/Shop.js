let labelFor = document.querySelectorAll(".label-for")


labelFor.forEach((label) => {
   
    label.addEventListener("click", () => {
        if (!label.classList.contains("checked")) {
            labelFor.forEach((e) => {
                e.classList.remove("checked")
            })
            label.classList.add("checked")
        }
        
    })
})
let labelForBrand = document.querySelectorAll(".label-brand span")


labelForBrand.forEach((label) => {

    label.addEventListener("click", () => {
        if (!label.classList.contains("brand-color")) {
            labelFor.forEach((e) => {
                e.classList.remove("brand-color")
            })
            label.classList.add("brand-color")
        }

    })
})
$(document).ready(function () {
    var rangeSlider = $("#myRange").ionRangeSlider({
        min: 0,
        max: 1000,
        from: 250,
        to: 750,
        type: "double",
        step: 10,
        grid: true,
    }).data("ionRangeSlider");

    rangeSlider.update({
        onFinish: function (data) {

            $("#minPrice").val(data.from)
            $("#maxPrice").val(data.to)
           
        },
    });
});
