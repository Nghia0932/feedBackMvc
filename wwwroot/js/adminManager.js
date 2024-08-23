document.addEventListener('DOMContentLoaded', function () {
  // Hiển thị modal thông báo thành công nếu có thông báo thành công
  var successModal = document.getElementById('successModal');
  if (successModal) {
    var modalInstance = new bootstrap.Modal(successModal);
    modalInstance.show();
  }

  // Đóng modal khi nhấn nút đóng
  var closeButton = successModal
    ? successModal.querySelector('.btn-secondary')
    : null;
  if (closeButton) {
    closeButton.addEventListener('click', function () {
      var modalInstance = bootstrap.Modal.getInstance(successModal);
      if (modalInstance) {
        modalInstance.hide();
      }
    });
  }

  // Thêm sự kiện click cho các liên kết trong sidebar
  document.querySelectorAll('.nav-link').forEach(function (item) {
    item.addEventListener('click', function () {
      // Xóa lớp active khỏi tất cả các liên kết
      document.querySelectorAll('.nav-link').forEach(function (link) {
        link.classList.remove('active');
      });
      // Thêm lớp active vào liên kết được click
      item.classList.add('active');
    });
  });

  // Function to fetch admin info and update the name
  function fetchAdminInfo() {
    fetch('/AdminManager/GetAdminInfo') // Ensure this URL is correct
      .then((response) => response.json())
      .then((data) => {
        // Update the admin name in the DOM
        var adminNameElement = document.getElementById('adminName');
        if (adminNameElement) {
          adminNameElement.textContent = data.ten || 'No name available'; // Assuming the property name is `ten`
        }
      })
      .catch((error) => {
        console.error('Error fetching admin info:', error);
        var adminNameElement = document.getElementById('adminName');
        if (adminNameElement) {
          adminNameElement.textContent = 'Error loading admin info';
        }
      });
  }

  // Call the function to fetch and display admin info
  fetchAdminInfo();
});
