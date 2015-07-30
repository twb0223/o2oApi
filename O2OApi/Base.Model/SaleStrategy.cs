using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseData.Model
{
    /// <summary>
    /// 销售策略
    /// </summary>
    public class SaleStrategy
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int SaleStrategyID { get; set; }

        /// <summary>
        /// 策略名称
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string SaleStrategyName { get; set; }


        /// <summary>
        /// 计算公式（例如：{0}*0.8表示8折;{0}>500:-50:表示）
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Formula { get; set; }
             
    }
}
