using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using NewsPublish.Model.Entity;
using NewsPublish.Model.Request;
using NewsPublish.Model.Response;

namespace NewsPublish.Service
{
    /// <summary>
    /// News service.
    /// </summary>
    public class NewsService
    {
        private Db _db;
        public NewsService(Db db)
        {
            _db = db;
        }
        /// <summary>
        /// Adds the news classify.
        /// </summary>
        /// <returns>The news classify.</returns>
        /// <param name="newsClassify">News classify.</param>
        public ResponseModel AddNewsClassify(AddNewsClassify newsClassify)
        {
            var exit = _db.NewsClassify.FirstOrDefault(c => c.Name == newsClassify.Name) != null;
            if (exit)
                return new ResponseModel { code = 0, result = "该类别已经存在" };
            var classify = new NewsClassify() { Name = newsClassify.Name, Sort = newsClassify.Sort, Remark = newsClassify.Remark };
            _db.NewsClassify.Add(classify);
            int i = _db.SaveChanges();
            if (i > 0)
                return new ResponseModel { code = 200, result = "新闻类别添加成功" };
            return new ResponseModel { code = 0, result = "新闻类别添加失败" };

        }
        /// <summary>
        /// Gets the one news classify.
        /// </summary>
        /// <returns>The one news classify.</returns>
        /// <param name="id">Identifier.</param>
        public ResponseModel GetOneNewsClassify(int id)
        {
            var classify = _db.NewsClassify.Find(id);
            if (classify == null)
                return new ResponseModel() { code = 0, result = "该类别不存在" };
            return new ResponseModel()
            {
                code = 200,
                result = "添加类别成功",
                data = new NewsClassifyModel
                {
                    Id = classify.Id,
                    Name = classify.Name,
                    Sort = classify.Sort,
                    Remark = classify.Remark
                }
            };
        }
        /// <summary>
        /// Gets the one news classify.
        /// </summary>
        /// <returns>The one news classify.</returns>
        /// <param name="where">Where.</param>
        private NewsClassify GetOneNewsClassify(Expression<Func<NewsClassify, bool>> where)
        {
            return _db.NewsClassify.FirstOrDefault(where);
        }
        /// <summary>
        /// Edits the news classify.
        /// </summary>
        /// <returns>The news classify.</returns>
        /// <param name="newsClassify">News classify.</param>
        public ResponseModel EditNewsClassify(EditNewsClassify newsClassify)
        {
            var classify = this.GetOneNewsClassify(c => c.Id == newsClassify.Id);
            if (classify == null)
                return new ResponseModel() { code = 0, result = "该类别不存在" };
            classify.Id = newsClassify.Id;
            classify.Name = newsClassify.Name;
            classify.Sort = newsClassify.Sort;
            classify.Remark = newsClassify.Remark;
            _db.NewsClassify.Update(classify);
            int i = _db.SaveChanges();
            if (i > 0)
                return new ResponseModel { code = 200, result = "新闻类别编辑成功" };
            return new ResponseModel { code = 0, result = "新闻类别编辑失败" };
        }
        /// <summary>
        /// Gets the news classify list.
        /// </summary>
        /// <returns>The news classify list.</returns>
        public ResponseModel GetNewsClassifyList()
        {

            var classifys = _db.NewsClassify.OrderByDescending(c => c.Sort).ToList();
            var response = new ResponseModel { code = 200, result = "新闻类别集合获取成功" };
            response.data = new List<NewsClassifyModel>();
            foreach (var classify in classifys)
            {
                response.data.Add(new NewsClassifyModel
                {
                    Id = classify.Id,
                    Name = classify.Name,
                    Sort = classify.Sort,
                    Remark = classify.Remark
                });
            }
            return response;
        }
        /// <summary>
        /// Adds the news.
        /// </summary>
        /// <returns>The news.</returns>
        /// <param name="addNews">Add news.</param>
        public ResponseModel AddNews(AddNews addNews)
        {
            try
            {
                var classify = this.GetOneNewsClassify(c => c.Id == addNews.NewsClassifyId);
                if (classify == null)
                    return new ResponseModel() { code = 0, result = "没有对应的分类" };
                var n = new News
                {
 NewsClassifyId = addNews.NewsClassifyId,
                Title = addNews.Title,
                Image = addNews.Image,
                Contents = addNews.Contents,
                PublishDate = DateTime.Now,
                Remark = addNews.Remark
                
                };
                _db.News.Add(n);
                int i = _db.SaveChanges();
                if (i > 0)
                    return new ResponseModel { code = 200, result = "新闻添加成功" };
                return new ResponseModel { code = 0, result = "新闻添加失败" };
            }
            catch (Exception ex)
            {
                return new ResponseModel { code = 0, result = ex.Message.ToString() };
            }
        }
        /// <summary>
        /// Gets the one news.
        /// </summary>
        /// <returns>The one news.</returns>
        /// <param name="id">Identifier.</param>
        public ResponseModel GetOneNews(int id)
        {
            var news = _db.News.Include("NewsClassify").Include("NewsComment").FirstOrDefault(c => c.Id == id);
            if (news != null)
                return new ResponseModel { code = 0, result = "没有找到该ID新闻" };
            return new ResponseModel
            {
                code = 200,
                result = "新闻获取成功",
                data = new NewsModel
                {
                    Id = news.Id,
                    ClassifyName = news.NewClassify.Name,
                    Title = news.Title,
                    Image = news.Image,
                    Contents = news.Contents,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComments.Count(),
                    Remark = news.Remark
                }
            };
        }
        /// <summary>
        /// Deletes the one news.
        /// </summary>
        /// <returns>The one news.</returns>
        /// <param name="id">Identifier.</param>
        public ResponseModel DeleteOneNews(int id)
        {
            var news = _db.News.FirstOrDefault(c => c.Id == id);
            if (news == null)
                return new ResponseModel { code = 0, result = "没有找到该ID新闻" };
            _db.News.Remove(news);
            int i = _db.SaveChanges();
            if (i > 0)
                return new ResponseModel { code = 200, result = "新闻删除成功" };
            return new ResponseModel { code = 0, result = "新闻删除失败" };
        }

