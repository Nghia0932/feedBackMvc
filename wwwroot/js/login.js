document.querySelectorAll('.toggle-password').forEach((item) => {
  item.addEventListener('click', function () {
    var passwordInput = document.getElementById('Password');
    var type =
      passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordInput.setAttribute('type', type);
    this.classList.toggle('fa-eye-slash');
  });
});

document.querySelectorAll('.close-icon').forEach((item) => {
  item.addEventListener('click', function () {
    var input = this.previousElementSibling;
    input.value = '';
  });
});
