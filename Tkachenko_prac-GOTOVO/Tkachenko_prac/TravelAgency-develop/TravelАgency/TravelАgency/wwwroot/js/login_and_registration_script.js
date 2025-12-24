document.addEventListener('DOMContentLoaded', function () {

    // ===== ФУНКЦИИ МОДАЛЬНЫХ ОКОН =====
    function hiddenOpen_Closeclick(conteiner) {
        let x = document.querySelector(conteiner);
        if (x.style.display == "none" || x.style.display == "") {
            x.style.display = "grid";
            document.body.style.overflow = "hidden";
        } else {
            x.style.display = "none";
            document.body.style.overflow = "auto";
        }
    }

    // Обработчики модальных окон
    const hfhd = document.getElementById("click-to-hide");
    if (hfhd) {
        hfhd.addEventListener("click", function () {
            hiddenOpen_Closeclick(".container-login-registration");
        });
    }

    const sideMenuBtn = document.getElementById("side-menu-button-click-to-hide");
    if (sideMenuBtn) {
        sideMenuBtn.addEventListener("click", function () {
            hiddenOpen_Closeclick(".container-login-registration");
        });
    }

    const overlays = document.querySelectorAll(".overlay");
    overlays.forEach(overlay => {
        overlay.addEventListener("click", function () {
            const container = overlay.closest('.container-login-registration, .confirm-email-container');
            if (container) {
                container.style.display = "none";
                document.body.style.overflow = "auto";
            }
        });
    });

    const confirmClose = document.querySelector(".button_confirm_close");
    if (confirmClose) {
        confirmClose.addEventListener("click", function () {
            hiddenOpen_Closeclick(".confirm-email-container");
        });
    }

    // ===== ПЕРЕКЛЮЧЕНИЕ МЕЖДУ ВХОДОМ И РЕГИСТРАЦИЕЙ =====
    const signInBtn = document.querySelector('.signin-btn');
    const signUpBtn = document.querySelector('.signup-btn');
    const formBox = document.querySelector('.form-box');
    const block = document.querySelector('.block');

    if (signInBtn && signUpBtn) {
        signUpBtn.addEventListener('click', function () {
            formBox.classList.add('active');
            block.classList.add('active');
        });

        signInBtn.addEventListener('click', function () {
            formBox.classList.remove('active');
            block.classList.remove('active');
        });
    }

    // ===== ОБРАБОТКА ВХОДА =====
    const form_btn_signin = document.querySelector('.form_btn_signin');

    if (form_btn_signin) {
        form_btn_signin.addEventListener('click', function (e) {
            e.preventDefault();

            const requestURL = '/Home/Login';
            const errorContainer = document.getElementById('error-messages-singin');

            const form = {
                email: document.getElementById("signin_email"),
                password: document.getElementById("signin_password")
            };

            if (!form.email.value || !form.password.value) {
                displayErrors(['Пожалуйста, заполните все поля'], errorContainer);
                return;
            }

            const body = {
                email: form.email.value,
                password: form.password.value
            };

            showLoading();

            sendRequest('POST', requestURL, body)
                .then(data => {
                    hideLoading();
                    cleaningAndClosingForm(form, errorContainer);
                    location.reload();
                })
                .catch(err => {
                    hideLoading();
                    displayErrors(Array.isArray(err) ? err : [err.message || 'Ошибка входа'], errorContainer);
                });
        });
    }

    // ===== ОБРАБОТКА РЕГИСТРАЦИИ =====
    const form_btn_signup = document.querySelector('.form_btn_signup');

    if (form_btn_signup) {
        form_btn_signup.addEventListener('click', function (e) {
            e.preventDefault();

            const requestURL = '/Home/Register';
            const errorContainer = document.getElementById('error-messages-singup');

            const form = {
                login: document.getElementById("signup_login"),
                email: document.getElementById("signup_email"),
                password: document.getElementById("signup_password"),
                passwordConfirm: document.getElementById("signup_confirm_password"),
            };

            if (!form.login.value || !form.email.value || !form.password.value || !form.passwordConfirm.value) {
                displayErrors(['Пожалуйста, заполните все поля'], errorContainer);
                return;
            }

            if (form.password.value !== form.passwordConfirm.value) {
                displayErrors(['Пароли не совпадают'], errorContainer);
                return;
            }

            if (form.password.value.length < 6) {
                displayErrors(['Пароль должен быть минимум 6 символов'], errorContainer);
                return;
            }

            const body = {
                login: form.login.value,
                email: form.email.value,
                password: form.password.value,
                passwordConfirm: form.passwordConfirm.value,
            };

            showLoading();

            sendRequest('POST', requestURL, body)
                .then(data => {
                    hideLoading();
                    cleaningAndClosingForm(form, errorContainer);
                    hiddenOpen_Closeclick(".confirm-email-container");
                    confirmEmail(data);
                })
                .catch(err => {
                    hideLoading();
                    displayErrors(Array.isArray(err) ? err : [err.message || 'Ошибка регистрации'], errorContainer);
                });
        });
    }

    // ===== ПОДТВЕРЖДЕНИЕ EMAIL =====
    function confirmEmail(body) {
        const confirmBtn = document.querySelector(".send_confirm");
        if (!confirmBtn) return;

        const handler = function () {
            const code = document.getElementById('code_confirm').value;

            if (!code) {
                displayErrors(['Пожалуйста, введите код подтверждения'], document.getElementById('error-messages-singup'));
                return;
            }

            body.codeConfirm = code;
            const requestURL = '/Home/ConfirmEmail';

            showLoading();

            sendRequest('POST', requestURL, body)
                .then(data => {
                    hideLoading();
                    hiddenOpen_Closeclick(".confirm-email-container");
                    location.reload();
                })
                .catch(err => {
                    hideLoading();
                    displayErrors(Array.isArray(err) ? err : [err.message || 'Ошибка подтверждения'], document.getElementById('error-messages-singup'));
                });
        };

        confirmBtn.removeEventListener('click', handler);
        confirmBtn.addEventListener('click', handler);
    }

    // ===== FETCH ЗАПРОСЫ =====
    function sendRequest(method, url, body = null) {
        const headers = {
            'Content-Type': 'application/json'
        };

        return fetch(url, {
            method: method,
            body: JSON.stringify(body),
            headers: headers
        }).then(response => {
            if (!response.ok) {
                return response.json().then(errorData => {
                    throw errorData;
                });
            }
            return response.json();
        });
    }

    // ===== ОТОБРАЖЕНИЕ ОШИБОК =====
    function displayErrors(errors, errorContainer) {
        if (!errorContainer) return;

        errorContainer.innerHTML = '';
        const errorsArray = Array.isArray(errors) ? errors : [errors];

        errorsArray.forEach(error => {
            const errorMessage = document.createElement('div');
            errorMessage.classList.add('error');
            errorMessage.textContent = typeof error === 'string' ? error : (error.message || 'Неизвестная ошибка');
            errorContainer.appendChild(errorMessage);
        });
    }

    // ===== ОЧИСТКА И ЗАКРЫТИЕ ФОРМЫ =====
    function cleaningAndClosingForm(form, errorContainer) {
        if (errorContainer) {
            errorContainer.innerHTML = '';
        }

        for (const key in form) {
            if (form.hasOwnProperty(key) && form[key].value !== undefined) {
                form[key].value = '';
            }
        }

        hiddenOpen_Closeclick(".container-login-registration");
    }

    // ===== GOOGLE АВТОРИЗАЦИЯ =====
    const googleBtns = document.querySelectorAll('.google');

    googleBtns.forEach(btn => {
        btn.addEventListener('click', function (e) {
            e.preventDefault();
            showLoading();
            window.location.href = `/Home/AuthenticationGoogle?returnUrl=${encodeURIComponent(window.location.href)}`;
        });
    });

    // ===== LOADING OVERLAY =====
    function hideLoading() {
        const overlay = document.getElementById("loading-overlay");
        if (overlay) {
            overlay.style.display = "none";
        }
    }

    function showLoading() {
        const overlay = document.getElementById("loading-overlay");
        if (overlay) {
            overlay.style.display = "flex";
        }
    }

    // ===== БАНЕР КНОПКА =====
    const bannerBtn = document.getElementById('banner-login-btn');
    if (bannerBtn) {
        bannerBtn.addEventListener('click', function () {
            hiddenOpen_Closeclick(".container-login-registration");
        });
    }
});

