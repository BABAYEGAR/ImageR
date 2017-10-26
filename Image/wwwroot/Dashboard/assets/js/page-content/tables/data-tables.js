$(document).ready(function() {
    $('#example-1').dataTable();

    var table = $('#example-2').DataTable({
        lengthChange: false,
        buttons: ['copy', 'excel', 'pdf', 'print']
    });

    table.buttons().container().appendTo('#example-2_wrapper .col-sm-6:eq(0)');

    var table1 = $('#example-2a').DataTable({
        lengthChange: false,
        buttons: ['copy', 'excel', 'pdf', 'print']
    });

    table1.buttons().container().appendTo('#example-2a_wrapper .col-sm-6:eq(0)');

    var table2 = $('#example-2b').DataTable({
        lengthChange: false,
        buttons: ['copy', 'excel', 'pdf', 'print']
    });

    table1.buttons().container().appendTo('#example-2b_wrapper .col-sm-6:eq(0)');

    $('#example-3').DataTable({
        colReorder: true
    });

    $('#example-5').DataTable({
        keys: true
    });

    $('#example-6').DataTable({
        select: {
            style: 'os'
        }
    });

    $('#example-7').DataTable();

    $('#example-8').DataTable({
        scrollX: true,
        scrollCollapse: true,
        fixedColumns: true
    });

    $('#example-9').DataTable({
        fixedHeader: true
    });
});