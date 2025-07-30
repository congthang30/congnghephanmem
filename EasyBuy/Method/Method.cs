using System.Text.RegularExpressions;

namespace EasyBuy.Method
{
    public class Method
    {
        private string contex { get; set; }
        public Method() { }

        public bool IsEmpty(string context)
        {
            return string.IsNullOrWhiteSpace(context);
        }
        public bool IsValidName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return false;

            // Chỉ cho phép chữ cái (bao gồm dấu tiếng Việt) và khoảng trắng giữa các từ
            string pattern = @"^[\p{L}\s]{2,50}$";
            return Regex.IsMatch(name, pattern);
        }
        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password) || password.Length <= 8)
                return false;

            bool hasUpper = false;
            bool hasLower = false;
            bool hasSpecial = false;

            foreach (char c in password)
            {
                if (char.IsUpper(c)) hasUpper = true;
                else if (char.IsLower(c)) hasLower = true;
                else if (!char.IsLetterOrDigit(c)) hasSpecial = true;

                // Nếu đã đủ hết điều kiện thì trả về true luôn
                if (hasUpper && hasLower && hasSpecial)
                    return true;
            }

            return false;
        }
        public bool IsValidVietnamPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            // Biểu thức chính quy kiểm tra số điện thoại VN
            string pattern = @"^(03|05|07|08|09)\d{8}$";
            return Regex.IsMatch(phoneNumber, pattern);
        }
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
