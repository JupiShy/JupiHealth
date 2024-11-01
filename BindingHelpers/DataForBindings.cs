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

        public static string GetBMIColor()
        {
            double bmi = 0;

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
                    bmi = metrics.bmi;
                }
                else if (previousMetrics != null)
                {
                    bmi = previousMetrics.bmi;
                }
            }

            return GetColor(bmi);
        }

        public static string GetTargetColor()
        {
            double targetBmi = 0;

            using (var db = new DatabaseSource())
            {
                

                var user = db.user.FirstOrDefault();

                if (user != null)
                {
                    targetBmi = user.target;
                }
            }

            return GetColor(targetBmi);
        }

        private static string GetColor(double value)
        {
            string color;

            if(value >= 18.5 && value <= 24.9)
            {
                color = "#98FB98";
            }
            else if(value < 16 || value > 35)
            {
                color = "#FA8072";
            }
            else
            {
                color = "#FFCC99";
            }

            return color;
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
