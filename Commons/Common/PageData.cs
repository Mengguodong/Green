using System.Collections.Generic;

namespace Common
{
    /// <summary>
    /// 分页操作类
    /// </summary>
    public class PageData<T>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PageData()
        {
        }

        /// <summary>
        ///构造函数
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页显示记录数</param>
        /// <param name="totalCount">总记录数</param>
        /// <param name="resultData">结果集</param>
        public PageData(int pageIndex, int pageSize, int totalCount, IList<T> resultData)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            TotalCount = totalCount;
            ResultData = resultData;
        }

        /// <summary>
        ///页数
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        ///页记录数
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        ///总页数
        /// </summary>
        public int TotalPage { get; set; }

        /// <summary>
        ///结果集
        /// </summary>
        public IList<T> ResultData { get; set; }

       
    }
}