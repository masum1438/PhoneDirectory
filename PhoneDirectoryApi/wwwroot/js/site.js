// Confirm before delete
$(document).ready(function () {
    $('.delete-btn').click(function (e) {
        if (!confirm('Are you sure you want to delete this item?')) {
            e.preventDefault();
        }
    });

    // Toast notification
    if ($('.alert').length) {
        setTimeout(function () {
            $('.alert').fadeOut('slow');
        }, 5000);
    }

    // DataTable initialization
    $('.table').DataTable({
        responsive: true
    });
});