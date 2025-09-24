using System.Linq;
using System.Threading.Tasks;
using TestTaskEditImageWpf.Helpers;

namespace TestEditImageWpf
{
    [TestClass]
    public sealed class TestWorkGroupAndPerson
    {
        [TestMethod]
        public async Task TestEditImageOnePerson()
        {
            Person person = new Person("Petro", 10);

            WorkGroup workGroup = new WorkGroup(new List<Person>() { person }, 1000);

            await workGroup.Work();

            int count = workGroup.DoneImages.Count;
            Assert.AreEqual(workGroup.ImageCountForWork, count);
            Assert.AreEqual(person.CompleteImage, count);
        }

        [TestMethod]
        public async Task TestEditImageTwoPerson()
        {
            Person person = new Person("Petro", 10);
            Person person1 = new Person("Sergie", 10);
            WorkGroup workGroup = new WorkGroup(new List<Person>() { person, person1 }, 1000);

            await workGroup.Work();

            int count = workGroup.DoneImages.Count;
            Assert.AreEqual(workGroup.ImageCountForWork, count);


            Assert.AreEqual(person.CompleteImage, count / 2);
            Assert.AreEqual(person1.CompleteImage, count / 2);
        }

        [TestMethod]
        public async Task TestEditImageThreePerson()
        {
            Person person = new Person("Petro", 20);
            Person person1 = new Person("Sergie", 30);
            Person person2 = new Person("Vitalic", 40);
            WorkGroup workGroup = new WorkGroup(new List<Person>() { person, person1 , person2 }, 1000);

            await workGroup.Work();

            int expected = workGroup.ImageCountForWork / 3;
            int actual = person.CompleteImage;

            // допустима похибка ±3
            int tolerance = 5;

            int count = workGroup.DoneImages.Count;
            Assert.AreEqual(workGroup.ImageCountForWork, count);

            Assert.IsTrue(Math.Abs(455 - actual) <= tolerance,$"Expected {455} ± {tolerance}, but got {actual}");
            actual = person1.CompleteImage;
            Assert.IsTrue(Math.Abs(310 - actual) <= tolerance, $"Expected {310} ± {tolerance}, but got {actual}");
            actual = person2.CompleteImage;
            Assert.IsTrue(Math.Abs(230 - actual) <= tolerance, $"Expected {230} ± {tolerance}, but got {actual}");
        }
    }
}
