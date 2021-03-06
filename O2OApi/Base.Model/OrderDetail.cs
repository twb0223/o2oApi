﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseData.Model
{
    /// <summary>
    /// 订单明细
    /// </summary>
    public class OrderDetail
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long OrderDetailsID { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        [MaxLength(100)]
        [Required]
        public string ProductCode { get; set; }

        public Product Product { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        public int Num { get; set; }

        /// <summary>
        /// 卖出单价
        /// </summary>

        public decimal Prices { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [Required]
        public string OrderID { get; set; }

        public virtual Order Order { get; set; }
    }
}
