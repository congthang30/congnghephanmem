# DANH SÁCH CÁC TASK CÔNG VIỆC TESTCASE - HỆ THỐNG EASYBUY

## 1. MODULE QUẢN LÝ TÀI KHOẢN (ACCOUNT MANAGEMENT)

### 1.1. Đăng ký tài khoản (Register)
- **TC_REG_001**: Đăng ký thành công với thông tin hợp lệ
- **TC_REG_002**: Đăng ký thất bại - thiếu thông tin bắt buộc (phone, password, name, email)
- **TC_REG_003**: Đăng ký thất bại - mật khẩu không đủ độ dài (< 8 ký tự)
- **TC_REG_004**: Đăng ký thất bại - mật khẩu không có chữ hoa/chữ thường
- **TC_REG_005**: Đăng ký thất bại - số điện thoại không hợp lệ (không đúng format VN)
- **TC_REG_006**: Đăng ký thất bại - mật khẩu nhập lại không khớp
- **TC_REG_007**: Đăng ký thất bại - tên chứa số hoặc ký tự đặc biệt
- **TC_REG_008**: Đăng ký thất bại - email không hợp lệ
- **TC_REG_009**: Đăng ký thất bại - số điện thoại đã tồn tại
- **TC_REG_010**: Đăng ký thất bại - email đã tồn tại
- **TC_REG_011**: Đăng ký bằng Google - thành công
- **TC_REG_012**: Đăng ký bằng Google - đặt mật khẩu thành công
- **TC_REG_013**: Đăng ký bằng Google - đặt mật khẩu thất bại (mật khẩu không hợp lệ)

### 1.2. Đăng nhập (Login)
- **TC_LOGIN_001**: Đăng nhập thành công bằng email
- **TC_LOGIN_002**: Đăng nhập thành công bằng số điện thoại
- **TC_LOGIN_003**: Đăng nhập thất bại - tài khoản không tồn tại
- **TC_LOGIN_004**: Đăng nhập thất bại - mật khẩu sai
- **TC_LOGIN_005**: Đăng nhập thất bại - tài khoản bị khóa (sau 3 lần sai)
- **TC_LOGIN_006**: Đăng nhập thành công sau khi tài khoản tự mở khóa (sau 15 phút)
- **TC_LOGIN_007**: Đăng nhập thành công - redirect theo role (Admin, NVKD, NVKho, NVKT, Customer)
- **TC_LOGIN_008**: Đăng nhập bằng Google - thành công
- **TC_LOGIN_009**: Đăng nhập bằng Google - tài khoản bị khóa
- **TC_LOGIN_010**: Đăng nhập khi đã đăng nhập - redirect về trang chủ

### 1.3. Đăng xuất (Logout)
- **TC_LOGOUT_001**: Đăng xuất thành công
- **TC_LOGOUT_002**: Đăng xuất thất bại - xử lý exception

### 1.4. Cập nhật thông tin tài khoản (Update Account)
- **TC_UPDATE_ACC_001**: Cập nhật thành công - thay đổi tên
- **TC_UPDATE_ACC_002**: Cập nhật thành công - thay đổi email
- **TC_UPDATE_ACC_003**: Cập nhật thành công - thay đổi số điện thoại
- **TC_UPDATE_ACC_004**: Cập nhật thất bại - không nhập mật khẩu xác nhận
- **TC_UPDATE_ACC_005**: Cập nhật thất bại - mật khẩu xác nhận sai
- **TC_UPDATE_ACC_006**: Cập nhật thất bại - email không hợp lệ
- **TC_UPDATE_ACC_007**: Cập nhật thất bại - email đã được sử dụng
- **TC_UPDATE_ACC_008**: Cập nhật thất bại - số điện thoại không hợp lệ
- **TC_UPDATE_ACC_009**: Cập nhật thất bại - số điện thoại đã được sử dụng
- **TC_UPDATE_ACC_010**: Cập nhật khi chưa đăng nhập - redirect về trang login

