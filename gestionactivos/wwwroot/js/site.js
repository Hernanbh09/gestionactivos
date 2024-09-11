// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

let table = new DataTable('#usuariosTable', {
    pageLength: 5
});
let table2 = new DataTable('#funcionarioTable', {
    pageLength: 5, // Puedes cambiar la configuración aquí también
    responsive: true // Por ejemplo, para hacerla responsive
});