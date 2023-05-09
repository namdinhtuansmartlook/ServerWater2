using ServerWater2.Models;

namespace ServerWater2.APIs
{
    public class MyType
    {
        public MyType()
        {

        }
        public async Task initAsync()
        {
            using (DataContext context = new DataContext())
            {
                SqlType? type = context.types!.Where(s => s.code.CompareTo("LM") == 0).FirstOrDefault();
                if (type == null)
                {
                    SqlType item = new SqlType();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "LM";
                    item.name = "Lắp mới";
                    item.des = "Lắp mới";
                    item.isdeleted = false;
                    context.types!.Add(item);
                }
                type = context.types!.Where(s => s.code.CompareTo("SC") == 0).FirstOrDefault();
                if (type == null)
                {
                    SqlType item = new SqlType();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "SC";
                    item.name = "Sửa chữa";
                    item.des = "Sửa chữa";
                    item.isdeleted = false;
                    context.types!.Add(item);
                }
                type = context.types!.Where(s => s.code.CompareTo("TT") == 0).FirstOrDefault();
                if (type == null)
                {
                    SqlType item = new SqlType();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "TT";
                    item.name = "Thay thế";
                    item.des = "Thay thế";
                    item.isdeleted = false;
                    context.types!.Add(item);
                }
                type = context.types!.Where(s => s.code.CompareTo("ST") == 0).FirstOrDefault();
                if (type == null)
                {
                    SqlType item = new SqlType();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "ST";
                    item.name = "Sang tên";
                    item.des = "Sang tên";
                    item.isdeleted = false;
                    context.types!.Add(item);
                }
                type = context.types!.Where(s => s.code.CompareTo("DKDM") == 0).FirstOrDefault();
                if (type == null)
                {
                    SqlType item = new SqlType();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "DKDM";
                    item.name = "Đăng kí định mức";
                    item.des = "Đăng kí định mức";
                    item.isdeleted = false;
                    context.types!.Add(item);
                }
                int rows = await context.SaveChangesAsync();
            }
        }
    }
}
