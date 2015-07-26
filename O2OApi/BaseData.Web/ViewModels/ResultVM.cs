using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseData.Web.ViewModels
{
    /// <summary>
    /// Post结果返回VM
    /// </summary>
    public class ResultVM
    {
        /// <summary>
        /// 结果状态
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 结果信息
        /// </summary>
        public string Message { get; set; }
    }
}