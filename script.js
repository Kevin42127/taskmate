document.addEventListener('contextmenu', function(e) { e.preventDefault(); });

document.addEventListener('DOMContentLoaded', function() {
    const downloadBtn = document.querySelector('.download-btn');
    const navLinks = document.querySelectorAll('.nav-link');
    const menuToggle = document.querySelector('.menu-toggle');
    const nav = document.querySelector('.nav');

    const carousel = document.querySelector('.hero-preview.carousel');
    if (carousel) {
        const track = carousel.querySelector('.carousel-track');
        const inner = carousel.querySelector('.carousel-inner');
        const prevBtn = carousel.querySelector('.carousel-prev');
        const nextBtn = carousel.querySelector('.carousel-next');
        const dots = carousel.querySelectorAll('.carousel-dot');
        const total = dots.length;
        var currentIndex = 0;

        function goTo(i) {
            if (i < 0 || i >= total) return;
            currentIndex = i;
            track.style.transform = 'translateX(-' + i * 100 + '%)';
            dots.forEach(function(d, j) { d.classList.toggle('active', j === i); });
        }

        if (prevBtn) prevBtn.addEventListener('click', function() { goTo((currentIndex - 1 + total) % total); });
        if (nextBtn) nextBtn.addEventListener('click', function() { goTo((currentIndex + 1) % total); });
        dots.forEach(function(dot, i) { dot.addEventListener('click', function() { goTo(i); }); });

        if (inner) {
            var startX = 0, startY = 0, isHorizontal = null;
            inner.addEventListener('touchstart', function(e) {
                startX = e.touches[0].clientX;
                startY = e.touches[0].clientY;
                isHorizontal = null;
            }, { passive: true });
            inner.addEventListener('touchmove', function(e) {
                if (isHorizontal === null) {
                    var dx = e.touches[0].clientX - startX, dy = e.touches[0].clientY - startY;
                    if (Math.abs(dx) > 10 || Math.abs(dy) > 10) isHorizontal = Math.abs(dx) > Math.abs(dy);
                }
                if (isHorizontal === true) e.preventDefault();
            }, { passive: false });
            inner.addEventListener('touchend', function(e) {
                var dx = e.changedTouches[0].clientX - startX;
                if (isHorizontal === true && Math.abs(dx) > 50) {
                    e.preventDefault();
                    if (dx < 0) goTo((currentIndex + 1) % total);
                    else goTo((currentIndex - 1 + total) % total);
                }
            }, { passive: false });
        }
    }

    if (menuToggle && nav) {
        menuToggle.addEventListener('click', function() {
            nav.classList.toggle('active');
        });

        navLinks.forEach(link => {
            link.addEventListener('click', function() {
                if (window.innerWidth <= 768) {
                    nav.classList.remove('active');
                }
            });
        });

        document.addEventListener('click', function(e) {
            if (window.innerWidth <= 768 && 
                !nav.contains(e.target) && 
                !menuToggle.contains(e.target)) {
                nav.classList.remove('active');
            }
        });
    }

    navLinks.forEach(link => {
        link.addEventListener('click', function(e) {
            e.preventDefault();
            const targetId = this.getAttribute('href');
            if (targetId.startsWith('#')) {
                const targetElement = document.querySelector(targetId);
                if (targetElement) {
                    targetElement.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }
        });
    });

    if (downloadBtn) {
        downloadBtn.addEventListener('click', function(e) {
            const fileExists = this.getAttribute('href');
            if (!fileExists || fileExists === '#') {
                e.preventDefault();
                alert('安裝檔尚未準備完成，請稍後再試。');
            }
            
            createRipple(e, this);
        });
    }

    function createRipple(event, element) {
        const ripple = document.createElement('span');
        const rect = element.getBoundingClientRect();
        const size = Math.max(rect.width, rect.height);
        const x = event.clientX - rect.left - size / 2;
        const y = event.clientY - rect.top - size / 2;
        
        ripple.classList.add('ripple');
        ripple.style.width = ripple.style.height = size + 'px';
        ripple.style.left = x + 'px';
        ripple.style.top = y + 'px';
        ripple.style.position = 'absolute';
        ripple.style.margin = '0';
        ripple.style.padding = '0';
        
        element.appendChild(ripple);
        
        setTimeout(() => {
            if (ripple.parentNode) {
                ripple.remove();
            }
        }, 600);
    }

    const observerOptions = {
        threshold: 0.1,
        rootMargin: '0px 0px -100px 0px'
    };

    const observer = new IntersectionObserver(function(entries) {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.style.opacity = '1';
                entry.target.style.transform = 'translateY(0)';
            }
        });
    }, observerOptions);

    const animatedElements = document.querySelectorAll('.feature-card, .step');
    animatedElements.forEach(el => {
        el.style.opacity = '0';
        el.style.transform = 'translateY(20px)';
        el.style.transition = 'opacity 0.6s ease, transform 0.6s ease';
        observer.observe(el);
    });
});
