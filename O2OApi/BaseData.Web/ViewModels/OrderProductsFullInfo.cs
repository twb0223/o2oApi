using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseData.Web.ViewModels
{
    public class OrderProductsFullInfo
    {
        public long OrderDetailsID { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 卖出单价
        /// </summary>
        public decimal Prices { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProdcutName { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
        public string ProdcutDes { get; set; }

        /// <summary>
        /// 产品图片[img1.jpg|img2.jpg|img3.jpg]
        /// </summary>

        public string ImgUrl { get; set; }

        /// <summary>
        /// 产品类型ID
        /// </summary>
        public int ProductTypeID { get; set; }
    }
}