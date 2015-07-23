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
