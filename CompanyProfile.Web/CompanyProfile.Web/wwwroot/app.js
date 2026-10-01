window.companyProfile = {
    scrollSlider: function (element, direction) {
        if (!element) return;
        element.scrollBy({ left: direction * element.clientWidth * 0.82, behavior: "smooth" });
    },
    startSliders: function () {
        document.querySelectorAll("[data-autoplay='true']").forEach(function (element) {
            if (element.dataset.sliderStarted === "true") return;
            element.dataset.sliderStarted = "true";

            var paused = false;
            var direction = element.dataset.direction === "rtl" ? -1 : 1;
            var move = function () {
                if (paused || element.scrollWidth <= element.clientWidth) return;

                var amount = element.clientWidth * 0.82;
                var atEnd = direction > 0
                    ? element.scrollLeft + element.clientWidth >= element.scrollWidth - 4
                    : Math.abs(element.scrollLeft) >= element.scrollWidth - element.clientWidth - 4;

                if (atEnd) {
                    element.scrollTo({ left: direction > 0 ? 0 : element.scrollWidth, behavior: "smooth" });
                } else {
                    element.scrollBy({ left: direction * amount, behavior: "smooth" });
                }
            };

            window.setInterval(move, 4200);
            element.addEventListener("mouseenter", function () { paused = true; });
            element.addEventListener("mouseleave", function () { paused = false; });
            element.addEventListener("focusin", function () { paused = true; });
            element.addEventListener("focusout", function () { paused = false; });
            element.addEventListener("touchstart", function () { paused = true; }, { passive: true });
            element.addEventListener("touchend", function () { paused = false; });
        });
    }
};