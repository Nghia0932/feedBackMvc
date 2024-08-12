// Scroll to top button functionality
const scrollToTopBtn = document.getElementById('scrollToTopBtn');
window.onscroll = function () {
  if (
    document.body.scrollTop > 100 ||
    document.documentElement.scrollTop > 100
  ) {
    scrollToTopBtn.classList.add('show');
  } else {
    scrollToTopBtn.classList.remove('show');
  }
};
scrollToTopBtn.onclick = function () {
  window.scrollTo({
    top: 0,
    behavior: 'smooth',
  });
};
