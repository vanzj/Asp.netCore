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
    public class CommentService
    {
        private Db _db;
        private NewsService _newsService;
        public CommentService (Db db,NewsService newsService)
        {
            _db = db;
            _newsService = newsService;
        }

        public ResponseModel AddComment(AddComment addComment){
            var news = _newsService.GetOneNews(addComment.NewsId);
            if (news.code == 0)
                return new ResponseModel { code = 0, result = "新闻不存在" };
            var com = new NewsComment { AddTime = DateTime.Now, NewsId = addComment.NewsId, Contents = addComment.Contents };
            _db.NewsComment.Add(com);
            int i = _db.SaveChanges();
            if (i>0)
            {
                return new ResponseModel
                {
                    code = 200,
                    result = "新闻评论添加成功",
                    data = new CommentModel
                    {
                        Contents = addComment.Contents,
                        Floor = "#" + (news.data.CommentCount + 1),
                        AddTime = DateTime.Now
                    }
                };
            }
            return new ResponseModel
            {
                code = 0,
                result = "新闻评论添加失败"
            };
        }
        /// <summary>
        /// Deletes the comment.
        /// </summary>
        /// <returns>The comment.</returns>
        /// <param name="id">Identifier.</param>
        public ResponseModel DeleteComment(int id){
            var comment = _db.NewsComment.Find(id);
            if (comment == null)
                return new ResponseModel { code = 0, result = "评论不存在" };
            _db.NewsComment.Remove(comment);
            int i = _db.SaveChanges();
            if (i>0)
            {
                return new ResponseModel
                {
                    code = 200,
                    result = "新闻评论删除成功"
                };
            }
            return new ResponseModel { code = 0, result = "新闻评论删除失败" };
        }



        public ResponseModel GetCommentList(Expression<Func<NewsComment,bool>> where){
            var comments = _db.NewsComment.Include("News").Where(where).OrderBy(c => c.AddTime).ToList();
            var response = new ResponseModel
            {
                code = 200,
                result = "获取评论成功"
            };
            response.data = new List<CommentModel>();
            int floor = 1;
            foreach (var comment in comments)
            {
                response.data.Add(new CommentModel
                {
                    Id = comment.Id,
                    NewsName = comment.News.Title,
                    Contents = comment.Contents,
                    AddTime = comment.AddTime,
                    Remark = comment.Remark,
                    Floor = "#" + floor,
                });
                floor++;
            }
            response.data.Reverse();
            return response;
        }


    }
}
