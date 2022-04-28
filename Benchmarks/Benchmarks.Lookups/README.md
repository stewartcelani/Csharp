﻿# Lookups
### Dictionary vs ConcurrentDictionary vs List vs MemoryCache (100k items)
|                         Method |            Mean |         Error |        StdDev |  Gen 0 | Allocated |
|------------------------------- |----------------:|--------------:|--------------:|-------:|----------:|
|           DictionaryFindMiddle |        56.04 ns |      1.142 ns |      2.646 ns |      - |         - |
| ConcurrentDictionaryFindMiddle |        55.93 ns |      1.088 ns |      1.069 ns |      - |         - |
|                 ListFindMiddle | 3,877,002.15 ns | 75,923.506 ns | 59,276.079 ns |      - |      51 B |
|          MemoryCacheFindMiddle |       133.68 ns |      2.657 ns |      2.729 ns | 0.0076 |      32 B |

```csharp
using System.Collections.Concurrent;
using System.Net;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Bogus;
using System.Runtime.Caching;

namespace Lookups
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            BenchmarkRunner.Run<Benchy>();
        }
    }

    [MemoryDiagnoser()]
    public class Benchy
    {
        private readonly Dictionary<Guid, Person> _dictionary = new();
        private readonly ConcurrentDictionary<Guid, Person> _concurrentDictionary = new();
        private readonly List<Person> _list = new();
        private readonly MemoryCache _memoryCache = MemoryCache.Default;
        

        public Benchy()
        {
                SeedDictionary();
        }

        private void SeedDictionary()
        {
            var faker = new Faker();
            DateTimeOffset fiveMinutesFromNow = DateTimeOffset.UtcNow.AddMinutes(5);
            for (int i = 0; i < 100000; i++)
            {
                var p = new Person(faker.Name.FullName(), faker.Random.Number(18, 67));
                _dictionary[p.Id] = p;
                _concurrentDictionary[p.Id] = p;
                _list.Add(p);
                _memoryCache.Set($"{p.Id}", p, fiveMinutesFromNow);
                if (i == 50000)
                {
                    var s = new Person(Guid.Parse("077a83cb-bc73-46bc-a3b8-8f807de42e94"), "Stewart Celani", 33);
                    _dictionary[s.Id] = s;
                    _concurrentDictionary[s.Id] = s;
                    _list.Add(s);
                    _memoryCache.Set("077a83cb-bc73-46bc-a3b8-8f807de42e94", s, fiveMinutesFromNow);
                }
            }
        }
        
        [Benchmark]
        public void DictionaryFindMiddle()
        {
            Person? lookup = _dictionary.GetValueOrDefault(Guid.Parse("077a83cb-bc73-46bc-a3b8-8f807de42e94"));
        }
        
        [Benchmark]
        public void ConcurrentDictionaryFindMiddle()
        {
            Person? lookup = _concurrentDictionary.GetValueOrDefault(Guid.Parse("077a83cb-bc73-46bc-a3b8-8f807de42e94"));
        }

        [Benchmark]
        public void ListFindMiddle()
        {
            Person? lookup = _list.FirstOrDefault(x => x.Id == Guid.Parse("077a83cb-bc73-46bc-a3b8-8f807de42e94"));
        }
        
        [Benchmark]
        public void MemoryCacheFindMiddle()
        {
            var lookup = (Person?)_memoryCache.Get("077a83cb-bc73-46bc-a3b8-8f807de42e94");
        }
    }

    public class Person
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Name { get; set; }
        public int Age { get; set; }
        

        public Person(string name, int age)
        {
            this.Name = name;
            this.Age = age;
        }
        
        public Person(Guid id, string name, int age)
        {
            this.Id = id;
            this.Name = name;
            this.Age = age;
        }
    }
}
```
