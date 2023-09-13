let addBasket = document.querySelectorAll(".add-basket")
console.log(addBasket)

addBasket.forEach((btn) => {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        let url = btn.getAttribute("href")
        fetch(url)
        
        
    })
})