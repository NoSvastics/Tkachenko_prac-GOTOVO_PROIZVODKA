// modern-carousel.js
class ModernCarousel {
    constructor(containerId) {
        this.container = document.getElementById(containerId);
        if (!this.container) return;

        this.wrapper = this.container.querySelector('.carousel-wrapper');
        this.items = this.container.querySelectorAll('.carousel-item');
        this.prevBtn = this.container.querySelector('.carousel-arrow.prev');
        this.nextBtn = this.container.querySelector('.carousel-arrow.next');
        this.indicatorsContainer = this.container.querySelector('.carousel-indicators');

        this.currentIndex = 0;
        this.itemsPerView = this.getItemsPerView();
        this.totalSlides = Math.ceil(this.items.length / this.itemsPerView);
        this.isAnimating = false;
        this.autoPlayInterval = null;

        this.init();
    }

    getItemsPerView() {
        const width = window.innerWidth;
        if (width <= 768) return 1;
        if (width <= 1200) return 2;
        return 3;
    }

    init() {
        this.createIndicators();
        this.setupEventListeners();
        this.updateCarousel();
        this.setupResizeHandler();
        this.startAutoPlay();
    }

    createIndicators() {
        if (!this.indicatorsContainer) return;

        this.indicatorsContainer.innerHTML = '';
        for (let i = 0; i < this.totalSlides; i++) {
            const indicator = document.createElement('button');
            indicator.className = `carousel-indicator ${i === 0 ? 'active' : ''}`;
            indicator.setAttribute('aria-label', `Перейти к слайду ${i + 1}`);
            indicator.addEventListener('click', () => this.goToSlide(i));
            this.indicatorsContainer.appendChild(indicator);
        }
    }

    setupEventListeners() {
        if (this.prevBtn) {
            this.prevBtn.addEventListener('click', () => this.prev());
        }

        if (this.nextBtn) {
            this.nextBtn.addEventListener('click', () => this.next());
        }

        // Touch/swipe support
        this.setupTouchEvents();

        // Keyboard navigation
        document.addEventListener('keydown', (e) => {
            if (e.key === 'ArrowLeft') this.prev();
            if (e.key === 'ArrowRight') this.next();
        });

        // Pause autoplay on hover
        this.container.addEventListener('mouseenter', () => this.stopAutoPlay());
        this.container.addEventListener('mouseleave', () => this.startAutoPlay());
    }

    setupTouchEvents() {
        let touchStartX = 0;
        let touchEndX = 0;
        const swipeThreshold = 50;

        this.wrapper.addEventListener('touchstart', (e) => {
            touchStartX = e.changedTouches[0].screenX;
            this.stopAutoPlay();
        }, { passive: true });

        this.wrapper.addEventListener('touchend', (e) => {
            touchEndX = e.changedTouches[0].screenX;
            this.handleSwipe(touchStartX, touchEndX, swipeThreshold);
            this.startAutoPlay();
        }, { passive: true });
    }

    handleSwipe(startX, endX, threshold) {
        const diff = startX - endX;

        if (Math.abs(diff) > threshold) {
            if (diff > 0) {
                this.next();
            } else {
                this.prev();
            }
        }
    }

    setupResizeHandler() {
        let resizeTimeout;
        window.addEventListener('resize', () => {
            clearTimeout(resizeTimeout);
            resizeTimeout = setTimeout(() => {
                const newItemsPerView = this.getItemsPerView();
                if (newItemsPerView !== this.itemsPerView) {
                    this.itemsPerView = newItemsPerView;
                    this.totalSlides = Math.ceil(this.items.length / this.itemsPerView);
                    this.currentIndex = Math.min(this.currentIndex, this.totalSlides - 1);
                    this.createIndicators();
                    this.updateCarousel();
                }
            }, 250);
        });
    }

    next() {
        if (this.isAnimating) return;

        this.isAnimating = true;
        if (this.currentIndex < this.totalSlides - 1) {
            this.currentIndex++;
        } else {
            this.currentIndex = 0; // Loop to start
        }
        this.updateCarousel();

        setTimeout(() => {
            this.isAnimating = false;
        }, 600);
    }

    prev() {
        if (this.isAnimating) return;

        this.isAnimating = true;
        if (this.currentIndex > 0) {
            this.currentIndex--;
        } else {
            this.currentIndex = this.totalSlides - 1; // Loop to end
        }
        this.updateCarousel();

        setTimeout(() => {
            this.isAnimating = false;
        }, 600);
    }

    goToSlide(index) {
        if (this.isAnimating || index === this.currentIndex) return;

        this.isAnimating = true;
        this.currentIndex = index;
        this.updateCarousel();

        setTimeout(() => {
            this.isAnimating = false;
        }, 600);
    }

    updateCarousel() {
        const offset = this.currentIndex * (100 / this.itemsPerView);
        this.wrapper.style.transform = `translateX(-${offset}%)`;

        // Update indicators
        this.updateIndicators();

        // Add animation class
        this.wrapper.classList.add('transitioning');
        setTimeout(() => {
            this.wrapper.classList.remove('transitioning');
        }, 600);
    }

    updateIndicators() {
        const indicators = this.indicatorsContainer?.querySelectorAll('.carousel-indicator');
        indicators?.forEach((indicator, index) => {
            indicator.classList.toggle('active', index === this.currentIndex);
        });
    }

    startAutoPlay() {
        this.stopAutoPlay(); // Clear existing interval
        this.autoPlayInterval = setInterval(() => {
            this.next();
        }, 5000); // Change slide every 5 seconds
    }

    stopAutoPlay() {
        if (this.autoPlayInterval) {
            clearInterval(this.autoPlayInterval);
            this.autoPlayInterval = null;
        }
    }

    destroy() {
        this.stopAutoPlay();
        // Remove event listeners
        if (this.prevBtn) {
            this.prevBtn.removeEventListener('click', () => this.prev());
        }
        if (this.nextBtn) {
            this.nextBtn.removeEventListener('click', () => this.next());
        }
        document.removeEventListener('keydown', (e) => {
            if (e.key === 'ArrowLeft') this.prev();
            if (e.key === 'ArrowRight') this.next();
        });
    }
}

// Инициализация всех каруселей на странице
document.addEventListener('DOMContentLoaded', function () {
    const carousels = document.querySelectorAll('.carousel-container');
    carousels.forEach((container, index) => {
        const id = container.id || `carousel-${index + 1}`;
        container.id = id;
        new ModernCarousel(id);
    });
});