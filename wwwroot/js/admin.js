let sidebar = document.querySelector(".sidebar");
let sidebarBtn = document.querySelector(".sidebarBtn");
const dropdown = document.querySelector('.dropdownad');

sidebarBtn.onclick = function () {
    sidebar.classList.toggle("active");
    if (sidebar.classList.contains("active")) {
        sidebarBtn.classList.replace("bx-menu", "bx-menu-alt-right");
    } else {
        sidebarBtn.classList.replace("bx-menu-alt-right", "bx-menu");
    }
};

dropdown.onclick = function (event) {
    event.stopPropagation(); // Prevent the click event from reaching the document nex

    if (!sidebar.classList.contains("active")) {
        sidebar.classList.add("active");
        sidebarBtn.classList.replace("bx-menu", "bx-menu-alt-right");
    }

    dropdown.classList.toggle('active');
};

document.addEventListener('DOMContentLoaded', function () {
    dropdown.addEventListener('click', function (event) {
        event.stopPropagation(); // Prevent the click event from reaching the document

        if (!sidebar.classList.contains("active")) {
            sidebar.classList.add("active");
            sidebarBtn.classList.replace("bx-menu", "bx-menu-alt-right");
        }

        dropdown.classList.toggle('active');
    });

    document.addEventListener('click', function () {
        dropdown.classList.remove('active'); // Close the dropdown when clicking anywhere in the document

        if (sidebar.classList.contains("active")) {
            sidebar.classList.remove("active");
            sidebarBtn.classList.replace("bx-menu-alt-right", "bx-menu");
        }
    });
});
