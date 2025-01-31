﻿using System.Reflection;

namespace SalesService.Domain.Abstractions
{
    public abstract class Enumeration : IComparable
    {
        public string Name { get; private set; }
        public int Id { get; private set; }

        protected Enumeration(int id, string name) => (Id, Name) = (id, name);

        public override string ToString() => Name;

        public static IEnumerable<T> GetAll<T>() where T : Enumeration =>
            typeof(T).GetFields(BindingFlags.Public |
                                BindingFlags.Static |
                                BindingFlags.DeclaredOnly)
                        .Select(f => f.GetValue(null))
                        .Cast<T>();

        public override bool Equals(object obj)
        {
            if (obj is not Enumeration otherValue)
            {
                return false;
            }

            var typeMatches = GetType().Equals(obj.GetType());
            var valueMatches = Id.Equals(otherValue.Id);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static int AbsoluteDifference(Enumeration firstValue, Enumeration secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Id - secondValue.Id);
            return absoluteDifference;
        }

        public static T FromId<T>(int id) where T : Enumeration
        {
            return Parse<T, int>(id, nameof(Id), item => item.Id == id);
        }

        public static T FromName<T>(string name) where T : Enumeration
        {
            return Parse<T, string>(name, nameof(Name), item => item.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        protected static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            return GetAll<T>().FirstOrDefault(predicate) ?? throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
        }

        public int CompareTo(object obj) => Id.CompareTo(((Enumeration)obj).Id);
    }
}