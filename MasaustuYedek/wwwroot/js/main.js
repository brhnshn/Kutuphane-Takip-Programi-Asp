// Sayfa yüklendiğinde çalışacak kodlar
document.addEventListener("DOMContentLoaded", () => {
    // Dark mode toggle
    const themeToggle = document.getElementById("theme-toggle")
    const themeIcon = document.getElementById("theme-icon")
    const body = document.body

    // Check for saved theme preference or default to light mode
    const savedTheme = localStorage.getItem("theme") || "dark"
    body.setAttribute("data-theme", savedTheme)
    updateThemeIcon(savedTheme)

    themeToggle.addEventListener("click", () => {
        const currentTheme = body.getAttribute("data-theme")
        const newTheme = currentTheme === "dark" ? "light" : "dark"

        body.setAttribute("data-theme", newTheme)
        localStorage.setItem("theme", newTheme)
        updateThemeIcon(newTheme)
    })

    function updateThemeIcon(theme) {
        if (theme === "dark") {
            themeIcon.className = "fas fa-sun"
        } else {
            themeIcon.className = "fas fa-moon"
        }
    }

    // Mobil menü toggle
    const navToggle = document.querySelector(".nav-toggle")
    const navMenu = document.querySelector(".nav-menu")

    if (navToggle) {
        navToggle.addEventListener("click", () => {
            navMenu.classList.toggle("active")
            navToggle.classList.toggle("active")
        })

        // Close mobile menu when clicking on a link
        const navLinks = document.querySelectorAll(".nav-link")
        navLinks.forEach((link) => {
            link.addEventListener("click", () => {
                navMenu.classList.remove("active")
                navToggle.classList.remove("active")
            })
        })
        // Close mobile menu when clicking outside
        document.addEventListener("click", (e) => {
            if (!navToggle.contains(e.target) && !navMenu.contains(e.target)) {
                navMenu.classList.remove("active")
                navToggle.classList.remove("active")
            }
        })
    }

    // Scroll olduğunda header'ı küçült
    const header = document.querySelector(".header")
    window.addEventListener("scroll", () => {
        if (window.scrollY > 100) {
            header.classList.add("scrolled")
        } else {
            header.classList.remove("scrolled")
        }
    })

    // Sayfa içi linkler için smooth scroll
    const anchorLinks = document.querySelectorAll('a[href^="#"]')
    anchorLinks.forEach((link) => {
        link.addEventListener("click", function (e) {
            e.preventDefault()
            const targetId = this.getAttribute("href")
            const targetElement = document.querySelector(targetId)

            if (targetElement) {
                const headerHeight = header.offsetHeight
                const targetPosition = targetElement.offsetTop - headerHeight

                window.scrollTo({
                    top: targetPosition,
                    behavior: "smooth",
                })
            }
        })
    })

    // Active nav link on scroll
    const sections = document.querySelectorAll("section[id]")
    const navLinks = document.querySelectorAll(".nav-link")

    function updateActiveNavLink() {
        const scrollPosition = window.scrollY + 150

        sections.forEach((section) => {
            const sectionTop = section.offsetTop
            const sectionHeight = section.offsetHeight
            const sectionId = section.getAttribute("id")

            if (scrollPosition >= sectionTop && scrollPosition < sectionTop + sectionHeight) {
                navLinks.forEach((link) => {
                    link.classList.remove("active")
                    if (link.getAttribute("href") === `#${sectionId}`) {
                        link.classList.add("active")
                    }
                })
            }
        })
    }

    window.addEventListener("scroll", updateActiveNavLink)

    // Back to top button
    const backToTopButton = document.getElementById("back-to-top")

    if (backToTopButton) {
        window.addEventListener("scroll", () => {
            if (window.scrollY > 300) {
                backToTopButton.classList.add("visible")
            } else {
                backToTopButton.classList.remove("visible")
            }
        })

        backToTopButton.addEventListener("click", () => {
            window.scrollTo({
                top: 0,
                behavior: "smooth",
            })
        })
    }

    // Contact form submission (Formspree) — başarıda alert gösterir
    const contactForm = document.getElementById("contact-form");

    if (contactForm) {
        contactForm.addEventListener("submit", async function (e) {
            e.preventDefault();

            // statusEl yoksa oluştur
            let statusEl = document.getElementById("contact-status");
            if (!statusEl) {
                statusEl = document.createElement("div");
                statusEl.id = "contact-status";
                statusEl.setAttribute("aria-live", "polite");
                const actions = contactForm.querySelector(".form-actions");
                if (actions) actions.appendChild(statusEl);
                else contactForm.appendChild(statusEl);
            }

            const submitBtn = document.getElementById("contact-submit");
            const formData = new FormData(this);

            // Honeypot kontrolü
            if (formData.get("_hp")) {
                statusEl.textContent = "Spam tespit edildi.";
                statusEl.classList.remove("success", "error");
                statusEl.classList.add("error");
                return;
            }

            // Basit alan kontrolü
            const name = formData.get("name")?.toString().trim();
            const email = formData.get("email")?.toString().trim();
            const message = formData.get("message")?.toString().trim();
            if (!name || !email || !message) {
                statusEl.textContent = "Lütfen isim, e-posta ve mesaj alanlarını doldurun.";
                statusEl.classList.remove("success", "error");
                statusEl.classList.add("error");
                return;
            }

            const originalBtnHTML = submitBtn ? submitBtn.innerHTML : null;
            if (submitBtn) {
                submitBtn.disabled = true;
                submitBtn.innerHTML = "Gönderiliyor...";
            }

            try {
                const response = await fetch(contactForm.action, {
                    method: contactForm.method || "POST",
                    headers: { Accept: "application/json" },
                    body: formData,
                });

                if (response.ok) {
                    // Başarı: alert göster, formu resetle
                    alert("Mesaj başarıyla gönderildi — teşekkürler!");
                    this.reset();

                    // isteğe bağlı: statusEl'de kısa bilgi göster (ve sonra temizle)
                    statusEl.textContent = "Mesaj gönderildi.";
                    statusEl.classList.remove("error");
                    statusEl.classList.add("success");
                    setTimeout(() => {
                        if (statusEl) {
                            statusEl.textContent = "";
                            statusEl.classList.remove("success");
                        }
                    }, 3000);
                } else {
                    const data = await response.json().catch(() => null);
                    const errMsg = (data && data.error) ? `Hata: ${data.error}` : "Gönderme sırasında hata oluştu.";
                    statusEl.textContent = errMsg;
                    statusEl.classList.remove("success");
                    statusEl.classList.add("error");
                }
            } catch (err) {
                console.error("Form gönderim hatası:", err);
                statusEl.textContent = "Ağ hatası: mesaj gönderilemedi.";
                statusEl.classList.remove("success");
                statusEl.classList.add("error");
            } finally {
                if (submitBtn) {
                    submitBtn.disabled = false;
                    if (originalBtnHTML) submitBtn.innerHTML = originalBtnHTML;
                }
            }
        });
    }


    // Scroll animations
    const observerOptions = {
        threshold: 0.1,
        rootMargin: "0px 0px -50px 0px",
    }

    const observer = new IntersectionObserver((entries) => {
        entries.forEach((entry) => {
            if (entry.isIntersecting) {
                entry.target.classList.add("animate")
            }
        })
    }, observerOptions)

    // Add scroll animation to elements
    const animateElements = document.querySelectorAll(".skill-card, .tech-card, .project-card, .contact-card, .main-card")
    animateElements.forEach((el) => {
        el.classList.add("scroll-animate")
        observer.observe(el)
    })

    // Typing animation for code window
    const codeLines = document.querySelectorAll(".code-line")
    codeLines.forEach((line, index) => {
        line.style.opacity = "0"
        line.style.transform = "translateX(-20px)"

        setTimeout(
            () => {
                line.style.transition = "all 0.5s ease"
                line.style.opacity = "1"
                line.style.transform = "translateX(0)"
            },
            1000 + index * 200,
        )
    })

    // Parallax effect for hero background
    window.addEventListener("scroll", () => {
        const scrolled = window.pageYOffset
        const heroPattern = document.querySelector(".hero-pattern")

        if (heroPattern) {
            heroPattern.style.transform = `translateY(${scrolled * 0.5}px)`
        }
    })
})
