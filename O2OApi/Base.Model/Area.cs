using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseData.Model
{
    /// <summary>
    /// 区域
    /// </summary>
    public class Area
    {
        /// <summary>
        /// 区域ID
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int AreaID { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string AreaName { get; set; }

        /// <summary>
        /// 所属城市ID
        /// </summary>
        [Required]
        public int CityID { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public virtual City City { get; set; }
    }
}
