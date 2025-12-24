document.addEventListener('DOMContentLoaded', function () {
    function toggleDisplaMenuUser(element) {
        if (element.style.display === "none") {
            element.style.display = "flex";
        } else {
            element.style.display = "none";
        }
    }
    const click_to_user_conteiner = document.getElementById("click-to-user-conteiner");
    const overlay_user_conteiner = document.querySelector(".click-to-user-conteiner");

    if (click_to_user_conteiner) {
        click_to_user_conteiner.addEventListener("click", () => toggleDisplaMenuUser(document.querySelector(".wrapper-user-menu")));
    }
    if (overlay_user_conteiner) {
        overlay_user_conteiner.addEventListener("click", () => toggleDisplaMenuUser(document.querySelector(".wrapper-user-menu")));
    }
});