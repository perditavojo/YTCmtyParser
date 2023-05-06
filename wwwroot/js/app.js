"use strict"

document.addEventListener("DOMContentLoaded", () => {
    // 延後執行，以利事件註冊成功。
    setTimeout(() => {
        const elementBtnScrollToTop = document.getElementById("btnScrollToTop");
        const elementBtnScrollToBottom = document.getElementById("btnScrollToBottom");

        if (elementBtnScrollToTop !== undefined &&
            elementBtnScrollToTop !== null &&
            elementBtnScrollToBottom !== undefined &&
            elementBtnScrollToBottom !== null) {
            window.addEventListener("scroll", () => {
                if (window.scrollY > 50) {
                    elementBtnScrollToTop.style.display = "block";

                    const valueHeight = Math.ceil(window.pageYOffset + window.innerHeight);

                    if (document.body.scrollHeight <= valueHeight) {
                        elementBtnScrollToBottom.style.display = "none";
                    } else {
                        elementBtnScrollToBottom.style.display = "block";
                    }
                } else {
                    elementBtnScrollToTop.style.display = "none";
                    elementBtnScrollToBottom.style.display = "none";
                }
            });

            elementBtnScrollToTop.addEventListener("click", () => {
                scrollToTop();
            });
            elementBtnScrollToBottom.addEventListener("click", () => {
                scrollToBottom();
            });
        } else {
            console.log("事件監聽失敗，找不到 HTML DOM 元素 btnScrollToTop 以及 btnScrollToBottom。");
        }

        clearTimeout(this);
    }, 1000);
});

/**
 * 捲動至最上方
 */
function scrollToTop() {
    window.scrollTo({
        top: 0,
        behavior: "smooth"
    });
}

/**
 * 捲動至最下方
 */
function scrollToBottom() {
    window.scrollTo({
        top: document.body.scrollHeight,
        behavior: "smooth"
    });
}