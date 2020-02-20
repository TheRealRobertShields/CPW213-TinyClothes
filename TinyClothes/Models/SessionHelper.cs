using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TinyClothes.Models
{
    public static class SessionHelper
    {
        private const string IdKey = "Id";
        private const string UsernameKey = "Username";
        public static void CreateUserSession(int id, string username, IHttpContextAccessor http)
        {
            http.HttpContext.Session.SetInt32(IdKey, id);
            http.HttpContext.Session.SetString(UsernameKey, username);
        }

        /// <summary>
        /// Returns true if the user is logged in.
        /// </summary>
        public static bool IsUserLoggedIn(IHttpContextAccessor http)
        {
            int? memberId = http.HttpContext.Session.GetInt32(IdKey);
            return memberId.HasValue;
        }

        /// <summary>
        /// Logs user out/Clears session.
        /// </summary>
        public static void DestroyUserSession(IHttpContextAccessor http)
        {
            http.HttpContext.Session.Clear();
        }
    }
}
