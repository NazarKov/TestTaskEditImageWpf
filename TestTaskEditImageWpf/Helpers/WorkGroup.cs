using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskEditImageWpf.Helpers
{
    public class WorkGroup
    {
        public List<Person> Persons { get; set; }
        public BlockingCollection<Image> Images { get; set; }

        public BlockingCollection<Image> DoneImages { get; set; }
        public int TotalTimeWorkForImage { get; set; } = 0;

        public int ImageCountForWork { get; set; } = 0;

        public WorkGroup()
        {
            Images = [];
            DoneImages = [];
            Persons = [];
        }
        public WorkGroup(List<Person> persons, int imageCount)
        {
            Persons = persons;
            Images = [];
            DoneImages = [];
            ImageCountForWork = imageCount;
            for (int i = 0; i < ImageCountForWork; i++)
            {
                Images.Add(new Image());
            }
            Images.CompleteAdding();
        }

        public async Task<bool> Work()
        {
            try
            {
                List<Task> tasks = new List<Task>();
                if (Persons != null && Persons.Count > 0)
                {
                    foreach (var person in Persons)
                    {
                        tasks.Add(Task.Run(() => WorkPerson(person)));
                    }
                }
                await Task.WhenAll(tasks);
                СalculationWorkingTime();
                return true;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }
        private void СalculationWorkingTime()
        {
            TotalTimeWorkForImage = 0;
            foreach (var person in Persons)
            {
                TotalTimeWorkForImage += person.TotalTimeWorkForImages;
            }
        }

        private async Task WorkPerson(Person person)
        {
            Task task = Task.Run(async () =>
            {
                bool isWorkPerson = true;

                while (isWorkPerson)
                {
                    if (DoneImages.Count >= ImageCountForWork)
                    {
                        isWorkPerson = false;
                        break;
                    }
                    else
                    {
                        try
                        {
                            var imageGoWork = Images.Take();
                            imageGoWork = await person.WorkInOneImage(imageGoWork);
                            DoneImages.Add(imageGoWork);
                        }
                        catch (InvalidOperationException)
                        {
                            break;
                        }
                    }
                }
            });
            await task;
        }
    }
}
