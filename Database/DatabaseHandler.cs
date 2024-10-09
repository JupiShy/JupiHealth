using HealthApp.Database.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthApp.Database
{
    public class DatabaseHandler
    {
        public readonly DatabaseSource db = new DatabaseSource();

        public async Task ChangeUserInfo(string name, int age)
        {

            var user = await db.user.FindAsync(1);

            if (user != null)
            {
                user.name = name;
                user.age = age;

                db.user.Update(user);
            }
            else
            {
                user = new User
                {
                    id = 1,
                    name = name,
                    age = age
                };

                db.user.Add(user);

            }

            await db.SaveChangesAsync();
        }
    }
}
