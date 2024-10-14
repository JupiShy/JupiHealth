using HealthApp.Database;
using Microsoft.EntityFrameworkCore;
using System.Xml.Schema;

namespace HealthApp.BindingHelpers
{
    public class DataForBindings
    {
        public static string GetUserName()
        {
            using (var db = new DatabaseSource())
            {
                var user = db.user.FirstOrDefault();

                if (user != null)
                {
                    return user.name;
                }
                else
                {
                    return "користувач";
                }
            }
        }

        public static string GetGreetingMessage()
        {
            var currentTime = DateTime.Now.TimeOfDay;

            if (currentTime >= TimeSpan.FromHours(6) && currentTime < TimeSpan.FromHours(12))
            {
                return "Добрий ранок,";
            }
            else if (currentTime >= TimeSpan.FromHours(12) && currentTime < TimeSpan.FromHours(18))
            {
                return "Добрий день,";
            }
            else if (currentTime >= TimeSpan.FromHours(18) && currentTime < TimeSpan.FromHours(22))
            {
                return "Добрий вечір,";
            }
            else
            {
                return "Доброї ночі,";
            }
        }

        public static string GetWaterValue()
        {
            using (var db = new DatabaseSource())
            {
                var metrics = db.metrics
                .OrderByDescending(m => m.date)
                .FirstOrDefault();

                if (metrics != null)
                {
                    return Convert.ToString(metrics.water + " мл");
                }
                else
                {
                    return "0 мл";
                }
            }
        }
    }
}
