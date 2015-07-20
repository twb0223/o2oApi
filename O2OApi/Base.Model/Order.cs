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
        /// 状态 0：已提交；1：配送中；2：已收货；-1：退货
        /// </summary>
        [Required]
        public int Status { get; set; }


        public List<OrderDetail> OrderDetailList { get; set; }

        /// <summary>
        /// 对应配送员
        /// </summary>
        public int? DeliveryManID { get; set; }
    }
}
