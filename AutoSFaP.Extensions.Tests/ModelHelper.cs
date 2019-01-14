namespace AutoSFaP.Extension.Test
{
    public static class ModelHelper
    {
        public static TestModel[] GetTestData()
        {
            var testData = new TestModel[10];
            testData[0] = new TestModel { Id = 0, Name = "TestName_0", IsEven = true };
            testData[1] = new TestModel { Id = 1, Name = "TestName_1", IsEven = false };
            testData[2] = new TestModel { Id = 2, Name = "TestName_2", IsEven = true };
            testData[3] = new TestModel { Id = 3, Name = "TestName_3", IsEven = false };
            testData[4] = new TestModel { Id = 4, Name = "TestName_4", IsEven = true };
            testData[5] = new TestModel { Id = 5, Name = "TestName_5", IsEven = false };
            testData[6] = new TestModel { Id = 6, Name = "TestName_6", IsEven = true };
            testData[7] = new TestModel { Id = 7, Name = "TestName_7", IsEven = false };
            testData[8] = new TestModel { Id = 8, Name = "TestName_8", IsEven = true };
            testData[9] = new TestModel { Id = 9, Name = "TestName_9", IsEven = false };

            return testData;
        }
    }

    public class TestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsEven { get; set; }
    }
}
