// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.





function firecustomtoast(title,text,icon) {
    swal({
        title: title,
        text: text,
        icon: icon,
        animation: true,
        timer: 3000,
        buttons: false,
        toast: true,
        timerProgressBar: true,
        //didOpen: (toast) => {
        //    toast.addEventListener('mouseenter', swal.stopTimer)
        //    toast.addEventListener('mouseleave', swal.resumeTimer)
        //}

    });
}




