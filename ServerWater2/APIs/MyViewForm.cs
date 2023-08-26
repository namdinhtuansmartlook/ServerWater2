using Microsoft.Data.SqlClient.DataClassification;
using Newtonsoft.Json;
using ServerWater2.Models;
using System.Data;

namespace ServerWater2.APIs
{
    public class MyViewForm
    {
        public class ItemJson
        {
            public string key { get; set; } = "";
            public string index { get; set; } = "";
            public string field { get; set; } = "";
            public string value { get; set; } = "";
            public string stateImage { get; set; } = "";
            public string label { get; set; } = "";
        }

        public async Task initAsync()
        {
            List<ItemJson> items = new List<ItemJson>();

            using (DataContext context = new DataContext())
            {
                SqlViewForm? m_form = context.forms!.Where(s => s.code.CompareTo("SVF") == 0 && s.isdeleted == false).FirstOrDefault();
                if (m_form == null)
                {
                    SqlViewForm item = new SqlViewForm();
                    item.ID = DateTime.Now.Ticks;
                    item.code = "SVF";
                   
                    ItemJson itemJson = new ItemJson();
                    itemJson.key = "1";
                    itemJson.index = "1";
                    itemJson.field = "address";
                    itemJson.value = "110 Bui Ta Han";
                    itemJson.label = "string";

                    items.Add(itemJson);

                    item.data = JsonConvert.SerializeObject(items);

                    item.isdeleted = false;
                    context.forms!.Add(item);
                }


                int rows = await context.SaveChangesAsync();
            }
        }

        public async Task<bool> createFormAsync(string code, List<ItemJson> datas)
        {
            if(string.IsNullOrEmpty(code))
            {
                return false;
            }    
            using(DataContext context = new DataContext())
            {
                SqlViewForm? m_form = context.forms!.Where(s => s.code.CompareTo(code) == 0).FirstOrDefault();
                if(m_form != null)
                {
                    return false;
                }

                m_form = new SqlViewForm();
                m_form.ID = DateTime.Now.Ticks;
                m_form.code = code;
                
                m_form.data = JsonConvert.SerializeObject(datas);
                m_form.isdeleted = false;

                context.forms!.Add(m_form);

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }    
        }
        
        public async Task<bool> deleteFormAsync(string code)
        {
            using (DataContext context = new DataContext())
            {
                SqlViewForm? m_form = context.forms!.Where(s => s.code.CompareTo(code) == 0).FirstOrDefault();
                if (m_form == null)
                {
                    return false;
                }

                m_form.isdeleted = true;
                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> addFieldForm(string code, string key, string index, string field, string label, string state)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(index) || string.IsNullOrEmpty(field) || string.IsNullOrEmpty(label))
            {
                return false;
            }

            using (DataContext context = new DataContext())
            {
                SqlViewForm? m_form = context.forms!.Where(s => s.code.CompareTo(code) == 0).FirstOrDefault();
                if (m_form == null)
                {
                    return false;
                }
                ItemJson item = new ItemJson();
                item.key = key;
                item.field = field;
                item.index = index;
                item.label = label;

                if(!string.IsNullOrEmpty(state))
                {
                    item.stateImage = state;
                }

                List<ItemJson>? tmps = JsonConvert.DeserializeObject<List<ItemJson>>(m_form.data);
                if(tmps == null)
                {
                    tmps = new List<ItemJson>();

                    tmps.Add(item);

                }
                else
                {
                    tmps.Add(item);
                }

                m_form.data = JsonConvert.SerializeObject(tmps);
                
                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> removeFieldForm(string code, string key, string field)
        {
            using (DataContext context = new DataContext())
            {
                SqlViewForm? m_form = context.forms!.Where(s => s.code.CompareTo(code) == 0).FirstOrDefault();
                if (m_form == null)
                {
                    return false;
                }
                
                List<ItemJson>? tmps = JsonConvert.DeserializeObject<List<ItemJson>>(m_form.data);
                if (tmps == null)
                {
                    return false;

                }

                ItemJson? m_item = tmps.Where(s => s.key.CompareTo(key) == 0 && s.field.CompareTo(field) == 0).FirstOrDefault();
                if (m_item == null)
                {
                    return false;
                }
                tmps.Remove(m_item);
                m_form.data = JsonConvert.SerializeObject(tmps);

                int rows = await context.SaveChangesAsync();
                if (rows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public class ItemCodeForm
        {
            public string code { get; set; } = "";
        }

        public List<ItemCodeForm> getListCode()
        {
            List<ItemCodeForm> items = new List<ItemCodeForm>();

            using (DataContext context = new DataContext())
            {
                List<SqlViewForm> forms = context.forms!.Where(s => s.isdeleted == false).ToList();
                if (forms.Count > 0)
                {
                    foreach (SqlViewForm m_item in forms)
                    {
                        ItemCodeForm m_form = new ItemCodeForm();
                        m_form.code = m_item.code;

                        
                        items.Add(m_form);
                    }
                }

                return items;
            }
        }


        public List<ItemJson> getForm(string code)
        {

            using (DataContext context = new DataContext())
            {
                SqlViewForm? m_form = context.forms!.Where(s => s.isdeleted == false && s.code.CompareTo(code) == 0).FirstOrDefault();
                if(m_form == null)
                {
                    return new List<ItemJson>();
                }

                List<ItemJson>? tmps = JsonConvert.DeserializeObject<List<ItemJson>>(m_form.data);
                if(tmps == null)
                {
                    return new List<ItemJson>();
                }    

                return tmps;
            }
        }
    }
}
