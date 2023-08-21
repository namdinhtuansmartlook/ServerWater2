using Microsoft.AspNetCore.Http.Features;
using Microsoft.OpenApi.Writers;
using Newtonsoft.Json;
using ServerWater2.Models;
using System.Collections.Generic;

namespace ServerWater2.APIs
{
    public class MyViewForm
    {
        public class ItemJson
        {
            public string key { get; set; } = "";
            public string field { get; set; } = "";
            public string value { get; set; } = "";
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
                    item.name = "Survey Form";
                    item.type = "LM";
                    
                    ItemJson itemJson = new ItemJson();
                    itemJson.key = "1";
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

        public async Task<bool> createFormAsync(string code, string name, string type, List<ItemJson> datas)
        {
            if(string.IsNullOrEmpty(code) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(type))
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
                m_form.name = name;
                m_form.type = type;
                
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

        public async Task<bool> editFormAsync(string code, string name, List<ItemJson> datas)
        {
            using (DataContext context = new DataContext())
            {
                SqlViewForm? m_form = context.forms!.Where(s => s.code.CompareTo(code) == 0).FirstOrDefault();
                if (m_form == null)
                {
                    return false;
                }

                m_form.code = code;
                m_form.name = name;
                m_form.data = JsonConvert.SerializeObject(datas);

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

        public async Task<bool> addFieldForm(string code, string key, string field, string label)
        {
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
                item.label = label;

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
                else
                {
                    ItemJson? m_item = tmps.Where(s => s.key.CompareTo(key) == 0 && s.field.CompareTo(field) == 0).FirstOrDefault();
                    if(m_item == null)
                    {
                        return false;
                    }
                    tmps.Remove(m_item);
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

        public class ItemMyJson
        {
            public string code { get; set; } = "";
            public string name { get; set; } = "";
            public string type { get; set; } = "";
            public List<ItemJson> datas { get; set; } = new List<ItemJson>();
        }

        public List<ItemMyJson> getList()
        {
            List<ItemMyJson> items = new List<ItemMyJson>();

            using (DataContext context = new DataContext())
            {
                List<SqlViewForm> forms = context.forms!.Where(s => s.isdeleted == false).ToList();
                if(forms.Count> 0)
                {
                    foreach(SqlViewForm m_item in forms)
                    {
                        ItemMyJson m_form = new ItemMyJson();
                        m_form.code = m_item.code;
                        m_form.name = m_item.name;
                        m_form.type = m_item.type;

                        List<ItemJson>? tmp = JsonConvert.DeserializeObject<List<ItemJson>>(m_item.data);
                        if(tmp != null)
                        {
                            foreach(ItemJson item in tmp)
                            {
                                m_form.datas.Add(item);
                            }    
                        }
                        items.Add(m_form);
                    }    
                }

                return items;
            }    
        }
    }
}
