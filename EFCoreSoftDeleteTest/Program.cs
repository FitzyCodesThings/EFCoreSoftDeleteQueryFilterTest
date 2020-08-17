using System;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreSoftDeleteTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            using (var db = new AppDbContext())
            {
                Student student = new Student()
                {
                    FirstName = "John",
                    LastName = "Doe"                    
                };

                student.Addresses.Add(new Address()
                {
                    StreetAddress = "123 Main Street",
                    City = "Las Vegas",
                    State = "Nevada",
                    Zip = "12345"
                });

                student.Addresses.Add(new Address()
                {
                    StreetAddress = "456 Poplar Street",
                    City = "Reno",
                    State = "Nevada",
                    Zip = "12345"
                });

                student.Addresses.Add(new Address()
                {
                    StreetAddress = "789 Broad Street",
                    City = "Los Angeles",
                    State = "California",
                    Zip = "12345"
                });

                await db.AddAsync(student);

                await db.SaveChangesAsync();

                DisplayStudent(student);

                db.Remove(student.Addresses.FirstOrDefault(p => p.Id == 2));

                //await db.SaveChangesAsync(); // Works fine of course
                await db.SaveChangesWithSoftDeleteAsync(); // IsDeleted is set to true, but object is still present in ICollection<Address> on Student

                DisplayStudent(student);

            }

            Console.ReadKey();
        }

        static void DisplayStudent(Student student)
        {
            Console.WriteLine($"Student {student.Id} - {student.FirstName} {student.LastName}");
            Console.WriteLine("----------------------");

            foreach (var address in student.Addresses)
                Console.WriteLine($"Address Id: {address.Id} - {address.StreetAddress}, {address.City}, {address.State} {address.Zip}");

            Console.WriteLine("");
        }
    }
}
