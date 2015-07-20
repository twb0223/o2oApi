﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseData.Model
{
    public class DeliveryMan
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int DeliveryManID { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string DeliveryManName { get; set; }


        /// <summary>
        /// 身份证
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string IDNumber { get; set; }


        /// <summary>
        /// 联系方式
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Mobile { get; set; }


        /// <summary>
        /// 所属区域
        /// </summary>
        [Required]
        public int AreaID { get; set; }

        public virtual Area Area { get; set; }
    }
}
