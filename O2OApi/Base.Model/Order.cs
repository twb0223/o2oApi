using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseData.Model
{
    /// <summary>
    /// 订单记录
    /// </summary>
    public class Order
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        [Key]
        public string OrderID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string OrderNo { get; set; }


        /// <summary>
        /// 订购用户
        /// </summary>
        [Required]
        public string AccountID { get; set; }


        /// <summary>
        /// 状态 0：已提交；1：配送中；2：已收货；3：退货
        /// </summary>
        [Required]
        public int Status { get; set; }

        /// <summary>
        /// 对应配送员
        /// </summary>
        public int? DeliveryManID { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime? OrderDate { get; set; }


        /// <summary>
        /// 订单总金额
        /// </summary>
       
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 订单折扣金额
        /// </summary>
      
        public decimal DiscountAmount { get; set; }


        /// <summary>
        /// 订单实际金额
        /// </summary>
       
        public decimal PayAmount { get; set; }

        ///// <summary>
        ///// 销售策略(折扣，满xx减xx等)
        ///// </summary>
        //[Required]
        //public int SaleStrategyID { get; set; }


        /// <summary>
        /// 订单说明
        /// </summary>
        [MaxLength(400)]
        public string Content { get; set; }

    }
}
