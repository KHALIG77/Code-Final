let quickBtns = document.querySelectorAll(".modal-btn-quick")

quickBtns.forEach((btn) => {
    btn.addEventListener("click", (e) => {
        e.preventDefault();
        let url = btn.getAttribute("href");
        fetch(url)
            .then(response => response.text())
            .then(data => {
                
                let modalContainer = document.querySelector(".quickview-container")
                modalContainer.innerHTML=data
                
                


            })
    })
})