### 1.5. Đổi mật khẩu (Change Password)
- **TC_CHANGE_PASS_001**: Đổi mật khẩu thành công
- **TC_CHANGE_PASS_002**: Đổi mật khẩu thất bại - thiếu thông tin
- **TC_CHANGE_PASS_003**: Đổi mật khẩu thất bại - mật khẩu hiện tại sai
- **TC_CHANGE_PASS_004**: Đổi mật khẩu thất bại - mật khẩu mới không khớp
- **TC_CHANGE_PASS_005**: Đổi mật khẩu thất bại - mật khẩu mới không đủ độ mạnh
- **TC_CHANGE_PASS_006**: Đổi mật khẩu khi chưa đăng nhập - redirect về trang login

### 1.6. Quản lý địa chỉ (Address Management)
- **TC_ADDR_001**: Thêm địa chỉ thành công
- **TC_ADDR_002**: Thêm địa chỉ thất bại - thiếu thông tin
- **TC_ADDR_003**: Thêm địa chỉ thất bại - số điện thoại không hợp lệ
- **TC_ADDR_004**: Xem danh sách địa chỉ thành công
- **TC_ADDR_005**: Cập nhật địa chỉ thành công
- **TC_ADDR_006**: Cập nhật địa chỉ thất bại - không có quyền sửa
- **TC_ADDR_007**: Xóa địa chỉ thành công
- **TC_ADDR_008**: Xóa địa chỉ thất bại - không có quyền xóa
- **TC_ADDR_009**: Quản lý địa chỉ khi chưa đăng nhập - redirect về trang login

### 1.7. Khóa tài khoản (Lock Account)
- **TC_LOCK_ACC_001**: Khóa tài khoản thành công
- **TC_LOCK_ACC_002**: Khóa tài khoản - hủy thao tác

---

## 2. MODULE QUẢN LÝ SẢN PHẨM (PRODUCT MANAGEMENT)

### 2.1. Xem danh sách sản phẩm (Home/TrangChu)
- **TC_PRODUCT_LIST_001**: Hiển thị danh sách sản phẩm thành công
- **TC_PRODUCT_LIST_002**: Lọc sản phẩm theo danh mục (category)
- **TC_PRODUCT_LIST_003**: Lọc sản phẩm theo thương hiệu (brand)
- **TC_PRODUCT_LIST_004**: Lọc sản phẩm theo khoảng giá (minPrice, maxPrice)
- **TC_PRODUCT_LIST_005**: Tìm kiếm sản phẩm theo tên
- **TC_PRODUCT_LIST_006**: Kết hợp nhiều bộ lọc
- **TC_PRODUCT_LIST_007**: Không hiển thị sản phẩm có status "hidden"
- **TC_PRODUCT_LIST_008**: Không hiển thị sản phẩm hết hàng (quantity = 0)
- **TC_PRODUCT_LIST_009**: Xử lý lỗi hệ thống khi load danh sách

### 2.2. Xem chi tiết sản phẩm (ViewProductDetails)
- **TC_PRODUCT_DETAIL_001**: Hiển thị chi tiết sản phẩm thành công
- **TC_PRODUCT_DETAIL_002**: Hiển thị đánh giá đã được duyệt
- **TC_PRODUCT_DETAIL_003**: Kiểm tra trạng thái đã mua hàng (hasPurchased)
- **TC_PRODUCT_DETAIL_004**: Kiểm tra trạng thái đã đánh giá (existingRating)
- **TC_PRODUCT_DETAIL_005**: Tăng ViewCount khi xem sản phẩm
- **TC_PRODUCT_DETAIL_006**: Sản phẩm không tồn tại - redirect về trang lỗi
- **TC_PRODUCT_DETAIL_007**: Xử lý lỗi hệ thống

---

## 3. MODULE QUẢN LÝ GIỎ HÀNG (CART MANAGEMENT)

### 3.1. Xem giỏ hàng (UserCart)
- **TC_CART_VIEW_001**: Hiển thị giỏ hàng thành công
- **TC_CART_VIEW_002**: Giỏ hàng trống - hiển thị danh sách rỗng
- **TC_CART_VIEW_003**: Xem giỏ hàng khi chưa đăng nhập - redirect về trang login

