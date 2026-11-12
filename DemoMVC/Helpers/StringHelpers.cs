using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DemoMVC.Helpers
{
    public class StringHelpers
    {
        public static string StudentIdGenerator(int namNhaphoc, int heDaiHoc, int nganhHoc, int soThuTu)
        {
            // 2321050003
            // Hai số đầu là năm nhập học (xử lý trường hợp truyền vào 4 số như 2023 hoặc 2 số như 23)
            int nam = namNhaphoc >= 100 ? namNhaphoc % 100 : namNhaphoc;

            // Trả về mã sinh viên theo đúng định dạng
            // :D2 -> định dạng hiển thị đủ 2 chữ số (vd: 5 thành 05)
            // :D4 -> định dạng hiển thị đủ 4 chữ số (vd: 3 thành 0003)
            return $"{nam:D2}{heDaiHoc:D2}{nganhHoc:D2}{soThuTu:D4}";
        }
    }
}