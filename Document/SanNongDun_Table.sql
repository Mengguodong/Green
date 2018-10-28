--			UserInfo										
--UserId	UserName	Pwd	PayPwd	IdCard	BankNumber	Phone							
--�û���	��¼��	����	��������	���֤��	���п���	�绰��	ֱ��ID	����ID	����ID	�ŶӸ�ID	����	��ʵ����	����ʱ��
													
--			AccountInfo										
													
--AccountId	UserId												
--		            ���ʲ�	ep	Zfc	����	����ҵ��	����ҵ��						
													
--			OrderInfo										
													
--OrderId  �����û�Id	  ���û�Id	����	״̬	����	�ܼ�ֵ	����ʱ��	��ע					
													
--			LogInfo										
--LogId	Type		Userid										
--	1 :��ֵ  2������ 3����̬�����¼ 4 ����̬�����¼  5��ep��Ͷ��¼	����ʱ��		����									
													
--			ConfigInfo										
--	Key	Value											
------------------------------=====================
create table UserInfo (
UserId int primary key ,
UserName varchar(250),   --��¼id
Pwd varchar(250),        --��¼����
PayPwd varchar(250),     --��������
IdCard varchar(250),     --���֤��
BankName varchar(250),   --��������
BankNumber varchar(250), --���п���
Phone varchar(250),      --�绰��
ParentId int,            --ֱ�Ƹ���ID
LeftId varchar(250),     --�����Ŷ�ID
RightId varchar(250),    --�����Ŷ�ID
TeamParentId varchar(250),-- �����Ŷ�ID
IsActivation int ,        --1:���0:δ����
RealName varchar(250),    --��ʵ����
CreateTime datetime,
TeamTime datetime,            --�����Ŷ�ʱ��
TeamType int,              --1:�����Ŷӣ�0:δ�����Ŷ�
[level] int,               --�ȼ�
UserStatus int,           --�û�״̬
UserType int,              --�û�����
TodayIsLogin int,          --�����Ƿ��¼
YesTodayIsLogin int,        --�����Ƿ��¼
ViolationsCount int          ---Υ�����
) 

create table AccountInfo (
AccountId int primary key ,
AccountName varchar(250), --  �û�Ǯ����ַ  guid32λ�ַ���
UserId int,               --  �û�id
Ep decimal(18,4),         
ZFC decimal(18,4),
ActivationCount int,
LeftAchievement decimal(18,2), --����ҵ��
RightAchievement decimal(18,2),--����ҵ��
WaitRelease decimal(18,4),     --��̬���ͷ�
StaticsRelease decimal(18,4),   --��̬���ͷ�
CreateTime datetime,
TotalAssets  decimal(18,2),    -------���ʲ�
FreezeEp decimal(18,4),
FreezeZfc decimal(18,4),  
RightCount int ,
LeftCount int  

         
) 

create table OrderInfo (
OrderId int primary key ,
BuyUserId int,                 --���û�ID
BuyAccountName varchar(250),   --���˻�Ǯ����ַ
SellUserId int,                --�����û�ID
SellAccountName varchar(250),  --�����˻�Ǯ����ַ
IsMoney  int,                  --�Ƿ����ֽ���
OrderType int ,                --��������   1:��EP��2:��Zfc
Sataus int ,                   --1:���ۣ�2:��������3:��ȷ�ϣ�4:�����5��ȡ��6�Ѷ���
Qty int,                       --��������
BuyMoney decimal(18,4),         --������ֵ
CreateTime datetime,
Remark  varchar(250),
UpdateTime date ,              -------�޸�ʱ��
imgPath varchar(250)   -------------���ƾ֤ͼƬ·��                               
) 



create table LogInfo (
LogId int primary key ,
LogType int,                 --1 :��ֵ  2������ 3����̬�����¼ 4 ����̬�����¼  5��ep��Ͷ��¼
UserId int,                     
Number decimal(18,4),--����                                  ��ֻ�ڳ�ֵ����Ͷ��ʱ���ã�
AdminLogType int,             --1.EP   (2.Zfc)        ��ֻ�ڳ�ֵ��ʱ���ã�     
CreateTime datetime,
Ep decimal(18,4),             -------EP           (ֻ�ھ�̬���棬��̬����ʱ����)
Zfc decimal(18,4)            -------Zfc                  
) 





create table ConfigInfo (
ConfKey varchar(250) primary key,
ConfValue varchar(250)               
                  
) 

create table CrossRotation
(
    Id int primary key,
    CRUserId int,                 --ת���û�ID
CRAccountName varchar(250),      --���˻�Ǯ����ַ
REUserId int,                   --�����û�ID
REAccountName varchar(250),     --�����˻�Ǯ����ַ  
CRType int ,                   --��������   1:תEP��2:תZFC
Qty int,                       --��������
CreateTime datetime,
Remark  varchar(250)         
    
)


create table CreateActivation
(
	Id int primary key,
	UserId int,                 --ת���û�ID
	ActivationCount int,      --���˻�Ǯ����ַ
	CAType int ,              --1:�Լ��һ�  2:ϵͳ���
	CreateTime datetime, 
	Remark  varchar(250)         
    
)

create table ActivationTable
(
    Id int primary key,
	UserId int,                 --ʹ�ü���û�ID
	ReUserId int ,              --���������û�ID
	CreateTime datetime, 
	Remark  varchar(250) ,       
  ActivationType int,           --1:ʹ�ü��  2:����
  ReUserName varchar(250)       --�������˵�¼�˺�
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
