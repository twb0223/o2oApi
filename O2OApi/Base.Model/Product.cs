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
    /// 商品信息
    /// </summary>
    public class Product
    {
        /// <summary>
        /// 产品条码
        /// </summary>
        [Key]
        [MaxLength(100)]
        public string ProductCode { get; set; }


        /// <summary>
        /// 产品名称
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string ProdcutName { get; set; }

        /// <summary>
        /// 产品描述
        /// </summary>
     
        [MaxLength(500)]
        public string ProdcutDes { get; set; }

        /// <summary>
        /// 产品图片[img1.jpg|img2.jpg|img3.jpg]
        /// </summary>
    
        public string ImgUrl { get; set; }

        /// <summary>
        /// 产品类型ID
        /// </summary>
        [Required]
        public int ProductTypeID { get; set; }
        /// <summary>
        /// 产品类型
        /// </summary>
        public virtual ProductType ProductType { get; set; }

        /// <summary>
        /// 当前售价
        /// </summary>
          [Required]
        public decimal DynamicPrice { get; set; }
 
    }
}
