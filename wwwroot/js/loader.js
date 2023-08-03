$(document).ready(function () {
    // Show the loader
    $(".loader_bg").show();

    // Hide the loader after 2 seconds
    setTimeout(function () {
        $(".loader_bg").hide();
    }, 2000);
});
