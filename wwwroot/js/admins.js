let sidebar = document.querySelector(".sidebar");
let sidebarBtn = document.querySelector(".sidebarBtn");
let dropdown = document.querySelector('.dropdownad');

sidebarBtn.onclick = function () {
    if (dropdown.classList.contains('active')) {
        dropdown.classList.remove('active');
    }

    sidebar.classList.toggle("active");
    if (sidebar.classList.contains("active")) {
        sidebarBtn.classList.replace("bx-menu", "bx-menu-alt-right");
    } else {
        sidebarBtn.classList.replace("bx-menu-alt-right", "bx-menu");
    }
};

dropdown.addEventListener('click', function () {
    if (sidebar.classList.contains('active')) {
        sidebar.classList.remove('active');
        sidebarBtn.classList.replace("bx-menu-alt-right", "bx-menu");
    }

    dropdown.classList.toggle('active');
});
