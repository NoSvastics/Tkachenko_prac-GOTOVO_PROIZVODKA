document.addEventListener('DOMContentLoaded', function () {

    // === ГАМБУРГЕР МЕНЮ ===
    const hamburger = document.getElementById('hamburger');
    const sideMenu = document.getElementById('side-menu');

    if (hamburger) {
        hamburger.addEventListener('click', (e) => {
            e.stopPropagation();
            sideMenu.classList.toggle('active');

            const spans = hamburger.querySelectorAll('span');
            if (sideMenu.classList.contains('active')) {
                spans[0].style.transform = 'rotate(45deg) translate(5px, 5px)';
                spans[1].style.opacity = '0';
                spans[2].style.transform = 'rotate(-45deg) translate(5px, -5px)';
            } else {
                spans[0].style.transform = 'none';
                spans[1].style.opacity = '1';
                spans[2].style.transform = 'none';
            }
        });

        document.addEventListener('click', (e) => {
            if (!sideMenu.contains(e.target) && !hamburger.contains(e.target)) {
                sideMenu.classList.remove('active');
                const spans = hamburger.querySelectorAll('span');
                spans[0].style.transform = 'none';
                spans[1].style.opacity = '1';
                spans[2].style.transform = 'none';
            }
        });
    }

    // === МОДАЛЬНОЕ ОКНО ===
    const modal = document.getElementById('authModal');
    const formBox = document.querySelector('.form-box');
    const block = document.querySelector('.block');
    const signinForm = document.querySelector('.form_signin');
    const signupForm = document.querySelector('.form_signup');

    // Функция открытия
    window.openModal = function () {
        if (modal) {
            modal.style.display = 'flex';
            document.body.style.overflow = 'hidden';
            // Сброс на мобильных при открытии (по умолчанию Вход)
            if (window.innerWidth <= 768) {
                activateMobileTab('signin');
            }
        }
    };

    if (modal) {
        modal.addEventListener('click', (e) => {
            if (e.target === modal) {
                modal.style.display = 'none';
                document.body.style.overflow = '';
            }
        });

        // --- Логика для ПК (Слайдер) ---
        const pcSignInBtns = document.querySelectorAll('.signin-btn');
        const pcSignUpBtns = document.querySelectorAll('.signup-btn');

        pcSignUpBtns.forEach(btn => {
            btn.addEventListener('click', () => {
                formBox.classList.add('active');
                if (block) block.classList.add('active');
            });
        });

        pcSignInBtns.forEach(btn => {
            btn.addEventListener('click', () => {
                formBox.classList.remove('active');
                if (block) block.classList.remove('active');
            });
        });

        // --- Логика для МОБИЛЬНЫХ (Табы) ---
        const tabSignin = document.getElementById('tab-signin');
        const tabSignup = document.getElementById('tab-signup');

        function activateMobileTab(type) {
            // Убираем активные классы везде
            tabSignin.classList.remove('active');
            tabSignup.classList.remove('active');
            signinForm.classList.remove('show');
            signupForm.classList.remove('show');

            if (type === 'signin') {
                tabSignin.classList.add('active');
                signinForm.classList.add('show');
            } else {
                tabSignup.classList.add('active');
                signupForm.classList.add('show');
            }
        }

        if (tabSignin && tabSignup) {
            tabSignin.addEventListener('click', () => activateMobileTab('signin'));
            tabSignup.addEventListener('click', () => activateMobileTab('signup'));
        }
    }
});