### 3.2. Thêm sản phẩm vào giỏ hàng (AddToCart)
- **TC_CART_ADD_001**: Thêm sản phẩm vào giỏ hàng thành công
- **TC_CART_ADD_002**: Thêm sản phẩm với số lượng cụ thể
- **TC_CART_ADD_003**: Thêm sản phẩm đã có trong giỏ - tăng số lượng
- **TC_CART_ADD_004**: Thêm sản phẩm không tồn tại - trả về NotFound
- **TC_CART_ADD_005**: Thêm sản phẩm khi chưa đăng nhập - redirect về trang login
- **TC_CART_ADD_006**: Thêm sản phẩm với số lượng <= 0 - tự động set = 1
- **TC_CART_ADD_007**: Xử lý lỗi hệ thống khi thêm vào giỏ hàng

### 3.3. Xóa sản phẩm khỏi giỏ hàng (RemoveFromCart)
- **TC_CART_REMOVE_001**: Xóa sản phẩm khỏi giỏ hàng thành công
- **TC_CART_REMOVE_002**: Xóa sản phẩm không có trong giỏ - trả về NotFound
- **TC_CART_REMOVE_003**: Xóa khi giỏ hàng không tồn tại - trả về NotFound
- **TC_CART_REMOVE_004**: Xóa khi chưa đăng nhập - redirect về trang login
- **TC_CART_REMOVE_005**: Xử lý lỗi hệ thống

### 3.4. Cập nhật số lượng sản phẩm (UpdateCart)
- **TC_CART_UPDATE_001**: Cập nhật số lượng thành công
- **TC_CART_UPDATE_002**: Cập nhật số lượng = 0 - tự động xóa khỏi giỏ hàng
- **TC_CART_UPDATE_003**: Cập nhật sản phẩm không tồn tại - trả về NotFound
- **TC_CART_UPDATE_004**: Cập nhật khi chưa đăng nhập - redirect về trang login
- **TC_CART_UPDATE_005**: Xử lý lỗi hệ thống

---

## 4. MODULE QUẢN LÝ ĐƠN HÀNG (ORDER MANAGEMENT)

### 4.1. Xem trang thanh toán (Checkout)
- **TC_CHECKOUT_001**: Hiển thị trang checkout thành công
- **TC_CHECKOUT_002**: Hiển thị danh sách địa chỉ của user
- **TC_CHECKOUT_003**: Hiển thị danh sách phương thức thanh toán
- **TC_CHECKOUT_004**: Giỏ hàng trống - redirect về giỏ hàng
- **TC_CHECKOUT_005**: Xem checkout khi chưa đăng nhập - redirect về trang login
- **TC_CHECKOUT_006**: Xử lý lỗi hệ thống

### 4.2. Xử lý đặt hàng (Checkout POST)
- **TC_ORDER_CREATE_001**: Đặt hàng thành công - COD (Thanh toán khi nhận hàng)
- **TC_ORDER_CREATE_002**: Đặt hàng thành công - VNPay
- **TC_ORDER_CREATE_003**: Đặt hàng thành công - MoMo
- **TC_ORDER_CREATE_004**: Đặt hàng thất bại - giỏ hàng trống
- **TC_ORDER_CREATE_005**: Đặt hàng thất bại - mã voucher không hợp lệ
- **TC_ORDER_CREATE_006**: Đặt hàng thất bại - đơn hàng không đạt giá trị tối thiểu cho voucher
- **TC_ORDER_CREATE_007**: Áp dụng voucher thành công - giảm giá theo phần trăm
- **TC_ORDER_CREATE_008**: Áp dụng voucher thành công - giảm giá theo số tiền cố định
- **TC_ORDER_CREATE_009**: Áp dụng voucher - giảm giá không vượt quá MaxDiscountAmount
- **TC_ORDER_CREATE_010**: Đặt hàng thất bại - phương thức thanh toán không hợp lệ
- **TC_ORDER_CREATE_011**: Xử lý lỗi hệ thống

