document.addEventListener('DOMContentLoaded', function () {
  const form = document.querySelector('form');

  form.addEventListener('submit', async function (event) {
    // Clear previous error messages
    document.getElementById('emailError').textContent = '';
    document.getElementById('phoneError').textContent = '';

    // Validate phone number
    const phoneNumber = document.getElementById('PhoneNumber').value;
    const phonePattern =
      /^(032|033|034|035|036|037|038|039|081|082|083|084|085|088|070|071|072|073|074|075|076|077|078|079|093|090|089|052|056|058|092)\d{7}$/;

    if (!phonePattern.test(phoneNumber)) {
      document.getElementById('phoneError').textContent =
        'Số điện thoại không hợp lệ';
      event.preventDefault(); // Prevent form submission
      return;
    }

    // Validate email
    const email = document.getElementById('Email').value;
    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (email != '' && !emailPattern.test(email)) {
      document.getElementById('emailError').textContent = 'Email không hợp lệ';
      event.preventDefault(); // Prevent form submission
      return;
    }

    // Prevent default form submission
    event.preventDefault();

    // Collect form data
    const formData = new FormData(this);
    const data = {};
    formData.forEach((value, key) => {
      data[key] = value;
    });

    try {
      // Send form data to the API
      const response = await fetch('/api/surrvey/submit', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
      });

      const result = await response.json();
      if (response.ok) {
        $('#successModal').modal('show');
        setTimeout(function () {
          $('#successModal').modal('hide');
        }, 2500); // Close modal after 5 seconds
      } else {
        document.getElementById('errorMessage').textContent =
          'Submission failed. Please try again.';
        $('#errorModal').modal('show');
        setTimeout(function () {
          $('#errorModal').modal('hide');
        }, 2500); // Close modal after 5 seconds
      }
    } catch (error) {
      document.getElementById('errorMessage').textContent =
        'An error occurred. Please try again.';
      $('#errorModal').modal('show');
      setTimeout(function () {
        $('#errorModal').modal('hide');
      }, 2500); // Close modal after 5 seconds
      console.error(error);
    }
  });

  // Event listener for the close button on the success modal
  document
    .querySelector('#successModal .close')
    .addEventListener('click', function () {
      $('#successModal').modal('hide');
    });

  // Event listener for the close button on the error modal
  document
    .querySelector('#errorModal .close')
    .addEventListener('click', function () {
      $('#errorModal').modal('hide');
    });
});
