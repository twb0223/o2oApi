using System;
using System.ComponentModel.DataAnnotations;

namespace BaseData.Model
{
    /// <summary>
    /// 货损记录
    /// </summary>
    public class BreakRecord
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        [Key]
        public string BreakRecordID { get; set; }

        [MaxLength(100)]
        public string ProductCode { get; set; }

        public virtual Product Product { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [Required]
        public int Num { get; set; }

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