        /// <summary>
        /// Newses the page query.
        /// </summary>
        /// <returns>The page query.</returns>
        public ResponseModel NewsPageQuery(int pageSize, int PageIndex, out int total, List<Expression<Func<News, bool>>> where)
        {

            var list = _db.News.Include("NewClassify").Include("NewsComments");
            foreach (var item in where)
            {
                list = list.Where(item);
            }
            total = list.Count();

            var templist = list.ToList();
            var pageData = list.OrderByDescending(c => c.PublishDate).Skip(pageSize * (PageIndex - 1)).Take(pageSize).ToList();

            var response = new ResponseModel
            {
                code = 200,
                result = "分页新闻获取成功"
            };
            response.data = new List<NewsModel>();
            foreach (var news in pageData)
            {
                response.data.Add(new NewsModel
                {
                    Id = news.Id,
                    ClassifyName = news.NewClassify.Name,
                    Title = news.Title,
                    Image = news.Image,
                    Contents = news.Contents,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComments.Count(),
                    Remark = news.Remark
                });
            }
            return response;



        }

        /// <summary>
        /// Gets the new list.
        /// </summary>
        /// <returns>The new list.</returns>
        /// <param name="where">Where.</param>
        /// <param name="count">Count.</param>
        public ResponseModel GetNewList(Expression<Func<News, bool>> where, int count)
        {
            try
            {
                var list = _db.News.Include("NewClassify").Include("NewsComments").Where(where).OrderByDescending(c => c.PublishDate).Take(count).ToList();
                var response = new ResponseModel
                {
                    code = 200,
                    result = "新闻列表获取成功"
                };
             
                response.data = new List<NewsModel>();
                foreach (var news in list)
                {
                    response.data.Add(new NewsModel
                    {
                        Id = news.Id,
                        ClassifyName = news.NewClassify.Name,
                        Title = news.Title,
                        Image = news.Image,
                        Contents = news.Contents.Length > 50 ? news.Contents.Substring(0, 50) : news.Contents,
                        PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                        CommentCount = news.NewsComments.Count(),
                        Remark = news.Remark
                    });
                }
                return response;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the new comment news list.
        /// </summary>
        /// <returns>The news comment news list.</returns>
        /// <param name="where">Where.</param>
        /// <param name="topCount">Top count.</param>
        public ResponseModel GetNewCommentNewsList(Expression<Func<News, bool>> where, int topCount)
        {
            var newsIds = _db.NewsComment.OrderByDescending(c => c.AddTime).
                             GroupBy(c => c.NewsId).Select(c => c.Key).Take(topCount);
            var list = _db.News.Include("NewsClassify").Include("NewsComment").
                          Where(c => newsIds.Contains(c.Id)).OrderByDescending(c => c.PublishDate);
            var response = new ResponseModel
            {
                code = 200,
                result = "最新评论新闻获取成功"
            };
            response.data = new List<NewsModel>();
            foreach (var news in list)
            {
                response.data.Add(new NewsModel
                {
                    Id = news.Id,
                    ClassifyName = news.NewClassify.Name,
                    Title = news.Title,
                    Image = news.Image,
                    Contents = news.Contents.Length > 50 ? news.Contents.Substring(0, 50) : news.Contents,
                    PublishDate = news.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = news.NewsComments.Count(),
                    Remark = news.Remark
                });
            }
            return response;
        }
        /// <summary>
        /// Gets the search one news.
        /// </summary>
        /// <returns>The search one news.</returns>
        /// <param name="where">Where.</param>
        public ResponseModel GetSearchOneNews(Expression<Func<News, bool>> where)
        {
            var news = _db.News.Where(where).FirstOrDefault();
            if (news == null)
                return new ResponseModel { code = 0, result = "新闻搜索失败" };
            return new ResponseModel { code = 200, result = "新闻搜索成功" };
        }
        /// <summary>
        /// Gets the news count.
        /// </summary>
        /// <returns>The news count.</returns>
        public ResponseModel GetNewsCount(Expression<Func<News, bool>> where)
        {
            var count = _db.News.Where(where).Count();
            return new ResponseModel { code = 200, result = "获取成功", data = count };
        }

        public ResponseModel GetRecommendNewsLis(int newsId)
        {
            var news = _db.News.FirstOrDefault(c => c.Id == newsId);
            if (news != null)
                return new ResponseModel { code = 0, result = "新闻不存在" };
            var newlist = _db.News.Include("NewsComment").Where(c => c.NewsClassifyId == news.NewsClassifyId && c.Id != newsId)
                            .OrderByDescending(c => c.PublishDate).OrderByDescending(c => c.NewsComments.Count).Take(5);
            var response = new ResponseModel
            {
                code = 200,
                result = "最新评论新闻获取成功"
            };
            response.data = new List<NewsModel>();
            foreach (var n in newlist)
            {
                response.data.Add(new NewsModel
                {
                    Id = n.Id,
                    ClassifyName = n.NewClassify.Name,
                    Title = n.Title,
                    Image = n.Image,
                    Contents = n.Contents.Length > 50 ? n.Contents.Substring(0, 50) : n.Contents,
                    PublishDate = n.PublishDate.ToString("yyyy-MM-dd"),
                    CommentCount = n.NewsComments.Count(),
                    Remark = n.Remark
                });
            }
            return response;
        }

    }

}

