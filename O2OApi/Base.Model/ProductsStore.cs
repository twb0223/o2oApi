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
    /// 产品库存表
    /// </summary>
    public class ProductsStore
    {
        /// <summary>
        /// 主键ID
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long ProductsStoreID { get; set; }

        /// <summary>
        /// 商品条码
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ProductCode { get; set; }

        public virtual Product Product { get; set; }

        /// <summary>
        /// 历史进货总量
        /// </summary>
        [Required]
        public long TotalInNum { get; set; }

        /// <summary>
        /// 历史销售总量
        /// </summary>
        [Required]
        public long TotalSaleNum { get; set; }

        /// <summary>
        /// 历史损坏总量
        /// </summary>
        [Required]
        public long TotalBreakNum { get; set; }


    }
}
