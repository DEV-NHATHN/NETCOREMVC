## Controller

-   Action trong controller là public
-   Trả về IActionResult
-   Các dịch vụ inject vào controller qua hàm tạo

## Truyền dữ liệu sang View

-   Model
-   ViewData
-   ViewBag
-   TempData

## Areas
- Là tên dùng để routing
- Là cấu trúc thư mục chứa M.V.C
- Thiết lập Area cho controller bằng ```[Area("AreaName")]```
- Tạo cấu trúc thư mục
```
dotnet aspnet-codegenerator area Product
```