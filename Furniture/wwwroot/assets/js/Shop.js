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
            labelForBrand.forEach((e) => {
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

let labelMaterial = document.querySelectorAll(".label-material")


labelMaterial.forEach((label) => {

    label.addEventListener("click", () => {
        if (!label.classList.contains("checked")) {
            labelMaterial.forEach((e) => {
                e.classList.remove("checked")
            })
            label.classList.add("checked")
        }

    })
})

let labelTags = document.querySelectorAll(".label-tags")


labelTags.forEach((label) => {

    label.addEventListener("click", () => {
        if (!label.classList.contains("checked")) {

            label.classList.add("checked")
        }
        else {
            label.classList.remove("checked")
        }

    })
})


let labelSizes = document.querySelectorAll(".label-size")


labelSizes.forEach((label) => {

    label.addEventListener("click", () => {
        if (!label.classList.contains("size-color")) {

            label.classList.add("size-color")
        }
        else {
            label.classList.remove("size-color")
        }

    })
})