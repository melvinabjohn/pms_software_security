using System;
using Newtonsoft.Json;

namespace pms_testProject.MockData
{
	public static class MockDataReader
	{
        private static readonly TestData<object> TestData = new() { Tests = new List<Test<object>>() };

        static  MockDataReader()
		{
            var x = Directory.GetCurrentDirectory();
            var y = Directory.Exists("/Users/melvinabraham/Desktop/Melvin/Hochschule/pms/Pms/pms-testProject/Stubs");
            var path = "/Users/melvinabraham/Desktop/Melvin/Hochschule/pms/Pms/pms-testProject/Stubs";
            foreach (string file in Directory.EnumerateFiles(path))
            {
                var content = File.ReadAllText(file);
            if (!string.IsNullOrWhiteSpace(content))
            {
                TestData<object> data =
                    JsonConvert.DeserializeObject<TestData<object>>(content);

                if (data != null && data.Tests.Any())
                {
                    TestData.Tests.AddRange(data.Tests);
                }
            }
        }
    }

        public static T GetStubData<T>(string Name) where T : class
        {
            var item = TestData?.Tests?.FirstOrDefault(x => x.Name.Equals(Name));

            if (item != null && item.Data != null)
            {
                var content = JsonConvert.SerializeObject(item.Data);
                var data = JsonConvert.DeserializeObject<T>(content);
                return data;
            }

            return null;
        }
    }

    public class Test<T> where T : class
    {
        public string Name { get; set; }
        public T Data { get; set; }
    }

    public class TestData<T> where T : class
    {
        public List<Test<T>> Tests { get; set; }
    }
}

