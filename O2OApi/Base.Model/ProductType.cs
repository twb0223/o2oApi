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
    /// 商品类型
    /// </summary>
    public class ProductType
    {
        /// <summary>
        /// 类型ID
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProductTypeID { get; set; }

        /// <summary>
        /// 类型ID
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string ProductTypeName { get; set; }

    }
}
