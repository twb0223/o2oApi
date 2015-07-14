using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace BaseData.Web.Areas.HelpPage
{
    public class Parameter
    {
        public void Reg(HttpConfiguration config)
        {
            Assembly _Assembyle = Assembly.GetAssembly(this.GetType());
            List<Type> supportedClass = _Assembyle.GetTypes().Where(t => t.BaseType != null && t.BaseType.Name == "Parameter").ToList();

            foreach (var s in supportedClass)
            {
                string str = "";
                PropertyInfo[] ps = s.GetProperties();
                foreach (var p in ps)
                {
                    switch (p.PropertyType.Name)
                    {
                        case "Guid":
                            str += p.Name + "=" + Guid.NewGuid().ToString();
                            break;
                        case "int":
                        case "long":
                            str += p.Name + "=1000";
                            break;
                        default:
                            str += p.Name + "=" + p.Name;
                            break;
                    }
                    str += "&";


                }
                str = str.TrimEnd('&');
                config.SetSampleForType(str, new MediaTypeHeaderValue("application/x-www-form-urlencoded"), s);
            }
        }
    }
}