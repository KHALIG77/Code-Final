let removeImg = document.querySelectorAll(".x-icon")
removeImg.forEach((x) => {
    x.addEventListener("click", () => {
        var parent = x.parentElement
        if (!parent.nextElementSibling) {
            parent.remove()
            return;

        }
        parent.nextElementSibling.classList.add("active")
        parent.remove()
        
    })
})
