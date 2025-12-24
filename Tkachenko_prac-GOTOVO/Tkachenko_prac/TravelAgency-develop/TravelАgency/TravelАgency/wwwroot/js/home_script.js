// home_script.js - CAROUSEL ENHANCEMENT

document.addEventListener('DOMContentLoaded', function () {

    let currentSlide = 0;
    const cardWrapper = document.querySelector('.card-wrapper');
    const cards = document.querySelectorAll('.card');
    const cardsPerRow = 3;
    const totalSlides = Math.ceil(cards.length / cardsPerRow);

    const leftArrow = document.querySelector('.arrow.left');
    const rightArrow = document.querySelector('.arrow.right');

    // ===== ФУНКЦИИ НАВИГАЦИИ =====
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

    // ===== EVENT LISTENERS =====
    if (rightArrow) {
        rightArrow.addEventListener('click', next);
    }

    if (leftArrow) {
        leftArrow.addEventListener('click', prev);
    }

    // ===== KEYBOARD NAVIGATION =====
    document.addEventListener('keydown', function (e) {
        if (e.key === 'ArrowRight') next();
        if (e.key === 'ArrowLeft') prev();
    });

    // ===== TOUCH SWIPE =====
    let touchStartX = 0;
    let touchEndX = 0;

    if (cardWrapper) {
        cardWrapper.addEventListener('touchstart', function (e) {
            touchStartX = e.changedTouches[0].screenX;
        }, false);

        cardWrapper.addEventListener('touchend', function (e) {
            touchEndX = e.changedTouches[0].screenX;
            handleSwipe();
        }, false);

        function handleSwipe() {
            if (touchStartX - touchEndX > 50) {
                next();
            } else if (touchEndX - touchStartX > 50) {
                prev();
            }
        }
    }

    // ===== AUTO SCROLL (опционально) =====
    // Раскомментируйте для автоматического скролла
    /*
    let autoScrollInterval;

    function startAutoScroll() {
        autoScrollInterval = setInterval(() => {
            if (currentSlide < totalSlides - 1) {
                next();
            } else {
                currentSlide = 0;
                updateCarousel();
            }
        }, 5000);
    }

    function stopAutoScroll() {
        clearInterval(autoScrollInterval);
    }

    if (cardWrapper) {
        startAutoScroll();
        cardWrapper.addEventListener('mouseenter', stopAutoScroll);
        cardWrapper.addEventListener('mouseleave', startAutoScroll);
    }
    */

});