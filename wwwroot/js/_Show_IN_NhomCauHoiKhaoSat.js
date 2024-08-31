$(document).ready(function() {
    var originalData = {};
    var editedRows = new Set();
    var existingTitles = $('#existing-titles-data').val();
    // Toggle visibility of child questions
    function toggleChildQuestions(target) {
        var icon = $(target).closest('tr').find('.toggle-icon i');
        var childRow = $(target).closest('tr').next('.child-questions');
        childRow.toggle();
        icon.toggleClass('fa-chevron-right fa-chevron-down');
    }

    // Handle click on the toggle icon
    $('.toggle-icon').on('click', function() {
        toggleChildQuestions(this);
    });
    // Handle adding new rows
    $('#add-row-btn').on('click', function() {
        var newRowHtml = `
            <tr class="new-row">
                <td style="max-width: 146px;"><input type="text" class="form-control" placeholder="Nhập nhóm câu hỏi"></td>
                <td><input type="text" class="form-control" placeholder="Nhập nội dung"></td>
                <td class="delete-row"><i class="fas fa-times"></i></td>
            </tr>`;
        $('#question-group-container').append(newRowHtml);
        $('#cancel-all-btn').show();
        $('#add-all-btn').show();
    });

    // Handle removing new rows
    $(document).on('click', '.delete-row', function() {
        $(this).closest('.new-row').remove();
        if ($('.new-row').length === 0) {
            $('#cancel-all-btn').hide();
            $('#add-all-btn').hide();
        }
    });

    // Handle canceling all new rows
    $('#cancel-all-btn').on('click', function() {
        $('.new-row').remove();
        $('#cancel-all-btn').hide();
        $('#add-all-btn').hide();
    });

    // Handle adding all rows
    $('#add-all-btn').on('click', function() {
        var allFilled = true;
        var titles = [];
        var hasDuplicate = false;
        var titleLength = false;
        $('.new-row').each(function() {
            var inputs = $(this).find('input');
            var title = inputs.eq(0).val().trim();
            var content = inputs.eq(1).val().trim();
            if (title === '' || content === '') {
                allFilled = false;
            }
            if (title.length > 5) {
                titleLength = true;
            }
            if (titles.includes(title) || existingTitles.includes(title)) {
                hasDuplicate = true;
            }
            titles.push(title);
        });

        if (!allFilled) {
            var modalData = {
                Title: 'Nhắc nhở',
                Message: 'Vui lòng nhập đầy đủ thông tin.',
                IconClass: 'fa-info-circle',
                HasAction: false,
                NotificationType: 'info',
            };
            showNotificationModal(modalData);
        } else if (titleLength) {
            var modalData = {
                Title: 'Nhắc nhở',
                Message: 'Tên nhóm không được dài quá 5 ký tự.',
                IconClass: 'fa-info-circle',
                HasAction: false,
                NotificationType: 'info',
            };
            showNotificationModal(modalData);
            return;
        } else if (hasDuplicate) {
            var modalData = {
                Title: 'Lỗi',
                Message: 'Tiêu đề đã tồn tại!',
                IconClass: 'fa-exclamation-triangle',
                HasAction: false,
                NotificationType: 'error',
            };
            showNotificationModal(modalData);
            return;
        } else {
            var titles = [];
            var contents = [];
            $('.new-row').each(function() {
                var inputs = $(this).find('input');
                titles.push(inputs.eq(0).val().trim());
                contents.push(inputs.eq(1).val().trim());
            });
            $.ajax({
                url: '/In_NhomCauHoiKhaoSat/ThemNhomCauHoiKhaoSat',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ TieuDes: titles, NoiDungs: contents }),
                success: function(response) {
                    $('.new-row').remove();
                    $('#cancel-all-btn').hide();
                    $('#add-all-btn').hide();
                    $('.table-striped').empty();
                    $('.table-striped')
                        .empty()
                        .load(
                            '/IN_NhomCauHoiKhaoSat/Show_IN_NhomCauHoiKhaoSat',
                            function() {
                                // Chỉ gắn lại sự kiện nếu có nội dung mới
                            }
                        );
                    existingTitles = existingTitles.concat(titles);
                    // Show success notification
                    var modalData = {
                        Title: 'Thành công',
                        Message: 'Dữ liệu đã được thêm thành công.',
                        IconClass: 'fa-check-circle',
                        HasAction: false,
                        NotificationType: 'success',
                    };
                    showNotificationModal(modalData);
                },
                error: function(xhr, status, error) {
                    alert('Có lỗi xảy ra rồi nè: ' + error);
                },
            });
        }
    });

    // Handle delete icon click
    $('.delete-icon').on('click', function() {
        var id = $(this).data('id');
        var row = $(this).closest('.parent-row');
        var child = row.next('.child-questions');
        var title = row.find('.title').text().trim();
        showWarningModal({
                Title: 'Xác nhận xóa',
                Message: 'Bạn có chắc chắn muốn xóa nhóm câu hỏi này không?',
                IconClass: 'fa-exclamation-triangle',
            },
            function() {
                $.ajax({
                    url: '/In_NhomCauHoiKhaoSat/XoaNhomCauHoiKhaoSat',
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify({ Id: id }),
                    success: function(response) {
                        row.remove();
                        child.remove();
                        existingTitles = existingTitles.filter(function(existingTitle) {
                            return existingTitle !== title;
                        });
                        var modalData = {
                            Title: 'Thành công',
                            Message: 'Dữ liệu đã xóa thành công.',
                            IconClass: 'fa-check-circle',
                            HasAction: false,
                            NotificationType: 'success',
                        };
                        showNotificationModal(modalData);
                    },
                    error: function(xhr, status, error) {
                        alert('Có lỗi xảy ra ngoại lệ: ' + error);
                    },
                });
            }
        );
    });

    // Handle update icon click
    $('.update-icon').on('click', function() {
        if ($(this).hasClass('delete-update-icon')) {
            var rowId = $(this).data('id'); // Lấy ID của hàng đang chỉnh sửa
            var original = originalData[rowId]; // Lấy dữ liệu gốc đã lưu trữ trước đó
            if (original) {
                var row = $(`tr[data-target='#questions-${rowId}']`); // Lấy hàng tương ứng
                row.find('.title').html(original.title); // Khôi phục tiêu đề
                row.find('.content').html(original.content); // Khôi phục nội dung
                $(this).html('<i class="fas fa-edit"></i>'); // Thay đổi biểu tượng trở lại thành "edit"
                $(this).addClass('update-icon').removeClass('delete-update-icon'); // Thay đổi lại class
            }
            editedRows.delete(rowId);
            existingTitles.push(original.title);
            // Kiểm tra nếu không còn hàng nào đang chỉnh sửa, ẩn các nút cập nhật hàng loạt
            if ($('.delete-update-icon').length === 0) {
                $('#update-all-btn').hide();
                $('#cancel-update-all-btn').hide();
                $('#add-row-btn').removeClass('hidden');
            }
        } else {
            var title = $(this).closest('tr').find('.title');
            var content = $(this).closest('tr').find('.content');
            var currentTitle = title.text();
            var currentContent = content.text();
            var rowId = $(this).data('id');
            existingTitles = existingTitles.filter(function(existingTitle) {
                return existingTitle !== currentTitle;
            });
            // Store original values
            if (!originalData[rowId]) {
                originalData[rowId] = {
                    title: currentTitle,
                    content: currentContent,
                };
            }
            title.html('<input type="text" value="' + currentTitle + '">');
            content.html('<input type="text" value="' + currentContent + '">');
            $(this)
                .html('<i class="fas fa-save"></i>')
                .addClass('delete-update-icon')
                .removeClass('update-icon');
            editedRows.add(rowId);
            $('#update-all-btn').show();
            $('#cancel-update-all-btn').show();
            $('#add-row-btn').addClass('hidden');
        }
    });

    // Handle update all button click
    $('#update-all-btn').on('click', function() {
        var updates = [];
        editedRows.forEach(function(rowId) {
            var row = $(`tr[data-target='#questions-${rowId}']`);
            var title = row.find('.title input').val().trim();
            var content = row.find('.content input').val().trim();
            if (title !== '' && content !== '') {
                if (existingTitles.includes(title)) {
                    showWarningModal({
                        Title: 'Lỗi',
                        Message: 'Tiêu đề đã tồn tại!',
                        IconClass: 'fa-exclamation-triangle',
                    });
                    return;
                }
                updates.push({
                    Id: rowId,
                    Title: title,
                    Content: content,
                });
            } else {
                showWarningModal({
                    Title: 'Nhắc nhở',
                    Message: 'Vui lòng điền đầy đủ thông tin!',
                    IconClass: 'fa-info-circle',
                });
                return;
            }
        });
        if (updates.length > 0) {
            $.ajax({
                url: '/In_NhomCauHoiKhaoSat/CapNhatNhomCauHoiKhaoSat',
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(updates),
                success: function(response) {
                    // Update existing titles and hide update buttons
                    $('#update-all-btn').hide();
                    $('#cancel-update-all-btn').hide();
                    $('#add-row-btn').removeClass('hidden');
                    $('.delete-update-icon').each(function() {
                        var rowId = $(this).data('id');
                        var original = originalData[rowId];
                        if (original) {
                            var row = $(`tr[data-target='#questions-${rowId}']`);
                            row.find('.title').html(original.title);
                            row.find('.content').html(original.content);
                        }
                        $(this)
                            .html('<i class="fas fa-edit"></i>')
                            .removeClass('delete-update-icon')
                            .addClass('update-icon');
                    });
                    showNotificationModal({
                        Title: 'Thành công',
                        Message: 'Cập nhật thành công!',
                        IconClass: 'fa-check-circle',
                        NotificationType: 'success',
                    });
                },
                error: function(xhr, status, error) {
                    alert('Có lỗi xảy ra ngoại lệ: ' + error);
                },
            });
        }
    });

    // Handle cancel update all button click
    $('#cancel-update-all-btn').on('click', function() {
        $('.delete-update-icon').each(function() {
            var rowId = $(this).data('id');
            var original = originalData[rowId];
            if (original) {
                var row = $(`tr[data-target='#questions-${rowId}']`);
                row.find('.title').html(original.title);
                row.find('.content').html(original.content);
            }
            $(this)
                .html('<i class="fas fa-edit"></i>')
                .removeClass('delete-update-icon')
                .addClass('update-icon');
        });
        $('#update-all-btn').hide();
        $('#cancel-update-all-btn').hide();
        $('#add-row-btn').removeClass('hidden');
        editedRows.clear();
    });

    // Show notification modal
    function showNotificationModal(data) {
        $('#notificationModalTitle').text(data.Title);
        $('#notificationModalBody').text(data.Message);
        $('#notificationModalIcon')
            .removeClass()
            .addClass('fas ' + data.IconClass);
        $('#notificationModal').modal('show');
    }

    // Show warning modal
    function showWarningModal(data, callback) {
        $('#warningModalTitle').text(data.Title);
        $('#warningModalBody').text(data.Message);
        $('#warningModalIcon')
            .removeClass()
            .addClass('fas ' + data.IconClass);
        $('#warningModalConfirm')
            .off('click')
            .on('click', function() {
                $('#warningModal').modal('hide');
                if (callback) callback();
            });
        $('#warningModal').modal('show');
    }
});