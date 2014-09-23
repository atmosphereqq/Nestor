﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nestor.Common;
using Nestor.Data;
using Nestor.Models;
using Nestor.Models.Entities;

namespace Nestor.Business
{
    /// <summary>
    /// 用户业务类
    /// </summary>
    public class UserBusiness
    {
        #region Field
        /// <summary>
        /// 用户Repository
        /// </summary>
        private UserRepository userRepository;
        #endregion //Field

        #region Constructor
        /// <summary>
        /// 用户业务类
        /// </summary>
        public UserBusiness()
        {
            this.userRepository = new UserRepository();
        }
        #endregion //Constructor

        #region Method
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="isRoot">是否Root</param>
        /// <returns></returns>
        public IEnumerable<User> Get(bool isRoot)
        {
            if (isRoot)
            {
                return this.userRepository.Get();
            }
            else
            {
                return this.userRepository.Get().Where(r => r.UserType != (int)UserType.Root);
            }
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public User Get(int id)
        {
            return this.userRepository.Get(id);
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns></returns>
        public User Get(string userName)
        {
            return this.userRepository.Get(userName);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        public ErrorCode Create(User user)
        {
            return this.userRepository.Create(user);
        }

        /// <summary>
        /// 更新用户
        /// </summary>
        /// <param name="user">用户对象</param>
        /// <returns></returns>
        public ErrorCode Update(User user)
        {
            return this.userRepository.Update(user);
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public ErrorCode Login(string userName, string password)
        {
            var user = this.userRepository.Get(userName);
            if (user == null)
                return ErrorCode.UserNotExist;

            if (user.Status == (int)EntityStatus.UserDisable)
                return ErrorCode.UserDisabled;

            if (user.Password != Hasher.SHA1Encrypt(password))
                return ErrorCode.WrongPassword;

            return ErrorCode.Success;
        }
        #endregion //Method
    }
}
