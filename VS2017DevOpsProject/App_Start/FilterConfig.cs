﻿using System.Web;
using System.Web.Mvc;

namespace VS2017DevOpsProject
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