### 4.3. Xác minh OTP (VerifyOtp) - COD
- **TC_OTP_VERIFY_001**: Xác minh OTP thành công - tạo đơn hàng
- **TC_OTP_VERIFY_002**: Xác minh OTP thất bại - mã OTP sai
- **TC_OTP_VERIFY_003**: Xác minh OTP thất bại - mã OTP hết hạn (sau 5 phút)
- **TC_OTP_VERIFY_004**: Xác minh OTP thất bại - vượt quá 3 lần thử
- **TC_OTP_VERIFY_005**: Gửi lại OTP thành công
- **TC_OTP_VERIFY_006**: Xác minh OTP - giảm số lượng voucher
- **TC_OTP_VERIFY_007**: Xác minh OTP - giảm số lượng sản phẩm trong kho
- **TC_OTP_VERIFY_008**: Xác minh OTP - sản phẩm không đủ số lượng
- **TC_OTP_VERIFY_009**: Xác minh OTP - gửi email xác nhận đơn hàng
- **TC_OTP_VERIFY_010**: Xử lý transaction rollback khi có lỗi

### 4.4. Callback thanh toán VNPay (PaymentCallbackVnpay)
- **TC_VNPAY_CALLBACK_001**: Callback thành công - tạo đơn hàng
- **TC_VNPAY_CALLBACK_002**: Callback thất bại - thiếu thông tin session
- **TC_VNPAY_CALLBACK_003**: Callback thành công - giảm số lượng sản phẩm
- **TC_VNPAY_CALLBACK_004**: Callback thành công - đánh dấu giỏ hàng đã checkout
- **TC_VNPAY_CALLBACK_005**: Callback thành công - áp dụng voucher

### 4.5. Callback thanh toán MoMo (PaymentCallbackMomo)
- **TC_MOMO_CALLBACK_001**: Callback thành công - cập nhật trạng thái đơn hàng
- **TC_MOMO_CALLBACK_002**: Callback thất bại - không tìm thấy đơn hàng
- **TC_MOMO_CALLBACK_003**: Callback - đơn hàng đã được xử lý trước đó
- **TC_MOMO_CALLBACK_004**: Callback thất bại - cập nhật trạng thái "Thanh toán thất bại"

### 4.6. Xác thực voucher (ValidateVoucher)
- **TC_VOUCHER_VALIDATE_001**: Xác thực voucher thành công
- **TC_VOUCHER_VALIDATE_002**: Xác thực voucher thất bại - mã không tồn tại
- **TC_VOUCHER_VALIDATE_003**: Xác thực voucher thất bại - voucher đã hết hạn
- **TC_VOUCHER_VALIDATE_004**: Xác thực voucher thất bại - voucher đã hết số lượng
- **TC_VOUCHER_VALIDATE_005**: Xác thực voucher thất bại - đơn hàng không đạt giá trị tối thiểu
- **TC_VOUCHER_VALIDATE_006**: Xác thực voucher thất bại - voucher không active

### 4.7. Xem danh sách đơn hàng (ListOrder)
- **TC_ORDER_LIST_001**: Hiển thị danh sách đơn hàng thành công
- **TC_ORDER_LIST_002**: Sắp xếp đơn hàng theo ngày tạo (mới nhất trước)
- **TC_ORDER_LIST_003**: Xem danh sách khi chưa đăng nhập - redirect về trang login
- **TC_ORDER_LIST_004**: Xử lý lỗi hệ thống

### 4.8. Xem chi tiết đơn hàng (ViewOrderDetails)
- **TC_ORDER_DETAIL_001**: Hiển thị chi tiết đơn hàng thành công
- **TC_ORDER_DETAIL_002**: Đơn hàng không tồn tại - redirect về danh sách
- **TC_ORDER_DETAIL_003**: Xử lý lỗi hệ thống

### 4.9. Hủy đơn hàng (CancelOrder)
- **TC_ORDER_CANCEL_001**: Hủy đơn hàng thành công - trạng thái "Chờ xác nhận"
- **TC_ORDER_CANCEL_002**: Hủy đơn hàng thất bại - đơn hàng không ở trạng thái "Chờ xác nhận"
- **TC_ORDER_CANCEL_003**: Hủy đơn hàng thất bại - đơn hàng không tồn tại
- **TC_ORDER_CANCEL_004**: Hủy đơn hàng - trigger trả số lượng tồn kho
- **TC_ORDER_CANCEL_005**: Hủy đơn hàng - trigger trả số lượng voucher

