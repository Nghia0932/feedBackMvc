// Toggle password visibility
document.querySelectorAll('.toggle-password').forEach((item) => {
  item.addEventListener('click', function () {
    var passwordInput = document.getElementById('Password');
    var type =
      passwordInput.getAttribute('type') === 'password' ? 'text' : 'password';
    passwordInput.setAttribute('type', type);
    this.classList.toggle('fa-eye-slash');
  });
});

// Clear input value on click
document.querySelectorAll('.close-icon').forEach((item) => {
  item.addEventListener('click', function () {
    var input = this.previousElementSibling;
    input.value = '';
  });
});

// Clear input value on click
document.querySelectorAll('.close-icon').forEach(function (item) {
  item.addEventListener('click', function () {
    var input = this.previousElementSibling;
    input.value = '';
  });
});
