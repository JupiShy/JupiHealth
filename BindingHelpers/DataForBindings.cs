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

        public static string GetBMIValue()
        {
            using (var db = new DatabaseSource())
            {
                var metrics = db.metrics
                .OrderByDescending(m => m.date)
                .FirstOrDefault();

                var previousMetrics = db.metrics
                    .OrderByDescending(m => m.date)
                    .Skip(1)
                    .FirstOrDefault();

                if (metrics != null)
                {
                    return Convert.ToString(metrics.bmi.ToString("F1"));
                }
                else if (previousMetrics != null)
                {
                    return Convert.ToString(previousMetrics.bmi.ToString("F1"));
                }
                else
                {
                    return "0.0";
                }

            }
        }

        public static string GetTargetValue()
        {
            using (var db = new DatabaseSource())
            {
                var user = db.user.FirstOrDefault();

                if (user != null)
                {
                    return Convert.ToString(user.target);
                }
                else
                {
                    return "0,0";
                }
            }
        }

        public static string GetWeightValue()
        {
            using (var db = new DatabaseSource())
            {
                var metrics = db.metrics
                .OrderByDescending(m => m.date)
                .FirstOrDefault();

                var previousMetrics = db.metrics
                    .OrderByDescending(m => m.date)
                    .Skip(1)
                    .FirstOrDefault();

                if (metrics != null)
                {
                    return Convert.ToString(metrics.weight + " кг");
                }
                else if (previousMetrics != null)
                {
                    return Convert.ToString(previousMetrics.height + " см");
                }
                else
                {
                    return "0 кг";
                }

            }
        }

        public static string GetHeightValue()
        {
            using (var db = new DatabaseSource())
            {
                var metrics = db.metrics
                .OrderByDescending(m => m.date)
                .FirstOrDefault();

                var previousMetrics = db.metrics
                    .OrderByDescending(m => m.date)
                    .Skip(1)
                    .FirstOrDefault();

                if (metrics != null)
                {
                    return Convert.ToString(metrics.height + " см");
                }
                else if(previousMetrics != null)
                {
                    return Convert.ToString(previousMetrics.height + " см");
                }
                else
                {
                    return "0 см";
                }
            }
        }
    }
}