### 4.10. Mua lại đơn hàng (RepeatOrder)
- **TC_ORDER_REPEAT_001**: Mua lại đơn hàng thành công
- **TC_ORDER_REPEAT_002**: Mua lại đơn hàng thất bại - chưa đăng nhập
- **TC_ORDER_REPEAT_003**: Mua lại đơn hàng thất bại - đơn hàng không tồn tại
- **TC_ORDER_REPEAT_004**: Mua lại đơn hàng thất bại - đơn hàng không có sản phẩm
- **TC_ORDER_REPEAT_005**: Mua lại đơn hàng - chỉ thêm sản phẩm còn hàng
- **TC_ORDER_REPEAT_006**: Mua lại đơn hàng - số lượng không vượt quá tồn kho
- **TC_ORDER_REPEAT_007**: Mua lại đơn hàng - cộng vào giỏ hàng hiện có
- **TC_ORDER_REPEAT_008**: Xử lý transaction rollback khi có lỗi

### 4.11. Hủy checkout (CancelCheckout)
- **TC_CHECKOUT_CANCEL_001**: Hủy checkout thành công - xóa session
- **TC_CHECKOUT_CANCEL_002**: Xử lý lỗi khi hủy checkout

---

## 5. MODULE ĐÁNH GIÁ SẢN PHẨM (RATING)

### 5.1. Đăng đánh giá (PostRating)
- **TC_RATING_POST_001**: Đăng đánh giá thành công
- **TC_RATING_POST_002**: Đăng đánh giá thất bại - chưa mua sản phẩm
- **TC_RATING_POST_003**: Đăng đánh giá thất bại - đã đánh giá rồi
- **TC_RATING_POST_004**: Đăng đánh giá thất bại - chưa đăng nhập
- **TC_RATING_POST_005**: Đăng đánh giá với ảnh thành công
- **TC_RATING_POST_006**: Đăng đánh giá thất bại - lỗi upload ảnh
- **TC_RATING_POST_007**: Đánh giá chưa được duyệt (IsApproved = false)
- **TC_RATING_POST_008**: Xử lý lỗi hệ thống

---

## 6. MODULE YÊU THÍCH (WISHLIST)

### 6.1. Xem danh sách yêu thích (Wishlist)
- **TC_WISHLIST_VIEW_001**: Hiển thị danh sách yêu thích thành công
- **TC_WISHLIST_VIEW_002**: Xem yêu thích khi chưa đăng nhập - redirect về trang login
- **TC_WISHLIST_VIEW_003**: Xử lý lỗi hệ thống

### 6.2. Thêm/Xóa yêu thích (AddWishList)
- **TC_WISHLIST_ADD_001**: Thêm vào yêu thích thành công
- **TC_WISHLIST_ADD_002**: Xóa khỏi yêu thích thành công (sản phẩm đã có trong wishlist)
- **TC_WISHLIST_ADD_003**: Thêm yêu thích khi chưa đăng nhập - redirect về trang login
- **TC_WISHLIST_ADD_004**: Xử lý lỗi hệ thống

### 6.3. Thêm/Xóa yêu thích bằng Ajax (AddWishListAjax)
- **TC_WISHLIST_AJAX_001**: Thêm vào yêu thích bằng Ajax thành công
- **TC_WISHLIST_AJAX_002**: Xóa khỏi yêu thích bằng Ajax thành công
- **TC_WISHLIST_AJAX_003**: Thêm yêu thích Ajax khi chưa đăng nhập - trả về JSON yêu cầu đăng nhập
- **TC_WISHLIST_AJAX_004**: Xử lý lỗi hệ thống

### 6.4. Xóa khỏi yêu thích (RemoveWishList)
- **TC_WISHLIST_REMOVE_001**: Xóa khỏi yêu thích thành công
- **TC_WISHLIST_REMOVE_002**: Xóa khi chưa đăng nhập - redirect về trang login

### 6.5. Kiểm tra trạng thái yêu thích (CheckWishlistStatus)
- **TC_WISHLIST_CHECK_001**: Kiểm tra sản phẩm có trong yêu thích
- **TC_WISHLIST_CHECK_002**: Kiểm tra sản phẩm không có trong yêu thích
- **TC_WISHLIST_CHECK_003**: Kiểm tra khi chưa đăng nhập - trả về false

---

## 7. MODULE QUẢN TRỊ ADMIN

