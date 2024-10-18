// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let table = new DataTable('#usuariosTable', {
    pageLength: 5,
    responsive: true,
    autoWidth: true
});
let table2 = new DataTable('#funcionarioTable', {
    pageLength: 5, // Puedes cambiar la configuración aquí también
    responsive: true,
    autoWidth: true // Por ejemplo, para hacerla responsive
});

let table3 = new DataTable('#MovimientosTable', {
    pageLength: 5, // Puedes cambiar la configuración aquí también
    responsive: true,
    autoWidth: true // Por ejemplo, para hacerla responsive
});
let table4 = new DataTable('#ArticulosTable', {
    pageLength: 5, // Puedes cambiar la configuración aquí también
    responsive: true,
    autoWidth: true // Por ejemplo, para hacerla responsive
});
let table5 = new DataTable('#AdicionalesTable', {
    pageLength: 5, // Puedes cambiar la configuración aquí también
    responsive: true,
    autoWidth: true // Por ejemplo, para hacerla responsive
});



let table6 = new DataTable('#clientesTable', {
    pageLength: 5, // Puedes cambiar la configuración aquí también
    responsive: true,
    autoWidth: true // Por ejemplo, para hacerla responsive
});

window.addEventListener('scroll', function () {
    const navTop = document.querySelector('.nav-top');
    const navBottom = document.querySelector('.nav-bottom');

    // Detecta la cantidad de desplazamiento
    if (window.pageYOffset > 50) {
        navTop.classList.add('minimized');
        navBottom.classList.add('fixed');
    } else {
        navTop.classList.remove('minimized');
        navBottom.classList.remove('fixed');
    }
});

let botonSubir = document.getElementById("subirArriba");

window.onscroll = function () {
    if (document.body.scrollTop > 100 || document.documentElement.scrollTop > 100) {
        botonSubir.style.display = "block";
    } else {
        botonSubir.style.display = "none";
    }
};

botonSubir.onclick = function () {
    document.body.scrollTop = 0;
    document.documentElement.scrollTop = 0;
};