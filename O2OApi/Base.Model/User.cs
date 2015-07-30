using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseData.Model
{
    /// <summary>
    /// 系统用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }

        /// <summary>
        /// 用户账号（登陆使用）
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string UserAccount { get; set; }

        /// <summary>
        /// 用户名称(显示用)
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否可用 True:可用，false：禁用
        /// </summary>
        public bool Enable { get; set; }

        
        public int AreaID { get; set; }

        public virtual Area Area { get; set; }


    }
}
