using System;
using System.ComponentModel;
using System.Reflection;


namespace Common.Web
{
    ///// <summary>
    /////     登录信息数据实体类
    /////     创建人:menggd
    /////     创建时间:2014-10-28
    ///// </summary>
    //public class LoginUserInfo
    //{
    //    private int userId;
    //    /// <summary>
    //    /// 用户ID
    //    /// </summary>
    //    public int UserId
    //    {
    //        get { return userId; }
    //        set { userId = value; }
    //    }
    //    /// <summary>
    //    /// 用户名
    //    /// </summary>
    //    private string userName;
    //    public string UserName
    //    {
    //        get { return userName; }
    //        set { userName = value; }
    //    }

      
    //    /// <summary>
    //    /// 真实姓名
    //    /// </summary>
    //    private string realName;
    //    public string RealName
    //    {
    //        get { return realName; }
    //        set { realName = value; }
    //    }
    //    /// <summary>
    //    /// 创建时间
    //    /// </summary>
    //    private DateTime createTime;
    //    public DateTime CreateTime
    //    {
    //        get { return createTime; }
    //        set { createTime = value; }
    //    }
    //    /// <summary>
    //    /// 用户状态
    //    /// </summary>
    //    private int userStatues;
    //    public int UserStatues
    //    {
    //        get { return userStatues; }
    //        set { userStatues = value; }
    //    }
    //    /// <summary>
    //    /// 是否删除
    //    /// </summary>
    //    private int isDelete;
    //    public int IsDelete
    //    {
    //        get { return isDelete; }
    //        set { isDelete = value; }
    //    }
    //    /// <summary>
    //    /// 父级ID
    //    /// </summary>
    //    private int parentId;
    //    public int ParentId
    //    {
    //        get { return parentId; }
    //        set { parentId = value; }
    //    }
    //    /// <summary>
    //    /// 用户类型
    //    /// </summary>
    //    private UserType userType;
    //    public UserType UserType
    //    {
    //        get { return userType; }
    //        set { userType = value; }
    //    }
    //    /// <summary>
    //    /// 等级ID
    //    /// </summary>
    //    private int levelId;
    //    public int LevelId
    //    {
    //        get { return levelId; }
    //        set { levelId = value; }
    //    }
    //    /// <summary>
    //    /// 下级数量
    //    /// </summary>
    //    private int downCount;
    //    public int DownCount
    //    {
    //        get { return downCount; }
    //        set { downCount = value; }
    //    }
    //    /// <summary>
    //    /// 手机号
    //    /// </summary>
    //    private string mobilePhone;
    //    public string MobilePhone
    //    {
    //        get { return mobilePhone; }
    //        set { mobilePhone = value; }
    //    }
    //   /// <summary>
    //   /// 是否成团
    //   /// </summary>
    //    private int isTeam;
    //    public int IsTeam
    //    {
    //        get { return isTeam; }
    //        set { isTeam = value; }
    //    }
    //    /// <summary>
    //    /// 是否收回本金
    //    /// </summary>
    //    private int isReturn;
    //    public int IsReturn
    //    {
    //        get { return isReturn; }
    //        set { isReturn = value; }
    //    }

    //    /// <summary>
    //    /// 最高等级
    //    /// </summary>
    //    private int bestLevel;
    //    public int BestLevel
    //    {
    //        get { return bestLevel; }
    //        set { bestLevel = value; }
    //    }
    //    /// <summary>
    //    /// 是否测试账号
    //    /// </summary>
    //    private int isTestAccount;
    //    public int IsTestAccount
    //    {
    //        get { return isTestAccount; }
    //        set { isTestAccount = value; }
    //    }
    //    /// <summary>
    //    /// 是否已激活
    //    /// </summary>
    //    public int IsActivate { get; set; }
      
    //    /// <summary>
    //    /// 等级
    //    /// </summary>
    //    public string LevelType
    //    {
    //        get
    //        {
    //            if (LevelId != 0)
    //            {
    //                return GetDescription((EnumLevelType)LevelId);
    //            }
    //            else
    //            {
    //                return "";
    //            }
    //        }
    //    }

    //    /// <summary>
    //    /// 获取枚举变量值的 Description 属性
    //    /// </summary>
    //    /// <param name="obj">枚举变量</param>
    //    /// <param name="isTop">是否改变为返回该类、枚举类型的头 Description 属性，而不是当前的属性或枚举变量值的 Description 属性</param>
    //    /// <returns>如果包含 Description 属性，则返回 Description 属性的值，否则返回枚举变量值的名称</returns>
    //    public string GetDescription(object obj)
    //    {
    //        bool isTop = false;
    //        if (obj == null)
    //        {
    //            return string.Empty;
    //        }
    //        try
    //        {
    //            Type _enumType = obj.GetType();
    //            DescriptionAttribute dna = null;
    //            if (isTop)
    //            {
    //                dna = (DescriptionAttribute)Attribute.GetCustomAttribute(_enumType, typeof(DescriptionAttribute));
    //            }
    //            else
    //            {
    //                FieldInfo fi = _enumType.GetField(Enum.GetName(_enumType, obj));
    //                dna = (DescriptionAttribute)Attribute.GetCustomAttribute(
    //                   fi, typeof(DescriptionAttribute));
    //            }
    //            if (dna != null && string.IsNullOrEmpty(dna.Description) == false)
    //                return dna.Description;
    //        }
    //        catch
    //        {
    //        }
    //        return obj.ToString();
    //    }
      
    //    public enum EnumLevelType
    //    {
    //        [Description("酿酒工")]
    //        JiuNong = 1,
    //        [Description("酒坊")]
    //        ZuoFang = 2,
    //        [Description("酒坊主")]
    //        CheJian = 3,
    //        [Description("庄园")]
    //        JiuChang = 4
    //    }
   

    //}

}
