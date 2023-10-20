let deleteBtn = document.querySelectorAll(".delete-action")
console.log(deleteBtn)

deleteBtn.forEach((btn) => {
    btn.addEventListener("click", (e) => {
        let url = btn.getAttribute("href")
        console.log(url)
        e.preventDefault();
        Swal.fire({
            title: 'Are you sure?',
            text: "You won't be able to revert this!",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes, delete it!'
        }).then((result) => {
            if (result.isConfirmed) {
                fetch(url).then(response => {
                    if (response.ok) {
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                    }
                }).then(() => location.reload())
               
                
               
            }
        })
    })
})

