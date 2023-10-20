let showBtns = document.querySelectorAll(".show-reply")
showBtns.forEach((btn) => {
    btn.addEventListener("click", () => {
        if (btn.parentElement.nextElementSibling.classList.contains("d-none")) {
            btn.parentElement.nextElementSibling.classList.remove("d-none")
            btn.innerHTML = "Close Reply"
        }
        else {
            btn.parentElement.nextElementSibling.classList.add("d-none")
            btn.innerHTML = "Show Reply"
        }
    })
})
