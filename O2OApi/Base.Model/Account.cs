using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseData.Model
{
    /// <summary>
    /// 账户信息
    /// </summary>
    public class Account
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public long AccountID { get; set; }

        /// <summary>
        /// 账号名称
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string AccountNo { get; set; }

        /// <summary>
        /// 账号密码
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string AccountPwd { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// 配送地址
        /// </summary>
        [MaxLength(300)]
        public string DispatchAddress { get; set; }

        /// <summary>
        /// 收货人姓名
        /// </summary>
        [MaxLength(20)]
        public string ConnectName { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        [MaxLength(20)]
        public string Mobile { get; set; }

        /// <summary>
        /// 微信等第三方openid
        /// </summary>
        [MaxLength(100)]
        public string OpenID { get; set; }

        /// <summary>
        /// 账号类型；wechat,weibo,qq,reg
        /// </summary>
        [MaxLength(100)]
        public string AccountType { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        [Required]
        public DateTime RegDate { get; set; }

        /// <summary>
        /// 最后登录时间
        /// </summary>
        [Required]
        public DateTime LastLoginDate { get; set; }


    }
}
