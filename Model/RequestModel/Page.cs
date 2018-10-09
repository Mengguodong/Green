using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.RequestModel
{
    /// <summary>
    /// 分页数据返回实体
    /// Author：m
    /// Date：2016年12月22日17:30:47
    /// </summary>
    /// <typeparam name="T"></typeparam>
   public class Page<T>
    {
       /// <summary>
       /// 当前页码
       /// </summary>
        public int PageIndex { get; set; }
       /// <summary>
       /// 每页条数
       /// </summary>
        public int PageSize { get; set; }
       /// <summary>
       /// 数据总条数
       /// </summary>
        public int TotalCount { get; set; }
       /// <summary>
       /// 当前页数据
       /// </summary>
        public List<T> Data { get; set; }
    }
}
