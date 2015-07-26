using BaseData.Web.Areas.HelpPage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BaseData.Web.ViewModels
{
    public class PostOrderInfoVM : Parameter
    {
        /// <summary>
        /// 订购用户账号
        /// </summary>
        public string AccountID { get; set; }

        /// <summary>
        /// 订单明细
        /// </summary>
        public ICollection<PostProductListVM> ProductList { get; set; }

        /// <summary>
        /// 订单总金额
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 打折金额
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 应付金额
        /// </summary>
        public decimal PayAmount { get; set; }

    }

    public class PostProductListVM : Parameter
    {
        /// <summary>
        /// 商品条码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 单品售价
        /// </summary>
        public decimal Prices { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }


    }
}