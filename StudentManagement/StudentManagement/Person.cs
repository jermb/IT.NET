﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagement
{
    public abstract class Person : IComparable<Person>
    {
        private string? firstName;
        private string? lastName;
        /// <summary>
        /// 0 for male
        /// 1 for female
        /// 2 for other
        /// </summary>
        private int gender;
        private int age;

        public string? FirstName
        {
            get
            {
                return firstName;
            }
            set
            {
                firstName = value;
            }
        }

        public string? LastName
        {
            get
            {
                return lastName;
            }
            set
            {
                lastName = value;
            }
        }

        public int Gender
        {
            get
            {
                return gender;
            }
            set
            {
                gender = value;
            }
        }

        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                age = value;
            }
        }

        public string Display { get => $"{LastName} {FirstName}"; }

        public int CompareTo(Person? other)
        {
            string fullName = this.LastName + " " + this.FirstName;
            string otherFull = other?.LastName + " " + other?.FirstName;

            return fullName.CompareTo(otherFull);

        }
    }
}
