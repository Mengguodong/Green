--			UserInfo										
--UserId	UserName	Pwd	PayPwd	IdCard	BankNumber	Phone							
--用户名	登录名	密码	交易密码	身份证号	银行卡号	电话号	直推ID	左区ID	右区ID	团队父ID	激活	真实姓名	创建时间
													
--			AccountInfo										
													
--AccountId	UserId												
--		            总资产	ep	Zfc	积分	左区业绩	右区业绩						
													
--			OrderInfo										
													
--OrderId  卖方用户Id	  买方用户Id	类型	状态	数量	总价值	创建时间	备注					
													
--			LogInfo										
--LogId	Type		Userid										
--	1 :充值  2：提现 3：静态收益记录 4 ：动态收益记录  5：ep复投记录	创建时间		数量									
													
--			ConfigInfo										
--	Key	Value											
------------------------------=====================
create table UserInfo (
UserId int primary key ,
UserName varchar(250),   --登录id
Pwd varchar(250),        --登录密码
PayPwd varchar(250),     --交易密码
IdCard varchar(250),     --身份证号
BankName varchar(250),   --银行名称
BankNumber varchar(250), --银行卡号
Phone varchar(250),      --电话号
ParentId int,            --直推父级ID
LeftId varchar(250),     --左区团队ID
RightId varchar(250),    --右区团队ID
TeamParentId varchar(250),-- 父级团队ID
IsActivation int ,        --1:激活，0:未激活
RealName varchar(250),    --真实姓名
CreateTime datetime,
TeamTime datetime,            --加入团队时间
TeamType int,              --1:加入团队，0:未加入团队
[level] int,               --等级
UserStatus int,           --用户状态
UserType int,              --用户类型
TodayIsLogin int,          --今日是否登录
YesTodayIsLogin int,        --昨日是否登录
ViolationsCount int          ---违规次数
) 

create table AccountInfo (
AccountId int primary key ,
AccountName varchar(250), --  用户钱包地址  guid32位字符串
UserId int,               --  用户id
Ep decimal(18,4),         
ZFC decimal(18,4),
ActivationCount int,
LeftAchievement decimal(18,2), --左区业绩
RightAchievement decimal(18,2),--右区业绩
WaitRelease decimal(18,4),     --动态待释放
StaticsRelease decimal(18,4),   --静态待释放
CreateTime datetime,
TotalAssets  decimal(18,2),    -------总资产
FreezeEp decimal(18,4),
FreezeZfc decimal(18,4),  
RightCount int ,
LeftCount int  

         
) 

create table OrderInfo (
OrderId int primary key ,
BuyUserId int,                 --买方用户ID
BuyAccountName varchar(250),   --买方账户钱包地址
SellUserId int,                --卖方用户ID
SellAccountName varchar(250),  --卖方账户钱包地址
IsMoney  int,                  --是否是现金交易
OrderType int ,                --交易类型   1:卖EP，2:卖Zfc
Sataus int ,                   --1:在售，2:已锁定，3:待确认，4:已完成5已取消6已冻结
Qty int,                       --卖出数量
BuyMoney decimal(18,4),         --购买总值
CreateTime datetime,
Remark  varchar(250),
UpdateTime date ,              -------修改时间
imgPath varchar(250)   -------------打款凭证图片路径                               
) 



create table LogInfo (
LogId int primary key ,
LogType int,                 --1 :充值  2：提现 3：静态收益记录 4 ：动态收益记录  5：ep复投记录
UserId int,                     
Number decimal(18,4),--数量                                  （只在充值，复投的时候用）
AdminLogType int,             --1.EP   (2.Zfc)        （只在充值的时候用）     
CreateTime datetime,
Ep decimal(18,4),             -------EP           (只在静态收益，动态收益时候用)
Zfc decimal(18,4)            -------Zfc                  
) 





create table ConfigInfo (
ConfKey varchar(250) primary key,
ConfValue varchar(250)               
                  
) 

create table CrossRotation
(
    Id int primary key,
    CRUserId int,                 --转方用户ID
CRAccountName varchar(250),      --买方账户钱包地址
REUserId int,                   --卖方用户ID
REAccountName varchar(250),     --卖方账户钱包地址  
CRType int ,                   --交易类型   1:转EP，2:转ZFC
Qty int,                       --卖出数量
CreateTime datetime,
Remark  varchar(250)         
    
)


create table CreateActivation
(
	Id int primary key,
	UserId int,                 --转方用户ID
	ActivationCount int,      --买方账户钱包地址
	CAType int ,              --1:自己兑换  2:系统添加
	CreateTime datetime, 
	Remark  varchar(250)         
    
)

create table ActivationTable
(
    Id int primary key,
	UserId int,                 --使用激活卡用户ID
	ReUserId int ,              --被激活人用户ID
	CreateTime datetime, 
	Remark  varchar(250) ,       
  ActivationType int,           --1:使用激活卡  2:赠送
  ReUserName varchar(250)       --被激活人登录账号
)
create table ShopRecord 
(
    Id int primary key,
	UserName varchar(250),                 

	CreateTime datetime, 
	Remark  varchar(250) ,    
	ShopType int ,   
  Number decimal(18,4)
)
  
insert into ConfigInfo  (ConfKey,ConfValue) values ('ZfcPrice','1')

insert into UserInfo (UserName,Pwd,IsActivation,CreateTime,UserStatus,UserType) values ('admin','96C13712437D09B4',1,GETDATE(),1,2)

insert INTO UserInfo (UserName,Pwd,IsActivation,CreateTime,UserStatus,UserType,LeftId,RightId)values ('13787136188','96C13712437D09B4',1,GETDATE(),1,1,'qwe123','asd123')

insert into AccountInfo (UserId,CreateTime,AccountName)values('2',GETDATE(),'werty4u5i2bn5mdfghj')
