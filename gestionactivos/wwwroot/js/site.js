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

let table3 = new DataTable('#MovimientosTable', {
    pageLength: 5, // Puedes cambiar la configuración aquí también
    responsive: true // Por ejemplo, para hacerla responsive
});
let table4 = new DataTable('#ArticulosTable', {
    pageLength: 5, // Puedes cambiar la configuración aquí también
    responsive: true // Por ejemplo, para hacerla responsive
});
let table5 = new DataTable('#AdicionalesTable', {
    pageLength: 5, // Puedes cambiar la configuración aquí también
    responsive: true // Por ejemplo, para hacerla responsive
});