### 7.1. Quản lý sản phẩm (ProductsController)

#### 7.1.1. Danh sách sản phẩm (ListProducts)
- **TC_ADMIN_PRODUCT_LIST_001**: Hiển thị danh sách sản phẩm thành công
- **TC_ADMIN_PRODUCT_LIST_002**: Hiển thị thống kê (tổng sản phẩm, active, hidden, hết hàng)
- **TC_ADMIN_PRODUCT_LIST_003**: Sắp xếp theo ngày cập nhật (mới nhất trước)
- **TC_ADMIN_PRODUCT_LIST_004**: Truy cập khi không có quyền Admin - từ chối truy cập
- **TC_ADMIN_PRODUCT_LIST_005**: Xử lý lỗi hệ thống

#### 7.1.2. Tạo sản phẩm (CreateProducts)
- **TC_ADMIN_PRODUCT_CREATE_001**: Tạo sản phẩm thành công với đầy đủ thông tin
- **TC_ADMIN_PRODUCT_CREATE_002**: Tạo sản phẩm thành công với ảnh
- **TC_ADMIN_PRODUCT_CREATE_003**: Tạo sản phẩm thất bại - thiếu thông tin bắt buộc
- **TC_ADMIN_PRODUCT_CREATE_004**: Tạo sản phẩm thất bại - barcode đã tồn tại
- **TC_ADMIN_PRODUCT_CREATE_005**: Tạo sản phẩm thất bại - số lượng <= 0
- **TC_ADMIN_PRODUCT_CREATE_006**: Tạo sản phẩm thất bại - giá nhập/giá bán <= 0
- **TC_ADMIN_PRODUCT_CREATE_007**: Xử lý lỗi hệ thống

#### 7.1.3. Cập nhật sản phẩm (UpdateProducts)
- **TC_ADMIN_PRODUCT_UPDATE_001**: Cập nhật sản phẩm thành công
- **TC_ADMIN_PRODUCT_UPDATE_002**: Cập nhật sản phẩm thành công - thay đổi ảnh
- **TC_ADMIN_PRODUCT_UPDATE_003**: Cập nhật sản phẩm thất bại - sản phẩm không tồn tại
- **TC_ADMIN_PRODUCT_UPDATE_004**: Cập nhật sản phẩm thất bại - barcode trùng với sản phẩm khác
- **TC_ADMIN_PRODUCT_UPDATE_005**: Cập nhật một phần thông tin thành công
- **TC_ADMIN_PRODUCT_UPDATE_006**: Xử lý lỗi hệ thống

#### 7.1.4. Xóa sản phẩm (DeleteProducts)
- **TC_ADMIN_PRODUCT_DELETE_001**: Xóa sản phẩm thành công
- **TC_ADMIN_PRODUCT_DELETE_002**: Xóa sản phẩm thất bại - sản phẩm không tồn tại
- **TC_ADMIN_PRODUCT_DELETE_003**: Xử lý lỗi hệ thống

#### 7.1.5. Chi tiết sản phẩm (ProductDetail)
- **TC_ADMIN_PRODUCT_DETAIL_001**: Lấy chi tiết sản phẩm thành công (JSON)
- **TC_ADMIN_PRODUCT_DETAIL_002**: Sản phẩm không tồn tại - trả về NotFound

### 7.2. Quản lý người dùng (UsersController)

#### 7.2.1. Danh sách người dùng (ListUsers)
- **TC_ADMIN_USER_LIST_001**: Hiển thị danh sách người dùng thành công
- **TC_ADMIN_USER_LIST_002**: Truy cập khi không có quyền Admin - từ chối truy cập

#### 7.2.2. Tạo người dùng (CreateUser)
- **TC_ADMIN_USER_CREATE_001**: Tạo người dùng thành công
- **TC_ADMIN_USER_CREATE_002**: Tạo người dùng thất bại - thiếu thông tin
- **TC_ADMIN_USER_CREATE_003**: Tạo người dùng thất bại - số điện thoại đã tồn tại
- **TC_ADMIN_USER_CREATE_004**: Tạo người dùng thất bại - email đã tồn tại
- **TC_ADMIN_USER_CREATE_005**: Xử lý lỗi hệ thống