// ========== ФАЙЛ 3: home_script.js ==========
document.addEventListener('DOMContentLoaded', function () {

    let currentSlide = 0;
    const cardWrapper = document.querySelector('.card-wrapper');
    const cards = document.querySelectorAll('.card');
    const cardsPerRow = 3;
    const totalSlides = Math.ceil(cards.length / cardsPerRow);

    const leftArrow = document.querySelector('.arrow.left');
    const rightArrow = document.querySelector('.arrow.right');

    function updateCarousel() {
        if (cardWrapper) {
            const offset = currentSlide * 100;
            cardWrapper.style.transform = `translateX(-${offset}%)`;
        }
    }

    function next() {
        if (currentSlide < totalSlides - 1) {
            currentSlide++;
            updateCarousel();
        }
    }

    function prev() {
        if (currentSlide > 0) {
            currentSlide--;
            updateCarousel();
        }
    }

    if (rightArrow) {
        rightArrow.addEventListener('click', next);
    }

    if (leftArrow) {
        leftArrow.addEventListener('click', prev);
    }

    document.addEventListener('keydown', function (e) {
        if (e.key === 'ArrowRight') next();
        if (e.key === 'ArrowLeft') prev();
    });

    let touchStartX = 0;
    let touchEndX = 0;

    if (cardWrapper) {
        cardWrapper.addEventListener('touchstart', function (e) {
            touchStartX = e.changedTouches[0].screenX;
        }, false);

        cardWrapper.addEventListener('touchend', function (e) {
            touchEndX = e.changedTouches[0].screenX;
            if (touchStartX - touchEndX > 50) {
                next();
            } else if (touchEndX - touchStartX > 50) {
                prev();
            }
        }, false);
    }
});