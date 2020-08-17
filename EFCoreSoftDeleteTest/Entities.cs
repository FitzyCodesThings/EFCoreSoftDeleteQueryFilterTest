using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreSoftDeleteTest
{
    public interface ISoftDeletable
    {
        bool IsDeleted { get; set; }
    }

    public class Student : ISoftDeletable
    {
        public Student()
        {
            Addresses = new HashSet<Address>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }        
        public bool IsDeleted { get; set; }

        public ICollection<Address> Addresses { get; set; }
    }

    public class Address : ISoftDeletable
    {
        public int Id { get; set; }        
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public bool IsDeleted { get; set; }

        public int StudentId { get; set; }
        public Student Student { get; set; }
    }
}