#### 7.2.3. Sửa người dùng (EditUser)
- **TC_ADMIN_USER_EDIT_001**: Sửa người dùng thành công
- **TC_ADMIN_USER_EDIT_002**: Sửa người dùng thất bại - người dùng không tồn tại
- **TC_ADMIN_USER_EDIT_003**: Sửa người dùng thất bại - số điện thoại trùng với người khác
- **TC_ADMIN_USER_EDIT_004**: Sửa người dùng thất bại - email trùng với người khác
- **TC_ADMIN_USER_EDIT_005**: Xử lý lỗi hệ thống

#### 7.2.4. Xóa người dùng (DeleteUser)
- **TC_ADMIN_USER_DELETE_001**: Xóa người dùng thành công
- **TC_ADMIN_USER_DELETE_002**: Xóa người dùng thất bại - ID không hợp lệ
- **TC_ADMIN_USER_DELETE_003**: Xóa người dùng thất bại - người dùng không tồn tại
- **TC_ADMIN_USER_DELETE_004**: Xóa người dùng thất bại - đang được sử dụng ở bảng khác
- **TC_ADMIN_USER_DELETE_005**: Xử lý lỗi hệ thống

#### 7.2.5. Chi tiết người dùng (UserDetail)
- **TC_ADMIN_USER_DETAIL_001**: Lấy chi tiết người dùng thành công (JSON)
- **TC_ADMIN_USER_DETAIL_002**: ID không hợp lệ - trả về BadRequest
- **TC_ADMIN_USER_DETAIL_003**: Người dùng không tồn tại - trả về NotFound

---

## 8. MODULE NHÂN VIÊN KINH DOANH (NVKD)

### 8.1. Quản lý đơn hàng (OrderNVKDController)

#### 8.1.1. Danh sách đơn hàng mới (ListOrderNew)
- **TC_NVKD_ORDER_NEW_001**: Hiển thị danh sách đơn hàng "Chờ xác nhận" thành công
- **TC_NVKD_ORDER_NEW_002**: Sắp xếp theo ngày tạo (mới nhất trước)
- **TC_NVKD_ORDER_NEW_003**: Truy cập khi không có quyền NVKD/Admin - từ chối truy cập
- **TC_NVKD_ORDER_NEW_004**: Xử lý lỗi hệ thống

#### 8.1.2. Danh sách đơn hàng đã xác nhận (ListOrderConfirmed)
- **TC_NVKD_ORDER_CONFIRMED_001**: Hiển thị danh sách đơn hàng "Đã xác nhận" thành công
- **TC_NVKD_ORDER_CONFIRMED_002**: Xử lý lỗi hệ thống

#### 8.1.3. Chi tiết đơn hàng (Details)
- **TC_NVKD_ORDER_DETAIL_001**: Hiển thị chi tiết đơn hàng thành công
- **TC_NVKD_ORDER_DETAIL_002**: Đơn hàng không tồn tại - trả về NotFound
- **TC_NVKD_ORDER_DETAIL_003**: Xử lý lỗi hệ thống

#### 8.1.4. Xuất hóa đơn (ExportInvoice)
- **TC_NVKD_INVOICE_EXPORT_001**: Xuất hóa đơn thành công - tạo Invoice
- **TC_NVKD_INVOICE_EXPORT_002**: Xuất hóa đơn thành công - cập nhật trạng thái đơn hàng "Đã xác nhận"
- **TC_NVKD_INVOICE_EXPORT_003**: Xuất hóa đơn thành công - gửi email yêu cầu xuất kho
- **TC_NVKD_INVOICE_EXPORT_004**: Xuất hóa đơn thất bại - chưa đăng nhập
- **TC_NVKD_INVOICE_EXPORT_005**: Xuất hóa đơn thất bại - đơn hàng không tồn tại
- **TC_NVKD_INVOICE_EXPORT_006**: Xử lý lỗi hệ thống

### 8.2. Quản lý hóa đơn (InvoiceNVKDController)

