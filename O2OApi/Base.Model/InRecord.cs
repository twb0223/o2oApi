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
    /// 进货记录
    /// </summary>
    public class InRecord
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        [Key]
        public string InRecordID { get; set; }

        /// <summary>
        /// 产品条码
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string ProductCode { get; set; }

        public virtual Product Product { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        public int Num { get; set; }

        /// <summary>
        /// 单品进价
        /// </summary>
        [Required]
        public decimal InPrice { get; set; }


        /// <summary>
        /// 经办人
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string CreateBy { get; set; }


        /// <summary>
        /// 经办时间
        /// </summary>
        [Required]
        public DateTime CreateAt { get; set; }
    }
}
