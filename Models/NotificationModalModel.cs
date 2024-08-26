public class NotificationModalModel
{
    public string Title { get; set; }
    public string Message { get; set; }
    public string IconClass { get; set; } // Class icon FontAwesome
    public string ImageUrl { get; set; } // URL của hình ảnh
    public bool HasAction { get; set; } // Xác định có nút hành động hay không
    public string ActionText { get; set; } // Văn bản nút hành động
    public string NotificationType { get; set; } // Phân loại thông báo (info, warning, error, etc.)
}