#### 8.2.1. Danh sách hóa đơn (ListInvoice)
- **TC_NVKD_INVOICE_LIST_001**: Hiển thị danh sách hóa đơn thành công
- **TC_NVKD_INVOICE_LIST_002**: Sắp xếp theo ngày tạo (mới nhất trước)
- **TC_NVKD_INVOICE_LIST_003**: Truy cập khi không có quyền NVKD/Admin - từ chối truy cập
- **TC_NVKD_INVOICE_LIST_004**: Xử lý lỗi hệ thống

#### 8.2.2. Chi tiết hóa đơn (DetailsInvoice)
- **TC_NVKD_INVOICE_DETAIL_001**: Hiển thị chi tiết hóa đơn thành công
- **TC_NVKD_INVOICE_DETAIL_002**: Hóa đơn không tồn tại - redirect về danh sách
- **TC_NVKD_INVOICE_DETAIL_003**: Xử lý lỗi hệ thống

---

## 9. MODULE BẢO MẬT VÀ PHÂN QUYỀN

### 9.1. Xác thực và phân quyền
- **TC_AUTH_001**: Kiểm tra session đăng nhập
- **TC_AUTH_002**: Kiểm tra role-based access control
- **TC_AUTH_003**: Truy cập trang Admin khi không có quyền Admin - từ chối
- **TC_AUTH_004**: Truy cập trang NVKD khi không có quyền NVKD/Admin - từ chối
- **TC_AUTH_005**: Session timeout - redirect về trang login
- **TC_AUTH_006**: Xác thực Google OAuth thành công
- **TC_AUTH_007**: Xác thực Google OAuth thất bại

### 9.2. Bảo mật tài khoản
- **TC_SECURITY_001**: Khóa tài khoản sau 3 lần đăng nhập sai
- **TC_SECURITY_002**: Tự động mở khóa sau 15 phút
- **TC_SECURITY_003**: Mật khẩu được hash bằng BCrypt
- **TC_SECURITY_004**: Session cookie HttpOnly
- **TC_SECURITY_005**: XSS protection trong input
- **TC_SECURITY_006**: SQL Injection protection (Entity Framework)

---

## 10. MODULE EMAIL VÀ THÔNG BÁO

### 10.1. Gửi email
- **TC_EMAIL_001**: Gửi email OTP thành công
- **TC_EMAIL_002**: Gửi email xác nhận đơn hàng thành công
- **TC_EMAIL_003**: Gửi email yêu cầu xuất kho thành công
- **TC_EMAIL_004**: Gửi email thất bại - email không hợp lệ
- **TC_EMAIL_005**: Gửi email thất bại - lỗi SMTP

---

## 11. MODULE TÍNH NĂNG BỔ SUNG

### 11.1. Log hoạt động (LogActivity)
- **TC_LOG_001**: Ghi log đăng nhập thành công
- **TC_LOG_002**: Ghi log đăng nhập bằng Google
- **TC_LOG_003**: Ghi log đăng ký bằng Google

### 11.2. Xử lý lỗi
- **TC_ERROR_001**: Xử lý trang 404 (NotFoundPage)
- **TC_ERROR_002**: Xử lý lỗi hệ thống chung
- **TC_ERROR_003**: Hiển thị thông báo lỗi phù hợp

---

## TỔNG KẾT

**Tổng số testcase: ~250+ testcase**

### Phân loại theo độ ưu tiên:
- **P0 (Critical)**: ~80 testcase - Các chức năng cốt lõi (đăng ký, đăng nhập, đặt hàng, thanh toán)
- **P1 (High)**: ~100 testcase - Các chức năng quan trọng (quản lý sản phẩm, quản lý đơn hàng, admin)
- **P2 (Medium)**: ~50 testcase - Các chức năng phụ (wishlist, đánh giá, voucher)
- **P3 (Low)**: ~20 testcase - Các edge cases và error handling

### Phân loại theo loại test:
- **Functional Testing**: ~200 testcase
- **Security Testing**: ~20 testcase
- **Integration Testing**: ~20 testcase
- **Error Handling Testing**: ~10 testcase

---

**Ghi chú**: 
- Các testcase này cần được viết chi tiết với các bước test cụ thể, dữ liệu test, kết quả mong đợi
- Cần có test data setup và teardown cho mỗi testcase
- Nên sử dụng automation testing framework như xUnit, NUnit cho .NET
- Cần có test coverage tối thiểu 80% cho các module quan trọng

