﻿using Blogs.Domain.Entity.Blogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Blogs.Core.Entity.Blogs
{
    public class BlogsUser : BaseEntity
    {
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }



        // 博客相关属性
        public string Bio { get; private set; } // 个人简介
        public string Avatar { get; private set; } // 头像
        public string Website { get; private set; } // 个人网站

        // 统计属性
        public int ArticleCount { get; private set; } // 文章数量
        public int FollowerCount { get; private set; } // 粉丝数量
        public int FollowingCount { get; private set; } // 关注数量

        // 导航属性
        private readonly List<BlogsArticle> _articles = new();
        public virtual IReadOnlyCollection<BlogsArticle> Articles => _articles.AsReadOnly();
        private readonly List<BlogsComment> _comments = new();
        public virtual IReadOnlyCollection<BlogsComment> Comments => _comments.AsReadOnly();

        // 博客相关领域方法
        public void UpdateProfile(string bio, string avatar, string website)
        {
            Bio = bio;
            Avatar = avatar;
            Website = website;

            //AddDomainEvent(new UserProfileUpdatedEvent(Id));
        }

        public void IncreaseArticleCount()
        {
            ArticleCount++;
        }

        public void DecreaseArticleCount()
        {
            if (ArticleCount > 0)
                ArticleCount--;
        }

        public void IncreaseFollowerCount()
        {
            FollowerCount++;
        }

        public void DecreaseFollowerCount()
        {
            if (FollowerCount > 0)
                FollowerCount--;
        }

        public void IncreaseFollowingCount()
        {
            FollowingCount++;
        }

        public void DecreaseFollowingCount()
        {
            if (FollowingCount > 0)
                FollowingCount--;
        }
    }
}
