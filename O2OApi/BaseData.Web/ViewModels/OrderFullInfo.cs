using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BaseData.Model;

namespace BaseData.Web.ViewModels
{
    public class OrderFullInfo
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 顾客账号ID
        /// </summary>
        public string AccountID { get; set; }

        /// <summary>
        /// 顾客名称
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 顾客名称
        /// </summary>
        public string AccountMobile { get; set; }

        /// <summary>
        /// 顾客名称
        /// </summary>
        public string AccountAddress { get; set; }

        /// <summary>
        /// 订单产品列表
        /// </summary>
        public ICollection<OrderProductsFullInfo> ProductList { get; set; }

       // public int DeliverManID { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime? OrderDate { get; set; }

        /// <summary>
        /// 总价
        /// </summary>

        public decimal TotalAmount { get; set; }


        /// <summary>
        /// 优惠金额
        /// </summary>

        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal PayAmount { get; set; }
    }
}